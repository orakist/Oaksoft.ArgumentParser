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

    /// <summary>
    /// Indicates that the argument parsing result is valid. 
    /// </summary>
    bool IsValid { get; }

    /// <summary>
    /// Indicates that the arguments array was empty. 
    /// </summary>
    bool IsEmpty { get; }

    /// <summary>
    /// Indicates that the parser found a valid help option usage. 
    /// </summary>
    bool IsHelpOption { get; }

    /// <summary>
    /// Indicates that the parser found a valid version option usage. 
    /// </summary>
    bool IsVersionOption { get; }

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

    void Run(Action<TOptions> callback, params string[] args);

    void Run(Action<IArgumentParser<TOptions>, TOptions> callback, params string[] args);

    void Run(string? title, Action<TOptions> callback, params string[] args);

    void Run(string? title, Action<IArgumentParser<TOptions>, TOptions> callback, params string[] args);
}