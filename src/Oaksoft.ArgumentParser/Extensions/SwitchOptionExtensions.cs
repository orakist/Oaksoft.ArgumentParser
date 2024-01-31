using System;
using System.Linq.Expressions;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Builder;
using Oaksoft.ArgumentParser.Options;

namespace Oaksoft.ArgumentParser.Extensions;

public static partial class OptionExtensions
{
    /// <summary>
    /// Registers a switch option.
    /// </summary>
    public static IArgumentParserBuilder<TSource> AddSwitchOption<TSource>(
        this IArgumentParserBuilder<TSource> builder, 
        Expression<Func<TSource, bool>> keyPropExpr, 
        Action<ISwitchOption>? configure = null,
        bool mandatoryOption = false)
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr, typeof(bool).ToString());

        var option = builder.RegisterSwitchOption<TSource>(keyProperty, mandatoryOption);
        
        configure?.Invoke(option);

        return builder;
    }

    /// <summary>
    /// Registers a switch option.
    /// </summary>
    public static IArgumentParserBuilder<TSource> AddSwitchOption<TSource>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, bool?>> keyPropExpr,
        Action<ISwitchOption>? configure = null,
        bool mandatoryOption = false)
    {
        var keyProperty = builder.ValidateExpression(keyPropExpr, typeof(bool).ToString());

        var option = builder.RegisterSwitchOption<TSource>(keyProperty, mandatoryOption);

        configure?.Invoke(option);

        return builder;
    }
}
