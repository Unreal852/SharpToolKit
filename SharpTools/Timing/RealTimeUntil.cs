namespace SharpTools.Timing;

public readonly struct RealTimeUntil : IEquatable<RealTimeUntil>
{
    public static implicit operator bool(RealTimeUntil timeUntil)                             => RealTime.Now >= timeUntil.Time;
    public static implicit operator double(RealTimeUntil timeUntil)                           => timeUntil.Time - RealTime.Now;
    public static implicit operator RealTimeUntil(double timeUntil)                           => new(RealTime.Now + timeUntil);
    public static                   bool operator ==(RealTimeUntil left, RealTimeUntil right) => left.Equals(right);
    public static                   bool operator !=(RealTimeUntil left, RealTimeUntil right) => !(left == right);

    public RealTimeUntil(double time)
    {
        Time = time;
    }

    public double Time { get; }

    public          bool   Equals(RealTimeUntil other) => Math.Abs(Time - other.Time) < 0.001;
    public override bool   Equals(object? obj)         => obj is RealTimeUntil other && Equals(other);
    public override string ToString()                  => $"{(double)this}";
    public override int    GetHashCode()               => HashCode.Combine(Time);
}