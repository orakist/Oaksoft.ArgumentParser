﻿using System;
using System.Collections.Generic;
using System.Linq;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Options;

internal sealed class ScalarNamedOption<TValue>
    : BaseScalarValueOption<TValue>, IScalarNamedOption<TValue>
    where TValue : IComparable, IEquatable<TValue>
{
    public string ShortAlias => _prefixAliases.MinBy(k => k.Length)!;

    public List<string> Aliases => _prefixAliases.ToList();

    public List<string> OptionTokens => _optionTokens.ToList();

    public override int OptionCount => _optionTokens.Count;

    private readonly List<string> _aliases;
    private readonly List<string> _optionTokens;
    private readonly List<string> _prefixAliases;

    public ScalarNamedOption(
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
                _valueTokens.Add(string.Empty);

                // parse --option (optional value)
                // parse --option val (single value)
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
                    // parse --option=val or -o=val or -oval
                    // parse --option:val or -o:val
                    // parse "--option val" or "-o val"
                    _valueTokens[^1] = token.Value;
                }

                break;
            }
        }

        // add parsed values to input list for validation
        _inputValues.AddRange(_valueTokens.Where(v => !string.IsNullOrWhiteSpace(v)));
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
