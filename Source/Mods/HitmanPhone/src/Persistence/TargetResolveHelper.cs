#if (IL2CPPMELON)
using S1NPC = Il2CppScheduleOne.NPCs.NPC;
using S1NPCManager = Il2CppScheduleOne.NPCs.NPCManager;
#elif MONOMELON
using S1NPC = ScheduleOne.NPCs.NPC;
using S1NPCManager = ScheduleOne.NPCs.NPCManager;
#endif

namespace HitmanPhone.Persistence;

/// <summary>
/// Phase D helper: Look up an NPC by its id, returning the typed native reference
/// or null. Wraps the static <see cref="S1NPCManager.GetNPC(string)"/> so callers
/// (e.g. force-fire path) don't need to know about <c>Il2CppScheduleOne.NPCs</c>.
/// </summary>
public static class TargetResolveHelper
{
    public static S1NPC? FindById(string id)
    {
        if (string.IsNullOrEmpty(id)) return null;
        try { return S1NPCManager.GetNPC(id); }
        catch { return null; }
    }
}
