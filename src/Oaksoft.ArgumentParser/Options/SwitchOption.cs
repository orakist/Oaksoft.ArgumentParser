using System.Linq;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Options;

internal sealed class SwitchOption : CommandOption
{
    public override int ValidInputCount => _validated ? _commandTokens.Count : 0;

    public SwitchOption(int requiredTokenCount = 0, int maximumTokenCount = 1)
        : base(requiredTokenCount, maximumTokenCount)
    {
    }

    public override void Initialize(IArgumentParser parser)
    {
        base.Initialize(parser);

        if (string.IsNullOrWhiteSpace(Usage))
            Usage = Command;
    }

    public override void Parse(string[] arguments, IArgumentParser parser)
    {
        foreach (var argument in arguments)
        {
            if (!_commands.Any(c => c.Equals(argument, parser.ComparisonFlag())))
                continue;

            _commandTokens.Add(argument);
        }
    }

    public override void Validate(IArgumentParser parser)
    {
        base.Validate(parser);

        _validated = true;
    }
}
