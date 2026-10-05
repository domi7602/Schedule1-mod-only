using HarmonyLib;
using Il2CppScheduleOne.UI.Phone;
using MelonLoader;

namespace TabToHome.Patches;

// TEMP-DIAG (wird nach der Diagnose wieder entfernt): alle anderen
// Phone-State-Einstiege mitloggen, um den Wegsteck-Auslöser zu finden, falls
// er nicht über SetIsOpen läuft.
[HarmonyPatch(typeof(Phone), nameof(Phone.RequestCloseApp))]
internal static class Phone_RequestCloseApp_TabToHome_Diag
{
    private static void Prefix(Phone __instance)
    {
        try
        {
            bool isOpen = false;
            try { isOpen = __instance != null && !__instance.WasCollected && __instance.IsOpen; }
            catch { }
            MelonLogger.Msg($"[TabToHome][DIAG] RequestCloseApp() frame={UnityEngine.Time.frameCount} IsOpen={isOpen}");
        }
        catch { }
    }
}

[HarmonyPatch(typeof(Phone), nameof(Phone.SetIsHorizontal))]
internal static class Phone_SetIsHorizontal_TabToHome_Diag
{
    private static void Prefix(bool h)
    {
        try { MelonLogger.Msg($"[TabToHome][DIAG] SetIsHorizontal(h={h}) frame={UnityEngine.Time.frameCount}"); }
        catch { }
    }
}

[HarmonyPatch(typeof(Phone), nameof(Phone.SetIsActiveGameplayScreen))]
internal static class Phone_SetIsActiveGameplayScreen_TabToHome_Diag
{
    private static void Prefix(bool isActive)
    {
        try { MelonLogger.Msg($"[TabToHome][DIAG] SetIsActiveGameplayScreen(a={isActive}) frame={UnityEngine.Time.frameCount}"); }
        catch { }
    }
}

[HarmonyPatch(typeof(Phone), nameof(Phone.SetLookOffsetMultiplier))]
internal static class Phone_SetLookOffsetMultiplier_TabToHome_Diag
{
    private static void Prefix(float multiplier)
    {
        try { MelonLogger.Msg($"[TabToHome][DIAG] SetLookOffsetMultiplier(m={multiplier}) frame={UnityEngine.Time.frameCount}"); }
        catch { }
    }
}
