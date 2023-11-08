using System;
using System.Collections.Generic;
using System.Linq;

namespace Oaksoft.ArgumentParser.Callbacks;

public abstract class BaseParsingCallbacks<TValue> : IParsingCallbacks<TValue>
    where TValue : IComparable, IEquatable<TValue>
{
    public virtual bool TryParseValue(string value, out TValue result)
    {
        result = default!;
        return false;
    }

    public virtual List<TValue> TryParseValues(List<string> values)
    {
        return Enumerable.Empty<TValue>().ToList();
    }
}
