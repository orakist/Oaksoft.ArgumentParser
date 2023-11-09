using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Builder;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Extensions;

public static partial class OptionsExtensions
{
    public static IArgumentParserBuilder<TSource> AddNamedOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, TValue?>> keyPropExpr,
        Action<IScalarNamedOption<TValue>>? configure = null,
        bool mustHaveOneValue = true, bool mandatoryOption = false)
        where TSource : IApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr);

        var option = builder.RegisterNamedOption<TSource, TValue>(keyProperty, mustHaveOneValue, mandatoryOption);

        configure?.Invoke(option);

        return builder;
    }

    public static IArgumentParserBuilder<TSource> AddNamedOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, TValue?>> keyPropExpr,
        Action<IScalarNamedOption<TValue>>? configure = null,
        bool mustHaveOneValue = true, bool mandatoryOption = false)
        where TSource : IApplicationOptions
        where TValue : struct, IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr);

        var option = builder.RegisterNamedOption<TSource, TValue>(keyProperty, mustHaveOneValue, mandatoryOption);

        configure?.Invoke(option);

        return builder;
    }

    public static IArgumentParserBuilder<TSource> AddNamedOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, IEnumerable<TValue?>?>> keyPropExpr,
        Action<ISequentialNamedOption<TValue>>? configure = null, 
        ArityType valueArity = ArityType.ZeroOrMore, ArityType optionArity = ArityType.ZeroOrMore)
        where TSource : IApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr);

        var option = builder.RegisterNamedOption<TSource, TValue>(keyProperty, valueArity, optionArity);

        configure?.Invoke(option);

        return builder;
    }

    public static IArgumentParserBuilder<TSource> AddNamedOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, IEnumerable<TValue?>?>> keyPropExpr,
        Action<ISequentialNamedOption<TValue>>? configure = null,
        ArityType valueArity = ArityType.ZeroOrMore, ArityType optionArity = ArityType.ZeroOrMore)
        where TSource : IApplicationOptions
        where TValue : struct, IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr);

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
        where TSource : IApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr);
        var flagProperty = builder.ValidateExpression(flagPropExpr);

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
        where TSource : IApplicationOptions
        where TValue : struct, IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr);
        var flagProperty = builder.ValidateExpression(flagPropExpr);

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
        where TSource : IApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr);
        var countProperty = builder.ValidateExpression(countPropExpr);

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
        where TSource : IApplicationOptions
        where TValue : struct, IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr);
        var countProperty = builder.ValidateExpression(countPropExpr);

        var option = builder.RegisterNamedOption<TSource, TValue>(keyProperty, countProperty, valueArity, optionArity);

        configure?.Invoke(option);

        return builder;
    }
}
