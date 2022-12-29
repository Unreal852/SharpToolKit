namespace SharpTools.Timing;

public class BackgroundTimer
{
    private          Task?                   _timerTask;
    private readonly Action?                 _callback;
    private readonly Func<Task>?             _asyncCallback;
    private readonly PeriodicTimer           _timer;
    private readonly CancellationTokenSource _cts = new();

    private BackgroundTimer(TimeSpan interval)
    {
        _timer = new PeriodicTimer(interval);
    }

    public BackgroundTimer(TimeSpan interval, Action callback) : this(interval)
    {
        _callback = callback;
    }

    public BackgroundTimer(TimeSpan interval, Func<Task> asyncCallback) : this(interval)
    {
        _asyncCallback = asyncCallback;
    }

    public void Start()
    {
        _timerTask = WorkerAsync();
    }

    public async Task StopAsync()
    {
        if (_timerTask == null)
            return;
        _cts.Cancel();
        await _timerTask;
        _cts.Dispose();
    }

    private async Task WorkerAsync()
    {
        try
        {
            while (await _timer.WaitForNextTickAsync(_cts.Token))
            {
                if (_asyncCallback != null)
                    await _asyncCallback();
                else
                    _callback?.Invoke();
            }
        }
        catch (OperationCanceledException)
        {
        }
    }
}