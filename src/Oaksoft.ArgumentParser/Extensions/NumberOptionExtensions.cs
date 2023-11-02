using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Extensions;

public static partial class OptionsExtensions
{
    public static IScalarCommandOption<float> AddScalarOption<TSource>(
        this TSource source, Expression<Func<TSource, float>> keyPropExpr,
        bool valueTokenMustExist = true, bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        return source.RegisterScalarOption<TSource, float>(
            keyProperty, valueTokenMustExist, mandatory);
    }

    public static IScalarCommandOption<float> AddScalarOption<TSource>(
        this TSource source, Expression<Func<TSource, float?>> keyPropExpr,
        bool valueTokenMustExist = true, bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        return source.RegisterScalarOption<TSource, float>(
            keyProperty, valueTokenMustExist, mandatory);
    }

    public static IScalarCommandOption<double> AddScalarOption<TSource>(
        this TSource source, Expression<Func<TSource, double>> keyPropExpr,
        bool valueTokenMustExist = true, bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        return source.RegisterScalarOption<TSource, double>(
            keyProperty, valueTokenMustExist, mandatory);
    }

    public static IScalarCommandOption<double> AddScalarOption<TSource>(
        this TSource source, Expression<Func<TSource, double?>> keyPropExpr,
        bool valueTokenMustExist = true, bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        return source.RegisterScalarOption<TSource, double>(
            keyProperty, valueTokenMustExist, mandatory);
    }

    public static IScalarCommandOption<decimal> AddScalarOption<TSource>(
        this TSource source, Expression<Func<TSource, decimal>> keyPropExpr,
        bool valueTokenMustExist = true, bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        return source.RegisterScalarOption<TSource, decimal>(
            keyProperty, valueTokenMustExist, mandatory);
    }

    public static IScalarCommandOption<decimal> AddScalarOption<TSource>(
        this TSource source, Expression<Func<TSource, decimal?>> keyPropExpr,
        bool valueTokenMustExist = true, bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        return source.RegisterScalarOption<TSource, decimal>(
            keyProperty, valueTokenMustExist, mandatory);
    }

    public static IScalarCommandOption<float> AddScalarOption<TSource>(
        this TSource source, 
        Expression<Func<TSource, IEnumerable<float>?>> keyPropExpr,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true,
        bool allowSequentialValues = true, int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        return source.RegisterScalarOption<TSource, float>(
            keyProperty, valueTokenMustExist, enableValueTokenSplitting,
            allowSequentialValues, requiredTokenCount, maximumTokenCount);
    }

    public static IScalarCommandOption<float> AddScalarOption<TSource>(
        this TSource source, Expression<Func<TSource, IEnumerable<float?>?>> keyPropExpr,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true,
        bool allowSequentialValues = true, int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        return source.RegisterScalarOption<TSource, float>(
            keyProperty, valueTokenMustExist, enableValueTokenSplitting,
            allowSequentialValues, requiredTokenCount, maximumTokenCount);
    }

    public static IScalarCommandOption<double> AddScalarOption<TSource>(
        this TSource source, Expression<Func<TSource, IEnumerable<double>?>> keyPropExpr,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true,
        bool allowSequentialValues = true, int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        return source.RegisterScalarOption<TSource, double>(
            keyProperty, valueTokenMustExist, enableValueTokenSplitting,
            allowSequentialValues, requiredTokenCount, maximumTokenCount);
    }

    public static IScalarCommandOption<double> AddScalarOption<TSource>(
        this TSource source, Expression<Func<TSource, IEnumerable<double?>?>> keyPropExpr,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true,
        bool allowSequentialValues = true, int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        return source.RegisterScalarOption<TSource, double>(
            keyProperty, valueTokenMustExist, enableValueTokenSplitting,
            allowSequentialValues, requiredTokenCount, maximumTokenCount);
    }

    public static IScalarCommandOption<decimal> AddScalarOption<TSource>(
        this TSource source, Expression<Func<TSource, IEnumerable<decimal>?>> keyPropExpr,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true,
        bool allowSequentialValues = true, int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        return source.RegisterScalarOption<TSource, decimal>(
            keyProperty, valueTokenMustExist, enableValueTokenSplitting,
            allowSequentialValues, requiredTokenCount, maximumTokenCount);
    }

    public static IScalarCommandOption<decimal> AddScalarOption<TSource>(
        this TSource source, Expression<Func<TSource, IEnumerable<decimal?>?>> keyPropExpr,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true,
        bool allowSequentialValues = true, int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        return source.RegisterScalarOption<TSource, decimal>(
            keyProperty, valueTokenMustExist, enableValueTokenSplitting,
            allowSequentialValues, requiredTokenCount, maximumTokenCount);
    }

    public static IScalarCommandOption<float> AddScalarOption<TSource>(
        this TSource source, 
        Expression<Func<TSource, float>> keyPropExpr,
        Expression<Func<TSource, bool>> flagPropExpr,
        bool valueTokenMustExist = true, bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var flagProperty = source.ValidateExpression(flagPropExpr);

        return source.RegisterScalarOption<TSource, float>(
            keyProperty, flagProperty, valueTokenMustExist, mandatory);
    }

    public static IScalarCommandOption<float> AddScalarOption<TSource>(
        this TSource source,
        Expression<Func<TSource, float?>> keyPropExpr,
        Expression<Func<TSource, bool>> flagPropExpr,
        bool valueTokenMustExist = true, bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var flagProperty = source.ValidateExpression(flagPropExpr);

        return source.RegisterScalarOption<TSource, float>(
            keyProperty, flagProperty, valueTokenMustExist, mandatory);
    }

    public static IScalarCommandOption<double> AddScalarOption<TSource>(
        this TSource source,
        Expression<Func<TSource, double>> keyPropExpr,
        Expression<Func<TSource, bool>> flagPropExpr,
        bool valueTokenMustExist = true, bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var flagProperty = source.ValidateExpression(flagPropExpr);

        return source.RegisterScalarOption<TSource, double>(
            keyProperty, flagProperty, valueTokenMustExist, mandatory);
    }

    public static IScalarCommandOption<double> AddScalarOption<TSource>(
        this TSource source,
        Expression<Func<TSource, double?>> keyPropExpr,
        Expression<Func<TSource, bool>> flagPropExpr,
        bool valueTokenMustExist = true, bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var flagProperty = source.ValidateExpression(flagPropExpr);

        return source.RegisterScalarOption<TSource, double>(
            keyProperty, flagProperty, valueTokenMustExist, mandatory);
    }
    
    public static IScalarCommandOption<decimal> AddScalarOption<TSource>(
        this TSource source,
        Expression<Func<TSource, decimal>> keyPropExpr,
        Expression<Func<TSource, bool>> flagPropExpr,
        bool valueTokenMustExist = true, bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var flagProperty = source.ValidateExpression(flagPropExpr);

        return source.RegisterScalarOption<TSource, decimal>(
            keyProperty, flagProperty, valueTokenMustExist, mandatory);
    }

    public static IScalarCommandOption<decimal> AddScalarOption<TSource>(
        this TSource source,
        Expression<Func<TSource, decimal?>> keyPropExpr,
        Expression<Func<TSource, bool>> flagPropExpr,
        bool valueTokenMustExist = true, bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var flagProperty = source.ValidateExpression(flagPropExpr);

        return source.RegisterScalarOption<TSource, decimal>(
            keyProperty, flagProperty, valueTokenMustExist, mandatory);
    }

    public static IScalarCommandOption<float> AddScalarOption<TSource>(
        this TSource source,
        Expression<Func<TSource, IEnumerable<float>?>> keyPropExpr,
        Expression<Func<TSource, int>> countPropExpr,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true,
        bool allowSequentialValues = true, int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var countProperty = source.ValidateExpression(countPropExpr);

        return source.RegisterScalarOption<TSource, float>(
            keyProperty, countProperty, valueTokenMustExist, enableValueTokenSplitting,
            allowSequentialValues, requiredTokenCount, maximumTokenCount);
    }

    public static IScalarCommandOption<float> AddScalarOption<TSource>(
        this TSource source,
        Expression<Func<TSource, IEnumerable<float?>?>> keyPropExpr,
        Expression<Func<TSource, int>> countPropExpr,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true,
        bool allowSequentialValues = true, int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var countProperty = source.ValidateExpression(countPropExpr);

        return source.RegisterScalarOption<TSource, float>(
            keyProperty, countProperty, valueTokenMustExist, enableValueTokenSplitting,
            allowSequentialValues, requiredTokenCount, maximumTokenCount);
    }

    public static IScalarCommandOption<double> AddScalarOption<TSource>(
        this TSource source,
        Expression<Func<TSource, IEnumerable<double>?>> keyPropExpr,
        Expression<Func<TSource, int>> countPropExpr,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true,
        bool allowSequentialValues = true, int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var countProperty = source.ValidateExpression(countPropExpr);

        return source.RegisterScalarOption<TSource, double>(
            keyProperty, countProperty, valueTokenMustExist, enableValueTokenSplitting,
            allowSequentialValues, requiredTokenCount, maximumTokenCount);
    }

    public static IScalarCommandOption<double> AddScalarOption<TSource>(
        this TSource source,
        Expression<Func<TSource, IEnumerable<double?>?>> keyPropExpr,
        Expression<Func<TSource, int>> countPropExpr,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true,
        bool allowSequentialValues = true, int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var countProperty = source.ValidateExpression(countPropExpr);

        return source.RegisterScalarOption<TSource, double>(
            keyProperty, countProperty, valueTokenMustExist, enableValueTokenSplitting,
            allowSequentialValues, requiredTokenCount, maximumTokenCount);
    }

    public static IScalarCommandOption<decimal> AddScalarOption<TSource>(
        this TSource source,
        Expression<Func<TSource, IEnumerable<decimal>?>> keyPropExpr,
        Expression<Func<TSource, int>> countPropExpr,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true,
        bool allowSequentialValues = true, int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var countProperty = source.ValidateExpression(countPropExpr);

        return source.RegisterScalarOption<TSource, decimal>(
            keyProperty, countProperty, valueTokenMustExist, enableValueTokenSplitting,
            allowSequentialValues, requiredTokenCount, maximumTokenCount);
    }

    public static IScalarCommandOption<decimal> AddScalarOption<TSource>(
        this TSource source,
        Expression<Func<TSource, IEnumerable<decimal?>?>> keyPropExpr,
        Expression<Func<TSource, int>> countPropExpr,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true,
        bool allowSequentialValues = true, int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var countProperty = source.ValidateExpression(countPropExpr);

        return source.RegisterScalarOption<TSource, decimal>(
            keyProperty, countProperty, valueTokenMustExist, enableValueTokenSplitting,
            allowSequentialValues, requiredTokenCount, maximumTokenCount);
    }
}
