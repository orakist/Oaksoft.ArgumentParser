using Oaksoft.ArgumentParser.Builder;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Errors.Parser;
using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Tests.TestModels;
using Shouldly;

namespace Oaksoft.ArgumentParser.Tests.ParserTests;

public class ArgumentParsingTests
{
    [Theory]
    [InlineData("-v", "10")]
    [InlineData("--v", "10")]
    [InlineData("-value", "10")]
    [InlineData("--value", "10")]
    [InlineData("/v", "10")]
    [InlineData("/value", "10")]
    [InlineData("-c", "15")]
    [InlineData("--c", "15")]
    [InlineData("-value-count", "15")]
    [InlineData("--value-count", "15")]
    [InlineData("/c", "15")]
    [InlineData("/value-count", "10")]
    [InlineData("-v", "10", "-c", "15")]
    [InlineData("-c", "15", "/v", "10")]
    [InlineData("--value", "10", "--value-count", "15")]
    [InlineData("--value-count", "15", "/value", "10")]
    [InlineData("-v", "10", "--value-count", "15")]
    [InlineData("--value", "10", "-c", "15")]
    [InlineData("/v", "10", "/value-count", "15")]
    [InlineData("--value", "10", "/c", "15")]
    [InlineData("--v", "10", "--c", "15")]
    [InlineData("--c", "15", "--v", "10")]
    [InlineData("-value", "10", "-value-count", "15")]
    [InlineData("-value-count", "15", "-value", "10")]
    [InlineData("--v", "10", "-value-count", "15")]
    [InlineData("-value", "10", "--c", "15")]
    public void ScalarNamedOption_TestAnyOptionPrefixRules(params string[] args)
    {
        foreach (var prefixRule in TestExtensions.AllPrefixRules)
        {
            // Arrange
            var sut = CommandLine.CreateParser<IntAppOptions>(prefixRule, 0)
                .AddNamedOption(s => s.Value)
                .AddNamedOption(s => s.ValueCount)
                .Build();

            // Act
            sut.Parse(args);
            var isFirstValid = prefixRule.IsValidAlias(args[0]);
            var isSecondValid = args.Length < 3 || prefixRule.IsValidAlias(args[2]);

            // Assert
            sut.IsValid.ShouldBe(isFirstValid && isSecondValid);

            if (!sut.IsValid)
            {
                sut.Errors.ShouldNotBeEmpty();
                var count = (isFirstValid ? 0 : 2) + (isSecondValid ? 0 : 2);
                sut.Errors.Count.ShouldBe(count);
            }

            if (isFirstValid)
            {
                var option = sut.GetOptionByAlias(args[0]);
                option.ShouldNotBeNull();
                option.OptionTokens.ShouldHaveSingleItem();
                option.OptionTokens.ShouldContain(args[0]);
                var valueOption = option as IValueOption;
                valueOption.ShouldNotBeNull();
                valueOption.ValueTokens.ShouldHaveSingleItem();
                valueOption.ValueTokens.ShouldContain(args[1]);
            }

            if (args.Length < 3)
                return;

            if (isSecondValid)
            {
                var option = sut.GetOptionByAlias(args[2]);
                option.ShouldNotBeNull();
                option.OptionTokens.ShouldHaveSingleItem();
                option.OptionTokens.ShouldContain(args[2]);
                var valueOption = option as IValueOption;
                valueOption.ShouldNotBeNull();
                valueOption.ValueTokens.ShouldHaveSingleItem();
                valueOption.ValueTokens.ShouldContain(args[3]);
            }
        }
    }

    [Theory]
    [InlineData("--v", "10")]
    [InlineData("--c", "15")]
    [InlineData("-value-count", "15")]
    [InlineData("--v", "10", "--c", "15")]
    [InlineData("--c", "15", "--v", "10")]
    [InlineData("-value", "10", "-value-count", "15")]
    [InlineData("-value-count", "15", "-value", "10")]
    [InlineData("--v", "10", "-value-count", "15")]
    [InlineData("-value", "10", "--c", "15")]
    public void ScalarNamedOption_ShouldNotParse_InvalidArguments_WithDefaultRules(params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.ValueCount)
            .Build();

        // Act
        sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeFalse();
        sut.Errors.ShouldNotBeEmpty();

        var isDoubleDash = args[0].StartsWith("--");
        sut.Errors[0].Error.Code.ShouldBe(isDoubleDash ? ParserErrors.InvalidDoubleDashToken.Code : ParserErrors.InvalidSingleDashToken.Code);
        sut.Errors[0].Message.ShouldBe(string.Format(sut.Errors[0].Error.Format, args[0]));

        if (args.Length < 3)
            return;

        isDoubleDash = args[2].StartsWith("--");
        sut.Errors[1].Error.Code.ShouldBe(isDoubleDash ? ParserErrors.InvalidDoubleDashToken.Code : ParserErrors.InvalidSingleDashToken.Code);
        sut.Errors[1].Message.ShouldBe(string.Format(sut.Errors[1].Error.Format, args[2]));
    }

    [Theory]
    [InlineData("--v", "10")]
    [InlineData("-value", "10")]
    [InlineData("--c", "15")]
    [InlineData("-value-count", "15")]
    [InlineData("--v", "10", "--c", "15")]
    [InlineData("--c", "15", "--v", "10")]
    [InlineData("-value", "10", "-value-count", "15")]
    [InlineData("-value-count", "15", "-value", "10")]
    [InlineData("--v", "10", "-value-count", "15")]
    [InlineData("-value", "10", "--c", "15")]
    public void ShouldParseScalar_WhenAliasArgumentsValid_WithAllPrefixRules(params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>(OptionPrefixRules.All)
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.ValueCount)
            .Build();

        // Act
        sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeTrue();

        var option = sut.GetOptionByAlias(args[0]);
        option.ShouldNotBeNull();
        option.OptionTokens.ShouldHaveSingleItem();
        option.OptionTokens.ShouldContain(args[0]);
        var valueOption = option as IValueOption;
        valueOption.ShouldNotBeNull();
        valueOption.ValueTokens.ShouldHaveSingleItem();
        valueOption.ValueTokens.ShouldContain(args[1]);

        if (args.Length < 3)
            return;

        option = sut.GetOptionByAlias(args[2]);
        option.ShouldNotBeNull();
        option.OptionTokens.ShouldHaveSingleItem();
        option.OptionTokens.ShouldContain(args[2]);
        valueOption = option as IValueOption;
        valueOption.ShouldNotBeNull();
        valueOption.ValueTokens.ShouldHaveSingleItem();
        valueOption.ValueTokens.ShouldContain(args[3]);
    }

    [Theory]
    [InlineData("--v:10")]
    [InlineData("-value:10")]
    [InlineData("--c:15")]
    [InlineData("-value-count:15")]
    [InlineData("--v:10", "--c:15")]
    [InlineData("--c:15", "--v:10")]
    [InlineData("-value:10", "-value-count:15")]
    [InlineData("-value-count:15", "-value:10")]
    public void ShouldParseScalar_WhenAliasArgumentsValid_WithAllPrefixRulesAndColonDelimiter(params string[] args)
    {
        var sut = CommandLine.CreateParser<IntAppOptions>(
            OptionPrefixRules.All, AliasDelimiterRules.AllowColonSymbol);
        TestAliasDelimiter(sut, args, ':');
    }

    [Theory]
    [InlineData("-v:10")]
    [InlineData("--value:10")]
    [InlineData("/v:10")]
    [InlineData("/value:10")]
    [InlineData("-c:15")]
    [InlineData("--value-count:15")]
    [InlineData("/c:15")]
    [InlineData("/value-count:15")]
    [InlineData("-v:10", "-c:15")]
    [InlineData("-c:15", "/v:10")]
    [InlineData("--value:10", "--value-count:15")]
    [InlineData("--value-count:15", "/value:10")]
    [InlineData("-v:10", "--value-count:15")]
    [InlineData("--value:10", "-c:15")]
    [InlineData("/v:10", "/value-count:15")]
    [InlineData("--value:10", "/c:15")]
    public void ShouldParseScalar_WhenAliasArgumentsValid_WithColonDelimiter(params string[] args)
    {
        var sut = CommandLine.CreateParser<IntAppOptions>(
            aliasDelimiter: AliasDelimiterRules.AllowColonSymbol);
        TestAliasDelimiter(sut, args, ':');
    }

    [Theory]
    [InlineData("-v=10")]
    [InlineData("--value=10")]
    [InlineData("/v=10")]
    [InlineData("/value=10")]
    [InlineData("-c=15")]
    [InlineData("--value-count=15")]
    [InlineData("/c=15")]
    [InlineData("/value-count=15")]
    [InlineData("-v=10", "-c=15")]
    [InlineData("-c=15", "/v=10")]
    [InlineData("--value=10", "--value-count=15")]
    [InlineData("--value-count=15", "/value=10")]
    [InlineData("-v=10", "--value-count=15")]
    [InlineData("--value=10", "-c=15")]
    [InlineData("/v=10", "/value-count=15")]
    [InlineData("--value=10", "/c=15")]
    public void ShouldParseScalar_WhenAliasArgumentsValid_WithEqualsDelimiter(params string[] args)
    {
        var sut = CommandLine.CreateParser<IntAppOptions>(
            aliasDelimiter: AliasDelimiterRules.AllowEqualSymbol);
        TestAliasDelimiter(sut, args, '=');
    }

    [Theory]
    [InlineData("-v 10")]
    [InlineData("--value 10")]
    [InlineData("/v 10")]
    [InlineData("/value 10")]
    [InlineData("-c 15")]
    [InlineData("--value-count 15")]
    [InlineData("/c 15")]
    [InlineData("/value-count 15")]
    [InlineData("-v 10", "-c 15")]
    [InlineData("-c 15", "/v 10")]
    [InlineData("--value 10", "--value-count 15")]
    [InlineData("--value-count 15", "/value 10")]
    [InlineData("-v 10", "--value-count 15")]
    [InlineData("--value 10", "-c 15")]
    [InlineData("/v 10", "/value-count 15")]
    [InlineData("--value 10", "/c 15")]
    public void ShouldParseScalar_WhenAliasArgumentsValid_WithSpaceDelimiter(params string[] args)
    {
        var sut = CommandLine.CreateParser<IntAppOptions>(
            aliasDelimiter: AliasDelimiterRules.AllowWhitespace);
        TestAliasDelimiter(sut, args, ' ');
    }

    [Theory]
    [InlineData("-v:10")]
    [InlineData("--value=10")]
    [InlineData("/v:10")]
    [InlineData("/value:10")]
    [InlineData("-c15")]
    [InlineData("--value-count=15")]
    [InlineData("/c=15")]
    [InlineData("/value-count:15")]
    [InlineData("-v10", "-c:15")]
    [InlineData("-c:15", "/v=10")]
    [InlineData("--value:10", "--value-count=15")]
    [InlineData("--value-count=15", "/value:10")]
    [InlineData("-v10", "--value-count=15")]
    [InlineData("--value:10", "-c15")]
    [InlineData("/v=10", "/value-count:15")]
    [InlineData("--value=10", "/c:15")]
    public void ShouldParseScalar_WhenAliasArgumentsValid_WithMixedDelimiter(params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.ValueCount)
            .Build();

        // Act
        sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeTrue();

        var option = sut.GetOptionByAlias("-v");
        var valueOption = option as IValueOption;
        valueOption.ShouldNotBeNull();
        if (valueOption.IsParsed)
        {
            valueOption.ValueTokens.ShouldHaveSingleItem();
            valueOption.ValueTokens.ShouldContain("10");
            valueOption.InputValues.ShouldHaveSingleItem();
            valueOption.InputValues.ShouldContain("10");
        }

        option = sut.GetOptionByAlias("-c");
        valueOption = option as IValueOption;
        valueOption.ShouldNotBeNull();
        if (valueOption.IsParsed)
        {
            valueOption.ValueTokens.ShouldHaveSingleItem();
            valueOption.ValueTokens.ShouldContain("15");
            valueOption.InputValues.ShouldHaveSingleItem();
            valueOption.InputValues.ShouldContain("15");
        }
    }

    [Theory]
    [InlineData("-v10")]
    [InlineData("-c15")]
    [InlineData("-v10", "-c15")]
    [InlineData("-c15", "-v10")]
    public void ShouldParseScalar_WhenAliasArgumentsValid_WithOmittingDelimiter(params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>(
                aliasDelimiter: AliasDelimiterRules.AllowOmittingDelimiter)
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.ValueCount)
            .Build();

        // Act
        sut.Parse(args);

        // Assert
        foreach (var arg in args)
        {
            var option = sut.GetOptionByAlias(arg[..2]);
            option.ShouldNotBeNull();
            option.OptionTokens.ShouldHaveSingleItem();
            option.OptionTokens.ShouldContain(arg[..2]);
            var valueOption = option as IValueOption;
            valueOption.ShouldNotBeNull();
            valueOption.ValueTokens.ShouldHaveSingleItem();
            valueOption.ValueTokens.ShouldContain(arg[2..]);
        }
    }

    [Theory]
    [InlineData("-v")]
    [InlineData("--values")]
    [InlineData("-n")]
    [InlineData("--null-values")]
    [InlineData("-v", "10")]
    [InlineData("--values", "10")]
    [InlineData("-n", "10")]
    [InlineData("--null-values", "10")]
    [InlineData("-v", "10", "--values", "15", "/v", "/values", "20", "-v", "25")]
    [InlineData("-n", "10", "/null-values", "15", "/n", "--null-values", "20", "/n", "25")]
    [InlineData("-v", "--values", "10", "/values", "15", "20", "-n")]
    [InlineData("-v", "10", "--values", "15", "20", "/n", "-v", "25", "30", "35")]
    [InlineData("-v", "10", "-n", "10", "15", "/n", "--values", "15", "20", "/n", "20", "25", "-v", "25", "30", "35")]
    public void ShouldParseSequential_WhenAliasArgumentsValid_WithoutDelimiter(params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Values)
            .AddNamedOption(s => s.NullValues)
            .Build();

        // Act
        sut.Parse(args);
        var option1 = sut.GetOptionByAlias("-v");
        var option2 = sut.GetOptionByAlias("-n");
        var option1Count = option1?.OptionCount ?? 0;
        var option2Count = option2?.OptionCount ?? 0;
        var value1Count = option1?.ValueCount ?? 0;
        var value2Count = option2?.ValueCount ?? 0;

        // Assert
        sut.IsValid.ShouldBeTrue();
        args.Length.ShouldBe(option1Count + option2Count + value1Count + value2Count);

        if (value1Count > 0)
        {
            var valueOption = option1 as IValueOption;
            valueOption.ShouldNotBeNull();
            for (var i = 0; i < value1Count; i++)
            {
                valueOption.InputValues.ShouldContain((10 + i * 5).ToString());
            }
        }

        if (value2Count > 0)
        {
            var valueOption = option2 as IValueOption;
            valueOption.ShouldNotBeNull();
            for (var i = 0; i < value2Count; i++)
            {
                valueOption.InputValues.ShouldContain((10 + i * 5).ToString());
            }
        }
    }

    [Theory]
    [InlineData(3, 0, "-v", "10", "/v", "15,20")]
    [InlineData(4, 0, "--values:10", "-v15", "/v:20,25")]
    [InlineData(0, 4, "/n", "10", "-n", "-n15,20,25")]
    [InlineData(0, 5, "--null-values", "10", "-n=15,20,25,30")]
    [InlineData(5, 0, "-v", "10", "15", "--values=20", "/v", "/values:25,30", "-v")]
    [InlineData(0, 5, "-n:10,15", "/null-values", "/n", "--null-values", "20,25,30")]
    [InlineData(3, 4, "-v", "10,15,20", "-n", "10,15,20,25")]
    [InlineData(6, 2, "/v", "10,15,20", "/n:10,15", "-v25,30,35")]
    [InlineData(6, 4, "-n", "10,15", "/n", "--values", "10,15,20", "/n", "20,25", "-v", "25,30,35")]
    public void ShouldParseSequential_WhenAliasArgumentsValid_WithCommaDelimiter(int count1, int count2, params string[] args)
    {
        TestSequentialValues(count1, count2, args);
    }

    [Theory]
    [InlineData(3, 0, "-v", "10", "/v", "15;20")]
    [InlineData(4, 0, "--values:10", "-v15", "/v:20;25")]
    [InlineData(0, 4, "/n", "10", "-n", "-n15;20;25")]
    [InlineData(0, 5, "--null-values", "10", "-n=15;20;25;30")]
    [InlineData(5, 0, "-v", "10", "15", "--values=20", "/v", "/values:25;30", "-v")]
    [InlineData(0, 5, "-n:10;15", "/null-values", "/n", "--null-values", "20;25;30")]
    [InlineData(3, 4, "-v", "10;15;20", "-n", "10;15;20;25")]
    [InlineData(6, 2, "/v", "10;15;20", "/n:10;15", "-v25;30;35")]
    [InlineData(6, 4, "-n", "10;15", "/n", "--values", "10;15;20", "/n", "20;25", "-v", "25;30;35")]
    public void ShouldParseSequential_WhenAliasArgumentsValid_WithSemicolonDelimiter(int count1, int count2, params string[] args)
    {
        TestSequentialValues(count1, count2, args);
    }

    [Theory]
    [InlineData(3, 0, "-v", "10", "/v", "15|20")]
    [InlineData(4, 0, "--values:10", "-v15", "/v:20|25")]
    [InlineData(0, 4, "/n", "10", "-n", "-n15|20|25")]
    [InlineData(0, 5, "--null-values", "10", "-n=15|20|25|30")]
    [InlineData(5, 0, "-v", "10", "15", "--values=20", "/v", "/values:25|30", "-v")]
    [InlineData(0, 5, "-n:10|15", "/null-values", "/n", "--null-values", "20|25|30")]
    [InlineData(3, 4, "-v", "10|15|20", "-n", "10|15|20|25")]
    [InlineData(6, 2, "/v", "10|15|20", "/n:10|15", "-v25|30|35")]
    [InlineData(6, 4, "-n", "10|15", "/n", "--values", "10|15|20", "/n", "20|25", "-v", "25|30|35")]
    public void ShouldParseSequential_WhenAliasArgumentsValid_WithPipeDelimiter(int count1, int count2, params string[] args)
    {
        TestSequentialValues(count1, count2, args);
    }

    [Theory]
    [InlineData(10, 0, "-v", "10;15;20;25;30", "/v", "35|40|45|50|55")]
    [InlineData(10, 0, "--values=10;15;20;25;30|35|40|45|50", "-v55")]
    [InlineData(0, 8, "-n10|15|20;25,30,35;40", "/n", "45")]
    [InlineData(0, 5, "--null-values", "10", "-n=15|20;25;30")]
    [InlineData(5, 0, "-v", "10|15,20;25|30", "-v")]
    [InlineData(0, 5, "-n:10|15,20|25|30")]
    [InlineData(3, 4, "-v", "10|15|20", "-n", "10|15;20;25")]
    [InlineData(6, 2, "/v", "10|15|20", "/n:10,15", "-v25|30|35")]
    [InlineData(7, 9, "-v", "10|15;20,25|30;35,40", "/n", "--values", "-n", "10|15|20;25|30|35,40,45,50")]
    public void ShouldParseSequential_WhenAliasArgumentsValid_WithMixedDelimiter(int count1, int count2, params string[] args)
    {
        TestSequentialValues(count1, count2, args);
    }

    private static void TestAliasDelimiter(IArgumentParserBuilder<IntAppOptions> builder, string[] args, char delimiter)
    {
        // Arrange
        var sut = builder.AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.ValueCount)
            .Build();

        // Act
        sut.Parse(args);

        // Assert
        foreach (var arg in args)
        {
            var token = arg.Split(delimiter);
            var option = sut.GetOptionByAlias(token[0]);
            option.ShouldNotBeNull();
            option.OptionTokens.ShouldHaveSingleItem();
            option.OptionTokens.ShouldContain(token[0]);
            var valueOption = option as IValueOption;
            valueOption.ShouldNotBeNull();
            valueOption.ValueTokens.ShouldHaveSingleItem();
            valueOption.ValueTokens.ShouldContain(token[1]);
        }
    }

    private static void TestSequentialValues(int count1, int count2, string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Values)
            .AddNamedOption(s => s.NullValues)
            .Build();

        // Act
        sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeTrue();

        if (count1 > 0)
        {
            var valueOption = sut.GetOptionByAlias("-v") as IValueOption;
            valueOption.ShouldNotBeNull();
            valueOption.InputValues.ShouldNotBeEmpty();
            valueOption.InputValues.Count.ShouldBe(count1);
            for (var i = 0; i < count1; i++)
            {
                valueOption.InputValues.ShouldContain((10 + i * 5).ToString());
            }
        }

        if (count2 > 0)
        {
            var valueOption = sut.GetOptionByAlias("-n") as IValueOption;
            valueOption.ShouldNotBeNull();
            valueOption.InputValues.ShouldNotBeEmpty();
            valueOption.InputValues.Count.ShouldBe(count2);
            for (var i = 0; i < count2; i++)
            {
                valueOption.InputValues.ShouldContain((10 + i * 5).ToString());
            }
        }
    }
}