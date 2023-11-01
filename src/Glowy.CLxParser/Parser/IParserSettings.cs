namespace Oaksoft.ArgumentParser.Parser;

public interface IParserSettings
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
}
