using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Tests.Options;
using Shouldly;

namespace Oaksoft.ArgumentParser.Tests
{
    public class NameConfigurationTests
    {
        [Fact]
        public void ShouldBuildOptions_WhenDifferentNamesUsed()
        {
            // Arrange
            var sut = CommandLine.CreateParser<IntAppOptions>()
                .AddNamedOption(s => s.Value, o => o.WithName("Value"))
                .AddCountOption(s => s.NullValue, o => o.WithName("ValueCount"))
                .AddNamedOption(s => s.Values, o => o.WithName("ValueList"))
                .AddValueOption(s => s.NullValues, o => o.WithName("ValueX"))
                .AddValueOption(s => s.ValueFlag, o => o.WithName("ValueY"));

            // Act
            var result = sut.Build();

            // Assert
            result.GetOptions().Count.ShouldBe(6);

            var option = result.GetOptionByName(nameof(IntAppOptions.Value));
            var namedOption = option as INamedOption;
            namedOption.ShouldNotBeNull();
            namedOption.Name.ShouldBe("Value");

            option = result.GetOptionByName(nameof(IntAppOptions.NullValue));
            namedOption = option as INamedOption;
            namedOption.ShouldNotBeNull();
            namedOption.Name.ShouldBe("ValueCount");

            option = result.GetOptionByName("ValueList");
            namedOption = option as INamedOption;
            namedOption.ShouldNotBeNull();
            namedOption.Name.ShouldBe("ValueList");

            option = result.GetOptionByName(nameof(IntAppOptions.NullValues));
            var valueOption = option as IValueOption;
            valueOption.ShouldNotBeNull();
            valueOption.Name.ShouldBe("ValueX");

            option = result.GetOptionByName(nameof(IntAppOptions.ValueFlag));
            valueOption = option as IValueOption;
            valueOption.ShouldNotBeNull();
            valueOption.Name.ShouldBe("ValueY");
        }

        [Fact]
        public void ShouldBuildOptions_WhenNumberAndLetterNamesUsed()
        {
            // Arrange
            var sut = CommandLine.CreateParser<SampleOptionNames>()
                .AddNamedOption(s => s.__1_Help_1)
                .AddNamedOption(s => s.__2_Value)
                .AddNamedOption(s => s.ValueTest);

            // Act
            var result = sut.Build();

            // Assert
            result.GetOptions().Count.ShouldBe(4);

            var option = result.GetOptionByName(nameof(SampleOptionNames.__1_Help_1));
            var namedOption = option as INamedOption;
            namedOption.ShouldNotBeNull();
            namedOption.Name.ShouldBe("Help 1");
            option = result.GetOptionByName("Help 1");
            namedOption = option as INamedOption;
            namedOption.ShouldNotBeNull();
            namedOption.Name.ShouldBe("Help 1");

            option = result.GetOptionByName(nameof(SampleOptionNames.__2_Value));
            namedOption = option as INamedOption;
            namedOption.ShouldNotBeNull();
            namedOption.Name.ShouldBe("Value");
            option = result.GetOptionByName("Value");
            namedOption = option as INamedOption;
            namedOption.ShouldNotBeNull();
            namedOption.Name.ShouldBe("Value");

            option = result.GetOptionByName(nameof(SampleOptionNames.ValueTest));
            namedOption = option as INamedOption;
            namedOption.ShouldNotBeNull();
            namedOption.Name.ShouldBe("Value Test");
            option = result.GetOptionByName("Value Test");
            namedOption = option as INamedOption;
            namedOption.ShouldNotBeNull();
            namedOption.Name.ShouldBe("Value Test");
        }

        [Fact]
        public void ShouldThrowException_WhenSameNameUsed()
        {
            // Arrange
            var sut = CommandLine.CreateParser<SampleOptionNames>()
                .AddNamedOption(s => s.Value)
                .AddNamedOption(s => s.__2_Value);

            // Act & Assert
            Should.Throw<Exception>(() => sut.Build());
        }

        [Fact]
        public void ShouldThrowException_WhenSameCustomNameUsed()
        {
            // Arrange
            var sut = CommandLine.CreateParser<LongAppOptions>()
                .AddNamedOption(s => s.Value, o => o.WithName("Test"))
                .AddNamedOption(s => s.ValueCount, o => o.WithName("Test"));

            // Act & Assert
            Should.Throw<Exception>(() => sut.Build());
        }

        [Fact]
        public void ShouldThrowException_WhenTryToUpdateNameAfterBuild()
        {
            // Arrange
            var sut = CommandLine.CreateParser<LongAppOptions>()
                .AddSwitchOption(s => s.ValueFlag);

            // Act
            var result = sut.Build();
            var option = result.GetOptionByName(nameof(LongAppOptions.ValueFlag));
            var namedOption = option as ISwitchOption;
            namedOption.ShouldNotBeNull();

            // Assert
            Should.Throw<Exception>(() => namedOption.WithName("NewName"));
        }
    }
}