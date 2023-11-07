namespace Oaksoft.ArgumentParser.Options;

public interface ISwitchOption : INamedOption
{
    bool? DefaultValue { get; }

    bool? ResultValue { get; }
}
