using System;
using System.Collections.Generic;
using System.Drawing;
#if !NET7_0_OR_GREATER
    using System.Linq;
#endif
using System.Text;
using System.Text.RegularExpressions;

namespace Oaksoft.ArgumentParser.Base;

/// <summary>
/// Controls colored console output by extension methods.
/// </summary>
#if NET7_0_OR_GREATER
internal static partial class TextColoring
#else
internal static class TextColoring
#endif
{
    private static bool _enabled;

    private enum ColorPlane : byte
    {
        Foreground,
        Background
    }

    private static readonly Dictionary<ConsoleColor, Color> _consoleColorMapper = new()
    {
        [ConsoleColor.Black] = Color.FromArgb(0x000000),
        [ConsoleColor.DarkBlue] = Color.FromArgb(0x00008B),
        [ConsoleColor.DarkGreen] = Color.FromArgb(0x006400),
        [ConsoleColor.DarkCyan] = Color.FromArgb(0x008B8B),
        [ConsoleColor.DarkRed] = Color.FromArgb(0x8B0000),
        [ConsoleColor.DarkMagenta] = Color.FromArgb(0x8B008B),
        [ConsoleColor.DarkYellow] = Color.FromArgb(0x808000),
        [ConsoleColor.Gray] = Color.FromArgb(0x808080),
        [ConsoleColor.DarkGray] = Color.FromArgb(0xA9A9A9),
        [ConsoleColor.Blue] = Color.FromArgb(0x0000FF),
        [ConsoleColor.Green] = Color.FromArgb(0x008000),
        [ConsoleColor.Cyan] = Color.FromArgb(0x00FFFF),
        [ConsoleColor.Red] = Color.FromArgb(0xFF0000),
        [ConsoleColor.Magenta] = Color.FromArgb(0xFF00FF),
        [ConsoleColor.Yellow] = Color.FromArgb(0xFFFF00),
        [ConsoleColor.White] = Color.FromArgb(0xFFFFFF)
    };

    private static readonly Dictionary<ColorPlane, string> _planeFormatModifiers = new()
    {
        [ColorPlane.Foreground] = "38",
        [ColorPlane.Background] = "48"
    };

#if NET7_0_OR_GREATER
    [GeneratedRegex("(?:\u001b\\[0m)+")]
    private static partial Regex CloseNestedPastelStringRegex1();

    [GeneratedRegex("(?<!^)(?<!\u001b\\[0m)(?<!\u001b\\[(?:38|48);2;\\d{1,3};\\d{1,3};\\d{1,3}m)(?:\u001b\\[(?:38|48);2;)")]
    private static partial Regex CloseNestedPastelStringRegex2();

    [GeneratedRegex("(?:\u001b\\[0m)(?!\u001b\\[38;2;)(?!$)")]
    private static partial Regex CloseNestedPastelStringRegex3Foreground();

    [GeneratedRegex("(?:\u001b\\[0m)(?!\u001b\\[48;2;)(?!$)")]
    private static partial Regex CloseNestedPastelStringRegex3Background();

    private static readonly Dictionary<ColorPlane, Regex> _closeNestedPastelStringRegex3 = new()
    {
        [ColorPlane.Foreground] = CloseNestedPastelStringRegex3Foreground(),
        [ColorPlane.Background] = CloseNestedPastelStringRegex3Background()
    };
#else
    private static readonly Regex _closeNestedPastelStringRegex1
        = new($"({FormatStringEnd.Replace("[", @"\[")})+", RegexOptions.Compiled);

    private static readonly Regex _closeNestedPastelStringRegex2 
        = new(InitializeCloseNestedPastelString2(), RegexOptions.Compiled);

    private static Regex CloseNestedPastelStringRegex1() => _closeNestedPastelStringRegex1;
    private static Regex CloseNestedPastelStringRegex2() => _closeNestedPastelStringRegex2;

    private static string InitializeCloseNestedPastelString2()
    {
        var formatPart = $"(?:{_planeFormatModifiers[ColorPlane.Foreground]}|{_planeFormatModifiers[ColorPlane.Background]})";
        var arguments = new[] { formatPart }.Concat(Enumerable.Repeat(@"\d{1,3}", 3)).Cast<object>().ToArray();

        return $"(?<!^)(?<!{FormatStringEnd.Replace("[", @"\[")})" +
            $"(?<!{string.Format($"{FormatStringStart.Replace("[", @"\[")}{FormatStringColor}", arguments)})" +
            $"(?:{string.Format(FormatStringStart.Replace("[", @"\["), formatPart)})";
    }


    private static readonly string _closeNestedPastelStringRegex3FormatString = $"(?:{FormatStringEnd.Replace("[", @"\[")})(?!{FormatStringStart.Replace("[", @"\[")})(?!$)";
    private static readonly Dictionary<ColorPlane, Regex> _closeNestedPastelStringRegex3 = new()
    {
        [ColorPlane.Foreground] = new Regex(string.Format(_closeNestedPastelStringRegex3FormatString, _planeFormatModifiers[ColorPlane.Foreground])),
        [ColorPlane.Background] = new Regex(string.Format(_closeNestedPastelStringRegex3FormatString, _planeFormatModifiers[ColorPlane.Background]))
    };
#endif

    private const string FormatStringStart = "\u001b[{0};2;";
    private const string FormatStringColor = "{1};{2};{3}m";
    private const string FormatStringContent = "{4}";
    private const string FormatStringEnd = "\u001b[0m";
    private const string FormatStringPartial = $"{FormatStringStart}{FormatStringColor}";
    private const string FormatStringFull = $"{FormatStringStart}{FormatStringColor}{FormatStringContent}{FormatStringEnd}";

    /// <summary>
    /// Enables or disables any future console color output produced by Pastel.
    /// </summary>
    public static void SetEnabled(bool enabled)
    {
        _enabled = enabled;
    }

    /// <summary>
    /// Returns a string wrapped in an ANSI foreground color code using the specified color.
    /// </summary>
    /// <param name="input">The string to color.</param>
    /// <param name="color">The color to use on the specified string.</param>
    public static string Pastel(this string input, Color color)
    {
        return _enabled ? ColorFormat(input, color, ColorPlane.Foreground) : input;
    }

    /// <summary>
    /// Returns a string wrapped in an ANSI foreground color code using the specified color.
    /// </summary>
    /// <param name="input">The string to color.</param>
    /// <param name="color">The color to use on the specified string.</param>
    public static string Pastel(this string input, ConsoleColor color)
    {
        return Pastel(input, _consoleColorMapper[color]);
    }

    /// <summary>
    /// Appends a string wrapped in an ANSI foreground color code using the specified color.
    /// </summary>
    /// <param name="sb">string builder to append the colorized input string</param>
    /// <param name="input">The string to color.</param>
    /// <param name="color">The color to use on the specified string.</param>
    public static StringBuilder Pastel(this StringBuilder sb, string input, ConsoleColor color)
    {
        return sb.Append(Pastel(input, _consoleColorMapper[color]));
    }

    /// <summary>
    /// Returns a string wrapped in an ANSI background color code using the specified color.
    /// </summary>
    /// <param name="input">The string to color.</param>
    /// <param name="color">The color to use on the specified string.</param>
    public static string PastelBg(this string input, Color color)
    {
        return _enabled ? ColorFormat(input, color, ColorPlane.Background) : input;
    }

    /// <summary>
    /// Returns a string wrapped in an ANSI background color code using the specified color.
    /// </summary>
    /// <param name="input">The string to color.</param>
    /// <param name="color">The color to use on the specified string.</param>
    public static string PastelBg(this string input, ConsoleColor color)
    {
        return PastelBg(input, _consoleColorMapper[color]);
    }

    private static string ColorFormat(string input, Color color, ColorPlane colorPlane)
    {
        return string.Format(
            FormatStringFull,
            _planeFormatModifiers[colorPlane], color.R, color.G, color.B,
            CloseNestedPastelStrings(input, color, colorPlane));
    }

    private static string CloseNestedPastelStrings(string input, Color color, ColorPlane colorPlane)
    {
        var closedString = CloseNestedPastelStringRegex1().Replace(input, FormatStringEnd);

        closedString = CloseNestedPastelStringRegex2().Replace(closedString, $"{FormatStringEnd}$0");
        closedString = _closeNestedPastelStringRegex3[colorPlane].Replace(closedString,
            $"$0{string.Format(FormatStringPartial, _planeFormatModifiers[colorPlane], color.R, color.G, color.B)}");

        return closedString;
    }
}
