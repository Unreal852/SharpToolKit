using System.Diagnostics;

// ReSharper disable MemberCanBePrivate.Global

namespace SharpTools.Timing;

public sealed class Profiler : IDisposable
{
    public static Profiler RunNew(string? profilerName = null, Action<OperationResult>? endCallback = null)
    {
        var profiler = new Profiler(profilerName, endCallback);
        profiler.Run();
        return profiler;
    }

    private readonly string?                  _profilerName;
    private readonly Action<OperationResult>? _endCallback;
    private          DateTime                 _startedAt;
    private          long                     _startTimestamp;
    private          bool                     _isDisposed;

    public Profiler(string? profilerName = null, Action<OperationResult>? endCallback = null)
    {
        _profilerName = profilerName;
        _endCallback = endCallback;
    }

    public OperationResult Result { get; private set; }

    public void Run()
    {
        if (_isDisposed)
            throw new ObjectDisposedException(nameof(Profiler));
        _startedAt = DateTime.Now;
        _startTimestamp = Stopwatch.GetTimestamp();
    }

    public void End()
    {
        if (_isDisposed)
            throw new ObjectDisposedException(nameof(Profiler));
        var elapsedTime = Stopwatch.GetElapsedTime(_startTimestamp);
        Result = new OperationResult(_profilerName, _startedAt, elapsedTime);
        _endCallback?.Invoke(Result);
    }

    public void Dispose()
    {
        if (_isDisposed)
            return;
        End();
        _isDisposed = true;
    }
}