using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Tests.TestModels;
using Shouldly;

namespace Oaksoft.ArgumentParser.Tests.NamedOptionTests;

public class BoolNamedOptionTests
{
    [Theory]
    [InlineData(false, true, "-vfalse", "-ntrue")]
    [InlineData(true, true, "--value:true", "--null-value:TRUE")]
    [InlineData(true, false, "-n:False", "-v:true")]
    [InlineData(true, true, "-v=true", "-n=true")]
    [InlineData(true, false, "--value:true", "--null-value=FALSE")]
    [InlineData(false, true, "/v=false", "/null-value:TRUE")]
    [InlineData(false, true, "/null-value", "TRUE", "/v", "FALSE")]
    public void ParseScalarOption_WhenArgumentsValid(bool val1, bool val2, params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<BoolAppOptions>()
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
    [InlineData(false, true, "-vfalse", "-ntrue")]
    [InlineData(true, true, "--value:true", "--null-value:TRUE")]
    [InlineData(true, false, "-n:False", "-v:true")]
    [InlineData(true, true, "-v=true", "-n=true")]
    [InlineData(true, false, "--value:true", "--null-value=FALSE")]
    [InlineData(false, true, "/v=false", "/null-value:TRUE")]
    [InlineData(false, true, "/null-value", "TRUE", "/v", "FALSE")]
    public void ParseScalarOption_WhenArgumentsValid_WithFlagProps(bool val1, bool val2, params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<BoolAppOptions>()
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
    [InlineData(false, true, "-v", "-n")]
    [InlineData(true, false, "-vTrue", "-nFalse")]
    [InlineData(false, true, "--value:false", "--null-value:TRUE")]
    [InlineData(false, true, "-n")]
    [InlineData(false, null, "-v")]
    [InlineData(false, null)]
    [InlineData(true, null, "/v=true")]
    [InlineData(false, true, "/null-value", "TRUE")]
    public void ParseScalarOption_WhenArgumentsValid_WithFlagPropsAndDefaultValue(bool val1, bool? val2, params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<BoolAppOptions>()
            .AddNamedOption(s => s.Value, o => o.WithDefaultValue(false), mustHaveOneValue: false)
            .AddNamedOption(s => s.NullValue, o => o.WithDefaultValue(true), mustHaveOneValue: false)
            .Build();

        // Act
        var result = sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeTrue();
        result.Value.ShouldBeEquivalentTo(val1);
        result.NullValue.ShouldBeEquivalentTo(val2);
    }

    [Theory]
    [InlineData(new[] { false, false }, new[] { true }, "-vfalse", "-vfalse", "-ntrue")]
    [InlineData(new[] { false, true }, new[] { true }, "-vfalse", "--values:true", "--null-values:TRUE")]
    [InlineData(new[] { false, true }, new[] { false, true }, "-vfalse", "-n:False", "-v:true", "-n:true")]
    [InlineData(new[] { false, true }, new[] { true, false }, "-vfalse", "-v=true", "-n=true;false")]
    [InlineData(new[] { true, false, true }, new[] { false, true }, "-vtrue;false", "--values:true", "--null-values=FALSE|true")]
    [InlineData(new[] { false, true, false }, new[] { true }, "-vfalse;true", "/v=false", "/null-values:TRUE")]
    [InlineData(new[] { false, true, true }, new[] { true, false }, "/null-values", "TRUE,false", "-vfalse", "/v", "true|true")]
    public void ParseSequentialOption_WhenArgumentsValid(bool[] val1, bool[] val2, params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<BoolAppOptions>()
            .AddNamedOption(s => s.Values)
            .AddNamedOption(s => s.NullValues)
            .Build();

        // Act
        var result = sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeTrue();
        result.Values.ShouldBe(val1);
        result.NullValues.ShouldBe(val2.Cast<bool?>());
    }
}