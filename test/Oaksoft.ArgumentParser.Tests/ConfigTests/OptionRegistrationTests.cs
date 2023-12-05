using Oaksoft.ArgumentParser.Errors.Builder;
using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Tests.TestModels;
using Shouldly;

namespace Oaksoft.ArgumentParser.Tests.ConfigTests;

public class OptionRegistrationTests
{
    [Fact]
    public void ShouldThrowException_WhenTryToAddReservedProperty()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>()
            .AddSwitchOption(s => s.Help);

        // Act
        var exception = Should.Throw<OptionBuilderException>(sut.Build);
        var info = exception.Error;

        // Assert
        info.Error.Code.ShouldBe(BuilderErrors.ReservedProperty.Code);
        info.Values.ShouldHaveSingleItem();
        info.Values.ShouldContain(nameof(SampleOptionNames.Help));
        info.OptionName.ShouldBeNull();
        var message = string.Format(info.Error.Format, nameof(SampleOptionNames.Help));
        exception.Message.ShouldBe(message);
    }
}