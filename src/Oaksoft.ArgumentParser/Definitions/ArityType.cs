namespace Oaksoft.ArgumentParser.Definitions;

public enum ArityType
{
    /// <summary>
    /// No values allowed.
    /// </summary>
    Zero,

    /// <summary>
    /// May have one value, may have no values.
    /// </summary>
    ZeroOrOne,

    /// <summary>
    /// Must have one value.
    /// </summary>
    ExactlyOne,

    /// <summary>
    /// May have one value, multiple values, or no values.
    /// </summary>
    ZeroOrMore,

    /// <summary>
    /// May have multiple values, must have at least one value.
    /// </summary>
    OneOrMore
}
