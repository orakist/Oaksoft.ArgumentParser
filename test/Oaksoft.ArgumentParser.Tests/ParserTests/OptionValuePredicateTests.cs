using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Tests.TestModels;
using Shouldly;

namespace Oaksoft.ArgumentParser.Tests.ParserTests;

public class OptionValuePredicateTests : ArgumentParserTestBase
{
    [Theory]
    [InlineData("-v", "4")]
    [InlineData("-v:5")]
    [InlineData("-v=6")]
    [InlineData("-v7")]
    [InlineData("--value", "8")]
    [InlineData("--value:5")]
    [InlineData("--value=6")]
    [InlineData("/v", "9")]
    [InlineData("/v:5")]
    [InlineData("/v=6")]
    [InlineData("/value", "10")]
    [InlineData("/value:10")]
    [InlineData("/value=10")]
    public void ShouldParseNamedOption_WhenOptionValuesValid_WithPredicateFunction(params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value, o => o.AddPredicate(v => v is > 3 and < 11))
            .Build();

        // Act
        sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeTrue();

        var option = sut.GetOptionByName(nameof(IntAppOptions.Value)) as IScalarValueOption<int>;
        option.ShouldNotBeNull();
        option.ValueTokens.ShouldHaveSingleItem();
        option.InputValues.ShouldHaveSingleItem();
        option.ResultValue.ShouldNotBeNull();
        option.ResultValue.Value.ShouldBeGreaterThan(3);
        option.ResultValue.Value.ShouldBeLessThan(11);
    }

    [Theory]
    [InlineData("-v", "1")]
    [InlineData("-v:2")]
    [InlineData("-v=3")]
    [InlineData("-v11")]
    [InlineData("--value", "12")]
    [InlineData("--value:13")]
    [InlineData("--value=-1")]
    [InlineData("/v", "-2")]
    [InlineData("/v:-3")]
    [InlineData("/v=-4")]
    [InlineData("/value", "-5")]
    [InlineData("/value:-6")]
    [InlineData("/value=-7")]
    public void ShouldParseNamedOption_WhenOptionValuesInvalid_WithPredicateFunction(params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value, o => o.AddPredicate(v => v is > 3 and < 11))
            .Build();

        // Act
        sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeFalse();

        var option = sut.GetOptionByName(nameof(IntAppOptions.Value)) as IScalarValueOption<int>;
        option.ShouldNotBeNull();
        option.ValueTokens.ShouldHaveSingleItem();
        option.InputValues.ShouldHaveSingleItem();
        option.ResultValue.ShouldBeNull();

        sut.Errors.ShouldHaveSingleItem();
        sut.Errors[0].Error.Code.ShouldBe("ParserErrors.PredicateFailure");
        sut.Errors[0].Message.ShouldStartWith(string.Format(sut.Errors[0].Error.Format, option.ValueTokens[0]));
    }

    [Theory]
    [InlineData("-v", "4", "5")]
    [InlineData("-v:5,6,7,8")]
    [InlineData("-v=6,7,9,4")]
    [InlineData("-v7,6,7,7")]
    [InlineData("--values", "8;5;6")]
    [InlineData("--values:5|6|7")]
    [InlineData("--values=6,5")]
    [InlineData("/v", "9", "10")]
    [InlineData("/v:5,7,8")]
    [InlineData("/v=6,7,8")]
    [InlineData("/values", "10,7,8", "10,7,8")]
    [InlineData("/values:10,10,7,8")]
    [InlineData("/values=10;10,7,8;10,7,8")]
    public void ShouldParseNamedOption_WhenSequentialOptionValuesValid_WithPredicateFunction(params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Values, o => o.AddPredicate(v => v is > 3 and < 11)
                .AddListPredicate(l => l.Count > 1))
            .Build();

        // Act
        sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeTrue();

        var option = sut.GetOptionByName(nameof(IntAppOptions.Values)) as ISequentialNamedOption<int>;
        option.ShouldNotBeNull();
        option.ValueTokens.ShouldNotBeEmpty();
        option.InputValues.ShouldNotBeEmpty();
        option.ResultValues.ShouldNotBeEmpty();
        option.ResultValues.ShouldAllBe(v => v > 3 && v < 11);
    }

    [Theory]
    [InlineData("-v", "-4", "-5")]
    [InlineData("-v:-5,-6,-7,8")]
    [InlineData("-v=-6,-7,9,4")]
    [InlineData("-v-7,-6,7,7")]
    [InlineData("--values", "-8;-5;6")]
    [InlineData("--values:-5|6|7")]
    [InlineData("--values=-6")]
    [InlineData("/v", "9", "10")]
    [InlineData("/v:-5,7,8")]
    [InlineData("/v=-6,7,8")]
    [InlineData("/values", "-10,-7,8", "10,-7,8")]
    [InlineData("/values:-10,10,-7,8")]
    [InlineData("/values=-10;-10,-7,8;10,7,8")]
    public void ShouldParseNamedOption_WhenSequentialOptionValuesInvalid_WithPredicateFunction(params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Values,
                o => o.AddPredicate(v => v is > 3 and < 11)
                    .AddListPredicate(l => l.Count > 2))
            .Build();

        // Act
        sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeFalse();

        var option = sut.GetOptionByName(nameof(IntAppOptions.Values)) as ISequentialNamedOption<int>;
        option.ShouldNotBeNull();
        option.ValueTokens.ShouldNotBeEmpty();
        option.InputValues.ShouldNotBeEmpty();
        option.ResultValues.ShouldBeEmpty();

        sut.Errors.ShouldNotBeEmpty();
        sut.Errors[0].Error.Code.ShouldBeOneOf("ParserErrors.PredicateFailure", "ParserErrors.ListPredicateFailure");
        sut.Errors[0].Message.ShouldStartWith(string.Format(sut.Errors[0].Error.Format, option.InputValues[0]));
    }

    [Theory]
    [InlineData("4")]
    [InlineData("5")]
    [InlineData("6")]
    [InlineData("7")]
    [InlineData("8")]
    [InlineData("9")]
    [InlineData("10")]
    public void ShouldParseValueOption_WhenOptionValuesValid_WithPredicateFunction(params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddValueOption(s => s.Value, o => o.AddPredicate(v => v is > 3 and < 11))
            .Build();

        // Act
        sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeTrue();

        var option = sut.GetOptionByName(nameof(IntAppOptions.Value)) as IScalarValueOption<int>;
        option.ShouldNotBeNull();
        option.ValueTokens.ShouldHaveSingleItem();
        option.InputValues.ShouldHaveSingleItem();
        option.ResultValue.ShouldNotBeNull();
        option.ResultValue.Value.ShouldBeGreaterThan(3);
        option.ResultValue.Value.ShouldBeLessThan(11);
    }

    [Theory]
    [InlineData("4", "10,5,6,7")]
    [InlineData("5", "10,5,6,7", "10,5,6,7")]
    [InlineData("6", "10,5,6,7;8")]
    [InlineData("7", "10,5,6,7;8")]
    [InlineData("8", "10,5,6,7;8")]
    [InlineData("9", "10,5,6,7;8")]
    [InlineData("10", "10,5,6,7", "8", "10|5|6,7;8")]
    public void ShouldParseValueOption_WhenSequentialOptionValuesValid_WithPredicateFunction(params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddValueOption(s => s.Value,
                o => o.AddPredicate(v => v is > 3 and < 11))
            .AddValueOption(s => s.Values,
                o => o.AddPredicate(v => v is > 3 and < 11)
                .AddListPredicate(l => l.Count > 2))
            .Build();

        // Act
        sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeTrue();

        var option = sut.GetOptionByName(nameof(IntAppOptions.Value)) as IScalarValueOption<int>;
        option.ShouldNotBeNull();
        option.ValueTokens.ShouldHaveSingleItem();
        option.InputValues.ShouldHaveSingleItem();
        option.ResultValue.ShouldNotBeNull();
        option.ResultValue.Value.ShouldBeGreaterThan(3);
        option.ResultValue.Value.ShouldBeLessThan(11);

        var option2 = sut.GetOptionByName(nameof(IntAppOptions.Values)) as ISequentialValueOption<int>;
        option2.ShouldNotBeNull();
        option2.ValueTokens.ShouldNotBeEmpty();
        option2.InputValues.ShouldNotBeEmpty();
        option2.ResultValues.ShouldNotBeEmpty();
        option2.ResultValues.ShouldAllBe(v => v > 3 && v < 11);
    }
}