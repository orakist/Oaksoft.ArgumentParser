## How to Use Tutorial

1. Create a class to define your options.
2. Register and configure your options.
3. Build the parser.
4. Parse the inputs.

### Tutorial Step 1
Following example is the simplest Oaksoft.ArgumentParser configuration.

```cs
class CalculatorOptions
{
    public int Left { get; set; }
    public int Right { get; set; }
    public string? Calculate { get; set; }
}

static IArgumentParser<CalculatorOptions> Build()
{
    return CommandLine.CreateParser<CalculatorOptions>()
        .AddNamedOption(p => p.Left)
        .AddNamedOption(p => p.Right)
        .AddNamedOption(o => o.Calculate)
        .Build();
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
     Error(s)!
01 - Invalid option value '5.1' found!, Option: Left
02 - Invalid option value '3.1' found!, Option: Right
Result: Invalid argument!

Inputs: -l 5 -r 3 -c
     Error(s)!
01 - At least '1' value(s) expected but '0' value(s) provided. Option: Calculate
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

--ver    Usage: --ver
         Aliases: --ver, --version, /ver, /version
         Shows version information.

Usage: [-l <value>] [-r <value>] [-c <value>] [-h] [--ver]
Result: Invalid argument!
```

Following is the default version option output of the parser for this configuration.

```
Inputs: --version
1.0.0
Result: Invalid argument!
```

### Tutorial Step 2
In the previous example, it prints "Result: Invalid argument!" output unnecessarily. To prevent this we can simply identify error cases, empty arguments cases and build-in option cases with following code. 

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
     Error(s)!
01 - Invalid option value '5.1' found!, Option: Left
02 - Invalid option value '3.1' found!, Option: Right

Inputs: -l 5 -r 3 -c
     Error(s)!
01 - At least '1' value(s) expected but '0' value(s) provided. Option: Calculate

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
- Make mandotary all options.
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
     Error(s)!
01 - At least '1' value(s) expected but '0' value(s) provided. Option: Calculate
```

We can cover the all cases in the output above with simple changes. See the sample code below. To satisfy these case;
- Add custom description to options.
- Set allowed values of calculate option.
- Set default value of calculate option.
- Make all options mandotary.
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

As you can see it is very simple. Now it covers all the cases. Compare the output.

```
>>> Missing Numbers Case
Inputs: -c add
     Error(s)!
01 - At least '1' option(s) expected but '0' option(s) provided. Option: Left
02 - At least '1' option(s) expected but '0' option(s) provided. Option: Right
>>> Unknown Calculation Type Case
Inputs: -l 5 -r 1 -c abc
     Error(s)!
>>> Missing Operation Value Case
Inputs: -l 5 -r 3 -c
Result: 5 + 3 = 8
```

This is the new help output. You can see the customized descriptions, allowed values and default values.

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

-c       Usage: -c (value), Default Value: 'add'
         Allowed Values: add | sub | mul | div | pow
         Aliases: -c, --calculate, /c, /calculate
         Defines operator type of the calculation.

-h       Usage: -h
         Aliases: -h, -?, --help, /h, /?, /help
         Shows help and usage information.

--ver    Usage: --ver
         Aliases: --ver, --version, /ver, /version
         Shows version information.

Usage: [-l <value>] [-r <value>] [-c (value)] [-h] [--ver]
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
     Error(s)!
01 - Invalid option value '(5.1)' found!, Option: Left
02 - Invalid option value '(2.4)' found!, Option: Right
```

We can cover the all cases in the output above with simple changes. See the sample code below. To satisfy these case;
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

As you can see it is very simple. Now it covers new parsing and validation cases. Compare the output.

```
>>> Negative Number Arguments Case
Inputs: -l -5 -r -1 -c add
     Error(s)!
01 - Option value(s) validation failed. Value(s): -5, Option: Left
02 - Option value(s) validation failed. Value(s): -1, Option: Right
>>> Parentheses Number Arguments Case
Inputs: -l (5.1) -r (2.4) -c add
Result: 5.1 + 2.4 = 7.5
```
