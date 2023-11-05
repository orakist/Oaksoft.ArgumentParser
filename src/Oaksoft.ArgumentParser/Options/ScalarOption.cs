using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Options;

internal sealed class ScalarOption<TValue> : BaseValueOption<TValue>, IScalarOption<TValue>
    where TValue : IComparable, IEquatable<TValue>
{
    public string ShortAlias => _aliases.MinBy(k => k.Length)!;

    public List<string> Aliases => _aliases.ToList();

    public List<string> OptionTokens => _optionTokens.ToList();

    public override int OptionCount => _optionTokens.Count;

    public bool AllowSequentialValues { get; init; }

    private readonly List<string> _aliases;
    private readonly List<string> _optionTokens;

    public ScalarOption(
        int requiredOptionCount, int maximumOptionCount, int requiredValueCount, int maximumValueCount)
        : base(requiredValueCount, maximumValueCount)
    {
        OptionArity = (requiredOptionCount, maximumOptionCount);

        _aliases = new List<string>();
        _optionTokens = new List<string>();
    }

    public void SetOptionArity(ArityType optionArity)
    {
        OptionArity = optionArity.GetLimits();
    }

    public void SetOptionArity(int requiredOptionCount, int maximumOptionCount)
    {
        OptionArity = (requiredOptionCount, maximumOptionCount);
    }

    public override void SetAliases(params string[] aliases)
    {
        var values = aliases
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => s.Trim());

        _aliases.AddRange(values);
    }

    public override void Initialize(IArgumentParser parser)
    {
        base.Initialize(parser);

        if (_aliases.Count < 1)
            throw new ArgumentException("Option alias not found! Use WithAliases() to set aliases of the option.");

        for (var index = 0; index < _aliases.Count; ++index)
        {
            if (!_aliases[index].StartsWith(parser.OptionPrefix))
                _aliases[index] = $"{parser.OptionPrefix}{_aliases[index]}";

            if (!parser.CaseSensitive)
                _aliases[index] = _aliases[index].ToLowerInvariant();
        }

        if (string.IsNullOrWhiteSpace(Usage))
        {
            Usage = $"{ShortAlias}{(ValueArity.Min > 0 ? " <value>" : " (value)")}";
        }
    }

    public override void Parse(string[] arguments, IArgumentParser parser)
    {
        var flag = parser.ComparisonFlag();
        var delimiter = parser.TokenDelimiter;
        for (var index = 0; index < arguments.Length; ++index)
        {
            var argument = arguments[index];

            if (!Aliases.Any(c => argument.StartsWith(c, flag)))
                continue;

            // parse --cmd (optional value)
            // parse --cmd val (single value)
            // parse --cmd val1 val2 val3 (sequential values)
            if (Aliases.Any(c => argument.Equals(c, flag)))
            {
                _optionTokens.Add(argument);

                for (; index + 1 < arguments.Length; ++index)
                {
                    var value = arguments[index + 1];
                    if (value.StartsWith(parser.OptionPrefix))
                        break;

                    _valueTokens.Add(value);
                    ++index;

                    if (!AllowSequentialValues)
                        break;
                }
            }

            // parse --cmd=val
            else if (Aliases.Any(c => argument.StartsWith($"{c}{delimiter}", flag)))
            {
                if (argument.Split(delimiter).Length > 2)
                    throw new Exception($"Invalid (option=value) token '{argument}' found! Multiple token delimiter usage!");

                var keyValue = argument.EnumerateByDelimiter(delimiter).ToArray();
                _optionTokens.Add(keyValue[0]);
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

        IsValid = true;
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
        _optionTokens.Clear();
    }
}
