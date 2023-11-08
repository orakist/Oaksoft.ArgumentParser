﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Extensions;

public static partial class OptionsExtensions
{ 
    public static IScalarValueOption<TValue> AddValueOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder, 
        Expression<Func<TSource, TValue?>> keyPropExpr,
        bool mustHaveOneValue = false)
        where TSource : IApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr);

        return builder.RegisterValueOption<TSource, TValue>(keyProperty, mustHaveOneValue);
    }

    public static IScalarValueOption<TValue> AddValueOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, TValue?>> keyPropExpr,
        bool mustHaveOneValue = false)
        where TSource : IApplicationOptions
        where TValue : struct, IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr);

        return builder.RegisterValueOption<TSource, TValue>(keyProperty, mustHaveOneValue);
    }

    public static ISequentialValueOption<TValue> AddValueOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, IEnumerable<TValue?>?>> keyPropExpr,
        bool enableValueTokenSplitting = true, ArityType valueArity = ArityType.ZeroOrMore)
        where TSource : IApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr);

        return builder.RegisterValueOption<TSource, TValue>(keyProperty, enableValueTokenSplitting, valueArity);
    }

    public static ISequentialValueOption<TValue> AddValueOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, IEnumerable<TValue?>?>> keyPropExpr,
        bool enableValueTokenSplitting = true, ArityType valueArity = ArityType.ZeroOrMore)
        where TSource : IApplicationOptions
        where TValue : struct, IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr);

        return builder.RegisterValueOption<TSource, TValue>(keyProperty, enableValueTokenSplitting, valueArity);
    }

    public static IScalarValueOption<TValue> AddValueOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, TValue?>> keyPropExpr,
        Expression<Func<TSource, bool>> flagPropExpr,
        bool mustHaveOneValue = false)
        where TSource : IApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr);
        var flagProperty = builder.ValidateExpression(flagPropExpr);

        return builder.RegisterValueOption<TSource, TValue>(keyProperty, flagProperty, mustHaveOneValue);
    }

    public static IScalarValueOption<TValue> AddValueOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, TValue?>> keyPropExpr,
        Expression<Func<TSource, bool>> flagPropExpr,
        bool mustHaveOneValue = false)
        where TSource : IApplicationOptions
        where TValue : struct, IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr);
        var flagProperty = builder.ValidateExpression(flagPropExpr);

        return builder.RegisterValueOption<TSource, TValue>(keyProperty, flagProperty, mustHaveOneValue);
    }

    public static ISequentialValueOption<TValue> AddValueOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, IEnumerable<TValue?>?>> keyPropExpr,
        Expression<Func<TSource, int>> countPropExpr,
        bool enableValueTokenSplitting = true, ArityType valueArity = ArityType.ZeroOrMore)
        where TSource : IApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr);
        var countProperty = builder.ValidateExpression(countPropExpr);

        return builder.RegisterValueOption<TSource, TValue>(keyProperty, countProperty, enableValueTokenSplitting, valueArity);
    }

    public static ISequentialValueOption<TValue> AddValueOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, IEnumerable<TValue?>?>> keyPropExpr,
        Expression<Func<TSource, int>> countPropExpr,
        bool enableValueTokenSplitting = true, ArityType valueArity = ArityType.ZeroOrMore)
        where TSource : IApplicationOptions
        where TValue : struct, IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr);
        var countProperty = builder.ValidateExpression(countPropExpr);

        return builder.RegisterValueOption<TSource, TValue>(keyProperty, countProperty, enableValueTokenSplitting, valueArity);
    }
}
