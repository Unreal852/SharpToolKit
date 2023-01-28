using SharpToolKit.Extensions;
using SharpToolKit.Optional;
using SharpToolKit.Timing;

namespace SharpToolKit.Sample;

internal static class Program
{
    private static void Main(string[] args)
    {
        ReflectionSample.CompareSpeed(OnOperationCompleted);

        using (Profiler.RunNew("TimeUntil and Range iteration", OnOperationCompleted))
        {
            TimeUntil until = 0.5;
            while (!until)
            {
            }

            foreach (var i in 5..10)
            {
            }
        }

        var input = ReadLine("Write anything: ");
        var result = ExtractWordsFromString(input);
        result.Match(s
                        => Console.WriteLine($"The string '{input}' contains {s.Length} word(s)."),
                () => Console.WriteLine($"The string '{input}' doesn't contains any word."));
    }

    private static Option<string[]> ExtractWordsFromString(in string str)
    {
        var words = str.Split(' ');
        return words.Length == 1 ? Option.None<string[]>() : Option.Some(words);
    }

    private static void OnOperationCompleted(OperationResult op)
    {
        Console.WriteLine(
                $"Operation '{op.OperationName}' started at {op.StartedAt:G} completed at {op.EndedAt:G} in {op.TimeElapsed.TotalMilliseconds}ms");
    }

    private static string ReadLine(string? message = null)
    {
        if (!string.IsNullOrWhiteSpace(message))
            Console.Write(message);
        return Console.ReadLine() ?? string.Empty;
    }
}