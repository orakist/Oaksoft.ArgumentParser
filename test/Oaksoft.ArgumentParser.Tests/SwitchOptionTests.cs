using Oaksoft.ArgumentParser.Extensions;
using Shouldly;

namespace Oaksoft.ArgumentParser.Tests
{
    public class SwitchOptionTests
    {
        [Theory]
        [InlineData("-v" , "-n")]
        public void Parse_bool_switch_option(params string[] args)
        {
            // Fixture setup
            var sut = CommandLine.CreateParser<BoolScalarOptions>()
                .AddSwitchOption(s => s.Value)
                .AddSwitchOption(s => s.NullValue)
                .Build();

            // Exercise system
            var result = sut.Parse(args);

            // Verify outcome
            sut.IsValid.ShouldBeTrue();
            result.Value.ShouldBeTrue();
            result.NullValue.ShouldBeEquivalentTo(true);
        }
    }
}