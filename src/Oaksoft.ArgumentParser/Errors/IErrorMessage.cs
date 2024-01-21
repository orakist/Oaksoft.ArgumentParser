using System;

namespace Oaksoft.ArgumentParser.Errors;

public record ErrorInfo(string Code, string Format);

public interface IErrorMessage
{
    ErrorInfo Error { get; }

    Exception? Exception { get; }

    string Message { get; }

    object[]? Values { get; }

    string? OptionName { get; }
}