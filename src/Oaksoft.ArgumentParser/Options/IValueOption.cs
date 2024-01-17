using System;
using System.Collections.Generic;

namespace Oaksoft.ArgumentParser.Options;

public interface IValueOption : IHaveValueOption, IBaseOption
{
}

public interface IScalarValueOption : IValueOption
{
}

public interface ISequentialValueOption : IValueOption
{
    bool EnableValueTokenSplitting { get; }
}

public interface IScalarValueOption<TValue>
    : IScalarValueOption, IHaveResultValue<TValue>, IHaveAllowedValues<TValue>
    where TValue : IComparable, IEquatable<TValue>
{
}

public interface ISequentialValueOption<TValue>
    : ISequentialValueOption, IHaveResultValues<TValue>, IHaveAllowedValues<TValue>
    where TValue : IComparable, IEquatable<TValue>
{
}

public interface IHaveValueOption
{
    List<string> ValueTokens { get; }

    List<string> InputValues { get; }
}

public interface IHaveAllowedValues
{
    List<string> GetAllowedValues();
}

public interface IHaveAllowedValues<TValue> : IHaveAllowedValues
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

public interface IHaveDefaultValue
{
    string? GetDefaultValue();
}

public interface IHaveDefaultValue<TValue> : IHaveDefaultValue
{
    Ref<TValue>? DefaultValue { get; }
}

public class Ref<TValue>
{
    public TValue Value { get; }

    public Ref(TValue value)
    {
        Value = value;
    }
}
