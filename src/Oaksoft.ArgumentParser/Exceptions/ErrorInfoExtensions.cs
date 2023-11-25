using System;

namespace Oaksoft.ArgumentParser.Exceptions;

internal static class ErrorInfoExtensions
{
    public static ErrorMessage With(this ErrorInfo error, params object[] values)
    {
        if (error.Code.StartsWith(BuilderErrors.Name))
            return new BuilderError(error).With(values);

        throw new NotSupportedException();
    }

    public static ErrorMessage WithName(this ErrorInfo error, string optionName)
    {
        return error.With().WithName(optionName);
    }

    public static Exception ToException(this ErrorInfo error, params object[] values)
    {
        return error.With(values).ToException();
    }
}