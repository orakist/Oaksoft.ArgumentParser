using Oaksoft.ArgumentParser.Extensions;
using Shouldly;

namespace Oaksoft.ArgumentParser.Tests
{
    public class SwitchOptionTests
    {
        [Theory]
        [InlineData(true, true, "-v", "-n")]
        [InlineData(false, true, "-vfalse", "-ntrue")]
        [InlineData(true, true, "--value", "--null-value")]
        [InlineData(true, false, "-n:False", "-v:true")]
        [InlineData(true, true, "-v=true", "-n=true")]
        [InlineData(true, false, "--value:true", "--null-value=FALSE")]
        [InlineData(false, true, "/v=false", "/null-value:TRUE")]
        [InlineData(false, true, "/null-value", "TRUE", "/v", "FALSE")]
        public void Parse_bool_switch_option_when_arguments_valid(bool val1, bool val2, params string[] args)
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
            result.Value.ShouldBeEquivalentTo(val1);
            result.NullValue.ShouldBeEquivalentTo(val2);
        }

        [Theory]
        [InlineData("--v", "--n")]
        [InlineData("-value", "-null-value")]
        [InlineData("--v:true", "--n:true")]
        [InlineData("--v=true", "--n=true")]
        [InlineData("-value:true", "-null-value=true")]
        [InlineData("/vtrue", "/null-valuex")]
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
            sut.IsValid.ShouldBeFalse();
            sut.Errors.Count.ShouldBe(2);
            result.NullValue.ShouldBe(null);
            result.Value.ShouldBe(false);
        }

        [Theory]
        [InlineData("-v")]
        [InlineData("--null-value")]
        [InlineData("-v:true")]
        [InlineData("-n=true")]
        [InlineData("--null-value=true")]
        [InlineData("/v:true")]
        public void Parse_bool_switch_option_when_options_mandatory(params string[] args)
        {
            // Fixture setup
            var sut = CommandLine.CreateParser<BoolScalarOptions>()
                .AddSwitchOption(s => s.Value, mandatoryOption: true)
                .AddSwitchOption(s => s.NullValue, mandatoryOption: true)
                .Build();

            // Exercise system
            var result = sut.Parse(args);

            // Verify outcome
            sut.IsValid.ShouldBeFalse();
            sut.Errors.Count.ShouldBe(1);
            sut.Errors[0].StartsWith("At least '1' option was expected").ShouldBeTrue();
            result.NullValue.ShouldBe(null);
            result.Value.ShouldBe(false);
        }

        [Theory]
        [InlineData(1, "-v", "-n", "-n")]
        [InlineData(2, "--null-value")]
        [InlineData(0, "-v:true", "-n:true", "-n:false")]
        [InlineData(1, "-v=true", "-n=true")]
        [InlineData(1, "--value:true", "--null-value=true")]
        [InlineData(1, "/v=true", "/null-value:true")]
        public void Parse_bool_switch_option_when_multiple_option_allowed(int count, params string[] args)
        {
            // Fixture setup
            var sut = CommandLine.CreateParser<BoolScalarOptions>()
                .AddSwitchOption(s => s.Value, o => o.WithOptionArity(1, 3))
                .AddSwitchOption(s => s.NullValue, o => o.WithOptionArity(1, 3).WithValueArity(2, 3))
                .Build();

            // Exercise system
            var result = sut.Parse(args);

            // Verify outcome
            sut.IsValid.ShouldBeEquivalentTo(count < 1);
            sut.Errors.Count.ShouldBeEquivalentTo(count);
            result.NullValue.ShouldBeEquivalentTo(sut.IsValid ? false : null);
            result.Value.ShouldBe(sut.IsValid);
        }

        [Theory]
        [InlineData(true, false, "-v", "-n")]
        [InlineData(false, true, "-vfalse", "-ntrue")]
        [InlineData(true, null, "--value")]
        [InlineData(true, false, "-n:False", "-v:true")]
        [InlineData(true, true, "-v=true", "-n=true")]
        [InlineData(true, false, "--value:true", "--null-value")]
        [InlineData(false, true, "/v=false", "/null-value:TRUE")]
        [InlineData(false, true, "/null-value", "TRUE", "/v", "FALSE")]
        public void Parse_bool_switch_option_when_default_value_set(bool val1, bool? val2, params string[] args)
        {
            // Fixture setup
            var sut = CommandLine.CreateParser<BoolScalarOptions>()
                .AddSwitchOption(s => s.Value, o => o.WithDefaultValue(true))
                .AddSwitchOption(s => s.NullValue, o => o.WithDefaultValue(false))
                .Build();

            // Exercise system
            var result = sut.Parse(args);

            // Verify outcome
            sut.IsValid.ShouldBeTrue();
            result.NullValue.ShouldBeEquivalentTo(val2);
            result.Value.ShouldBe(val1);
        }
    }
}