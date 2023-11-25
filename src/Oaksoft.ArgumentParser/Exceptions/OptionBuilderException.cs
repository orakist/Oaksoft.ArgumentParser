using System;

namespace Oaksoft.ArgumentParser.Exceptions;

public class OptionBuilderException : Exception
{
    public BaseError Error { get; }

    public string? OptionName { get; }

    public OptionBuilderException(BaseError error)
        : base(GetMessage(error, null))
    {
        Error = error;
    }

    public OptionBuilderException(BaseError error, string? optionName)
        : base(GetMessage(error, optionName))
    {
        Error = error;
        OptionName = optionName;
    }

    private static string GetMessage(BaseError error, string? optionName)
    {
        var message = error.FormatMessage();
        if (string.IsNullOrWhiteSpace(optionName))
            return message;

        var comma = message.EndsWith(".") ? string.Empty : ",";
        return $"{message}{comma} Option: {optionName}";
    }
}