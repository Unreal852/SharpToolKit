using System.Diagnostics;

namespace SharpTools.Timing;

public sealed class Profiler : IDisposable
{
    public static Profiler RunNew(string? profilerName = null, string? message = null,
                               Action<Profiler>? endCallback = null)
    {
        var profiler = new Profiler(profilerName, message, endCallback);
        profiler.Run();
        return profiler;
    }

    private readonly string?           _profilerName;
    private readonly string?           _message;
    private readonly Action<Profiler>? _endCallback;
    private          long              _startTime;
    private          bool              _isDisposed;

    public Profiler(string? profilerName = null, string? message = null, Action<Profiler>? endCallback = null)
    {
        _profilerName = profilerName;
        _message = message;
        _endCallback = endCallback;
    }

    public TimeSpan Elapsed { get; private set; }

    public void Run()
    {
        if (_isDisposed)
            throw new ObjectDisposedException(nameof(Profiler));
        _startTime = Stopwatch.GetTimestamp();
    }

    public void End()
    {
        if (_isDisposed)
            throw new ObjectDisposedException(nameof(Profiler));
        Elapsed = Stopwatch.GetElapsedTime(_startTime);
        _endCallback?.Invoke(this);
    }

    public void Dispose()
    {
        if (_isDisposed)
            return;
        End();
        _isDisposed = true;
    }
}