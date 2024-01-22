using System;
using Oaksoft.ArgumentParser.Errors.Builder;
using Oaksoft.ArgumentParser.Errors.Parser;

namespace Oaksoft.ArgumentParser.Errors;

internal static class ErrorExtensions
{
    public static ErrorMessage With(this ErrorInfo error, params object[] values)
    {
        if (error.Code.StartsWith(BuilderErrors.Name))
        {
            return new BuilderErrorMessage(error).With(values);
        }

        if (error.Code.StartsWith(ParserErrors.Name))
        {
            return new ParserErrorMessage(error).With(values);
        }

        throw new NotSupportedException();
    }

    public static ErrorMessage WithException(this ErrorInfo error, Exception exception)
    {
        return error.With().WithException(exception);
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