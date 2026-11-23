using System;
using Il2CppScheduleOne.Quests;
using MelonLoader;
using S1API.Lifecycle;
using S1Mods.Shared;
using UnityEngine;

[assembly: MelonInfo(typeof(PreserveRV.Mod), "PreserveRV", "0.1.0", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace PreserveRV;

public sealed class Mod : MelonMod
{
    internal static Mod? Instance;
    private readonly RvHooks _hooks = new();
    private readonly RvWorld _world = new();
    private string _status = "";
    private Quest_WelcomeToHylandPoint? _quest;
    private bool _loaded;
    private float _nextPoll;

    internal static void Log(string text) => MelonLogger.Msg("[PreserveRV] " + text);
    private void Status(string value)
    {
        if (_status == value) return;
        _status = value;
        Log(value);
    }

    public override void OnInitializeMelon()
    {
        Instance = this;
        _hooks.Install(); // Before any save restore / quest onComplete callback.
        GameLifecycle.OnLoadComplete += Loaded;
        GameLifecycle.OnPreLoad += Leaving;
        GameLifecycle.OnPreSceneChange += Leaving;
    }

    internal bool RunOriginal(Quest_WelcomeToHylandPoint quest)
    {
        if (!_hooks.Ready || !SessionAuthority.Supported()) return true;
        try
        {
            if (!_world.Prepare(quest))
            {
                // Without the original note "Read the note" could never be read again, so a
                // cosmetic preparation failure must not soft-lock the quest: run the native path.
                Status("ERROR original note unavailable; native RV destruction left enabled");
                return true;
            }
            // Native RV.BlowUp()/BlowupRV() return immediately when _exploded is already true
            // (f12 GameAssembly reads the bool at +0x289), so this also neutralises any caller
            // that is not hooked. IsDestroyed stays false, so the property keeps updating/saving.
            if (NetworkGuard.IsAlive(quest) && NetworkGuard.IsAlive(quest.RV) && !quest.RV._exploded)
                quest.RV._exploded = true;
            Status("original RV destruction blocked; original note prepared");
            return false; // Block: the note is safely reachable, so the RV may stay intact.
        }
        catch (Exception ex)
        {
            Status("ERROR note preparation failed; native RV destruction left enabled: " + ex.Message);
            return true;
        }
    }

    private void Loaded()
    {
        Leaving();
        _loaded = true;
    }

    private void Leaving()
    {
        _loaded = false;
        _quest = null;
        _nextPoll = 0;
        _status = "";
    }

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        if (sceneName == "Main") Leaving();
    }

    public override void OnUpdate()
    {
        if (!_loaded || !_hooks.Ready || Time.unscaledTime < _nextPoll) return;
        _nextPoll = Time.unscaledTime + 1f;
        if (!SessionAuthority.Supported()) return;
        try
        {
            if (!NetworkGuard.IsAlive(_quest)) _quest = FindQuest();
            if (_quest == null) return;
            if (_world.SavedAsDestroyed(_quest))
            {
                // Native state wins in a save that already lost the RV: read-only report.
                Status("save already destroyed; original contents not restored; RV left as saved");
                return;
            }
            // A reload rebuilds the scene from the prefab, so the note is back under the wreck.
            // Reparenting again is idempotent; visibility stays with the native SetActive calls.
            if (_world.Prepare(_quest))
                Status("original note restored to the RV root after load; native quest interaction unchanged");
            else
                Status("ERROR note hierarchy not understood on load; native behaviour stays in charge");
        }
        catch (Exception ex) { Status("ERROR load reconciliation failed: " + ex.Message); }
    }

    private static Quest_WelcomeToHylandPoint? FindQuest()
    {
        Quest_WelcomeToHylandPoint? found = null;
        var quests = Quest.Quests;
        if (quests == null) return null;
        foreach (var quest in quests)
        {
            if (!NetworkGuard.IsAlive(quest)) continue;
            var candidate = quest.TryCast<Quest_WelcomeToHylandPoint>();
            if (!NetworkGuard.IsAlive(candidate)) continue;
            if (found != null) return null;
            found = candidate;
        }
        return found;
    }

    public override void OnDeinitializeMelon()
    {
        GameLifecycle.OnLoadComplete -= Loaded;
        GameLifecycle.OnPreLoad -= Leaving;
        GameLifecycle.OnPreSceneChange -= Leaving;
        Leaving();
        _hooks.Remove();
        Instance = null;
    }
}
