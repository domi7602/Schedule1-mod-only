using System;

#if (IL2CPPMELON)
using S1StorageEntity = Il2CppScheduleOne.Storage.StorageEntity;
#elif MONOMELON
using S1StorageEntity = ScheduleOne.Storage.StorageEntity;
#endif

namespace HitmanPhone.Bounty;

/// <summary>
/// Phase F — Storage write hooks (v0.1.7 design).
///
/// History: v0.1.0–v0.1.5 hooked <c>StorageEntity.InsertItem</c>; the 0.1.5 live
/// test proved the dead-drop UI never calls it. v0.1.6 hooked
/// <c>StorageEntity.ContentsChanged()</c>; the 0.1.6 live test proved the game
/// never invokes it for dead drops either — the method exists (interop dump:
/// <c>ContentsChanged() : Void</c>), the patch applied cleanly (NPCDeathPatch
/// from the same PatchAll demonstrably ran), but the dead-drop UI writes slots
/// through the FishNet RPC chain
/// (<c>SetStoredInstance → SetStoredInstance_Internal</c> and
/// <c>SetItemSlotQuantity → SetItemSlotQuantity_Internal</c>) and never raises
/// ContentsChanged. ContentsChanged is only serviced by UpdateWhileOpen, which
/// runs solely for storages opened through the regular storage UI — dead drops
/// use their own UI and never call Open().
///
/// New design: hook the two actual write paths plus the old signal. Every slot
/// write in the game flows through SetStoredInstance_Internal (full instance
/// write) or SetItemSlotQuantity_Internal (quantity-only write), regardless of
/// which UI issued it. The three hooks are registered explicitly via
/// PatchGuard.TryPatch in Mod.cs (house standard), so a missing method is
/// logged at startup instead of being silently skipped.
///
/// Idempotency: scanning on each write is safe — a payout removes the contract
/// from Active, so repeated events cannot double-pay.
///
/// Skill Rule #3: every postfix body is wrapped in try/catch so a thrown
/// exception never destabilises the engine.
/// </summary>
internal static class DeadDropPatch
{
    public static void PostfixContentsChanged(S1StorageEntity __instance)
        => SafeScan(__instance, "ContentsChanged");

    public static void PostfixSetStoredInstance(S1StorageEntity __instance)
        => SafeScan(__instance, "SetStoredInstance_Internal");

    public static void PostfixSetItemSlotQuantity(S1StorageEntity __instance)
        => SafeScan(__instance, "SetItemSlotQuantity_Internal");

    private static void SafeScan(S1StorageEntity instance, string source)
    {
        try
        {
            BountyReceiptService.OnStorageContentsChanged(instance);
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"DeadDropPatch[{source}] swallowed exception: {ex.Message}");
        }
    }
}
