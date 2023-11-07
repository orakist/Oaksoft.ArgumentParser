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
        this TSource source, 
        Expression<Func<TSource, bool>> keyPropExpr, 
        bool mandatoryOption = false)
        where TSource : BaseApplicationOptions
    {
        return source.RegisterSwitchOption(keyPropExpr, mandatoryOption);
    }

    public static ISwitchOption AddSwitchOption<TSource>(
        this TSource source,
        Expression<Func<TSource, bool?>> keyPropExpr,
        bool mandatoryOption = false)
        where TSource : BaseApplicationOptions
    {
        return source.RegisterSwitchOption(keyPropExpr, mandatoryOption);
    }

    public static ISwitchOption AddCountOption<TSource>(
        this TSource source,
        Expression<Func<TSource, int>> keyPropExpr,
        ArityType optionArity = ArityType.ZeroOrMore)
        where TSource : BaseApplicationOptions
    {
        return source.RegisterSwitchOption(keyPropExpr, optionArity);
    }

    public static ISwitchOption AddCountOption<TSource>(
        this TSource source,
        Expression<Func<TSource, int?>> keyPropExpr,
        ArityType optionArity = ArityType.ZeroOrMore)
        where TSource : BaseApplicationOptions
    {
        return source.RegisterSwitchOption(keyPropExpr, optionArity);
    }
}
