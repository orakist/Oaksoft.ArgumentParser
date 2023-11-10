using System;
using Oaksoft.ArgumentParser.Builder;
using Oaksoft.ArgumentParser.Extensions;

namespace Oaksoft.ArgumentParser.Console;

internal static class Program
{
    private static void Main(string[] args)
    {
        var parser = CommandLine.CreateParser<ApplicationOptions>()
            .ConfigureOptions()
            .Build();

        try
        {
            while (true)
            {
                var result = parser.Parse(args);

                System.Console.WriteLine("Type the options and press enter. Type 'q' to quit.");
                System.Console.Write("./> ");
                var options = System.Console.In.ReadLine();
                if (options is "q" or "Q")
                    break;

                var arguments = options?.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                args = arguments ?? Array.Empty<string>();
            }
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Fatal error occurred.");
            System.Console.WriteLine(ex.Message);
        }
    }

    private static IArgumentParserBuilder<ApplicationOptions> ConfigureOptions(this IArgumentParserBuilder<ApplicationOptions> builder)
    {
        return builder
            .AddSwitchOption(p => p.AddSwitch,
                o => o.WithDescription("Enables the addition operator."))

            .AddSwitchOption(p => p.SubtractSwitch,
                o => o.WithDescription("Enables the subtraction operator."))

            .AddCountOption(o => o.MultiplySwitch,
                o => o.WithOptionArity(0, 5)
                    .WithDescription("Enables the multiplication operator."))

            .AddCountOption(o => o.DivideSwitch,
                o => o.WithDescription("Enables the division operator."))

            .AddNamedOption(o => o.AddCount,
                o => o.WithDefaultValue(1)
                    .AddPredicate(v => v is > 0 and < 21)
                    .WithDescription(
                        "Sets addition count. If no option value is given, a random value is generated. Value must be between 1 and 20."))

            .AddNamedOption(o => o.SubtractCount,
                o => o.WithDefaultValue("2")
                    .WithDescription("Sets subtraction count."))

            .AddNamedOption(o => o.StartTime,
                o => o.WithDefaultValue(DateTime.Now)
                    .WithDescription("Sets the start time."))

            .AddNamedOption(o => o.MultiplyCount,
                o => o.WithDefaultValue(0F)
                    .WithDescription("Sets multiplication count."))

            .AddNamedOption(o => o.DivideCount,
                o => o.WithDefaultValue(0L)
                    .WithDescription("Sets division count."))

            .AddNamedOption(o => o.AddNumbers,
                o => o.WithOptionArity(0, 2)
                    .WithValueArity(0, 20)
                    .WithDescription("Defines numbers for addition."))

            .AddNamedOption(o => o.SubtractNumbers,
                o => o.WithDescription("Defines numbers for subtraction."))

            .AddNamedOption(o => o.MultiplyNumbers,
                o => o.WithDescription("Defines numbers for multiplication."))

            .AddNamedOption(o => o.DivideNumbers,
                o => o.WithDescription("Defines numbers for division."))

            .AddNamedOption(o => o.FormulaSign, o => o.FormulaEnabled,
                o => o.WithDefaultValue("=")
                    .WithDescription("Sets formula sign."))

            .AddNamedOption(o => o.FormulaResults, o => o.FormulaCount,
                o => o.WithDescription("Defines formula expressions."))

            .AddValueOption(o => o.Variables,
                o => o.WithUsage("variable-names")
                    .WithDescription("Defines variables to use them in formulas."));
    }
}
