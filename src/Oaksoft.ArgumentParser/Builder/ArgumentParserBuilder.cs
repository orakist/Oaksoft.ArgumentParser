using System;
using System.Collections.Generic;
using System.Linq;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Exceptions;
using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Builder;

internal sealed class ArgumentParserBuilder<TOptions> : IArgumentParserBuilder<TOptions>
    where TOptions : IApplicationOptions
{
    public bool CaseSensitive { get; }

    public OptionPrefixRules OptionPrefix { get; }

    public AliasDelimiterRules AliasDelimiter { get; }

    public ValueDelimiterRules ValueDelimiter { get; }

    private readonly TOptions _appOptions;
    private readonly IParserSettingsBuilder _settingsBuilder;
    private readonly List<BaseOption> _baseOptions;

    public ArgumentParserBuilder(
        TOptions options, bool caseSensitive, OptionPrefixRules optionPrefix,
        AliasDelimiterRules aliasDelimiter, ValueDelimiterRules valueDelimiter)
    {
        CaseSensitive = caseSensitive;
        OptionPrefix = optionPrefix;
        AliasDelimiter = aliasDelimiter;
        ValueDelimiter = valueDelimiter;

        _appOptions = options;
        _baseOptions = new List<BaseOption>();
        _settingsBuilder = new ParserSettingsBuilder();
    }

    public IParserSettings GetSettings()
    {
        return new ParserSettings
        {
            AutoPrintHeader = _settingsBuilder.AutoPrintHeader ?? true,
            AutoPrintHelp = _settingsBuilder.AutoPrintHelp ?? true,
            AutoPrintErrors = _settingsBuilder.AutoPrintErrors ?? true,

            HelpDisplayWidth = _settingsBuilder.HelpDisplayWidth ?? 75,
            NewLineAfterOption = _settingsBuilder.NewLineAfterOption ?? true,
            ShowTitle = _settingsBuilder.ShowTitle ?? true,
            ShowDescription = _settingsBuilder.ShowDescription ?? true,
            EnableColoring = _settingsBuilder.EnableColoring ??= true,
            Title = _settingsBuilder.Title,
            Description = _settingsBuilder.Description,

            MaxAliasLength = _settingsBuilder.MaxAliasLength ?? 32,
            MaxSuggestedAliasWordCount = _settingsBuilder.MaxSuggestedAliasWordCount ?? 4
        };
    }

    public List<BaseOption> GetBaseOptions()
    {
        return _baseOptions;
    }

    public TOptions GetAppOptions()
    {
        return _appOptions;
    }

    public IArgumentParserBuilder<TOptions> ConfigureSettings(Action<IParserSettingsBuilder> action)
    {
        action.Invoke(_settingsBuilder);
        return this;
    }

    public void RegisterOption(BaseOption option)
    {
        _baseOptions.Add(option);
    }

    public List<string> GetRegisteredPropertyNames()
    {
        var propertyNames = _baseOptions
            .Where(o => !string.IsNullOrWhiteSpace(o.CountProperty?.Name))
            .Select(a => a.CountProperty!.Name)
            .ToList();

        propertyNames.AddRange(_baseOptions.Select(o => o.KeyProperty.Name));

        return propertyNames;
    }

    public IArgumentParser<TOptions> Build()
    {
        BuildDefaultSettings();

        BuildDefaultOptions();

        var parser = new ArgumentParser<TOptions>(_appOptions, this);
        parser.Initialize();

        return parser;
    }

    private void BuildDefaultSettings()
    {
        if ((OptionPrefix & OptionPrefixRules.All) == 0)
        {
            throw BuilderErrors.InvalidEnum.ToException(nameof(OptionPrefixRules), (int)OptionPrefix);
        }

        if ((AliasDelimiter & AliasDelimiterRules.All) == 0)
        {
            throw BuilderErrors.InvalidEnum.ToException(nameof(AliasDelimiterRules), (int)AliasDelimiter);
        }

        if ((ValueDelimiter & ValueDelimiterRules.All) == 0)
        {
            throw BuilderErrors.InvalidEnum.ToException(nameof(ValueDelimiterRules), (int)ValueDelimiter);
        }

        _settingsBuilder.AutoPrintHeader ??= true;
        _settingsBuilder.AutoPrintHelp ??= true;
        _settingsBuilder.AutoPrintErrors ??= true;
        _settingsBuilder.HelpDisplayWidth ??= 75;
        _settingsBuilder.NewLineAfterOption ??= true;
        _settingsBuilder.ShowTitle ??= true;
        _settingsBuilder.ShowDescription ??= true;
        _settingsBuilder.EnableColoring ??= true;
        _settingsBuilder.MaxAliasLength ??= 32;
        _settingsBuilder.MaxSuggestedAliasWordCount ??= 4;

        if (_settingsBuilder.HelpDisplayWidth is < 40 or > 320)
        {
            throw BuilderErrors.OutOfRange.ToException(nameof(IParserSettings.HelpDisplayWidth), (40, 320));
        }

        if (_settingsBuilder.MaxAliasLength is < 8 or > 64)
        {
            throw BuilderErrors.OutOfRange.ToException(nameof(IParserSettings.MaxAliasLength), (8, 64));
        }

        if (_settingsBuilder.MaxSuggestedAliasWordCount is < 1 or > 8)
        {
            throw BuilderErrors.OutOfRange.ToException(nameof(IParserSettings.MaxSuggestedAliasWordCount), (1, 8));
        }

        if (string.IsNullOrWhiteSpace(_settingsBuilder.Title))
            _settingsBuilder.Title = BuildTitleLine();

        if (string.IsNullOrWhiteSpace(_settingsBuilder.Description))
            _settingsBuilder.Description = BuildDescriptionLine();
    }

    private void BuildDefaultOptions()
    {
        if (_baseOptions.Any(o => o.KeyProperty.Name == nameof(IApplicationOptions.Help)))
        {
            throw BuilderErrors.ReservedProperty.ToException(nameof(IApplicationOptions.Help));
        }

        this.AddSwitchOption(p => p.Help);

        var aliases = new[] { "h", "?", "help" };
        var option = _baseOptions.First(o => o.KeyProperty.Name == nameof(IApplicationOptions.Help));

        var validAliases = aliases
            .ValidateAliases(OptionPrefix, CaseSensitive, _settingsBuilder.MaxAliasLength!.Value, false)
            .GetOrThrow();
        
        option.SetName("Help");
        option.SetValidAliases(validAliases);
        option.SetDescription("Prints this help information.");
    }

    private static string? BuildTitleLine()
    {
        var title = AssemblyHelper.GetAssemblyTitle() ?? string.Empty;

        var version = AssemblyHelper.GetAssemblyVersion();
        if (!string.IsNullOrWhiteSpace(version))
            title += $" v{version}";

        var company = AssemblyHelper.GetAssemblyCompany();
        if (!string.IsNullOrWhiteSpace(company))
            title += $", {company}";

        return string.IsNullOrWhiteSpace(title) ? null : title;
    }

    private static string? BuildDescriptionLine()
    {
        var copyright = AssemblyHelper.GetAssemblyCopyright();
        var description = AssemblyHelper.GetAssemblyDescription() ?? string.Empty;

        if (!string.IsNullOrWhiteSpace(copyright))
        {
            var comma = string.IsNullOrWhiteSpace(description) ?
                string.Empty : (description.EndsWith(".") ? " " : ", ");
            description += $"{comma}{copyright}";
        }

        return string.IsNullOrWhiteSpace(description) ? null : description;
    }
}

