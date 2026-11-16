using System.Collections.Generic;
using Il2CppScheduleOne.PlayerScripts;
using MelonLoader;
using S1API.Console;
using UnityEngine;

namespace NorthIsland;

/// <summary>
/// Developer console commands for NorthIsland ('island help', 'island bridge', 'island ramp', 'island tp', 'island hideout', 'island rebuild', 'island scan', 'island raycast', 'island status').
/// </summary>
public sealed class IslandConsoleCommand : BaseConsoleCommand
{
    public override string CommandWord => "island";
    public override string CommandDescription => "NorthIsland: Bruecke, Rampe und Teleport zur Nordinsel. 'island help'.";
    public override string ExampleUsage => "island tp";

    public override void ExecuteCommand(List<string> args)
    {
        MelonLogger.Msg("[NorthIsland] " + ExecuteArgs(args));
    }

    internal static string ExecuteArgs(List<string> args)
    {
        if (args == null || args.Count == 0) return HelpText;
        string sub = (args[0] ?? string.Empty).Trim().ToLowerInvariant();

        var mod = Mod.Instance;
        if (mod == null || !mod.IsWorldReady)
            return "Welt noch nicht geladen. Bitte zuerst einen Spielstand laden.";

        switch (sub)
        {
            case "help":
                return HelpText;

            case "ramp":
                return TryTeleportPlayer(
                    BridgeBuilder.CityRampStart + new Vector3(0f, 1.5f, 3f),
                    0f,
                    "Zum Stadt-Auffahrtsbeginn (Overpass Ramp) teleportiert.");

            case "bridge":
                return TryTeleportPlayer(
                    BridgeBuilder.BridgeMidPoint,
                    180f,
                    "Auf die neue Overpass-Bruecke teleportiert.");

            case "tp":
            case "town":
                return TryTeleportPlayer(
                    TownBuilder.TownPlazaSpawn,
                    90f,
                    "Nach Northshore Town (Nordinsel Boulevard) teleportiert!");

            case "hideout":
            case "plateau":
                return TryTeleportPlayer(
                    TownBuilder.HideoutPlateauSpawn,
                    270f,
                    "Zum Northshore Hideout Warehouse (8m West-Plateau) teleportiert!");

            case "rebuild":
                mod.World.Build();
                return "Bruecke, Northshore Town & Absperrungen neu aufgebaut.";

            case "status":
                var w = mod.World;
                return $"NorthIsland aktiv: {(w.IsBuilt ? "JA" : "NEIN")} | Segmente={w.Bridge.ProceduralSegmentCount} | Fallback={w.Bridge.UsedProceduralFallback} | Gebaeude={w.Town.BuildingCount} | Laternen={w.Town.StreetlightCount} | Baeume={w.Town.TreeCount} | Barrieren={w.Barriers.DisabledObjectCount} | BoundaryCollider={w.Barriers.DisabledColliderCount} | Raycast={(mod.RaycastToolEnabled ? "AN" : "AUS")}";

            case "scan":
                return ScanNearbyColliders();

            case "raycast":
                string mode = args.Count > 1 ? (args[1] ?? string.Empty).Trim().ToLowerInvariant() : string.Empty;
                mod.RaycastToolEnabled = mode switch
                {
                    "on" or "an" or "1" or "true" => true,
                    "off" or "aus" or "0" or "false" => false,
                    _ => !mod.RaycastToolEnabled
                };
                return $"Debug-Raycast per Mittelklick ist jetzt {(mod.RaycastToolEnabled ? "AKTIV" : "AUS (mit Strg+Mittelklick weiter nutzbar)")}.";

            default:
                return $"Unbekannter Unterbefehl '{sub}'.\n{HelpText}";
        }
    }

    private static string ScanNearbyColliders()
    {
        if (!PlayerMovement.InstanceExists) return "Spieler nicht gefunden.";
        Vector3 pos = PlayerMovement.Instance.transform.position;
        int count = 0;
        foreach (var col in UnityEngine.Object.FindObjectsOfType<Collider>())
        {
            if (col == null || !col.enabled) continue;
            Bounds b = col.bounds;
            if (Vector3.Distance(b.ClosestPoint(pos), pos) > 15f) continue;
            MelonLogger.Msg($"[NorthIsland:Scan] '{col.gameObject.name}' trigger={col.isTrigger} center={b.center} size={b.size}");
            count++;
            if (count >= 25) break;
        }
        return $"{count} nahe Collider um {pos} ins MelonLoader-Log geschrieben.";
    }

    private static string TryTeleportPlayer(Vector3 pos, float yaw, string successMessage)
    {
        if (!PlayerMovement.InstanceExists)
            return "Spieler nicht gefunden.";

        if (PlayerMovement.Instance.CurrentVehicle != null)
            return "Bitte zuerst aus dem Fahrzeug aussteigen, bevor 'island ...' teleportiert wird.";

        PlayerMovement.Instance.Teleport(pos, Quaternion.Euler(0f, yaw, 0f), true);
        return successMessage;
    }

    private const string HelpText =
        "Befehle: island ramp (Overpass Ramp) | island bridge (Brueckenmitte) | island town (Northshore Boulevard) | island hideout (8m West-Plateau Warehouse) | island rebuild | island scan | island raycast [on|off] | island status";
}
