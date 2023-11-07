using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Options;

internal sealed class ScalarOption<TValue> : BaseValueOption<TValue>, IScalarOption<TValue>
    where TValue : IComparable, IEquatable<TValue>
{
    public string ShortAlias => _prefixAliases.MinBy(k => k.Length)!;

    public List<string> Aliases => _prefixAliases.ToList();

    public List<string> OptionTokens => _optionTokens.ToList();

    public override int OptionCount => _optionTokens.Count;

    public bool AllowSequentialValues { get; init; }

    private readonly List<string> _aliases;
    private readonly List<string> _optionTokens;
    private readonly List<string> _prefixAliases;

    public ScalarOption(
        int requiredOptionCount, int maximumOptionCount, int requiredValueCount, int maximumValueCount)
        : base(requiredValueCount, maximumValueCount)
    {
        OptionArity = (requiredOptionCount, maximumOptionCount);

        _aliases = new List<string>();
        _optionTokens = new List<string>();
        _prefixAliases = new List<string>();
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

    public override bool StartsWithAnyAlias(string token, StringComparison flag)
    {
        return _prefixAliases.Any(a => token.StartsWith(a, flag));
    }

    public override void Initialize(IArgumentParser parser)
    {
        base.Initialize(parser);

        if (_aliases.Count < 1)
            throw new ArgumentException("Option alias not found! Use WithAliases() to set aliases of the option.");

        for (var index = 0; index < _aliases.Count; ++index)
        {
            var alias = _aliases[index].TrimAlias();
            if (string.IsNullOrWhiteSpace(alias) || !char.IsAsciiLetter(alias[0]))
                throw new ArgumentException($"Invalid alias '{_aliases[index]}' found!");

            _aliases[index] = parser.CaseSensitive ? alias : alias.ToLowerInvariant();
        }

        var aliases = parser.OptionPrefix.GetPrefixedAliases(_aliases);
        _prefixAliases.AddRange(aliases.OrderByDescending(a => a.Length).ToList());

        if (string.IsNullOrWhiteSpace(Usage))
            Usage = $"{ShortAlias}{(ValueArity.Min > 0 ? " <value>" : " (value)")}";
    }

    public override void Parse(TokenValue[] tokens, IArgumentParser parser)
    {
        var flag = parser.ComparisonFlag();
        var baseParser = (BaseArgumentParser)parser;

        for (var i = 0; i < tokens.Length; ++i)
        {
            var token = tokens[i];
            if (token.Invalid || token.IsParsed)
                continue;

            var argument = token.Argument;
            foreach (var alias in _prefixAliases)
            {
                if (!token.Argument.StartsWith(alias, flag))
                    continue;

                token.IsParsed = true;
                _optionTokens.Add(alias);

                // parse --option (optional value)
                // parse --option val (single value)
                // parse --option val1 val2 val3 (sequential values)
                if (argument.Length == alias.Length)
                {
                    for (; i + 1 < tokens.Length; ++i)
                    {
                        var nextToken = tokens[i + 1];

                        if (baseParser.StartsWithAnyAlias(argument))
                            break;

                        nextToken.IsParsed = true;
                        _valueTokens.Add(nextToken.Argument);

                        if (!AllowSequentialValues)
                            break;
                    }

                    break;
                }

                // parse --option=val or -o=val or -oval
                // parse --option:val or -o:val
                // parse "--option val" or "-o val"
                var optionValue = argument.GetOptionValue(alias, parser.TokenDelimiter);
                if (string.IsNullOrWhiteSpace(optionValue))
                    continue;

                _valueTokens.Add(optionValue);
                break;
            }
        }

        // parse multiple values 'str1;str2;str3'
        var inputValues = _valueTokens.GetInputValues(parser.ValueDelimiter, EnableValueTokenSplitting);
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
