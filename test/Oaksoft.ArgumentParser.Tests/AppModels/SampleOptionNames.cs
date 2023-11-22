using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Tests.AppModels;

public class SampleOptionNames : IApplicationOptions
{
    public int Value { get; set; }

    public bool ValueTest { get; set; }

    public List<int>? ValueTestProp { get; set; }

    public int ValueTestPropEx { get; set; }

    public int VeryLongApplicationOptionValuePropertyName { get; set; }

    public int __1_Help_1 { get; set; }

    public int __1_Help { get; set; }

    public int __2_Value { get; set; }

    public int __3_Va1_l2_3ue { get; set; }

    public int Val1 { get; set; }

    public int Val2 { get; set; }

    public int Val3 { get; set; }

    public int Val4 { get; set; }

    public bool Help { get; set; }
}