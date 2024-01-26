using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Oaksoft.ArgumentParser.Builder;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Errors;
using Oaksoft.ArgumentParser.Errors.Builder;
using Oaksoft.ArgumentParser.Options;

namespace Oaksoft.ArgumentParser.Base;

internal static class OptionRegistrar
{
    public static ISwitchOption RegisterSwitchOption<TSource>(
        this IArgumentParserBuilder builder,
        PropertyInfo keyProperty, bool mandatoryOption)
    {
        var optionLimits = mandatoryOption.GetLimits();
        var option = new SwitchOption(optionLimits.Min, optionLimits.Max);

        builder.RegisterOptionProperty<TSource>(option, keyProperty);

        return option;
    }

    public static ICounterOption RegisterCounterOption<TSource>(
        this IArgumentParserBuilder builder,
        PropertyInfo keyProperty, ArityType optionArity)
    {
        var optionLimits = optionArity.GetLimits().GetOrThrow(keyProperty.Name);
        var option = new CounterOption(optionLimits.Min, optionLimits.Max);

        builder.RegisterOptionProperty<TSource>(option, keyProperty);

        return option;
    }

    public static IScalarNamedOption<TValue> RegisterNamedOption<TSource, TValue>(
        this IArgumentParserBuilder builder,
        PropertyInfo keyProperty, bool mustHaveOneValue, bool mandatoryOption)
        where TValue : IComparable
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
        where TValue : IComparable
    {
        var optionLimits = optionArity.GetLimits().GetOrThrow(keyProperty.Name);
        var valueLimits = valueArity.GetLimits().GetOrThrow(keyProperty.Name);
        var option = new SequentialNamedOption<TValue>(optionLimits.Min, optionLimits.Max, valueLimits.Min, valueLimits.Max);

        builder.RegisterOptionProperty<TSource>(option, keyProperty);

        return option;
    }

    public static IScalarValueOption<TValue> RegisterValueOption<TSource, TValue>(
        this IArgumentParserBuilder builder,
        PropertyInfo keyProperty, bool mustHaveOneValue)
        where TValue : IComparable
    {
        var valueLimits = mustHaveOneValue.GetLimits();
        var option = new ScalarValueOption<TValue>(valueLimits.Min, valueLimits.Max);

        builder.RegisterOptionProperty<TSource>(option, keyProperty);

        return option;
    }

    public static ISequentialValueOption<TValue> RegisterValueOption<TSource, TValue>(
        this IArgumentParserBuilder builder,
        PropertyInfo keyProperty, ArityType valueArity)
        where TValue : IComparable
    {
        var valueLimits = valueArity.GetLimits().GetOrThrow(keyProperty.Name);
        var option = new SequentialValueOption<TValue>(valueLimits.Min, valueLimits.Max);

        builder.RegisterOptionProperty<TSource>(option, keyProperty);

        return option;
    }

    public static PropertyInfo ValidateExpression<TSource, TValue>(
        this IArgumentParserBuilder builder, Expression<Func<TSource, TValue>>? expression, string typeName)
    {
        if (expression?.Body == null)
        {
            throw BuilderErrors.NullValue.ToException(nameof(expression));
        }

        if (expression.NodeType != ExpressionType.Lambda ||
            expression.Body is not MemberExpression member ||
            member.Member.MemberType != MemberTypes.Property)
        {
            throw BuilderErrors.InvalidPropertyExpression.ToException(typeName);
        }

        var parserBuilder = (ArgumentParserBuilder<TSource>)builder;
        var properties = parserBuilder.GetApplicationOptions()!.GetType().GetProperties();
        var property = properties.First(p => p.Name == member.Member.Name);

        if (property.GetOptionType().OptionType == null)
        {
            throw BuilderErrors.UnsupportedPropertyType.ToException(property.Name, property.PropertyType.ToString());
        }

        if (property.SetMethod == null)
        {
            throw BuilderErrors.PropertyWithoutSetMethod.ToException(property.Name);
        }

        if (member.Type == typeof(string) && typeName == typeof(char).ToString())
        {
            throw BuilderErrors.InvalidStringPropertyUsage.ToException(property.Name);
        }

        return property;
    }

    public static void RegisterOptionProperty<TSource>(
        this IArgumentParserBuilder builder, BaseOption option, PropertyInfo keyProperty)
    {
        var parserBuilder = (ArgumentParserBuilder<TSource>)builder;

        var propertyNames = parserBuilder.GetRegisteredPropertyNames();

        if (propertyNames.Contains(keyProperty.Name))
        {
            throw BuilderErrors.PropertyAlreadyInUse.ToException(keyProperty.Name);
        }

        option.SetKeyProperty(keyProperty);

        parserBuilder.RegisterOption(option);
    }

    public static (Type? OptionType, bool Sequential) GetOptionType(this PropertyInfo property)
    {
        var propType = property.PropertyType;
        var isSequential = IsSequentialPropertyType(propType);
        var valueType = isSequential ? propType.GetElementType() ?? propType.GetGenericArguments()[0] : propType;
        var optionType = Nullable.GetUnderlyingType(valueType) ?? valueType;

        if (isSequential)
        {
            var genericType = typeof(List<>).MakeGenericType(valueType);
            if (propType.IsAssignableFrom(genericType))
                return (optionType, true);

            genericType = valueType.MakeArrayType();
            if (propType.IsAssignableFrom(genericType))
                return (optionType, true);

            genericType = typeof(Collection<>).MakeGenericType(valueType);
            if (propType.IsAssignableFrom(genericType))
                return (optionType, true);

            genericType = typeof(HashSet<>).MakeGenericType(valueType);
            if (propType.IsAssignableFrom(genericType))
                return (optionType, true);

            return (null, true);
        }

        return (optionType, false);
    }

    private static bool IsSequentialPropertyType(Type propType)
    {
        if (propType == typeof(string))
            return false;

        if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            return true;

        if (propType.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
            return true;

        return false;
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
