namespace Oaksoft.ArgumentParser.Options;

/// <summary>
/// Base option interface
/// </summary>
public interface IBaseOption
{
    /// <summary>
    /// Name of the option
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Usage help text of the option
    /// </summary>
    string Usage { get; }

    /// <summary>
    /// Description help text of the option
    /// </summary>
    string? Description { get; }

    /// <summary>
    /// Indicates allowed option usage limits
    /// </summary>
    (int Min, int Max) OptionArity { get; }

    /// <summary>
    /// Indicates allowed option value limits
    /// </summary>
    (int Min, int Max) ValueArity { get; }

    /// <summary>
    /// Indicates parsed option alias count.
    /// </summary>
    int OptionCount { get; }

    /// <summary>
    /// Indicates parsed option value count.
    /// </summary>
    int ValueCount { get; }

    /// <summary>
    /// Indicates option state is valid and parsed. 
    /// </summary>
    bool IsParsed { get; }

    /// <summary>
    /// Indicates hidden option 
    /// </summary>
    bool IsHidden { get; }
}
