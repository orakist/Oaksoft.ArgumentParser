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

    public override void Parse(TokenItem[] tokens)
    {
        foreach (var token in tokens)
        {
            if (!token.IsOnlyValue)
                continue;

            var value = SplitByValueDelimiter(token.Value!).First();
            if (!IsValidValue(value)) 
                continue;

            token.IsParsed = true;
            _valueTokens.Add(token.Value!);
        }

        // parse multiple values 'str1;str2;str3'
        var inputValues = _valueTokens.SelectMany(SplitByValueDelimiter);
        _inputValues.AddRange(inputValues);
    }

    public override void Validate()
    {
        base.Validate();

        IsValid = true;
    }
}
