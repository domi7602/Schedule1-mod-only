using Il2CppScheduleOne.Quests;
using S1Mods.Shared;
using UnityEngine;

namespace PreserveRV;

/// <summary>
/// Borrow the original note; never replace its interaction, its activation or the quest events.
/// The note is authored inactive beneath the inactive "Destroyed RV" wreck. The native
/// Investigate/Read onStart calls SetActive on it, so once it hangs under the active property
/// root its own activeSelf keeps deciding visibility - quest state is not re-implemented here.
/// </summary>
internal sealed class RvWorld
{
    internal const string NoteName = "CartelNote";
    internal const string WreckName = "Destroyed RV";

    /// <summary>Idempotent: moves the original note out of the wreck once, or reports failure.</summary>
    internal bool Prepare(Quest_WelcomeToHylandPoint quest)
    {
        if (!NetworkGuard.IsAlive(quest) || !NetworkGuard.IsAlive(quest.RV)) return false;
        var root = quest.RV.transform;
        if (!NetworkGuard.IsAlive(root)) return false;

        if (!FindChild(root, NoteName, out var placed)) return false; // Ambiguous names: refuse.
        if (placed != null)
        {
            // Already prepared. Honour that even if native code later removed the wreck; only a
            // second note still sitting in the wreck makes the state ambiguous again.
            if (!FindChild(root, WreckName, out var known)) return false;
            if (known == null) return true;
            if (!FindChild(known, NoteName, out var leftover)) return false;
            return leftover == null;
        }

        if (!FindChild(root, WreckName, out var wreck) || wreck == null) return false;
        if (!FindChild(wreck, NoteName, out var nested) || nested == null) return false;

        // Preserve world pose and the note's own activeSelf; native SetActive calls stay in charge.
        bool active = nested.gameObject.activeSelf;
        nested.SetParent(root, true);
        if (nested.gameObject.activeSelf != active) nested.gameObject.SetActive(active);
        return true;
    }

    internal bool SavedAsDestroyed(Quest_WelcomeToHylandPoint quest)
        => NetworkGuard.IsAlive(quest) && NetworkGuard.IsAlive(quest.RV) && quest.RV.IsDestroyed;

    private static bool FindChild(Transform root, string name, out Transform? result)
    {
        result = null;
        for (int i = 0; i < root.childCount; i++)
        {
            var child = root.GetChild(i);
            if (child.name != name) continue;
            if (!NetworkGuard.IsAlive(child) || result != null) return false;
            result = child;
        }
        return true;
    }
}
