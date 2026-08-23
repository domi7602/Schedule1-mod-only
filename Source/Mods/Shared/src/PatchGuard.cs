using System;
using System.Reflection;
using HarmonyLib;

namespace S1Mods.Shared;

/// <summary>
/// Resilienter Harmony-Patcher mit Graceful Degradation.
/// Verhindert Mod-Crashes beim Spielstart, falls sich Methodensignaturen
/// in Game-Patches (z. B. Schedule I Updates) geändert haben.
/// </summary>
public static class PatchGuard
{
    public static int PatchesApplied { get; private set; }
    public static int PatchesFailed { get; private set; }
    public static int TotalPatches => PatchesApplied + PatchesFailed;

    /// <summary>Setzt die globalen Patch-Zähler zurück.</summary>
    public static void ResetStats()
    {
        PatchesApplied = 0;
        PatchesFailed = 0;
    }

    /// <summary>
    /// Schreibt eine strukturierte Zusammenfassung der angewendeten Patches in das Log.
    /// Beispiel: "PatchGuard: 8/8 patches successfully applied."
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
    /// Versucht eine Methode sicher mit Harmony zu patchen. Bei Fehlern wird
    /// geloggt und false zurückgegeben (kein Crash).
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
            PatchesFailed++;
            log?.Error("PatchGuard: Harmony instance is null — patch übersprungen.");
            try { MelonLoader.MelonLogger.Error("[PatchGuard] Harmony instance is null — patch übersprungen."); } catch { }
            return false;
        }

        if (prefix == null && postfix == null && transpiler == null && finalizer == null)
        {
            string name = original?.Name ?? "unknown";
            log?.Warn($"PatchGuard: Keine HarmonyMethod für '{name}' angegeben — No-Op, übersprungen.");
            return false;
        }

        if (original == null)
        {
            PatchesFailed++;
            log?.Warn("PatchGuard: Ziel-Methode ist null (möglicherweise durch Game-Patch entfernt). Patch übersprungen.");
            return false;
        }

        string targetDesc = $"{original.DeclaringType?.FullName ?? "Unknown"}.{original.Name}";
        try
        {
            harmony.Patch(original, prefix, postfix, transpiler, finalizer, null);
            PatchesApplied++;
            log?.Debug($"PatchGuard: Erfolgreich gepatcht -> {targetDesc}");
            return true;
        }
        catch (Exception ex)
        {
            PatchesFailed++;
            log?.Error($"PatchGuard: Patch für '{targetDesc}' fehlgeschlagen (Feature sicher deaktiviert): {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Sucht eine Methode via Reflection und patcht sie sicher. Unterstützt Prefix, Postfix, Transpiler und Finalizer.
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
            PatchesFailed++;
            log?.Warn($"PatchGuard: Methode '{targetType?.Name}.{methodName}' nicht gefunden. Signatur nach Game-Patch evtl. geändert.");
            return false;
        }

        return TryPatch(harmony, method, prefix, postfix, transpiler, finalizer, log);
    }

    /// <summary>
    /// Abwärtskompatibler Overload für Aufrufe mit parameterTypes vor log.
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
    /// Sucht sicher nach einer Methode mit beliebiger Visibility (Public/NonPublic, Instance/Static).
    /// Bei mehrdeutigen Überladungen ohne parameterTypes wird null zurückgegeben und eine Warnung geloggt (kein Ratespiel).
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
            log?.Warn($"PatchGuard: Mehrere Überladungen für '{targetType.Name}.{methodName}' gefunden. Bitte explizite 'parameterTypes' angeben statt Ratespiel.");
            return null;
        }
        catch (Exception ex)
        {
            log?.Warn($"PatchGuard: Fehler bei Suche nach '{targetType.Name}.{methodName}': {ex.Message}");
            return null;
        }
    }
}
