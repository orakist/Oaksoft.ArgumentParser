using System.Collections.Generic;
using System.IO;
using System.Linq;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Errors;
using Oaksoft.ArgumentParser.Errors.Builder;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Builder;

internal abstract class BaseArgumentParserBuilder : IArgumentParserBuilder
{
    public bool CaseSensitive { get; }

    public OptionPrefixRules OptionPrefix { get; }

    public AliasDelimiterRules AliasDelimiter { get; }

    public ValueDelimiterRules ValueDelimiter { get; }

    public TextReader? TextReader { get; }

    public TextWriter? TextWriter { get; }

    protected readonly IParserSettingsBuilder _settingsBuilder;
    protected readonly List<BaseOption> _baseOptions;

    protected BaseArgumentParserBuilder(
        bool caseSensitive, OptionPrefixRules optionPrefix,
        AliasDelimiterRules aliasDelimiter, ValueDelimiterRules valueDelimiter,
        TextReader? textReader, TextWriter? textWriter)
    {
        CaseSensitive = caseSensitive;
        OptionPrefix = optionPrefix;
        AliasDelimiter = aliasDelimiter;
        ValueDelimiter = valueDelimiter;
        TextReader = textReader;
        TextWriter = textWriter;

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
            AutoPrintArguments = _settingsBuilder.AutoPrintArguments ?? false,

            HelpDisplayWidth = _settingsBuilder.HelpDisplayWidth ?? 90,
            NewLineAfterOption = _settingsBuilder.NewLineAfterOption ?? true,
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

    public void RegisterOption(BaseOption option)
    {
        _baseOptions.Add(option);
    }

    public List<string> GetRegisteredPropertyNames()
    {
        return _baseOptions.Select(o => o.KeyProperty.Name).ToList();
    }

    protected void BuildDefaultSettings()
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
        _settingsBuilder.AutoPrintArguments ??= false;
        _settingsBuilder.HelpDisplayWidth ??= 90;
        _settingsBuilder.NewLineAfterOption ??= true;
        _settingsBuilder.ShowCopyright ??= false;
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

    private string? BuildTitleLine()
    {
        var title = AssemblyHelper.GetAssemblyTitle() ?? string.Empty;

        var version = AssemblyHelper.GetAssemblyVersion();
        if (!string.IsNullOrWhiteSpace(version))
        {
            title += string.IsNullOrWhiteSpace(title) ? $"v{version}" : $" v{version}";
        }

        var company = AssemblyHelper.GetAssemblyCompany();
        if (!string.IsNullOrWhiteSpace(company))
        {
            title += string.IsNullOrWhiteSpace(title) ? company : $", {company}";
        }

        if (_settingsBuilder.ShowCopyright == true)
        {
            var copyright = AssemblyHelper.GetAssemblyCopyright();
            if (!string.IsNullOrWhiteSpace(copyright))
            {
                title += string.IsNullOrWhiteSpace(title) ? copyright : $", {copyright}";
            }
        }

        return string.IsNullOrWhiteSpace(title) ? null : title;
    }

    private static string? BuildDescriptionLine()
    {
        var description = AssemblyHelper.GetAssemblyDescription() ?? string.Empty;

        return string.IsNullOrWhiteSpace(description) ? null : description;
    }
}
