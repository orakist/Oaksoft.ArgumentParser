using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Tests.TestModels;
using Shouldly;

namespace Oaksoft.ArgumentParser.Tests.ParserTests;

public class AllowedValuesParsingTests : ArgumentParserTestBase
{
    private readonly string[] _allowedValues = { "Cat", "Puppy", "Pussy", "Dog" };

    private readonly int[] _allowedIntValues = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    [Theory]
    [InlineData("-a", "Cat;dog", "-v", "Cat", "Dog", "Dog|cat|Pussy")]
    [InlineData("-a", "Cat;dog", "-v", "Puppy", "Puppy", "Dog|cat|Pussy")]
    [InlineData("-a", "Cat;dog", "-v", "Pussy", "DOG", "Dog|cat|pUPpY")]
    [InlineData("-a", "Cat;dog", "-v", "DOG", "Puppy", "Dog|Pussy|PuPpY")]
    [InlineData("-a", "Cat;dog", "-v", "cat", "Puppy", "Dog|Pussy|PUppY")]
    public void ShouldAllowValue_WhenOptionValuesAreValid(params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<StringAppOptions>()
            .AddNamedOption(s => s.Value, o => o.WithAllowedValues(_allowedValues))
            .AddNamedOption(s => s.Values, o => o.WithAllowedValues(_allowedValues))
            .AddValueOption(s => s.NullValue, o => o.WithAllowedValues(_allowedValues))
            .AddValueOption(s => s.NullValues, o => o.WithAllowedValues(_allowedValues))
            .Build();

        // Act
        sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeTrue();

        var option = sut.GetOptionByAlias(args[0]) as ISequentialNamedOption<string>;
        option.ShouldNotBeNull();
        option.ValueTokens.Count.ShouldBe(1);
        option.InputValues.Count.ShouldBe(2);
        option.ResultValues.Count.ShouldBe(2);

        var option2 = sut.GetOptionByAlias(args[2]) as IScalarNamedOption<string>;
        option2.ShouldNotBeNull();
        option2.ValueTokens.Count.ShouldBe(1);
        option2.InputValues.Count.ShouldBe(1);
        option2.ResultValue.ShouldNotBeNull();

        var option3 = sut.GetOptionByName(nameof(StringAppOptions.NullValue)) as IScalarValueOption<string>;
        option3.ShouldNotBeNull();
        option3.ValueTokens.ShouldHaveSingleItem();
        option3.InputValues.ShouldHaveSingleItem();
        option3.ResultValue.ShouldNotBeNull();

        var option4 = sut.GetOptionByName(nameof(StringAppOptions.NullValues)) as ISequentialValueOption<string>;
        option4.ShouldNotBeNull();
        option4.ValueTokens.Count.ShouldBe(1);
        option4.InputValues.Count.ShouldBe(3);
        option4.ResultValues.Count.ShouldBe(3);
    }

    [Theory]
    [InlineData("-a", "Cat;dog", "-V", "CaT", "DOg", "Dog|cat|Pussy")]
    [InlineData("-a", "Cat;pussy", "-V", "PUppY", "puppy", "Dog|cat|Pussy")]
    [InlineData("-a", "pussy;dog", "-V", "PUSsy", "DOG", "Dog|cat|pUPpY")]
    [InlineData("-a", "Cat;DOG", "-V", "DOG", "puppy", "Dog|Pussy|PuPpY")]
    [InlineData("-a", "Cat;dog", "-V", "cat", "puppy", "Dog|Pussy|PUppY")]
    public void ShouldNotAllowValue_WhenOptionValuesAreValid_CaseSensitive(params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<StringAppOptions>(caseSensitive: true)
            .AddNamedOption(s => s.Value, o => o.WithAllowedValues(_allowedValues))
            .AddNamedOption(s => s.Values, o => o.WithAllowedValues(_allowedValues))
            .AddValueOption(s => s.NullValue, o => o.WithAllowedValues(_allowedValues))
            .AddValueOption(s => s.NullValues, o => o.WithAllowedValues(_allowedValues))
            .Build();

        // Act
        sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeFalse();
        sut.Errors.Count.ShouldBe(4);
        sut.Errors.ShouldAllBe(e => e.Error.Code == "ParserErrors.ValueMustBeOneOf");
    }

    [Theory]
    [InlineData("-a", "1;2", "-v", "1", "1", "2|1|5")]
    [InlineData("-a", "2;4", "-v", "3", "3", "3|5|6")]
    [InlineData("-a", "4|4", "-v", "4", "5", "4|6|6")]
    [InlineData("-a", "5|6", "-v", "5", "3", "5|7|9")]
    [InlineData("-a", "7;9", "-v", "8", "5", "1|3|6")]
    public void ShouldAllowIntValue_WhenOptionValuesAreValid(params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value, o => o.WithAllowedValues(_allowedIntValues))
            .AddNamedOption(s => s.Values, o => o.WithAllowedValues(_allowedIntValues))
            .AddValueOption(s => s.NullValue, o => o.WithAllowedValues(_allowedIntValues))
            .AddValueOption(s => s.NullValues, o => o.WithAllowedValues(_allowedIntValues))
            .Build();

        // Act
        sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeTrue();

        var option = sut.GetOptionByAlias(args[0]) as ISequentialNamedOption<int>;
        option.ShouldNotBeNull();
        option.ValueTokens.Count.ShouldBe(1);
        option.InputValues.Count.ShouldBe(2);
        option.ResultValues.Count.ShouldBe(2);

        var option2 = sut.GetOptionByAlias(args[2]) as IScalarNamedOption<int>;
        option2.ShouldNotBeNull();
        option2.ValueTokens.Count.ShouldBe(1);
        option2.InputValues.Count.ShouldBe(1);
        option2.ResultValue.ShouldNotBeNull();

        var option3 = sut.GetOptionByName(nameof(StringAppOptions.NullValue)) as IScalarValueOption<int>;
        option3.ShouldNotBeNull();
        option3.ValueTokens.ShouldHaveSingleItem();
        option3.InputValues.ShouldHaveSingleItem();
        option3.ResultValue.ShouldNotBeNull();

        var option4 = sut.GetOptionByName(nameof(StringAppOptions.NullValues)) as ISequentialValueOption<int>;
        option4.ShouldNotBeNull();
        option4.ValueTokens.Count.ShouldBe(1);
        option4.InputValues.Count.ShouldBe(3);
        option4.ResultValues.Count.ShouldBe(3);
    }

    [Theory]
    [InlineData("-a", "11;2", "-v", "11", "11", "21|1|5")]
    [InlineData("-a", "21;4", "-v", "13", "13", "3|15|6")]
    [InlineData("-a", "41|4", "-v", "14", "51", "4|16|6")]
    [InlineData("-a", "51|6", "-v", "15", "31", "5|17|9")]
    [InlineData("-a", "71;9", "-v", "18", "55", "11|3|6")]
    public void ShouldNotAllowIntValue_WhenOptionValuesAreInvalid(params string[] args)
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value, o => o.WithAllowedValues(_allowedIntValues))
            .AddNamedOption(s => s.Values, o => o.WithAllowedValues(_allowedIntValues))
            .AddValueOption(s => s.NullValue, o => o.WithAllowedValues(_allowedIntValues))
            .AddValueOption(s => s.NullValues, o => o.WithAllowedValues(_allowedIntValues))
            .Build();

        // Act
        sut.Parse(args);

        // Assert
        sut.IsValid.ShouldBeFalse();
        sut.Errors.Count.ShouldBe(4);
        sut.Errors.ShouldAllBe(e => e.Error.Code == "ParserErrors.ValueMustBeOneOf");
    }
}