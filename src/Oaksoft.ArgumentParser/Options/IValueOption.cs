using System;
using System.Collections.Generic;

namespace Oaksoft.ArgumentParser.Options;

public interface IValueOption : IHaveValueOption, IBaseOption
{
}

public interface ISwitchValueOption
    : IHaveDefaultValue<bool>, IHaveResultValue<bool>, IValueOption
{
}

public interface IScalarValueOption<TValue>
    : IHaveDefaultValue<TValue>, IHaveResultValue<TValue>, IHaveAllowedValues<TValue>, IValueOption
    where TValue : IComparable, IEquatable<TValue>
{
}

public interface ISequentialValueOption<TValue>
    : IHaveResultValues<TValue>, IHaveAllowedValues<TValue>, IValueOption
    where TValue : IComparable, IEquatable<TValue>
{
    bool EnableValueTokenSplitting { get; }
}

public interface IHaveValueOption
{
    List<string> ValueTokens { get; }

    List<string> InputValues { get; }
}

public interface IHaveDefaultValue<TValue>
{
    Ref<TValue>? DefaultValue { get; }
}

public interface IHaveAllowedValues<TValue>
{
    List<TValue> AllowedValues { get; }
}

public interface IHaveResultValues<TValue>
{
    List<TValue> ResultValues { get; }
}

public interface IHaveResultValue<TValue>
{
    Ref<TValue>? ResultValue { get; }
}

public class Ref<TValue>
{
    public TValue Value { get; }

    public Ref(TValue value)
    {
        Value = value;
    }
}
