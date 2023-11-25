using Oaksoft.ArgumentParser.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using Oaksoft.ArgumentParser.Exceptions;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Base;

internal static class AliasExtensions
{
    private static readonly char[] _suggestionTrimChars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ' ' };
    private static readonly char[] _allowedAliasSymbols = { '?', '%', '$', '€', '£', '#', '@', '-' };
    private static readonly string[] _reservedAliases = { "h", "?", "help" };

    public static Result<string> ValidateName(this string name)
    {
        name = name.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            return BuilderErrors.EmptyValue.With(nameof(name));
        }

        if (!name.All(c => char.IsAsciiDigit(c) || char.IsAsciiLetter(c) || c is '_' or '-'))
        {
            return BuilderErrors.InvalidName.With(name);
        }

        return string.Join(' ', name.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
    }

    public static Result<string> ValidateAlias(this string alias)
    {
        alias = alias.Trim();
        if (string.IsNullOrWhiteSpace(alias))
        {
            return BuilderErrors.EmptyValue.With(nameof(alias));
        }

        var trimmed = TrimStartPrefixes(alias);
        var result = string.Join('-', GetNormalizedWords(trimmed, _allowedAliasSymbols));

        if (string.IsNullOrWhiteSpace(trimmed) || result.Length < trimmed.Length)
        {
            var symbols = string.Join(", ", _allowedAliasSymbols.Select(s => $"'{s}'"));
            return BuilderErrors.InvalidAlias.With(alias, symbols);
        }

        if (_reservedAliases.Any(r => r.Equals(result, StringComparison.OrdinalIgnoreCase)))
        {
            var aliases = string.Join(", ", _reservedAliases.Select(s => $"'{s}'"));
            return BuilderErrors.ReservedAlias.With(trimmed, aliases);
        }

        return result;
    }

    public static Result<List<string>> ValidateAliases(
        this IEnumerable<string> aliases, OptionPrefixRules rules,
        bool caseSensitive, int maxAliasLength, bool allowFailureResult)
    {
        var resultAliases = new List<string>();
        foreach (var alias in aliases.Select(a => caseSensitive ? a : a.ToLowerInvariant()))
        {
            if (alias.Length > maxAliasLength)
            {
                if (allowFailureResult)
                {
                    return BuilderErrors.TooLongAlias.With(alias, maxAliasLength);
                }

                continue;
            }

            if (!IsAliasAllowed(alias, rules))
            {
                if (allowFailureResult)
                {
                    return BuilderErrors.NotAllowedAlias.With(alias.Length < 2 ? "Short" : "Long", alias);
                }

                continue;
            }

            resultAliases.Add(alias);
        }

        return resultAliases;
    }

    public static IEnumerable<string> SuggestAliasesHeuristically(
        this string name, ICollection<string> filter, bool caseSensitive, int maxAliasLength, int maxAliasWordCount)
    {
        var words = name.GetHumanizedWords().ToList();
        if (words.Count < 1)
            yield break;

        var compareFlag = caseSensitive
            ? StringComparison.Ordinal
            : StringComparison.OrdinalIgnoreCase;

        // find a short alias
        for (var i = 0; i < 8; ++i)
        {
            var candidateFound = false;
            foreach (var word in words.Take(maxAliasWordCount).Where(w => i < w.Length))
            {
                if (char.IsAsciiDigit(word[i]))
                    continue;

                var candidate = word.Substring(i, 1);
                if (filter.Any(f => f.Equals(candidate, compareFlag)))
                    continue;

                if (_reservedAliases.Any(f => f.Equals(candidate, StringComparison.OrdinalIgnoreCase)))
                    continue;

                candidateFound = true;
                yield return candidate;
                break;
            }

            if (candidateFound)
                break;
        }

        for (var i = maxAliasWordCount; i > 0; --i)
        {
            // find a long alias
            if (words.Count < i || words.Take(i).Sum(s => s.Length) + i - 1 > maxAliasLength)
                continue;

            var candidate = string.Join('-', words.Take(i));
            if (_reservedAliases.Any(r => r.Equals(candidate, StringComparison.OrdinalIgnoreCase)))
                continue;

            if (filter.Any(f => f.Equals(candidate, compareFlag)))
                continue;

            yield return candidate;
            yield break;
        }
    }

    public static IEnumerable<string> GetPrefixedAliases(this List<string> aliases, OptionPrefixRules rules)
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

    public static void ExtractAliasAndValue(
        this TokenItem tokenItem, List<string> aliases, bool caseSensitive,
        OptionPrefixRules prefixRules, AliasDelimiterRules aliasRules)
    {
        var token = tokenItem.Token;
        var compareFlag = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

        var alias = CheckDoubleDashAlias(token, aliases, compareFlag, prefixRules, aliasRules);
        if (!string.IsNullOrWhiteSpace(alias))
        {
            tokenItem.Alias = alias;
            tokenItem.Value = GetAliasValue(token, alias.Length, aliasRules);
            return;
        }

        alias = CheckSingleDashAlias(token, aliases, compareFlag, prefixRules, aliasRules);
        if (!string.IsNullOrWhiteSpace(alias))
        {
            tokenItem.Alias = alias;
            tokenItem.Value = GetAliasValue(token, alias.Length, aliasRules);
            return;
        }

        alias = CheckForwardSlashAlias(token, aliases, compareFlag, prefixRules, aliasRules);
        if (!string.IsNullOrWhiteSpace(alias))
        {
            tokenItem.Alias = alias;
            tokenItem.Value = GetAliasValue(token, alias.Length, aliasRules);
            return;
        }

        tokenItem.Value = token;
    }

    private static string? CheckDoubleDashAlias(
        string token, List<string> aliases, StringComparison compareFlag,
        OptionPrefixRules prefixRules, AliasDelimiterRules aliasRules)
    {
        if ((prefixRules & (OptionPrefixRules.AllowDoubleDash | OptionPrefixRules.AllowDoubleDashLongAlias)) <= 0)
            return null;

        if (!token.StartsWith("--"))
            return null;

        if (token.Length < 3)
            throw new Exception($"Invalid token '{token}' found!");

        foreach (var alias in aliases)
        {
            if (!token.AsSpan(2).StartsWith(alias, compareFlag))
                continue;

            if (prefixRules.HasFlag(OptionPrefixRules.AllowDoubleDash))
            {
                // allow --o or --opt
                if (token.Length == alias.Length + 2)
                    return token;

                // allow --o:value or --o=value, --opt:value or --opt=value
                if (token.Length > alias.Length + 3 && aliasRules.GetSymbols().Any(s => token[alias.Length + 2] == s))
                    return token[..(alias.Length + 2)];
            }

            if (prefixRules.HasFlag(OptionPrefixRules.AllowDoubleDashLongAlias))
            {
                if (alias.Length < 2)
                    throw new Exception($"Double dash '--' is not allowed with short aliases! Invalid token: {token}");

                // allow only --opt
                if (token.Length == alias.Length + 2)
                    return token;

                // allow --opt:value or --opt=value
                if (token.Length > alias.Length + 3 && aliasRules.GetSymbols().Any(s => token[alias.Length + 2] == s))
                    return token[..(alias.Length + 2)];
            }

            throw new Exception($"Invalid double dash alias '{alias}' usage! Token: {token}");
        }

        throw new Exception($"Unknown double dash alias token '{token}' found!");
    }

    private static string? CheckSingleDashAlias(
        string token, List<string> aliases, StringComparison compareFlag,
        OptionPrefixRules prefixRules, AliasDelimiterRules aliasRules)
    {
        if ((prefixRules & (OptionPrefixRules.AllowSingleDash | OptionPrefixRules.AllowSingleDashShortAlias)) <= 0)
            return null;

        if (token[0] != '-')
            return null;

        if (token.Length < 2)
            throw new Exception($"Invalid token '{token}' found!");

        if (!char.IsAsciiLetter(token[1]) && !_allowedAliasSymbols.Contains(token[1]))
            return null;

        foreach (var alias in aliases)
        {
            if (!token.AsSpan(1).StartsWith(alias, compareFlag))
                continue;

            if (prefixRules.HasFlag(OptionPrefixRules.AllowSingleDash))
            {
                // allow -o or -opt
                if (token.Length == alias.Length + 1)
                    return token;

                // allow -o=value, -o:value, -opt:value or -opt=value
                if (token.Length > alias.Length + 2 && aliasRules.GetSymbols().Any(s => token[alias.Length + 1] == s))
                    return token[..(alias.Length + 1)];

                // allow only -ovalue
                if (alias.Length == 1 && aliasRules.HasFlag(AliasDelimiterRules.AllowOmittingDelimiter))
                    return token[..2];
            }

            if (prefixRules.HasFlag(OptionPrefixRules.AllowSingleDashShortAlias))
            {
                if (alias.Length > 1)
                    throw new Exception($"Single dash '-' is not allowed with long aliases! Invalid token: {token}");

                // allow only -o
                if (token.Length == 2)
                    return token;

                // allow -o:value or -o=value
                if (token.Length > 3 && aliasRules.GetSymbols().Any(s => token[2] == s))
                    return token[..2];

                // allow only -ovalue
                if (aliasRules.HasFlag(AliasDelimiterRules.AllowOmittingDelimiter))
                    return token[..2];
            }

            throw new Exception($"Invalid single dash alias '{alias}' usage! Token: {token}");
        }

        throw new Exception($"Unknown single dash alias token '{token}' found!");
    }

    private static string? CheckForwardSlashAlias(
        string token, List<string> aliases, StringComparison compareFlag,
        OptionPrefixRules prefixRules, AliasDelimiterRules aliasRules)
    {
        if ((prefixRules & OptionPrefixRules.AllowForwardSlash) <= 0)
            return null;

        if (token[0] != '/')
            return null;

        if (token.Length < 2)
            throw new Exception($"Invalid token '{token}' found!");

        foreach (var alias in aliases)
        {
            if (!token.AsSpan(1).StartsWith(alias, compareFlag))
                continue;

            // allow /o or /opt
            if (token.Length == alias.Length + 1)
                return token;

            // allow /o=value, /o:value, /opt:value or /opt=value
            if (token.Length > alias.Length + 2 && aliasRules.GetSymbols().Any(s => token[alias.Length + 1] == s))
                return token[..(alias.Length + 1)];

            throw new Exception($"Invalid forward slash alias '{alias}' usage! Token: {token}");
        }

        throw new Exception($"Unknown forward slash token '{token}' found!");
    }

    private static bool IsAliasAllowed(string alias, OptionPrefixRules rules)
    {
        if (rules.HasFlag(OptionPrefixRules.AllowSingleDash))
            return true;

        if (alias.Length < 2 && rules.HasFlag(OptionPrefixRules.AllowSingleDashShortAlias))
            return true;

        if (rules.HasFlag(OptionPrefixRules.AllowDoubleDash))
            return true;

        if (alias.Length > 1 && rules.HasFlag(OptionPrefixRules.AllowDoubleDashLongAlias))
            return true;

        if (rules.HasFlag(OptionPrefixRules.AllowForwardSlash))
            return true;

        return false;
    }

    /// <summary>
    /// Split the token by using alias delimiter flags and return the value
    /// </summary>
    private static string? GetAliasValue(string token, int aliasLength, AliasDelimiterRules aliasRules)
    {
        if (token.Length <= aliasLength)
            return null;

        if (aliasRules.GetSymbols().Any(s => token[aliasLength] == s))
        {
            return token[(aliasLength + 1)..];
        }

        if (aliasLength == 2 && token.StartsWith('-') &&
            aliasRules.HasFlag(AliasDelimiterRules.AllowOmittingDelimiter))
        {
            return token[aliasLength..];
        }

        return null;
    }

    private static IEnumerable<string> GetHumanizedWords(this string name)
    {
        var candidates = GetNormalizedWords(name);

        foreach (var candidate in candidates)
        {
            if (candidate.Length < 2)
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

    private static IEnumerable<string> GetNormalizedWords(string input, params char[] allowedSymbols)
    {
        var validChars = input.Replace('_', ' ').Replace('-', ' ')
            .Where(c => char.IsAsciiDigit(c) || char.IsAsciiLetter(c) || c == ' ' || allowedSymbols.Contains(c));

        return string.Join("", validChars)
            .TrimStart(_suggestionTrimChars)
            .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    }

    private static string TrimStartPrefixes(string input)
    {
        if (input.StartsWith('/'))
            input = input[1..];

        input = input.Replace('_', ' ').Replace('-', ' ');
        return string.Join('-', input.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries));
    }
}
