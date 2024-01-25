﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Errors;
using Oaksoft.ArgumentParser.Errors.Builder;
using Oaksoft.ArgumentParser.Errors.Parser;
using Oaksoft.ArgumentParser.Options;

namespace Oaksoft.ArgumentParser.Parser;

internal abstract class BaseArgumentParser : IArgumentParser
{
    public bool CaseSensitive { get; }

    public OptionPrefixRules OptionPrefix { get; }

    public AliasDelimiterRules AliasDelimiter { get; }

    public ValueDelimiterRules ValueDelimiter { get; }

    public abstract IParserSettings Settings { get; }

    public bool IsValid => _errors.Count < 1;

    public bool IsEmpty { get; private set; }

    public abstract bool IsHelpOption { get; }

    public abstract bool IsVersionOption { get; }

    public abstract VerbosityLevelType VerbosityLevel { get; }

    public List<IErrorMessage> Errors => _errors.ToList();

    protected readonly List<BaseOption> _baseOptions;
    protected readonly List<PropertyInfo> _propertyInfos;
    protected readonly List<IErrorMessage> _errors;
    private readonly List<string> _allAliases;

    protected BaseArgumentParser(
        bool caseSensitive, OptionPrefixRules optionPrefix,
        AliasDelimiterRules aliasDelimiter, ValueDelimiterRules valueDelimiter)
    {
        CaseSensitive = caseSensitive;
        OptionPrefix = optionPrefix;
        AliasDelimiter = aliasDelimiter;
        ValueDelimiter = valueDelimiter;

        _baseOptions = new List<BaseOption>();
        _propertyInfos = new List<PropertyInfo>();

        _errors = new List<IErrorMessage>();
        _allAliases = new List<string>();
    }

    public List<IBaseOption> GetOptions()
    {
        return _baseOptions
            .Where(o => !AliasExtensions.BuiltInOptionNames.Contains(o.Name))
            .Cast<IBaseOption>()
            .ToList();
    }

    public IBaseOption? GetOptionByName(string name)
    {
        return _baseOptions.FirstOrDefault(
            o => o.Name.Equals(name, StringComparison.OrdinalIgnoreCase) ||
                 o.KeyProperty.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public INamedOption? GetOptionByAlias(string alias)
    {
        var flag = CaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

        return _baseOptions.OfType<INamedOption>()
            .FirstOrDefault(o => o.Aliases.Any(a => a.Equals(alias, flag)));
    }

    public string GetHeaderText()
    {
        return BuildHeaderText(true, true).ToString();
    }

    public string GetHelpText(bool? enableColoring = default)
    {
        var coloring = enableColoring ?? Settings.EnableColoring;
        return BuildHelpText(coloring).ToString();
    }

    public string GetErrorText(bool? enableColoring = default)
    {
        var coloring = enableColoring ?? Settings.EnableColoring;
        return BuildErrorText(coloring).ToString();
    }

    protected void InitializeOptions()
    {
        var names = new List<string>();
        var aliases = new List<string>();
        ValidateCustomNames(names, aliases);

        foreach (var option in _baseOptions)
        {
            AutoInitializeOptionName(option, names);

            AutoInitializeOptionAliases(option, aliases);

            option.SetParser(this);

            option.Initialize();
        }

        _allAliases.AddRange(aliases.OrderByDescending(a => a.Length));
    }

    protected void ParseTokens(string[] arguments)
    {
        try
        {
            ClearOptions();

            var tokens = PrepareTokens(arguments);
            if (tokens.Length < 1)
            {
                IsEmpty = true;
                return;
            }

            ParseOptions(tokens);

            ValidateOptions(tokens);

            AutoPrintHelpText();

            AutoPrintVersion();
        }
        catch (Exception ex)
        {
            var error = new ErrorInfo($"{ParserErrors.Name}.UnexpectedError", ex.Message);
            _errors.Add(error.WithException(ex));
        }
        finally
        {
            BindOptionsToAttributes();
        }
    }

    protected abstract void ClearOptionPropertiesByReflection();

    protected abstract void UpdateOptionPropertiesByReflection(BaseOption option);

    private void ValidateCustomNames(ICollection<string> names, ICollection<string> aliases)
    {
        foreach (var option in _baseOptions)
        {
            if (string.IsNullOrEmpty(option.Name))
            {
                continue;
            }

            if (names.Contains(option.Name))
            {
                throw BuilderErrors.NameAlreadyInUse.WithName(option.KeyProperty.Name).ToException(option.Name);
            }

            names.Add(option.Name);
        }

        foreach (var option in _baseOptions)
        {
            if (option is not INamedOption)
            {
                continue;
            }

            if (option.GetAliases().Count < 1)
            {
                continue;
            }

            var validAliases = option.GetAliases()
                .ValidateAliases(OptionPrefix, CaseSensitive, Settings.MaxAliasLength, true)
                .GetOrThrow(option.KeyProperty.Name)
                .Distinct().ToList();

            foreach (var alias in validAliases)
            {
                if (aliases.Contains(alias))
                {
                    throw BuilderErrors.AliasAlreadyInUse.WithName(option.KeyProperty.Name).ToException(alias);
                }

                aliases.Add(alias);
            }

            option.SetValidAliases(validAliases);
        }
    }

    private void ClearOptions()
    {
        _errors.Clear();
        IsEmpty = false;

        foreach (var option in _baseOptions)
        {
            option.Clear();
        }

        ClearOptionPropertiesByReflection();
    }

    private TokenItem[] PrepareTokens(string[] arguments)
    {
        var tokens = arguments
            .Where(a => !string.IsNullOrEmpty(a))
            .Select(a => new TokenItem { Token = a })
            .ToArray();

        foreach (var token in tokens)
        {
            try
            {
                token.ExtractAliasAndValue(
                    _allAliases, CaseSensitive, OptionPrefix, AliasDelimiter);
            }
            catch (OptionParserException ex)
            {
                _errors.Add(ex.Error);
                token.Invalid = true;
            }
        }

        return tokens;
    }

    private void ParseOptions(TokenItem[] tokens)
    {
        var orderedOptions = new List<BaseOption>();
        orderedOptions.AddRange(_baseOptions.Where(o => o is ISwitchOption));
        orderedOptions.AddRange(_baseOptions.Where(o => o is ICounterOption));
        orderedOptions.AddRange(_baseOptions.Where(o => o is IScalarNamedOption and not ISwitchOption));
        orderedOptions.AddRange(_baseOptions.Where(o => o is ISequentialNamedOption));
        orderedOptions.AddRange(_baseOptions.Where(o => o is not INamedOption));

        foreach (var option in orderedOptions)
        {
            try
            {
                option.Parse(tokens);
            }
            catch (OptionParserException ex)
            {
                _errors.Add(ex.Error.WithName(option.Name));
            }
        }
    }

    private void ValidateOptions(ICollection<TokenItem> tokens)
    {
        foreach (var option in _baseOptions)
        {
            try
            {
                option.Validate();
            }
            catch (OptionParserException ex)
            {
                _errors.Add(ex.Error.WithName(option.Name));
            }
            catch (Exception ex)
            {
                var error = new ErrorInfo($"{ParserErrors.Name}.UnexpectedError", ex.Message);
                _errors.Add(error.WithName(option.Name).WithException(ex));
            }
        }

        ValidateBuiltInTokens(tokens.All(t => t.IsParsed));

        foreach (var token in tokens.Where(s => s is { IsParsed: false, Invalid: false }))
        {
            _errors.Add(ParserErrors.UnknownToken.With(token.Token));
        }
    }

    private void BindOptionsToAttributes()
    {
        var options = _errors.Count > 0 
            ? _baseOptions.Where(n => n.Name == nameof(IBuiltInOptions.Verbosity))
            : _baseOptions;

        foreach (var option in options)
        {
            try
            {
                UpdateOptionPropertiesByReflection(option);
            }
            catch (OptionParserException ex)
            {
                _errors.Add(ex.Error.WithName(option.Name));
            }
            catch (Exception ex)
            {
                var error = new ErrorInfo($"{ParserErrors.Name}.UnexpectedError", ex.Message);
                _errors.Add(error.WithName(option.Name).WithException(ex));
            }
        }
    }

    protected void AutoPrintHeaderText()
    {
        if (Settings.AutoPrintHeader != true)
        {
            return;
        }

        Console.Write(BuildHeaderText(true, true).ToString());
        Console.WriteLine();
    }

    private void AutoPrintHelpText()
    {
        if (_errors.Count > 0 || Settings.AutoPrintHelp != true)
        {
            return;
        }

        var helpOption = _baseOptions.OfType<SwitchOption>().
            First(o => o.KeyProperty.Name == nameof(IBuiltInOptions.Help));

        if (!IsOnlyOption(helpOption))
        {
            return;
        }

        Console.Write(BuildHelpText(Settings.EnableColoring).ToString());
        Console.WriteLine();
    }

    private void AutoPrintVersion()
    {
        if (_errors.Count > 0 || Settings.AutoPrintVersion != true)
        {
            return;
        }

        var versionOption = _baseOptions.OfType<SwitchOption>().
            First(o => o.KeyProperty.Name == nameof(IBuiltInOptions.Version));

        if (!IsOnlyOption(versionOption))
        {
            return;
        }

        Console.Write(AssemblyHelper.GetAssemblyVersion());
        Console.WriteLine();
    }

    protected void AutoPrintErrorText()
    {
        if (Settings.AutoPrintErrors != true || _errors.Count < 1)
        {
            return;
        }

        Console.Write(BuildErrorText(Settings.EnableColoring).ToString());
        Console.WriteLine();
    }

    private StringBuilder BuildHeaderText(bool showTitle, bool showDescription)
    {
        var sb = new StringBuilder();

        if (showTitle && !string.IsNullOrWhiteSpace(Settings.Title))
        {
            sb.AppendLine(Settings.Title);
        }

        if (showDescription && !string.IsNullOrWhiteSpace(Settings.Description))
        {
            sb.AppendLine(Settings.Description);
        }

        return sb;
    }

    private StringBuilder BuildHelpText(bool enableColoring)
    {
        TextColoring.SetEnabled(enableColoring);

        var sb = BuildHeaderText(Settings.ShowTitle, Settings.ShowDescription);
        sb.AppendLine("These are command line options of this application.");
        sb.AppendLine();

        var padLength = _baseOptions.OfType<INamedOption>().Select(n => n.Alias.Length).Max();
        if (padLength < 8)
        {
            padLength = 8;
        }

        if (padLength > 16)
        {
            padLength = 16;
        }

        var paddingString = string.Empty.PadRight(padLength, ' ');
        var lineLength = Settings.HelpDisplayWidth - padLength;

        foreach (var option in _baseOptions)
        {
            if (VerbosityLevel < VerbosityLevelType.Detailed && option.IsHidden)
            {
                continue;
            }

            var namedOption = option as INamedOption;
            var shortAlias = namedOption?.Alias ?? string.Empty;
            sb.Pastel($"{shortAlias.PadRight(padLength, ' ')} ", ConsoleColor.DarkGreen);
            sb.Pastel("Usage: ", ConsoleColor.DarkYellow);
            sb.AppendLine(option.Usage);

            if (namedOption is not null)
            {
                sb.Pastel($"{paddingString} Aliases:", ConsoleColor.DarkYellow);
                sb.AppendLine($" {string.Join(", ", namedOption.Aliases.OrderBy(n => n[0] == '/').ThenBy(n => n.Length))}");
            }

            var descBuilder = new StringBuilder();
            if (option.Description is not null)
            {
                descBuilder.Append(option.Description);
            }

            if (option is IHaveAllowedValues allowedValueOption)
            {
                var allowedValues = allowedValueOption.GetAllowedValues();
                if (allowedValues.Count > 0)
                {
                    descBuilder.Append(descBuilder.GetCommaByEndsWith());
                    descBuilder.Append($"[Allowed-Values: {string.Join(" | ", allowedValues)}]");
                }
            }

            if (option is IHaveDefaultValue defaultValueOption)
            {
                var defaultValue = defaultValueOption.GetDefaultValue();
                if (!string.IsNullOrEmpty(defaultValue))
                {
                    descBuilder.Append(descBuilder.GetCommaByEndsWith());
                    descBuilder.Append($"[Default: {defaultValue}]");
                }
            }

            if (descBuilder.Length > 0)
            {
                var words = descBuilder.ToString().Split(' ');
                var lines = CreateLinesByWidth(words, lineLength);
                foreach (var line in lines)
                {
                    sb.AppendLine($"{paddingString} {line}");
                }
            }

            if (Settings.NewLineAfterOption)
            {
                sb.AppendLine();
            }
        }

        var usages = VerbosityLevel >= VerbosityLevelType.Detailed 
            ? _baseOptions.Select(o => o.Usage)
            : _baseOptions.Where(o => !AliasExtensions.BuiltInOptionNames.Contains(o.Name)).Select(o => o.Usage);

        var usageLines = CreateLinesByWidth(usages, lineLength, true);
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
        {
            return sb;
        }

        TextColoring.SetEnabled(enableColoring);

        if (VerbosityLevel >= VerbosityLevelType.Detailed)
        {
            sb.Append("###  ");
            sb.Pastel("Error(s)!", ConsoleColor.DarkRed);
            sb.AppendLine("  ###");
        }

        for (var i = 0; i < _errors.Count; ++i)
        {
            if (VerbosityLevel >= VerbosityLevelType.Normal)
            {
                sb.Pastel($"{(i + 1):00} - ", ConsoleColor.DarkYellow);
            }

            if (VerbosityLevel >= VerbosityLevelType.Detailed)
            {
                sb.Append($"Code: {_errors[i].Error.Code}, Message: ");
            }

            sb.AppendLine(_errors[i].Message);

            if (_errors[i].Exception is not null)
            {
                if (VerbosityLevel >= VerbosityLevelType.Trace)
                {
                    sb.AppendLine(_errors[i].Exception!.ToString());
                }
            }

            if (VerbosityLevel < VerbosityLevelType.Minimal)
                break;
        }

        return sb;
    }

    private void ValidateBuiltInTokens(bool isAllTokensParsed)
    {
        var help = _baseOptions.OfType<SwitchOption>().
            First(o => o.KeyProperty.Name == nameof(IBuiltInOptions.Help));

        if (IsOnlyOption(help))
        {
            if (isAllTokensParsed)
            {
                _errors.Clear();
            }

            return;
        }

        if (help.OptionTokens.Count > 0)
        {
            _errors.Add(ParserErrors.InvalidSingleOptionUsage.With(help.Alias).WithName(help.Name));
            return;
        }

        var version = _baseOptions.OfType<SwitchOption>().
            First(o => o.KeyProperty.Name == nameof(IBuiltInOptions.Version));

        if (IsOnlyOption(version))
        {
            if (isAllTokensParsed)
            {
                _errors.Clear();
            }

            return;
        }

        if (version.OptionTokens.Count > 0)
        {
            _errors.Add(ParserErrors.InvalidSingleOptionUsage.With(version.Alias).WithName(version.Name));
        }
    }

    private bool IsOnlyOption(IBaseOption option)
    {
        var optionCount = option switch
        {
            INamedOption c => c.OptionTokens.Count,
            IValueOption d => d.ValueTokens.Count,
            _ => 0
        };

        if (optionCount < 1)
        {
            return false;
        }

        var totalInputCount = _baseOptions
            .Where(n => n.Name != nameof(IBuiltInOptions.Verbosity))
            .Sum(o => o switch
            {
                INamedOption c => c.OptionTokens.Count,
                IValueOption d => d.ValueTokens.Count,
                _ => 0
            });

        return totalInputCount - optionCount < 1;
    }

    private List<string> CreateLinesByWidth(IEnumerable<string> textWords, int lineLength, bool addBrackets = false)
    {
        var textLines = new List<string> { string.Empty };

        foreach (var word in textWords)
        {
            if (textLines[^1].Length + word.Length > lineLength)
            {
                textLines.Add(string.Empty);
            }

            var space = textLines[^1].Length > 0 ? " " : string.Empty;
            var newWord = addBrackets ? $"[{word}]" : word;

            textLines[^1] += $"{space}{newWord}";
        }

        return textLines;
    }

    private static void AutoInitializeOptionName(BaseOption option, List<string> names)
    {
        if (!string.IsNullOrEmpty(option.Name))
        {
            return;
        }

        if (names.Contains(option.KeyProperty.Name))
        {
            throw BuilderErrors.NameAlreadyInUse.WithName(option.KeyProperty.Name)
                .ToException(option.KeyProperty.Name);
        }

        option.SetName(option.KeyProperty.Name);
        names.Add(option.KeyProperty.Name);
    }

    private void AutoInitializeOptionAliases(BaseOption option, List<string> aliases)
    {
        if (option is not INamedOption)
        {
            return;
        }

        if (option.GetAliases().Count > 0)
        {
            return;
        }

        // suggest aliases for option by using the registered property name
        var suggestedAliases = option.KeyProperty.Name.SuggestAliasesHeuristically(
            aliases, CaseSensitive, Settings.MaxAliasLength, Settings.MaxSuggestedAliasWordCount);

        if (!CaseSensitive)
        {
            suggestedAliases = suggestedAliases.Select(a => a.ToLowerInvariant());
        }

        var validAliases = suggestedAliases
            .ValidateAliases(OptionPrefix, CaseSensitive, Settings.MaxAliasLength, false)
            .GetOrThrow(option.Name);

        // Can't suggest alias because all alias possible names have already been used 
        if (validAliases.Count < 1)
        {
            throw BuilderErrors.UnableToSuggestAlias.WithName(option.Name).ToException(option.Name);
        }

        aliases.AddRange(validAliases);
        option.SetValidAliases(validAliases);
    }
}
