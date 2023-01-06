namespace SharpTools.Timing;

public readonly struct TimeUntil : IEquatable<TimeUntil>
{
    public static implicit operator bool(TimeUntil timeUntil)                             => Timing.Time.Now >= timeUntil.Time;
    public static implicit operator double(TimeUntil timeUntil)                           => timeUntil.Time - Timing.Time.Now;
    public static implicit operator TimeUntil(double timeUntil)                           => new(Timing.Time.Now + timeUntil);
    public static                   bool operator ==(TimeUntil left, TimeUntil right) => left.Equals(right);
    public static                   bool operator !=(TimeUntil left, TimeUntil right) => !(left == right);

    public TimeUntil(double time)
    {
        Time = time;
    }

    public double Time { get; }

    public          bool   Equals(TimeUntil other) => Math.Abs(Time - other.Time) < 0.001;
    public override bool   Equals(object? obj)         => obj is TimeUntil other && Equals(other);
    public override string ToString()                  => $"{(double)this}";
    public override int    GetHashCode()               => HashCode.Combine(Time);
}