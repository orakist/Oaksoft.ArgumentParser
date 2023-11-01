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
        options.AddDefaultOption(o => o.FilePaths, requiredTokenCount: 1, maximumTokenCount: 10)
            .WithName("FilePath")
            .WithUsage("log-file-path")
            .WithDescription("File path of the log file or directory path of the '*.log' files. " +
                             "Provided path count must be between 1 and 10. At least 1 path is required.");

        options.AddSwitchOption(o => o.ShowMismatches)
            .WithCommands("m", "mismatch")
            .WithDescription("Prints calculation mismatches between Spreadsheet and Microsoft Excel.");

        options.AddSwitchOption(o => o.ShowFormulas)
            .WithCommands("f", "formula-counts")
            .WithDescription("Prints used formula names and usage count of the formulas in the workbook.");

        options.AddSwitchOption(o => o.ShowLoadingErrors)
            .WithCommands("e", "loading-errors")
            .WithDescription("Prints all collected errors while loading the workbook.");

        options.AddSwitchOption(o => o.CompareGrammars)
            .WithCommands("g", "compare-grammars")
            .WithDescription("Compares fast grammar and slow grammar parsing results.");

        options.AddSwitchOption(o => o.CalculationPerformance)
            .WithCommands("cp", "calc-perf")
            .WithDescription("Prints calculation performance statistics sheet by sheet (default).");

        options.AddSwitchOption(o => o.ReadingPerformance)
            .WithCommands("rp", "read-perf")
            .WithDescription("Prints total calculation performance statistics of workbook then prints reading " +
                             "performance statistics sheet by sheet.");

        options.AddIntegerOption(o => o.CalculationCount, minIntegerValue: 1, maxIntegerValue: 20)
            .WithCommands("cc", "calc-count")
            .WithDefaultValue("0")
            .WithDescription("Subsequent calculation count of the workbook. Default count value is 0. " +
                             "It must be between 1 and 20.");

        options.AddStringOption(o => o.RangeFilter)
            .WithUsage("--r <reference>")
            .WithDescription("Specifies the range where mismatches will be checked or specifies the range will be calculated. " +
                             "Range can be reference address or defined name address.");

        options.AddStringOption(o => o.ValueTypeFilter, valueTokenMustExist: true, maximumTokenCount: 1)
            .WithCommands("i", "ignore-type")
            .WithAllowedValues("number", "string", "boolean", "error", "blank")
            .WithUsage("--i <type1(;type2)(;...)>")
            .WithDescription("Specifies the ignored cell types while printing mismatches. " +
                             "Valid Types: number, string, boolean, error, blank");

        options.AddIntegerOption(o => o.PrecisionFilter, minIntegerValue: 1, maxIntegerValue: 15)
            .WithDefaultValue("8")
            .WithDescription("Sets the precision (epsilon) number between two numbers. The number represents the decimal digit. " +
                             "Default value is 8. It must be between 1 and 15.");

        options.AddStringOption(o => o.AutomationOutputPath, o => o.AutomationEnabled, valueTokenMustExist: false)
            .WithUsage("--a (automation-nodes-file-path)")
            .WithDescription("Calculates automated application input nodes and writes output to a csv file. If output file path is not " +
                             "provided path of the workbook is used. Default name of the output file is 'automation-nodes-{datetime}.csv'.");

        options.AddStringOption(o => o.DependencyOutputPath, o => o.DependencyEnabled, valueTokenMustExist: false)
            .WithCommands("d", "dependency", "dep-all")
            .WithUsage("--d (dependency-nodes-file-path)")
            .WithDescription("Calculates dependency tree nodes and writes (only the defined name nodes) output to a csv file. If you " +
                             "want to see all nodes of the tree use --dep-all command. If output file path is not provided path of " +
                             "the workbook is used. Default name of the output file is 'dependency-nodes-{datetime}.csv'.");

        options.AddStringOption(o => o.SolverInputPath, o => o.SolverEnabled)
            .WithUsage("--s <solver-input-file-path>")
            .WithDescription("Executes solver with a given input json file and writes output to a file. Solver output file is created " +
                             "in the same folder as the given input file. Name of the output file is 'solver-output-{datetime}.json'.");

        options.AddStringOption(o => o.LogFileOutputPath, o => o.LogFileEnabled, valueTokenMustExist: false)
            .WithCommands("o", "log-output")
            .WithUsage("--o (log-output-file-path)")
            .WithDescription("Mirrors console output text into a file. If output filepath is not provided path of the workbook is used. " +
                             "Default name of the output file is 'log-output-{datetime}.txt'.");

        options.AddStringOption(o => o.Culture)
            .WithCommands("l", "locale")
            .WithDefaultValue("en-US")
            .WithUsage("--l <culture-name>")
            .WithDescription("Changes the culture of the formatting. Default: en-US");

        options.AddSwitchOption(o => o.Verbose)
            .WithDescription("Prints detailed information about calculation of the workbook.");
    }
}
