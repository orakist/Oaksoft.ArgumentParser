using Oaksoft.ArgumentParser.Definitions;

namespace Oaksoft.ArgumentParser.Parser;

internal class ParserSettings : IParserSettings
{
    public bool AutoPrintHeader { get; init; }

    public bool AutoPrintErrors { get; init; }

    public bool AutoPrintHelp { get; init; }

    public bool AutoPrintVersion { get; init; }

    public int HelpDisplayWidth { get; init; }

    public bool NewLineAfterOption { get; init; }

    public bool ShowDescription { get; init; }

    public bool EnableColoring { get; init; }

    public string? Title { get; init; }

    public string? Description { get; init; }

    public VerbosityLevelType VerbosityLevel { get; init; }

    public int MaxAliasLength { get; init; }

    public int MaxSuggestedAliasWordCount { get; init; }
}
