using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Extensions;

public static partial class OptionsExtensions
{
    public static IScalarCommandOption<int> AddScalarOption<TSource>(
        this TSource source, Expression<Func<TSource, int>> keyPropExpr,
        bool valueTokenMustExist = true, bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        return source.RegisterScalarOption<TSource, int>(
            keyProperty, valueTokenMustExist, mandatory);
    }

    public static IScalarCommandOption<int> AddScalarOption<TSource>(
        this TSource source, Expression<Func<TSource, int?>> keyPropExpr,
        bool valueTokenMustExist = true, bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        return source.RegisterScalarOption<TSource, int>(
            keyProperty, valueTokenMustExist, mandatory);
    }

    public static IScalarCommandOption<short> AddScalarOption<TSource>(
        this TSource source, Expression<Func<TSource, short>> keyPropExpr,
        bool valueTokenMustExist = true, bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        return source.RegisterScalarOption<TSource, short>(
            keyProperty, valueTokenMustExist, mandatory);
    }

    public static IScalarCommandOption<short> AddScalarOption<TSource>(
        this TSource source, Expression<Func<TSource, short?>> keyPropExpr,
        bool valueTokenMustExist = true, bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        return source.RegisterScalarOption<TSource, short>(
            keyProperty, valueTokenMustExist, mandatory);
    }

    public static IScalarCommandOption<long> AddScalarOption<TSource>(
        this TSource source, Expression<Func<TSource, long>> keyPropExpr,
        bool valueTokenMustExist = true, bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        return source.RegisterScalarOption<TSource, long>(
            keyProperty, valueTokenMustExist, mandatory);
    }

    public static IScalarCommandOption<long> AddScalarOption<TSource>(
        this TSource source, Expression<Func<TSource, long?>> keyPropExpr,
        bool valueTokenMustExist = true, bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        return source.RegisterScalarOption<TSource, long>(
            keyProperty, valueTokenMustExist, mandatory);
    }

    public static IScalarCommandOption<int> AddScalarOption<TSource>(
        this TSource source, Expression<Func<TSource, IEnumerable<int>?>> keyPropExpr,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true,
        bool allowSequentialValues = true, int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        return source.RegisterScalarOption<TSource, int>(
            keyProperty, valueTokenMustExist, enableValueTokenSplitting, 
            allowSequentialValues, requiredTokenCount, maximumTokenCount);
    }

    public static IScalarCommandOption<int> AddScalarOption<TSource>(
        this TSource source, Expression<Func<TSource, IEnumerable<int?>?>> keyPropExpr,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true,
        bool allowSequentialValues = true, int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        return source.RegisterScalarOption<TSource, int>(
            keyProperty, valueTokenMustExist, enableValueTokenSplitting,
            allowSequentialValues, requiredTokenCount, maximumTokenCount);
    }

    public static IScalarCommandOption<short> AddScalarOption<TSource>(
        this TSource source, Expression<Func<TSource, IEnumerable<short>?>> keyPropExpr,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true,
        bool allowSequentialValues = true, int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        return source.RegisterScalarOption<TSource, short>(
            keyProperty, valueTokenMustExist, enableValueTokenSplitting,
            allowSequentialValues, requiredTokenCount, maximumTokenCount);
    }

    public static IScalarCommandOption<short> AddScalarOption<TSource>(
        this TSource source, Expression<Func<TSource, IEnumerable<short?>?>> keyPropExpr,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true,
        bool allowSequentialValues = true, int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        return source.RegisterScalarOption<TSource, short>(
            keyProperty, valueTokenMustExist, enableValueTokenSplitting,
            allowSequentialValues, requiredTokenCount, maximumTokenCount);
    }

    public static IScalarCommandOption<long> AddScalarOption<TSource>(
        this TSource source, Expression<Func<TSource, IEnumerable<long>?>> keyPropExpr,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true,
        bool allowSequentialValues = true, int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        return source.RegisterScalarOption<TSource, long>(
            keyProperty, valueTokenMustExist, enableValueTokenSplitting,
            allowSequentialValues, requiredTokenCount, maximumTokenCount);
    }

    public static IScalarCommandOption<long> AddScalarOption<TSource>(
        this TSource source, Expression<Func<TSource, IEnumerable<long?>?>> keyPropExpr,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true,
        bool allowSequentialValues = true, int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        return source.RegisterScalarOption<TSource, long>(
            keyProperty, valueTokenMustExist, enableValueTokenSplitting,
            allowSequentialValues, requiredTokenCount, maximumTokenCount);
    }

    public static IScalarCommandOption<int> AddScalarOption<TSource>(
        this TSource source, 
        Expression<Func<TSource, int>> keyPropExpr,
        Expression<Func<TSource, bool>> flagPropExpr,
        bool valueTokenMustExist = true, bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var flagProperty = source.ValidateExpression(flagPropExpr);

        return source.RegisterScalarOption<TSource, int>(
            keyProperty, flagProperty, valueTokenMustExist, mandatory);
    }

    public static IScalarCommandOption<int> AddScalarOption<TSource>(
        this TSource source,
        Expression<Func<TSource, int?>> keyPropExpr,
        Expression<Func<TSource, bool>> flagPropExpr,
        bool valueTokenMustExist = true, bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var flagProperty = source.ValidateExpression(flagPropExpr);

        return source.RegisterScalarOption<TSource, int>(
            keyProperty, flagProperty, valueTokenMustExist, mandatory);
    }

    public static IScalarCommandOption<short> AddScalarOption<TSource>(
        this TSource source,
        Expression<Func<TSource, short>> keyPropExpr,
        Expression<Func<TSource, bool>> flagPropExpr,
        bool valueTokenMustExist = true, bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var flagProperty = source.ValidateExpression(flagPropExpr);

        return source.RegisterScalarOption<TSource, short>(
            keyProperty, flagProperty, valueTokenMustExist, mandatory);
    }

    public static IScalarCommandOption<short> AddScalarOption<TSource>(
        this TSource source,
        Expression<Func<TSource, short?>> keyPropExpr,
        Expression<Func<TSource, bool>> flagPropExpr,
        bool valueTokenMustExist = true, bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var flagProperty = source.ValidateExpression(flagPropExpr);

        return source.RegisterScalarOption<TSource, short>(
            keyProperty, flagProperty, valueTokenMustExist, mandatory);
    }
    
    public static IScalarCommandOption<long> AddScalarOption<TSource>(
        this TSource source,
        Expression<Func<TSource, long>> keyPropExpr,
        Expression<Func<TSource, bool>> flagPropExpr,
        bool valueTokenMustExist = true, bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var flagProperty = source.ValidateExpression(flagPropExpr);

        return source.RegisterScalarOption<TSource, long>(
            keyProperty, flagProperty, valueTokenMustExist, mandatory);
    }

    public static IScalarCommandOption<long> AddScalarOption<TSource>(
        this TSource source,
        Expression<Func<TSource, long?>> keyPropExpr,
        Expression<Func<TSource, bool>> flagPropExpr,
        bool valueTokenMustExist = true, bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var flagProperty = source.ValidateExpression(flagPropExpr);

        return source.RegisterScalarOption<TSource, long>(
            keyProperty, flagProperty, valueTokenMustExist, mandatory);
    }

    public static IScalarCommandOption<int> AddScalarOption<TSource>(
        this TSource source,
        Expression<Func<TSource, IEnumerable<int>?>> keyPropExpr,
        Expression<Func<TSource, int>> countPropExpr,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true,
        bool allowSequentialValues = true, int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var countProperty = source.ValidateExpression(countPropExpr);

        return source.RegisterScalarOption<TSource, int>(
            keyProperty, countProperty, valueTokenMustExist, enableValueTokenSplitting, 
            allowSequentialValues, requiredTokenCount, maximumTokenCount);
    }

    public static IScalarCommandOption<int> AddScalarOption<TSource>(
        this TSource source,
        Expression<Func<TSource, IEnumerable<int?>?>> keyPropExpr,
        Expression<Func<TSource, int>> countPropExpr,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true,
        bool allowSequentialValues = true, int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var countProperty = source.ValidateExpression(countPropExpr);

        return source.RegisterScalarOption<TSource, int>(
            keyProperty, countProperty, valueTokenMustExist, enableValueTokenSplitting,
            allowSequentialValues, requiredTokenCount, maximumTokenCount);
    }

    public static IScalarCommandOption<short> AddScalarOption<TSource>(
        this TSource source,
        Expression<Func<TSource, IEnumerable<short>?>> keyPropExpr,
        Expression<Func<TSource, int>> countPropExpr,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true,
        bool allowSequentialValues = true, int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var countProperty = source.ValidateExpression(countPropExpr);

        return source.RegisterScalarOption<TSource, short>(
            keyProperty, countProperty, valueTokenMustExist, enableValueTokenSplitting,
            allowSequentialValues, requiredTokenCount, maximumTokenCount);
    }

    public static IScalarCommandOption<short> AddScalarOption<TSource>(
        this TSource source,
        Expression<Func<TSource, IEnumerable<short?>?>> keyPropExpr,
        Expression<Func<TSource, int>> countPropExpr,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true,
        bool allowSequentialValues = true, int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var countProperty = source.ValidateExpression(countPropExpr);

        return source.RegisterScalarOption<TSource, short>(
            keyProperty, countProperty, valueTokenMustExist, enableValueTokenSplitting,
            allowSequentialValues, requiredTokenCount, maximumTokenCount);
    }

    public static IScalarCommandOption<long> AddScalarOption<TSource>(
        this TSource source,
        Expression<Func<TSource, IEnumerable<long>?>> keyPropExpr,
        Expression<Func<TSource, int>> countPropExpr,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true,
        bool allowSequentialValues = true, int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var countProperty = source.ValidateExpression(countPropExpr);

        return source.RegisterScalarOption<TSource, long>(
            keyProperty, countProperty, valueTokenMustExist, enableValueTokenSplitting,
            allowSequentialValues, requiredTokenCount, maximumTokenCount);
    }

    public static IScalarCommandOption<long> AddScalarOption<TSource>(
        this TSource source,
        Expression<Func<TSource, IEnumerable<long?>?>> keyPropExpr,
        Expression<Func<TSource, int>> countPropExpr,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true,
        bool allowSequentialValues = true, int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var countProperty = source.ValidateExpression(countPropExpr);

        return source.RegisterScalarOption<TSource, long>(
            keyProperty, countProperty, valueTokenMustExist, enableValueTokenSplitting,
            allowSequentialValues, requiredTokenCount, maximumTokenCount);
    }
}
