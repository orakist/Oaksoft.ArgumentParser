using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Tutorial;

internal static class Tutorial06
{
public class CalculatorOptions
{
    public IEnumerable<double>? Numbers { get; set; }

    public string? Calculate { get; set; }
}

private static bool TryParseCustom(string value, out double result)
{
    if (value.StartsWith('(') && value.EndsWith(')'))
        value = value.Substring(1, value.Length - 2);

    return double.TryParse(value, out result);
}

public static IArgumentParser<CalculatorOptions> Build()
{
    return CommandLine.CreateParser<CalculatorOptions>()
        .AddNamedOption(p => p.Numbers,
            o => o.WithDescription("Defines the numbers to be calculated.")
                .AddPredicate(v => v >= 0)
                .WithTryParseCallback(TryParseCustom)
                .WithOptionArity(ArityType.OneOrMore)
                .WithValueArity(2, int.MaxValue))
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

    if (!parser.IsValid || parser.IsEmpty || parser.IsHelpOption || parser.IsVersionOption)
        return;

    var numbers = options.Numbers!.ToList();
    var equation = options.Calculate?.ToUpperInvariant() switch
    {
        "ADD" => $"{string.Join(" + ", numbers)} = {numbers.Sum()}",
        "SUB" => $"{string.Join(" - ", numbers)} = {numbers.First() - numbers.Skip(1).Sum()}",
        "MUL" => $"{string.Join(" * ", numbers)} = {numbers.Aggregate<double, double>(1, (x, y) => x * y)}",
        "DIV" => $"{string.Join(" / ", numbers)} = {numbers.Skip(1).Aggregate(numbers.First(), (x, y) => x / y)}",
        "POW" => $"{string.Join(" ^ ", numbers)} = {numbers.Skip(1).Aggregate(numbers.First(), Math.Pow)}",
        _ => "Invalid Argument!"
    };

    Console.WriteLine($"Result: {equation}");
}
}
