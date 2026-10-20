# Third-Party Mod Pattern Notes

> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 12/32 identifier-shaped tokens resolve (11 documented as absent). External-reference symbols are listed at the end of this file. Runtime behaviour is not covered by this sweep.

This note records reusable ideas from eight third-party Schedule I mods: six inspected on 2026-10-08, plus two content/world mods inspected 2026-10-19 (see the last two sections). Check every API, hierarchy path, and behavior against the current game before adapting a pattern.

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

## Runtime-built world content (inspected 2026-10-19)

- **Greenhouse Island.dll** (`PelicanKey` namespace, `Core : MelonMod`): no S1API at all — raw `Il2CppScheduleOne.*` interop. Independent of the S1API line, but the first to break when the game renames a type.
  - **Property created entirely in code:** builds a `Property` at runtime (property code, price, bounding box, spawn/interior/idle points, loading docks, disposal area) and gives its `NetworkObject` a **fixed SceneId** so host and clients agree on identity; a dialogue hook injects the purchase option into the estate agent, and a `PropertyManager.LoadProperty` prefix restores the property from save data with a guard against double-application. Reusable shape for "a new buyable property" — the fixed SceneId is the part that makes multiplayer agree.
  - **Placement search:** walkways to the mainland are sited by stepping outward from a fixed centre and probing ground height until it rises above sea level, then snapping to the quay; the mod also opens the game's invisible sea walls for its footprint. Good pattern for connecting a custom island to the existing shore without hand-placed coordinates for every approach.
  - **Procedural material catalogue:** every surface (sand, jungle floor, dirt, concrete, gravel, asphalt, planks, metal, glass, leaves, fronds, flowers, glow) is generated from a noise/plank texture helper — no external assets, which is why the DLL stays small. Useful when a build must not ship art.
  - **Door/ownership coupling:** sliding doors open on proximity only when the property is owned, and additionally when an assigned employee is close, so NPCs are not locked out of a player-owned building.
  - **Multiplayer shape:** host builds, clients rebuild after load, because a late-joining client never receives the construction. Copy the shape (host authority + client rebuild + fixed scene id), not the code.
  - **Cautions:** hard-coded world coordinates and a fixed SceneId collide with anything else building in the same area; the property price is written as zero and never recomputed afterwards, so "free property" is a design decision to check, not a bug to assume.

## Runtime GLB pipeline and gated vanilla-station reuse (inspected 2026-10-19)

- **Strain-o-nator (SON) DLL** (`SeedWorks` namespace, `SeedWorksMod : MelonMod`): the opposite architecture — S1API plus SharpGLTF, ~200 Harmony patches, single-player only. Pairs with the triage note (`third-party-mod-triage.md`) for the compatibility method.
  - **Runtime GLB pipeline:** SharpGLTF parses `.glb` files from `Mods\<Mod>\Models\`, and the mod builds Unity meshes and materials from the glTF accessors itself: POSITION/NORMAL/TEXCOORD_0/TANGENT, triangles with **reversed winding (A, C, B)** for the glTF→Unity handedness, `RecalculateNormals`/`RecalculateTangents` as fallbacks, and morph targets converted to blend shapes. Textures come from the glTF-embedded images through `ImageConversion.LoadImage`. Skinned assets are rejected outright, so the models are pure node hierarchies. See `schedule1-s1mapi` before hand-rolling any of this.
  - **Animation without an Animator:** glTF `NodeCurveSamplers` are sampled every frame and written straight into `Transform.localPosition/localRotation/localScale` plus `SetBlendShapeWeight`. Clip GLBs are loaded purely as curve suppliers and cached with their duration, and the animation is time-scaled to the process duration rather than the reverse. This is the pattern to reach for when imported animation must be driven by gameplay state.
  - **Template cache with no clear:** each file is parsed once into a static dictionary that is never emptied, the shared parent object is `DontDestroyOnLoad`, and per-instance materials are cloned. Consequences: the first station init pays the parse cost (a visible hitch with multi-megabyte assets), and any model change requires a restart.
  - **Relevance gates instead of blanket patches:** the patches on vanilla station classes return early unless the station is "theirs" — decided by item ids sitting in the station's slots, plus a feature-unlock flag, plus an explicit Mk2 exclusion. This is the pattern to copy whenever a mod must reuse a vanilla station without taking over other instances of it, including clones made by another mod.
  - **Transactional inputs:** each batch stores a receipt of slot indices, quantities and before/after slot snapshots, so a failed or interrupted batch retains its inputs instead of consuming them. Worth adopting for any multi-slot station.
  - **Vanilla physics with own limits:** pouring reuses the game's pourable/fillable/draggable modules while patching their entry points and substituting its own tilt, flow, drip and ripple constants.
  - **Cautions:** ~200 patches across cauldron, mixing station, lab oven, dialogue, phone, suppliers, quests and vehicles — a single game update is likely to break several. Its station patches also match cloned vanilla stations belonging to other mods, so check the gates before running it beside our own station mods. Its content unlocks default to deliberately extreme requirements, and the whole build is single-player only.

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
- `PelicanKey` / `IslandBuilder` / `JungleProperty` (Greenhouse Island)
- `SeedWorks` / `SonService` / `PseudoPackaging` / `ReGeneRuntime` / `MethMixerRuntime` (Strain-o-nator)
- `SharpGLTF.Core` (reference assembly shipped in the mod's `UserLibs\` folder, not a game type)
