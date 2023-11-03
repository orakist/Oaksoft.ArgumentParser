using System.Collections.Generic;

namespace Oaksoft.ArgumentParser.Options;

public interface IAliasedOption : IBaseOption
{
    string ShortAlias { get; }

    List<string> Aliases { get; }

    List<string> CommandTokens { get; }
}
