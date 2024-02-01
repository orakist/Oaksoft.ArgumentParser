using Oaksoft.ArgumentParser.Definitions;

namespace Oaksoft.ArgumentParser.Builder;

internal class ParserSettingsBuilder : IParserSettingsBuilder
{
    public bool? AutoPrintHeader { get; set; }

    public bool? AutoPrintErrors { get; set; }

    public bool? AutoPrintHelp { get; set; }

    public bool? AutoPrintVersion { get; set; }

    public bool? AutoPrintArguments { get; set; }

    public int? HelpDisplayWidth { get; set; }

    public bool? NewLineAfterOption { get; set; }

    public bool? ShowCopyright { get; set; }

    public bool? ShowDescription { get; set; }

    public bool? EnableColoring { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public VerbosityLevelType? VerbosityLevel { get; set; }

    public int? MaxAliasLength { get; set; }

    public int? MaxSuggestedAliasWordCount { get; set; }
}
