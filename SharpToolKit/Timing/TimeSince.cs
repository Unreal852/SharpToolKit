namespace SharpToolKit.Timing;

public readonly struct TimeSince : IEquatable<TimeSince>
{
    public static implicit operator double(TimeSince timeSince) => Timing.Time.Now - timeSince.Time;
    public static implicit operator TimeSince(double timeSince) => new(Timing.Time.Now - timeSince);
    public static                   bool operator ==(TimeSince left, TimeSince right) => left.Equals(right);
    public static                   bool operator !=(TimeSince left, TimeSince right) => !(left == right);

    public TimeSince(double time)
    {
        Time = time;
    }

    public double Time { get; }

    public          bool   Equals(TimeSince other) => Math.Abs(Time - other.Time) < 0.001;
    public override bool   Equals(object? obj)         => obj is TimeSince other && Equals(other);
    public override string ToString()                  => $"{(double)this}";
    public override int    GetHashCode()               => HashCode.Combine(Time);
}