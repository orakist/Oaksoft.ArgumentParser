using System.Reflection;
using Glowy.CLxParser.Options;
using Glowy.CLxParser.Parser;

namespace Glowy.CLxParser.Callbacks;

public abstract class BaseParsingCallbacks : IParsingCallbacks
{
    public virtual bool ValidateValue(string value)
    {
        return true;
    }

    public virtual bool ValidateOption(IValueContext context, IArgumentParser parser)
    {
        return true;
    }

    public virtual void ApplyDefaultValue(IValueContext context, IApplicationOptions appOptions, PropertyInfo property)
    {
    }

    public virtual void UpdateOptionValue(IValueContext context, IApplicationOptions appOptions, PropertyInfo property)
    {
    }
}
