using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Callbacks;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Options;

internal sealed class ScalarCommandOption<TValue> 
    : ScalarCommandOption, IScalarCommandOption<TValue>, IValueContext<TValue>
    where TValue : IComparable, IEquatable<TValue>
{
    public override int ValidInputCount => _validated ? _resultValues.Count : 0;

    public TValue? DefaultValue { get; private set; }

    public List<TValue?> Constraints => _constraints.ToList();

    public List<TValue> AllowedValues => _allowedValues.ToList();

    public List<TValue> ResultValues => _resultValues.ToList();

    private Func<string, bool>? _validateValueDelegate;
    private Func<string, TValue>? _convertValueDelegate;
    private Func<IValueContext<TValue>, IArgumentParser, bool>? _validateOptionDelegate;

    private readonly List<TValue?> _constraints;
    private readonly HashSet<TValue> _allowedValues;
    private readonly List<TValue> _resultValues;

    public ScalarCommandOption(
        bool valueTokenMustExist = true, int requiredTokenCount = 0, int maximumTokenCount = 1)
        : base(valueTokenMustExist, requiredTokenCount, maximumTokenCount)
    {
        _constraints = new List<TValue?>();
        _resultValues = new List<TValue>();
        _allowedValues = new HashSet<TValue>();
        ValueTokenMustExist = valueTokenMustExist;
    }

    public void SetDefaultValue(TValue? defaultValue)
    {
        if (defaultValue is string strValue)
        {
            var value = string.IsNullOrWhiteSpace(strValue) ? null : strValue.Trim();
            defaultValue = (TValue?)(object?)value;
        }

        DefaultValue = defaultValue;
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

        if(DefaultParsingCallbacks<TValue>.Instance.IsValidParser)
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

        _validated = true;
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
            throw new Exception($"Missing value validator! Configure a value validator for type '{typeof(TValue).Name}'.");
        }

        if (_convertValueDelegate is null)
        {
            throw new Exception($"Missing value convertor! Configure a value convertor for type '{typeof(TValue).Name}'.");
        }
    }
}

internal abstract class ScalarCommandOption : CommandOption, IScalarCommandOption
{
    public bool EnableValueTokenSplitting { get; init; }

    public bool ValueTokenMustExist { get; init; }

    public bool AllowSequentialValues { get; init; }

    public List<string> ValueTokens => _valueTokens.ToList();

    public List<string> InputValues => _inputValues.ToList();

    private readonly List<string> _valueTokens;
    protected readonly List<string> _inputValues;

    protected ScalarCommandOption(
        bool valueTokenMustExist, int requiredTokenCount, int maximumTokenCount)
        : base(requiredTokenCount, maximumTokenCount)
    {
        _valueTokens = new List<string>();
        _inputValues = new List<string>();
        ValueTokenMustExist = valueTokenMustExist;
    }

    public override void Initialize(IArgumentParser parser)
    {
        base.Initialize(parser);

        if (string.IsNullOrWhiteSpace(Usage))
        {
            Usage = $"{Command}{(ValueTokenMustExist ? " <value>" : " (value)")}";
        }
    }

    public override void Parse(string[] arguments, IArgumentParser parser)
    {
        var flag = parser.ComparisonFlag();
        var separator = parser.TokenSeparator;
        for (var index = 0; index < arguments.Length; ++index)
        {
            var argument = arguments[index];

            if (!Commands.Any(c => argument.StartsWith(c, flag)))
                continue;

            // parse --cmd (optional value)
            // parse --cmd val (single value)
            // parse --cmd val1 val2 val3 (sequential values)
            if (Commands.Any(c => argument.Equals(c, flag)))
            {
                _commandTokens.Add(argument);

                for (; index + 1 < arguments.Length; ++index)
                {
                    var value = arguments[index + 1];
                    if (value.StartsWith(parser.CommandPrefix))
                        break;

                    _valueTokens.Add(value);
                    ++index;

                    if (!AllowSequentialValues)
                        break;
                }
            }

            // parse --cmd=val
            else if (Commands.Any(c => argument.StartsWith($"{c}{separator}", flag)))
            {
                if (argument.Split(separator).Length > 2)
                    throw new Exception($"Invalid argument '{argument}' found! Multiple token separator usage!");

                var keyValue = argument.EnumerateBySeparator(separator).ToArray();
                _commandTokens.Add(keyValue[0]);
                if (keyValue.Length > 1)
                    _valueTokens.Add(keyValue[1]);
            }
        }

        // parse multiple values 'str1;str2;str3'
        var inputValues = parser.GetInputValues(_valueTokens, EnableValueTokenSplitting);
        _inputValues.AddRange(inputValues);
    }

    public override void Validate(IArgumentParser parser)
    {
        base.Validate(parser);

        if (!AllowSequentialValues && _valueTokens.Count > CommandTokens.Count)
            throw new Exception("An option command found with too many values.");

        if (ValueTokenMustExist && _valueTokens.Count < CommandTokens.Count)
            throw new Exception("An option command found with a missing value.");

        if (_valueTokens.Any(string.IsNullOrWhiteSpace))
            throw new Exception("Any provided option value cannot be empty.");
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
