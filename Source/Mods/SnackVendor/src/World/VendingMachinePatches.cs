using System;
using HarmonyLib;
using MelonLoader;
using UnityEngine;
using S1Mods.Shared;
using Il2CppInterop.Runtime;
using Il2CppScheduleOne.Money;
using Il2CppScheduleOne.ObjectScripts;
using Il2CppScheduleOne.NPCs;
using SnackVendor.Config;

namespace SnackVendor.World;

/// <summary>
/// Harmony patches that intercept the vanilla purchase flow on VendingMachine
/// instances we own (those carrying the SnackVendorMarker). All other
/// instances — including every Cuke-Machine in vanilla — are untouched.
///
/// Hook points (verified via Vanilla RPC anatomy from the decompiled proxy):
///   - SendPurchase (server-side RPC): marker gate, our own purchase path
///     commits stock decrement + adds cash to player wallet.
///   - PurchaseRoutine / DropItem: this is where Vanilla would otherwise
///     spawn a Cuke-Pickup. We just block these for our instances so we
///     don't drop a cuke on the ground that the player can mistake for an
///     ingredient. We do NOT replace DropItem behaviour (no NPC pickup).
///   - Interacted: passthrough — Vanilla still shows the interaction prompt.
///
/// Note (Audit 0.0.3): the [HarmonyPatch(typeof(VendingMachine))] class-
/// attribute and the per-method [HarmonyPatch("…")] attributes are
/// intentionally absent. We register each patch explicitly through
/// PatchGuard.TryPatch in Mod.cs so the PatchGuard statistics cover them
/// (graceful-degradation on signature drift, visible failure-log on game
/// updates).
/// </summary>
public static class VendingMachinePatches
{
    public static bool SendPurchase_Prefix(VendingMachine __instance)
    {
        try
        {
            if (!IsOurs(__instance))
            {
                return true; // vanilla Cuke-machines untouched
            }

            // Host-Authority (Audit 0.0.3): the cash credit below must NEVER run
            // on a MP-client, otherwise host + client would each credit their own
            // wallet for the same NPC purchase. Refuse cleanly → vanilla drops
            // nothing (we own the marker, so our other prefixes already block
            // DropItem/DropCash anyway). Singleplayer is covered by the
            // NetworkManager-null branch in IsHostOrSingleplayer (returns true).
            if (!S1Mods.Shared.NetworkGuard.IsHostOrSingleplayer())
            {
                return false;
            }

            var controller = FindController(__instance);
            if (controller == null) return false; // no controller → refuse (no free cukes!)

            // Consume ONE stock slot at random; bail out if empty.
            int consumedId = controller.TryConsumeOne();
            if (consumedId < 0)
            {
                // No stock — refuse the purchase cleanly. Original would
                // spawn the Cuke in this case; we cut the branch to keep
                // "no stock = no cuke" the actual gameplay rule.
                return false;
            }

            // Price = vanilla market price for the ingredient, clamped.
            float price = controller.ResolveIngredientPrice(consumedId);
            if (price < 0f) price = 1f;
            var cfg = ModConfig<SnackVendorConfig>.Instance;
            if (cfg.ClampPrice)
            {
                price = Mathf.Clamp(price, cfg.MinPayoutPerItem, cfg.MaxPayoutPerItem);
            }

            // CASH CREDIT (host-gated above). EconomyHelper routes through the
            // S1API Money wrapper — the same payout path HitmanPhone uses,
            // MP-verified since v0.1.9 (syncs the balance over FishNet).
            // In-game verify for the vending context pending (spike gate).
            S1Mods.Shared.EconomyHelper.ChangeCashBalance(price, true, false);

            Mod.Log.Info($"[SnackVendor] purchase: ingredient #{consumedId} for ${price:0.00} (cash credited).");

            // TODO: route ingredient into the NPC's inventory. NPC entity is
            // not in scope here in the vanilla prefix (SendPurchase only sees
            // the VendingMachine), so we tuck it for the post-purchase hook
            // (right now the vanilla DropItem path is BLOCKED for our marker
            // so the item would be lost). Future work: hook the WalkCallback
            // of NPCSignal_UseVendingMachine to capture the npc reference and
            // transfer from there.

            // Persist new stock state immediately so a host crash mid-purchase
            // does not leave us with phantom items already debited from the
            // visual count.
            try { controller.PersistSlotsToDisk(); }
            catch (System.Exception ex) { Mod.Log.Warn("post-purchase persist failed", ex); }

            // Block the vanilla cuke-spawn and the vanilla (uncompensated)
            // cash-prefab path.
            return false;
        }
        catch (System.Exception ex)
        {
            Mod.Log.Error("SendPurchase_Prefix", ex);
            return true; // on unexpected error, fall back to vanilla so we don't freeze the station
        }
    }

    [HarmonyPrefix]
    public static bool PurchaseRoutine_Prefix(VendingMachine __instance)
    {
        try
        {
            if (!IsOurs(__instance)) return true; // vanilla untouched
            // We already did the work (stock + cash) in SendPurchase_Prefix;
            // suppress the vanilla routine that would play the cuke-dispense anim.
            return false;
        }
        catch
        {
            return true;
        }
    }

    [HarmonyPrefix]
    public static bool DropItem_Prefix(VendingMachine __instance)
    {
        // Guarantee no cuke appears on the ground in front of our marker.
        try { return !IsOurs(__instance); }
        catch { return true; }
    }

    [HarmonyPrefix]
    public static bool DropCash_Prefix(VendingMachine __instance)
    {
        // Suppress the vanilla "broken-machine spits bills" cash-prefab; we
        // credit directly via AddCash above.
        try { return !IsOurs(__instance); }
        catch { return true; }
    }

    // ----- helpers -----

    private static bool IsOurs(VendingMachine machine)
    {
        try
        {
            if (machine == null || machine.Pointer == IntPtr.Zero || machine.gameObject == null) return false;
            var go = machine.gameObject;
            // Check the GameObject and its parents for our marker (cheap, runs per RPC).
            return go.GetComponent<SnackVendorMarker>() != null
                || go.GetComponentInParent<SnackVendorMarker>() != null;
        }
        catch { return false; }
    }

    private static SnackVendorController? FindController(VendingMachine machine)
    {
        try
        {
            var go = machine.gameObject;
            // The controller sits on the parent buildable item (which is what
            // BuildableItem.Start-Postfix attaches the controller to).
            var ctrl = go.GetComponentInParent<SnackVendorController>();
            return ctrl;
        }
        catch { return null; }
    }
}
