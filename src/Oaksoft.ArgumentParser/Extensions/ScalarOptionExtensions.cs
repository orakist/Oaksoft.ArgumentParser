using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Extensions;

public static partial class OptionsExtensions
{
    public static IScalarOption<TValue> AddScalarOption<TSource, TValue>(
        this TSource source, Expression<Func<TSource, TValue?>> keyPropExpr,
        bool valueTokenMustExist = true, bool mandatory = false)
        where TSource : BaseApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        return source.RegisterScalarOption<TSource, TValue>(
            keyProperty, valueTokenMustExist, mandatory);
    }

    public static IScalarOption<TValue> AddScalarOption<TSource, TValue>(
        this TSource source, Expression<Func<TSource, TValue?>> keyPropExpr,
        bool valueTokenMustExist = true, bool mandatory = false)
        where TSource : BaseApplicationOptions
        where TValue : struct, IComparable, IEquatable<TValue>
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        return source.RegisterScalarOption<TSource, TValue>(
            keyProperty, valueTokenMustExist, mandatory);
    }

    public static IScalarOption<TValue> AddScalarOption<TSource, TValue>(
        this TSource source,
        Expression<Func<TSource, IEnumerable<TValue>?>> keyPropExpr,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true,
        bool allowSequentialValues = true, int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        return source.RegisterScalarOption<TSource, TValue>(
            keyProperty, valueTokenMustExist, enableValueTokenSplitting,
            allowSequentialValues, requiredTokenCount, maximumTokenCount);
    }

    public static IScalarOption<TValue> AddScalarOption<TSource, TValue>(
        this TSource source,
        Expression<Func<TSource, IEnumerable<TValue?>?>> keyPropExpr,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true,
        bool allowSequentialValues = true, int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
        where TValue : struct, IComparable, IEquatable<TValue>
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        return source.RegisterScalarOption<TSource, TValue>(
            keyProperty, valueTokenMustExist, enableValueTokenSplitting,
            allowSequentialValues, requiredTokenCount, maximumTokenCount);
    }

    public static IScalarOption<TValue> AddScalarOption<TSource, TValue>(
        this TSource source,
        Expression<Func<TSource, TValue?>> keyPropExpr,
        Expression<Func<TSource, bool>> flagPropExpr,
        bool valueTokenMustExist = true,
        bool mandatory = false)
        where TSource : BaseApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var flagProperty = source.ValidateExpression(flagPropExpr);

        return source.RegisterScalarOption<TSource, TValue>(
            keyProperty, flagProperty, valueTokenMustExist, mandatory);
    }

    public static IScalarOption<TValue> AddScalarOption<TSource, TValue>(
        this TSource source,
        Expression<Func<TSource, TValue?>> keyPropExpr,
        Expression<Func<TSource, bool>> flagPropExpr,
        bool valueTokenMustExist = true,
        bool mandatory = false)
        where TSource : BaseApplicationOptions
        where TValue : struct, IComparable, IEquatable<TValue>
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var flagProperty = source.ValidateExpression(flagPropExpr);

        return source.RegisterScalarOption<TSource, TValue>(
            keyProperty, flagProperty, valueTokenMustExist, mandatory);
    }

    public static IScalarOption<TValue> AddScalarOption<TSource, TValue>(
        this TSource source,
        Expression<Func<TSource, IEnumerable<TValue>?>> keyPropExpr,
        Expression<Func<TSource, int>> countPropExpr,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true,
        bool allowSequentialValues = true, int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var countProperty = source.ValidateExpression(countPropExpr);

        return source.RegisterScalarOption<TSource, TValue>(
            keyProperty, countProperty, valueTokenMustExist, enableValueTokenSplitting,
            allowSequentialValues, requiredTokenCount, maximumTokenCount);
    }

    public static IScalarOption<TValue> AddScalarOption<TSource, TValue>(
        this TSource source,
        Expression<Func<TSource, IEnumerable<TValue?>?>> keyPropExpr,
        Expression<Func<TSource, int>> countPropExpr,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true,
        bool allowSequentialValues = true, int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
        where TValue : struct, IComparable, IEquatable<TValue>
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var countProperty = source.ValidateExpression(countPropExpr);

        return source.RegisterScalarOption<TSource, TValue>(
            keyProperty, countProperty, valueTokenMustExist, enableValueTokenSplitting,
            allowSequentialValues, requiredTokenCount, maximumTokenCount);
    }
}
