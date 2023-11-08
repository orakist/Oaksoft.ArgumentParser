using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Options;

namespace Oaksoft.ArgumentParser.Parser;

internal abstract class BaseArgumentParser : IArgumentParser
{
    public bool CaseSensitive { get; }

    public OptionPrefixRules OptionPrefix { get; }

    public TokenDelimiterRules TokenDelimiter { get; }

    public ValueDelimiterRules ValueDelimiter { get; }

    public abstract IParserSettings Settings { get; }

    public bool IsValid => _errors.Count < 1;

    public List<string> Errors => _errors.ToList();

    protected readonly List<string> _errors;
    protected readonly List<BaseOption> _baseOptions;
    protected readonly List<PropertyInfo> _propertyInfos;

    protected BaseArgumentParser(
        bool caseSensitive, OptionPrefixRules optionPrefix, 
        TokenDelimiterRules tokenDelimiter, ValueDelimiterRules valueDelimiter)
    {
        CaseSensitive = caseSensitive;
        OptionPrefix = optionPrefix;
        TokenDelimiter = tokenDelimiter;
        ValueDelimiter = valueDelimiter;

        _errors = new List<string>();
        _baseOptions = new List<BaseOption>();
        _propertyInfos = new List<PropertyInfo>();
    }

    public abstract string GetHeaderText();

    public abstract string GetHelpText(bool? colorized = default);

    public abstract string GetErrorText(bool? colorized = default);

    protected abstract void ClearOptionPropertiesByReflection();

    protected abstract void UpdateOptionPropertiesByReflection(BaseOption option);

    protected void InitializeOptions()
    {
        var aliases = new List<string>();

        foreach (var option in _baseOptions.Cast<BaseOption>())
        {
            try
            {
                ValidateOptionAliases(option, aliases);

                option.Initialize(this);
            }
            catch (Exception ex)
            {
                throw new Exception(BuildErrorMessage(option, ex));
            }
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

    protected void ParseArguments(TokenValue[] tokens)
    {
        var orderedOptions = new List<IBaseOption>();
        orderedOptions.AddRange(_baseOptions.Where(o => o is ISwitchOption));
        orderedOptions.AddRange(_baseOptions.Where(o => o is IScalarNamedOption));
        orderedOptions.AddRange(_baseOptions.Where(o => o is ISequentialNamedOption));
        orderedOptions.AddRange(_baseOptions.Where(o => o is not INamedOption));

        foreach (var option in orderedOptions.Cast<BaseOption>())
        {
            try
            {
                option.Parse(tokens, this);
            }
            catch (Exception ex)
            {
                _errors.Add(BuildErrorMessage(option, ex));
            }
        }
    }

    protected void ValidateOptions(TokenValue[] tokens)
    {
        foreach (var option in _baseOptions.Cast<BaseOption>())
        {
            try
            {
                option.Validate(this);
            }
            catch (Exception ex)
            {
                _errors.Add(BuildErrorMessage(option, ex));
            }
        }

        ValidateHelpToken();

        foreach (var option in tokens.Where(s => !s.IsParsed && !s.Invalid))
            _errors.Add($"Unknown option found. Option: {option.Argument}");
    }

    protected void BindOptionsToAttributes()
    {
        if (_errors.Count > 0)
            return;

        foreach (var option in _baseOptions)
        {
            try
            {
                UpdateOptionPropertiesByReflection(option);
            }
            catch (Exception ex)
            {
                _errors.Add(BuildErrorMessage(option, ex));
            }
        }
    }

    protected bool IsOnlyOption(IBaseOption option)
    {
        var optionCount = option switch
        {
            INamedOption c => c.OptionTokens.Count,
            IValueOption d => d.ValueTokens.Count,
            _ => 0
        };

        if (optionCount < 1)
            return false;

        var totalInputCount = _baseOptions.Sum(o => o switch
        {
            INamedOption c => c.OptionTokens.Count,
            IValueOption d => d.ValueTokens.Count,
            _ => 0
        });

        return totalInputCount - optionCount < 1;
    }

    protected void ValidateArguments(TokenValue[] tokens)
    {
        foreach (var token in tokens)
        {
            try
            {
                token.Argument.ValidateArgument(OptionPrefix);
            }
            catch (Exception ex)
            {
                _errors.Add(ex.Message);
                token.Invalid = true;
            }
        }
    }

    protected void AutoPrintHeaderText()
    {
        if (Settings.AutoPrintHeader != true)
            return;

        Console.Write(BuildHeaderText(true, true).ToString());
        Console.WriteLine();
    }

    protected void AutoPrintHelpText()
    {
        if (Settings.AutoPrintHelp != true || _errors.Count > 0)
            return;

        var helpOption = _baseOptions.OfType<SwitchOption>().
            First(o => o.KeyProperty.Name == nameof(IApplicationOptions.Help));

        if (!IsOnlyOption(helpOption))
            return;

        Console.Write(BuildHelpText(Settings.EnableColoring ?? true).ToString());
        Console.WriteLine();
    }

    protected void AutoPrintErrorText()
    {
        if (Settings.AutoPrintErrors != true || _errors.Count < 1)
            return;

        Console.Write(BuildErrorText(Settings.EnableColoring ?? true).ToString());
        Console.WriteLine();
    }

    protected StringBuilder BuildHeaderText(bool showTitle, bool showDescription)
    {
        var sb = new StringBuilder();

        if (showTitle && !string.IsNullOrWhiteSpace(Settings.Title))
            sb.AppendLine(Settings.Title);
        if (showDescription && !string.IsNullOrWhiteSpace(Settings.Description))
            sb.AppendLine(Settings.Description);

        return sb;
    }

    protected StringBuilder BuildHelpText(bool enableColoring)
    {
        TextColoring.SetEnabled(enableColoring);

        var sb = BuildHeaderText(Settings.ShowTitle ?? true, Settings.ShowDescription ?? true);
        sb.AppendLine("These are command line options of this application.");
        sb.AppendLine();

        foreach (var option in _baseOptions)
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

        var usageLines = CreateLinesByWidth(_baseOptions.Select(o => o.Usage), true);
        sb.Pastel("Usage: ", ConsoleColor.DarkYellow);
        sb.AppendLine(usageLines[0]);

        for (var index = 1; index < usageLines.Count; ++index)
            sb.AppendLine($"       {usageLines[index]}");

        return sb;
    }

    protected StringBuilder BuildErrorText(bool enableColoring)
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

    private void ValidateHelpToken()
    {
        var helpOption = _baseOptions.OfType<SwitchOption>().
            First(o => o.KeyProperty.Name == nameof(IApplicationOptions.Help));

        if (IsOnlyOption(helpOption))
        {
            _errors.Clear();
        }
        else if (helpOption.OptionTokens.Count > 0)
        {
            _errors.Add($"{helpOption.Name} ({helpOption.ShortAlias}) option cannot be combined with other options.");
        }
    }

    private List<string> CreateLinesByWidth(IEnumerable<string> textWords, bool addBrackets = false)
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

    private static string BuildErrorMessage(IBaseOption option, Exception ex)
    {
        var name = (option as INamedOption)?.ShortAlias ?? option.Name;
        var comma = ex.Message.EndsWith(".") ? string.Empty : ",";
        return $"{ex.Message}{comma} Option: {name}";
    }
}
