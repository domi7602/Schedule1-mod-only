using System.Globalization;
using System.Text.RegularExpressions;
using Xunit;

namespace TaxiDriver.Tests;

public class DriverWaitingPositionTests
{
    [Fact]
    public void WaitingPosition_IsFourMetresSouthOfEntry_AwayFromNorthernExit()
    {
        string stand = DriverSafetySourceContractTests.ReadSource("TaxiStand.cs");
        var entry = ReadCoordinate(stand, "StandCoordinate");
        var waiting = ReadCoordinate(stand, "DriverWaitingCoordinate");
        // Recorded entry forward is +X; exit is north at z=86.0.
        Assert.Equal((-13f, 0f, 84.2f), entry);
        Assert.Equal(entry.X, waiting.X);
        Assert.Equal(entry.Y, waiting.Y);
        Assert.Equal(-4f, waiting.Z - entry.Z, 3);
    }

    [Fact]
    public void DriverSpawnScheduleAndReturn_UseWaitingPoint_NotVehicleEntry()
    {
        string npc = DriverSafetySourceContractTests.ReadSource("TaxiDriverNPC.cs");
        Assert.Contains(".WithSpawnPosition(TaxiStand.DriverWaitingCoordinate)", npc);
        Assert.Contains("plan.WalkTo(TaxiStand.DriverWaitingCoordinate, 0, warpIfSkipped: true)", npc);
        Assert.Contains("ours.Position = TaxiStand.DriverWaitingCoordinate;", npc);
        Assert.DoesNotContain("TaxiStand.StandCoordinate", npc);
    }

    [Fact]
    public void ReturnToStand_ReadsBackPositionBeforeReportingSuccess()
    {
        string source = DriverSafetySourceContractTests.ReadSource("TaxiDriverNPC.cs");
        string returning = source[source.IndexOf("internal static void ReturnToStand()", StringComparison.Ordinal)..];
        Assert.Contains("Vector3 actual = ours.Position;", returning);
        Assert.Contains("Vector3.Distance(actual, TaxiStand.DriverWaitingCoordinate) > 0.5f", returning);
        Assert.Contains("waiting-point warp not confirmed", returning);
        Assert.Contains("waiting point confirmed", returning);
    }

    private static (float X, float Y, float Z) ReadCoordinate(string source, string name)
    {
        Match match = Regex.Match(source, name + @" = new Vector3\(([-\d.]+)f, ([-\d.]+)f, ([-\d.]+)f\)");
        Assert.True(match.Success, $"Missing explicit coordinate: {name}");
        return (Parse(1), Parse(2), Parse(3));
        float Parse(int group) => float.Parse(match.Groups[group].Value, CultureInfo.InvariantCulture);
    }
}
