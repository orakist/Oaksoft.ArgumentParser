using System.Collections.Generic;

namespace Oaksoft.ArgumentParser.Tester;

internal enum OperatorType { Add, Sub, Mul, Div }

internal class CalculatorOptions
{
    public double? LeftOperand { get; set; }

    public double? RightOperand { get; set; }

    public IEnumerable<double>? Numbers { get; set; }

    public IEnumerable<int>? Integers { get; set; }

    public OperatorType? Operator { get; set; }
}
