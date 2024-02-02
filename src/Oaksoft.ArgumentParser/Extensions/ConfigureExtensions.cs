using System;
using System.Collections.Generic;
using Oaksoft.ArgumentParser.Callbacks;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Options;

namespace Oaksoft.ArgumentParser.Extensions;

/// <summary>
/// Option configuration and registration extensions.
/// </summary>
public static partial class OptionExtensions
{
    #region Option Name Configuration
    /// <summary>
    /// Configures name of the ICounterOption.
    /// </summary>
    public static ICounterOption WithName(this ICounterOption option, string name)
    {
        ((CounterOption)option).SetName(name);
        return option;
    }

    /// <summary>
    /// Configures name of the IScalarNamedOption.
    /// </summary>
    public static IScalarNamedOption<TValue> WithName<TValue>(this IScalarNamedOption<TValue> option, string name)
        where TValue : IComparable
    {
        ((ScalarNamedOption<TValue>)option).SetName(name);
        return option;
    }

    /// <summary>
    /// Configures name of the ISequentialNamedOption.
    /// </summary>
    public static ISequentialNamedOption<TValue> WithName<TValue>(this ISequentialNamedOption<TValue> option, string name)
        where TValue : IComparable
    {
        ((SequentialNamedOption<TValue>)option).SetName(name);
        return option;
    }

    /// <summary>
    /// Configures name of the IScalarValueOption.
    /// </summary>
    public static IScalarValueOption<TValue> WithName<TValue>(this IScalarValueOption<TValue> option, string name)
        where TValue : IComparable
    {
        ((ScalarValueOption<TValue>)option).SetName(name);
        return option;
    }

    /// <summary>
    /// Configures name of the ISequentialValueOption.
    /// </summary>
    public static ISequentialValueOption<TValue> WithName<TValue>(this ISequentialValueOption<TValue> option, string name)
        where TValue : IComparable
    {
        ((SequentialValueOption<TValue>)option).SetName(name);
        return option;
    }
    #endregion

    #region Option Hidden Configuration
    /// <summary>
    /// Configures Hidden property of the ICounterOption.
    /// </summary>
    public static ICounterOption WithHidden(this ICounterOption option, bool hidden)
    {
        ((CounterOption)option).SetHidden(hidden);
        return option;
    }

    /// <summary>
    /// Configures Hidden property of the IScalarNamedOption.
    /// </summary>
    public static IScalarNamedOption<TValue> WithHidden<TValue>(this IScalarNamedOption<TValue> option, bool hidden)
        where TValue : IComparable
    {
        ((ScalarNamedOption<TValue>)option).SetHidden(hidden);
        return option;
    }

    /// <summary>
    /// Configures Hidden property of the ISequentialNamedOption.
    /// </summary>
    public static ISequentialNamedOption<TValue> WithHidden<TValue>(this ISequentialNamedOption<TValue> option, bool hidden)
        where TValue : IComparable
    {
        ((SequentialNamedOption<TValue>)option).SetHidden(hidden);
        return option;
    }

    /// <summary>
    /// Configures Hidden property of the IScalarValueOption.
    /// </summary>v
    public static IScalarValueOption<TValue> WithHidden<TValue>(this IScalarValueOption<TValue> option, bool hidden)
        where TValue : IComparable
    {
        ((ScalarValueOption<TValue>)option).SetHidden(hidden);
        return option;
    }

    /// <summary>
    /// Configures Hidden property of the ISequentialValueOption.
    /// </summary>
    public static ISequentialValueOption<TValue> WithHidden<TValue>(this ISequentialValueOption<TValue> option, bool hidden)
        where TValue : IComparable
    {
        ((SequentialValueOption<TValue>)option).SetHidden(hidden);
        return option;
    }
    #endregion

    #region Option Usage Configuration
    /// <summary>
    /// Configures Usage text of the ICounterOption.
    /// </summary>
    public static ICounterOption WithUsage(this ICounterOption option, string usage)
    {
        ((CounterOption)option).SetUsage(usage);
        return option;
    }

    /// <summary>
    /// Configures Usage text of the IScalarNamedOption.
    /// </summary>
    public static IScalarNamedOption<TValue> WithUsage<TValue>(this IScalarNamedOption<TValue> option, string usage)
        where TValue : IComparable
    {
        ((ScalarNamedOption<TValue>)option).SetUsage(usage);
        return option;
    }

    /// <summary>
    /// Configures Usage text of the ISequentialNamedOption.
    /// </summary>
    public static ISequentialNamedOption<TValue> WithUsage<TValue>(this ISequentialNamedOption<TValue> option, string usage)
        where TValue : IComparable
    {
        ((SequentialNamedOption<TValue>)option).SetUsage(usage);
        return option;
    }
    #endregion

    #region Option Description Configuration
    /// <summary>
    /// Configures Description of the ICounterOption.
    /// </summary>
    public static ICounterOption WithDescription(this ICounterOption option, string description)
    {
        ((CounterOption)option).SetDescription(description);
        return option;
    }

    /// <summary>
    /// Configures Description of the IScalarNamedOption.
    /// </summary>
    public static IScalarNamedOption<TValue> WithDescription<TValue>(this IScalarNamedOption<TValue> option, string description)
        where TValue : IComparable
    {
        ((ScalarNamedOption<TValue>)option).SetDescription(description);
        return option;
    }

    /// <summary>
    /// Configures Description of the ISequentialNamedOption.
    /// </summary>
    public static ISequentialNamedOption<TValue> WithDescription<TValue>(this ISequentialNamedOption<TValue> option, string description)
        where TValue : IComparable
    {
        ((SequentialNamedOption<TValue>)option).SetDescription(description);
        return option;
    }

    /// <summary>
    /// Configures Description of the IScalarValueOption.
    /// </summary>
    public static IScalarValueOption<TValue> WithDescription<TValue>(this IScalarValueOption<TValue> option, string description)
        where TValue : IComparable
    {
        ((ScalarValueOption<TValue>)option).SetDescription(description);
        return option;
    }

    /// <summary>
    /// Configures Description of the ISequentialValueOption.
    /// </summary>
    public static ISequentialValueOption<TValue> WithDescription<TValue>(this ISequentialValueOption<TValue> option, string description)
        where TValue : IComparable
    {
        ((SequentialValueOption<TValue>)option).SetDescription(description);
        return option;
    }
    #endregion

    #region Option Alias Configuration
    /// <summary>
    /// Configures aliases of the ICounterOption.
    /// </summary>
    public static ICounterOption AddAliases(this ICounterOption option, params string[] aliases)
    {
        ((CounterOption)option).AddAliases(aliases);
        return option;
    }

    /// <summary>
    /// Configures aliases of the IScalarNamedOption.
    /// </summary>
    public static IScalarNamedOption<TValue> AddAliases<TValue>(this IScalarNamedOption<TValue> option, params string[] aliases)
        where TValue : IComparable
    {
        ((ScalarNamedOption<TValue>)option).AddAliases(aliases);
        return option;
    }

    /// <summary>
    /// Configures aliases of the ISequentialNamedOption.
    /// </summary>
    public static ISequentialNamedOption<TValue> AddAliases<TValue>(this ISequentialNamedOption<TValue> option, params string[] aliases)
        where TValue : IComparable
    {
        ((SequentialNamedOption<TValue>)option).AddAliases(aliases);
        return option;
    }
    #endregion

    #region Value Arity Configuration
    /// <summary>
    /// Configures ValueArity of the IScalarNamedOption.
    /// </summary>
    public static IScalarNamedOption<TValue> WithValueArity<TValue>(this IScalarNamedOption<TValue> option, ArityType valueArity)
        where TValue : IComparable
    {
        ((ScalarNamedOption<TValue>)option).SetValueArity(valueArity);
        return option;
    }

    /// <summary>
    /// Configures ValueArity of the IScalarNamedOption.
    /// </summary>
    public static IScalarNamedOption<TValue> WithValueArity<TValue>(this IScalarNamedOption<TValue> option, int requiredValueCount, int maximumValueCount)
        where TValue : IComparable
    {
        ((ScalarNamedOption<TValue>)option).SetValueArity(requiredValueCount, maximumValueCount);
        return option;
    }

    /// <summary>
    /// Configures ValueArity of the ISequentialNamedOption.
    /// </summary>
    public static ISequentialNamedOption<TValue> WithValueArity<TValue>(this ISequentialNamedOption<TValue> option, ArityType valueArity)
        where TValue : IComparable
    {
        ((SequentialNamedOption<TValue>)option).SetValueArity(valueArity);
        return option;
    }

    /// <summary>
    /// Configures ValueArity of the ISequentialNamedOption.
    /// </summary>
    public static ISequentialNamedOption<TValue> WithValueArity<TValue>(this ISequentialNamedOption<TValue> option, int requiredValueCount, int maximumValueCount)
        where TValue : IComparable
    {
        ((SequentialNamedOption<TValue>)option).SetValueArity(requiredValueCount, maximumValueCount);
        return option;
    }

    /// <summary>
    /// Configures ValueArity of the IScalarValueOption.
    /// </summary>
    public static IScalarValueOption<TValue> WithValueArity<TValue>(this IScalarValueOption<TValue> option, ArityType valueArity)
        where TValue : IComparable
    {
        ((ScalarValueOption<TValue>)option).SetValueArity(valueArity);
        return option;
    }

    /// <summary>
    /// Configures ValueArity of the IScalarValueOption.
    /// </summary>
    public static IScalarValueOption<TValue> WithValueArity<TValue>(this IScalarValueOption<TValue> option, int requiredValueCount, int maximumValueCount)
        where TValue : IComparable
    {
        ((ScalarValueOption<TValue>)option).SetValueArity(requiredValueCount, maximumValueCount);
        return option;
    }

    /// <summary>
    /// Configures ValueArity of the ISequentialValueOption.
    /// </summary>
    public static ISequentialValueOption<TValue> WithValueArity<TValue>(this ISequentialValueOption<TValue> option, ArityType valueArity)
        where TValue : IComparable
    {
        ((SequentialValueOption<TValue>)option).SetValueArity(valueArity);
        return option;
    }

    /// <summary>
    /// Configures ValueArity of the ISequentialValueOption.
    /// </summary>
    public static ISequentialValueOption<TValue> WithValueArity<TValue>(this ISequentialValueOption<TValue> option, int requiredValueCount, int maximumValueCount)
        where TValue : IComparable
    {
        ((SequentialValueOption<TValue>)option).SetValueArity(requiredValueCount, maximumValueCount);
        return option;
    }
    #endregion

    #region Option Arity Configuration
    /// <summary>
    /// Configures OptionArity of the ICounterOption.
    /// </summary>
    public static ICounterOption WithOptionArity(this ICounterOption option, ArityType optionArity)
    {
        ((CounterOption)option).SetOptionArity(optionArity);
        return option;
    }

    /// <summary>
    /// Configures OptionArity of the ICounterOption.
    /// </summary>
    public static ICounterOption WithOptionArity(this ICounterOption option, int requiredOptionCount, int maximumOptionCount)
    {
        ((CounterOption)option).SetOptionArity(requiredOptionCount, maximumOptionCount);
        return option;
    }

    /// <summary>
    /// Configures OptionArity of the IScalarNamedOption.
    /// </summary>
    public static IScalarNamedOption<TValue> WithOptionArity<TValue>(this IScalarNamedOption<TValue> option, ArityType optionArity)
        where TValue : IComparable
    {
        ((ScalarNamedOption<TValue>)option).SetOptionArity(optionArity);
        return option;
    }

    /// <summary>
    /// Configures OptionArity of the IScalarNamedOption.
    /// </summary>
    public static IScalarNamedOption<TValue> WithOptionArity<TValue>(this IScalarNamedOption<TValue> option, int requiredOptionCount, int maximumOptionCount)
        where TValue : IComparable
    {
        ((ScalarNamedOption<TValue>)option).SetOptionArity(requiredOptionCount, maximumOptionCount);
        return option;
    }

    /// <summary>
    /// Configures OptionArity of the ISequentialNamedOption.
    /// </summary>
    public static ISequentialNamedOption<TValue> WithOptionArity<TValue>(this ISequentialNamedOption<TValue> option, ArityType optionArity)
        where TValue : IComparable
    {
        ((SequentialNamedOption<TValue>)option).SetOptionArity(optionArity);
        return option;
    }

    /// <summary>
    /// Configures OptionArity of the ISequentialNamedOption.
    /// </summary>
    public static ISequentialNamedOption<TValue> WithOptionArity<TValue>(this ISequentialNamedOption<TValue> option, int requiredOptionCount, int maximumOptionCount)
        where TValue : IComparable
    {
        ((SequentialNamedOption<TValue>)option).SetOptionArity(requiredOptionCount, maximumOptionCount);
        return option;
    }
    #endregion

    #region Default Value Configuration
    /// <summary>
    /// Configures DefaultValue of the IScalarNamedOption.
    /// </summary>
    public static IScalarNamedOption<TValue> WithDefaultValue<TValue>(this IScalarNamedOption<TValue> option, TValue defaultValue)
        where TValue : IComparable
    {
        ((ScalarNamedOption<TValue>)option).SetDefaultValue(defaultValue);
        return option;
    }
    #endregion

    #region Sequential Value Configuration
    /// <summary>
    /// Configures EnableSequentialValues property of the ISequentialNamedOption.
    /// </summary>
    public static ISequentialNamedOption<TValue> WithEnableSequentialValues<TValue>(this ISequentialNamedOption<TValue> option, bool enabled)
        where TValue : IComparable
    {
        ((SequentialNamedOption<TValue>)option).SetEnableSequentialValues(enabled);
        return option;
    }

    /// <summary>
    /// Configures EnableValueTokenSplitting property of the ISequentialNamedOption.
    /// </summary>
    public static ISequentialNamedOption<TValue> WithEnableValueTokenSplitting<TValue>(this ISequentialNamedOption<TValue> option, bool enabled)
        where TValue : IComparable
    {
        ((SequentialNamedOption<TValue>)option).SetEnableValueTokenSplitting(enabled);
        return option;
    }

    /// <summary>
    /// Configures EnableValueTokenSplitting property of the ISequentialValueOption.
    /// </summary>
    public static ISequentialValueOption<TValue> WithEnableValueTokenSplitting<TValue>(this ISequentialValueOption<TValue> option, bool enabled)
        where TValue : IComparable
    {
        ((SequentialValueOption<TValue>)option).SetEnableValueTokenSplitting(enabled);
        return option;
    }
    #endregion

    #region Allowed Values Configuration
    /// <summary>
    /// Configures AllowedValues of the IScalarNamedOption.
    /// </summary>
    public static IScalarNamedOption<TValue> WithAllowedValues<TValue>(this IScalarNamedOption<TValue> option, params TValue[] allowedValues)
        where TValue : IComparable
    {
        ((ScalarNamedOption<TValue>)option).SetAllowedValues(allowedValues);
        return option;
    }

    /// <summary>
    /// Configures AllowedValues of the ISequentialNamedOption.
    /// </summary>
    public static ISequentialNamedOption<TValue> WithAllowedValues<TValue>(this ISequentialNamedOption<TValue> option, params TValue[] allowedValues)
        where TValue : IComparable
    {
        ((SequentialNamedOption<TValue>)option).SetAllowedValues(allowedValues);
        return option;
    }

    /// <summary>
    /// Configures AllowedValues of the IScalarValueOption.
    /// </summary>
    public static IScalarValueOption<TValue> WithAllowedValues<TValue>(this IScalarValueOption<TValue> option, params TValue[] allowedValues)
        where TValue : IComparable
    {
        ((ScalarValueOption<TValue>)option).SetAllowedValues(allowedValues);
        return option;
    }

    /// <summary>
    /// Configures AllowedValues of the ISequentialValueOption.
    /// </summary>
    public static ISequentialValueOption<TValue> WithAllowedValues<TValue>(this ISequentialValueOption<TValue> option, params TValue[] allowedValues)
        where TValue : IComparable
    {
        ((SequentialValueOption<TValue>)option).SetAllowedValues(allowedValues);
        return option;
    }
    #endregion

    #region Predicate Configuration
    /// <summary>
    /// Configures Predicate delegate of the IScalarNamedOption.
    /// </summary>
    public static IScalarNamedOption<TValue> AddPredicate<TValue>(this IScalarNamedOption<TValue> option, Predicate<TValue> predicate)
        where TValue : IComparable
    {
        ((ScalarNamedOption<TValue>)option).AddPredicate(predicate);
        return option;
    }

    /// <summary>
    /// Configures Predicate delegate of the ISequentialNamedOption.
    /// </summary>
    public static ISequentialNamedOption<TValue> AddPredicate<TValue>(this ISequentialNamedOption<TValue> option, Predicate<TValue> predicate)
        where TValue : IComparable
    {
        ((SequentialNamedOption<TValue>)option).AddPredicate(predicate);
        return option;
    }

    /// <summary>
    /// Configures Predicate delegate of the IScalarValueOption.
    /// </summary>
    public static IScalarValueOption<TValue> AddPredicate<TValue>(this IScalarValueOption<TValue> option, Predicate<TValue> predicate)
        where TValue : IComparable
    {
        ((ScalarValueOption<TValue>)option).AddPredicate(predicate);
        return option;
    }

    /// <summary>
    /// Configures Predicate delegate of the ISequentialValueOption.
    /// </summary>
    public static ISequentialValueOption<TValue> AddPredicate<TValue>(this ISequentialValueOption<TValue> option, Predicate<TValue> predicate)
        where TValue : IComparable
    {
        ((SequentialValueOption<TValue>)option).AddPredicate(predicate);
        return option;
    }

    /// <summary>
    /// Configures ListPredicate delegate of the ISequentialNamedOption.
    /// </summary>
    public static ISequentialNamedOption<TValue> AddListPredicate<TValue>(this ISequentialNamedOption<TValue> option, Predicate<List<TValue>> predicate)
        where TValue : IComparable
    {
        ((SequentialNamedOption<TValue>)option).AddListPredicate(predicate);
        return option;
    }

    /// <summary>
    /// Configures ListPredicate delegate of the ISequentialValueOption.
    /// </summary>
    public static ISequentialValueOption<TValue> AddListPredicate<TValue>(this ISequentialValueOption<TValue> option, Predicate<List<TValue>> predicate)
        where TValue : IComparable
    {
        ((SequentialValueOption<TValue>)option).AddListPredicate(predicate);
        return option;
    }
    #endregion

    #region Parsing Callbacks Configuration
    /// <summary>
    /// Configures TryParseCallback delegate of the IScalarNamedOption.
    /// </summary>
    public static IScalarNamedOption<TValue> WithTryParseCallback<TValue>(this IScalarNamedOption<TValue> option, TryParse<TValue> callback)
        where TValue : IComparable
    {
        ((ScalarNamedOption<TValue>)option).SetTryParseCallback(callback);
        return option;
    }

    /// <summary>
    /// Configures TryParseCallback delegate of the ISequentialNamedOption.
    /// </summary>
    public static ISequentialNamedOption<TValue> WithTryParseCallback<TValue>(this ISequentialNamedOption<TValue> option, TryParse<TValue> callback)
        where TValue : IComparable
    {
        ((SequentialNamedOption<TValue>)option).SetTryParseCallback(callback);
        return option;
    }

    /// <summary>
    /// Configures TryParseCallback delegate of the IScalarValueOption.
    /// </summary>
    public static IScalarValueOption<TValue> WithTryParseCallback<TValue>(this IScalarValueOption<TValue> option, TryParse<TValue> callback)
        where TValue : IComparable
    {
        ((ScalarValueOption<TValue>)option).SetTryParseCallback(callback);
        return option;
    }

    /// <summary>
    /// Configures TryParseCallback delegate of the ISequentialValueOption.
    /// </summary>
    public static ISequentialValueOption<TValue> WithTryParseCallback<TValue>(this ISequentialValueOption<TValue> option, TryParse<TValue> callback)
        where TValue : IComparable
    {
        ((SequentialValueOption<TValue>)option).SetTryParseCallback(callback);
        return option;
    }
    #endregion
}
