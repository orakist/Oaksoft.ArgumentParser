using System;
using System.Linq.Expressions;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Builder;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Options;

namespace Oaksoft.ArgumentParser.Extensions;

public static partial class OptionExtensions
{
    /// <summary>
    /// Registers a counter option.
    /// </summary>
    public static IArgumentParserBuilder<TSource> AddCounterOption<TSource>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, int>> keyPropExpr,
        Func<ICounterOption, ICounterOption>? configure = null,
        ArityType optionArity = ArityType.ZeroOrMore)
        where TSource : new()
    {
        var keyProperty = keyPropExpr.ValidateExpression(typeof(int).ToString());

        var option = builder.RegisterCounterOption(keyProperty, optionArity);

        configure?.Invoke(option);

        return builder;
    }

    /// <summary>
    /// Registers a counter option.
    /// </summary>
    public static IArgumentParserBuilder<TSource> AddCounterOption<TSource>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, int?>> keyPropExpr,
        Func<ICounterOption, ICounterOption>? configure = null,
        ArityType optionArity = ArityType.ZeroOrMore)
        where TSource : new()
    {
        var keyProperty = keyPropExpr.ValidateExpression(typeof(int).ToString());

        var option = builder.RegisterCounterOption(keyProperty, optionArity);

        configure?.Invoke(option);

        return builder;
    }
}
