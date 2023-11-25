namespace Oaksoft.ArgumentParser.Exceptions;

internal class Result<TValue>
{
    public TValue? Value { get; }

    public ErrorMessage? Error { get; }

    public bool Success => Error is null;

    private Result(TValue value)
    {
        Value = value;
    }

    private Result(ErrorMessage? error)
    {
        Error = error;
    }

    public TValue GetOrThrow()
    {
        if (Success)
            return Value!;

        throw new OptionBuilderException(Error!);
    }

    public TValue GetOrThrow(string optionName)
    {
        if (Success)
            return Value!;

        Error!.WithName(optionName);

        throw new OptionBuilderException(Error!);
    }

    public static Result<TValue> Create(TValue value) => new(value);

    public static Result<TValue> Create(ErrorMessage error) => new(error);

    public static implicit operator Result<TValue>(TValue value) => Create(value);

    public static implicit operator Result<TValue>(ErrorMessage error) => Create(error);
}