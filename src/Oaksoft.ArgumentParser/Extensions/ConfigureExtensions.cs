using System;
using System.Linq;
using Oaksoft.ArgumentParser.Callbacks;
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
        ((AliasedOption)option).SetAliases(aliases);
        return option;
    }

    public static IScalarOption WithAliases(this IScalarOption option, params string[] aliases)
    {
        ((AliasedOption)option).SetAliases(aliases);
        return option;
    }

    public static ISwitchOption WithAliases(this ISwitchOption option, params string[] aliases)
    {
        ((AliasedOption)option).SetAliases(aliases);
        return option;
    }

    public static IAliasedOption WithAliases(this IAliasedOption option, params string[] aliases)
    {
        ((AliasedOption)option).SetAliases(aliases);
        return option;
    }

    public static ISwitchOption WithDefaultValue(this ISwitchOption option, bool? defaultValue)
    {
        ((SwitchOption)option).SetDefaultValue(defaultValue);
        return option;
    }

    public static IScalarOption<TValue> WithDefaultValue<TValue>(
        this IScalarOption<TValue> option, TValue defaultValue)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarOption<TValue>)option).SetDefaultValue(defaultValue);
        return option;
    }

    public static IValueOption<TValue> WithDefaultValue<TValue>(
        this IValueOption<TValue> option, TValue defaultValue)
        where TValue : IComparable, IEquatable<TValue>
    {
        switch (option)
        {
            case ScalarOption<TValue> scalarCommand:
                scalarCommand.SetDefaultValue(defaultValue);
                break;
            case ValueOption nonCommand:
                nonCommand.SetDefaultValue(defaultValue as string);
                break;
        }

        return option;
    }

    public static IScalarOption<TValue> WithConstraints<TValue>(
        this IScalarOption<TValue> option, params TValue?[] constraints)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarOption<TValue>)option).SetConstraints(constraints);
        return option;
    }

    public static IValueOption<TValue> WithConstraints<TValue>(
        this IValueOption<TValue> option, params TValue?[] constraints)
        where TValue : IComparable, IEquatable<TValue>
    {
        switch (option)
        {
            case ScalarOption<TValue> scalarCommand:
                scalarCommand.SetConstraints(constraints);
                break;
            case ValueOption nonCommand:
                nonCommand.SetConstraints(constraints.Cast<string>().ToArray());
                break;
        }

        return option;
    }

    public static IScalarOption<TValue> WithAllowedValues<TValue>(
        this IScalarOption<TValue> option, params TValue[] allowedValues)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarOption<TValue>)option).SetAllowedValues(allowedValues);
        return option;
    }

    public static IValueOption<TValue> WithAllowedValues<TValue>(
        this IValueOption<TValue> option, params TValue[] allowedValues) 
        where TValue : IComparable, IEquatable<TValue>
    {
        switch (option)
        {
            case ScalarOption<TValue> scalarCommand:
                scalarCommand.SetAllowedValues(allowedValues);
                break;
            case ValueOption nonCommand:
                nonCommand.SetAllowedValues(allowedValues.Cast<string>().ToArray());
                break;
        }

        return option;
    }

    public static IScalarOption<TValue> WithParsingCallbacks<TValue>(
        this IScalarOption<TValue> option, IParsingCallbacks<TValue> optionCallbacks)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarOption<TValue>)option).SetParsingCallbacks(optionCallbacks);
        return option;
    }

    public static IScalarOption<TValue> WithValueValidator<TValue>(
        this IScalarOption<TValue> option, Func<string, bool> validator)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarOption<TValue>)option).SetValueValidator(validator);
        return option;
    }

    public static IScalarOption<TValue> WithValueConvertor<TValue>(
        this IScalarOption<TValue> option, Func<string, TValue> validator)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarOption<TValue>)option).SetValueConvertor(validator);
        return option;
    }

    public static IScalarOption<TValue> WithOptionValidator<TValue>(
        this IScalarOption<TValue> option, Func<IValueContext<TValue>, IArgumentParser, bool> validator)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarOption<TValue>)option).SetOptionValidator(validator);
        return option;
    }
}
