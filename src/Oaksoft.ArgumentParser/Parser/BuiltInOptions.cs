using Oaksoft.ArgumentParser.Definitions;

namespace Oaksoft.ArgumentParser.Parser;

internal class BuiltInOptions : IBuiltInOptions
{
    public bool? Help { get; set; }

    public bool? Version { get; set; }

    public VerbosityLevelType? Verbosity { get; set; }
}
