namespace SharpTools.Optional;

public readonly struct DelayedError<T>
{
    public T Value { get; }

    public DelayedError(T value)
    {
        Value = value;
    }
}