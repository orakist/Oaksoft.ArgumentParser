﻿namespace Oaksoft.ArgumentParser.Errors.Parser;

internal static class ParserErrors
{
    public const string Name = nameof(ParserErrors);

    public static readonly ErrorInfo InvalidToken = new(
        $"{Name}.{nameof(InvalidToken)}",
        "Invalid token '{0}' found!");

    public static readonly ErrorInfo UnknownToken = new(
        $"{Name}.{nameof(UnknownToken)}",
        "Unknown token '{0}' found!");

    public static readonly ErrorInfo InvalidDoubleDashToken = new(
        $"{Name}.{nameof(InvalidDoubleDashToken)}",
        "Invalid token '{0}' found! Double dash '--' is not allowed with short aliases!");

    public static readonly ErrorInfo UnknownDoubleDashToken = new(
       $"{Name}.{nameof(UnknownDoubleDashToken)}",
       "Unknown double dash alias token '{0}' found!");

    public static readonly ErrorInfo InvalidSingleDashToken = new(
       $"{Name}.{nameof(InvalidSingleDashToken)}",
       "Invalid token '{0}' found! Single dash '-' is not allowed with long aliases!!");

    public static readonly ErrorInfo UnknownSingleDashToken = new(
        $"{Name}.{nameof(UnknownSingleDashToken)}",
        "Unknown single dash alias token '{0}' found!");

    public static readonly ErrorInfo UnknownForwardSlashToken = new(
        $"{Name}.{nameof(UnknownForwardSlashToken)}",
        "Unknown forward slash alias token '{0}' found!");

    public static readonly ErrorInfo InvalidSingleOptionUsage = new(
        $"{Name}.{nameof(InvalidSingleOptionUsage)}",
        "'{0}' option cannot be combined with other options.");

    public static readonly ErrorInfo TooManyOption = new(
        $"{Name}.{nameof(TooManyOption)}",
        "At most '{0}' option(s) expected but '{1}' option(s) provided.");

    public static readonly ErrorInfo VeryFewOption = new(
        $"{Name}.{nameof(VeryFewOption)}",
        "At least '{0}' option(s) expected but '{1}' option(s) provided.");

    public static readonly ErrorInfo TooManyValue = new(
        $"{Name}.{nameof(TooManyValue)}",
        "At most '{0}' value(s) expected but '{1}' value(s) provided.");

    public static readonly ErrorInfo VeryFewValue = new(
        $"{Name}.{nameof(VeryFewValue)}",
        "At least '{0}' value(s) expected but '{1}' value(s) provided.");

    public static readonly ErrorInfo InvalidOptionValue = new(
        $"{Name}.{nameof(InvalidOptionValue)}",
        "Invalid option value '{0}' found!");

    public static readonly ErrorInfo ValueMustBeOneOf = new(
        $"{Name}.{nameof(ValueMustBeOneOf)}",
        "Option value '{0}' not recognized. Must be one of: [{1}]");

    public static readonly ErrorInfo PredicateFailure = new(
        $"{Name}.{nameof(PredicateFailure)}",
        "Option value validation failed. Value(s): {0}");

    public static readonly ErrorInfo ListPredicateFailure = new(
        $"{Name}.{nameof(ListPredicateFailure)}",
        "Option value list validation failed. Value(s): {0}");
}
