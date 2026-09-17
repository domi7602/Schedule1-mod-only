using System.Collections.Generic;

namespace SnackVendor.Persistence;

/// <summary>
/// Serialized representation of a single ingredient slot inside one
/// Snack-Vendor station. Mirrors AutoPack's SlotItemData (state id,
/// quantity, quality) but trimmed to what NPCs actually buy:
/// quantity + the underlying ingredient id. Quality tier is kept for
/// the cash formula (BasePurchasePrice * QualityMultiplier — same
/// as the gas market uses) but not persisted here.
/// </summary>
public class SnackSlotData
{
    public string IngredientId { get; set; } = string.Empty;
    public int Quantity { get; set; } = 0;
    public int QualityTier { get; set; } = 2; // 0=Trash, 1=Poor, 2=Standard, 3=Premium, 4=Heavenly
}

/// <summary>
/// Persistent state for one Snack-Vendor machine in the world.
/// Keyed by the spawned VendingMachine-InstanceID (not the GUID,
/// because vanilla VendingMachines regenerate their GUID at Awake
/// when not baked into the scene — see Awake_Public_Virtual_Void_0 in
/// the decompiled proxy; we pin the BakedGUID ourselves after spawn
/// to keep load->save stable).
/// </summary>
public class SnackStationSaveData
{
    public string InstanceGuid { get; set; } = string.Empty; // Stable ID across save/load
    public float[] Position { get; set; } = new float[3];
    public float[] Rotation { get; set; } = new float[4];
    public List<SnackSlotData> Slots { get; set; } = new();
    public bool Initialized { get; set; } = false; // consumed by load to suppress purchase on first frame
    public long LastInteractionDay { get; set; } = -1; // day on which the stock last changed; -1 = untouched
}

/// <summary>
/// Root payload for a save-slot file. Slot-specific (one file per
/// save slot via snacks_slot_{n}.json pattern, never shared).
/// </summary>
public class SnackVendorSaveFile
{
    public List<SnackStationSaveData> Stations { get; set; } = new();
}
