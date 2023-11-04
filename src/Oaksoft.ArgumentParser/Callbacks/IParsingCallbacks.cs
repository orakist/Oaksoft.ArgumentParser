using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Parser;
using System;

namespace Oaksoft.ArgumentParser.Callbacks;

public interface IParsingCallbacks<TValue>
    where TValue : IComparable, IEquatable<TValue>
{
    bool ValidateValue(string value);

    TValue ConvertValue(string value);

    bool ValidateOption(IValueContext<TValue> context, IArgumentParser parser);
}
