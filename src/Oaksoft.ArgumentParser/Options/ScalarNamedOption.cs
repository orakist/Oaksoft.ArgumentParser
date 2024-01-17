using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Options;

internal class ScalarNamedOption<TValue>
    : BaseScalarValueOption<TValue>, IScalarNamedOption<TValue>
    where TValue : IComparable, IEquatable<TValue>
{
    public string Alias => _prefixAliases.OrderBy(n => n[0] == '/').ThenBy(n => n.Length).First();

    public List<string> Aliases => _prefixAliases.ToList();

    public List<string> OptionTokens => _optionTokens.ToList();

    public override int OptionCount => _optionTokens.Count;

    public Ref<TValue>? DefaultValue { get; private set; }

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

    public void SetDefaultValue(TValue defaultValue)
    {
        ParserInitializedGuard();

        DefaultValue = new Ref<TValue>(defaultValue);
    }

    public void SetOptionArity(ArityType optionArity)
    {
        ParserInitializedGuard();

        OptionArity = optionArity.GetLimits().GetOrThrow(KeyProperty.Name);
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

    public void AddAliases(params string[] aliases)
    {
        ParserInitializedGuard();

        foreach (var alias in aliases.Select(s => s.ValidateAlias().GetOrThrow(KeyProperty.Name)))
        {
            if(!_aliases.Contains(alias))
                _aliases.Add(alias);
        }
    }

    public override void SetValidAliases(IEnumerable<string> aliases)
    {
        _aliases.Clear();
        _aliases.AddRange(aliases);
    }

    public override void Initialize()
    {
        base.Initialize();

        var prefixedAliases = _aliases.GetPrefixedAliases(_parser!.OptionPrefix);
        _prefixAliases.AddRange(prefixedAliases);

        if (string.IsNullOrWhiteSpace(Usage))
            Usage = $"{Alias}{(ValueArity.Min > 0 ? " <value>" : ValueArity.Max > 0 ? " (value)" : string.Empty)}";

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

    public override void ApplyOptionResult(object appOptions, PropertyInfo keyProperty)
    {
        if (!keyProperty.PropertyType.IsAssignableFrom(typeof(TValue)))
            return;

        var result = ResultValue != null
            ? ResultValue.Value
            : DefaultValue != null ? DefaultValue.Value : default;

        keyProperty.SetValue(appOptions, result);
    }

    public override void Clear()
    {
        base.Clear();
        _optionTokens.Clear();
    }

    public string? GetDefaultValue()
    {
        return DefaultValue?.Value.ToString();
    }
}
