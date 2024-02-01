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
    /// Registers a scalar named option.
    /// </summary>
    public static IArgumentParserBuilder<TSource> AddNamedOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, TValue?>> keyPropExpr,
        Action<IScalarNamedOption<TValue>>? configure = null,
        bool mustHaveOneValue = true, bool mandatoryOption = false)
        where TValue : IComparable
        where TSource : new()
    {
        var keyProperty = keyPropExpr.ValidateExpression(typeof(TValue).ToString());

        var option = builder.RegisterNamedOption<TValue>(keyProperty, mustHaveOneValue, mandatoryOption);

        configure?.Invoke(option);

        return builder;
    }

    /// <summary>
    /// Registers a scalar named option.
    /// </summary>
    public static IArgumentParserBuilder<TSource> AddNamedOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, TValue?>> keyPropExpr,
        Action<IScalarNamedOption<TValue>>? configure = null,
        bool mustHaveOneValue = true, bool mandatoryOption = false)
        where TValue : struct, IComparable
        where TSource : new()
    {
        var keyProperty = keyPropExpr.ValidateExpression(typeof(TValue).ToString());

        var option = builder.RegisterNamedOption<TValue>(keyProperty, mustHaveOneValue, mandatoryOption);

        configure?.Invoke(option);

        return builder;
    }

    /// <summary>
    /// Registers a sequential named option.
    /// </summary>
    public static IArgumentParserBuilder<TSource> AddNamedOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, IEnumerable<TValue?>?>> keyPropExpr,
        Action<ISequentialNamedOption<TValue>>? configure = null, 
        ArityType valueArity = ArityType.ZeroOrMore, ArityType optionArity = ArityType.ZeroOrMore)
        where TValue : IComparable
        where TSource : new()
    {
        var keyProperty = keyPropExpr.ValidateExpression(typeof(TValue).ToString());

        var option = builder.RegisterNamedOption<TValue>(keyProperty, valueArity, optionArity);

        configure?.Invoke(option);

        return builder;
    }

    /// <summary>
    /// Registers a sequential named option.
    /// </summary>
    public static IArgumentParserBuilder<TSource> AddNamedOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, IEnumerable<TValue?>?>> keyPropExpr,
        Action<ISequentialNamedOption<TValue>>? configure = null,
        ArityType valueArity = ArityType.ZeroOrMore, ArityType optionArity = ArityType.ZeroOrMore)
        where TValue : struct, IComparable
        where TSource : new()
    {
        var keyProperty = keyPropExpr.ValidateExpression(typeof(TValue).ToString());

        var option = builder.RegisterNamedOption<TValue>(keyProperty, valueArity, optionArity);

        configure?.Invoke(option);

        return builder;
    }
}
