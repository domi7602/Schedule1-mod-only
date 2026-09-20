using S1Mods.Shared;

namespace PocketShop.Config;

public enum PaymentMode
{
    /// <summary>Legacy v0.2.x mode — removed in v0.3.0 (Schedule I: legal shops are card-only). Kept for config compat.</summary>
    Auto = 0,
    /// <summary>Legacy v0.2.x mode — removed in v0.3.0 (Schedule I: legal shops are card-only). Kept for config compat.</summary>
    Cash = 1,
    Bank = 2
}

/// <summary>
/// Persisted settings for PocketShop. Loaded/saved via Shared.ModConfig&lt;T&gt;
/// (MelonPreferences under the hood). Add new settings as auto-properties
/// and they will be picked up automatically on next save.
/// </summary>
public class PocketShopConfig
{
    /// <summary>Service fee percent added on top of vanilla shop price (e.g. 10 = +10%).</summary>
    public float ServiceFeePercent { get; set; } = 10f;

    /// <summary>Flat delivery surcharge added per order (e.g. 0 = none).</summary>
    public float DeliveryFeeFlat { get; set; } = 0f;

    /// <summary>
    /// Legacy setting from the v0.2.x multi-payment era. v0.3.0 is Bank-card-only
    /// (Schedule I pays legal shops by card); the value is retained for config
    /// compatibility but no longer affects payment behavior.
    /// </summary>
    public PaymentMode SelectedPaymentMode { get; set; } = PaymentMode.Bank;

    /// <summary>Whether audio feedback is enabled for UI clicks, purchases, and alerts.</summary>
    public bool EnableSoundEffects { get; set; } = true;

    /// <summary>Index of the last selected shop, restored on next open (0-based, clamped on load).</summary>
    public int LastShopIndex { get; set; } = 0;

    /// <summary>Whether items that require a higher player rank/level are locked from purchase (matching vanilla shops).</summary>
    public bool EnforceLevelRequirements { get; set; } = true;

    /// <summary>Static accessor for EnforceLevelRequirements.</summary>
    public static bool EnforceLevelRequirementsStatic => ModConfig<PocketShopConfig>.Instance?.EnforceLevelRequirements ?? true;

    /// <summary>Static accessor for ServiceFeePercent so UI code can read it directly.</summary>
    public static float ServiceFeePercentStatic => ModConfig<PocketShopConfig>.Instance?.ServiceFeePercent ?? 10f;

    /// <summary>Static accessor for DeliveryFeeFlat.</summary>
    public static float DeliveryFeeFlatStatic => ModConfig<PocketShopConfig>.Instance?.DeliveryFeeFlat ?? 0f;

    /// <summary>
    /// Static accessor for SelectedPaymentMode. v0.3.0: always resolves to Bank —
    /// PocketShop is a legal storefront, Schedule I charges legal shops by card.
    /// The setter is a no-op kept for source compatibility.
    /// </summary>
    public static PaymentMode PaymentModeStatic
    {
        get => PaymentMode.Bank;
        set { /* v0.3.0: Bank-only — legacy setter kept for compile compat */ }
    }

    /// <summary>Static accessor for EnableSoundEffects.</summary>
    public static bool SoundEffectsEnabled => ModConfig<PocketShopConfig>.Instance?.EnableSoundEffects ?? true;
}
