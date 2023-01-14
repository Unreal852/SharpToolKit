namespace SharpTools.Optional;

public readonly struct DelayedOk<T>
{
    public T Value { get; }

    public DelayedOk(T value)
    {
        Value = value;
    }
}