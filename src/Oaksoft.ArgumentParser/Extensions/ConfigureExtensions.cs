using System;
using Oaksoft.ArgumentParser.Callbacks;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Extensions;

public static class ConfigureExtensions
{
    public static IBaseOption WithName(this IBaseOption option, string name)
    {
        ((BaseOption)option).SetName(name);
        return option;
    }

    public static IBaseOption WithUsage(this IBaseOption option, string usage)
    {
        ((BaseOption)option).SetUsage(usage);
        return option;
    }

    public static IBaseOption WithDescription(this IBaseOption option, string? description)
    {
        ((BaseOption)option).SetDescription(description);
        return option;
    }

    public static IScalarOption<TValue> WithAliases<TValue>(
        this IScalarOption<TValue> option, params string[] aliases)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseOption)option).SetAliases(aliases);
        return option;
    }

    public static ISwitchOption WithAliases(this ISwitchOption option, params string[] aliases)
    {
        ((BaseOption)option).SetAliases(aliases);
        return option;
    }

    public static IScalarOption<TValue> WithValueArity<TValue>(
        this IScalarOption<TValue> option, ArityType valueArity)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption<TValue>)option).SetValueArity(valueArity);
        return option;
    }

    public static IValueOption<TValue> WithValueArity<TValue>(
        this IValueOption<TValue> option, ArityType valueArity)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption<TValue>)option).SetValueArity(valueArity);
        return option;
    }

    public static IScalarOption<TValue> WithValueArity<TValue>(
        this IScalarOption<TValue> option, int requiredValueCount, int maximumValueCount)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption<TValue>)option).SetValueArity(requiredValueCount, maximumValueCount);
        return option;
    }

    public static IValueOption<TValue> WithValueArity<TValue>(
        this IValueOption<TValue> option, int requiredValueCount, int maximumValueCount)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption<TValue>)option).SetValueArity(requiredValueCount, maximumValueCount);
        return option;
    }

    public static IScalarOption<TValue> WithOptionArity<TValue>(
        this IScalarOption<TValue> option, ArityType optionArity)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarOption<TValue>)option).SetOptionArity(optionArity);
        return option;
    }
    
    public static IScalarOption<TValue> WithOptionArity<TValue>(
        this IScalarOption<TValue> option, int requiredOptionCount, int maximumOptionCount)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarOption<TValue>)option).SetOptionArity(requiredOptionCount, maximumOptionCount);
        return option;
    }

    public static ISwitchOption WithOptionArity(
        this ISwitchOption option, ArityType optionArity)
    {
        ((SwitchOption)option).SetOptionArity(optionArity);
        return option;
    }

    public static ISwitchOption WithOptionArity(
        this ISwitchOption option, int requiredOptionCount, int maximumOptionCount)
    {
        ((SwitchOption)option).SetOptionArity(requiredOptionCount, maximumOptionCount);
        return option;
    }

    public static ISwitchOption WithDefaultValue(this ISwitchOption option, bool defaultValue)
    {
        ((SwitchOption)option).SetDefaultValue(defaultValue);
        return option;
    }

    public static IScalarOption<TValue> WithDefaultValue<TValue>(
        this IScalarOption<TValue> option, TValue defaultValue)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption<TValue>)option).SetDefaultValue(defaultValue);
        return option;
    }

    public static IValueOption<TValue> WithDefaultValue<TValue>(
        this IValueOption<TValue> option, TValue defaultValue)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption<TValue>)option).SetDefaultValue(defaultValue);
        return option;
    }

    public static IScalarOption<TValue> WithConstraints<TValue>(
        this IScalarOption<TValue> option, params TValue?[] constraints)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption<TValue>)option).SetConstraints(constraints);
        return option;
    }

    public static IValueOption<TValue> WithConstraints<TValue>(
        this IValueOption<TValue> option, params TValue?[] constraints)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption<TValue>)option).SetConstraints(constraints);
        return option;
    }

    public static IScalarOption<TValue> WithAllowedValues<TValue>(
        this IScalarOption<TValue> option, params TValue[] allowedValues)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption<TValue>)option).SetAllowedValues(allowedValues);
        return option;
    }

    public static IValueOption<TValue> WithAllowedValues<TValue>(
        this IValueOption<TValue> option, params TValue[] allowedValues) 
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption<TValue>)option).SetAllowedValues(allowedValues);
        return option;
    }

    public static IScalarOption<TValue> WithParsingCallbacks<TValue>(
        this IScalarOption<TValue> option, IParsingCallbacks<TValue> optionCallbacks)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption<TValue>)option).SetParsingCallbacks(optionCallbacks);
        return option;
    }

    public static IValueOption<TValue> WithParsingCallbacks<TValue>(
        this IValueOption<TValue> option, IParsingCallbacks<TValue> optionCallbacks)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption<TValue>)option).SetParsingCallbacks(optionCallbacks);
        return option;
    }

    public static IScalarOption<TValue> WithValueValidator<TValue>(
        this IScalarOption<TValue> option, Func<string, bool> validator)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption<TValue>)option).SetValueValidator(validator);
        return option;
    }

    public static IValueOption<TValue> WithValueValidator<TValue>(
        this IValueOption<TValue> option, Func<string, bool> validator)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption<TValue>)option).SetValueValidator(validator);
        return option;
    }

    public static IScalarOption<TValue> WithValueConvertor<TValue>(
        this IScalarOption<TValue> option, Func<string, TValue> validator)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption<TValue>)option).SetValueConvertor(validator);
        return option;
    }

    public static IValueOption<TValue> WithValueConvertor<TValue>(
        this IValueOption<TValue> option, Func<string, TValue> validator)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption<TValue>)option).SetValueConvertor(validator);
        return option;
    }

    public static IScalarOption<TValue> WithOptionValidator<TValue>(
        this IScalarOption<TValue> option, Func<IValueContext<TValue>, IArgumentParser, bool> validator)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption<TValue>)option).SetOptionValidator(validator);
        return option;
    }

    public static IValueOption<TValue> WithOptionValidator<TValue>(
        this IValueOption<TValue> option, Func<IValueContext<TValue>, IArgumentParser, bool> validator)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption<TValue>)option).SetOptionValidator(validator);
        return option;
    }
}
