using System.Reflection;
using Xunit;
namespace PoliceTestMode.Tests;
public class VehicleOccupancyTests
{
    private static string Read(Func<int> count, Func<int, bool?> seat)
    {
        var method = typeof(SessionPolicy).GetMethod("ReadVehicleOccupancy");
        Assert.NotNull(method);
        return method.Invoke(null, new object[] { count, seat })!.ToString()!;
    }

    [Fact]
    public void Empty_or_NPC_only_seats_are_clear() =>
        Assert.Equal("Clear", Read(() => 4, _ => false));

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(3)]
    public void Any_player_seat_is_protected(int playerSeat) =>
        Assert.Equal("Player", Read(() => 4, i => i == playerSeat));

    [Fact]
    public void Unknown_seat_is_protected() =>
        Assert.Equal("Unknown", Read(() => 4, i => i == 2 ? null : false));

    [Fact]
    public void Throwing_seat_is_protected() =>
        Assert.Equal("Unknown", Read(() => 4, _ => throw new InvalidOperationException()));

    [Fact]
    public void Throwing_registry_is_protected() =>
        Assert.Equal("Unknown", Read(() => throw new InvalidOperationException(), _ => false));

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(513)]
    public void Missing_empty_or_oversized_registry_is_not_proof_of_vacancy(int count) =>
        Assert.Equal("Unknown", Read(() => count, _ => false));
}
