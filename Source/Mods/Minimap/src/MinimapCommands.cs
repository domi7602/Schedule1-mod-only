using System;
using System.Collections.Generic;
using System.Globalization;
using MelonLoader;
using S1API.Console;
using S1Mods.Shared;
using UnityEngine;

namespace Minimap;

public sealed class MinimapConsoleCommand : BaseConsoleCommand
{
    public override string CommandWord => "minimap";

    public override string CommandDescription =>
        "Minimap & HUD: toggle, shape <circle|rounded|square>, rotation <follow|north>, zoom <val>, size <val>, day <header|footer|off>, pos <corner|reset>, blips <type> <on|off>, status, help";

    public override string ExampleUsage => "minimap shape circle";

    public override void ExecuteCommand(List<string> args)
    {
        try
        {
            var mod = MinimapMod.Instance;
            if (mod == null)
            {
                Print("Minimap mod instance is not running.");
                return;
            }

            if (args == null || args.Count == 0 || Is(args, 0, "status", "info", "s"))
            {
                mod.PrintStatusToConsole();
                return;
            }

            string sub = args[0].ToLowerInvariant();
            switch (sub)
            {
                case "toggle":
                case "t":
                    mod.ToggleMinimap();
                    Print($"<color=#60f080> Minimap is now {(mod.Config.MinimapVisible ? "VISIBLE" : "HIDDEN")}.");
                    return;

                case "shape":
                    if (args.Count >= 2)
                    {
                        string s = args[1].ToLowerInvariant();
                        if (s.StartsWith("circ") || s == "c" || s == "0")
                        {
                            mod.Config.Shape = MinimapShape.Circle;
                            Print("<color=#60f080> Minimap shape set to: CIRCLE (Radar).");
                        }
                        else if (s.StartsWith("round") || s == "r" || s == "1")
                        {
                            mod.Config.Shape = MinimapShape.RoundedSquare;
                            Print("<color=#60f080> Minimap shape set to: ROUNDED SQUARE (Tactical GPS).");
                        }
                        else if (s.StartsWith("sq") || s == "s" || s == "2")
                        {
                            mod.Config.Shape = MinimapShape.Square;
                            Print("<color=#60f080> Minimap shape set to: SQUARE.");
                        }
                        else
                        {
                            Print("<color=#f06060> Unknown shape. Usage: minimap shape <circle|rounded|square>");
                            return;
                        }
                        mod.SaveAndApply();
                    }
                    else
                    {
                        Print($"<color=#60f080> Current shape: {mod.Config.Shape}");
                    }
                    return;

                case "rotation":
                case "rot":
                    if (args.Count >= 2)
                    {
                        string r = args[1].ToLowerInvariant();
                        if (r.StartsWith("f") || r == "follow" || r == "player")
                        {
                            mod.Config.RotationMode = MinimapRotation.FollowPlayer;
                            Print("<color=#60f080> Rotation mode: FOLLOW PLAYER (Map rotates).");
                        }
                        else if (r.StartsWith("n") || r == "north")
                        {
                            mod.Config.RotationMode = MinimapRotation.NorthUp;
                            Print("<color=#60f080> Rotation mode: NORTH-UP (Fixed North).");
                        }
                        else
                        {
                            Print("<color=#f06060> Unknown rotation mode. Usage: minimap rot <follow|north>");
                            return;
                        }
                        mod.SaveAndApply();
                    }
                    else
                    {
                        Print($"<color=#60f080> Current rotation mode: {mod.Config.RotationMode}");
                    }
                    return;

                case "zoom":
                case "z":
                    if (args.Count >= 2 && float.TryParse(args[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float zVal))
                    {
                        mod.Config.ZoomLevel = Mathf.Clamp(zVal, 0.75f, 4.0f);
                        mod.SaveAndApply();
                        Print($"<color=#60f080> Zoom level set to: {mod.Config.ZoomLevel:F2}x");
                    }
                    else
                    {
                        Print($"<color=#60f080> Current zoom: {mod.Config.ZoomLevel:F2}x. Usage: minimap zoom <0.75..4.0>");
                    }
                    return;

                case "size":
                    if (args.Count >= 2 && float.TryParse(args[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float szVal))
                    {
                        mod.Config.MapSize = Mathf.Clamp(szVal, 140f, 350f);
                        mod.SaveAndApply();
                        Print($"<color=#60f080> Map size set to: {mod.Config.MapSize:F0}px");
                    }
                    else
                    {
                        Print($"<color=#60f080> Current size: {mod.Config.MapSize:F0}px. Usage: minimap size <140..350>");
                    }
                    return;

                case "day":
                case "daycounter":
                    if (args.Count >= 2)
                    {
                        string d = args[1].ToLowerInvariant();
                        if (d.StartsWith("head") || d == "top" || d == "on")
                        {
                            mod.Config.DayMode = DayCounterMode.Header;
                            Print("<color=#60f080> DayCounter set to: HEADER (Top bar).");
                        }
                        else if (d.StartsWith("foot") || d == "bottom")
                        {
                            mod.Config.DayMode = DayCounterMode.Footer;
                            Print("<color=#60f080> DayCounter set to: FOOTER (Bottom bar).");
                        }
                        else if (d == "off" || d == "hide" || d == "hidden")
                        {
                            mod.Config.DayMode = DayCounterMode.Hidden;
                            Print("<color=#60f080> DayCounter is now HIDDEN.");
                        }
                        else
                        {
                            Print("<color=#f06060> Unknown daycounter mode. Usage: minimap day <header|footer|off>");
                            return;
                        }
                        mod.SaveAndApply();
                    }
                    else
                    {
                        Print($"<color=#60f080> Current DayCounter mode: {mod.Config.DayMode}");
                    }
                    return;

                case "pos":
                case "position":
                case "anchor":
                    if (args.Count >= 2)
                    {
                        string pos = args[1].ToLowerInvariant();
                        if (pos == "reset")
                        {
                            mod.Config.HasCustomPosition = false;
                            mod.Config.AnchorPosition = MinimapAnchor.TopRight;
                            mod.SaveAndApply();
                            Print("<color=#60f080> Position reset to Top-Right default.");
                            return;
                        }
                        else if (pos == "topright" || pos == "tr")
                        {
                            mod.Config.HasCustomPosition = false;
                            mod.Config.AnchorPosition = MinimapAnchor.TopRight;
                        }
                        else if (pos == "topleft" || pos == "tl")
                        {
                            mod.Config.HasCustomPosition = false;
                            mod.Config.AnchorPosition = MinimapAnchor.TopLeft;
                        }
                        else if (pos == "bottomright" || pos == "br")
                        {
                            mod.Config.HasCustomPosition = false;
                            mod.Config.AnchorPosition = MinimapAnchor.BottomRight;
                        }
                        else if (pos == "bottomleft" || pos == "bl")
                        {
                            mod.Config.HasCustomPosition = false;
                            mod.Config.AnchorPosition = MinimapAnchor.BottomLeft;
                        }
                        else
                        {
                            Print("<color=#f06060> Unknown anchor. Usage: minimap pos <topright|topleft|bottomright|bottomleft|reset>");
                            return;
                        }
                        mod.SaveAndApply();
                        Print($"<color=#60f080> Anchor set to: {mod.Config.AnchorPosition}");
                    }
                    else
                    {
                        Print("<color=#60f080> Usage: minimap pos <topright|topleft|bottomright|bottomleft|reset>");
                    }
                    return;

                case "blips":
                    if (args.Count >= 3)
                    {
                        string bType = args[1].ToLowerInvariant();
                        bool bState = args[2].ToLowerInvariant() == "on" || args[2] == "1" || args[2] == "true";

                        if (bType.StartsWith("pol") || bType.StartsWith("cop")) mod.Config.ShowPoliceBlips = bState;
                        else if (bType.StartsWith("deal")) mod.Config.ShowActiveDeals = bState;
                        else if (bType.StartsWith("pot") || bType.StartsWith("sample") || bType.StartsWith("lead")) mod.Config.ShowPotentialCustomers = bState;
                        else if (bType.StartsWith("cust") || bType.StartsWith("known")) mod.Config.ShowKnownCustomers = bState;
                        else if (bType.StartsWith("prop")) mod.Config.ShowPropertyBlips = bState;
                        else if (bType.StartsWith("shop") || bType.StartsWith("market")) mod.Config.ShowShopBlips = bState;
                        else if (bType.StartsWith("quest") || bType.StartsWith("mission")) mod.Config.ShowQuestBlips = bState;
                        else if (bType.StartsWith("veh") || bType.StartsWith("car") || bType.StartsWith("skate")) mod.Config.ShowVehicleBlips = bState;
                        else if (bType.StartsWith("clamp")) mod.Config.EnableEdgeClamping = bState;

                        mod.SaveAndApply();
                        Print($"<color=#60f080> Blip filter '{bType}' set to: {(bState ? "ON" : "OFF")}");
                    }
                    else
                    {
                        Print("<color=#60f080> Usage: minimap blips <police|deals|potential|customers|properties|shops|quests|vehicles|clamp> <on|off>");
                    }
                    return;

                case "help":
                case "?":
                case "h":
                    PrintHelp();
                    return;

                default:
                    Print($"<color=#f06060> Unknown subcommand '{args[0]}'. Type 'minimap help' for a list of commands.");
                    break;
            }
        }
        catch (Exception ex)
        {
            Print($"<color=#f06060> Minimap command error: {ex.Message}");
        }
    }

    private static bool Is(List<string> args, int index, params string[] values)
    {
        if (args == null || index >= args.Count) return false;
        string v = args[index].ToLowerInvariant();
        foreach (var val in values)
        {
            if (v == val) return true;
        }
        return false;
    }

    private void PrintHelp()
    {
        Print("<color=#f0c040>=== Minimap & Unified HUD Commands ===</color>");
        Print("<color=#ffffff>minimap toggle</color> - Toggle Minimap HUD visibility");
        Print("<color=#ffffff>minimap shape <circle|rounded|square></color> - Change map shape");
        Print("<color=#ffffff>minimap rot <follow|north></color> - Rotate with player vs North-Up");
        Print("<color=#ffffff>minimap zoom <0.75..4.0></color> - Set map zoom level");
        Print("<color=#ffffff>minimap size <140..350></color> - Set minimap dimension in pixels");
        Print("<color=#ffffff>minimap day <header|footer|off></color> - DayCounter position");
        Print("<color=#ffffff>minimap pos <topright|topleft|bottomright|bottomleft|reset></color> - Anchor position");
        Print("<color=#ffffff>minimap blips <police|dealers|properties|quests|clamp> <on|off></color> - Filter POI blips");
        Print("<color=#ffffff>minimap status</color> - Display live settings and state");
        Print("<color=#ffffff>Hotkeys: [M] Toggle HUD, [ / ] Zoom In/Out, Drag & Drop with mouse</color>");
    }

    private void Print(string message)
    {
        MelonLogger.Msg(message);
    }
}

public sealed class MinimapMapAliasCommand : BaseConsoleCommand
{
    private static readonly MinimapConsoleCommand _mainCmd = new();

    public override string CommandWord => "map";
    public override string CommandDescription => "Alias for 'minimap' command suite.";
    public override string ExampleUsage => "map status";

    public override void ExecuteCommand(List<string> args)
    {
        _mainCmd.ExecuteCommand(args);
    }
}
