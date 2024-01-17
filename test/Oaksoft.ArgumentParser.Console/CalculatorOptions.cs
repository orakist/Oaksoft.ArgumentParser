using System.Collections.Generic;

namespace Oaksoft.ArgumentParser.Console;

internal class CalculatorOptions
{
    public double? LeftOperand { get; set; }

    public double? RightOperand { get; set; }

    public IEnumerable<double>? Numbers { get; set; }

    public IEnumerable<int>? Integers { get; set; }

    public string? Operator { get; set; }
}
