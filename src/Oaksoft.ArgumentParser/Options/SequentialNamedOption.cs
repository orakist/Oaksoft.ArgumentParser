﻿using System;
using System.Collections.Generic;
using System.Linq;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Options;

internal sealed class SequentialNamedOption<TValue> 
    : BaseSequentialValueOption<TValue>, ISequentialNamedOption<TValue>
    where TValue : IComparable, IEquatable<TValue>
{
    public bool AllowSequentialValues { get; private set; }

    public string ShortAlias => _prefixAliases.MinBy(k => k.Length)!;

    public List<string> Aliases => _prefixAliases.ToList();

    public List<string> OptionTokens => _optionTokens.ToList();

    public override int OptionCount => _optionTokens.Count;

    private readonly List<string> _aliases;
    private readonly List<string> _optionTokens;
    private readonly List<string> _prefixAliases;

    public SequentialNamedOption(
        int requiredOptionCount, int maximumOptionCount, int requiredValueCount, int maximumValueCount)
        : base(requiredValueCount, maximumValueCount)
    {
        OptionArity = (requiredOptionCount, maximumOptionCount);
        AllowSequentialValues = true;

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

    public override void AddAliases(bool skipValidation, params string[] aliases)
    {
        ParserInitializedGuard();

        var values = aliases
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => s.Trim().ValidateAlias(skipValidation));

        _aliases.AddRange(values.Distinct());
    }

    public void SetAllowSequentialValues(bool enabled)
    {
        ParserInitializedGuard();

        AllowSequentialValues = enabled;
    }

    public override void Initialize()
    {
        base.Initialize();

        var prefixedAliases = _aliases.GetPrefixedAliases(
            _parser!.OptionPrefix, _parser!.CaseSensitive);
        _prefixAliases.AddRange(prefixedAliases);

        if (string.IsNullOrWhiteSpace(Usage))
            Usage = $"{ShortAlias}{(ValueArity.Min > 0 ? " <value>" : " (value)")}";

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

                // parse --option (optional value)
                // parse --option val (single value)
                // parse --option val1 val2 val3 (sequential values)
                if (token.Value is null)
                {
                    for (; i + 1 < tokens.Length; ++i)
                    {
                        var nextToken = tokens[i + 1];
                        if (!nextToken.IsOnlyValue)
                            break;

                        nextToken.IsParsed = true;
                        _valueTokens.Add(nextToken.Value!);

                        if (!AllowSequentialValues)
                            break;
                    }
                }
                else
                {                
                    // parse --option=val or -o=val or -oval
                    // parse --option:val or -o:val
                    // parse "--option val" or "-o val"
                    _valueTokens.Add(token.Value);
                }

                break;
            }
        }

        // parse multiple values 'str1;str2;str3'
        var inputValues = _valueTokens.SelectMany(SplitByValueDelimiter);
        _inputValues.AddRange(inputValues);
    }

    public override void Validate()
    {
        base.Validate();

        IsValid = true;
    }

    public override void Clear()
    {
        base.Clear();
        _optionTokens.Clear();
    }
}
