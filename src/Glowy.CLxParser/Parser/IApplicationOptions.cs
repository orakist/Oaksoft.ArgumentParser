using System.Collections.Generic;
using Oaksoft.ArgumentParser.Options;

namespace Oaksoft.ArgumentParser.Parser;

public interface IApplicationOptions
{
    bool Help { get; set; }

    List<IBaseOption> Options { get; }
}
