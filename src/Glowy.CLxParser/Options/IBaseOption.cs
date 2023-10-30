namespace Glowy.CLxParser.Options;

public interface IBaseOption
{
    string Name { get; }

    string Usage { get; }

    string? Description { get; }

    int RequiredTokenCount { get; }

    int MaximumTokenCount { get; }

    int ValidatedTokenCount { get; }
}
