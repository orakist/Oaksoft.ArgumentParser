using System;
using Oaksoft.ArgumentParser.Definitions;
using System.Collections.Generic;
using Oaksoft.ArgumentParser.Errors;
using Oaksoft.ArgumentParser.Options;

namespace Oaksoft.ArgumentParser.Parser;

public interface IArgumentParser
{
    bool CaseSensitive { get; }

    OptionPrefixRules OptionPrefix { get; }

    AliasDelimiterRules AliasDelimiter { get; }

    ValueDelimiterRules ValueDelimiter { get; }

    IParserSettings Settings { get; }

    bool IsValid { get; }

    List<IErrorMessage> Errors { get; }
    
    string GetHeaderText();

    string GetHelpText(bool? enableColoring = default);

    string GetErrorText(bool? enableColoring = default);

    List<IBaseOption> GetOptions();

    IBaseOption? GetOptionByName(string name);

    INamedOption? GetOptionByAlias(string alias);
}

public interface IArgumentParser<out TOptions> : IArgumentParser
{
    IBuiltInOptions GetBuiltInOptions();

    TOptions GetApplicationOptions();

    TOptions Parse(string[] args);

    void Run(Action<TOptions> callback);

    void Run(Action<IArgumentParser<TOptions>, TOptions> callback);

    void Run(string[] args, Action<TOptions> callback);

    void Run(string[] args, Action<IArgumentParser<TOptions>, TOptions> callback);
}