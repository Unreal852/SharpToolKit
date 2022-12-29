using System.Diagnostics;
using SharpTools.Timing;
using Xunit.Abstractions;

namespace SharpTools.Tests;

public class RealTimeTest
{
    private readonly ITestOutputHelper _output;

    public RealTimeTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Theory(DisplayName = "Real Time Since"), InlineData(1.5)]
    public void Theory_RealTimeSince(double value)
    {
        // Act
        var sw = Stopwatch.StartNew();
        RealTimeSince since = 0;
        // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
        while (since < value)
        {
        }

        sw.Stop();

        // Assert
        _output.WriteLine($"Elapsed seconds {sw.Elapsed.TotalSeconds}");
        Assert.Equal(value, sw.Elapsed.TotalSeconds, 0.1);
    }

    [Theory(DisplayName = "Real Time Until"), InlineData(1.5)]
    public void Theory_RealTimeUntilTest(double value)
    {
        // Act
        var sw = Stopwatch.StartNew();
        RealTimeUntil until = 1.5;
        // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
        while (!until)
        {
        }

        sw.Stop();

        // Assert
        _output.WriteLine($"Elapsed seconds {sw.Elapsed.TotalSeconds}");
        Assert.Equal(value, sw.Elapsed.TotalSeconds, 0.1);
    }
}