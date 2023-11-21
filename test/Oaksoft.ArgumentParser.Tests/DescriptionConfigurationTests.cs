using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Tests.Options;
using Shouldly;

namespace Oaksoft.ArgumentParser.Tests;

public class DescriptionConfigurationTests
{
    [Fact]
    public void ShouldBuildOptions_WhenDescriptionUsed()
    {
        // Arrange
        const string description = "Cats have an adorable face with a tiny nose.";
        var sut = CommandLine.CreateParser<ByteAppOptions>()
            .AddNamedOption(s => s.NullValue, o => o.WithDescription(description))
            .AddCountOption(s => s.NullValueCount, o => o.WithDescription(description))
            .AddNamedOption(s => s.Values, o => o.WithDescription(description))
            .AddValueOption(s => s.NullValues, o => o.WithDescription(description))
            .AddValueOption(s => s.ValueFlag, o => o.WithDescription(description));

        // Act
        var result = sut.Build();

        // Assert
        result.GetOptions().Count.ShouldBe(6);
        var text = result.GetHelpText(false);

        var option = result.GetOptionByName(nameof(ByteAppOptions.NullValue));
        option.ShouldNotBeNull();
        option.Description.ShouldBe(description);
        text.ShouldContain(option.Description!);

        option = result.GetOptionByName(nameof(ByteAppOptions.NullValueCount));
        option.ShouldNotBeNull();
        option.Description.ShouldBe(description);
        text.ShouldContain(option.Description!);

        option = result.GetOptionByName(nameof(ByteAppOptions.Values));
        option.ShouldNotBeNull();
        option.Description.ShouldBe(description);
        text.ShouldContain(option.Description!);

        option = result.GetOptionByName(nameof(ByteAppOptions.NullValues));
        option.ShouldNotBeNull();
        option.Description.ShouldBe(description);
        text.ShouldContain(option.Description!);

        option = result.GetOptionByName(nameof(ByteAppOptions.ValueFlag));
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
        var result = sut.Build();

        // Assert
        result.GetOptions().Count.ShouldBe(6);
        var text = result.GetHelpText(false);

        var option = result.GetOptionByName(nameof(IntAppOptions.Value));
        option.ShouldNotBeNull();
        option.Description.ShouldBe($"Performs '{option.Name}' option.");
        text.ShouldContain(option.Description!);

        option = result.GetOptionByName(nameof(IntAppOptions.NullValue));
        option.ShouldNotBeNull();
        option.Description.ShouldBe($"Performs '{option.Name}' option.");
        text.ShouldContain(option.Description!);

        option = result.GetOptionByName(nameof(IntAppOptions.Values));
        option.ShouldNotBeNull();
        option.Description.ShouldBe($"Performs '{option.Name}' option.");
        text.ShouldContain(option.Description!);

        option = result.GetOptionByName(nameof(IntAppOptions.NullValues));
        option.ShouldNotBeNull();
        option.Description.ShouldBe($"Captures value for '{option.Name}' option.");
        text.ShouldContain(option.Description!);

        option = result.GetOptionByName(nameof(IntAppOptions.ValueFlag));
        option.ShouldNotBeNull();
        option.Description.ShouldBe($"Captures value for '{option.Name}' option.");
        text.ShouldContain(option.Description!);
    }
}