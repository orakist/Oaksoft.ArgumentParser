[![NuGet](https://img.shields.io/nuget/dt/Oaksoft.ArgumentParser.svg)](https://www.nuget.org/packages/Oaksoft.ArgumentParser/)
[![NuGet](https://img.shields.io/nuget/vpre/Oaksoft.ArgumentParser.svg)](https://www.nuget.org/packages/Oaksoft.ArgumentParser/)

# Command Line Arguments Parser Library for .Net

**Oaksoft.ArgumentParser** is a fluent and simple command line arguments parser library. It is currently in under development and this documentation is for version v1.0.0.

## Quick Start Example

1. Create a class to define your options.
2. Register your options and build the parser.

```cs
using Oaksoft.ArgumentParser;
using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Parser;

namespace QuickStart;

internal class CalculatorOptions
{
    public double LeftOperand { get; set; }
    public double RightOperand { get; set; }
    public string? Operator { get; set; }
}

internal static class Program
{
    private static void Main(string[] args)
    {
        try
        {
            var parser = CommandLine.CreateParser<CalculatorOptions>()
                .AddNamedOption(p => p.LeftOperand)
                .AddNamedOption(p => p.RightOperand)
                .AddNamedOption(o => o.Operator)
                .Build();

            parser.Run(args, EvaluateOption);
        }
        catch (System.Exception ex)
        {
            System.Console.WriteLine(ex.Message);
        }
    }

    private static void EvaluateOption(IArgumentParser<CalculatorOptions> parser, CalculatorOptions option)
    {
        if (!parser.IsValid || string.IsNullOrWhiteSpace(option.Operator))
            return;

        var result = option.Operator.ToUpperInvariant() switch
        {
            "ADD" => $"{option.LeftOperand} + {option.RightOperand} = {option.LeftOperand + option.RightOperand}",
            "SUB" => $"{option.LeftOperand} - {option.RightOperand} = {option.LeftOperand - option.RightOperand}",
            "MUL" => $"{option.LeftOperand} * {option.RightOperand} = {option.LeftOperand * option.RightOperand}",
            "DIV" => $"{option.LeftOperand} / {option.RightOperand} = {option.LeftOperand / option.RightOperand}",
            _ => "Invalid argument!"
        };

        System.Console.WriteLine($"Result: {result}");
        System.Console.WriteLine();
    }
}
```

Sample Command line output for the above console application
```
Type the options and press enter. Type 'q' to quit.
./> -l 13 -r 8 -o MUL
Result: 13 * 8 = 104
```

## Library Overview
This documentation shows how to create a .NET command-line app that uses the Oaksoft.ArgumentParser library. You'll begin by creating a simple option. Then you'll add to that base, creating a more complex app that contains multiple options.

In this documentation, you learn how to:

- Create switch, counter, named and value options.
- Specify default value for options.
- Specify allowed values for options.
- Create aliases for named options.
- Work with string, string[], int, bool, double, ... option types.
- Use custom code for parsing and validating options.
- Configure option and value counts.
- Configure sequential values.
- Configure option usage and decription texts.

Soon, i will add detailed documentation and describe the topics above!
