using BankApp.Config;
using S1Mods.Shared;

namespace BankApp.Services;

/// <summary>
/// Handles audio feedback for BankApp actions (transaction success, insufficient funds / limits, UI clicks).
/// Delegated to S1Mods.Shared.AudioHelper with user config toggle checks.
/// </summary>
public static class BankSoundService
{
    public static void PlayCashSuccess()
    {
        if (!ModConfig<BankAppConfig>.Instance.EnableSoundEffects) return;
        AudioHelper.PlayCashSound();
    }

    public static void PlayError()
    {
        if (!ModConfig<BankAppConfig>.Instance.EnableSoundEffects) return;
        AudioHelper.PlayDenySound();
    }

    public static void PlayClick()
    {
        if (!ModConfig<BankAppConfig>.Instance.EnableSoundEffects) return;
        AudioHelper.PlayClickSound();
    }
}
