# BuildUpdate Patching — Lifecycle & Harmony Guards

Covers `BuildUpdate_Grid` interception and save/load timing. See `outdoor-placement.md` for GridItem rules and `collision-and-ghosts.md` for ghost raycasts.

## 2. Strip FishNet Networking

Vanilla prefabs carry `NetworkIdentity` + navigation — outdoor clones need stripping:

```csharp
var buildMgr = NetworkSingleton<BuildManager>.Instance;
if (buildMgr != null && buildMgr.Pointer != IntPtr.Zero)
{
    buildMgr.DisableNetworking(placedObj);
    buildMgr.DisableNavigation(placedObj);
}
```

## 3. Harmony Guards for BuildableItem.Start & SetCulled

- `Start()` looks up parent `Property` → throws on virtual root.
- `SetCulled(bool)` hides outdoor items when leaving a vanilla interior.

```csharp
[HarmonyPatch(typeof(BuildableItem), "Start")]
public static class BuildableItem_Start_Patch
{
    public static bool Prefix(BuildableItem __instance)
    {
        if (__instance != null && __instance.Pointer != IntPtr.Zero && __instance.gameObject != null)
            if (StreetPropertyManager.IsOutdoorItem(__instance.gameObject))
                return false;
        return true;
    }
}

[HarmonyPatch(typeof(BuildableItem), nameof(BuildableItem.SetCulled))]
public static class BuildableItem_SetCulled_Patch
{
    public static bool Prefix(BuildableItem __instance, bool culled)
    {
        if (__instance != null && __instance.Pointer != IntPtr.Zero && __instance.gameObject != null)
            if (StreetPropertyManager.IsOutdoorItem(__instance.gameObject))
                return false;
        return true;
    }
}
```

Also set `b.enabled = false` on child BuildableItems after placement.

## 6. Save Synchronicity (Anti-Dupe)

Keep placement/pack-up **in-memory** during play (`_activeStreetObjects`, `_knownGuids`). Serialize **only** on `GameLifecycle.OnSaveComplete`. Isolate per slot: `street_items_slot_{slotId}.json` via `LoadManager.Instance.ActiveSaveInfo.SaveSlotNumber`. Clear on `OnPreLoad`.

## 7. IL2CPP Pointer Safety

Always check `obj == null || obj.Pointer == IntPtr.Zero`. Use iterative `transform.parent` walk for `IsOutdoorItem`, not `GetComponentInParent<T>()` (see SKILL.md for full method). Required for all singletons, transforms, colliders.

## Placement Execution (BuildUpdate_Grid.Place Prefix)

1. If `!IsCustomPlacementValid` → vanilla pass-through (`return true`)
2. Get `itemInstance.Definition` / `bDef.BuiltItem`; deactivate source prefab if active, `Instantiate`, reactivate source
3. Parent to `StreetPropertyManager.StreetRoot.transform`
4. `DisableNetworking` + `DisableNavigation`
5. Attach `OutdoorItemInteractable` (`ItemId = itemId`)
6. Footprint suppression + disable `GridItem`/`BuildableItem`
7. Filter renderers, set colliders `isKinematic = true`, `SetActive(true)`
8. `RegisterStreetItem`, `RemoveAmountOfItem`, `PlayBuildSound`, `NotifyItemPlaced`, `Stop()` and `return false` to skip native `Place()`

## Save & Load Lifecycle

```
OnPreLoad → ResetState()
OnSaveInfoLoaded → register item defs
OnLoadComplete → LoadAndSpawnStreetItems()
Gameplay → in-memory Register/Unregister
OnSaveComplete → SaveStreetItems() atomic
```

Align strictly with `S1API.Lifecycle.GameLifecycle` — see `schedule1-persistence` for atomic patterns.
