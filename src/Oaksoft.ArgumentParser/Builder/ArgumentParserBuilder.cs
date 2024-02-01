using System;
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

internal sealed class ArgumentParserBuilder<TOptions>
    : BaseArgumentParserBuilder, IArgumentParserBuilder<TOptions>
    where TOptions : new()
{
    public ArgumentParserBuilder(
        bool caseSensitive, OptionPrefixRules optionPrefix,
        AliasDelimiterRules aliasDelimiter, ValueDelimiterRules valueDelimiter,
        TextReader? textReader, TextWriter? textWriter)
        : base(caseSensitive, optionPrefix, aliasDelimiter, valueDelimiter, textReader, textWriter)
    {
    }

    public IArgumentParserBuilder<TOptions> ConfigureSettings(Action<IParserSettingsBuilder> action)
    {
        action.Invoke(_settingsBuilder);
        return this;
    }

    public IArgumentParser<TOptions> AutoBuild()
    {
        AutoBuildApplicationOptions();

        BuildDefaultSettings();

        BuildDefaultOptions();

        var parser = new ArgumentParser<TOptions>(this);
        parser.Initialize();

        return parser;
    }

    private void AutoBuildApplicationOptions()
    {
        var registeredNames = GetRegisteredPropertyNames();
        var properties = typeof(TOptions).GetProperties()
            .Where(p => p.SetMethod is not null)
            .Where(p => !AliasExtensions.BuiltInOptionNames.Any(r => r.Equals(p.Name, StringComparison.OrdinalIgnoreCase)))
            .Where(p => !registeredNames.Contains(p.Name)).ToList();

        foreach (var property in properties)
        {
            var (optionType, isSequential) = property.GetOptionType();
            if (optionType == null)
            {
                continue;
            }

            if (isSequential)
            {
                var genericType = typeof(SequentialNamedOption<>).MakeGenericType(optionType);
                var option = Activator.CreateInstance(genericType, 0, int.MaxValue, 0, int.MaxValue) as BaseOption;
                this.RegisterOptionProperty(option!, property);
            }
            else
            {
                if (optionType == typeof(bool))
                {
                    var option = new SwitchOption(0, 1);
                    this.RegisterOptionProperty(option, property);
                }
                else
                {
                    var genericType = typeof(ScalarNamedOption<>).MakeGenericType(optionType);
                    var option = Activator.CreateInstance(genericType, 0, 1, 1, 1) as BaseOption;
                    this.RegisterOptionProperty(option!, property);
                }
            }
        }
    }

    public IArgumentParser<TOptions> Build()
    {
        BuildDefaultSettings();

        BuildDefaultOptions();

        var parser = new ArgumentParser<TOptions>(this);
        parser.Initialize();

        return parser;
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
        this.RegisterSwitchOption(keyProperty, false);

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
        this.RegisterSwitchOption(keyProperty, false);

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
        this.RegisterNamedOption<VerbosityLevelType>(keyProperty, false, false);

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
}
