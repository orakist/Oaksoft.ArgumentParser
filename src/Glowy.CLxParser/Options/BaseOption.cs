using System;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Options;

internal abstract class BaseOption : IBaseOption
{
    public string Name { get; protected set; } = default!;

    public string Usage { get; protected set; } = default!;

    public string? Description { get; protected set; }

    public int RequiredTokenCount { get; }

    public int MaximumTokenCount { get; }

    public abstract int ValidatedTokenCount { get; }

    public string KeyProperty { get; private set; } = default!;

    public string? CountProperty { get; private set; }

    protected bool _validated;

    protected BaseOption(int requiredTokenCount = 0, int maximumTokenCount = 1)
    {
        RequiredTokenCount = requiredTokenCount;
        MaximumTokenCount = maximumTokenCount;
        if (RequiredTokenCount > MaximumTokenCount)
            MaximumTokenCount = RequiredTokenCount;
    }

    public void SetKeyProperty(string property)
    {
        KeyProperty = property;
    }

    public void SetCountProperty(string? property)
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

    public virtual void Initialize(IArgumentParser parser)
    {
        if (RequiredTokenCount is < 0 or > 100)
        {
            throw new ArgumentOutOfRangeException(
                nameof(RequiredTokenCount),
                $"Invalid required token count value. Valid interval is [0, 100]. Property: {KeyProperty}");
        }

        if (MaximumTokenCount is < 1 or > 100)
        {
            throw new ArgumentOutOfRangeException(
                nameof(MaximumTokenCount),
                $"Invalid maximum token count value. Valid interval is [1, 100]. Property: {KeyProperty}");
        }
    }

    public abstract void Parse(string[] arguments, IArgumentParser parser);

    public abstract void Validate(IArgumentParser parser);

    public virtual void Clear()
    {
        _validated = false;
    }
}
