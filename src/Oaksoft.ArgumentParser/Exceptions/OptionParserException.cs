using System;

namespace Oaksoft.ArgumentParser.Exceptions;

internal class OptionParserException : Exception
{
    public ErrorMessage Error { get; }

    public OptionParserException(ErrorMessage error)
        : base(error.Message)
    {
        Error = error;
    }
}