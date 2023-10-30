using System;
using System.Reflection;

namespace Glowy.CLxParser.Parser;

internal static class AssemblyHelper
{
    public static void Write(string text, ConsoleColor color)
    {
        var mainColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ForegroundColor = mainColor;
    }

    public static void WriteLine()
    {
        Console.WriteLine();
    }

    public static void WriteLine(string text)
    {
        Console.WriteLine(text);
    }

    public static string? GetAssemblyTitle()
    {
        var assembly = Assembly.GetEntryAssembly();
        var value = assembly?.GetCustomAttribute<AssemblyProductAttribute>()?.Product;

        if (string.IsNullOrWhiteSpace(value))
            value = assembly?.GetCustomAttribute<AssemblyTitleAttribute>()?.Title;

        if (string.IsNullOrWhiteSpace(value))
            value = Assembly.GetEntryAssembly()?.GetName().Name;

        return string.IsNullOrWhiteSpace(value) ? null : value;
    }

    public static string? GetAssemblyCompany()
    {
        var assembly = Assembly.GetEntryAssembly();
        var value = assembly?.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company;

        return string.IsNullOrWhiteSpace(value) ? null : value;
    }

    public static string? GetAssemblyCopyright()
    {
        var assembly = Assembly.GetEntryAssembly();
        var value = assembly?.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright;

        return string.IsNullOrWhiteSpace(value) ? null : value;
    }

    public static string? GetAssemblyDescription()
    {
        var assembly = Assembly.GetEntryAssembly();
        var value = assembly?.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;

        return string.IsNullOrWhiteSpace(value) ? null : value;
    }

    public static string? GetAssemblyVersion()
    {
        return Assembly.GetEntryAssembly()?.GetName().Version?.ToString();
    }
}
