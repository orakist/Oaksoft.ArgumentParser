using Oaksoft.ArgumentParser.Callbacks;

namespace Oaksoft.ArgumentParser.Tests.TestModels;

internal record Coordinate(int X, int Y) : IComparable
{
    public int CompareTo(object? obj)
    {
        if (obj == null)
            return 1;

        if (ReferenceEquals(obj, this))
            return 0;

        if (obj is not Coordinate point)
            throw new ArgumentException("Invalid argument type!");

        if (X < point.X)
            return -1;
        if (X > point.X)
            return 1;
        if (Y < point.Y)
            return -1;
        if (Y > point.Y)
            return 1;

        return 0;
    }
}

internal static class PointTryParseCallback
{
    public static bool TryParse(string value, out Coordinate result)
    {
        result = default!;

        if (string.IsNullOrWhiteSpace(value) || value.Length < 5)
            return false;

        if (value[0] != '(' || value[^1] != ')')
            return false;

        var values = value[1..^1].Split(';');
        if (values.Length != 2)
            return false;

        if (!int.TryParse(values[0], out var x))
            return false;
        if (!int.TryParse(values[1], out var y))
            return false;

        result = new Coordinate(x, y);
        return true;
    }
}
