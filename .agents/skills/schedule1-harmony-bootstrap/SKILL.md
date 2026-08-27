---
name: schedule1-harmony-bootstrap
description: >-
  Auto-discover all HarmonyPatch classes in a mod assembly, apply them with per-class guards and applied/skipped/failed counters, and unpatch cleanly on shutdown. Use when a mod has many patches across many files and a single bootstrap call beats manual PatchAll.
  Keywords: PatchClassProcessor, HarmonyBootstrap, PatchTargetGuard, IsAllowed, applied/skipped/failed counter, auto-discovery, LogHealth, UnpatchSelf, HarmonyId.
---

# Schedule I — Harmony Bootstrap (Auto-Discover All Patch Classes)

> **Source:** distilled from decompiling `LegalProduce 0.4.1` (`LegalProduce.Util.Harmony.HarmonyBootstrap` + `PatchTargetGuard`). Verified 2026-08-26.
>
> **The trap:** A mod with 30 patches across 15 files calling `harmony.PatchAll()` silently no-ops if a single target method moved (game patch, signature change). The user gets "mod loaded" with **zero effect** and no error in the log. Worse — `harmony.PatchAll` throws and *kills the entire mod init* on the first bad patch.
>
> **The fix:** Walk the assembly yourself, apply patches one-by-one, count outcomes, log a health summary. Failed patches get logged by full type name + reason; mod init completes; user can see exactly what's broken.

---

## 1. The 3 Components

```
┌────────────────────────────────┐
│ HarmonyBootstrap.Apply(asm)    │  called once in OnInitializeMelon
└──────────────┬─────────────────┘
               │ finds [HarmonyPatch] classes
               ▼
┌────────────────────────────────┐
│ PatchTargetGuard.IsAllowed(t)  │  per-class: pre-flight check
│  → AccessTools.Method(...)     │     target still exists?
│  → signature still matches?    │     target still in scene?
└──────────────┬─────────────────┘
               │ IsAllowed=true → apply; false → skip+log
               ▼
┌────────────────────────────────┐
│ PatchClassProcessor.Patch()    │  wrapped in try/catch per class
│ → _appliedCount++              │
│ → on exception: _failedCount++ │  logged with FULL type name
└────────────────────────────────┘
```

---

## 2. Drop-in Implementation

Place in `Source/Mods/Shared/src/HarmonyBootstrap.cs`:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using MelonLoader;

namespace S1Mods.Shared.Harmony;

/// <summary>
/// Auto-discovers all [HarmonyPatch] classes in the mod assembly and applies them
/// with per-class guards and outcome tracking. Logs a health summary on demand.
/// </summary>
public static class HarmonyBootstrap
{
    private static Harmony? _harmony;
    private static string? _harmonyId;
    private static int _appliedCount;
    private static int _failedCount;
    private static int _skippedCount;

    public static bool IsApplied => _harmony != null && _appliedCount > 0;
    public static int AppliedCount => _appliedCount;
    public static int FailedCount => _failedCount;
    public static int SkippedCount => _skippedCount;

    /// <summary>Override the harmony id (default = the MelonMod's HarmonyId).</summary>
    public static void Configure(string harmonyId) {
        if (!string.IsNullOrWhiteSpace(harmonyId)) _harmonyId = harmonyId;
    }

    /// <summary>Apply from MelonMod instance. Call from OnInitializeMelon.</summary>
    public static void ApplyFromMelon(MelonMod mod) {
        if (mod == null) { Log("MelonMod is null"); return; }
        Apply(((MelonBase)mod).MelonAssembly.Assembly);
    }

    public static void Apply(Assembly assembly, Harmony? harmony = null) {
        if (_harmony != null) return;          // idempotent — second call is no-op
        _harmonyId ??= assembly.GetName().Name + ".AutoPatches";
        _harmony = harmony ?? new Harmony(_harmonyId);
        ApplyPatchClasses(assembly);
    }

    private static void ApplyPatchClasses(Assembly assembly) {
        _appliedCount = _failedCount = _skippedCount = 0;

        var patchTypes = assembly.GetTypes()
            .Where(t => t.GetCustomAttributes(typeof(HarmonyPatch), inherit: false).Length != 0)
            .OrderBy(t => t.FullName, StringComparer.Ordinal);

        foreach (var patchType in patchTypes) {
            if (!PatchTargetGuard.IsAllowed(patchType, out var skipReason)) {
                _skippedCount++;
                Log($"skip {patchType.FullName}: {skipReason}");
                continue;
            }
            try {
                new PatchClassProcessor(_harmony, patchType).Patch();
                _appliedCount++;
            } catch (Exception ex) {
                _failedCount++;
                // CRITICAL: include the FULL type name so the user can locate the file
                Log($"FAIL {patchType.FullName}: {ex.GetType().Name} {ex.Message}");
            }
        }

        if (_appliedCount == 0) {
            Log("ERROR: zero patches applied — mod hooks inactive. Removing.");
            Remove();
            return;
        }
        LogHealth();
    }

    public static void LogHealth() {
        if (_harmony == null) { Log("harmony not applied"); return; }
        Log($"Harmony ready ({_harmony.Id}): {_appliedCount} applied" +
            (_skippedCount > 0 ? $", {_skippedCount} skipped" : "") +
            (_failedCount > 0 ? $", {_failedCount} FAILED" : "") + ".");
    }

    /// <summary>Call from OnDeinitializeMelon.</summary>
    public static void Remove() {
        if (_harmony == null) return;
        try {
            _harmony.UnpatchSelf();
        } catch (Exception ex) {
            Log($"unpatch failed: {ex.Message}");
        } finally {
            _harmony = null;
            _appliedCount = _failedCount = _skippedCount = 0;
        }
    }

    private static void Log(string msg) {
        MelonLogger.Msg($"[Harmony] {msg}");
    }
}
```

---

## 3. PatchTargetGuard (Per-Class Pre-Flight)

```csharp
using System;
using HarmonyLib;

namespace S1Mods.Shared.Harmony;

public static class PatchTargetGuard
{
    /// <summary>
    /// Default: target method exists AND the harmony target method resolution didn't throw.
    /// Override IsAllowed in your mod if a patch needs extra preconditions
    /// (e.g. game version, scene state, other mod loaded).
    /// </summary>
    public static bool IsAllowed(Type patchType, out string reason) {
        try {
            var targetMethods = (HarmonyTargetMethod[])patchType
                .GetMethod("TargetMethod", Binding.Static | Binding.NonPublic | Binding.Public)?
                .Invoke(null, Array.Empty<object>()) is var tm && tm != null
                ? tm
                : Array.Empty<HarmonyTargetMethod>();

            // Easiest path: try to construct PatchClassProcessor and let Harmony resolve.
            // If the target method doesn't exist, Prepare() returns false and Patch() is a no-op.
            // For early-out (skip log), check HarmonyPrepare manually:
            var prepare = patchType.GetMethod("Prepare", Binding.Static | Binding.NonPublic | Binding.Public);
            if (prepare != null) {
                var result = prepare.Invoke(null, Array.Empty<object>());
                if (result is bool ok && !ok) {
                    reason = "Prepare() returned false";
                    return false;
                }
            }
            reason = "ok";
            return true;
        } catch (Exception ex) {
            reason = $"guard exception: {ex.GetType().Name} {ex.Message}";
            return false;
        }
    }
}
```

Override per-mod if you need custom logic:

```csharp
// In your mod:
HarmonyBootstrap.PatchTargetGuard.IsAllowed = (patchType, out reason) => {
    if (patchType.Name == "MyPatchThatNeedsUnityExplorer") {
        if (!UnityExplorerCompat.IsUnityExplorerLoaded()) {
            reason = "requires UnityExplorer";
            return false;
        }
    }
    reason = "ok";
    return true;
};
```

---

## 4. Wiring into a MelonMod

```csharp
public class MyBigMod : MelonMod {
    public override void OnInitializeMelon() {
        HarmonyBootstrap.Configure("com.dominik.mybigmod");   // stable id
        HarmonyBootstrap.ApplyFromMelon(this);
    }

    public override void OnDeinitializeMelon() {
        HarmonyBootstrap.Remove();
    }

    // Optional: a console command to print health on demand
    [ConsoleCommand("mybigmod health", "Print harmony patch health")]
    private static void HealthCommand() => HarmonyBootstrap.LogHealth();
}
```

---

## 5. Why Auto-Discover Beats PatchAll

| `harmony.PatchAll()` | `HarmonyBootstrap.Apply()` |
|---|---|
| Throws on first bad target → kills mod init | Continues past failures → mod partially functional |
| No visibility into which patches applied | Counter + log per class with full type name |
| All-or-nothing for the user | User sees "12/15 applied, 3 skipped" and can act |
| No log on silent no-op patches (Prepare()=false) | Each skip logged with reason |

When you have **more than 3 patches** in different files, the bootstrap version is worth the 100 lines.

---

## 6. Pitfalls

| Symptom | Cause | Fix |
|---|---|---|
| All patches "applied" but game unchanged | Game patched, target methods moved | Re-verify `TargetMethod()` per patch class; check `[HarmonyPrepare]` returns |
| `PatchClassProcessor.Patch()` throws on one class → entire apply aborts | Native code uses `harmony.PatchAll()` (not bootstrap) | Switch to `HarmonyBootstrap.Apply` |
| Two mods using the same HarmonyId → mutual unpatch | Default id is class name, but unpatch-self un-patches by id | Use a stable per-mod id (`com.dominik.mybigmod`) via `Configure()` |
| `UnpatchSelf()` doesn't fully remove prefixes | Some game code caches the trampoline | Acceptable for hot-reload; on full mod unload it's fine |
| `Apply()` called twice → no-op (idempotent guard) | Expected behavior | Call `Remove()` first if you really want a re-apply |
| Skip log floods with hundreds of entries | Many expected skips (e.g. 20 patches per faction, 10 factions missing) | Deduplicate skip reasons in your guard before logging |

---

## 7. When NOT to Use

- **Single-patch mods** (1-2 patches in one file): direct `new Harmony(id).PatchAll(assembly)` is simpler.
- **Patches that need to be applied in a specific order** with cross-dependencies: apply individually with explicit ordering — auto-discovery orders alphabetically by type name, which is usually but not always what you want.
- **Patches that should be re-applied at runtime** (scene reload, mod hot-reload): this bootstrap is one-shot; use `Harmony.Patch` directly per method.

---

## 8. References

- Source of pattern: decompiled `LegalProduce/LegalProduce.Util.Harmony/HarmonyBootstrap.cs` + `PatchTargetGuard.cs` (kept under `.scratch/mod-decompile/_decompiled/`)
- Companion: `schedule1-modding` §5 — Harmony patching conventions, `[HarmonyPrepare]` + `[HarmonyTargetMethod]`
- Companion: `schedule1-il2cpp-reflection` §5 — namespace-fallback for resilient `TargetMethod()` resolution
- Harmony 2.x docs: https://harmony.pardeike.net/articles/intro.html
