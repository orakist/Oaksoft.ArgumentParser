using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Callbacks;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Errors;
using Oaksoft.ArgumentParser.Errors.Builder;
using Oaksoft.ArgumentParser.Errors.Parser;

namespace Oaksoft.ArgumentParser.Options;

internal abstract class BaseScalarValueOption<TValue>
    : BaseAllowedValuesOption<TValue>, IScalarValueOption<TValue>
    where TValue : IComparable, IEquatable<TValue>
{
    public Ref<TValue>? ResultValue { get; private set; }

    protected BaseScalarValueOption(int requiredValueCount, int maximumValueCount)
        : base(requiredValueCount, maximumValueCount)
    {
    }

    public override void Validate()
    {
        base.Validate();

        if (_inputValues.Count <= 0)
            return;

        var resultValues = GetValidatedValues();

        // if last switch option contains value assign it,
        // otherwise we will use default value
        if (!string.IsNullOrWhiteSpace(_valueTokens[^1]))
        {
            ResultValue = new Ref<TValue>(resultValues[^1]);
        }
    }

    public override void Clear()
    {
        base.Clear();
        ResultValue = null;
    }
}

internal abstract class BaseSequentialValueOption<TValue>
    : BaseAllowedValuesOption<TValue>, ISequentialValueOption<TValue>
    where TValue : IComparable, IEquatable<TValue>
{
    public bool EnableValueTokenSplitting { get; private set; }

    public List<TValue> ResultValues => _resultValues.ToList();

    private readonly List<TValue> _resultValues;

    private readonly List<Predicate<List<TValue>>> _listPredicates;

    protected BaseSequentialValueOption(int requiredValueCount, int maximumValueCount)
        : base(requiredValueCount, maximumValueCount)
    {
        _resultValues = new List<TValue>();
        _listPredicates = new List<Predicate<List<TValue>>>();
        EnableValueTokenSplitting = true;
    }

    public void AddListPredicate(Predicate<List<TValue>> predicate)
    {
        ParserInitializedGuard();

        _listPredicates.Add(predicate);
    }

    public void SetEnableValueTokenSplitting(bool enabled)
    {
        ParserInitializedGuard();

        EnableValueTokenSplitting = enabled;
    }

    public override void Validate()
    {
        base.Validate();

        if (_inputValues.Count <= 0)
            return;

        var resultValues = GetValidatedValues();

        if (_listPredicates.Any(predicate => !predicate.Invoke(resultValues)))
        {
            var values = string.Join(", ", _inputValues);
            throw ParserErrors.PredicateFailure.ToException(values);
        }

        _resultValues.AddRange(resultValues);
    }

    public override void ApplyOptionResult(object appOptions, PropertyInfo keyProperty)
    {
        if (keyProperty.PropertyType.IsAssignableFrom(typeof(List<TValue>)))
        {
            keyProperty.SetValue(appOptions, _resultValues);
        }
        else if (keyProperty.PropertyType.IsAssignableFrom(typeof(TValue[])))
        {
            keyProperty.SetValue(appOptions, _resultValues.ToArray());
        }
        else // if nullable generic TValue
        {
            keyProperty.SetValue(appOptions, CreateNullableListOrArray(keyProperty));
        }
    }

    private object? CreateNullableListOrArray(PropertyInfo keyProperty)
    {
        Type itemType;
        if (keyProperty.PropertyType.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
        {
            itemType = keyProperty.PropertyType.GetGenericArguments()[0];
            if (!itemType.IsAssignableFrom(typeof(TValue)))
            {
                return null;
            }
        }
        else
        {
            return null;
        }

        // first try list creation
        var listType = typeof(List<>);
        var constructedListType = listType.MakeGenericType(itemType);
        if (keyProperty.PropertyType.IsAssignableFrom(constructedListType))
        {
            var listInstance = (IList)Activator.CreateInstance(constructedListType)!;

            foreach (var resultValue in _resultValues)
            {
                listInstance.Add(resultValue);
            }

            return listInstance;
        }

        var arrType = Array.CreateInstance(itemType, _resultValues.Count);
        if(!keyProperty.PropertyType.IsInstanceOfType(arrType))
            return false;

        for (var index = 0; index < _resultValues.Count; ++index)
        {
            arrType.SetValue(_resultValues[index], index);
        }

        return arrType;
    }

    public override void Clear()
    {
        base.Clear();
        _resultValues.Clear();
    }

    protected IEnumerable<string> SplitByValueDelimiter(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            yield break;

        if (!EnableValueTokenSplitting)
            yield return value.Trim();

        var values = new List<string> { value };

        values = _parser!.ValueDelimiter.GetSymbols()
            .Aggregate(values, (current, symbol) => current.SelectMany(v => v.Split(symbol)).ToList());

        foreach (var v in values.Where(v => !string.IsNullOrWhiteSpace(v)))
        {
            yield return v.Trim();
        }
    }
}

internal abstract class BaseAllowedValuesOption<TValue>
    : BaseValueOption<TValue>, IHaveAllowedValues<TValue>
    where TValue : IComparable, IEquatable<TValue>
{
    public List<TValue> AllowedValues => _allowedValues.ToList();

    private readonly HashSet<TValue> _allowedValues;
    private readonly List<Predicate<TValue>> _predicates;

    protected BaseAllowedValuesOption(int requiredValueCount, int maximumValueCount)
        : base(requiredValueCount, maximumValueCount)
    {
        _allowedValues = new HashSet<TValue>();
        _predicates = new List<Predicate<TValue>>();
    }

    public void AddPredicate(Predicate<TValue> predicate)
    {
        ParserInitializedGuard();

        _predicates.Add(predicate);
    }

    public void SetAllowedValues(params TValue[] allowedValues)
    {
        ParserInitializedGuard();

        if (allowedValues.Cast<object?>()
            .Any(v => v is null || (v is string s && string.IsNullOrWhiteSpace(s))))
        {
            throw BuilderErrors.InvalidAllowedValue.WithName(KeyProperty.Name).ToException();
        }

        var values = allowedValues
            .Select(v => v is string s ? (TValue)(object)s.Trim() : v);

        _allowedValues.Clear();
        foreach (var value in values)
            _allowedValues.Add(value);
    }

    protected override List<TValue> GetValidatedValues()
    {
        var resultValues = base.GetValidatedValues();

        ValidateByAllowedValues(resultValues);

        foreach (var predicate in _predicates)
        {
            for (var i = 0; i < resultValues.Count; ++i)
            {
                if (predicate(resultValues[i]))
                    continue;

                throw ParserErrors.PredicateFailure.ToException(_inputValues[i]);
            }
        }

        return resultValues;
    }

    private void ValidateByAllowedValues(IReadOnlyList<TValue> inputValues)
    {
        if (_allowedValues.Count <= 0)
            return;

        if (typeof(TValue) == typeof(string))
        {
            var flag = _parser!.CaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            for (var i = 0; i < inputValues.Count; ++i)
            {
                var inputValue = inputValues[i] as string;
                var allowedValues = _allowedValues.Select(a => (a as string)!);
                if (allowedValues.Any(a => a.Equals(inputValue, flag)))
                    continue;

                var values = string.Join(", ", _allowedValues);
                throw ParserErrors.ValueMustBeOneOf.ToException(_inputValues[i], values);
            }
        }
        else
        {
            for (var i = 0; i < inputValues.Count; ++i)
            {
                var inputValue = inputValues[i];
                if (_allowedValues.Any(a => a.Equals(inputValue)))
                    continue;

                var values = string.Join(", ", _allowedValues);
                throw ParserErrors.ValueMustBeOneOf.ToException(_inputValues[i], values);
            }
        }
    }

    public List<string> GetAllowedValues()
    {
        return AllowedValues.Select(a => a.ToString()!).ToList();
    }
}

internal abstract class BaseValueOption<TValue> : BaseValueOption
    where TValue : IComparable, IEquatable<TValue>
{
    private TryParse<TValue>? _tryParseValueCallback;

    protected BaseValueOption(int requiredValueCount, int maximumValueCount)
        : base(requiredValueCount, maximumValueCount)
    {
    }

    public void SetTryParseCallback(TryParse<TValue> callback)
    {
        ParserInitializedGuard();

        _tryParseValueCallback = callback;
    }

    public override void Initialize()
    {
        base.Initialize();

        if (_tryParseValueCallback is null && DefaultTryParseCallback<TValue>.Instance.IsValidParser)
        {
            _tryParseValueCallback = DefaultTryParseCallback<TValue>.Instance.TryParse;
        }

        if (_tryParseValueCallback is null)
        {
            throw BuilderErrors.MissingCallback.WithName(Name).ToException(typeof(TValue).Name);
        }
    }

    protected virtual List<TValue> GetValidatedValues()
    {
        var resultValues = new List<TValue>();
        foreach (var inputValue in _inputValues)
        {
            if (!_tryParseValueCallback!.Invoke(inputValue, out var result))
            {
                throw ParserErrors.InvalidOptionValue.ToException(inputValue);
            }

            resultValues.Add(result);
        }

        return resultValues;
    }

    protected bool IsValidValue(string value)
    {
        return _tryParseValueCallback is not null &&
               _tryParseValueCallback(value, out _);
    }
}

internal abstract class BaseValueOption : BaseOption, IValueOption
{
    public List<string> ValueTokens => _valueTokens.ToList();

    public List<string> InputValues => _inputValues.ToList();

    public override int ValueCount => _inputValues.Count;

    protected readonly List<string> _valueTokens;
    protected readonly List<string> _inputValues;

    protected BaseValueOption(int requiredValueCount, int maximumValueCount)
    {
        ValueArity = (requiredValueCount, maximumValueCount);

        _valueTokens = new List<string>();
        _inputValues = new List<string>();
    }

    public void SetValueArity(ArityType valueArity)
    {
        ParserInitializedGuard();

        ValueArity = valueArity.GetLimits().GetOrThrow(KeyProperty.Name);
    }

    public void SetValueArity(int requiredValueCount, int maximumValueCount)
    {
        ParserInitializedGuard();

        ValueArity = (requiredValueCount, maximumValueCount);
    }

    public abstract void ApplyOptionResult(object appOptions, PropertyInfo keyProperty);

    public override void Clear()
    {
        base.Clear();
        _valueTokens.Clear();
        _inputValues.Clear();
    }
}