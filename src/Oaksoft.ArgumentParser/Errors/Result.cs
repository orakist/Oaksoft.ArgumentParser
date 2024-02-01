namespace Oaksoft.ArgumentParser.Errors;

internal sealed class Result<TValue>
{
    public TValue? Value { get; }

    public ErrorMessage? Error { get; }

    public bool Success => Error is null;

    private Result(ErrorMessage? error)
    {
        Error = error;
    }

    private Result(TValue value)
    {
        Value = value;
    }

    public TValue GetOrThrow()
    {
        if (Success)
        {
            return Value!;
        }

        throw Error!.ToException();
    }

    public TValue GetOrThrow(string optionName)
    {
        Error?.WithName(optionName);

        return GetOrThrow();
    }

    public static implicit operator Result<TValue>(TValue value) => new(value);

    public static implicit operator Result<TValue>(ErrorMessage error) => new(error);
}
