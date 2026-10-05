using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;

namespace S1MCPServer.Utils;

/// <summary>
/// In-memory ring buffer of MelonLoader log lines INCLUDING [DEBUG] level
/// (which never reaches Latest.log, so capture_logs on the file alone hides
/// exactly the diagnostics that matter most).
///
/// Entry points are discovered at runtime by NAME on MelonLoader.MelonLogger /
/// MelonLoader.MelonDebug and hooked with Harmony postfixes - tolerant to
/// MelonLoader version differences: every entry point found is hooked, and
/// when none is found the capture stays empty and that fact is logged once.
/// Consecutive duplicate lines (delegate chains like Msg(string) ->
/// Msg(Color, string)) are collapsed.
/// </summary>
public static class ModLogCapture
{
    private const int Capacity = 4000;
    private static readonly ConcurrentQueue<KeyValuePair<string, string>> _ring = new();
    private static readonly object _gate = new();
    private static string _lastContent;
    private static bool _installed;

    /// <summary>Installs the log hooks (idempotent). Returns how many entry points were hooked.</summary>
    public static int Install()
    {
        lock (_gate)
        {
            if (_installed) return 0;
            _installed = true;
        }

        int hooked = 0;
        var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Msg", "Debug", "Warning", "Error", "Internal_Msg", "Log"
        };
        var harmony = new HarmonyLib.Harmony("S1MCPServer.logcapture");

        foreach (var type in ResolveLogTypes())
        {
            MethodInfo[] methods;
            try { methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static); }
            catch { continue; }
            foreach (var method in methods)
            {
                if (!names.Contains(method.Name)) continue;
                if (method.IsGenericMethodDefinition) continue;
                try
                {
                    harmony.Patch(method, postfix: new HarmonyLib.HarmonyMethod(typeof(ModLogCapture), nameof(Postfix)));
                    hooked++;
                }
                catch { /* entry point unusable - try the others */ }
            }
        }

        ModLogger.Info($"ModLogCapture hooked {hooked} MelonLoader log entry point(s)");
        return hooked;
    }

    private static IEnumerable<Type> ResolveLogTypes()
    {
        var found = new List<Type>();
        try { found.Add(typeof(MelonLoader.MelonLogger)); } catch { }
        try
        {
            var asm = typeof(MelonLoader.MelonLogger).Assembly;
            var debug = asm.GetType("MelonLoader.MelonDebug");
            if (debug != null) found.Add(debug);
        }
        catch { }
        return found;
    }

    private static void Postfix(object[] __args, MethodBase __originalMethod)
    {
        try
        {
            string level = __originalMethod != null ? __originalMethod.Name.ToUpperInvariant() : "LOG";
            var parts = new List<string>();
            if (__args != null)
            {
                foreach (var arg in __args)
                {
                    if (arg == null || arg is Enum) continue;
                    parts.Add(arg.ToString());
                }
            }
            if (parts.Count == 0) return;
            string content = string.Join(" ", parts);

            lock (_gate)
            {
                if (content == _lastContent) return; // collapse delegate-chain duplicates
                _lastContent = content;
            }

            _ring.Enqueue(new KeyValuePair<string, string>(
                DateTime.Now.ToString("HH:mm:ss.fff"), $"[{level}] {content}"));
            while (_ring.Count > Capacity && _ring.TryDequeue(out _)) { }
        }
        catch { /* logging must never break the caller */ }
    }

    /// <summary>Snapshot of captured lines (optionally only the last N), oldest first.</summary>
    public static List<KeyValuePair<string, string>> Snapshot(int? lastN)
    {
        var all = new List<KeyValuePair<string, string>>(_ring);
        if (lastN.HasValue && lastN.Value > 0 && all.Count > lastN.Value)
            all = all.GetRange(all.Count - lastN.Value, lastN.Value);
        return all;
    }
}
