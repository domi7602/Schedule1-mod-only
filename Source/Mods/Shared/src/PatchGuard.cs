using System;
using System.Reflection;
using System.Threading;
using HarmonyLib;

namespace S1Mods.Shared;

/// <summary>
/// Resilient Harmony patcher with graceful degradation.
/// Prevents mod crashes on game start if method signatures have changed
/// in game patches (e.g. Schedule I updates).
/// </summary>
public static class PatchGuard
{
    private static int _patchesApplied;
    private static int _patchesFailed;

    public static int PatchesApplied => Volatile.Read(ref _patchesApplied);
    public static int PatchesFailed => Volatile.Read(ref _patchesFailed);
    public static int TotalPatches => PatchesApplied + PatchesFailed;

    /// <summary>Resets the global patch counters.</summary>
    public static void ResetStats()
    {
        Interlocked.Exchange(ref _patchesApplied, 0);
        Interlocked.Exchange(ref _patchesFailed, 0);
    }

    /// <summary>
    /// Writes a structured summary of the applied patches to the log.
    /// Example: "PatchGuard: 8/8 patches successfully applied."
    /// </summary>
    public static void Report(ModLogger? log = null)
    {
        string msg = PatchesFailed == 0
            ? $"PatchGuard: {PatchesApplied}/{TotalPatches} patches successfully applied."
            : $"PatchGuard: {PatchesApplied}/{TotalPatches} patches applied ({PatchesFailed} failed - check warnings).";

        if (log != null)
        {
            if (PatchesFailed == 0) log.Info(msg);
            else log.Warn(msg);
        }
        else
        {
            if (PatchesFailed == 0) MelonLoader.MelonLogger.Msg(msg);
            else MelonLoader.MelonLogger.Warning(msg);
        }
    }

    /// <summary>
    /// Safely tries to patch a method with Harmony. On failure, logs and returns false (no crash).
    /// </summary>
    public static bool TryPatch(
        HarmonyLib.Harmony harmony,
        MethodBase? original,
        HarmonyMethod? prefix = null,
        HarmonyMethod? postfix = null,
        HarmonyMethod? transpiler = null,
        HarmonyMethod? finalizer = null,
        ModLogger? log = null)
    {
        if (harmony == null)
        {
            Interlocked.Increment(ref _patchesFailed);
            // Gatekeeper-fix 2026-08-29: only log via the provided `log` to avoid double-logging
            // (was: log?.Error + MelonLoader.MelonLogger.Error). Consistent with the other
            // paths in this file (lines 78, 116) which only use log?.Warn.
            log?.Error("PatchGuard: Harmony instance is null — patch skipped.");
            return false;
        }

        if (prefix == null && postfix == null && transpiler == null && finalizer == null)
        {
            Interlocked.Increment(ref _patchesFailed);
            string name = original?.Name ?? "unknown";
            log?.Warn($"PatchGuard: No HarmonyMethod provided for '{name}' — no-op, skipped.");
            return false;
        }

        if (original == null)
        {
            Interlocked.Increment(ref _patchesFailed);
            log?.Warn("PatchGuard: Target method is null (possibly removed by game patch). Patch skipped.");
            return false;
        }

        string targetDesc = $"{original.DeclaringType?.FullName ?? "Unknown"}.{original.Name}";
        try
        {
            harmony.Patch(original, prefix, postfix, transpiler, finalizer, null);
            Interlocked.Increment(ref _patchesApplied);
            log?.Debug($"PatchGuard: Successfully patched -> {targetDesc}");
            return true;
        }
        catch (Exception ex)
        {
            Interlocked.Increment(ref _patchesFailed);
            log?.Error($"PatchGuard: Patch for '{targetDesc}' failed (feature safely disabled): {ex.Message}");
            // Gatekeeper-fix 2026-08-29 (diagnostic fallback only): some ModLogger implementations
            // swallow errors during very-early Init (before their sink is fully wired). The throw
            // path is the only place that needs this fallback — other paths (harmony == null,
            // original == null, no-op) are informational and `log?.Warn/Error` is sufficient.
            // Removing this would silently hide the one-patch-fail in StackLimitMod (20/21) etc.
            try { MelonLoader.MelonLogger.Error($"[PatchGuard] Patch for '{targetDesc}' failed: {ex.GetType().Name}: {ex.Message}"); } catch { }
            return false;
        }
    }

    /// <summary>
    /// Finds a method via reflection and patches it safely. Supports prefix, postfix, transpiler, and finalizer.
    /// </summary>
    public static bool TryPatch(
        HarmonyLib.Harmony harmony,
        Type targetType,
        string methodName,
        HarmonyMethod? prefix = null,
        HarmonyMethod? postfix = null,
        HarmonyMethod? transpiler = null,
        HarmonyMethod? finalizer = null,
        Type[]? parameterTypes = null,
        ModLogger? log = null)
    {
        MethodInfo? method = FindMethod(targetType, methodName, parameterTypes, log: log);
        if (method == null)
        {
            Interlocked.Increment(ref _patchesFailed);
            log?.Warn($"PatchGuard: Method '{targetType?.Name}.{methodName}' not found. Signature may have changed after a game patch.");
            return false;
        }

        return TryPatch(harmony, method, prefix, postfix, transpiler, finalizer, log);
    }

    /// <summary>
    /// Backwards-compatible overload for calls with parameterTypes before log.
    /// </summary>
    public static bool TryPatch(
        HarmonyLib.Harmony harmony,
        Type targetType,
        string methodName,
        HarmonyMethod? prefix,
        HarmonyMethod? postfix,
        Type[]? parameterTypes,
        ModLogger? log)
    {
        return TryPatch(harmony, targetType, methodName, prefix, postfix, null, null, parameterTypes, log);
    }

    /// <summary>
    /// Safely searches for a method with any visibility (public/non-public, instance/static).
    /// For ambiguous overloads without parameterTypes, returns null and logs a warning (no guessing).
    /// </summary>
    public static MethodInfo? FindMethod(
        Type targetType,
        string methodName,
        Type[]? parameterTypes = null,
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static,
        ModLogger? log = null)
    {
        if (targetType == null || string.IsNullOrEmpty(methodName))
            return null;

        try
        {
            if (parameterTypes != null)
                return targetType.GetMethod(methodName, flags, null, parameterTypes, null);

            return targetType.GetMethod(methodName, flags);
        }
        catch (AmbiguousMatchException)
        {
            log?.Warn($"PatchGuard: Multiple overloads found for '{targetType.Name}.{methodName}'. Please provide explicit 'parameterTypes' instead of guessing.");
            return null;
        }
        catch (Exception ex)
        {
            log?.Warn($"PatchGuard: Error searching for '{targetType.Name}.{methodName}': {ex.Message}");
            return null;
        }
    }
}
