using System.Collections.Generic;

namespace Oaksoft.ArgumentParser.Console;

enum TestEnum
{
    abc,
    Cdf,
    Xfd
}

internal class CalculatorOptions
{
    public TestEnum? Test{ get; set; }

    public double? LeftOperand { get; set; }

    public double? RightOperand { get; set; }

    public IEnumerable<double>? Numbers { get; set; }

    public IEnumerable<int>? Integers { get; set; }

    public string? Operator { get; set; }
}
