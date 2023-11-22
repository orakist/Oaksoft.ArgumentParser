using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Tests.AppModels;
using Shouldly;

namespace Oaksoft.ArgumentParser.Tests.ConfigTests;

public class DescriptionConfigurationTests
{
    [Fact]
    public void ShouldBuildOptions_WhenDescriptionUsed()
    {
        // Arrange
        const string description = "Cats have an adorable face with a tiny nose.";
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.NullValue, o => o.WithDescription(description))
            .AddCountOption(s => s.NullValueCount, o => o.WithDescription(description))
            .AddNamedOption(s => s.Values, o => o.WithDescription(description))
            .AddValueOption(s => s.NullValues, o => o.WithDescription(description))
            .AddValueOption(s => s.ValueFlag, o => o.WithDescription(description));

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(6);
        var text = parser.GetHelpText(false);

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
    }

    [Fact]
    public void ShouldBuildOptions_WithDefaultDescription()
    {
        // Arrange, should ignore empty descriptions
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value)
            .AddCountOption(s => s.NullValue)
            .AddNamedOption(s => s.Values)
            .AddValueOption(s => s.NullValues, o => o.WithDescription(""))
            .AddValueOption(s => s.ValueFlag, o => o.WithDescription("   "));

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(6);
        var text = parser.GetHelpText(false);

        var option = parser.GetOptionByName(nameof(IntAppOptions.Value));
        option.ShouldNotBeNull();
        option.Description.ShouldBe($"Performs '{option.Name}' option.");
        text.ShouldContain(option.Description!);

        option = parser.GetOptionByName(nameof(IntAppOptions.NullValue));
        option.ShouldNotBeNull();
        option.Description.ShouldBe($"Performs '{option.Name}' option.");
        text.ShouldContain(option.Description!);

        option = parser.GetOptionByName(nameof(IntAppOptions.Values));
        option.ShouldNotBeNull();
        option.Description.ShouldBe($"Performs '{option.Name}' option.");
        text.ShouldContain(option.Description!);

        option = parser.GetOptionByName(nameof(IntAppOptions.NullValues));
        option.ShouldNotBeNull();
        option.Description.ShouldBe($"Captures value for '{option.Name}' option.");
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
        Should.Throw<Exception>(() => namedOption.WithDescription("test"))
            .Message.ShouldStartWith("An option cannot be modified after");
    }
}