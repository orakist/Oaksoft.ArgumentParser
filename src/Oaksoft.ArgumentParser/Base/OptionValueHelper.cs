using System;
using System.Collections.Generic;
using System.Linq;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Base;

internal static class OptionValueHelper
{
    public static void ValidateByAllowedValues<TValue>(
        this IArgumentParser parser, List<TValue> inputValues, ICollection<TValue> allowedValues)
        where TValue : IComparable, IEquatable<TValue>
    {
        if (allowedValues.Count <= 0)
            return;

        if (typeof(TValue) == typeof(string))
        {
            var flag = parser.ComparisonFlag();
            foreach (var inputValue in inputValues.Cast<string>())
            {
                if (allowedValues.Cast<string>().Any(a => a.Equals(inputValue, flag)))
                    continue;

                var values = string.Join(parser.ValueDelimiter, allowedValues);
                throw new Exception($"Option value '{inputValue}' not recognized. Must be one of: {values}");
            }
        }
        else
        {
            foreach (var inputValue in inputValues)
            {
                if (allowedValues.Any(a => a.Equals(inputValue)))
                    continue;

                var values = string.Join(parser.ValueDelimiter, allowedValues);
                throw new Exception($"Option value '{inputValue}' not recognized. Must be one of: {values}");
            }
        }
    }

    public static IEnumerable<string> GetInputValues(
        this IArgumentParser parser, List<string> valueTokens, bool enableValueTokenSplitting)
    {
        foreach (var valueToken in valueTokens)
        {
            if (enableValueTokenSplitting)
            {
                foreach (var value in valueToken.EnumerateByDelimiter(parser.ValueDelimiter))
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

    public static StringComparison ComparisonFlag(this IArgumentParser parser)
    {
        return parser.CaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
    }

    public static IEnumerable<string> EnumerateByDelimiter(this string textValue, string delimiter)
    {
        if (string.IsNullOrWhiteSpace(textValue))
            yield break;

        foreach (var value in textValue.Split(delimiter))
        {
            if (!string.IsNullOrWhiteSpace(value))
                yield return value.Trim();

        }
    }
}
