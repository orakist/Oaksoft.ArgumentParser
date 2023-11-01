using System;
using System.Linq;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser;

public static class CommandLine
{
    private static readonly string[] _commandPrefixes = { "-", "--", "/" };
    private static readonly string[] _valueSeparators = { ";", ",", "|" };
    private static readonly string[] _tokenSeparators = { "=", ":" };

    public static IArgumentParser<TOptions> CreateParser<TOptions>(
        string commandPrefix = "--", string valueSeparator = ";", 
        string tokenSeparator = "=", bool caseSensitive = false)
        where TOptions : BaseApplicationOptions, new()
    {
        ValidateParameters(commandPrefix, valueSeparator, tokenSeparator);

        var applicationOptions = new TOptions();
        return new ArgumentParser<TOptions>(
            applicationOptions, commandPrefix, valueSeparator, 
            tokenSeparator, caseSensitive);
    }

    private static void ValidateParameters(string commandPrefix, string valueSeparator, string tokenSeparator)
    {
        if (!_commandPrefixes.Contains(commandPrefix))
        {
            throw new ArgumentOutOfRangeException(nameof(commandPrefix),
                $"Invalid command prefix! Valid prefixes are '{string.Join(", ", _commandPrefixes)}'.");
        }

        if (!_valueSeparators.Contains(valueSeparator))
        {
            throw new ArgumentOutOfRangeException(nameof(valueSeparator),
                $"Invalid value separator! Valid separators are '{string.Join(", ", _valueSeparators)}'.");
        }

        if (!_tokenSeparators.Contains(tokenSeparator))
        {
            throw new ArgumentOutOfRangeException(nameof(tokenSeparator),
                $"Invalid token separator! Valid separators are '{string.Join(", ", _tokenSeparators)}'.");
        }
    }
}

