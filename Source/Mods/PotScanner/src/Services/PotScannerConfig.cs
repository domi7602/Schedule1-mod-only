namespace PotScanner.Services;

/// <summary>
/// Persisted settings for PotScanner. Loaded/saved via Shared.ModConfig&lt;T&gt;
/// (which uses MelonPreferences under the hood). Add new settings as auto-properties
/// and they will be picked up automatically on next save.
/// </summary>
public class PotScannerConfig
{
    /// <summary>v0.3.0: when true, dry pots are auto-watered on each in-game minute via Harmony-Patch. -50g per watered pot.</summary>
    public bool AutoWaterEnabled { get; set; } = false;
}
