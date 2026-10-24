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

    public static string SerializeSearch(SearchFile file)
        => JsonSerializer.Serialize(file, WriteOptions);

    public static SearchFile? DeserializeSearch(string json)
        => JsonSerializer.Deserialize<SearchFile>(json, ReadOptions);
}

/// <summary>
/// Versioned search-store document. <see cref="Version"/> is written on every save and a file
/// with any other version is rejected (never silently migrated), so a future format change
/// cannot be mistaken for data.
/// </summary>
public sealed class SearchFile
{
    public int Version { get; set; }
    public List<SearchRow>? Records { get; set; } = new();
}

/// <summary>Persistence row for one world's search intent. Never carries a completed flag.</summary>
public sealed class SearchRow
{
    public string World { get; set; } = "";
    public string Target { get; set; } = "";
    public string AreaId { get; set; } = "";
    public int AreaVersion { get; set; }
    public bool Accepted { get; set; }
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
