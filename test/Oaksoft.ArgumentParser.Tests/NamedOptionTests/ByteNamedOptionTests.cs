using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Tests.TestModels;
using Shouldly;

namespace Oaksoft.ArgumentParser.Tests.NamedOptionTests;

public class ByteNamedOptionTests
{
    [Theory]
    [InlineData(10, 200, "-v10", "-n200")]
    [InlineData(20, 255, "--value:20", "--null-value:255")]
    [InlineData(1, 0, "-n:0", "-v:1")]
    [InlineData(11, 22, "-v=11", "-n=22")]
    [InlineData(100, 150, "--value:100", "--null-value=150")]
    [InlineData(50, 250, "/v=50", "/null-value:250")]
    [InlineData(4, 2, "/null-value", "2", "/v", "4")]
    public void ParseScalarOption_WhenArgumentsValid(byte val1, byte val2, params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<ByteAppOptions>()
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
    [InlineData(10, 200, "-v10", "-n200")]
    [InlineData(20, 255, "--value:20", "--null-value:255")]
    [InlineData(1, 0, "-n:0", "-v:1")]
    [InlineData(11, 22, "-v=11", "-n=22")]
    [InlineData(100, 150, "--value:100", "--null-value=150")]
    [InlineData(50, 250, "/v=50", "/null-value:250")]
    [InlineData(4, 2, "/null-value", "2", "/v", "4")]
    public void ParseScalarOption_WhenArgumentsValid_WithFlagProps(byte val1, byte val2, params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<ByteAppOptions>()
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
    [InlineData(11, 55, "-v", "-n")]
    [InlineData(111, 222, "-v111", "-n222")]
    [InlineData(11, 55, "--value:11", "--null-value:55")]
    [InlineData(0, 55, "-n")]
    [InlineData(11, 0, "-v")]
    [InlineData(0, 0)]
    [InlineData(45, 0, "/v=45")]
    [InlineData(0, 12, "/null-value", "12")]
    public void ParseScalarOption_WhenArgumentsValid_WithFlagPropsAndDefaultValues(byte val1, byte val2, params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<ByteAppOptions>()
            .AddNamedOption(s => s.Value, s => s.ValueFlag, o => o.WithDefaultValue((byte)11), mustHaveOneValue: false)
            .AddNamedOption(s => s.NullValue, s => s.NullValueFlag, o => o.WithDefaultValue((byte)55), mustHaveOneValue: false)
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
    [InlineData(new byte[] { 1, 2 }, new byte[] { 3 }, "-v1", "-v2", "-n3")]
    [InlineData(new byte[] { 4, 5 }, new byte[] { 6 }, "-v4", "--values:5", "--null-values:6")]
    [InlineData(new byte[] { 11, 33 }, new byte[] { 22, 44 }, "-v11", "-n:22", "-v:33", "-n:44")]
    [InlineData(new byte[] { 12, 13 }, new byte[] { 14, 15 }, "-v12", "-v=13", "-n=14;15")]
    [InlineData(new byte[] { 1, 2, 3 }, new byte[] { 4, 5 }, "-v1;2", "--values:3", "--null-values=4|5")]
    [InlineData(new byte[] { 10, 12, 14 }, new byte[] { 16 }, "-v10;12", "/v=14", "/null-values:16")]
    [InlineData(new byte[] { 6, 8, 10 }, new byte[] { 2, 4 }, "/null-values", "2,4", "-v6", "/v", "8|10")]
    public void ParseSequentialOption_WhenArgumentsValid(byte[] val1, byte[] val2, params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<ByteAppOptions>()
            .AddNamedOption(s => s.Values)
            .AddNamedOption(s => s.NullValues)
            .Build();

        // Act
        var result = sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeTrue();
        result.Values.ShouldBe(val1);
        result.NullValues.ShouldBe(val2.Cast<byte?>());
    }

    [Theory]
    [InlineData(new byte[] { 1, 2 }, new byte[] { 3 }, "-v1", "-v2", "-n3")]
    [InlineData(new byte[] { 4, 5 }, new byte[] { 6 }, "-v4", "--values:5", "--null-values:6")]
    [InlineData(new byte[] { 11, 33 }, new byte[] { 22, 44 }, "-v11", "-n:22", "-v:33", "-n:44")]
    [InlineData(new byte[] { 12, 13 }, new byte[] { 14, 15 }, "-v12", "-v=13", "-n=14;15")]
    [InlineData(new byte[] { 1, 2, 3 }, new byte[] { 4, 5 }, "-v1;2", "--values:3", "--null-values=4|5")]
    [InlineData(new byte[] { 10, 12, 14 }, new byte[] { 16 }, "-v10;12", "/v=14", "/null-values:16")]
    [InlineData(new byte[] { 6, 8, 10 }, new byte[] { 2, 4 }, "/null-values", "2,4", "-v6", "/v", "8|10")]
    public void ParseSequentialOption_WhenArgumentsValid_WithCount(byte[] val1, byte[] val2, params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<ByteAppOptions>()
            .AddNamedOption(s => s.Values, s => s.ValueCount)
            .AddNamedOption(s => s.NullValues, s => s.NullValueCount)
            .Build();

        // Act
        var result = sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeTrue();
        result.Values.ShouldBe(val1);
        result.NullValues.ShouldBe(val2.Cast<byte?>());
        result.ValueCount.ShouldBeGreaterThan(0);
        result.NullValueCount.ShouldBeGreaterThan(0);
        result.ValueCount.ShouldBeLessThanOrEqualTo(val1.Length);
        result.NullValueCount.ShouldBeLessThanOrEqualTo(val2.Length);
    }
}