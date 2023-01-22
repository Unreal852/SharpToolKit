namespace SharpToolKit.Optional;

public static class Option
{
    public static Option<T> Some<T>(T value) => new(value);

    public static NoneOption None { get; } = new();
}

public readonly struct Option<T>
{
    private readonly T    _value;
    private readonly bool _hasValue;

    private Option(T value, bool hasValue)
    {
        _value = value;
        _hasValue = hasValue;
    }

    public Option(T value)
            : this(value, true)
    {
    }

    public static implicit operator Option<T>(NoneOption none) => new();
}