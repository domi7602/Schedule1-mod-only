using System;
using System.Collections.Generic;

#if (IL2CPPMELON)
using S1NPC = Il2CppScheduleOne.NPCs.NPC;
using S1Dealer = Il2CppScheduleOne.Economy.Dealer;
#elif MONOMELON
using S1NPC = ScheduleOne.NPCs.NPC;
using S1Dealer = ScheduleOne.Economy.Dealer;
#endif

namespace HitmanPhone.Bounty;

/// <summary>
/// Selects bounty targets from the player's actual deal network.
///
/// A target is eligible only when it is assigned to a recruited dealer in
/// <c>Dealer.AllPlayerDealers</c>. The old implementation used a hard-coded list
/// of generic NPC IDs and could therefore select unrelated civilians.
/// </summary>
public static class TargetSelector
{
    /// <summary>
    /// NPC ids that are never valid Hitman targets, even if a broken/old save
    /// happens to expose one through a dealer assignment.
    /// </summary>
    private static readonly HashSet<string> ExcludedIds = new(StringComparer.OrdinalIgnoreCase)
    {
        // Dealers
        "benji_coleman", "brad_crosby", "jane_lucero", "leo_rivers", "molly_presley", "wei_long",
        // Suppliers
        "albert_hoover", "phil_wentworth", "salvador_moreno", "shirley_watts",
        // Specials
        "uncle_nelson", "igor_romanovich", "oscar_holland", "stan_carney",
        // Police officers
        "officer_bailey", "officer_cooper", "officer_green", "officer_howard",
        "officer_jackson", "officer_lee", "officer_lopez", "officer_murphy", "officer_oakley",
        // Mercedes (S1API test NPC id)
        "mercedes"
    };

    /// <summary>
    /// Picks a random living customer from the player's recruited dealers.
    /// Returns null when the player has no assigned customers.
    /// </summary>
    public static S1NPC? PickRandom()
    {
        var candidates = CollectOwnCustomers();
        if (candidates.Count == 0) return null;
        return candidates[UnityEngine.Random.Range(0, candidates.Count)];
    }

    /// <summary>
    /// Verifies that a target belongs to the player's current deal network.
    /// Used by the test/force-offer command as well as the normal selector so
    /// that the manual path cannot bypass the target restriction.
    /// </summary>
    public static bool IsOwnCustomer(S1NPC? target)
    {
        if (!IsEligible(target)) return false;

        string targetId;
        try
        {
            targetId = target!.ID;
        }
        catch
        {
            return false;
        }

        try
        {
            var dealers = S1Dealer.AllPlayerDealers;
            if (dealers == null) return false;

            for (int dealerIndex = 0; dealerIndex < dealers.Count; dealerIndex++)
            {
                var dealer = dealers[dealerIndex];
                if (dealer == null || !dealer.IsRecruited) continue;

                var assignedCustomers = dealer.AssignedCustomers;
                if (assignedCustomers == null) continue;

                for (int customerIndex = 0; customerIndex < assignedCustomers.Count; customerIndex++)
                {
                    var customerNpc = assignedCustomers[customerIndex]?.NPC;
                    if (customerNpc == null) continue;
                    if (string.Equals(customerNpc.ID, targetId, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"TargetSelector.IsOwnCustomer failed: {ex.Message}");
        }

        return false;
    }

    private static List<S1NPC> CollectOwnCustomers()
    {
        var candidates = new List<S1NPC>();
        var seenIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        try
        {
            var dealers = S1Dealer.AllPlayerDealers;
            if (dealers == null) return candidates;

            // IL2CPP collections must be indexed, not enumerated with foreach.
            for (int dealerIndex = 0; dealerIndex < dealers.Count; dealerIndex++)
            {
                var dealer = dealers[dealerIndex];
                if (dealer == null || !dealer.IsRecruited) continue;

                var assignedCustomers = dealer.AssignedCustomers;
                if (assignedCustomers == null) continue;

                for (int customerIndex = 0; customerIndex < assignedCustomers.Count; customerIndex++)
                {
                    var customerNpc = assignedCustomers[customerIndex]?.NPC;
                    if (!IsEligible(customerNpc)) continue;

                    string id = customerNpc!.ID;
                    if (seenIds.Add(id)) candidates.Add(customerNpc);
                }
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"TargetSelector.CollectOwnCustomers failed: {ex.Message}");
        }

        return candidates;
    }

    private static bool IsEligible(S1NPC? npc)
    {
        if (npc == null) return false;

        try
        {
            if (!npc.IsConscious) return false;
            if (string.IsNullOrEmpty(npc.ID)) return false;
            if (ExcludedIds.Contains(npc.ID)) return false;
            return true;
        }
        catch
        {
            // NPC may be destroyed while the assignment list is being read.
            return false;
        }
    }
}
