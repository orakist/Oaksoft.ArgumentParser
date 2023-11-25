using Oaksoft.ArgumentParser.Exceptions;
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
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddSwitchOption(s => s.Help);

        // Act
        var exception = Should.Throw<OptionBuilderException>(sut.Build);
        var info = exception.Error;

        // Assert
        info.Error.Code.ShouldBe(BuilderErrors.ReservedProperty.Code);
        info.Values.ShouldHaveSingleItem();
        info.Values.ShouldContain(nameof(IntAppOptions.Help));
        info.OptionName.ShouldBeNull();
        var message = string.Format(info.Error.Format, nameof(IntAppOptions.Help));
        exception.Message.ShouldBe(message);
    }
}