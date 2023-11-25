using Oaksoft.ArgumentParser.Definitions;

namespace Oaksoft.ArgumentParser.Tests.TestModels;

internal static class TestExtensions
{
    public static (int Min, int Max) GetLimits(this ArityType arityType)
    {
        return arityType switch
        {
            ArityType.ZeroOrOne => (0, 1),
            ArityType.ExactlyOne => (1, 1),
            ArityType.ZeroOrMore => (0, int.MaxValue),
            ArityType.OneOrMore => (1, int.MaxValue),
            _ => (0, 0),
        };
    }
}