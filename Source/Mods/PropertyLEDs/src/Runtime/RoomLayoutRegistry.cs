using System;
using System.Collections.Generic;
using PropertyLEDs.Core;
using S1Mods.Shared;
using UnityEngine;

namespace PropertyLEDs;

/// <summary>
/// All coordinates come from an explicitly authored external catalog. No scene-derived guesses,
/// synthetic room rectangles or coordinate fallbacks. AnchorPath is relative to the owned root;
/// "." explicitly selects that root. Unverified geometry is only eligible for manual preview.
/// </summary>
internal sealed class RoomLayoutRegistry
{
    private readonly Dictionary<string, List<RoomLayout>> _rooms = new(StringComparer.Ordinal);
    internal static string CatalogPath => SafeStorage.GetUserDataPath("PropertyLEDs", "layouts.json");

    internal void Reload()
    {
        Clear();
        var catalog = SafeStorage.LoadSafe(CatalogPath, new LayoutCatalog(), Mod.Log);
        if (catalog.FormatVersion != LayoutCatalog.CurrentFormatVersion || catalog.Rooms == null || catalog.Rooms.Count > 1024)
        {
            Mod.Log.Warn("Layout catalog has an unsupported schema or invalid room list; file preserved, no layouts loaded.");
            return;
        }
        var seen = new Dictionary<(string Property, string Room), RoomLayout>();
        var ambiguous = new HashSet<(string Property, string Room)>();
        for (int i = 0; i < catalog.Rooms.Count; i++)
        {
            var room = catalog.Rooms[i];
            if (!ValidateGeometry(room, out string error))
            {
                Mod.Log.Warn($"Skipped catalog room {i}: {error}");
                continue;
            }
            var key = (room.PropertyId, room.RoomId);
            if (ambiguous.Contains(key)) continue;
            if (seen.ContainsKey(key))
            {
                seen.Remove(key);
                ambiguous.Add(key);
                Mod.Log.Warn($"Skipped ambiguous catalog room '{key.PropertyId}/{key.RoomId}'.");
                continue;
            }
            seen.Add(key, Copy(room));
        }
        int verified = 0;
        foreach (var pair in seen)
        {
            var room = pair.Value;
            if (!_rooms.TryGetValue(room.PropertyId, out var list))
                _rooms.Add(room.PropertyId, list = new List<RoomLayout>());
            list.Add(room);
            if (room.Verified) verified++;
        }
        foreach (var list in _rooms.Values)
            list.Sort((left, right) => StringComparer.Ordinal.Compare(left.RoomId, right.RoomId));
        Mod.Log.Info($"Layout catalog: {seen.Count} valid rooms, {verified} marked verified; unverified rooms require explicit preview.");
    }

    /// <summary>Catalog property ids only (diagnostics); rooms stay behind GetRooms copies.</summary>
    internal List<string> GetPropertyIds() => new List<string>(_rooms.Keys);

    internal IReadOnlyList<RoomLayout> GetRooms(string? propertyId)
    {
        if (propertyId == null || !_rooms.TryGetValue(propertyId, out var rooms))
            return Array.Empty<RoomLayout>();
        // DTOs are mutable: do not let UI/diagnostics promote a cached unverified layout.
        var result = new List<RoomLayout>(rooms.Count);
        for (int i = 0; i < rooms.Count; i++) result.Add(Copy(rooms[i]));
        return result.AsReadOnly();
    }

    internal bool Contains(string propertyId, string roomId)
    {
        if (!_rooms.TryGetValue(propertyId, out var rooms)) return false;
        for (int i = 0; i < rooms.Count; i++)
            if (StringComparer.Ordinal.Equals(rooms[i].RoomId, roomId)) return true;
        return false;
    }

    internal static bool ValidateGeometry(RoomLayout? room, out string error)
    {
        if (room == null) { error = "Null room."; return false; }
        // RoomLayout.Validate requires Verified. Preview checks only shape validity, without
        // ever flipping Verified (not even on a temporary clone).
        if (room.Verified && !room.Validate(out error)) return false;
        if (string.IsNullOrWhiteSpace(room.PropertyId) || string.IsNullOrWhiteSpace(room.RoomId) ||
            string.IsNullOrWhiteSpace(room.DisplayName) || !ValidAnchorPath(room.AnchorPath))
        { error = "Room identity/display name and safe property-relative anchor path are required."; return false; }
        if (!float.IsFinite(room.Width) || room.Width < 0.001f || room.Width > 1f ||
            room.Paths == null || room.Paths.Count == 0 || room.Paths.Count > 256)
        { error = "Invalid beam width or path count."; return false; }
        int points = 0;
        for (int i = 0; i < room.Paths.Count; i++)
        {
            var path = room.Paths[i];
            if (path == null) { error = "Null path."; return false; }
            if (!path.Validate(out error)) return false;
            points += path.Points.Count;
            if (points > 8192) { error = "Room exceeds the 8192-point mesh budget."; return false; }
        }
        error = "";
        return true;
    }

    private static bool ValidAnchorPath(string? path)
    {
        if (path == ".") return true;
        if (string.IsNullOrWhiteSpace(path) || path.Length > 2048 || path.StartsWith("/", StringComparison.Ordinal) ||
            path.Contains('\\') || path.Contains(':')) return false;
        var segments = path.Split('/');
        for (int i = 0; i < segments.Length; i++)
            if (string.IsNullOrWhiteSpace(segments[i]) || segments[i] == "." || segments[i] == "..") return false;
        return true;
    }

    internal static Transform? ResolveAnchor(PropertyEntry property, RoomLayout room)
    {
        if (!PropertyRegistry.UnityAlive(property.Anchor) || !ValidAnchorPath(room.AnchorPath)) return null;
        try
        {
            var anchor = room.AnchorPath == "." ? property.Anchor : property.Anchor.Find(room.AnchorPath);
            return PropertyRegistry.UnityAlive(anchor) ? anchor : null;
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"Anchor lookup failed for '{room.PropertyId}/{room.RoomId}': {ex.Message}");
            return null;
        }
    }

    internal static RoomLayout Copy(RoomLayout source)
    {
        var copy = new RoomLayout
        {
            PropertyId = source.PropertyId,
            RoomId = source.RoomId,
            DisplayName = source.DisplayName,
            AnchorPath = source.AnchorPath,
            Verified = source.Verified,
            Width = source.Width,
            Paths = new List<LEDPath>(source.Paths.Count)
        };
        for (int i = 0; i < source.Paths.Count; i++)
        {
            var path = source.Paths[i];
            var copiedPath = new LEDPath { Closed = path.Closed, Points = new List<LEDPoint>(path.Points.Count) };
            for (int j = 0; j < path.Points.Count; j++)
            {
                var point = path.Points[j];
                copiedPath.Points.Add(new LEDPoint { X = point.X, Y = point.Y, Z = point.Z });
            }
            copy.Paths.Add(copiedPath);
        }
        return copy;
    }

    internal void Clear() => _rooms.Clear();
}
