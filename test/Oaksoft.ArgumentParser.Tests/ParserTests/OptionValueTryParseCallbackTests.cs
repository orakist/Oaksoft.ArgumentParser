using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Errors.Builder;
using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Tests.TestModels;
using Shouldly;

namespace Oaksoft.ArgumentParser.Tests.ParserTests;

public class OptionValueTryParseCallbackTests : ArgumentParserTestBase
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
    public void ShouldParseNamedOption_WhenOptionValuesValid_WithTryParseFunction(params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value, o => o.WithTryParseCallback(int.TryParse))
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
    [InlineData("-v", "a")]
    [InlineData("-v:b")]
    [InlineData("-v=c")]
    [InlineData("-v11c")]
    [InlineData("--value", "1c2")]
    [InlineData("--value:c13")]
    [InlineData("--value=c-1")]
    [InlineData("/v", "c")]
    [InlineData("/v:c3")]
    [InlineData("/v=c4")]
    [InlineData("/value", "a5")]
    [InlineData("/value:x6")]
    [InlineData("/value=x7")]
    public void ShouldParseNamedOption_WhenOptionValuesInvalid_WithTryParseFunction(params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value, o => o.WithTryParseCallback(int.TryParse))
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
        sut.Errors[0].Error.Code.ShouldBe("ParserErrors.InvalidOptionValue");
        sut.Errors[0].Message.ShouldStartWith(string.Format(sut.Errors[0].Error.Format, option.ValueTokens[0]));
    }

    [Theory]
    [InlineData("-v", "4", "5")]
    [InlineData("-v:5,6,7,8")]
    [InlineData("-v=6,7,9,4")]
    [InlineData("-v7,6,7,7")]
    [InlineData("--values", "8;5;6")]
    [InlineData("--values:5|6|7")]
    [InlineData("--values=6")]
    [InlineData("/v", "9", "10")]
    [InlineData("/v:5,7,8")]
    [InlineData("/v=6,7,8")]
    [InlineData("/values", "10,7,8", "10,7,8")]
    [InlineData("/values:10,10,7,8")]
    [InlineData("/values=10;10,7,8;10,7,8")]
    public void ShouldParseNamedOption_WhenSequentialOptionValuesValid_WithTryParseFunction(params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Values, o => o.WithTryParseCallback(int.TryParse))
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
    [InlineData("-v", "a-4", "-5")]
    [InlineData("-v:b-5,a-6,-7,8")]
    [InlineData("-v=a-6,-7,9,4")]
    [InlineData("-va-7,a-6,7,7")]
    [InlineData("--values", "a-8;-5;6")]
    [InlineData("--values:a-5|6|7")]
    [InlineData("--values=a-6")]
    [InlineData("/v", "a-9", "10")]
    [InlineData("/v:a-5,7,8")]
    [InlineData("/v=a-6,7,8")]
    [InlineData("/values", "a-10,-7,8", "10,-7,8")]
    [InlineData("/values:a-10,a10,-7,8")]
    [InlineData("/values=a-10;-10,-7,8;10,7,8")]
    public void ShouldParseNamedOption_WhenSequentialOptionValuesInvalid_WithTryParseFunction(params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Values, o => o.WithTryParseCallback(int.TryParse))
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
        sut.Errors[0].Error.Code.ShouldBe("ParserErrors.InvalidOptionValue");
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
    public void ShouldParseValueOption_WhenOptionValuesValid_WithTryParseFunction(params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddValueOption(s => s.Value, o => o.WithTryParseCallback(int.TryParse))
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
    public void ShouldParseValueOption_WhenSequentialOptionValuesValid_WithTryParseFunction(params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddValueOption(s => s.Value, o => o.WithTryParseCallback(int.TryParse))
            .AddValueOption(s => s.Values, o => o.WithTryParseCallback(int.TryParse))
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

    [Fact]
    public void ShouldNotRegisterNamedOption_WithoutTryParsePointFunction()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>()
            .AddNamedOption(s => s.PointValue);

        // Act & Assert
        var exception = Should.Throw<OptionBuilderException>(() => sut.Build());

        exception.Error.Error.Code.ShouldBe("BuilderErrors.MissingCallback");
    }

    [Theory]
    [InlineData("-p", "(4;5)")]
    [InlineData("-p:(4;5)")]
    [InlineData("-p=(4;5)")]
    [InlineData("-p(4;5)")]
    [InlineData("--point-value", "(4;5)")]
    [InlineData("--point-value:(4;5)")]
    [InlineData("--point-value=(-4;5)")]
    [InlineData("/p", "(-4;-5)")]
    [InlineData("/p:(4;-5)")]
    [InlineData("/p=(4;5)")]
    [InlineData("/point-value", "(-4;5)")]
    [InlineData("/point-value:(-4;-5)")]
    [InlineData("/point-value=(4;5)")]
    public void ShouldParseNamedOption_WhenOptionValuesValid_WithTryParsePointFunction(params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>()
            .AddNamedOption(s => s.PointValue, o => o.WithTryParseCallback(PointTryParseCallback.TryParse))
            .Build();

        // Act
        sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeTrue();

        var option = sut.GetOptionByName(nameof(SampleOptionNames.PointValue)) as IScalarValueOption<Coordinate>;
        option.ShouldNotBeNull();
        option.ValueTokens.ShouldHaveSingleItem();
        option.InputValues.ShouldHaveSingleItem();
        option.ResultValue.ShouldNotBeNull();
    }

    [Theory]
    [InlineData("-p", "(4;5)|(1;2)")]
    [InlineData("-p:(4;5)|(1;2)")]
    [InlineData("-p=(4;5)|(1;2)")]
    [InlineData("-p(4;5)|(1;2)")]
    [InlineData("--point-values", "(4;5)|(1;2)")]
    [InlineData("--point-values:(4;5)|(1;2)")]
    [InlineData("--point-values=(4;5)|(1;2)")]
    [InlineData("/p", "(4;5)|(1;2)")]
    [InlineData("/p:(4;5)|(1;2)")]
    [InlineData("/p=(4;5)|(1;2)")]
    [InlineData("/point-values", "(4;5)|(1;2)")]
    [InlineData("/point-values:(4;5)|(1;2)")]
    [InlineData("/point-values=(4;5)|(1;2)")]
    public void ShouldParseNamedOption_WhenSequentialOptionValuesValid_WithTryParsePointFunction(params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>(valueDelimiter: ValueDelimiterRules.AllowPipeSymbol)
            .AddNamedOption(s => s.PointValues, o => o.WithTryParseCallback(PointTryParseCallback.TryParse))
            .Build();

        // Act
        sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeTrue();

        var option = sut.GetOptionByName(nameof(SampleOptionNames.PointValues)) as ISequentialValueOption<Coordinate>;
        option.ShouldNotBeNull();
        option.ValueTokens.ShouldNotBeEmpty();
        option.InputValues.ShouldNotBeEmpty();
        option.ResultValues.ShouldNotBeEmpty();
        option.ResultValues[0].ShouldBe(new Coordinate(4, 5));
        option.ResultValues[1].ShouldBe(new Coordinate(1, 2));
    }
}