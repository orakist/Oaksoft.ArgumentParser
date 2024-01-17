using System;
using System.Linq;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Builder;
using Oaksoft.ArgumentParser.Options;

namespace Oaksoft.ArgumentParser.Parser;

internal sealed class ArgumentParser<TOptions>
    : BaseArgumentParser, IArgumentParser<TOptions>
{
    public override IParserSettings Settings { get; }

    private readonly TOptions _appOptions;
    private readonly BuiltInOptions _builtInOptions;

    public ArgumentParser(TOptions options, ArgumentParserBuilder<TOptions> builder)
        : base(builder.CaseSensitive, builder.OptionPrefix, builder.AliasDelimiter, builder.ValueDelimiter)
    {
        Settings = builder.GetSettings();

        _appOptions = options;
        _builtInOptions = new BuiltInOptions();
        _baseOptions.AddRange(builder.GetBaseOptions());
    }

    public IBuiltInOptions GetBuiltInOptions()
    {
        return _builtInOptions;
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
        ParseTokens(arguments);

        return _appOptions;
    }

    public void Run(Action<TOptions> callback)
    {
        Console.WriteLine("Type the options and press enter. Type 'q' to quit.");

        while (true)
        {
            Console.Write("./> ");

            var options = Console.In.ReadLine();
            if (options is "q" or "Q")
                break;

            var arguments = options?.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (!(arguments?.Length > 0))
                continue;

            var result = Parse(arguments);
            if (!IsValid)
                continue;

            callback.Invoke(result);
        }
    }

    public void Run(Action<IArgumentParser<TOptions>, TOptions> callback)
    {
        Console.WriteLine("Type the options and press enter. Type 'q' to quit.");

        while (true)
        {
            Console.Write("./> ");

            var options = Console.In.ReadLine();
            if (options is "q" or "Q")
                break;

            var arguments = options?.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (!(arguments?.Length > 0))
                continue;

            callback.Invoke(this, Parse(arguments));
        }
    }

    public void Run(string[] args, Action<TOptions> callback)
    {
        var loopCount = 0;
        while (true)
        {
            if (args.Length > 0)
            {
                var result = Parse(args);

                if (IsValid)
                {
                    callback.Invoke(result);
                }
            }

            if (loopCount < 1)
            {
                Console.WriteLine("Type the options and press enter. Type 'q' to quit.");
            }

            Console.Write("./> ");
            var options = Console.In.ReadLine();
            if (options is "q" or "Q")
                break;

            var arguments = options?.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            args = arguments ?? Array.Empty<string>();
            ++loopCount;
        }
    }

    public void Run(string[] args, Action<IArgumentParser<TOptions>, TOptions> callback)
    {
        var loopCount = 0;

        while (true)
        {
            if (args.Length > 0)
            {
                callback.Invoke(this, Parse(args));
            }

            if (loopCount < 1)
            {
                Console.WriteLine("Type the options and press enter. Type 'q' to quit.");
            }

            Console.Write("./> ");
            var options = Console.In.ReadLine();
            if (options is "q" or "Q")
                break;

            var arguments = options?.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            args = arguments ?? Array.Empty<string>();
            ++loopCount;
        }
    }

    private void InitializePropertyInfos()
    {
        _propertyInfos.AddRange(_baseOptions.Where(o => o.CountProperty is not null).Select(a => a.CountProperty!));
        _propertyInfos.AddRange(_baseOptions.Select(o => o.KeyProperty));
    }

    protected override void ClearOptionPropertiesByReflection()
    {
        foreach (var property in _propertyInfos)
        {
            var type = property.PropertyType;
            var defaultValue = type.IsValueType ? Activator.CreateInstance(type) : null;

            if (AliasExtensions.BuiltInOptionNames.Contains(property.Name))
            {
                property.SetValue(_builtInOptions, defaultValue);
            }
            else
            {
                property.SetValue(_appOptions, defaultValue);
            }
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
            if (AliasExtensions.BuiltInOptionNames.Contains(keyProp.Name))
            {
                valOption.ApplyOptionResult(_builtInOptions, keyProp);
            }
            else
            {
                valOption.ApplyOptionResult(_appOptions!, keyProp);
            }
        }
    }
}

