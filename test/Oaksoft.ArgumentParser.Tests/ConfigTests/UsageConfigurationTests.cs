using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Errors.Builder;
using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Tests.TestModels;
using Shouldly;

namespace Oaksoft.ArgumentParser.Tests.ConfigTests;

public class UsageConfigurationTests : ArgumentParserTestBase
{
    [Fact]
    public void ShouldBuild_WithCustomUsage()
    {
        // Arrange
        const string usage1 = "-n <integer-value>";
        const string usage2 = " -v     <flag-value>   ";
        const string usage2Trimmed = "-v <flag-value>";
        const string usage4 = "--abc";
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.NullValue, o => o.WithUsage(usage1))
            .AddSwitchOption(s => s.NullValueFlag, o => o.WithUsage(usage2))
            .AddNamedOption(s => s.Values, o => o.WithUsage(usage2))
            .AddCounterOption(s => s.ValueCount, o => o.WithUsage(usage4));

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(4);
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

        option = parser.GetOptionByName(nameof(IntAppOptions.ValueCount));
        option.ShouldNotBeNull();
        option.Usage.ShouldBe(usage4);
        text.ShouldContain(option.Usage);
    }

    [Fact]
    public void ShouldThrowException_WhenUsageIsEmpty()
    {
        // Arrange
        const string name = "usage";
        var sut = CommandLine.CreateParser<IntAppOptions>();

        // Act 
        var exception = Should.Throw<OptionBuilderException>(() => sut.AddNamedOption(s => s.Value, o => o.WithUsage(" ")));
        var info = exception.Error;

        // Assert
        info.Error.Code.ShouldBe("BuilderErrors.EmptyValue");
        info.Values.ShouldHaveSingleItem();
        info.Values.ShouldContain(name);
        info.OptionName.ShouldBe(nameof(IntAppOptions.Value));
        var message = string.Format(info.Error.Format, name);
        exception.Message.ShouldStartWith(message);
    }

    [Fact]
    public void ShouldBuild_WithDefaultUsage()
    {
        // Arrange, should ignore empty usages
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value, o => o.WithDefaultValue(5).WithAllowedValues(1, 2, 3))
            .AddNamedOption(s => s.NullValue, mustHaveOneValue: false)
            .AddNamedOption(s => s.Values)
            .AddValueOption(s => s.NullValues)
            .AddValueOption(s => s.ValueFlag);

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(5);
        var text = parser.GetHelpText(false);

        var option = parser.GetOptionByName(nameof(IntAppOptions.Value));
        option.ShouldNotBeNull();
        option.Usage.ShouldBe("-v <value>");
        text.ShouldContain(option.Usage);

        text.ShouldContain("[Default: 5]");
        text.ShouldContain("[Allowed-Values: 1 | 2 | 3]");

        option = parser.GetOptionByName(nameof(IntAppOptions.NullValue));
        option.ShouldNotBeNull();
        option.Usage.ShouldBe("-n (value)");
        text.ShouldContain(option.Usage);

        option = parser.GetOptionByName(nameof(IntAppOptions.Values));
        option.ShouldNotBeNull();
        option.Usage.ShouldBe("-a (values)");
        text.ShouldContain(option.Usage);

        option = parser.GetOptionByName(nameof(IntAppOptions.NullValues));
        option.ShouldNotBeNull();
        option.Usage.ShouldBeNullOrEmpty();

        option = parser.GetOptionByName(nameof(IntAppOptions.ValueFlag));
        option.ShouldNotBeNull();
        option.Usage.ShouldBeNullOrEmpty();
    }

    [Fact]
    public void ShouldBuild_WithDefaultUsageForwardSlash()
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
        parser.GetOptions().Count.ShouldBe(5);
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
        option.Usage.ShouldBe("/a (values)");
        text.ShouldContain(option.Usage);

        option = parser.GetOptionByName(nameof(IntAppOptions.NullValues));
        option.ShouldNotBeNull();
        option.Usage.ShouldBeNullOrEmpty();

        option = parser.GetOptionByName(nameof(IntAppOptions.ValueFlag));
        option.ShouldNotBeNull();
        option.Usage.ShouldBeNullOrEmpty();
    }

    [Fact]
    public void ShouldThrowException_WhenTryToUpdateUsageAfterBuild()
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
        var exception = Should.Throw<OptionBuilderException>(() => namedOption.WithUsage("test"));
        var info = exception.Error;

        info.Error.Code.ShouldBe("BuilderErrors.CannotBeModified");
        info.Values.ShouldBeNull();
        info.OptionName.ShouldBe(nameof(IntAppOptions.Value));
    }
}