using System;
using System.Collections.Generic;
using System.Linq;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Options;

internal sealed class SwitchOption : BaseOption, ISwitchOption
{
    public string ShortAlias => _aliases.MinBy(k => k.Length)!;

    public List<string> Aliases => _aliases.ToList();

    public List<string> OptionTokens => _optionTokens.ToList();

    public bool? DefaultValue { get; private set; }

    public override int OptionCount => _optionTokens.Count;

    public override int ValueCount => 0;

    private readonly List<string> _aliases;
    private readonly List<string> _optionTokens;

    public SwitchOption(int requiredOptionCount, int maximumOptionCount)
    {
        OptionArity = (requiredOptionCount, maximumOptionCount);
        ValueArity = (0, 0);

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

    public void SetDefaultValue(bool defaultValue)
    {
        DefaultValue = defaultValue;
    }
    
    public override void Initialize(IArgumentParser parser)
    {
        base.Initialize(parser);

        if (string.IsNullOrWhiteSpace(Usage))
            Usage = ShortAlias;

        if (_aliases.Count < 1)
            throw new ArgumentException("Option alias not found! Use WithAliases() to set aliases of the option.");

        for (var index = 0; index < _aliases.Count; ++index)
        {
            if (!_aliases[index].StartsWith(parser.OptionPrefix))
                _aliases[index] = $"{parser.OptionPrefix}{_aliases[index]}";
        }
    }

    public override void Parse(string[] arguments, IArgumentParser parser)
    {
        foreach (var argument in arguments)
        {
            if (!_aliases.Any(c => c.Equals(argument, parser.ComparisonFlag())))
                continue;

            _optionTokens.Add(argument);
        }
    }

    public override void Validate(IArgumentParser parser)
    {
        base.Validate(parser);

        IsValid = true;
    }

    public override void Clear()
    {
        base.Clear();
        _optionTokens.Clear();
    }
}
