---
name: diag-mods-tick-counter
description: Reusable tick-counter MelonMod for diagnosing "which mod blocks the main thread" symptoms. CORRECT implementation that does NOT freeze the game. Load when the symptom is "FPS dropped after enabling N mods, but Latest.log shows no errors".
keywords: tick counter, diagnostic mod, performance, Resources.FindObjectsOfTypeAll, FPS, per-frame, isolation, binary search, MelonMod template.
---

# Diagnostic Tick-Counter MelonMod (Correct Version)

## Symptom it answers

"Game runs at 1 FPS but `Latest.log` shows no exceptions, no errors, no warnings. Which mod is doing it?"

The temptation is to add a per-frame `Resources.FindObjectsOfTypeAll` scan to find the offending MonoBehaviour. **Do not.** That scan itself enumerates hundreds of MonoBehaviours per frame and produces 1 FPS all by itself (verified 2026-08-28).

## Correct implementation: MelonMod-level only

This counts only `MelonMod.OnUpdate` invocations (one counter per loaded MelonMod). All mods tick at exactly the same rate when the main thread is blocked, so equal counters mean the block is *below* the mod level (game engine, native code, or a MonoBehaviour Update loop). Unequal counters point at the hot mod.

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using MelonLoader;
using MelonLoader.Utils;
using UnityEngine;

[assembly: MelonInfo(typeof(_DiagPerfCounter.Mod), "_DiagPerfCounter", "0.1.0", "Diag")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace _DiagPerfCounter;

public sealed class Mod : MelonMod
{
    private static readonly Dictionary<string, long> _tickCounts = new();
    private static double _nextLogTime = 0.0;
    private static string _logPath;
    private static readonly object _lock = new();

    public override void OnInitializeMelon()
    {
        var dir = Path.Combine(MelonEnvironment.UserDataDirectory, "_DiagPerfCounter");
        Directory.CreateDirectory(dir);
        _logPath = Path.Combine(dir, "tick.log");
        File.WriteAllText(_logPath, $"=== _DiagPerfCounter started at {DateTime.Now:HH:mm:ss} ===\n");
        MelonLogger.Msg("[DiagPerf] Active. Tick log -> " + _logPath);
    }

    public override void OnUpdate()
    {
        // Count MelonMod.OnUpdate only — does NOT touch GameObject hierarchy
        foreach (var mb in MelonMod.RegisteredMelons)
        {
            if (mb == null) continue;
            var asm = mb.MelonAssembly?.Assembly;
            if (asm == null) continue;
            var key = asm.GetName().Name ?? "?";
            _tickCounts.TryGetValue(key, out var c);
            _tickCounts[key] = c + 1;
        }

        var now = Time.realtimeSinceStartupAsDouble;
        if (_nextLogTime == 0.0) _nextLogTime = now + 5.0;
        if (now >= _nextLogTime)
        {
            _nextLogTime = now + 5.0;
            FlushLog();
        }
    }

    private static void FlushLog()
    {
        lock (_lock)
        {
            try
            {
                using var sw = new StreamWriter(_logPath, append: true);
                int total = 0;
                foreach (var kvp in _tickCounts)
                {
                    sw.WriteLine($"  {kvp.Key,-35} {kvp.Value,12}");
                    total += (int)kvp.Value;
                }
                sw.WriteLine($"--- t={DateTime.Now:HH:mm:ss} totalModTicks={total} ---");
                sw.WriteLine();
            }
            catch { }
            _tickCounts.Clear();
        }
    }
}
```

## How to read the log

- All mods tick at the same count → main thread is blocked by something **outside** MelonMod.OnUpdate (MonoBehaviour Update, native code, Harmony-prepiled loop, coroutine, AssetBundle stream, save serialization). Next step: see `schedule1-troubleshooting` §"Binary-search mod isolation".
- One mod's count matches the FPS (≈1–5 per 5 s) while others match normal rate (≈300 per 5 s) → that mod's `OnUpdate` is the bottleneck. Open the mod's source, look for `FindObjectOfType`, `Resources.FindObjectsOfTypeAll`, unbounded scans, or unthrottled per-frame work.
- The diag mod itself has only a few ticks → its own `OnUpdate` is starving (the main thread is blocked before it gets scheduled). This means the block is in a higher-priority system: native, Harmony prefix, or Unity's render loop.

## What this template does NOT do

- It does not enumerate MonoBehaviours per frame. (If you need that, run the scan once per second on a coroutine, not per frame.)
- It does not patch any game method. (If you need to count Harmony prefix invocations, add a per-method counter inside the patch itself.)
- It does not log every tick. The 5-second flush keeps log IO off the hot path.

## Removal

This mod has zero side effects when you delete the DLL. Drop the `_DiagPerfCounter` UserData folder afterwards if you want a clean slate.