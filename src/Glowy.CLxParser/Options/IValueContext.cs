using System.Collections.Generic;

namespace Oaksoft.ArgumentParser.Options;

public interface IValueContext 
{
    string? DefaultValue { get; }

    List<string?> Constraints { get; }

    List<string> AllowedValues { get; }

    List<string> ParsedValues { get; }
}
