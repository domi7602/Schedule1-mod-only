using System.Collections.Generic;
using Minimap;
using Xunit;

namespace Minimap.Tests;

/// <summary>
/// Executable regression for the waypoint JSON persistence defect: the DTO carried
/// public fields only and was (de)serialized with default System.Text.Json options,
/// so every row was written as "{}" and read back empty — silently losing all
/// names, coordinates and colors. These tests exercise the Unity-free serializer
/// policy that production links into the mod.
/// </summary>
public sealed class WaypointSerializationTests
{
    private static List<WaypointFile> Sample() => new()
    {
        new WaypointFile { Name = "Base", X = 12.5f, Y = -3.25f, Z = 48.75f, ColorHex = "#E14BFF" },
        new WaypointFile { Name = "Docks", X = -100f, Y = 0f, Z = 200f, ColorHex = "#33AAFF" },
    };

    [Fact]
    public void Serialize_EmitsEveryFieldPerRow()
    {
        string json = MinimapJson.SerializeWaypoints(Sample());

        // The defect wrote an array of empty objects ("[{},{}]").
        Assert.Contains("\"Name\"", json);
        Assert.Contains("\"Base\"", json);
        Assert.Contains("\"Docks\"", json);
        Assert.Contains("#E14BFF", json);
        Assert.Contains("#33AAFF", json);
    }

    [Fact]
    public void RoundTrip_PreservesNamesCoordinatesAndColors()
    {
        var original = Sample();

        string json = MinimapJson.SerializeWaypoints(original);
        var restored = MinimapJson.DeserializeWaypoints(json);

        Assert.NotNull(restored);
        Assert.Equal(original.Count, restored!.Count);
        for (int i = 0; i < original.Count; i++)
        {
            Assert.Equal(original[i].Name, restored[i].Name);
            Assert.Equal(original[i].X, restored[i].X, 3);
            Assert.Equal(original[i].Y, restored[i].Y, 3);
            Assert.Equal(original[i].Z, restored[i].Z, 3);
            Assert.Equal(original[i].ColorHex, restored[i].ColorHex);
        }
    }

    [Fact]
    public void Deserialize_ReadsUsefulPascalCaseJson()
    {
        // A waypoint file that actually carries data (the format the fix writes).
        const string json =
            "[{\"Name\":\"Quarry\",\"X\":10.5,\"Y\":-2.0,\"Z\":30.25,\"ColorHex\":\"#FFAA00\"}]";

        var rows = MinimapJson.DeserializeWaypoints(json);

        Assert.NotNull(rows);
        Assert.Single(rows!);
        Assert.Equal("Quarry", rows![0].Name);
        Assert.Equal(10.5f, rows[0].X, 3);
        Assert.Equal(-2.0f, rows[0].Y, 3);
        Assert.Equal(30.25f, rows[0].Z, 3);
        Assert.Equal("#FFAA00", rows[0].ColorHex);
    }

    [Fact]
    public void Deserialize_ReadsLegacyCamelCaseJson()
    {
        // Robustness for hand-edited / older lowercase-key files.
        const string json =
            "[{\"name\":\"Quarry\",\"x\":10.5,\"y\":-2.0,\"z\":30.25,\"colorHex\":\"#FFAA00\"}]";

        var rows = MinimapJson.DeserializeWaypoints(json);

        Assert.NotNull(rows);
        Assert.Single(rows!);
        Assert.Equal("Quarry", rows![0].Name);
        Assert.Equal(10.5f, rows[0].X, 3);
        Assert.Equal(-2.0f, rows[0].Y, 3);
        Assert.Equal(30.25f, rows[0].Z, 3);
        Assert.Equal("#FFAA00", rows[0].ColorHex);
    }

    [Fact]
    public void Deserialize_LegacyEmptyObjectArray_YieldsNameLessRowsWithoutThrowing()
    {
        // Files written by the defect were "[{},{}]"; must parse, not throw, and the
        // loader skips the name-less entries.
        var rows = MinimapJson.DeserializeWaypoints("[{},{}]");

        Assert.NotNull(rows);
        Assert.Equal(2, rows!.Count);
        Assert.All(rows, r => Assert.True(string.IsNullOrWhiteSpace(r.Name)));
    }
}
