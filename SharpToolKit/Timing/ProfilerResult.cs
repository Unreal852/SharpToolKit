namespace SharpToolKit.Timing;

public readonly struct ProfilerResult
{
    public readonly string? OperationName;
    public readonly DateTime StartedAt;
    public readonly DateTime EndedAt;
    public readonly TimeSpan TimeElapsed;

    public ProfilerResult(string? operationName, DateTime startedAt, TimeSpan timeElapsed)
    {
        OperationName = operationName;
        StartedAt = startedAt;
        TimeElapsed = timeElapsed;
        EndedAt = StartedAt + TimeElapsed;
    }
}