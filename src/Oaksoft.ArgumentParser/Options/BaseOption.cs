using System;
using System.Reflection;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Options;

internal abstract class BaseOption : IBaseOption
{
    public string Name { get; protected set; } = default!;

    public string Usage { get; protected set; } = default!;

    public string? Description { get; protected set; }

    public (int Min, int Max) OptionArity { get; protected set; }

    public (int Min, int Max) ValueArity { get; protected set; }

    public bool IsValid { get; protected set; }

    public bool IsActive => IsValid && OptionCount + ValueCount > 0;

    public abstract int OptionCount { get; }

    public abstract int ValueCount { get; }

    public PropertyInfo KeyProperty { get; private set; } = default!;

    public PropertyInfo? CountProperty { get; private set; }

    public void SetKeyProperty(PropertyInfo property)
    {
        KeyProperty = property;
    }

    public void SetCountProperty(PropertyInfo? property)
    {
        CountProperty = property;
    }

    public void SetName(string name)
    {
        if (!string.IsNullOrWhiteSpace(name))
            Name = name.Trim();
    }

    public void SetUsage(string usage)
    {
        if (!string.IsNullOrWhiteSpace(usage))
            Usage = usage.Trim();
    }

    public void SetDescription(string? description)
    {
        Description = string.IsNullOrWhiteSpace(description) 
            ? null : description.Trim();
    }
    
    public virtual void SetAliases(params string[] aliases)
    {
    }

    public virtual bool StartsWithAnyAlias(string token, StringComparison flag)
    {
        return false;
    }

    public virtual void Initialize(IArgumentParser parser)
    {
        if (ValueArity.Min < 0 || ValueArity.Max < ValueArity.Min)
        {
            throw new ArgumentException(
                nameof(ValueArity),
                $"Invalid value arity. Do not use negative or inconsistent values. Arity: {ValueArity}");
        }

        if (OptionArity.Min < 0 || OptionArity.Max < OptionArity.Min)
        {
            throw new ArgumentException(
                nameof(OptionArity),
                $"Invalid option arity. Do not use negative or inconsistent values. Arity: {OptionArity}");
        }

        if (string.IsNullOrWhiteSpace(Name))
        {
            Name = KeyProperty.Name;
        }

        if (string.IsNullOrWhiteSpace(Description))
        {
            Description = $"Performs '{Name}' option.";
        }
    }

    public abstract void Parse(TokenValue[] tokens, IArgumentParser parser);

    public virtual void Validate(IArgumentParser parser)
    {
        if (OptionCount < OptionArity.Min)
        {
            throw new Exception(
                $"At least '{OptionArity.Min}' option{S(OptionArity.Min)} expected but '{OptionCount}' option{S(OptionCount)} provided.");
        }

        if (OptionCount > OptionArity.Max)
        {
            throw new Exception(
                $"At most '{OptionArity.Max}' option{S(OptionArity.Max)} expected but '{OptionCount}' option{S(OptionCount)} provided.");
        }

        if (OptionArity.Max >= 1 && OptionCount <= 0) 
            return;

        if (ValueCount < ValueArity.Min)
        {
            throw new Exception(
                $"At least '{ValueArity.Min}' value{S(ValueArity.Min)} expected but '{ValueCount}' value{S(ValueCount)} provided.");
        }

        if (ValueCount > ValueArity.Max)
        {
            throw new Exception(
                $"At most '{ValueArity.Max}' value{S(ValueArity.Max)} expected but '{ValueCount}' value{S(ValueCount)} provided.");
        }
    }

    public virtual void Clear()
    {
        IsValid = false;
    }

    protected static string S(int value)
    {
        return value < 2 ? " was" : "s were";
    }
}
