using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Numerics;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Extensions;

public static partial class OptionsExtensions
{
    public static IScalarCommandOption AddScalarOption<TSource>(
        this TSource source, Expression<Func<TSource, string?>> keyPropExpr,
        bool valueTokenMustExist = true, bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        return source.RegisterScalarOption(keyPropExpr, valueTokenMustExist, mandatory);
    }

    public static IScalarCommandOption AddScalarOption<TSource>(
        this TSource source,
        Expression<Func<TSource, IEnumerable<string>?>> keyPropExpr,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true,
        bool allowSequentialValues = true, int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        return source.RegisterScalarOption(
            keyPropExpr, valueTokenMustExist, enableValueTokenSplitting,
            allowSequentialValues, requiredTokenCount, maximumTokenCount);
    }

    public static IScalarCommandOption AddScalarOption<TSource>(
        this TSource source,
        Expression<Func<TSource, string?>> keyPropExpr,
        Expression<Func<TSource, bool>> flagPropExpr,
        bool valueTokenMustExist = true,
        bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        return source.RegisterScalarOption(keyPropExpr, flagPropExpr, valueTokenMustExist, mandatory);
    }

    public static IScalarCommandOption AddScalarOption<TSource>(
        this TSource source,
        Expression<Func<TSource, IEnumerable<string>?>> keyPropExpr,
        Expression<Func<TSource, int>> countPropExpr,
        bool valueTokenMustExist = false, bool enableValueTokenSplitting = true,
        bool allowSequentialValues = true, int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        return source.RegisterScalarOption(
            keyPropExpr, countPropExpr, valueTokenMustExist, enableValueTokenSplitting,
            allowSequentialValues, requiredTokenCount, maximumTokenCount);
    }
}
