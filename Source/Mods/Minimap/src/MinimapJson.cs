using System.Collections.Generic;
using System.Text.Json;

namespace Minimap;

/// <summary>
/// Minimal JSON (de)serialization helpers for the waypoint file. Kept separate so
/// MinimapWaypoints stays free of System.Text.Json details.
/// </summary>
public static class MinimapJson
{
    // DTO members are properties (not fields) so the default System.Text.Json contract
    // serializes them; a field-only DTO was written as "{}" and lost all data.
    private static readonly JsonSerializerOptions WriteOptions = new() { WriteIndented = true };

    // Case-insensitive reads keep older / hand-edited files with lowercase keys readable.
    private static readonly JsonSerializerOptions ReadOptions = new() { PropertyNameCaseInsensitive = true };

    public static string SerializeWaypoints(List<WaypointFile> rows)
        => JsonSerializer.Serialize(rows, WriteOptions);

    public static List<WaypointFile>? DeserializeWaypoints(string json)
        => JsonSerializer.Deserialize<List<WaypointFile>>(json, ReadOptions);
}

/// <summary>
/// Persistence row for one waypoint. Internal plumbing between MinimapWaypoints and
/// MinimapJson; not part of the runtime API.
/// </summary>
public sealed class WaypointFile
{
    public string Name { get; set; } = "";
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public string ColorHex { get; set; } = "";
}
