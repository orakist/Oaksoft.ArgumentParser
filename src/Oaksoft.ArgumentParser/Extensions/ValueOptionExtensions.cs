using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Extensions;

public static partial class OptionsExtensions
{ 
    public static IValueOption<string> AddValueOption<TSource>(
        this TSource source,
        Expression<Func<TSource, string?>> keyPropExpr,
        bool mustHaveOneValue = false)
        where TSource : BaseApplicationOptions
    {
        return source.RegisterValueOption(keyPropExpr, mustHaveOneValue);
    }

    public static IValueOption<string> AddValueOption<TSource>(
        this TSource source,
        Expression<Func<TSource, IEnumerable<string>?>> keyPropExpr,
        bool enableValueTokenSplitting = true, ArityType valueArity = ArityType.ZeroOrMore)
        where TSource : BaseApplicationOptions
    {
        return source.RegisterValueOption(keyPropExpr, enableValueTokenSplitting, valueArity);
    }

    public static IValueOption<string> AddValueOption<TSource>(
        this TSource source,
        Expression<Func<TSource, string?>> keyPropExpr,
        Expression<Func<TSource, bool>> flagPropExpr,
        bool mustHaveOneValue = false)
        where TSource : BaseApplicationOptions
    {
        return source.RegisterValueOption(keyPropExpr, flagPropExpr, mustHaveOneValue);
    }

    public static IValueOption<string> AddValueOption<TSource>(
        this TSource source,
        Expression<Func<TSource, IEnumerable<string>?>> keyPropExpr,
        Expression<Func<TSource, int>> countPropExpr,
        bool enableValueTokenSplitting = true, ArityType valueArity = ArityType.ZeroOrMore)
        where TSource : BaseApplicationOptions
    {
        return source.RegisterValueOption(keyPropExpr, countPropExpr, enableValueTokenSplitting, valueArity);
    }
}
