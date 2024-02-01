[![NuGet](https://img.shields.io/nuget/dt/Oaksoft.ArgumentParser.svg)](https://www.nuget.org/packages/Oaksoft.ArgumentParser/)
[![NuGet](https://img.shields.io/nuget/vpre/Oaksoft.ArgumentParser.svg)](https://www.nuget.org/packages/Oaksoft.ArgumentParser/)
[![Build Status](https://github.com/orakist/Oaksoft.ArgumentParser/actions/workflows/coveralls.yml/badge.svg)](https://github.com/orakist/Oaksoft.ArgumentParser/actions/workflows/coveralls.yml)
[![Coverage Status](https://coveralls.io/repos/github/orakist/Oaksoft.ArgumentParser/badge.svg?branch=dev)](https://coveralls.io/github/orakist/Oaksoft.ArgumentParser?branch=dev)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/orakist/Oaksoft.ArgumentParser/blob/dev/LICENSE.md)

# Command Line Arguments Parser Library for .Net

**Oaksoft.ArgumentParser** is a fluent and simple command line arguments parser library. It is currently under 
development but latest version is stable. And this documentation is for the latest version.\
This library is compatible with **.Net 6.0+**, **.Net Standard 2.1**

## Quick Start Example

1. Create a class to define your options.
2. Register your options and build the parser.
3. See following example for a quick start.

```cs
using Oaksoft.ArgumentParser;

namespace QuickStart;

enum OperatorType { Add, Sub, Mul, Div }

class CalculatorOptions
{
    public double Left { get; set; }
    public double Right { get; set; }
    public OperatorType? Operator { get; set; }
}

static class Program
{
    private static void Main(string[] args)
    {
        var parser = CommandLine.AutoBuild<CalculatorOptions>();

        var options = parser.Parse(args);

        if (!parser.IsParsed)
            return;

        var result = options.Operator switch
        {
            OperatorType.Add => $"{options.Left} + {options.Right} = {options.Left + options.Right}",
            OperatorType.Sub => $"{options.Left} - {options.Right} = {options.Left - options.Right}",
            OperatorType.Mul => $"{options.Left} * {options.Right} = {options.Left * options.Right}",
            OperatorType.Div => $"{options.Left} / {options.Right} = {options.Left / options.Right}",
            _ => "Invalid argument!"
        };

        System.Console.WriteLine($"Result: {result}");
    }
}
```

Sample Command line output for the preceding quick start example.

```console
./> -l 13 -r 8 -o MUL
Result: 13 * 8 = 104

./> --help
Description:
  This console application calculates given numbers.

Usage:
  appname [options]

Options:
  -l       Usage: -l <value>
           Aliases: -l, --left, /l, /left
           Performs 'Left' option.

  -r       Usage: -r <value>
           Aliases: -r, --right, /r, /right
           Performs 'Right' option.

  -o       Usage: -o <value>
           Aliases: -o, --operator, /o, /operator
           Performs 'Operator' option. [Allowed-Values: Add | Sub | Mul | Div]

  -h       Usage: -h
           Aliases: -h, -?, --help, /h, /?, /help
           Shows help and usage information.

  --vn     Usage: --vn
           Aliases: --vn, --version, /vn, /version
           Shows version-number of the application.
```

## Library Features & Overview
This documentation shows how to create a .NET command-line app that uses the Oaksoft.ArgumentParser library.

Please see [Tutorial with Examples](https://github.com/orakist/Oaksoft.ArgumentParser/blob/dev/docs/Tutorial.md) for a detailed tutorial.

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

```console
./> myapp --open file.txt --read 100 --verbosity quiet
./> myapp /open file.txt /read 100 /verbosity quiet
./> myapp -o file.txt -r 100 -v quiet
```

First command is parsed by the library into these options: (--open file.txt), (--read 10), (--verbosity quiet).\
Please see [Parsing Rules](https://github.com/orakist/Oaksoft.ArgumentParser/blob/dev/docs/ParsingRules.md) for detailed parsing settings.

There are 4 types of named options.

1. Scalar Named Option
   Scalar named option requires zero or one argument value. Option name may be repeated more than one time. Scalar named option grabs only last value.\
   Example: --number 123 --number 456 (number: 456)
2. Sequential Named Option
   Sequential named option requires one or more argument values. Option name may be repeated more than one time. A sequential named option grabs all values.\
   Example: --numbers 123 321 --numbers 456|789 (numbers: {123, 321, 456, 789})
3. Switch Option
   Switch option is a boolean type. It is a shorthand scalar named option for boolean types. If it is passed in the command-line, its default value will be true.\
   Example: --start (start: true)
5. Counter Option
   Counter option counts occurrences of the option in the command-line.\
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

See, how the value option registration affects the parser result for the below command-line inputs.

```cs
var parser = CommandLine.CreateParser<ExampleOptions>()
    .AddValueOption(p => p.Count) // Firstly, bind first integer token
    .AddValueOption(p => p.Total) // Secondly, bind first double token from unbinded tokens 
    .AddValueOption(o => o.Names) // Thirdly, bind all remaining tokens
    .Build();
```
```console
./> frodo 10.5 sam 30 gandalf
count: 30, total: 10.5, names: {frodo, sam, gandalf}
./> frodo 10 sam 30.5 gandalf
count: 10, total: 30.5, names: {frodo, sam, gandalf}
```

```cs
var parser = CommandLine.CreateParser<ExampleOptions>()
    .AddValueOption(p => p.Count) // firstly, bind first integer token
    .AddValueOption(p => p.Names) // Secondly, bind all remaining tokens
    .AddValueOption(o => o.Total) // Thirdly, bind first double token from unbinded tokens
    .Build();
```
```console
./> frodo 10.5 sam 30 gandalf
count: 30, total: 0, names: {frodo, 10.5, sam, gandalf}
./> frodo 10 sam 30.5 gandalf
count: 10, total: 0, names: {frodo, sam, 30.5, gandalf}
```

## 2. Default Value

Options can have default values that apply if no value is explicitly provided. For example, switch options 
are values with a default of true when the option name is in the command line. 
The following command-line examples are equivalent:

```console
./> myapp --enabled
./> myapp --enabled true
```

An option that is defined with a default value, such as number option in the following example, value of the option is treated as optional. 
*'.WithDefaultValue()'* extension method configures the default value of a switch option and scalar named option.

```cs
var parser = CommandLine.CreateParser<MyOptions>()
    .AddNamedOption(p => p.Number, o => o.WithDefaultValue(12))
    .Build();
```

According to the preceding code, values of the ***'--number'*** option in the following first two lines are equal. And value of the last line is 15.

```console
./> myapp --number
./> myapp --number 12
./> myapp --number 15
```

## 3. Allowed Option Values

To specify a list of allowed values for an option, specify an enum as the option type or use ***.WithAllowedValues()***, as shown in the following example.

```cs
enum OperatorType { Add, Sub, Mul, Div }

class MyOptions
{
    public string? Language { get; set; }
    public OperatorType? Operator { get; set; }
}

var parser = CommandLine.CreateParser<MyOptions>()
        .AddNamedOption(o => o.Operator)
        .AddNamedOption(o => o.Language, o.WithAllowedValues("C#", "C++", "Java", "PHP", "SQL"))
        .Build();
```

Here's an example of command-line input and the resulting output for the preceding example code:

```console
./> myapp --language my-lang
Option value 'my-lang' not recognized. Must be one of: [C#, C++, Java, PHP, SQL], Option: Language
./> myapp --operator abc
Option value 'abc' not recognized. Must be one of: [Add, Sub, Mul, Div], Option: Operator
```

## 4. Option Aliases

In both POSIX and Windows, it's common for some commands and options to have aliases. These are usually short forms that are easier to type. 
Aliases can also be used for other purposes, such as to simulate case-insensitivity and to support alternate spellings of a word.
Option names and aliases are case-insensitive by default. But it is configurable while creating the parser. 

POSIX short forms typically have a single leading dash followed by a single character. The following commands are equivalent:

```console
./> myapp --operator add
./> myapp -o add
```

*Oaksoft.ArgumentParser* lets you use a space, '=', or ':' as the delimiter between an option name and its argument.
For example, the following commands are equivalent:

```console
./> myapp -o add
./> myapp -o:add
./> myapp -o=add
```

A POSIX convention lets you omit the delimiter when you are specifying a single-character option alias. 
*Oaksoft.ArgumentParser* supports this syntax by default. For example, the following commands are equivalent:

```console
./> myapp -o add
./> myapp -oadd
```

Oaksoft.ArgumentParser heuristically creates aliases by using the property name. For example parser creates "f" and "file" for the 'File' property.
Aliases can also be configured manually using the parser's fluent api. If parser can't suggest an alias for an option it throws an exception.
This can happen if lots of option property names are similar. Alias names are manually configurable as shown in the following example.

```cs
var parser = CommandLine.CreateParser<MyOptions>()
        .AddNamedOption(o => o.Language, o => o.AddAliases("l", "lang", "language"))
        .Build();
```

Now, the following commands are equivalent:

```console
./> myapp -l SQL
./> myapp --lang SQL
./> myapp --language SQL
```

## 5. Arity Configuration

The arity of an option or value is the number of option or values that can be passed if that option or value is specified.
There are two types of arity configuration. These are option arity and value arity. Arity is expressed with a minimum value and a maximum value.

- Zero - No values allowed.
- ZeroOrOne - May have one value, may have no values.
- ExactlyOne - Must have one value.
- ZeroOrMore - May have one value, multiple values, or no values.
- OneOrMore - May have multiple values, must have at least one value.
- Custom minimum and maximum configuration.

Following table illustrates option arity for option '-n':

| Min | Max  | Example validity | Example                     |
|-----|------|------------------|-----------------------------|
| 0   | 0    | Invalid          | -n                          |
|     |      | Invalid          | -n 1                        |
|     |      | Invalid          | -n 1 -n 2                   |
| 0   | 1    | Valid            | -n                          |
|     |      | Valid            | -n 1                        |
|     |      | Invalid:         | -n 1 -n 2                   |
| 1   | 1    | Valid            | -n                          |
|     |      | Valid            | -n 1                        |
|     |      | Invalid:         | -n 1 -n 2                   |
| 0   | *n*  | Valid:           | -n                          |
|     |      | Valid:           | -n 1                        |
|     |      | Valid:           | -n 1 -n 2                   |
| 1   | *n*  | Valid:           | -n                          |
|     |      | Valid:           | -n 1                        |
|     |      | Valid:           | -n 1 -n 2                   |

Following table illustrates value arity:

| Min | Max     | Example validity | Example                     |
|-----|---------|------------------|-----------------------------|
| 0   | 0       | Valid:           | --file                      |
|     |         | Invalid:         | --file a.json               |
|     |         | Invalid:         | --file a.json --file b.json |
| 0   | 1       | Valid:           | --flag                      |
|     |         | Valid:           | --flag true                 |
|     |         | Valid:           | --flag false                |
|     |         | Invalid:         | --flag false --flag false   |
| 1   | 1       | Valid:           | --file a.json               |
|     |         | Invalid:         | --file                      |
|     |         | Invalid:         | --file a.json --file b.json |
| 0   | *n*     | Valid:           | --file                      |
|     |         | Valid:           | --file a.json               |
|     |         | Valid:           | --file a.json --file b.json |
| 1   | *n*     | Valid:           | --file a.json               |
|     |         | Valid:           | --file a.json b.json        |
|     |         | Invalid:         | --file                      |

#### Option overrides
If the arity maximum for an option is ZeroOrOne, *Oaksoft.ArgumentParser* can still be configured to accept multiple instances of an option.
In that case, the last instance of a repeated option overwrites any earlier instances. In the following example, the value 2 would be passed to the delay option.

```console
./> myapp -delay 3 --message example --delay 2
```

In the following example, the list passed to the list option would contain "a", "b", "c", and "d":

```console
./> myapp --list a b c --list d
```

By default,
- Option arity of Scalar Named options and Switch options are ZeroOrOne.
- Option arity of Sequential Named options and Counter options are ZeroOrMore.
- Value arity of Scalar Named options are ExactyOne.
- Value arity of Sequential Named options are ZeroOrMore.
- Value arity of Switch options are ZeroOrOne.
- Value arity of Counter options are Zero.
- Value arity of Scalar Value options are ZeroOrOne.
- Value arity of Sequential Value options are ZeroOrMore.

Also, option and value arities are manually configurable as shown in the following example.

```cs
var parser = CommandLine.CreateParser<MyOptions>()
    .AddNamedOption(s => s.Value, 
        o => o.WithOptionArity(ArityType.ZeroOrOne)
            .WithValueArity(ArityType.ExactlyOne))
    .AddNamedOption(s => s.Values, 
        o => o.WithOptionArity(ArityType.ZeroOrMore)
            .WithValueArity(ArityType.OneOrMore))
    .Build();
```

## 6. Custom Value Validator & Parser

### 6.1. Custom Option Value Validator

To provide custom validation code for parsed option values, call *.AddPredicate(...)* method to configure a custom predicate delegate.
Here is a simple code example, to invalidate negative numbers.

```cs
class MyOptions
{
    public double? Number { get; set; }
}

static void Main(params string[] args)
{
    var parser = CommandLine.CreateParser<MyOptions>()
        .AddNamedOption(p => p.Number, o => o.AddPredicate(v => v >= 0))
        .Build();

    var options = parser.Parse(args);

    if (parser.IsValid)
    {
        Console.WriteLine("Result: " + options.Number);
    }
}
```

Now, parser invalidates negative numbers, see the following command-line output for the preceding example:

```console
./> myapp -n -5
Option value(s) validation failed. Value(s): -5, Option: Number
```

### 6.2. Custom Option Value Parser

If you want to parse the value input of an option, use *.WithTryParseCallback(...)* method to configure a custom parser delegate.
Here are some examples of what you can do with *.WithTryParseCallback(...)*
- Parsing of custom types, classes, structs.
- Parsing of other kinds of input strings (for example, parse "1-2-3-4" into int[]).
- Parsing of postcodes, telephone number, emails etc.

Here is a simple code example, to parse the number inside parentheses:

```cs
class MyOptions
{
    public double? Number { get; set; }
}

static bool TryParseCustom(string value, out double result)
{
    if (value.StartsWith('(') && value.EndsWith(')'))
        value = value.Substring(1, value.Length - 2);

    return double.TryParse(value, out result);
}

static void Main(params string[] args)
{
    var parser = CommandLine.CreateParser<MyOptions>()
        .AddNamedOption(p => p.Number,
            o => o.AddPredicate(v => v >= 0).WithTryParseCallback(TryParseCustom))
        .Build();

    var options = parser.Parse(args);

    if (parser.IsValid)
    {
        Console.WriteLine("Result: " + options.Number);
    }
}
```

Now, parser can parse a number inside parentheses, see the following command-line output for the preceding example:

```console
./> myapp -n (2.3)
Result: 2.3
```

## 7. Other Option Configurations

You can manually configure following properties of an option.

- **Name:** *.WithName(string name)*\
  Configures name of the options.
  
- **Description:** *.WithDescription(string description)*\
  Sets description of the options to show a custom description in the help output.
  
- **Usage:** *.WithUsage(string usage)*\
  Sets usage string of the options to show a custom usage string in the help output.
  
- **IsHidden:** *.WithHidden(bool hidden)*\
  Do not prints the option in the help output, if it is a hidden option.
  
- **Aliases:** *.AddAliases(params string[] aliases)*\
  Sets aliases of named options. By default parser heuristically creates aliases of named options. Or it is manually configurable by this *AddAliases* method. AddAliases can be used multiple times for an option.
  
- **ValueArity:** *.WithValueArity(ArityType valueArity)*\
  Sets value arity of options. By default parser configures value arity of options. Or it is manually configurable by this *WithValueArity* method. See [Arity Configuration](#5-arity-configuration)
  
- **OptionArity:** *.WithOptionArity(ArityType optionArity)*\
  Sets option arity of named options. By default parser configures option arity of options. Or it is manually configurable by this *WithOptionArity* method. See [Arity Configuration](#5-arity-configuration)
  
- **DefaultValue:** *.WithDefaultValue<TValue>(TValue defaultValue)*\
  Sets default value of scalar named options. See [Default Value](#2-default-value)
  
- **EnableSequentialValues:** *.WithEnableSequentialValues(bool enabled)*\
  Sets EnableSequentialValues property of sequential named options. It is true by default.\
  If it is set to true, 'one two three' inputs will be parsed as 3 string values: "myapp --items one two three"\
  If it is set to false, parser grabs only first input "one" as a value.
  
- **EnableValueTokenSplitting:** *.WithEnableValueTokenSplitting(bool enabled)*\
  Sets EnableValueTokenSplitting property of sequential named and value options. It is true by default.\
  If it is set to true, 'one;two;three' inputs will be parsed as 3 string values: "myapp --items one;two;three"\
  If it is set to false, inputs will be parsed as 1 string "one;two;three" value.
  
- **AllowedValues:** *.WithAllowedValues<TValue>(params TValue[] allowedValues)*\
  Sets allowed values of options. See [Allowed Option Values](#3-allowed-option-values)
  
- **Predicates or Validators:** *.AddPredicate<TValue>(Predicate<TValue> predicate)*\
  Adds a value predicate delegate for option values. AddPredicate can be used multiple times for an option. See [Option Value Validator](#61-custom-option-value-validator)
  
- **List Predicates or List Validators:** *.AddListPredicate<TValue>(Predicate<List<TValue>> predicate)*\
  Adds a value list predicate delegate for sequential option values. AddListPredicate can be used multiple times for an option. See [Option Value Validator](#61-custom-option-value-validator)
  
- **Value Parser Callback:** *.WithTryParseCallback<TValue>(TryParse<TValue> callback)*\
  Sets a parser callback delegate for options to parse inputs manually. See [Option Value Parser](#62-custom-option-value-parser)
  
## 8. Built-In Options

There are 3 built-in options: --help, --version and --verbosity. See the following example to demonstrate built-in options.

```cs
enum OperatorType { Add, Sub, Mul, Div }

class CalculatorOptions
{
    public IEnumerable<double>? Numbers { get; set; }
    public OperatorType? Calculate { get; set; }
}

static void EvaluateOptions(CalculatorOptions options)
{
    var numbers = options.Numbers!.ToList();

    var equation = options.Calculate switch
    {
        OperatorType.Add => $"{string.Join(" + ", numbers)} = {numbers.Sum()}",
        OperatorType.Sub => $"{string.Join(" - ", numbers)} = {numbers.First() - numbers.Skip(1).Sum()}",
        OperatorType.Mul => $"{string.Join(" * ", numbers)} = {numbers.Aggregate<double, double>(1, (x, y) => x * y)}",
        OperatorType.Div => $"{string.Join(" / ", numbers)} = {numbers.Skip(1).Aggregate(numbers.First(), (x, y) => x / y)}",
        _ => "Invalid Argument!"
    };

    Console.WriteLine($"Result: {equation}");
}

static void Main(string[] args)
{
    var parser = CommandLine.CreateParser<CalculatorOptions>()
        .AddNamedOption(p => p.Numbers, o => o.AddPredicate(v => v > 0 ? true : throw new NotImplementedException()))
        .AddNamedOption(o => o.Calculate)
        .Build();

    parser.Run(EvaluateOptions, args);
}
```

### 8.1 Verbosity Level option

*Oaksoft.ArgumentParser* typically offer a --verbosity (or --vl) option that specifies how much output is sent to the console. Here are the standard five settings: 

Quiet, Minimal, Normal, Detailed, Trace

By default, verbosity level is Minimal. You can globally configure it by using *.ConfigureSettings()* method of parser builder. Or you can overwrite verbosity level by using as a command-line option.
Here are a some error outputs of preceding example with different verbosity levels.

```console
./> --abc 234 -c add -n 5 -n -3
Unknown double dash alias token '--abc' found!
The method or operation is not implemented. Option: Calculate
Unknown token '234' found!

./> --abc 234 -c add -n 5 -n -3 --verbosity:quiet
Unknown double dash alias token '--abc' found!

./> --abc 234 -c add -n 5 -n -3 --verbosity:minimal
Unknown double dash alias token '--abc' found!
The method or operation is not implemented. Option: Calculate
Unknown token '234' found!

./> --abc 234 -c add -n 5 -n -3 --verbosity:normal
01 - Unknown double dash alias token '--abc' found!
02 - The method or operation is not implemented. Option: Calculate
03 - Unknown token '234' found!

./> --abc 234 -c add -n 5 -n -3 --verbosity:detailed
###  Error(s)!  ###
01 - Code: ParserErrors.UnknownDoubleDashToken, Message: Unknown double dash alias token '--abc' found!
02 - Code: ParserErrors.UnexpectedError, Message: The method or operation is not implemented. Option: Calculate
03 - Code: ParserErrors.UnknownToken, Message: Unknown token '234' found!

./> --abc 234 -c add -n 5 -n -3 --verbosity:trace
###  Error(s)!  ###
01 - Code: ParserErrors.UnknownDoubleDashToken, Message: Unknown double dash alias token '--abc' found!
02 - Code: ParserErrors.UnexpectedError, Message: The method or operation is not implemented. Option: Calculate
System.NotImplementedException: The method or operation is not implemented.
   at Oaksoft.ArgumentParser.Tester.Program.<>c.<Main>b__3_3(OperatorType v) in ***\Program.cs:line 42
   at Oaksoft.ArgumentParser.Options.BaseAllowedValuesOption`1.<>c__DisplayClass8_0.<GetValidatedValues>b__3(Predicate`1 p) in ***\Oaksoft.ArgumentParser\Options\BaseValueOption.cs:line 268
   at System.Linq.Enumerable.Any[TSource](IEnumerable`1 source, Func`2 predicate)
   at Oaksoft.ArgumentParser.Options.BaseAllowedValuesOption`1.<GetValidatedValues>b__8_1(ValueTuple`2 n) in ***\Oaksoft.ArgumentParser\Options\BaseValueOption.cs:line 268
   at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.ToList()
   at Oaksoft.ArgumentParser.Options.BaseAllowedValuesOption`1.GetValidatedValues() in ***\Oaksoft.ArgumentParser\Options\BaseValueOption.cs:line 266
   at Oaksoft.ArgumentParser.Options.BaseScalarValueOption`1.Validate() in ***\Oaksoft.ArgumentParser\Options\BaseValueOption.cs:line 35
   at Oaksoft.ArgumentParser.Options.ScalarNamedOption`1.Validate() in ***\Oaksoft.ArgumentParser\Options\ScalarNamedOption.cs:line 156
   at Oaksoft.ArgumentParser.Parser.BaseArgumentParser.ValidateOptions(ICollection`1 tokens) in ***\Oaksoft.ArgumentParser\Parser\BaseArgumentParser.cs:line 269
03 - Code: ParserErrors.UnknownToken, Message: Unknown token '234' found!
```

As you can see in the preceding output;
- Level Quiet: Prints only first error message.
- Level Minimal: Prints all error messages. this is the default verbosity.
- Level Normal: Prints all error messages with error index.
- Level Detailed: Prints all error messages with error index, title and code.
- Level Trace: In addition to Detailed level error, prints exception call stack of validator in the example code. See the throwal of NotImplementedException.

### 8.2 Version Number option

*Oaksoft.ArgumentParser* typically offer a --version (or --vn) option that shows the version of the application. 
Version is read from AssemblyInformationalVersionAttribute. If you configure version of the application in the .csproj file with this setting "<Version>0.9.1</Version>".\
Here is the example version output.

```console
./> --version
0.9.1
```

### 8.3 Help option

Command-line apps typically provide an option to display a brief description of the available commands, options, and arguments. *Oaksoft.ArgumentParser* automatically generates help output.
Help output is configurable by Description metadata, ArgumentParser settings and option configuration (name, alias, description etc.). 
See the [here (source code)](https://github.com/orakist/Oaksoft.ArgumentParser/tree/dev/test/Oaksoft.ArgumentParser.Console) for the project settings.

Example help output:

```console
./> --help
Description:
  This console application calculates given numbers.

Usage:
  appname [options]

Options:
  -n       Usage: -n (value)
           Aliases: -n, --numbers, /n, /numbers
           Performs 'Numbers' option.

  -c       Usage: -c <value>
           Aliases: -c, --calculate, /c, /calculate
           Performs 'Calculate' option. [Allowed-Values: Add | Sub | Mul | Div]

  -h       Usage: -h
           Aliases: -h, -?, --help, /h, /?, /help
           Shows help and usage information.

  --vn     Usage: --vn
           Aliases: --vn, --version, /vn, /version
           Shows version-number of the application.
```

To print hidden options too, set verbosity level to detailed or trace. See the following help output with hidden options:

```console
./> --help --vl:detailed
Description:
  This console application calculates given numbers.

Usage:
  appname [options]

Options:
  -n       Usage: -n (value)
           Aliases: -n, --numbers, /n, /numbers
           Performs 'Numbers' option.

  -c       Usage: -c <value>
           Aliases: -c, --calculate, /c, /calculate
           Performs 'Calculate' option. [Allowed-Values: Add | Sub | Mul | Div]

  -h       Usage: -h
           Aliases: -h, -?, --help, /h, /?, /help
           Shows help and usage information.

  --vn     Usage: --vn
           Aliases: --vn, --version, /vn, /version
           Shows version-number of the application.

  --vl     Usage: --vl (value)
           Aliases: --vl, --verbosity, /vl, /verbosity
           Sets verbosity-level that specifies how much output is sent to the
           console. [Allowed-Values: Quiet | Minimal | Normal | Detailed | Trace],
           [Default: Minimal]
```
