using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using S1Mods.Shared;
using UnityEngine;

namespace Minimap;

/// <summary>
/// Minimal JSON (de)serialization helpers for the waypoint file. Kept separate so
/// MinimapWaypoints stays free of System.Text.Json details.
/// </summary>
public static class MinimapJson
{
    public static string SerializeWaypoints(List<MinimapWaypoints.WaypointFile> rows)
        => JsonSerializer.Serialize(rows, new JsonSerializerOptions { WriteIndented = true });

    public static List<MinimapWaypoints.WaypointFile>? DeserializeWaypoints(string json)
        => JsonSerializer.Deserialize<List<MinimapWaypoints.WaypointFile>>(json);
}

/// <summary>
/// M4 Static/World Markers: user-set waypoints, persisted per save slot via SafeStorage
/// (atomic write + .bak rotation, slot_{n} isolation with the mandatory -1 guard).
/// Waypoints render as diamond blips that are ALWAYS clamped to the minimap edge,
/// regardless of MaxEntityRange — their whole purpose is long-distance navigation.
/// </summary>
public sealed class MinimapWaypoints
{
    public sealed class Waypoint
    {
        public string Name = "";
        public float X;
        public float Y;
        public float Z;
        public string ColorHex = "#E14BFF"; // Heavenly violet default

        public Vector3 ToVector() => new Vector3(X, Y, Z);
    }

    private readonly ModLogger _log;
    private readonly List<Waypoint> _waypoints = new();
    private string _lastKnownSlot = "default";
    private bool _dirty;

    public MinimapWaypoints(ModLogger log) { _log = log; }

    public int Count => _waypoints.Count;
    public IReadOnlyList<Waypoint> All => _waypoints;

    // ────────────────────────────────────────────────────────────
    // CRUD
    // ────────────────────────────────────────────────────────────

    public bool Add(string name, Vector3 pos, string colorHex)
    {
        if (string.IsNullOrWhiteSpace(name)) return false;

        // Normalize color once at input (culture-safe hex parse).
        if (!ColorUtility.TryParseHtmlString(colorHex, out _)) colorHex = "#E14BFF";

        // Replace same-name waypoint instead of duplicating (idempotent UX).
        for (int i = 0; i < _waypoints.Count; i++)
        {
            if (string.Equals(_waypoints[i].Name, name, StringComparison.OrdinalIgnoreCase))
            {
                _waypoints[i].X = pos.x;
                _waypoints[i].Y = pos.y;
                _waypoints[i].Z = pos.z;
                _waypoints[i].ColorHex = colorHex;
                _dirty = true;
                return true;
            }
        }

        if (_waypoints.Count >= MaxWaypoints)
        {
            _log.Warn($"Waypoint limit ({MaxWaypoints}) reached — delete one first.");
            return false;
        }

        _waypoints.Add(new Waypoint
        {
            Name = name.Trim(),
            X = pos.x,
            Y = pos.y,
            Z = pos.z,
            ColorHex = colorHex
        });
        _dirty = true;
        return true;
    }

    public bool Remove(string name)
    {
        for (int i = 0; i < _waypoints.Count; i++)
        {
            if (string.Equals(_waypoints[i].Name, name, StringComparison.OrdinalIgnoreCase))
            {
                _waypoints.RemoveAt(i);
                _dirty = true;
                return true;
            }
        }
        return false;
    }

    public void Clear()
    {
        if (_waypoints.Count == 0) return;
        _waypoints.Clear();
        _dirty = true;
    }

    public const int MaxWaypoints = 16;

    // ────────────────────────────────────────────────────────────
    // Persistence — SafeStorage atomic + slot isolation (Rule 1/2)
    // ────────────────────────────────────────────────────────────

        /// <summary>Public for MinimapJson access; not part of the runtime API.</summary>
        public sealed class WaypointFile
    {
        public string Name = "";
        public float X;
        public float Y;
        public float Z;
        public string ColorHex = "";
    }

    private string GetFilePath()
    {
        string slot = ResolveSlotSuffix();
        return SafeStorage.GetUserDataPath("Minimap", $"waypoints_{slot}.json");
    }

    private string ResolveSlotSuffix()
    {
        try
        {
            var loadMgr = Il2CppScheduleOne.DevUtilities.PersistentSingleton<Il2CppScheduleOne.Persistence.LoadManager>.Instance;
            if (loadMgr != null && loadMgr.Pointer != IntPtr.Zero && !loadMgr.WasCollected)
            {
                var info = loadMgr.ActiveSaveInfo;
                if (info != null && info.Pointer != IntPtr.Zero && !info.WasCollected && info.SaveSlotNumber >= 0)
                {
                    string slot = $"slot_{info.SaveSlotNumber}";
                    if (!string.Equals(slot, _lastKnownSlot, StringComparison.Ordinal))
                    {
                        // Slot switch: flush pending writes to the OLD file first.
                        // Bug-Audit 2026-09-12: Save() calls GetFilePath() → ResolveSlotSuffix()
                        // again; without a guard this recurses forever and ends in
                        // StackOverflowException (uncatchable in .NET 6). Consume _dirty
                        // BEFORE the Save call so the recursive re-entry finds it false.
                        string oldSuffix = _lastKnownSlot;
                        _lastKnownSlot = slot;
                        if (_dirty)
                        {
                            bool wasDirty = _dirty;
                            _dirty = false;
                            try
                            {
                                string oldPath = SafeStorage.GetUserDataPath("Minimap", $"waypoints_{oldSuffix}.json");
                                FlushToPath(oldPath);
                            }
                            catch { /* fall through — flush best-effort */ }
                        }
                    }
                    return slot;
                }
            }
        }
        catch { }
        return _lastKnownSlot;
    }

    /// <summary>
    /// Serialises current waypoints and writes them to an explicit path. Bypasses
    /// GetFilePath() so callers can flush to a stale path during slot switches without
    /// re-entering ResolveSlotSuffix().
    /// </summary>
    private void FlushToPath(string path)
    {
        try
        {
            var rows = new List<WaypointFile>(_waypoints.Count);
            for (int i = 0; i < _waypoints.Count; i++)
            {
                var w = _waypoints[i];
                rows.Add(new WaypointFile { Name = w.Name, X = w.X, Y = w.Y, Z = w.Z, ColorHex = w.ColorHex });
            }
            string json = MinimapJson.SerializeWaypoints(rows);
            SafeStorage.SaveTextAtomic(path, json, null);
            _log.Info($"Saved {rows.Count} waypoints to {path}.");
        }
        catch (Exception ex)
        {
            _log.Error($"Waypoint flush failed: {ex.Message}");
        }
    }

    public void Save()
    {
        try
        {
            if (string.Equals(_lastKnownSlot, "default", StringComparison.Ordinal) && !HasResolvedSlot())
            {
                // Never write waypoints_default.json from an unresolved session (Rule 2).
                return;
            }

            var rows = new List<WaypointFile>(_waypoints.Count);
            for (int i = 0; i < _waypoints.Count; i++)
            {
                var w = _waypoints[i];
                rows.Add(new WaypointFile
                {
                    Name = w.Name,
                    X = w.X,
                    Y = w.Y,
                    Z = w.Z,
                    ColorHex = w.ColorHex
                });
            }

            string json = MinimapJson.SerializeWaypoints(rows);
            SafeStorage.SaveTextAtomic(GetFilePath(), json, null);
            _dirty = false;
            _log.Info($"Saved {rows.Count} waypoints.");
        }
        catch (Exception ex)
        {
            _log.Error($"Waypoint save failed: {ex.Message}");
        }
    }

    private static bool HasResolvedSlot()
    {
        try
        {
            var loadMgr = Il2CppScheduleOne.DevUtilities.PersistentSingleton<Il2CppScheduleOne.Persistence.LoadManager>.Instance;
            if (loadMgr != null && loadMgr.Pointer != IntPtr.Zero && !loadMgr.WasCollected)
            {
                var info = loadMgr.ActiveSaveInfo;
                return info != null && info.Pointer != IntPtr.Zero && !info.WasCollected && info.SaveSlotNumber >= 0;
            }
        }
        catch { }
        return false;
    }

    public void Load()
    {
        try
        {
            _waypoints.Clear();
            string path = GetFilePath();
            string json = SafeStorage.LoadTextSafe(path, "", null);
            if (string.IsNullOrWhiteSpace(json))
            {
                _log.Info("No waypoint file for this slot (fresh start).");
                return;
            }

            var rows = MinimapJson.DeserializeWaypoints(json);
            if (rows == null) return;

            for (int i = 0; i < rows.Count; i++)
            {
                var r = rows[i];
                if (r == null || string.IsNullOrWhiteSpace(r.Name)) continue;
                _waypoints.Add(new Waypoint
                {
                    Name = r.Name,
                    X = r.X,
                    Y = r.Y,
                    Z = r.Z,
                    ColorHex = string.IsNullOrEmpty(r.ColorHex) ? "#E14BFF" : r.ColorHex
                });
            }
            _dirty = false;
            _log.Info($"Loaded {_waypoints.Count} waypoints.");
        }
        catch (Exception ex)
        {
            _log.Error($"Waypoint load failed: {ex.Message}");
        }
    }

    /// <summary>Scene teardown: flush pending changes, keep loaded data for the same slot.</summary>
    public void FlushIfDirty() { if (_dirty) Save(); }
}
