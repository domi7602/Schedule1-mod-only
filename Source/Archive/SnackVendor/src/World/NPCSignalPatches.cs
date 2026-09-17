using System;
using System.Collections.Generic;
using HarmonyLib;
using Il2CppScheduleOne.NPCs;
using Il2CppScheduleOne.NPCs.Schedules;
using Il2CppScheduleOne.ObjectScripts;
using UnityEngine;
using NPC = Il2CppScheduleOne.NPCs.NPC;

namespace SnackVendor.World;

/// <summary>
/// Captures WHICH npc is buying so the purchase can credit the ingredient into
/// <c>NPC.Inventory</c>. The vanilla <c>SendPurchase</c> RPC only sees the
/// VendingMachine — the npc lives on the schedule signal that walks up to the
/// machine. This prefix runs on <c>NPCSignal_UseVendingMachine.Purchase</c>
/// (observers RPC, fires on every client), registers the buyer in a
/// short-lived pending map keyed by machine instance-id, and
/// <see cref="VendingMachinePatches.SendPurchase_Prefix"/> consumes the entry
/// (server-side only, host-gated there).
///
/// Rules: never blocks the vanilla signal, never throws, entries expire
/// (60 s) and are dropped when the npc reference dies across a scene unload.
/// </summary>
public static class NPCSignalPatches
{
    private const float PendingLifetimeSeconds = 60f;

    private readonly struct PendingPurchase
    {
        public readonly NPC Npc;
        public readonly float CapturedAt;
        public PendingPurchase(NPC npc, float capturedAt) { Npc = npc; CapturedAt = capturedAt; }
    }

    private static readonly Dictionary<int, PendingPurchase> _pending = new();

    [HarmonyPrefix]
    public static bool Purchase_Prefix(NPCSignal_UseVendingMachine __instance)
    {
        try
        {
            // Registration itself is harmless on clients, but the pending map
            // is only consumed on the host — keep it host-side to avoid
            // cross-client reference churn.
            if (!S1Mods.Shared.NetworkGuard.IsHostOrSingleplayer()) return true;

            var machine = ResolveMachine(__instance);
            if (machine == null || machine.Pointer == IntPtr.Zero) return true;
            if (!VendingMachinePatches.IsOurs(machine)) return true; // vanilla machines untouched

            var npc = ResolveNpc(__instance);
            if (npc == null || npc.Pointer == IntPtr.Zero)
            {
                Mod.Log.Warn("SnackVendor purchase signal but NPC not resolvable — ingredient credit will be skipped (cash still credited).");
                return true;
            }

            PruneExpired();
            _pending[machine.GetInstanceID()] = new PendingPurchase(npc, Time.realtimeSinceStartup);
            Mod.Log.Debug($"Captured NPC '{npc.name}' for SnackVendor purchase at '{machine.name}'.");
        }
        catch (Exception ex)
        {
            Mod.Log.Warn("NPCSignalPatches.Purchase_Prefix", ex);
        }
        return true; // always let the vanilla signal run
    }

    /// <summary>
    /// Consumes the pending buyer for the given machine (single use).
    /// Returns null when no fresh entry exists or the npc reference died.
    /// </summary>
    internal static NPC? ConsumePendingNpc(VendingMachine machine)
    {
        try
        {
            if (machine == null || machine.Pointer == IntPtr.Zero) return null;
            int key = machine.GetInstanceID();
            if (!_pending.TryGetValue(key, out var p)) return null;
            _pending.Remove(key);
            if (Time.realtimeSinceStartup - p.CapturedAt > PendingLifetimeSeconds) return null;
            if (p.Npc == null || p.Npc.Pointer == IntPtr.Zero || p.Npc.WasCollected) return null;
            return p.Npc;
        }
        catch { return null; }
    }

    /// <summary>Drops every pending entry (scene transitions — stale npc refs).</summary>
    internal static void ClearPending() => _pending.Clear();

    private static void PruneExpired()
    {
        if (_pending.Count == 0) return;
        List<int>? expired = null;
        float now = Time.realtimeSinceStartup;
        foreach (var kvp in _pending)
        {
            if (now - kvp.Value.CapturedAt > PendingLifetimeSeconds)
            {
                (expired ??= new List<int>()).Add(kvp.Key);
            }
        }
        if (expired == null) return;
        for (int i = 0; i < expired.Count; i++) _pending.Remove(expired[i]);
    }

    private static VendingMachine? ResolveMachine(NPCSignal_UseVendingMachine signal)
    {
        try
        {
            var m = signal.GetTargetMachine();
            if (m != null && m.Pointer != IntPtr.Zero) return m;
        }
        catch { /* fall through to property */ }
        try
        {
            var m = signal.TargetMachine;
            if (m != null && m.Pointer != IntPtr.Zero) return m;
        }
        catch { }
        return null;
    }

    private static NPC? ResolveNpc(NPCSignal_UseVendingMachine signal)
    {
        try
        {
            // NPCSignal components live on (or under) the buyer's GameObject.
            var npc = signal.GetComponentInParent<NPC>();
            if (npc != null && npc.Pointer != IntPtr.Zero) return npc;
        }
        catch { }
        try
        {
            var npc = signal.gameObject.GetComponentInParent<NPC>();
            if (npc != null && npc.Pointer != IntPtr.Zero) return npc;
        }
        catch { }
        return null;
    }
}
