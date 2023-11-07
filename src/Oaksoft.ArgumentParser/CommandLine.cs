using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser;

public static class CommandLine
{
    public static IArgumentParser<TOptions> CreateParser<TOptions>(
        OptionPrefixRules optionPrefix = OptionPrefixRules.Default, 
        TokenDelimiterRules tokenDelimiter = TokenDelimiterRules.Default, 
        ValueDelimiterRules valueDelimiter = ValueDelimiterRules.Default, 
        bool caseSensitive = false)
        where TOptions : BaseApplicationOptions, new()
    {
        var applicationOptions = new TOptions();
        return new ArgumentParser<TOptions>(
            applicationOptions, optionPrefix, tokenDelimiter,
            valueDelimiter, caseSensitive);
    }
}

