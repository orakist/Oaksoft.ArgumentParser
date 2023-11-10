using Oaksoft.ArgumentParser.Extensions;
using Shouldly;

namespace Oaksoft.ArgumentParser.Tests
{
    public class SwitchOptionTests
    {
        [Theory]
        [InlineData("-v", "-n")]
        [InlineData("-vtrue", "-ntrue")]
        [InlineData("--value", "--null-value")]
        [InlineData("-v:true", "-n:true")]
        [InlineData("-v=true", "-n=true")]
        [InlineData("--value:true", "--null-value=true")]
        [InlineData("/v=true", "/null-value:true")]
        public void Parse_bool_switch_option_when_arguments_valid(params string[] args)
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

        [Theory]
        [InlineData("-v", "-n")]
        [InlineData("--value", "--null-value")]
        [InlineData("-v:true", "-n:true")]
        [InlineData("-v=true", "-n=true")]
        [InlineData("--value:true", "--null-value=true")]
        [InlineData("/v=true", "/null-value:true")]
        public void Parse_bool_switch_option_when_arguments_invalid(params string[] args)
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

        [Theory]
        [InlineData("-v", "-n")]
        [InlineData("--value", "--null-value")]
        [InlineData("-v:true", "-n:true")]
        [InlineData("-v=true", "-n=true")]
        [InlineData("--value:true", "--null-value=true")]
        [InlineData("/v=true", "/null-value:true")]
        public void Parse_bool_switch_option_when_multiple_option_allowed(params string[] args)
        {
            // Fixture setup
            var sut = CommandLine.CreateParser<BoolScalarOptions>()
                .AddSwitchOption(s => s.Value, o => o.WithOptionArity(1, 3))
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