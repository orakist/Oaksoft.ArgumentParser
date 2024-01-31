using Oaksoft.ArgumentParser.Definitions;

namespace Oaksoft.ArgumentParser.Parser;

/// <summary>
/// Built-in option properties.
/// </summary>
public interface IBuiltInOptions
{
    /// <summary>
    /// Indicates that the parser found a valid help option usage. 
    /// </summary>
    bool? Help { get; }

    /// <summary>
    /// Indicates that the parser found a valid version option usage. 
    /// </summary>
    bool? Version { get; }

    /// <summary>
    /// Indicates the verbosity level type. 
    /// </summary>
    VerbosityLevelType? Verbosity { get; }
}
