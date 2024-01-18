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
            var args = GetInputArguments();
            if (args.Length == 1 && args[0] is "q" or "Q")
                break;

            if (args.Length < 1)
                continue;

            var result = Parse(args);
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
            var args = GetInputArguments();
            if (args.Length == 1 && args[0] is "q" or "Q")
                break;

            if (args.Length < 1)
                continue;

            callback.Invoke(this, Parse(args));
        }
    }

    public void Run(string[] args, Action<TOptions> callback)
    {
        Console.WriteLine("Type the options and press enter. Type 'q' to quit.");

        if (args.Length < 1)
        {
            args = GetInputArguments();
        }
        else
        {
            Console.WriteLine($"./> {string.Join(' ', args)}");
        }

        while (args.Length != 1 || (args[0] != "q" && args[0] != "Q"))
        {
            if (args.Length > 0)
            {
                var result = Parse(args);

                if (IsValid)
                {
                    callback.Invoke(result);
                }
            }

            args = GetInputArguments();
        }
    }

    public void Run(string[] args, Action<IArgumentParser<TOptions>, TOptions> callback)
    {
        Console.WriteLine("Type the options and press enter. Type 'q' to quit.");

        if (args.Length < 1)
        {
            args = GetInputArguments();
        }
        else
        {
            Console.WriteLine($"./> {string.Join(' ', args)}");
        }

        while (args.Length != 1 || (args[0] != "q" && args[0] != "Q"))
        {
            if (args.Length > 0)
            {
                callback.Invoke(this, Parse(args));
            }

            args = GetInputArguments();
        }
    }

    private static string[] GetInputArguments()
    {
        Console.Write("./> ");
        var commandLine = Console.In.ReadLine();

        return commandLine?.SplitToArguments().ToArray() ??
               Array.Empty<string>();
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

