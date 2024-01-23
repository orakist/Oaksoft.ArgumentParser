using System;
using System.Collections.Generic;

namespace Oaksoft.ArgumentParser.Options;

public interface INamedOption : IBaseOption
{
    string Alias { get; }

    List<string> Aliases { get; }

    List<string> OptionTokens { get; }
}

public interface IScalarNamedOption : INamedOption, IScalarValueOption
{
}

public interface ISequentialNamedOption : INamedOption, ISequentialValueOption
{
    bool EnableSequentialValues { get; }
}

public interface ICounterOption : INamedOption, IValueOption
{
}

public interface ISwitchOption : IScalarNamedOption, IHaveResultValue<bool>, IHaveDefaultValue<bool>
{
}

public interface IScalarNamedOption<TValue> : IScalarNamedOption, IScalarValueOption<TValue>, IHaveDefaultValue<TValue>
    where TValue : IComparable
{
}

public interface ISequentialNamedOption<TValue> : ISequentialNamedOption, ISequentialValueOption<TValue>
    where TValue : IComparable
{
}
