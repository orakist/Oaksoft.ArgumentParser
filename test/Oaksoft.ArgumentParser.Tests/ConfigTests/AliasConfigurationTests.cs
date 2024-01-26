using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Errors.Builder;
using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Tests.TestModels;
using Shouldly;

namespace Oaksoft.ArgumentParser.Tests.ConfigTests;

public class AliasConfigurationTests : ArgumentParserTestBase
{
    [Fact]
    public void ShouldBuild_WhenOnlyLettersUsedInAliases()
    {
        // Arrange
        var sut = CommandLine.CreateParser<StringAppOptions>()
            .AddNamedOption(s => s.Value, o => o.AddAliases("v", "value"))
            .AddNamedOption(s => s.NullValues, o => o.AddAliases("n", "null-value"))
            .AddSwitchOption(s => s.NullValueFlag, o => o.AddAliases("f", "null-value-flag"))
            .AddCounterOption(s => s.ValueCount, o => o.AddAliases("c", "value-count"));

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(4);

        var option = parser.GetOptionByName(nameof(StringAppOptions.Value));
        var namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(4);
        namedOption.Aliases.ShouldContain("-v");
        namedOption.Aliases.ShouldContain("--value");
        namedOption.Aliases.ShouldContain("/v");
        namedOption.Aliases.ShouldContain("/value");

        option = parser.GetOptionByName(nameof(StringAppOptions.NullValues));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(4);
        namedOption.Aliases.ShouldContain("-n");
        namedOption.Aliases.ShouldContain("--null-value");
        namedOption.Aliases.ShouldContain("/n");
        namedOption.Aliases.ShouldContain("/null-value");

        option = parser.GetOptionByName(nameof(StringAppOptions.NullValueFlag));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(4);
        namedOption.Aliases.ShouldContain("-f");
        namedOption.Aliases.ShouldContain("--null-value-flag");
        namedOption.Aliases.ShouldContain("/f");
        namedOption.Aliases.ShouldContain("/null-value-flag");

        option = parser.GetOptionByName(nameof(StringAppOptions.ValueCount));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(4);
        namedOption.Aliases.ShouldContain("-c");
        namedOption.Aliases.ShouldContain("--value-count");
        namedOption.Aliases.ShouldContain("/c");
        namedOption.Aliases.ShouldContain("/value-count");
    }

    [Fact]
    public void ShouldBuild_WhenDuplicateAliasUsedWithSameOption()
    {
        // Arrange
        var sut = CommandLine.CreateParser<StringAppOptions>()
            .AddNamedOption(s => s.Value, o => o.AddAliases("v", "v"))
            .AddNamedOption(s => s.NullValues, o => o.AddAliases("value", "value"))
            .AddSwitchOption(s => s.NullValueFlag, o => o.AddAliases("test", "tesT"));

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(3);

        var option = parser.GetOptionByName(nameof(StringAppOptions.Value));
        var namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(2);
        namedOption.Aliases.ShouldContain("-v");
        namedOption.Aliases.ShouldContain("/v");

        option = parser.GetOptionByName(nameof(StringAppOptions.NullValues));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(2);
        namedOption.Aliases.ShouldContain("--value");
        namedOption.Aliases.ShouldContain("/value");

        option = parser.GetOptionByName(nameof(StringAppOptions.NullValueFlag));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(2);
        namedOption.Aliases.ShouldContain("--test");
        namedOption.Aliases.ShouldContain("/test");
    }

    [Fact]
    public void ShouldBuild_WhenDuplicateAliasUsedWithSameOptionAndCaseSensitive()
    {
        // Arrange
        var sut = CommandLine.CreateParser<StringAppOptions>(caseSensitive: true)
            .AddNamedOption(s => s.Value, o => o.AddAliases("v", "/v"))
            .AddNamedOption(s => s.NullValues, o => o.AddAliases("value", "/value", "Value"))
            .AddSwitchOption(s => s.NullValueFlag, o => o.AddAliases("test", "Test", "TEST", "TEST"));

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(3);

        var option = parser.GetOptionByName(nameof(StringAppOptions.Value));
        var namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(2);
        namedOption.Aliases.ShouldContain("-v");
        namedOption.Aliases.ShouldContain("/v");

        option = parser.GetOptionByName(nameof(StringAppOptions.NullValues));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(4);
        namedOption.Aliases.ShouldContain("--value");
        namedOption.Aliases.ShouldContain("/value");
        namedOption.Aliases.ShouldContain("--Value");
        namedOption.Aliases.ShouldContain("/Value");

        option = parser.GetOptionByName(nameof(StringAppOptions.NullValueFlag));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(6);
        namedOption.Aliases.ShouldContain("--test");
        namedOption.Aliases.ShouldContain("/test");
        namedOption.Aliases.ShouldContain("--Test");
        namedOption.Aliases.ShouldContain("/Test");
        namedOption.Aliases.ShouldContain("--TEST");
        namedOption.Aliases.ShouldContain("/TEST");
    }

    [Fact]
    public void ShouldBuildAndCleanAliases_WhenWhitespacesUsed()
    {
        // Arrange
        var sut = CommandLine.CreateParser<StringAppOptions>()
            .AddNamedOption(s => s.Value, o => o.AddAliases("v", "-- value"))
            .AddNamedOption(s => s.NullValues, o => o.AddAliases("n", "   null value  "))
            .AddCounterOption(s => s.NullValueCount, o => o.AddAliases("f", "--null -  value  --- flag--"));

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(3);

        var option = parser.GetOptionByName(nameof(StringAppOptions.Value));
        var namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(4);
        namedOption.Aliases.ShouldContain("-v");
        namedOption.Aliases.ShouldContain("--value");
        namedOption.Aliases.ShouldContain("/v");
        namedOption.Aliases.ShouldContain("/value");

        option = parser.GetOptionByName(nameof(StringAppOptions.NullValues));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(4);
        namedOption.Aliases.ShouldContain("-n");
        namedOption.Aliases.ShouldContain("--null-value");
        namedOption.Aliases.ShouldContain("/n");
        namedOption.Aliases.ShouldContain("/null-value");

        option = parser.GetOptionByName(nameof(StringAppOptions.NullValueCount));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(4);
        namedOption.Aliases.ShouldContain("-f");
        namedOption.Aliases.ShouldContain("--null-value-flag");
        namedOption.Aliases.ShouldContain("/f");
        namedOption.Aliases.ShouldContain("/null-value-flag");
    }

    [Fact]
    public void ShouldSuggestAlias_WhenSameCustomAliasUsed1()
    {
        // Arrange
        var sut = CommandLine.CreateParser<StringAppOptions>()
            .AddNamedOption(s => s.NullValueCount)
            .AddNamedOption(s => s.Value, o => o.AddAliases("v", "value"))
            .AddNamedOption(s => s.NullValue, o => o.AddAliases("n", "null-value-count"));

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(3);

        var option = parser.GetOptionByName(nameof(StringAppOptions.Value));
        var namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(4);
        namedOption.Aliases.ShouldContain("-v");
        namedOption.Aliases.ShouldContain("--value");
        namedOption.Aliases.ShouldContain("/v");
        namedOption.Aliases.ShouldContain("/value");

        option = parser.GetOptionByName(nameof(StringAppOptions.NullValue));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(4);
        namedOption.Aliases.ShouldContain("-n");
        namedOption.Aliases.ShouldContain("--null-value-count");
        namedOption.Aliases.ShouldContain("/n");
        namedOption.Aliases.ShouldContain("/null-value-count");

        option = parser.GetOptionByName(nameof(StringAppOptions.NullValueCount));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(4);
        namedOption.Aliases.ShouldContain("-c");
        namedOption.Aliases.ShouldContain("--null-value");
        namedOption.Aliases.ShouldContain("/c");
        namedOption.Aliases.ShouldContain("/null-value");
    }

    [Fact]
    public void ShouldSuggestAlias_WhenSameCustomAliasUsed2()
    {
        // Arrange
        var sut = CommandLine.CreateParser<StringAppOptions>()
            .AddNamedOption(s => s.NullValueCount, o => o.AddAliases("v", "value"))
            .AddNamedOption(s => s.NullValue, o => o.AddAliases("a", "null-value-count"))
            .AddNamedOption(s => s.Value); // should only suggest "l"

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(3);

        var option = parser.GetOptionByName(nameof(StringAppOptions.Value));
        var namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(2);
        namedOption.Aliases.ShouldContain("-l");
        namedOption.Aliases.ShouldContain("/l");

        option = parser.GetOptionByName(nameof(StringAppOptions.NullValue));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(4);
        namedOption.Aliases.ShouldContain("-a");
        namedOption.Aliases.ShouldContain("--null-value-count");
        namedOption.Aliases.ShouldContain("/a");
        namedOption.Aliases.ShouldContain("/null-value-count");

        option = parser.GetOptionByName(nameof(StringAppOptions.NullValueCount));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(4);
        namedOption.Aliases.ShouldContain("-v");
        namedOption.Aliases.ShouldContain("--value");
        namedOption.Aliases.ShouldContain("/v");
        namedOption.Aliases.ShouldContain("/value");
    }

    [Fact]
    public void ShouldBuild_WhenOnlySingleDashShortAliasAllowed()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>(OptionPrefixRules.AllowSingleDashShortAlias)
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.ValueTest, o => o.AddAliases("a"))
            .AddNamedOption(s => s.ValueTestProp, o => o.AddAliases("v"));

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(3);

        var option = parser.GetOptionByName(nameof(SampleOptionNames.Value));
        var namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.ShouldContain("-l");
        namedOption.Aliases.ShouldHaveSingleItem();

        option = parser.GetOptionByName(nameof(SampleOptionNames.ValueTest));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.ShouldContain("-a");
        namedOption.Aliases.ShouldHaveSingleItem();

        option = parser.GetOptionByName(nameof(SampleOptionNames.ValueTestProp));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.ShouldContain("-v");
        namedOption.Aliases.ShouldHaveSingleItem();
    }

    [Fact]
    public void ShouldBuild_WhenOnlyDoubleDashLongAliasAllowed()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>(OptionPrefixRules.AllowDoubleDashLongAlias)
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.ValueTest, o => o.AddAliases("test1"))
            .AddNamedOption(s => s.ValueTestProp, o => o.AddAliases("test2"));

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(3);

        var option = parser.GetOptionByName(nameof(SampleOptionNames.Value));
        var namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.ShouldContain("--value");
        namedOption.Aliases.ShouldHaveSingleItem();

        option = parser.GetOptionByName(nameof(SampleOptionNames.ValueTest));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.ShouldContain("--test1");
        namedOption.Aliases.ShouldHaveSingleItem();

        option = parser.GetOptionByName(nameof(SampleOptionNames.ValueTestProp));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.ShouldContain("--test2");
        namedOption.Aliases.ShouldHaveSingleItem();
    }

    [Fact]
    public void ShouldBuild_WhenAllAliasPrefixesAllowed()
    {
        // Arrange
        var sut = CommandLine.CreateParser<StringAppOptions>(OptionPrefixRules.All)
            .AddNamedOption(s => s.Value, o => o.AddAliases("a", "b"))
            .AddNamedOption(s => s.ValueFlag, o => o.AddAliases("test", "alias"));

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(2);

        var option = parser.GetOptionByName(nameof(StringAppOptions.Value));
        var namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(6);
        namedOption.Aliases.ShouldContain("-a");
        namedOption.Aliases.ShouldContain("-b");
        namedOption.Aliases.ShouldContain("--a");
        namedOption.Aliases.ShouldContain("--b");
        namedOption.Aliases.ShouldContain("/a");
        namedOption.Aliases.ShouldContain("/b");

        option = parser.GetOptionByName(nameof(StringAppOptions.ValueFlag));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(6);
        namedOption.Aliases.ShouldContain("-test");
        namedOption.Aliases.ShouldContain("-alias");
        namedOption.Aliases.ShouldContain("--test");
        namedOption.Aliases.ShouldContain("--alias");
        namedOption.Aliases.ShouldContain("/test");
        namedOption.Aliases.ShouldContain("/alias");
    }

    [Fact]
    public void ShouldBuild_WhenCaseSensitiveIsTrue()
    {
        // Arrange
        var sut = CommandLine.CreateParser<StringAppOptions>(caseSensitive: true)
            .AddNamedOption(s => s.Value, o => o.AddAliases("v", "a", "A"))
            .AddSwitchOption(s => s.ValueFlag, o => o.AddAliases("test", "TeSt"))
            .AddNamedOption(s => s.Values);

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(3);

        var option = parser.GetOptionByName(nameof(StringAppOptions.Value));
        var namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(6);
        namedOption.Aliases.ShouldContain("-v");
        namedOption.Aliases.ShouldContain("-a");
        namedOption.Aliases.ShouldContain("-A");
        namedOption.Aliases.ShouldContain("/v");
        namedOption.Aliases.ShouldContain("/a");
        namedOption.Aliases.ShouldContain("/A");

        option = parser.GetOptionByName(nameof(StringAppOptions.ValueFlag));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(4);
        namedOption.Aliases.ShouldContain("--test");
        namedOption.Aliases.ShouldContain("--TeSt");
        namedOption.Aliases.ShouldContain("/test");
        namedOption.Aliases.ShouldContain("/TeSt");

        option = parser.GetOptionByName(nameof(StringAppOptions.Values));
        namedOption = option as INamedOption;
        namedOption.ShouldNotBeNull();
        namedOption.Aliases.Count.ShouldBe(4);
        namedOption.Aliases.ShouldContain("-V");
        namedOption.Aliases.ShouldContain("--Values");
        namedOption.Aliases.ShouldContain("/V");
        namedOption.Aliases.ShouldContain("/Values");
    }

    [Theory]
    [InlineData("Test#Property")]
    [InlineData("---")]
    [InlineData("TestProperty!")]
    public void ShouldThrowException_WhenInvalidSymbolUsed(string alias)
    {
        // Arrange
        const string symbols = "'?', '.', '-'";
        var sut = CommandLine.CreateParser<IntAppOptions>();

        // Act
        var exception = Should.Throw<OptionBuilderException>(
            () => sut.AddNamedOption(s => s.Value, a => a.AddAliases(alias)));
        var info = exception.Error;

        // Assert
        info.Error.Code.ShouldBe(BuilderErrors.InvalidAlias.Code);
        info.Values.ShouldNotBeEmpty();
        info.Values.ShouldContain(alias);
        info.Values.ShouldContain(symbols);
        info.OptionName.ShouldBe(nameof(IntAppOptions.Value));
        var message = string.Format(info.Error.Format, alias, symbols);
        exception.Message.ShouldStartWith(message);
    }

    [Fact]
    public void ShouldThrowException_WhenEmptyAliasUsed()
    {
        // Arrange
        const string value = "alias";
        var sut = CommandLine.CreateParser<IntAppOptions>();

        // Act
        var exception = Should.Throw<OptionBuilderException>(
            () => sut.AddNamedOption(s => s.Value, a => a.AddAliases("  ")));
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
    public void ShouldThrowException_WhenReservedAliasUsed()
    {
        // Arrange
        const string alias = "help";
        var sut = CommandLine.CreateParser<IntAppOptions>();

        // Act
        var exception = Should.Throw<OptionBuilderException>(
            () => sut.AddNamedOption(s => s.Value, a => a.AddAliases(alias)));
        var info = exception.Error;

        // Assert
        info.Error.Code.ShouldBe(BuilderErrors.ReservedAlias.Code);
        info.Values.ShouldNotBeEmpty();
        info.Values.ShouldContain(alias);
        info.OptionName.ShouldBe(nameof(IntAppOptions.Value));
    }

    [Fact]
    public void ShouldThrowException_WhenLongAliasUsed_WithCustomWordLength()
    {
        // Arrange
        const int length = 10;
        const string alias = "Test-Property";
        var sut = CommandLine.CreateParser<IntAppOptions>(caseSensitive: true)
            .ConfigureSettings(s => s.MaxAliasLength = length)
            .AddNamedOption(s => s.Value, a => a.AddAliases(alias));

        // Act
        var exception = Should.Throw<OptionBuilderException>(sut.Build);
        var info = exception.Error;

        // Assert
        info.Error.Code.ShouldBe(BuilderErrors.TooLongAlias.Code);
        info.Values.ShouldNotBeEmpty();
        info.Values.ShouldContain(alias);
        info.Values.ShouldContain(length);
        info.OptionName.ShouldBe(nameof(IntAppOptions.Value));
        var message = string.Format(info.Error.Format, alias, length);
        exception.Message.ShouldStartWith(message);
    }

    [Fact]
    public void ShouldThrowException_WhenLongAliasUsed_WithDefaultAliasLength()
    {
        // Arrange
        const string alias = "This is Very Long Application Alias Name";
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value, a => a.AddAliases(alias));

        // Act
        var validAlias = alias.Replace(' ', '-').ToLowerInvariant();
        var exception = Should.Throw<OptionBuilderException>(sut.Build);
        var info = exception.Error;

        // Assert
        info.Error.Code.ShouldBe(BuilderErrors.TooLongAlias.Code);
        info.Values.ShouldNotBeEmpty();
        info.Values.ShouldContain(validAlias);
        info.Values.ShouldContain(32);
        info.OptionName.ShouldBe(nameof(IntAppOptions.Value));
        var message = string.Format(info.Error.Format, validAlias, 32);
        exception.Message.ShouldStartWith(message);
    }

    [Fact]
    public void ShouldThrowException_WhenSameCustomAliasUsed()
    {
        // Arrange
        const string alias = "same";
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value, o => o.AddAliases(alias))
            .AddNamedOption(s => s.NullValues, o => o.AddAliases(alias));

        // Act
        var exception = Should.Throw<OptionBuilderException>(sut.Build);
        var info = exception.Error;

        // Assert
        info.Error.Code.ShouldBe(BuilderErrors.AliasAlreadyInUse.Code);
        info.Values.ShouldHaveSingleItem();
        info.Values.ShouldContain(alias);
        info.OptionName.ShouldBe(nameof(IntAppOptions.NullValues));
        var message = string.Format(info.Error.Format, alias);
        exception.Message.ShouldStartWith(message);
    }

    [Fact]
    public void ShouldThrowException_WhenShortAliasNotAllowed()
    {
        // Arrange
        string[] aliases = { "t", "test" };
        var sut = CommandLine.CreateParser<StringAppOptions>(OptionPrefixRules.AllowDoubleDashLongAlias)
            .AddNamedOption(s => s.Value, o => o.AddAliases(aliases));

        // Act
        var exception = Should.Throw<OptionBuilderException>(sut.Build);
        var info = exception.Error;

        // Assert
        info.Error.Code.ShouldBe(BuilderErrors.NotAllowedAlias.Code);
        info.Values.ShouldNotBeEmpty();
        info.Values.ShouldContain("Short");
        info.Values.ShouldContain(aliases[0]);
        info.OptionName.ShouldBe(nameof(IntAppOptions.Value));
        var message = string.Format(info.Error.Format, "Short", aliases[0]);
        exception.Message.ShouldStartWith(message);
    }

    [Fact]
    public void ShouldThrowException_WhenLongAliasNotAllowed()
    {
        // Arrange
        string[] aliases = { "t", "test" };
        var sut = CommandLine.CreateParser<StringAppOptions>(OptionPrefixRules.AllowSingleDashShortAlias)
            .AddNamedOption(s => s.Value, o => o.AddAliases(aliases));

        // Act
        var exception = Should.Throw<OptionBuilderException>(sut.Build);
        var info = exception.Error;

        // Assert
        info.Error.Code.ShouldBe(BuilderErrors.NotAllowedAlias.Code);
        info.Values.ShouldNotBeEmpty();
        info.Values.ShouldContain("Long");
        info.Values.ShouldContain(aliases[1]);
        info.OptionName.ShouldBe(nameof(IntAppOptions.Value));
        var message = string.Format(info.Error.Format, "Long", aliases[1]);
        exception.Message.ShouldStartWith(message);
    }

    [Fact]
    public void ShouldThrowException_WhenTryToUpdateAliasAfterBuild()
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
        var exception = Should.Throw<OptionBuilderException>(() => namedOption.AddAliases("s"));
        var info = exception.Error;

        info.Error.Code.ShouldBe(BuilderErrors.CannotBeModified.Code);
        info.Values.ShouldBeNull();
        info.OptionName.ShouldBe(nameof(IntAppOptions.Value));
    }
}