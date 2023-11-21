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
    public static IArgumentParserBuilder<TSource> AddSwitchOption<TSource>(
        this IArgumentParserBuilder<TSource> builder, 
        Expression<Func<TSource, bool>> keyPropExpr, 
        Action<ISwitchOption>? configure = null,
        bool mandatoryOption = false)
        where TSource : IApplicationOptions
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr, typeof(bool).ToString());

        var option = builder.RegisterSwitchOption<TSource>(keyProperty, mandatoryOption);
        
        configure?.Invoke(option);

        return builder;
    }

    public static IArgumentParserBuilder<TSource> AddSwitchOption<TSource>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, bool?>> keyPropExpr,
        Action<ISwitchOption>? configure = null,
        bool mandatoryOption = false)
        where TSource : IApplicationOptions
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr, typeof(bool).ToString());

        var option = builder.RegisterSwitchOption<TSource>(keyProperty, mandatoryOption);

        configure?.Invoke(option);

        return builder;
    }

    public static IArgumentParserBuilder<TSource> AddCountOption<TSource>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, int>> keyPropExpr,
        Action<ISwitchOption>? configure = null,
        ArityType optionArity = ArityType.ZeroOrMore)
        where TSource : IApplicationOptions
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr, typeof(int).ToString());

        var option = builder.RegisterSwitchOption<TSource>(keyProperty, optionArity);

        configure?.Invoke(option);

        return builder;
    }

    public static IArgumentParserBuilder<TSource> AddCountOption<TSource>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, int?>> keyPropExpr,
        Action<ISwitchOption>? configure = null,
        ArityType optionArity = ArityType.ZeroOrMore)
        where TSource : IApplicationOptions
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr, typeof(int).ToString());

        var option = builder.RegisterSwitchOption<TSource>(keyProperty, optionArity);

        configure?.Invoke(option);

        return builder;
    }
}
