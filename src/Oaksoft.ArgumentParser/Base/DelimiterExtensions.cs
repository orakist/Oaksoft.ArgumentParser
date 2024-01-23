using Oaksoft.ArgumentParser.Definitions;
using System.Collections.Generic;

namespace Oaksoft.ArgumentParser.Base;

internal static class DelimiterExtensions
{
    public static IEnumerable<char> GetSymbols(this AliasDelimiterRules rules)
    {
        if (rules.HasFlag(AliasDelimiterRules.AllowEqualSymbol))
        {
            yield return '=';
        }

        if (rules.HasFlag(AliasDelimiterRules.AllowColonSymbol))
        {
            yield return ':';
        }

        if (rules.HasFlag(AliasDelimiterRules.AllowWhitespace))
        {
            yield return ' ';
        }
    }

    public static IEnumerable<char> GetSymbols(this ValueDelimiterRules rules)
    {
        if (rules.HasFlag(ValueDelimiterRules.AllowSemicolonSymbol))
        {
            yield return ';';
        }

        if (rules.HasFlag(ValueDelimiterRules.AllowCommaSymbol))
        {
            yield return ',';
        }

        if (rules.HasFlag(ValueDelimiterRules.AllowPipeSymbol))
        {
            yield return '|';
        }
    }
}
