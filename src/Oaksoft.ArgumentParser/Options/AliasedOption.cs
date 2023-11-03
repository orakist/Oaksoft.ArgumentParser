using System;
using System.Collections.Generic;
using System.Linq;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Options;

internal abstract class AliasedOption : BaseOption, IAliasedOption
{
    public string ShortAlias => _aliases.MinBy(k => k.Length)!;

    public List<string> Aliases => _aliases.ToList();

    public List<string> CommandTokens => _commandTokens.ToList();

    protected List<string> _aliases;
    protected List<string> _commandTokens;
    private static readonly char[] _trimChars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ' ' };

    protected AliasedOption(int requiredTokenCount, int maximumTokenCount)
        : base(requiredTokenCount, maximumTokenCount)
    {
        _aliases = new List<string>();
        _commandTokens = new List<string>();
    }

    public void SetAliases(params string[] aliases)
    {
        var values = aliases
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => s.Trim());

        _aliases.AddRange(values);
    }

    public override void Initialize(IArgumentParser parser)
    {
        base.Initialize(parser);

        AddCommandsHeuristically();

        if (_aliases.Count < 1)
            throw new ArgumentException($"Command name not found! Use AddCommands() to set commands of an option. Property: {KeyProperty}");

        for (var index = 0; index < _aliases.Count; ++index)
        {
            if (!_aliases[index].StartsWith(parser.CommandPrefix))
                _aliases[index] = $"{parser.CommandPrefix}{_aliases[index]}";
        }

        if (string.IsNullOrWhiteSpace(Name))
        {
            Name = KeyProperty.Name;
        }

        if (string.IsNullOrWhiteSpace(Description))
        {
            Description = $"Command option to get '{Name}'.";
        }
    }

    public override void Validate(IArgumentParser parser)
    {
        if (_commandTokens.Count < RequiredTokenCount)
            throw new Exception($"Missing option. Required Option Count: {RequiredTokenCount}");

        if (_commandTokens.Count > MaximumTokenCount)
            throw new Exception($"An option with too many entries. Maximum Option Count: {MaximumTokenCount}");

        if (_commandTokens.Any(string.IsNullOrWhiteSpace))
            throw new Exception("Any provided option command cannot be empty.");
    }

    public override void Clear()
    {
        base.Clear();
        _commandTokens.Clear();
    }


    private void AddCommandsHeuristically()
    {
        if (_aliases.Count > 0)
            return;

        var firstWord = GetFirstWordOfKeyProperty();
        if (firstWord.Length < 1)
            return;

        _aliases.Add(firstWord.Length < 3 ? firstWord : firstWord[..1]);
        if (firstWord.Length > 2)
            _aliases.Add(firstWord);
    }

    private string GetFirstWordOfKeyProperty()
    {
        var property = KeyProperty.Name.Replace('_', ' ')
            .TrimStart(_trimChars).TrimEnd()
            .Split(' ').First();

        if (property.Length < 3)
            return property.ToLowerInvariant();

        var endIndex = 0;
        var firstChar = property[1];
        for (var i = 2; i < property.Length; ++i)
        {
            if (char.IsDigit(property[i]))
            {
                while (char.IsDigit(property[i]))
                {
                    ++i;
                }

                endIndex = i - 1;
                break;
            }

            if (char.IsUpper(property[i]) == char.IsUpper(firstChar) ||
                char.IsLower(property[i]) == char.IsLower(firstChar))
            {
                continue;
            }

            endIndex = char.IsUpper(firstChar) ? i - 2 : i - 1;
            break;
        }

        return property[..(endIndex + 1)].ToLowerInvariant();
    }
}
