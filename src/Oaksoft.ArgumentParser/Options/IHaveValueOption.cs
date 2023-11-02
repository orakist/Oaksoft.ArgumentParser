using System;
using System.Collections.Generic;

namespace Oaksoft.ArgumentParser.Options;

public interface IHaveValueOption : IBaseOption
{
    List<string> ValueTokens { get; }

    List<string> InputValues { get; }

    bool EnableValueTokenSplitting { get; }
}

public interface IHaveValueOption<TValue> : IHaveValueOption
    where TValue : IComparable, IEquatable<TValue>
{
    TValue? DefaultValue { get; }

    List<TValue?> Constraints { get; }

    List<TValue> AllowedValues { get; }

    List<TValue> ResultValues { get; }
}
