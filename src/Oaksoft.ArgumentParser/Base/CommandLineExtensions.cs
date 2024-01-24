using System.Collections.Generic;
using System.Text;

namespace Oaksoft.ArgumentParser.Base;

internal static class CommandLineExtensions
{
    public static IEnumerable<string> SplitToArguments(this string commandLine)
    {
        var result = new StringBuilder();

        var quoted = false;
        var escaped = false;
        var started = false;
        var allowCaret = false;

        for (var i = 0; i < commandLine.Length; i++)
        {
            var chr = commandLine[i];

            if (chr == '^' && !quoted)
            {
                if (allowCaret)
                {
                    result.Append(chr);
                    started = true;
                    escaped = false;
                    allowCaret = false;
                }
                else if (i + 1 < commandLine.Length && commandLine[i + 1] == '^')
                {
                    allowCaret = true;
                }
                else if (i + 1 == commandLine.Length)
                {
                    result.Append(chr);
                    started = true;
                    escaped = false;
                }
            }
            else if (escaped)
            {
                result.Append(chr);
                started = true;
                escaped = false;
            }
            else if (chr == '"')
            {
                quoted = !quoted;
                started = true;
            }
            else if (chr == '\\' && i + 1 < commandLine.Length && commandLine[i + 1] == '"')
            {
                escaped = true;
            }
            else if (chr == ' ' && !quoted)
            {
                if (started)
                {
                    yield return result.ToString();
                }

                result.Clear();
                started = false;
            }
            else
            {
                result.Append(chr);
                started = true;
            }
        }

        if (started)
        {
            yield return result.ToString();
        }
    }

    public static string GetCommaByEndsWith(this StringBuilder builder)
    {
        if (builder.Length < 1)
        {
            return string.Empty;
        }

        return builder[^1] == '.' ? " " : ", ";
    }
}
