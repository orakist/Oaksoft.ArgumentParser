using System;
using System.Collections.Generic;
using System.Linq;
using Oaksoft.ArgumentParser.Builder;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Console;

internal static class Program
{
    private static void Main(string[] args)
    {
        try
        {
            var parser = CommandLine.CreateParser<CalculatorOptions>()
                .ConfigureOptions()
                .Build();

            parser.Run(EvaluateOptions, args);
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex);
        }
    }

    private static void EvaluateOptions(IArgumentParser<CalculatorOptions> parser, CalculatorOptions options)
    {
        if (!parser.IsValid || string.IsNullOrEmpty(options.Operator))
            return;

        var numbers = new List<double>();
        if (options.LeftOperand.HasValue || options.RightOperand.HasValue)
        {
            if (options.LeftOperand.HasValue)
                numbers.Add(options.LeftOperand.Value);
            if (options.RightOperand.HasValue)
                numbers.Add(options.RightOperand.Value);
        }
        else if (options.Numbers?.Any() == true)
        {
            numbers.AddRange(options.Numbers);
        }
        else if (options.Integers?.Any() == true)
        {
            numbers.AddRange(options.Integers.Select(i => (double)i));
        }

        if (numbers.Count < 1)
        {
            System.Console.WriteLine("Provide at least one number.");
            return;
        }

        var result = options.Operator.ToUpperInvariant() switch
        {
            "ADD" => numbers.Sum(),
            "SUB" => numbers.First() - numbers.Skip(1).Sum(),
            "MUL" => numbers.Aggregate<double, double>(1, (current, number) => current * number),
            "DIV" => numbers.Skip(1).Aggregate(numbers.First(), (current, number) => current / number),
            _ => 0
        };

        var equation = options.Operator.ToUpperInvariant() switch
        {
            "ADD" => $"{string.Join(" + ", numbers)} = {result}",
            "SUB" => $"{string.Join(" - ", numbers)} = {result}",
            "MUL" => $"{string.Join(" * ", numbers)} = {result}",
            "DIV" => $"{string.Join(" / ", numbers)} = {result}",
            _ => "Invalid Argument!"
        };

        System.Console.WriteLine($"Result: {equation}");
        System.Console.WriteLine();
    }

    private static IArgumentParserBuilder<CalculatorOptions> ConfigureOptions(this IArgumentParserBuilder<CalculatorOptions> builder)
    {
        return builder
            .AddNamedOption(p => p.LeftOperand,
                o => o.WithDescription("Left operand of the operation."))

            .AddNamedOption(p => p.RightOperand,
                o => o.WithDescription("Right operand of the operation."))

            .AddNamedOption(p => p.Numbers,
                o => o.WithDescription("Defines numbers for the operation."),
                ArityType.OneOrMore)

            .AddNamedOption(p => p.Integers,
                o => o.WithDescription("Defines integers for the operation."),
                ArityType.OneOrMore)

            .AddNamedOption(o => o.Operator,
                o => o.WithDescription("Defines operator type of the operation.")
                    .WithAllowedValues("ADD", "SUB", "MUL", "DIV"),
                mandatoryOption: true);
    }
}
