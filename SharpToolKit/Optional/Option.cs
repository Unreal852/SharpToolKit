namespace SharpToolKit.Optional;

/// <summary>
/// Provides a set of functions for creating optional values.
/// Based on https://github.com/nlkl/Optional
/// </summary>
public static class Option
{
    /// <summary>
    /// Wraps an existing value in an Option&lt;T&gt; instance.
    /// </summary>
    /// <param name="value">The value to be wrapped.</param>
    /// <returns>An optional containing the specified value.</returns>
    public static Option<T> Some<T>(T value) => new(value, true);

    /// <summary>
    /// Creates an empty Option&lt;T&gt; instance.
    /// </summary>
    /// <returns>An empty optional.</returns>
    public static Option<T> None<T>() => new(default!, false);
}