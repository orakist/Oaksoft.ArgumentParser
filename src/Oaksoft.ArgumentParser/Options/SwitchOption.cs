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
        ParserInitializedGuard();

        OptionArity = optionArity.GetLimits();
    }

    public void SetOptionArity(int requiredOptionCount, int maximumOptionCount)
    {
        ParserInitializedGuard();

        OptionArity = (requiredOptionCount, maximumOptionCount);
    }

    public override List<string> GetAliases()
    {
        return _aliases;
    }

    public override void AddAliases(params string[] aliases)
    {
        ParserInitializedGuard();

        var values = aliases.Select(s => s.ValidateAlias());
        _aliases.AddRange(values.Distinct());
    }

    public override void SetValidAliases(params string[] aliases)
    {
        _aliases.Clear();
        _aliases.AddRange(aliases);
    }

    public void SetDefaultValue(bool defaultValue)
    {
        ParserInitializedGuard();

        DefaultValue = new Ref<bool>(defaultValue);
    }

    public override void Initialize()
    {
        base.Initialize();

        var prefixedAliases = _aliases.GetPrefixedAliases(_parser!.OptionPrefix);
        _prefixAliases.AddRange(prefixedAliases);

        if (string.IsNullOrWhiteSpace(Usage))
            Usage = ShortAlias;

        if (string.IsNullOrWhiteSpace(Description))
            Description = $"Performs '{Name}' option.";
    }

    public override void Parse(TokenItem[] tokens)
    {
        var compareFlag = _parser!.CaseSensitive
            ? StringComparison.Ordinal
            : StringComparison.OrdinalIgnoreCase;

        for (var i = 0; i < tokens.Length; ++i)
        {
            var token = tokens[i];
            if (token.Invalid || token.IsParsed || token.Alias is null)
                continue;

            foreach (var alias in _prefixAliases)
            {
                if (!token.Alias.Equals(alias, compareFlag))
                    continue;

                token.IsParsed = true;
                _optionTokens.Add(alias);
                _valueTokens.Add(string.Empty);

                // parse --option (optional value)
                // parse --option true (single value)
                if (token.Value is null)
                {
                    if (i + 1 < tokens.Length && tokens[i + 1].IsOnlyValue)
                    {
                        tokens[i + 1].IsParsed = true;
                        _valueTokens[^1] = tokens[i + 1].Value!;
                    }
                }
                else
                {
                    // parse --option=true or -o=true or -otrue
                    // parse --option:true or -o:true
                    // parse "--option true" or "-o true"
                    _valueTokens[^1] = token.Value;
                }

                break;
            }
        }

        _inputValues.AddRange(_valueTokens.Where(v => !string.IsNullOrWhiteSpace(v)));
    }

    public override void Validate()
    {
        base.Validate();

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