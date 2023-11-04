using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Parser;
using System;

namespace Oaksoft.ArgumentParser.Callbacks;

public abstract class BaseParsingCallbacks<TValue> : IParsingCallbacks<TValue>
    where TValue : IComparable, IEquatable<TValue>
{
    public virtual bool ValidateValue(string value)
    {
        return true;
    }

    public virtual TValue ConvertValue(string value)
    {
        return default!;
    }

    public virtual bool ValidateOption(IValueContext<TValue> context, IArgumentParser parser)
    {
        return true;
    }
}
