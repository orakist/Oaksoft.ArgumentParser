using System;
using System.Collections.Generic;

namespace Oaksoft.ArgumentParser.Options;

/// <summary>
/// Represents option values / arguments. They don't begin with option prefixes.
/// </summary>
public interface IValueOption : IHaveValueOption, IBaseOption
{
}

/// <summary>
/// Represents Scalar (single value) Value Option Interface.
/// </summary>
public interface IScalarValueOption : IValueOption
{
}

/// <summary>
/// Represents Sequential (multiple value) Value Option Interface.
/// </summary>
public interface ISequentialValueOption : IValueOption
{
    /// <summary>
    /// Sets EnableValueTokenSplitting property of sequential named and value options. It is true by default.<br/>
    /// If it is set to true, 'one;two;three' inputs will be parsed as 3 string values: "my-app --items one;two;three"<br/>
    /// If it is set to false, inputs will be parsed as 1 string "one;two;three" value.
    /// </summary>
    bool EnableValueTokenSplitting { get; }
}

/// <inheritdoc cref="IScalarValueOption"/>
/// <typeparam name="TValue">Type of the value</typeparam>
public interface IScalarValueOption<TValue>
    : IScalarValueOption, IHaveResultValue<TValue>, IHaveAllowedValues<TValue>
    where TValue : IComparable
{
}

/// <inheritdoc cref="ISequentialValueOption"/>
/// <typeparam name="TValue">Type of the value</typeparam>
public interface ISequentialValueOption<TValue>
    : ISequentialValueOption, IHaveResultValues<TValue>, IHaveAllowedValues<TValue>
    where TValue : IComparable
{
}

/// <summary>
/// Represents values of an option.
/// </summary>
public interface IHaveValueOption
{
    /// <summary>
    /// Represents value tokens of an option from given command-line arguments.
    /// </summary>
    List<string> ValueTokens { get; }

    /// <summary>
    /// Represents parsed values of an option according to the ValueDelimiterRules of the parser.
    /// </summary>
    List<string> InputValues { get; }
}

/// <summary>
/// Represents allowed value having option.
/// </summary>
public interface IHaveAllowedValues
{
    /// <summary>
    /// Returns all existing allowed values of the option. If it returns empty option accepts all given values. 
    /// </summary>
    List<string> GetAllowedValues();
}

/// <inheritdoc cref="IHaveAllowedValues"/>
/// <typeparam name="TValue">Type of the value</typeparam>
public interface IHaveAllowedValues<TValue> : IHaveAllowedValues
{
    /// <summary>
    /// Allowed values of the option 
    /// </summary>
    List<TValue> AllowedValues { get; }
}

/// <summary>
/// Represents parsed, validated and converted values of a sequential option.
/// </summary>
/// <typeparam name="TValue">Type of the value</typeparam>
public interface IHaveResultValues<TValue>
{
    /// <summary>
    /// Parsed result values of a sequential option
    /// </summary>
    List<TValue> ResultValues { get; }
}

/// <summary>
/// Represents parsed, validated and converted value of a scalar option.
/// </summary>
public interface IHaveResultValue<TValue>
{
    /// <summary>
    /// Parsed result value of a scalar option
    /// </summary>
    Ref<TValue>? ResultValue { get; }
}

/// <summary>
/// Represents default value of an option, that apply if no value is explicitly provided.
/// </summary>
public interface IHaveDefaultValue
{
    /// <summary>
    /// Returns default value of the option as string. If it returns null, there is no default value. 
    /// </summary>
    string? GetDefaultValue();
}

/// <inheritdoc cref="IHaveDefaultValue"/>
/// <typeparam name="TValue">Type of the value</typeparam>
public interface IHaveDefaultValue<TValue> : IHaveDefaultValue
{
    /// <summary>
    /// Represents default value of the option. 
    /// </summary>
    Ref<TValue>? DefaultValue { get; }
}

/// <summary>
/// Represents a nullable value class.
/// </summary>
public class Ref<TValue>
{
    /// <summary>
    /// Value of the Value Referer
    /// </summary>
    public TValue Value { get; }

    /// <summary>
    /// Constructor of the Value Referer
    /// </summary>
    public Ref(TValue value)
    {
        Value = value;
    }
}
