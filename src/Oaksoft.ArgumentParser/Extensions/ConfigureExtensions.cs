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

    public static IScalarCommandOption<TValue> WithCommands<TValue>(
        this IScalarCommandOption<TValue> option, params string[] commands)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((CommandOption)option).SetCommands(commands);
        return option;
    }

    public static IScalarCommandOption WithCommands(this IScalarCommandOption option, params string[] commands)
    {
        ((CommandOption)option).SetCommands(commands);
        return option;
    }

    public static ICommandOption WithCommands(this ICommandOption option, params string[] commands)
    {
        ((CommandOption)option).SetCommands(commands);
        return option;
    }

    public static IHaveValueOption<TValue> WithDefaultValue<TValue>(
        this IHaveValueOption<TValue> option, TValue defaultValue)
        where TValue : IComparable, IEquatable<TValue>
    {
        switch (option)
        {
            case ScalarCommandOption<TValue> scalarCommand:
                scalarCommand.SetDefaultValue(defaultValue);
                break;
            case NonCommandOption nonCommand:
                nonCommand.SetDefaultValue(defaultValue as string);
                break;
        }

        return option;
    }

    public static IHaveValueOption<TValue> WithConstraints<TValue>(
        this IHaveValueOption<TValue> option, params TValue?[] constraints)
        where TValue : IComparable, IEquatable<TValue>
    {
        switch (option)
        {
            case ScalarCommandOption<TValue> scalarCommand:
                scalarCommand.SetConstraints(constraints);
                break;
            case NonCommandOption nonCommand:
                nonCommand.SetConstraints(constraints.Cast<string>().ToArray());
                break;
        }

        return option;
    }

    public static IHaveValueOption<TValue> WithAllowedValues<TValue>(
        this IHaveValueOption<TValue> option, params TValue[] allowedValues) 
        where TValue : IComparable, IEquatable<TValue>
    {
        switch (option)
        {
            case ScalarCommandOption<TValue> scalarCommand:
                scalarCommand.SetAllowedValues(allowedValues);
                break;
            case NonCommandOption nonCommand:
                nonCommand.SetAllowedValues(allowedValues.Cast<string>().ToArray());
                break;
        }

        return option;
    }

    public static IScalarCommandOption<TValue> WithParsingCallbacks<TValue>(
        this IScalarCommandOption<TValue> option, IParsingCallbacks<TValue> optionCallbacks)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarCommandOption<TValue>)option).SetParsingCallbacks(optionCallbacks);
        return option;
    }

    public static IScalarCommandOption<TValue> WithValueValidator<TValue>(
        this IScalarCommandOption<TValue> option, Func<string, bool> validator)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarCommandOption<TValue>)option).SetValueValidator(validator);
        return option;
    }

    public static IScalarCommandOption<TValue> WithValueConvertor<TValue>(
        this IScalarCommandOption<TValue> option, Func<string, TValue> validator)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarCommandOption<TValue>)option).SetValueConvertor(validator);
        return option;
    }

    public static IScalarCommandOption<TValue> WithOptionValidator<TValue>(
        this IScalarCommandOption<TValue> option, Func<IValueContext<TValue>, IArgumentParser, bool> validator)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarCommandOption<TValue>)option).SetOptionValidator(validator);
        return option;
    }
}
