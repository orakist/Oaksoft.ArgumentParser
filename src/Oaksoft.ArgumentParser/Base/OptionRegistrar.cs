using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Oaksoft.ArgumentParser.Builder;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Exceptions;
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
        var optionLimits = mandatoryOption.GetLimits();
        var option = new SwitchOption(optionLimits.Min, optionLimits.Max);

        builder.RegisterOptionProperty<TSource>(option, keyProperty);

        return option;
    }

    public static ICounterOption RegisterCounterOption<TSource>(
        this IArgumentParserBuilder builder,
        PropertyInfo keyProperty, ArityType optionArity)
        where TSource : IApplicationOptions
    {
        var optionLimits = optionArity.GetLimits().GetOrThrow(keyProperty.Name);
        var option = new CounterOption(optionLimits.Min, optionLimits.Max);

        builder.RegisterOptionProperty<TSource>(option, keyProperty);

        return option;
    }

    public static IScalarNamedOption<TValue> RegisterNamedOption<TSource, TValue>(
        this IArgumentParserBuilder builder,
        PropertyInfo keyProperty, bool mustHaveOneValue, bool mandatoryOption)
        where TSource : IApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var optionLimits = mandatoryOption.GetLimits();
        var valueLimits = mustHaveOneValue.GetLimits();
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
        var optionLimits = optionArity.GetLimits().GetOrThrow(keyProperty.Name);
        var valueLimits = valueArity.GetLimits().GetOrThrow(keyProperty.Name);
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
        var optionLimits = mandatoryOption.GetLimits();
        var valueLimits = mustHaveOneValue.GetLimits();
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
        var optionLimits = optionArity.GetLimits().GetOrThrow(keyProperty.Name);
        var valueLimits = valueArity.GetLimits().GetOrThrow(keyProperty.Name);
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
        var valueLimits = mustHaveOneValue.GetLimits();
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
        var valueLimits = valueArity.GetLimits().GetOrThrow(keyProperty.Name);
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
        var valueLimits = mustHaveOneValue.GetLimits();
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
        var valueLimits = valueArity.GetLimits().GetOrThrow(keyProperty.Name);
        var option = new SequentialValueOption<TValue>(valueLimits.Min, valueLimits.Max);

        builder.RegisterOptionProperty<TSource>(option, keyProperty, countProperty);

        return option;
    }

    public static PropertyInfo ValidateExpression<TSource, TValue>(
        this IArgumentParserBuilder builder, Expression<Func<TSource, TValue>>? expression, string type)
        where TSource : IApplicationOptions
    {
        if (expression?.Body == null)
        {
            throw BuilderErrors.NullValue.With(nameof(expression)).ToException();
        }

        if (expression.NodeType != ExpressionType.Lambda ||
            expression.Body is not MemberExpression member ||
            member.Member.MemberType != MemberTypes.Property)
        {
            throw BuilderErrors.InvalidPropertyExpression.With(type).ToException();
        }

        var parserBuilder = (ArgumentParserBuilder<TSource>)builder;
        var properties = parserBuilder.GetAppOptions().GetType().GetProperties();
        var property = properties.First(p => p.Name == member.Member.Name);

        if (member.Type == typeof(string) && type == typeof(char).ToString())
        {
            throw BuilderErrors.InvalidStringPropertyUsage.With(property.Name).ToException();
        }

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
        {
            throw BuilderErrors.SamePropertyUsage.With(keyProperty.Name, countProperty.Name).ToException();
        }

        if (propertyNames.Contains(keyProperty.Name))
        {
            throw BuilderErrors.PropertyAlreadyInUse.With(keyProperty.Name).ToException();
        }

        if (countProperty != null && propertyNames.Contains(countProperty.Name))
        {
            throw BuilderErrors.PropertyAlreadyInUse.With(countProperty.Name).ToException();
        }

        option.SetKeyProperty(keyProperty);
        option.SetCountProperty(countProperty);

        parserBuilder.RegisterOption(option);
    }

    private static (int Min, int Max) GetLimits(this bool arityType)
    {
        return arityType ? (1, 1) : (0, 1);
    }

    public static Result<(int Min, int Max)> GetLimits(this ArityType arityType)
    {
        return arityType switch
        {
            ArityType.Zero => (0, 0),
            ArityType.ZeroOrOne => (0, 1),
            ArityType.ExactlyOne => (1, 1),
            ArityType.ZeroOrMore => (0, int.MaxValue),
            ArityType.OneOrMore => (1, int.MaxValue),
            _ => BuilderErrors.InvalidEnum.With(nameof(ArityType), arityType)
        };
    }
}
