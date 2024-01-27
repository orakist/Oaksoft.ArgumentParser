using System;

namespace Oaksoft.ArgumentParser.Definitions;

[Flags]
public enum AliasDelimiterRules
{
    /// <summary>
    /// Parser allows equal symbol ('=') as the delimiter between an option name and its value.
    /// Valid examples: -v=quiet, --verbosity=quiet
    /// </summary>
    AllowEqualSymbol = 0x01,

    /// <summary>
    /// Parser allows colon symbol (':') as the delimiter between an option name and its value.
    /// Valid examples: -v:quiet, --verbosity:quiet
    /// </summary>
    AllowColonSymbol = 0x02,

    /// <summary>
    /// Parser allows omitting the delimiter when you are specifying an option with short alias.
    /// Following commands are equivalent: -vquiet, -v quiet
    /// </summary>
    AllowOmittingDelimiter = 0x04,

    /// <summary>
    /// Parser allows whitespace chars (' ') as the delimiter between an option name and its value when
    /// a token enclosed in quotation marks (").
    /// Following commands are equivalent: app.exe "-v quiet", app.exe -v quiet
    /// </summary>
    AllowWhitespace = 0x08,

    /// <summary>
    /// Default token delimiter rule
    /// </summary>
    Default = AllowEqualSymbol | AllowColonSymbol | AllowOmittingDelimiter | AllowWhitespace,

    /// <summary>
    /// All token delimiter rules enabled
    /// </summary>
    All = Default
}
