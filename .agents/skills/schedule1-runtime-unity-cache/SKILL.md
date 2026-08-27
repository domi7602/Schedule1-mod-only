---
name: schedule1-runtime-unity-cache
description: >-
  Lifecycle-safe cache for runtime-created Unity objects (Texture2D, Sprite, AudioClip, Material) in MelonLoader IL2CPP mods. Use when a mod reloads content at runtime (live asset folder, hot-edit config) and needs to avoid leaking GPU memory and GameObject refs across reloads.
  Keywords: Object.Destroy, SpriteCache, dirty flag, MarkDirty, EnsureLoaded, IDisposable, Texture2D cache, AudioClip pool, GC.Alloc-free reload.
---

# Schedule I — Runtime Unity-Object Cache (Leak-Free Reload)

> **Source:** distilled from decompiling `CustomLoadingScreens 1.1.0` (`SpriteCache`, `SpriteLoader`). Verified 2026-08-26.
>
> **The trap:** When you `new Texture2D(...)` or `Sprite.Create(...)` at runtime, those are **UnityEngine.Object** subclasses that live on the **native heap**, not the managed GC heap. The GC will never collect them. Forgetting to call `Object.Destroy()` causes a leak per reload: after 10 config-edits the user has 10× the texture memory allocated, plus stale Sprite refs in random fields.
>
> **The fix:** A cache wrapper that owns its objects and destroys them deterministically when reloaded or disposed. Same pattern works for `Texture2D`, `Sprite`, `AudioClip`, `Material`, `RenderTexture`, `Mesh`.

---

## 1. The Pattern (SpriteCache-Style)

```csharp
using System;
using UnityEngine;

namespace S1Mods.Shared.Assets;

/// <summary>
/// Owns runtime-created Unity Objects. Rebuilds on dirty; calls Object.Destroy on old.
/// Generic over the array type stored (Sprite[] / Texture2D[] / AudioClip[]).
/// </summary>
public sealed class UnityObjectCache<T> : IDisposable where T : UnityEngine.Object
{
    private readonly Action<string>? _log;
    private bool _isDirty = true;
    private string? _lastSourceKey;
    private T[] _items = Array.Empty<T>();
    private bool _disposed;

    public UnityObjectCache(Action<string>? log = null) => _log = log;

    public IReadOnlyList<T> Items => _items;
    public bool IsEmpty => _items.Length == 0;

    public void MarkDirty() => _isDirty = true;

    /// <summary>Re-load only if dirty or if source key changed.</summary>
    public void EnsureLoaded(string sourceKey, Func<T[]> loader) {
        if (_disposed) throw new ObjectDisposedException(nameof(UnityObjectCache<T>));
        if (!_isDirty && string.Equals(_lastSourceKey, sourceKey, StringComparison.Ordinal)) return;
        Reload(sourceKey, loader);
    }

    private void Reload(string sourceKey, Func<T[]> loader) {
        Clear();                                  // destroy old first — never leak
        _lastSourceKey = sourceKey;
        _isDirty = false;
        try {
            _items = loader() ?? Array.Empty<T>();
        } catch (Exception ex) {
            _log?.Invoke($"[{typeof(T).Name}] reload failed: {ex.Message}");
            _items = Array.Empty<T>();
            _isDirty = true;                      // retry next EnsureLoaded
        }
    }

    /// <summary>Destroy all currently-owned objects and reset.</summary>
    public void Clear() {
        for (int i = 0; i < _items.Length; i++) {
            var obj = _items[i];
            if (obj != null) {
                try { UnityEngine.Object.Destroy(obj); }
                catch (Exception ex) { _log?.Invoke($"[{typeof(T).Name}] destroy failed: {ex.Message}"); }
            }
        }
        _items = Array.Empty<T>();
    }

    public void Dispose() {
        if (_disposed) return;
        _disposed = true;
        Clear();
    }
}
```

**Use it:**

```csharp
private readonly UnityObjectCache<Sprite> _sprites = new(msg => ModLogger.Info(msg));

// From OnUpdate after debounced reload fires:
_sprites.EnsureLoaded(
    sourceKey: $"v1:{folderPath}:{includeTutorial}",
    loader: () => SpriteLoader.LoadSprites(ImageCatalog.GetImageFiles(folderPath)));

// Game-side field assignment — wrap managed[] into Il2CppReferenceArray<Sprite>:
gameField.SetValue(il2cppOwner, il2cppRefArrayCtor.Invoke(new object[] { _sprites.Items.ToArray() }));
// (see schedule1-il2cpp-reflection §3 for the ctor wrapper)
```

---

## 2. Critical Rules

### 2a. Always destroy BEFORE rebuild, never after

```csharp
// ✅ RIGHT
Clear();                // destroy old
_items = loader();      // load new
// ❌ WRONG — keeps old + new alive until Clear() at next reload
_items = loader();
Clear();
```

### 2b. Never store Unity Objects in a long-lived managed collection that outlives the cache

```csharp
// ❌ WRONG — texture lives as long as the dictionary, even after "removal"
private static readonly Dictionary<string, Texture2D> _allLoaded = new();

// ✅ RIGHT — use a cache wrapper; its Dispose() actually frees the textures
private readonly UnityObjectCache<Texture2D> _allLoaded = new();
```

### 2c. Use `Object.Destroy` for runtime-created, `Object.DestroyImmediate` only in Edit mode

At runtime in a build, `Object.Destroy(obj)` is correct. `DestroyImmediate` is for editor scripts and **will not flush in time** for frame-accurate teardown in IL2CPP. Exception: during `OnApplicationQuit` when no frames will run anyway, `DestroyImmediate` is safe and immediate.

### 2d. Null-check before destroy — `Destroy(null)` is silently a no-op but throws `MissingReferenceException` on already-destroyed objects

```csharp
if (obj != null) {                            // managed null-check
    try { UnityEngine.Object.Destroy(obj); }
    catch (MissingReferenceException) { }      // already destroyed — fine
}
```

The `obj != null` check uses Unity's overloaded `==` which handles "fake null" (destroyed but reference still alive).

---

## 3. When the Dirty Flag Isn't Enough

`UnityObjectCache<T>.EnsureLoaded` short-circuits on unchanged key. Add a generation counter when the source folder can change content without changing path:

```csharp
public void EnsureLoaded(string sourceKey, int generation, Func<T[]> loader) {
    if (!_isDirty && _cachedGeneration == generation && string.Equals(_lastSourceKey, sourceKey, Ordinal)) return;
    _cachedGeneration = generation;
    Reload(sourceKey, loader);
}
```

Pull `generation` from `Directory.GetLastWriteTimeUtc(folder).Ticks` or a `File.GetLastWriteTimeUtc` per file.

---

## 4. Sprite-Specific Gotcha

When you wrap a `Texture2D` into a `Sprite.Create(...)`, the Sprite holds an internal ref to the Texture2D. Destroying the Texture2D before the Sprite leaves a "dead" Sprite that throws on render. Always destroy in **reverse allocation order**:

```csharp
// Created: texture1, sprite1, sprite2 (both ref texture1), texture2, sprite3
// Destroy order: sprite3, sprite2, sprite1, texture2, texture1
// → use a single cache per "batch" (e.g. one SpriteCache owns all sprites AND the textures they reference)
```

Simplest fix: keep textures and sprites in **one cache** per "bundle", with a `BundleDisposer` that destroys sprites first, then textures.

---

## 5. AudioClip Variant

Same pattern, slightly different ownership rules:

```csharp
public sealed class AudioClipCache : IDisposable {
    private AudioClip[] _clips = Array.Empty<AudioClip>();
    public void Reload(string folder, AudioType type = AudioType.OGGVORBIS) {
        Clear();
        var files = Directory.GetFiles(folder, "*.ogg");
        _clips = files.Select(f => {
            // S1API/Sideload has helpers; raw path:
            var req = UnityWebRequestMultimedia.GetAudioClip($"file://{f}", type);
            // ... handle async via coroutine or sync-blocking with SendWebRequest()
        }).ToArray();
    }
    public void Clear() { foreach (var c in _clips) if (c != null) Object.Destroy(c); _clips = Array.Empty<AudioClip>(); }
}
```

AudioClips loaded via `UnityWebRequest` need a coroutine; load asynchronously and only flip the cache atomically at completion (via `Interlocked.Exchange` on a local field, then assign to the public field — no torn reads because reference assignment is atomic).

---

## 6. Pitfalls

| Symptom | Cause | Fix |
|---|---|---|
| Memory grows on every config reload | Old Unity Objects not destroyed | Always `Clear()` before `loader()` |
| "MissingReferenceException" on destroy | Object already destroyed by scene unload | try/catch around `Destroy` |
| Sprites render as magenta after reload | Texture destroyed before Sprite | Destroy in reverse-creation order (§4) |
| Reload blocks the main thread for 2s | Synchronous file I/O in loader | Pre-load on background thread, swap via atomic reference assignment |
| Loader throws → cache empty forever | `_isDirty` not re-set on failure | Catch in `Reload`, set `_isDirty = true` (§1) |
| `Array.Empty<T>()` typed wrong for `Sprite[]` | `T[]` is `Sprite[]`, but assignment fails in IL2CPP | See `schedule1-il2cpp-reflection` §3 for `Il2CppReferenceArray<T>` wrapping |

---

## 7. References

- Source: decompiled `CustomLoadingScreens/CustomLoadingScreens.LoadingScreen/SpriteCache.cs` + `SpriteLoader.cs`
- Companion skill: `schedule1-debounced-reload` — fires the dirty signal
- Companion skill: `schedule1-il2cpp-reflection` §3 — wrapping managed `T[]` into `Il2CppReferenceArray<T>` for game-field assignment
- S1API 3.2.0: `S1API.Assets` (if present) may have wrappers — check before rolling your own
