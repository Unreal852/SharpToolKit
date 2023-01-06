// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using SharpTools.Extensions;
using SharpTools.Timing;

namespace SharpTools.Sample;

internal static class Program
{
    private static void Main(string[] args)
    {
        var start = Stopwatch.GetTimestamp();
        Thread.Sleep(1000);
        var end = Stopwatch.GetElapsedTime(start);
        Console.WriteLine($"Took: {end.TotalSeconds}");

        // var start = Stopwatch.GetTimestamp();
        // TimeSince2 since = 0;
        // // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
        // while (since < 1.5)
        // {
        // }
        //
        // var end = Stopwatch.GetElapsedTime(start);
        //
        // Console.WriteLine($"Took: {end.TotalSeconds}");
        //
        //
        // foreach (var i in 5..10)
        // {
        //     Console.WriteLine(i);
        // }
    }
}