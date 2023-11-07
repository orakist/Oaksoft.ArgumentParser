namespace Oaksoft.ArgumentParser.Parser;

internal class TokenValue
{
    public string Argument { get; init; } = default!;

    public bool Invalid { get; set; }

    public bool IsParsed { get; set; }
}
