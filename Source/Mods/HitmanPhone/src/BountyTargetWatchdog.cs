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
                // If the NPC is loaded and has been knocked out or killed
                if (npc != null && !npc.IsConscious)
                {
                    // Trigger the existing polaroid logic
                    BountyService.OnNpcDied(npc);
                }
            }
            catch { }
        }
    }
}
