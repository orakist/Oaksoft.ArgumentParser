namespace Oaksoft.ArgumentParser.Options;

public interface ISwitchOption : IAliasedOption
{
    bool? DefaultValue { get; }
}
