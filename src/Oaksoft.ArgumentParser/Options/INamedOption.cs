using System;
using System.Collections.Generic;

namespace Oaksoft.ArgumentParser.Options;

/// <summary>
/// Represents Named Option Interface
/// </summary>
public interface INamedOption : IBaseOption
{
    /// <summary>
    /// Represents shortest alias of the option.
    /// </summary>
    string Alias { get; }

    /// <summary>
    /// Represents all aliases of the option.
    /// </summary>
    List<string> Aliases { get; }

    /// <summary>
    /// Represents parsed option alias token from command line arguments.
    /// </summary>
    List<string> OptionTokens { get; }
}

/// <summary>
/// Represents Scalar (single value) Named Option Interface
/// </summary>
public interface IScalarNamedOption : INamedOption, IScalarValueOption
{
}

/// <summary>
/// Represents Sequential (multiple value) Named Option Interface.<br/>
/// Sequential named option requires one or more argument values. Option name may be repeated more than one time.
/// A sequential named option grabs all values.
/// </summary>
public interface ISequentialNamedOption : INamedOption, ISequentialValueOption
{
    /// <summary>
    /// Sets EnableSequentialValues property of sequential named options. It is true by default.<br/>
    /// If it is set to true, 'one two three' inputs will be parsed as 3 string values: "my-app --items one two three"<br/>
    /// If it is set to false, parser grabs only first input "one" as a value.
    /// </summary>
    bool EnableSequentialValues { get; }
}

/// <summary>
/// Counter option counts occurrences of the option in the command-line.
/// </summary>
public interface ICounterOption : INamedOption, IValueOption
{
}

/// <inheritdoc cref="IScalarNamedOption" />
/// <typeparam name="TValue">Type of the value</typeparam>
public interface IScalarNamedOption<TValue> : IScalarNamedOption, IScalarValueOption<TValue>, IHaveDefaultValue<TValue>
    where TValue : IComparable
{
}

/// <inheritdoc cref="ISequentialNamedOption" />
/// <typeparam name="TValue">Type of the value</typeparam>
public interface ISequentialNamedOption<TValue> : ISequentialNamedOption, ISequentialValueOption<TValue>
    where TValue : IComparable
{
}
