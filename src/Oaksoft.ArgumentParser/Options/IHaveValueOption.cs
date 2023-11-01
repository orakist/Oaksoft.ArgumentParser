using System.Collections.Generic;

namespace Oaksoft.ArgumentParser.Options;

public interface IHaveValueOption : IBaseOption
{
    List<string> ValueTokens { get; }

    bool EnableValueTokenSplitting { get; }

    string? DefaultValue { get; }

    List<string?> Constraints { get; }

    List<string> AllowedValues { get; }

    List<string> ParsedValues { get; }
}
