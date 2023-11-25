namespace Oaksoft.ArgumentParser.Exceptions;

public class Result<TValue>
{
    public TValue? Value { get; }

    public BaseError? Error { get; }

    public bool Success => Error is null && Value is not null;

    private Result(TValue value)
    {
        Value = value;
    }

    private Result(BaseError? error)
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

        if (string.IsNullOrWhiteSpace(optionName))
            return GetOrThrow();

        throw new OptionBuilderException(Error!, optionName);
    }

    public static Result<TValue> Create(TValue value) => new(value);

    public static Result<TValue> Create(BaseError error) => new(error);

    public static implicit operator Result<TValue>(TValue value) => Create(value);

    public static implicit operator Result<TValue>(BaseError error) => Create(error);
}