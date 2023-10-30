using System;
using System.Linq;
using Glowy.CLxParser.Options;

namespace Glowy.CLxParser.Parser;

internal sealed class ArgumentParser<TOptions> : BaseArgumentParser, IArgumentParser<TOptions>
    where TOptions : BaseApplicationOptions, new()
{
    public override BaseApplicationOptions AppOptions
        => _appOptions;

    private readonly TOptions _appOptions;
    private Action<TOptions>? _configureOptions;
    private Action<IParserSettings>? _configureParser;

    public ArgumentParser(
        TOptions options, string commandPrefix, string valueSeparator, string tokenSeparator, bool caseSensitive)
        : base(commandPrefix, valueSeparator, tokenSeparator, caseSensitive)
    {
        _appOptions = options;
    }

    public TOptions GetApplicationOptions()
    {
        return _appOptions;
    }

    public IArgumentParser<TOptions> ConfigureOptions(Action<TOptions> action)
    {
        _configureOptions = action;
        return this;
    }

    public IArgumentParser<TOptions> ConfigureParser(Action<IParserSettings> action)
    {
        _configureParser = action;
        return this;
    }

    public IArgumentParser<TOptions> Build()
    {
        _configureParser?.Invoke(Settings);
        _configureOptions?.Invoke(_appOptions);

        CreateDefaultSettings();

        CreateDefaultOptions();

        InitializeOptions();

        PrintHelpInformation();
        return this;
    }

    public override void Parse(string[] arguments)
    {
        ClearOptions();

        ValidateParserSettings();

        ParseArguments(arguments);

        ValidateOptions(arguments);

        BindOptionsToAttributes();
    }

    public override string GetHeaderText()
    {
        throw new NotImplementedException();
    }

    public override string GetHelpText()
    {
        throw new NotImplementedException();
    }

    public override string GetErrorText()
    {
        throw new NotImplementedException();
    }

    public void PrintHelpInformation()
    {
        if (Settings.ShowTitle is true && !string.IsNullOrWhiteSpace(Settings.Title))
            AssemblyHelper.WriteLine(Settings.Title);
        if (Settings.ShowTitle is true && !string.IsNullOrWhiteSpace(Settings.Description))
            AssemblyHelper.WriteLine(Settings.Description);
        AssemblyHelper.WriteLine("These are command line options of this application.");
        AssemblyHelper.WriteLine();

        var options = _appOptions.Options;
        foreach (var option in options)
        {
            var command = option as ICommandOption;
            var commandName = command?.Command ?? string.Empty;
            AssemblyHelper.Write($"[{commandName,-4}] ", ConsoleColor.DarkGreen);
            AssemblyHelper.Write("Usage: ", ConsoleColor.DarkYellow);
            AssemblyHelper.WriteLine(option.Usage);

            if (command is not null)
            {
                AssemblyHelper.Write("       Commands:", ConsoleColor.DarkYellow);
                AssemblyHelper.WriteLine($" {string.Join(", ", command.Commands)} ");
            }

            if (option.Description is not null)
            {
                var descriptionWords = option.Description.Split(' ');
                var descriptionLines = CreateLinesByWidth(descriptionWords);
                foreach (var description in descriptionLines)
                    AssemblyHelper.WriteLine($"       {description}");
            }

            if (Settings.NewLineAfterOption is true)
                AssemblyHelper.WriteLine();
        }

        var usageLines = CreateLinesByWidth(options.Select(o => o.Usage), true);
        AssemblyHelper.Write("Usage: ", ConsoleColor.DarkYellow);
        AssemblyHelper.WriteLine(usageLines[0]);

        for (var index = 1; index < usageLines.Count; ++index)
            AssemblyHelper.WriteLine($"       {usageLines[index]}");

        AssemblyHelper.WriteLine();
    }
}

