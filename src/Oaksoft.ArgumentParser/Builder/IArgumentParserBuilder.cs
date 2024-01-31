using System;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Builder;

/// <summary>
/// Represents the builder of the IArgumentParser
/// </summary>
public interface IArgumentParserBuilder
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
}

/// <inheritdoc cref="IArgumentParserBuilder"/>
/// <typeparam name="TOptions">Type of the 'Application Options' class</typeparam>
public interface IArgumentParserBuilder<out TOptions> : IArgumentParserBuilder
{
    /// <summary>
    /// Configures parser by various configuration settings.
    /// </summary>
    IArgumentParserBuilder<TOptions> ConfigureSettings(Action<IParserSettingsBuilder> action);

    /// <summary>
    /// Builds parser automatically registering all unregistered properties of the 'TOptions'.
    /// </summary>
    IArgumentParser<TOptions> AutoBuild();

    /// <summary>
    /// Builds parser by using registered properties of the 'TOptions'.
    /// </summary>
    IArgumentParser<TOptions> Build();
}
