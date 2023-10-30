using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Glowy.CLxParser.Options;
using Glowy.CLxParser.Parser;

namespace Glowy.CLxParser.Extensions;

public static partial class OptionsExtensions
{ 
    public static INonCommandOption AddDefaultOption<TSource>(
        this TSource source,
        Expression<Func<TSource, string?>> keyProperty,
        bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        var keyPropName = ValidatePropertyExpression(keyProperty);

        var command = new NonCommandOption(mandatory ? 1 : 0);

        AddOption(source, command, keyPropName);

        return command;
    }

    public static INonCommandOption AddDefaultOption<TSource>(
        this TSource source,
        Expression<Func<TSource, ICollection<string>?>> keyProperty,
        bool enableValueTokenSplitting = true,
        int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        var keyPropName = ValidatePropertyExpression(keyProperty);

        var command = new NonCommandOption(requiredTokenCount, maximumTokenCount)
        {
            EnableValueTokenSplitting = enableValueTokenSplitting
        };

        AddOption(source, command, keyPropName);

        return command;
    }

    public static INonCommandOption AddDefaultOption<TSource>(
        this TSource source,
        Expression<Func<TSource, string?>> keyProperty,
        Expression<Func<TSource, bool>> countProperty,
        bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        var keyPropName = ValidatePropertyExpression(keyProperty);
        var countPropName = ValidatePropertyExpression(countProperty);

        var command = new NonCommandOption(mandatory ? 1 : 0);

        AddOption(source, command, keyPropName, countPropName);

        return command;
    }

    public static INonCommandOption AddDefaultOption<TSource>(
        this TSource source,
        Expression<Func<TSource, ICollection<string>?>> keyProperty,
        Expression<Func<TSource, int>> countProperty,
        bool enableValueTokenSplitting = true,
        int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        var keyPropName = ValidatePropertyExpression(keyProperty);
        var countPropName = ValidatePropertyExpression(countProperty);

        var command = new NonCommandOption(requiredTokenCount, maximumTokenCount)
        {
            EnableValueTokenSplitting = enableValueTokenSplitting
        };

        AddOption(source, command, keyPropName, countPropName);

        return command;
    }
}
