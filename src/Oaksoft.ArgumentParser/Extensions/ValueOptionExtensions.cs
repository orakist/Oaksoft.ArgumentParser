using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Builder;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Extensions;

public static partial class OptionExtensions
{ 
    public static IArgumentParserBuilder<TSource> AddValueOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder, 
        Expression<Func<TSource, TValue?>> keyPropExpr,
        Action<IScalarValueOption<TValue>>? configure = null,
        bool mustHaveOneValue = false)
        where TSource : IApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr, typeof(TValue).ToString());

        var option = builder.RegisterValueOption<TSource, TValue>(keyProperty, mustHaveOneValue);

        configure?.Invoke(option);

        return builder;
    }

    public static IArgumentParserBuilder<TSource> AddValueOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, TValue?>> keyPropExpr,
        Action<IScalarValueOption<TValue>>? configure = null,
        bool mustHaveOneValue = false)
        where TSource : IApplicationOptions
        where TValue : struct, IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr, typeof(TValue).ToString());

        var option = builder.RegisterValueOption<TSource, TValue>(keyProperty, mustHaveOneValue);

        configure?.Invoke(option);

        return builder;
    }

    public static IArgumentParserBuilder<TSource> AddValueOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, IEnumerable<TValue?>?>> keyPropExpr,
        Action<ISequentialValueOption<TValue>>? configure = null,
        ArityType valueArity = ArityType.ZeroOrMore)
        where TSource : IApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr, typeof(TValue).ToString());

        var option = builder.RegisterValueOption<TSource, TValue>(keyProperty, valueArity);

        configure?.Invoke(option);

        return builder;
    }

    public static IArgumentParserBuilder<TSource> AddValueOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, IEnumerable<TValue?>?>> keyPropExpr,
        Action<ISequentialValueOption<TValue>>? configure = null,
        ArityType valueArity = ArityType.ZeroOrMore)
        where TSource : IApplicationOptions
        where TValue : struct, IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr, typeof(TValue).ToString());

        var option = builder.RegisterValueOption<TSource, TValue>(keyProperty, valueArity);

        configure?.Invoke(option);

        return builder;
    }

    public static IArgumentParserBuilder<TSource> AddValueOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, TValue?>> keyPropExpr,
        Expression<Func<TSource, bool>> flagPropExpr,
        Action<IScalarValueOption<TValue>>? configure = null,
        bool mustHaveOneValue = false)
        where TSource : IApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr, typeof(TValue).ToString());
        var flagProperty = builder.ValidateExpression(flagPropExpr, typeof(bool).ToString());

        var option = builder.RegisterValueOption<TSource, TValue>(keyProperty, flagProperty, mustHaveOneValue);

        configure?.Invoke(option);

        return builder;
    }

    public static IArgumentParserBuilder<TSource> AddValueOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, TValue?>> keyPropExpr,
        Expression<Func<TSource, bool>> flagPropExpr,
        Action<IScalarValueOption<TValue>>? configure = null,
        bool mustHaveOneValue = false)
        where TSource : IApplicationOptions
        where TValue : struct, IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr, typeof(TValue).ToString());
        var flagProperty = builder.ValidateExpression(flagPropExpr, typeof(bool).ToString());

        var option = builder.RegisterValueOption<TSource, TValue>(keyProperty, flagProperty, mustHaveOneValue);

        configure?.Invoke(option);

        return builder;
    }

    public static IArgumentParserBuilder<TSource> AddValueOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, IEnumerable<TValue?>?>> keyPropExpr,
        Expression<Func<TSource, int>> countPropExpr,
        Action<ISequentialValueOption<TValue>>? configure = null,
        ArityType valueArity = ArityType.ZeroOrMore)
        where TSource : IApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr, typeof(TValue).ToString());
        var countProperty = builder.ValidateExpression(countPropExpr, typeof(int).ToString());

        var option = builder.RegisterValueOption<TSource, TValue>(keyProperty, countProperty, valueArity);

        configure?.Invoke(option);

        return builder;
    }

    public static IArgumentParserBuilder<TSource> AddValueOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, IEnumerable<TValue?>?>> keyPropExpr,
        Expression<Func<TSource, int>> countPropExpr,
        Action<ISequentialValueOption<TValue>>? configure = null,
        ArityType valueArity = ArityType.ZeroOrMore)
        where TSource : IApplicationOptions
        where TValue : struct, IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr, typeof(TValue).ToString());
        var countProperty = builder.ValidateExpression(countPropExpr, typeof(int).ToString());

        var option = builder.RegisterValueOption<TSource, TValue>(keyProperty, countProperty, valueArity);

        configure?.Invoke(option);

        return builder;
    }
}
