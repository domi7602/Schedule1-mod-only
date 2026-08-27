---
name: schedule1-debounced-reload
description: >-
  Debounced live-reload pattern for MelonLoader mods in Schedule I v0.4.6f13. Use when a mod needs to react to file or folder changes (config hot-edit, user asset drop, MelonPreferences.cfg watcher) without thrashing the main thread.
  Keywords: FileSystemWatcher, debounce, TryConsumeScheduledReload, OnUpdate, main-thread pump, config hot-reload, ImageFolderWatcher, PreferencesFileWatcher.
---

# Schedule I — Debounced Live-Reload Pattern

> **Source:** distilled from decompiling `CustomLoadingScreens 1.1.0` (DooDesch). Verified 2026-08-26.
>
> **The trap:** `FileSystemWatcher` fires multiple events per logical change (Create + Changed + sometimes Renamed, plus double-fires from text editors using "atomic save" via temp file). A naive handler re-reads the file 5× per save, allocating GC pressure on the main thread and potentially corrupting state mid-read.
>
> **The fix:** Watcher only sets a `DateTime?` flag (debounced 150–250ms). The actual reload happens once, on the **main thread**, driven by `OnUpdate()`. No locks, no allocations on the hot path.

---

## 1. The 3 Components

```
┌──────────────────────┐    FileSystemWatcher     ┌──────────────────────────────┐
│  External change     │ ───────────────────────▶ │  DebouncedWatcher            │
│  (user saves file)   │   fires 1-5× per save    │  • sets _reloadDueUtc        │
└──────────────────────┘                          │  • lock-free                 │
                                                  │  • no allocation per event   │
                                                  └──────────────┬───────────────┘
                                                                 │ TryConsumeScheduledReload()
                                                                 ▼
                                                  ┌──────────────────────────────┐
                                                  │  MelonMod.OnUpdate()         │
                                                  │  main-thread pump            │
                                                  │  • single reload per debounce│
                                                  │  • catches exceptions safely │
                                                  └──────────────────────────────┘
```

---

## 2. Reusable Implementation

Drop this in `Source/Mods/Shared/src/DebouncedFileWatcher.cs`:

```csharp
using System;
using System.IO;

namespace S1Mods.Shared.IO;

/// <summary>
/// Debounced wrapper around FileSystemWatcher. Watcher events only stamp
/// _reloadDueUtc; the actual reload runs on the main thread when
/// TryConsumeScheduledReload() returns true. Lock-protected but allocation-free
/// on the hot path (just two DateTime reads/writes).
/// </summary>
public sealed class DebouncedFileWatcher : IDisposable
{
    private readonly object _lock = new();
    private readonly FileSystemWatcher? _watcher;
    private readonly TimeSpan _debounce;
    private readonly string? _filter;
    private DateTime? _reloadDueUtc;
    private bool _disposed;

    /// <param name="directory">Directory to watch. If null/unwatchable, watcher is a no-op.</param>
    /// <param name="filter">Optional filename filter (e.g. "MelonPreferences.cfg"). Null = "*.*".</param>
    /// <param name="debounceMs">Coalesce window. 150-300ms is typical.</param>
    /// <param name="includeSubdirectories">True for image-asset folders, false for single-file watchers.</param>
    public DebouncedFileWatcher(string directory, string? filter, int debounceMs = 250, bool includeSubdirectories = false)
    {
        _debounce = TimeSpan.FromMilliseconds(debounceMs);
        _filter = filter;
        if (string.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory)) return;

        try {
            _watcher = new FileSystemWatcher(directory, filter ?? "*.*") {
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.CreationTime,
                IncludeSubdirectories = includeSubdirectories,
                EnableRaisingEvents = true,
            };
            _watcher.Changed += OnChanged;
            _watcher.Created += OnChanged;
            _watcher.Deleted += OnChanged;
            _watcher.Renamed += OnRenamed;
        } catch (Exception) {
            _watcher?.Dispose();
            _watcher = null;          // silent degrade — caller checks IsActive
        }
    }

    public bool IsActive => _watcher != null;

    private void OnChanged(object _, FileSystemEventArgs __) => Schedule();
    private void OnRenamed(object _, RenamedEventArgs __) => Schedule();

    public void Schedule() {
        lock (_lock) {
            _reloadDueUtc = DateTime.UtcNow + _debounce;
        }
    }

    /// <summary>Call from MelonMod.OnUpdate(). Returns true exactly once per debounce window.</summary>
    public bool TryConsumeScheduledReload() {
        lock (_lock) {
            if (!_reloadDueUtc.HasValue || DateTime.UtcNow < _reloadDueUtc.Value) return false;
            _reloadDueUtc = null;
            return true;
        }
    }

    public void Dispose() {
        if (_disposed) return;
        _disposed = true;
        if (_watcher == null) return;
        try {
            _watcher.EnableRaisingEvents = false;
            _watcher.Changed -= OnChanged;
            _watcher.Created -= OnChanged;
            _watcher.Deleted -= OnChanged;
            _watcher.Renamed -= OnRenamed;
            _watcher.Dispose();
        } catch { /* swallow on shutdown */ }
    }
}
```

---

## 3. Wiring into a MelonMod

```csharp
private DebouncedFileWatcher? _configWatcher;
private DebouncedFileWatcher? _assetWatcher;

public override void OnInitializeMelon() {
    var userData = MelonEnvironment.UserDataDirectory;
    _configWatcher = new DebouncedFileWatcher(
        directory: Path.Combine(userData, "MyMod"),
        filter: "config.json",
        debounceMs: 250);
    _assetWatcher = new DebouncedFileWatcher(
        directory: Path.Combine(userData, "MyMod", "Assets"),
        filter: null,
        debounceMs: 500,
        includeSubdirectories: true);
}

public override void OnUpdate() {
    if (_configWatcher?.TryConsumeScheduledReload() == true)
        ReloadConfig();
    if (_assetWatcher?.TryConsumeScheduledReload() == true)
        MarkAssetCacheDirty();        // defer actual work — see schedule1-runtime-unity-cache
}

public override void OnApplicationQuit() {
    _configWatcher?.Dispose(); _configWatcher = null;
    _assetWatcher?.Dispose(); _assetWatcher = null;
}
```

---

## 4. The Two Common Cases

### 4a. MelonPreferences.cfg hot-reload

```csharp
var path = Path.Combine(MelonEnvironment.UserDataDirectory, "MelonPreferences.cfg");
var dir  = Path.GetDirectoryName(path);
var watcher = new DebouncedFileWatcher(dir, "MelonPreferences.cfg", debounceMs: 150);
// On consume: call MelonPreferences.Load() then re-read all entries into your cached values.
```

Note: 150ms (not 250ms) because MelonPreferences writes are atomic and the user expects fast response.

### 4b. User asset drop-folder

```csharp
var watcher = new DebouncedFileWatcher(
    Path.Combine(MelonEnvironment.UserDataDirectory, "MyMod", "Images"),
    filter: null,
    debounceMs: 500,
    includeSubdirectories: true);
// On consume: re-scan folder, rebuild sprite cache (don't do this on the FSW thread).
```

500ms because image loads are heavier and the user typically drops several files at once.

---

## 5. Pitfalls

| Symptom | Cause | Fix |
|---|---|---|
| Reload fires multiple times per save | No debounce — FSW double-fires on atomic-rename saves | `_reloadDueUtc` flag (§2) |
| Reload never fires | Watcher disposed too early, or directory didn't exist | Check `IsActive`, log a warning if false |
| Reload runs on FSW thread → throws | File still being written by editor | Always reload from `OnUpdate`, never from FSW callback |
| Reload throws → next reload blocked | Exception in handler keeps `_reloadDueUtc` null forever | Wrap reload body in try/catch; on failure, **call `Schedule()` again** to retry next tick |
| GC pressure from FSW events | `FileSystemEventArgs` allocated per event | Debounce converts N events → 1 reload — single allocation per debounce window |
| Watcher fires during shutdown → ObjectDisposed | Race with `OnApplicationQuit` | Set `_disposed` flag; null-check before each access |

**The retry-on-failure rule is the most-missed:** if the user saves while the file is locked, you must reschedule, not silently swallow.

---

## 6. When NOT to Use

- **One-shot reads** (e.g. settings at startup): no watcher needed.
- **Very large files** (100MB+): reload is expensive — consider a generation counter (`File.GetLastWriteTimeUtc`) and skip reload if timestamp unchanged.
- **Cross-process notifications** (IPC): use named pipes or a memory-mapped file instead.

---

## 7. References

- Source of pattern: decompiled `CustomLoadingScreens/CustomLoadingScreens.Config/ImageFolderWatcher.cs` + `PreferencesFileWatcher.cs` (kept under `.scratch/mod-decompile/_decompiled/`)
- Companion skill: `schedule1-runtime-unity-cache` — for what to do AFTER the debounce fires (rebuild Texture/Sprite/AudioClip caches with proper `Object.Destroy` lifecycle)
- Companion skill: `schedule1-modding` §5 — main-thread pump rules
