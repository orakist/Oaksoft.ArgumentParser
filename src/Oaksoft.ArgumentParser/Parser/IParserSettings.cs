using Oaksoft.ArgumentParser.Definitions;

namespace Oaksoft.ArgumentParser.Parser;

/// <summary>
/// Configuration settings of the parser.
/// </summary>
public interface IParserSettings
{
    /// <summary>
    /// If true, automatically prints application header to console. Default is true.
    /// </summary>
    bool AutoPrintHeader { get; }

    /// <summary>
    /// If true, automatically prints parsing errors to console. Default is true.
    /// </summary>
    bool AutoPrintErrors { get; }

    /// <summary>
    /// If true, automatically prints help text to console. Default is true.
    /// </summary>
    bool AutoPrintHelp { get; }

    /// <summary>
    /// If true, automatically prints version text to console. Default is true.
    /// </summary>
    bool AutoPrintVersion { get; }

    /// <summary>
    /// Represents help text width. Default is 80 chars.
    /// </summary>
    int HelpDisplayWidth { get; }

    /// <summary>
    /// If true, while printing help text, prints an empty new line after printing an option details. Default is true.
    /// </summary>
    bool NewLineAfterOption { get; }

    /// <summary>
    /// If true, prints description text of the application in the header and help text. Default is true.
    /// </summary>
    bool ShowDescription { get; }

    /// <summary>
    /// If true, prints colorized help text. Default is true.
    /// </summary>
    bool EnableColoring { get; }

    /// <summary>
    /// If null, parser generates title text by using application Version, Product, Company and Copyright metadata. Default is null.
    /// </summary>
    string? Title { get; }

    /// <summary>
    /// If null, parser generates description text by using application Description metadata. Default is null.
    /// </summary>
    string? Description { get; }

    /// <summary>
    /// Represents default verbosity level of the parser. Default is Minimal.
    /// </summary>
    VerbosityLevelType VerbosityLevel { get; }

    /// <summary>
    /// Represents maximum length of an option alias. Default is 32 chars.
    /// </summary>
    int MaxAliasLength { get; }

    /// <summary>
    /// Represents maximum word count of an option alias. Default is 4 words.
    /// </summary>
    int MaxSuggestedAliasWordCount { get; }
}
