using Oaksoft.ArgumentParser.Errors.Builder;
using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Tests.TestModels;
using Shouldly;

namespace Oaksoft.ArgumentParser.Tests.ConfigTests;

public class NameConfigurationTests
{
    [Fact]
    public void ShouldBuild_WhenDifferentNamesUsed()
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value, o => o.WithName("Value"))
            .AddCounterOption(s => s.NullValue, o => o.WithName("ValueCount"))
            .AddSwitchOption(s => s.NullValueFlag, o => o.WithName("ValueFlag"))
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

        option = parser.GetOptionByName(nameof(IntAppOptions.NullValueFlag));
        option.ShouldNotBeNull();
        option.Name.ShouldBe("ValueFlag");
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
        option.Name.ShouldBe("ValueFlag");
        option = parser.GetOptionByName(option.Name);
        option.ShouldNotBeNull();
    }

    [Fact]
    public void ShouldBuild_WhenDefaultNamesUsed()
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
        parser.GetOptions().Count.ShouldBe(5);

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
    public void ShouldBuild_WhenNumberAndLetterNamesUsed()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>()
            .AddNamedOption(s => s.__1_2_3, o => o.AddAliases("x"))
            .AddNamedOption(s => s.__2_Value)
            .AddNamedOption(s => s.ValueTest);

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(3);

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
    public void ShouldThrowException_WhenNameIsEmpty()
    {
        // Arrange
        const string value = "name";
        var sut = CommandLine.CreateParser<IntAppOptions>();

        // Act
        var exception = Should.Throw<OptionBuilderException>(() => sut.AddNamedOption(s => s.Value, o => o.WithName(" ")));
        var info = exception.Error;

        // Assert
        info.Error.Code.ShouldBe(BuilderErrors.EmptyValue.Code);
        info.Values.ShouldHaveSingleItem();
        info.Values.ShouldContain(value);
        info.OptionName.ShouldBe(nameof(IntAppOptions.Value));
        var message = string.Format(info.Error.Format, value);
        exception.Message.ShouldStartWith(message);
    }

    [Fact]
    public void ShouldThrowException_WhenNameIsInvalid()
    {
        // Arrange
        const string name = "a+a-a";
        var sut = CommandLine.CreateParser<IntAppOptions>();

        // Act
        var exception = Should.Throw<OptionBuilderException>(() => sut.AddNamedOption(s => s.Value, o => o.WithName(name)));
        var info = exception.Error;

        // Assert
        info.Error.Code.ShouldBe(BuilderErrors.InvalidName.Code);
        info.Values.ShouldHaveSingleItem();
        info.Values.ShouldContain(name);
        info.OptionName.ShouldBe(nameof(IntAppOptions.Value));
        var message = string.Format(info.Error.Format, name);
        exception.Message.ShouldStartWith(message);
    }

    [Fact]
    public void ShouldThrowException_WhenReservedNameUsed()
    {
        // Arrange
        const string name = "Help";
        const string reservedNames = "'Help', 'Version'";
        var sut = CommandLine.CreateParser<IntAppOptions>();

        // Act
        var exception = Should.Throw<OptionBuilderException>(() => sut.AddNamedOption(s => s.Value, o => o.WithName(name)));
        var info = exception.Error;

        // Assert
        info.Error.Code.ShouldBe(BuilderErrors.ReservedOptionName.Code);
        info.Values.ShouldNotBeEmpty();
        info.Values.ShouldContain(name);
        info.Values.ShouldContain(reservedNames);
        info.OptionName.ShouldBe(nameof(IntAppOptions.Value));
        var message = string.Format(info.Error.Format, name, reservedNames);
        exception.Message.ShouldStartWith(message);
    }

    [Fact]
    public void ShouldThrowException_WhenSameNameWithPropertyUsed()
    {
        // Arrange
        const string name = "Value";
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.ValueCount, o => o.WithName(name));

        // Act
        var exception = Should.Throw<OptionBuilderException>(sut.Build);
        var info = exception.Error;

        // Assert
        info.Error.Code.ShouldBe(BuilderErrors.NameAlreadyInUse.Code);
        info.Values.ShouldHaveSingleItem();
        info.Values.ShouldContain(name);
        info.OptionName.ShouldBe(nameof(IntAppOptions.Value));
        var message = string.Format(info.Error.Format, name);
        exception.Message.ShouldStartWith(message);
    }

    [Fact]
    public void ShouldThrowException_WhenSameCustomNameUsed()
    {
        // Arrange
        const string name = "Test";
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value, o => o.WithName(name))
            .AddNamedOption(s => s.ValueCount, o => o.WithName(name));

        // Act
        var exception = Should.Throw<OptionBuilderException>(sut.Build);
        var info = exception.Error;

        // Assert
        info.Error.Code.ShouldBe(BuilderErrors.NameAlreadyInUse.Code);
        info.Values.ShouldHaveSingleItem();
        info.Values.ShouldContain(name);
        info.OptionName.ShouldBe(nameof(IntAppOptions.ValueCount));
        var message = string.Format(info.Error.Format, name);
        exception.Message.ShouldStartWith(message);
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
        var exception = Should.Throw<OptionBuilderException>(() => namedOption.WithName("NewName"));
        var info = exception.Error;

        info.Error.Code.ShouldBe(BuilderErrors.CannotBeModified.Code);
        info.Values.ShouldBeNull();
        info.OptionName.ShouldBe(nameof(IntAppOptions.ValueFlag));
    }
}