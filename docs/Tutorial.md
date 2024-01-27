## How to Use Oaksoft.ArgumentParser

In this tutorial, you'll begin by creating a simple example. Then you'll add to that base, creating a more complex app that contains advanced scenarios.
Please see [here (source code)](https://github.com/orakist/Oaksoft.ArgumentParser/tree/dev/test/Oaksoft.ArgumentParser.Tutorial) for the source code of this tutorial.

1. Create a class to define your options.
2. Register and configure your options.
3. Build the parser.
4. Parse the inputs.

### Tutorial Step 1
Following example is a simple usage of *Oaksoft.ArgumentParser*.

```cs
class CalculatorOptions
{
    public int Left { get; set; }
    public int Right { get; set; }
    public string? Calculate { get; set; }
}

static IArgumentParser<CalculatorOptions> Build()
{
    return CommandLine.AutoBuild<CalculatorOptions>();
}

static void Parse(IArgumentParser<CalculatorOptions> parser, string[] args)
{
    Console.WriteLine($"/> {string.Join(' ', args)}");

    var options = parser.Parse(args);

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

static void Main(string[] args)
{
     Parse(Build(), args);
}
```

Parser supports almost any command-line inputs. It is case-insensitive by default. Following is the output of the example application for possible valid command-line inputs. 

```
Inputs: --left 5 --right 3 --calculate pow
Result: 5 ^ 3 = 125
Inputs: --Left=5 --Right=3 --Calculate=ADD
Result: 5 + 3 = 8
Inputs: --LEFT:5 --RIGHT:3 --CALCULATE:SUB
Result: 5 - 3 = 2
Inputs: /left=5 /right=3 /calculate=Add
Result: 5 + 3 = 8
Inputs: /left:5 /right:3 /calculate:Sub
Result: 5 - 3 = 2
Inputs: -L=5 -R=3 -C=Add
Result: 5 + 3 = 8
Inputs: -l:5 -r:3 -c:div
Result: 5 / 3 = 1
Inputs: -l=5 -r=3 -c=mul
Result: 5 * 3 = 15
Inputs: /L 5 /R 3 /c mUl
Result: 5 * 3 = 15
Inputs: /L=5 /R=3 /c=mUl
Result: 5 * 3 = 15
Inputs: /L:5 /R:3 /c:mUl
Result: 5 * 3 = 15
Inputs: -L5 -R3 -cMUl
Result: 5 * 3 = 15
Inputs: -l 35 -r 21 -c mul
Result: 35 * 21 = 735
```

Following is the output of the example application for invalid command-line inputs. 

```
Inputs: /left 5.1 /right 3.1 /calculate add
Invalid option value '5.1' found!, Option: Left
Invalid option value '3.1' found!, Option: Right
Result: Invalid argument!

Inputs: -l 5 -r 3 -c
At least '1' value(s) expected but '0' value(s) provided. Option: Calculate
Result: Invalid argument!
```

Following is the default help option output of the parser for this configuration.

```
Inputs: --help
Oaksoft.ArgumentParser.Tutorial v1.0.0
These are command line options of this application.

-l       Usage: -l <value>
         Aliases: -l, --left, /l, /left
         Performs 'Left' option.

-r       Usage: -r <value>
         Aliases: -r, --right, /r, /right
         Performs 'Right' option.

-c       Usage: -c <value>
         Aliases: -c, --calculate, /c, /calculate
         Performs 'Calculate' option.

-h       Usage: -h
         Aliases: -h, -?, --help, /h, /?, /help
         Shows help and usage information.

--vn     Usage: --vn
         Aliases: --vn, --version, /vn, /version
         Shows version-number of the application.

Usage: [-l <value>] [-r <value>] [-c <value>]
Result: Invalid argument!
```

Following is the default version option output of the parser for this configuration.

```
Inputs: --version
1.0.0
Result: Invalid argument!
```

### Tutorial Step 2
In the previous example, it prints "Result: Invalid argument!" output unnecessarily. To prevent this we can simply identify error cases, empty arguments cases and build-in option cases with following code. See the updated sample code below.

```cs
public static void Parse(IArgumentParser<CalculatorOptions> parser, string[] args)
{
    Console.WriteLine($"Inputs: {string.Join(' ', args)}");

    var options = parser.Parse(args);

    // Add these two lines
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
```

Following is the output of the code above for those command-line inputs. 

```
Inputs: /left 5.1 /right 3.1 /calculate add
Invalid option value '5.1' found!, Option: Left
Invalid option value '3.1' found!, Option: Right

Inputs: -l 5 -r 3 -c
At least '1' value(s) expected but '0' value(s) provided. Option: Calculate

Inputs: --version
1.0.0
```

### Tutorial Step 3

In the previous examples, it can not calculate floating point numbers. To calculate floating point numbers simply change type of your options. 

```cs
class CalculatorOptions
{
    public double Left { get; set; }
    public double Right { get; set; }
    public string? Calculate { get; set; }
}
```

Now it can parse and calculate floating point numbers.

```
Inputs: -l 35.2 -r 1.2 -c div
Result: 35.2 / 1.2 = 29.333333333333336
Inputs: /left 5.1 /right 3.1 /calculate add
Result: 5.1 + 3.1 = 8.2
```

### Tutorial Step 4

We can customize option registration to satisfy following cases.
- Make mandatory all options.
- Customize help description of options.
- Allow only add, sub, mul, div and pow values for calculate option.
- Make add value default for calculate option.

Before updating the application, this is the parsing output for these cases.

```
>>> Missing Numbers Case: This should not be allowed.
Inputs: -c add
Result: 0 + 0 = 0

>>> Unknown Calculation Type Case: Parser should catch unknown calculation type.
Inputs: -l 5 -r 1 -c abc
Result: Invalid argument!

>>> Missing Operation Value Case: This should not cause an error. Default Calculation Type is add.
Inputs: -l 5 -r 3 -c
At least '1' value(s) expected but '0' value(s) provided. Option: Calculate
```

We can cover the all cases in the output above with simple changes. See the updated sample code below. To satisfy these case;
- Add custom description to options.
- Set allowed values of calculate option.
- Set default value of calculate option.
- Make all options mandatory.
- Make value of the calculate option optional.

```cs
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
```

Now it covers all the cases. Compare the next output with previous.

```
>>> Missing Numbers Case
Inputs: -c add
At least '1' option(s) expected but '0' option(s) provided. Option: Left
At least '1' option(s) expected but '0' option(s) provided. Option: Right

>>> Unknown Calculation Type Case
Inputs: -l 5 -r 1 -c abc
Option value 'abc' not recognized. Must be one of: [add, sub, mul, div, pow], Option: Operator

>>> Missing Operation Value Case
Inputs: -l 5 -r 3 -c
Result: 5 + 3 = 8
```

This is the new help output. See the customized descriptions, allowed values and default values.

```
Inputs: --help
Oaksoft.ArgumentParser.Tutorial v1.0.0
These are command line options of this application.

-l       Usage: -l <value>
         Aliases: -l, --left, /l, /left
         Left operand of the operation.

-r       Usage: -r <value>
         Aliases: -r, --right, /r, /right
         Right operand of the operation.

-c       Usage: -c (value)
         Aliases: -c, --calculate, /c, /calculate
         Defines operator type of the calculation. [Allowed-Values: add | sub |
         mul | div | pow], [Default: add]

-h       Usage: -h
         Aliases: -h, -?, --help, /h, /?, /help
         Shows help and usage information.

--vn     Usage: --vn
         Aliases: --vn, --version, /vn, /version
         Shows version-number of the application.

Usage: [-l <value>] [-r <value>] [-c (value)]
```

### Tutorial Step 5

We can customize option registration to satisfy following cases.
- Don't allow negative numbers.
- Parse the numbers in parentheses.
  
Before updating the application, this is the parsing output for these cases.

```
>>> Negative Number Arguments Case: Negative numbers should not allowed.
Inputs: -l -5 -r -1 -c add
Result: -5 + -1 = -6

>>> Parentheses Number Arguments Case : It should parse numbers in parentheses.
Inputs: -l (5.1) -r (2.4) -c add
Invalid option value '(5.1)' found!, Option: Left
Invalid option value '(2.4)' found!, Option: Right
```

We can cover the all cases in the output above with simple changes. See the updated sample code below. To satisfy these case;
- Add predicates to left and right options. You can add multiple predicates to an option.
- Set try parse callbacks of left and right options.

```cs
private static bool TryParseCustom(string value, out double result)
{
    if (value.StartsWith('(') && value.EndsWith(')'))
        value = value.Substring(1, value.Length - 2);

    return double.TryParse(value, out result);
}

public static IArgumentParser<CalculatorOptions> Build()
{
    return CommandLine.CreateParser<CalculatorOptions>()
        .AddNamedOption(p => p.Left,
            o => o.WithDescription("Left operand of the operation.")
                .AddPredicate(v => v >= 0)
                .WithTryParseCallback(TryParseCustom),
            mandatoryOption: true)
        .AddNamedOption(p => p.Right,
            o => o.WithDescription("Right operand of the operation.")
                .AddPredicate(v => v >= 0)
                .WithTryParseCallback(TryParseCustom),
            mandatoryOption: true)
        .AddNamedOption(o => o.Calculate,
            o => o.WithDescription("Defines operator type of the calculation.")
                .WithAllowedValues("add", "sub", "mul", "div", "pow")
                .WithDefaultValue("add"),
            mandatoryOption: true, mustHaveOneValue: false)
        .Build();
}
```

Now it covers new parsing and validation cases. Compare the next output with previous.

```
>>> Negative Number Arguments Case
Inputs: -l -5 -r -1 -c add
Option value(s) validation failed. Value(s): -5, Option: Left
Option value(s) validation failed. Value(s): -1, Option: Right

>>> Parentheses Number Arguments Case
Inputs: -l (5.1) -r (2.4) -c add
Result: 5.1 + 2.4 = 7.5
```

### Tutorial Step 6

Previous examples calculates only 2 numbers. But if we want to calculate more than 2 numbers. We need to apply these simple changes.
- Create a double type sequential option.
- Update parser builder.
- Update result calculation

```cs
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
                .AddPredicate(v => v >= 0) // Check for negative numbers.
                .WithTryParseCallback(TryParseCustom) // Parse numbers in parentheses.
                .WithOptionArity(ArityType.OneOrMore) // At least one option required.
                .WithValueArity(2, int.MaxValue)) // At least two numbers required.
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
    var result = options.Calculate?.ToUpperInvariant() switch
    {
        "ADD" => $"{string.Join(" + ", numbers)} = {numbers.Sum()}",
        "SUB" => $"{string.Join(" - ", numbers)} = {numbers.First() - numbers.Skip(1).Sum()}",
        "MUL" => $"{string.Join(" * ", numbers)} = {numbers.Aggregate<double, double>(1, (x, y) => x * y)}",
        "DIV" => $"{string.Join(" / ", numbers)} = {numbers.Skip(1).Aggregate(numbers.First(), (x, y) => x / y)}",
        "POW" => $"{string.Join(" ^ ", numbers)} = {numbers.Skip(1).Aggregate(numbers.First(), Math.Pow)}",
        _ => "Invalid Argument!"
    };

    Console.WriteLine($"Result: {result}");
}
```

We updated the parser to calculate more than two numbers. ArgumentParser can parse many types of sequential inputs. Some of the supported sequential value input cases are:
- Multiple options with values, e.g: -n 5 -n 3 -n 2 => means -n=5,3,2
- Options with different alias prefixes, e.g: --numbers 5 /numbers 7 -n 3 /n 6  => means -n=5,7,3,6
- Options with sequential values, e.g: -n 5,7 -n 3,6  => means -n=5,7,3,6
- Options with different value delimiters. Valid delimiters are (','), (';'), ('|'). e.g: -n 5;7;2|1 -n 3|6  => means -n=5,7,2,1,3,6
- Options with different alias delimiters. Valid delimiters are ('='), (':'), (' '). e.g: -n=5;7 -n:3|6  => means -n=5,7,3,6
- Option with omitted alias delimiter. Only single dash alias supports this. e.g: -n5;7;3;6  => means -n=5,7,3,6

Check out the output below from the above code for some example sequential inputs.

```
Inputs: --numbers 5 --numbers 3 --numbers 2 --calculate pow
Result: 5 ^ 3 ^ 2 = 15625
Inputs: --numbers 5 /numbers 7 -n 3 /n 6 --calculate add
Result: 5 + 7 + 3 + 6 = 21
Inputs: --numbers 5 7 3 6 --calculate mul
Result: 5 * 7 * 3 * 6 = 630
Inputs: -n 5;7 (3)|6 --calculate add
Result: 5 + 7 + 3 + 6 = 21
Inputs: -n 28;7;3|6 -n1,2,4 --calculate
Result: 28 + 7 + 3 + 6 + 1 + 2 + 4 = 51
Inputs: -n 28|7|3|6 -c
Result: 28 + 7 + 3 + 6 = 44
Inputs: /n=(28),(7),3,(6),1.5,1.2 -c mul
Result: 28 * 7 * 3 * 6 * 1.5 * 1.2 = 6350.4
Inputs: /n:(28)|(7)|(3)|6 -c sub
Result: 28 - 7 - 3 - 6 = 12
Inputs: -n4.1;7.5;3.2;6.7 -c mul
Result: 4.1 * 7.5 * 3.2 * 6.7 = 659.28
```

And this is the output for some exceptional cases. 

```
Inputs: -n 4 -c mul
At least '2' value(s) expected but '1' value(s) provided. Option: Numbers

Inputs: -n 4|-5|-2|6|8 -c mul
Option value validation failed. Value(s): -5, -2, Option: Numbers
```

### Tutorial Step 7

With the changes in the step 6 example application can calculate more than 2 numbers. But now it can not parse incoming inputs with "left" and "right" aliases.
To be able to parse "left" and "right" aliases we simply add the "left" and "right" aliases to the numbers option.

Before updating the application, this is the parsing output for these cases. As expected application can't parse the "left" and "right" aliases.

```
Inputs: -l 35.2 -r 1.2 -c div
Unknown single dash alias token '-l' found!
Unknown single dash alias token '-r' found!
At least '1' option(s) expected but '0' option(s) provided. Option: Numbers
Unknown token '35.2' found!
Unknown token '1.2' found!

Inputs: /left 5.1 /right 3.1 /calculate add
Unknown forward slash alias token '/left' found!
Unknown forward slash alias token '/right' found!
At least '1' option(s) expected but '0' option(s) provided. Option: Numbers
Unknown token '5.1' found!
Unknown token '3.1' found!
```

See the updated ***Build()*** method below. Only this line *'.AddAliases("n", "numbers", "l", "left", "r", "right")'* added.

```cs
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
                .WithAllowedValues("add", "sub", "mul", "div", "pow")
                .WithDefaultValue("add"),
            mandatoryOption: true, mustHaveOneValue: false)
        .Build();
}
```

Now it can parse "left" and "right" aliases. Compare the next output with previous.

```
Inputs: -l 35.2 -r 1.2 -c div
Result: 35.2 / 1.2 = 29.333333333333336
Inputs: /left 5.1 /right 3.1 /calculate add
Result: 5.1 + 3.1 = 8.2
```

This is the new --help output. See the aliases of the "numbers" option.

```
Oaksoft.ArgumentParser.Tutorial v1.0.0
These are command line options of this application.

-n       Usage: -n <value>
         Aliases: -n, -l, -r, --left, --right, --numbers, /n, /l, /r, /left, /right, /numbers
         Defines the numbers to be calculated.

-c       Usage: -c (value)
         Aliases: -c, --calculate, /c, /calculate
         Defines operator type of the calculation. [Allowed-Values: add | sub |
         mul | div | pow], [Default: add]

-h       Usage: -h
         Aliases: -h, -?, --help, /h, /?, /help
         Shows help and usage information.

--vn     Usage: --vn
         Aliases: --vn, --version, /vn, /version
         Shows version-number of the application.

Usage: [-n <value>] [-c (value)]
```

### Tutorial Step 8

To simplify calculate option registration;
- Define an enum for Operator types
- Change type of the calculate option to an enum.

Enum option has some benefits. 
- Parser configures allowed values automatically for enum options.
- Any enum value update, updates the parser enum options. Because enums are strongly typed. 
- Also, you can pass integer values to enum options.
- See the updated codes method below.

```cs
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

    if (!parser.IsValid || parser.IsEmpty || parser.IsHelpOption || parser.IsVersionOption)
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
```

This is the new --help output. See the "calculate" option.

```
Oaksoft.ArgumentParser.Tutorial v1.0.0
These are command line options of this application.

-n       Usage: -n <value>
         Aliases: -n, -l, -r, --left, --right, --numbers, /n, /l, /r, /left, /right, /numbers
         Defines the numbers to be calculated.

-c       Usage: -c (value)
         Aliases: -c, --calculate, /c, /calculate
         Defines operator type of the calculation. [Allowed-Values: Add | Sub |
         Mul | Div | Pow | Rem], [Default: Add]

-h       Usage: -h
         Aliases: -h, -?, --help, /h, /?, /help
         Shows help and usage information.

--vn     Usage: --vn
         Aliases: --vn, --version, /vn, /version
         Shows version-number of the application.

Usage: [-n <value>] [-c (value)]
```

#### to be continued ...
