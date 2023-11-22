using Oaksoft.ArgumentParser.Definitions;

namespace Oaksoft.ArgumentParser.Tests.Extensions;

internal static class TestExtensions
{
    public static (int Min, int Max) GetLimits(this ArityType arityType)
    {
        return arityType switch
        {
            ArityType.Zero => (0, 0),
            ArityType.ZeroOrOne => (0, 1),
            ArityType.ExactlyOne => (1, 1),
            ArityType.ZeroOrMore => (0, int.MaxValue),
            ArityType.OneOrMore => (1, int.MaxValue),
            _ => throw new ArgumentOutOfRangeException(nameof(ArityType), arityType, "Invalid ArityType enum value.")
        };
    }
}