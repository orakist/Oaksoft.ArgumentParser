using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Tests.TestModels;
using Shouldly;

namespace Oaksoft.ArgumentParser.Tests.ParserTests;

public class SwitchOptionTests : ArgumentParserTestBase
{
    [Theory]
    [InlineData(true, true, "-v", "-n")]
    [InlineData(false, true, "-vfalse", "-ntrue")]
    [InlineData(true, true, "--value", "--null-value")]
    [InlineData(true, false, "-n:False", "-v:true")]
    [InlineData(true, true, "-v=true", "-n=true")]
    [InlineData(true, false, "--value:true", "--null-value=FALSE")]
    [InlineData(false, true, "/v=false", "/null-value:TRUE")]
    [InlineData(false, true, "/null-value", "TRUE", "/v", "FALSE")]
    public void ParseSwitchOption_WhenArgumentsValid(bool val1, bool val2, params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<BoolAppOptions>()
            .AddSwitchOption(s => s.Value)
            .AddSwitchOption(s => s.NullValue)
            .Build();

        // Act
        var result = sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeTrue();
        result.Value.ShouldBeEquivalentTo(val1);
        result.NullValue.ShouldBeEquivalentTo(val2);
    }

    [Theory]
    [InlineData("--v", "--n")]
    [InlineData("-value", "-null-value")]
    [InlineData("--v:true", "--n:true")]
    [InlineData("--v=true", "--n=true")]
    [InlineData("-value:true", "-null-value=true")]
    [InlineData("/vtrue", "/null-valuex")]
    public void ParseSwitchOption_WhenArgumentsInvalid(params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<BoolAppOptions>()
            .AddSwitchOption(s => s.Value)
            .AddSwitchOption(s => s.NullValue)
            .Build();

        // Act
        var result = sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeFalse();
        sut.Errors.Count.ShouldBe(2);
        result.NullValue.ShouldBe(null);
        result.Value.ShouldBe(false);
    }

    [Theory]
    [InlineData("-v")]
    [InlineData("--null-value")]
    [InlineData("-v:true")]
    [InlineData("-n=true")]
    [InlineData("--null-value=true")]
    [InlineData("/v:true")]
    public void ParseSwitchOption_WhenOptionsMandatory(params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<BoolAppOptions>()
            .AddSwitchOption(s => s.Value, mandatoryOption: true)
            .AddSwitchOption(s => s.NullValue, mandatoryOption: true)
            .Build();

        // Act
        var result = sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeFalse();
        sut.Errors.ShouldHaveSingleItem();
        sut.Errors[0].Error.Code.ShouldBe("ParserErrors.VeryFewOption");
        result.NullValue.ShouldBe(null);
        result.Value.ShouldBe(false);
    }

    [Theory]
    [InlineData(1, "-v", "-n", "-n")]
    [InlineData(2, "--null-value")]
    [InlineData(0, "-v:true", "-n:true", "-n:false")]
    [InlineData(1, "-v=true", "-n=true")]
    [InlineData(1, "--value:true", "--null-value=true")]
    [InlineData(1, "/v=true", "/null-value:true")]
    public void ParseSwitchOption_WhenMultipleOptionAllowed(int errorCount, params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<BoolAppOptions>()
            .AddSwitchOption(s => s.Value, o => o.WithOptionArity(1, 3))
            .AddSwitchOption(s => s.NullValue, o => o.WithOptionArity(1, 3).WithValueArity(2, 3))
            .Build();

        // Act
        var result = sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeEquivalentTo(errorCount < 1);
        sut.Errors.Count.ShouldBeEquivalentTo(errorCount);
        result.NullValue.ShouldBeEquivalentTo(sut.IsValid ? false : null);
        result.Value.ShouldBe(sut.IsValid);
    }

    [Theory]
    [InlineData(2, "-v", "-n", "-n")]
    [InlineData(2, "--null-value", "-v")]
    [InlineData(1, "-v:true", "-n:true", "-n:false")]
    [InlineData(1, "-v=true", "-n=true", "-ntrue")]
    [InlineData(2, "--value:true", "--null-value=true")]
    [InlineData(2, "/v=true", "/null-value:true")]
    public void ParseSwitchOption_WhenMultipleValueAllowed(int errorCount, params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<BoolAppOptions>()
            .AddSwitchOption(s => s.Value, o => o.WithOptionArity(1, 3).WithValueArity(2, 3))
            .AddSwitchOption(s => s.NullValue, o => o.WithOptionArity(1, 3).WithValueArity(2, 3))
            .Build();

        // Act
        sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeFalse();
        sut.Errors.Count.ShouldBeEquivalentTo(errorCount);
        sut.Errors[0].Error.Code.ShouldBe("ParserErrors.VeryFewValue");
    }

    [Theory]
    [InlineData(2, "-v", "-v", "-v", "-v", "-n", "-n")]
    [InlineData(1, "--null-value", "--null-value", "--null-value", "--null-value", "-v:false", "-v:true")]
    [InlineData(1, "-v:true", "-v:true", "-v:true", "-v:true", "-n:true", "-n:false")]
    [InlineData(1, "-v=true", "-v=true", "-v=true", "-v=true", "-n=true", "-ntrue")]
    [InlineData(2, "--value:true", "--value:true", "--value:true", "--value:true", "--null-value=true")]
    [InlineData(2, "/v=true", "-v=true", "-v=true", "-v=true", "/null-value:true")]
    public void ParseSwitchOption_WhenTooManyOptionError(int errorCount, params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<BoolAppOptions>()
            .AddSwitchOption(s => s.Value, o => o.WithOptionArity(1, 3).WithValueArity(2, 3))
            .AddSwitchOption(s => s.NullValue, o => o.WithOptionArity(1, 3).WithValueArity(2, 3))
            .Build();

        // Act
        sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeFalse();
        sut.Errors.Count.ShouldBeEquivalentTo(errorCount);
        sut.Errors[0].Error.Code.ShouldBe("ParserErrors.TooManyOption");
    }

    [Theory]
    [InlineData(2, "-v:true", "-v:true", "-v:true", "-v", "-n", "-n")]
    [InlineData(1, "--null-value:true", "--null-value:true", "--null-value:true", "--null-value", "-v:false", "-v:true")]
    [InlineData(1, "-v:true", "-v:true", "-v:true", "-v:true", "-n:true", "-n:false")]
    [InlineData(1, "-v=true", "-v=true", "-v=true", "-v=true", "-n=true", "-ntrue")]
    [InlineData(1, "--value:true", "--value:true", "--value:true", "--value:true", "--null-value=true")]
    [InlineData(1, "/v=true", "-v=true", "-v=true", "-v=true", "/null-value:true")]
    public void ParseSwitchOption_WhenTooManyValueError(int errorCount, params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<BoolAppOptions>()
            .AddSwitchOption(s => s.Value, o => o.WithOptionArity(1, 5).WithValueArity(1, 2))
            .AddSwitchOption(s => s.NullValue, o => o.WithOptionArity(1, 5).WithValueArity(1, 2))
            .Build();

        // Act
        sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeFalse();
        sut.Errors.Count.ShouldBeEquivalentTo(errorCount);
        sut.Errors[0].Error.Code.ShouldBe("ParserErrors.TooManyValue");
    }

    [Theory]
    [InlineData(true, false, "-v", "-n")]
    [InlineData(false, true, "-vfalse", "-ntrue")]
    [InlineData(true, null, "--value")]
    [InlineData(true, false, "-n:False", "-v:true")]
    [InlineData(true, true, "-v=true", "-n=true")]
    [InlineData(true, false, "--value:true", "--null-value")]
    [InlineData(false, true, "/v=false", "/null-value:TRUE")]
    [InlineData(false, true, "/null-value", "TRUE", "/v", "FALSE")]
    public void ParseSwitchOption_WhenDefaultValueSet(bool val1, bool? val2, params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<BoolAppOptions>()
            .AddSwitchOption(s => s.Value, o => o.WithDefaultValue(true))
            .AddSwitchOption(s => s.NullValue, o => o.WithDefaultValue(false))
            .Build();

        // Act
        var result = sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeTrue();
        result.NullValue.ShouldBeEquivalentTo(val2);
        result.Value.ShouldBe(val1);
    }
}