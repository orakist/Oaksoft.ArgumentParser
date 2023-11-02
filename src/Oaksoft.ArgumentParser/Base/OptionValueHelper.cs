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

                var values = string.Join(parser.ValueSeparator, allowedValues);
                throw new Exception($"Invalid input value found. Value: {inputValue}, Allowed Values: : {values}");
            }
        }
        else
        {
            foreach (var inputValue in inputValues)
            {
                if (allowedValues.Any(a => a.Equals(inputValue)))
                    continue;

                var values = string.Join(parser.ValueSeparator, allowedValues);
                throw new Exception($"Invalid input value found. Value: {inputValue}, Allowed Values: : {values}");
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
                foreach (var value in valueToken.EnumerateBySeparator(parser.ValueSeparator))
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

    public static IEnumerable<string> EnumerateBySeparator(this string textValue, string separator)
    {
        if (string.IsNullOrWhiteSpace(textValue))
            yield break;

        foreach (var value in textValue.Split(separator))
        {
            if (!string.IsNullOrWhiteSpace(value))
                yield return value.Trim();

        }
    }
}
