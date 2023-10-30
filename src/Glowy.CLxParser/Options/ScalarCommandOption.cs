using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Glowy.CLxParser.Callbacks;
using Glowy.CLxParser.Parser;

namespace Glowy.CLxParser.Options;

internal sealed class ScalarCommandOption : CommandOption, IScalarCommandOption, IValueContext
{
    public bool EnableValueTokenSplitting { get; init; }

    public bool ValueTokenMustExist { get; init; }

    public bool AllowSequentialValues { get; init; }

    public string? DefaultValue { get; private set; }

    public List<string?> Constraints => _constraints.ToList();

    public List<string> AllowedValues => _allowedValues.ToList();

    public List<string> ValueTokens => _valueTokens.ToList();

    public List<string> ParsedValues => _parsedValues.ToList();

    private Func<string, bool>? _validateValueAction;
    private Func<IValueContext, IArgumentParser, bool>? _validateOptionAction;
    private Action<IValueContext, IApplicationOptions, PropertyInfo>? _applyDefaultValueAction;
    private Action<IValueContext, IApplicationOptions, PropertyInfo>? _updateOptionValueAction;

    private readonly List<string?> _constraints;
    private readonly HashSet<string> _allowedValues;
    private readonly List<string> _valueTokens;
    private readonly List<string> _parsedValues;

    public ScalarCommandOption(
        bool valueTokenMustExist = true, int requiredTokenCount = 0, int maximumTokenCount = 1)
        : base(requiredTokenCount, maximumTokenCount)
    {
        _constraints = new List<string?>();
        _valueTokens = new List<string>();
        _parsedValues = new List<string>();
        _allowedValues = new HashSet<string>();
        ValueTokenMustExist = valueTokenMustExist;
    }

    public void SetDefaultValue(string? defaultValue)
    {
        DefaultValue = string.IsNullOrWhiteSpace(defaultValue)
            ? null : defaultValue.Trim();
    }

    public void SetConstraints(params string?[] constraints)
    {

        _constraints.AddRange(constraints.Select(s => s?.Trim()));
    }

    public void SetAllowedValues(params string[] allowedValues)
    {
        var values = allowedValues
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => s.Trim());

        foreach (var value in values)
            _allowedValues.Add(value);
    }

    public void SetParsingCallbacks(IParsingCallbacks optionCallbacks)
    {
        var type = optionCallbacks.GetType();

        var methodName = nameof(IParsingCallbacks.ValidateValue);
        if (type.GetMethod(methodName)?.DeclaringType == type)
            _validateValueAction ??= optionCallbacks.ValidateValue;

        methodName = nameof(IParsingCallbacks.ValidateOption);
        if (type.GetMethod(methodName)?.DeclaringType == type)
            _validateOptionAction ??= optionCallbacks.ValidateOption;

        methodName = nameof(IParsingCallbacks.ApplyDefaultValue);
        if (type.GetMethod(methodName)?.DeclaringType == type)
            _applyDefaultValueAction ??= optionCallbacks.ApplyDefaultValue;

        methodName = nameof(IParsingCallbacks.UpdateOptionValue);
        if (type.GetMethod(methodName)?.DeclaringType == type)
            _updateOptionValueAction ??= optionCallbacks.UpdateOptionValue;
    }

    public void SetBaseValidator(Func<string, bool> validator)
    {
        _validateValueAction = validator;
    }

    public void SetOptionValidator(Func<IValueContext, IArgumentParser, bool> validator)
    {
        _validateOptionAction = validator;
    }

    public void SetDefaultValueSetterAction(Action<IValueContext, IApplicationOptions, PropertyInfo> setterAction)
    {
        _applyDefaultValueAction = setterAction;
    }

    public void SetOptionValueSetterAction(Action<IValueContext, IApplicationOptions, PropertyInfo> setterAction)
    {
        _updateOptionValueAction = setterAction;
    }

    public override void Initialize(IArgumentParser parser)
    {
        base.Initialize(parser);

        if (string.IsNullOrWhiteSpace(Usage))
        {
            Usage = $"{Command}{(ValueTokenMustExist ? " <value>" : " (value)")}";
        }

        _validateValueAction?.ValidateDefaultValue(DefaultValue);
        _validateValueAction?.ValidateAllowedValues(_allowedValues);
        _validateValueAction?.ValidateConstraints(_constraints);
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
        var parsedValues = parser.GetParsedValues(_valueTokens, EnableValueTokenSplitting);
        _parsedValues.AddRange(parsedValues);
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

        if (_validateOptionAction is not null)
        {
            if (!_validateOptionAction.Invoke(this, parser))
                throw new Exception("Option values cannot be validated.");
        }
        else
        {
            parser.ValidateByAllowedValues(_parsedValues, _allowedValues);
        }

        _validated = true;
    }

    public override void Clear()
    {
        base.Clear();
        _valueTokens.Clear();
        _parsedValues.Clear();
    }

    public void ApplyDefaultValue(IApplicationOptions appOptions, PropertyInfo keyProperty)
    {
        _applyDefaultValueAction?.Invoke(this, appOptions, keyProperty);
    }

    public void UpdatePropertyValue(IApplicationOptions appOptions, PropertyInfo keyProperty)
    {
        _updateOptionValueAction?.Invoke(this, appOptions, keyProperty);
    }
}
