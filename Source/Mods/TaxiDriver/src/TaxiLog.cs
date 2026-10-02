using System;
using S1Mods.Shared;

namespace TaxiDriver;

/// <summary>
/// Mod-local log split (2026-10-02). Info/Warn/Error stay unconditional; the automatic
/// diagnostic stream (navigation polls, ride heartbeat, one-shot dumps) goes through
/// <see cref="Verbose"/> and is quiet unless <c>UserData/TaxiDriver/log.json</c> sets
/// <c>VerboseLogging=true</c>. The file is written on first run so it can be hand-edited;
/// on-demand output (hotkeys, <c>taxi</c> diagnostics) is not affected either way.
/// </summary>
internal static class TaxiLog
{
    private sealed class LogConfig
    {
        /// <summary>Re-enables the full diagnostic stream (nav poll, heartbeat, dumps).</summary>
        public bool VerboseLogging { get; set; }
    }

    private static bool _loaded;
    private static LogConfig? _config;

    /// <summary>True when <c>log.json</c> enables the diagnostic stream.</summary>
    internal static bool VerboseEnabled { get; private set; }

    private static string FilePath => SafeStorage.GetUserDataPath("TaxiDriver", "log.json");

    /// <summary>Loads <c>log.json</c> once (idempotent); missing file = quiet + default written.</summary>
    internal static void Load()
    {
        if (_loaded)
            return;

        _loaded = true;
        try
        {
            _config = SafeStorage.LoadSafe(FilePath, new LogConfig(), Mod.Log);
            VerboseEnabled = _config.VerboseLogging;
            if (!System.IO.File.Exists(FilePath))
            {
                SafeStorage.SaveAtomic(FilePath, _config, Mod.Log);
                Mod.Log.Info($"[log] quiet by default - {FilePath} written (set VerboseLogging=true for the full diagnostic stream).");
            }
        }
        catch (Exception ex)
        {
            _config = null;
            Mod.Log.Warn($"[log] log.json load failed ({ex.Message}) - verbose logging stays off.");
        }
    }

    /// <summary>Diagnostic-only line: printed when <c>log.json</c> enables verbose logging.</summary>
    internal static void Verbose(string msg)
    {
        if (VerboseEnabled)
            Mod.Log.Info(msg);
    }
}
