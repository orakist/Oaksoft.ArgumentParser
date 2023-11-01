using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Extensions;

public static partial class OptionsExtensions
{ 
    public static INonCommandOption AddDefaultOption<TSource>(
        this TSource source,
        Expression<Func<TSource, string?>> keyPropExpr,
        bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        return source.RegisterDefaultOption(keyPropExpr, mandatory);
    }

    public static INonCommandOption AddDefaultOption<TSource>(
        this TSource source,
        Expression<Func<TSource, IEnumerable<string>?>> keyPropExpr,
        bool enableValueTokenSplitting = true, 
        int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        return source.RegisterDefaultOption(
            keyPropExpr, enableValueTokenSplitting, requiredTokenCount, maximumTokenCount);
    }

    public static INonCommandOption AddDefaultOption<TSource>(
        this TSource source,
        Expression<Func<TSource, string?>> keyPropExpr,
        Expression<Func<TSource, bool>> countPropExpr,
        bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        return source.RegisterDefaultOption(keyPropExpr, countPropExpr, mandatory);
    }

    public static INonCommandOption AddDefaultOption<TSource>(
        this TSource source,
        Expression<Func<TSource, IEnumerable<string>?>> keyPropExpr,
        Expression<Func<TSource, int>> countPropExpr,
        bool enableValueTokenSplitting = true, 
        int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        return source.RegisterDefaultOption(
            keyPropExpr, countPropExpr, enableValueTokenSplitting, 
            requiredTokenCount, maximumTokenCount);
    }
}
