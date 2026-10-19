using System;
using System.Collections.Generic;
using System.IO;
using S1Mods.Shared;
using UnityEngine;

namespace Minimap;

/// <summary>
/// M4 Static/World Markers: user-set waypoints, persisted per save slot via SafeStorage
/// (atomic write + .bak rotation, slot_{n} isolation with the mandatory -1 guard).
/// Waypoints render as diamond blips that are ALWAYS clamped to the minimap edge,
/// regardless of MaxEntityRange — their whole purpose is long-distance navigation.
///
/// All normalization and persistence policy lives in the Unity-free
/// <see cref="WaypointStore"/> (linked into the test project); this class only adapts it
/// to UnityEngine types and the save-slot/SafeStorage seams.
/// </summary>
public sealed class MinimapWaypoints
{
    public sealed class Waypoint
    {
        public string Name = "";
        public float X;
        public float Y;
        public float Z;
        public string ColorHex = WaypointStore.DefaultColorHex;

        public Vector3 ToVector() => new Vector3(X, Y, Z);
    }

    public const int MaxWaypoints = WaypointStore.MaxWaypoints;

    private readonly ModLogger _log;
    private readonly WaypointStore _store;

    // Cached UnityEngine projection of the store rows, rebuilt only when the store mutates.
    private List<Waypoint> _projection = new();
    private int _projectionRevision = -1;

    public MinimapWaypoints(ModLogger log)
    {
        _log = log;
        _store = new WaypointStore(
            activeSlot: ResolveSlotToken,
            pathForSlot: slot => SafeStorage.GetUserDataPath("Minimap", $"waypoints_{slot}.json"),
            writeAtomic: (path, json) => SafeStorage.SaveTextAtomic(path, json, null),
            fileExists: FileExistsStrict,
            readText: ReadTextOrNull,
            logInfo: message => _log.Info(message),
            logWarn: message => _log.Warn(message),
            logError: message => _log.Error(message));
    }

    public int Count => _store.Count;

    /// <summary>True when the last load found an unreadable/invalid file: saving is paused until Clear().</summary>
    public bool IsLoadFaulted => _store.IsFaulted;

    public IReadOnlyList<Waypoint> All
    {
        get
        {
            EnsureProjection();
            return _projection;
        }
    }

    // ────────────────────────────────────────────────────────────
    // CRUD
    // ────────────────────────────────────────────────────────────

    public bool Add(string name, Vector3 pos, string colorHex)
        => _store.Add(name, pos.x, pos.y, pos.z, colorHex);

    public bool Remove(string name) => _store.Remove(name);

    /// <summary>Removes every waypoint; also the explicit recover action after a load fault.</summary>
    public bool TryClear() => _store.TryClear();

    public void Clear() => TryClear();

    // ────────────────────────────────────────────────────────────
    // Persistence — SafeStorage atomic + slot isolation (Rule 1/2)
    // ────────────────────────────────────────────────────────────

    public bool TrySave() => _store.Save();

    public void Save() => TrySave();

    public void Load() => _store.Load();

    /// <summary>Scene teardown: flush pending changes, keep loaded data for the same slot.</summary>
    public void FlushIfDirty() => _store.FlushIfDirty();

    // ────────────────────────────────────────────────────────────
    // Projection + seams
    // ────────────────────────────────────────────────────────────

    private void EnsureProjection()
    {
        if (_projectionRevision == _store.Revision) return;

        IReadOnlyList<WaypointFile> rows = _store.Rows;
        var list = new List<Waypoint>(rows.Count);
        for (int i = 0; i < rows.Count; i++)
        {
            WaypointFile row = rows[i];
            list.Add(new Waypoint { Name = row.Name, X = row.X, Y = row.Y, Z = row.Z, ColorHex = row.ColorHex });
        }
        _projection = list;
        _projectionRevision = _store.Revision;
    }

    /// <summary>
    /// Active save slot as a file-suffix token, or null when no save is loaded. The
    /// <c>&gt;= 0</c> guard prevents slot_-1 files (bug audit 2026-09-12).
    /// </summary>
    private static string? ResolveSlotToken()
    {
        try
        {
            int slotNumber = SaveSlots.GetActiveSlotNumber();
            return slotNumber >= 0 ? $"slot_{slotNumber}" : null;
        }
        catch
        {
            return null;
        }
    }

    private static bool FileExistsStrict(string path)
    {
        try
        {
            if ((File.GetAttributes(path) & FileAttributes.Directory) != 0)
                throw new IOException("A waypoint file path points to a directory.");
            return true;
        }
        catch (FileNotFoundException) { return false; }
        catch (DirectoryNotFoundException) { return false; }
    }

    private static string? ReadTextOrNull(string path)
    {
        try
        {
            if (new FileInfo(path).Length > WaypointStore.MaxJsonLength) return null;
            return File.ReadAllText(path);
        }
        catch { return null; }
    }
}
