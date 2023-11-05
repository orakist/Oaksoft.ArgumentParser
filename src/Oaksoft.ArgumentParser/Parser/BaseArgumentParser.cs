using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Options;

namespace Oaksoft.ArgumentParser.Parser;

internal abstract class BaseArgumentParser : IArgumentParser
{
    public string OptionPrefix { get; }

    public string ValueDelimiter { get; }

    public string TokenDelimiter { get; }

    public bool CaseSensitive { get; }

    public bool IsValid => _errors.Count < 1;

    public List<string> Errors => _errors.ToList();

    public IParserSettings Settings { get; }

    public abstract BaseApplicationOptions AppOptions { get; }

    protected readonly List<string> _errors;
    private PropertyInfo[]? _propertyInfos;
    private List<string>? _registeredPropertyNames;

    protected BaseArgumentParser(
        string optionPrefix, string valueDelimiter, string tokenDelimiter, bool caseSensitive)
    {
        OptionPrefix = optionPrefix;
        ValueDelimiter = valueDelimiter;
        TokenDelimiter = tokenDelimiter;
        CaseSensitive = caseSensitive;

        Settings = new ParserSettings();
        _errors = new List<string>();
    }

    public abstract string GetHeaderText();

    public abstract string GetHelpText(bool? colorized = default);

    public abstract string GetErrorText(bool? colorized = default);

    protected void BuildDefaultSettings()
    {
        Settings.AutoPrintHeader ??= true;
        Settings.AutoPrintHelp ??= true;
        Settings.AutoPrintErrors ??= true;
        Settings.HelpDisplayWidth ??= 75;
        Settings.NewLineAfterOption ??= true;
        Settings.ShowTitle ??= true;
        Settings.ShowDescription ??= true;
        Settings.EnableColoring ??= true;
        Settings.MaxAliasLength ??= 32;

        ValidateParserSettings();
    }

    protected void ValidateParserSettings()
    {
        if (Settings.HelpDisplayWidth is < 40 or > 320)
        {
            throw new ArgumentOutOfRangeException(nameof(Settings.HelpDisplayWidth),
                "Invalid Help Display Width value! Valid interval is [40, 320].");
        }

        if (Settings.MaxAliasLength is < 4 or > 64)
        {
            throw new ArgumentOutOfRangeException(nameof(Settings.MaxAliasLength),
                "Invalid Max Alias Length value! Valid interval is [4, 64].");
        }

        if (string.IsNullOrWhiteSpace(Settings.Title))
            Settings.Title = BuildTitleLine();

        if (string.IsNullOrWhiteSpace(Settings.Description))
            Settings.Description = BuildDescriptionLine();
    }

    protected void BuildDefaultOptions()
    {
        var options = AppOptions.Options.Cast<BaseOption>();
        if (options.Any(o => o.KeyProperty.Name == nameof(AppOptions.Help)))
            return;

        AppOptions.AddSwitchOption(o => o.Help)
            .WithDescription("Prints this help information.");
    }

    protected void InitializeOptions()
    {
        var options = AppOptions.Options;
        var aliases = new List<string>();

        foreach (var option in options.Cast<BaseOption>())
        {
            ValidateOptionAliases(option, aliases);

            option.Initialize(this);
        }

        aliases = aliases.GroupBy(c => c)
            .Where(c => c.Count() > 1)
            .Select(c => c.Key)
            .ToList();

        if (!aliases.Any())
            return;

        var aliasText = string.Join(", ", aliases);
        throw new Exception($"Option aliases must be unique. Duplicate Aliases: {aliasText}");
    }

    protected void ClearOptions()
    {
        _errors.Clear();

        ClearOptionPropertiesByReflection();
    }

    protected void ParseArguments(string[] arguments)
    {
        var options = AppOptions.Options;
        foreach (var option in options.Cast<BaseOption>())
        {
            try
            {
                option.Parse(arguments, this);
            }
            catch (Exception ex)
            {
                AddErrorMessage(option, ex);
            }
        }
    }

    protected void ValidateOptions(string[] arguments)
    {
        var options = AppOptions.Options;
        foreach (var option in options.Cast<BaseOption>())
        {
            try
            {
                option.Validate(this);
            }
            catch (Exception ex)
            {
                AddErrorMessage(option, ex);
            }
        }

        ValidateHelpToken(options);

        var values = options.OfType<IValueOption>().SelectMany(a => a.ValueTokens).ToList();
        var inputs = options.OfType<INamedOption>().SelectMany(a => a.OptionTokens).ToList();
        var invalidOptions = arguments.Where(s => !values.Contains(s)).ToList();
        invalidOptions = invalidOptions.Where(s => !inputs.Contains(s)).ToList();

        foreach (var option in invalidOptions)
            _errors.Add($"Unknown option found. Option: {option}");
    }

    protected void BindOptionsToAttributes()
    {
        if (_errors.Count > 0)
            return;

        foreach (var option in AppOptions.Options)
        {
            try
            {
                UpdateOptionPropertiesByReflection(option);
            }
            catch (Exception ex)
            {
                AddErrorMessage(option, ex);
            }
        }
    }

    protected List<string> CreateLinesByWidth(
        IEnumerable<string> textWords, bool addBrackets = false)
    {
        var displayWidth = Settings.HelpDisplayWidth!.Value;
        var textLines = new List<string> { string.Empty };

        foreach (var word in textWords)
        {
            if (textLines[^1].Length > displayWidth)
                textLines.Add(string.Empty);

            var space = textLines[^1].Length > 0 ? " " : string.Empty;
            var newWord = addBrackets ? $"[{word}]" : word;

            textLines[^1] += $"{space}{newWord}";
        }

        return textLines;
    }

    protected static bool IsOnlyOption(IBaseOption option, IReadOnlyList<IBaseOption> options)
    {
        var optionCount = option switch
        {
            INamedOption c => c.OptionTokens.Count,
            IValueOption d => d.ValueTokens.Count,
            _ => 0
        };

        if (optionCount < 1)
            return false;

        var totalInputCount = options.Sum(o => o switch
        {
            INamedOption c => c.OptionTokens.Count,
            IValueOption d => d.ValueTokens.Count,
            _ => 0
        });

        return totalInputCount - optionCount < 1;
    }

    protected void ValidateHelpToken(IReadOnlyList<IBaseOption> options)
    {
        var helpOption = options.OfType<SwitchOption>().
            First(o => o.KeyProperty.Name == nameof(IApplicationOptions.Help));

        if (IsOnlyOption(helpOption, options))
        {
            _errors.Clear();
        }
        else if (helpOption.OptionTokens.Count > 0)
        {
            _errors.Add($"{helpOption.Name} ({helpOption.ShortAlias}) option cannot be combined with other options.");
        }
    }

    private void ValidateOptionAliases(BaseOption option, List<string> aliases)
    {
        if (option is not INamedOption namedOption)
            return;

        if (namedOption.Aliases.Count > 0)
        {
            var optionAliases = CaseSensitive
                ? namedOption.Aliases
                : namedOption.Aliases.Select(a => a.ToLowerInvariant());

            aliases.AddRange(optionAliases);
            return;
        }

        // suggest aliases for option by using the registered property name
        var autoAliases = AliasHelper.GetAliasesHeuristically(
            option.KeyProperty.Name, aliases, Settings.MaxAliasLength!.Value, CaseSensitive);

        if (!CaseSensitive)
            autoAliases = autoAliases.Select(a => a.ToLowerInvariant());

        var suggestedAliases = autoAliases.ToArray();
        option.SetAliases(suggestedAliases);
        aliases.AddRange(suggestedAliases);
    }

    private void AddErrorMessage(IBaseOption option, Exception ex)
    {
        var name = (option as INamedOption)?.ShortAlias ?? option.Name;
        var comma = ex.Message.EndsWith(".") ? string.Empty : ",";
        _errors.Add($"{ex.Message}{comma} Option: {name}");
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

    private void ClearOptionPropertiesByReflection()
    {
        var options = AppOptions.Options.Cast<BaseOption>().ToList();
        _registeredPropertyNames ??= GetRegisteredPropertyNames(options);

        _propertyInfos ??= AppOptions.GetType().GetProperties()
            .Where(p => _registeredPropertyNames.Contains(p.Name))
            .ToArray();

        foreach (var option in options)
        {
            option.Clear();
        }

        foreach (var property in _propertyInfos)
        {
            var type = property.PropertyType;
            var defaultValue = type.IsValueType ? Activator.CreateInstance(type) : null;
            property.SetValue(AppOptions, defaultValue);
        }
    }

    private void UpdateOptionPropertiesByReflection(IBaseOption option)
    {
        if (!option.IsActive)
            return;

        var baseOption = (option as BaseOption)!;
        if (baseOption.CountProperty is not null)
        {
            var countProp = _propertyInfos!.First(p => p.Name == baseOption.CountProperty.Name);
            if (countProp.PropertyType == typeof(bool))
            {
                countProp.SetValue(AppOptions, true);
            }
            else if (countProp.PropertyType == typeof(int))
            {
                countProp.SetValue(AppOptions, option is INamedOption ? option.OptionCount : option.ValueCount);
            }
        }

        var keyProp = _propertyInfos!.First(p => p.Name == baseOption.KeyProperty.Name);
        var type = keyProp.PropertyType;
        if (option is SwitchOption)
        {
            if (type == typeof(bool))
            {
                keyProp.SetValue(AppOptions, true);
            }
            else if (type == typeof(int))
            {
                keyProp.SetValue(AppOptions, option is INamedOption ? option.OptionCount : option.ValueCount);
            }
        }
        else if (option is BaseValueOption valOption)
        {
            valOption.UpdatePropertyValue(AppOptions, keyProp);
        }
    }

    private static List<string> GetRegisteredPropertyNames(IReadOnlyList<BaseOption> options)
    {
        var propertyNames = options
            .Where(o => !string.IsNullOrWhiteSpace(o.CountProperty?.Name))
            .Select(a => a.CountProperty!.Name)
            .ToList();

        propertyNames.AddRange(options.Select(o => o.KeyProperty.Name));

        return propertyNames;
    }
}
