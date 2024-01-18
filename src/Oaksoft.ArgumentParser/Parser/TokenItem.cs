namespace Oaksoft.ArgumentParser.Parser;

internal class TokenItem
{
    /// <summary>
    /// Represents a command line argument element
    /// </summary>
    public string Token { get; set; } = default!;

    /// <summary>
    /// If exists, represents alias part of the token
    /// </summary>
    public string? Alias { get; set; }

    /// <summary>
    /// If exists, represents value part of the token
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// Indicates that the token is invalid.
    /// </summary>
    public bool Invalid { get; set; }

    /// <summary>
    /// Indicates that the token was parsed successfully.
    /// </summary>
    public bool IsParsed { get; set; }

    /// <summary>
    /// Indicates that token is not parsed and contains only value 
    /// </summary>
    public bool IsOnlyValue 
        => !IsParsed && !Invalid && Alias is null && Value is not null;
}
