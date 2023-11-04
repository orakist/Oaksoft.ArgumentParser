using System;
using System.Collections.Generic;

namespace Oaksoft.ArgumentParser.Options;

public interface IValueOption : IBaseOption
{
    List<string> ValueTokens { get; }

    List<string> InputValues { get; }

    bool EnableValueTokenSplitting { get; }
}

public interface IValueContext<TValue>
    where TValue : IComparable, IEquatable<TValue>
{
    List<string> InputValues { get; }

    TValue? DefaultValue { get; }

    List<TValue?> Constraints { get; }

    List<TValue> AllowedValues { get; }
}

public interface IValueOption<TValue> : IValueOption
    where TValue : IComparable, IEquatable<TValue>
{
    TValue? DefaultValue { get; }

    List<TValue?> Constraints { get; }

    List<TValue> AllowedValues { get; }

    List<TValue> ResultValues { get; }
}


