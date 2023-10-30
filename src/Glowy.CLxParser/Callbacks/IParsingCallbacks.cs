using System.Reflection;
using Glowy.CLxParser.Options;
using Glowy.CLxParser.Parser;

namespace Glowy.CLxParser.Callbacks;

public interface IParsingCallbacks
{
    bool ValidateValue(string value);

    bool ValidateOption(IValueContext context, IArgumentParser parser);

    void ApplyDefaultValue(IValueContext context, IApplicationOptions appOptions, PropertyInfo property);

    void UpdateOptionValue(IValueContext context, IApplicationOptions appOptions, PropertyInfo property);
}
