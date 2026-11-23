using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Il2CppScheduleOne.Quests;
using S1Mods.Shared;

namespace PreserveRV;

/// <summary>No Harmony attributes: MelonLoader's automatic PatchAll must not own these patches.</summary>
internal sealed class RvHooks
{
    private const string Owner = "Dominik.PreserveRV";
    private readonly HarmonyLib.Harmony _harmony = new(Owner);
    private readonly List<MethodBase> _targets = new();
    internal bool Ready { get; private set; }
    internal int Installed => _targets.Count(t => HarmonyLib.Harmony.GetPatchInfo(t)?.Owners.Contains(Owner) == true);

    internal void Install()
    {
        if (Ready) return;
        try
        {
            foreach (string name in new[] { "BlowupRV", "SetRVDestroyed" })
            {
                var target = typeof(Quest_WelcomeToHylandPoint).GetMethod(name,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly, null, Type.EmptyTypes, null)
                    ?? throw new MissingMethodException(typeof(Quest_WelcomeToHylandPoint).FullName, name);
                if (target.ReturnType != typeof(void)) throw new InvalidOperationException("Changed return signature: " + target);
                if (!_targets.Contains(target)) _targets.Add(target);
                var prefix = new HarmonyMethod(typeof(RvHooks), nameof(Prefix));
                if (HarmonyLib.Harmony.GetPatchInfo(target)?.Owners.Contains(Owner) != true &&
                    !PatchGuard.TryPatch(_harmony, target, prefix: prefix))
                    throw new InvalidOperationException("PatchGuard refused: " + target);
                var own = HarmonyLib.Harmony.GetPatchInfo(target)?.Prefixes.Where(p => p.owner == Owner).ToArray();
                if (own == null || own.Length != 1 || own[0].PatchMethod != typeof(RvHooks).GetMethod(nameof(Prefix), BindingFlags.Static | BindingFlags.NonPublic))
                    throw new InvalidOperationException("Missing/wrong owner prefix metadata: " + target);
            }
            Ready = _targets.Count == 2 && Installed == 2;
            if (!Ready) throw new InvalidOperationException("Required hook group incomplete");
            Mod.Log("hooks registered=2/2; invocation/gameplay not yet verified");
        }
        catch (Exception ex)
        {
            Mod.Log("ERROR hooks unavailable: " + ex.Message);
            Remove();
        }
    }

    internal void Remove()
    {
        Ready = false;
        try { _harmony.UnpatchSelf(); }
        catch (Exception ex) { Mod.Log("ERROR hook rollback: " + ex.Message); }
        int remaining = Installed;
        Mod.Log(remaining == 0 ? "hooks removed; remaining=0" : $"ERROR owner registrations remain={remaining}");
    }

    private static bool Prefix(Quest_WelcomeToHylandPoint __instance)
        => Mod.Instance?.RunOriginal(__instance) ?? true;
}
