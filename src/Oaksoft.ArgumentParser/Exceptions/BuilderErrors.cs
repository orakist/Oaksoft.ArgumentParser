using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Extensions;

namespace Oaksoft.ArgumentParser.Exceptions;

public static class BuilderErrors
{
    public static readonly BaseError NullValue = new BuilderError(
        GetCode(nameof(NullValue)),
        "The '{0}' value cannot be null!");

    public static readonly BaseError EmptyValue = new BuilderError(
        GetCode(nameof(EmptyValue)),
        "The given '{0}' value cannot be empty!");

    public static readonly BaseError InvalidEnum = new BuilderError(
        GetCode(nameof(InvalidEnum)),
        "Invalid '{0}' enum value '{1}'!");

    public static readonly BaseError OutOfRange = new BuilderError(
        GetCode(nameof(OutOfRange)),
        "Invalid '{0}' value! Valid interval is {1}.");

    public static readonly BaseError CannotBeModified = new BuilderError(
        GetCode(nameof(CannotBeModified)),
        "An option cannot be modified after building the argument parser.");

    public static readonly BaseError InvalidPropertyExpression = new BuilderError(
        GetCode(nameof(InvalidPropertyExpression)),
        "Invalid lambda expression, please select a '{0}' type property!");

    public static readonly BaseError InvalidStringPropertyUsage = new BuilderError(
        GetCode(nameof(InvalidStringPropertyUsage)),
        "Invalid lambda expression, string property '{0}' cannot be used as a collection!");

    public static readonly BaseError SamePropertyUsage = new BuilderError(
        GetCode(nameof(SamePropertyUsage)),
        "Selected Key and Count property cannot be same! Properties: {0}, {1}");

    public static readonly BaseError PropertyAlreadyInUse = new BuilderError(
        GetCode(nameof(PropertyAlreadyInUse)),
        "Property '{0}' already registered with an option!");

    public static readonly BaseError ReservedProperty = new BuilderError(
        GetCode(nameof(ReservedProperty)),
        "The reserved '{0}' property is not configurable.");

    public static readonly BaseError InvalidName = new BuilderError(
        GetCode(nameof(InvalidName)),
        "Invalid name '{0}' found! Only use ascii letters, ascii digits and ('_', '-') symbols.");

    public static readonly BaseError NameAlreadyInUse = new BuilderError(
        GetCode(nameof(NameAlreadyInUse)),
        "Name '{0}' is already in use! An option name must be unique.");

    public static readonly BaseError InvalidArity = new BuilderError(
        GetCode(nameof(InvalidArity)),
        "Invalid {0} '{1}'! Values cannot be negative and max value cannot be smaller then min value.");

    public static readonly BaseError MissingCallback = new BuilderError(
        GetCode(nameof(MissingCallback)),
        "Missing TryParse callback for custom type '{0}'! Configure a TryParse callback for option '{1}'.");

    public static readonly BaseError InvalidAllowedValue = new BuilderError(
        GetCode(nameof(InvalidAllowedValue)),
        "Any given allowed value cannot be empty or null!");

    public static readonly BaseError InvalidAlias = new BuilderError(
        GetCode(nameof(InvalidAlias)),
        "Invalid alias '{0}' found! Only use ascii letters, ascii digits and ({1}) symbols. " +
        "Alias should not start with digit and should have at least one letter or symbol.");

    public static readonly BaseError ReservedAlias = new BuilderError(
        GetCode(nameof(ReservedAlias)),
        "Invalid alias '{0}' found! Reserved aliases ({1}) cannot be used.");

    public static readonly BaseError TooLongAlias = new BuilderError(
        GetCode(nameof(TooLongAlias)),
        "Option alias '{0}' not allowed! Allowed max alias length is {1}.");

    public static readonly BaseError NotAllowedAlias = new BuilderError(
        GetCode(nameof(NotAllowedAlias)),
        $"{{0}} option alias '{{1}}' not allowed! Use appropriate {nameof(OptionPrefixRules)} and alias combination.");

    public static readonly BaseError AliasAlreadyInUse = new BuilderError(
        GetCode(nameof(AliasAlreadyInUse)),
        "Alias '{0}' is already in use! An option alias must be unique.");

    public static readonly BaseError UnableToSuggestAlias = new BuilderError(
        GetCode(nameof(UnableToSuggestAlias)),
        $"Unable to suggest alias for the option '{{0}}'! Use '{nameof(OptionExtensions.AddAliases)}(...)' to set aliases of the option.");

    private static string GetCode(string code)
    {
        return $"{nameof(BuilderErrors)}.{code}";
    }
}