using System.Reflection;

namespace Glowy.CLxParser.Parser;

internal static class AssemblyHelper
{
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
        var name = Assembly.GetEntryAssembly()?.GetName().Name;

        return string.IsNullOrWhiteSpace(value) || name == value ? null : value;
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
        var assembly = Assembly.GetEntryAssembly();
        var value = assembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

        if (string.IsNullOrWhiteSpace(value))
            value = Assembly.GetEntryAssembly()?.GetName().Version?.ToString(3);

        return string.IsNullOrWhiteSpace(value) ? null : value;
    }
}
