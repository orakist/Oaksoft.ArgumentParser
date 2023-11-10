using System;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Builder;

public interface IArgumentParserBuilder
{
    bool CaseSensitive { get; }

    OptionPrefixRules OptionPrefix { get; }

    AliasDelimiterRules AliasDelimiter { get; }

    ValueDelimiterRules ValueDelimiter { get; }
}

public interface IArgumentParserBuilder<out TOptions> : IArgumentParserBuilder
    where TOptions : IApplicationOptions
{
    IArgumentParserBuilder<TOptions> ConfigureSettings(Action<IParserSettingsBuilder> action);

    IArgumentParser<TOptions> Build();
}
