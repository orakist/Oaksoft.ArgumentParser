using System;
using Oaksoft.ArgumentParser.Definitions;
using System.Collections.Generic;
using System.IO;
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
    /// Indicates that at least one valid user option parsed. 
    /// </summary>
    bool IsParsed { get; }

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

    /// <summary>
    /// Indicates that the parser found a valid verbosity option usage. 
    /// </summary>
    VerbosityLevelType VerbosityLevel { get; }

    List<IErrorMessage> Errors { get; }
    
    string GetHeaderText();

    string GetHelpText(bool? enableColoring = default);

    string GetErrorText(bool? enableColoring = default);

    string GetVersionText();

    List<IBaseOption> GetOptions();

    IBaseOption? GetOption(string nameOrAlias);

    IBaseOption? GetOptionByName(string name);

    INamedOption? GetOptionByAlias(string alias);

    IBuiltInOptions GetBuiltInOptions();

    bool ContainsOption(string nameOrAlias);

    bool IsOptionParsed(string nameOrAlias);

    void SetTextReader(TextReader reader);
}

public interface IArgumentParser<out TOptions> : IArgumentParser
{
    TOptions GetApplicationOptions();

    TOptions Parse(params string[] args);

    void Run(Action<TOptions> callback, params string[] args);

    void Run(Action<IArgumentParser<TOptions>, TOptions> callback, params string[] args);

    void Run(string? comment, Action<TOptions> callback, params string[] args);

    void Run(string? comment, Action<IArgumentParser<TOptions>, TOptions> callback, params string[] args);
}