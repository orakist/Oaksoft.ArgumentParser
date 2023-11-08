using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Callbacks;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Options;

internal abstract class BaseScalarValueOption<TValue>
    : BaseAllowedValuesOption<TValue>, IScalarValueOption<TValue>
    where TValue : IComparable, IEquatable<TValue>
{
    public Ref<TValue>? DefaultValue { get; private set; }

    public Ref<TValue>? ResultValue { get; private set; }

    protected BaseScalarValueOption(int requiredValueCount, int maximumValueCount)
        : base(requiredValueCount, maximumValueCount)
    {
    }

    public void SetDefaultValue(TValue defaultValue)
    {
        if (defaultValue is string strValue)
        {
            var value = string.IsNullOrWhiteSpace(strValue) ? null : strValue.Trim();
            DefaultValue = value is null ? null : new Ref<TValue>((TValue)(object)value);
        }
        else
        {
            DefaultValue = new Ref<TValue>(defaultValue);
        }
    }

    public override void Validate(IArgumentParser parser)
    {
        base.Validate(parser);

        if (_inputValues.Count <= 0) 
            return;

        var resultValues = GetValidatedValues(parser.CaseSensitive);

        // if last switch option contains value assign it,
        // otherwise we will use default value
        if (!string.IsNullOrWhiteSpace(_valueTokens[^1]))
        {
            ResultValue = new Ref<TValue>(resultValues[^1]);
        }
    }

    public override void ApplyOptionResult(IApplicationOptions appOptions, PropertyInfo keyProperty)
    {
        if (!keyProperty.PropertyType.IsAssignableFrom(typeof(TValue))) 
            return;

        var result = ResultValue != null 
            ? ResultValue.Value 
            : DefaultValue != null ? DefaultValue.Value : default;

        keyProperty.SetValue(appOptions, result);
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
    public bool EnableValueTokenSplitting { get; init; }

    public List<TValue> ResultValues => _resultValues.ToList();

    protected readonly List<TValue> _resultValues;

    protected BaseSequentialValueOption(int requiredValueCount, int maximumValueCount)
        : base(requiredValueCount, maximumValueCount)
    {
        _resultValues = new List<TValue>();
    }

    public override void Validate(IArgumentParser parser)
    {
        base.Validate(parser);

        if (_inputValues.Count <= 0)
            return;

        var resultValues = GetValidatedValues(parser.CaseSensitive);

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
        _predicates.Add(predicate);
    }

    public void SetAllowedValues(params TValue[] allowedValues)
    {
        var values = allowedValues
            .Where(s => s is not string value || !string.IsNullOrWhiteSpace(value))
            .Select(s => s is string value ? (TValue)(object)value.Trim() : s);

        foreach (var value in values)
            _allowedValues.Add(value);
    }

    protected override List<TValue> GetValidatedValues(bool caseSensitive = false)
    {
        var resultValues = base.GetValidatedValues(caseSensitive);

        resultValues.ValidateByAllowedValues(_allowedValues, caseSensitive);

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
        var type = optionCallbacks.GetType();

        // only use overriden methods
        var methodName = nameof(IParsingCallbacks<TValue>.TryParseValue);
        if (type.GetMethod(methodName)?.DeclaringType == type)
            _tryParseValueCallback ??= optionCallbacks.TryParseValue;

        methodName = nameof(IParsingCallbacks<TValue>.TryParseValues);
        if (type.GetMethod(methodName)?.DeclaringType == type)
            _tryParseValuesCallback ??= optionCallbacks.TryParseValues;
    }

    public void SetTryParseValueCallback(TryParse<TValue> callback)
    {
        _tryParseValueCallback = callback;
    }

    public void SetTryParseValuesCallback(Func<List<string>, List<TValue>> callback)
    {
        _tryParseValuesCallback = callback;
    }

    public override void Initialize(IArgumentParser parser)
    {
        base.Initialize(parser);

        if (DefaultParsingCallbacks<TValue>.Instance.IsValidParser)
            SetParsingCallbacks(DefaultParsingCallbacks<TValue>.Instance);

        if (_tryParseValueCallback is null && _tryParseValuesCallback is null)
        {
            throw new Exception(
                $"Missing TryParse callback for custom type! Configure a TryParse callback for type '{typeof(TValue).Name}'.");
        }
    }

    protected virtual List<TValue> GetValidatedValues(bool caseSensitive = false)
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
        ValueArity = valueArity.GetLimits();
    }

    public void SetValueArity(int requiredValueCount, int maximumValueCount)
    {
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