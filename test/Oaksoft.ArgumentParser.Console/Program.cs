using System;
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

                System.Console.WriteLine("Type the commands and press enter. Type 'q' to quit.");
                System.Console.Write("./> ");
                var commands = System.Console.In.ReadLine();
                if (commands is "q" or "Q")
                    break;

                var arguments = commands?.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
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
            .WithDescription("Enables add operator.");

        options.AddSwitchOption(o => o.SubtractSwitch)
            .WithDescription("Enables subtract operator.");

        options.AddCountOption(o => o.MultiplySwitch, requiredTokenCount: 0, maximumTokenCount: 5)
            .WithDescription("Enables multiply operator.");

        options.AddCountOption(o => o.DivideSwitch, requiredTokenCount: 0, maximumTokenCount: 2)
            .WithDescription("Enables divide operator.");

        options.AddScalarOption(o => o.AddCount, valueTokenMustExist: false)
            .WithDefaultValue("1")
            .WithConstraints("1", "20")
            .WithDescription("Specifies addition count. If no option value is given, a random value is generated. Value must be between 1 and 20.");

        options.AddScalarOption(o => o.SubtractCount)
            .WithDefaultValue("2")
            .WithDescription("Specifies subtraction count.");

        options.AddScalarOption(o => o.MultiplyCount)
            .WithDefaultValue("0")
            .WithDescription("Specifies multiplication count.");

        options.AddScalarOption(o => o.DivideCount)
            .WithDefaultValue("0")
            .WithDescription("Specifies division count.");

        options.AddScalarOption(o => o.AddNumbers, valueTokenMustExist: false)
            .WithDefaultValue("1")
            .WithConstraints("1", "20")
            .WithDescription("Specifies addition count. If no option value is given, a random value is generated. Value must be between 1 and 20.");

        options.AddScalarOption(o => o.SubtractNumbers)
            .WithDefaultValue("2")
            .WithDescription("Specifies subtraction count.");

        options.AddScalarOption(o => o.MultiplyNumbers)
            .WithDefaultValue("0")
            .WithDescription("Specifies multiplication count.");

        options.AddScalarOption(o => o.DivideNumbers)
            .WithDefaultValue("0")
            .WithDescription("Specifies division count.");

        options.AddDefaultOption(o => o.Variables, requiredTokenCount: 0, maximumTokenCount: 10)
            .WithUsage("variable-names")
            .WithDescription("Defines variables to use them in formulas.");
    }
}
