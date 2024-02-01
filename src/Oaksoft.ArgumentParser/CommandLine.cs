using System.IO;
using Oaksoft.ArgumentParser.Builder;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser;

/// <summary>
/// Entry point to create an argument parser. 
/// </summary>
public static class CommandLine
{
    /// <summary>
    /// Globally disables text writer. Default is false.
    /// </summary>
    public static bool DisableTextWriter { get; set; }

    /// <summary>
    /// Builds an IArgumentParser parser, automatically registering all properties of the 'TOptions'.
    /// </summary>
    /// <typeparam name="TOptions">Type of the Application Options</typeparam>
    /// <param name="optionPrefix">Sets alias prefix rule of  the parser.</param>
    /// <param name="aliasDelimiter">Sets alias delimiter rule of the parser.</param>
    /// <param name="valueDelimiter">Sets value delimiter rule of the parser.</param>
    /// <param name="caseSensitive">Sets case sensitivity of the parser.</param>
    /// <param name="textReader">Sets text reader of the parser. If it is null, parser uses System.Console.In as a text reader.</param>
    /// <param name="textWriter">Sets text writer of the parser. If it is null, parser uses System.Console.Out as a text writer.</param>
    public static IArgumentParser<TOptions> AutoBuild<TOptions>(
        OptionPrefixRules optionPrefix = OptionPrefixRules.Default,
        AliasDelimiterRules aliasDelimiter = AliasDelimiterRules.Default,
        ValueDelimiterRules valueDelimiter = ValueDelimiterRules.Default,
        bool caseSensitive = false,
        TextReader? textReader = null,
        TextWriter? textWriter = null)
        where TOptions : new()
    {
        var builder = CreateParser<TOptions>(
            optionPrefix, aliasDelimiter, valueDelimiter, 
            caseSensitive, textReader, textWriter);

        return builder.AutoBuild();
    }


    /// <summary>
    /// Creates an IArgumentParserBuilder to configure an IArgumentParser and register properties of the 'TOptions'.
    /// </summary>
    /// <typeparam name="TOptions">Type of the Application Options</typeparam>
    /// <param name="optionPrefix">Sets alias prefix rule of  the parser.</param>
    /// <param name="aliasDelimiter">Sets alias delimiter rule of the parser.</param>
    /// <param name="valueDelimiter">Sets value delimiter rule of the parser.</param>
    /// <param name="caseSensitive">Sets case sensitivity of the parser.</param>
    /// <param name="textReader">Sets text reader of the parser. If it is null, parser uses System.Console.In as a text reader.</param>
    /// <param name="textWriter">Sets text writer of the parser. If it is null, parser uses System.Console.Out as a text writer.</param>
    public static IArgumentParserBuilder<TOptions> CreateParser<TOptions>(
        OptionPrefixRules optionPrefix = OptionPrefixRules.Default,
        AliasDelimiterRules aliasDelimiter = AliasDelimiterRules.Default,
        ValueDelimiterRules valueDelimiter = ValueDelimiterRules.Default,
        bool caseSensitive = false, 
        TextReader? textReader = null, 
        TextWriter? textWriter = null)
        where TOptions : new()
    {
        return new ArgumentParserBuilder<TOptions>(
            caseSensitive, optionPrefix, aliasDelimiter, 
            valueDelimiter, textReader, textWriter);
    }
}
