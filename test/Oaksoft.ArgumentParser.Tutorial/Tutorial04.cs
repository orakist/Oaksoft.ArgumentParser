using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Tutorial;

internal static class Tutorial04
{
    public class CalculatorOptions
    {
        public double Left { get; set; }

        public double Right { get; set; }

        public string? Calculate { get; set; }
    }

    public static IArgumentParser<CalculatorOptions> Build()
    {
        return CommandLine.CreateParser<CalculatorOptions>()
            .AddNamedOption(p => p.Left,
                o => o.WithDescription("Left operand of the operation."),
                mandatoryOption: true)
            .AddNamedOption(p => p.Right,
                o => o.WithDescription("Right operand of the operation."),
                mandatoryOption: true)
            .AddNamedOption(o => o.Calculate,
                o => o.WithDescription("Defines operator type of the calculation.")
                    .WithAllowedValues("add", "sub", "mul", "div", "pow")
                    .WithDefaultValue("add"),
                mandatoryOption: true, mustHaveOneValue: false)
            .Build();
    }

    public static void Parse(IArgumentParser<CalculatorOptions> parser, string[] args)
    {
        Console.WriteLine($"Inputs: {string.Join(' ', args)}");

        var options = parser.Parse(args);

        if (!parser.IsParsed)
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
