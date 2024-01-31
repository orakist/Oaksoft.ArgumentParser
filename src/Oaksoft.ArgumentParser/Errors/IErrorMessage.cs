using System;

namespace Oaksoft.ArgumentParser.Errors;

/// <summary>
/// Represents error code and error format (raw error message)
/// </summary>
/// <param name="Code"></param>
/// <param name="Format"></param>
public record ErrorInfo(string Code, string Format);

/// <summary>
/// Represents an error message
/// </summary>
public interface IErrorMessage
{
    /// <summary>
    /// Error of the error message
    /// </summary>
    ErrorInfo Error { get; }

    /// <summary>
    /// Exception of the error message 
    /// </summary>
    Exception? Exception { get; }

    /// <summary>
    /// Formatted message text of the error message 
    /// </summary>
    string Message { get; }

    /// <summary>
    /// Values of the error message 
    /// </summary>
    object[]? Values { get; }

    /// <summary>
    /// OptionName of the error message 
    /// </summary>
    string? OptionName { get; }
}