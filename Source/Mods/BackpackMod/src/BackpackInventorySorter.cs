extern alias il2cpp;
using Il2CppScheduleOne.Core.Items.Framework;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppScheduleOne.UI;
using Il2CppScheduleOne.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BackpackMod
{
    /// <summary>
    /// B1 (QoL Spec v1.1.0): Sort engine for backpack storage, player inventory and open
    /// vanilla storage containers. Pure slot-content rewrite — no UI patching, no second
    /// write path (slots fire onItemDataChanged, vanilla UI updates itself).
    ///
    /// Design notes (Spec 2026-09-12):
    /// - Merge: same ID + same Quality + same Packaging (matches vanilla StackLimit rules)
    ///   up to the effective stack limit; overflow spills into empty slots.
    /// - Order: configurable default = Category → Quality (desc) → Name.
    /// - Player inventory sort includes hotbar slots 0-7; clipboard (8) and cash (9) are
    ///   NEVER touched (special slot types live there).
    /// - No item can be lost: capacity check runs before any write (SortSlots is atomic —
    ///   it plans the new layout in managed memory, then commits. If the plan does not fit,
    ///   nothing is written).
    /// - IL2CPP: WasCollected/IntPtr guards at every native touch point.
    /// </summary>
    public static class BackpackInventorySorter
    {
        /// <summary>Configurable via ModConfig (Spec §Konfiguration). Default: Category→Quality→Name.</summary>
        public static SortOrder ActiveOrder { get; set; } = SortOrder.CategoryQualityName;

        public enum SortOrder
        {
            CategoryQualityName = 0,
            Name = 1,
            QualityName = 2,
        }

        private static int _lastSortStackCount = -1;
        private static int _lastSortMergeCount = -1;

        // ────────────────────────────────────────────────────────────────
        // Public entry points
        // ────────────────────────────────────────────────────────────────

        /// <summary>Sorts the backpack storage entity (the mod's own StorageEntity).</summary>
        public static bool SortBackpack()
        {
            var entity = BackpackStorageManager.GetStorageEntityForSort();
            if (entity == null)
            {
                Mod.Log?.Msg("Sort: no backpack storage open/available.");
                return false;
            }
            return SortSlots(entity.ItemSlots, "backpack");
        }

        /// <summary>
        /// Sorts the player inventory (slots 0-8 region: hotbar 0-7 + clipboard 8).
        /// Cash slot (9) and clothing slots are excluded by design.
        /// </summary>
        public static bool SortPlayerInventory()
        {
            var inv = PlayerInventory.Instance;
            if (inv == null || inv.Pointer == IntPtr.Zero || inv.WasCollected)
            {
                Mod.Log?.Msg("Sort: PlayerInventory not available.");
                return false;
            }

            try
            {
                var slots = inv.GetAllInventorySlots();
                if (slots == null || slots.Pointer == IntPtr.Zero || slots.Count == 0)
                {
                    Mod.Log?.Msg("Sort: player inventory has no slots.");
                    return false;
                }

                // Defensive: sort hotbar slots 0-7 ONLY. The clipboard slot (8) and cash
                // slot (9) are special slot types — writing normal stacks there via the
                // _internal path bypasses their UI validation. Exclude them explicitly
                // by reference, not just by index (defence in depth against slot-list
                // composition changes).
                var clipboardSlot = inv.clipboardSlot;
                var cashSlot = inv.cashSlot;
                bool IsSpecial(ItemSlot s)
                {
                    try
                    {
                        if (clipboardSlot != null && clipboardSlot.Pointer != IntPtr.Zero && !clipboardSlot.WasCollected && s.Pointer == clipboardSlot.Pointer) return true;
                        if (cashSlot != null && cashSlot.Pointer != IntPtr.Zero && !cashSlot.WasCollected && s.Pointer == cashSlot.Pointer) return true;
                    }
                    catch { }
                    return false;
                }

                int safeCount = System.Math.Min(8, slots.Count); // hotbar 0-7 only
                var target = new List<ItemSlot>();
                for (int i = 0; i < safeCount; i++)
                {
                    var s = slots[i];
                    if (s == null || s.Pointer == IntPtr.Zero || s.WasCollected) continue;
                    if (IsSpecial(s)) continue;
                    target.Add(s);
                }
                return SortSlots(target, "player-inventory");
            }
            catch (Exception ex)
            {
                Mod.Log?.Error($"Sort player inventory failed: {ex}");
                return false;
            }
        }

        /// <summary>Sorts whichever vanilla storage container is currently open (B1 scope: world containers).</summary>
        public static bool SortOpenStorageMenu()
        {
            var menu = Singleton<StorageMenu>.Instance;
            if (menu == null || menu.Pointer == IntPtr.Zero || !menu.IsOpen)
            {
                Mod.Log?.Msg("Sort: no storage menu open.");
                return false;
            }

            var entity = menu.OpenedStorageEntity;
            if (entity == null || entity.Pointer == IntPtr.Zero || entity.WasCollected)
            {
                Mod.Log?.Msg("Sort: open storage menu has no StorageEntity.");
                return false;
            }

            // Never double-sort our own backpack entity through the vanilla path.
            if (BackpackStorageManager.IsOwnedStorageEntity(entity))
            {
                return SortBackpack();
            }

            return SortSlots(entity.ItemSlots, "storage-container");
        }

        // ────────────────────────────────────────────────────────────────
        // Core algorithm — plan in managed memory, commit atomically
        // ────────────────────────────────────────────────────────────────

        /// <summary>Counts of the last sort, for the Spec §Observability log line.</summary>
        public static (int Stacks, int Merges) LastSortStats => (_lastSortStackCount, _lastSortMergeCount);

        /// <summary>
        /// Managed-list entry point. Copies the Il2Cpp list up front (snapshot semantics —
        /// safer against native mutation mid-sort) and delegates to the core.
        /// NOTE: StorageEntity.ItemSlots is an Il2CppSystem.List living in Il2Cppmscorlib.dll,
        /// which is referenced with the extern alias `il2cpp` (Directory.Build.props) — hence
        /// the il2cpp:: qualified generic type (pattern proven in MoreSaveSlots).
        /// </summary>
        public static bool SortSlots(il2cpp::Il2CppSystem.Collections.Generic.List<ItemSlot>? il2cppSlots, string contextLabel)
        {
            try
            {
                if (il2cppSlots == null || il2cppSlots.Pointer == IntPtr.Zero || il2cppSlots.WasCollected || il2cppSlots.Count == 0)
                    return false;

                var managed = new List<ItemSlot>();
                for (int i = 0; i < il2cppSlots.Count; i++)
                {
                    var s = il2cppSlots[i];
                    if (s != null && s.Pointer != IntPtr.Zero && !s.WasCollected) managed.Add(s);
                }
                return SortSlots(managed, contextLabel);
            }
            catch (Exception ex)
            {
                Mod.Log?.Error($"Sort [{contextLabel}] snapshot failed: {ex}");
                return false;
            }
        }

        public static bool SortSlots(List<ItemSlot>? slots, string contextLabel)
        {
            try
            {
                if (slots == null || slots.Count == 0) return false;

                // 1. Snapshot all (instance, quantity) pairs from valid slots.
                var entries = new List<SortEntry>();
                for (int i = 0; i < slots.Count; i++)
                {
                    var slot = slots[i];
                    if (slot == null || slot.Pointer == IntPtr.Zero || slot.WasCollected) continue;
                    var inst = slot.ItemInstance;
                    if (inst == null || inst.Pointer == IntPtr.Zero || inst.WasCollected) continue;
                    var def = inst.Definition;
                    if (def == null || def.Pointer == IntPtr.Zero || def.WasCollected) continue;
                    if (slot.Quantity <= 0) continue;

                    entries.Add(new SortEntry
                    {
                        ItemId = def.ID,
                        Name = SafeName(def),
                        Category = SafeCategory(def),
                        Quality = ReadQuality(inst),
                        PackagingId = ReadPackagingId(inst),
                        SourceInstance = inst,
                        Quantity = slot.Quantity,
                    });
                }

                if (entries.Count == 0)
                {
                    Mod.Log?.Msg($"Sort [{contextLabel}]: nothing to sort.");
                    return false;
                }

                // 2. Sort entries by active order FIRST, then group — the ordered
                //    group iteration then produces `planned` in final sorted order.
                var orderedEntries = entries
                    .OrderBy(e => e, SortEntryComparer(ActiveOrder))
                    .ToList();

                var groups = orderedEntries
                    .GroupBy(e => e.MergeKey, StringComparer.OrdinalIgnoreCase)
                    .ToList();

                // 3. Flatten groups (inner stacks stay quantity-ordered) — kept for stats.
                var orderedStacks = new List<SortEntry>();
                foreach (var group in groups)
                {
                    foreach (var e in group.OrderBy(x => x.Quantity))
                        orderedStacks.Add(e);
                }

                // 4. Merge groups into stacks up to stack limit. Groups were built from
                //    `entries` in sorted order, so `planned` inherits that order — the
                //    sort actually happens in step 3b below BEFORE grouping.
                var planned = new List<SortEntry>();
                int merges = 0;
                foreach (var g in groups)
                {
                    int limit = EffectiveStackLimit(g.First().SourceInstance);
                    if (limit < 1) limit = 1;

                    int remaining = g.Sum(x => x.Quantity);

                    // Split into full stacks + remainder.
                    int produced = 0;
                    while (remaining > 0)
                    {
                        int take = System.Math.Min(remaining, limit);
                        var proto = g.First();
                        planned.Add(new SortEntry
                        {
                            ItemId = proto.ItemId,
                            Name = proto.Name,
                            Category = proto.Category,
                            Quality = proto.Quality,
                            PackagingId = proto.PackagingId,
                            SourceInstance = proto.SourceInstance,
                            Quantity = take,
                        });
                        remaining -= take;
                        produced++;
                    }

                    // A "merge" = we reduced the number of stacks for this key.
                    int originalStackCount = g.Count();
                    // (Re-splitting full stacks can legitimately increase count when a single
                    //  stack exceeded the limit — that is a split, not a merge.)
                    if (produced < originalStackCount) merges += originalStackCount - produced;
                }

                // Re-apply ordering is NOT needed: `planned` was built from ordered groups.

                // 5. Capacity check: every planned stack needs a slot.
                int writableSlots = 0;
                foreach (var slot in slots)
                {
                    if (slot != null && slot.Pointer != IntPtr.Zero && !slot.WasCollected) writableSlots++;
                }
                if (planned.Count > writableSlots)
                {
                    Mod.Log?.Warning($"Sort [{contextLabel}]: aborted — plan needs {planned.Count} slots, only {writableSlots} writable. No changes written.");
                    return false;
                }

                // 5b. Pre-build ALL instances BEFORE the first write (atomicity): if any
                //     instance cannot be built, we abort with ZERO writes — no partial
                //     state, no item loss/duplication. GetCopy preserves quality,
                //     packaging and instance state (unlike GetDefaultInstance).
                var builtInstances = new List<ItemInstance>(planned.Count);
                for (int i = 0; i < planned.Count; i++)
                {
                    var p = planned[i];
                    ItemInstance? inst = null;
                    try
                    {
                        inst = p.SourceInstance.GetCopy(p.Quantity);
                    }
                    catch { inst = null; }
                    if (inst == null || inst.Pointer == IntPtr.Zero || inst.WasCollected)
                    {
                        Mod.Log?.Warning($"Sort [{contextLabel}]: could not copy instance for '{p.ItemId}' — aborted BEFORE any write ({i}/{planned.Count} planned). Inventory unchanged.");
                        return false;
                    }
                    builtInstances.Add(inst);
                }

                // 6. Commit: walk slots in order, write planned stacks via SetStoredItem,
                //    clear the tail. SetStoredItem(x, true) = _internal, fires onItemDataChanged
                //    so any open UI refreshes itself (vanilla behaviour).
                int builtIndex = 0;
                for (int i = 0; i < slots.Count; i++)
                {
                    var slot = slots[i];
                    if (slot == null || slot.Pointer == IntPtr.Zero || slot.WasCollected) continue;

                    // Find next planned stack that still has quantity to place.
                    SortEntry? next = null;
                    foreach (var p in planned)
                    {
                        if (p.PlacedQuantity < p.Quantity) { next = p; break; }
                    }

                    if (next == null)
                    {
                        // Everything placed — clear this slot if it still holds something.
                        if (slot.ItemInstance != null && slot.ItemInstance.Pointer != IntPtr.Zero && !slot.ItemInstance.WasCollected)
                        {
                            slot.ClearStoredInstance(true);
                        }
                        continue;
                    }

                    var instance = builtInstances[builtIndex];
                    builtIndex++;
                    slot.SetStoredItem(instance, true);
                    next.PlacedQuantity = next.Quantity;
                }

                _lastSortStackCount = planned.Count;
                _lastSortMergeCount = merges;
                Mod.Log?.Msg($"[BackpackMod] Sorted {planned.Count} stacks ({merges} merges) in {contextLabel}.");
                return true;
            }
            catch (Exception ex)
            {
                Mod.Log?.Error($"Sort [{contextLabel}] failed: {ex}");
                return false;
            }
        }

        // ────────────────────────────────────────────────────────────────
        // Helpers — every IL2CPP access guarded
        // ────────────────────────────────────────────────────────────────

        private static string SafeName(BaseItemDefinition def)
        {
            try { return def.Name ?? def.ID ?? "?"; }
            catch { return def.ID ?? "?"; }
        }

        private static EItemCategory SafeCategory(BaseItemDefinition def)
        {
            try { return def.Category; }
            catch { return EItemCategory.Consumable; }
        }

        private static int ReadQuality(ItemInstance inst)
        {
            try
            {
                var q = inst.TryCast<QualityItemInstance>();
                if (q != null && q.Pointer != IntPtr.Zero && !q.WasCollected) return (int)q.Quality;
            }
            catch { }
            return -1; // non-quality item — sorts after quality items within same category
        }

        private static string ReadPackagingId(ItemInstance inst)
        {
            try
            {
                var p = inst.TryCast<Il2CppScheduleOne.Product.ProductItemInstance>();
                if (p != null && p.Pointer != IntPtr.Zero && !p.WasCollected)
                    return p.AppliedPackaging?.ID ?? p.PackagingID ?? string.Empty;
            }
            catch { }
            return string.Empty;
        }

        private static int EffectiveStackLimit(ItemInstance inst)
        {
            try
            {
                // get_StackLimit lives on BaseItemInstance (patchable accessor — proven by
                // StackLimitMod). Respect the effective limit so StackLimitMod configs hold.
                return inst.StackLimit;
            }
            catch
            {
                try
                {
                    var def = inst.Definition;
                    if (def != null && def.Pointer != IntPtr.Zero && !def.WasCollected)
                    {
                        // DefaultStackLimit is a STATIC member on the interop proxy
                        // (CS0176 proven 2026-09-12) — access via type, not instance.
                        return BaseItemDefinition.DefaultStackLimit;
                    }
                }
                catch { }
            }
            return 10; // game default
        }

        /// <summary>Comparer over sort entries implementing the active sort order.</summary>
        private static IComparer<SortEntry> SortEntryComparer(SortOrder order)
        {
            return Comparer<SortEntry>.Create((a, b) =>
            {
                int c;
                switch (order)
                {
                    case SortOrder.Name:
                        c = string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase);
                        if (c != 0) return c;
                        break;
                    case SortOrder.QualityName:
                        c = b.Quality.CompareTo(a.Quality); // quality descending
                        if (c != 0) return c;
                        c = string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase);
                        if (c != 0) return c;
                        break;
                    default: // CategoryQualityName
                        c = string.Compare(a.Category.ToString(), b.Category.ToString(), StringComparison.OrdinalIgnoreCase);
                        if (c != 0) return c;
                        c = b.Quality.CompareTo(a.Quality); // quality descending (Heavenly first)
                        if (c != 0) return c;
                        c = string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase);
                        if (c != 0) return c;
                        break;
                }
                return string.Compare(a.ItemId, b.ItemId, StringComparison.OrdinalIgnoreCase);
            });
        }

        private class SortEntry
        {
            public string ItemId = "";
            public string Name = "";
            public EItemCategory Category = EItemCategory.Consumable;
            public int Quality = -1;
            public string PackagingId = "";
            public ItemInstance SourceInstance = null!;
            public int Quantity;
            public int PlacedQuantity;

            public string MergeKey => $"{(int)Category}|{Quality}|{Name}|{PackagingId}|{ItemId}";

            /// <summary>
            /// Builds a fresh IL2CPP instance with the original's quality/packaging preserved
            /// (same Fix-3.1 pattern as BackpackStorageManager.LoadStorage).
            /// </summary>
            public ItemInstance? BuildInstance(int quantity)
            {
                try
                {
                    var def = Il2CppScheduleOne.Registry.GetItem(ItemId);
                    if (def == null || def.Pointer == IntPtr.Zero || def.WasCollected) return null;
                    var inst = def.GetDefaultInstance(quantity);
                    if (inst == null || inst.Pointer == IntPtr.Zero) return null;

                    try
                    {
                        var qInst = inst.TryCast<QualityItemInstance>();
                        if (qInst != null && qInst.Pointer != IntPtr.Zero && !qInst.WasCollected && Quality >= 0)
                            qInst.Quality = (EQuality)Quality;
                    }
                    catch { }

                    try
                    {
                        if (!string.IsNullOrEmpty(PackagingId))
                        {
                            var pInst = inst.TryCast<Il2CppScheduleOne.Product.ProductItemInstance>();
                            if (pInst != null && pInst.Pointer != IntPtr.Zero && !pInst.WasCollected)
                            {
                                var pkgDef = Il2CppScheduleOne.Registry.GetItem(PackagingId);
                                var appliedPkg = pkgDef?.TryCast<Il2CppScheduleOne.Product.Packaging.PackagingDefinition>();
                                if (appliedPkg != null && appliedPkg.Pointer != IntPtr.Zero && !appliedPkg.WasCollected)
                                    pInst.SetPackaging(appliedPkg);
                                else
                                    pInst.PackagingID = PackagingId;
                            }
                        }
                    }
                    catch { }

                    return inst;
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
