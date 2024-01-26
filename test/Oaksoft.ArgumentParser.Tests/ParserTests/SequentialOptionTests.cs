using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Tests.TestModels;
using Shouldly;

namespace Oaksoft.ArgumentParser.Tests.ParserTests;

public class SequentialOptionTests : ArgumentParserTestBase
{
    [Fact]
    public void ParseSequentialOption_WhenHelpInputValid()
    {
        // Arrange
        var sut = CommandLine.CreateParser<ValueOptions>()
            .AddNamedOption(o => o.HashSet2ValueItems, o => o.WithTryParseCallback(PointTryParseCallback.TryParse))
            .AutoBuild();

        // Act
        var result = sut.Parse("-h");
        var text = sut.GetHelpText(false);

        // Assert
        sut.IsValid.ShouldBeTrue();
        sut.IsHelpOption.ShouldBeTrue();
        result.ArrayValueItems.ShouldBeNull();
        text.ShouldNotBeEmpty();
    }

    [Fact]
    public void ParseSequentialOption_WhenListArgumentsValid()
    {
        // Arrange
        var sut = CommandLine.CreateParser<ValueOptions>(valueDelimiter: ValueDelimiterRules.AllowPipeSymbol)
            .AddNamedOption(o => o.HashSet2ValueItems, o => o.WithTryParseCallback(PointTryParseCallback.TryParse))
            .AutoBuild();

        // Act
        var result = sut.Parse("-e", "1|3|2", "-l:1|3|2", "-a:1|3|2", "-t:1|3|2", "-r:1|3|2");

        // Assert
        sut.IsValid.ShouldBeTrue();
        result.EnumerableValueItems.ShouldBe(new List<int> { 1,3,2 });
        result.ListValueItems.ShouldBe(new List<double> { 1, 3, 2 });
        result.List1ValueItems.ShouldBe(new List<int> { 1, 3, 2 });
        result.List2ValueItems.ShouldBe(new List<float> { 1, 3, 2 });
        result.ArrayValueItems.ShouldBe(new [] { 1, 3, 2 });
    }

    [Fact]
    public void ParseSequentialOption_WhenCollectionArgumentsValid()
    {
        // Arrange
        var sut = CommandLine.CreateParser<ValueOptions>(valueDelimiter: ValueDelimiterRules.AllowPipeSymbol)
            .AddNamedOption(o => o.HashSet2ValueItems, o => o.WithTryParseCallback(PointTryParseCallback.TryParse))
            .AutoBuild();

        // Act
        var result = sut.Parse("-c", "1|3|2", "-v:1|3|2", "-i:1|3|2");

        // Assert
        sut.IsValid.ShouldBeTrue();
        result.CollectionValueItems.ShouldBe(new List<int> { 1, 3, 2 });
        result.Collection1ValueItems.ShouldBe(new List<short> { 1, 3, 2 });
        result.Collection2ValueItems.ShouldBe(new List<long> { 1, 3, 2 });
    }

    [Fact]
    public void ParseSequentialOption_WhenHashSetArgumentsValid()
    {
        // Arrange
        var sut = CommandLine.CreateParser<ValueOptions>(valueDelimiter: ValueDelimiterRules.AllowPipeSymbol)
            .AddNamedOption(o => o.HashSet2ValueItems, o => o.WithTryParseCallback(PointTryParseCallback.TryParse))
            .AutoBuild();

        // Act
        var result = sut.Parse("-s", "(1;2)|(3;4)|(1;2)", "-u:abc|def", "-m:sdf|fdr");

        // Assert
        sut.IsValid.ShouldBeTrue();
        result.HashSet2ValueItems.ShouldBe(new HashSet<Coordinate> { new(1, 2), new(3, 4) }!);
        result.HashSetValueItems.ShouldBe(new HashSet<string> { "abc", "def"});
        result.HashSet1ValueItems.ShouldBe(new HashSet<string> { "sdf", "fdr" });
    }


    [Fact]
    public void ParseSequentialOption_WhenNullableHelpInputValid()
    {
        // Arrange
        var sut = CommandLine.CreateParser<NullableValueOptions>()
            .AddNamedOption(o => o.HashSet2NullableItems, o => o.WithTryParseCallback(PointTryParseCallback.TryParse))
            .AutoBuild();

        // Act
        var result = sut.Parse("-h");
        var text = sut.GetHelpText(false);

        // Assert
        sut.IsValid.ShouldBeTrue();
        sut.IsHelpOption.ShouldBeTrue();
        result.ArrayNullableItems.ShouldBeNull();
        text.ShouldNotBeEmpty();
    }

    [Fact]
    public void ParseSequentialOption_WhenNullableListArgumentsValid()
    {
        // Arrange
        var sut = CommandLine.CreateParser<NullableValueOptions>(valueDelimiter: ValueDelimiterRules.AllowPipeSymbol)
            .AddNamedOption(o => o.HashSet2NullableItems, o => o.WithTryParseCallback(PointTryParseCallback.TryParse))
            .AutoBuild();

        // Act
        var result = sut.Parse("-e", "1|3|2", "-l:1|3|2", "-u:1|3|2", "-t:1|3|2", "-a:1|3|2");

        // Assert
        sut.IsValid.ShouldBeTrue();
        result.EnumerableNullableItems.ShouldBe(new List<int?> { 1, 3, 2 });
        result.ListNullableItems.ShouldBe(new List<double?> { 1, 3, 2 });
        result.List1NullableItems.ShouldBe(new List<int?> { 1, 3, 2 });
        result.List2NullableItems.ShouldBe(new List<float?> { 1, 3, 2 });
        result.ArrayNullableItems.ShouldBe(new int?[] { 1, 3, 2 });
    }

    [Fact]
    public void ParseSequentialOption_WhenNullableCollectionArgumentsValid()
    {
        // Arrange
        var sut = CommandLine.CreateParser<NullableValueOptions>(valueDelimiter: ValueDelimiterRules.AllowPipeSymbol)
            .AddNamedOption(o => o.HashSet2NullableItems, o => o.WithTryParseCallback(PointTryParseCallback.TryParse))
            .AutoBuild();

        // Act
        var result = sut.Parse("-c", "1|3|2", "-n:1|3|2", "-i:1|3|2");

        // Assert
        sut.IsValid.ShouldBeTrue();
        result.CollectionNullableItems.ShouldBe(new List<int?> { 1, 3, 2 });
        result.Collection1NullableItems.ShouldBe(new List<short?> { 1, 3, 2 });
        result.Collection2NullableItems.ShouldBe(new List<long?> { 1, 3, 2 });
    }

    [Fact]
    public void ParseSequentialOption_WhenNullableHashSetArgumentsValid()
    {
        // Arrange
        var sut = CommandLine.CreateParser<NullableValueOptions>(valueDelimiter: ValueDelimiterRules.AllowPipeSymbol)
            .AddNamedOption(o => o.HashSet2NullableItems, o => o.WithTryParseCallback(PointTryParseCallback.TryParse))
            .AutoBuild();

        // Act
        var result = sut.Parse("-s", "(1;2)|(3;4)|(1;2)", "-m:abc|def", "-b:sdf|fdr");

        // Assert
        sut.IsValid.ShouldBeTrue();
        result.HashSet2NullableItems.ShouldBe(new HashSet<Coordinate?> { new(1, 2), new(3, 4) }!);
        result.HashSetNullableItems.ShouldBe(new HashSet<string?> { "abc", "def" });
        result.HashSet1NullableItems.ShouldBe(new HashSet<string?> { "sdf", "fdr" });
    }
}