using System.Reflection;
using Oaksoft.ArgumentParser.Options;
using Oaksoft.ArgumentParser.Parser;

namespace Oaksoft.ArgumentParser.Callbacks;

public interface IParsingCallbacks
{
    bool ValidateValue(string value);

    bool ValidateOption(IValueContext context, IArgumentParser parser);

    void ApplyDefaultValue(IValueContext context, IApplicationOptions appOptions, PropertyInfo property);

    void UpdateOptionValue(IValueContext context, IApplicationOptions appOptions, PropertyInfo property);
}
