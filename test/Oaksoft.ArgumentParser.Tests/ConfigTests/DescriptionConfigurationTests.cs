using Oaksoft.ArgumentParser.Errors.Builder;
using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Tests.TestModels;
using Shouldly;

namespace Oaksoft.ArgumentParser.Tests.ConfigTests;

public class DescriptionConfigurationTests : ArgumentParserTestBase
{
    [Fact]
    public void ShouldBuild_WhenDescriptionUsed()
    {
        // Arrange
        const string description = "Cats have an adorable face with a tiny nose.";
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .ConfigureSettings(s => s.Description = "Test description!")
            .AddNamedOption(s => s.NullValue, o => o.WithDescription(description))
            .AddCounterOption(s => s.NullValueCount, o => o.WithDescription(description))
            .AddNamedOption(s => s.Values, o => o.WithDescription(description))
            .AddValueOption(s => s.NullValues, o => o.WithDescription(description))
            .AddSwitchOption(s => s.ValueFlag, o => o.WithDescription(description))
            .AddValueOption(s => s.Value, o => o.WithDescription(description));

        // Act
        var parser = sut.Build();
        var text = parser.GetHelpText(false);
        var header = parser.GetHeaderText();

        // Assert
        parser.GetOptions().Count.ShouldBe(6);

        text.ShouldContain(parser.Settings.Description!);
        header.ShouldContain(parser.Settings.Description!);

        var option = parser.GetOptionByName(nameof(IntAppOptions.NullValue));
        option.ShouldNotBeNull();
        option.Description.ShouldBe(description);
        text.ShouldContain(option.Description!);

        option = parser.GetOptionByName(nameof(IntAppOptions.NullValueCount));
        option.ShouldNotBeNull();
        option.Description.ShouldBe(description);
        text.ShouldContain(option.Description!);

        option = parser.GetOptionByName(nameof(IntAppOptions.Values));
        option.ShouldNotBeNull();
        option.Description.ShouldBe(description);
        text.ShouldContain(option.Description!);

        option = parser.GetOptionByName(nameof(IntAppOptions.NullValues));
        option.ShouldNotBeNull();
        option.Description.ShouldBe(description);
        text.ShouldContain(option.Description!);

        option = parser.GetOptionByName(nameof(IntAppOptions.ValueFlag));
        option.ShouldNotBeNull();
        option.Description.ShouldBe(description);
        text.ShouldContain(option.Description!);

        option = parser.GetOptionByName(nameof(IntAppOptions.Value));
        option.ShouldNotBeNull();
        option.Description.ShouldBe(description);
        text.ShouldContain(option.Description!);
    }

    [Fact]
    public void ShouldBuild_WithDefaultDescription()
    {
        // Arrange, should ignore empty descriptions
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value)
            .AddCounterOption(s => s.NullValue)
            .AddNamedOption(s => s.Values)
            .AddValueOption(s => s.NullValues, o => o.WithDescription(""))
            .AddValueOption(s => s.ValueFlag, o => o.WithDescription("   "));

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(5);
        var text = parser.GetHelpText(false);

        var option = parser.GetOption(nameof(IntAppOptions.Value));
        option.ShouldNotBeNull();
        option.Description.ShouldBe($"Performs '{option.Name}' option.");
        text.ShouldContain(option.Description!);

        option = parser.GetOption(nameof(IntAppOptions.NullValue));
        option.ShouldNotBeNull();
        option.Description.ShouldBe($"Performs '{option.Name}' option.");
        text.ShouldContain(option.Description!);

        option = parser.GetOption(nameof(IntAppOptions.Values));
        option.ShouldNotBeNull();
        option.Description.ShouldBe($"Performs '{option.Name}' option.");
        text.ShouldContain(option.Description!);

        option = parser.GetOptionByName(nameof(IntAppOptions.NullValues));
        option.ShouldNotBeNull();
        option.Description.ShouldBe($"Captures values for '{option.Name}' option.");
        text.ShouldContain(option.Description!);

        option = parser.GetOptionByName(nameof(IntAppOptions.ValueFlag));
        option.ShouldNotBeNull();
        option.Description.ShouldBe($"Captures value for '{option.Name}' option.");
        text.ShouldContain(option.Description!);
    }

    [Fact]
    public void ShouldThrowException_WhenTryToUpdateDescriptionAfterBuild()
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
        var exception = Should.Throw<OptionBuilderException>(() => namedOption.WithDescription("test"));
        var info = exception.Error;

        info.Error.Code.ShouldBe(BuilderErrors.CannotBeModified.Code);
        info.Values.ShouldBeNull();
        info.OptionName.ShouldBe(nameof(IntAppOptions.Value));
    }
}