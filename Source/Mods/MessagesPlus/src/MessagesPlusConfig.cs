namespace MessagesPlus;

/// <summary>
/// MelonPreferences-backed configuration for MessagesPlus (via S1Mods.Shared.ModConfig).
/// The v0.1.x trash schema is gone with the trash feature; the remaining fields
/// are the Phase 2/3 placeholders (toast/sound + custom background) so the
/// config schema stays stable across phases.
/// </summary>
public sealed class MessagesPlusConfig
{
    /// <summary>Dark theme for the whole Messages app (injected surface + vanilla
    /// pages). PERMANENT since v0.4.1 — always ON, no in-app toggle any more; the
    /// field is kept so the config schema stays stable and a stale "false" from
    /// older versions is self-healed to ON at startup.</summary>
    public bool DarkMode { get; set; } = true;

    /// <summary>Top color of the app background gradient (hex, Phase 3).</summary>
    public string BackgroundColor1 { get; set; } = "#101318";

    /// <summary>Bottom color of the app background gradient (hex, Phase 3).</summary>
    public string BackgroundColor2 { get; set; } = "#1C2230";

    /// <summary>Show toast popups on new messages (Phase 2).</summary>
    public bool ToastEnabled { get; set; } = true;

    /// <summary>Play sound effects (Phase 2).</summary>
    public bool SoundEnabled { get; set; } = true;
}
