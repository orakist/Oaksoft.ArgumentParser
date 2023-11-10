using System;
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

    public override void AddAliases(params string[] aliases)
    {
        ParserInitializedGuard();

        var values = aliases
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => s.Trim());

        _aliases.AddRange(values);
    }

    public void SetAllowSequentialValues(bool enabled)
    {
        ParserInitializedGuard();

        AllowSequentialValues = enabled;
    }

    public override void Initialize()
    {
        base.Initialize();

        if (_aliases.Count < 1)
            throw new ArgumentException("Option alias not found! Use WithAliases() to set aliases of the option.");

        for (var index = 0; index < _aliases.Count; ++index)
        {
            var alias = _aliases[index].ValidateAlias();

            _aliases[index] = _parser!.CaseSensitive ? alias : alias.ToLowerInvariant();
        }

        var aliases = _aliases.GetPrefixedAliases(_parser!.OptionPrefix);
        _prefixAliases.AddRange(aliases.OrderByDescending(a => a.Length).ToList());

        if (string.IsNullOrWhiteSpace(Usage))
            Usage = $"{ShortAlias}{(ValueArity.Min > 0 ? " <value>" : " (value)")}";
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
        var inputValues = _valueTokens.GetInputValues(_parser.ValueDelimiter, EnableValueTokenSplitting);
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
