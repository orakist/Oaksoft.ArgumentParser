﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Options;

internal sealed class SwitchOption : BaseValueOption<bool>, ISwitchOption
{
    public string ShortAlias => _prefixAliases.MinBy(k => k.Length)!;

    public List<string> Aliases => _prefixAliases.ToList();

    public List<string> OptionTokens => _optionTokens.ToList();

    public override int OptionCount => _optionTokens.Count;

    public Ref<bool>? DefaultValue { get; private set; }

    public Ref<bool>? ResultValue { get; private set; }

    private readonly List<string> _aliases;
    private readonly List<string> _optionTokens;
    private readonly List<string> _prefixAliases;

    public SwitchOption(int requiredOptionCount, int maximumOptionCount)
        : base(0, 1)
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

    public void SetDefaultValue(bool defaultValue)
    {
        DefaultValue = new Ref<bool>(defaultValue);
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
            Usage = ShortAlias;
    }

    public override void Parse(TokenValue[] tokens, IArgumentParser parser)
    {
        var compareFlag = parser.CaseSensitive
            ? StringComparison.Ordinal
            : StringComparison.OrdinalIgnoreCase;

        for (var i = 0; i < tokens.Length; ++i)
        {
            var token = tokens[i];
            if (token.Invalid || token.IsParsed)
                continue;

            var argument = token.Argument;
            foreach (var alias in _prefixAliases)
            {
                if (!argument.StartsWith(alias, compareFlag))
                    continue;

                token.IsParsed = true;
                _optionTokens.Add(alias);
                _valueTokens.Add(string.Empty);

                // parse --option (optional value)
                // parse --option true (single value)
                if (argument.Length == alias.Length)
                {
                    if (i + 1 < tokens.Length && !tokens[i + 1].IsParsed)
                    {
                        var nextToken = tokens[i + 1];
                        if (bool.TryParse(nextToken.Argument, out _))
                        {
                            nextToken.IsParsed = true;
                            _valueTokens[^1] = nextToken.Argument;
                        }
                    }
                }
                else
                {
                    // parse --option=true or -o=true or -otrue
                    // parse --option:true or -o:true
                    // parse "--option true" or "-o true"
                    var optionValue = argument.GetOptionValue(alias, parser.TokenDelimiter);
                    if (string.IsNullOrWhiteSpace(optionValue))
                        continue;

                    _valueTokens[^1] = optionValue;
                }

                break;
            }
        }

        _inputValues.AddRange(_valueTokens.Where(v => !string.IsNullOrWhiteSpace(v)));
    }

    public override void Validate(IArgumentParser parser)
    {
        base.Validate(parser);

        if (_inputValues.Count > 0)
        {
            var resultValues = GetValidatedValues();

            // if last switch option contains value assign it,
            // otherwise we will use default value
            if (!string.IsNullOrWhiteSpace(_valueTokens[^1]))
            {
                ResultValue = new Ref<bool>(resultValues[^1]);
            }
        }

        IsValid = true;
    }

    public override void ApplyOptionResult(IApplicationOptions appOptions, PropertyInfo keyProperty)
    {
        if (!keyProperty.PropertyType.IsAssignableFrom(typeof(bool)))
            return;

        keyProperty.SetValue(appOptions, ResultValue?.Value ?? DefaultValue?.Value ?? true);
    }

    public override void Clear()
    {
        base.Clear();
        _optionTokens.Clear();
        ResultValue = null;
    }

}
