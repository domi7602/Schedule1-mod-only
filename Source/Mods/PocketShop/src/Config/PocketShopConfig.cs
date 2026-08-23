using S1Mods.Shared;

namespace PocketShop.Config;

public enum PaymentMode
{
    Auto = 0,
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

    /// <summary>Active payment mode (Auto, Cash, or Bank).</summary>
    public PaymentMode SelectedPaymentMode { get; set; } = PaymentMode.Auto;

    /// <summary>Whether audio feedback is enabled for UI clicks, purchases, and alerts.</summary>
    public bool EnableSoundEffects { get; set; } = true;

    /// <summary>Index of the last selected shop, restored on next open (0-based, clamped on load).</summary>
    public int LastShopIndex { get; set; } = 0;

    /// <summary>Static accessor for ServiceFeePercent so UI code can read it directly.</summary>
    public static float ServiceFeePercentStatic => ModConfig<PocketShopConfig>.Instance?.ServiceFeePercent ?? 10f;

    /// <summary>Static accessor for DeliveryFeeFlat.</summary>
    public static float DeliveryFeeFlatStatic => ModConfig<PocketShopConfig>.Instance?.DeliveryFeeFlat ?? 0f;

    /// <summary>Static accessor for SelectedPaymentMode.</summary>
    public static PaymentMode PaymentModeStatic
    {
        get => ModConfig<PocketShopConfig>.Instance?.SelectedPaymentMode ?? PaymentMode.Auto;
        set
        {
            if (ModConfig<PocketShopConfig>.Instance != null)
            {
                ModConfig<PocketShopConfig>.SetAndSave("SelectedPaymentMode", value);
            }
        }
    }

    /// <summary>Static accessor for EnableSoundEffects.</summary>
    public static bool SoundEffectsEnabled => ModConfig<PocketShopConfig>.Instance?.EnableSoundEffects ?? true;
}
