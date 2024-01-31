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
    /// Globally disables console output. Default is false.
    /// </summary>
    public static bool DisableConsoleOutput { get; set; }

    /// <summary>
    /// Builds an IArgumentParser parser, automatically registering all properties of the 'TOptions'.
    /// </summary>
    public static IArgumentParser<TOptions> AutoBuild<TOptions>(
        OptionPrefixRules optionPrefix = OptionPrefixRules.Default,
        AliasDelimiterRules aliasDelimiter = AliasDelimiterRules.Default,
        ValueDelimiterRules valueDelimiter = ValueDelimiterRules.Default,
        bool caseSensitive = false)
        where TOptions : new()
    {
        var builder = CreateParser<TOptions>(
            optionPrefix, aliasDelimiter, valueDelimiter, caseSensitive);

        return builder.AutoBuild();
    }

    /// <summary>
    /// Creates an IArgumentParserBuilder to configure an IArgumentParser and register properties of the 'TOptions'.
    /// </summary>
    public static IArgumentParserBuilder<TOptions> CreateParser<TOptions>(
        OptionPrefixRules optionPrefix = OptionPrefixRules.Default, 
        AliasDelimiterRules aliasDelimiter = AliasDelimiterRules.Default, 
        ValueDelimiterRules valueDelimiter = ValueDelimiterRules.Default, 
        bool caseSensitive = false)
        where TOptions : new()
    {
        var applicationOptions = new TOptions();

        return new ArgumentParserBuilder<TOptions>(
            applicationOptions, caseSensitive, optionPrefix, 
            aliasDelimiter, valueDelimiter);
    }
}
