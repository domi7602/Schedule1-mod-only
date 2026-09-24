namespace MessagesPlus;

/// <summary>
/// MelonPreferences-backed configuration for MessagesPlus (via S1Mods.Shared.ModConfig).
/// Phase 1 only persists the trash; background colors and toast/sound toggles are
/// pre-defined for Phase 2/3 (liveliness + custom background) so the config schema
/// stays stable across phases.
/// </summary>
public sealed class MessagesPlusConfig
{
    /// <summary>Top color of the app background gradient (hex, Phase 3).</summary>
    public string BackgroundColor1 { get; set; } = "#101318";

    /// <summary>Bottom color of the app background gradient (hex, Phase 3).</summary>
    public string BackgroundColor2 { get; set; } = "#1C2230";

    /// <summary>Show toast popups on new messages (Phase 2).</summary>
    public bool ToastEnabled { get; set; } = true;

    /// <summary>Play sound effects (Phase 2).</summary>
    public bool SoundEnabled { get; set; } = true;
}
