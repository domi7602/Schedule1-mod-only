# IL2CPP & Harmony Patching Guide (Schedule I)

Schedule I is built on Unity 2022.3 compiled with **IL2CPP**. Interacting with IL2CPP types via MelonLoader 0.7.3 requires specific patterns.

---

## 1. Zero-Allocation Polling in IL2CPP
In IL2CPP, crossing the managed-to-native boundary generates allocations if LINQ, enumerators (`foreach` on unhollowed collections), or boxing are used inside high-frequency loops (`OnUpdate` or frequent polling).

### Best Practices:
* **Avoid LINQ in `Update()` / Tick loops**: Use indexed `for` loops where possible.
* **Cache references**: Cache `FindObjectOfType`, `Component` lookups, and `Type` instances in `Awake` / `Start` / Scene load handlers.
* **Cooldown timers**: Poll only when necessary (e.g. `_timer += Time.deltaTime; if (_timer >= 2.0f) { _timer = 0; PollData(); }`).

---

## 2. Harmony Patches on IL2CPP Methods

### Declaring Harmony Patches:
```csharp
using HarmonyLib;
using Il2CppScheduleOne.Growing;

[HarmonyPatch(typeof(GrowContainer), nameof(GrowContainer.Water))]
internal static class Patch_GrowContainer_Water
{
    [HarmonyPrefix]
    private static void Prefix(GrowContainer __instance, float amount)
    {
        // Execute custom pre-water logic
    }
}
```

### Safety & Scene Transitions:
* When returning to Main Menu (`MenuScreen`), objects from gameplay scenes are destroyed.
* Always check `if (__instance == null || __instance.WasCollected)` before accessing unhollowed IL2CPP objects.
* Clear cached gameplay lists on scene unload (`OnSceneWasUnloaded`).

---

## 3. Dealing with Unhollowed IL2CPP Collections
* `Il2CppSystem.Collections.Generic.List<T>` is NOT `System.Collections.Generic.List<T>`.
* When enumerating `Il2CppSystem.Collections.Generic.List<T>`, use `for (int i = 0; i < list.Count; i++)` instead of `foreach` to avoid creating garbage collection objects.
* **Prefer plain managed collections** (`System.Collections.Generic.List<T>`) for temp buffers inside one method — they never cross into native interop and are zero-cost to iterate. Only pass Il2Cpp collections to Il2Cpp APIs.

---

## 4. Sprite/Texture/Curve Caches — Key by Parameters (verified 2026-08-20/21)

Procedurally generated `Sprite`/`Texture2D`/`AnimationCurve`/`Gradient` caches must be keyed by their constructor parameters. A single `private static Sprite? _x` field shared across different radii/thicknesses silently returns the FIRST caller's asset for everyone (Minimap Rounded-vs-Square shape bug + **circle mask bug 2026-08-21**: `_circleMaskSprite/_circleBorderSprite` single-field → wrong mask after `size` change, fixed via `_circleMaskCache/_circleBorderCache` keyed `"{size}"`/`"{size}_{thickness}"` in `MinimapTextures.cs:9`).

```csharp
// ❌ WRONG — two radii share one cache slot
private static Sprite? _roundedSquareMask;
public static Sprite GetMask(float r) { return _roundedSquareMask ??= Build(r); }

// ✅ RIGHT — key by parameters
private static readonly Dictionary<string, Sprite> _maskCache = new();
public static Sprite GetMask(float r)
{
    string key = $"mask_{r}";
    if (!_maskCache.TryGetValue(key, out var s)) _maskCache[key] = s = Build(r);
    return s;
}
```

Static `readonly AnimationCurve`/`Gradient` (CustomSkateboard) are fine when the curve is truly global; if variants exist, key them too.

## 5. Field-Accessor Not Patchable (verified 2026-08-21)

`BaseItemDefinition.get_DefaultStackLimit` is a C++ field accessor → `Il2CppInterop "field accessor, it can't be patched"` (`Latest.log:17:43:04.438`). Harmony postfix never fires, PatchGuard 16/16 is false-positive. **Do not patch** field accessors — verify with `ilspycmd -t Il2CppScheduleOne.Core.Items.Framework.BaseItemDefinition` first; handle via direct scan/field write (`StackLimitEngine.cs:39-68` + `BaseItemInstance.get_StackLimit` postfix instead). Reference: `StackLimitMod/Mod.cs:109` removed dead patch.
