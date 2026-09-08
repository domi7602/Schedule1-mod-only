using System;
using System.Reflection;

#if (IL2CPPMELON)
using S1StorageEntity = Il2CppScheduleOne.Storage.StorageEntity;
using S1DeadDrop = Il2CppScheduleOne.Economy.DeadDrop;
using S1ItemInstance = Il2CppScheduleOne.ItemFramework.ItemInstance;
#elif MONOMELON
using S1StorageEntity = ScheduleOne.Storage.StorageEntity;
using S1DeadDrop = ScheduleOne.Economy.DeadDrop;
using S1ItemInstance = ScheduleOne.ItemFramework.ItemInstance;
#endif

namespace HitmanPhone.Bounty;

/// <summary>
/// Phase F — Receipt Identifier.
///
/// BUGFIX (2026-08-31, v0.1.4): the previous check walked the inheritance chain
/// assuming <c>Economy.DeadDrop</c> derives from <c>StorageEntity</c>. The
/// decompiled stub proves the opposite (Archive/Temp_DeadDrop.cs line 16:
/// <c>DeadDrop : MonoBehaviour</c>) — the drop OWNS a <c>WorldStorageEntity</c>
/// in its <c>Storage</c> field. The inheritance test therefore returned false
/// for every object in the game and the payout chain never ran.
///
/// Correct semantics: a storage entity belongs to a dead drop when one of the
/// registered <see cref="S1DeadDrop.DeadDrops"/> holds exactly that entity.
/// </summary>
internal static class DeadDropIdentifier
{
    /// <summary>
    /// Returns the registered DeadDrop that owns this storage entity, or null.
    /// Comparison via Unity instance id (stable within a session, unlike the
    /// Il2Cpp wrapper references which may differ per access).
    /// </summary>
    public static S1DeadDrop? FindOwningDeadDrop(S1StorageEntity entity)
    {
        if (entity == null) return null;
        try
        {
            var drops = S1DeadDrop.DeadDrops;
            if (drops == null) return null;
            int instanceId = entity.GetInstanceID();
            for (int i = 0; i < drops.Count; i++)
            {
                var dd = drops[i];
                if (dd == null) continue;
                var st = dd.Storage;
                if (st == null) continue;
                if (st.GetInstanceID() == instanceId) return dd;
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"FindOwningDeadDrop failed: {ex.Message}");
        }
        return null;
    }

    /// <summary>True if the given storage entity belongs to any registered dead drop.</summary>
    public static bool IsDeadDropStorage(S1StorageEntity entity) => FindOwningDeadDrop(entity) != null;

    /// <summary>
    /// The dead drop's GUID. Lives on the <see cref="S1DeadDrop"/> component itself,
    /// NOT on the storage entity (the old entity-based reflection lookup could
    /// never find it for the same reason as the inheritance bug).
    /// </summary>
    public static string GetGuidString(S1DeadDrop drop)
    {
        if (drop == null) return string.Empty;
        try
        {
            return drop.GUID.ToString();
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"GetGuidString failed: {ex.Message}");
            return string.Empty;
        }
    }
}
