using System;
using System.Collections.Generic;
using System.Linq;
using Oaksoft.ArgumentParser.Builder;
using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Tester;

internal static class Program
{
    private static void Main(string[] args)
    {
        try
        {
            CommandLine.CreateParser<CalculatorOptions>()
                .ConfigureOptions()
                .Run(EvaluateOptions, args);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private static void EvaluateOptions(IArgumentParser parser, CalculatorOptions options)
    {
        if (!parser.IsParsed || options.Operator == null)
        {
            return;
        }

        var numbers = new List<double>();
        if (options.LeftOperand.HasValue || options.RightOperand.HasValue)
        {
            if (options.LeftOperand.HasValue)
            {
                numbers.Add(options.LeftOperand.Value);
            }

            if (options.RightOperand.HasValue)
            {
                numbers.Add(options.RightOperand.Value);
            }
        }
        else
        {
            if (options.Numbers?.Any() == true)
            {
                numbers.AddRange(options.Numbers);
            }

            if (options.Integers?.Any() == true)
            {
                numbers.AddRange(options.Integers.Select(i => (double)i));
            }

            if (options.Decimals?.Any() == true)
            {
                numbers.AddRange(options.Decimals.Select(i => (double)i));
            }
        }

        if (numbers.Count < 1)
        {
            Console.WriteLine("Provide at least one number.");
            return;
        }

        var equation = options.Operator switch
        {
            OperatorType.Add => $"{string.Join(" + ", numbers)} = {numbers.Sum()}",
            OperatorType.Sub => $"{string.Join(" - ", numbers)} = {numbers.First() - numbers.Skip(1).Sum()}",
            OperatorType.Mul => $"{string.Join(" * ", numbers)} = {numbers.Aggregate<double, double>(1, (x, y) => x * y)}",
            OperatorType.Div => $"{string.Join(" / ", numbers)} = {numbers.Skip(1).Aggregate(numbers.First(), (x, y) => x / y)}",
            _ => "Invalid Argument!"
        };

        Console.WriteLine($"Result: {equation}");
    }

    private static IArgumentParser<CalculatorOptions> ConfigureOptions(this IArgumentParserBuilder<CalculatorOptions> builder)
    {
        return builder
            .AddValueOption(p => p.Integers)
            .AddValueOption(p => p.Decimals)
            .AddNamedOption(p => p.LeftOperand, o => o.WithDescription("Left operand of the operation."))
            .AddNamedOption(p => p.RightOperand, o => o.WithDescription("Right operand of the operation."))
            .AddNamedOption(p => p.Numbers, o => o.WithDescription("Defines numbers for the operation."))
            .AddNamedOption(o => o.Operator, o => o.WithDescription("Sets the operator type."), mandatoryOption: true)
            .Build();
    }
}
