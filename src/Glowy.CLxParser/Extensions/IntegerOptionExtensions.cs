using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Glowy.CLxParser.Callbacks;
using Glowy.CLxParser.Options;
using Glowy.CLxParser.Parser;

namespace Glowy.CLxParser.Extensions;

public static partial class OptionsExtensions
{
    public static IScalarCommandOption AddIntegerOption<TSource>(
        this TSource source,
        Expression<Func<TSource, int>> keyProperty,
        bool valueTokenMustExist = true,
        int? minIntegerValue = null, int? maxIntegerValue = null,
        bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        var keyPropName = ValidatePropertyExpression(keyProperty);

        var command = new ScalarCommandOption(valueTokenMustExist, mandatory ? 1 : 0);

        AddOption(source, command, keyPropName);

        return (IScalarCommandOption)command
            .WithParsingCallbacks(IntegerParsingCallbacks.Instance)
            .WithConstraints(minIntegerValue?.ToString(), maxIntegerValue?.ToString());
    }

    public static IScalarCommandOption AddIntegerOption<TSource>(
        this TSource source,
        Expression<Func<TSource, ICollection<int>>> keyProperty,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true, 
        bool allowSequentialValues = true,
        int? minIntegerValue = null, int? maxIntegerValue = null,
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

        return (IScalarCommandOption)command
            .WithParsingCallbacks(IntegerParsingCallbacks.Instance)
            .WithConstraints(minIntegerValue?.ToString(), maxIntegerValue?.ToString());
    }

    public static IScalarCommandOption AddIntegerOption<TSource>(
        this TSource source,
        Expression<Func<TSource, int>> keyProperty,
        Expression<Func<TSource, bool>> countProperty,
        bool valueTokenMustExist = true,
        int? minIntegerValue = null, int? maxIntegerValue = null,
        bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        var keyPropName = ValidatePropertyExpression(keyProperty);
        var countPropName = ValidatePropertyExpression(countProperty);

        var command = new ScalarCommandOption(valueTokenMustExist, mandatory ? 1 : 0);

        AddOption(source, command, keyPropName, countPropName);

        return (IScalarCommandOption)command
            .WithParsingCallbacks(IntegerParsingCallbacks.Instance)
            .WithConstraints(minIntegerValue?.ToString(), maxIntegerValue?.ToString());
    }

    public static IScalarCommandOption AddIntegerOption<TSource>(
        this TSource source,
        Expression<Func<TSource, ICollection<int>>> keyProperty,
        Expression<Func<TSource, int>> countProperty,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true, 
        bool allowSequentialValues = true,
        int? minIntegerValue = null, int? maxIntegerValue = null,
        int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        var keyPropName = ValidatePropertyExpression(keyProperty);
        var countPropName = ValidatePropertyExpression(countProperty);

        var command = new ScalarCommandOption(valueTokenMustExist, requiredTokenCount, maximumTokenCount)
        {
            EnableValueTokenSplitting = enableValueTokenSplitting,
            AllowSequentialValues = allowSequentialValues,
        };

        AddOption(source, command, keyPropName, countPropName);

        return (IScalarCommandOption)command
            .WithParsingCallbacks(IntegerParsingCallbacks.Instance)
            .WithConstraints(minIntegerValue?.ToString(), maxIntegerValue?.ToString());
    }
}
