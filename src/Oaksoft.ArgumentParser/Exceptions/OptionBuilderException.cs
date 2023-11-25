using System;

namespace Oaksoft.ArgumentParser.Exceptions;

public class OptionBuilderException : Exception
{
    public IErrorMessage Error { get; }

    public OptionBuilderException(IErrorMessage error)
        : base(error.Message)
    {
        Error = error;
    }
}