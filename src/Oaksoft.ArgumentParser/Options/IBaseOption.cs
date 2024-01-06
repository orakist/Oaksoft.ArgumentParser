namespace Oaksoft.ArgumentParser.Options;

public interface IBaseOption
{
    string Name { get; }

    string Usage { get; }

    string? Description { get; }

    (int Min, int Max) OptionArity { get; }

    (int Min, int Max) ValueArity { get; }

    int OptionCount { get; }

    int ValueCount { get; }

    bool IsValid { get; }

    bool IsParsed { get; }
}