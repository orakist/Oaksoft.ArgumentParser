using Oaksoft.ArgumentParser.Definitions;

namespace Oaksoft.ArgumentParser.Builder;

/// <summary>
/// Configuration settings of the parser builder.
/// </summary>
public interface IParserSettingsBuilder
{
    /// <summary>
    /// If true, automatically prints application header to console. Default is true.
    /// </summary>
    bool? AutoPrintHeader { get; set; }

    /// <summary>
    /// If true, automatically prints parsing errors to the text writer. Default is true.
    /// </summary>
    bool? AutoPrintErrors { get; set; }

    /// <summary>
    /// If true, automatically prints help text to the text writer. Default is true.
    /// </summary>
    bool? AutoPrintHelp { get; set; }

    /// <summary>
    /// If true, automatically prints version text to the text writer. Default is true.
    /// </summary>
    bool? AutoPrintVersion { get; set; }

    /// <summary>
    /// If true, automatically prints given arguments to the text writer. Default is false.
    /// </summary>
    bool? AutoPrintArguments { get; set; }

    /// <summary>
    /// Represents help text width. Default is 90 chars.
    /// </summary>
    int? HelpDisplayWidth { get; set; }

    /// <summary>
    /// If true, while printing help text, prints an empty new line after printing an option details. Default is true.
    /// </summary>
    bool? NewLineAfterOption { get; set; }

    /// <summary>
    /// If true, prints copyright information in the header text. Default is false.
    /// </summary>
    bool? ShowCopyright { get; set; }

    /// <summary>
    /// If true, prints description text of the application in the header and help text. Default is true.
    /// </summary>
    bool? ShowDescription { get; set; }

    /// <summary>
    /// If true, prints colorized help text. Default is true.
    /// </summary>
    bool? EnableColoring { get; set; }

    /// <summary>
    /// If null, parser generates title text by using application Version, Product, Company and Copyright metadata. Default is null.
    /// </summary>
    string? Title { get; set; }

    /// <summary>
    /// If null, parser generates description text by using application Description metadata. Default is null.
    /// </summary>
    string? Description { get; set; }

    /// <summary>
    /// Represents default verbosity level of the parser. Default is Minimal.
    /// </summary>
    VerbosityLevelType? VerbosityLevel { get; set; }

    /// <summary>
    /// Represents maximum length of an option alias. Default is 32 chars.
    /// </summary>
    int? MaxAliasLength { get; set; }

    /// <summary>
    /// Represents maximum word count of an option alias. Default is 4 words.
    /// </summary>
    int? MaxSuggestedAliasWordCount { get; set; }
}
