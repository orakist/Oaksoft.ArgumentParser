using System;

namespace Oaksoft.ArgumentParser.Errors.Builder;

public class OptionBuilderException : Exception
{
    public IErrorMessage Error { get; }

    public OptionBuilderException(IErrorMessage error)
        : base(error.Message)
    {
        Error = error;
    }
}