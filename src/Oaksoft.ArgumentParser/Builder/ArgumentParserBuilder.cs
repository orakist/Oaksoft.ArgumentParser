using System;
using System.Collections.Generic;
using System.Linq;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Errors;
using Oaksoft.ArgumentParser.Errors.Builder;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Builder;

internal sealed class ArgumentParserBuilder<TOptions> : IArgumentParserBuilder<TOptions>
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
            AutoPrintVersion = _settingsBuilder.AutoPrintVersion ?? true,
            AutoPrintErrors = _settingsBuilder.AutoPrintErrors ?? true,

            HelpDisplayWidth = _settingsBuilder.HelpDisplayWidth ?? 80,
            NewLineAfterOption = _settingsBuilder.NewLineAfterOption ?? true,
            ShowTitle = _settingsBuilder.ShowTitle ?? true,
            ShowDescription = _settingsBuilder.ShowDescription ?? true,
            EnableColoring = _settingsBuilder.EnableColoring ??= true,
            Title = _settingsBuilder.Title,
            Description = _settingsBuilder.Description,
            VerbosityLevel = _settingsBuilder.VerbosityLevel ?? VerbosityLevelType.Minimal,

            MaxAliasLength = _settingsBuilder.MaxAliasLength ?? 32,
            MaxSuggestedAliasWordCount = _settingsBuilder.MaxSuggestedAliasWordCount ?? 4
        };
    }

    public List<BaseOption> GetBaseOptions()
    {
        return _baseOptions;
    }

    public TOptions GetApplicationOptions()
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
        return _baseOptions.Select(o => o.KeyProperty.Name).ToList();
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

        if (AliasDelimiter != 0 && (AliasDelimiter & AliasDelimiterRules.All) == 0)
        {
            throw BuilderErrors.InvalidEnum.ToException(nameof(AliasDelimiterRules), (int)AliasDelimiter);
        }

        if (ValueDelimiter != 0 && (ValueDelimiter & ValueDelimiterRules.All) == 0)
        {
            throw BuilderErrors.InvalidEnum.ToException(nameof(ValueDelimiterRules), (int)ValueDelimiter);
        }

        _settingsBuilder.AutoPrintHeader ??= true;
        _settingsBuilder.AutoPrintHelp ??= true;
        _settingsBuilder.AutoPrintVersion ??= true;
        _settingsBuilder.AutoPrintErrors ??= true;
        _settingsBuilder.HelpDisplayWidth ??= 80;
        _settingsBuilder.NewLineAfterOption ??= true;
        _settingsBuilder.ShowTitle ??= true;
        _settingsBuilder.ShowDescription ??= true;
        _settingsBuilder.VerbosityLevel ??= VerbosityLevelType.Minimal;
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
        {
            _settingsBuilder.Title = BuildTitleLine();
        }

        if (string.IsNullOrWhiteSpace(_settingsBuilder.Description))
        {
            _settingsBuilder.Description = BuildDescriptionLine();
        }
    }

    private void BuildDefaultOptions()
    {
        // add help option
        BuildHelpOption();

        // add version option
        BuildVersionOption();

        // add verbosity option
        BuildVerbosityOption();
    }

    private void BuildVersionOption()
    {
        if (_baseOptions.Any(o => o.KeyProperty.Name == nameof(IBuiltInOptions.Version)))
        {
            throw BuilderErrors.ReservedProperty.ToException(nameof(IBuiltInOptions.Version));
        }

        var properties = typeof(BuiltInOptions).GetProperties();
        var keyProperty = properties.First(p => p.Name == nameof(IBuiltInOptions.Version));
        this.RegisterSwitchOption<TOptions>(keyProperty, false);

        string[] aliases = new[] { "VN", "Version" };
        var option = _baseOptions.First(o => o.KeyProperty.Name == nameof(IBuiltInOptions.Version));

        List<string> validAliases = aliases
            .ValidateAliases(OptionPrefix, CaseSensitive, _settingsBuilder.MaxAliasLength!.Value, false)
            .GetOrThrow();

        option.SetName(nameof(IBuiltInOptions.Version), false);
        option.SetValidAliases(validAliases);
        option.SetDescription("Shows version-number of the application.");
        ((BaseValueOption)option).SetValueArity(ArityType.Zero);
    }

    private void BuildHelpOption()
    {
        if (_baseOptions.Any(o => o.KeyProperty.Name == nameof(IBuiltInOptions.Help)))
        {
            throw BuilderErrors.ReservedProperty.ToException(nameof(IBuiltInOptions.Help));
        }

        var properties = typeof(BuiltInOptions).GetProperties();
        var keyProperty = properties.First(p => p.Name == nameof(IBuiltInOptions.Help));
        this.RegisterSwitchOption<TOptions>(keyProperty, false);

        var aliases = new[] { "h", "?", "Help" };
        var option = _baseOptions.First(o => o.KeyProperty.Name == nameof(IBuiltInOptions.Help));

        var validAliases = aliases
            .ValidateAliases(OptionPrefix, CaseSensitive, _settingsBuilder.MaxAliasLength!.Value, false)
            .GetOrThrow();

        option.SetName(nameof(IBuiltInOptions.Help), false);
        option.SetValidAliases(validAliases);
        option.SetDescription("Shows help and usage information.");
        ((BaseValueOption)option).SetValueArity(ArityType.Zero);
    }

    private void BuildVerbosityOption()
    {
        if (_baseOptions.Any(o => o.KeyProperty.Name == nameof(IBuiltInOptions.Verbosity)))
        {
            throw BuilderErrors.ReservedProperty.ToException(nameof(IBuiltInOptions.Verbosity));
        }

        var properties = typeof(BuiltInOptions).GetProperties();
        var keyProperty = properties.First(p => p.Name == nameof(IBuiltInOptions.Verbosity));
        this.RegisterNamedOption<TOptions, VerbosityLevelType>(keyProperty, false, false);

        var aliases = new[] { "VL", "Verbosity" };
        var option = _baseOptions.First(o => o.KeyProperty.Name == nameof(IBuiltInOptions.Verbosity));


        var validAliases = aliases
            .ValidateAliases(OptionPrefix, CaseSensitive, _settingsBuilder.MaxAliasLength!.Value, false)
            .GetOrThrow();

        option.SetHidden(true);
        option.SetName(nameof(IBuiltInOptions.Verbosity), false);
        option.SetValidAliases(validAliases);
        option.SetDescription("Sets verbosity-level that specifies how much output is sent to the console.");

        var scalarOption = option as ScalarNamedOption<VerbosityLevelType>;
        scalarOption!.SetDefaultValue(_settingsBuilder.VerbosityLevel!.Value);
    }

    private static string? BuildTitleLine()
    {
        var title = AssemblyHelper.GetAssemblyTitle() ?? string.Empty;

        var version = AssemblyHelper.GetAssemblyVersion();
        if (!string.IsNullOrWhiteSpace(version))
        {
            title += $" v{version}";
        }

        var company = AssemblyHelper.GetAssemblyCompany();
        if (!string.IsNullOrWhiteSpace(company))
        {
            title += $", {company}";
        }

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

