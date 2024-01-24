using System;

namespace Oaksoft.ArgumentParser.Errors;

internal abstract class ErrorMessage : IErrorMessage
{
    public ErrorInfo Error { get; }

    public Exception? Exception { get; private set; }

    public string Message => _message ?? InitializeMessage();

    public object[]? Values { get; private set; }

    public string? OptionName { get; private set; }
    
    private string? _message;

    protected ErrorMessage(ErrorInfo error)
    {
        Error = error;
    }

    public ErrorMessage With(params object[] values)
    {
        _message = null;

        if(values.Length > 0)
        {
            Values = values;
        }

        return this;
    }

    public ErrorMessage WithName(string optionName)
    {
        _message = null;
        OptionName = optionName;
        return this;
    }

    public ErrorMessage WithException(Exception exception)
    {
        _message = null;
        Exception = exception;
        return this;
    }

    public abstract Exception ToException(params object[] values);

    private string InitializeMessage()
    {
        var message = Values?.Length > 0 ? string.Format(Error.Format, Values) : Error.Format;

        if (string.IsNullOrWhiteSpace(OptionName))
        {
            return message;
        }

        var comma = message.EndsWith(".") ? string.Empty : ",";
        _message = $"{message}{comma} Option: {OptionName}";

        return _message;
    }
}