using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Base;

internal static class OptionRegistrar
{
    public static ICommandOption RegisterSwitchOption<TSource, TResult>(
        this TSource source,
        Expression<Func<TSource, TResult>> keyPropExpr, 
        bool mandatory)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        var command = new SwitchOption(mandatory ? 1 : 0);

        source.RegisterOptionProperty(command, keyProperty);

        return command;
    }

    public static ICommandOption RegisterSwitchOption<TSource, TResult>(
        this TSource source,
        Expression<Func<TSource, TResult>> keyPropExpr,
        int requiredTokenCount, int maximumTokenCount)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        var command = new SwitchOption(requiredTokenCount, maximumTokenCount);

        source.RegisterOptionProperty(command, keyProperty);

        return command;
    }

    public static IScalarCommandOption RegisterScalarOption<TSource, TResult>(
        this TSource source, 
        Expression<Func<TSource, TResult>> keyPropExpr,
        bool valueTokenMustExist, bool mandatory)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        var command = new ScalarCommandOption(valueTokenMustExist, mandatory ? 1 : 0);

        source.RegisterOptionProperty(command, keyProperty);

        return command;
    }

    public static IScalarCommandOption RegisterScalarOption<TSource, TResult>(
        this TSource source, 
        Expression<Func<TSource, IEnumerable<TResult>?>> keyPropExpr,
        bool valueTokenMustExist, bool enableValueTokenSplitting,
        bool allowSequentialValues, int requiredTokenCount, int maximumTokenCount)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        var command = new ScalarCommandOption(valueTokenMustExist, requiredTokenCount, maximumTokenCount)
        {
            EnableValueTokenSplitting = enableValueTokenSplitting,
            AllowSequentialValues = allowSequentialValues
        };

        source.RegisterOptionProperty(command, keyProperty);

        return command;
    }

    public static IScalarCommandOption RegisterScalarOption<TSource, TResult>(
        this TSource source,
        Expression<Func<TSource, TResult>> keyPropExpr,
        Expression<Func<TSource, bool>> flagPropExpr,
        bool valueTokenMustExist, bool mandatory)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var flagProperty = source.ValidateExpression(flagPropExpr);

        var command = new ScalarCommandOption(valueTokenMustExist, mandatory ? 1 : 0);

        source.RegisterOptionProperty(command, keyProperty, flagProperty);

        return command;
    }

    public static IScalarCommandOption RegisterScalarOption<TSource, TResult>(
        this TSource source, 
        Expression<Func<TSource, IEnumerable<TResult>?>> keyPropExpr,
        Expression<Func<TSource, int>> countPropExpr, 
        bool valueTokenMustExist, bool enableValueTokenSplitting, 
        bool allowSequentialValues, int requiredTokenCount, int maximumTokenCount) 
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var countProperty = source.ValidateExpression(countPropExpr);

        var command = new ScalarCommandOption(valueTokenMustExist, requiredTokenCount, maximumTokenCount)
        {
            EnableValueTokenSplitting = enableValueTokenSplitting,
            AllowSequentialValues = allowSequentialValues
        };

        source.RegisterOptionProperty(command, keyProperty, countProperty);

        return command;
    }

    public static INonCommandOption RegisterDefaultOption<TSource>(
        this TSource source,
        Expression<Func<TSource, string?>> keyPropExpr,
        bool mandatory)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        var command = new NonCommandOption(mandatory ? 1 : 0);

        source.RegisterOptionProperty(command, keyProperty);

        return command;
    }

    public static INonCommandOption RegisterDefaultOption<TSource>(
        this TSource source,
        Expression<Func<TSource, IEnumerable<string>?>> keyPropExpr,
        bool enableValueTokenSplitting, int requiredTokenCount, int maximumTokenCount)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        var command = new NonCommandOption(requiredTokenCount, maximumTokenCount)
        {
            EnableValueTokenSplitting = enableValueTokenSplitting
        };

        source.RegisterOptionProperty(command, keyProperty);

        return command;
    }

    public static INonCommandOption RegisterDefaultOption<TSource>(
        this TSource source,
        Expression<Func<TSource, string?>> keyPropExpr,
        Expression<Func<TSource, bool>> countPropExpr, 
        bool mandatory)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var countProperty = source.ValidateExpression(countPropExpr);

        var command = new NonCommandOption(mandatory ? 1 : 0);

        source.RegisterOptionProperty(command, keyProperty, countProperty);

        return command;
    }

    public static INonCommandOption RegisterDefaultOption<TSource>(
        this TSource source,
        Expression<Func<TSource, IEnumerable<string>?>> keyPropExpr,
        Expression<Func<TSource, int>> countPropExpr,
        bool enableValueTokenSplitting, int requiredTokenCount, int maximumTokenCount)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var countProperty = source.ValidateExpression(countPropExpr);

        var command = new NonCommandOption(requiredTokenCount, maximumTokenCount)
        {
            EnableValueTokenSplitting = enableValueTokenSplitting
        };

        source.RegisterOptionProperty(command, keyProperty, countProperty);

        return command;
    }

    private static PropertyInfo ValidateExpression<TSource, TResult>(
        this TSource source,
        Expression<Func<TSource, TResult>>? expression)
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

    public static bool IsAssignableFrom<TFromType>(this PropertyInfo property)
    {
        if (property.PropertyType.IsAssignableFrom(typeof(TFromType)))
            return true;

        if (property.PropertyType.IsAssignableFrom(typeof(TFromType?)))
            return true;

        if (property.PropertyType.IsAssignableFrom(typeof(List<TFromType>)))
            return true;

        if (property.PropertyType.IsAssignableFrom(typeof(List<TFromType?>)))
            return true;

        if (property.PropertyType.IsAssignableFrom(typeof(TFromType[])))
            return true;

        if (property.PropertyType.IsAssignableFrom(typeof(TFromType?[])))
            return true;

        return false;
    }
}
