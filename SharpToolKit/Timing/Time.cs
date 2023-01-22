using System.Diagnostics;

namespace SharpToolKit.Timing;

public static class Time
{
    private static readonly Stopwatch Stopwatch = Stopwatch.StartNew();
    public static           double    Now => Stopwatch.Elapsed.TotalSeconds;
}