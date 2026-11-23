using System;
using System.Collections.Generic;

namespace NorthIsland;

internal enum HideoutPurchaseStatus
{
    Purchased,
    AlreadyOwned,
    InsufficientFunds,
    NotReady,
    Refused
}

internal readonly record struct HideoutPurchaseDecision(
    HideoutPurchaseStatus Status,
    bool ChargeBank,
    float Amount);

internal static class HideoutListing
{
    internal static bool IsPropertyMenu(string? label, string? text)
    {
        string hay = ((label ?? "") + " " + (text ?? "")).ToLowerInvariant();
        if (hay.Contains("laundromat") || hay.Contains("post office") || hay.Contains("car wash")
            || hay.Contains("taco") || hay.Contains("medical") || hay.Contains("business"))
            return false;
        return hay.Contains("bungalow") || hay.Contains("barn") || hay.Contains("manor")
            || hay.Contains("motel") || hay.Contains("sweatshop") || hay.Contains("docks")
            || hay.Contains("storage unit") || hay.Contains("rv");
    }
}

internal static class HideoutPurchase
{
    internal static HideoutPurchaseDecision Evaluate(bool ready, bool owned, float bank, float cash)
    {
        if (!ready)
            return new HideoutPurchaseDecision(HideoutPurchaseStatus.NotReady, false, 0f);
        if (owned)
            return new HideoutPurchaseDecision(HideoutPurchaseStatus.AlreadyOwned, false, 0f);

        float price = HideoutPropertyPlan.Price;
        if (!float.IsFinite(price) || price <= 0f)
            return new HideoutPurchaseDecision(HideoutPurchaseStatus.Refused, false, 0f);
        if (float.IsFinite(bank) && bank + 0.001f >= price)
            return new HideoutPurchaseDecision(HideoutPurchaseStatus.Purchased, true, price);
        if (float.IsFinite(cash) && cash + 0.001f >= price)
            return new HideoutPurchaseDecision(HideoutPurchaseStatus.Purchased, false, price);
        return new HideoutPurchaseDecision(HideoutPurchaseStatus.InsufficientFunds, false, 0f);
    }
}

internal enum HideoutSaveKind
{
    Missing,
    Ready,
    ForeignSlot,
    Unsupported,
    Corrupt
}

internal sealed class HideoutSlotRecord
{
    public string ItemId { get; set; } = "";
    public int Slot { get; set; }
    public int Quantity { get; set; }
}

internal sealed class HideoutPlacementRecord
{
    public string Id { get; set; } = "";
    public string ItemId { get; set; } = "";
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public float RotX { get; set; }
    public float RotY { get; set; }
    public float RotZ { get; set; }
    public float RotW { get; set; } = 1f;
    public List<HideoutSlotRecord> Contents { get; set; } = new();

    internal bool IsPlausible()
    {
        if (string.IsNullOrWhiteSpace(Id) || Id.Length > 64)
            return false;
        if (string.IsNullOrWhiteSpace(ItemId) || ItemId.Length > 80)
            return false;
        if (ItemId.IndexOfAny(new[] { '/', '\\', '\n', '\r' }) >= 0)
            return false;
        if (!float.IsFinite(X) || !float.IsFinite(Y) || !float.IsFinite(Z))
            return false;
        if (MathF.Abs(Y - HideoutPropertyPlan.FloorTopY) > 3f)
            return false;
        if (!HideoutPropertyPlan.ContainsInterior(X, Z))
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
        }

        return true;
    }
}

internal sealed class HideoutSaveFile
{
    public int Schema { get; set; } = HideoutPropertyPlan.Schema;
    public string PropertyCode { get; set; } = HideoutPropertyPlan.Code;
    public string SaveFolder { get; set; } = "";
    public bool Owned { get; set; }
    public List<HideoutPlacementRecord> Placements { get; set; } = new();
}

internal static class HideoutSaveRules
{
    internal static HideoutSaveKind Classify(HideoutSaveFile? file, string saveFolder, bool fileExists)
    {
        if (!fileExists)
            return HideoutSaveKind.Missing;
        if (file == null || file.Placements == null)
            return HideoutSaveKind.Corrupt;
        if (file.Schema != HideoutPropertyPlan.Schema)
            return HideoutSaveKind.Unsupported;
        if (!string.Equals(file.PropertyCode, HideoutPropertyPlan.Code, StringComparison.Ordinal))
            return HideoutSaveKind.Corrupt;
        if (string.IsNullOrWhiteSpace(saveFolder) || string.IsNullOrWhiteSpace(file.SaveFolder))
            return HideoutSaveKind.Corrupt;
        if (!string.Equals(Normalize(file.SaveFolder), Normalize(saveFolder), StringComparison.OrdinalIgnoreCase))
            return HideoutSaveKind.ForeignSlot;

        var ids = new HashSet<string>(StringComparer.Ordinal);
        foreach (var placement in file.Placements)
        {
            if (placement == null || !placement.IsPlausible() || !ids.Add(placement.Id))
                return HideoutSaveKind.Corrupt;
        }

        if (!file.Owned && file.Placements.Count > 0)
            return HideoutSaveKind.Corrupt;
        return HideoutSaveKind.Ready;
    }

    internal static string Normalize(string path) =>
        path.Replace('/', '\\').Trim().TrimEnd('\\');
}
