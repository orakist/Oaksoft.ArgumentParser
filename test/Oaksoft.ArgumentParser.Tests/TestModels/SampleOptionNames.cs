using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace Oaksoft.ArgumentParser.Tests.TestModels;

internal class ValueOptions
{
    public IEnumerable<int>? EnumerableValueItems { get; set; }

    public Collection<int>? CollectionValueItems { get; set; }
    public ICollection<short>? Collection1ValueItems { get; set; }
    public IReadOnlyCollection<long>? Collection2ValueItems { get; set; }

    public List<double>? ListValueItems { get; set; }
    public IList<int>? List1ValueItems { get; set; }
    public IReadOnlyList<float>? List2ValueItems { get; set; }

    public int[]? ArrayValueItems { get; set; }

    public HashSet<string>? HashSetValueItems { get; set; }
    public ISet<string>? HashSet1ValueItems { get; set; }
    public IReadOnlySet<Coordinate>? HashSet2ValueItems { get; set; }
}

internal class NullableValueOptions
{
    public IEnumerable<int?>? EnumerableNullableItems { get; set; }

    public Collection<int?>? CollectionNullableItems { get; set; }
    public ICollection<short?>? Collection1NullableItems { get; set; }
    public IReadOnlyCollection<long?>? Collection2NullableItems { get; set; }

    public List<double?>? ListNullableItems { get; set; }
    public IList<int?>? List1NullableItems { get; set; }
    public IReadOnlyList<float?>? List2NullableItems { get; set; }

    public int?[]? ArrayNullableItems { get; set; }
    
    public HashSet<string?>? HashSetNullableItems { get; set; }
    public ISet<string?>? HashSet1NullableItems { get; set; }
    public IReadOnlySet<Coordinate?>? HashSet2NullableItems { get; set; }
}

internal class SampleOptionNames
{
    public int V { get; set; }

    public int Value { get; set; }

    public bool ValueTest { get; set; }

    public List<int>? ValueTestProp { get; set; }

    public Coordinate? PointValue { get; set; }

    public List<Coordinate>? PointValues { get; set; }
    
    public int ValueTestPropEx { get; set; }

    public int ABCValue { get; set; }

    public int VeryLongApplicationOptionValuePropertyName { get; set; }

    public int __1_Help_1 { get; set; }

    public int __1_Help { get; set; }

    public int __2_Value { get; set; }

    public int __1_2_3 { get; set; }

    public int __3_Va1_l2_3ue { get; set; }

    public int Val1 { get; set; }

    public int Val2 { get; set; }

    public int Val3 { get; set; }

    public int Val4 { get; set; }

    public int Val4_ { get; set; }

    public bool Help { get; set; }

    public List<int?>? WithoutSet { get; }

    public ImmutableHashSet<int?>? Unknown { get; set; }
}