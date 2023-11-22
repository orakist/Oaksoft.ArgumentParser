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
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(6);

        var option = parser.GetOptionByName(nameof(IntAppOptions.Value));
        option.ShouldNotBeNull();
        option.Name.ShouldBe("Value");
        option = parser.GetOptionByName(option.Name);
        option.ShouldNotBeNull();

        option = parser.GetOptionByName(nameof(IntAppOptions.NullValue));
        option.ShouldNotBeNull();
        option.Name.ShouldBe("ValueCount");
        option = parser.GetOptionByName(option.Name);
        option.ShouldNotBeNull();

        option = parser.GetOptionByName(nameof(IntAppOptions.Values));
        option.ShouldNotBeNull();
        option.Name.ShouldBe("ValueList");
        option = parser.GetOptionByName(option.Name);
        option.ShouldNotBeNull();

        option = parser.GetOptionByName(nameof(IntAppOptions.NullValues));
        option.ShouldNotBeNull();
        option.Name.ShouldBe("ValueX");
        option = parser.GetOptionByName(option.Name);
        option.ShouldNotBeNull();

        option = parser.GetOptionByName(nameof(IntAppOptions.ValueFlag));
        option.ShouldNotBeNull();
        option.Name.ShouldBe("ValueY");
        option = parser.GetOptionByName(option.Name);
        option.ShouldNotBeNull();
    }

    [Fact]
    public void ShouldBuildOptions_WhenDefaultNamesUsed()
    {
        // Arrange, should ignore empty names
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.NullValue, s => s.NullValueFlag)
            .AddNamedOption(s => s.Values)
            .AddValueOption(s => s.NullValues)
            .AddValueOption(s => s.ValueFlag);

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(6);

        var option = parser.GetOptionByName(nameof(IntAppOptions.Value));
        option.ShouldNotBeNull();
        option.Name.ShouldBe(nameof(IntAppOptions.Value));

        option = parser.GetOptionByName(nameof(IntAppOptions.NullValue));
        option.ShouldNotBeNull();
        option.Name.ShouldBe(nameof(IntAppOptions.NullValue));
        option = parser.GetOptionByName(nameof(IntAppOptions.NullValueFlag));
        option.ShouldNotBeNull();

        option = parser.GetOptionByName(nameof(IntAppOptions.Values));
        option.ShouldNotBeNull();
        option.Name.ShouldBe(nameof(IntAppOptions.Values));

        option = parser.GetOptionByName(nameof(IntAppOptions.NullValues));
        option.ShouldNotBeNull();
        option.Name.ShouldBe(nameof(IntAppOptions.NullValues));

        option = parser.GetOptionByName(nameof(IntAppOptions.ValueFlag));
        option.ShouldNotBeNull();
        option.Name.ShouldBe(nameof(IntAppOptions.ValueFlag));
    }

    [Fact]
    public void ShouldBuildOptions_WhenNumberAndLetterNamesUsed()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>()
            .AddNamedOption(s => s.__1_2_3, o => o.AddAliases("x"))
            .AddNamedOption(s => s.__2_Value)
            .AddNamedOption(s => s.ValueTest);

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(4);

        var option = parser.GetOptionByName(nameof(SampleOptionNames.__1_2_3));
        option.ShouldNotBeNull();
        option.Name.ShouldBe(nameof(SampleOptionNames.__1_2_3));

        option = parser.GetOptionByName(nameof(SampleOptionNames.__2_Value));
        option.ShouldNotBeNull();
        option.Name.ShouldBe(nameof(SampleOptionNames.__2_Value));

        option = parser.GetOptionByName(nameof(SampleOptionNames.ValueTest));
        option.ShouldNotBeNull();
        option.Name.ShouldBe(nameof(SampleOptionNames.ValueTest));
    }

    [Fact]
    public void ShouldThrowException_WhenSameNameWithPropertyUsed()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>()
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.__2_Value, o => o.WithName("Value"));

        // Act & Assert
        Should.Throw<Exception>(() => sut.Build())
            .Message.ShouldStartWith("Name 'Value' is already in use!");
    }

    [Fact]
    public void ShouldThrowException_WhenNameIsEmpty()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>();

        // Act & Assert
        Should.Throw<Exception>(() => sut.AddNamedOption(s => s.Value, o => o.WithName(" ")))
            .Message.ShouldStartWith("The name string cannot be empty!");
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
            .Message.ShouldStartWith("Name 'Test' is already in use!");
    }

    [Fact]
    public void ShouldThrowException_WhenTryToUpdateNameAfterBuild()
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddSwitchOption(s => s.ValueFlag);

        // Act
        var parser = sut.Build();
        var option = parser.GetOptionByName(nameof(IntAppOptions.ValueFlag));
        var namedOption = option as ISwitchOption;

        // Assert
        namedOption.ShouldNotBeNull();
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
            .Message.ShouldStartWith("The reserved 'Help' property is not configurable.");
    }
}