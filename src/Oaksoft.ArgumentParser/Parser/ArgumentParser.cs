using System;
using System.Linq;
using Oaksoft.ArgumentParser.Builder;
using Oaksoft.ArgumentParser.Options;

namespace Oaksoft.ArgumentParser.Parser;

internal sealed class ArgumentParser<TOptions> 
    : BaseArgumentParser, IArgumentParser<TOptions>
    where TOptions : IApplicationOptions
{
    public override IParserSettings Settings { get; }

    private readonly TOptions _appOptions;

    public ArgumentParser(TOptions options, ArgumentParserBuilder<TOptions> builder)
        : base(builder.CaseSensitive, builder.OptionPrefix, builder.AliasDelimiter, builder.ValueDelimiter)
    {
        Settings = builder.GetSettings();

        _appOptions = options;
        _baseOptions.AddRange(builder.GetBaseOptions());
    }

    public TOptions GetApplicationOptions()
    {
        return _appOptions;
    }

    public void Initialize()
    {
        InitializePropertyInfos();

        InitializeOptions();

        AutoPrintHeaderText();
    }

    public TOptions Parse(string[] arguments)
    {
        ClearOptions();

        var tokens = PrepareTokens(arguments);

        ParseOptions(tokens);

        ValidateOptions(tokens);

        BindOptionsToAttributes();

        AutoPrintHelpText();

        AutoPrintErrorText();

        return _appOptions;
    }

    private void InitializePropertyInfos()
    {
        var propertyNames = _baseOptions
            .Where(o => !string.IsNullOrWhiteSpace(o.CountProperty?.Name))
            .Select(a => a.CountProperty!.Name)
            .ToList();

        propertyNames.AddRange(_baseOptions.Select(o => o.KeyProperty.Name));

        var properties = _appOptions.GetType().GetProperties()
            .Where(p => propertyNames.Contains(p.Name));

        _propertyInfos.AddRange(properties);
    }

    protected override void ClearOptionPropertiesByReflection()
    {
        foreach (var property in _propertyInfos)
        {
            var type = property.PropertyType;
            var defaultValue = type.IsValueType ? Activator.CreateInstance(type) : null;
            property.SetValue(_appOptions, defaultValue);
        }
    }

    protected override void UpdateOptionPropertiesByReflection(BaseOption option)
    {
        if (!option.IsParsed)
            return;

        if (option.CountProperty is not null)
        {
            var countProp = _propertyInfos.First(p => p.Name == option.CountProperty.Name);
            if (countProp.PropertyType == typeof(bool))
            {
                countProp.SetValue(_appOptions, true);
            }
            else if (countProp.PropertyType == typeof(int))
            {
                countProp.SetValue(_appOptions, option is INamedOption ? option.OptionCount : option.ValueCount);
            }
        }

        var keyProp = _propertyInfos.First(p => p.Name == option.KeyProperty.Name);
        if (option is BaseValueOption valOption)
        {
            valOption.ApplyOptionResult(_appOptions, keyProp);
        }
    }
}

