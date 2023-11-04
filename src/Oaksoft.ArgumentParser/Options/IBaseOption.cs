using System.Collections.Generic;

namespace Oaksoft.ArgumentParser.Options;

public interface IBaseOption
{
    string Name { get; }

    string Usage { get; }

    string? Description { get; }

    (int Min, int Max) OptionArity { get; }

    (int Min, int Max) ValueArity { get; }

    bool IsValid { get; }

    int OptionCount { get; }

    int ValueCount { get; }
}

public interface IAliasedOption : IBaseOption
{
    string ShortAlias { get; }

    List<string> Aliases { get; }

    List<string> OptionTokens { get; }
}
