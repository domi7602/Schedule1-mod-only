# Third-Party Mod Pattern Notes

> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 12/32 identifier-shaped tokens resolve (11 documented as absent). External-reference symbols are listed at the end of this file. Runtime behaviour is not covered by this sweep.

This note records reusable ideas from six third-party Schedule I mods inspected on 2026-10-08. Check every API, hierarchy path, and behavior against the current game before adapting a pattern.

## World construction and population

- **PierExpansion.dll:** separates location definitions from property-template construction (`PierProperties.MakeDefs`, `Prepare`, `MakeTemplate`); gates property spawn on server authority; builds decor/navigation after dependencies are ready; tracks and clears nav instances. `NavBuilder.Build` builds for multiple agent types, adds explicit links, and logs source/link counts and elapsed time. `MeshMerge.Merge` partitions by material, shadow mode, layer, and spatial cell while excluding LOD renderers. See `schedule1-world-expansion` for the generalized pipeline.
- **DaffysHills.dll:** `StreetLayout.Load` selects external or embedded layout data, parses with invariant culture, tracks removed homes, and maps unavailable slots to a remaining location. Exact coordinates and the embedded map paths are version-specific and must not be treated as current game truth.

## Per-object visuals and scene-bound UI

- **✽ Locker Customization.dll:** `LockerEditor.GetLockerId` uses the parent `BuildableItem.GUID` for persisted per-locker identity, prefers the known renderer array then falls back to child renderers, and edits color through a `MaterialPropertyBlock`. `OnSceneLoaded` starts a 0.5-second reapply scan with a deadline. Its `ClearTint` clears the whole property block, which can erase unrelated renderer overrides; its `PersistSavedColours` uses raw `File.WriteAllText`, which conflicts with the workspace's atomic `SafeStorage` rule. Treat these as pattern plus caution, not drop-in code.
- **Map_Show_Regions.dll:** `MapShowRegionsMod` parents its overlay to the map's point-of-interest container and uses `MapPositionUtility.GetMapPosition` for region points. It checks the active scene handle and container size, then rebuilds stale UI. The probe is throttled; it should not be copied into a hot per-frame object search. This complements the map/UI lifecycle rules in `schedule1-phoneapp`.

## User data and UI control

- **DarksMixBook.dll:** `CustomRecipeStore` separates save-specific and shared records, normalizes deserialized entries, tries backup/cleaned recovery, and preserves unreadable input. It writes through temporary and backup files but uses custom `File.Replace` fallbacks; new workspace code should use `S1Mods.Shared.SafeStorage` instead. `MixBookMod` captures prior time-scale/input state while its modal is open; any UI that takes over input must restore the captured prior state on every close/error/scene-exit path.
- **MixBook search model:** the mod has dedicated `MixSearchNode` and `MixSearchResult` types. Inspect their actual implementation before using this as an algorithm reference; no behavior claim is made here.

## Geometry, multiplayer, and interrupted saves

- **BaseBuilder.dll** (from the supplied Build-A-Base package): `PlacementGeometry` keeps snapping, support selection, and plane intersection in pure helpers with finite-input checks. `WorldDelta` carries protocol/schema, session, revisions, fingerprints, changed/removed records, then applies to a copy and validates before acceptance. `AuthenticatedBuildingMessagePatch` inspects callback sender/lobby identity; `LobbySync` also tracks pending/late edits, fragmentation, and snapshot/delta fallback. `SaveSafety` records a pending intent and snapshot around native saves and refuses ambiguous recovery. These are design observations from one static binary, not proof of security or reliable multiplayer behavior. See `schedule1-networked-world-state` for safe adaptation criteria.

## Evidence limits

ILSpy may emit unresolved-IL annotations when dependencies or interop metadata are missing. A decompiled method body can be incomplete, and a class name does not prove the feature works in game. Do not reuse protocol constants, transport hooks, game coordinates, or serialized formats. Confirm exact current APIs from live assemblies and verify host/client, scene transitions, saves, and recovery in a controlled test.

---

---

---

---

---

---

## Unresolved identifiers (f12 static check 2026-10-08)

The identifiers below come from mod assemblies that are **not part of this repository**. They were read from external decompiles, so they cannot be resolved against the game assemblies, the checked-in S1API/S1MAPI source, or the workspace source. Treat them as external-reference symbols, not as game-API claims, and re-verify them against those mods before relying on them.

- `MixSearchNode`
- `MixBookMod`
- `MapShowRegionsMod`
- `LockerEditor`
- `MixSearchResult`
- `GetLockerId`
- `PersistSavedColours`
- `CustomRecipeStore`
- `ClearTint`
