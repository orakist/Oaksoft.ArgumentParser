using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Tests.AppModels;
using Shouldly;

namespace Oaksoft.ArgumentParser.Tests.ConfigTests;

public class NameConfigurationTests
{
    [Fact]
    public void ShouldBuildOptions_WhenDifferentNamesUsed()
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value, o => o.WithName("Value"))
            .AddCountOption(s => s.NullValue, o => o.WithName("ValueCount"))
            .AddNamedOption(s => s.Values, o => o.WithName("ValueList"))
            .AddValueOption(s => s.NullValues, o => o.WithName("ValueX"))
            .AddValueOption(s => s.ValueFlag, o => o.WithName("ValueY"));

        // Act
        var result = sut.Build();

        // Assert
        result.GetOptions().Count.ShouldBe(6);

        var option = result.GetOptionByName(nameof(IntAppOptions.Value));
        option.ShouldNotBeNull();
        option.Name.ShouldBe("Value");

        option = result.GetOptionByName(nameof(IntAppOptions.NullValue));
        option.ShouldNotBeNull();
        option.Name.ShouldBe("ValueCount");

        option = result.GetOptionByName(nameof(IntAppOptions.Values));
        option.ShouldNotBeNull();
        option.Name.ShouldBe("ValueList");

        option = result.GetOptionByName(nameof(IntAppOptions.NullValues));
        option.ShouldNotBeNull();
        option.Name.ShouldBe("ValueX");

        option = result.GetOptionByName(nameof(IntAppOptions.ValueFlag));
        option.ShouldNotBeNull();
        option.Name.ShouldBe("ValueY");
    }

    [Fact]
    public void ShouldBuildOptions_WhenDefaultNamesUsed()
    {
        // Arrange, should ignore empty names
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.NullValue, s => s.NullValueFlag)
            .AddNamedOption(s => s.Values)
            .AddValueOption(s => s.NullValues, o => o.WithName(""))
            .AddValueOption(s => s.ValueFlag, o => o.WithName("   "));

        // Act
        var result = sut.Build();

        // Assert
        result.GetOptions().Count.ShouldBe(6);

        var option = result.GetOptionByName(nameof(IntAppOptions.Value));
        option.ShouldNotBeNull();
        option.Name.ShouldBe("Value");
        option = result.GetOptionByName(option.Name);
        option.ShouldNotBeNull();

        option = result.GetOptionByName(nameof(IntAppOptions.NullValue));
        option.ShouldNotBeNull();
        option.Name.ShouldBe("Null Value");
        option = result.GetOptionByName(option.Name);
        option.ShouldNotBeNull();
        option = result.GetOptionByName(nameof(IntAppOptions.NullValueFlag));
        option.ShouldNotBeNull();

        option = result.GetOptionByName(nameof(IntAppOptions.Values));
        option.ShouldNotBeNull();
        option.Name.ShouldBe("Values");
        option = result.GetOptionByName(option.Name);
        option.ShouldNotBeNull();

        option = result.GetOptionByName(nameof(IntAppOptions.NullValues));
        option.ShouldNotBeNull();
        option.Name.ShouldBe("Null Values");
        option = result.GetOptionByName(option.Name);
        option.ShouldNotBeNull();

        option = result.GetOptionByName(nameof(IntAppOptions.ValueFlag));
        option.ShouldNotBeNull();
        option.Name.ShouldBe("Value Flag");
        option = result.GetOptionByName(option.Name);
        option.ShouldNotBeNull();
    }

    [Fact]
    public void ShouldBuildOptions_WhenNumberAndLetterNamesUsed()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>()
            .AddNamedOption(s => s.__1_Help_1)
            .AddNamedOption(s => s.__2_Value)
            .AddNamedOption(s => s.ValueTest);

        // Act
        var result = sut.Build();

        // Assert
        result.GetOptions().Count.ShouldBe(4);

        var option = result.GetOptionByName(nameof(SampleOptionNames.__1_Help_1));
        option.ShouldNotBeNull();
        option.Name.ShouldBe("Help 1");
        option = result.GetOptionByName(option.Name);
        option.ShouldNotBeNull();

        option = result.GetOptionByName(nameof(SampleOptionNames.__2_Value));
        option.ShouldNotBeNull();
        option.Name.ShouldBe("Value");
        option = result.GetOptionByName(option.Name);
        option.ShouldNotBeNull();

        option = result.GetOptionByName(nameof(SampleOptionNames.ValueTest));
        option.ShouldNotBeNull();
        option.Name.ShouldBe("Value Test");
        option = result.GetOptionByName(option.Name);
        option.ShouldNotBeNull();
    }

    [Fact]
    public void ShouldThrowException_WhenSameNameUsed()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>()
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.__2_Value);

        // Act & Assert
        Should.Throw<Exception>(() => sut.Build())
            .Message.ShouldStartWith("Option 'Value' name must be unique.");
    }

    [Fact]
    public void ShouldThrowException_WhenSameCustomNameUsed()
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value, o => o.WithName("Test"))
            .AddNamedOption(s => s.ValueCount, o => o.WithName("Test"));

        // Act & Assert
        Should.Throw<Exception>(() => sut.Build())
            .Message.ShouldStartWith("Option name 'Test' must be unique.");
    }

    [Fact]
    public void ShouldThrowException_WhenTryToUpdateNameAfterBuild()
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddSwitchOption(s => s.ValueFlag);

        // Act
        var result = sut.Build();
        var option = result.GetOptionByName(nameof(IntAppOptions.ValueFlag));
        var namedOption = option as ISwitchOption;
        namedOption.ShouldNotBeNull();

        // Assert
        Should.Throw<Exception>(() => namedOption.WithName("NewName"))
            .Message.ShouldStartWith("An option cannot be modified after");
    }

    [Fact]
    public void ShouldThrowException_WhenTryToAddReservedProperty()
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddSwitchOption(s => s.Help);

        // Act & Assert
        Should.Throw<Exception>(() => sut.Build())
            .Message.ShouldStartWith("Reserved properties ('Help') cannot be used.");
    }
}