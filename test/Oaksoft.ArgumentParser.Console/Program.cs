using System;
using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Console;

internal static class Program
{
    private static void Main(string[] args)
    {
        var parser = CommandLine.CreateParser<ApplicationOptions>()
            .ConfigureOptions(AddCustomOptions)
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

    private static void AddCustomOptions(IArgumentParserBuilder<ApplicationOptions> builder)
    {
        builder.AddSwitchOption(o => o.AddSwitch)
            .WithDescription("Enables the addition operator.");

        builder.AddSwitchOption(o => o.SubtractSwitch)
            .WithDescription("Enables the subtraction operator.");

        builder.AddCountOption(o => o.MultiplySwitch)
            .WithCustomOptionArity(0, 5)
            .WithDescription("Enables the multiplication operator.");

        builder.AddCountOption(o => o.DivideSwitch)
            .WithDescription("Enables the division operator.");

        builder.AddNamedOption(o => o.AddCount, mustHaveOneValue: false)
            .WithDefaultValue(1)
            .AddPredicate(v => v is > 0 and < 21)
            .WithDescription("Sets addition count. If no option value is given, a random value is generated. Value must be between 1 and 20.");

        builder.AddNamedOption(o => o.SubtractCount)
            .WithDefaultValue("2")
            .WithDescription("Sets subtraction count.");

        builder.AddNamedOption(o => o.StartTime)
            .WithDefaultValue(DateTime.Now)
            .WithDescription("Sets the start time.");

        builder.AddNamedOption(o => o.MultiplyCount)
            .WithDefaultValue(0F)
            .WithDescription("Sets multiplication count.");

        builder.AddNamedOption(o => o.DivideCount)
            .WithDefaultValue(0L)
            .WithDescription("Sets division count.");

        builder.AddNamedOption(o => o.AddNumbers)
            .WithCustomOptionArity(0, 2)
            .WithCustomValueArity(0, 20)
            .WithDescription("Defines numbers for addition.");

        builder.AddNamedOption(o => o.SubtractNumbers)
            .WithDescription("Defines numbers for subtraction.");

        builder.AddNamedOption(o => o.MultiplyNumbers)
            .WithDescription("Defines numbers for multiplication.");

        builder.AddNamedOption(o => o.DivideNumbers)
            .WithDescription("Defines numbers for division.");

        builder.AddNamedOption(o => o.FormulaSign, o => o.FormulaEnabled)
            .WithDefaultValue("=")
            .WithDescription("Sets formula sign.");

        builder.AddNamedOption(o => o.FormulaResults, o => o.FormulaCount)
            .WithDescription("Defines formula expressions.");

        builder.AddValueOption(o => o.Variables)
            .WithUsage("variable-names")
            .WithDescription("Defines variables to use them in formulas.");
    }
}
