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
    public static IScalarNamedOption<TValue> AddNamedOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, TValue?>> keyPropExpr,
        bool mustHaveOneValue = true, bool mandatoryOption = false)
        where TSource : IApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr);

        return builder.RegisterNamedOption<TSource, TValue>(
            keyProperty, mustHaveOneValue, mandatoryOption);
    }

    public static IScalarNamedOption<TValue> AddNamedOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, TValue?>> keyPropExpr,
        bool mustHaveOneValue = true, bool mandatoryOption = false)
        where TSource : IApplicationOptions
        where TValue : struct, IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr);

        return builder.RegisterNamedOption<TSource, TValue>(
            keyProperty, mustHaveOneValue, mandatoryOption);
    }

    public static ISequentialNamedOption<TValue> AddNamedOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, IEnumerable<TValue?>?>> keyPropExpr,
        bool enableValueTokenSplitting = true, bool allowSequentialValues = true, 
        ArityType valueArity = ArityType.ZeroOrMore, ArityType optionArity = ArityType.ZeroOrMore)
        where TSource : IApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr);

        return builder.RegisterNamedOption<TSource, TValue>(
            keyProperty, enableValueTokenSplitting, allowSequentialValues, 
            valueArity, optionArity);
    }

    public static ISequentialNamedOption<TValue> AddNamedOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, IEnumerable<TValue?>?>> keyPropExpr,
        bool enableValueTokenSplitting = true, bool allowSequentialValues = true, 
        ArityType valueArity = ArityType.ZeroOrMore, ArityType optionArity = ArityType.ZeroOrMore)
        where TSource : IApplicationOptions
        where TValue : struct, IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr);

        return builder.RegisterNamedOption<TSource, TValue>(
            keyProperty, enableValueTokenSplitting, allowSequentialValues, 
            valueArity, optionArity);
    }

    public static IScalarNamedOption<TValue> AddNamedOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, TValue?>> keyPropExpr,
        Expression<Func<TSource, bool>> flagPropExpr,
        bool mustHaveOneValue = true, bool mandatoryOption = false)
        where TSource : IApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr);
        var flagProperty = builder.ValidateExpression(flagPropExpr);

        return builder.RegisterNamedOption<TSource, TValue>(
            keyProperty, flagProperty, mustHaveOneValue, mandatoryOption);
    }

    public static IScalarNamedOption<TValue> AddNamedOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, TValue?>> keyPropExpr,
        Expression<Func<TSource, bool>> flagPropExpr,
        bool mustHaveOneValue = true, bool mandatoryOption = false)
        where TSource : IApplicationOptions
        where TValue : struct, IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr);
        var flagProperty = builder.ValidateExpression(flagPropExpr);

        return builder.RegisterNamedOption<TSource, TValue>(
            keyProperty, flagProperty, mustHaveOneValue, mandatoryOption);
    }

    public static ISequentialNamedOption<TValue> AddNamedOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, IEnumerable<TValue?>?>> keyPropExpr,
        Expression<Func<TSource, int>> countPropExpr,
        bool enableValueTokenSplitting = true, bool allowSequentialValues = true,
        ArityType valueArity = ArityType.ZeroOrMore, ArityType optionArity = ArityType.ZeroOrMore)
        where TSource : IApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr);
        var countProperty = builder.ValidateExpression(countPropExpr);

        return builder.RegisterNamedOption<TSource, TValue>(
            keyProperty, countProperty, enableValueTokenSplitting, 
            allowSequentialValues, valueArity, optionArity);
    }

    public static ISequentialNamedOption<TValue> AddNamedOption<TSource, TValue>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, IEnumerable<TValue?>?>> keyPropExpr,
        Expression<Func<TSource, int>> countPropExpr,
        bool enableValueTokenSplitting = true, bool allowSequentialValues = true, 
        ArityType valueArity = ArityType.ZeroOrMore, ArityType optionArity = ArityType.ZeroOrMore)
        where TSource : IApplicationOptions
        where TValue : struct, IComparable, IEquatable<TValue>
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr);
        var countProperty = builder.ValidateExpression(countPropExpr);

        return builder.RegisterNamedOption<TSource, TValue>(
            keyProperty, countProperty, enableValueTokenSplitting, 
            allowSequentialValues, valueArity, optionArity);
    }
}
