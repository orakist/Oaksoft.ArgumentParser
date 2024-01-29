using Oaksoft.ArgumentParser.Definitions;

namespace Oaksoft.ArgumentParser.Parser;

public interface IParserSettings
{
    bool AutoPrintHeader { get; }

    bool AutoPrintErrors { get; }

    bool AutoPrintHelp { get; }

    bool AutoPrintVersion { get; }

    int HelpDisplayWidth { get; }

    bool NewLineAfterOption { get; }

    bool ShowDescription { get; }

    bool EnableColoring { get; }

    string? Title { get; }

    string? Description { get; }

    VerbosityLevelType VerbosityLevel { get; }

    int MaxAliasLength { get; }

    int MaxSuggestedAliasWordCount { get; }
}
