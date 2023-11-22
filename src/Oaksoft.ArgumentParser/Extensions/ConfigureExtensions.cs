using System;
using System.Collections.Generic;
using Oaksoft.ArgumentParser.Callbacks;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Options;

namespace Oaksoft.ArgumentParser.Extensions;

public static partial class OptionExtensions
{
    #region Option Name Configuration
    public static ISwitchOption WithName(this ISwitchOption option, string name)
    {
        ((SwitchOption)option).SetName(name);
        return option;
    }

    public static IScalarNamedOption<TValue> WithName<TValue>(this IScalarNamedOption<TValue> option, string name)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarNamedOption<TValue>)option).SetName(name);
        return option;
    }

    public static ISequentialNamedOption<TValue> WithName<TValue>(this ISequentialNamedOption<TValue> option, string name)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((SequentialNamedOption<TValue>)option).SetName(name);
        return option;
    }

    public static IScalarValueOption<TValue> WithName<TValue>(this IScalarValueOption<TValue> option, string name)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarValueOption<TValue>)option).SetName(name);
        return option;
    }

    public static ISequentialValueOption<TValue> WithName<TValue>(this ISequentialValueOption<TValue> option, string name)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((SequentialValueOption<TValue>)option).SetName(name);
        return option;
    }
    #endregion

    #region Option Usage Configuration
    public static ISwitchOption WithUsage(this ISwitchOption option, string usage)
    {
        ((SwitchOption)option).SetUsage(usage);
        return option;
    }

    public static IScalarNamedOption<TValue> WithUsage<TValue>(this IScalarNamedOption<TValue> option, string usage)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarNamedOption<TValue>)option).SetUsage(usage);
        return option;
    }

    public static ISequentialNamedOption<TValue> WithUsage<TValue>(this ISequentialNamedOption<TValue> option, string usage)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((SequentialNamedOption<TValue>)option).SetUsage(usage);
        return option;
    }

    public static IScalarValueOption<TValue> WithUsage<TValue>(this IScalarValueOption<TValue> option, string usage)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarValueOption<TValue>)option).SetUsage(usage);
        return option;
    }

    public static ISequentialValueOption<TValue> WithUsage<TValue>(this ISequentialValueOption<TValue> option, string usage)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((SequentialValueOption<TValue>)option).SetUsage(usage);
        return option;
    }
    #endregion

    #region Option Description Configuration
    public static ISwitchOption WithDescription(this ISwitchOption option, string description)
    {
        ((SwitchOption)option).SetDescription(description);
        return option;
    }

    public static IScalarNamedOption<TValue> WithDescription<TValue>(this IScalarNamedOption<TValue> option, string description)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarNamedOption<TValue>)option).SetDescription(description);
        return option;
    }

    public static ISequentialNamedOption<TValue> WithDescription<TValue>(this ISequentialNamedOption<TValue> option, string description)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((SequentialNamedOption<TValue>)option).SetDescription(description);
        return option;
    }

    public static IScalarValueOption<TValue> WithDescription<TValue>(this IScalarValueOption<TValue> option, string description)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarValueOption<TValue>)option).SetDescription(description);
        return option;
    }

    public static ISequentialValueOption<TValue> WithDescription<TValue>(this ISequentialValueOption<TValue> option, string description)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((SequentialValueOption<TValue>)option).SetDescription(description);
        return option;
    }
    #endregion

    #region Option Alias Configuration
    public static ISwitchOption AddAliases(this ISwitchOption option, params string[] aliases)
    {
        ((SwitchOption)option).AddAliases(aliases);
        return option;
    }

    public static IScalarNamedOption<TValue> AddAliases<TValue>(this IScalarNamedOption<TValue> option, params string[] aliases)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarNamedOption<TValue>)option).AddAliases(aliases);
        return option;
    }

    public static ISequentialNamedOption<TValue> AddAliases<TValue>(this ISequentialNamedOption<TValue> option, params string[] aliases)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((SequentialNamedOption<TValue>)option).AddAliases(aliases);
        return option;
    }
    #endregion

    #region Value Arity Configuration
    public static ISwitchOption WithValueArity(this ISwitchOption option, ArityType optionArity)
    {
        ((SwitchOption)option).SetValueArity(optionArity);
        return option;
    }

    public static ISwitchOption WithValueArity(this ISwitchOption option, int requiredOptionCount, int maximumOptionCount)
    {
        ((SwitchOption)option).SetValueArity(requiredOptionCount, maximumOptionCount);
        return option;
    }

    public static IScalarNamedOption<TValue> WithValueArity<TValue>(this IScalarNamedOption<TValue> option, ArityType valueArity)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarNamedOption<TValue>)option).SetValueArity(valueArity);
        return option;
    }

    public static IScalarNamedOption<TValue> WithValueArity<TValue>(this IScalarNamedOption<TValue> option, int requiredValueCount, int maximumValueCount)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarNamedOption<TValue>)option).SetValueArity(requiredValueCount, maximumValueCount);
        return option;
    }

    public static ISequentialNamedOption<TValue> WithValueArity<TValue>(this ISequentialNamedOption<TValue> option, ArityType valueArity)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((SequentialNamedOption<TValue>)option).SetValueArity(valueArity);
        return option;
    }

    public static ISequentialNamedOption<TValue> WithValueArity<TValue>(this ISequentialNamedOption<TValue> option, int requiredValueCount, int maximumValueCount)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((SequentialNamedOption<TValue>)option).SetValueArity(requiredValueCount, maximumValueCount);
        return option;
    }

    public static IScalarValueOption<TValue> WithValueArity<TValue>(this IScalarValueOption<TValue> option, ArityType valueArity)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarValueOption<TValue>)option).SetValueArity(valueArity);
        return option;
    }

    public static IScalarValueOption<TValue> WithValueArity<TValue>(this IScalarValueOption<TValue> option, int requiredValueCount, int maximumValueCount)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarValueOption<TValue>)option).SetValueArity(requiredValueCount, maximumValueCount);
        return option;
    }

    public static ISequentialValueOption<TValue> WithValueArity<TValue>(this ISequentialValueOption<TValue> option, ArityType valueArity)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((SequentialValueOption<TValue>)option).SetValueArity(valueArity);
        return option;
    }

    public static ISequentialValueOption<TValue> WithValueArity<TValue>(this ISequentialValueOption<TValue> option, int requiredValueCount, int maximumValueCount)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((SequentialValueOption<TValue>)option).SetValueArity(requiredValueCount, maximumValueCount);
        return option;
    }
    #endregion

    #region Option Arity Configuration
    public static ISwitchOption WithOptionArity(this ISwitchOption option, ArityType optionArity)
    {
        ((SwitchOption)option).SetOptionArity(optionArity);
        return option;
    }
    
    public static ISwitchOption WithOptionArity(this ISwitchOption option, int requiredOptionCount, int maximumOptionCount)
    {
        ((SwitchOption)option).SetOptionArity(requiredOptionCount, maximumOptionCount);
        return option;
    }

    public static IScalarNamedOption<TValue> WithOptionArity<TValue>(this IScalarNamedOption<TValue> option, ArityType optionArity)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarNamedOption<TValue>)option).SetOptionArity(optionArity);
        return option;
    }

    public static IScalarNamedOption<TValue> WithOptionArity<TValue>(this IScalarNamedOption<TValue> option, int requiredOptionCount, int maximumOptionCount)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarNamedOption<TValue>)option).SetOptionArity(requiredOptionCount, maximumOptionCount);
        return option;
    }

    public static ISequentialNamedOption<TValue> WithOptionArity<TValue>(this ISequentialNamedOption<TValue> option, ArityType optionArity)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((SequentialNamedOption<TValue>)option).SetOptionArity(optionArity);
        return option;
    }

    public static ISequentialNamedOption<TValue> WithOptionArity<TValue>(this ISequentialNamedOption<TValue> option, int requiredOptionCount, int maximumOptionCount)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((SequentialNamedOption<TValue>)option).SetOptionArity(requiredOptionCount, maximumOptionCount);
        return option;
    }
    #endregion

    #region Default Value Configuration
    public static ISwitchOption WithDefaultValue(this ISwitchOption option, bool defaultValue)
    {
        ((SwitchOption)option).SetDefaultValue(defaultValue);
        return option;
    }

    public static IScalarNamedOption<TValue> WithDefaultValue<TValue>(this IScalarNamedOption<TValue> option, TValue defaultValue)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarNamedOption<TValue>)option).SetDefaultValue(defaultValue);
        return option;
    }
    #endregion

    #region Sequential Value Configuration
    public static ISequentialNamedOption<TValue> WithAllowSequentialValues<TValue>(this ISequentialNamedOption<TValue> option, bool enabled)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((SequentialNamedOption<TValue>)option).SetAllowSequentialValues(enabled);
        return option;
    }

    public static ISequentialNamedOption<TValue> WithEnableValueTokenSplitting<TValue>(this ISequentialNamedOption<TValue> option, bool enabled)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((SequentialNamedOption<TValue>)option).SetEnableValueTokenSplitting(enabled);
        return option;
    }

    public static ISequentialValueOption<TValue> WithEnableValueTokenSplitting<TValue>(this ISequentialValueOption<TValue> option, bool enabled)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((SequentialValueOption<TValue>)option).SetEnableValueTokenSplitting(enabled);
        return option;
    }
    #endregion

    #region Allowed Values Configuration
    public static IScalarNamedOption<TValue> WithAllowedValues<TValue>(this IScalarNamedOption<TValue> option, params TValue[] allowedValues)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarNamedOption<TValue>)option).SetAllowedValues(allowedValues);
        return option;
    }

    public static ISequentialNamedOption<TValue> WithAllowedValues<TValue>(this ISequentialNamedOption<TValue> option, params TValue[] allowedValues)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((SequentialNamedOption<TValue>)option).SetAllowedValues(allowedValues);
        return option;
    }

    public static IScalarValueOption<TValue> WithAllowedValues<TValue>(this IScalarValueOption<TValue> option, params TValue[] allowedValues)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarValueOption<TValue>)option).SetAllowedValues(allowedValues);
        return option;
    }

    public static ISequentialValueOption<TValue> WithAllowedValues<TValue>(this ISequentialValueOption<TValue> option, params TValue[] allowedValues)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((SequentialValueOption<TValue>)option).SetAllowedValues(allowedValues);
        return option;
    }
    #endregion

    #region Predicate Configuration
    public static IScalarNamedOption<TValue> AddPredicate<TValue>(this IScalarNamedOption<TValue> option, Predicate<TValue> predicate)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarNamedOption<TValue>)option).AddPredicate(predicate);
        return option;
    }

    public static ISequentialNamedOption<TValue> AddPredicate<TValue>(this ISequentialNamedOption<TValue> option, Predicate<TValue> predicate)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((SequentialNamedOption<TValue>)option).AddPredicate(predicate);
        return option;
    }

    public static IScalarValueOption<TValue> AddPredicate<TValue>(this IScalarValueOption<TValue> option, Predicate<TValue> predicate)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarValueOption<TValue>)option).AddPredicate(predicate);
        return option;
    }

    public static ISequentialValueOption<TValue> AddPredicate<TValue>(this ISequentialValueOption<TValue> option, Predicate<TValue> predicate)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((SequentialValueOption<TValue>)option).AddPredicate(predicate);
        return option;
    }
    #endregion

    #region Parsing Callbacks Configuration
    public static IScalarNamedOption<TValue> WithParsingCallbacks<TValue>(this IScalarNamedOption<TValue> option, IParsingCallbacks<TValue> optionCallbacks)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarNamedOption<TValue>)option).SetParsingCallbacks(optionCallbacks);
        return option;
    }

    public static ISequentialNamedOption<TValue> WithParsingCallbacks<TValue>(this ISequentialNamedOption<TValue> option, IParsingCallbacks<TValue> optionCallbacks)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((SequentialNamedOption<TValue>)option).SetParsingCallbacks(optionCallbacks);
        return option;
    }

    public static IScalarValueOption<TValue> WithParsingCallbacks<TValue>(this IScalarValueOption<TValue> option, IParsingCallbacks<TValue> optionCallbacks)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarValueOption<TValue>)option).SetParsingCallbacks(optionCallbacks);
        return option;
    }
    
    public static ISequentialValueOption<TValue> WithParsingCallbacks<TValue>(this ISequentialValueOption<TValue> option, IParsingCallbacks<TValue> optionCallbacks)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((SequentialValueOption<TValue>)option).SetParsingCallbacks(optionCallbacks);
        return option;
    }
    
    public static IScalarNamedOption<TValue> WithTryParseValueCallback<TValue>(this IScalarNamedOption<TValue> option, TryParse<TValue> callback)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarNamedOption<TValue>)option).SetTryParseValueCallback(callback);
        return option;
    }

    public static ISequentialNamedOption<TValue> WithTryParseValueCallback<TValue>(this ISequentialNamedOption<TValue> option, TryParse<TValue> callback)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((SequentialNamedOption<TValue>)option).SetTryParseValueCallback(callback);
        return option;
    }

    public static IScalarValueOption<TValue> WithTryParseValueCallback<TValue>(this IScalarValueOption<TValue> option, TryParse<TValue> callback)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarValueOption<TValue>)option).SetTryParseValueCallback(callback);
        return option;
    }

    public static ISequentialValueOption<TValue> WithTryParseValueCallback<TValue>(this ISequentialValueOption<TValue> option, TryParse<TValue> callback)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((SequentialValueOption<TValue>)option).SetTryParseValueCallback(callback);
        return option;
    }

    public static IScalarNamedOption<TValue> WithTryParseValuesCallback<TValue>(this IScalarNamedOption<TValue> option, Func<List<string>, List<TValue>> callback)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarNamedOption<TValue>)option).SetTryParseValuesCallback(callback);
        return option;
    }

    public static ISequentialNamedOption<TValue> WithTryParseValuesCallback<TValue>(this ISequentialNamedOption<TValue> option, Func<List<string>, List<TValue>> callback)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((SequentialNamedOption<TValue>)option).SetTryParseValuesCallback(callback);
        return option;
    }

    public static IScalarValueOption<TValue> WithTryParseValuesCallback<TValue>(this IScalarValueOption<TValue> option, Func<List<string>, List<TValue>> callback)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((ScalarValueOption<TValue>)option).SetTryParseValuesCallback(callback);
        return option;
    }

    public static ISequentialValueOption<TValue> WithTryParseValuesCallback<TValue>(this ISequentialValueOption<TValue> option, Func<List<string>, List<TValue>> callback)
        where TValue : IComparable, IEquatable<TValue>
    {
        ((SequentialValueOption<TValue>)option).SetTryParseValuesCallback(callback);
        return option;
    }
    #endregion
}
