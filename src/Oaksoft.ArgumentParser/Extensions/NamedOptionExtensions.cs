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
    public static IArgumentParserBuilder<TSource> AddNamedOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, TValue?>> keyPropExpr,
        Action<IScalarNamedOption<TValue>>? configure = null,
        bool mustHaveOneValue = true, bool mandatoryOption = false)
        where TValue : IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr, typeof(TValue).ToString());

        var option = builder.RegisterNamedOption<TSource, TValue>(keyProperty, mustHaveOneValue, mandatoryOption);

        configure?.Invoke(option);

        return builder;
    }

    public static IArgumentParserBuilder<TSource> AddNamedOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, TValue?>> keyPropExpr,
        Action<IScalarNamedOption<TValue>>? configure = null,
        bool mustHaveOneValue = true, bool mandatoryOption = false)
        where TValue : struct, IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr, typeof(TValue).ToString());

        var option = builder.RegisterNamedOption<TSource, TValue>(keyProperty, mustHaveOneValue, mandatoryOption);

        configure?.Invoke(option);

        return builder;
    }

    public static IArgumentParserBuilder<TSource> AddNamedOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, IEnumerable<TValue?>?>> keyPropExpr,
        Action<ISequentialNamedOption<TValue>>? configure = null, 
        ArityType valueArity = ArityType.ZeroOrMore, ArityType optionArity = ArityType.ZeroOrMore)
        where TValue : IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr, typeof(TValue).ToString());

        var option = builder.RegisterNamedOption<TSource, TValue>(keyProperty, valueArity, optionArity);

        configure?.Invoke(option);

        return builder;
    }

    public static IArgumentParserBuilder<TSource> AddNamedOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, IEnumerable<TValue?>?>> keyPropExpr,
        Action<ISequentialNamedOption<TValue>>? configure = null,
        ArityType valueArity = ArityType.ZeroOrMore, ArityType optionArity = ArityType.ZeroOrMore)
        where TValue : struct, IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr, typeof(TValue).ToString());

        var option = builder.RegisterNamedOption<TSource, TValue>(keyProperty, valueArity, optionArity);

        configure?.Invoke(option);

        return builder;
    }

    public static IArgumentParserBuilder<TSource> AddNamedOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, TValue?>> keyPropExpr,
        Expression<Func<TSource, bool>> flagPropExpr,
        Action<IScalarNamedOption<TValue>>? configure = null,
        bool mustHaveOneValue = true, bool mandatoryOption = false)
        where TValue : IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr, typeof(TValue).ToString());
        var flagProperty = builder.ValidateExpression(flagPropExpr, typeof(bool).ToString());

        var option = builder.RegisterNamedOption<TSource, TValue>(keyProperty, flagProperty, mustHaveOneValue, mandatoryOption);

        configure?.Invoke(option);

        return builder;
    }

    public static IArgumentParserBuilder<TSource> AddNamedOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, TValue?>> keyPropExpr,
        Expression<Func<TSource, bool>> flagPropExpr,
        Action<IScalarNamedOption<TValue>>? configure = null,
        bool mustHaveOneValue = true, bool mandatoryOption = false)
        where TValue : struct, IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr, typeof(TValue).ToString());
        var flagProperty = builder.ValidateExpression(flagPropExpr, typeof(bool).ToString());

        var option = builder.RegisterNamedOption<TSource, TValue>(keyProperty, flagProperty, mustHaveOneValue, mandatoryOption);

        configure?.Invoke(option);

        return builder;
    }

    public static IArgumentParserBuilder<TSource> AddNamedOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, IEnumerable<TValue?>?>> keyPropExpr,
        Expression<Func<TSource, int>> countPropExpr,
        Action<ISequentialNamedOption<TValue>>? configure = null,
        ArityType valueArity = ArityType.ZeroOrMore, ArityType optionArity = ArityType.ZeroOrMore)
        where TValue : IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr, typeof(TValue).ToString());
        var countProperty = builder.ValidateExpression(countPropExpr, typeof(int).ToString());

        var option = builder.RegisterNamedOption<TSource, TValue>(keyProperty, countProperty, valueArity, optionArity);

        configure?.Invoke(option);

        return builder;
    }

    public static IArgumentParserBuilder<TSource> AddNamedOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, IEnumerable<TValue?>?>> keyPropExpr,
        Expression<Func<TSource, int>> countPropExpr,
        Action<ISequentialNamedOption<TValue>>? configure = null,
        ArityType valueArity = ArityType.ZeroOrMore, ArityType optionArity = ArityType.ZeroOrMore)
        where TValue : struct, IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr, typeof(TValue).ToString());
        var countProperty = builder.ValidateExpression(countPropExpr, typeof(int).ToString());

        var option = builder.RegisterNamedOption<TSource, TValue>(keyProperty, countProperty, valueArity, optionArity);

        configure?.Invoke(option);

        return builder;
    }
}
