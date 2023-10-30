using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Glowy.CLxParser.Options;
using Glowy.CLxParser.Parser;

namespace Glowy.CLxParser.Extensions;

public static partial class OptionsExtensions
{ 
    public static IScalarCommandOption AddStringOption<TSource>(
        this TSource source,
        Expression<Func<TSource, string?>> keyProperty,
        bool valueTokenMustExist = true,
        bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        var keyPropName = ValidatePropertyExpression(keyProperty);

        var command = new ScalarCommandOption(valueTokenMustExist, mandatory ? 1 : 0);

        AddOption(source, command, keyPropName);

        return command;
    }

    public static IScalarCommandOption AddStringOption<TSource>(
        this TSource source,
        Expression<Func<TSource, ICollection<string>?>> keyProperty,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true, 
        bool allowSequentialValues = true,
        int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        var keyPropName = ValidatePropertyExpression(keyProperty);

        var command = new ScalarCommandOption(valueTokenMustExist, requiredTokenCount, maximumTokenCount)
        {
            EnableValueTokenSplitting = enableValueTokenSplitting,
            AllowSequentialValues = allowSequentialValues
        };

        AddOption(source, command, keyPropName);

        return command;
    }

    public static IScalarCommandOption AddStringOption<TSource>(
        this TSource source,
        Expression<Func<TSource, string?>> keyProperty,
        Expression<Func<TSource, bool>> countProperty,
        bool valueTokenMustExist = true,
        bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        var keyPropName = ValidatePropertyExpression(keyProperty);
        var countPropName = ValidatePropertyExpression(countProperty);

        var command = new ScalarCommandOption(valueTokenMustExist, mandatory ? 1 : 0);

        AddOption(source, command, keyPropName, countPropName);

        return command;
    }

    public static IScalarCommandOption AddStringOption<TSource>(
        this TSource source,
        Expression<Func<TSource, ICollection<string>?>> keyProperty,
        Expression<Func<TSource, int>> countProperty,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true, 
        bool allowSequentialValues = true,
        int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        var keyPropName = ValidatePropertyExpression(keyProperty);
        var countPropName = ValidatePropertyExpression(countProperty);

        var command = new ScalarCommandOption(valueTokenMustExist, requiredTokenCount, maximumTokenCount)
        {
            EnableValueTokenSplitting = enableValueTokenSplitting,
            AllowSequentialValues = allowSequentialValues
        };

        AddOption(source, command, keyPropName, countPropName);

        return command;
    }
}
