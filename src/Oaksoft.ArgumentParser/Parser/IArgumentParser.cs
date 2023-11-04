using System;
using System.Collections.Generic;

namespace Oaksoft.ArgumentParser.Parser;

public interface IArgumentParser
{
    string OptionPrefix { get; }

    string ValueDelimiter { get; }

    string TokenDelimiter { get; }

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

