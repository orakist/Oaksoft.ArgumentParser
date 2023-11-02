namespace Oaksoft.ArgumentParser.Options;

public interface IBaseOption
{
    string Name { get; }

    string Usage { get; }

    string? Description { get; }

    int RequiredTokenCount { get; }

    int MaximumTokenCount { get; }

    int ValidInputCount { get; }
}
