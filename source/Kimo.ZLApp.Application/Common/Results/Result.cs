namespace Kimo.ZLApp.Application.Common.Results;

public record Result<TValue>
{
    private readonly Error.Error? _error;
    private readonly TValue? _value;

    public Result(TValue value)
    {
        _value = value;
        _error = null;
        IsError = false;
    }

    public Result(Error.Error error)
    {
        _value = default;
        _error = error;
        IsError = true;
    }

    public bool IsError { get; }
    public bool IsSuccess => !IsError;

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access Value when in error state");

    public Error.Error Error => IsError
        ? _error!
        : throw new InvalidOperationException("Cannot access Error when in success state");

    public static implicit operator Result<TValue>(TValue value)
    {
        return new Result<TValue>(value);
    }

    public static implicit operator Result<TValue>(Error.Error error)
    {
        return new Result<TValue>(error);
    }
}

public record Result
{
    private readonly Error.Error? _error;

    public Result()
    {
        _error = null;
        IsError = false;
    }

    public Result(Error.Error error)
    {
        _error = error;
        IsError = true;
    }

    public bool IsError { get; }
    public bool IsSuccess => !IsError;

    public Error.Error Error => IsError
        ? _error!
        : throw new InvalidOperationException("Cannot access Error when in success state");

    public static Result Success()
    {
        return new Result();
    }

    public static implicit operator Result(Error.Error error)
    {
        return new Result(error);
    }
}