using System;
using System.Collections.Generic;
using System.IO;
using HomelessMod.Building;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Money;
using Il2CppScheduleOne.Persistence;
using S1API.Quests;
using S1API.Quests.Constants;
using S1API.Quests.Identifiers;
using S1Mods.Shared;
using UnityEngine;

namespace HomelessMod.Quests;

[QuestName("Cold Concrete")]
public class Quest_ColdConcrete : Quest
{
    protected override string Title => "Cold Concrete";
    protected override string Description => "You have no roof over your head. Find a quiet spot on the street, deploy your sleeping bag, and sleep through the night.";

    protected override bool AutoBegin => true;

    protected override void OnCreated()
    {
        AddEntry("Deploy a Sleeping Bag on the street");
        AddEntry("Sleep through the night in your Sleeping Bag");
    }
}

[QuestName("Alley Operations")]
public class Quest_AlleyOperations : Quest
{
    protected override string Title => "Alley Operations";
    protected override string Description => "Operating from the shadows. Set up makeshift gear outdoors and earn your first $500.";

    protected override bool AutoBegin => true;

    protected override void OnCreated()
    {
        AddEntry("Place a workstation or grow container outdoors");
        AddEntry("Accumulate $500 in cash");
    }
}

[QuestName("Street Sovereign")]
public class Quest_StreetSovereign : Quest
{
    protected override string Title => "Street Sovereign";
    protected override string Description => "Transform the city streets into your personal empire. Build a complete street camp and amass $5,000.";

    protected override bool AutoBegin => true;

    protected override void OnCreated()
    {
        AddEntry("Expand your street camp (3+ outdoor items)");
        AddEntry("Earn $5,000 total cash");
    }
}

/// <summary>
/// Orchestrates homeless quests progression and triggers.
///
/// Save-load robustness notes:
/// - Quest and entry references are re-resolved fresh on every trigger instead of
///   caching them, because S1API's quest restore replaces the underlying vanilla
///   Quest/QuestEntry objects on save load (cached wrappers go stale and never
///   complete).
/// - Completed quests are persisted separately so they do not reappear as a fresh
///   active copy after a save reload (the S1API quest registry removes completed
///   quests, which makes them invisible to the normal restore path).
/// </summary>
public static class HomelessQuestManager
{
    private static bool _initialized;

    private static readonly HashSet<string> _completedQuests = new(StringComparer.OrdinalIgnoreCase);

    public static void InitializeQuests(bool force = false)
    {
        if (!Mod.CurrentConfig.EnableHomelessQuests) return;
        if (_initialized && !force) return;

        try
        {
            LoadCompletedState();
            if (!IsQuestCompleted("Cold Concrete"))
            {
                if (ResolveQuest1() != null)
                {
                    Mod.Log.Info("Attached to existing 'Cold Concrete' quest.");
                }
                else
                {
                    // Gatekeeper-fix 2026-08-30 H1/L7: S1API CreateQuest takes internal id (homeless_quest_1) while GetQuestByName resolves by Title (Cold Concrete). Verify type before use.
                    var created = QuestManager.CreateQuest<Quest_ColdConcrete>("homeless_quest_1");
                    if (created is Quest_ColdConcrete)
                        Mod.Log.Info("Registered new 'Cold Concrete' quest with S1API.");
                    else
                        Mod.Log.Warn($"CreateQuest homeless_quest_1 returned unexpected type {created?.GetType().Name ?? "null"} — S1API semantics mismatch?");
                }
            }
            else if (!IsQuestCompleted("Alley Operations"))
            {
                // Gatekeeper-fix 2026-08-30 H1: restore chain after save-crash where Q1 completed but Q2 never persisted
                if (ResolveQuest2() == null) GetOrCreateQuest2();
            }
            else if (!IsQuestCompleted("Street Sovereign"))
            {
                // Gatekeeper-fix 2026-08-30 H1: restore chain where Q2 completed but Q3 never persisted
                if (ResolveQuest3() == null) GetOrCreateQuest3();
            }
            _initialized = true;
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"Failed to initialize homeless quests: {ex.Message}");
        }
    }

    public static void NotifyItemPlaced(string itemId, GameObject? placedObj = null)
    {
        try
        {
            // Quest 1: Deploy the sleeping bag
            var q1 = ResolveQuest1();
            if (q1 != null && itemId.Equals(Mod.CurrentConfig.SleepingBagItemId, StringComparison.OrdinalIgnoreCase))
            {
                var deploy = FindEntry(q1, "Deploy a Sleeping Bag on the street");
                if (deploy != null && deploy.State != QuestState.Completed)
                {
                    deploy.Complete();
                    FindEntry(q1, "Sleep through the night in your Sleeping Bag")?.Begin();
                    Mod.Log.Info("Completed: 'Deploy a Sleeping Bag on the street'! Next objective: Sleep through the night.");
                }
            }

            // Quest 2: Place gear outdoors
            // Review-fix 2026-09-09 (v0.1.5, high): only production gear completes this entry.
            // The sleeping bag (Quest 1 item) must NOT satisfy "workstation or grow container".
            var q2 = ResolveQuest2();
            if (q2 != null)
            {
                if (!IsProductionGear(placedObj, itemId))
                {
                    Mod.Log.Debug($"[Quest2] '{itemId}' is not production gear — entry not counted.");
                }
                else
                {
                    var gear = FindEntry(q2, "Place a workstation or grow container outdoors");
                    if (gear != null && gear.State != QuestState.Completed)
                    {
                        gear.Complete();
                        FindEntry(q2, "Accumulate $500 in cash")?.Begin();
                        Mod.Log.Info("Completed: 'Place a workstation or grow container outdoors'!");
                    }
                    // Fix 2026-09-09 (v0.1.4): finalize on EVERY placement trigger, regardless of
                    // which entry (gear vs. $500) completed first. Previously the both-done check
                    // lived only in CheckCashProgress' money branch — if money completed first,
                    // MarkQuestCompleted never fired and the quest reset on every save reload.
                    TryFinalizeQuest2(q2);
                }
            }

            // Quest 3: Expand the camp
            var q3 = ResolveQuest3();
            if (q3 != null)
            {
                var expand = FindEntry(q3, "Expand your street camp (3+ outdoor items)");
                if (expand != null && expand.State != QuestState.Completed)
                {
                    if (StreetPropertyManager.ActiveStreetObjects.Count >= 3)
                    {
                        expand.Complete();
                        FindEntry(q3, "Earn $5,000 total cash")?.Begin();
                        Mod.Log.Info("Completed: 'Expand your street camp (3+ outdoor items)'!");
                    }
                }
                // Fix 2026-09-09 (v0.1.4): same order-of-completion fix as Quest 2 —
                // also prevents the old premature completion (money-only) from persisting.
                TryFinalizeQuest3(q3);
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Debug($"NotifyItemPlaced quest error: {ex.Message}");
        }
    }

    public static void NotifyPlayerSlept()
    {
        try
        {
            var q1 = ResolveQuest1();
            if (q1 != null)
            {
                var sleep = FindEntry(q1, "Sleep through the night in your Sleeping Bag");
                if (sleep != null && sleep.State != QuestState.Completed)
                {
                    sleep.Complete();
                    Mod.Log.Info("Completed: 'Sleep through the night in your Sleeping Bag'!");

                    MarkQuestCompleted("Cold Concrete");

                    // Advance to Quest 2
                    GetOrCreateQuest2();
                }
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Debug($"NotifyPlayerSlept quest error: {ex.Message}");
        }
    }

    public static void CheckCashProgress()
    {
        // Gatekeeper-fix 2026-08-30 L3: early-out when quests disabled (Mod.OnUpdate also guards, defense in depth)
        if (!Mod.CurrentConfig.EnableHomelessQuests) return;
        try
        {
            var moneyMgr = NetworkSingleton<MoneyManager>.Instance;
            if (moneyMgr == null || moneyMgr.Pointer == IntPtr.Zero) return;

            float cash = moneyMgr.cashBalance;

            var q2 = ResolveQuest2();
            if (q2 != null)
            {
                var earn500 = FindEntry(q2, "Accumulate $500 in cash");
                if (earn500 != null && earn500.State != QuestState.Completed)
                {
                    if (cash >= 500f)
                    {
                        earn500.Complete();
                        Mod.Log.Info("Completed: 'Accumulate $500 in cash'!");
                    }
                }
                // Fix 2026-09-09 (v0.1.4): finalize runs on EVERY poll — order-independent
                // (old code nested the both-done check inside the money branch, so a
                // gear-late completion never persisted the quest).
                TryFinalizeQuest2(q2);
            }

            var q3 = ResolveQuest3();
            if (q3 != null)
            {
                var earn5000 = FindEntry(q3, "Earn $5,000 total cash");
                if (earn5000 != null && earn5000.State != QuestState.Completed)
                {
                    if (cash >= 5000f)
                    {
                        earn5000.Complete();
                        Mod.Log.Info("Completed: 'Earn $5,000 total cash' - Street Kingpin Achieved!");
                    }
                }
                // Fix 2026-09-09 (v0.1.4): both-entries check (old code persisted on money alone).
                TryFinalizeQuest3(q3);
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Debug($"CheckCashProgress quest error: {ex.Message}");
        }
    }

    // ----- Fix 2026-09-09 (v0.1.4): order-independent quest finalization -----

    /// <summary>
    /// Marks 'Alley Operations' completed (and spawns Quest 3) once BOTH entries are done.
    /// Called from every trigger path (item placement + cash poll) so completion persists
    /// regardless of which entry finished first. Idempotent via _completedQuests HashSet.
    /// </summary>
    private static void TryFinalizeQuest2(Quest_AlleyOperations? q2)
    {
        try
        {
            if (q2 == null || IsQuestCompleted("Alley Operations")) return;
            var gear = FindEntry(q2, "Place a workstation or grow container outdoors");
            var earn500 = FindEntry(q2, "Accumulate $500 in cash");
            if (gear != null && gear.State == QuestState.Completed &&
                earn500 != null && earn500.State == QuestState.Completed)
            {
                MarkQuestCompleted("Alley Operations");
                Mod.Log.Info("Quest 'Alley Operations' fully completed — persists on next save.");
                GetOrCreateQuest3();
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"TryFinalizeQuest2 notice: {ex.Message}");
        }
    }

    /// <summary>
    /// Marks 'Street Sovereign' completed once BOTH entries are done. Fixes the old
    /// premature path where a $5,000 balance alone persisted the quest while the
    /// camp-size entry was still open.
    /// </summary>
    private static void TryFinalizeQuest3(Quest_StreetSovereign? q3)
    {
        try
        {
            if (q3 == null || IsQuestCompleted("Street Sovereign")) return;
            var expand = FindEntry(q3, "Expand your street camp (3+ outdoor items)");
            var earn5000 = FindEntry(q3, "Earn $5,000 total cash");
            if (expand != null && expand.State == QuestState.Completed &&
                earn5000 != null && earn5000.State == QuestState.Completed)
            {
                MarkQuestCompleted("Street Sovereign");
                Mod.Log.Info("Quest 'Street Sovereign' fully completed — persists on next save.");
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"TryFinalizeQuest3 notice: {ex.Message}");
        }
    }

    // ----- Production gear detection (Review-fix 2026-09-09, v0.1.5) -----

    /// <summary>
    /// Determines whether a placed outdoor item counts as production gear for Quest 2
    /// ("workstation or grow container"). Component check on the placed object first
    /// (vanilla Pot/MixingStation/Cauldron/ChemistryStation/PackagingStation/BrickPress/
    /// DryingRack — all verified in live Assembly-CSharp decompile 2026-09-09), then a
    /// string fallback for modded gear (e.g. autopackagingstation) that lacks those
    /// components. The sleeping bag must NOT match.
    /// </summary>
    private static bool IsProductionGear(GameObject? placedObj, string itemId)
    {
        if (placedObj != null && placedObj.Pointer != IntPtr.Zero && !placedObj.WasCollected)
        {
            if (placedObj.GetComponentInChildren<Il2CppScheduleOne.ObjectScripts.Pot>(true) != null) return true;
            if (placedObj.GetComponentInChildren<Il2CppScheduleOne.ObjectScripts.MixingStation>(true) != null) return true;
            if (placedObj.GetComponentInChildren<Il2CppScheduleOne.ObjectScripts.MixingStationMk2>(true) != null) return true;
            if (placedObj.GetComponentInChildren<Il2CppScheduleOne.ObjectScripts.Cauldron>(true) != null) return true;
            if (placedObj.GetComponentInChildren<Il2CppScheduleOne.ObjectScripts.ChemistryStation>(true) != null) return true;
            if (placedObj.GetComponentInChildren<Il2CppScheduleOne.ObjectScripts.PackagingStation>(true) != null) return true;
            if (placedObj.GetComponentInChildren<Il2CppScheduleOne.ObjectScripts.BrickPress>(true) != null) return true;
            if (placedObj.GetComponentInChildren<Il2CppScheduleOne.ObjectScripts.DryingRack>(true) != null) return true;
        }

        // String fallback for modded workstations without the known vanilla components.
        string id = (itemId ?? string.Empty).ToLowerInvariant();
        return id.Contains("station") || id.Contains("press") || id.Contains("cauldron")
            || id.Contains("rack") || id.EndsWith("pot");
    }

    public static void ResetState(bool keepSlot = false)
    {
        _initialized = false;
        _completedQuests.Clear();
        _sleepStartedInBag = false;
        if (!keepSlot) _lastKnownQuestSlot = "default"; // Review-fix 2026-09-09 (v0.1.5): mirror StreetPropertyManager keepSlot semantics
    }

    public static void ResetForSceneUnload()
    {
        ResetState(keepSlot: true);
    }

    // ----- Sleep-credit gating (Review-fix 2026-09-09, v0.1.5) -----

    private static bool _sleepStartedInBag;

    /// <summary>Called at StartSleep in the sleeping bag — defers quest credit to the wake hook.</summary>
    public static void NotifySleepStartedInBag()
    {
        _sleepStartedInBag = true;
        Mod.Log.Debug("[Quest1] Sleep started in sleeping bag — quest credit deferred to wake confirmation.");
    }

    /// <summary>
    /// Handler for S1API.GameTime.TimeManager.OnSleepEnd (Action&lt;int&gt;). Only credits
    /// 'Sleep through the night' if the sleep was actually started in the sleeping bag
    /// AND ran to completion (aborting sleep never fires OnSleepEnd).
    /// </summary>
    public static void OnSleepEnded(int _)
    {
        try
        {
            if (!_sleepStartedInBag) return; // vanilla bed sleep or mod-less sleep — no credit
            _sleepStartedInBag = false;
            Mod.Log.Info("Sleep completed (wake confirmed) — applying quest progress.");
            NotifyPlayerSlept();
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"OnSleepEnded notice: {ex.Message}");
        }
    }

    // ----- Fresh lookup helpers (save-load safe) -----

    private static Quest_ColdConcrete? ResolveQuest1() => QuestManager.GetQuestByName("Cold Concrete") as Quest_ColdConcrete;

    private static Quest_AlleyOperations? ResolveQuest2() => QuestManager.GetQuestByName("Alley Operations") as Quest_AlleyOperations;

    private static Quest_StreetSovereign? ResolveQuest3() => QuestManager.GetQuestByName("Street Sovereign") as Quest_StreetSovereign;

    private static Quest_AlleyOperations? GetOrCreateQuest2()
    {
        var existing = ResolveQuest2();
        if (existing != null) return existing;
        var created = QuestManager.CreateQuest<Quest_AlleyOperations>("homeless_quest_2");
        // Gatekeeper-fix 2026-08-30 H1/L7: verify S1API returned expected subtype before cast
        if (created is Quest_AlleyOperations typed) return typed;
        Mod.Log.Warn($"CreateQuest homeless_quest_2 returned unexpected type {created?.GetType().Name ?? "null"}");
        return null;
    }

    private static Quest_StreetSovereign? GetOrCreateQuest3()
    {
        var existing = ResolveQuest3();
        if (existing != null) return existing;
        var created = QuestManager.CreateQuest<Quest_StreetSovereign>("homeless_quest_3");
        // Gatekeeper-fix 2026-08-30 H1/L7: verify S1API returned expected subtype before cast
        if (created is Quest_StreetSovereign typed) return typed;
        Mod.Log.Warn($"CreateQuest homeless_quest_3 returned unexpected type {created?.GetType().Name ?? "null"}");
        return null;
    }

    private static QuestEntry? FindEntry(Quest quest, string title)
    {
        if (quest == null) return null;
        var entries = quest.QuestEntries;
        if (entries == null) return null;

        for (int i = 0; i < entries.Count; i++)
        {
            var entry = entries[i];
            if (entry == null) continue;
            try
            {
                if (entry.Title == title) return entry;
            }
            catch
            {
                // Stale wrapper from a previous save-load — skip it.
            }
        }
        return null;
    }

    // ----- Completion persistence -----

    private static bool IsQuestCompleted(string questName) => _completedQuests.Contains(questName);

    private static void MarkQuestCompleted(string questName)
    {
        // Fix 2026-09-10: completion stays in memory only — flushed to disk on
        // GameLifecycle.OnSaveComplete (same hook as the street-item store),
        // not written mid-gameplay on every quest trigger.
        _completedQuests.Add(questName);
    }

    private static string _lastKnownQuestSlot = "default";

    private static string GetStateFilePath()
    {
        string slotSuffix = "default";
        bool resolved = false;
        try
        {
            var loadMgr = PersistentSingleton<LoadManager>.Instance;
            if (loadMgr != null && loadMgr.Pointer != IntPtr.Zero && !loadMgr.WasCollected)
            {
                var saveInfo = loadMgr.ActiveSaveInfo;
                if (saveInfo != null && saveInfo.Pointer != IntPtr.Zero && !saveInfo.WasCollected)
                {
                    if (saveInfo.SaveSlotNumber >= 0)
                    {
                        slotSuffix = $"slot_{saveInfo.SaveSlotNumber}";
                        resolved = true;
                    }
                    else if (!string.IsNullOrEmpty(saveInfo.SavePath))
                    {
                        slotSuffix = Path.GetFileName(saveInfo.SavePath);
                        resolved = true;
                    }
                }
            }
            if (!resolved)
            {
                var legacyMgr = Singleton<LoadManager>.Instance;
                if (legacyMgr != null && legacyMgr.Pointer != IntPtr.Zero && !legacyMgr.WasCollected)
                {
                    var saveInfo = legacyMgr.ActiveSaveInfo;
                    if (saveInfo != null && saveInfo.Pointer != IntPtr.Zero && !saveInfo.WasCollected)
                    {
                        if (saveInfo.SaveSlotNumber >= 0)
                        {
                            slotSuffix = $"slot_{saveInfo.SaveSlotNumber}";
                            resolved = true;
                        }
                        else if (!string.IsNullOrEmpty(saveInfo.SavePath))
                        {
                            slotSuffix = Path.GetFileName(saveInfo.SavePath);
                            resolved = true;
                        }
                    }
                }
            }
        }
        catch { }
        if (resolved) _lastKnownQuestSlot = slotSuffix;
        else if (!string.IsNullOrEmpty(_lastKnownQuestSlot)) slotSuffix = _lastKnownQuestSlot;

        string dir = SafeStorage.GetUserDataPath("HomelessMod");
        string slotPath = Path.Combine(dir, $"quest_progress_{slotSuffix}.json");
        string legacyPath = Path.Combine(dir, "quest_progress.json");
        TryMigrateQuestLegacy(slotPath, legacyPath);
        return slotPath;
    }

    private static void TryMigrateQuestLegacy(string slotPath, string legacyPath)
    {
        if (!File.Exists(legacyPath)) return;
        if (File.Exists(slotPath))
        {
            try { File.Delete(legacyPath); } catch { }
            return;
        }
        try
        {
            var legacy = SafeStorage.LoadSafe<List<string>>(legacyPath, null, Mod.Log);
            if (legacy != null && legacy.Count > 0)
            {
                if (SafeStorage.SaveAtomic(slotPath, legacy, Mod.Log))
                {
                    try { File.Delete(legacyPath); } catch { }
                    Mod.Log.Info($"Migrated legacy quest_progress.json -> {Path.GetFileName(slotPath)}");
                    return;
                }
            }
            SafeStorage.EnsureDirectoryForFile(slotPath);
            File.Copy(legacyPath, slotPath, true);
            try { File.Delete(legacyPath); } catch { }
            Mod.Log.Info($"Migrated legacy quest_progress.json -> {Path.GetFileName(slotPath)} (copy)");
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"Quest legacy migration failed: {ex.Message}");
        }
    }

    private static void LoadCompletedState()
    {
        try
        {
            string path = GetStateFilePath();
            var list = SafeStorage.LoadSafe<List<string>>(path, new List<string>(), Mod.Log);
            if (list == null) return;
            _completedQuests.Clear();
            foreach (var name in list)
            {
                if (!string.IsNullOrEmpty(name)) _completedQuests.Add(name);
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"Failed to load quest progress: {ex.Message}");
        }
    }

    // Fix 2026-09-10: called from Mod.OnSaveComplete alongside SaveStreetItems —
    // quest completions hit disk only when the game actually saves. Load point is
    // unchanged (LoadCompletedState via InitializeQuests on OnLoadComplete).
    internal static void FlushCompletedState()
    {
        try
        {
            var list = new List<string>(_completedQuests);
            SafeStorage.SaveAtomic(GetStateFilePath(), list, Mod.Log);
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"Failed to save quest progress: {ex.Message}");
        }
    }
}
