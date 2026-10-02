using System;
using Il2CppInterop.Runtime.Injection;
using MelonLoader;
using S1API.Input;
using S1Mods.Shared;
using UnityEngine;
using UnityEngine.UI;

[assembly: MelonInfo(typeof(MessagesPlus.Mod), "MessagesPlus", "0.4.1", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace MessagesPlus;

/// <summary>
/// MessagesPlus — inbox hygiene for the vanilla MessagesApp: customer-only
/// Clear All + Clear Read, live name search, category filter chips and an
/// unread counter. Patch-only mod (plain MelonMod + Harmony, like
/// MoreSaveSlots/StackLimitMod): it enhances the EXISTING in-game Messages app
/// and never registers a new PhoneApp or homescreen icon.
/// See MessagesAppPatch/InboxUI/InboxView/AppTheme.
/// </summary>
public class Mod : MelonMod
{
    /// <summary>Shared logger for the mod (wired in <see cref="OnInitializeMelon"/>).</summary>
    public static ModLogger Log { get; private set; } = null!;

    public override void OnInitializeMelon()
    {
        Log = new ModLogger("MessagesPlus");

        // 0. IL2CPP registration for the search field's InputFocus guard (Key Rule 5).
        try { ClassInjector.RegisterTypeInIl2Cpp<MessagesPlusInputFocus>(); }
        catch (Exception ex) { Log.Warn($"Failed to register MessagesPlusInputFocus: {ex.Message}"); }

        // 1. Config (Phase 2 toast/sound + Phase 3 background placeholders).
        ModConfig<MessagesPlusConfig>.Initialize("MessagesPlus", Log);

        // 1b. Dark mode is PERMANENT (v0.4.1): the in-app toggle is gone — self-heal
        //     a stale "false" left behind by the 0.4.0 toggle so the saved look is
        //     always dark. The config field itself stays for schema stability.
        MessagesPlusConfig? cfg = ModConfig<MessagesPlusConfig>.Instance;
        if (cfg != null && !cfg.DarkMode)
        {
            ModConfig<MessagesPlusConfig>.SetAndSave(nameof(MessagesPlusConfig.DarkMode), true);
            Log.Info("Dark mode is permanent — config forced to ON.");
        }

        // 2. Harmony patches on the vanilla MessagesApp + the deal-window popup
        //    (PatchGuard = graceful degradation if a game update renames a method).
        MessagesAppPatch.ApplyAll(HarmonyInstance, Log);
        DealWindowSelectorPatch.ApplyAll(HarmonyInstance, Log);
        PatchGuard.Report(Log);

        Log.Info("MessagesPlus v0.4.1 initialized (search band + category chips + unread counter + ... menu with Clear Read/All + permanent whole-app dark mode + instant deal-popup theming).");
    }

    /// <summary>
    /// Single throttled dispatcher (1 s, see InboxUI.Tick) for the injected UI:
    /// re-applies the search/filter view (vanilla can re-show entries on its own
    /// events — the v0.1.x TrashUI W5 lesson) and refreshes the unread counter.
    /// Free when the phone is closed or no view is active.
    /// </summary>
    public override void OnUpdate()
    {
        InboxUI.Tick();
    }

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        base.OnSceneWasUnloaded(buildIndex, sceneName);
        if (sceneName.Equals("Main", StringComparison.OrdinalIgnoreCase))
        {
            // UI references die with the scene — drop them (never Destroy here).
            InboxUI.TearDownForSceneUnload();
            AppTheme.HandleSceneUnload();
        }
    }
}

// ---------------------------------------------------------------------------
// Merged from MessagesPlusConfig.cs (2026-10-02) — MelonPreferences schema.
// ---------------------------------------------------------------------------

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

// ---------------------------------------------------------------------------
// Merged from MessagesPlusInputFocus.cs (2026-10-02) — typing guard.
// ---------------------------------------------------------------------------

/// <summary>
/// InputFocus guard (Key Rule 5) for the MessagesPlus search field: while the
/// InputField is focused, `Controls.IsTyping` suppresses player movement
/// (WASD). Registered via ClassInjector in Mod.OnInitializeMelon — the public
/// IntPtr constructor is mandatory (without it AddComponent crashes the IL2CPP
/// bridge). Mirrors NotesAppInputFocus, minus the editor auto-focus.
/// </summary>
[RegisterTypeInIl2Cpp]
internal sealed class MessagesPlusInputFocus : MonoBehaviour
{
    public MessagesPlusInputFocus(IntPtr ptr) : base(ptr) { }

    public InputField searchInput = null!;

    private bool _lastTyping;

    private void Update()
    {
        bool typing = searchInput != null && searchInput.isFocused;
        if (typing != _lastTyping)
        {
            _lastTyping = typing;
            Controls.IsTyping = typing;
        }
    }

    private void OnDisable()
    {
        _lastTyping = false;
        Controls.IsTyping = false;
    }
}
