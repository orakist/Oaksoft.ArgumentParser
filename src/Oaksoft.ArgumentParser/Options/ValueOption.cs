using Oaksoft.ArgumentParser.Parser;

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

    public override void Validate(IArgumentParser parser)
    {
        base.Validate(parser);

        IsValid = true;
    }
}
