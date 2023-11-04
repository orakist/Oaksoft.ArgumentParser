﻿using System;
using Oaksoft.ArgumentParser.Extensions;

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

    private static void AddCustomOptions(ApplicationOptions options)
    {
        options.AddSwitchOption(o => o.AddSwitch)
            .WithDescription("Enables the addition operator.");

        options.AddSwitchOption(o => o.SubtractSwitch)
            .WithDescription("Enables the subtraction operator.");

        options.AddCountOption(o => o.MultiplySwitch)
            .WithOptionArity(0, 5)
            .WithDescription("Enables the multiplication operator.");

        options.AddCountOption(o => o.DivideSwitch)
            .WithDescription("Enables the division operator.");

        options.AddScalarOption(o => o.AddCount, mustHaveOneValue: false)
            .WithDefaultValue(1)
            .WithConstraints(1, 20)
            .WithDescription("Sets addition count. If no option value is given, a random value is generated. Value must be between 1 and 20.");

        options.AddScalarOption(o => o.SubtractCount)
            .WithDefaultValue("2")
            .WithDescription("Sets subtraction count.");

        options.AddScalarOption(o => o.StartTime)
            .WithDefaultValue(DateTime.Now)
            .WithDescription("Sets the start time.");

        options.AddScalarOption(o => o.MultiplyCount)
            .WithDefaultValue(0F)
            .WithDescription("Sets multiplication count.");

        options.AddScalarOption(o => o.DivideCount)
            .WithDefaultValue(0L)
            .WithDescription("Sets division count.");

        options.AddScalarOption(o => o.AddNumbers)
            .WithOptionArity(0, 2)
            .WithValueArity(0, 20)
            .WithDescription("Defines numbers for addition.");

        options.AddScalarOption(o => o.SubtractNumbers)
            .WithDescription("Defines numbers for subtraction.");

        options.AddScalarOption(o => o.MultiplyNumbers)
            .WithDescription("Defines numbers for multiplication.");

        options.AddScalarOption(o => o.DivideNumbers)
            .WithDescription("Defines numbers for division.");

        options.AddScalarOption(o => o.FormulaSign, o => o.FormulaEnabled)
            .WithDefaultValue("=")
            .WithDescription("Sets formula sign.");

        options.AddScalarOption(o => o.FormulaResults, o => o.FormulaCount)
            .WithDescription("Defines formula expressions.");

        options.AddValueOption(o => o.Variables)
            .WithUsage("variable-names")
            .WithDescription("Defines variables to use them in formulas.");
    }
}
