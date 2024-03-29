using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Errors.Builder;
using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Tests.TestModels;
using Shouldly;
using System.Reflection;

namespace Oaksoft.ArgumentParser.Tests.ConfigTests;

public class OptionRegistrationTests : ArgumentParserTestBase
{
    [Fact]
    public void ShouldBuild_WithValidVersionText()
    {
        // Arrange
        var sut = CommandLine.CreateParser<IntAppOptions>()
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.Values);

        // Act
        var parser = sut.Build();
        var assembly = Assembly.GetEntryAssembly();
        var value = assembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

        // Assert
        parser.GetOptions().Count.ShouldBe(2);
        var version = parser.GetVersionText();
        version.ShouldBe(value);
    }

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
        info.Error.Code.ShouldBe("BuilderErrors.ReservedProperty");
        info.Values.ShouldHaveSingleItem();
        info.Values.ShouldContain(nameof(SampleOptionNames.Help));
        info.OptionName.ShouldBeNull();
        var message = string.Format(info.Error.Format, nameof(SampleOptionNames.Help));
        exception.Message.ShouldBe(message);
    }

    [Fact]
    public void ShouldNotRegisterNamedOption_WithInvalidBodyExpression()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>();

        // Act & Assert
        var exception = Should.Throw<OptionBuilderException>(() => sut.AddNamedOption(s => s.Value.ToString()));

        exception.Error.Error.Code.ShouldBe("BuilderErrors.InvalidPropertyExpression");
    }

    [Fact]
    public void ShouldNotRegisterNamedOption_WithoutSetMethod()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>();

        // Act & Assert
        var exception = Should.Throw<OptionBuilderException>(() => sut.AddNamedOption(s => s.WithoutSet));

        exception.Error.Error.Code.ShouldBe("BuilderErrors.PropertyWithoutSetMethod");
    }

    [Fact]
    public void ShouldNotRegisterNamedOption_UnsupportedProperty()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>();

        // Act & Assert
        var exception = Should.Throw<OptionBuilderException>(() => sut.AddNamedOption(s => s.Unknown));

        exception.Error.Error.Code.ShouldBe("BuilderErrors.UnsupportedPropertyType");
    }

    [Fact]
    public void ShouldParseHelp_WhenHelpInputIsTrue()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>()
            .AddNamedOption(s => s.Value);

        // Act & Assert
        var parser = sut.Build();
        parser.Parse("-h");

        var builtInOpts = parser.GetBuiltInOptions();
        builtInOpts.Help.ShouldBe(true);
        parser.IsHelpOption.ShouldBeTrue();
    }

    [Fact]
    public void ShouldCreateError_WhenHelpInputIsNotValid()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>()
            .AddNamedOption(s => s.Value);

        // Act & Assert
        var parser = sut.Build();
        parser.Parse("-h", "-v:1");

        var builtInOpts = parser.GetBuiltInOptions();
        parser.Errors.Count.ShouldBe(1);
        parser.Errors[0].Error.Code.ShouldBe("ParserErrors.InvalidSingleOptionUsage");

        builtInOpts.Help.ShouldBeNull();
        parser.IsHelpOption.ShouldBeFalse();
    }

    [Fact]
    public void ShouldParseVersion_WhenVersionInputIsTrue()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>()
            .AddNamedOption(s => s.Value);

        // Act & Assert
        var parser = sut.Build();
        parser.Parse("--vn");

        var builtInOpts = parser.GetBuiltInOptions();
        builtInOpts.Version.ShouldBe(true);
        parser.IsVersionOption.ShouldBeTrue();
    }

    [Fact]
    public void ShouldCreateError_WhenVersionInputIsNotValid()
    {
        // Arrange
        var sut = CommandLine.CreateParser<SampleOptionNames>()
            .AddNamedOption(s => s.Value);

        // Act & Assert
        var parser = sut.Build();
        parser.Parse("--version", "-v:1");

        var builtInOpts = parser.GetBuiltInOptions();
        parser.Errors.Count.ShouldBe(1);
        parser.Errors[0].Error.Code.ShouldBe("ParserErrors.InvalidSingleOptionUsage");

        builtInOpts.Help.ShouldBeNull();
        parser.IsHelpOption.ShouldBeFalse();
    }

    [Fact]
    public void ShouldAutoBuild_WhenOptionsAreValid()
    {
        // Arrange
        var parser = CommandLine.AutoBuild<IntAppOptions>();
        var header = parser.GetHeaderText();
        var help = parser.GetHelpText(false);

        // Act & Assert
        var options = parser.Parse("-v:5", "-l", "-n:10");

        parser.GetOptions().Count.ShouldBe(8);
        var builtInOpts = parser.GetBuiltInOptions();
        builtInOpts.Help.ShouldBeNull();
        parser.IsHelpOption.ShouldBeFalse();
        options.Value.ShouldBe(5);
        options.NullValue.ShouldBe(10);
        options.NullValueFlag.ShouldBeTrue();
        header.ShouldNotBeEmpty();
    }

    [Fact]
    public void ShouldBuildError_WhenArgumentInvalid1()
    {
        // Arrange
        var sut = CommandLine.CreateParser<DoubleAppOptions>()
            .ConfigureSettings(s => s.VerbosityLevel = VerbosityLevelType.Trace)
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.Values);

        // Act & Assert
        var parser = sut.Build();
        parser.Parse("a", "b");

        parser.Errors.Count.ShouldBe(2);
        var errors = parser.GetErrorText(false);
        errors.ShouldContain("ParserErrors.UnknownToken");
    }

    [Fact]
    public void ShouldBuildError_WhenArgumentInvalid2()
    {
        // Arrange
        var sut = CommandLine.CreateParser<DoubleAppOptions>()
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.Values);

        // Act & Assert
        var parser = sut.Build();
        parser.Parse("a", "b", "--vl:detailed");

        parser.Errors.Count.ShouldBe(2);
        var errors = parser.GetErrorText(false);
        errors.ShouldContain("ParserErrors.UnknownToken");
    }

    [Fact]
    public void ShouldBuildError_WhenArgumentInvalid3()
    {
        // Arrange
        var sut = CommandLine.CreateParser<DoubleAppOptions>()
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.Values);

        // Act & Assert
        var parser = sut.Build();
        parser.Parse("a", "b", "--vl:normal");

        parser.Errors.Count.ShouldBe(2);
        var errors = parser.GetErrorText(false);
        errors.ShouldNotContain("ParserErrors.UnknownToken");
    }

    [Fact]
    public void ShouldBuildError_WhenArgumentInvalid4()
    {
        // Arrange
        var sut = CommandLine.CreateParser<DoubleAppOptions>()
            .AddNamedOption(s => s.Value, o => o.AddPredicate(v => v < 0 ? throw new NotImplementedException() : true))
            .AddNamedOption(s => s.Values);

        // Act & Assert
        var parser = sut.Build();
        parser.Parse("-v:-5", "b", "--vl:trace");
        parser.Errors.Count.ShouldBe(2);
        var errors = parser.GetErrorText(false);
        errors.ShouldContain(nameof(NotImplementedException));
    }

    [Fact]
    public void ShouldNotBuildError_WhenArgumentsValid()
    {
        // Arrange
        var sut = CommandLine.CreateParser<DoubleAppOptions>()
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.Values);

        // Act & Assert
        var parser = sut.Build();
        parser.Parse("-v:5");

        parser.Errors.Count.ShouldBe(0);
        var errors = parser.GetErrorText(false);
        errors.ShouldBeEmpty();
    }

    [Theory]
    [InlineData("--value ^5 \"--values 1,3,5\"\n-f -c -c -c\nq")]
    [InlineData("--value ^5 --values 1,3,5\n-f -c -c -c\nquit")]
    public void ShouldRun1_WhenTryToPassInputsByReader(string argument)
    {
        // Arrange
        CommandLine.DisableTextWriter = false;
        var reader = new StringReader(argument);
        var writer = new StringWriter();

        var sut = CommandLine.CreateParser<StringAppOptions>(textReader: reader, textWriter: writer)
            .ConfigureSettings(s =>
            {
                s.AutoPrintErrors = false; // to throw user errors
                s.AutoPrintArguments = true;
            })
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.Values)
            .AddSwitchOption(s => s.ValueFlag)
            .AddCounterOption(s => s.ValueCount);

        // Act & Assert
        var loopIndex = 0;
        var parser = sut.Build();

        parser.Run(opts =>
        {
            if (loopIndex == 0)
            {
                opts.Value.ShouldBe("5");
                opts.Values.ShouldBe(new List<string> { "1", "3", "5" });
            }
            else
            {
                opts.Values.ShouldBeNull();
                opts.ValueFlag.ShouldBeTrue();
                opts.ValueCount.ShouldBe(3);
            }

            ++loopIndex;
        });

        writer.ToString().ShouldContain("Type the options");
        writer.ToString().ShouldContain("-value");
    }

    [Theory]
    [InlineData("--value ^5 \"--values 1,3,5\"\n-f -c -c -c\nq")]
    [InlineData("--value ^5 --values 1,3,5\n-f -c -c -c\nquit")]
    public async Task ShouldRunAsync1_WhenTryToPassInputsByReader(string argument)
    {
        // Arrange
        CommandLine.DisableTextWriter = false;
        var reader = new StringReader(argument);
        var writer = new StringWriter();

        var sut = CommandLine.CreateParser<StringAppOptions>(textReader: reader, textWriter: writer)
            .ConfigureSettings(s =>
            {
                s.AutoPrintErrors = false; // to throw user errors
                s.AutoPrintArguments = true;
            })
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.Values)
            .AddSwitchOption(s => s.ValueFlag)
            .AddCounterOption(s => s.ValueCount);

        // Act & Assert
        var loopIndex = 0;
        var parser = sut.Build();

        await parser.RunAsync(opts =>
        {
            if (loopIndex == 0)
            {
                opts.Value.ShouldBe("5");
                opts.Values.ShouldBe(new List<string> { "1", "3", "5" });
            }
            else
            {
                opts.Values.ShouldBeNull();
                opts.ValueFlag.ShouldBeTrue();
                opts.ValueCount.ShouldBe(3);
            }

            ++loopIndex;

            return Task.CompletedTask;
        });

        writer.ToString().ShouldContain("Type the options");
        writer.ToString().ShouldContain("-value");
    }

    [Theory]
    [InlineData("--value 5")]
    [InlineData("--values 1,3,5")]
    [InlineData("--null-value-flag")]
    public void ShouldRunOnce1_WhenTryToPassInputsByReader(string argument)
    {
        // Arrange
        CommandLine.DisableTextWriter = false;
        var reader = new StringReader(argument);
        var writer = new StringWriter();

        var sut = CommandLine.CreateParser<StringAppOptions>(textReader: reader, textWriter: writer)
            .ConfigureSettings(s =>
            {
                s.AutoPrintErrors = false; // to throw user errors
                s.AutoPrintArguments = true;
            })
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.Values)
            .AddSwitchOption(s => s.NullValueFlag);

        // Act & Assert
        var parser = sut.Build();
        parser.RunOnce(opts =>
        {
            opts.Value.ShouldBeOneOf("5", null);
            opts.Values?.ShouldBe(new List<string> { "1", "3", "5" });
        });

        parser.IsParsed.ShouldBeTrue();
        writer.ToString().ShouldContain("Type the options");
        writer.ToString().ShouldContain("-value");
    }

    [Theory]
    [InlineData("--value 5")]
    [InlineData("--values 1,3,5")]
    [InlineData("--null-value-flag")]
    public async Task ShouldRunOnceAsync1_WhenTryToPassInputsByReader(string argument)
    {
        // Arrange
        CommandLine.DisableTextWriter = false;
        var reader = new StringReader(argument);
        var writer = new StringWriter();

        var sut = CommandLine.CreateParser<StringAppOptions>(textReader: reader, textWriter: writer)
            .ConfigureSettings(s =>
            {
                s.AutoPrintErrors = false; // to throw user errors
                s.AutoPrintArguments = true;
            })
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.Values)
            .AddSwitchOption(s => s.NullValueFlag);

        // Act & Assert
        var parser = sut.Build();
        await parser.RunOnceAsync(opts =>
        {
            opts.Value.ShouldBeOneOf("5", null);
            opts.Values?.ShouldBe(new List<string> { "1", "3", "5" });
            return Task.CompletedTask;
        });

        parser.IsParsed.ShouldBeTrue();
        writer.ToString().ShouldContain("Type the options");
        writer.ToString().ShouldContain("-value");
    }

    [Theory]
    [InlineData("--value 5 --values 1,3,5\n-f -c -c -c\nq")]
    [InlineData("--value 5 --values 1,3,5\n-f -c -c -c\nquit")]
    public void ShouldRun1_WhenTryToPassInputsByReaderWithComment(string argument)
    {
        // Arrange
        CommandLine.DisableTextWriter = false;
        var sut = CommandLine.CreateParser<DoubleAppOptions>()
            .ConfigureSettings(s =>
            {
                s.AutoPrintErrors = false; // to throw user errors
                s.AutoPrintArguments = true;
            })
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.Values)
            .AddSwitchOption(s => s.ValueFlag)
            .AddCounterOption(s => s.ValueCount);

        var reader = new StringReader(argument);
        var writer = new StringWriter();

        // Act & Assert
        var loopIndex = 0;
        var parser = sut.Build();
        parser.SetTextReader(reader);
        parser.SetTextWriter(writer);

        parser.Run("Dummy Comment", opts =>
        {
            if (loopIndex == 0)
            {
                opts.Value.ShouldBe(5);
                opts.Values.ShouldBe(new List<double> { 1, 3, 5 });
            }
            else
            {
                opts.Values.ShouldBeNull();
                opts.ValueFlag.ShouldBeTrue();
                opts.ValueCount.ShouldBe(3);
            }

            ++loopIndex;
        });

        writer.ToString().ShouldContain("Dummy Comment");
        writer.ToString().ShouldContain("-value");
    }

    [Theory]
    [InlineData("--value 5 --values 1,3,5\n-f -c -c -c\nq")]
    [InlineData("--value 5 --values 1,3,5\n-f -c -c -c\nquit")]
    public async Task ShouldRunAsync1_WhenTryToPassInputsByReaderWithComment(string argument)
    {
        // Arrange
        CommandLine.DisableTextWriter = false;
        var sut = CommandLine.CreateParser<DoubleAppOptions>()
            .ConfigureSettings(s =>
            {
                s.AutoPrintErrors = false; // to throw user errors
                s.AutoPrintArguments = true;
            })
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.Values)
            .AddSwitchOption(s => s.ValueFlag)
            .AddCounterOption(s => s.ValueCount);

        var reader = new StringReader(argument);
        var writer = new StringWriter();

        // Act & Assert
        var loopIndex = 0;
        var parser = sut.Build();
        parser.SetTextReader(reader);
        parser.SetTextWriter(writer);

        await parser.RunAsync("Dummy Comment", opts =>
        {
            if (loopIndex == 0)
            {
                opts.Value.ShouldBe(5);
                opts.Values.ShouldBe(new List<double> { 1, 3, 5 });
            }
            else
            {
                opts.Values.ShouldBeNull();
                opts.ValueFlag.ShouldBeTrue();
                opts.ValueCount.ShouldBe(3);
            }

            ++loopIndex;

            return Task.CompletedTask;
        });

        writer.ToString().ShouldContain("Dummy Comment");
        writer.ToString().ShouldContain("-value");
    }

    [Theory]
    [InlineData("--null-value 5")]
    [InlineData("--values 1,3,5")]
    [InlineData("--null-value-flag")]
    public void ShouldRunOnce1_WhenTryToPassInputsByReaderWithComment(string argument)
    {
        // Arrange
        CommandLine.DisableTextWriter = false;
        var sut = CommandLine.CreateParser<DoubleAppOptions>()
            .ConfigureSettings(s =>
            {
                s.AutoPrintErrors = false; // to throw user errors
                s.AutoPrintArguments = true;
            })
            .AddNamedOption(s => s.NullValue)
            .AddNamedOption(s => s.Values)
            .AddSwitchOption(s => s.NullValueFlag);

        var reader = new StringReader(argument);
        var writer = new StringWriter();

        // Act & Assert
        var parser = sut.Build();
        parser.SetTextReader(reader);
        parser.SetTextWriter(writer);

        parser.RunOnce("Dummy Comment", opts =>
        {
            opts.NullValue.ShouldBeOneOf(5, null);
            if (opts.Values is null)
            {
                return;
            }

            opts.Values.ShouldBe(new List<double> { 1, 3, 5 });
        });

        parser.IsParsed.ShouldBeTrue();
        writer.ToString().ShouldContain("Dummy Comment");
        writer.ToString().ShouldContain("-value");
    }

    [Theory]
    [InlineData("--null-value 5")]
    [InlineData("--values 1,3,5")]
    [InlineData("--null-value-flag")]
    public async Task ShouldRunOnceAsync1_WhenTryToPassInputsByReaderWithComment(string argument)
    {
        // Arrange
        CommandLine.DisableTextWriter = false;
        var sut = CommandLine.CreateParser<DoubleAppOptions>()
            .ConfigureSettings(s =>
            {
                s.AutoPrintErrors = false; // to throw user errors
                s.AutoPrintArguments = true;
            })
            .AddNamedOption(s => s.NullValue)
            .AddNamedOption(s => s.Values)
            .AddSwitchOption(s => s.NullValueFlag);

        var reader = new StringReader(argument);
        var writer = new StringWriter();

        // Act & Assert
        var parser = sut.Build();
        parser.SetTextReader(reader);
        parser.SetTextWriter(writer);

        await parser.RunOnceAsync("Dummy Comment", opts =>
        {
            opts.NullValue.ShouldBeOneOf(5, null);
            opts.Values?.ShouldBe(new List<double> { 1, 3, 5 });

            return Task.CompletedTask;
        });

        parser.IsParsed.ShouldBeTrue();
        writer.ToString().ShouldContain("Dummy Comment");
        writer.ToString().ShouldContain("-value");
    }

    [Theory]
    [InlineData("--value 5 --values 1,3,5\n-f -c -c -c\nq")]
    [InlineData("--value 5 --values 1,3,5\n-f -c -c -c\nquit")]
    public async Task ShouldRun1_WhenTryToPassInputsByReaderAndArguments(string argument)
    {
        // Arrange
        var sut = CommandLine.CreateParser<DoubleAppOptions>()
            .ConfigureSettings(s => s.AutoPrintErrors = false)
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.Values)
            .AddSwitchOption(s => s.ValueFlag)
            .AddCounterOption(s => s.ValueCount);

        var reader = new StringReader(argument);

        // Act & Assert
        var loopIndex = 0;
        var parser = sut.Build();
        parser.SetTextReader(reader);

        var task = Task.Run(() =>
        {
            parser.Run(opts =>
            {
                if (loopIndex == 0)
                {
                    opts.Value.ShouldBe(3);
                    opts.Values.ShouldBe(new List<double> { 1, 2, 3 });
                }
                else if (loopIndex == 1)
                {
                    opts.Value.ShouldBe(5);
                    opts.Values.ShouldBe(new List<double> { 1, 3, 5 });
                }
                else
                {
                    opts.Values.ShouldBeNull();
                    opts.ValueFlag.ShouldBeTrue();
                    opts.ValueCount.ShouldBe(3);
                }

                ++loopIndex;
            }, "-v3", "--values:1,2,3");
        });

        await Should.NotThrowAsync(() => task.WaitAsync(TimeSpan.FromSeconds(1)));
    }

    [Theory]
    [InlineData("--value 5 --values 1,3,5\n-f -c -c -c\nq")]
    [InlineData("--value 5 --values 1,3,5\n-f -c -c -c\nquit")]
    public async Task ShouldRun2_WhenTryToPassInputsByReader(string argument)
    {
        // Arrange
        var sut = CommandLine.CreateParser<DoubleAppOptions>()
            .ConfigureSettings(s => s.AutoPrintErrors = false)
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.Values)
            .AddSwitchOption(s => s.ValueFlag)
            .AddCounterOption(s => s.ValueCount);

        var reader = new StringReader(argument);

        // Act & Assert
        var loopIndex = 0;
        var parser = sut.Build();
        parser.SetTextReader(reader);

        var task = Task.Run(() =>
        {
            parser.Run((prsr, opts) =>
            {
                prsr.IsValid.ShouldBeTrue();
                if (loopIndex == 0)
                {
                    opts.Value.ShouldBe(5);
                    opts.Values.ShouldBe(new List<double> { 1, 3, 5 });
                }
                else
                {
                    opts.Values.ShouldBeNull();
                    opts.ValueFlag.ShouldBeTrue();
                    opts.ValueCount.ShouldBe(3);
                }

                ++loopIndex;
            });
        });

        await Should.NotThrowAsync(() => task.WaitAsync(TimeSpan.FromSeconds(1)));

        var opts = parser.GetApplicationOptions();
        opts.Values.ShouldBeNull();
        opts.ValueFlag.ShouldBeTrue();
        opts.ValueCount.ShouldBe(3);
    }

    [Theory]
    [InlineData("--value 5 --values 1,3,5\n-f -c -c -c\nq")]
    [InlineData("--value 5 --values 1,3,5\n-f -c -c -c\nquit")]
    public async Task ShouldRun2_WhenTryToPassInputsByReaderWithComment(string argument)
    {
        // Arrange
        var sut = CommandLine.CreateParser<DoubleAppOptions>()
            .ConfigureSettings(s => s.AutoPrintErrors = false)
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.Values)
            .AddSwitchOption(s => s.ValueFlag)
            .AddCounterOption(s => s.ValueCount);

        var reader = new StringReader(argument);

        // Act & Assert
        var loopIndex = 0;
        var parser = sut.Build();
        parser.SetTextReader(reader);

        var task = Task.Run(() =>
        {
            parser.Run("Dummy Comment", (prsr, opts) =>
            {
                prsr.IsValid.ShouldBeTrue();
                if (loopIndex == 0)
                {
                    opts.Value.ShouldBe(5);
                    opts.Values.ShouldBe(new List<double> { 1, 3, 5 });
                }
                else
                {
                    opts.Values.ShouldBeNull();
                    opts.ValueFlag.ShouldBeTrue();
                    opts.ValueCount.ShouldBe(3);
                }

                ++loopIndex;
            });
        });

        await Should.NotThrowAsync(() => task.WaitAsync(TimeSpan.FromSeconds(1)));

        var opts = parser.GetApplicationOptions();
        opts.Values.ShouldBeNull();
        opts.ValueFlag.ShouldBeTrue();
        opts.ValueCount.ShouldBe(3);
    }

    [Theory]
    [InlineData("--value 5 --values 1,3,5\n-f -c -c -c\nq")]
    [InlineData("--value 5 --values 1,3,5\n-f -c -c -c\nquit")]
    public async Task ShouldRunAsync2_WhenTryToPassInputsByReaderWithComment(string argument)
    {
        // Arrange
        var sut = CommandLine.CreateParser<DoubleAppOptions>()
            .ConfigureSettings(s => s.AutoPrintErrors = false)
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.Values)
            .AddSwitchOption(s => s.ValueFlag)
            .AddCounterOption(s => s.ValueCount);

        var reader = new StringReader(argument);

        // Act & Assert
        var loopIndex = 0;
        var parser = sut.Build();
        parser.SetTextReader(reader);

        var task = parser.RunAsync("Dummy Comment", (prsr, opts) =>
        {
            prsr.IsValid.ShouldBeTrue();
            if (loopIndex == 0)
            {
                opts.Value.ShouldBe(5);
                opts.Values.ShouldBe(new List<double> { 1, 3, 5 });
            }
            else
            {
                opts.Values.ShouldBeNull();
                opts.ValueFlag.ShouldBeTrue();
                opts.ValueCount.ShouldBe(3);
            }

            ++loopIndex;
            return Task.CompletedTask;
        });

        await Should.NotThrowAsync(() => task.WaitAsync(TimeSpan.FromSeconds(1)));

        var opts = parser.GetApplicationOptions();
        opts.Values.ShouldBeNull();
        opts.ValueFlag.ShouldBeTrue();
        opts.ValueCount.ShouldBe(3);
    }

    [Theory]
    [InlineData("--values=1,3,5 --value=5")]
    [InlineData("--value 5 --values 1,3,5")]
    public void ShouldRunOnce2_WhenTryToPassInputsByReaderWithComment(string argument)
    {
        // Arrange
        var sut = CommandLine.CreateParser<DoubleAppOptions>()
            .ConfigureSettings(s => s.AutoPrintErrors = false)
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.Values);

        var reader = new StringReader(argument);

        // Act & Assert
        var parser = sut.Build();
        parser.SetTextReader(reader);

        parser.RunOnce("Dummy Comment", (prsr, opts) =>
        {
            prsr.IsValid.ShouldBeTrue();
            opts.Value.ShouldBe(5);
            opts.Values.ShouldBe(new List<double> { 1, 3, 5 });
        });

        var opts = parser.GetApplicationOptions();
        parser.IsParsed.ShouldBeTrue();
        opts.Value.ShouldBe(5);
        opts.Values.ShouldBe(new List<double> { 1, 3, 5 });
    }

    [Theory]
    [InlineData("--values=1,3,5 --value=5")]
    [InlineData("--value 5 --values 1,3,5")]
    public async Task ShouldRunOnceAsync2_WhenTryToPassInputsByReaderWithComment(string argument)
    {
        // Arrange
        var sut = CommandLine.CreateParser<DoubleAppOptions>()
            .ConfigureSettings(s => s.AutoPrintErrors = false)
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.Values);

        var reader = new StringReader(argument);

        // Act & Assert
        var parser = sut.Build();
        parser.SetTextReader(reader);

        await parser.RunOnceAsync("Dummy Comment", (prsr, opts) =>
        {
            prsr.IsValid.ShouldBeTrue();
            opts.Value.ShouldBe(5);
            opts.Values.ShouldBe(new List<double> { 1, 3, 5 });

            return Task.CompletedTask;
        });

        var opts = parser.GetApplicationOptions();
        parser.IsParsed.ShouldBeTrue();
        opts.Value.ShouldBe(5);
        opts.Values.ShouldBe(new List<double> { 1, 3, 5 });
    }

    [Theory]
    [InlineData("--value 5 --values 1,3,5\n-f -c -c -c\nq")]
    [InlineData("--value 5 --values 1,3,5\n-f -c -c -c\nquit")]
    public async Task ShouldRun2_WhenTryToPassInputsByReaderAndArguments(string argument)
    {
        // Arrange
        var sut = CommandLine.CreateParser<DoubleAppOptions>()
            .ConfigureSettings(s => s.AutoPrintErrors = false)
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.Values)
            .AddSwitchOption(s => s.ValueFlag)
            .AddCounterOption(s => s.ValueCount);

        var reader = new StringReader(argument);

        // Act & Assert
        var loopIndex = 0;
        var parser = sut.Build();
        parser.SetTextReader(reader);

        var task = Task.Run(() =>
        {
            parser.Run((prsr, opts) =>
            {
                prsr.IsValid.ShouldBeTrue();
                if (loopIndex == 0)
                {
                    opts.Value.ShouldBe(3);
                    opts.Values.ShouldBe(new List<double> { 1, 2, 3 });
                }
                else if (loopIndex == 1)
                {
                    opts.Value.ShouldBe(5);
                    opts.Values.ShouldBe(new List<double> { 1, 3, 5 });
                }
                else
                {
                    opts.Values.ShouldBeNull();
                    opts.ValueFlag.ShouldBeTrue();
                    opts.ValueCount.ShouldBe(3);
                }

                ++loopIndex;
            }, "-v3", "--values:1,2,3");
        });

        await Should.NotThrowAsync(() => task.WaitAsync(TimeSpan.FromSeconds(1)));

        var opts = parser.GetApplicationOptions();
        opts.Values.ShouldBeNull();
        opts.ValueFlag.ShouldBeTrue();
        opts.ValueCount.ShouldBe(3);
    }

    [Theory]
    [InlineData("--value 5 --values 1,3,5\n-f -c -c -c\nq")]
    [InlineData("--value 5 --values 1,3,5\n-f -c -c -c\nquit")]
    public async Task ShouldRun2_WhenThrowingException(string argument)
    {
        // Arrange
        var sut = CommandLine.CreateParser<DoubleAppOptions>()
            .ConfigureSettings(s => s.AutoPrintErrors = false)
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.Values)
            .AddSwitchOption(s => s.ValueFlag)
            .AddCounterOption(s => s.ValueCount);

        var reader = new StringReader(argument);

        // Act & Assert
        var parser = sut.Build();
        parser.SetTextReader(reader);

        var task = Task.Run(() =>
        {
            parser.Run((_, _) => throw new NotImplementedException(), "-v3", "--values:1,2,3");
        });

        await Should.ThrowAsync<Exception>(() => task.WaitAsync(TimeSpan.FromSeconds(1)));

        parser.IsValid.ShouldBeFalse();
        parser.Errors.Count.ShouldBe(1);
    }

    [Theory]
    [InlineData("--value 5 --values 1,3,5\n-f -c -c -c\nq")]
    [InlineData("--value 5 --values 1,3,5\n-f -c -c -c\nquit")]
    public async Task ShouldRunAsync2_WhenThrowingException(string argument)
    {
        // Arrange
        var sut = CommandLine.CreateParser<DoubleAppOptions>()
            .ConfigureSettings(s => s.AutoPrintErrors = false)
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.Values)
            .AddSwitchOption(s => s.ValueFlag)
            .AddCounterOption(s => s.ValueCount);

        var reader = new StringReader(argument);

        // Act & Assert
        var parser = sut.Build();
        parser.SetTextReader(reader);

        var task = parser.RunAsync((_, _) => throw new NotImplementedException(), "-v3", "--values:1,2,3");

        await Should.ThrowAsync<Exception>(() => task.WaitAsync(TimeSpan.FromSeconds(1)));

        parser.IsValid.ShouldBeFalse();
        parser.Errors.Count.ShouldBe(1);
    }

    [Theory]
    [InlineData("--value 5 --values 1,3,5\n-f -c -c -c\nq")]
    [InlineData("--value 5 --values 1,3,5\n-f -c -c -c\nquit")]
    public async Task ShouldRunOnce2_WhenThrowingException(string argument)
    {
        // Arrange
        var sut = CommandLine.CreateParser<DoubleAppOptions>()
            .ConfigureSettings(s => s.AutoPrintErrors = false)
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.Values)
            .AddSwitchOption(s => s.ValueFlag)
            .AddCounterOption(s => s.ValueCount);

        var reader = new StringReader(argument);

        // Act & Assert
        var parser = sut.Build();
        parser.SetTextReader(reader);

        var task = Task.Run(() =>
        {
            parser.RunOnce((_, _) => throw new NotImplementedException(), "-v3", "--values:1,2,3");
        });

        await Should.ThrowAsync<Exception>(() => task.WaitAsync(TimeSpan.FromSeconds(1)));

        parser.IsValid.ShouldBeFalse();
        parser.Errors.Count.ShouldBe(1);
    }

    [Theory]
    [InlineData("--value 5 --values 1,3,5\n-f -c -c -c\nq")]
    [InlineData("--value 5 --values 1,3,5\n-f -c -c -c\nquit")]
    public async Task ShouldRunOnceAsync2_WhenThrowingException(string argument)
    {
        // Arrange
        var sut = CommandLine.CreateParser<DoubleAppOptions>()
            .ConfigureSettings(s => s.AutoPrintErrors = false)
            .AddNamedOption(s => s.Value)
            .AddNamedOption(s => s.Values)
            .AddSwitchOption(s => s.ValueFlag)
            .AddCounterOption(s => s.ValueCount);

        var reader = new StringReader(argument);

        // Act & Assert
        var parser = sut.Build();
        parser.SetTextReader(reader);

        var task = parser.RunOnceAsync((_, _) => throw new NotImplementedException(), "-v3", "--values:1,2,3");

        await Should.ThrowAsync<Exception>(() => task.WaitAsync(TimeSpan.FromSeconds(1)));

        parser.IsValid.ShouldBeFalse();
        parser.Errors.Count.ShouldBe(1);
    }
}