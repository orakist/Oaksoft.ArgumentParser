using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Errors.Builder;
using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Tests.TestModels;
using Shouldly;

namespace Oaksoft.ArgumentParser.Tests.ConfigTests;

public class AliasSuggestionTests
{
    [Fact]
    public void ShouldSuggestAlias_WhenOnlyLettersUsedInOptionNames()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>()
            .AddNamedOption(s => s.Value)
            .AddSwitchOption(s => s.ValueTest)
            .AddNamedOption(s => s.ValueTestProp);

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(3);

        var option = parser.GetOptionByName(nameof(SampleOptionNames.Value));
        var namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(4);
        namedOption.Aliases.ShouldContain("-v");
        namedOption.Aliases.ShouldContain("--value");
        namedOption.Aliases.ShouldContain("/v");
        namedOption.Aliases.ShouldContain("/value");

        option = parser.GetOptionByName(nameof(SampleOptionNames.ValueTest));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(4);
        namedOption.Aliases.ShouldContain("-t");
        namedOption.Aliases.ShouldContain("--value-test");
        namedOption.Aliases.ShouldContain("/t");
        namedOption.Aliases.ShouldContain("/value-test");

        option = parser.GetOptionByName(nameof(SampleOptionNames.ValueTestProp));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(4);
        namedOption.Aliases.ShouldContain("-p");
        namedOption.Aliases.ShouldContain("--value-test-prop");
        namedOption.Aliases.ShouldContain("/p");
        namedOption.Aliases.ShouldContain("/value-test-prop");
    }

    [Fact]
    public void ShouldSuggestAlias_WhenMoreThanThreeWordsOptionUsed1()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>()
            .ConfigureSettings(s => s.MaxSuggestedAliasWordCount = 3)
            .AddNamedOption(s => s.ValueTestPropEx);

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(1);

        var option = parser.GetOptionByName(nameof(SampleOptionNames.ValueTestPropEx));
        var namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(4);
        namedOption.Aliases.ShouldContain("-v");
        namedOption.Aliases.ShouldContain("--value-test-prop");
        namedOption.Aliases.ShouldContain("/v");
        namedOption.Aliases.ShouldContain("/value-test-prop");
    }

    [Fact]
    public void ShouldSuggestAlias_WhenVeryLongOptionPropertyUsed_WithDefaultWordCount()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>()
            .ConfigureSettings(s => s.MaxAliasLength = 60)
            .AddNamedOption(s => s.VeryLongApplicationOptionValuePropertyName);

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(1);

        var option = parser.GetOptionByName(nameof(SampleOptionNames.VeryLongApplicationOptionValuePropertyName));
        var namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(4);
        namedOption.Aliases.ShouldContain("-v");
        namedOption.Aliases.ShouldContain("--very-long-application-option");
        namedOption.Aliases.ShouldContain("/v");
        namedOption.Aliases.ShouldContain("/very-long-application-option");
    }

    [Fact]
    public void ShouldSuggestAlias_WhenVeryLongOptionPropertyUsed_WithDefaultAliasLength()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>()
            .ConfigureSettings(s => s.MaxSuggestedAliasWordCount = 7)
            .AddNamedOption(s => s.VeryLongApplicationOptionValuePropertyName);

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(1);

        var option = parser.GetOptionByName(nameof(SampleOptionNames.VeryLongApplicationOptionValuePropertyName));
        var namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(4);
        namedOption.Aliases.ShouldContain("-v");
        namedOption.Aliases.ShouldContain("--very-long-application-option");
        namedOption.Aliases.ShouldContain("/v");
        namedOption.Aliases.ShouldContain("/very-long-application-option");
    }

    [Fact]
    public void ShouldSuggestAlias_WhenVeryLongOptionPropertyUsed_WithCustomAliasLength()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>()
            .ConfigureSettings(s => s.MaxSuggestedAliasWordCount = 7)
            .ConfigureSettings(s => s.MaxAliasLength = 10)
            .AddNamedOption(s => s.VeryLongApplicationOptionValuePropertyName);

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(1);

        var option = parser.GetOptionByName(nameof(SampleOptionNames.VeryLongApplicationOptionValuePropertyName));
        var namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(4);
        namedOption.Aliases.ShouldContain("-v");
        namedOption.Aliases.ShouldContain("--very-long");
        namedOption.Aliases.ShouldContain("/v");
        namedOption.Aliases.ShouldContain("/very-long");
    }

    [Fact]
    public void ShouldSuggestAlias_WhenMoreThanThreeWordsOptionUsed2()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>()
            .ConfigureSettings(s => s.MaxSuggestedAliasWordCount = 3)
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.ValueTest)
            .AddNamedOption(s => s.ValueTestProp)
            .AddNamedOption(s => s.ValueTestPropEx);

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(4);

        var option = parser.GetOptionByName(nameof(SampleOptionNames.ValueTestPropEx));
        var namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(2);
        namedOption.Aliases.ShouldContain("-a");
        namedOption.Aliases.ShouldContain("/a");
    }

    [Fact]
    public void ShouldSuggestAlias_WhenOnlySingleDashShortAliasAllowed()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>(OptionPrefixRules.AllowSingleDashShortAlias)
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.ValueTest)
            .AddNamedOption(s => s.ValueTestProp);

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(3);

        var option = parser.GetOptionByName(nameof(SampleOptionNames.Value));
        var namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.ShouldContain("-v");
        namedOption.Aliases.ShouldHaveSingleItem();

        option = parser.GetOptionByName(nameof(SampleOptionNames.ValueTest));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.ShouldContain("-t");
        namedOption.Aliases.ShouldHaveSingleItem();

        option = parser.GetOptionByName(nameof(SampleOptionNames.ValueTestProp));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.ShouldContain("-p");
        namedOption.Aliases.ShouldHaveSingleItem();
    }

    [Fact]
    public void ShouldSuggestAlias_WhenOnlyDoubleDashLongAliasAllowed()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>(OptionPrefixRules.AllowDoubleDashLongAlias)
            .AddNamedOption(s => s.ABCValue)
            .AddNamedOption(s => s.ValueTest)
            .AddNamedOption(s => s.ValueTestProp);

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(3);

        var option = parser.GetOptionByName(nameof(SampleOptionNames.ABCValue));
        var namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.ShouldContain("--abc-value");
        namedOption.Aliases.ShouldHaveSingleItem();

        option = parser.GetOptionByName(nameof(SampleOptionNames.ValueTest));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.ShouldContain("--value-test");
        namedOption.Aliases.ShouldHaveSingleItem();

        option = parser.GetOptionByName(nameof(SampleOptionNames.ValueTestProp));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.ShouldContain("--value-test-prop");
        namedOption.Aliases.ShouldHaveSingleItem();
    }

    [Fact]
    public void ShouldSuggestAlias_WhenOnlyDoubleDashAllowed()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>(OptionPrefixRules.AllowDoubleDash)
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.ValueTest)
            .AddNamedOption(s => s.ValueTestProp);

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(3);

        var option = parser.GetOptionByName(nameof(SampleOptionNames.Value));
        var namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(2);
        namedOption.Aliases.ShouldContain("--v");
        namedOption.Aliases.ShouldContain("--value");

        option = parser.GetOptionByName(nameof(SampleOptionNames.ValueTest));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(2);
        namedOption.Aliases.ShouldContain("--t");
        namedOption.Aliases.ShouldContain("--value-test");

        option = parser.GetOptionByName(nameof(SampleOptionNames.ValueTestProp));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(2);
        namedOption.Aliases.ShouldContain("--p");
        namedOption.Aliases.ShouldContain("--value-test-prop");
    }

    [Fact]
    public void ShouldSuggestAlias_WhenOnlySingleDashAllowed()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>(OptionPrefixRules.AllowSingleDash)
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.ValueTest)
            .AddNamedOption(s => s.ValueTestProp);

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(3);

        var option = parser.GetOptionByName(nameof(SampleOptionNames.Value));
        var namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(2);
        namedOption.Aliases.ShouldContain("-v");
        namedOption.Aliases.ShouldContain("-value");

        option = parser.GetOptionByName(nameof(SampleOptionNames.ValueTest));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(2);
        namedOption.Aliases.ShouldContain("-t");
        namedOption.Aliases.ShouldContain("-value-test");

        option = parser.GetOptionByName(nameof(SampleOptionNames.ValueTestProp));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(2);
        namedOption.Aliases.ShouldContain("-p");
        namedOption.Aliases.ShouldContain("-value-test-prop");
    }

    [Fact]
    public void ShouldSuggestAlias_WhenOnlyForwardSlashAllowed()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>(OptionPrefixRules.AllowForwardSlash)
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.ValueTest)
            .AddNamedOption(s => s.ValueTestProp);

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(3);

        var option = parser.GetOptionByName(nameof(SampleOptionNames.Value));
        var namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(2);
        namedOption.Aliases.ShouldContain("/v");
        namedOption.Aliases.ShouldContain("/value");

        option = parser.GetOptionByName(nameof(SampleOptionNames.ValueTest));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(2);
        namedOption.Aliases.ShouldContain("/t");
        namedOption.Aliases.ShouldContain("/value-test");

        option = parser.GetOptionByName(nameof(SampleOptionNames.ValueTestProp));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(2);
        namedOption.Aliases.ShouldContain("/p");
        namedOption.Aliases.ShouldContain("/value-test-prop");
    }

    [Fact]
    public void ShouldSuggestAlias_WhenAllAliasPrefixesAllowed()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>(OptionPrefixRules.All)
            .ConfigureSettings(s => s.MaxSuggestedAliasWordCount = 3)
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.ValueTest)
            .AddNamedOption(s => s.ValueTestProp)
            .AddCounterOption(s => s.ValueTestPropEx);

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(4);

        var option = parser.GetOptionByName(nameof(SampleOptionNames.Value));
        var namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(6);
        namedOption.Aliases.ShouldContain("-v");
        namedOption.Aliases.ShouldContain("-value");
        namedOption.Aliases.ShouldContain("--v");
        namedOption.Aliases.ShouldContain("--value");
        namedOption.Aliases.ShouldContain("/v");
        namedOption.Aliases.ShouldContain("/value");

        option = parser.GetOptionByName(nameof(SampleOptionNames.ValueTest));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(6);
        namedOption.Aliases.ShouldContain("-t");
        namedOption.Aliases.ShouldContain("-value-test");
        namedOption.Aliases.ShouldContain("--t");
        namedOption.Aliases.ShouldContain("--value-test");
        namedOption.Aliases.ShouldContain("/t");
        namedOption.Aliases.ShouldContain("/value-test");

        option = parser.GetOptionByName(nameof(SampleOptionNames.ValueTestProp));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(6);
        namedOption.Aliases.ShouldContain("-p");
        namedOption.Aliases.ShouldContain("-value-test-prop");
        namedOption.Aliases.ShouldContain("--p");
        namedOption.Aliases.ShouldContain("--value-test-prop");
        namedOption.Aliases.ShouldContain("/p");
        namedOption.Aliases.ShouldContain("/value-test-prop");

        option = parser.GetOptionByName(nameof(SampleOptionNames.ValueTestPropEx));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();

        // all long aliases have already registered with other options, so long aliases are empty
        // because suggestion algorithm suggests the alias only from first three words of the property name 
        namedOption.Aliases.Count.ShouldBe(3);
        namedOption.Aliases.ShouldContain("-a");
        namedOption.Aliases.ShouldContain("--a");
        namedOption.Aliases.ShouldContain("/a");
    }

    [Fact]
    public void ShouldSuggestAlias_WhenCaseSensitiveIsTrue()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>(caseSensitive: true)
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.Val1);

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(2);

        var option = parser.GetOptionByName(nameof(SampleOptionNames.Value));
        var namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(4);
        namedOption.Aliases.ShouldContain("-V");
        namedOption.Aliases.ShouldContain("--Value");
        namedOption.Aliases.ShouldContain("/V");
        namedOption.Aliases.ShouldContain("/Value");

        option = parser.GetOptionByName(nameof(SampleOptionNames.Val1));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(4);
        namedOption.Aliases.ShouldContain("-a");
        namedOption.Aliases.ShouldContain("--Val1");
        namedOption.Aliases.ShouldContain("/a");
        namedOption.Aliases.ShouldContain("/Val1");
    }

    [Fact]
    public void ShouldSuggestAlias_WhenReservedAliasUsed()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>(caseSensitive: true)
            .AddNamedOption(s => s.__1_Help_1)
            .AddNamedOption(s => s.Val1);

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(2);

        var option = parser.GetOptionByName(nameof(SampleOptionNames.__1_Help_1));
        var namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(4);
        namedOption.Aliases.ShouldContain("-e");
        namedOption.Aliases.ShouldContain("/e");
        namedOption.Aliases.ShouldContain("--Help-1");
        namedOption.Aliases.ShouldContain("/Help-1");

        option = parser.GetOptionByName(nameof(SampleOptionNames.Val1));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(4);
        namedOption.Aliases.ShouldContain("-V");
        namedOption.Aliases.ShouldContain("--Val1");
        namedOption.Aliases.ShouldContain("/V");
        namedOption.Aliases.ShouldContain("/Val1");
    }

    [Fact]
    public void ShouldNotSuggestShortAlias_WhenAllLettersAreUsed()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>()
            .AddNamedOption(s => s.Val1)
            .AddNamedOption(s => s.Val2)
            .AddCounterOption(s => s.Val3)
            .AddCounterOption(s => s.Val4);

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(4);

        var option = parser.GetOptionByName(nameof(SampleOptionNames.Val1));
        var namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(4);
        namedOption.Aliases.ShouldContain("-v");
        namedOption.Aliases.ShouldContain("--val1");
        namedOption.Aliases.ShouldContain("/v");
        namedOption.Aliases.ShouldContain("/val1");

        option = parser.GetOptionByName(nameof(SampleOptionNames.Val2));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(4);
        namedOption.Aliases.ShouldContain("-a");
        namedOption.Aliases.ShouldContain("--val2");
        namedOption.Aliases.ShouldContain("/a");
        namedOption.Aliases.ShouldContain("/val2");

        option = parser.GetOptionByName(nameof(SampleOptionNames.Val3));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(4);
        namedOption.Aliases.ShouldContain("-l");
        namedOption.Aliases.ShouldContain("--val3");
        namedOption.Aliases.ShouldContain("/l");
        namedOption.Aliases.ShouldContain("/val3");

        option = parser.GetOptionByName(nameof(SampleOptionNames.Val4));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(2);
        namedOption.Aliases.ShouldContain("--val4");
        namedOption.Aliases.ShouldContain("/val4");
    }

    [Fact]
    public void ShouldThrowException_WhenAliasSuggestionFails1()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>()
            .AddNamedOption(s => s.Val1) // will suggest => v, val1
            .AddNamedOption(s => s.Val2) // will suggest => a, val2
            .AddNamedOption(s => s.Val3) // will suggest => l, val3
            .AddNamedOption(s => s.Val4) // will suggest => val4
            .AddNamedOption(s => s.Val4_); // will suggest nothing

        // Act
        // Can't suggest alias because all alias possible names have already been used 
        var exception = Should.Throw<OptionBuilderException>(sut.Build);
        var info = exception.Error;

        // Assert
        info.Error.Code.ShouldBe(BuilderErrors.UnableToSuggestAlias.Code);
        info.Values.ShouldHaveSingleItem();
        info.Values.ShouldContain(nameof(SampleOptionNames.Val4_));
        info.OptionName.ShouldBe(nameof(SampleOptionNames.Val4_));
        var message = string.Format(info.Error.Format, nameof(SampleOptionNames.Val4_));
        exception.Message.ShouldStartWith(message);
    }

    [Fact]
    public void ShouldThrowException_WhenAliasSuggestionFails2()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>(OptionPrefixRules.AllowSingleDashShortAlias)
            .AddNamedOption(s => s.Val1) // will suggest => v
            .AddNamedOption(s => s.Val2) // will suggest => a
            .AddNamedOption(s => s.Val3) // will suggest => l
            .AddNamedOption(s => s.Val4); // will suggest => nothing

        // Act
        // Can't suggest alias because all alias possible names have already been used 
        var exception = Should.Throw<OptionBuilderException>(sut.Build);
        var info = exception.Error;

        // Assert
        info.Error.Code.ShouldBe(BuilderErrors.UnableToSuggestAlias.Code);
        info.Values.ShouldHaveSingleItem();
        info.Values.ShouldContain(nameof(SampleOptionNames.Val4));
        info.OptionName.ShouldBe(nameof(SampleOptionNames.Val4));
        var message = string.Format(info.Error.Format, nameof(SampleOptionNames.Val4));
        exception.Message.ShouldStartWith(message);
    }

    [Fact]
    public void ShouldThrowException_WhenAliasSuggestionFails3()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>(OptionPrefixRules.AllowDoubleDashLongAlias)
            .AddNamedOption(s => s.Val4) // will suggest => val4
            .AddNamedOption(s => s.Val4_); // will suggest nothing

        // Act
        // Can't suggest alias because all alias possible names have already been used 
        var exception = Should.Throw<OptionBuilderException>(sut.Build);
        var info = exception.Error;

        // Assert
        info.Error.Code.ShouldBe(BuilderErrors.UnableToSuggestAlias.Code);
        info.Values.ShouldHaveSingleItem();
        info.Values.ShouldContain(nameof(SampleOptionNames.Val4_));
        info.OptionName.ShouldBe(nameof(SampleOptionNames.Val4_));
        var message = string.Format(info.Error.Format, nameof(SampleOptionNames.Val4_));
        exception.Message.ShouldStartWith(message);
    }

    [Fact]
    public void ShouldThrowException_WhenAliasSuggestionFails4()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>()
            .AddNamedOption(s => s.Value) // will suggest => v, value
            .AddNamedOption(s => s.V); // will suggest nothing

        // Act
        // Can't suggest alias because all alias possible names have already been used 
        var exception = Should.Throw<OptionBuilderException>(sut.Build);
        var info = exception.Error;

        // Assert
        info.Error.Code.ShouldBe(BuilderErrors.UnableToSuggestAlias.Code);
        info.Values.ShouldHaveSingleItem();
        info.Values.ShouldContain(nameof(SampleOptionNames.V));
        info.OptionName.ShouldBe(nameof(SampleOptionNames.V));
        var message = string.Format(info.Error.Format, nameof(SampleOptionNames.V));
        exception.Message.ShouldStartWith(message);
    }

    [Fact]
    public void ShouldSuggestAlias_WhenNumbersAreUsed()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>()
            .AddNamedOption(s => s.__1_Help, o => o.WithName("Test"))
            .AddNamedOption(s => s.__2_Value)
            .AddNamedOption(s => s.__3_Va1_l2_3ue);

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(3);

        var option = parser.GetOptionByName(nameof(SampleOptionNames.__1_Help));
        var namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(2);
        namedOption.Aliases.ShouldContain("-e");
        namedOption.Aliases.ShouldContain("/e");

        option = parser.GetOptionByName(nameof(SampleOptionNames.__2_Value));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(4);
        namedOption.Aliases.ShouldContain("-v");
        namedOption.Aliases.ShouldContain("--value");
        namedOption.Aliases.ShouldContain("/v");
        namedOption.Aliases.ShouldContain("/value");

        option = parser.GetOptionByName(nameof(SampleOptionNames.__3_Va1_l2_3ue));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(4);
        namedOption.Aliases.ShouldContain("-l");
        namedOption.Aliases.ShouldContain("--va1-l2-3-ue");
        namedOption.Aliases.ShouldContain("/l");
        namedOption.Aliases.ShouldContain("/va1-l2-3-ue");
    }
}