using System;
using System.Collections.Generic;
using System.Linq;
using Glowy.CLxParser.Parser;

namespace Glowy.CLxParser.Options;

internal sealed class NonCommandOption : BaseOption, INonCommandOption
{
    public override int ValidatedTokenCount => _validated ? _valueTokens.Count : 0;

    public bool EnableValueTokenSplitting { get; init; }

    public string? DefaultValue { get; private set; }

    public List<string?> Constraints => _constraints.ToList();

    public List<string> AllowedValues => _allowedValues.ToList();

    public List<string> ValueTokens => _valueTokens.ToList();

    public List<string> ParsedValues => _parsedValues.ToList();

    private readonly List<string?> _constraints;
    private readonly HashSet<string> _allowedValues;
    private readonly List<string> _valueTokens;
    private readonly List<string> _parsedValues;

    public NonCommandOption(int requiredTokenCount = 0, int maximumTokenCount = 1)
        : base(requiredTokenCount, maximumTokenCount)
    {
        _constraints = new List<string?>();
        _valueTokens = new List<string>();
        _parsedValues = new List<string>();
        _allowedValues = new HashSet<string>();
    }

    public void SetDefaultValue(string? defaultValue)
    {
        DefaultValue = string.IsNullOrWhiteSpace(defaultValue) 
            ? null : defaultValue.Trim();
    }

    public void SetConstraints(params string?[] constraints)
    {
        _constraints.AddRange(constraints.Select(s => s?.Trim()));
    }

    public void SetAllowedValues(params string[] allowedValues)
    {
        var values = allowedValues
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => s.Trim());

        foreach (var value in values)
            _allowedValues.Add(value);
    }

    public override void Initialize(IArgumentParser parser)
    {
        base.Initialize(parser);

        if (string.IsNullOrWhiteSpace(Usage))
        {
            Usage = Name.Replace(" ", "-").ToLowerInvariant();
        }
    }

    public override void Parse(string[] arguments, IArgumentParser parser)
    {
        var options = ((BaseArgumentParser)parser).AppOptions.Options;

        var compareFlag = parser.ComparisonFlag();
        for (var index = 0; index < arguments.Length; ++index)
        {
            var argument = arguments[index];

            if (!argument.StartsWith(parser.CommandPrefix))
                _valueTokens.Add(argument);

            var scalarCommand = options.OfType<IScalarCommandOption>()
                .FirstOrDefault(o => o.Commands.Any(c => c.Equals(argument, compareFlag)));

            if (scalarCommand is null)
                continue;

            for (; index + 1 < arguments.Length; ++index)
            {
                var value = arguments[index + 1];
                if (value.StartsWith(parser.CommandPrefix))
                    break;

                ++index;

                if (!scalarCommand.AllowSequentialValues)
                    break;
            }
        }

        var parsedValues = parser.GetParsedValues(_valueTokens, EnableValueTokenSplitting);
        _parsedValues.AddRange(parsedValues);
    }

    public override void Validate(IArgumentParser parser)
    {
        if (_valueTokens.Count < RequiredTokenCount)
            throw new Exception($"Missing option value. Required Option Count: {RequiredTokenCount}");

        if (_valueTokens.Count > MaximumTokenCount)
            throw new Exception($"An option with too many values. Maximum Option Count: {MaximumTokenCount}");

        parser.ValidateByAllowedValues(_parsedValues, _allowedValues);

        _validated = true;
    }

    public override void Clear()
    {
        base.Clear();
        _valueTokens.Clear();
        _parsedValues.Clear();
    }
}
