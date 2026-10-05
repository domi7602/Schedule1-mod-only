using System;
using HarmonyLib;
using Il2CppScheduleOne.UI.Phone;
using MelonLoader;

namespace TabToHome.Patches;

// Tab closes the active phone app to the HomeScreen instead of putting the phone
// away. The game routes the Tab put-away through Phone.SetIsOpen(false). When an
// app is open (Phone.ActiveApp != null), this prefix fires the same closeApps
// event the game would fire and then skips the put-away, so the phone stays up
// on the HomeScreen — the exact state Escape produces. A second Tab (no app
// open) passes through untouched and puts the phone away as usual.
//
// Notes:
// - Vanilla and S1API apps alike close via closeApps, so behavior matches a
//   normal Tab-close for every app; only the lowering animation is skipped.
// - If the game fires closeApps itself around SetIsOpen, handlers run twice.
//   S1API's CloseApp is idempotent (IsOpen check) and vanilla handlers are
//   close-guarded the same way, so a double close is a no-op.
// - Any failure falls through to the game's original put-away (return true),
//   so this patch can never trap the phone on screen.
[HarmonyPatch(typeof(Phone), nameof(Phone.SetIsOpen))]
internal static class Phone_SetIsOpen_TabToHome_Patch
{
    // Burst-Tracking statt starrem Fenster: Jede unterdrückte Schließung
    // verlängert das Echo-Fenster um EchoGapSeconds. Echte zweite Tabs
    // (>0,35 s Pause nach der letzten Aktivierung) laufen normal durch.
    // Redundante Öffnungen (o=True bei IsOpen) sind immer No-ops und werden
    // zeitlich unbegrenzt geschluckt – ein echtes Öffnen hat IsOpen == false.
    private const float EchoGapSeconds = 0.35f;
    private static float _burstEnd = -100000f;

    [HarmonyPrefix]
    private static bool Prefix(Phone __instance, bool o)
    {
        // TEMP-DIAG (wird nach der Diagnose wieder entfernt): jeden Aufruf mit
        // Frame und Zustand protokollieren.
        int frame = -1;
        try
        {
            string activeName = "null";
            try
            {
                var probe = Phone.ActiveApp;
                if (probe != null && !probe.WasCollected)
                    activeName = probe.name;
            }
            catch { activeName = "<err>"; }

            bool isOpen = false;
            try { isOpen = __instance != null && !__instance.WasCollected && __instance.IsOpen; }
            catch { }

            frame = UnityEngine.Time.frameCount;
            MelonLogger.Msg($"[TabToHome][DIAG] SetIsOpen(o={o}) frame={frame} IsOpen={isOpen} ActiveApp={activeName}");
        }
        catch { }

        bool isOpenNow = false;
        try { isOpenNow = __instance != null && !__instance.WasCollected && __instance.IsOpen; }
        catch { }

        // Redundantes Öffnen bei bereits offenem Phone: immer No-op, immer
        // schlucken (egal aus welcher Quelle oder mit welchem Abstand).
        // Echte Öffnungen haben IsOpen == false und sind nie betroffen.
        if (o && isOpenNow)
        {
            MelonLogger.Msg("[TabToHome][DIAG] redundant open suppressed.");
            return false;
        }
        if (o)
            return true; // opening the phone: never interfere

        // Burst-Tracking: Null-App-Schließung kurz nach Redirect oder letztem
        // Echo = derselbe Tastendruck (Release-Doppelfeuer/Tastenwiederholung
        // mit spielseitigem Toggle-Bit). Jede unterdrückte Schließung startet
        // das Fenster neu; ein bewusst zweiter Tab (Pause > 0,35 s) läuft durch.
        try
        {
            float now = Now();
            if (isOpenNow && now - _burstEnd <= EchoGapSeconds)
            {
                _burstEnd = now;
                MelonLogger.Msg("[TabToHome][DIAG] echo suppressed (burst).");
                return false;
            }
        }
        catch { }

        if (o)
            return true; // unreachable guard (o == false ab hier)

        try
        {
            if (__instance == null || __instance.WasCollected)
                return true;
            if (!__instance.IsOpen)
                return true;

            var activeApp = Phone.ActiveApp;
            if (activeApp == null || activeApp.WasCollected)
                return true; // HomeScreen: normal put-away

            __instance.closeApps?.Invoke();
            try { _burstEnd = Now(); }
            catch { }
            MelonLogger.Msg("[TabToHome] Tab redirected to HomeScreen (put-away suppressed).");
            return false; // skip the put-away
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[TabToHome] redirect failed ({ex.GetType().Name}), falling through to game put-away.");
            return true;
        }
    }

    private static float Now()
    {
        try { return UnityEngine.Time.realtimeSinceStartup; }
        catch { return UnityEngine.Time.frameCount / 60f; }
    }
}
