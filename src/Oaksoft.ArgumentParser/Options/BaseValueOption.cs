using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Callbacks;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Options;

internal abstract class BaseValueOption<TValue> 
    : BaseValueOption, IValueOption<TValue>, IValueContext<TValue>
    where TValue : IComparable, IEquatable<TValue>
{
    public TValue? DefaultValue { get; private set; }

    public List<TValue?> Constraints => _constraints.ToList();

    public List<TValue> AllowedValues => _allowedValues.ToList();

    public List<TValue> ResultValues => _resultValues.ToList();

    private Func<string, bool>? _validateValueDelegate;
    private Func<string, TValue>? _convertValueDelegate;
    private Func<IValueContext<TValue>, IArgumentParser, bool>? _validateOptionDelegate;

    private readonly List<TValue?> _constraints;
    private readonly HashSet<TValue> _allowedValues;
    protected readonly List<TValue> _resultValues;

    protected BaseValueOption(int requiredValueCount, int maximumValueCount)
        : base(requiredValueCount, maximumValueCount)
    {
        _constraints = new List<TValue?>();
        _resultValues = new List<TValue>();
        _allowedValues = new HashSet<TValue>();
    }

    public void SetDefaultValue(TValue defaultValue)
    {
        if (defaultValue is string strValue)
        {
            var value = string.IsNullOrWhiteSpace(strValue) ? null : strValue.Trim();
            DefaultValue = (TValue?)(object?)value;
        }
        else
        {
            DefaultValue = defaultValue;
        }
    }

    public void SetConstraints(params TValue?[] constraints)
    {
        _constraints.AddRange(
            constraints.Select(s => s is string value ? (TValue)(object)value.Trim() : s));
    }

    public void SetAllowedValues(params TValue[] allowedValues)
    {
        var values = allowedValues
            .Where(s => s is not string value || !string.IsNullOrWhiteSpace(value))
            .Select(s => s is string value ? (TValue)(object)value.Trim() : s);

        foreach (var value in values)
            _allowedValues.Add(value);
    }

    public void SetParsingCallbacks(IParsingCallbacks<TValue> optionCallbacks)
    {
        var type = optionCallbacks.GetType();

        // only use overriden methods
        var methodName = nameof(IParsingCallbacks<TValue>.ValidateValue);
        if (type.GetMethod(methodName)?.DeclaringType == type)
            _validateValueDelegate ??= optionCallbacks.ValidateValue;

        methodName = nameof(IParsingCallbacks<TValue>.ConvertValue);
        if (type.GetMethod(methodName)?.DeclaringType == type)
            _convertValueDelegate ??= optionCallbacks.ConvertValue;

        methodName = nameof(IParsingCallbacks<TValue>.ValidateOption);
        if (type.GetMethod(methodName)?.DeclaringType == type)
            _validateOptionDelegate ??= optionCallbacks.ValidateOption;
    }

    public void SetValueValidator(Func<string, bool> validator)
    {
        _validateValueDelegate = validator;
    }

    public void SetValueConvertor(Func<string, TValue> convertor)
    {
        _convertValueDelegate = convertor;
    }

    public void SetOptionValidator(Func<IValueContext<TValue>, IArgumentParser, bool> validator)
    {
        _validateOptionDelegate = validator;
    }

    public override void Initialize(IArgumentParser parser)
    {
        base.Initialize(parser);

        if (DefaultParsingCallbacks<TValue>.Instance.IsValidParser)
            SetParsingCallbacks(DefaultParsingCallbacks<TValue>.Instance);

        CallbackValidatorGuard();
    }

    public override void Validate(IArgumentParser parser)
    {
        base.Validate(parser);

        if (_validateOptionDelegate is not null)
        {
            if (!_validateOptionDelegate.Invoke(this, parser))
                throw new Exception("Option values cannot be validated.");
        }
        else
        {
            CallbackValidatorGuard();

            foreach (var inputValue in _inputValues.Where(v => !_validateValueDelegate!.Invoke(v)))
            {
                throw new Exception($"Invalid input value found!. Value: {inputValue}");
            }

            var values = _inputValues.Select(v => _convertValueDelegate!.Invoke(v)).ToList();
            parser.ValidateByAllowedValues(values, _allowedValues);

            _resultValues.AddRange(values);
        }
    }

    public override void ApplyDefaultValue(IApplicationOptions appOptions, PropertyInfo keyProperty)
    {
        if (DefaultValue is null)
            return;

        if (!keyProperty.PropertyType.IsAssignableFrom(typeof(TValue)))
            return;

        keyProperty.SetValue(appOptions, DefaultValue);
    }

    public override void UpdatePropertyValue(IApplicationOptions appOptions, PropertyInfo keyProperty)
    {
        if (keyProperty.PropertyType.IsAssignableFrom(typeof(List<TValue>)))
        {
            keyProperty.SetValue(appOptions, _resultValues);
        }
        else if (keyProperty.PropertyType.IsAssignableFrom(typeof(TValue[])))
        {
            keyProperty.SetValue(appOptions, _resultValues.ToArray());
        }
        else if (keyProperty.PropertyType.IsAssignableFrom(typeof(TValue)))
        {
            keyProperty.SetValue(appOptions, _resultValues.First());
        }
    }

    public override void Clear()
    {
        base.Clear();
        _resultValues.Clear();
    }

    private void CallbackValidatorGuard()
    {
        if (_validateOptionDelegate is null)
        {
            throw new Exception(
                $"Missing value validator! Configure a value validator for type '{typeof(TValue).Name}'.");
        }

        if (_convertValueDelegate is null)
        {
            throw new Exception(
                $"Missing value convertor! Configure a value convertor for type '{typeof(TValue).Name}'.");
        }
    }
}

internal abstract class BaseValueOption : BaseOption, IValueOption
{
    public bool EnableValueTokenSplitting { get; init; }

    public List<string> ValueTokens => _valueTokens.ToList();

    public List<string> InputValues => _inputValues.ToList();

    public override int ValueCount => _inputValues.Count;

    private protected List<string> _valueTokens;
    private protected List<string> _inputValues;

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

    public override void Parse(string[] arguments, IArgumentParser parser)
    {
        var options = ((BaseArgumentParser)parser).AppOptions.Options;

        var compareFlag = parser.ComparisonFlag();
        for (var index = 0; index < arguments.Length; ++index)
        {
            var argument = arguments[index];

            if (!argument.StartsWith(parser.CommandPrefix))
                _valueTokens.Add(argument);

            var scalarCommand = options.OfType<IScalarOption>()
                .FirstOrDefault(o => o.Aliases.Any(c => c.Equals(argument, compareFlag)));

            if (scalarCommand is null)
                continue;

            for (; index + 1 < arguments.Length; ++index)
            {
                var value = arguments[index + 1];
                if (value.StartsWith(parser.CommandPrefix))
                    break;

                ++index;

                if (!scalarCommand.AllowSequentialValues)
                    break;
            }
        }

        var inputValues = parser.GetInputValues(_valueTokens, EnableValueTokenSplitting);
        _inputValues.AddRange(inputValues);
    }

    public abstract void ApplyDefaultValue(IApplicationOptions appOptions, PropertyInfo keyProperty);

    public abstract void UpdatePropertyValue(IApplicationOptions appOptions, PropertyInfo keyProperty);

    public override void Clear()
    {
        base.Clear();
        _valueTokens.Clear();
        _inputValues.Clear();
    }
}