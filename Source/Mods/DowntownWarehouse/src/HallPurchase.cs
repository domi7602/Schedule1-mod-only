using System;
using System.Collections.Generic;
using System.Text.Json;

namespace DowntownWarehouse;

internal enum HallPurchaseStatus
{
    Purchased,
    AlreadyOwned,
    InsufficientFunds,
    NotReady,
    Refused
}

internal readonly record struct HallPurchaseDecision(
    HallPurchaseStatus Status,
    bool ChargeBank,
    float Amount);

internal static class HallListing
{
    internal static bool IsPropertyMenu(string? label, string? text)
    {
        string hay = ((label ?? "") + " " + (text ?? "")).ToLowerInvariant();
        if (hay.Contains("laundromat") || hay.Contains("post office") || hay.Contains("car wash")
            || hay.Contains("taco") || hay.Contains("medical") || hay.Contains("business"))
            return false;
        return hay.Contains("bungalow") || hay.Contains("barn") || hay.Contains("manor")
            || hay.Contains("motel") || hay.Contains("sweatshop") || hay.Contains("docks")
            || hay.Contains("storage unit") || ContainsWholeWord(hay, "rv");
    }

    internal static bool ContainsWholeWord(string text, string word)
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(word))
            return false;
        int start = 0;
        while (start <= text.Length - word.Length)
        {
            int idx = text.IndexOf(word, start, StringComparison.Ordinal);
            if (idx < 0)
                return false;
            bool leftOk = idx == 0 || !char.IsLetterOrDigit(text[idx - 1]);
            int end = idx + word.Length;
            bool rightOk = end >= text.Length || !char.IsLetterOrDigit(text[end]);
            if (leftOk && rightOk)
                return true;
            start = idx + 1;
        }
        return false;
    }
}

internal static class HallPickupPrompt
{
    internal static bool IsPickupMessage(string? message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return false;
        return message.IndexOf("Pick up", StringComparison.OrdinalIgnoreCase) >= 0
            || message.IndexOf("Must be empty", StringComparison.OrdinalIgnoreCase) >= 0
            || message.IndexOf("Not empty", StringComparison.OrdinalIgnoreCase) >= 0
            || message.IndexOf("Contains items", StringComparison.OrdinalIgnoreCase) >= 0
            || message.IndexOf("Won't fit", StringComparison.OrdinalIgnoreCase) >= 0
            || message.IndexOf("Doesn't fit", StringComparison.OrdinalIgnoreCase) >= 0
            || message.IndexOf("Passt nicht", StringComparison.OrdinalIgnoreCase) >= 0
            || message.IndexOf("Inhalt geschuetzt", StringComparison.OrdinalIgnoreCase) >= 0
            || message.IndexOf("Lager ist offen", StringComparison.OrdinalIgnoreCase) >= 0
            || message.IndexOf("Belegt", StringComparison.OrdinalIgnoreCase) >= 0
            || message.IndexOf("In Benutzung", StringComparison.OrdinalIgnoreCase) >= 0;
    }

    internal static bool IsEmptyOnlyBlockReason(string? reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            return false;
        return reason.IndexOf("Must be empty", StringComparison.OrdinalIgnoreCase) >= 0
            || reason.IndexOf("Not empty", StringComparison.OrdinalIgnoreCase) >= 0
            || reason.IndexOf("Contains items", StringComparison.OrdinalIgnoreCase) >= 0;
    }
}

internal static class HallWalletCheck
{
    internal static bool MatchesExpected(float before, float delta, float now)
    {
        if (!float.IsFinite(before) || !float.IsFinite(delta) || !float.IsFinite(now))
            return false;
        float expected = before + delta;
        if (!float.IsFinite(expected))
            return false;
        float scale = MathF.Max(1f, MathF.Max(MathF.Abs(before), MathF.Abs(expected)));
        float ulp = MathF.BitIncrement(scale) - scale;
        float tol = MathF.Max(0.01f, ulp * 4f);
        return MathF.Abs(now - expected) <= tol;
    }
}

internal static class HallPurchase
{
    internal static HallPurchaseDecision Evaluate(bool ready, bool owned, float bank, float cash)
    {
        if (!ready)
            return new HallPurchaseDecision(HallPurchaseStatus.NotReady, false, 0f);
        if (owned)
            return new HallPurchaseDecision(HallPurchaseStatus.AlreadyOwned, false, 0f);

        float price = HallPlan.Price;
        if (!float.IsFinite(price) || price <= 0f)
            return new HallPurchaseDecision(HallPurchaseStatus.Refused, false, 0f);
        if (float.IsFinite(bank) && bank + 0.001f >= price)
            return new HallPurchaseDecision(HallPurchaseStatus.Purchased, true, price);
        if (float.IsFinite(cash) && cash + 0.001f >= price)
            return new HallPurchaseDecision(HallPurchaseStatus.Purchased, false, price);
        return new HallPurchaseDecision(HallPurchaseStatus.InsufficientFunds, false, 0f);
    }
}

internal enum HallSaveKind
{
    Missing,
    Ready,
    ForeignSlot,
    Unsupported,
    Corrupt
}

internal sealed class HallSlotRecord
{
    public string ItemId { get; set; } = "";
    public int Slot { get; set; }
    public int Quantity { get; set; }
    public string? ItemJson { get; set; }
}

internal sealed class HallPlacementRecord
{
    internal const int MaxNativeDataJsonLength = 262144;

    public string Id { get; set; } = "";
    public string ItemId { get; set; } = "";
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public float RotX { get; set; }
    public float RotY { get; set; }
    public float RotZ { get; set; }
    public float RotW { get; set; } = 1f;
    public List<HallSlotRecord> Contents { get; set; } = new();
    public string? NativeDataType { get; set; }
    public string? NativeDataJson { get; set; }
    public int LoadOrder { get; set; }

    internal bool IsPlausible()
    {
        if (string.IsNullOrWhiteSpace(Id) || Id.Length > 64)
            return false;
        if (string.IsNullOrWhiteSpace(ItemId) || ItemId.Length > 80)
            return false;
        if (ItemId.IndexOfAny(new[] { '/', '\\', '\n', '\r' }) >= 0)
            return false;
        if (!HallPlan.ContainsPlacement(X, Y, Z))
            return false;
        if (LoadOrder < -100 || LoadOrder > 1000)
            return false;
        if (NativeDataType != null && (NativeDataType.Length > 128 || NativeDataType.IndexOfAny(new[] { '/', '\\', '\n', '\r' }) >= 0))
            return false;
        if (NativeDataJson != null && (string.IsNullOrWhiteSpace(NativeDataType) || NativeDataJson.Length > MaxNativeDataJsonLength))
            return false;

        float mag = RotX * RotX + RotY * RotY + RotZ * RotZ + RotW * RotW;
        if (mag < 0.9f || mag > 1.1f)
            return false;
        if (Contents == null || Contents.Count > 64)
            return false;

        var slots = new HashSet<int>();
        foreach (var line in Contents)
        {
            if (line == null || line.Slot < 0 || line.Slot > 63 || !slots.Add(line.Slot))
                return false;
            if (string.IsNullOrWhiteSpace(line.ItemId) || line.ItemId.Length > 80)
                return false;
            if (line.Quantity < 1 || line.Quantity > 999)
                return false;
            if (line.ItemJson != null && line.ItemJson.Length > 4096)
                return false;
        }

        return true;
    }
}

internal static class HallPlacementOrdering
{
    internal static List<HallPlacementRecord> SortForRestore(IReadOnlyList<HallPlacementRecord>? records)
    {
        var sorted = new List<HallPlacementRecord>();
        if (records == null || records.Count == 0)
            return sorted;

        var indexed = new List<(int Index, HallPlacementRecord Record)>(records.Count);
        for (int i = 0; i < records.Count; i++)
        {
            var rec = records[i];
            if (rec != null)
                indexed.Add((i, rec));
        }

        indexed.Sort((a, b) =>
        {
            int cmp = a.Record.LoadOrder.CompareTo(b.Record.LoadOrder);
            return cmp != 0 ? cmp : a.Index.CompareTo(b.Index);
        });

        for (int i = 0; i < indexed.Count; i++)
            sorted.Add(indexed[i].Record);
        return sorted;
    }
}

internal sealed class HallAdditionalDataPayload
{
    public string? Name { get; set; }
    public string? Contents { get; set; }
}

internal sealed class HallDynamicDataPayload
{
    public string? DataType { get; set; }
    public int DataVersion { get; set; }
    public string? GameVersion { get; set; }
    public string? BaseData { get; set; }
    public List<HallAdditionalDataPayload>? AdditionalDatas { get; set; }

    internal static HallDynamicDataPayload? TryParse(string? json)
    {
        if (string.IsNullOrWhiteSpace(json) || json.Length > HallPlacementRecord.MaxNativeDataJsonLength)
            return null;
        try
        {
            var payload = JsonSerializer.Deserialize<HallDynamicDataPayload>(json);
            if (payload == null || string.IsNullOrWhiteSpace(payload.BaseData))
                return null;
            return payload;
        }
        catch
        {
            return null;
        }
    }
}

internal sealed class HallSaveFile
{
    public int Schema { get; set; } = HallPlan.Schema;
    public string PropertyCode { get; set; } = HallPlan.Code;
    public string SaveFolder { get; set; } = "";
    public bool Owned { get; set; }
    public List<HallPlacementRecord> Placements { get; set; } = new();
}

internal static class HallSaveRules
{
    internal static HallSaveKind Classify(HallSaveFile? loaded, string folder, bool exists)
    {
        if (!exists)
            return HallSaveKind.Missing;
        if (loaded == null || loaded.Placements == null)
            return HallSaveKind.Corrupt;
        if (loaded.Schema != HallPlan.Schema)
            return HallSaveKind.Unsupported;
        if (!string.Equals(loaded.PropertyCode, HallPlan.Code, StringComparison.Ordinal))
            return HallSaveKind.Corrupt;
        if (string.IsNullOrWhiteSpace(folder) || string.IsNullOrWhiteSpace(loaded.SaveFolder))
            return HallSaveKind.Corrupt;
        if (!string.Equals(Normalize(loaded.SaveFolder), Normalize(folder), StringComparison.OrdinalIgnoreCase))
            return HallSaveKind.ForeignSlot;

        var ids = new HashSet<string>(StringComparer.Ordinal);
        foreach (var placement in loaded.Placements)
        {
            if (placement == null || !placement.IsPlausible() || !ids.Add(placement.Id))
                return HallSaveKind.Corrupt;
        }

        if (!loaded.Owned && loaded.Placements.Count > 0)
            return HallSaveKind.Corrupt;
        return HallSaveKind.Ready;
    }

    internal static bool ShouldWriteSave(bool owned, HallSaveKind kind, int placementCount)
    {
        if (kind is HallSaveKind.ForeignSlot or HallSaveKind.Unsupported or HallSaveKind.Corrupt)
            return false;
        if (!owned && kind == HallSaveKind.Missing && placementCount == 0)
            return false;
        return true;
    }

    internal static string Normalize(string path) =>
        path.Replace('/', '\\').Trim().TrimEnd('\\');
}

internal sealed class HallSlotBudget
{
    public int Index { get; init; }
    public bool Locked { get; init; }
    public bool IsEmpty { get; init; }
    public int Quantity { get; init; }
    public int Capacity { get; init; }
}

internal sealed class HallReturnNeed
{
    public string StackKey { get; init; } = "";
    public int Quantity { get; init; }
    public int StackLimit { get; init; }
    public IReadOnlyDictionary<int, int>? ExistingStackCapacities { get; init; }
    public IReadOnlyCollection<int>? AllowedEmptySlots { get; init; }
}

internal static class HallInventoryPlanner
{
    internal static bool CanFitAll(IReadOnlyList<HallSlotBudget>? slots, IReadOnlyList<HallReturnNeed>? needs)
    {
        if (slots == null || needs == null || needs.Count == 0)
            return false;

        var usedExisting = new Dictionary<int, int>();
        var claimedEmpty = new HashSet<int>();
        var assignedKey = new Dictionary<int, string>();
        var assignedRemaining = new Dictionary<int, int>();

        foreach (var need in needs)
        {
            if (need == null || need.Quantity < 1 || need.StackLimit < 1)
                return false;

            int left = need.Quantity;

            if (need.ExistingStackCapacities != null)
            {
                for (int i = 0; i < slots.Count && left > 0; i++)
                {
                    var slot = slots[i];
                    if (slot.Locked || slot.IsEmpty)
                        continue;
                    if (!need.ExistingStackCapacities.TryGetValue(slot.Index, out int baseRoom) || baseRoom <= 0)
                        continue;
                    usedExisting.TryGetValue(slot.Index, out int alreadyUsed);
                    int avail = Math.Max(0, baseRoom - alreadyUsed);
                    if (avail <= 0)
                        continue;
                    int take = Math.Min(left, avail);
                    usedExisting[slot.Index] = alreadyUsed + take;
                    left -= take;
                }
            }

            if (left > 0 && !string.IsNullOrEmpty(need.StackKey) && need.AllowedEmptySlots != null)
            {
                for (int i = 0; i < slots.Count && left > 0; i++)
                {
                    var slot = slots[i];
                    if (slot.Locked || !slot.IsEmpty || !claimedEmpty.Contains(slot.Index))
                        continue;
                    if (!ContainsSlot(need.AllowedEmptySlots, slot.Index))
                        continue;
                    if (!assignedKey.TryGetValue(slot.Index, out var ownerKey)
                        || !string.Equals(ownerKey, need.StackKey, StringComparison.Ordinal))
                        continue;
                    if (!assignedRemaining.TryGetValue(slot.Index, out int room) || room <= 0)
                        continue;
                    int take = Math.Min(left, room);
                    assignedRemaining[slot.Index] = room - take;
                    left -= take;
                }
            }

            if (left > 0 && need.AllowedEmptySlots != null)
            {
                for (int i = 0; i < slots.Count && left > 0; i++)
                {
                    var slot = slots[i];
                    if (slot.Locked || !slot.IsEmpty || claimedEmpty.Contains(slot.Index))
                        continue;
                    if (!ContainsSlot(need.AllowedEmptySlots, slot.Index))
                        continue;
                    int slotCap = Math.Max(1, need.StackLimit);
                    int take = Math.Min(left, slotCap);
                    claimedEmpty.Add(slot.Index);
                    assignedKey[slot.Index] = need.StackKey;
                    assignedRemaining[slot.Index] = slotCap - take;
                    left -= take;
                }
            }

            if (left > 0)
                return false;
        }

        return true;
    }

    private static bool ContainsSlot(IReadOnlyCollection<int> set, int index)
    {
        if (set is ISet<int> fast)
            return fast.Contains(index);
        foreach (int candidate in set)
        {
            if (candidate == index)
                return true;
        }
        return false;
    }
}
