// See https://aka.ms/new-console-template for more information

using SharpTools.Extensions;

namespace SharpTools.Sample;

internal static class Program
{
    private static void Main(string[] args)
    {
        foreach (var i in 5..10)
        {
            Console.WriteLine(i);
        }
    }
}