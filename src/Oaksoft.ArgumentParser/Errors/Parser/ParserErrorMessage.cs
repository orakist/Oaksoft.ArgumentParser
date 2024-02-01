using System;

namespace Oaksoft.ArgumentParser.Errors.Parser;

internal sealed class ParserErrorMessage : ErrorMessage
{
    public ParserErrorMessage(ErrorInfo error)
        : base(error)
    {
    }

    public override Exception ToException(params object[] values)
    {
        With(values);
        return new OptionParserException(this);
    }
}
