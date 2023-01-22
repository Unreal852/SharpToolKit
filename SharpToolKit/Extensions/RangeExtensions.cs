using SharpToolKit.Range;

namespace SharpToolKit.Extensions;

/// <summary>
/// Provide extensions for <see cref="Range"/>.
/// </summary>
public static class RangeExtensions
{
    /// <summary>
    /// Create an <see cref="IntEnumerator"/> for the given <see cref="Range"/>.
    /// </summary>
    /// <param name="range">The range to iterate on.</param>
    /// <returns>The <see cref="IntEnumerator"/> used to iterate over the given <see cref="Range"/></returns>
    public static IntEnumerator GetEnumerator(this System.Range range)
    {
        return new IntEnumerator(range);
    }
}