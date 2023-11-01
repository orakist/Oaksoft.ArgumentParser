using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Extensions;

public static partial class OptionsExtensions
{ 
    public static ICommandOption AddSwitchOption<TSource>(
        this TSource source,
        Expression<Func<TSource, bool>> keyProperty,
        bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        var keyPropName = ValidatePropertyExpression(keyProperty);

        var command = new SwitchOption(mandatory ? 1 : 0);

        AddOption(source, command, keyPropName);

        return command;
    }

    public static ICommandOption AddCountOption<TSource>(
        this TSource source,
        Expression<Func<TSource, int>> keyProperty,
        int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        var keyPropName = ValidatePropertyExpression(keyProperty);

        var command = new SwitchOption(requiredTokenCount, maximumTokenCount);

        AddOption(source, command, keyPropName);

        return command;
    }

    private static string ValidatePropertyExpression<TSource, TResult>(
        Expression<Func<TSource, TResult>> expression)
        where TSource : BaseApplicationOptions
    {
        if (expression.Body == null)
            throw new ArgumentNullException(nameof(expression));

        if (expression.NodeType != ExpressionType.Lambda || 
            expression.Body is not MemberExpression member || 
            member.Member.MemberType != MemberTypes.Property)
            throw new ArgumentException("Invalid lambda expression, please select a property!");

        return member.Member.Name;
    }

    private static void AddOption<TSource>(
        TSource source, BaseOption option, string keyProperty, string? countProperty = null)
        where TSource : BaseApplicationOptions
    {
        var options = source.GetType().BaseType?
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
            .Select(f => f.GetValue(source))
            .OfType<Dictionary<string, IBaseOption>>()
            .FirstOrDefault();

        var propertyNames = options!.Values
            .Select(a => ((BaseOption)a).CountProperty)
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .ToList();

        propertyNames.AddRange(options.Keys);

        if (keyProperty == countProperty)
            throw new ArgumentException($"Selected Key and Count property cannot be same. Property: {keyProperty}");

        if (propertyNames.Contains(keyProperty))
            throw new ArgumentException($"Property '{keyProperty}' already registered with an option!");

        if (!string.IsNullOrWhiteSpace(countProperty) && propertyNames.Contains(countProperty))
            throw new ArgumentException($"Property '{countProperty}' already registered with an option!");

        option.SetKeyProperty(keyProperty);
        option.SetCountProperty(countProperty);

        options[keyProperty] = option;
    }
}
