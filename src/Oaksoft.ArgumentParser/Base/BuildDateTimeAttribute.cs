using System;

namespace Oaksoft.ArgumentParser.Base;

[AttributeUsage(AttributeTargets.Assembly)]
internal sealed class BuildDateTimeAttribute : Attribute
{
    public string BuildDateTime { get; }

    public BuildDateTimeAttribute(string buildDateTime)
    {
        BuildDateTime = buildDateTime;
    }
}
