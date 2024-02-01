using System;
using Oaksoft.ArgumentParser.Definitions;
using System.Collections.Generic;
using System.IO;
using Oaksoft.ArgumentParser.Errors;
using Oaksoft.ArgumentParser.Options;

namespace Oaksoft.ArgumentParser.Parser;

/// <summary>
/// Represents a configured argument parser.
/// </summary>
public interface IArgumentParser
{
    /// <summary>
    /// Represents Case Sensitivity of the parser. By default, parser is case-insensitive. 
    /// </summary>
    bool CaseSensitive { get; }

    /// <summary>
    /// Represents option prefix rules of the parser. This rule lets to configure a prefix of aliases.
    /// </summary>
    OptionPrefixRules OptionPrefix { get; }

    /// <summary>
    /// Represents alias delimiter rules of the parser. This rule lets to configure a space, '=', or ':' as the delimiter between an option name and its argument.
    /// </summary>
    AliasDelimiterRules AliasDelimiter { get; }

    /// <summary>
    /// Represents value delimiter rules of the parser. This rule lets to configure a ',', ';', or '|' as the delimiter between option values.
    /// </summary>
    ValueDelimiterRules ValueDelimiter { get; }

    /// <summary>
    /// Configuration settings of the parser.
    /// </summary>
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

    /// <summary>
    /// Represents parsing errors.
    /// </summary>
    List<IErrorMessage> Errors { get; }
    
    /// <summary>
    /// Gets application header text.
    /// </summary>
    string GetHeaderText();

    /// <summary>
    /// Gets application help text.
    /// </summary>
    string GetHelpText(bool? enableColoring = default);

    /// <summary>
    /// Gets application error text.
    /// </summary>
    string GetErrorText(bool? enableColoring = default);

    /// <summary>
    /// Gets application version text.
    /// </summary>
    string GetVersionText();

    /// <summary>
    /// Gets all registered options.
    /// </summary>
    List<IBaseOption> GetOptions();

    /// <summary>
    /// Gets an option by name or alias.
    /// </summary>
    IBaseOption? GetOption(string nameOrAlias);

    /// <summary>
    /// Gets an option by name.
    /// </summary>
    IBaseOption? GetOptionByName(string name);

    /// <summary>
    /// Gets an option by alias.
    /// </summary>
    INamedOption? GetOptionByAlias(string alias);

    /// <summary>
    /// Gets built-in options.
    /// </summary>
    IBuiltInOptions GetBuiltInOptions();

    /// <summary>
    /// Checks existence of an option by name or alias.
    /// </summary>
    bool ContainsOption(string nameOrAlias);

    /// <summary>
    /// Checks parsed state of an option by name or alias.
    /// </summary>
    bool IsOptionParsed(string nameOrAlias);

    /// <summary>
    /// Sets TextReader of the parser. Default is 'System.Console.In'.
    /// </summary>
    void SetTextReader(TextReader reader);
}

/// <inheritdoc cref="IArgumentParser"/>
/// <typeparam name="TOptions">Type of the 'Application Options' class</typeparam>
public interface IArgumentParser<out TOptions> : IArgumentParser
{
    /// <summary>
    /// Gets instance of the 'TOptions' with parsed values.
    /// </summary>
    TOptions GetApplicationOptions();

    /// <summary>
    /// Tries to parse given arguments.
    /// </summary>
    TOptions Parse(params string[] args);

    /// <summary>
    /// Runs a parser loop to parse given 'args' or console inputs. At each step of the loop,
    /// after arguments are parsed successfully, invokes the given callback function by passing parsed option values. 
    /// </summary>
    /// <param name="callback">Parser invokes this callback after each successful parsing.</param>
    /// <param name="args">Arguments to parse</param>
    void Run(Action<TOptions> callback, params string[] args);

    /// <summary>
    /// Runs a parser loop to parse given 'args' or console inputs. At each step of the loop,
    /// after arguments are parsed, it invokes the given callback delegate by passing parsed option values. 
    /// </summary>
    /// <param name="callback">Parser invokes this callback after each parsing.</param>
    /// <param name="args">Arguments to parse</param>
    void Run(Action<IArgumentParser, TOptions> callback, params string[] args);

    /// <summary>
    /// Runs a parser loop to parse given 'args' or console inputs. At each step of the loop,
    /// after arguments are parsed successfully, invokes the given callback function by passing parsed option values. 
    /// </summary>
    /// <param name="comment">Prompted text</param>
    /// <param name="callback">Parser invokes this callback after each successful parsing.</param>
    /// <param name="args">Arguments to parse</param>
    void Run(string? comment, Action<TOptions> callback, params string[] args);

    /// <summary>
    /// Runs a parser loop to parse given 'args' or console inputs. At each step of the loop,
    /// after arguments are parsed, it invokes the given callback delegate by passing parsed option values. 
    /// </summary>
    /// <param name="comment">Prompted text</param>
    /// <param name="callback">Parser invokes this callback after each parsing.</param>
    /// <param name="args">Arguments to parse</param>
    void Run(string? comment, Action<IArgumentParser, TOptions> callback, params string[] args);

    /// <summary>
    /// Runs parser only once to parse given 'args' or console inputs. After arguments are parsed successfully,
    /// invokes the given callback function by passing parsed option values. 
    /// </summary>
    /// <param name="callback">Parser invokes this callback after each successful parsing.</param>
    /// <param name="args">Arguments to parse</param>
    void RunOnce(Action<TOptions> callback, params string[] args);

    /// <summary>
    /// Runs parser only once to parse given 'args' or console inputs. After arguments are parsed,
    /// it invokes the given callback delegate by passing parsed option values. 
    /// </summary>
    /// <param name="callback">Parser invokes this callback after each parsing.</param>
    /// <param name="args">Arguments to parse</param>
    void RunOnce(Action<IArgumentParser, TOptions> callback, params string[] args);

    /// <summary>
    /// Runs parser only once to parse given 'args' or console inputs. After arguments are parsed successfully,
    /// invokes the given callback function by passing parsed option values. 
    /// </summary>
    /// <param name="comment">Prompted text</param>
    /// <param name="callback">Parser invokes this callback after each successful parsing.</param>
    /// <param name="args">Arguments to parse</param>
    void RunOnce(string? comment, Action<TOptions> callback, params string[] args);

    /// <summary>
    /// Runs parser only once to parse given 'args' or console inputs. After arguments are parsed,
    /// it invokes the given callback delegate by passing parsed option values.
    /// </summary>
    /// <param name="comment">Prompted text</param>
    /// <param name="callback">Parser invokes this callback after each parsing.</param>
    /// <param name="args">Arguments to parse</param>
    void RunOnce(string? comment, Action<IArgumentParser, TOptions> callback, params string[] args);
}
