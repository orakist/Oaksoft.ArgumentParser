using System;
using System.Linq;
using System.Threading.Tasks;
using Oaksoft.ArgumentParser.Base;
using Oaksoft.ArgumentParser.Builder;
using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Errors;
using Oaksoft.ArgumentParser.Errors.Parser;
using Oaksoft.ArgumentParser.Options;

namespace Oaksoft.ArgumentParser.Parser;

internal sealed class ArgumentParser<TOptions>
    : BaseArgumentParser, IArgumentParser<TOptions>
    where TOptions : new()
{
    public override IParserSettings Settings { get; }

    public override bool IsHelpOption => GetBuiltInOptions().Help ?? false;

    public override bool IsVersionOption => GetBuiltInOptions().Version ?? false;

    public override VerbosityLevelType VerbosityLevel => GetBuiltInOptions().Verbosity ?? Settings.VerbosityLevel;

    private readonly TOptions _appOptions;

    public ArgumentParser(BaseArgumentParserBuilder builder)
        : base(builder)
    {
        Settings = builder.GetSettings();

        _appOptions = new TOptions();
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

    public async Task RunAsync(Func<TOptions, Task> callback, params string[] args)
    {
        await RunInnerAsync("Type the options or type 'q' to quit; press enter.", args, async () =>
        {
            if (IsValid && !IsHelpOption && !IsVersionOption)
            {
                await callback.Invoke(_appOptions);
            }
        });
    }

    public void Run(Action<IArgumentParser, TOptions> callback, params string[] args)
    {
        RunInner("Type the options or type 'q' to quit; press enter.", args, () =>
        {
            callback.Invoke(this, _appOptions);
        });
    }

    public async Task RunAsync(Func<IArgumentParser, TOptions, Task> callback, params string[] args)
    {
        await RunInnerAsync("Type the options or type 'q' to quit; press enter.", args, async () =>
        {
            await callback.Invoke(this, _appOptions);
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

    public async Task RunAsync(string? comment, Func<TOptions, Task> callback, params string[] args)
    {
        await RunInnerAsync(comment, args, async () =>
        {
            if (IsValid && !IsHelpOption && !IsVersionOption)
            {
                await callback.Invoke(_appOptions);
            }
        });
    }

    public void Run(string? comment, Action<IArgumentParser, TOptions> callback, params string[] args)
    {
        RunInner(comment, args, () =>
        {
            callback.Invoke(this, _appOptions);
        });
    }

    public async Task RunAsync(string? comment, Func<IArgumentParser, TOptions, Task> callback, params string[] args)
    {
        await RunInnerAsync(comment, args, async () =>
        {
            await callback.Invoke(this, _appOptions);
        });
    }

    public void RunOnce(Action<TOptions> callback, params string[] args)
    {
        RunOnceInner("Type the options; press enter.", args, () =>
        {
            if (IsValid && !IsHelpOption && !IsVersionOption)
            {
                callback.Invoke(_appOptions);
            }
        });
    }

    public async Task RunOnceAsync(Func<TOptions, Task> callback, params string[] args)
    {
        await RunOnceInnerAsync("Type the options; press enter.", args, async () =>
        {
            if (IsValid && !IsHelpOption && !IsVersionOption)
            {
                await callback.Invoke(_appOptions);
            }
        });
    }

    public void RunOnce(Action<IArgumentParser, TOptions> callback, params string[] args)
    {
        RunOnceInner("Type the options; press enter.", args, () =>
        {
            callback.Invoke(this, _appOptions);
        });
    }

    public async Task RunOnceAsync(Func<IArgumentParser, TOptions, Task> callback, params string[] args)
    {
        await RunOnceInnerAsync("Type the options; press enter.", args, async () =>
        {
            await callback.Invoke(this, _appOptions);
        });
    }

    public void RunOnce(string? comment, Action<TOptions> callback, params string[] args)
    {
        RunOnceInner(comment, args, () =>
        {
            if (IsValid && !IsHelpOption && !IsVersionOption)
            {
                callback.Invoke(_appOptions);
            }
        });
    }

    public async Task RunOnceAsync(string? comment, Func<TOptions, Task> callback, params string[] args)
    {
        await RunOnceInnerAsync(comment, args, async () =>
        {
            if (IsValid && !IsHelpOption && !IsVersionOption)
            {
                await callback.Invoke(_appOptions);
            }
        });
    }

    public void RunOnce(string? comment, Action<IArgumentParser, TOptions> callback, params string[] args)
    {
        RunOnceInner(comment, args, () =>
        {
            callback.Invoke(this, _appOptions);
        });
    }

    public async Task RunOnceAsync(string? comment, Func<IArgumentParser, TOptions, Task> callback, params string[] args)
    {
        await RunOnceInnerAsync(comment, args, async () =>
        {
            await callback.Invoke(this, _appOptions);
        });
    }

    private void RunInner(string? comment, string[]? args, Action callback)
    {
        args = InitializeArguments(comment, args);

        while (!IsQuitArgument(args))
        {
            EvaluateArguments(args, callback);

            args = GetInputArguments();
        }
    }

    private async Task RunInnerAsync(string? comment, string[]? args, Func<Task> callback)
    {
        args = await InitializeArgumentsAsync(comment, args);

        while (!IsQuitArgument(args))
        {
            await EvaluateArgumentsAsync(args, callback);

            args = await GetInputArgumentsAsync();
        }
    }

    private void RunOnceInner(string? comment, string[]? args, Action callback)
    {
        args = InitializeArguments(comment, args);

        EvaluateArguments(args, callback);
    }

    private async Task RunOnceInnerAsync(string? comment, string[]? args, Func<Task> callback)
    {
        args = await InitializeArgumentsAsync(comment, args);

        await EvaluateArgumentsAsync(args, callback);
    }

    private string[] InitializeArguments(string? comment, string[]? args)
    {
        if (!string.IsNullOrWhiteSpace(comment))
        {
            if (!CommandLine.DisableTextWriter)
            {
                _writer.WriteLine(comment);
            }
        }

        if (args?.Length > 0)
        {
            if (!CommandLine.DisableTextWriter && Settings.AutoPrintArguments)
            {
                _writer.WriteLine($"./> {string.Join(' ', args)}");
            }
        }
        else
        {
            args = GetInputArguments();
        }

        return args;
    }

    private async Task<string[]> InitializeArgumentsAsync(string? comment, string[]? args)
    {
        if (!string.IsNullOrWhiteSpace(comment))
        {
            if (!CommandLine.DisableTextWriter)
            {
                await _writer.WriteLineAsync(comment);
            }
        }

        if (args?.Length > 0)
        {
            if (!CommandLine.DisableTextWriter && Settings.AutoPrintArguments)
            {
                await _writer.WriteLineAsync($"./> {string.Join(' ', args)}");
            }
        }
        else
        {
            args = await GetInputArgumentsAsync();
        }

        return args;
    }

    private void EvaluateArguments(string[] args, Action callback)
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
            var error = new ErrorInfo($"{ParserErrors.Name}.RunCallbackError", ex.Message);
            _errors.Add(error.WithException(ex));

            if (!Settings.AutoPrintErrors)
            {
                throw;
            }
        }

        AutoPrintErrorText();
    }

    private async Task EvaluateArgumentsAsync(string[] args, Func<Task> callback)
    {
        ParseTokens(args);

        try
        {
            if (!IsEmpty)
            {
                await callback.Invoke();
            }
        }
        catch (Exception ex)
        {
            var error = new ErrorInfo($"{ParserErrors.Name}.RunCallbackError", ex.Message);
            _errors.Add(error.WithException(ex));

            if (!Settings.AutoPrintErrors)
            {
                throw;
            }
        }

        AutoPrintErrorText();
    }

    private static bool IsQuitArgument(string[] args)
    {
        if (args.Length != 1)
            return false;

        if (args[0].Equals("q", StringComparison.OrdinalIgnoreCase))
            return true;

        return args[0].Equals("quit", StringComparison.OrdinalIgnoreCase);
    }

    private string[] GetInputArguments()
    {
        if (!CommandLine.DisableTextWriter)
        {
            _writer.Write("./> ");
        }

        var commandLine = _reader.ReadLine();

        var result = commandLine?.SplitToArguments().ToArray() ??
               Array.Empty<string>();

        if (!CommandLine.DisableTextWriter && Settings.AutoPrintArguments)
        {
            _writer.WriteLine($"./> {commandLine}");
        }

        return result;
    }

    private async Task<string[]> GetInputArgumentsAsync()
    {
        if (!CommandLine.DisableTextWriter)
        {
            await _writer.WriteAsync("./> ");
        }

        var commandLine = await _reader.ReadLineAsync();

        var result = commandLine?.SplitToArguments().ToArray() ??
                     Array.Empty<string>();

        if (!CommandLine.DisableTextWriter && Settings.AutoPrintArguments)
        {
            await _writer.WriteLineAsync($"./> {commandLine}");
        }

        return result;
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
