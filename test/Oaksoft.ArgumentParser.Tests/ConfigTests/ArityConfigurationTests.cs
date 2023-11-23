using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Options;
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
            .AddValueOption(s => s.NullValues, o => o.WithValueArity(type))
            .AddValueOption(s => s.NullValue, o => o.WithValueArity(type));

        // Act
        var parser = sut.Build();

        // Assert
        foreach (var option in parser.GetOptions().Where(o => o.Name != "Help"))
        {
            option.ValueArity.ShouldBe(type.GetLimits());
            option.ValueCount.ShouldBe(0);
        }
    }

    [Theory]
    [InlineData(ArityType.Zero)]
    [InlineData(ArityType.ZeroOrOne)]
    [InlineData(ArityType.ExactlyOne)]
    [InlineData(ArityType.ZeroOrMore)]
    [InlineData(ArityType.OneOrMore)]
    public void ShouldBuild_WhenValidValueArityByParameter(ArityType type)
    {
        // Arrange
        var sut1 = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Values, valueArity: type)
            .AddNamedOption(s => s.NullValues, valueArity: type);
        var sut2 = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Values, s => s.ValueCount, valueArity: type)
            .AddNamedOption(s => s.NullValues, s => s.NullValueCount, valueArity: type);
        var sut3 = CommandLine.CreateParser<IntAppOptions>()
            .AddValueOption(s => s.Values, valueArity: type)
            .AddValueOption(s => s.NullValues, valueArity: type);
        var sut4 = CommandLine.CreateParser<IntAppOptions>()
            .AddValueOption(s => s.Values, s => s.ValueCount, valueArity: type)
            .AddValueOption(s => s.NullValues, s => s.NullValueCount, valueArity: type);

        var builders = new []{ sut1, sut2, sut3, sut4 };
        foreach (var sut in builders)
        {        
            // Act
            var parser = sut.Build();

            // Assert
            foreach (var option in parser.GetOptions().Where(o => o.Name != "Help"))
                option.ValueArity.ShouldBe(type.GetLimits());
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShouldBuild_WhenMustHaveOneValueIsSet(bool enabled)
    {
        // Arrange
        var sut1 = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value, mustHaveOneValue: enabled)
            .AddNamedOption(s => s.NullValue, mustHaveOneValue: enabled);
        var sut2 = CommandLine.CreateParser<IntAppOptions>()
            .AddValueOption(s => s.Value, mustHaveOneValue: enabled)
            .AddValueOption(s => s.NullValue, mustHaveOneValue: enabled);
        var sut3 = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value, s => s.ValueFlag, mustHaveOneValue: enabled)
            .AddNamedOption(s => s.NullValue, s => s.NullValueFlag, mustHaveOneValue: enabled);
        var sut4 = CommandLine.CreateParser<IntAppOptions>()
            .AddValueOption(s => s.Value, s => s.ValueFlag, mustHaveOneValue: enabled)
            .AddValueOption(s => s.NullValue, s => s.NullValueFlag, mustHaveOneValue: enabled);

        var builders = new[] { sut1, sut2, sut3, sut4 };
        foreach (var sut in builders)
        {
            // Act
            var parser = sut.Build();

            // Assert
            foreach (var option in parser.GetOptions().Where(o => o.Name != "Help"))
                option.ValueArity.ShouldBe(enabled ? (1, 1) : (0, 1));
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
            .AddCounterOption(s => s.ValueCount, o => o.WithOptionArity(type));

        // Act
        var parser = sut.Build();

        // Assert
        foreach (var option in parser.GetOptions().Where(o => o.Name != "Help"))
        {
            option.OptionArity.ShouldBe(type.GetLimits());
            option.OptionCount.ShouldBe(0);
        }
    }

    [Theory]
    [InlineData(ArityType.Zero)]
    [InlineData(ArityType.ZeroOrOne)]
    [InlineData(ArityType.ExactlyOne)]
    [InlineData(ArityType.ZeroOrMore)]
    [InlineData(ArityType.OneOrMore)]
    public void ShouldBuild_WhenValidOptionArityByParameter(ArityType type)
    {
        // Arrange
        var sut1 = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Values, optionArity: type)
            .AddNamedOption(s => s.NullValues, optionArity: type);
        var sut2 = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Values, s => s.ValueCount, optionArity: type)
            .AddNamedOption(s => s.NullValues, s => s.NullValueCount, optionArity: type);

        var builders = new[] { sut1, sut2 };
        foreach (var sut in builders)
        {
            // Act
            var parser = sut.Build();

            // Assert
            foreach (var option in parser.GetOptions().Where(o => o.Name != "Help"))
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
            .AddCounterOption(s => s.ValueCount, o => o.WithOptionArity(min, max));

        // Act
        var parser = sut.Build();

        // Assert
        foreach (var option in parser.GetOptions().Where(o => o.Name != "Help"))
        {
            option.OptionArity.ShouldBe((min, max));
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShouldBuild_WhenMandatoryOptionIsSet(bool enabled)
    {
        // Arrange
        var sut1 = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value, mandatoryOption: enabled)
            .AddNamedOption(s => s.NullValue, mandatoryOption: enabled);
        var sut2 = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value, s => s.ValueFlag, mandatoryOption: enabled)
            .AddNamedOption(s => s.NullValue, s => s.NullValueFlag, mandatoryOption: enabled);

        var builders = new[] { sut1, sut2 };
        foreach (var sut in builders)
        {
            // Act
            var parser = sut.Build();

            // Assert
            foreach (var option in parser.GetOptions().Where(o => o.Name != "Help"))
                option.OptionArity.ShouldBe(enabled ? (1, 1) : (0, 1));
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
            .AddCounterOption(s => s.ValueCount, o => o.WithOptionArity(min, max));

        // Act & Assert
        Should.Throw<Exception>(() => sut.Build())
            .Message.ShouldStartWith($"Invalid option {(min, max)} arity!");
    }

    [Fact]
    public void ShouldThrowException_WhenInvalidArityEnumeration()
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>();

        // Act & Assert
        Should.Throw<Exception>(() => sut.AddNamedOption(s => s.Value, o => o.WithOptionArity((ArityType)100)))
            .Message.ShouldStartWith("Invalid ArityType enum value!");
    }

    [Fact]
    public void ShouldThrowException_WhenTryToUpdateArityAfterBuild()
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value);

        // Act
        var parser = sut.Build();
        var option = parser.GetOptionByName(nameof(IntAppOptions.Value));
        var namedOption = option as IScalarNamedOption<int>;

        // Assert
        namedOption.ShouldNotBeNull();
        Should.Throw<Exception>(() => namedOption.WithValueArity(ArityType.ZeroOrOne))
            .Message.ShouldStartWith("An option cannot be modified after");
        Should.Throw<Exception>(() => namedOption.WithOptionArity(ArityType.ZeroOrOne))
            .Message.ShouldStartWith("An option cannot be modified after");
    }
}