using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Base;

internal static class OptionRegistrar
{
    public static ISwitchOption RegisterSwitchOption<TSource, TValue>(
        this TSource source, Expression<Func<TSource, TValue>> keyPropExpr, bool mandatoryOption)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        var optionLimits = (mandatoryOption ? ArityType.ExactlyOne : ArityType.ZeroOrOne).GetLimits();
        var option = new SwitchOption(optionLimits.Min, optionLimits.Max);

        source.RegisterOptionProperty(option, keyProperty);

        return option;
    }

    public static ISwitchOption RegisterSwitchOption<TSource, TValue>(
        this TSource source, Expression<Func<TSource, TValue>> keyPropExpr, ArityType optionArity)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        var optionLimits = optionArity.GetLimits();
        var option = new SwitchOption(optionLimits.Min, optionLimits.Max);

        source.RegisterOptionProperty(option, keyProperty);

        return option;
    }

    public static IScalarOption<TValue> RegisterScalarOption<TSource, TValue>(
        this TSource source, PropertyInfo keyProperty, 
        bool mustHaveOneValue, bool mandatoryOption)
        where TSource : BaseApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var optionLimits = (mandatoryOption ? ArityType.ExactlyOne : ArityType.ZeroOrOne).GetLimits();
        var valueLimits = (mustHaveOneValue ? ArityType.ExactlyOne : ArityType.ZeroOrOne).GetLimits();
        var option = new ScalarOption<TValue>(optionLimits.Min, optionLimits.Max, valueLimits.Min, valueLimits.Max);

        source.RegisterOptionProperty(option, keyProperty);

        return option;
    }

    public static IScalarOption<TValue> RegisterScalarOption<TSource, TValue>(
        this TSource source, PropertyInfo keyProperty, 
        bool enableValueTokenSplitting, bool allowSequentialValues,
        ArityType valueArity, ArityType optionArity)
        where TSource : BaseApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var optionLimits = optionArity.GetLimits();
        var valueLimits = valueArity.GetLimits();
        var option = new ScalarOption<TValue>(optionLimits.Min, optionLimits.Max, valueLimits.Min, valueLimits.Max)
        {
            EnableValueTokenSplitting = enableValueTokenSplitting,
            AllowSequentialValues = allowSequentialValues
        };

        source.RegisterOptionProperty(option, keyProperty);

        return option;
    }

    public static IScalarOption<TValue> RegisterScalarOption<TSource, TValue>(
        this TSource source, PropertyInfo keyProperty, PropertyInfo flagProperty,
        bool mustHaveOneValue, bool mandatoryOption)
        where TSource : BaseApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var optionLimits = (mandatoryOption ? ArityType.ExactlyOne : ArityType.ZeroOrOne).GetLimits();
        var valueLimits = (mustHaveOneValue ? ArityType.ExactlyOne : ArityType.ZeroOrOne).GetLimits();
        var option = new ScalarOption<TValue>(optionLimits.Min, optionLimits.Max, valueLimits.Min, valueLimits.Max);

        source.RegisterOptionProperty(option, keyProperty, flagProperty);

        return option;
    }

    public static IScalarOption<TValue> RegisterScalarOption<TSource, TValue>(
        this TSource source, PropertyInfo keyProperty, PropertyInfo countProperty,
        bool enableValueTokenSplitting, bool allowSequentialValues, 
        ArityType valueArity, ArityType optionArity)
        where TSource : BaseApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {

        var optionLimits = optionArity.GetLimits();
        var valueLimits = valueArity.GetLimits();
        var option = new ScalarOption<TValue>(optionLimits.Min, optionLimits.Max, valueLimits.Min, valueLimits.Max)
        {
            EnableValueTokenSplitting = enableValueTokenSplitting,
            AllowSequentialValues = allowSequentialValues
        };

        source.RegisterOptionProperty(option, keyProperty, countProperty);

        return option;
    }

    public static IValueOption<string> RegisterValueOption<TSource>(
        this TSource source, Expression<Func<TSource, string?>> keyPropExpr, bool mustHaveOneValue)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        var valueLimits = (mustHaveOneValue ? ArityType.ExactlyOne : ArityType.ZeroOrOne).GetLimits();
        var option = new ValueOption(valueLimits.Min, valueLimits.Max);

        source.RegisterOptionProperty(option, keyProperty);

        return option;
    }

    public static IValueOption<string> RegisterValueOption<TSource>(
        this TSource source, Expression<Func<TSource, IEnumerable<string>?>> keyPropExpr,
        bool enableValueTokenSplitting, ArityType valueArity)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        var valueLimits = valueArity.GetLimits();
        var option = new ValueOption(valueLimits.Min, valueLimits.Max)
        {
            EnableValueTokenSplitting = enableValueTokenSplitting
        };

        source.RegisterOptionProperty(option, keyProperty);

        return option;
    }

    public static IValueOption<string> RegisterValueOption<TSource>(
        this TSource source, 
        Expression<Func<TSource, string?>> keyPropExpr,
        Expression<Func<TSource, bool>> flagPropExpr, 
        bool mustHaveOneValue)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var flagProperty = source.ValidateExpression(flagPropExpr);

        var valueLimits = (mustHaveOneValue ? ArityType.ExactlyOne : ArityType.ZeroOrOne).GetLimits();
        var option = new ValueOption(valueLimits.Min, valueLimits.Max);

        source.RegisterOptionProperty(option, keyProperty, flagProperty);

        return option;
    }

    public static IValueOption<string> RegisterValueOption<TSource>(
        this TSource source,
        Expression<Func<TSource, IEnumerable<string>?>> keyPropExpr,
        Expression<Func<TSource, int>> countPropExpr,
        bool enableValueTokenSplitting, ArityType valueArity)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var countProperty = source.ValidateExpression(countPropExpr);

        var valueLimits = valueArity.GetLimits();
        var option = new ValueOption(valueLimits.Min, valueLimits.Max)
        {
            EnableValueTokenSplitting = enableValueTokenSplitting
        };

        source.RegisterOptionProperty(option, keyProperty, countProperty);

        return option;
    }

    public static PropertyInfo ValidateExpression<TSource, TValue>(
        this TSource source, Expression<Func<TSource, TValue>>? expression)
        where TSource : BaseApplicationOptions
    {
        if (expression?.Body == null)
            throw new ArgumentNullException(nameof(expression));

        if (expression.NodeType != ExpressionType.Lambda ||
            expression.Body is not MemberExpression member ||
            member.Member.MemberType != MemberTypes.Property)
            throw new ArgumentException("Invalid lambda expression, please select a property!");

        var properties = source.GetType().GetProperties();
        var property = properties.FirstOrDefault(p => p.Name == member.Member.Name);
        if (property == null)
            throw new ArgumentException("Unknown property, please select a property!");

        return property;
    }

    private static void RegisterOptionProperty<TSource>(
        this TSource source, BaseOption option, 
        PropertyInfo keyProperty, PropertyInfo? countProperty = null)
        where TSource : BaseApplicationOptions
    {
        var options = source.GetType().BaseType?
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
            .Select(f => f.GetValue(source))
            .OfType<Dictionary<string, IBaseOption>>()
            .FirstOrDefault();

        var propertyNames = options!.Values
            .Select(a => ((BaseOption)a).CountProperty?.Name)
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .ToList();

        propertyNames.AddRange(options.Keys);

        if (keyProperty.Name == countProperty?.Name)
            throw new ArgumentException($"Selected Key and Count property cannot be same. Properties: {keyProperty.Name}, {countProperty.Name}");

        if (propertyNames.Contains(keyProperty.Name))
            throw new ArgumentException($"Property '{keyProperty.Name}' already registered with an option!");

        if (countProperty != null && propertyNames.Contains(countProperty.Name))
            throw new ArgumentException($"Property '{countProperty.Name}' already registered with an option!");

        option.SetKeyProperty(keyProperty);
        option.SetCountProperty(countProperty);

        options[keyProperty.Name] = option;
    }

    public static (int Min, int Max) GetLimits(this ArityType arityType)
    {
        return arityType switch
        {
            ArityType.Zero => (0, 0),
            ArityType.ZeroOrOne => (0, 1),
            ArityType.ExactlyOne => (1, 1),
            ArityType.ZeroOrMore => (0, int.MaxValue),
            ArityType.OneOrMore => (1, int.MaxValue),
            _ => throw new ArgumentOutOfRangeException(nameof(ArityType), arityType, "Invalid ArityType enum value.")
        };
    }
}
