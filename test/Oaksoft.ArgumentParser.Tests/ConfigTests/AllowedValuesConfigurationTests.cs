using Oaksoft.ArgumentParser.Errors.Builder;
using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Tests.TestModels;
using Shouldly;

namespace Oaksoft.ArgumentParser.Tests.ConfigTests;

public class AllowedValuesConfigurationTests : ArgumentParserTestBase
{
    [Theory]
    [InlineData("Cat", "Puppy", "007")]
    [InlineData("  s", "--  ", " a b c ")]
    public void ShouldBuild_WhenAllowedValuesConfiguredString(params string[] values)
    {
        // Arrange
        var sut = CommandLine.CreateParser<StringAppOptions>()
            .AddNamedOption(s => s.Value, o => o.WithAllowedValues(values))
            .AddNamedOption(s => s.Values, o => o.WithAllowedValues(values))
            .AddValueOption(s => s.NullValue, o => o.WithAllowedValues(values))
            .AddValueOption(s => s.NullValues, o => o.WithAllowedValues(values));

        // Act
        var parser = sut.Build();

        // Assert
        var options = parser.GetOptions()
            .Where(o => o.Name != "Help")
            .OfType<IHaveAllowedValues<string>>()
            .ToList();

        options.Count.ShouldBe(4);
        foreach (var option in options)
        {
            var trimmed = values.Select(s => s.Trim());
            option.AllowedValues.ShouldAllBe(a => trimmed.Contains(a));
        }
    }

    [Theory]
    [InlineData(new[] { 1.0, 2.1, 0.55 })]
    [InlineData(new[] { 100, 200.1, 110.55 })]
    public void ShouldBuild_WhenAllowedValuesConfiguredDouble(double[] values)
    {
        // Arrange
        var sut = CommandLine.CreateParser<DoubleAppOptions>()
            .AddNamedOption(s => s.Value, o => o.WithAllowedValues(values))
            .AddNamedOption(s => s.Values, o => o.WithAllowedValues(values))
            .AddValueOption(s => s.NullValue, o => o.WithAllowedValues(values))
            .AddValueOption(s => s.NullValues, o => o.WithAllowedValues(values));

        // Act
        var parser = sut.Build();

        // Assert
        var options = parser.GetOptions()
            .Where(o => o.Name != "Help")
            .OfType<IHaveAllowedValues<double>>()
            .ToList();

        options.Count.ShouldBe(4);
        foreach (var option in options)
        {
            option.AllowedValues.ShouldAllBe(a => values.Contains(a));
        }
    }

    [Fact]
    public void ShouldBuild_WhenAllowedValuesNotConfigured()
    {
        // Arrange
        var sut = CommandLine.CreateParser<StringAppOptions>()
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.Values)
            .AddValueOption(s => s.NullValue)
            .AddValueOption(s => s.NullValues);

        // Act
        var parser = sut.Build();

        // Assert
        var options = parser.GetOptions()
            .Where(o => o.Name != "Help")
            .OfType<IHaveAllowedValues<string>>()
            .ToList();

        options.Count.ShouldBe(4);
        foreach (var option in options)
        {
            option.AllowedValues.ShouldBeEmpty();
        }
    }

    [Theory]
    [InlineData("Cat", null, "007")]
    [InlineData("  s", "  ", "x")]
    [InlineData("  ab ", "", "x")]
    public void ShouldThrowException_WhenTryToUpdateEmptyAllowedValues(params string?[] values)
    {
        // Arrange
        var sut = CommandLine.CreateParser<StringAppOptions>();
        string[] valueStrings = values.ToArray()!;

        // Act & Assert
        var exception = Should.Throw<OptionBuilderException>(() => 
            sut.AddNamedOption(s => s.Value, o => o.WithAllowedValues(valueStrings)));
        var info = exception.Error;

        info.Error.Code.ShouldBe("BuilderErrors.EmptyAllowedValue");
        info.Values.ShouldBeNull();
        info.OptionName.ShouldBe(nameof(StringAppOptions.Value));
    }

    [Fact]
    public void ShouldThrowException_WhenTryToUpdateAllowedValuesAfterBuild()
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value);

        // Act
        var parser = sut.Build();
        var option = parser.GetOptionByName(nameof(IntAppOptions.Value));
        var option1 = option as IScalarNamedOption<int>;

        // Assert
        option1.ShouldNotBeNull();
        var exception = Should.Throw<OptionBuilderException>(() => option1.WithAllowedValues(2));
        var info = exception.Error;

        info.Error.Code.ShouldBe("BuilderErrors.CannotBeModified");
        info.Values.ShouldBeNull();
        info.OptionName.ShouldBe(nameof(IntAppOptions.Value));
    }
}