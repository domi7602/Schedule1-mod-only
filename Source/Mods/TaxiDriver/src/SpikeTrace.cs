using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using HarmonyLib;
using Il2CppScheduleOne.Math;
using Il2CppScheduleOne.Vehicles.AI;
using UnityEngine;

namespace TaxiDriver;

/// <summary>
/// On-demand instrumentation for test finding 1 ("Navigate fails silently").
/// Four Harmony prefixes answer the decisive question — does a dispatched
/// <c>VehicleAgent.Navigate</c> reach the vanilla path calculation at all, and
/// what does the vanilla side report back?
///
/// <list type="bullet">
/// <item><c>taxi trace on</c> patches <c>VehicleAgent.Navigate</c>,
/// <c>NavigationUtility.CalculatePath</c>,
/// <c>VehicleAgent.NavigationCalculationCallback</c> and
/// <c>VehicleAgent.StopNavigating</c> through <c>PatchGuard</c> (a missing or
/// renamed method degrades to a logged warning, never a crash).</item>
/// <item><c>taxi trace off</c> only stops the logging — the prefixes stay installed
/// and become no-ops, so toggling never risks an unpatch mismatch.</item>
/// </list>
/// </summary>
internal static class SpikeTrace
{
    private static HarmonyLib.Harmony? _harmony;

    /// <summary>Prefix logging is gated by this flag; the patches themselves stay.</summary>
    private static volatile bool _logging;

    internal static bool Patched { get; private set; }

    internal static bool Logging => _logging;

    /// <summary>Applies the prefixes once and switches their logging on.</summary>
    internal static bool Enable()
    {
        if (!Patched)
        {
            _harmony ??= new HarmonyLib.Harmony("TaxiDriver.trace");

            bool navigate = S1Mods.Shared.PatchGuard.TryPatch(
                _harmony, typeof(VehicleAgent), nameof(VehicleAgent.Navigate),
                prefix: new HarmonyMethod(typeof(SpikeTrace), nameof(NavigatePrefix)), log: Mod.Log);

            bool calculatePath = S1Mods.Shared.PatchGuard.TryPatch(
                _harmony, typeof(NavigationUtility), nameof(NavigationUtility.CalculatePath),
                prefix: new HarmonyMethod(typeof(SpikeTrace), nameof(CalculatePathPrefix)), log: Mod.Log);

            // Round 10: the async result relay and the stop call are public and
            // managed — they are the only reliable observers of the native
            // Navigate body (CalculatePath is never dispatched through managed).
            bool navResult = S1Mods.Shared.PatchGuard.TryPatch(
                _harmony, typeof(VehicleAgent), nameof(VehicleAgent.NavigationCalculationCallback),
                prefix: new HarmonyMethod(typeof(SpikeTrace), nameof(NavCalcResultPrefix)), log: Mod.Log);

            bool stopNav = S1Mods.Shared.PatchGuard.TryPatch(
                _harmony, typeof(VehicleAgent), nameof(VehicleAgent.StopNavigating),
                prefix: new HarmonyMethod(typeof(SpikeTrace), nameof(StopNavigatingPrefix)), log: Mod.Log);

            Patched = navigate || calculatePath || navResult || stopNav;
            if (!Patched)
            {
                Mod.Log.Error("taxi trace: no VehicleAgent/NavigationUtility prefix could be patched — tracing unavailable.");
                return false;
            }

            Mod.Log.Info($"taxi trace: patches applied (Navigate={navigate}, CalculatePath={calculatePath}, NavigationCalculationCallback={navResult}, StopNavigating={stopNav}).");
            LogNativePointers();
        }

        _logging = true;
        Mod.Log.Info("taxi trace ON — every Navigate dispatch and every path calculation now logs one line.");
        return true;
    }

    /// <summary>Stops the log lines; the (harmless) prefixes remain installed.</summary>
    internal static void Disable()
    {
        _logging = false;
        Mod.Log.Info(Patched
            ? "taxi trace OFF — prefixes stay installed but are silent (`taxi trace on` re-enables the lines)."
            : "taxi trace OFF.");
    }

    // ------------------------------------------------------------ prefixes

    private static void NavigatePrefix(Vector3 location, NavigationSettings settings)
    {
        if (!_logging)
            return;

        Mod.Log.Info(
            $"[trace] VehicleAgent.Navigate(location=({SpikeCommands.Fmt(location)}), " +
            $"settings={(settings == null ? "null" : "NavigationSettings")}) entered (frame={Time.frameCount}) — " +
            "reaching this line proves the agent accepted the dispatch.");
    }

    private static void CalculatePathPrefix(Vector3 startPosition, Vector3 destination)
    {
        if (!_logging)
            return;

        Mod.Log.Info(
            $"[trace] NavigationUtility.CalculatePath(start=({SpikeCommands.Fmt(startPosition)}), " +
            $"dest=({SpikeCommands.Fmt(destination)})) entered (frame={Time.frameCount}) — " +
            "the vanilla path calculation IS running (dispatch reached the graph layer).");
    }

    /// <summary>Async path-search relay: Success/Failed + managed stack.</summary>
    private static void NavCalcResultPrefix(NavigationUtility.ENavigationCalculationResult result, PathSmoothingUtility.SmoothedPath _path)
    {
        if (!_logging)
            return;

        Mod.Log.Warn(
            $"[trace] VehicleAgent.NavigationCalculationCallback(result={result}, path={(_path == null ? "null" : "SmoothedPath")}) " +
            $"frame={Time.frameCount}\n{ShortStack()}");
    }

    /// <summary>Who ends a navigation (stack shows caller context).</summary>
    private static void StopNavigatingPrefix()
    {
        if (!_logging)
            return;

        Mod.Log.Warn($"[trace] VehicleAgent.StopNavigating() frame={Time.frameCount}\n{ShortStack()}");
    }

    private static string ShortStack()
    {
        try
        {
            var st = new StackTrace(1, false);
            var frames = st.GetFrames();
            if (frames == null || frames.Length == 0)
                return "    <no stack>";

            var sb = new StringBuilder();
            int n = 0;
            foreach (var f in frames)
            {
                var m = f.GetMethod();
                if (m == null)
                    continue;
                string type = m.DeclaringType?.FullName ?? "?";
                sb.Append("    at ").Append(type).Append('.').AppendLine(m.Name);
                if (++n >= 12)
                    break;
            }

            return sb.ToString().TrimEnd();
        }
        catch (Exception ex)
        {
            return "    <stacktrace failed: " + ex.Message + ">";
        }
    }

    /// <summary>
    /// Logs the IL2CPP native method pointers of the Navigate family (reflection
    /// over Il2CppInterop's NativeMethodInfoPtr_* statics). The VAs let us
    /// disassemble the real native body of Navigate from GameAssembly.dll —
    /// the only place that still holds the logic behind 'Failed'.
    /// </summary>
    private static void LogNativePointers()
    {
        try
        {
            const BindingFlags F = BindingFlags.NonPublic | BindingFlags.Static;
            foreach (var fi in typeof(VehicleAgent).GetFields(F))
            {
                if (!fi.Name.Contains("Navigate", StringComparison.OrdinalIgnoreCase))
                    continue;
                object? v = fi.GetValue(null);
                if (v is IntPtr p && p != IntPtr.Zero)
                    Mod.Log.Info($"[trace] native ptr {fi.Name} = 0x{p.ToInt64():X}");
                else
                    Mod.Log.Info($"[trace] native ptr {fi.Name} = {v ?? "null"}");
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"LogNativePointers failed: {ex.Message}");
        }
    }
}
