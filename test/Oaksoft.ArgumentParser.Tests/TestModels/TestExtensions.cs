using Oaksoft.ArgumentParser.Definitions;

namespace Oaksoft.ArgumentParser.Tests.TestModels;

internal static class TestExtensions
{
    public static readonly OptionPrefixRules[] AllPrefixRules =
    {
        OptionPrefixRules.AllowSingleDash,
        OptionPrefixRules.AllowSingleDashShortAlias,
        OptionPrefixRules.AllowDoubleDash,
        OptionPrefixRules.AllowDoubleDashLongAlias,
        OptionPrefixRules.AllowForwardSlash,

        OptionPrefixRules.AllowSingleDash | OptionPrefixRules.AllowSingleDashShortAlias,
        OptionPrefixRules.AllowSingleDash | OptionPrefixRules.AllowDoubleDash,
        OptionPrefixRules.AllowSingleDash | OptionPrefixRules.AllowDoubleDashLongAlias,
        OptionPrefixRules.AllowSingleDash | OptionPrefixRules.AllowForwardSlash,
        OptionPrefixRules.AllowSingleDashShortAlias | OptionPrefixRules.AllowDoubleDash,
        OptionPrefixRules.AllowSingleDashShortAlias | OptionPrefixRules.AllowDoubleDashLongAlias,
        OptionPrefixRules.AllowSingleDashShortAlias | OptionPrefixRules.AllowForwardSlash,
        OptionPrefixRules.AllowDoubleDash | OptionPrefixRules.AllowDoubleDashLongAlias,
        OptionPrefixRules.AllowDoubleDash | OptionPrefixRules.AllowForwardSlash,
        OptionPrefixRules.AllowDoubleDashLongAlias | OptionPrefixRules.AllowForwardSlash,

        OptionPrefixRules.AllowSingleDash | OptionPrefixRules.AllowSingleDashShortAlias | OptionPrefixRules.AllowDoubleDash,
        OptionPrefixRules.AllowSingleDash | OptionPrefixRules.AllowSingleDashShortAlias | OptionPrefixRules.AllowDoubleDashLongAlias,
        OptionPrefixRules.AllowSingleDash | OptionPrefixRules.AllowSingleDashShortAlias | OptionPrefixRules.AllowForwardSlash,
        OptionPrefixRules.AllowSingleDash | OptionPrefixRules.AllowDoubleDash | OptionPrefixRules.AllowDoubleDashLongAlias,
        OptionPrefixRules.AllowSingleDash | OptionPrefixRules.AllowDoubleDash | OptionPrefixRules.AllowForwardSlash,
        OptionPrefixRules.AllowSingleDash | OptionPrefixRules.AllowDoubleDashLongAlias | OptionPrefixRules.AllowForwardSlash,
        OptionPrefixRules.AllowSingleDashShortAlias | OptionPrefixRules.AllowDoubleDash | OptionPrefixRules.AllowDoubleDashLongAlias,
        OptionPrefixRules.AllowSingleDashShortAlias | OptionPrefixRules.AllowDoubleDash | OptionPrefixRules.AllowForwardSlash,
        OptionPrefixRules.AllowSingleDashShortAlias | OptionPrefixRules.AllowDoubleDashLongAlias | OptionPrefixRules.AllowForwardSlash,
        OptionPrefixRules.AllowDoubleDash | OptionPrefixRules.AllowDoubleDashLongAlias | OptionPrefixRules.AllowForwardSlash,

        OptionPrefixRules.AllowSingleDash | OptionPrefixRules.AllowSingleDashShortAlias | OptionPrefixRules.AllowDoubleDash | OptionPrefixRules.AllowDoubleDashLongAlias,
        OptionPrefixRules.AllowSingleDash | OptionPrefixRules.AllowDoubleDash | OptionPrefixRules.AllowDoubleDashLongAlias | OptionPrefixRules.AllowForwardSlash,
        OptionPrefixRules.AllowSingleDash | OptionPrefixRules.AllowSingleDashShortAlias | OptionPrefixRules.AllowDoubleDashLongAlias | OptionPrefixRules.AllowForwardSlash,
        OptionPrefixRules.AllowSingleDash | OptionPrefixRules.AllowSingleDashShortAlias | OptionPrefixRules.AllowDoubleDash | OptionPrefixRules.AllowForwardSlash,
        OptionPrefixRules.AllowSingleDashShortAlias | OptionPrefixRules.AllowDoubleDash | OptionPrefixRules.AllowDoubleDashLongAlias | OptionPrefixRules.AllowForwardSlash,

        OptionPrefixRules.All
    };

    private record PrefixRules(string Prefix, bool IsShortAlias, OptionPrefixRules Flag);

    private static readonly PrefixRules[] _allRules =
    {
        new ("--", false, OptionPrefixRules.AllowDoubleDash | OptionPrefixRules.AllowDoubleDashLongAlias),
        new ("--", true, OptionPrefixRules.AllowDoubleDash ),
        new ("-", false, OptionPrefixRules.AllowSingleDash ),
        new ("-", true, OptionPrefixRules.AllowSingleDash | OptionPrefixRules.AllowSingleDashShortAlias),
        new ("/", false, OptionPrefixRules.AllowForwardSlash),
        new ("/", true, OptionPrefixRules.AllowForwardSlash)
    };

    public static (int Min, int Max) GetLimits(this ArityType arityType)
    {
        return arityType switch
        {
            ArityType.ZeroOrOne => (0, 1),
            ArityType.ExactlyOne => (1, 1),
            ArityType.ZeroOrMore => (0, int.MaxValue),
            ArityType.OneOrMore => (1, int.MaxValue),
            _ => (0, 0),
        };
    }

    public static bool IsValidAlias(this OptionPrefixRules flags, string alias)
    {
        var rules = _allRules.Where(r => alias.StartsWith(r.Prefix)).ToList();
        var isShortAlias = alias.Length - rules.First().Prefix.Length < 2;
        var rule = rules.First(r => r.IsShortAlias == isShortAlias);

        return (flags & rule.Flag) != 0;
    }
}