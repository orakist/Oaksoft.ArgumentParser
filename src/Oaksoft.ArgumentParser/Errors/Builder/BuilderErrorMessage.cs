using System;

namespace Oaksoft.ArgumentParser.Errors.Builder;

internal sealed class BuilderErrorMessage : ErrorMessage
{
    public BuilderErrorMessage(ErrorInfo error)
        : base(error)
    {
    }

    public override Exception ToException(params object[] values)
    {
        With(values);
        return new OptionBuilderException(this);
    }
}