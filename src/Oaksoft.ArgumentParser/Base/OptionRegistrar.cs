using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Oaksoft.ArgumentParser.Builder;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Base;

internal static class OptionRegistrar
{
    public static ISwitchOption RegisterSwitchOption<TSource>(
        this IArgumentParserBuilder builder, 
        PropertyInfo keyProperty, bool mandatoryOption)
        where TSource : IApplicationOptions
    {
        var optionLimits = (mandatoryOption ? ArityType.ExactlyOne : ArityType.ZeroOrOne).GetLimits();
        var option = new SwitchOption(optionLimits.Min, optionLimits.Max);

        builder.RegisterOptionProperty<TSource>(option, keyProperty);

        return option;
    }

    public static ISwitchOption RegisterSwitchOption<TSource>(
        this IArgumentParserBuilder builder,
        PropertyInfo keyProperty, ArityType optionArity)
        where TSource : IApplicationOptions
    {
        var optionLimits = optionArity.GetLimits();
        var option = new SwitchOption(optionLimits.Min, optionLimits.Max);

        builder.RegisterOptionProperty<TSource>(option, keyProperty);

        return option;
    }

    public static IScalarNamedOption<TValue> RegisterNamedOption<TSource, TValue>(
        this IArgumentParserBuilder builder, 
        PropertyInfo keyProperty, bool mustHaveOneValue, bool mandatoryOption)
        where TSource : IApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var optionLimits = (mandatoryOption ? ArityType.ExactlyOne : ArityType.ZeroOrOne).GetLimits();
        var valueLimits = (mustHaveOneValue ? ArityType.ExactlyOne : ArityType.ZeroOrOne).GetLimits();
        var option = new ScalarNamedOption<TValue>(optionLimits.Min, optionLimits.Max, valueLimits.Min, valueLimits.Max);

        builder.RegisterOptionProperty<TSource>(option, keyProperty);

        return option;
    }

    public static ISequentialNamedOption<TValue> RegisterNamedOption<TSource, TValue>(
        this IArgumentParserBuilder builder, 
        PropertyInfo keyProperty, ArityType valueArity, ArityType optionArity)
        where TSource : IApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var optionLimits = optionArity.GetLimits();
        var valueLimits = valueArity.GetLimits();
        var option = new SequentialNamedOption<TValue>(optionLimits.Min, optionLimits.Max, valueLimits.Min, valueLimits.Max);

        builder.RegisterOptionProperty<TSource>(option, keyProperty);

        return option;
    }

    public static IScalarNamedOption<TValue> RegisterNamedOption<TSource, TValue>(
        this IArgumentParserBuilder builder, 
        PropertyInfo keyProperty, PropertyInfo flagProperty,
        bool mustHaveOneValue, bool mandatoryOption)
        where TSource : IApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var optionLimits = (mandatoryOption ? ArityType.ExactlyOne : ArityType.ZeroOrOne).GetLimits();
        var valueLimits = (mustHaveOneValue ? ArityType.ExactlyOne : ArityType.ZeroOrOne).GetLimits();
        var option = new ScalarNamedOption<TValue>(optionLimits.Min, optionLimits.Max, valueLimits.Min, valueLimits.Max);

        builder.RegisterOptionProperty<TSource>(option, keyProperty, flagProperty);

        return option;
    }

    public static ISequentialNamedOption<TValue> RegisterNamedOption<TSource, TValue>(
        this IArgumentParserBuilder builder, 
        PropertyInfo keyProperty, PropertyInfo countProperty,
        ArityType valueArity, ArityType optionArity)
        where TSource : IApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var optionLimits = optionArity.GetLimits();
        var valueLimits = valueArity.GetLimits();
        var option = new SequentialNamedOption<TValue>(optionLimits.Min, optionLimits.Max, valueLimits.Min, valueLimits.Max);

        builder.RegisterOptionProperty<TSource>(option, keyProperty, countProperty);

        return option;
    }

    public static IScalarValueOption<TValue> RegisterValueOption<TSource, TValue>(
        this IArgumentParserBuilder builder,
        PropertyInfo keyProperty, bool mustHaveOneValue)
        where TSource : IApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var valueLimits = (mustHaveOneValue ? ArityType.ExactlyOne : ArityType.ZeroOrOne).GetLimits();
        var option = new ScalarValueOption<TValue>(valueLimits.Min, valueLimits.Max);

        builder.RegisterOptionProperty<TSource>(option, keyProperty);

        return option;
    }

    public static ISequentialValueOption<TValue> RegisterValueOption<TSource, TValue>(
        this IArgumentParserBuilder builder, 
        PropertyInfo keyProperty, ArityType valueArity)
        where TSource : IApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var valueLimits = valueArity.GetLimits();
        var option = new SequentialValueOption<TValue>(valueLimits.Min, valueLimits.Max);

        builder.RegisterOptionProperty<TSource>(option, keyProperty);

        return option;
    }

    public static IScalarValueOption<TValue> RegisterValueOption<TSource, TValue>(
        this IArgumentParserBuilder builder, 
        PropertyInfo keyProperty, PropertyInfo flagProperty, bool mustHaveOneValue)
        where TSource : IApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var valueLimits = (mustHaveOneValue ? ArityType.ExactlyOne : ArityType.ZeroOrOne).GetLimits();
        var option = new ScalarValueOption<TValue>(valueLimits.Min, valueLimits.Max);

        builder.RegisterOptionProperty<TSource>(option, keyProperty, flagProperty);

        return option;
    }

    public static ISequentialValueOption<TValue> RegisterValueOption<TSource, TValue>(
        this IArgumentParserBuilder builder, 
        PropertyInfo keyProperty, PropertyInfo countProperty, ArityType valueArity)
        where TSource : IApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var valueLimits = valueArity.GetLimits();
        var option = new SequentialValueOption<TValue>(valueLimits.Min, valueLimits.Max);

        builder.RegisterOptionProperty<TSource>(option, keyProperty, countProperty);

        return option;
    }

    public static PropertyInfo ValidateExpression<TSource, TValue>(
        this IArgumentParserBuilder builder, Expression<Func<TSource, TValue>>? expression, string type)
        where TSource : IApplicationOptions
    {
        if (expression?.Body == null)
            throw new ArgumentNullException(nameof(expression));

        if (expression.NodeType != ExpressionType.Lambda ||
            expression.Body is not MemberExpression member ||
            member.Member.MemberType != MemberTypes.Property)
            throw new ArgumentException($"Invalid lambda expression, please select a {type} type property!");

        var parserBuilder = (ArgumentParserBuilder<TSource>)builder;
        var properties = parserBuilder.GetAppOptions().GetType().GetProperties();

        var property = properties.FirstOrDefault(p => p.Name == member.Member.Name);
        if (property == null)
            throw new ArgumentException($"Unknown property, please select a {type} type property!");

        return property;
    }

    private static void RegisterOptionProperty<TSource>(
        this IArgumentParserBuilder builder, BaseOption option, 
        PropertyInfo keyProperty, PropertyInfo? countProperty = null)
        where TSource : IApplicationOptions
    {
        var parserBuilder = (ArgumentParserBuilder<TSource>)builder;

        var propertyNames = parserBuilder.GetRegisteredPropertyNames();

        if (keyProperty.Name == countProperty?.Name)
            throw new ArgumentException($"Selected Key and Count property cannot be same. Properties: {keyProperty.Name}, {countProperty.Name}");

        if (propertyNames.Contains(keyProperty.Name))
            throw new ArgumentException($"Property '{keyProperty.Name}' already registered with an option!");

        if (countProperty != null && propertyNames.Contains(countProperty.Name))
            throw new ArgumentException($"Property '{countProperty.Name}' already registered with an option!");

        option.SetKeyProperty(keyProperty);
        option.SetCountProperty(countProperty);

        parserBuilder.RegisterOption(option);
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
