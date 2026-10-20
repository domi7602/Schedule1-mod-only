using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MelonLoader;

namespace S1Mods.Shared;

/// <summary>
/// Fail-visible diagnostics for code that runs inside game hooks.
///
/// Two problems this solves, both learned from reading third-party mods after a
/// game update:
/// <list type="bullet">
/// <item><description>
/// A hook that throws every frame floods the log until the real cause is unfindable.
/// <see cref="ErrorOnce"/> logs the first failure with the full exception and then
/// stays silent, counting repeats instead.
/// </description></item>
/// <item><description>
/// Silent degradation looks identical to a working mod. <see cref="StartupReport"/>
/// prints one compact block at startup with the patch counters from
/// <see cref="PatchGuard"/> plus the names of every patch that did not apply, so a
/// broken game-update is visible in the first ten lines of <c>MelonLoader/Latest.log</c>.
/// </description></item>
/// </list>
///
/// Use this instead of a bare <c>try { } catch { }</c> around hook bodies, and instead
/// of logging an exception unconditionally inside a per-frame path.
/// </summary>
public static class Diagnostics
{
    /// <summary>Repeats of the same failure before a reminder line is logged again.</summary>
    public const int RepeatLogEvery = 25;

    private static readonly object Gate = new();
    private static readonly Dictionary<string, int> Hits = new(StringComparer.Ordinal);
    private static readonly HashSet<string> PrimaryLogged = new(StringComparer.Ordinal);

    /// <summary>
    /// Logs <paramref name="ex"/> the first time <paramref name="where"/> fails, then only every
    /// <see cref="RepeatLogEvery"/>th repeat. Never throws (logging must not break a hook).
    /// </summary>
    /// <param name="where">Stable call-site key, e.g. "Wave.Tick" or "ShopListingSync.Refresh".</param>
    public static void ErrorOnce(string where, Exception ex, ModLogger? log = null)
    {
        if (string.IsNullOrEmpty(where)) where = "unknown";

        int count;
        bool first;
        bool milestone;
        lock (Gate)
        {
            count = Hits.TryGetValue(where, out int seen) ? seen + 1 : 1;
            Hits[where] = count;
            first = PrimaryLogged.Add(where);
            milestone = !first && count % RepeatLogEvery == 0;
        }

        if (first)
        {
            Emit(log, where, ex, isError: true);
        }
        else if (milestone)
        {
            EmitLine(log, isError: false, $"{where}: still failing ({count} hits since start)");
        }
    }

    /// <summary>
    /// Same de-duplication as <see cref="ErrorOnce"/> for non-exception warnings
    /// (e.g. "target method not found — feature disabled").
    /// </summary>
    public static void WarnOnce(string where, string message, ModLogger? log = null)
    {
        if (string.IsNullOrEmpty(where)) where = "unknown";

        int count;
        bool first;
        lock (Gate)
        {
            string key = "w:" + where;
            count = Hits.TryGetValue(key, out int seen) ? seen + 1 : 1;
            Hits[key] = count;
            first = PrimaryLogged.Add(key);
        }

        if (first)
        {
            EmitLine(log, isError: false, $"{where}: {message}");
        }
        else if (count % RepeatLogEvery == 0)
        {
            EmitLine(log, isError: false, $"{where}: still warning ({count} hits since start): {message}");
        }
    }

    /// <summary>How often <paramref name="where"/> failed after its first, already-logged occurrence.</summary>
    public static int SuppressedCount(string where)
    {
        if (string.IsNullOrEmpty(where)) return 0;
        lock (Gate)
        {
            int hits = Hits.TryGetValue(where, out int n) ? n : 0;
            return hits > 1 ? hits - 1 : 0;
        }
    }

    /// <summary>Total number of failures that were counted but not logged individually.</summary>
    public static int SuppressedTotal
    {
        get
        {
            lock (Gate)
            {
                int total = 0;
                foreach (KeyValuePair<string, int> entry in Hits)
                {
                    if (entry.Value > 1) total += entry.Value - 1;
                }
                return total;
            }
        }
    }

    /// <summary>
    /// Writes one line per call site that failed more than once since the last
    /// <see cref="Reset"/>. Safe to call repeatedly; it does not clear the counters.
    /// </summary>
    public static void FlushSummary(ModLogger? log = null)
    {
        List<string> lines = new();
        lock (Gate)
        {
            foreach (KeyValuePair<string, int> entry in Hits.OrderBy(e => e.Key, StringComparer.Ordinal))
            {
                if (entry.Value > 1)
                {
                    // Warning keys carry a "w:" prefix so they do not collide with error call sites.
                    string key = entry.Key.StartsWith("w:", StringComparison.Ordinal)
                        ? entry.Key.Substring(2)
                        : entry.Key;
                    lines.Add($"{key} failed {entry.Value} times (1 logged, {entry.Value - 1} suppressed)");
                }
            }
        }

        if (lines.Count == 0) return;
        foreach (string line in lines) EmitLine(log, isError: false, "Diagnostics: " + line);
    }

    /// <summary>Clears every counter (test seam and mod-deinitialize helper).</summary>
    public static void Reset()
    {
        lock (Gate)
        {
            Hits.Clear();
            PrimaryLogged.Clear();
        }
    }

    /// <summary>
    /// One compact startup block: mod identity, patch counters from <see cref="PatchGuard"/>,
    /// the names of failed patch targets, and any caller-supplied lines.
    /// Call once at the end of <c>OnInitializeMelon</c>, after all patches were registered.
    /// </summary>
    public static void StartupReport(
        string modName,
        string version,
        ModLogger? log = null,
        IEnumerable<string>? extraLines = null)
    {
        if (string.IsNullOrEmpty(modName)) modName = "Mod";

        List<string> lines = new()
        {
            $"{modName} {version}",
            $"patches   {PatchGuard.PatchesApplied}/{PatchGuard.TotalPatches} applied" +
                (PatchGuard.PatchesFailed > 0 ? $"  ({PatchGuard.PatchesFailed} failed)" : string.Empty)
        };

        IReadOnlyList<string> failed = PatchGuard.FailedTargets;
        if (failed.Count > 0)
        {
            // Keep the block readable in a log that already has hundreds of lines.
            const int MaxListed = 6;
            string listed = string.Join(", ", failed.Take(MaxListed));
            if (failed.Count > MaxListed) listed += $" (+{failed.Count - MaxListed} more)";
            lines.Add("failed    " + listed);
        }

        if (extraLines != null)
        {
            foreach (string extra in extraLines)
            {
                if (!string.IsNullOrWhiteSpace(extra)) lines.Add(extra);
            }
        }

        int width = lines.Max(l => l.Length) + 2;
        string rule = new('═', width);
        var sb = new StringBuilder();
        sb.AppendLine(rule);
        foreach (string line in lines) sb.AppendLine(" " + line.PadRight(width - 1));
        sb.Append(rule);

        // One logger call per line: MelonLoader prefixes only the first line of a
        // multi-line message, which makes pasted log excerpts ambiguous.
        foreach (string line in sb.ToString().Split('\n'))
        {
            string trimmed = line.TrimEnd('\r');
            if (trimmed.Length > 0) EmitLine(log, isError: false, trimmed);
        }
        EmitLine(log, isError: PatchGuard.PatchesFailed > 0,
            PatchGuard.PatchesFailed > 0
                ? $"{modName}: {PatchGuard.PatchesFailed} patch(es) did not apply — related features are disabled."
                : $"{modName}: all patches applied.");
    }

    private static void Emit(ModLogger? log, string where, Exception ex, bool isError)
    {
        string message = where + ": " + ex;
        if (log != null)
        {
            if (isError) log.Error(message); else log.Warn(message);
            return;
        }
        try
        {
            if (isError) MelonLogger.Error("[Diagnostics] " + message);
            else MelonLogger.Warning("[Diagnostics] " + message);
        }
        catch
        {
            // Logging during very early init can fail; diagnostics must never throw.
        }
    }

    private static void EmitLine(ModLogger? log, bool isError, string message)
    {
        try
        {
            if (log != null)
            {
                if (isError) log.Warn(message); else log.Info(message);
                return;
            }
            if (isError) MelonLogger.Warning("[Diagnostics] " + message);
            else MelonLogger.Msg("[Diagnostics] " + message);
        }
        catch
        {
            // See Emit.
        }
    }
}
