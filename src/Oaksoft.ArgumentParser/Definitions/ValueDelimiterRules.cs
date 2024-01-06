using System;

namespace Oaksoft.ArgumentParser.Definitions;

[Flags]
public enum ValueDelimiterRules
{
    /// <summary>
    /// Parser allows semicolon symbol (';') as the delimiter between values.
    /// Valid examples: --colors blue;green;yellow
    /// </summary>
    AllowSemicolonSymbol = 0x01,

    /// <summary>
    /// Parser allows comma symbol (',') as the delimiter between values.
    /// Valid examples: --colors blue,green,yellow
    /// </summary>
    AllowCommaSymbol = 0x02,

    /// <summary>
    /// Parser allows pipe symbol ('|') as the delimiter between values.
    /// Valid examples: --colors blue|green|yellow
    /// </summary>
    AllowPipeSymbol = 0x04,

    /// <summary>
    /// Default value delimiter rule
    /// </summary>
    Default = AllowSemicolonSymbol | AllowCommaSymbol | AllowPipeSymbol,

    /// <summary>
    /// All value delimiter rules enabled
    /// </summary>
    All = Default
}
