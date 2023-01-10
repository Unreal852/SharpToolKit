// See https://aka.ms/new-console-template for more information

using SharpTools.Extensions;
using SharpTools.Timing;

namespace SharpTools.Sample;

internal static class Program
{
    private static void Main(string[] args)
    {
        using (Profiler.RunNew("RangeIterator", OnOperationCompleted))
        {
            foreach (var i in 5..1000000)
            {
                
            }
        }
    }

    private static void OnOperationCompleted(OperationResult op)
    {
        Console.WriteLine(
                $"Operation '{op.OperationName}' started at {op.StartedAt:G} completed in {op.TimeElapsed.TotalMilliseconds}ms");
    }
}