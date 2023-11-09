using System;

namespace Oaksoft.ArgumentParser.Definitions;

[Flags]
public enum OptionPrefixRules
{
    /// <summary>
    /// The parser allows all option tokens that have single dash ('-') prefixed aliases.
    /// Valid examples: -o, -d, -start, -close
    /// </summary>
    AllowSingleDash = 0x01,

    /// <summary>
    /// The parser only allows option tokens with short aliases with single dash ('-') prefixes.
    /// Valid examples: -o, -d
    /// </summary>
    AllowSingleDashShortAlias = 0x02,

    /// <summary>
    /// The parser allows all option tokens that have double dash ('--') prefixed aliases.
    /// Valid examples: --o, --d, --start, --close
    /// </summary>
    AllowDoubleDash = 0x04,

    /// <summary>
    /// The parser only allows option tokens with long aliases with double dash ('--') prefixes.
    /// Valid examples: --start, --close
    /// </summary>
    AllowDoubleDashLongAlias = 0x08,

    /// <summary>
    /// The parser allows all option tokens that have forward slash ('/') prefixed aliases.
    /// Valid examples: /o, /d, /start, /close
    /// </summary>
    AllowForwardSlash = 0x10,

    /// <summary>
    /// Default option alias prefix rule
    /// Valid examples: -o, -d, --start, --close, /d, /start
    /// </summary>
    Default = AllowSingleDashShortAlias | AllowDoubleDashLongAlias | AllowForwardSlash,
}
