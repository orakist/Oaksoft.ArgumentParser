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
    public static IScalarOption<TValue> AddScalarOption<TSource, TValue>(
        this TSource source, Expression<Func<TSource, TValue?>> keyPropExpr,
        bool mustHaveOneValue = true, bool mandatoryOption = false)
        where TSource : BaseApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        return source.RegisterScalarOption<TSource, TValue>(
            keyProperty, mustHaveOneValue, mandatoryOption);
    }

    public static IScalarOption<TValue> AddScalarOption<TSource, TValue>(
        this TSource source, Expression<Func<TSource, TValue?>> keyPropExpr,
        bool mustHaveOneValue = true, bool mandatoryOption = false)
        where TSource : BaseApplicationOptions
        where TValue : struct, IComparable, IEquatable<TValue>
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        return source.RegisterScalarOption<TSource, TValue>(
            keyProperty, mustHaveOneValue, mandatoryOption);
    }

    public static IScalarOption<TValue> AddScalarOption<TSource, TValue>(
        this TSource source,
        Expression<Func<TSource, IEnumerable<TValue>?>> keyPropExpr,
        bool enableValueTokenSplitting = true, bool allowSequentialValues = true, 
        ArityType valueArity = ArityType.ZeroOrMore, ArityType optionArity = ArityType.ZeroOrMore)
        where TSource : BaseApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        return source.RegisterScalarOption<TSource, TValue>(
            keyProperty, enableValueTokenSplitting, allowSequentialValues, valueArity, optionArity);
    }

    public static IScalarOption<TValue> AddScalarOption<TSource, TValue>(
        this TSource source,
        Expression<Func<TSource, IEnumerable<TValue?>?>> keyPropExpr,
        bool enableValueTokenSplitting = true, bool allowSequentialValues = true, 
        ArityType valueArity = ArityType.ZeroOrMore, ArityType optionArity = ArityType.ZeroOrMore)
        where TSource : BaseApplicationOptions
        where TValue : struct, IComparable, IEquatable<TValue>
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);

        return source.RegisterScalarOption<TSource, TValue>(
            keyProperty, enableValueTokenSplitting, allowSequentialValues, valueArity, optionArity);
    }

    public static IScalarOption<TValue> AddScalarOption<TSource, TValue>(
        this TSource source,
        Expression<Func<TSource, TValue?>> keyPropExpr,
        Expression<Func<TSource, bool>> flagPropExpr,
        bool mustHaveOneValue = true, bool mandatoryOption = false)
        where TSource : BaseApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var flagProperty = source.ValidateExpression(flagPropExpr);

        return source.RegisterScalarOption<TSource, TValue>(
            keyProperty, flagProperty, mustHaveOneValue, mandatoryOption);
    }

    public static IScalarOption<TValue> AddScalarOption<TSource, TValue>(
        this TSource source,
        Expression<Func<TSource, TValue?>> keyPropExpr,
        Expression<Func<TSource, bool>> flagPropExpr,
        bool mustHaveOneValue = true, bool mandatoryOption = false)
        where TSource : BaseApplicationOptions
        where TValue : struct, IComparable, IEquatable<TValue>
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var flagProperty = source.ValidateExpression(flagPropExpr);

        return source.RegisterScalarOption<TSource, TValue>(
            keyProperty, flagProperty, mustHaveOneValue, mandatoryOption);
    }

    public static IScalarOption<TValue> AddScalarOption<TSource, TValue>(
        this TSource source,
        Expression<Func<TSource, IEnumerable<TValue>?>> keyPropExpr,
        Expression<Func<TSource, int>> countPropExpr,
        bool enableValueTokenSplitting = true, bool allowSequentialValues = true,
        ArityType valueArity = ArityType.ZeroOrMore, ArityType optionArity = ArityType.ZeroOrMore)
        where TSource : BaseApplicationOptions
        where TValue : IComparable, IEquatable<TValue>
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var countProperty = source.ValidateExpression(countPropExpr);

        return source.RegisterScalarOption<TSource, TValue>(
            keyProperty, countProperty, enableValueTokenSplitting, allowSequentialValues, valueArity, optionArity);
    }

    public static IScalarOption<TValue> AddScalarOption<TSource, TValue>(
        this TSource source,
        Expression<Func<TSource, IEnumerable<TValue?>?>> keyPropExpr,
        Expression<Func<TSource, int>> countPropExpr,
        bool enableValueTokenSplitting = true, bool allowSequentialValues = true, 
        ArityType valueArity = ArityType.ZeroOrMore, ArityType optionArity = ArityType.ZeroOrMore)
        where TSource : BaseApplicationOptions
        where TValue : struct, IComparable, IEquatable<TValue>
    {
        var keyProperty = source.ValidateExpression(keyPropExpr);
        var countProperty = source.ValidateExpression(countPropExpr);

        return source.RegisterScalarOption<TSource, TValue>(
            keyProperty, countProperty, enableValueTokenSplitting, allowSequentialValues, valueArity, optionArity);
    }
}
