﻿namespace Oaksoft.ArgumentParser.Builder;

public interface IParserSettingsBuilder
{
    bool? AutoPrintHeader { get; set; }

    bool? AutoPrintErrors { get; set; }

    bool? AutoPrintHelp { get; set; }

    bool? AutoPrintVersion { get; set; }

    int? HelpDisplayWidth { get; set; }

    bool? NewLineAfterOption { get; set; }

    bool? ShowTitle { get; set; }

    bool? ShowDescription { get; set; }

    bool? EnableColoring { get; set; }

    string? Title { get; set; }

    string? Description { get; set; }

    int? MaxAliasLength { get; set; }

    int? MaxSuggestedAliasWordCount { get; set; }
}
