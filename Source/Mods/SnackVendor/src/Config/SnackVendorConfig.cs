namespace SnackVendor.Config;

using System;

/// <summary>
/// Owns every tunable knob for the SnackVendor mod. Field defaults are the
/// source of truth on first launch — MelonPreferences will serialize them
/// to disk for the user via ModConfig&lt;T&gt; (see Mod.cs OnInitializeMelon).
/// Arrays / lists / dictionaries are NOT persisted here on purpose; the
/// stock-payload lives in the snacks_slot_{n}.json sidecar (see
/// Persistence/SnackVendorStore.cs). Mirrors the AutoPackagingStation split.
/// </summary>
public sealed class SnackVendorConfig
{
    public string StationItemId { get; set; } = "snackvendor";
    public string StationDisplayName { get; set; } = "Snack Vendor";
    public string StationDescription { get; set; } =
        "Player-stocked vending machine. Fill it with gas-market ingredients and NPCs buy them at the vanilla market price.";
    public string HardwareStoreStockShopName { get; set; } = "Handy Hank's";

    public int StationPurchasePrice { get; set; } = 1500;
    public float StationResellMultiplier { get; set; } = 0.5f;
    public int MaxStationsPerSave { get; set; } = 4;
    public int MaxIngredientSlots { get; set; } = 8;

    public bool ClampPrice { get; set; } = true;
    public float MinPayoutPerItem { get; set; } = 1f;
    public float MaxPayoutPerItem { get; set; } = 200f;

    public bool InjectIntoHardwareShop { get; set; } = true;

    /// <summary>
    /// credit the purchased ingredient into the buying NPC's inventory
    /// (captured via NPCSignal_UseVendingMachine.Purchase prefix). When false,
    /// purchases credit cash only and the ingredient vanishes (old MVP behavior).
    /// </summary>
    public bool CreditNpcInventory { get; set; } = true;

    /// <summary>Max distance (meters) between player and station for the deposit/extract panel to stay open.</summary>
    public float PanelRange { get; set; } = 3.5f;

    /// <summary>
    /// When true, allows placing the SnackVendor on the street / outside bought properties
    /// completely standalone without requiring HomelessMod. Strictly applies ONLY to the SnackVendor.
    /// </summary>
    public bool EnableStandaloneOutdoorPlacement { get; set; } = true;
}
