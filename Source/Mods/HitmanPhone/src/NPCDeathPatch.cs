using System;
using System.Reflection;
using HarmonyLib;
using S1Mods.Shared;

#if (IL2CPPMELON)
using S1NPC = Il2CppScheduleOne.NPCs.NPC;
#elif MONOMELON
using S1NPC = ScheduleOne.NPCs.NPC;
#endif

namespace HitmanPhone.Bounty;

/// <summary>
/// Phase C — Harmony-Postfix on <see cref="S1NPC"/>.OnDie().
///
/// We do not throw inside the patch. If anything goes wrong (game version drift,
/// nulled instance, save corruption) PatchGuard logs and swallows the exception
/// so the game remains stable — Skill Rule #3 ("Never let a Harmony patch throw").
///
/// Actual NPC.OnDie signature (from stub decompile):
///   protected virtual void OnDie();
/// We patch with <c>HarmonyPrefix</c> semantics (Harmony actually routes any
/// declared Patch type — Prefix/Postfix/Finalizer — to the corresponding slot).
/// We use Postfix here so we don't disturb the engine's death logic.
/// </summary>
[HarmonyPatch]
internal static class NPCDeathPatch
{
    /// <summary>
    /// Harmony requires static <c>TargetMethod()</c> returning the MethodInfo
    /// to patch because the stub decompile hides the runtime signature. We find
    /// OnDie via reflection (parameterless instance method on S1NPC).
    /// </summary>
    private static MethodBase? TargetMethod()
    {
        try
        {
            return typeof(S1NPC).GetMethod(
                "OnDie",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                binder: null,
                types: Type.EmptyTypes,
                modifiers: null);
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"NPCDeathPatch.TargetMethod failed: {ex}");
            return null;
        }
    }

    /// <summary>
    /// Postfix runs after the engine's OnDie. We do our work in BountyService.OnNpcDied
    /// so the patch body itself stays thin and PatchGuard-friendly.
    /// Audit (2026-09-10): internal (was private) so Mod.cs can bind it via an
    /// explicit PatchGuard.TryPatch HarmonyMethod instead of blind PatchAll.
    /// </summary>
    [HarmonyPostfix]
    internal static void Postfix(S1NPC __instance)
    {
        try
        {
            BountyService.OnNpcDied(__instance);
        }
        catch (Exception ex)
        {
            // Skill Rule #3: never let a Harmony patch throw. Log it and continue.
            Mod.Log.Error($"NPCDeathPatch.Postfix swallowed exception: {ex}");
        }
    }
}
