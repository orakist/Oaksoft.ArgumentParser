namespace Oaksoft.ArgumentParser.Parser;

internal class ParserSettings : IParserSettings
{
    public bool? AutoPrintHeader { get; set; }

    public bool? AutoPrintErrors { get; set; }

    public bool? AutoPrintHelp { get; set; }

    public int? HelpDisplayWidth { get; set; }

    public bool? NewLineAfterOption { get; set; }

    public bool? ShowTitle { get; set; }

    public bool? ShowDescription { get; set; }

    public bool? EnableColoring { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int? MaxAliasLength { get; set; }
}

internal class ParserSettingsBuilder : IParserSettingsBuilder
{
    public bool? AutoPrintHeader { get; set; }

    public bool? AutoPrintErrors { get; set; }

    public bool? AutoPrintHelp { get; set; }

    public int? HelpDisplayWidth { get; set; }

    public bool? NewLineAfterOption { get; set; }

    public bool? ShowTitle { get; set; }

    public bool? ShowDescription { get; set; }

    public bool? EnableColoring { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int? MaxAliasLength { get; set; }
}
