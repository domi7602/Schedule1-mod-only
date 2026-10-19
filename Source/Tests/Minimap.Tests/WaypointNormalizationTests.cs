using Minimap;
using Xunit;

namespace Minimap.Tests;

/// <summary>
/// D10: input normalization must be identical on Add and Load. The old Add trimmed only
/// when inserting (so "Base " slipped past the duplicate check as a second waypoint),
/// accepted non-finite coordinates, stored raw rich-text markup in the label and kept
/// arbitrary hex spellings; Load bypassed every one of those checks.
/// </summary>
public sealed class WaypointNormalizationTests
{
    [Fact]
    public void Add_TrimsNameBeforeDuplicateLookup()
    {
        var io = new FakeWaypointIo();
        var store = WaypointTestData.Store(io, () => "slot_1");

        Assert.True(store.Add("Base", 1, 2, 3, "#E14BFF"));
        Assert.True(store.Add("Base ", 4, 5, 6, "#33AAFF"));

        Assert.Equal(1, store.Count);
        Assert.Equal("Base", store.Rows[0].Name);
        Assert.Equal(4f, store.Rows[0].X, 3);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Add_RejectsBlankName(string? name)
    {
        var io = new FakeWaypointIo();
        var store = WaypointTestData.Store(io, () => "slot_1");
        Assert.False(store.Add(name, 1, 2, 3, "#E14BFF"));
        Assert.Equal(0, store.Count);
        Assert.True(store.Add("Base", 1, 2, 3, "#E14BFF")); // positive control: not rejecting everything
    }

    [Theory]
    [InlineData(float.NaN, 0f, 0f)]
    [InlineData(0f, float.PositiveInfinity, 0f)]
    [InlineData(0f, 0f, float.NegativeInfinity)]
    public void Add_RejectsNonFiniteCoordinates(float x, float y, float z)
    {
        var io = new FakeWaypointIo();
        var store = WaypointTestData.Store(io, () => "slot_1");
        Assert.False(store.Add("Bad", x, y, z, "#E14BFF"));
        Assert.Equal(0, store.Count);
        Assert.True(store.Add("Good", 1, 2, 3, "#E14BFF"));
    }

    [Theory]
    [InlineData("<color=red>Base</color>")]
    [InlineData("<b>Base</b>")]
    [InlineData("<size=40>Base")]
    public void Add_RejectsRichTextMarkupName(string name)
    {
        var io = new FakeWaypointIo();
        var store = WaypointTestData.Store(io, () => "slot_1");
        Assert.False(store.Add(name, 1, 2, 3, "#E14BFF"));
        Assert.Equal(0, store.Count);
        Assert.True(store.Add("Base", 1, 2, 3, "#E14BFF"));
    }

    [Fact]
    public void Add_CanonicalizesHexToUppercase()
    {
        var io = new FakeWaypointIo();
        var store = WaypointTestData.Store(io, () => "slot_1");
        Assert.True(store.Add("Base", 1, 2, 3, "#ff0000"));
        Assert.Equal("#FF0000", store.Rows[0].ColorHex);
    }

    [Fact]
    public void Add_ExpandsShortHex()
    {
        var io = new FakeWaypointIo();
        var store = WaypointTestData.Store(io, () => "slot_1");
        Assert.True(store.Add("Base", 1, 2, 3, "#0af"));
        Assert.Equal("#00AAFF", store.Rows[0].ColorHex);
    }

    [Theory]
    [InlineData("not-a-color")]
    [InlineData("#12345")]
    [InlineData("#GGGGGG")]
    [InlineData("")]
    [InlineData(null)]
    public void Add_InvalidHexFallsBackToDefault(string? hex)
    {
        var io = new FakeWaypointIo();
        var store = WaypointTestData.Store(io, () => "slot_1");
        Assert.True(store.Add("Base", 1, 2, 3, hex));
        Assert.Equal(WaypointStore.DefaultColorHex, store.Rows[0].ColorHex);
    }

    [Fact]
    public void Add_CapsUsableRowsAtMax()
    {
        var io = new FakeWaypointIo();
        var store = WaypointTestData.Store(io, () => "slot_1");
        for (int i = 0; i < WaypointStore.MaxWaypoints; i++)
            Assert.True(store.Add($"W{i}", i, 0, 0, "#E14BFF"));

        Assert.Equal(WaypointStore.MaxWaypoints, store.Count);
        Assert.False(store.Add("Overflow", 99, 0, 0, "#E14BFF"));
        Assert.Equal(WaypointStore.MaxWaypoints, store.Count);
    }

    [Fact]
    public void Load_TrimsCapsAndRejectsInvalidRows()
    {
        var io = new FakeWaypointIo();
        // Hand-edited/legacy file. A literal that overflows float reads back non-finite; the
        // production serializer never emits one (that is the point of the input validation).
        var sb = new System.Text.StringBuilder();
        sb.Append('[');
        sb.Append("{\"Name\":\"Base\",\"X\":1,\"Y\":2,\"Z\":3,\"ColorHex\":\"#ff0000\"},");       // canonicalized
        sb.Append("{\"Name\":\"  Docks  \",\"X\":4,\"Y\":5,\"Z\":6,\"ColorHex\":\"#33AAFF\"},"); // trimmed
        sb.Append("{\"Name\":\"Bad\",\"X\":1e40,\"Y\":0,\"Z\":0,\"ColorHex\":\"#E14BFF\"},");     // non-finite -> dropped
        sb.Append("{\"Name\":\"<b>Rich</b>\",\"X\":7,\"Y\":8,\"Z\":9,\"ColorHex\":\"#E14BFF\"},"); // markup -> dropped
        sb.Append("{\"Name\":\"\",\"X\":1,\"Y\":1,\"Z\":1,\"ColorHex\":\"#E14BFF\"}");            // blank -> dropped
        for (int i = 0; i < 20; i++)
            sb.Append($",{{\"Name\":\"W{i}\",\"X\":{i},\"Y\":0,\"Z\":0,\"ColorHex\":\"#E14BFF\"}}"); // overflow beyond the cap
        sb.Append(']');
        io.Files[WaypointTestData.Slot1Path] = sb.ToString();

        var store = WaypointTestData.Store(io, () => "slot_1");
        store.Load();

        Assert.Equal(WaypointStore.MaxWaypoints, store.Count);
        Assert.Equal("Base", store.Rows[0].Name);
        Assert.Equal("#FF0000", store.Rows[0].ColorHex);
        Assert.Equal("Docks", store.Rows[1].Name);
        Assert.DoesNotContain(store.Rows, r => r.Name == "Bad");
        Assert.DoesNotContain(store.Rows, r => r.Name == "Rich");
    }

    [Fact]
    public void Load_CollapsesDuplicateNamesDeterministically()
    {
        var io = new FakeWaypointIo();
        io.Files[WaypointTestData.Slot1Path] = WaypointTestData.Serialize(
            ("Base", 1, 1, 1, "#E14BFF"),
            ("Base ", 2, 2, 2, "#33AAFF"));

        var store = WaypointTestData.Store(io, () => "slot_1");
        store.Load();

        Assert.Single(store.Rows);
        Assert.Equal("Base", store.Rows[0].Name);
        Assert.Equal(2f, store.Rows[0].X, 3);
    }
}
