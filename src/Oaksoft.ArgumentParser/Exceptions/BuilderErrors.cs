using Oaksoft.ArgumentParser.Definitions;
using Oaksoft.ArgumentParser.Extensions;

namespace Oaksoft.ArgumentParser.Exceptions;

public static class BuilderErrors
{
    public const string Name = nameof(BuilderErrors);

    public static readonly ErrorInfo NullValue = new(
        $"{Name}.{nameof(NullValue)}",
        "The '{0}' value cannot be null!");

    public static readonly ErrorInfo EmptyValue = new(
        $"{Name}.{nameof(EmptyValue)}",
        "The given '{0}' value cannot be empty!");

    public static readonly ErrorInfo InvalidEnum = new(
        $"{Name}.{nameof(InvalidEnum)}",
        "Invalid '{0}' enum value '{1}'!");

    public static readonly ErrorInfo OutOfRange = new(
        $"{Name}.{nameof(OutOfRange)}",
        "Invalid '{0}' value! Valid interval is {1}.");

    public static readonly ErrorInfo CannotBeModified = new(
        $"{Name}.{nameof(CannotBeModified)}",
        "An option cannot be modified after building the argument parser.");

    public static readonly ErrorInfo InvalidPropertyExpression = new(
        $"{Name}.{nameof(InvalidPropertyExpression)}",
        "Invalid lambda expression, please select a '{0}' type property!");

    public static readonly ErrorInfo InvalidStringPropertyUsage = new(
        $"{Name}.{nameof(InvalidStringPropertyUsage)}",
        "Invalid lambda expression, string property '{0}' cannot be used as a collection!");

    public static readonly ErrorInfo SamePropertyUsage = new(
        $"{Name}.{nameof(SamePropertyUsage)}",
        "Selected Key and Count property cannot be same! Properties: {0}, {1}");

    public static readonly ErrorInfo PropertyAlreadyInUse = new(
        $"{Name}.{nameof(PropertyAlreadyInUse)}",
        "Property '{0}' already registered with an option!");

    public static readonly ErrorInfo ReservedProperty = new(
        $"{Name}.{nameof(ReservedProperty)}",
        "The reserved '{0}' property is not configurable.");

    public static readonly ErrorInfo InvalidName = new(
        $"{Name}.{nameof(InvalidName)}",
        "Invalid name '{0}' found! Only use ascii letters, ascii digits and ('_', '-') symbols.");

    public static readonly ErrorInfo NameAlreadyInUse = new(
        $"{Name}.{nameof(NameAlreadyInUse)}",
        "Name '{0}' is already in use! An option name must be unique.");

    public static readonly ErrorInfo InvalidArity = new(
        $"{Name}.{nameof(InvalidArity)}",
        "Invalid {0} '{1}'! Values cannot be negative and max value cannot be smaller then min value.");

    public static readonly ErrorInfo MissingCallback = new(
        $"{Name}.{nameof(MissingCallback)}",
        "Missing TryParse callback for custom type '{0}'! Configure a custom TryParse callback.");

    public static readonly ErrorInfo InvalidAllowedValue = new(
        $"{Name}.{nameof(InvalidAllowedValue)}",
        "Any given allowed value cannot be empty or null!");

    public static readonly ErrorInfo InvalidAlias = new(
        $"{Name}.{nameof(InvalidAlias)}",
        "Invalid alias '{0}' found! Only use ascii letters, ascii digits and ({1}) symbols. " +
        "Alias should not start with digit and should have at least one letter or symbol.");

    public static readonly ErrorInfo ReservedAlias = new(
        $"{Name}.{nameof(ReservedAlias)}",
        "Invalid alias '{0}' found! Reserved aliases ({1}) cannot be used.");

    public static readonly ErrorInfo TooLongAlias = new(
        $"{Name}.{nameof(TooLongAlias)}",
        "Option alias '{0}' not allowed! Allowed max alias length is {1}.");

    public static readonly ErrorInfo NotAllowedAlias = new(
        $"{Name}.{nameof(NotAllowedAlias)}",
        $"{{0}} option alias '{{1}}' not allowed! Use appropriate {nameof(OptionPrefixRules)} and alias combination.");

    public static readonly ErrorInfo AliasAlreadyInUse = new(
        $"{Name}.{nameof(AliasAlreadyInUse)}",
        "Alias '{0}' is already in use! An option alias must be unique.");

    public static readonly ErrorInfo UnableToSuggestAlias = new(
        $"{Name}.{nameof(UnableToSuggestAlias)}",
        $"Unable to suggest alias for the option '{{0}}'! Use '{nameof(OptionExtensions.AddAliases)}(...)' to set aliases of the option.");
}