using System;
using System.Collections.Generic;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Console;

internal class ApplicationOptions : BaseApplicationOptions
{
    public bool AddSwitch { get; set; }
    public bool? SubtractSwitch { get; set; }
    public int MultiplySwitch { get; set; }
    public int? DivideSwitch { get; set; }

    public int AddCount { get; set; }
    public string? SubtractCount { get; set; }
    public float? MultiplyCount { get; set; }
    public long? DivideCount { get; set; }
    public DateTime? StartTime { get; set; }

    public IEnumerable<int>? AddNumbers { get; set; }
    public ICollection<string>? SubtractNumbers { get; set; }
    public List<double?>? MultiplyNumbers { get; set; }
    public long[]? DivideNumbers { get; set; }

    public bool FormulaEnabled { get; set; }
    public string? FormulaSign { get; set; }

    public int FormulaCount { get; set; }
    public List<string>? FormulaResults { get; set; }

    public IEnumerable<string>? Variables { get; set; }
}
