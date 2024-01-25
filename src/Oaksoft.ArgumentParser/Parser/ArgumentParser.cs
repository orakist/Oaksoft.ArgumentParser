using System;
using System.Linq;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Builder;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Errors;
using Oaksoft.ArgumentParser.Errors.Parser;
using Oaksoft.ArgumentParser.Options;

namespace Oaksoft.ArgumentParser.Parser;

internal sealed class ArgumentParser<TOptions>
    : BaseArgumentParser, IArgumentParser<TOptions>
{
    public override IParserSettings Settings { get; }

    public override bool IsHelpOption => GetBuiltInOptions().Help ?? false;

    public override bool IsVersionOption => GetBuiltInOptions().Version ?? false;

    public override VerbosityLevelType VerbosityLevel => GetBuiltInOptions().Verbosity ?? Settings.VerbosityLevel;

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

    public TOptions Parse(params string[] arguments)
    {
        ParseTokens(arguments);

        AutoPrintErrorText();

        return _appOptions;
    }

    public void Run(Action<TOptions> callback, params string[] args)
    {
        RunInner("Type the options or type 'q' to quit; press enter.", args, () =>
        {
            if (IsValid && !IsHelpOption && !IsVersionOption)
            {
                callback.Invoke(_appOptions);
            }
        });
    }

    public void Run(Action<IArgumentParser<TOptions>, TOptions> callback, params string[] args)
    {
        RunInner("Type the options or type 'q' to quit; press enter.", args, () =>
        {
            callback.Invoke(this, _appOptions);
        });
    }

    public void Run(string? comment, Action<TOptions> callback, params string[] args)
    {
        RunInner(comment, args, () =>
        {
            if (IsValid && !IsHelpOption && !IsVersionOption)
            {
                callback.Invoke(_appOptions);
            }
        });
    }

    public void Run(string? comment, Action<IArgumentParser<TOptions>, TOptions> callback, params string[] args)
    {
        RunInner(comment, args, () =>
        {
            callback.Invoke(this, _appOptions);
        });
    }

    private void RunInner(string? comment, string[]? args, Action callback)
    {
        if (!string.IsNullOrWhiteSpace(comment))
        {
            Console.WriteLine(comment);
        }

        if (args?.Length > 0)
        {
            Console.WriteLine($"./> {string.Join(' ', args)}");
        }
        else
        {
            args = GetInputArguments();
        }

        while (args.Length != 1 || (args[0] != "q" && args[0] != "Q"))
        {
            ParseTokens(args);

            try
            {
                if (!IsEmpty)
                {
                    callback.Invoke();
                }
            }
            catch (Exception ex)
            {
                if (!Settings.AutoPrintErrors)
                {
                    throw;
                }

                var error = new ErrorInfo($"{ParserErrors.Name}.RunCallbackError", ex.Message);
                _errors.Add(error.WithException(ex));
            }

            AutoPrintErrorText();

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
        {
            return;
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
