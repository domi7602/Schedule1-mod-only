using System;
using System.Collections.Generic;
using Il2CppScheduleOne.PlayerScripts;
using S1Mods.Shared;
using UnityEngine;

namespace TaxiDriver;

/// <summary>
/// Custom checkpoints (Dominik, 2026-09-29: "Custom Checkpoints einrichten ...
/// wenn 5x Parking gelistet wird, weiß man am Ende nicht wo man rauskommt").
///
/// A hand-editable list of named waypoints in
/// <c>UserData/TaxiDriver/checkpoints.json</c> (SafeStorage: atomic write +
/// <c>.bak</c>, comment-tolerant JSON, trailing commas allowed):
/// <code>
/// {
///   "Checkpoints": [
///     { "Name": "Zuhause", "X": 172.9, "Y": 10.0, "Z": -72.5 }
///   ]
/// }
/// </code>
/// They appear at the TOP of the TaxiApp destination list with the "YOU" tag and
/// resolve through <c>taxi to &lt;name&gt;</c> like any catalog place (they win
/// name ties). Manage them in-game with <c>taxi wp add|remove|list</c> —
/// <c>add</c> captures the current player position, so no coordinates have to be
/// typed by hand.
/// </summary>
internal static class CustomCheckpoints
{
    /// <summary>One custom waypoint (public properties: System.Text.Json round-trip).</summary>
    public sealed class Checkpoint
    {
        public string Name { get; set; } = string.Empty;
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }

    /// <summary>The file shape (one list, room to grow).</summary>
    public sealed class CheckpointFile
    {
        public List<Checkpoint> Checkpoints { get; set; } = new();
    }

    private static CheckpointFile? _file;
    private static bool _loaded;

    /// <summary>The sidecar path (also logged on first load so it can be found).</summary>
    internal static string FilePath => SafeStorage.GetUserDataPath("TaxiDriver", "checkpoints.json");

    /// <summary>Loads once, then serves the cache. Creates an empty seed file on first run.</summary>
    internal static IReadOnlyList<Checkpoint> All
    {
        get
        {
            EnsureLoaded();
            return _file!.Checkpoints;
        }
    }

    private static void EnsureLoaded()
    {
        if (_loaded)
            return;

        _loaded = true;
        _file = SafeStorage.LoadSafe(FilePath, new CheckpointFile(), Mod.Log);

        // Seed the file on first run: the format is then visible in place instead
        // of having to be guessed from the README.
        if (_file.Checkpoints.Count == 0 && !System.IO.File.Exists(FilePath))
        {
            _file.Checkpoints.Add(new Checkpoint { Name = "EXAMPLE — rename me or run 'taxi wp add <name>'", X = 0f, Y = 0f, Z = 0f });
            SafeStorage.SaveAtomic(FilePath, _file, Mod.Log);
            _file.Checkpoints.Clear();
            Mod.Log.Info($"[wp] first run — seed written to {FilePath} (hand-edit it or use `taxi wp add <name>`).");
        }
        else
        {
            Mod.Log.Info($"[wp] {FilePath}: {_file.Checkpoints.Count} custom checkpoint(s) loaded.");
        }
    }

    /// <summary>Adds or replaces a checkpoint at <paramref name="position"/> and saves.</summary>
    internal static bool Add(string name, Vector3 position)
    {
        EnsureLoaded();
        string clean = (name ?? string.Empty).Trim();
        if (clean.Length == 0)
        {
            Mod.Log.Warn("[wp] `taxi wp add` needs a name: `taxi wp add <name>` (spaces allowed).");
            return false;
        }

        Checkpoint? existing = _file!.Checkpoints.Find(c => string.Equals(c.Name, clean, StringComparison.OrdinalIgnoreCase));
        if (existing != null)
        {
            existing.X = position.x;
            existing.Y = position.y;
            existing.Z = position.z;
            Mod.Log.Info($"[wp] checkpoint '{clean}' UPDATED to {SpikeCommands.Fmt(position)}.");
        }
        else
        {
            _file.Checkpoints.Add(new Checkpoint { Name = clean, X = position.x, Y = position.y, Z = position.z });
            Mod.Log.Info($"[wp] checkpoint '{clean}' ADDED at {SpikeCommands.Fmt(position)}.");
        }

        _file.Checkpoints.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));
        return Save();
    }

    /// <summary>Removes every checkpoint with that name and saves (false when none matched).</summary>
    internal static bool Remove(string name)
    {
        EnsureLoaded();
        string clean = (name ?? string.Empty).Trim();
        int removed = _file!.Checkpoints.RemoveAll(c => string.Equals(c.Name, clean, StringComparison.OrdinalIgnoreCase));
        if (removed == 0)
        {
            Mod.Log.Warn($"[wp] no custom checkpoint named '{clean}' (`taxi wp list` shows them).");
            return false;
        }

        Mod.Log.Info($"[wp] checkpoint '{clean}' removed ({removed}).");
        return Save();
    }

    /// <summary>Logs every custom checkpoint (name + position).</summary>
    internal static void List()
    {
        EnsureLoaded();
        Mod.Log.Info($"[wp] {_file!.Checkpoints.Count} custom checkpoint(s) in {FilePath}:");
        foreach (Checkpoint c in _file.Checkpoints)
            Mod.Log.Info($"[wp]   '{c.Name}' at ({Fmt(c.X)}, {Fmt(c.Y)}, {Fmt(c.Z)})");
    }

    private static bool Save()
    {
        bool ok = SafeStorage.SaveAtomic(FilePath, _file, Mod.Log);
        if (!ok)
            Mod.Log.Error($"[wp] saving {FilePath} failed — the change lives only in memory.");
        return ok;
    }

    /// <summary>
    /// Console front end: <c>taxi wp add &lt;name&gt;</c> (player position),
    /// <c>taxi wp remove &lt;name&gt;</c>, <c>taxi wp list</c>. Names may contain
    /// spaces — everything after the verb is the name.
    /// </summary>
    internal static void HandleCommand(List<string> args)
    {
        // args: ["wp", verb, name parts...]
        string verb = args.Count >= 2 ? args[1].ToLowerInvariant() : "list";
        string name = args.Count >= 3 ? string.Join(" ", args.GetRange(2, args.Count - 2)) : string.Empty;

        switch (verb)
        {
            case "add":
            case "set":
                Player? player = Player.Local;
                if (player == null || player.transform == null)
                {
                    Mod.Log.Error("[wp] Player.Local is null — `taxi wp add` needs to run in-game.");
                    return;
                }

                Add(name, player.transform.position);
                break;

            case "remove":
            case "del":
            case "delete":
                Remove(name);
                break;

            case "list":
            case "ls":
                List();
                break;

            default:
                Mod.Log.Warn($"[wp] unknown verb '{verb}' — use `taxi wp add <name>` | `taxi wp remove <name>` | `taxi wp list`.");
                break;
        }
    }

    private static string Fmt(float value) => value.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture);
}
