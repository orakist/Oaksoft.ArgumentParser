using System.Collections.Generic;
using System.Linq;

using Glowy.CLxParser.Options;

namespace Glowy.CLxParser.Parser;

public abstract class BaseApplicationOptions : IApplicationOptions
{
    public bool Help { get; set; }

    public List<IBaseOption> Options => _options.Values.ToList();

    private readonly Dictionary<string, IBaseOption> _options = new();
}
