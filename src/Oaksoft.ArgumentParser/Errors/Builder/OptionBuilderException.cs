using System;

namespace Oaksoft.ArgumentParser.Errors.Builder;

/// <summary>
/// Represents OptionBuilderException. Thrown while building the IArgumentParser. 
/// </summary>
public class OptionBuilderException : Exception
{
    /// <summary>
    /// Represents error details of the exception.
    /// </summary>
    public IErrorMessage Error { get; }

    /// <summary>
    /// Creates an OptionBuilderException.
    /// </summary>
    public OptionBuilderException(IErrorMessage error)
        : base(error.Message)
    {
        Error = error;
    }
}