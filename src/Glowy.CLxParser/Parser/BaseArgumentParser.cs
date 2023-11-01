﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Oaksoft.ArgumentParser.Extensions;
using Oaksoft.ArgumentParser.Options;

namespace Oaksoft.ArgumentParser.Parser;

internal abstract class BaseArgumentParser : IArgumentParser
{
    public string CommandPrefix { get; }

    public string ValueSeparator { get; }

    public string TokenSeparator { get; }

    public bool CaseSensitive { get; }

    public bool IsValid => _errors.Count < 1;

    public List<string> Errors => _errors.ToList();

    public IParserSettings Settings { get; }

    public abstract BaseApplicationOptions AppOptions { get; }

    protected readonly List<string> _errors;
    private PropertyInfo[]? _propertyInfos;
    private List<string>? _registeredPropertyNames;

    protected BaseArgumentParser(
        string commandPrefix, string valueSeparator, string tokenSeparator, bool caseSensitive)
    {
        CommandPrefix = commandPrefix;
        ValueSeparator = valueSeparator;
        TokenSeparator = tokenSeparator;
        CaseSensitive = caseSensitive;

        Settings = new ParserSettings();
        _errors = new List<string>();
    }

    public abstract string GetHeaderText();

    public abstract string GetHelpText(bool? colorized = default);

    public abstract string GetErrorText(bool? colorized = default);

    protected void CreateDefaultSettings()
    {
        Settings.AutoPrintHeader ??= true;
        Settings.AutoPrintHelp ??= true;
        Settings.AutoPrintErrors ??= true;
        Settings.HelpDisplayWidth ??= 75;
        Settings.NewLineAfterOption ??= true;
        Settings.ShowTitle ??= true;
        Settings.ShowDescription ??= true;
        Settings.EnableColoring ??= true;

        ValidateParserSettings();
    }

    protected void ValidateParserSettings()
    {
        if (Settings.HelpDisplayWidth is < 40 or > 320)
        {
            throw new ArgumentOutOfRangeException(nameof(Settings.HelpDisplayWidth),
                "Invalid Help Display Width value! Valid interval is [40, 320].");
        }

        if (string.IsNullOrWhiteSpace(Settings.Title))
            Settings.Title = BuildTitleLine();

        if (string.IsNullOrWhiteSpace(Settings.Description))
            Settings.Description = BuildDescriptionLine();
    }

    protected void CreateDefaultOptions()
    {
        var options = AppOptions.Options.Cast<BaseOption>();
        if (options.Any(o => o.KeyProperty == nameof(AppOptions.Help)))
            return;

        AppOptions.AddSwitchOption(o => o.Help)
            .WithDescription("Prints this help information.");
    }

    protected void InitializeOptions()
    {
        var options = AppOptions.Options;
        foreach (var option in options)
            ((BaseOption)option).Initialize(this);

        var commands = options.OfType<ICommandOption>()
            .SelectMany(o => o.Commands)
            .Select(a => a.ToLowerInvariant())
            .GroupBy(c => c)
            .Where(c => c.Count() > 1)
            .Select(c => c.Key)
            .ToList();

        if (!commands.Any())
            return;

        var commandText = string.Join(", ", commands);
        throw new Exception($"Option command names must be unique. Duplicate Commands: {commandText}");
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

        var values = options.OfType<IHaveValueOption>().SelectMany(a => a.ValueTokens).ToList();
        var inputs = options.OfType<ICommandOption>().SelectMany(a => a.CommandTokens).ToList();
        var invalidOptions = arguments.Where(s => !values.Contains(s)).ToList();
        invalidOptions = invalidOptions.Where(s => !inputs.Contains(s)).ToList();

        foreach (var option in invalidOptions)
            _errors.Add($"Unknown command found. Command: {option}");
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
            ICommandOption c => c.CommandTokens.Count,
            IHaveValueOption d => d.ValueTokens.Count,
            _ => 0
        };

        if (optionCount < 1)
            return false;

        var totalInputCount = options.Sum(o => o switch
        {
            ICommandOption c => c.CommandTokens.Count,
            IHaveValueOption d => d.ValueTokens.Count,
            _ => 0
        });

        return totalInputCount - optionCount < 1;
    }

    protected void ValidateHelpToken(IReadOnlyList<IBaseOption> options)
    {
        var helpOption = options.OfType<SwitchOption>().
            First(o => o.KeyProperty == nameof(IApplicationOptions.Help));

        if (IsOnlyOption(helpOption, options))
        {
            _errors.Clear();
        }
        else if (helpOption.CommandTokens.Count > 0)
        {
            _errors.Add($"{helpOption.Name} ({helpOption.Command}) command cannot be combined with other commands.");
        }
    }

    private void AddErrorMessage(IBaseOption option, Exception ex)
    {
        var name = (option as ICommandOption)?.Command ?? option.Name;
        var comma = ex.Message.EndsWith(".") ? string.Empty : ",";
        _errors.Add($"{ex.Message}{comma} Name: {name}");
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
            var option = options.FirstOrDefault(o => o.KeyProperty == property.Name);
            if (option is IHaveValueOption valOption && !string.IsNullOrWhiteSpace(valOption.DefaultValue))
            {
                // all scalar-command and non-command options may have a default option.
                // so apply default option to registered property if it exists.
                if (type.IsAssignableFrom(typeof(string)))
                {
                    property.SetValue(AppOptions, valOption.DefaultValue);
                    continue;
                }

                if (option is ScalarCommandOption scalarOption)
                {
                    scalarOption.ApplyDefaultValue(AppOptions, property);
                    continue;
                }
            }

            var defaultValue = type.IsValueType ? Activator.CreateInstance(type) : null;
            property.SetValue(AppOptions, defaultValue);
        }
    }

    private void UpdateOptionPropertiesByReflection(IBaseOption option)
    {
        if (option.ValidatedTokenCount < 1)
            return;

        var baseOption = (option as BaseOption)!;
        if (!string.IsNullOrWhiteSpace(baseOption.CountProperty))
        {
            var countProp = _propertyInfos!.First(p => p.Name == baseOption.CountProperty);
            if (countProp.PropertyType == typeof(bool))
            {
                countProp.SetValue(AppOptions, true);
            }
            else if (countProp.PropertyType == typeof(int))
            {
                countProp.SetValue(AppOptions, option.ValidatedTokenCount);
            }
        }

        var keyProp = _propertyInfos!.First(p => p.Name == baseOption.KeyProperty);
        var type = keyProp.PropertyType;
        if (option is SwitchOption)
        {
            if (type == typeof(bool))
            {
                keyProp.SetValue(AppOptions, true);
            }
            else if (type == typeof(int))
            {
                keyProp.SetValue(AppOptions, option.ValidatedTokenCount);
            }
        }
        else if (option is IHaveValueOption valOption)
        {
            if (type.IsAssignableFrom(typeof(List<string>)))
            {
                keyProp.SetValue(AppOptions, valOption.ParsedValues);
            }
            else if (type.IsAssignableFrom(typeof(string[])))
            {
                keyProp.SetValue(AppOptions, valOption.ParsedValues.ToArray());
            }
            else if (type.IsAssignableFrom(typeof(string)))
            {
                keyProp.SetValue(AppOptions, valOption.ParsedValues.First());
            }
            else if (option is ScalarCommandOption scalarOption)
            {
                scalarOption.UpdatePropertyValue(AppOptions, keyProp);
            }
        }
    }

    private List<string> GetRegisteredPropertyNames(IReadOnlyList<BaseOption> options)
    {
        var propertyNames = options
            .Where(o => !string.IsNullOrWhiteSpace(o.CountProperty))
            .Select(a => a.CountProperty!)
            .ToList();

        propertyNames.AddRange(options.Select(o => o.KeyProperty));

        return propertyNames;
    }
}
