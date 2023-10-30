using System.Collections.Generic;
using Glowy.CLxParser.Parser;

namespace Glowy.CLxParser.Console;

internal class ApplicationOptions : BaseApplicationOptions
{
    public ICollection<string>? FilePaths { get; set; }

    public bool ShowMismatches { get; set; }
    public bool ShowFormulas { get; set; }
    public bool ShowLoadingErrors { get; set; }
    public bool CompareGrammars { get; set; }

    public bool CalculationPerformance { get; set; }
    public bool ReadingPerformance { get; set; }
    public int CalculationCount { get; set; }

    public string? RangeFilter { get; set; }
    public List<string>? ValueTypeFilter { get; set; }
    public int PrecisionFilter { get; set; }

    public bool AutomationEnabled { get; set; }
    public string? AutomationOutputPath { get; set; }

    public bool DependencyEnabled { get; set; }
    public string? DependencyOutputPath { get; set; }

    public bool SolverEnabled { get; set; }
    public string? SolverInputPath { get; set; }

    public bool LogFileEnabled { get; set; }
    public string? LogFileOutputPath { get; set; }

    public string? Culture { get; set; }
    public bool Verbose { get; set; }
}
