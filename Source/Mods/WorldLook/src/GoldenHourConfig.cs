namespace GoldenHour;

/// <summary>TOML-sidecar config (MelonPreferences via S1Mods.Shared ModConfig).</summary>
public sealed class GoldenHourConfig
{
    /// <summary>Look strength 0..1; clamped by BlendPolicy at apply time.</summary>
    public float Strength { get; set; } = 1.0f;

    /// <summary>Bounded reapply cadence in seconds (minimum 0.5 enforced by ReapplyPolicy).</summary>
    public float ReapplyIntervalSeconds { get; set; } = 1.0f;

    /// <summary>
    /// Re-apply in <c>OnLateUpdate</c> every frame instead of on the bounded
    /// cadence. On by default since 0.1.4: the game's day/night grading rewrites
    /// sun/ambient state continuously (measured 2026-10-17 — the 0.1.3 assumption
    /// that it does not was wrong), and a once-per-second reapply loses every frame
    /// race, so the bias never shows. Costs seven IL2CPP read/write pairs per frame.
    /// </summary>
    public bool ApplyEveryLateUpdate { get; set; } = true;

    /// <summary>
    /// Opt-in diagnostic: use <see cref="GoldenHourTargets.DiagnosticVolumeTint"/> so a
    /// working URP volume path is unmissable on screen. Never leave this on for normal play.
    /// </summary>
    public bool DiagnosticStrongTint { get; set; } = false;
}
