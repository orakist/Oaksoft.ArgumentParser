using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Callbacks;

internal sealed class IntegerParsingCallbacks : BaseParsingCallbacks
{
    public static readonly IntegerParsingCallbacks Instance = new();

    private IntegerParsingCallbacks()
    {
    }

    public override bool ValidateValue(string value)
    {
        return int.TryParse(value, out _);
    }

    public override bool ValidateOption(IValueContext context, IArgumentParser parser)
    {
        var (minValue, maxValue) = GetMinMaxValues(context.Constraints);
        var allowedIntegers = context.AllowedValues.Select(int.Parse).ToArray();

        foreach (var parsedValue in context.ParsedValues)
        {
            if (!int.TryParse(parsedValue, out var number))
                throw new Exception($"Please provide a valid number option value. Value: {parsedValue}");

            if (number < minValue)
                throw new Exception($"Number value is out of the range. Value: {number}, Min: {minValue}");

            if (number > maxValue)
                throw new Exception($"Number value is out of the range. Value: {number}, Max: {maxValue}");

            if (allowedIntegers.Length > 0 && !allowedIntegers.Contains(number))
            {
                var values = string.Join(parser.ValueSeparator, allowedIntegers);
                throw new Exception($"Invalid allowed option value. Value: {number}, Allowed Values: {values}");
            }
        }

        return true;
    }

    public override void ApplyDefaultValue(IValueContext context, IApplicationOptions appOptions, PropertyInfo property)
    {
        if (!property.PropertyType.IsAssignableFrom(typeof(int)))
            return;

        if (int.TryParse(context.DefaultValue, out var number))
        {
            property.SetValue(appOptions, number);
        }
    }

    public override void UpdateOptionValue(IValueContext context, IApplicationOptions appOptions, PropertyInfo property)
    {
        var parsedValues = context.ParsedValues.Select(int.Parse);
        if (property.PropertyType.IsAssignableFrom(typeof(List<int>)))
        {
            property.SetValue(appOptions, parsedValues.ToList());
        }
        else if (property.PropertyType.IsAssignableFrom(typeof(int[])))
        {
            property.SetValue(appOptions, parsedValues.ToArray());
        }
        else if (property.PropertyType.IsAssignableFrom(typeof(int)))
        {
            property.SetValue(appOptions, parsedValues.First());
        }
    }

    private static (int? Min, int? Max) GetMinMaxValues(IReadOnlyList<string?> constraints)
    {
        int? minValue = constraints.Count > 0 && constraints[0] is not null ? int.Parse(constraints[0]!) : null;
        int? maxValue = constraints.Count > 1 && constraints[1] is not null ? int.Parse(constraints[1]!) : null;
        return (minValue, maxValue);
    }
}
