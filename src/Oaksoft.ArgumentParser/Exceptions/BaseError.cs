using System;

namespace Oaksoft.ArgumentParser.Exceptions;

public abstract record BaseError(string Code, string Message)
{
    public object[]? Values { get; init; }

    public string FormatMessage()
    {
        return Values?.Length > 0 ? string.Format(Message, Values) : Message;
    }

    public abstract BaseError With(params object[] values);

    public abstract Exception ToException();

    public abstract Exception ToException(string optionName);
}

internal sealed record BuilderError(string Code, string Message)
    : BaseError(Code, Message)
{
    public override BaseError With(params object[] values)
    {
        return values.Length > 0
            ? new BuilderError(Code, Message) { Values = values } 
            : this;
    }

    public override Exception ToException()
    {
        return new OptionBuilderException(this);
    }

    public override Exception ToException(string optionName)
    {
        return new OptionBuilderException(this, optionName);
    }
}

internal sealed record ParserError(string Code, string Message)
    : BaseError(Code, Message)
{
    public override BaseError With(params object[] values)
    {
        return values.Length > 0
            ? new ParserError(Code, Message) { Values = values }
            : this;
    }

    public override Exception ToException()
    {
        throw new NotImplementedException();
    }

    public override Exception ToException(string optionName)
    {
        throw new NotImplementedException();
    }
}