using System;
using System.Collections.Generic;

namespace Oaksoft.ArgumentParser.Options;

public interface INamedOption : IBaseOption
{
    string ShortAlias { get; }

    List<string> Aliases { get; }

    List<string> OptionTokens { get; }
}

public interface IScalarNamedOption : INamedOption, IScalarValueOption
{
}

public interface ISequentialNamedOption : INamedOption, ISequentialValueOption
{
    bool AllowSequentialValues { get; }
}

public interface ICounterOption : INamedOption, IValueOption
{
}

public interface ISwitchOption : IScalarNamedOption, IScalarValueOption<bool>, IHaveDefaultValue<bool>
{
}

public interface IScalarNamedOption<TValue> : IScalarNamedOption, IScalarValueOption<TValue>, IHaveDefaultValue<TValue>
    where TValue : IComparable, IEquatable<TValue>
{
}

public interface ISequentialNamedOption<TValue> : ISequentialNamedOption, ISequentialValueOption<TValue>
    where TValue : IComparable, IEquatable<TValue>
{
}
