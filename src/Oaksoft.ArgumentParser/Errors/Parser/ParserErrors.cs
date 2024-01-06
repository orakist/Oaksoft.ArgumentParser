namespace Oaksoft.ArgumentParser.Errors.Parser;

public static class ParserErrors
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
        "'{0}' option alias cannot be combined with other aliases.");

    public static readonly ErrorInfo TooManyOption = new(
        $"{Name}.{nameof(TooManyOption)}",
        "At least '{0}' option(s) expected but '{1}' option(s) provided.");

    public static readonly ErrorInfo VeryFewOption = new(
        $"{Name}.{nameof(VeryFewOption)}",
        "At most '{0}' option(s) expected but '{1}' option(s) provided.");

    public static readonly ErrorInfo TooManyValue = new(
        $"{Name}.{nameof(TooManyValue)}",
        "At least '{0}' value(s) expected but '{1}' value(s) provided.");

    public static readonly ErrorInfo VeryFewValue = new(
        $"{Name}.{nameof(VeryFewValue)}",
        "At most '{0}' value(s) expected but '{1}' value(s) provided.");

    public static readonly ErrorInfo InvalidOptionValue = new(
        $"{Name}.{nameof(InvalidOptionValue)}",
        "Invalid option value '{0}' found!");

    public static readonly ErrorInfo ValueMustBeOneOf = new(
        $"{Name}.{nameof(ValueMustBeOneOf)}",
        "Option value '{0}' not recognized. Must be one of: {1}");

    public static readonly ErrorInfo PredicateFailure = new(
        $"{Name}.{nameof(PredicateFailure)}",
        "At least one value is not validated by the predicate callback. Value(s): {0}");
}