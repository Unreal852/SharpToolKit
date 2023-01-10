using System.Diagnostics;
using SharpTools.Timing;
using Xunit.Abstractions;

namespace SharpTools.Tests;

public class TimeTest
{
    private readonly ITestOutputHelper _output;

    public TimeTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Theory(DisplayName = "Time Since"), InlineData(1.5)]
    public void Theory_TimeSince(double seconds)
    {
        // Act
        var startTime = Stopwatch.GetTimestamp();
        
        TimeSince since = 0;
        // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
        while (since < seconds)
        {
        }

        var endTime = Stopwatch.GetElapsedTime(startTime);

        // Assert
        _output.WriteLine($"Elapsed seconds {endTime.TotalSeconds}");
        Assert.Equal(seconds, endTime.TotalSeconds, 0.1);
    }

    [Theory(DisplayName = "Time Until"), InlineData(1.5)]
    public void Theory_TimeUntilTest(double seconds)
    {
        // Act
        var startTime = Stopwatch.GetTimestamp();
        
        TimeUntil until = 1.5;
        // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
        while (!until)
        {
        }

        var endTime = Stopwatch.GetElapsedTime(startTime);

        // Assert
        _output.WriteLine($"Elapsed seconds {endTime.TotalSeconds}");
        Assert.Equal(seconds, endTime.TotalSeconds, 0.1);
    }
    
    
    [Theory(DisplayName = "Profiler"), InlineData(1.5)]
    public void Theory_ProfilerTest(double seconds)
    {
        // Act
        var startTime = Stopwatch.GetTimestamp();

        var profiler = Profiler.RunNew("Profiler Unit Test");
        Thread.Sleep(TimeSpan.FromSeconds(seconds));
        profiler.End();
        
        var endTime = Stopwatch.GetElapsedTime(startTime);

        // Assert
        _output.WriteLine($"Elapsed seconds {endTime.TotalSeconds}");
        _output.WriteLine($"Profiler Elapsed seconds {profiler.Result.TimeElapsed.TotalSeconds}");
        Assert.Equal(seconds, profiler.Result.TimeElapsed.TotalSeconds, 0.1);
    }
}