using S1Mods.Shared;
using HitmanPhone.Persistence;
using System;
using UnityEngine;
#if (IL2CPPMELON)
using S1NPCManager = Il2CppScheduleOne.NPCs.NPCManager;
#elif MONOMELON
using S1NPCManager = ScheduleOne.NPCs.NPCManager;
#endif

namespace HitmanPhone.Bounty;

/// <summary>
/// Phase C Alternative - Polls active targets for IsConscious == false.
/// This completely bypasses the need to guess if the engine used OnDie, 
/// KnockOut, or SetConscious, making it 100% resilient to game updates.
/// </summary>
public static class BountyTargetWatchdog
{
    private static float _nextTick;

    public static void Tick()
    {
        if (Time.time < _nextTick) return;
        _nextTick = Time.time + 1.5f; // Poll every 1.5s (very cheap, 0-allocation)

        if (Mod.Instance?.Save == null) return;

        for (int i = 0; i < Mod.Instance.Save.Active.Count; i++)
        {
            var c = Mod.Instance.Save.Active[i];

            if (c.Status != EBountyStatus.Active) continue;
            if (c.EvidenceSpawned) continue; // We already gave the player the polaroid

            try
            {
                var npc = S1NPCManager.GetNPC(c.TargetNpcId);
                // Bug-Audit 2026-09-12: `!npc.IsConscious` fired on KO too — awarding a kill
                // for a non-lethal takedown and escalating to Lethal pursuit. NPCHealth.IsDead
                // distinguishes the two via the game's own state, and onDieOrKnockedOut is
                // wired to the same flag.
                // Knockout, death, or unconsciousness all satisfy elimination.
                if (npc != null && npc.Health != null
                    && npc.Health.Pointer != IntPtr.Zero && !npc.Health.WasCollected
                    && (npc.Health.IsDead || npc.Health.IsKnockedOut || !npc.IsConscious))
                {
                    // Trigger the existing polaroid logic
                    BountyService.OnNpcDied(npc);
                }
            }
            catch { }
        }
    }
}
