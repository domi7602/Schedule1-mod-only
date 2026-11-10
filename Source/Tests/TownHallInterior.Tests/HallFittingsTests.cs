using System;
using System.Collections.Generic;
using System.Linq;
using TownHallInterior;
using Xunit;

// Built-in earth-storey fittings: fit, collision flags and that the accepted portal and stair
// voids stay free. Pure geometry, no Unity.
public sealed class HallFittingsTests
{
    private static readonly IReadOnlyList<RoomBox> Fittings = HallFittings.Create();

    private static (double[] Lo, double[] Hi) Bounds(RoomBox b) => (
        new double[] { b.X - b.Width / 2.0, b.Y - b.Height / 2.0, b.Z - b.Depth / 2.0 },
        new double[] { b.X + b.Width / 2.0, b.Y + b.Height / 2.0, b.Z + b.Depth / 2.0 });

    private static bool Overlaps(RoomBox b, double[] lo, double[] hi)
    {
        var (blo, bhi) = Bounds(b);
        return Enumerable.Range(0, 3).All(i => Math.Min(bhi[i], hi[i]) - Math.Max(blo[i], lo[i]) > 1e-5);
    }

    [Fact]
    public void EveryFittingStaysInsideTheEarthStorey()
    {
        Assert.NotEmpty(Fittings);
        Assert.Equal(Fittings.Count, Fittings.Select(b => b.Name).Distinct().Count());
        foreach (var b in Fittings)
        {
            var (lo, hi) = Bounds(b);
            bool upper = b.Name.StartsWith("upper-");
            Assert.InRange(lo[0], upper ? 54.55 : 54.95, upper ? 63.15 : 62.45);
            Assert.InRange(hi[0], upper ? 54.55 : 54.95, upper ? 63.15 : 62.45);
            Assert.InRange(lo[2], upper ? 20.70 : 21.60, upper ? 41.30 : 40.40);
            Assert.InRange(hi[2], upper ? 20.70 : 21.60, upper ? 41.30 : 40.40);
            float fy = upper ? RoomPlan.Upper : PublicEntrance.FloorTop;
            Assert.InRange(lo[1], fy - 1e-4, upper ? 8.05 : 4.40);
            Assert.InRange(hi[1], fy, upper ? 8.05 : 4.40);
        }
    }

    [Fact]
    public void TheAcceptedPortalStaysClear()
    {
        // The walk-in opening must not be narrowed by furniture.
        double[] lo = { 54.18, PublicEntrance.FloorTop + .001, 30.04 };
        double[] hi = { 54.95, PublicEntrance.HeadLiningBottom, 31.96 };
        foreach (var b in Fittings)
            Assert.False(Overlaps(b, lo, hi), "portal: " + b.Name);
    }

    [Fact]
    public void TheStairVoidStaysClear()
    {
        double[] lo = { 60.7, PublicEntrance.FloorTop, 24.0 };
        double[] hi = { 62.1, 7.25, 29.6 };
        foreach (var b in Fittings)
            Assert.False(Overlaps(b, lo, hi), "stair void: " + b.Name);
    }

    [Fact]
    public void OnlyWalkableFittingsCarryCollision()
    {
        Assert.All(Fittings.Where(b => b.Name.StartsWith("wait-bench-")), b => Assert.True(b.Solid, b.Name));
        Assert.DoesNotContain(Fittings, b => b.Name.StartsWith("info-"));
        Assert.False(Fittings.Single(b => b.Name == "wait-runner").Solid);
        Assert.False(Fittings.Single(b => b.Name == "notice-frame").Solid);
    }

    [Fact]
    public void TheEntranceAisleHasNoCounterOrOtherSolidFitting()
    {
        // The removed info counter occupied x 57.2–58.1, z 29.6–32.4, opposite the door.
        double[] lo = { 55.2, PublicEntrance.FloorTop + .05, 29.4 };
        double[] hi = { 60.2, PublicEntrance.FloorTop + 1.7, 32.6 };
        foreach (var b in Fittings.Where(b => b.Solid))
            Assert.False(Overlaps(b, lo, hi), "walkway: " + b.Name);
    }



    [Fact]
    public void BenchRowRunsAlongTheSouthWallWithSeatHeight()
    {
        var benches = Fittings.Where(b => b.Name.StartsWith("wait-bench-seat-")).ToArray();
        Assert.Equal(3, benches.Length);
        foreach (var seat in benches)
        {
            var (lo, hi) = Bounds(seat);
            Assert.InRange(lo[2], 21.60, 22.0);
            Assert.InRange(hi[1] - PublicEntrance.FloorTop, 0.40, 0.48);
        }
    }
}
