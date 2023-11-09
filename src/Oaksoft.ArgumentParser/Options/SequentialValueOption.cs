using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Parser;
using System;
using System.Linq;

namespace Oaksoft.ArgumentParser.Options;

internal sealed class SequentialValueOption<TValue> : BaseSequentialValueOption<TValue>
    where TValue : IComparable, IEquatable<TValue>
{
    public override int OptionCount => 0;

    public SequentialValueOption(int requiredValueCount, int maximumValueCount)
        : base(requiredValueCount, maximumValueCount)
    {
        OptionArity = (0, 0);
    }

    public override void Initialize()
    {
        base.Initialize();

        if (string.IsNullOrWhiteSpace(Usage))
        {
            Usage = Name.Replace(" ", "-").ToLowerInvariant();
        }
    }

    public override void Parse(TokenValue[] tokens)
    {
        foreach (var token in tokens)
        {
            if (token.Invalid || token.IsParsed)
                continue;

            var argument = token.Argument;
            if (argument.IsAliasCandidate(_parser!.OptionPrefix))
                continue;

            var value = argument.GetInputValues(_parser.ValueDelimiter, EnableValueTokenSplitting).First();
            if (!IsValidValue(value)) 
                continue;

            token.IsParsed = true;
            _valueTokens.Add(argument);
        }

        var inputValues = _valueTokens.GetInputValues(_parser!.ValueDelimiter, EnableValueTokenSplitting);
        _inputValues.AddRange(inputValues);
    }

    public override void Validate()
    {
        base.Validate();

        IsValid = true;
    }
}
