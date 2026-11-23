using System;
using System.Collections.Generic;
using Il2CppScheduleOne.PlayerScripts;
using MelonLoader;
using S1API.Console;
using UnityEngine;

namespace MoonRocks;

/// <summary>
/// Developer console commands for MoonRocks ('moon help', 'moon pad', 'moon launch', 'moon tp', 'moon earth', 'moon status').
/// </summary>
public sealed class MoonConsoleCommand : BaseConsoleCommand
{
    public override string CommandWord => "moon";
    public override string CommandDescription => "MoonRocks: Launchpad, live rocket flight and lunar terrain controls. 'moon help'.";
    public override string ExampleUsage => "moon pad";

    public override void ExecuteCommand(List<string> args)
    {
        MelonLogger.Msg("[MoonRocks] " + ExecuteArgs(args));
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
            case "pad":
                if (mod.Flight.IsInFlight) return "Waehrend des Fluges nicht moeglich.";
                if (PlayerMovement.InstanceExists)
                    PlayerMovement.Instance.Teleport(mod.Pad.EarthArrivalPoint, Quaternion.Euler(0f, -90f, 0f), true);
                return $"Zur Startrampe teleportiert ({mod.Pad.EarthPadCenter.x:F1}, {mod.Pad.EarthPadCenter.y:F1}, {mod.Pad.EarthPadCenter.z:F1}).";
            case "launch":
                return mod.Flight.StartLaunchToMoon()
                    ? "Raketenstart zum Mond gestartet!"
                    : "Start aktuell nicht moeglich (evtl. bereits im Flug).";
            case "return":
                return mod.Flight.StartReturnToEarth()
                    ? "Rueckflug zur Erde gestartet!"
                    : "Rueckflug aktuell nicht moeglich.";
            case "tp":
                mod.Flight.TeleportDirectToMoon();
                return "Direkt auf die Mond-Landing-Base teleportiert.";
            case "earth":
                mod.Flight.TeleportDirectToEarth();
                return "Direkt zurueck auf die Erde teleportiert.";
            case "setpad":
                if (mod.Flight.IsInFlight || mod.Flight.Phase == FlightPhase.OnMoon)
                    return "Startrampe kann nur auf der Erde versetzt werden.";
                if (!PlayerMovement.InstanceExists) return "PlayerMovement nicht verfuegbar.";
                Vector3 p = PlayerMovement.Instance.transform.position + PlayerMovement.Instance.transform.forward * 10f;
                p.y = PlayerMovement.Instance.transform.position.y;
                mod.Pad.RepositionPad(p);
                return $"Startrampe vor dich versetzt ({p.x:F1}, {p.y:F1}, {p.z:F1}).";
            case "debug":
                mod.ToggleCoordPicker();
                return $"Koordinaten-Picker (Mausrad-Klick) ist jetzt: {(mod.CoordPickerEnabled ? "AN" : "AUS")}";
            case "status":
                return $"Phase={mod.Flight.Phase}, Gravity={PlayerMovement.GravityMultiplier:F2}, EarthPad={mod.Pad.EarthPadCenter}, MoonPad={mod.Moon.LandingPadCenter}";
            default:
                return HelpText;
        }
    }

    private const string HelpText =
        "Befehle: moon pad | moon launch | moon return | moon tp | moon earth | moon setpad | moon debug | moon status";
}
