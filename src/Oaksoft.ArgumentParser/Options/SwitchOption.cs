using System;
using System.Collections.Generic;
using System.Linq;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Options;

internal sealed class SwitchOption : BaseOption, ISwitchOption
{
    public string ShortAlias => _prefixAliases.MinBy(k => k.Length)!;

    public List<string> Aliases => _prefixAliases.ToList();

    public List<string> OptionTokens => _optionTokens.ToList();

    public bool? DefaultValue { get; private set; }

    public bool? ResultValue { get; private set; }

    public override int OptionCount => _optionTokens.Count;

    public override int ValueCount => ResultValue is null ? 0 : 1;

    private readonly List<string> _aliases;
    private readonly List<string> _optionTokens;
    private readonly List<string> _prefixAliases;

    public SwitchOption(int requiredOptionCount, int maximumOptionCount)
    {
        OptionArity = (requiredOptionCount, maximumOptionCount);
        ValueArity = (0, 1);

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
        DefaultValue = defaultValue;
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
            Usage = ShortAlias;
    }

    public override void Parse(TokenValue[] tokens, IArgumentParser parser)
    {
        bool? resultValue = null;
        var flag = parser.ComparisonFlag();

        for (var i = 0; i < tokens.Length; ++i)
        {
            var token = tokens[i];
            if(token.Invalid || token.IsParsed)
                continue;

            var argument = token.Argument;
            foreach (var alias in _prefixAliases)
            {
                if (!argument.StartsWith(alias, flag))
                    continue;

                token.IsParsed = true;
                _optionTokens.Add(alias);

                // parse --option (optional value)
                // parse --option true (single value)
                if (argument.Length == alias.Length)
                {
                    if (i + 1 < tokens.Length && !tokens[i + 1].IsParsed)
                    {
                        if (bool.TryParse(tokens[i + 1].Argument.ToLowerInvariant(), out var value))
                        {
                            tokens[i + 1].IsParsed = true;
                            resultValue = value;
                        }
                    }

                    break;
                }

                // parse --option=true or -o=true or -otrue
                // parse --option:true or -o:true
                // parse "--option true" or "-o true"
                var optionValue = argument.GetOptionValue(alias, parser.TokenDelimiter);
                if(string.IsNullOrWhiteSpace(optionValue))
                    continue;

                if (bool.TryParse(optionValue.ToLowerInvariant(), out var result))
                    throw new Exception($"Switch option value was expected to be boolean but '{optionValue}' value was provided.");


                resultValue = result;
                break;
            }
        }

        ResultValue = resultValue ?? DefaultValue ?? true;
    }

    public override void Validate(IArgumentParser parser)
    {
        base.Validate(parser);

        if(ResultValue is  null)
            return;

        IsValid = true;
    }

    public override void Clear()
    {
        base.Clear();
        _optionTokens.Clear();
    }
}
