namespace SharpTools.Optional;

public static class Result
{
    public static DelayedOk<TOk>       Ok<TOk>(TOk ok)             => new(ok);
    public static DelayedError<TError> Error<TError>(TError error) => new(error);
}

public readonly struct Result<TOk, TError>
{
    private readonly TOk    _ok;
    private readonly TError _error;
    private readonly bool   _isError;

    private Result(TOk ok, TError error, bool isError)
    {
        _ok = ok;
        _error = error;
        _isError = isError;
    }

    public Result(TOk ok) : this(ok, default!, false)
    {
    }

    public Result(TError error) : this(default!, error, true)
    {
    }

    public static implicit operator Result<TOk, TError>(DelayedOk<TOk> ok)          => new(ok.Value);
    public static implicit operator Result<TOk, TError>(DelayedError<TError> error) => new(error.Value);
}