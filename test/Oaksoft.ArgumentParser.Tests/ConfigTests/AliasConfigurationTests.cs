using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Tests.AppModels;
using Shouldly;

namespace Oaksoft.ArgumentParser.Tests.ConfigTests;

public class AliasConfigurationTests
{
    [Fact]
    public void ShouldBuild_WhenOnlyLettersUsedInAliases()
    {
        // Arrange
        var sut = CommandLine.CreateParser<StringAppOptions>()
            .AddNamedOption(s => s.Value, o => o.AddAliases("v", "value"))
            .AddNamedOption(s => s.NullValues, o => o.AddAliases("n", "null-value"))
            .AddSwitchOption(s => s.NullValueFlag, o => o.AddAliases("f", "null-value-flag"));

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
        parser.GetOptions().Count.ShouldBe(4);

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
            .AddNamedOption(s => s.Value, o => o.AddAliases("v", "v"))
            .AddNamedOption(s => s.NullValues, o => o.AddAliases("value", "value", "Value"))
            .AddSwitchOption(s => s.NullValueFlag, o => o.AddAliases("test", "Test", "TEST", "TEST"));

        // Act
        var parser = sut.Build();

        // Assert
        parser.GetOptions().Count.ShouldBe(4);

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
            .AddSwitchOption(s => s.NullValueFlag, o => o.AddAliases("f", "--null -  value  --- flag--"));

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
    }

    [Theory]
    [InlineData("Test.Property")]
    [InlineData("---")]
    [InlineData("TestProperty!")]
    public void ShouldThrowException_WhenInvalidSymbolUsed1(string alias)
    {
        // Arrange
        var sut = CommandLine.CreateParser<StringAppOptions>();

        // Act & Assert
        Should.Throw<Exception>(() => sut.AddNamedOption(s => s.Value, a => a.AddAliases(alias)))
            .Message.ShouldStartWith($"Invalid alias '{alias}' found!");
    }

    [Fact]
    public void ShouldThrowException_WhenEmptyAliasUsed()
    {
        // Arrange
        var sut = CommandLine.CreateParser<StringAppOptions>();

        // Act & Assert
        Should.Throw<Exception>(() => sut.AddNamedOption(s => s.Value, a => a.AddAliases("  ")))
            .Message.ShouldStartWith("The alias string cannot be empty!");
    }

    [Fact]
    public void ShouldThrowException_WhenReservedAliasUsed()
    {
        // Arrange
        var sut = CommandLine.CreateParser<StringAppOptions>();

        // Act & Assert
        Should.Throw<Exception>(() => sut.AddNamedOption(s => s.Value, a => a.AddAliases("--help ")))
            .Message.ShouldStartWith("Invalid alias 'help' found! Reserved aliases ('h', '?', 'help') cannot be used.");
    }

    [Fact]
    public void ShouldThrowException_WhenLongAliasUsed_WithCustomWordLength()
    {
        // Arrange
        var alias = "Test-Property";
        var validAlias = alias.ToLowerInvariant();
        var sut = CommandLine.CreateParser<StringAppOptions>()
            .ConfigureSettings(s => s.MaxAliasLength = 10)
            .AddNamedOption(s => s.Value, a => a.AddAliases(alias));

        // Act & Assert
        Should.Throw<Exception>(() => sut.Build())
            .Message.ShouldStartWith($"Option alias '{validAlias}' not allowed! Allowed max alias length is 10.");
    }

    [Fact]
    public void ShouldThrowException_WhenLongAliasUsed_WithDefaultAliasLength()
    {
        // Arrange
        var alias = "This is Very Long Application Alias Name";
        var validAlias = alias.Replace(' ', '-').ToLowerInvariant();
        var sut = CommandLine.CreateParser<StringAppOptions>()
            .AddNamedOption(s => s.Value, a => a.AddAliases("This is Very Long Application Alias Name"));

        // Act & Assert
        Should.Throw<Exception>(() => sut.Build())
            .Message.ShouldStartWith($"Option alias '{validAlias}' not allowed! Allowed max alias length is 32.");
    }

    [Fact]
    public void ShouldSuggestAlias_WhenSameCustomAliasUsed1()
    {
        // Arrange
        var sut = CommandLine.CreateParser<StringAppOptions>()
            .AddCountOption(s => s.NullValueCount)
            .AddNamedOption(s => s.Value, o => o.AddAliases("v", "value"))
            .AddNamedOption(s => s.NullValue, o => o.AddAliases("n", "null-value-count"));

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
        parser.GetOptions().Count.ShouldBe(4);

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
        parser.GetOptions().Count.ShouldBe(4);

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
        parser.GetOptions().Count.ShouldBe(4);

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
    public void ShouldThrowException_WhenSameCustomAliasUsed()
    {
        // Arrange
        var sut = CommandLine.CreateParser<StringAppOptions>()
            .AddNamedOption(s => s.Value, o => o.AddAliases("v"))
            .AddNamedOption(s => s.NullValues, o => o.AddAliases("v"));

        // Act & Assert
        Should.Throw<Exception>(() => sut.Build())
            .Message.ShouldStartWith("Alias 'v' is already in use!");
    }

    [Fact]
    public void ShouldThrowException_WhenShortAliasNotAllowed()
    {
        // Arrange
        var sut = CommandLine.CreateParser<StringAppOptions>(OptionPrefixRules.AllowDoubleDashLongAlias)
            .AddNamedOption(s => s.Value, o => o.AddAliases("v", "test"));

        // Act & Assert
        Should.Throw<Exception>(() => sut.Build())
            .Message.ShouldStartWith("Short option alias 'v' not allowed!");
    }

    [Fact]
    public void ShouldThrowException_WhenLongAliasNotAllowed()
    {
        // Arrange
        var sut = CommandLine.CreateParser<StringAppOptions>(OptionPrefixRules.AllowSingleDashShortAlias)
            .AddNamedOption(s => s.Value, o => o.AddAliases("v", "test"));

        // Act & Assert
        Should.Throw<Exception>(() => sut.Build())
            .Message.ShouldStartWith("Long option alias 'test' not allowed!");
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
        parser.GetOptions().Count.ShouldBe(3);

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
        parser.GetOptions().Count.ShouldBe(4);

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
        Should.Throw<Exception>(() => namedOption.AddAliases("s"))
            .Message.ShouldStartWith("An option cannot be modified after");
    }
}