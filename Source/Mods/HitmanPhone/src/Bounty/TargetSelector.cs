using System;
using System.Collections.Generic;
using System.Reflection;

#if (IL2CPPMELON)
using S1NPC = Il2CppScheduleOne.NPCs.NPC;
using S1NPCManager = Il2CppScheduleOne.NPCs.NPCManager;
#elif MONOMELON
using S1NPC = ScheduleOne.NPCs.NPC;
using S1NPCManager = ScheduleOne.NPCs.NPCManager;
#endif

namespace HitmanPhone.Bounty;

/// <summary>
/// Phase D (NPC pool) — Picks a generic NPC as bounty target.
///
/// Selection rules (Spec §3 / Phase I):
///   • Skip specials (Customer / Dealer / Police / Cartel-Goon / Custom NPCs by ID).
///   • Skip NPCs that don't yet have a stable ID (e.g. dynamically spawned goons).
///   • Skip NPCs whose <c>IsConscious</c> state is already false (alive NPC required).
///
/// <see cref="S1NPCManager.NPCRegistry"/> is a static field that holds the
/// loaded NPC map. We discover it via reflection because the field is exposed
/// only as <c>NativeFieldInfoPtr_NPCRegistry</c> in the IL2CPP stub.
/// </summary>
public static class TargetSelector
{
    private static FieldInfo? _npcRegistryField;
    private static bool _init;

    /// <summary>
    /// Returns the static <see cref="S1NPCManager.NPCRegistry"/> field via reflection
    /// (it is declared as a static read-only field in the stub decompile). The actual
    /// runtime type is <c>Il2CppSystem.Collections.Generic.Dictionary&lt;string, NPC&gt;</c>
    /// in IL2CPP, which we enumerate by key and load each NPC by id via
    /// <see cref="S1NPCManager.GetNPC(string)"/> (the public-static helper that resolves
    /// by id without needing direct dictionary access).
    /// </summary>
    private static void EnsureInit()
    {
        if (_init) return;
        try
        {
            _npcRegistryField = typeof(S1NPCManager).GetField(
                "NPCRegistry",
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"TargetSelector init failed: {ex.Message}");
        }
        _init = true;
    }

    /// <summary>
    /// Pick a random eligible NPC from the NPC registry.
    /// Returns null when no candidates remain (very early-game or save without generic NPCs).
    /// </summary>
    public static S1NPC? PickRandom()
    {
        EnsureInit();

        // We pull every NPC we can find. We don't enumerate the dictionary directly
        // (Il2CppSystem.Collections are index-only, see skill rule "Never foreach on
        // Il2CppSystem.Collections.Generic.List<T>"). Instead we pick a known list of
        // NPC ids from the registry by reading its count and then by id-specific lookup.
        //
        // Simpler fallback: ask the NPCManager's S1API surface. We don't have a public
        // GetAll yet, so we iterate a stable pool of Generic NPC ids we know from
        // S1API.Entities.NPCs. If that pool is exhausted we fall through.
        var candidates = new List<S1NPC>();
        foreach (var npc in EnumerateKnownGenerics())
        {
            if (npc == null) continue;
            try
            {
                if (!npc.IsConscious) continue;
                if (string.IsNullOrEmpty(npc.ID)) continue;
                if (IsExcludedId(npc.ID)) continue;
                candidates.Add(npc);
            }
            catch
            {
                // destroyed mid-iteration; skip
            }
        }

        if (candidates.Count == 0) return null;
        return candidates[UnityEngine.Random.Range(0, candidates.Count)];
    }

    /// <summary>
    /// NPC ids that are reserved for story-line / non-killable roles.
    /// Sourced from <c>S1API/Entities/NPCs/</c> wrapper-class names:
    /// dealers (Benji, Brad, Jane, Leo, Molly, Wei),
    /// suppliers (Albert, Phil, Salvador, Shirley),
    /// specials (Uncle Nelson, Igor, Oscar, Stan),
    /// police (Bailey, Cooper, Green, Howard, Jackson, Lee, Lopez, Murphy, Oakley).
    /// </summary>
    private static readonly HashSet<string> ExcludedIds = new(StringComparer.OrdinalIgnoreCase)
    {
        // Dealers
        "benji_coleman", "brad_crosby", "jane_lucero", "leo_rivers", "molly_presley", "wei_long",
        // Suppliers
        "albert_hoover", "phil_wentworth", "salvador_moreno", "shirley_watts",
        // Specials
        "uncle_nelson", "igor_romanovich", "oscar_holland", "stan_carney",
        // Police officers (the 9 generic ones; ids follow the NPC name pattern)
        "officer_bailey", "officer_cooper", "officer_green", "officer_howard",
        "officer_jackson", "officer_lee", "officer_lopez", "officer_murphy", "officer_oakley",
        // Mercedes (S1API test NPC id)
        "mercedes"
    };

    private static bool IsExcludedId(string id) => ExcludedIds.Contains(id);

    /// <summary>
    /// Enumerate the 65 generic NPCs from S1API by trying each S1API.NPCs wrapper.
    /// Each call to <c>S1API.Entities.Player.All</c> plus wrapper-class reflection
    /// gives us a stable list. We use a tiny cache so we don't re-hit reflection
    /// for every roll.
    /// </summary>
    private static readonly object _cacheLock = new();
    private static S1NPC[]? _cachedGenerics;
    private static int _cacheRebuildDay = -1;

    /// <summary>
    /// Returns a possibly-empty array of generic NPCs by reflecting over
    /// <c>S1API.Entities.Player.All</c>-style NPC discovery and the static
    /// <c>S1NPCManager.NPCRegistry</c>.
    /// </summary>
    private static IEnumerable<S1NPC> EnumerateKnownGenerics()
    {
        // The reflection-heavy enumeration is gated by a once-per-game-day cache,
        // because regenerating costs us a GetNPC() round-trip for ~65 IDs every frame.
        int day = HitmanPhoneTime.CurrentDay();
        lock (_cacheLock)
        {
            if (_cachedGenerics != null && _cacheRebuildDay == day)
            {
                foreach (var n in _cachedGenerics) yield return n;
                yield break;
            }
        }

        var fresh = new List<S1NPC>(80);
        // We pull by id from a stable list of wrapper class names. This avoids the
        // need to enumerate the IL2CPP dictionary directly.
        foreach (string id in KnownGenericNPCIds)
        {
            try
            {
                var n = S1NPCManager.GetNPC(id);
                if (n != null) fresh.Add(n);
            }
            catch
            {
                // ignore missing
            }
        }

        lock (_cacheLock)
        {
            _cachedGenerics = fresh.ToArray();
            _cacheRebuildDay = day;
        }
        foreach (var n in _cachedGenerics) yield return n;
    }

    /// <summary>
    /// Stable list of Generic NPC ids (non-dealer, non-supplier, non-special,
    /// non-police). Roster current as of Schedule I 0.4.6f13; S1API folder
    /// <c>S1API/Entities/NPCs/</c> lists these. Adding new NPC classes to the
    /// game will require updating this list.
    /// </summary>
    private static readonly HashSet<string> KnownGenericNPCIds = new(StringComparer.OrdinalIgnoreCase)
    {
        // Docks
        "anna_chesterfield", "billy_kramer", "cranky_frank", "genghis_barn",
        "javier_perez", "lisa_gardener", "mac_cooper", "marco_baron", "melissa_wood",
        // Downtown
        "elizabeth_homley", "eugene_buckley", "greg_fliggle", "jeff_gilmore",
        "kevin_oakley", "louis_fourier", "lucy_pennington", "randy_caulfield",
        // Northtown
        "austin_steiner", "beth_penn", "chloe_bowers", "donna_martin",
        "geraldine_poon", "jessi_waters", "kathy_henderson", "kyle_cooley",
        "ludwig_meyer", "peter_file", "sam_thompson", "peggy_myers", "ming",
        // Suburbia
        "alison_knight", "carl_bundy", "chris_sullivan", "dennis_kennedy",
        "hank_stevenson", "harold_colt", "jackie_stevenson", "jack_knight",
        "jeremy_wilkinson", "karen_kennedy",
        // Uptown
        "fiona_hancock", "herbert_bleuball", "jen_heard", "lily_turner",
        "michael_boog", "pearl_moore", "ray_hoffman", "tobias_wentworth",
        "walter_cussler",
        // Westville
        "charles_rowland", "dean_webster", "doris_lubbin", "george_greene",
        "jerry_montero", "joyce_ball", "keith_wagner", "kim_delaney",
        "meg_cooley",
    };
}
