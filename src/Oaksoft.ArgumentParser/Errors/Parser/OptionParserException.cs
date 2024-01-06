using System;

namespace Oaksoft.ArgumentParser.Errors.Parser;

internal class OptionParserException : Exception
{
    public ErrorMessage Error { get; }

    public OptionParserException(ErrorMessage error)
        : base(error.Message)
    {
        Error = error;
    }
}