// See https://aka.ms/new-console-template for more information

using SharpToolKit.Extensions;
using SharpToolKit.Timing;

namespace SharpToolKit.Sample;

internal static class Program
{
    private static void Main(string[] args)
    {
        using (Profiler.RunNew("RangeIterator", OnOperationCompleted))
        {
            TimeUntil until = 1.5;
            while (!until)
            {
            }

            foreach (var i in 5..10)
            {
            }
        }
    }

    private static void OnOperationCompleted(OperationResult op)
    {
        Console.WriteLine(
                $"Operation '{op.OperationName}' started at {op.StartedAt:G} completed at {op.EndedAt:G} in {op.TimeElapsed.TotalMilliseconds}ms");
    }
}