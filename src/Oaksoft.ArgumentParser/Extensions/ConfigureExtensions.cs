using System;
using System.Reflection;
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

    public static IHaveValueOption WithDefaultValue(this IHaveValueOption option, string? defaultValue)
    {
        switch (option)
        {
            case ScalarCommandOption scalarCommand:
                scalarCommand.SetDefaultValue(defaultValue);
                break;
            case NonCommandOption nonCommand:
                nonCommand.SetDefaultValue(defaultValue);
                break;
        }

        return option;
    }

    public static IHaveValueOption WithConstraints(this IHaveValueOption option, params string?[] constraints)
    {
        switch (option)
        {
            case ScalarCommandOption scalarCommand:
                scalarCommand.SetConstraints(constraints);
                break;
            case NonCommandOption nonCommand:
                nonCommand.SetConstraints(constraints);
                break;
        }

        return option;
    }

    public static IHaveValueOption WithAllowedValues(this IHaveValueOption option, params string[] allowedValues)
    {
        switch (option)
        {
            case ScalarCommandOption scalarCommand:
                scalarCommand.SetAllowedValues(allowedValues);
                break;
            case NonCommandOption nonCommand:
                nonCommand.SetAllowedValues(allowedValues);
                break;
        }

        return option;
    }

    public static IScalarCommandOption WithParsingCallbacks(
        this IScalarCommandOption option, IParsingCallbacks optionCallbacks)
    {
        ((ScalarCommandOption)option).SetParsingCallbacks(optionCallbacks);
        return option;
    }

    public static IScalarCommandOption WithValueValidator(
        this IScalarCommandOption option, Func<string, bool> validator)
    {
        ((ScalarCommandOption)option).SetBaseValidator(validator);
        return option;
    }

    public static IScalarCommandOption WithOptionValidator(
        this IScalarCommandOption option, Func<IValueContext, IArgumentParser, bool> validator)
    {
        ((ScalarCommandOption)option).SetOptionValidator(validator);
        return option;
    }

    public static IScalarCommandOption WithDefaultValueSetterAction(
        this IScalarCommandOption option, Action<IValueContext, IApplicationOptions, PropertyInfo> setterAction)
    {
        ((ScalarCommandOption)option).SetDefaultValueSetterAction(setterAction);
        return option;
    }

    public static IScalarCommandOption WithOptionValueSetterAction(
        this IScalarCommandOption option, Action<IValueContext, IApplicationOptions, PropertyInfo> setterAction)
    {
        ((ScalarCommandOption)option).SetOptionValueSetterAction(setterAction);
        return option;
    }
}
