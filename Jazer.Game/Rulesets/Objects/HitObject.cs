namespace Jazer.Game.Rulesets.Objects;

public class HitObject
{
    public double StartTime { get; set; }

    public virtual double Duration => 0;

    public double EndTime => StartTime + Duration;
}
