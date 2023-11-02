using System;

namespace Oaksoft.ArgumentParser.Options;

public interface IScalarCommandOption : ICommandOption, IHaveValueOption
{
    bool ValueTokenMustExist { get; }

    bool AllowSequentialValues { get; }
}

public interface IScalarCommandOption<TValue> : IScalarCommandOption, IHaveValueOption<TValue>
    where TValue : IComparable, IEquatable<TValue>
{
}
