using System;
using System.Linq;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser;

public static class CommandLine
{
    private static readonly string[] _optionPrefixes = { "-", "--", "/" };
    private static readonly string[] _valueDelimiters = { ";", ",", "|" };
    private static readonly string[] _tokenDelimiters = { "=", ":" };

    public static IArgumentParser<TOptions> CreateParser<TOptions>(
        string optionPrefix = "--", string valueDelimiter = ";", 
        string tokenDelimiter = "=", bool caseSensitive = false)
        where TOptions : BaseApplicationOptions, new()
    {
        ValidateParameters(optionPrefix, valueDelimiter, tokenDelimiter);

        var applicationOptions = new TOptions();
        return new ArgumentParser<TOptions>(
            applicationOptions, optionPrefix, valueDelimiter,
            tokenDelimiter, caseSensitive);
    }

    private static void ValidateParameters(string optionPrefix, string valueDelimiter, string tokenDelimiter)
    {
        if (!_optionPrefixes.Contains(optionPrefix))
        {
            throw new ArgumentOutOfRangeException(nameof(optionPrefix),
                $"Invalid option prefix! Valid prefixes are '{string.Join(", ", _optionPrefixes)}'.");
        }

        if (!_valueDelimiters.Contains(valueDelimiter))
        {
            throw new ArgumentOutOfRangeException(nameof(valueDelimiter),
                $"Invalid value delimiter! Valid delimiters are '{string.Join(", ", _valueDelimiters)}'.");
        }

        if (!_tokenDelimiters.Contains(tokenDelimiter))
        {
            throw new ArgumentOutOfRangeException(nameof(tokenDelimiter),
                $"Invalid token delimiter! Valid delimiters are '{string.Join(", ", _tokenDelimiters)}'.");
        }
    }
}

