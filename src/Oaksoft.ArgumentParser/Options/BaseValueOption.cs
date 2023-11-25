using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Callbacks;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Exceptions;
using Oaksoft.ArgumentParser.Parser;

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

    protected readonly List<TValue> _resultValues;

    protected BaseSequentialValueOption(int requiredValueCount, int maximumValueCount)
        : base(requiredValueCount, maximumValueCount)
    {
        _resultValues = new List<TValue>();
        EnableValueTokenSplitting = true;
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

        _resultValues.AddRange(resultValues);
    }

    public override void ApplyOptionResult(IApplicationOptions appOptions, PropertyInfo keyProperty)
    {
        if (keyProperty.PropertyType.IsAssignableFrom(typeof(List<TValue>)))
        {
            keyProperty.SetValue(appOptions, _resultValues);
        }
        else if (keyProperty.PropertyType.IsAssignableFrom(typeof(TValue[])))
        {
            keyProperty.SetValue(appOptions, _resultValues.ToArray());
        }
    }

    public override void Clear()
    {
        base.Clear();
        _resultValues.Clear();
    }

    protected IEnumerable<string> SplitByValueDelimiter(string value)
    {
        if(string.IsNullOrWhiteSpace(value))
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

    protected readonly HashSet<TValue> _allowedValues;
    protected readonly List<Predicate<TValue>> _predicates;

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
            for (var index = 0; index < resultValues.Count; index++)
            {
                if (predicate(resultValues[index]))
                    continue;

                throw new Exception($"Value '{_inputValues[index]}' is not validated by the predicate callback.");
            }
        }

        return resultValues;
    }

    private void ValidateByAllowedValues(List<TValue> inputValues)
    {
        if (_allowedValues.Count <= 0)
            return;

        if (typeof(TValue) == typeof(string))
        {
            var flag = _parser!.CaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            foreach (var inputValue in inputValues.Cast<string>())
            {
                if (_allowedValues.Cast<string>().Any(a => a.Equals(inputValue, flag)))
                    continue;

                var values = string.Join('|', _allowedValues);
                throw new Exception($"Option value '{inputValue}' not recognized. Must be one of: {values}");
            }
        }
        else
        {
            foreach (var inputValue in inputValues)
            {
                if (_allowedValues.Any(a => a.Equals(inputValue)))
                    continue;

                var values = string.Join('|', _allowedValues);
                throw new Exception($"Option value '{inputValue}' not recognized. Must be one of: {values}");
            }
        }
    }
}

internal abstract class BaseValueOption<TValue> : BaseValueOption
    where TValue : IComparable, IEquatable<TValue>
{
    private TryParse<TValue>? _tryParseValueCallback;
    private Func<List<string>, List<TValue>>? _tryParseValuesCallback;

    protected BaseValueOption(int requiredValueCount, int maximumValueCount)
        : base(requiredValueCount, maximumValueCount)
    {
    }

    public void SetParsingCallbacks(IParsingCallbacks<TValue> optionCallbacks)
    {
        ParserInitializedGuard();

        SetTryParseValueCallbacks(optionCallbacks);
    }

    public void SetTryParseValueCallback(TryParse<TValue> callback)
    {
        ParserInitializedGuard();

        _tryParseValueCallback = callback;
    }

    public void SetTryParseValuesCallback(Func<List<string>, List<TValue>> callback)
    {
        ParserInitializedGuard();

        _tryParseValuesCallback = callback;
    }

    public override void Initialize()
    {
        base.Initialize();

        if (DefaultParsingCallbacks<TValue>.Instance.IsValidParser)
            SetTryParseValueCallbacks(DefaultParsingCallbacks<TValue>.Instance);

        if (_tryParseValueCallback is null && _tryParseValuesCallback is null)
        {
            throw BuilderErrors.MissingCallback.WithName(Name).ToException(typeof(TValue).Name);
        }
    }

    protected virtual List<TValue> GetValidatedValues()
    {
        var resultValues = new List<TValue>();
        if (_tryParseValuesCallback is not null)
        {
            resultValues.AddRange(_tryParseValuesCallback(_inputValues));

            if (resultValues == null || resultValues.Count < _inputValues.Count)
                throw new Exception("Option values cannot be validated.");
        }
        else if (_tryParseValueCallback is not null)
        {
            foreach (var inputValue in _inputValues)
            {
                if (!_tryParseValueCallback(inputValue, out var result))
                    throw new Exception($"Invalid input value found!. Value: {inputValue}");

                resultValues.Add(result);
            }
        }

        return resultValues;
    }

    protected virtual bool IsValidValue(string value)
    {
        return _tryParseValueCallback is not null &&
               _tryParseValueCallback(value, out _);
    }

    private void SetTryParseValueCallbacks(IParsingCallbacks<TValue> optionCallbacks)
    {
        var type = optionCallbacks.GetType();

        // only use overriden methods
        var methodName = nameof(IParsingCallbacks<TValue>.TryParseValue);
        if (type.GetMethod(methodName)?.DeclaringType == type)
            _tryParseValueCallback ??= optionCallbacks.TryParseValue;

        methodName = nameof(IParsingCallbacks<TValue>.TryParseValues);
        if (type.GetMethod(methodName)?.DeclaringType == type)
            _tryParseValuesCallback ??= optionCallbacks.TryParseValues;
    }
}

internal abstract class BaseValueOption : BaseOption, IValueOption
{
    public List<string> ValueTokens => _valueTokens.ToList();

    public List<string> InputValues => _inputValues.ToList();

    public override int ValueCount => _inputValues.Count;

    protected List<string> _valueTokens;
    protected List<string> _inputValues;

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

    public abstract void ApplyOptionResult(IApplicationOptions appOptions, PropertyInfo keyProperty);

    public override void Clear()
    {
        base.Clear();
        _valueTokens.Clear();
        _inputValues.Clear();
    }
}