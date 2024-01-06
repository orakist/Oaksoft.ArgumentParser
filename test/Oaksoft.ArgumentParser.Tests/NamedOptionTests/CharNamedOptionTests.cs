using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Tests.TestModels;
using Shouldly;

namespace Oaksoft.ArgumentParser.Tests.NamedOptionTests;

public class CharNamedOptionTests
{
    [Theory]
    [InlineData('a', 'X', "-va", "-nX")]
    [InlineData('c', '9', "--value:c", "--null-value:9")]
    [InlineData('1', '0', "-n:0", "-v:1")]
    [InlineData('x', 'y', "-v=x", "-n=y")]
    [InlineData(65, 66, "--value:A", "--null-value=B")]
    [InlineData('A', 'B', "/v=A", "/null-value:B")]
    [InlineData('4', '2', "/null-value", "2", "/v", "4")]
    public void ParseScalarOption_WhenArgumentsValid(char val1, char val2, params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<CharAppOptions>()
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.NullValue)
            .Build();

        // Act
        var result = sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeTrue();
        result.Value.ShouldBeEquivalentTo(val1);
        result.NullValue.ShouldBeEquivalentTo(val2);
    }

    [Theory]
    [InlineData('a', 'X', "-va", "-nX")]
    [InlineData('c', '9', "--value:c", "--null-value:9")]
    [InlineData('1', '0', "-n:0", "-v:1")]
    [InlineData('x', 'y', "-v=x", "-n=y")]
    [InlineData(65, 66, "--value:A", "--null-value=B")]
    [InlineData('A', 'B', "/v=A", "/null-value:B")]
    [InlineData('4', '2', "/null-value", "2", "/v", "4")]
    public void ParseScalarOption_WhenArgumentsValid_WithFlagProps(char val1, char val2, params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<CharAppOptions>()
            .AddNamedOption(s => s.Value, s => s.ValueFlag)
            .AddNamedOption(s => s.NullValue, s => s.NullValueFlag)
            .Build();

        // Act
        var result = sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeTrue();
        result.Value.ShouldBeEquivalentTo(val1);
        result.NullValue.ShouldBeEquivalentTo(val2);
        result.ValueFlag.ShouldBeTrue();
        result.NullValueFlag.ShouldBeTrue();
    }

    [Theory]
    [InlineData(64, 65, "-v", "-n")]
    [InlineData('a', 'b', "-va", "-nb")]
    [InlineData('1', '5', "--value:1", "--null-value:5")]
    [InlineData(0, 65, "-n")]
    [InlineData(64, 0, "-v")]
    [InlineData(0, 0)]
    [InlineData('4', 0, "/v=4")]
    [InlineData(0, '1', "/null-value", "1")]
    public void ParseScalarOption_WhenArgumentsValid_WithFlagPropsAndDefaultValues(char val1, char val2, params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<CharAppOptions>()
            .AddNamedOption(s => s.Value, s => s.ValueFlag, o => o.WithDefaultValue((char)64), mustHaveOneValue: false)
            .AddNamedOption(s => s.NullValue, s => s.NullValueFlag, o => o.WithDefaultValue((char)65), mustHaveOneValue: false)
            .Build();

        // Act
        var result = sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeTrue();
        result.Value.ShouldBeEquivalentTo(val1);
        result.NullValue.ShouldBeEquivalentTo(val2 > 0 ? val2 : null);
        result.ValueFlag.ShouldBe(val1 > 0);
        result.NullValueFlag.ShouldBe(val2 > 0);
    }

    [Theory]
    [InlineData(new[] { '1', '2' }, new[] { '3' }, "-v1", "-v2", "-n3")]
    [InlineData(new[] { '4', '5' }, new[] { '6' }, "-v4", "--values:5", "--null-values:6")]
    [InlineData(new[] { '1', '3' }, new[] { '2', '4' }, "-v1", "-n:2", "-v:3", "-n:4")]
    [InlineData(new[] { '1', '3' }, new[] { '4', '5' }, "-v1", "-v=3", "-n=4;5")]
    [InlineData(new[] { '1', '2', '3' }, new[] { '4', '5' }, "-v1;2", "--values:3", "--null-values=4|5")]
    [InlineData(new[] { '2', '4', '6' }, new[] { '8' }, "-v2;4", "/v=6", "/null-values:8")]
    [InlineData(new[] { '6', '8', '9' }, new[] { '2', '4' }, "/null-values", "2,4", "-v6", "/v", "8|9")]
    public void ParseSequentialOption_WhenArgumentsValid(char[] val1, char[] val2, params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<CharAppOptions>()
            .AddNamedOption(s => s.Values)
            .AddNamedOption(s => s.NullValues)
            .Build();

        // Act
        var result = sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeTrue();
        result.Values.ShouldBe(val1);
        result.NullValues.ShouldBe(val2.Cast<char?>());
    }

    [Theory]
    [InlineData(new[] { '1', '2' }, new[] { '3' }, "-v1", "-v2", "-n3")]
    [InlineData(new[] { '4', '5' }, new[] { '6' }, "-v4", "--values:5", "--null-values:6")]
    [InlineData(new[] { '1', '3' }, new[] { '2', '4' }, "-v1", "-n:2", "-v:3", "-n:4")]
    [InlineData(new[] { '1', '3' }, new[] { '4', '5' }, "-v1", "-v=3", "-n=4;5")]
    [InlineData(new[] { '1', '2', '3' }, new[] { '4', '5' }, "-v1;2", "--values:3", "--null-values=4|5")]
    [InlineData(new[] { '2', '4', '6' }, new[] { '8' }, "-v2;4", "/v=6", "/null-values:8")]
    [InlineData(new[] { '6', '8', '9' }, new[] { '2', '4' }, "/null-values", "2,4", "-v6", "/v", "8|9")]
    public void ParseSequentialOption_WhenArgumentsValid_WithCount(char[] val1, char[] val2, params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<CharAppOptions>()
            .AddNamedOption(s => s.Values, s => s.ValueCount)
            .AddNamedOption(s => s.NullValues, s => s.NullValueCount)
            .Build();

        // Act
        var result = sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeTrue();
        result.Values.ShouldBe(val1);
        result.NullValues.ShouldBe(val2.Cast<char?>());
        result.ValueCount.ShouldBeGreaterThan(0);
        result.NullValueCount.ShouldBeGreaterThan(0);
        result.ValueCount.ShouldBeLessThanOrEqualTo(val1.Length);
        result.NullValueCount.ShouldBeLessThanOrEqualTo(val2.Length);
    }
}