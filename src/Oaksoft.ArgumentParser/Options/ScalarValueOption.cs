using Oaksoft.ArgumentParser.Parser;
using System;
using System.Reflection;

namespace Oaksoft.ArgumentParser.Options;

internal class ScalarValueOption<TValue> : BaseScalarValueOption<TValue>
    where TValue : IComparable
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

        if (string.IsNullOrWhiteSpace(Description))
        {
            Description = $"Captures value for '{Name}' option.";
        }
    }

    public override void Parse(TokenItem[] tokens)
    {
        foreach (var token in tokens)
        {
            if (!token.IsOnlyValue)
            {
                continue;
            }

            if (!IsValidValue(token.Value!))
            {
                continue;
            }

            token.IsParsed = true;
            _valueTokens.Add(token.Value!);
            break;
        }

        _inputValues.AddRange(_valueTokens);
    }

    public override void ApplyOptionResult(object appOptions, PropertyInfo keyProperty)
    {
        if (!keyProperty.PropertyType.IsAssignableFrom(typeof(TValue)))
        {
            return;
        }

        var result = ResultValue != null ? ResultValue.Value : default;

        keyProperty.SetValue(appOptions, result);
    }

    public override void Validate()
    {
        base.Validate();

        IsValid = true;
    }
}
