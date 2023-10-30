using System.Collections.Generic;

namespace Glowy.CLxParser.Options;

public interface ICommandOption : IBaseOption
{
    string Command { get; }

    List<string> Commands { get; }

    List<string> CommandTokens { get; }
}

public interface INonCommandOption : IHaveValueOption
{
}
