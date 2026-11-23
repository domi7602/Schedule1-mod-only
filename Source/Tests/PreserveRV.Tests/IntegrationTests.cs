using HarmonyLib;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Networking;
using Il2CppScheduleOne.PlayerScripts;
using S1Mods.Shared;
using Xunit;

namespace PreserveRV.Tests;

public class IntegrationTests
{
    internal static void Reset()
    {
        Harmony.Registry.Clear(); Harmony.MissingMetadataFor = null; Harmony.ThrowOnRemove = false;
        PatchGuard.Calls = 0;
        NetworkGuard.Main = NetworkGuard.Host = true; NetworkGuard.Throw = false;
        Singleton<Lobby>.Instance = new(); Player.PlayerList = new();
        MelonLoader.MelonLogger.Lines.Clear();
    }

    [Fact]
    public void Terminal_saved_destruction_is_reported_once_and_never_polled_into_resurrection()
    {
        Reset();
        var quest = RvWorldTests.Fixture();
        quest.RV.IsDestroyed = true;
        quest.RV.transform.Children[0].gameObject.activeSelf = false;
        quest.RV.transform.Children[1].gameObject.activeSelf = true;
        quest.Entries[0].State = Il2CppScheduleOne.Quests.EQuestState.Completed;
        quest.Entries[1].State = Il2CppScheduleOne.Quests.EQuestState.Completed;
        Il2CppScheduleOne.Quests.Quest.Quests = new() { quest };
        var mod = new Mod(); mod.OnInitializeMelon();
        try
        {
            S1API.Lifecycle.GameLifecycle.Loaded();
            UnityEngine.Time.unscaledTime = 0; mod.OnUpdate();
            UnityEngine.Time.unscaledTime = 1; mod.OnUpdate();
            UnityEngine.Time.unscaledTime = 2; mod.OnUpdate();
            Assert.True(quest.RV.IsDestroyed);
            Assert.True(quest.RV.transform.Children[1].gameObject.activeSelf);
            Assert.Equal(1, MelonLoader.MelonLogger.Lines.Count(l => l.Contains("already destroyed; original contents not restored")));
        }
        finally { mod.OnDeinitializeMelon(); }
    }

    [Fact]
    public void Refused_patch_rolls_back_and_leaves_native_destruction_enabled()
    {
        Reset();
        PatchGuard.Refuse = true;
        var mod = new Mod(); mod.OnInitializeMelon();
        try
        {
            Assert.DoesNotContain(Harmony.Registry.Values, p => p.Owners.Contains("Dominik.PreserveRV"));
            Assert.Contains(MelonLoader.MelonLogger.Lines, l => l.Contains("ERROR hooks unavailable"));
            Assert.True(mod.RunOriginal(RvWorldTests.Fixture()));
        }
        finally { PatchGuard.Refuse = false; mod.OnDeinitializeMelon(); }
    }

    [Fact]
    public void Missing_note_leaves_native_destruction_enabled_instead_of_soft_locking_the_quest()
    {
        Reset();
        var mod = new Mod(); mod.OnInitializeMelon();
        try
        {
            var quest = RvWorldTests.Fixture();
            quest.RV.transform.Children[1].Children[0].name = "something else";
            // A cosmetic preparation failure must not cost the player the rest of the quest.
            Assert.True(Harmony.RunPrefix("BlowupRV", quest));
            Assert.False(quest.RV._exploded);
            Assert.False(quest.RV.IsDestroyed);
            Assert.Contains(MelonLoader.MelonLogger.Lines, l => l.Contains("native RV destruction left enabled"));
            Assert.DoesNotContain(MelonLoader.MelonLogger.Lines, l => l.Contains("original note prepared"));
        }
        finally { mod.OnDeinitializeMelon(); }
    }

    [Fact]
    public void Failed_hook_metadata_leaves_original_behaviour_and_reports_missing_owner()
    {
        Reset();
        Harmony.MissingMetadataFor = "BlowupRV";
        var mod = new Mod(); mod.OnInitializeMelon();
        try
        {
            Assert.DoesNotContain(MelonLoader.MelonLogger.Lines, l => l.Contains("hooks registered"));
            Assert.Contains(MelonLoader.MelonLogger.Lines, l => l.Contains("ERROR hooks unavailable"));
            Assert.True(mod.RunOriginal(RvWorldTests.Fixture())); // Incomplete hook set never pretends readiness.
        }
        finally { Harmony.MissingMetadataFor = null; mod.OnDeinitializeMelon(); }
    }

    [Fact]
    public void Reload_reparents_the_note_again_and_clears_the_cached_quest()
    {
        Reset();
        var quest = RvWorldTests.Fixture();
        Il2CppScheduleOne.Quests.Quest.Quests = new() { quest };
        var note = quest.RV.transform.Children[1].Children[0];
        var mod = new Mod(); mod.OnInitializeMelon();
        try
        {
            S1API.Lifecycle.GameLifecycle.Loaded();
            UnityEngine.Time.unscaledTime = 0; mod.OnUpdate();
            Assert.Same(quest.RV.transform, note.parent);
            Assert.Equal(1, note.ParentWrites);
            // A reload rebuilds the prefab: the note is back under the wreck and must move again.
            S1API.Lifecycle.GameLifecycle.PreLoad();
            var replacement = RvWorldTests.Fixture();
            Il2CppScheduleOne.Quests.Quest.Quests = new() { replacement };
            var replacementNote = replacement.RV.transform.Children[1].Children[0];
            S1API.Lifecycle.GameLifecycle.Loaded(); mod.OnUpdate();
            Assert.Same(replacement.RV.transform, replacementNote.parent);
            Assert.Equal(1, replacementNote.ParentWrites);
            Assert.Equal(1, note.ParentWrites); // the stale instance is no longer touched
        }
        finally { mod.OnDeinitializeMelon(); }
    }

    [Fact]
    public void Blocked_explosion_marks_native_reentry_guard_without_destroying_the_rv()
    {
        Reset();
        var mod = new Mod(); mod.OnInitializeMelon();
        try
        {
            var quest = RvWorldTests.Fixture();
            Assert.False(quest.RV._exploded);
            Assert.False(Harmony.RunPrefix("BlowupRV", quest));
            // Native RV.BlowUp returns immediately when _exploded is set (f12 GameAssembly +0x289),
            // so any other caller is neutralised too - but the property must stay intact.
            Assert.True(quest.RV._exploded);
            Assert.False(quest.RV.IsDestroyed);
            // Outside a supported session the native path stays untouched.
            NetworkGuard.Host = false;
            var other = RvWorldTests.Fixture();
            Assert.True(Harmony.RunPrefix("BlowupRV", other));
            Assert.False(other.RV._exploded);
            Assert.False(other.RV.IsDestroyed);
        }
        finally { mod.OnDeinitializeMelon(); }
    }

    [Fact]
    public void Initialize_installs_exact_quest_hooks_before_load_and_prefix_preserves_original_note()
    {
        Reset();
        var mod = new Mod();
        mod.OnInitializeMelon();
        try
        {
            Assert.Equal(new[] { "BlowupRV", "SetRVDestroyed" }, Harmony.Registry.Where(p => p.Value.Owners.Contains("Dominik.PreserveRV")).Select(p => p.Key.Name).OrderBy(x => x));
            Assert.Equal(2, PatchGuard.Calls);
            var quest = RvWorldTests.Fixture();
            var root = quest.RV.transform;
            var note = root.Children[1].Children[0];
            Assert.False(Harmony.RunPrefix("SetRVDestroyed", quest));
            Assert.False(Harmony.RunPrefix("BlowupRV", quest));
            Assert.Same(root, note.parent);
            Assert.Equal(1, note.ParentWrites);
            Assert.False(quest.RV.IsDestroyed);
            Assert.False(note.gameObject.activeSelf);
            Player.PlayerList!.Add(new()); Player.PlayerList.Add(new());
            Assert.True(Harmony.RunPrefix("BlowupRV", RvWorldTests.Fixture()));
        }
        finally { mod.OnDeinitializeMelon(); }
        Assert.DoesNotContain(Harmony.Registry.Values, p => p.Owners.Contains("Dominik.PreserveRV"));
    }
}
