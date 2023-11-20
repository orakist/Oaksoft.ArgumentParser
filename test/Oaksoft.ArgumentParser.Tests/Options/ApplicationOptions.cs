using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Tests.Options;

public class BoolAppOptions : IApplicationOptions
{
    public bool Value { get; set; }
    public bool? NullValue { get; set; }
    public List<bool>? Values { get; set; }
    public List<bool?>? NullValues { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }
    public int ValueCount { get; set; }
    public int NullValueCount { get; set; }

    public bool Help { get; set; }
}

public class ByteAppOptions : IApplicationOptions
{
    public byte Value { get; set; }
    public byte? NullValue { get; set; }
    public List<byte>? Values { get; set; }
    public List<byte?>? NullValues { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }
    public int ValueCount { get; set; }
    public int NullValueCount { get; set; }

    public bool Help { get; set; }
}

public class SignedByteAppOptions : IApplicationOptions
{
    public sbyte Value { get; set; }
    public sbyte? NullValue { get; set; }
    public List<sbyte>? Values { get; set; }
    public List<sbyte?>? NullValues { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }
    public int ValueCount { get; set; }
    public int NullValueCount { get; set; }

    public bool Help { get; set; }
}

public class ShortAppOptions : IApplicationOptions
{
    public short Value { get; set; }
    public short? NullValue { get; set; }
    public List<short>? Values { get; set; }
    public List<short?>? NullValues { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }
    public int ValueCount { get; set; }
    public int NullValueCount { get; set; }

    public bool Help { get; set; }
}

public class UnsignedShortAppOptions : IApplicationOptions
{
    public ushort Value { get; set; }
    public ushort? NullValue { get; set; }
    public List<ushort>? Values { get; set; }
    public List<ushort?>? NullValues { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }
    public int ValueCount { get; set; }
    public int NullValueCount { get; set; }

    public bool Help { get; set; }
}

public class IntAppOptions : IApplicationOptions
{
    public int Value { get; set; }
    public int? NullValue { get; set; }
    public List<int>? Values { get; set; }
    public List<int?>? NullValues { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }
    public int ValueCount { get; set; }
    public int NullValueCount { get; set; }

    public bool Help { get; set; }
}

public class UnsignedIntAppOptions : IApplicationOptions
{
    public uint Value { get; set; }
    public uint? NullValue { get; set; }
    public List<uint>? Values { get; set; }
    public List<uint?>? NullValues { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }
    public int ValueCount { get; set; }
    public int NullValueCount { get; set; }

    public bool Help { get; set; }
}

public class LongAppOptions : IApplicationOptions
{
    public long Value { get; set; }
    public long? NullValue { get; set; }
    public List<long>? Values { get; set; }
    public List<long?>? NullValues { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }
    public int ValueCount { get; set; }
    public int NullValueCount { get; set; }

    public bool Help { get; set; }
}

public class UnsignedLongAppOptions : IApplicationOptions
{
    public ulong Value { get; set; }
    public ulong? NullValue { get; set; }
    public List<ulong>? Values { get; set; }
    public List<ulong?>? NullValues { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }
    public int ValueCount { get; set; }
    public int NullValueCount { get; set; }

    public bool Help { get; set; }
}

public class FloatAppOptions : IApplicationOptions
{
    public float Value { get; set; }
    public float? NullValue { get; set; }
    public List<float>? Values { get; set; }
    public List<float?>? NullValues { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }
    public int ValueCount { get; set; }
    public int NullValueCount { get; set; }

    public bool Help { get; set; }
}

public class DoubleAppOptions : IApplicationOptions
{
    public double Value { get; set; }
    public double? NullValue { get; set; }
    public List<double>? Values { get; set; }
    public List<double?>? NullValues { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }
    public int ValueCount { get; set; }
    public int NullValueCount { get; set; }

    public bool Help { get; set; }
}

public class DecimalAppOptions : IApplicationOptions
{
    public decimal Value { get; set; }
    public decimal? NullValue { get; set; }
    public List<decimal>? Values { get; set; }
    public List<decimal?>? NullValues { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }
    public int ValueCount { get; set; }
    public int NullValueCount { get; set; }

    public bool Help { get; set; }
}

public class CharAppOptions : IApplicationOptions
{
    public char Value { get; set; }
    public char? NullValue { get; set; }
    public List<char>? Values { get; set; }
    public List<char?>? NullValues { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }
    public int ValueCount { get; set; }
    public int NullValueCount { get; set; }

    public bool Help { get; set; }
}

public class StringAppOptions : IApplicationOptions
{
    public string Value { get; set; } = default!;
    public string? NullValue { get; set; }
    public List<string>? Values { get; set; }
    public List<string?>? NullValues { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }
    public int ValueCount { get; set; }
    public int NullValueCount { get; set; }

    public bool Help { get; set; }
}

public class DateTimeAppOptions : IApplicationOptions
{
    public DateTime Value { get; set; } = default!;
    public DateTime? NullValue { get; set; }
    public List<DateTime>? Values { get; set; }
    public List<DateTime?>? NullValues { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }
    public int ValueCount { get; set; }
    public int NullValueCount { get; set; }

    public bool Help { get; set; }
}

public class DateOnlyAppOptions : IApplicationOptions
{
    public DateOnly Value { get; set; } = default!;
    public DateOnly? NullValue { get; set; }
    public List<DateOnly>? Values { get; set; }
    public List<DateOnly?>? NullValues { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }
    public int ValueCount { get; set; }
    public int NullValueCount { get; set; }

    public bool Help { get; set; }
}

public class TimeOnlyAppOptions : IApplicationOptions
{
    public TimeOnly Value { get; set; } = default!;
    public TimeOnly? NullValue { get; set; }
    public List<TimeOnly>? Values { get; set; }
    public List<TimeOnly?>? NullValues { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }
    public int ValueCount { get; set; }
    public int NullValueCount { get; set; }

    public bool Help { get; set; }
}

public class TimeSpanAppOptions : IApplicationOptions
{
    public TimeSpan Value { get; set; } = default!;
    public TimeSpan? NullValue { get; set; }
    public List<TimeSpan>? Values { get; set; }
    public List<TimeSpan?>? NullValues { get; set; }

    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }
    public int ValueCount { get; set; }
    public int NullValueCount { get; set; }

    public bool Help { get; set; }
}