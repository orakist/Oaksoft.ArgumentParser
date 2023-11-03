using System;
using System.Linq.Expressions;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Extensions;

public static partial class OptionsExtensions
{
    public static ISwitchOption AddSwitchOption<TSource>(
        this TSource source, 
        Expression<Func<TSource, bool>> keyPropExpr, 
        bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        return source.RegisterSwitchOption(keyPropExpr, mandatory);
    }

    public static ISwitchOption AddSwitchOption<TSource>(
        this TSource source,
        Expression<Func<TSource, bool?>> keyPropExpr,
        bool mandatory = false)
        where TSource : BaseApplicationOptions
    {
        return source.RegisterSwitchOption(keyPropExpr, mandatory);
    }

    public static ISwitchOption AddCountOption<TSource>(
        this TSource source,
        Expression<Func<TSource, int>> keyPropExpr,
        int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        return source.RegisterSwitchOption(keyPropExpr, requiredTokenCount, maximumTokenCount);
    }

    public static ISwitchOption AddCountOption<TSource>(
        this TSource source,
        Expression<Func<TSource, int?>> keyPropExpr,
        int requiredTokenCount = 0, int maximumTokenCount = 10)
        where TSource : BaseApplicationOptions
    {
        return source.RegisterSwitchOption(keyPropExpr, requiredTokenCount, maximumTokenCount);
    }
}
