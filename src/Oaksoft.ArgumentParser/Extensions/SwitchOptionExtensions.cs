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
        Func<IScalarNamedOption<bool>, IScalarNamedOption<bool>>? configure = null,
        bool mandatoryOption = false)
        where TSource : new()
    {
        var keyProperty = keyPropExpr.ValidateExpression(typeof(bool).ToString());

        var option = builder.RegisterSwitchOption(keyProperty, mandatoryOption);
        
        configure?.Invoke(option);

        return builder;
    }

    /// <summary>
    /// Registers a switch option.
    /// </summary>
    public static IArgumentParserBuilder<TSource> AddSwitchOption<TSource>(
        this IArgumentParserBuilder<TSource> builder,
        Expression<Func<TSource, bool?>> keyPropExpr,
        Func<IScalarNamedOption<bool>, IScalarNamedOption<bool>>? configure = null,
        bool mandatoryOption = false)
        where TSource : new()
    {
        var keyProperty = keyPropExpr.ValidateExpression(typeof(bool).ToString());

        var option = builder.RegisterSwitchOption(keyProperty, mandatoryOption);

        configure?.Invoke(option);

        return builder;
    }
}
