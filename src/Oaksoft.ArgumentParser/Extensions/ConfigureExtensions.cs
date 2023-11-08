using System;
using System.Collections.Generic;
using Oaksoft.ArgumentParser.Callbacks;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Options;

namespace Oaksoft.ArgumentParser.Extensions;

public static class ConfigureExtensions
{
    #region Base Configurations
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

    public static ISequentialNamedOption<TValue> WithAliases<TValue>(
        this ISequentialNamedOption<TValue> option, params string[] aliases)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseOption)option).SetAliases(aliases);
        return option;
    }

    public static IScalarNamedOption<TValue> WithAliases<TValue>(
        this IScalarNamedOption<TValue> option, params string[] aliases)
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
    #endregion

    #region Arity Configuration
    public static ISequentialValueOption<TValue> WithValueArity<TValue>(
        this ISequentialValueOption<TValue> option, ArityType valueArity)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption)option).SetValueArity(valueArity);
        return option;
    }

    public static ISequentialValueOption<TValue> WithCustomValueArity<TValue>(
        this ISequentialValueOption<TValue> option, int requiredValueCount, int maximumValueCount)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption)option).SetValueArity(requiredValueCount, maximumValueCount);
        return option;
    }

    public static ISequentialNamedOption<TValue> WithValueArity<TValue>(
        this ISequentialNamedOption<TValue> option, ArityType valueArity)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption)option).SetValueArity(valueArity);
        return option;
    }

    public static ISequentialNamedOption<TValue> WithCustomValueArity<TValue>(
        this ISequentialNamedOption<TValue> option, int requiredValueCount, int maximumValueCount)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption)option).SetValueArity(requiredValueCount, maximumValueCount);
        return option;
    }

    public static IScalarNamedOption<TValue> WithOptionArity<TValue>(
        this IScalarNamedOption<TValue> option, ArityType optionArity)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarNamedOption<TValue>)option).SetOptionArity(optionArity);
        return option;
    }

    public static IScalarNamedOption<TValue> WithCustomOptionArity<TValue>(
        this IScalarNamedOption<TValue> option, int requiredOptionCount, int maximumOptionCount)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarNamedOption<TValue>)option).SetOptionArity(requiredOptionCount, maximumOptionCount);
        return option;
    }

    public static ISequentialNamedOption<TValue> WithOptionArity<TValue>(
        this ISequentialNamedOption<TValue> option, ArityType optionArity)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((SequentialNamedOption<TValue>)option).SetOptionArity(optionArity);
        return option;
    }

    public static ISequentialNamedOption<TValue> WithCustomOptionArity<TValue>(
        this ISequentialNamedOption<TValue> option, int requiredOptionCount, int maximumOptionCount)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((SequentialNamedOption<TValue>)option).SetOptionArity(requiredOptionCount, maximumOptionCount);
        return option;
    }

    public static ISwitchOption WithOptionArity(this ISwitchOption option, ArityType optionArity)
    {
        ((SwitchOption)option).SetOptionArity(optionArity);
        return option;
    }

    public static ISwitchOption WithCustomOptionArity(
        this ISwitchOption option, int requiredOptionCount, int maximumOptionCount)
    {
        ((SwitchOption)option).SetOptionArity(requiredOptionCount, maximumOptionCount);
        return option;
    }
    #endregion

    #region Default Value Configuration
    public static IScalarNamedOption<TValue> WithDefaultValue<TValue>(
        this IScalarNamedOption<TValue> option, TValue defaultValue)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseScalarValueOption<TValue>)option).SetDefaultValue(defaultValue);
        return option;
    }

    public static IScalarValueOption<TValue> WithDefaultValue<TValue>(
        this IScalarValueOption<TValue> option, TValue defaultValue)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseScalarValueOption<TValue>)option).SetDefaultValue(defaultValue);
        return option;
    }

    public static ISwitchOption WithDefaultValue<TValue>(
        this ISwitchOption option, bool defaultValue)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((SwitchOption)option).SetDefaultValue(defaultValue);
        return option;
    }
    #endregion

    #region Allowed Values Configuration
    public static ISequentialNamedOption<TValue> WithAllowedValues<TValue>(
        this ISequentialNamedOption<TValue> option, params TValue[] allowedValues)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseAllowedValuesOption<TValue>)option).SetAllowedValues(allowedValues);
        return option;
    }

    public static IScalarNamedOption<TValue> WithAllowedValues<TValue>(
        this IScalarNamedOption<TValue> option, params TValue[] allowedValues)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseAllowedValuesOption<TValue>)option).SetAllowedValues(allowedValues);
        return option;
    }

    public static ISequentialValueOption<TValue> WithAllowedValues<TValue>(
        this ISequentialValueOption<TValue> option, params TValue[] allowedValues)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseAllowedValuesOption<TValue>)option).SetAllowedValues(allowedValues);
        return option;
    }

    public static IScalarValueOption<TValue> WithAllowedValues<TValue>(
        this IScalarValueOption<TValue> option, params TValue[] allowedValues)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseAllowedValuesOption<TValue>)option).SetAllowedValues(allowedValues);
        return option;
    }
    #endregion

    #region Predicate Configuration
    public static ISequentialNamedOption<TValue> AddPredicate<TValue>(
        this ISequentialNamedOption<TValue> option, Predicate<TValue> predicate)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseAllowedValuesOption<TValue>)option).AddPredicate(predicate);
        return option;
    }

    public static IScalarNamedOption<TValue> AddPredicate<TValue>(
        this IScalarNamedOption<TValue> option, Predicate<TValue> predicate)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseAllowedValuesOption<TValue>)option).AddPredicate(predicate);
        return option;
    }

    public static ISequentialValueOption<TValue> AddPredicate<TValue>(
        this ISequentialValueOption<TValue> option, Predicate<TValue> predicate)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseAllowedValuesOption<TValue>)option).AddPredicate(predicate);
        return option;
    }

    public static IScalarValueOption<TValue> AddPredicate<TValue>(
        this IScalarValueOption<TValue> option, Predicate<TValue> predicate)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseAllowedValuesOption<TValue>)option).AddPredicate(predicate);
        return option;
    }
    #endregion

    #region Parsing Callbacks Configuration
    public static ISequentialNamedOption<TValue> WithParsingCallbacks<TValue>(
        this ISequentialNamedOption<TValue> option, IParsingCallbacks<TValue> optionCallbacks)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption<TValue>)option).SetParsingCallbacks(optionCallbacks);
        return option;
    }

    public static IScalarNamedOption<TValue> WithParsingCallbacks<TValue>(
        this IScalarNamedOption<TValue> option, IParsingCallbacks<TValue> optionCallbacks)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption<TValue>)option).SetParsingCallbacks(optionCallbacks);
        return option;
    }

    public static ISequentialValueOption<TValue> WithParsingCallbacks<TValue>(
        this ISequentialValueOption<TValue> option, IParsingCallbacks<TValue> optionCallbacks)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption<TValue>)option).SetParsingCallbacks(optionCallbacks);
        return option;
    }

    public static IScalarValueOption<TValue> WithParsingCallbacks<TValue>(
        this IScalarValueOption<TValue> option, IParsingCallbacks<TValue> optionCallbacks)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption<TValue>)option).SetParsingCallbacks(optionCallbacks);
        return option;
    }

    public static ISequentialNamedOption<TValue> WithTryParseValueCallback<TValue>(
        this ISequentialNamedOption<TValue> option, TryParse<TValue> callback)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption<TValue>)option).SetTryParseValueCallback(callback);
        return option;
    }

    public static IScalarNamedOption<TValue> WithTryParseValueCallback<TValue>(
        this IScalarNamedOption<TValue> option, TryParse<TValue> callback)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption<TValue>)option).SetTryParseValueCallback(callback);
        return option;
    }

    public static ISequentialValueOption<TValue> WithTryParseValueCallback<TValue>(
        this ISequentialValueOption<TValue> option, TryParse<TValue> callback)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption<TValue>)option).SetTryParseValueCallback(callback);
        return option;
    }

    public static IScalarValueOption<TValue> WithTryParseValueCallback<TValue>(
        this IScalarValueOption<TValue> option, TryParse<TValue> callback)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption<TValue>)option).SetTryParseValueCallback(callback);
        return option;
    }

    public static ISequentialNamedOption<TValue> WithTryParseValuesCallback<TValue>(
        this ISequentialNamedOption<TValue> option, Func<List<string>, List<TValue>> callback)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption<TValue>)option).SetTryParseValuesCallback(callback);
        return option;
    }

    public static IScalarNamedOption<TValue> WithTryParseValuesCallback<TValue>(
        this IScalarNamedOption<TValue> option, Func<List<string>, List<TValue>> callback)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption<TValue>)option).SetTryParseValuesCallback(callback);
        return option;
    }

    public static ISequentialValueOption<TValue> WithTryParseValuesCallback<TValue>(
        this ISequentialValueOption<TValue> option, Func<List<string>, List<TValue>> callback)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption<TValue>)option).SetTryParseValuesCallback(callback);
        return option;
    }

    public static IScalarValueOption<TValue> WithTryParseValuesCallback<TValue>(
        this IScalarValueOption<TValue> option, Func<List<string>, List<TValue>> callback)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((BaseValueOption<TValue>)option).SetTryParseValuesCallback(callback);
        return option;
    }
    #endregion
}
