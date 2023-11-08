using System;
using System.Linq.Expressions;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Extensions;

public static partial class OptionsExtensions
{
    public static ISwitchOption AddSwitchOption<TSource>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, bool>> keyPropExpr, 
        bool mandatoryOption = false)
        where TSource : IApplicationOptions
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr);

        return builder.RegisterSwitchOption<TSource>(keyProperty, mandatoryOption);
    }

    public static ISwitchOption AddSwitchOption<TSource>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, bool?>> keyPropExpr,
        bool mandatoryOption = false)
        where TSource : IApplicationOptions
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr);

        return builder.RegisterSwitchOption<TSource>(keyProperty, mandatoryOption);
    }

    public static ISwitchOption AddCountOption<TSource>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, int>> keyPropExpr,
        ArityType optionArity = ArityType.ZeroOrMore)
        where TSource : IApplicationOptions
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr);

        return builder.RegisterSwitchOption<TSource>(keyProperty, optionArity);
    }

    public static ISwitchOption AddCountOption<TSource>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, int?>> keyPropExpr,
        ArityType optionArity = ArityType.ZeroOrMore)
        where TSource : IApplicationOptions
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr);

        return builder.RegisterSwitchOption<TSource>(keyProperty, optionArity);
    }
}
