using Oaksoft.ArgumentParser.Definitions;
using System.Collections.Generic;

namespace Oaksoft.ArgumentParser.Parser;

public interface IArgumentParser
{
    bool CaseSensitive { get; }

    OptionPrefixRules OptionPrefix { get; }

    TokenDelimiterRules TokenDelimiter { get; }

    ValueDelimiterRules ValueDelimiter { get; }

    IParserSettings Settings { get; }

    bool IsValid { get; }

    List<string> Errors { get; }
    
    string GetHeaderText();

    string GetHelpText(bool? enableColoring = default);

    string GetErrorText(bool? enableColoring = default);
}

public interface IArgumentParser<out TOptions> : IArgumentParser
    where TOptions : IApplicationOptions
{
    TOptions GetApplicationOptions();

    TOptions Parse(string[] args);
}