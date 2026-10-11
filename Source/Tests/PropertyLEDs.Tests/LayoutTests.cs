using Xunit;
using PropertyLEDs.Core;
namespace PropertyLEDs.Tests;
public class LayoutTests
{
    [Fact]
    public void Catalog_defaults_to_empty_supported_version_without_shipped_coordinates()
    {
        var type = typeof(LayoutTests).Assembly.GetType("PropertyLEDs.Core.LayoutCatalog");
        Assert.NotNull(type);
        var catalog = Activator.CreateInstance(type!);
        Assert.Equal(1, type!.GetProperty("FormatVersion")!.GetValue(catalog));
        Assert.Empty((System.Collections.IEnumerable)type.GetProperty("Rooms")!.GetValue(catalog)!);
        var roomType = type.Assembly.GetType("PropertyLEDs.Core.RoomLayout");
        Assert.NotNull(roomType);
        var room = Activator.CreateInstance(roomType!);
        Assert.Equal(0.035f, roomType!.GetProperty("Width")!.GetValue(room));
        Assert.Equal(false, roomType.GetProperty("Verified")!.GetValue(room));
    }
    internal static RoomLayout Room() => new()
    {
        PropertyId = "p",
        RoomId = "r",
        DisplayName = "Measured test fixture",
        AnchorPath = "p/Room",
        Verified = true,
        Paths = new() { new() { Points = new() { new(0, 0, 0), new(1, 2, 3) } } }
    };

    [Fact]
    public void Room_validation_refuses_unverified_missing_ids_invalid_width_and_null_paths()
    {
        var method = typeof(RoomLayout).GetMethod("Validate");
        Assert.NotNull(method);
        bool Valid(RoomLayout room) => (bool)method!.Invoke(room, new object?[] { null })!;
        Assert.True(Valid(Room()));
        foreach (var mutate in new Action<RoomLayout>[]
        {
            r => r.Verified = false, r => r.PropertyId = null!, r => r.RoomId = " ",
            r => r.DisplayName = "", r => r.AnchorPath = null!, r => r.Width = float.NaN,
            r => r.Width = float.PositiveInfinity, r => r.Width = 0, r => r.Width = 2,
            r => r.Paths = null!, r => r.Paths.Clear(), r => r.Paths.Add(null!)
        })
        {
            var room = Room(); mutate(room); Assert.False(Valid(room));
        }
    }
    [Fact]
    public void Paths_validate_finite_3d_coordinates_distinct_vertices_and_bounded_counts()
    {
        var method = typeof(LEDPath).GetMethod("Validate");
        Assert.NotNull(method);
        bool Valid(LEDPath path) => (bool)method!.Invoke(path, new object?[] { null })!;
        var path = Room().Paths[0];
        Assert.True(Valid(path));
        path.Closed = true;
        Assert.False(Valid(path));
        path.Points.Add(new(3, 0, 1));
        Assert.True(Valid(path));
        foreach (var points in new List<LEDPoint>?[]
        {
            null, new(), new() { new(0,0,0) }, new() { null!, new(1,0,0) },
            new() { new(0,0,0), new(float.NaN,0,0) }, new() { new(0,0,0), new(float.PositiveInfinity,0,0) },
            new() { new(0,0,0), new(10001,0,0) }, new() { new(0,0,0), new(0,0,0) },
            Enumerable.Range(0,4097).Select(i => new LEDPoint(i,0,0)).ToList()
        }) Assert.False(Valid(new LEDPath { Points = points! }));
        var room = Room(); room.Paths[0].Points = null!;
        Assert.False(room.Validate(out _));
    }
    [Fact]
    public void Catalog_validation_rejects_unknown_versions_and_ambiguous_room_ids_without_mutation()
    {
        var method = typeof(LayoutCatalog).GetMethod("Validate");
        Assert.NotNull(method);
        bool Valid(LayoutCatalog c) => (bool)method!.Invoke(c, new object?[] { null })!;
        Assert.True(Valid(new()));
        var catalog = new LayoutCatalog { Rooms = new() { Room() } };
        Assert.True(Valid(catalog));
        catalog.Rooms.Add(Room()); Assert.False(Valid(catalog));
        catalog.Rooms[1].PropertyId = "q"; Assert.True(Valid(catalog));
        catalog.Rooms.Add(null!); Assert.False(Valid(catalog));
        catalog.FormatVersion = 999; catalog.Rooms = null!;
        var json = System.Text.Json.JsonSerializer.Serialize(catalog);
        Assert.False(Valid(catalog));
        Assert.Equal(json, System.Text.Json.JsonSerializer.Serialize(catalog));
    }
    // NEXT TEST
}
