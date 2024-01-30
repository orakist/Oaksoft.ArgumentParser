using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Tutorial;

internal static class Tutorial08
{
    public enum OperatorType { Add, Sub, Mul, Div, Pow, Rem }

    public class CalculatorOptions
    {
        public IEnumerable<double>? Numbers { get; set; }

        public OperatorType? Calculate { get; set; }
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
                    .AddAliases("n", "numbers", "l", "left", "r", "right")
                    .AddPredicate(v => v >= 0)
                    .WithTryParseCallback(TryParseCustom)
                    .WithOptionArity(ArityType.OneOrMore)
                    .WithValueArity(2, int.MaxValue))
            .AddNamedOption(o => o.Calculate,
                o => o.WithDescription("Defines operator type of the calculation.")
                    .WithDefaultValue(OperatorType.Add),
                mandatoryOption: true, mustHaveOneValue: false)
            .Build();
    }

    public static void Parse(IArgumentParser<CalculatorOptions> parser, string[] args)
    {
        Console.WriteLine($"Inputs: {string.Join(' ', args)}");

        var options = parser.Parse(args);

        if (!parser.IsParsed)
            return;

        var numbers = options.Numbers!.ToList();
        var equation = options.Calculate switch
        {
            OperatorType.Add => $"{string.Join(" + ", numbers)} = {numbers.Sum()}",
            OperatorType.Sub => $"{string.Join(" - ", numbers)} = {numbers.First() - numbers.Skip(1).Sum()}",
            OperatorType.Mul => $"{string.Join(" * ", numbers)} = {numbers.Aggregate<double, double>(1, (x, y) => x * y)}",
            OperatorType.Div => $"{string.Join(" / ", numbers)} = {numbers.Skip(1).Aggregate(numbers.First(), (x, y) => x / y)}",
            OperatorType.Pow => $"{string.Join(" ^ ", numbers)} = {numbers.Skip(1).Aggregate(numbers.First(), Math.Pow)}",
            OperatorType.Rem => $"{string.Join(" % ", numbers)} = {numbers.Skip(1).Aggregate(numbers.First(), (x, y) => x % y)}",
            _ => "Invalid Argument!"
        };

        Console.WriteLine($"Result: {equation}");
    }
}
