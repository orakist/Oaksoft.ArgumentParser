using System;
using System.Linq.Expressions;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Builder;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Extensions;

public static partial class OptionExtensions
{
    public static IArgumentParserBuilder<TSource> AddCounterOption<TSource>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, int>> keyPropExpr,
        Action<ICounterOption>? configure = null,
        ArityType optionArity = ArityType.ZeroOrMore)
        where TSource : IApplicationOptions
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr, typeof(int).ToString());

        var option = builder.RegisterCounterOption<TSource>(keyProperty, optionArity);

        configure?.Invoke(option);

        return builder;
    }

    public static IArgumentParserBuilder<TSource> AddCounterOption<TSource>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, int?>> keyPropExpr,
        Action<ICounterOption>? configure = null,
        ArityType optionArity = ArityType.ZeroOrMore)
        where TSource : IApplicationOptions
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr, typeof(int).ToString());

        var option = builder.RegisterCounterOption<TSource>(keyProperty, optionArity);

        configure?.Invoke(option);

        return builder;
    }
}
