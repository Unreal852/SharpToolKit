﻿namespace SharpToolKit.Optional;

public readonly struct Option<T> : IEquatable<Option<T>>, IComparable<Option<T>>
{
    private readonly T _value;
    private readonly bool _hasValue;

    internal Option(T value, bool hasValue)
    {
        _value = value;
        _hasValue = hasValue;
    }

    internal T Value => _value;

    public bool Equals(Option<T> other)
    {
        return _hasValue switch
        {
            false when !other._hasValue => true,
            true when other._hasValue => EqualityComparer<T>.Default.Equals(_value, other._value),
            _ => false
        };
    }

    /// <summary>
    /// Generates a hash code for the current optional.
    /// </summary>
    /// <returns>A hash code for the current optional.</returns>
    public override int GetHashCode()
    {
        if (!_hasValue)
            return 0;
        return _value == null ? 1 : _value.GetHashCode();
    }

    /// <summary>
    /// Determines whether two optionals are equal.
    /// </summary>
    /// <param name="obj">The optional to compare with the current one.</param>
    /// <returns>A boolean indicating whether the optionals are equal.</returns>
    public override bool Equals(object? obj)
    {
        return obj is Option<T> option && Equals(option);
    }

    /// <summary>
    /// Compares the relative order of two optionals. An empty optional is
    /// ordered before a non-empty one.
    /// </summary>
    /// <param name="other">The optional to compare with the current one.</param>
    /// <returns>An integer indicating the relative order of the optionals being compared.</returns>
    public int CompareTo(Option<T> other)
    {
        return _hasValue switch
        {
            true when !other._hasValue => 1,
            false when other._hasValue => -1,
            _ => Comparer<T>.Default.Compare(_value, other._value)
        };
    }

    /// <summary>
    /// Returns a string that represents the current optional.
    /// </summary>
    /// <returns>A string that represents the current optional.</returns>
    public override string ToString()
    {
        if (!_hasValue)
            return "None";
        return _value == null ? "Some(null)" : $"Some({_value})";
    }

    /// <summary>
    /// Converts the current optional into an enumerable with one or zero elements.
    /// </summary>
    /// <returns>A corresponding enumerable.</returns>
    public IEnumerable<T> ToEnumerable()
    {
        if (_hasValue)
            yield return _value;
    }

    /// <summary>
    /// Returns an enumerator for the optional.
    /// </summary>
    /// <returns>The enumerator.</returns>
    public IEnumerator<T> GetEnumerator()
    {
        if (_hasValue)
            yield return _value;
    }

    /// <summary>
    /// Determines if the current optional contains a specified value.
    /// </summary>
    /// <param name="value">The value to locate.</param>
    /// <returns>A boolean indicating whether the value was found.</returns>
    public bool Contains(T value)
    {
        if (!_hasValue)
            return false;
        if (_value == null)
            return value == null;
        return _value.Equals(value);
    }

    /// <summary>
    /// Determines if the current optional contains a value 
    /// satisfying a specified predicate.
    /// </summary>
    /// <param name="predicate">The predicate.</param>
    /// <returns>A boolean indicating whether the predicate was satisfied.</returns>
    public bool Exists(Func<T, bool> predicate)
    {
        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate));
        return _hasValue && predicate(_value);
    }

    /// <summary>
    /// Returns the existing value if present, and otherwise an alternative value.
    /// </summary>
    /// <param name="alternative">The alternative value.</param>
    /// <returns>The existing or alternative value.</returns>
    public T ValueOr(T alternative)
    {
        return _hasValue ? _value : alternative;
    }

    /// <summary>
    /// Returns the existing value if present, and otherwise an alternative value.
    /// </summary>
    /// <param name="alternativeFactory">A factory function to create an alternative value.</param>
    /// <returns>The existing or alternative value.</returns>
    public T ValueOr(Func<T> alternativeFactory)
    {
        ArgumentNullException.ThrowIfNull(alternativeFactory);
        return _hasValue ? _value : alternativeFactory();
    }

    /// <summary>
    /// Uses an alternative value, if no existing value is present.
    /// </summary>
    /// <param name="alternative">The alternative value.</param>
    /// <returns>A new optional, containing either the existing or alternative value.</returns>
    public Option<T> Or(T alternative)
    {
        return _hasValue ? this : Option.Some(alternative);
    }

    /// <summary>
    /// Uses an alternative value, if no existing value is present.
    /// </summary>
    /// <param name="alternativeFactory">A factory function to create an alternative value.</param>
    /// <returns>A new optional, containing either the existing or alternative value.</returns>
    public Option<T> Or(Func<T> alternativeFactory)
    {
        ArgumentNullException.ThrowIfNull(alternativeFactory);
        return _hasValue ? this : Option.Some(alternativeFactory());
    }

    /// <summary>
    /// Uses an alternative optional, if no existing value is present.
    /// </summary>
    /// <param name="alternativeOption">The alternative optional.</param>
    /// <returns>The alternative optional, if no value is present, otherwise itself.</returns>
    public Option<T> Else(Option<T> alternativeOption)
    {
        return _hasValue ? this : alternativeOption;
    }

    /// <summary>
    /// Uses an alternative optional, if no existing value is present.
    /// </summary>
    /// <param name="alternativeOptionFactory">A factory function to create an alternative optional.</param>
    /// <returns>The alternative optional, if no value is present, otherwise itself.</returns>
    public Option<T> Else(Func<Option<T>> alternativeOptionFactory)
    {
        ArgumentNullException.ThrowIfNull(alternativeOptionFactory);
        return _hasValue ? this : alternativeOptionFactory();
    }

    /// <summary>
    /// Evaluates a specified function, based on whether a value is present or not.
    /// </summary>
    /// <param name="some">The function to evaluate if the value is present.</param>
    /// <param name="none">The function to evaluate if the value is missing.</param>
    /// <returns>The result of the evaluated function.</returns>
    public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none)
    {
        ArgumentNullException.ThrowIfNull(some);
        ArgumentNullException.ThrowIfNull(none);
        return _hasValue ? some(_value) : none();
    }

    /// <summary>
    /// Evaluates a specified action, based on whether a value is present or not.
    /// </summary>
    /// <param name="some">The action to evaluate if the value is present.</param>
    /// <param name="none">The action to evaluate if the value is missing.</param>
    public void Match(Action<T> some, Action none)
    {
        ArgumentNullException.ThrowIfNull(some);
        ArgumentNullException.ThrowIfNull(none);

        if (_hasValue)
            some(_value);
        else
            none();
    }

    /// <summary>
    /// Evaluates a specified action if a value is present.
    /// </summary>
    /// <param name="some">The action to evaluate if the value is present.</param>
    public void MatchSome(Action<T> some)
    {
        ArgumentNullException.ThrowIfNull(some);

        if (_hasValue)
            some(_value);
    }

    /// <summary>
    /// Evaluates a specified action if no value is present.
    /// </summary>
    /// <param name="none">The action to evaluate if the value is missing.</param>
    public void MatchNone(Action none)
    {
        ArgumentNullException.ThrowIfNull(none);

        if (!_hasValue)
        {
            none();
        }
    }

    /// <summary>
    /// Transforms the inner value in an optional.
    /// If the instance is empty, an empty optional is returned.
    /// </summary>
    /// <param name="mapping">The transformation function.</param>
    /// <returns>The transformed optional.</returns>
    public Option<TResult> Map<TResult>(Func<T, TResult> mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping);

        return Match(
            some: value => Option.Some(mapping(value)),
            none: Option.None<TResult>
        );
    }

    /// <summary>
    /// Transforms the inner value in an optional
    /// into another optional. The result is flattened, 
    /// and if either is empty, an empty optional is returned.
    /// </summary>
    /// <param name="mapping">The transformation function.</param>
    /// <returns>The transformed optional.</returns>
    public Option<TResult> FlatMap<TResult>(Func<T, Option<TResult>> mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping);

        return Match(
            some: mapping,
            none: Option.None<TResult>
        );
    }

    /// <summary>
    /// Empties an optional if a specified condition
    /// is not satisfied.
    /// </summary>
    /// <param name="condition">The condition.</param>
    /// <returns>The filtered optional.</returns>
    public Option<T> Filter(bool condition)
    {
        return _hasValue && !condition ? Option.None<T>() : this;
    }

    /// <summary>
    /// Empties an optional if a specified predicate
    /// is not satisfied.
    /// </summary>
    /// <param name="predicate">The predicate.</param>
    /// <returns>The filtered optional.</returns>
    public Option<T> Filter(Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return _hasValue && !predicate(_value) ? Option.None<T>() : this;
    }

    /// <summary>
    /// Empties an optional if the value is null.
    /// </summary>
    /// <returns>The filtered optional.</returns>
    public Option<T> NotNull()
    {
        return _hasValue && _value == null ? Option.None<T>() : this;
    }

    /// <summary>
    /// Determines whether two optionals are equal.
    /// </summary>
    /// <param name="left">The first optional to compare.</param>
    /// <param name="right">The second optional to compare.</param>
    /// <returns>A boolean indicating whether the optionals are equal.</returns>
    public static bool operator ==(Option<T> left, Option<T> right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two optionals are unequal.
    /// </summary>
    /// <param name="left">The first optional to compare.</param>
    /// <param name="right">The second optional to compare.</param>
    /// <returns>A boolean indicating whether the optionals are unequal.</returns>
    public static bool operator !=(Option<T> left, Option<T> right)
    {
        return !left.Equals(right);
    }

    /// <summary>
    /// Determines if an optional is less than another optional.
    /// </summary>
    /// <param name="left">The first optional to compare.</param>
    /// <param name="right">The second optional to compare.</param>
    /// <returns>A boolean indicating whether the left optional is less than the right optional.</returns>
    public static bool operator <(Option<T> left, Option<T> right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <summary>
    /// Determines if an optional is less than or equal to another optional.
    /// </summary>
    /// <param name="left">The first optional to compare.</param>
    /// <param name="right">The second optional to compare.</param>
    /// <returns>A boolean indicating whether the left optional is less than or equal the right optional.</returns>
    public static bool operator <=(Option<T> left, Option<T> right)
    {
        return left.CompareTo(right) <= 0;
    }

    /// <summary>
    /// Determines if an optional is greater than another optional.
    /// </summary>
    /// <param name="left">The first optional to compare.</param>
    /// <param name="right">The second optional to compare.</param>
    /// <returns>A boolean indicating whether the left optional is greater than the right optional.</returns>
    public static bool operator >(Option<T> left, Option<T> right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <summary>
    /// Determines if an optional is greater than or equal to another optional.
    /// </summary>
    /// <param name="left">The first optional to compare.</param>
    /// <param name="right">The second optional to compare.</param>
    /// <returns>A boolean indicating whether the left optional is greater than or equal the right optional.</returns>
    public static bool operator >=(Option<T> left, Option<T> right)
    {
        return left.CompareTo(right) >= 0;
    }
}