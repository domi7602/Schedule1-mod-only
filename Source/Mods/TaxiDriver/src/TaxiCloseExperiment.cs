using System;
using System.Reflection;
using S1API.PhoneApp;
using S1Mods.Shared;
using UnityEngine;

namespace TaxiDriver;

/// <summary>
/// DELAYED CLOSE (2026-09-29) - the fix for the transparent close gap, not an
/// experiment any more. Screenshot + user confirmation (2026-09-29): S1API's
/// SetAppOpen(false) hides AppsCanvas + _appContainer SYNCHRONOUSLY while the
/// phone mesh animates away (~1 s) - during that window the phone stands upright
/// with a dead screen. The fix defers the whole close by HideDelayFrames so the
/// screen keeps its content until the phone is effectively gone; a re-open
/// cancels the pending hide. Same shape as the proposed upstream fix.
/// </summary>
internal static class TaxiCloseExperiment
{
    internal static bool Armed { get; private set; }

    private static HarmonyLib.Harmony? _harmony;

    /// <summary>Frames the close is deferred (~0.75 s at 60 fps - the fold animation).</summary>
    private const int HideDelayFrames = 45;

    private static PhoneApp? PendingApp;
    private static int HideAtFrame;
    private static bool ApplyingHide;
    private static MethodInfo? SetAppOpenMethod;

    /// <summary>True while a close is waiting out its defer window.</summary>
    internal static bool DeferActive => PendingApp != null;

    internal static void Enable()
    {
        if (Armed)
            return;

        try
        {
            _harmony = new HarmonyLib.Harmony("com.taxidriver.closeexperiment");
            MethodBase? original = typeof(S1API.PhoneApp.PhoneApp).GetMethod(
                "SetAppOpen", BindingFlags.Instance | BindingFlags.NonPublic);
            MethodInfo? prefix = typeof(TaxiCloseExperiment).GetMethod(
                nameof(SetAppOpen_Prefix), BindingFlags.Static | BindingFlags.NonPublic);
            var prefixMethod = prefix == null ? null : new HarmonyLib.HarmonyMethod(prefix);
            if (PatchGuard.TryPatch(_harmony, original, prefixMethod, null, null, null, Mod.Log))
            {
                Armed = true;
                Mod.Log.Info("[close-exp] armed: Taxi skips S1API's immediate canvas hide on close.");
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[close-exp] arming failed ({ex.GetType().Name}: {ex.Message}).");
        }
    }

    private static bool SetAppOpen_Prefix(object __instance, bool open)
    {
        if (ApplyingHide)
            return true;                       // our own deferred close runs unmodified

        if (open)
        {
            if (PendingApp != null)
            {
                Mod.Log.Info("[close-exp] re-open inside the defer window - pending hide cancelled.");
                PendingApp = null;
            }
            return true;
        }

        if (__instance is not TaxiApp app)
            return true;

        try
        {
            // Mirror the non-visual bookkeeping the base would do.
            Il2CppScheduleOne.UI.Phone.Phone.ActiveApp = null;
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[close-exp] ActiveApp reset failed ({ex.GetType().Name}: {ex.Message}).");
        }

        PendingApp = app;
        HideAtFrame = Time.frameCount + HideDelayFrames;
        Mod.Log.Info($"[close-exp] close deferred {HideDelayFrames} frames - the screen keeps its content while the phone folds away.");
        return false;
    }

    /// <summary>Applies the deferred close once the window has passed (TaxiApp.Update).</summary>
    internal static void TickDeferredHide()
    {
        if (PendingApp == null || Time.frameCount < HideAtFrame)
            return;

        PhoneApp app = PendingApp;
        PendingApp = null;
        try
        {
            SetAppOpenMethod ??= typeof(PhoneApp).GetMethod("SetAppOpen", BindingFlags.Instance | BindingFlags.NonPublic);
            if (SetAppOpenMethod != null)
            {
                ApplyingHide = true;
                SetAppOpenMethod.Invoke(app, new object[] { false });
                ApplyingHide = false;
                Mod.Log.Info("[close-exp] deferred close applied - the screen dies together with the phone now.");
            }
            (app as TaxiApp)?.ForceHideBg();
        }
        catch (Exception ex)
        {
            ApplyingHide = false;
            (app as TaxiApp)?.ForceHideBg();
            Mod.Log.Warn($"[close-exp] deferred close failed ({ex.GetType().Name}: {ex.Message}) - content hidden directly.");
        }
    }
}
