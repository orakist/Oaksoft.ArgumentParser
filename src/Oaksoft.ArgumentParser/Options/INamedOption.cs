using System;
using System.Collections.Generic;

namespace Oaksoft.ArgumentParser.Options;

public interface INamedOption : IBaseOption
{
    string ShortAlias { get; }

    List<string> Aliases { get; }

    List<string> OptionTokens { get; }
}

public interface ISwitchNamedOption : INamedOption, IValueOption
{
}

public interface ISwitchOption : ISwitchNamedOption, ISwitchValueOption
{
}

public interface IScalarNamedOption : INamedOption, IValueOption
{
}

public interface IScalarNamedOption<TValue> : IScalarNamedOption, IScalarValueOption<TValue>
    where TValue : IComparable, IEquatable<TValue>
{
}

public interface ISequentialNamedOption : INamedOption, IValueOption
{
    bool AllowSequentialValues { get; }
}

public interface ISequentialNamedOption<TValue> : ISequentialNamedOption, ISequentialValueOption<TValue>
    where TValue : IComparable, IEquatable<TValue>
{
}
