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

            parser.Run(args, EvaluateOption);
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Fatal error occurred.");
            System.Console.WriteLine(ex.Message);
        }
    }

    private static void EvaluateOption(IArgumentParser<CalculatorOptions> parser, CalculatorOptions option)
    {
        if (!parser.IsValid || string.IsNullOrEmpty(option.Operator))
            return;

        var numbers = new List<double>();
        if (option.LeftOperand.HasValue || option.RightOperand.HasValue)
        {
            if (option.LeftOperand.HasValue)
                numbers.Add(option.LeftOperand.Value);
            if (option.RightOperand.HasValue)
                numbers.Add(option.RightOperand.Value);
        }
        else if (option.Numbers?.Any() == true)
        {
            numbers.AddRange(option.Numbers);
        }
        else if (option.Integers?.Any() == true)
        {
            numbers.AddRange(option.Integers.Select(i => (double)i));
        }

        if (numbers.Count < 1)
        {
            System.Console.WriteLine("Provide at least one number.");
            return;
        }

        var result = option.Operator.ToUpperInvariant() switch
        {
            "ADD" => numbers.Sum(),
            "SUB" => numbers.First() - numbers.Skip(1).Sum(),
            "MUL" => numbers.Aggregate<double, double>(1, (current, number) => current * number),
            "DIV" => numbers.Skip(1).Aggregate(numbers.First(), (current, number) => current / number),
            _ => 0
        };

        var equation = option.Operator.ToUpperInvariant() switch
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
