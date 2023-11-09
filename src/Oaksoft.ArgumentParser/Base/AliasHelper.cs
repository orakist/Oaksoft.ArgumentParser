using Oaksoft.ArgumentParser.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oaksoft.ArgumentParser.Base;

internal static class AliasHelper
{
    private static readonly char[] _prefixTrimChars = { '-', '/' };
    private static readonly char[] _suggestionTrimChars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ' ' };

    public static void ValidateArgument(this string argument, OptionPrefixRules rules)
    {
        if (argument.Length < 2)
            return;

        if (argument[0] == '-' && char.IsLetter(argument[1]))
        {
            if (rules.HasFlag(OptionPrefixRules.AllowSingleDash))
                return;
            if (rules.HasFlag(OptionPrefixRules.AllowSingleDashShortAlias) && argument.Length == 2)
                return;

            var desc = argument.Length < 3 ? " with short aliases" : string.Empty;
            throw new Exception($"Single dash '-' is not allowed{desc}! Invalid token: {argument}");
        }

        if (argument.Length > 2 && argument.StartsWith("--") && char.IsLetter(argument[2]))
        {
            if (rules.HasFlag(OptionPrefixRules.AllowDoubleDash))
                return;
            if (rules.HasFlag(OptionPrefixRules.AllowDoubleDashLongAlias) && argument.Length > 3)
                return;

            var desc = argument.Length < 4 ? " with short aliases" : string.Empty;
            throw new Exception($"Double dash '--' is not allowed{desc}! Invalid token: {argument}");
        }

        if (argument[0] == '/' && char.IsLetter(argument[1]))
        {
            if (rules.HasFlag(OptionPrefixRules.AllowForwardSlash))
                return;

            throw new Exception("Forward slash '/' is not allowed! Invalid token: " + argument);
        }
    }

    public static bool IsAliasCandidate(this string token, OptionPrefixRules rules)
    {
        if (token.Length < 2)
            return false;

        if (token[0] == '-' && char.IsLetter(token[1]))
        {
            if (rules.HasFlag(OptionPrefixRules.AllowSingleDash))
                return true;
            if (rules.HasFlag(OptionPrefixRules.AllowSingleDashShortAlias) && token.Length == 2)
                return true;
        }

        if (token.Length > 2 && token.StartsWith("--") && char.IsLetter(token[2]))
        {
            if (rules.HasFlag(OptionPrefixRules.AllowDoubleDash))
                return true;
            if (rules.HasFlag(OptionPrefixRules.AllowDoubleDashLongAlias) && token.Length > 3)
                return true;
        }

        if (token[0] == '/' && char.IsLetter(token[1]))
        {
            if (rules.HasFlag(OptionPrefixRules.AllowForwardSlash))
                return true;
        }

        return false;
    }

    public static IEnumerable<string> GetPrefixedAliases(this OptionPrefixRules rules, List<string> aliases)
    {
        foreach (var alias in aliases)
        {
            if (rules.HasFlag(OptionPrefixRules.AllowSingleDash))
                yield return "-" + alias;
            else if (alias.Length < 2 && rules.HasFlag(OptionPrefixRules.AllowSingleDashShortAlias))
                yield return "-" + alias;

            if (rules.HasFlag(OptionPrefixRules.AllowDoubleDash))
                yield return "--" + alias;
            else if (alias.Length > 1 && rules.HasFlag(OptionPrefixRules.AllowDoubleDashLongAlias))
                yield return "--" + alias;

            if (rules.HasFlag(OptionPrefixRules.AllowForwardSlash))
                yield return "/" + alias;
        }
    }

    public static string TrimAlias(this string name)
    {
        return name.TrimStart(_prefixTrimChars).TrimEnd();
    }

    public static IEnumerable<string> GetAliasesHeuristically(
        string name, ICollection<string> filter, int maxAliasLength, bool caseSensitive)
    {
        var words = GetHumanizedWords(name).ToList();
        if (words.Count < 1)
            yield break;

        var compareFlag = caseSensitive
            ? StringComparison.Ordinal
            : StringComparison.OrdinalIgnoreCase;

        // find a short alias
        for (var i = 0; i < 4; ++i)
        {
            var candidateFound = false;
            foreach (var word in words.Where(w => i < w.Length))
            {
                if (char.IsDigit(word[i]))
                    continue;

                var candidate = word.Substring(i, 1);
                if (filter.Any(f => f.Equals(candidate, compareFlag)))
                    continue;

                candidateFound = true;
                yield return candidate;
                break;
            }

            if (candidateFound)
                break;
        }

        // find a long alias by using first 2 words
        if (words.Count > 1 && words.Take(2).Sum(s => s.Length) + 1 < maxAliasLength)
        {
            var candidate = string.Join('-', words.Take(2));
            if (!filter.Any(f => f.Equals(candidate, compareFlag)))
            {
                yield return candidate;
                yield break;
            }
        }

        // find a long alias by using first word
        if (words[0].Length < maxAliasLength)
        {
            var candidate = words[0];
            if (!filter.Any(f => f.Equals(candidate, compareFlag)))
            {
                yield return candidate;
                yield break;
            }
        }

        // find a long alias by using first 3 words
        if (words.Count > 2 && words.Take(3).Sum(s => s.Length) + 2 < maxAliasLength)
        {
            var candidate = string.Join('-', words.Take(3));
            if (!filter.Any(f => f.Equals(candidate, compareFlag)))
            {
                yield return candidate;
            }
        }
    }

    public static IEnumerable<string> GetHumanizedWords(string name)
    {
        var candidates = name.Replace('_', ' ')
            .TrimStart(_suggestionTrimChars)
            .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        foreach (var candidate in candidates)
        {
            if (candidate.Length < 3)
            {
                yield return candidate;
                continue;
            }

            var startIndex = 0;
            var prevChar = candidate[0];
            for (var i = 1; i < candidate.Length; ++i)
            {
                if ((char.IsDigit(prevChar) && !char.IsDigit(candidate[i])) ||
                    (char.IsLower(prevChar) && char.IsUpper(candidate[i])))
                {
                    yield return candidate[startIndex..i];
                    startIndex = i;
                }
                else if (char.IsUpper(prevChar) && char.IsLower(candidate[i]))
                {
                    if (startIndex < i - 1)
                    {
                        yield return candidate[startIndex..(i - 1)];
                        startIndex = i - 1;
                    }
                }

                prevChar = candidate[i];
            }

            yield return candidate[startIndex..];
        }
    }
}
