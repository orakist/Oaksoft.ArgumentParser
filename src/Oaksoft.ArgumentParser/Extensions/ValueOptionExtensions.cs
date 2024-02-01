using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Builder;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Options;

namespace Oaksoft.ArgumentParser.Extensions;

public static partial class OptionExtensions
{
    /// <summary>
    /// Registers a scalar value option.
    /// </summary>
    public static IArgumentParserBuilder<TSource> AddValueOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder, 
        Expression<Func<TSource, TValue?>> keyPropExpr,
        Action<IScalarValueOption<TValue>>? configure = null,
        bool mustHaveOneValue = false)
        where TValue : IComparable
        where TSource : new()
    {
        var keyProperty = keyPropExpr.ValidateExpression(typeof(TValue).ToString());

        var option = builder.RegisterValueOption<TValue>(keyProperty, mustHaveOneValue);

        configure?.Invoke(option);

        return builder;
    }

    /// <summary>
    /// Registers a scalar value option.
    /// </summary>
    public static IArgumentParserBuilder<TSource> AddValueOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, TValue?>> keyPropExpr,
        Action<IScalarValueOption<TValue>>? configure = null,
        bool mustHaveOneValue = false)
        where TValue : struct, IComparable
        where TSource : new()
    {
        var keyProperty = keyPropExpr.ValidateExpression(typeof(TValue).ToString());

        var option = builder.RegisterValueOption<TValue>(keyProperty, mustHaveOneValue);

        configure?.Invoke(option);

        return builder;
    }

    /// <summary>
    /// Registers a sequential value option.
    /// </summary>
    public static IArgumentParserBuilder<TSource> AddValueOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, IEnumerable<TValue?>?>> keyPropExpr,
        Action<ISequentialValueOption<TValue>>? configure = null,
        ArityType valueArity = ArityType.ZeroOrMore)
        where TValue : IComparable
        where TSource : new()
    {
        var keyProperty = keyPropExpr.ValidateExpression(typeof(TValue).ToString());

        var option = builder.RegisterValueOption<TValue>(keyProperty, valueArity);

        configure?.Invoke(option);

        return builder;
    }

    /// <summary>
    /// Registers a sequential value option.
    /// </summary>
    public static IArgumentParserBuilder<TSource> AddValueOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, IEnumerable<TValue?>?>> keyPropExpr,
        Action<ISequentialValueOption<TValue>>? configure = null,
        ArityType valueArity = ArityType.ZeroOrMore)
        where TValue : struct, IComparable
        where TSource : new()
    {
        var keyProperty = keyPropExpr.ValidateExpression(typeof(TValue).ToString());

        var option = builder.RegisterValueOption<TValue>(keyProperty, valueArity);

        configure?.Invoke(option);

        return builder;
    }
}
