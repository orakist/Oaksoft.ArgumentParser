using Oaksoft.ArgumentParser.Definitions;
using System;

namespace Oaksoft.ArgumentParser.Parser;

public interface IArgumentParserBuilder
{
    bool CaseSensitive { get; }

    OptionPrefixRules OptionPrefix { get; }

    TokenDelimiterRules TokenDelimiter { get; }

    ValueDelimiterRules ValueDelimiter { get; }
}

public interface IArgumentParserBuilder<out TOptions> : IArgumentParserBuilder
    where TOptions : IApplicationOptions
{
    IArgumentParserBuilder<TOptions> ConfigureSettings(Action<IParserSettingsBuilder> action);

    IArgumentParserBuilder<TOptions> ConfigureOptions(Action<IArgumentParserBuilder<TOptions>> action);

    IArgumentParser<TOptions> Build();
}
