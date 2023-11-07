namespace Oaksoft.ArgumentParser.Parser;

public interface IParserSettings
{
    bool? AutoPrintHeader { get; }

    bool? AutoPrintErrors { get; }

    bool? AutoPrintHelp { get; }

    int? HelpDisplayWidth { get; }

    bool? NewLineAfterOption { get; }

    bool? ShowTitle { get; }

    bool? ShowDescription { get; }

    bool? EnableColoring { get; }

    string? Title { get; }

    string? Description { get; }

    int? MaxAliasLength { get; }
}

public interface IParserSettingsBuilder
{
    bool? AutoPrintHeader { get; set; }

    bool? AutoPrintErrors { get; set; }

    bool? AutoPrintHelp { get; set; }

    int? HelpDisplayWidth { get; set; }

    bool? NewLineAfterOption { get; set; }

    bool? ShowTitle { get; set; }

    bool? ShowDescription { get; set; }

    bool? EnableColoring { get; set; }

    string? Title { get; set; }

    string? Description { get; set; }

    int? MaxAliasLength { get; set; }
}
