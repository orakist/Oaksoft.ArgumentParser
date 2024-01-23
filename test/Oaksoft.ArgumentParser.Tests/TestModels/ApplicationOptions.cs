namespace Oaksoft.ArgumentParser.Tests.TestModels;

public abstract class BaseAppOptions
{
    public bool ValueFlag { get; set; }
    public bool NullValueFlag { get; set; }
    public int ValueCount { get; set; }
    public int NullValueCount { get; set; }
}

public class BoolAppOptions : BaseAppOptions
{
    public bool Value { get; set; }
    public bool? NullValue { get; set; }
    public List<bool>? Values { get; set; }
    public List<bool?>? NullValues { get; set; }
}

public class ByteAppOptions : BaseAppOptions
{
    public byte Value { get; set; }
    public byte? NullValue { get; set; }
    public List<byte>? Values { get; set; }
    public List<byte?>? NullValues { get; set; }
}

public class SignedByteAppOptions : BaseAppOptions
{
    public sbyte Value { get; set; }
    public sbyte? NullValue { get; set; }
    public List<sbyte>? Values { get; set; }
    public List<sbyte?>? NullValues { get; set; }
}

public class ShortAppOptions : BaseAppOptions
{
    public short Value { get; set; }
    public short? NullValue { get; set; }
    public List<short>? Values { get; set; }
    public List<short?>? NullValues { get; set; }
}

public class UnsignedShortAppOptions : BaseAppOptions
{
    public ushort Value { get; set; }
    public ushort? NullValue { get; set; }
    public List<ushort>? Values { get; set; }
    public List<ushort?>? NullValues { get; set; }
}

public class IntAppOptions : BaseAppOptions
{
    public int Value { get; set; }
    public int? NullValue { get; set; }
    public int[]? Values { get; set; }
    public int?[]? NullValues { get; set; }
}

public class UnsignedIntAppOptions : BaseAppOptions
{
    public uint Value { get; set; }
    public uint? NullValue { get; set; }
    public List<uint>? Values { get; set; }
    public List<uint?>? NullValues { get; set; }
}

public class LongAppOptions : BaseAppOptions
{
    public long Value { get; set; }
    public long? NullValue { get; set; }
    public List<long>? Values { get; set; }
    public List<long?>? NullValues { get; set; }
}

public class UnsignedLongAppOptions : BaseAppOptions
{
    public ulong Value { get; set; }
    public ulong? NullValue { get; set; }
    public List<ulong>? Values { get; set; }
    public List<ulong?>? NullValues { get; set; }
}

public class FloatAppOptions : BaseAppOptions
{
    public float Value { get; set; }
    public float? NullValue { get; set; }
    public List<float>? Values { get; set; }
    public List<float?>? NullValues { get; set; }
}

public class DoubleAppOptions : BaseAppOptions
{
    public double Value { get; set; }
    public double? NullValue { get; set; }
    public List<double>? Values { get; set; }
    public List<double?>? NullValues { get; set; }
}

public class DecimalAppOptions : BaseAppOptions
{
    public decimal Value { get; set; }
    public decimal? NullValue { get; set; }
    public List<decimal>? Values { get; set; }
    public List<decimal?>? NullValues { get; set; }
}

public class CharAppOptions : BaseAppOptions
{
    public char Value { get; set; }
    public char? NullValue { get; set; }
    public List<char>? Values { get; set; }
    public List<char?>? NullValues { get; set; }
}

public class StringAppOptions : BaseAppOptions
{
    public string Value { get; set; } = default!;
    public string? NullValue { get; set; }
    public List<string>? Values { get; set; }
    public List<string?>? NullValues { get; set; }
}