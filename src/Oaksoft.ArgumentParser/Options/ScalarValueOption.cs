using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Parser;
using System;

namespace Oaksoft.ArgumentParser.Options;

internal sealed class ScalarValueOption<TValue> : BaseScalarValueOption<TValue>
    where TValue : IComparable, IEquatable<TValue>
{
    public override int OptionCount => 0;

    public ScalarValueOption(int requiredValueCount, int maximumValueCount)
        : base(requiredValueCount, maximumValueCount)
    {
        OptionArity = (0, 0);
    }

    public override void Initialize(IArgumentParser parser)
    {
        base.Initialize(parser);

        if (string.IsNullOrWhiteSpace(Usage))
        {
            Usage = Name.Replace(" ", "-").ToLowerInvariant();
        }
    }

    public override void Parse(TokenValue[] tokens, IArgumentParser parser)
    {
        foreach (var token in tokens)
        {
            if (token.Invalid || token.IsParsed)
                continue;

            var argument = token.Argument;
            if (argument.IsAliasCandidate(parser.OptionPrefix))
                continue;

            if (!IsValidValue(argument))
                continue;

            token.IsParsed = true;
            _valueTokens.Add(argument);
            break;
        }

        _inputValues.AddRange(_valueTokens);
    }

    public override void Validate(IArgumentParser parser)
    {
        base.Validate(parser);

        IsValid = true;
    }
}
