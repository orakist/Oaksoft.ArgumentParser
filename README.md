[![NuGet](https://img.shields.io/nuget/dt/Oaksoft.ArgumentParser.svg)](https://www.nuget.org/packages/Oaksoft.ArgumentParser/)
[![NuGet](https://img.shields.io/nuget/vpre/Oaksoft.ArgumentParser.svg)](https://www.nuget.org/packages/Oaksoft.ArgumentParser/)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/orakist/Oaksoft.ArgumentParser/blob/dev/LICENSE.md)

# Command Line Arguments Parser Library for .Net

**Oaksoft.ArgumentParser** is a fluent and simple command line arguments parser library. It is currently under development but latest version is stable. And this documentation is for version **v1.1.0.**</br>
This library is compatible with **.Net 6.0+**, **.Net Standard 2.1**

## Quick Start Example

1. Create a class to define your options.
2. Register your options and build the parser.
3. See following example for a quick start.
4. Please see [Tutorial Example](https://github.com/orakist/Oaksoft.ArgumentParser/blob/dev/docs/Tutorial.md) for a detailed example.

```cs
using Oaksoft.ArgumentParser;
using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Parser;

namespace QuickStart;

internal class CalculatorOptions
{
    public double Left { get; set; }
    public double Right { get; set; }
    public string? Operator { get; set; }
}

internal static class Program
{
    private static void Main(string[] args)
    {
        try
        {
            var parser = CommandLine.CreateParser<CalculatorOptions>()
                .AddNamedOption(p => p.Left)
                .AddNamedOption(p => p.Right)
                .AddNamedOption(o => o.Operator)
                .Build();

            parser.Run(args, EvaluateOptions);
        }
        catch (System.Exception ex)
        {
            System.Console.WriteLine(ex.Message);
        }
    }

    private static void EvaluateOptions(IArgumentParser<CalculatorOptions> parser, CalculatorOptions options)
    {
        if (!parser.IsValid || string.IsNullOrWhiteSpace(options.Operator))
            return;

        var result = options.Operator.ToUpperInvariant() switch
        {
            "ADD" => $"{options.Left} + {options.Right} = {options.Left + options.Right}",
            "SUB" => $"{options.Left} - {options.Right} = {options.Left - options.Right}",
            "MUL" => $"{options.Left} * {options.Right} = {options.Left * options.Right}",
            "DIV" => $"{options.Left} / {options.Right} = {options.Left / options.Right}",
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

## Library Features & Overview
This documentation shows how to create a .NET command-line app that uses the Oaksoft.ArgumentParser library. You'll begin by creating a simple option. Then you'll add to that base, creating a more complex app that contains multiple options.

In this documentation, you learn how to:

- Create named and value options.
- Specify ***default value*** for options.
- Specify ***allowed values*** for options.
- Create ***aliases*** for named options.
- Work with string, string[], int, bool, double, ... option types.
- Use custom code for parsing and validating options.
- Configure option and value counts.
- Configure sequential values.
- Configure option usage and description texts.
- Use built-in ***help*** and ***version*** options.

## 1. Option Types

There are two kinds of options. These are Named options and Value options.

### 1.1. Named Options

If an option has an alias, it is called a named option. Prefix of an alias can be two hyphens (--), one hyphen (-) or forward slash (/). 
These are some valid commands according to the default alias prefix rules.

```
./> myapp --open file.txt --read 100 --verbosity quiet
./> myapp /open file.txt /read 100 /verbosity quiet
./> myapp -o file.txt -r 100 -v quiet
```

First command is parsed by the library into these options: (--open file.txt), (--read 10), (--verbosity quiet).</br>
Please see [Parsing Rules](https://github.com/orakist/Oaksoft.ArgumentParser/blob/dev/docs/ParsingRules.md) for detailed parsing settings.

There are 4 types of named options.

1. Scalar Named Option
   Scalar named option requires zero or one argument value. Option name may be repeated more than one time. Scalar named option grabs only last value.</br>
   Example: --number 123 --number 456 (number: 456)
2. Sequential Named Option
   Sequential named option requires one or more argument values. Option name may be repeated more than one time. A sequential named option grabs all values.
   Example: --numbers 123 321 --numbers 456|789 (numbers: {123, 321, 456, 789})
3. Switch Option
   Switch option is a boolean type. If it is passed in the command-line, it default value will be true.
   Example: --start (start: true)
4. Counter Option
   Counter option counts occurences of the option in the command-line.
   Example: --next --next -n -n /n /next (next: 6)

Please see [Named Options](https://github.com/orakist/Oaksoft.ArgumentParser/blob/dev/docs/NamedOptions.md) for detailed named option usages and settings.

### 1.2. Value Options

- They are options without an alias. They are unbound values. They don't begin with option prefixes.
- If you don't have any named option, you can simply parse command line with value options.
- ArgumentParser tries to parse value options according to the option registration order.
- Also you can register value options and named options to the same parser.
- See following examples.

```cs
class ExampleOptions
{
    public int Count { get; set; }
    public double Total { get; set; }
    public List<string> Names { get; set; }
}
```

See, how the following parser configuration parses the commandline inputs.

```
var parser = CommandLine.CreateParser<ExampleOptions>()
    .AddValueOption(p => p.Count) // firstly register count value
    .AddValueOption(p => p.Total) // secondly register total value
    .AddValueOption(o => o.Names) // thirdly register name list
    .Build();

./> frodo 10.5 sam 30 gandalf|pippin => count: 30, total: 10.5, names: {frodo, sam, gandalf, pippin}
./> frodo 10 sam 30.5 gandalf|pippin => count: 10, total: 30.5, names: {frodo, sam, gandalf, pippin}
```

```
var parser = CommandLine.CreateParser<ExampleOptions>()
    .AddValueOption(p => p.Count) // firstly register count value
    .AddValueOption(p => p.Names) // secondly register name list
    .AddValueOption(o => o.Total) // thirdly register total value
    .Build();

./> frodo 10.5 sam 30 gandalf|pippin => count: 30, total: 0, names: {frodo, 10.5, sam, gandalf, pippin}
./> frodo 10 sam 30.5 gandalf|pippin => count: 10, total: 0, names: {frodo, sam, 30.5, gandalf, pippin}
```

## 2. Default Value

Description will be added!

## 3. Allowed Option Values

Description will be added!

## 4. Option Aliases

Description will be added!

## 5. Custom Option Parser & Validator

Description will be added!

## 6. Arity Configuration

Description will be added!

## 7. Other Option Configurations

Description will be added!

## 8. Built-In Options

Description will be added!

## 9. Step by Step Tutorial

Description will be added!
