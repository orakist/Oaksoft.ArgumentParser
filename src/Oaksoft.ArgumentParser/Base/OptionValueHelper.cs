﻿using System;
using System.Collections.Generic;
using System.Linq;
using Oaksoft.ArgumentParser.Definitions;

namespace Oaksoft.ArgumentParser.Base;

internal static class OptionValueHelper
{
    public static void ValidateByAllowedValues<TValue>(
        this List<TValue> inputValues, ICollection<TValue> allowedValues, bool caseSensitive)
        where TValue : IComparable, IEquatable<TValue>
    {
        if (allowedValues.Count <= 0)
            return;

        if (typeof(TValue) == typeof(string))
        {
            var flag = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            foreach (var inputValue in inputValues.Cast<string>())
            {
                if (allowedValues.Cast<string>().Any(a => a.Equals(inputValue, flag)))
                    continue;

                var values = string.Join('|', allowedValues);
                throw new Exception($"Option value '{inputValue}' not recognized. Must be one of: {values}");
            }
        }
        else
        {
            foreach (var inputValue in inputValues)
            {
                if (allowedValues.Any(a => a.Equals(inputValue)))
                    continue;

                var values = string.Join('|', allowedValues);
                throw new Exception($"Option value '{inputValue}' not recognized. Must be one of: {values}");
            }
        }
    }

    public static IEnumerable<string> GetInputValues(
        this string valueToken, ValueDelimiterRules valueDelimiter, bool enableValueTokenSplitting)
    {
        var symbols = valueDelimiter.GetSymbols().ToList();

        if (enableValueTokenSplitting)
        {
            foreach (var value in valueToken.EnumerateByDelimiter(symbols))
            {
                yield return value;
            }
        }
        else
        {
            yield return valueToken;
        }
    }

    public static IEnumerable<string> GetInputValues(
        this List<string> valueTokens, ValueDelimiterRules valueDelimiter, bool enableValueTokenSplitting)
    {
        var symbols = valueDelimiter.GetSymbols().ToList();

        foreach (var valueToken in valueTokens)
        {
            if (enableValueTokenSplitting)
            {
                foreach (var value in valueToken.EnumerateByDelimiter(symbols))
                {
                    yield return value;
                }
            }
            else
            {
                yield return valueToken;
            }
        }
    }

    /// <summary>
    /// Split the argument by using token delimiter flags and return the value
    /// </summary>
    public static string GetOptionValue(this string argument, string alias, AliasDelimiterRules rules)
    {
        foreach (var symbol in rules.GetSymbols())
        {
            if (argument[alias.Length] != symbol)
                continue;

            return argument[(alias.Length + 1)..];
        }

        if ((alias.Length == 2 || alias.StartsWith("--") && alias.Length == 3) &&
            rules.HasFlag(AliasDelimiterRules.AllowOmittingDelimiter))
        {
            return argument[alias.Length..];
        }

        return string.Empty;
    }

    public static IEnumerable<char> GetSymbols(this ValueDelimiterRules rules)
    {
        if (rules.HasFlag(ValueDelimiterRules.AllowSemicolonSymbol))
            yield return ';';
        if (rules.HasFlag(ValueDelimiterRules.AllowCommaSymbol))
            yield return ',';
        if (rules.HasFlag(ValueDelimiterRules.AllowPipeSymbol))
            yield return '|';
    }

    private static IEnumerable<string> EnumerateByDelimiter(this string textValue, IEnumerable<char> delimiters)
    {
        if (string.IsNullOrWhiteSpace(textValue))
            yield break;

        var values = new List<string> { textValue };
        values = delimiters.Aggregate(values, (current, d) => current.SelectMany(v => v.Split(d)).ToList());

        foreach (var value in values.Where(v => !string.IsNullOrWhiteSpace(v)))
        {
            yield return value.Trim();
        }
    }
}
