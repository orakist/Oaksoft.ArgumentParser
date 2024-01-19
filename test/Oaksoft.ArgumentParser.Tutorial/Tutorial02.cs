using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Tutorial;

internal static class Tutorial02
{
    public class CalculatorOptions
    {
        public int Left { get; set; }

        public int Right { get; set; }

        public string? Calculate { get; set; }
    }

    public static IArgumentParser<CalculatorOptions> Build()
    {
        return CommandLine.CreateParser<CalculatorOptions>()
            .AddNamedOption(p => p.Left)
            .AddNamedOption(p => p.Right)
            .AddNamedOption(o => o.Calculate)
            .Build();
    }

    public static void Parse(IArgumentParser<CalculatorOptions> parser, string[] args)
    {
        Console.WriteLine($"Inputs: {string.Join(' ', args)}");

        var options = parser.Parse(args);

        if (!parser.IsValid || parser.IsEmpty || parser.IsHelpOption || parser.IsVersionOption)
            return;

        var result = options.Calculate?.ToUpperInvariant() switch
        {
            "ADD" => $"{options.Left} + {options.Right} = {options.Left + options.Right}",
            "SUB" => $"{options.Left} - {options.Right} = {options.Left - options.Right}",
            "MUL" => $"{options.Left} * {options.Right} = {options.Left * options.Right}",
            "DIV" => $"{options.Left} / {options.Right} = {options.Left / options.Right}",
            "POW" => $"{options.Left} ^ {options.Right} = {Math.Pow(options.Left, options.Right)}",
            _ => "Invalid argument!"
        };

        Console.WriteLine($"Result: {result}");
    }
}
