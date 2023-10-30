using System;

namespace Glowy.CLxParser.Parser;

public interface IArgumentParser
{
    string CommandPrefix { get; }

    string ValueSeparator { get; }

    string TokenSeparator { get; }

    bool CaseSensitive { get; }

    bool IsValid { get; }

    IParserSettings Settings { get; }

    void Parse(string[] args);

    string GetHeaderText();

    string GetHelpText();

    string GetErrorText();
}

public interface IArgumentParser<out TOptions> : IArgumentParser
    where TOptions : IApplicationOptions
{
    TOptions GetApplicationOptions();

    IArgumentParser<TOptions> ConfigureOptions(Action<TOptions> action);

    IArgumentParser<TOptions> ConfigureParser(Action<IParserSettings> action);

    IArgumentParser<TOptions> Build();
}

