using System;
using System.Collections.Generic;
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

    public bool IsParsed => IsValid && OptionCount + ValueCount > 0;

    public abstract int OptionCount { get; }

    public abstract int ValueCount { get; }

    public PropertyInfo KeyProperty { get; private set; } = default!;

    public PropertyInfo? CountProperty { get; private set; }

    protected IArgumentParser? _parser;

    public void SetKeyProperty(PropertyInfo property)
    {
        KeyProperty = property;
    }

    public void SetCountProperty(PropertyInfo? property)
    {
        CountProperty = property;
    }

    public void SetParser(IArgumentParser parser)
    {
        _parser = parser;
    }

    public void SetName(string name)
    {
        ParserInitializedGuard();

        if (string.IsNullOrWhiteSpace(name))
            return;

        Name = string.Join(' ', name.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
    }

    public void SetUsage(string usage)
    {
        ParserInitializedGuard();

        if (string.IsNullOrWhiteSpace(usage))
            return;

        Usage = string.Join(' ', usage.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
    }

    public void SetDescription(string description)
    {
        ParserInitializedGuard();

        if (string.IsNullOrWhiteSpace(description))
            return;

        Description = description.Trim();
    }

    public virtual List<string> GetAliases()
    {
        throw new NotSupportedException("GetAliases() not supported by value options.");
    }

    public virtual void AddAliases(bool skipValidation, params string[] aliases)
    {
        throw new NotSupportedException("AddAliases() not supported by value options.");
    }

    public virtual void Initialize()
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
            throw new ArgumentException(
                nameof(Name),
                $"Empty option arity. Every option must have a valid name. Property: {KeyProperty.Name}");
        }
    }

    public abstract void Parse(TokenItem[] tokens);

    public virtual void Validate()
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

    protected void ParserInitializedGuard()
    {
        if (_parser != null)
            throw new Exception("An option cannot be modified after building the argument parser.");
    }

}
