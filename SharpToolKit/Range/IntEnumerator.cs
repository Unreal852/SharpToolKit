namespace SharpToolKit.Range;

/// <summary>
/// Represents an enumerator to iterate over a <see cref="Range"/>
/// </summary>
public ref struct IntEnumerator
{
    private readonly int _end;
    private          int _current;

    public IntEnumerator(System.Range range)
    {
        if (range.End.IsFromEnd)
            throw new NotSupportedException();

        _end = range.End.Value;
        _current = range.Start.Value - 1;
    }

    public int Current => _current;

    public bool MoveNext()
    {
        _current++;
        return _current <= _end;
    }
}