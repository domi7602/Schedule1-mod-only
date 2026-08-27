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

    public static void InitializeQuests()
    {
        if (!Mod.CurrentConfig.EnableHomelessQuests) return;
        if (_initialized) return;

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
                    QuestManager.CreateQuest<Quest_ColdConcrete>("homeless_quest_1");
                    Mod.Log.Info("Registered new 'Cold Concrete' quest with S1API.");
                }
            }

            _initialized = true;
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"Failed to initialize homeless quests: {ex.Message}");
        }
    }

    public static void NotifyItemPlaced(string itemId)
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
            var q2 = ResolveQuest2();
            if (q2 != null)
            {
                var gear = FindEntry(q2, "Place a workstation or grow container outdoors");
                if (gear != null && gear.State != QuestState.Completed)
                {
                    gear.Complete();
                    FindEntry(q2, "Accumulate $500 in cash")?.Begin();
                    Mod.Log.Info("Completed: 'Place a workstation or grow container outdoors'!");
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
                        MarkQuestCompleted("Alley Operations");

                        // Advance to Quest 3
                        GetOrCreateQuest3();
                    }
                }
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
                        MarkQuestCompleted("Street Sovereign");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Debug($"CheckCashProgress quest error: {ex.Message}");
        }
    }

    public static void ResetState()
    {
        _initialized = false;
        _completedQuests.Clear();
    }

    // ----- Fresh lookup helpers (save-load safe) -----

    private static Quest_ColdConcrete? ResolveQuest1() => QuestManager.GetQuestByName("Cold Concrete") as Quest_ColdConcrete;

    private static Quest_AlleyOperations? ResolveQuest2() => QuestManager.GetQuestByName("Alley Operations") as Quest_AlleyOperations;

    private static Quest_StreetSovereign? ResolveQuest3() => QuestManager.GetQuestByName("Street Sovereign") as Quest_StreetSovereign;

    private static Quest_AlleyOperations GetOrCreateQuest2()
        => ResolveQuest2() ?? (Quest_AlleyOperations)QuestManager.CreateQuest<Quest_AlleyOperations>("homeless_quest_2");

    private static Quest_StreetSovereign GetOrCreateQuest3()
        => ResolveQuest3() ?? (Quest_StreetSovereign)QuestManager.CreateQuest<Quest_StreetSovereign>("homeless_quest_3");

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
        if (_completedQuests.Add(questName))
        {
            SaveCompletedState();
        }
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

    private static void SaveCompletedState()
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
