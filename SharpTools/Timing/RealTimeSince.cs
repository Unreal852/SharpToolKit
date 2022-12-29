namespace SharpTools.Timing;

public readonly struct RealTimeSince : IEquatable<RealTimeSince>
{
    public static implicit operator double(RealTimeSince realTimeSince)                       => RealTime.Now - realTimeSince.Time;
    public static implicit operator RealTimeSince(double timeSince)                           => new(RealTime.Now - timeSince);
    public static                   bool operator ==(RealTimeSince left, RealTimeSince right) => left.Equals(right);
    public static                   bool operator !=(RealTimeSince left, RealTimeSince right) => !(left == right);

    public RealTimeSince(double time)
    {
        Time = time;
    }

    public double Time { get; }

    public          bool   Equals(RealTimeSince other) => Math.Abs(Time - other.Time) < 0.001;
    public override bool   Equals(object? obj)         => obj is RealTimeSince other && Equals(other);
    public override string ToString()                  => $"{(double)this}";
    public override int    GetHashCode()               => HashCode.Combine(Time);
}