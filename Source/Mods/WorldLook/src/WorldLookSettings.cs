using System;
using System.IO;
using System.Text.Json;

namespace WorldLook;

/// <summary>One startup profile. Warmth scales the mild recipe, not the old cloud lighting adjustment.</summary>
public sealed class WorldLookSettings
{
    public bool Enabled { get; set; } = true;
    public bool LightingEnabled { get; set; } = true;
    public bool WaterEnabled { get; set; } = true;
    public double Warmth { get; set; } = 1.0;

    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
        WriteIndented = true
    };

    public static WorldLookSettings Parse(string json)
    {
        var settings = JsonSerializer.Deserialize<WorldLookSettings>(json, Options)
            ?? throw new InvalidDataException("The WorldLook profile contains null.");
        settings.Warmth = double.IsFinite(settings.Warmth) ? Math.Clamp(settings.Warmth, 0.0, 1.0) : 0.0;
        return settings;
    }

    /// <summary>
    /// Only a confirmed absent profile creates defaults. Existing files, including legacy
    /// Clouds settings and unknown keys, are never rewritten by a read or by the F7 toggle.
    /// An unreadable profile leaves this session's look disabled and preserves the input.
    /// </summary>
    public static WorldLookSettings Load(Func<string> read, Func<string, bool> saveNew, Action<string> warning)
    {
        try
        {
            return Parse(read());
        }
        catch (FileNotFoundException) { }
        catch (DirectoryNotFoundException) { }
        catch (Exception ex)
        {
            warning("Profile unreadable; look disabled for this session, original file retained: " + ex.Message);
            return new WorldLookSettings { Enabled = false };
        }

        var defaults = new WorldLookSettings();
        try
        {
            if (!saveNew(JsonSerializer.Serialize(defaults, Options)))
                warning("Default profile could not be saved; session defaults are active.");
        }
        catch (Exception ex)
        {
            warning("Default profile could not be saved; session defaults are active: " + ex.Message);
        }
        return defaults;
    }
}
