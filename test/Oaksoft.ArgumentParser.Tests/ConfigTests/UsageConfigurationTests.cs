using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Tests.AppModels;
using Shouldly;

namespace Oaksoft.ArgumentParser.Tests.ConfigTests;

public class UsageConfigurationTests
{
    [Fact]
    public void ShouldBuildOptions_WithCustomUsage()
    {
        // Arrange
        const string usage1 = "-n <integer-value>";
        const string usage2 = " -v     <flag-value>   ";
        const string usage2Trimmed = "-v <flag-value>";
        const string usage3 = "type    integer     value";
        const string usage3Trimmed = "type integer value";
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.NullValue, o => o.WithUsage(usage1))
            .AddSwitchOption(s => s.NullValueFlag, o => o.WithUsage(usage2))
            .AddNamedOption(s => s.Values, o => o.WithUsage(usage2))
            .AddValueOption(s => s.NullValues, o => o.WithUsage(usage3))
            .AddValueOption(s => s.Value, o => o.WithUsage(usage3));

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(6);
        var text = parser.GetHelpText(false);

        var option = parser.GetOptionByName(nameof(IntAppOptions.NullValue));
        option.ShouldNotBeNull();
        option.Usage.ShouldBe(usage1);
        text.ShouldContain(option.Usage);

        option = parser.GetOptionByName(nameof(IntAppOptions.NullValueFlag));
        option.ShouldNotBeNull();
        option.Usage.ShouldBe(usage2Trimmed);
        text.ShouldContain(option.Usage);

        option = parser.GetOptionByName(nameof(IntAppOptions.Values));
        option.ShouldNotBeNull();
        option.Usage.ShouldBe(usage2Trimmed);
        text.ShouldContain(option.Usage);

        option = parser.GetOptionByName(nameof(IntAppOptions.NullValues));
        option.ShouldNotBeNull();
        option.Usage.ShouldBe(usage3Trimmed);
        text.ShouldContain(option.Usage);

        option = parser.GetOptionByName(nameof(IntAppOptions.Value));
        option.ShouldNotBeNull();
        option.Usage.ShouldBe(usage3Trimmed);
        text.ShouldContain(option.Usage);
    }

    [Fact]
    public void ShouldThrowException_WhenUsageIsEmpty()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>();

        // Act & Assert
        Should.Throw<Exception>(() => sut.AddNamedOption(s => s.Value, o => o.WithUsage(" ")))
            .Message.ShouldStartWith("The usage string cannot be empty!");
    }

    [Fact]
    public void ShouldBuildOptions_WithDefaultUsage()
    {
        // Arrange, should ignore empty usages
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.NullValue, mustHaveOneValue: false)
            .AddNamedOption(s => s.Values)
            .AddValueOption(s => s.NullValues)
            .AddValueOption(s => s.ValueFlag);

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(6);
        var text = parser.GetHelpText(false);

        var option = parser.GetOptionByName(nameof(IntAppOptions.Value));
        option.ShouldNotBeNull();
        option.Usage.ShouldBe("-v <value>");
        text.ShouldContain(option.Usage);

        option = parser.GetOptionByName(nameof(IntAppOptions.NullValue));
        option.ShouldNotBeNull();
        option.Usage.ShouldBe("-n (value)");
        text.ShouldContain(option.Usage);

        option = parser.GetOptionByName(nameof(IntAppOptions.Values));
        option.ShouldNotBeNull();
        option.Usage.ShouldBe("-a (value)");
        text.ShouldContain(option.Usage);

        option = parser.GetOptionByName(nameof(IntAppOptions.NullValues));
        option.ShouldNotBeNull();
        option.Usage.ShouldBe($"value for '{nameof(IntAppOptions.NullValues)}' option");
        text.ShouldContain(option.Usage);

        option = parser.GetOptionByName(nameof(IntAppOptions.ValueFlag));
        option.ShouldNotBeNull();
        option.Usage.ShouldBe($"value for '{nameof(IntAppOptions.ValueFlag)}' option");
        text.ShouldContain(option.Usage);
    }

    [Fact]
    public void ShouldBuildOptions_WithDefaultUsageForwardSlash()
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>(OptionPrefixRules.AllowForwardSlash)
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.NullValue, mustHaveOneValue: false)
            .AddNamedOption(s => s.Values)
            .AddValueOption(s => s.NullValues)
            .AddValueOption(s => s.ValueFlag);

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(6);
        var text = parser.GetHelpText(false);

        var option = parser.GetOptionByName(nameof(IntAppOptions.Value));
        option.ShouldNotBeNull();
        option.Usage.ShouldBe("/v <value>");
        text.ShouldContain(option.Usage);

        option = parser.GetOptionByName(nameof(IntAppOptions.NullValue));
        option.ShouldNotBeNull();
        option.Usage.ShouldBe("/n (value)");
        text.ShouldContain(option.Usage);

        option = parser.GetOptionByName(nameof(IntAppOptions.Values));
        option.ShouldNotBeNull();
        option.Usage.ShouldBe("/a (value)");
        text.ShouldContain(option.Usage);

        option = parser.GetOptionByName(nameof(IntAppOptions.NullValues));
        option.ShouldNotBeNull();
        option.Usage.ShouldBe($"value for '{nameof(IntAppOptions.NullValues)}' option");
        text.ShouldContain(option.Usage);

        option = parser.GetOptionByName(nameof(IntAppOptions.ValueFlag));
        option.ShouldNotBeNull();
        option.Usage.ShouldBe($"value for '{nameof(IntAppOptions.ValueFlag)}' option");
        text.ShouldContain(option.Usage);
    }
}