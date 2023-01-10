namespace SharpTools.Timing;

public readonly struct OperationResult
{
    public readonly string?  OperationName;
    public readonly DateTime StartedAt;
    public readonly TimeSpan TimeElapsed;

    public OperationResult(string? operationName, DateTime startedAt, TimeSpan timeElapsed)
    {
        OperationName = operationName;
        StartedAt = startedAt;
        TimeElapsed = timeElapsed;
    }
}