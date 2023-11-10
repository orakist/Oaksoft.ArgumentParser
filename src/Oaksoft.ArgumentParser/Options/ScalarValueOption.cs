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

    public override void Initialize()
    {
        base.Initialize();

        if (string.IsNullOrWhiteSpace(Usage))
        {
            Usage = Name.Replace(" ", "-").ToLowerInvariant();
        }
    }

    public override void Parse(TokenItem[] tokens)
    {
        foreach (var token in tokens)
        {
            if (!token.IsOnlyValue)
                continue;

            if (!IsValidValue(token.Value!))
                continue;

            token.IsParsed = true;
            _valueTokens.Add(token.Value!);
            break;
        }

        _inputValues.AddRange(_valueTokens);
    }

    public override void Validate()
    {
        base.Validate();

        IsValid = true;
    }
}
