using Oaksoft.ArgumentParser.Errors.Parser;
using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Tests.TestModels;
using Shouldly;

namespace Oaksoft.ArgumentParser.Tests.ParserTests;

public class CounterOptionTests
{
    [Theory]
    [InlineData(1, 1, "-v", "-n")]
    [InlineData(0, 2, "-n", "--null-value")]
    [InlineData(1, 1, "--value", "--null-value")]
    [InlineData(1, 1, "-n", "-v")]
    [InlineData(2, 2, "-v", "-v", "-n", "-n")]
    [InlineData(2, 3, "--value", "--value", "--null-value", "--null-value", "--null-value")]
    [InlineData(4, 5, "/v", "/v", "/n", "/v", "/n", "/v", "/n", "/n", "/n")]
    [InlineData(1, 6, "/n", "-n", "-n", "-n", "/v", "-n", "-n")]
    public void ParseCounterOption_WhenArgumentsValid(int val1, int val2, params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddCounterOption(s => s.Value)
            .AddCounterOption(s => s.NullValue)
            .Build();

        // Act
        var result = sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeTrue();
        result.Value.ShouldBeEquivalentTo(val1);
        result.NullValue.ShouldBeEquivalentTo(val2);
    }

    [Theory]
    [InlineData(1, "-v", "-v", "-v", "-n", "-n")]
    [InlineData(1, "-v", "-v", "-v", "--null-value")]
    [InlineData(2, "-v", "-v", "-v", "-n", "-n", "-n")]
    [InlineData(1, "-v", "-n", "-n", "/n")]
    [InlineData(1, "--value", "--value", "--value", "--null-value")]
    [InlineData(1, "/v", "/v", "/v", "/null-value")]
    public void ParseCounterOption_WhenTooManyOptionError(int errorCount, params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddCounterOption(s => s.Value, o => o.WithOptionArity(1, 2))
            .AddCounterOption(s => s.NullValue, o => o.WithOptionArity(1, 2))
            .Build();

        // Act
        var result = sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeEquivalentTo(errorCount < 1);
        sut.Errors.Count.ShouldBeEquivalentTo(errorCount);
        sut.Errors[0].Error.Code.ShouldBe(ParserErrors.TooManyOption.Code);

        result.Value.ShouldBe(0);
        result.NullValue.ShouldBeNull();
    }

    [Theory]
    [InlineData(2, "-v", "-v", "-v", "-n", "-n")]
    [InlineData(2, "-v", "-v", "-v", "--null-value")]
    [InlineData(2, "-v", "-v", "-v", "-n", "-n", "-n")]
    [InlineData(2, "-v", "-n", "-n", "/n")]
    [InlineData(2, "--value", "--value", "--value", "--null-value")]
    [InlineData(2, "/v", "/v", "/v", "/null-value")]
    public void ParseCounterOption_WhenVeryFewOptionError(int errorCount, params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddCounterOption(s => s.Value, o => o.WithOptionArity(4, 5))
            .AddCounterOption(s => s.NullValue, o => o.WithOptionArity(4, 5))
            .Build();

        // Act
        sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeFalse();
        sut.Errors.Count.ShouldBeEquivalentTo(errorCount);
        sut.Errors[0].Error.Code.ShouldBe(ParserErrors.VeryFewOption.Code);
    }


    [Theory]
    [InlineData(2, "-v:1", "-n", "2")]
    [InlineData(2, "-v=5", "--null-value", "1")]
    public void ParseCounterOption_WhenTooManyValueError(int errorCount, params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddCounterOption(s => s.Value)
            .AddCounterOption(s => s.NullValue)
            .Build();

        // Act
        sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeFalse();
        sut.Errors.Count.ShouldBeEquivalentTo(errorCount);
        sut.Errors[0].Error.Code.ShouldBe(ParserErrors.TooManyValue.Code);
    }
}