using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Tests;

public class BoolScalarOptions : IApplicationOptions
{
    public bool Value { get; set; }
    public bool? NullValue { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }

    public bool Help { get; set; }
}

public class ByteScalarOptions : IApplicationOptions
{
    public byte Value { get; set; }
    public byte? NullValue { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }

    public bool Help { get; set; }
}

public class SignedByteScalarOptions : IApplicationOptions
{
    public sbyte Value { get; set; }
    public sbyte? NullValue { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }

    public bool Help { get; set; }
}

public class ShortScalarOptions : IApplicationOptions
{
    public short Value { get; set; }
    public short? NullValue { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }

    public bool Help { get; set; }
}

public class UnsignedShortScalarOptions : IApplicationOptions
{
    public ushort Value { get; set; }
    public ushort? NullValue { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }

    public bool Help { get; set; }
}

public class IntScalarOptions : IApplicationOptions
{
    public int Value { get; set; }
    public int? NullValue { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }

    public bool Help { get; set; }
}

public class UnsignedIntScalarOptions : IApplicationOptions
{
    public uint Value { get; set; }
    public uint? NullValue { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }

    public bool Help { get; set; }
}

public class LongScalarOptions : IApplicationOptions
{
    public long Value { get; set; }
    public long? NullValue { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }

    public bool Help { get; set; }
}

public class UnsignedLongScalarOptions : IApplicationOptions
{
    public ulong Value { get; set; }
    public ulong? NullValue { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }

    public bool Help { get; set; }
}

public class FloatScalarOptions : IApplicationOptions
{
    public float Value { get; set; }
    public float? NullValue { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }

    public bool Help { get; set; }
}

public class DoubleScalarOptions : IApplicationOptions
{
    public double Value { get; set; }
    public double? NullValue { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }

    public bool Help { get; set; }
}

public class DecimalScalarOptions : IApplicationOptions
{
    public decimal Value { get; set; }
    public decimal? NullValue { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }

    public bool Help { get; set; }
}

public class CharScalarOptions : IApplicationOptions
{
    public char Value { get; set; }
    public char? NullValue { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }

    public bool Help { get; set; }
}

public class StringScalarOptions : IApplicationOptions
{
    public string Value { get; set; } = default!;
    public string? NullValue { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }

    public bool Help { get; set; }
}

public class DateTimeScalarOptions : IApplicationOptions
{
    public DateTime Value { get; set; } = default!;
    public DateTime? NullValue { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }

    public bool Help { get; set; }
}

public class DateOnlyScalarOptions : IApplicationOptions
{
    public DateOnly Value { get; set; } = default!;
    public DateOnly? NullValue { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }

    public bool Help { get; set; }
}

public class TimeOnlyScalarOptions : IApplicationOptions
{
    public TimeOnly Value { get; set; } = default!;
    public TimeOnly? NullValue { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }

    public bool Help { get; set; }
}

public class TimeSpanScalarOptions : IApplicationOptions
{
    public TimeSpan Value { get; set; } = default!;
    public TimeSpan? NullValue { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }

    public bool Help { get; set; }
}