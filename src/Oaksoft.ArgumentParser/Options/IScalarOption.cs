using System;

namespace Oaksoft.ArgumentParser.Options;

public interface IScalarOption : IAliasedOption, IValueOption
{
    bool AllowSequentialValues { get; }
}

public interface IScalarOption<TValue> : IScalarOption, IValueOption<TValue>
    where TValue : IComparable, IEquatable<TValue>
{
}
