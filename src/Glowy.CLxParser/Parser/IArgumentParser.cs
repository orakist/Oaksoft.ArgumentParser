using System;
using System.Collections.Generic;

namespace Glowy.CLxParser.Parser;

public interface IArgumentParser
{
    string CommandPrefix { get; }

    string ValueSeparator { get; }

    string TokenSeparator { get; }

    bool CaseSensitive { get; }

    bool IsValid { get; }

    List<string> Errors { get; }

    IParserSettings Settings { get; }

    string GetHeaderText();

    string GetHelpText(bool? enableColoring = default);

    string GetErrorText(bool? enableColoring = default);
}

public interface IArgumentParser<out TOptions> : IArgumentParser
    where TOptions : IApplicationOptions
{
    TOptions GetApplicationOptions();

    IArgumentParser<TOptions> ConfigureOptions(Action<TOptions> action);

    IArgumentParser<TOptions> ConfigureParser(Action<IParserSettings> action);

    IArgumentParser<TOptions> Build();

    TOptions Parse(string[] args);
}

