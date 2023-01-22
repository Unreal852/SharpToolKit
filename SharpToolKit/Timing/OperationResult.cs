namespace SharpToolKit.Timing;

public readonly struct OperationResult
{
    public readonly string?  OperationName;
    public readonly DateTime StartedAt;
    public readonly DateTime EndedAt;
    public readonly TimeSpan TimeElapsed;

    public OperationResult(string? operationName, DateTime startedAt, TimeSpan timeElapsed)
    {
        OperationName = operationName;
        StartedAt = startedAt;
        TimeElapsed = timeElapsed;
        EndedAt = StartedAt + TimeElapsed;
    }
}