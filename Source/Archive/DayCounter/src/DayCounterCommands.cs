using System;
using System.Collections.Generic;
using System.Globalization;
using MelonLoader;
using S1API.Console;
using S1Mods.Shared;
using UnityEngine;

namespace DayCounter;

public sealed class DayCounterConsoleCommand : BaseConsoleCommand
{
    public override string CommandWord => "daycounter";

    public override string CommandDescription =>
        "DayCounter: status, toggle, show, hide, compact, format <12h|24h>, shortday, daynum, dayname, time, strip, autohide, anchor <corner>, pos <x> <y>, scale <val>, opacity <val>, hotkey <key>, reset, help";

    public override string ExampleUsage => "day status";

    public override void ExecuteCommand(List<string> args)
    {
        try
        {
            var mod = DayCounterMod.Instance;
            if (mod == null)
            {
                Print("DayCounter mod instance is not running.");
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
                    mod.ToggleHUD();
                    Print($"<color=#60f080> HUD is now {(ModConfig<DayCounterConfig>.Instance.Visible ? "VISIBLE" : "HIDDEN")}.");
                    return;

                case "show":
                    ModConfig<DayCounterConfig>.SetAndSave("Visible", true);
                    mod.ApplyHUDConfig();
                    Print("<color=#60f080> HUD is now VISIBLE.");
                    return;

                case "hide":
                    ModConfig<DayCounterConfig>.SetAndSave("Visible", false);
                    mod.ApplyHUDConfig();
                    Print("<color=#60f080> HUD is now HIDDEN.");
                    return;

                case "compact":
                case "c":
                    bool newCompact;
                    if (args.Count >= 2 && bool.TryParse(args[1], out bool parsedCompact))
                    {
                        newCompact = parsedCompact;
                    }
                    else
                    {
                        newCompact = !ModConfig<DayCounterConfig>.Instance.CompactMode;
                    }
                    ModConfig<DayCounterConfig>.SetAndSave("CompactMode", newCompact);
                    mod.ApplyHUDConfig();
                    Print($"<color=#60f080> Compact mode: {(newCompact ? "ENABLED (Slim 1-Row Pill)" : "DISABLED (Standard 2-Row Badge)")}.");
                    return;

                case "format":
                case "f":
                    if (args.Count >= 2)
                    {
                        string fmt = args[1].ToLowerInvariant();
                        if (fmt == "24" || fmt == "24h")
                        {
                            ModConfig<DayCounterConfig>.SetAndSave("TimeFormat24H", true);
                            mod.ApplyHUDConfig();
                            Print("<color=#60f080> Time format set to: 24-Hour (15:30).");
                        }
                        else if (fmt == "12" || fmt == "12h")
                        {
                            ModConfig<DayCounterConfig>.SetAndSave("TimeFormat24H", false);
                            mod.ApplyHUDConfig();
                            Print("<color=#60f080> Time format set to: 12-Hour AM/PM (03:30 PM).");
                        }
                        else
                        {
                            Print("<color=#f06060> Invalid format. Usage: day format <24h|12h>");
                        }
                    }
                    else
                    {
                        bool toggle24 = !ModConfig<DayCounterConfig>.Instance.TimeFormat24H;
                        ModConfig<DayCounterConfig>.SetAndSave("TimeFormat24H", toggle24);
                        mod.ApplyHUDConfig();
                        Print($"<color=#60f080> Time format toggled to: {(toggle24 ? "24-Hour (15:30)" : "12-Hour (03:30 PM)")}.");
                    }
                    return;

                case "shortday":
                case "short":
                    bool shortDay = !ModConfig<DayCounterConfig>.Instance.ShortDayNames;
                    if (args.Count >= 2 && bool.TryParse(args[1], out bool pShort)) shortDay = pShort;
                    ModConfig<DayCounterConfig>.SetAndSave("ShortDayNames", shortDay);
                    mod.ApplyHUDConfig();
                    Print($"<color=#60f080> Short day names: {(shortDay ? "ENABLED (e.g. Wed)" : "DISABLED (e.g. Wednesday)")}.");
                    return;

                case "daynum":
                case "daynumber":
                case "num":
                    bool showDayNum = !ModConfig<DayCounterConfig>.Instance.ShowDayNumber;
                    if (args.Count >= 2 && bool.TryParse(args[1], out bool pNum)) showDayNum = pNum;
                    ModConfig<DayCounterConfig>.SetAndSave("ShowDayNumber", showDayNum);
                    mod.ApplyHUDConfig();
                    Print($"<color=#60f080> Day number (DAY X): {(showDayNum ? "ENABLED" : "DISABLED")}.");
                    return;

                case "dayname":
                case "name":
                    bool showName = !ModConfig<DayCounterConfig>.Instance.ShowDayName;
                    if (args.Count >= 2 && bool.TryParse(args[1], out bool pName)) showName = pName;
                    ModConfig<DayCounterConfig>.SetAndSave("ShowDayName", showName);
                    mod.ApplyHUDConfig();
                    Print($"<color=#60f080> Day name (e.g. Wednesday): {(showName ? "ENABLED" : "DISABLED")}.");
                    return;

                case "time":
                case "clock":
                    bool showTime = !ModConfig<DayCounterConfig>.Instance.ShowTime;
                    if (args.Count >= 2 && bool.TryParse(args[1], out bool pTime)) showTime = pTime;
                    ModConfig<DayCounterConfig>.SetAndSave("ShowTime", showTime);
                    mod.ApplyHUDConfig();
                    Print($"<color=#60f080> Time display: {(showTime ? "ENABLED" : "DISABLED")}.");
                    return;

                case "strip":
                    bool showStrip = !ModConfig<DayCounterConfig>.Instance.ShowAccentStrip;
                    if (args.Count >= 2 && bool.TryParse(args[1], out bool pStrip)) showStrip = pStrip;
                    ModConfig<DayCounterConfig>.SetAndSave("ShowAccentStrip", showStrip);
                    mod.ApplyHUDConfig();
                    Print($"<color=#60f080> Accent strip: {(showStrip ? "ENABLED" : "DISABLED")}.");
                    return;

                case "autohide":
                    bool autoHide = !ModConfig<DayCounterConfig>.Instance.AutoHideInMenus;
                    if (args.Count >= 2 && bool.TryParse(args[1], out bool pHide)) autoHide = pHide;
                    ModConfig<DayCounterConfig>.SetAndSave("AutoHideInMenus", autoHide);
                    mod.ApplyHUDConfig();
                    Print($"<color=#60f080> Auto-hide in menus (phone/pause/map): {(autoHide ? "ENABLED" : "DISABLED")}.");
                    return;

                case "anchor":
                case "a":
                    if (args.Count >= 2)
                    {
                        string anc = args[1].ToLowerInvariant();
                        string chosen;
                        switch (anc)
                        {
                            case "topright":
                            case "tr":
                                chosen = "TopRight";
                                break;
                            case "bottomleft":
                            case "bl":
                                chosen = "BottomLeft";
                                break;
                            case "bottomright":
                            case "br":
                                chosen = "BottomRight";
                                break;
                            case "topleft":
                            case "tl":
                            default:
                                chosen = "TopLeft";
                                break;
                        }
                        ModConfig<DayCounterConfig>.SetAndSave("Anchor", chosen);
                        mod.ApplyHUDConfig();
                        Print($"<color=#60f080> Anchor set to {chosen}.");
                    }
                    else
                    {
                        Print("<color=#f06060> Usage: day anchor <topleft|topright|bottomleft|bottomright>");
                    }
                    return;

                case "pos":
                case "position":
                    if (args.Count >= 3 &&
                        float.TryParse(args[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float px) &&
                        float.TryParse(args[2], NumberStyles.Float, CultureInfo.InvariantCulture, out float py))
                    {
                        ModConfig<DayCounterConfig>.Instance.PositionX = px;
                        ModConfig<DayCounterConfig>.Instance.PositionY = py;
                        ModConfig<DayCounterConfig>.Save();
                        mod.ApplyHUDConfig();
                        Print($"<color=#60f080> Position set to ({px:F1}, {py:F1}).");
                    }
                    else
                    {
                        Print("<color=#f06060> Usage: day pos <x> <y>");
                    }
                    return;

                case "scale":
                    if (args.Count >= 2 && float.TryParse(args[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float scale))
                    {
                        scale = Math.Clamp(scale, 0.5f, 2.5f);
                        ModConfig<DayCounterConfig>.SetAndSave("Scale", scale);
                        mod.ApplyHUDConfig();
                        Print($"<color=#60f080> Scale set to {scale:F2}.");
                    }
                    else
                    {
                        Print("<color=#f06060> Usage: day scale <0.5-2.5>");
                    }
                    return;

                case "opacity":
                case "alpha":
                    if (args.Count >= 2 && float.TryParse(args[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float op))
                    {
                        op = Math.Clamp(op, 0.0f, 1.0f);
                        ModConfig<DayCounterConfig>.SetAndSave("BackgroundOpacity", op);
                        mod.ApplyHUDConfig();
                        Print($"<color=#60f080> Background opacity set to {op:F2}.");
                    }
                    else
                    {
                        Print("<color=#f06060> Usage: day opacity <0.0-1.0>");
                    }
                    return;

                case "hotkey":
                case "key":
                    if (args.Count >= 2 && Enum.TryParse<KeyCode>(args[1], true, out KeyCode parsedKey))
                    {
                        ModConfig<DayCounterConfig>.SetAndSave("ToggleHotkey", parsedKey);
                        Print($"<color=#60f080> Toggle hotkey set to {parsedKey}.");
                    }
                    else
                    {
                        Print("<color=#f06060> Usage: day hotkey <KeyCode> (e.g. F7, F8, K, End)");
                    }
                    return;

                case "reset":
                    var cfg = ModConfig<DayCounterConfig>.Instance;
                    cfg.Visible = true;
                    cfg.ToggleHotkey = KeyCode.F7;
                    cfg.Anchor = "TopLeft";
                    cfg.PositionX = 24f;
                    cfg.PositionY = -24f;
                    cfg.Scale = 1.0f;
                    cfg.BackgroundOpacity = 0.75f;
                    cfg.CompactMode = true;
                    cfg.TimeFormat24H = true;
                    cfg.ShortDayNames = false;
                    cfg.ShowDayNumber = true;
                    cfg.ShowDayName = true;
                    cfg.ShowTime = false;
                    cfg.ShowAccentStrip = true;
                    cfg.AllowDragging = true;
                    cfg.AutoHideInMenus = false;
                    ModConfig<DayCounterConfig>.Save();
                    mod.ApplyHUDConfig();
                    Print("<color=#60f080> Reset all settings to defaults (Visible: true, Hotkey: F7, Anchor: TopLeft, Pos: 24, -24).");
                    return;

                case "help":
                case "h":
                case "?":
                default:
                    PrintHelp();
                    return;
            }
        }
        catch (Exception ex)
        {
            Print($"<color=#f06060>DayCounter Error: {ex.Message}</color>");
        }
    }

    private static void PrintHelp()
    {
        Print("<color=#60f080>=== DayCounter Commands ===</color>");
        Print("  <color=#80d0ff>day status</color>                     - Show current day, time, and overlay configuration");
        Print("  <color=#80d0ff>day toggle</color>                     - Toggle HUD overlay visibility");
        Print("  <color=#80d0ff>day compact</color>                    - Switch between Standard 2-Row and Slim 1-Row Pill");
        Print("  <color=#80d0ff>day format <24h|12h></color>           - Switch time format (24-Hour vs 12-Hour AM/PM)");
        Print("  <color=#80d0ff>day shortday [true|false]</color>      - Toggle short day names (e.g. Wed vs Wednesday)");
        Print("  <color=#80d0ff>day daynum [true|false]</color>        - Toggle day number display (DAY X)");
        Print("  <color=#80d0ff>day dayname [true|false]</color>       - Toggle day of week display");
        Print("  <color=#80d0ff>day time [true|false]</color>          - Toggle time clock display");
        Print("  <color=#80d0ff>day strip [true|false]</color>         - Toggle emerald accent strip");
        Print("  <color=#80d0ff>day autohide [true|false]</color>      - Toggle auto-hide when phone/menus open");
        Print("  <color=#80d0ff>day anchor <corner></color>            - Set corner anchor (TopLeft, TopRight, BottomLeft, BottomRight)");
        Print("  <color=#80d0ff>day pos <x> <y></color>                - Set custom anchored HUD position");
        Print("  <color=#80d0ff>day scale <0.5-2.5></color>            - Adjust HUD scale factor");
        Print("  <color=#80d0ff>day opacity <0.0-1.0></color>          - Adjust background transparency");
        Print("  <color=#80d0ff>day hotkey <KeyCode></color>           - Change HUD toggle hotkey (e.g. F7, F8, End)");
        Print("  <color=#80d0ff>day reset</color>                      - Reset all settings to defaults");
    }

    private static bool Is(List<string> args, int index, params string[] options)
    {
        if (index < 0 || index >= args.Count)
            return false;
        string val = args[index];
        foreach (var opt in options)
        {
            if (string.Equals(val, opt, StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }

    private static void Print(string message)
    {
        MelonLogger.Msg(message);
    }
}

public sealed class DayConsoleCommand : BaseConsoleCommand
{
    private static readonly DayCounterConsoleCommand _impl = new();

    public override string CommandWord => "day";

    public override string CommandDescription =>
        "DayCounter shorthand: status, toggle, show, hide, compact, format <12h|24h>, shortday, daynum, dayname, time, strip, autohide, anchor, pos, scale, opacity, hotkey, reset, help";

    public override string ExampleUsage => "day status";

    public override void ExecuteCommand(List<string> args)
    {
        _impl.ExecuteCommand(args);
    }
}
