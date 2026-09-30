namespace MessagesPlus;

/// <summary>
/// MelonPreferences-backed configuration for MessagesPlus (via S1Mods.Shared.ModConfig).
/// The v0.1.x trash schema is gone with the trash feature; the remaining fields
/// are the Phase 2/3 placeholders (toast/sound + custom background) so the
/// config schema stays stable across phases.
/// </summary>
public sealed class MessagesPlusConfig
{
    /// <summary>Dark theme for the injected inbox surface (search band + "..." menu +
    /// confirm dialog). Toggled in-app via the "..." menu; the vanilla conversation
    /// list is untouched. (v0.4.0)</summary>
    public bool DarkMode { get; set; } = false;

    /// <summary>Top color of the app background gradient (hex, Phase 3).</summary>
    public string BackgroundColor1 { get; set; } = "#101318";

    /// <summary>Bottom color of the app background gradient (hex, Phase 3).</summary>
    public string BackgroundColor2 { get; set; } = "#1C2230";

    /// <summary>Show toast popups on new messages (Phase 2).</summary>
    public bool ToastEnabled { get; set; } = true;

    /// <summary>Play sound effects (Phase 2).</summary>
    public bool SoundEnabled { get; set; } = true;
}
