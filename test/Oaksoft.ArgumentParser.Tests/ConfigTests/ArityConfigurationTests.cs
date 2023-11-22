using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Tests.AppModels;
using Oaksoft.ArgumentParser.Tests.Extensions;
using Shouldly;

namespace Oaksoft.ArgumentParser.Tests.ConfigTests;

public class ArityConfigurationTests
{
    [Theory]
    [InlineData(ArityType.Zero)]
    [InlineData(ArityType.ZeroOrOne)]
    [InlineData(ArityType.ExactlyOne)]
    [InlineData(ArityType.ZeroOrMore)]
    [InlineData(ArityType.OneOrMore)]
    public void ShouldBuild_WhenValidValueArity(ArityType type)
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value, o => o.WithValueArity(type))
            .AddNamedOption(s => s.Values, o => o.WithValueArity(type))
            .AddSwitchOption(s => s.ValueFlag, o => o.WithValueArity(type))
            .AddCountOption(s => s.ValueCount, o => o.WithValueArity(type))
            .AddValueOption(s => s.NullValues, o => o.WithValueArity(type))
            .AddValueOption(s => s.NullValue, o => o.WithValueArity(type));

        // Act
        var parser = sut.Build();

        // Assert
        foreach (var option in parser.GetOptions().Where(o => o.Name != "Help"))
        {
            option.ValueArity.ShouldBe(type.GetLimits());
        }
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    [InlineData(0, 1000)]
    [InlineData(1, 1000)]
    [InlineData(10, 3000)]
    [InlineData(20, 20)]
    public void ShouldBuild_WhenValidCustomValueArity(int min, int max)
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value, o => o.WithValueArity(min, max))
            .AddNamedOption(s => s.Values, o => o.WithValueArity(min, max))
            .AddSwitchOption(s => s.ValueFlag, o => o.WithValueArity(min, max))
            .AddCountOption(s => s.ValueCount, o => o.WithValueArity(min, max))
            .AddValueOption(s => s.NullValues, o => o.WithValueArity(min, max))
            .AddValueOption(s => s.NullValue, o => o.WithValueArity(min, max));

        // Act
        var parser = sut.Build();

        // Assert
        foreach (var option in parser.GetOptions().Where(o => o.Name != "Help"))
        {
            option.ValueArity.ShouldBe((min, max));
        }
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(2, 1)]
    [InlineData(5, 4)]
    [InlineData(1, 0)]
    [InlineData(10, -10)]
    [InlineData(-20, -10)]
    public void ShouldThrowException_WhenInvalidCustomValueArity(int min, int max)
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value, o => o.WithValueArity(min, max))
            .AddNamedOption(s => s.Values, o => o.WithValueArity(min, max))
            .AddSwitchOption(s => s.ValueFlag, o => o.WithValueArity(min, max))
            .AddCountOption(s => s.ValueCount, o => o.WithValueArity(min, max))
            .AddValueOption(s => s.NullValues, o => o.WithValueArity(min, max))
            .AddValueOption(s => s.NullValue, o => o.WithValueArity(min, max));

        // Act & Assert
        Should.Throw<Exception>(() => sut.Build())
            .Message.ShouldStartWith($"Invalid value {(min, max)} arity!");
    }

    [Theory]
    [InlineData(ArityType.Zero)]
    [InlineData(ArityType.ZeroOrOne)]
    [InlineData(ArityType.ExactlyOne)]
    [InlineData(ArityType.ZeroOrMore)]
    [InlineData(ArityType.OneOrMore)]
    public void ShouldBuild_WhenValidOptionArity(ArityType type)
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value, o => o.WithOptionArity(type))
            .AddNamedOption(s => s.Values, o => o.WithOptionArity(type))
            .AddSwitchOption(s => s.ValueFlag, o => o.WithOptionArity(type))
            .AddCountOption(s => s.ValueCount, o => o.WithOptionArity(type));

        // Act
        var parser = sut.Build();

        // Assert
        foreach (var option in parser.GetOptions().Where(o => o.Name != "Help"))
        {
            option.OptionArity.ShouldBe(type.GetLimits());
        }
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    [InlineData(0, 1000)]
    [InlineData(1, 1000)]
    [InlineData(10, 3000)]
    [InlineData(20, 20)]
    public void ShouldBuild_WhenValidCustomOptionArity(int min, int max)
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value, o => o.WithOptionArity(min, max))
            .AddNamedOption(s => s.Values, o => o.WithOptionArity(min, max))
            .AddSwitchOption(s => s.ValueFlag, o => o.WithOptionArity(min, max))
            .AddCountOption(s => s.ValueCount, o => o.WithOptionArity(min, max));

        // Act
        var parser = sut.Build();

        // Assert
        foreach (var option in parser.GetOptions().Where(o => o.Name != "Help"))
        {
            option.OptionArity.ShouldBe((min, max));
        }
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(2, 1)]
    [InlineData(5, 4)]
    [InlineData(1, 0)]
    [InlineData(10, -10)]
    [InlineData(-20, -10)]
    public void ShouldThrowException_WhenInvalidOptionValueArity(int min, int max)
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value, o => o.WithOptionArity(min, max))
            .AddNamedOption(s => s.Values, o => o.WithOptionArity(min, max))
            .AddSwitchOption(s => s.ValueFlag, o => o.WithOptionArity(min, max))
            .AddCountOption(s => s.ValueCount, o => o.WithOptionArity(min, max));

        // Act & Assert
        Should.Throw<Exception>(() => sut.Build())
            .Message.ShouldStartWith($"Invalid option {(min, max)} arity!");
    }

    [Theory]
    [InlineData("--v", "--n")]
    [InlineData("-value", "-null-value")]
    [InlineData("--v:true", "--n:true")]
    [InlineData("--v=true", "--n=true")]
    [InlineData("-value:true", "-null-value=true")]
    [InlineData("/vtrue", "/null-valuex")]
    public void Parse_BoolSwitchOption_WhenArgumentsInvalid(params string[] args)
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
    public void Parse_BoolSwitchOption_WhenOptionsMandatory(params string[] args)
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
        sut.Errors.Count.ShouldBe(1);
        sut.Errors[0].StartsWith("At least '1' option was expected").ShouldBeTrue();
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
    public void Parse_BoolSwitchOption_WhenMultipleOptionAllowed(int count, params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<BoolAppOptions>()
            .AddSwitchOption(s => s.Value, o => o.WithOptionArity(1, 3))
            .AddSwitchOption(s => s.NullValue, o => o.WithOptionArity(1, 3).WithValueArity(2, 3))
            .Build();

        // Act
        var result = sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeEquivalentTo(count < 1);
        sut.Errors.Count.ShouldBeEquivalentTo(count);
        result.NullValue.ShouldBeEquivalentTo(sut.IsValid ? false : null);
        result.Value.ShouldBe(sut.IsValid);
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
    public void Parse_BoolSwitchOption_WhenDefaultValueSet(bool val1, bool? val2, params string[] args)
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