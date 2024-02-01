using System.Collections.Generic;
using System.Text;

namespace Oaksoft.ArgumentParser.Base;

internal static class CommandLineExtensions
{
    public static IEnumerable<string> SplitToArguments(this string args)
    {
        var paramChars = args.ToCharArray();
        var inSingleQuote = false;
        var inDoubleQuote = false;
        var escaped = false;
        var lastSplitted = false;
        var justSplitted = false;
        var lastQuoted = false;
        var justQuoted = false;

        int i, j;

        for (i = 0, j = 0; i < paramChars.Length; i++, j++)
        {
            paramChars[j] = paramChars[i];

            if (!escaped)
            {
                if (paramChars[i] == '^')
                {
                    escaped = true;
                    j--;
                }
                else if (paramChars[i] == '"' && !inSingleQuote)
                {
                    inDoubleQuote = !inDoubleQuote;
                    paramChars[j] = '\n';
                    justSplitted = true;
                    justQuoted = true;
                }
                else if (paramChars[i] == '\'' && !inDoubleQuote)
                {
                    inSingleQuote = !inSingleQuote;
                    paramChars[j] = '\n';
                    justSplitted = true;
                    justQuoted = true;
                }
                else if (!inSingleQuote && !inDoubleQuote && paramChars[i] == ' ')
                {
                    paramChars[j] = '\n';
                    justSplitted = true;
                }

                if (justSplitted && lastSplitted && (!lastQuoted || !justQuoted))
                {
                    j--;
                }

                lastSplitted = justSplitted;
                justSplitted = false;

                lastQuoted = justQuoted;
                justQuoted = false;
            }
            else
            {
                escaped = false;
            }
        }

        if (lastQuoted)
        {
            j--;
        }

        return (new string(paramChars, 0, j)).Split(new[] { '\n' });
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
