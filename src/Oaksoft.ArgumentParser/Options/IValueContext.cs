using System;
using System.Collections.Generic;

namespace Oaksoft.ArgumentParser.Options;

public interface IValueContext<TValue>
    where TValue : IComparable, IEquatable<TValue>
{
    List<string> InputValues { get; }

    TValue? DefaultValue { get; }

    List<TValue?> Constraints { get; }

    List<TValue> AllowedValues { get; }
}
