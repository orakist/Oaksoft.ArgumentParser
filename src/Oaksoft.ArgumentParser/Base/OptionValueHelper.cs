using System;
using System.Collections.Generic;
using System.Linq;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Base;

internal static class OptionValueHelper
{
    public static void ValidateDefaultValue(this Func<string, bool>? validator, string? defaultValue)
    {
        if (validator is null || defaultValue is null)
            return;

        if (!validator.Invoke(defaultValue))
        {
            throw new ArgumentException(
                $"Invalid default value found! Value: {defaultValue}",
                nameof(defaultValue));
        }
    }

    public static void ValidateAllowedValues(this Func<string, bool>? validator, HashSet<string> allowedValues)
    {
        if (validator is null || allowedValues.Count < 1)
            return;

        foreach (var allowedValue in allowedValues.Where(v => !validator.Invoke(v)))
        {
            throw new ArgumentException(
                $"Invalid allowed value found!. Value: {allowedValue}",
                nameof(allowedValues));
        }
    }

    public static void ValidateConstraints(this Func<string, bool>? validator, List<string?> constraints)
    {
        if (validator is null || constraints.Count < 1)
            return;

        foreach (var constraint in constraints.Where(v => v is not null).Where(v => !validator.Invoke(v!)))
        {
            throw new ArgumentException(
                $"Invalid constraint value found!. Value: {constraint}",
                nameof(constraints));
        }
    }

    public static void ValidateByAllowedValues(this IArgumentParser parser, List<string> parsedValues, ICollection<string> allowedValues)
    {
        if (allowedValues.Count <= 0)
            return;

        var flag = parser.ComparisonFlag();
        foreach (var parsedValue in parsedValues)
        {
            if (allowedValues.Any(a => a.Equals(parsedValue, flag)))
                continue;

            var values = string.Join(parser.ValueSeparator, allowedValues);
            throw new Exception($"Invalid allowed option value. Value: {parsedValue}, Allowed Values: : {values}");
        }
    }

    public static IEnumerable<string> GetParsedValues(this IArgumentParser parser, List<string> valueTokens, bool enableValueTokenSplitting)
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
