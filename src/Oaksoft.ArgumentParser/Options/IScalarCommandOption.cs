namespace Oaksoft.ArgumentParser.Options;

public interface IScalarCommandOption : ICommandOption, IHaveValueOption
{
    bool ValueTokenMustExist { get; }

    bool AllowSequentialValues { get; }
}
