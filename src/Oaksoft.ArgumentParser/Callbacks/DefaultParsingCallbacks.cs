using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Callbacks;

internal sealed class DefaultParsingCallbacks : BaseParsingCallbacks
{
    public static readonly DefaultParsingCallbacks Instance = new();

    private DefaultParsingCallbacks()
    {
    }

    public override bool ValidateValue(string value)
    {
        return true;
    }

    public override bool ValidateOption(IValueContext context, IArgumentParser parser)
    {
        return true;
    }

    public override void ApplyDefaultValue(IValueContext context, IApplicationOptions appOptions, PropertyInfo property)
    {
    }

    public override void UpdateOptionValue(IValueContext context, IApplicationOptions appOptions, PropertyInfo property)
    {
    }
}
