using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Exceptions;
using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Tests.TestModels;
using Shouldly;
using System;

namespace Oaksoft.ArgumentParser.Tests.ConfigTests;

public class DefaultValueConfigurationTests
{
    [Fact]
    public void ShouldBuild_WhenDefaultValueUsed1()
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.NullValue, o => o.WithDefaultValue(5))
            .AddNamedOption(s => s.Value, o => o.WithDefaultValue(-6))
            .AddSwitchOption(s => s.ValueFlag, o => o.WithDefaultValue(false));

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(4);
        var option = parser.GetOptionByName(nameof(IntAppOptions.NullValue));
        var namedOption = option as IScalarNamedOption<int>;
        namedOption.ShouldNotBeNull();
        namedOption.DefaultValue.ShouldNotBeNull();
        namedOption.DefaultValue.Value.ShouldBe(5);

        option = parser.GetOptionByName(nameof(IntAppOptions.Value));
        namedOption = option as IScalarNamedOption<int>;
        namedOption.ShouldNotBeNull();
        namedOption.DefaultValue.ShouldNotBeNull();
        namedOption.DefaultValue.Value.ShouldBe(-6);

        option = parser.GetOptionByName(nameof(IntAppOptions.ValueFlag));
        var switchOption = option as ISwitchOption;
        switchOption.ShouldNotBeNull();
        switchOption.DefaultValue.ShouldNotBeNull();
        switchOption.DefaultValue.Value.ShouldBe(false);
    }

    [Fact]
    public void ShouldBuild_WhenDefaultValueUsed2()
    {
        // Arrange
        var sut = CommandLine.CreateParser<StringAppOptions>()
            .AddNamedOption(s => s.NullValue, o => o.WithDefaultValue("Funny Cat"))
            .AddNamedOption(s => s.Value, o => o.WithDefaultValue(" "))
            .AddSwitchOption(s => s.ValueFlag, o => o.WithDefaultValue(true));

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(4);
        var option = parser.GetOptionByName(nameof(StringAppOptions.NullValue));
        var namedOption = option as IScalarNamedOption<string>;
        namedOption.ShouldNotBeNull();
        namedOption.DefaultValue.ShouldNotBeNull();
        namedOption.DefaultValue.Value.ShouldBe("Funny Cat");

        option = parser.GetOptionByName(nameof(StringAppOptions.Value));
        namedOption = option as IScalarNamedOption<string>;
        namedOption.ShouldNotBeNull();
        namedOption.DefaultValue.ShouldNotBeNull();
        namedOption.DefaultValue.Value.ShouldBe(" ");

        option = parser.GetOptionByName(nameof(StringAppOptions.ValueFlag));
        var switchOption = option as ISwitchOption;
        switchOption.ShouldNotBeNull();
        switchOption.DefaultValue.ShouldNotBeNull();
        switchOption.DefaultValue.Value.ShouldBe(true);
    }

    [Fact]
    public void ShouldBuild_WhenDefaultValueNotUsed()
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.NullValue)
            .AddNamedOption(s => s.Value)
            .AddSwitchOption(s => s.ValueFlag);

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(4);
        var option = parser.GetOptionByName(nameof(IntAppOptions.NullValue));
        var namedOption = option as IScalarNamedOption<int>;
        namedOption.ShouldNotBeNull();
        namedOption.DefaultValue.ShouldBeNull();

        option = parser.GetOptionByName(nameof(IntAppOptions.Value));
        namedOption = option as IScalarNamedOption<int>;
        namedOption.ShouldNotBeNull();
        namedOption.DefaultValue.ShouldBeNull();

        option = parser.GetOptionByName(nameof(IntAppOptions.ValueFlag));
        var switchOption = option as ISwitchOption;
        switchOption.ShouldNotBeNull();
        switchOption.DefaultValue.ShouldBeNull();
    }

    [Fact]
    public void ShouldBuild_WhenDefaultValueNotUsed2()
    {
        // Arrange
        var sut = CommandLine.CreateParser<StringAppOptions>()
            .AddNamedOption(s => s.NullValue)
            .AddNamedOption(s => s.Value)
            .AddSwitchOption(s => s.ValueFlag);

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(4);
        var option = parser.GetOptionByName(nameof(StringAppOptions.NullValue));
        var namedOption = option as IScalarNamedOption<string>;
        namedOption.ShouldNotBeNull();
        namedOption.DefaultValue.ShouldBeNull();

        option = parser.GetOptionByName(nameof(StringAppOptions.Value));
        namedOption = option as IScalarNamedOption<string>;
        namedOption.ShouldNotBeNull();
        namedOption.DefaultValue.ShouldBeNull();

        option = parser.GetOptionByName(nameof(StringAppOptions.ValueFlag));
        var switchOption = option as ISwitchOption;
        switchOption.ShouldNotBeNull();
        switchOption.DefaultValue.ShouldBeNull();
    }

    [Fact]
    public void ShouldThrowException_WhenTryToUpdateDefaultValueAfterBuild()
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value)
            .AddSwitchOption(s => s.ValueFlag);

        // Act
        var parser = sut.Build();
        var option = parser.GetOptionByName(nameof(IntAppOptions.Value));
        var namedOption = option as IScalarNamedOption<int>;
        option = parser.GetOptionByName(nameof(IntAppOptions.ValueFlag));
        var switchOption = option as ISwitchOption;

        // Assert
        namedOption.ShouldNotBeNull();
        var exception = Should.Throw<OptionBuilderException>(() => namedOption.WithDefaultValue(5));
        exception.Error.Code.ShouldBe(BuilderErrors.CannotBeModified.Code);
        exception.Error.Values.ShouldBeNull();
        exception.OptionName.ShouldBe(nameof(IntAppOptions.Value));

        switchOption.ShouldNotBeNull();
        exception = Should.Throw<OptionBuilderException>(() => switchOption.WithDefaultValue(false));
        exception.Error.Code.ShouldBe(BuilderErrors.CannotBeModified.Code);
        exception.Error.Values.ShouldBeNull();
        exception.OptionName.ShouldBe(nameof(IntAppOptions.ValueFlag));
    }
}