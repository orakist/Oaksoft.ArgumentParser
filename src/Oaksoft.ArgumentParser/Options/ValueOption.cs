using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Parser;
using System.Linq;

namespace Oaksoft.ArgumentParser.Options;

internal sealed class ValueOption : BaseValueOption<string>
{
    public override int OptionCount => 0;

    public ValueOption(int requiredValueCount, int maximumValueCount)
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

    public override void Parse(string[] arguments, IArgumentParser parser)
    {
        var options = ((BaseArgumentParser)parser).AppOptions.Options;

        var compareFlag = parser.ComparisonFlag();
        for (var index = 0; index < arguments.Length; ++index)
        {
            var argument = arguments[index];

            if (!argument.StartsWith(parser.OptionPrefix))
                _valueTokens.Add(argument);

            var scalarOption = options.OfType<IScalarOption>()
                .FirstOrDefault(o => o.Aliases.Any(c => c.Equals(argument, compareFlag)));

            if (scalarOption is null)
                continue;

            for (; index + 1 < arguments.Length; ++index)
            {
                var value = arguments[index + 1];
                if (value.StartsWith(parser.OptionPrefix))
                    break;

                ++index;

                if (!scalarOption.AllowSequentialValues)
                    break;
            }
        }

        var inputValues = parser.GetInputValues(_valueTokens, EnableValueTokenSplitting);
        _inputValues.AddRange(inputValues);
    }

    public override void Validate(IArgumentParser parser)
    {
        base.Validate(parser);

        IsValid = true;
    }
}
