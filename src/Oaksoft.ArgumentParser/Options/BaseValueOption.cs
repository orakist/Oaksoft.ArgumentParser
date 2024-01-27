using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    where TValue : IComparable
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
        {
            return;
        }

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
    where TValue : IComparable
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
        {
            return;
        }

        var resultValues = GetValidatedValues();

        if (_listPredicates.Any(p => !p.Invoke(resultValues)))
        {
            var values = string.Join(", ", _inputValues);
            throw ParserErrors.ListPredicateFailure.ToException(values);
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
        else if (keyProperty.PropertyType.IsAssignableFrom(typeof(Collection<TValue>)))
        {
            keyProperty.SetValue(appOptions, new Collection<TValue>(_resultValues));
        }
        else if (keyProperty.PropertyType.IsAssignableFrom(typeof(HashSet<TValue>)))
        {
            keyProperty.SetValue(appOptions, _resultValues.ToHashSet());
        }
        else // if nullable generic TValue
        {
            keyProperty.SetValue(appOptions, CreateNullableCollection(keyProperty));
        }
    }

    private object? CreateNullableCollection(PropertyInfo keyProperty)
    {
        var type = typeof(TValue);
        var valueType = type.IsValueType ? typeof(Nullable<>).MakeGenericType(type) : type;

        // firstly try list creation
        var constructedListType = typeof(List<>).MakeGenericType(valueType);
        if (keyProperty.PropertyType.IsAssignableFrom(constructedListType))
        {
            var genericInstance = (IList)Activator.CreateInstance(constructedListType)!;

            foreach (var resultValue in _resultValues)
            {
                genericInstance.Add(resultValue);
            }

            return genericInstance;
        }

        // secondly try array creation
        var arrInstance = Array.CreateInstance(valueType, _resultValues.Count);
        if (keyProperty.PropertyType.IsInstanceOfType(arrInstance))
        {
            for (var index = 0; index < _resultValues.Count; ++index)
            {
                arrInstance.SetValue(_resultValues[index], index);
            }

            return arrInstance;
        }

        // thirdly try collection creation
        var constructedCollectionType = typeof(Collection<>).MakeGenericType(valueType);
        if (keyProperty.PropertyType.IsAssignableFrom(constructedCollectionType))
        {
            var genericInstance = (IList)Activator.CreateInstance(constructedCollectionType)!;

            foreach (var resultValue in _resultValues)
            {
                genericInstance.Add(resultValue);
            }

            return genericInstance;
        }

        // fourthly try hashset creation
        var constructedHashSetType = typeof(HashSet<>).MakeGenericType(valueType);
        if (keyProperty.PropertyType.IsAssignableFrom(constructedHashSetType))
        {
            var genericInstance = Activator.CreateInstance(constructedHashSetType)!;
            var addMethod = constructedHashSetType.GetMethods().FirstOrDefault(m => m.Name == "Add");

            if (addMethod != null)
            {
                foreach (var resultValue in _resultValues)
                {
                    addMethod.Invoke(genericInstance, new object[] { resultValue });
                }

                return genericInstance;
            }
        }

        return null;
    }

    public override void Clear()
    {
        base.Clear();
        _resultValues.Clear();
    }

    protected IEnumerable<string> SplitByValueDelimiter(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            yield break;
        }

        if (!EnableValueTokenSplitting)
        {
            yield return value.Trim();
        }

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
    where TValue : IComparable
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
            throw BuilderErrors.EmptyAllowedValue.WithName(KeyProperty.Name).ToException();
        }

        var values = allowedValues
            .Select(v => v is string s ? (TValue)(object)s.Trim() : v);

        _allowedValues.Clear();
        foreach (var value in values)
        {
            _allowedValues.Add(value);
        }

        var type = typeof(TValue);
        if (type.IsEnum && _allowedValues.Any(v => !type.IsEnumDefined(v)))
        {
            var value = _allowedValues.First(v => !type.IsEnumDefined(v)).ToString()!;
            throw BuilderErrors.InvalidAllowedValue.WithName(KeyProperty.Name).ToException(value);
        }
    }

    public override void Initialize()
    {
        base.Initialize();

        var type = typeof(TValue);
        if (type.IsEnum && _allowedValues.Count < 1)
        {
            foreach (var value in type.GetEnumValues())
            {
                _allowedValues.Add((TValue)value);
            }
        }
    }

    protected override List<TValue> GetValidatedValues()
    {
        var resultValues = base.GetValidatedValues();

        ValidateByAllowedValues(resultValues);

        var invalidValues = resultValues
            .Select((v, i) => (Value: v, Index: i))
            .Where(n => _predicates.Any(p => !p.Invoke(n.Value)))
            .Select(n => _inputValues[n.Index])
            .ToList();

        if (invalidValues.Count < 1)
        {
            return resultValues;
        }

        var values = string.Join(", ", invalidValues);
        throw ParserErrors.PredicateFailure.ToException(values);
    }

    private void ValidateByAllowedValues(IReadOnlyList<TValue> inputValues)
    {
        if (_allowedValues.Count <= 0)
        {
            return;
        }

        if (typeof(TValue) == typeof(string))
        {
            var flag = _parser!.CaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            for (var i = 0; i < inputValues.Count; ++i)
            {
                var inputValue = inputValues[i] as string;
                var allowedValues = _allowedValues.Select(a => (a as string)!);
                if (allowedValues.Any(a => a.Equals(inputValue, flag)))
                {
                    continue;
                }

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
                {
                    continue;
                }

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
    where TValue : IComparable
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
                if (typeof(TValue).IsEnum && this is IHaveAllowedValues values)
                {
                    var joinedValues = string.Join(", ", values.GetAllowedValues());
                    throw ParserErrors.ValueMustBeOneOf.ToException(inputValue, joinedValues);
                }

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