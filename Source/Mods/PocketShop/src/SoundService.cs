using PocketShop.Config;
using S1Mods.Shared;

namespace PocketShop.Services;

/// <summary>
/// Handles native and procedural audio feedback for PocketShop actions
/// (purchase success, insufficient funds / inventory error, button clicks).
/// Delegated to S1Mods.Shared.AudioHelper with user config toggle checks.
/// </summary>
public static class SoundService
{
    public static void PlayPurchaseSuccess()
    {
        if (!PocketShopConfig.SoundEffectsEnabled) return;
        AudioHelper.PlayCashSound();
    }

    public static void PlayPurchaseDenied()
    {
        if (!PocketShopConfig.SoundEffectsEnabled) return;
        AudioHelper.PlayDenySound();
    }

    public static void PlayButtonClick()
    {
        if (!PocketShopConfig.SoundEffectsEnabled) return;
        AudioHelper.PlayClickSound();
    }
}
