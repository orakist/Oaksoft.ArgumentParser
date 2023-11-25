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

        // Act & Assert
        var exception = Should.Throw<OptionBuilderException>(sut.Build);
        exception.Error.Code.ShouldBe(BuilderErrors.ReservedProperty.Code);
        exception.Error.Values.ShouldHaveSingleItem();
        exception.Error.Values.ShouldContain(nameof(IntAppOptions.Help));
        exception.OptionName.ShouldBeNull();
        var message = string.Format(exception.Error.Message, nameof(IntAppOptions.Help));
        exception.Message.ShouldBe(message);
    }
}