using System;
using System.Collections.Generic;
using System.Linq;

namespace Oaksoft.ArgumentParser.Base;

internal static class AliasHelper
{
    private static readonly char[] _trimChars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ' ' };

    public static IEnumerable<string> GetAliasesHeuristically(
        string name, ICollection<string> filter, int maxAliasLength, bool caseSensitive)
    {
        var words = GetHumanizedWords(name).ToList();
        if(words.Count < 1)
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

    private static IEnumerable<string> GetHumanizedWords(string name)
    {
        var candidates = name.Replace('_', ' ')
            .TrimStart(_trimChars)
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
