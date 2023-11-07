using System;
using System.Linq;
using System.Text;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Options;

namespace Oaksoft.ArgumentParser.Parser;

internal sealed class ArgumentParser<TOptions> : BaseArgumentParser, IArgumentParser<TOptions>
    where TOptions : BaseApplicationOptions, new()
{
    public override BaseApplicationOptions AppOptions
        => _appOptions;

    private readonly TOptions _appOptions;
    private Action<TOptions>? _configureOptions;
    private Action<IParserSettingsBuilder>? _configureParser;

    public ArgumentParser(
        TOptions options, OptionPrefixRules optionPrefix, TokenDelimiterRules tokenDelimiter,
        ValueDelimiterRules valueDelimiter, bool caseSensitive)
        : base(optionPrefix, tokenDelimiter, valueDelimiter, caseSensitive)
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

    public IArgumentParser<TOptions> ConfigureParser(Action<IParserSettingsBuilder> action)
    {
        _configureParser = action;
        return this;
    }

    public IArgumentParser<TOptions> Build()
    {
        _configureParser?.Invoke(_settingsBuilder);
        _configureOptions?.Invoke(_appOptions);

        BuildDefaultSettings();

        BuildDefaultOptions();

        InitializeOptions();

        PrintHeaderText();

        return this;
    }

    public TOptions Parse(string[] arguments)
    {
        ClearOptions();

        var tokens = arguments
            .Select(a => new TokenValue { Argument = a })
            .ToArray();

        ValidateArguments(tokens);

        ParseArguments(tokens);

        ValidateOptions(tokens);

        BindOptionsToAttributes();

        PrintHelpText();

        PrintErrorText();

        return _appOptions;
    }

    public override string GetHeaderText()
    {
        return BuildHeaderText(true, true).ToString();
    }

    public override string GetHelpText(bool? enableColoring = default)
    {
        var coloring = enableColoring ?? Settings.EnableColoring ?? true;
        return BuildHelpText(coloring).ToString();
    }

    public override string GetErrorText(bool? enableColoring = default)
    {
        var coloring = enableColoring ?? Settings.EnableColoring ?? true;
        return BuildErrorText(coloring).ToString();
    }

    private void PrintHeaderText()
    {
        if (Settings.AutoPrintHeader != true)
            return;

        Console.Write(BuildHeaderText(true, true).ToString());
        Console.WriteLine();
    }

    private void PrintHelpText()
    {
        if (Settings.AutoPrintHelp != true || _errors.Count > 0)
            return;

        var options = _appOptions.Options;
        var helpOption = _appOptions.Options.OfType<SwitchOption>().
            First(o => o.KeyProperty.Name == nameof(IApplicationOptions.Help));

        if (!IsOnlyOption(helpOption, options))
            return;

        Console.Write(BuildHelpText(Settings.EnableColoring ?? true).ToString());
        Console.WriteLine();
    }

    private void PrintErrorText()
    {
        if (Settings.AutoPrintErrors != true || _errors.Count < 1)
            return;

        Console.Write(BuildErrorText(Settings.EnableColoring ?? true).ToString());
        Console.WriteLine();
    }

    private StringBuilder BuildHeaderText(bool showTitle, bool showDescription)
    {
        var sb = new StringBuilder();

        if (showTitle && !string.IsNullOrWhiteSpace(Settings.Title))
            sb.AppendLine(Settings.Title);
        if (showDescription && !string.IsNullOrWhiteSpace(Settings.Description))
            sb.AppendLine(Settings.Description);

        return sb;
    }

    private StringBuilder BuildHelpText(bool enableColoring)
    {
        TextColoring.SetEnabled(enableColoring);

        var sb = BuildHeaderText(Settings.ShowTitle ?? true, Settings.ShowDescription ?? true);
        sb.AppendLine("These are command line options of this application.");
        sb.AppendLine();

        var options = _appOptions.Options;
        foreach (var option in options)
        {
            var namedOption = option as INamedOption;
            var shortAlias = namedOption?.ShortAlias ?? string.Empty;
            sb.Pastel($"[{shortAlias,-4}] ", ConsoleColor.DarkGreen);
            sb.Pastel("Usage: ", ConsoleColor.DarkYellow);
            sb.AppendLine(option.Usage);

            if (namedOption is not null)
            {
                sb.Pastel("       Aliases:", ConsoleColor.DarkYellow);
                sb.AppendLine($" {string.Join(", ", namedOption.Aliases.OrderBy(n => n[0] == '/').ThenBy(n => n.Length))} ");
            }

            if (option.Description is not null)
            {
                var descriptionWords = option.Description.Split(' ');
                var descriptionLines = CreateLinesByWidth(descriptionWords);
                foreach (var description in descriptionLines)
                    sb.AppendLine($"       {description}");
            }

            if (Settings.NewLineAfterOption is true)
                sb.AppendLine();
        }

        var usageLines = CreateLinesByWidth(options.Select(o => o.Usage), true);
        sb.Pastel("Usage: ", ConsoleColor.DarkYellow);
        sb.AppendLine(usageLines[0]);

        for (var index = 1; index < usageLines.Count; ++index)
            sb.AppendLine($"       {usageLines[index]}");

        return sb;
    }

    private StringBuilder BuildErrorText(bool enableColoring)
    {
        var sb = new StringBuilder();
        if (_errors.Count < 1)
            return sb;

        TextColoring.SetEnabled(enableColoring);
        
        sb.Pastel("     Error(s)!", ConsoleColor.Red);
        sb.AppendLine();

        for (int i = 0; i < _errors.Count; i++)
        {
            sb.Pastel($"{(i + 1):00} - ", ConsoleColor.DarkYellow);
            sb.AppendLine(_errors[i]);
        }

        return sb;
    }
}

