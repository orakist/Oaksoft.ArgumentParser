using System.Collections.Generic;
using Glowy.CLxParser.Options;

namespace Glowy.CLxParser.Parser;

public interface IApplicationOptions
{
    bool Help { get; set; }

    List<IBaseOption> Options { get; }
}
