using Oaksoft.ArgumentParser.Tutorial;

RunTutorial01();
RunTutorial02();
RunTutorial03();
RunTutorial04();
RunTutorial05();
RunTutorial06();

static void RunTutorial01()
{
    Console.WriteLine("#######  Tutorial - 01  #######");
    var parser = Tutorial01.Build();

    foreach (var args in GetLongArguments())
        Tutorial01.Parse(parser, args);

    foreach (var args in GetShortArguments())
        Tutorial01.Parse(parser, args);

    Console.WriteLine(">>> Help Option Case");
    Tutorial01.Parse(parser, Arguments.HelpArgs);

    Console.WriteLine(">>> Version Option Case");
    Tutorial01.Parse(parser, Arguments.VersionArgs);

    foreach (var args in GetFloatArguments())
        Tutorial01.Parse(parser, args);

    Console.WriteLine(">>> Empty Arguments Case");
    Tutorial01.Parse(parser, Arguments.EmptyArgs);

    Console.WriteLine(">>> Unknown Arguments Case");
    Tutorial01.Parse(parser, Arguments.UnknownArgs);

    Console.WriteLine(">>> Missing Numbers Case");
    Tutorial01.Parse(parser, Arguments.MissingNumberArgs);

    Console.WriteLine(">>> Missing Number Values Case");
    Tutorial01.Parse(parser, Arguments.MissingNumberValueArgs);

    Console.WriteLine(">>> Unknown Operation Value Case");
    Tutorial01.Parse(parser, Arguments.UnknownTypeArgs);

    Console.WriteLine(">>> Missing Operation Value Case");
    Tutorial01.Parse(parser, Arguments.MissingTypeArgs);

    Console.WriteLine(">>> Division By Zero Case");
    try
    {
        Tutorial01.Parse(parser, Arguments.DivByZeroArgs);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}

static void RunTutorial02()
{
    Console.WriteLine("#######  Tutorial - 02  #######");
    var parser = Tutorial02.Build();

    foreach (var args in GetLongArguments())
        Tutorial02.Parse(parser, args);

    foreach (var args in GetShortArguments())
        Tutorial02.Parse(parser, args);

    Console.WriteLine(">>> Help Option Case");
    Tutorial02.Parse(parser, Arguments.HelpArgs);

    Console.WriteLine(">>> Version Option Case");
    Tutorial02.Parse(parser, Arguments.VersionArgs);

    foreach (var args in GetFloatArguments())
        Tutorial02.Parse(parser, args);

    Console.WriteLine(">>> Empty Arguments Case");
    Tutorial02.Parse(parser, Arguments.EmptyArgs);

    Console.WriteLine(">>> Unknown Arguments Case");
    Tutorial02.Parse(parser, Arguments.UnknownArgs);

    Console.WriteLine(">>> Missing Numbers Case");
    Tutorial02.Parse(parser, Arguments.MissingNumberArgs);

    Console.WriteLine(">>> Missing Number Values Case");
    Tutorial02.Parse(parser, Arguments.MissingNumberValueArgs);

    Console.WriteLine(">>> Unknown Operation Value Case");
    Tutorial02.Parse(parser, Arguments.UnknownTypeArgs);

    Console.WriteLine(">>> Missing Operation Value Case");
    Tutorial02.Parse(parser, Arguments.MissingTypeArgs);

    Console.WriteLine(">>> Division By Zero Case");
    try
    {
        Tutorial02.Parse(parser, Arguments.DivByZeroArgs);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}

static void RunTutorial03()
{
    Console.WriteLine("#######  Tutorial - 03  #######");
    var parser = Tutorial03.Build();

    foreach (var args in GetLongArguments())
        Tutorial03.Parse(parser, args);

    foreach (var args in GetShortArguments())
        Tutorial03.Parse(parser, args);

    Console.WriteLine(">>> Help Option Case");
    Tutorial03.Parse(parser, Arguments.HelpArgs);

    Console.WriteLine(">>> Version Option Case");
    Tutorial03.Parse(parser, Arguments.VersionArgs);

    foreach (var args in GetFloatArguments())
        Tutorial03.Parse(parser, args);

    Console.WriteLine(">>> Empty Arguments Case");
    Tutorial03.Parse(parser, Arguments.EmptyArgs);

    Console.WriteLine(">>> Unknown Arguments Case");
    Tutorial03.Parse(parser, Arguments.UnknownArgs);

    Console.WriteLine(">>> Missing Numbers Case");
    Tutorial03.Parse(parser, Arguments.MissingNumberArgs);

    Console.WriteLine(">>> Missing Number Values Case");
    Tutorial03.Parse(parser, Arguments.MissingNumberValueArgs);

    Console.WriteLine(">>> Unknown Operation Value Case");
    Tutorial03.Parse(parser, Arguments.UnknownTypeArgs);

    Console.WriteLine(">>> Missing Operation Value Case");
    Tutorial03.Parse(parser, Arguments.MissingTypeArgs);

    Console.WriteLine(">>> Division By Zero Case");
    try
    {
        Tutorial03.Parse(parser, Arguments.DivByZeroArgs);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}

static void RunTutorial04()
{
    Console.WriteLine("#######  Tutorial - 04  #######");
    var parser = Tutorial04.Build();

    foreach (var args in GetLongArguments())
        Tutorial04.Parse(parser, args);

    foreach (var args in GetShortArguments())
        Tutorial04.Parse(parser, args);

    Console.WriteLine(">>> Help Option Case");
    Tutorial04.Parse(parser, Arguments.HelpArgs);

    Console.WriteLine(">>> Version Option Case");
    Tutorial04.Parse(parser, Arguments.VersionArgs);

    foreach (var args in GetFloatArguments())
        Tutorial04.Parse(parser, args);

    Console.WriteLine(">>> Empty Arguments Case");
    Tutorial04.Parse(parser, Arguments.EmptyArgs);

    Console.WriteLine(">>> Unknown Arguments Case");
    Tutorial04.Parse(parser, Arguments.UnknownArgs);

    Console.WriteLine(">>> Missing Numbers Case");
    Tutorial04.Parse(parser, Arguments.MissingNumberArgs);

    Console.WriteLine(">>> Missing Number Values Case");
    Tutorial04.Parse(parser, Arguments.MissingNumberValueArgs);

    Console.WriteLine(">>> Unknown Operation Value Case");
    Tutorial04.Parse(parser, Arguments.UnknownTypeArgs);

    Console.WriteLine(">>> Missing Operation Value Case");
    Tutorial04.Parse(parser, Arguments.MissingTypeArgs);

    Console.WriteLine(">>> Negative Number Arguments Case");
    Tutorial04.Parse(parser, Arguments.NegativeNumberArgs);

    Console.WriteLine(">>> Parentheses Number Arguments Case");
    Tutorial04.Parse(parser, Arguments.ParenthesesNumberArgs);

    Console.WriteLine(">>> Division By Zero Case");
    try
    {
        Tutorial04.Parse(parser, Arguments.DivByZeroArgs);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}

static void RunTutorial05()
{
    Console.WriteLine("#######  Tutorial - 05  #######");
    var parser = Tutorial05.Build();

    foreach (var args in GetLongArguments())
        Tutorial05.Parse(parser, args);

    foreach (var args in GetShortArguments())
        Tutorial05.Parse(parser, args);

    Console.WriteLine(">>> Help Option Case");
    Tutorial05.Parse(parser, Arguments.HelpArgs);

    Console.WriteLine(">>> Version Option Case");
    Tutorial05.Parse(parser, Arguments.VersionArgs);

    foreach (var args in GetFloatArguments())
        Tutorial05.Parse(parser, args);

    Console.WriteLine(">>> Empty Arguments Case");
    Tutorial05.Parse(parser, Arguments.EmptyArgs);

    Console.WriteLine(">>> Unknown Arguments Case");
    Tutorial05.Parse(parser, Arguments.UnknownArgs);

    Console.WriteLine(">>> Missing Numbers Case");
    Tutorial05.Parse(parser, Arguments.MissingNumberArgs);

    Console.WriteLine(">>> Missing Number Values Case");
    Tutorial05.Parse(parser, Arguments.MissingNumberValueArgs);

    Console.WriteLine(">>> Unknown Operation Value Case");
    Tutorial05.Parse(parser, Arguments.UnknownTypeArgs);

    Console.WriteLine(">>> Missing Operation Value Case");
    Tutorial05.Parse(parser, Arguments.MissingTypeArgs);

    Console.WriteLine(">>> Negative Number Arguments Case");
    Tutorial05.Parse(parser, Arguments.NegativeNumberArgs);

    Console.WriteLine(">>> Parentheses Number Arguments Case");
    Tutorial05.Parse(parser, Arguments.ParenthesesNumberArgs);

    Console.WriteLine(">>> Division By Zero Case");
    try
    {
        Tutorial05.Parse(parser, Arguments.DivByZeroArgs);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}

static void RunTutorial06()
{
    Console.WriteLine("#######  Tutorial - 06  #######");
    var parser = Tutorial06.Build();

    foreach (var args in GetNumberArguments())
        Tutorial06.Parse(parser, args);

    Console.WriteLine(">>> Help Option Case");
    Tutorial06.Parse(parser, Arguments.HelpArgs);

    Console.WriteLine(">>> Version Option Case");
    Tutorial06.Parse(parser, Arguments.VersionArgs);

    foreach (var args in GetFloatArguments())
        Tutorial06.Parse(parser, args);
}


static void RunTutorial07()
{
    Console.WriteLine("#######  Tutorial - 06  #######");
    var parser = Tutorial06.Build();

    foreach (var args in GetNumberArguments())
        Tutorial06.Parse(parser, args);

    foreach (var args in GetLongArguments())
        Tutorial06.Parse(parser, args);

    foreach (var args in GetShortArguments())
        Tutorial06.Parse(parser, args);

    Console.WriteLine(">>> Help Option Case");
    Tutorial06.Parse(parser, Arguments.HelpArgs);

    Console.WriteLine(">>> Version Option Case");
    Tutorial06.Parse(parser, Arguments.VersionArgs);

    foreach (var args in GetFloatArguments())
        Tutorial06.Parse(parser, args);
}

static IEnumerable<string[]> GetLongArguments()
{
    yield return new[] { "--left", "5", "--right", "3", "--calculate", "pow" };
    yield return new[] { "--Left=5", "--Right=3", "--Calculate=ADD" };
    yield return new[] { "--Left 5", "--Right 3", "--Calculate=ADD" };
    yield return new[] { "--LEFT:5", "--RIGHT:3", "--CALCULATE:SUB" };
    yield return new[] { "/left", "5", "/right", "3", "/calculate", "add" };
    yield return new[] { "/left=5", "/right=3", "/calculate=Add" };
    yield return new[] { "/left:5", "/right:3", "/calculate:Sub" };
}

static IEnumerable<string[]> GetShortArguments()
{
    yield return new[] { "-l", "35", "-r", "1", "-c", "div" };
    yield return new[] { "-L=5", "-R=3", "-C=Add" };
    yield return new[] { "-l:5", "-r:3", "-c:div" };
    yield return new[] { "-l=5", "-r=3", "-c=mul" };
    yield return new[] { "/L", "5", "/R", "3", "/c", "mUl" };
    yield return new[] { "/L=5", "/R=3", "/c=mUl" };
    yield return new[] { "/L:5", "/R:3", "/c:mUl" };
    yield return new[] { "-L5", "-R3", "-cMUl" };
    yield return new[] { "-l", "35", "-r", "21", "-c", "mul" };
}

static IEnumerable<string[]> GetNumberArguments()
{
    yield return new[] { "--numbers", "5", "--numbers", "3", "--numbers", "2", "--calculate", "pow" };
    yield return new[] { "--numbers", "5", "/numbers", "7", "-n", "3", "/n", "6", "--calculate", "add" };
    yield return new[] { "--numbers", "5", "7", "3", "6", "--calculate", "mul" };
    yield return new[] { "-n", "5;7", "(3)|6", "--calculate", "add" };
    yield return new[] { "-n", "28;7;3|6", "-n1,2,4", "--calculate"};
    yield return new[] { "-n 28|7|3|6", "-c" };
    yield return new[] { "/n=(28),(7),3,(6),1.5,1.2", "-c", "mul" };
    yield return new[] { "/n:(28)|(7)|(3)|6", "-c", "sub" };
    yield return new[] { "-n4.1;7.5;3.2;6.7", "-c", "mul" };
    yield return new[] { "-n", "4", "-c", "mul" };
    yield return new[] { "-n", "4|-5|-2|6|8", "-c", "mul" };
}

static IEnumerable<string[]> GetFloatArguments()
{
    yield return new[] { "-l", "35.2", "-r", "1.2", "-c", "div" };
    yield return new[] { "/left", "5.1", "/right", "3.1", "/calculate", "add" };
}

internal static class Arguments
{
    public static string[] HelpArgs { get; } = { "--help" };
    public static string[] VersionArgs { get; } = { "--version" };
    public static string[] EmptyArgs { get; } = { "" };
    public static string[] UnknownArgs { get; } = { "-cat", "1" };
    public static string[] DivByZeroArgs { get; } = { "-l", "5", "-r", "0", "-c", "div" };
    public static string[] MissingNumberArgs { get; } = { "-c", "add" };
    public static string[] MissingTypeArgs { get; } = { "-l", "5", "-r", "3", "-c" };
    public static string[] MissingNumberValueArgs { get; } = { "-l", "-r", "-c", "add" };
    public static string[] UnknownTypeArgs { get; } = { "-l", "5", "-r", "1", "-c", "abc" };
    public static string[] NegativeNumberArgs { get; } = { "-l", "-5", "-r", "-1", "-c", "add" };
    public static string[] ParenthesesNumberArgs { get; } = { "-l", "(5.1)", "-r", "(2.4)", "-c", "add" };
}