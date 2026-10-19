using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using MelonLoader;
using PropertyLEDs.Core;
using S1API.Console;
using UnityEngine;

namespace PropertyLEDs;

/// <summary>Installed S1API 3.2.1 command contract. Shared dispatcher serves console and primitive MCP bridge.</summary>
public sealed class LEDConsoleCommand : BaseConsoleCommand
{
    public override string CommandWord => "led";
    public override string CommandDescription => "PropertyLEDs: bounded property survey, measured capture, preview, export and explicit reload. 'led help'.";
    public override string ExampleUsage => "led properties";
    public override void ExecuteCommand(List<string> args) => MelonLogger.Msg(ExecuteArgs(args));

    internal static string ExecuteText(string command)
    {
        try
        {
            var args = Tokenize(command);
            if (args.Count > 0 && args[0].Equals("led", StringComparison.OrdinalIgnoreCase)) args.RemoveAt(0);
            return ExecuteArgs(args);
        }
        catch (Exception ex) { return "ERROR: " + ex.Message; }
    }

    internal static string ExecuteArgs(List<string> args)
    {
        try
        {
            if (args == null || args.Count == 0) return Help;
            if (args.Count > 16) throw new ArgumentException("Too many arguments (max 16).");
            for (int i = 0; i < args.Count; i++)
                if (args[i] == null || args[i].Length > 512) throw new ArgumentException("Argument exceeds 512 characters.");
            switch (args[0].ToLowerInvariant())
            {
                case "help": Count(args, 1, 1); return Help;
                case "status": Count(args, 1, 1); return LEDDiagnostics.StatusJson();
                case "properties": Count(args, 1, 1); return LEDDiagnostics.Properties();
                case "rooms": Count(args, 2, 2); return LEDDiagnostics.Rooms(args[1]);
                case "dump":
                    Count(args, 2, 5);
                    return LEDDiagnostics.Dump(args[1], args.Count > 2 ? args[2] : ".",
                        args.Count > 3 ? Integer(args[3]) : 256, args.Count > 4 ? Integer(args[4]) : 6);
                case "begin":
                    Count(args, 3, 5);
                    return LEDDiagnostics.Begin(args[1], args[2], args.Count > 3 ? args[3] : ".", args.Count > 4 ? args[4] : args[2]);
                case "local":
                case "world":
                    Count(args, 4, 4);
                    return LEDDiagnostics.Point(new Vector3(Number(args[1]), Number(args[2]), Number(args[3])), args[0].Equals("world", StringComparison.OrdinalIgnoreCase));
                case "aim": Count(args, 1, 2); return LEDDiagnostics.Aim(args.Count > 1 ? Number(args[1]) : 10f);
                case "arm": Count(args, 1, 2); return LEDDiagnostics.ArmAim(args.Count > 1 ? Number(args[1]) : 10f);
                case "path": Count(args, 1, 2); if (args.Count == 2 && !args[1].Equals("open", StringComparison.OrdinalIgnoreCase)) throw new ArgumentException("Use led path [open]."); return LEDDiagnostics.NewPath();
                case "closed": Count(args, 2, 2); return LEDDiagnostics.Closed(Boolean(args[1]));
                case "undo": Count(args, 1, 1); return LEDDiagnostics.Undo();
                case "width": Count(args, 2, 2); return LEDDiagnostics.Width(Number(args[1]));
                case "preview": Count(args, 1, 1); return LEDDiagnostics.Preview();
                case "hide": Count(args, 1, 1); LEDDiagnostics.HidePreview(); return "Temporary markers and LED preview removed. Capture retained.";
                case "clear": Count(args, 1, 1); return LEDDiagnostics.Clear();
                case "export": Count(args, 1, 2); return LEDDiagnostics.Export(args.Count > 1 ? args[1] : "layout_candidate");
                case "reload": Count(args, 1, 1); return LEDDiagnostics.Reload();
                case "refresh": Count(args, 1, 1); LEDDiagnostics.RequireReady(); LEDRuntime.Refresh(); return LEDDiagnostics.Properties();
                case "audit": Count(args, 1, 1); return LEDDiagnostics.CatalogAudit();
                case "set": Count(args, 5, 5); return Set(args[1], args[2], args[3], args[4]);
                case "white": Count(args, 3, 3); return White(args[1], args[2]);
                case "inherit":
                    Count(args, 3, 3);
                    string? room = Scope(args[1], args[2]);
                    if (room == null) throw new ArgumentException("Inheritance requires a room, not property scope '.'.");
                    LEDRuntime.UsePropertyColor(args[1], room);
                    return LEDDiagnostics.StatusJson();
                default: throw new ArgumentException("Unknown subcommand. Use led help.");
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn("led command rejected: " + ex.Message);
            return "ERROR: " + ex.Message;
        }
    }

    private static string? Scope(string propertyId, string roomId)
    {
        LEDDiagnostics.Property(propertyId);
        if (roomId == ".") return null;
        var rooms = LEDRuntime.GetRooms(propertyId);
        for (int i = 0; i < rooms.Count; i++) if (rooms[i].RoomId == roomId) return roomId;
        throw new ArgumentException("Unknown room ID for selected property; use led rooms. '.' selects property default.");
    }

    private static string Set(string propertyId, string roomId, string key, string text)
    {
        string? room = Scope(propertyId, roomId);
        var settings = LEDRuntime.GetSettings(propertyId, room).Clone();
        switch (key.ToLowerInvariant())
        {
            case "enabled": settings.Enabled = Boolean(text); break;
            case "hue": settings.Hue = Range(text, 0, 1); settings.EnableForEdit(); break;
            case "saturation": settings.Saturation = Range(text, 0, 1); settings.EnableForEdit(); break;
            case "intensity": settings.ColorIntensity = Range(text, 0, 1); settings.EnableForEdit(); break;
            case "brightness": settings.Brightness = Range(text, 0, 5); settings.EnableForEdit(); break;
            default: throw new ArgumentException("Setting: enabled, hue, saturation, intensity or brightness.");
        }
        LEDRuntime.Apply(propertyId, room, settings);
        // Runtime Apply returns void: show read-back state, never promise a disk write succeeded.
        return LEDDiagnostics.StatusJson();
    }

    private static string White(string propertyId, string roomId)
    {
        string? room = Scope(propertyId, roomId);
        var settings = LEDRuntime.GetSettings(propertyId, room).Clone();
        var white = LEDSettings.White();
        settings.Hue = white.Hue;
        settings.Saturation = white.Saturation;
        settings.ColorIntensity = white.ColorIntensity;
        settings.EnableForEdit();
        LEDRuntime.Apply(propertyId, room, settings);
        return LEDDiagnostics.StatusJson();
    }

    private static void Count(List<string> args, int min, int max)
    {
        if (args.Count < min || args.Count > max) throw new ArgumentException("Wrong argument count; see led help. Quote paths/display names that contain spaces.");
    }
    private static int Integer(string value)
    {
        if (!int.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out int result))
            throw new ArgumentException("Expected nonnegative decimal integer.");
        return result;
    }
    private static float Number(string value)
    {
        if (!float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out float result) || !float.IsFinite(result))
            throw new ArgumentException("Expected finite number with '.' decimal separator (no NaN/Infinity).");
        return result;
    }
    private static float Range(string text, float min, float max)
    {
        float value = Number(text);
        if (value < min || value > max) throw new ArgumentException("Value outside permitted setting range.");
        return value;
    }
    private static bool Boolean(string value)
    {
        if (!bool.TryParse(value, out bool result)) throw new ArgumentException("Expected true or false.");
        return result;
    }

    /// <summary>Bounded quote-aware parser; preserves all token text, no filesystem/path normalization.</summary>
    internal static List<string> Tokenize(string command)
    {
        if (command == null || command.Length > 4096) throw new ArgumentException("Command must be at most 4096 characters.");
        var args = new List<string>();
        var token = new StringBuilder();
        bool quoted = false, started = false;
        for (int i = 0; i < command.Length; i++)
        {
            char c = command[i];
            if (c == '"') { quoted = !quoted; started = true; continue; }
            if (!quoted && char.IsWhiteSpace(c))
            {
                if (started) { args.Add(token.ToString()); token.Clear(); started = false; }
                if (args.Count > 16) throw new ArgumentException("Too many arguments (max 16).");
                continue;
            }
            if (char.IsControl(c)) throw new ArgumentException("Control characters are not allowed in command tokens.");
            token.Append(c);
            started = true;
            if (token.Length > 512) throw new ArgumentException("Token exceeds 512 characters.");
        }
        if (quoted) throw new ArgumentException("Unclosed double quote.");
        if (started) args.Add(token.ToString());
        if (args.Count > 16) throw new ArgumentException("Too many arguments (max 16).");
        return args;
    }

    private const string Help = "PropertyLEDs diagnostics (exact IDs; quote paths/names with spaces):\n" +
        "led properties | rooms <propertyId> | status\n" +
        "led dump <propertyId> [anchorPath|.] [nodes=256, max512] [depth=6, max12]\n" +
        "led begin <propertyId> <roomId> [anchorPath|.] [displayName]\n" +
        "led local <x> <y> <z> | world <x> <y> <z> | aim [metres=10, max30]\n" +
        "led arm [metres] (close console, click within 30s; Escape cancels)\n" +
        "led path [open] | closed <true|false> | undo | width <0.005..0.25>\n" +
        "led preview | hide | clear | export [safeName] | reload | refresh | audit\n" +
        "led set <propertyId> <roomId|.> <enabled|hue|saturation|intensity|brightness> <value>\n" +
        "led white <propertyId> <roomId|.> | inherit <propertyId> <roomId>\n" +
        "Captures remain Verified=false. Candidate exports do not overwrite layouts.json.\n" +
        "No geometry guessed; no scene scans; settings and rendering owned by runtime.";
}
