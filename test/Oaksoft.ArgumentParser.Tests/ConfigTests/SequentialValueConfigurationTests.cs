using Oaksoft.ArgumentParser.Errors.Builder;
using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Tests.TestModels;
using Shouldly;

namespace Oaksoft.ArgumentParser.Tests.ConfigTests;

public class SequentialValueConfigurationTests
{
    [Fact]
    public void ShouldBuild_WhenAllowSequentialValuesConfigured()
    {
        // Arrange
        var sut = CommandLine.CreateParser<StringAppOptions>()
            .AddNamedOption(s => s.Values, o => o.WithAllowSequentialValues(true))
            .AddNamedOption(s => s.NullValues, o => o.WithAllowSequentialValues(false));

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(3);
        var option = parser.GetOptionByName(nameof(StringAppOptions.Values));
        var namedOption = option as ISequentialNamedOption<string>;
        namedOption.ShouldNotBeNull();
        namedOption.AllowSequentialValues.ShouldBeTrue();

        option = parser.GetOptionByName(nameof(StringAppOptions.NullValues));
        namedOption = option as ISequentialNamedOption<string>;
        namedOption.ShouldNotBeNull();
        namedOption.AllowSequentialValues.ShouldBeFalse();
    }

    [Fact]
    public void ShouldBuild_WhenAllowSequentialValuesNotConfigured()
    {
        // Arrange
        var sut = CommandLine.CreateParser<StringAppOptions>()
            .AddNamedOption(s => s.Values)
            .AddNamedOption(s => s.NullValues);

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(3);
        var option = parser.GetOptionByName(nameof(StringAppOptions.Values));
        var namedOption = option as ISequentialNamedOption<string>;
        namedOption.ShouldNotBeNull();
        namedOption.AllowSequentialValues.ShouldBeTrue();

        option = parser.GetOptionByName(nameof(StringAppOptions.NullValues));
        namedOption = option as ISequentialNamedOption<string>;
        namedOption.ShouldNotBeNull();
        namedOption.AllowSequentialValues.ShouldBeTrue();
    }

    [Fact]
    public void ShouldBuild_EnableValueTokenSplittingConfigured()
    {
        // Arrange
        var sut = CommandLine.CreateParser<StringAppOptions>()
            .AddNamedOption(s => s.Values, o => o.WithEnableValueTokenSplitting(false))
            .AddValueOption(s => s.NullValues, o => o.WithEnableValueTokenSplitting(false));

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(3);
        var option = parser.GetOptionByName(nameof(StringAppOptions.Values));
        var namedOption = option as ISequentialValueOption<string>;
        namedOption.ShouldNotBeNull();
        namedOption.EnableValueTokenSplitting.ShouldBeFalse();

        option = parser.GetOptionByName(nameof(StringAppOptions.NullValues));
        namedOption = option as ISequentialValueOption<string>;
        namedOption.ShouldNotBeNull();
        namedOption.EnableValueTokenSplitting.ShouldBeFalse();
    }

    [Fact]
    public void ShouldBuild_EnableValueTokenSplittingNotConfigured()
    {
        // Arrange
        var sut = CommandLine.CreateParser<StringAppOptions>()
            .AddValueOption(s => s.Values)
            .AddNamedOption(s => s.NullValues);

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(3);
        var option = parser.GetOptionByName(nameof(StringAppOptions.Values));
        var namedOption = option as ISequentialValueOption<string>;
        namedOption.ShouldNotBeNull();
        namedOption.EnableValueTokenSplitting.ShouldBeTrue();

        option = parser.GetOptionByName(nameof(StringAppOptions.NullValues));
        namedOption = option as ISequentialValueOption<string>;
        namedOption.ShouldNotBeNull();
        namedOption.EnableValueTokenSplitting.ShouldBeTrue();
    }

    [Fact]
    public void ShouldThrowException_WhenTryToUpdateSequentialValueConfigurationAfterBuild()
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Values)
            .AddValueOption(s => s.NullValues);

        // Act
        var parser = sut.Build();
        var option = parser.GetOptionByName(nameof(IntAppOptions.Values));
        var option1 = option as ISequentialNamedOption<int>;
        option = parser.GetOptionByName(nameof(IntAppOptions.NullValues));
        var option2 = option as ISequentialValueOption<int>;

        // Assert
        option1.ShouldNotBeNull();
        option2.ShouldNotBeNull();

        var exception = Should.Throw<OptionBuilderException>(() => option1.WithAllowSequentialValues(true));
        var info = exception.Error;
        info.Error.Code.ShouldBe(BuilderErrors.CannotBeModified.Code);
        info.Values.ShouldBeNull();
        info.OptionName.ShouldBe(nameof(IntAppOptions.Values));

        exception = Should.Throw<OptionBuilderException>(() => option1.WithEnableValueTokenSplitting(true));
        info = exception.Error;
        info.Error.Code.ShouldBe(BuilderErrors.CannotBeModified.Code);
        info.Values.ShouldBeNull();
        info.OptionName.ShouldBe(nameof(IntAppOptions.Values));

        exception = Should.Throw<OptionBuilderException>(() => option2.WithEnableValueTokenSplitting(true));
        info = exception.Error;
        info.Error.Code.ShouldBe(BuilderErrors.CannotBeModified.Code);
        info.Values.ShouldBeNull();
        info.OptionName.ShouldBe(nameof(IntAppOptions.NullValues));
    }
}