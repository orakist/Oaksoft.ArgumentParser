using System.Reflection;

namespace Oaksoft.ArgumentParser.Options;

internal sealed class SwitchOption 
    : ScalarNamedOption<bool>, ISwitchOption
{
    public SwitchOption(int requiredOptionCount, int maximumOptionCount)
        : base(requiredOptionCount, maximumOptionCount, 0, 1)
    {
    }

    public override void ApplyOptionResult(object appOptions, PropertyInfo keyProperty)
    {
        if (!keyProperty.PropertyType.IsAssignableFrom(typeof(bool)))
            return;

        keyProperty.SetValue(appOptions, ResultValue?.Value ?? DefaultValue?.Value ?? true);
    }
}
