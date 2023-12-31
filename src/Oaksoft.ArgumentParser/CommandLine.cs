﻿using Oaksoft.ArgumentParser.Builder;
using Oaksoft.ArgumentParser.Definitions;

namespace Oaksoft.ArgumentParser;

public static class CommandLine
{
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
