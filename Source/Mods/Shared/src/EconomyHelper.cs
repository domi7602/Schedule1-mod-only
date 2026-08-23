using System;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.PlayerScripts;
using UnityEngine;

namespace S1Mods.Shared;

/// <summary>
/// Centralized safe economy and inventory cash helper for Schedule I mods.
/// Provides safe cash/bank checks, inventory slot capacity calculation, and double-entry booking helpers.
/// </summary>
public static class EconomyHelper
{
    public const int DefaultMaxCashPerSlot = 1000;

    /// <summary>
    /// Live online bank balance.
    /// </summary>
    public static float GetOnlineBalance() => S1API.Money.Money.GetOnlineBalance();

    /// <summary>
    /// Live cash on hand.
    /// </summary>
    public static float GetCashBalance() => S1API.Money.Money.GetCashBalance();

    /// <summary>
    /// Live total net worth (cash + online bank + business equity).
    /// </summary>
    public static float GetNetWorth() => S1API.Money.Money.GetNetWorth();

    /// <summary>
    /// Returns the number of empty slots in the player's hotbar.
    /// </summary>
    public static int GetFreeInventorySlotsCount()
    {
        try
        {
            var inv = PlayerSingleton<PlayerInventory>.Instance;
            if (inv == null || inv.Pointer == IntPtr.Zero || inv.WasCollected) return 0;

            int freeCount = 0;
            var hotbar = inv.hotbarSlots;
            if (hotbar != null)
            {
                for (int i = 0; i < hotbar.Count; i++)
                {
                    var slot = hotbar[i];
                    if (slot == null || slot.Pointer == IntPtr.Zero || slot.WasCollected) continue;
                    if (slot.ItemInstance == null || slot.ItemInstance.Pointer == IntPtr.Zero || slot.ItemInstance.WasCollected)
                    {
                        freeCount++;
                    }
                }
            }
            return freeCount;
        }
        catch
        {
            return 0;
        }
    }

    /// <summary>
    /// Returns the quantity of cash stored in the dedicated cash slot.
    /// </summary>
    public static float GetCashInDedicatedCashSlot()
    {
        try
        {
            var inv = PlayerSingleton<PlayerInventory>.Instance;
            if (inv == null || inv.Pointer == IntPtr.Zero || inv.WasCollected) return 0f;
            var cashSlot = inv.cashSlot;
            if (cashSlot == null || cashSlot.Pointer == IntPtr.Zero || cashSlot.WasCollected) return 0f;
            if (cashSlot.ItemInstance != null && cashSlot.ItemInstance.Pointer != IntPtr.Zero && !cashSlot.ItemInstance.WasCollected)
            {
                return cashSlot.ItemInstance.Quantity;
            }
            return 0f;
        }
        catch
        {
            return 0f;
        }
    }

    /// <summary>
    /// Calculates the maximum amount of cash that can currently be added to the player's inventory
    /// across the dedicated cash slot and free hotbar slots.
    /// </summary>
    public static float GetMaxHoldableCashCapacity(int maxPerSlot = DefaultMaxCashPerSlot)
    {
        if (maxPerSlot <= 0) maxPerSlot = DefaultMaxCashPerSlot;

        try
        {
            float cashInCashSlot = GetCashInDedicatedCashSlot();
            float cashSlotRoom = Mathf.Max(0f, maxPerSlot - cashInCashSlot);
            int freeHotbarSlots = GetFreeInventorySlotsCount();

            // Also count partially filled cash stacks in hotbar (they can be topped up)
            float partialRoom = 0f;
            var inv = PlayerSingleton<PlayerInventory>.Instance;
            if (inv != null && inv.Pointer != IntPtr.Zero && !inv.WasCollected && inv.hotbarSlots != null)
            {
                var hotbar = inv.hotbarSlots;
                for (int i = 0; i < hotbar.Count; i++)
                {
                    var slot = hotbar[i];
                    if (slot == null || slot.Pointer == IntPtr.Zero || slot.WasCollected) continue;
                    var inst = slot.ItemInstance;
                    if (inst != null && inst.Pointer != IntPtr.Zero && !inst.WasCollected && inst.Definition != null)
                    {
                        // Cash stack identified by same ID as cashSlot definition or by ItemInstance type
                        try
                        {
                            if (inst.Quantity < maxPerSlot && inst.Quantity > 0)
                            {
                                // Only count room if this is actually a cash stack (ID check)
                                string? id = inst.Definition.ID;
                                if (!string.IsNullOrEmpty(id) && id.ToLowerInvariant().Contains("cash"))
                                {
                                    partialRoom += (maxPerSlot - inst.Quantity);
                                }
                            }
                        }
                        catch { }
                    }
                }
            }

            return cashSlotRoom + (freeHotbarSlots * maxPerSlot) + partialRoom;
        }
        catch
        {
            float cashInCashSlot = GetCashInDedicatedCashSlot();
            float cashSlotRoom = Mathf.Max(0f, maxPerSlot - cashInCashSlot);
            int freeHotbarSlots = GetFreeInventorySlotsCount();
            return cashSlotRoom + (freeHotbarSlots * maxPerSlot);
        }
    }

    /// <summary>
    /// Checks if the player's inventory has enough capacity to receive the specified cash amount.
    /// </summary>
    public static bool CanHoldCash(float amount, int maxPerSlot = DefaultMaxCashPerSlot)
    {
        if (amount <= 0f) return true;
        return GetMaxHoldableCashCapacity(maxPerSlot) >= amount;
    }

    /// <summary>
    /// Modifies cash on hand via S1API Money wrapper.
    /// </summary>
    public static void ChangeCashBalance(float delta, bool visualize = true, bool playSound = false)
    {
        S1API.Money.Money.ChangeCashBalance(delta, visualizeChange: visualize, playCashSound: playSound);
    }

    /// <summary>
    /// Creates an online bank transaction via S1API Money wrapper.
    /// </summary>
    public static void CreateOnlineTransaction(string label, float amount, float quantity = 1f, string description = "")
    {
        S1API.Money.Money.CreateOnlineTransaction(label, amount, quantity, description);
    }
}
