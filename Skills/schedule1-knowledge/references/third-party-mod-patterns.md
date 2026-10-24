# Inherited Third-Party Pattern Notes

> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 12/32 identifier-shaped tokens resolve (11 documented as absent). External-reference symbols are listed at the end of this file. Runtime behaviour is not covered by this sweep.

> Evidence scope: the original upload describes static inspections of eight third-party
> mods on 2026-10-08 and 2026-10-19. Their binaries and complete decompiles are not bundled.
> These are design leads with reported symbol names, not current API or runtime guarantees.

## World construction and population

**PierExpansion:** the earlier notes report separate location definitions, property-template
construction, authority gates, staged decor/navigation, and tracked navigation instances.
Reported symbols include `PierProperties.MakeDefs`, `Prepare`, `MakeTemplate`, `NavBuilder.Build`,
and `MeshMerge.Merge`. Material/shadow/layer/spatial partitioning and LOD exclusions were
observed in its merge design. Verify exact implementation before applying these ideas.

**DaffysHills:** reported external/embedded layout selection, invariant parsing, and handling
of unavailable location slots in `StreetLayout.Load`. Coordinates, names, and fallback
semantics belong to that artifact and must not be promoted to current game truth.

Use [world expansion](../../schedule1-world-expansion/SKILL.md) for a current implementation
contract and independent staging/cleanup tests.

## Per-object visuals and scene-bound UI

**Locker Customization:** reported persistent identity derived from the parent buildable,
renderer discovery, and per-renderer color through `MaterialPropertyBlock`.
The review identified whole-block clearing in `ClearTint` and raw persistence writes in
`PersistSavedColours` as points to inspect for ownership and recovery problems.
Preserve unrelated overrides and resolve identity at the correct lifecycle boundary.

**Map_Show_Regions:** reported map-container anchoring, coordinate conversion, scene/size
checks, and throttled reconstruction. Verify the current hierarchy and the actual cost;
a known container path or helper name from one mod is not a universal game API.

## User data and modal control

**DarksMixBook:** reported separation of shared and save-specific recipes, normalization,
backup/cleaned recovery, and preservation of unreadable input in `CustomRecipeStore`.
Reuse the proven workspace storage contract when available rather than importing an
uninspected replacement fallback. `MixBookMod` reportedly captured prior time/input state;
restoration must cover close, failure, and scene exit while respecting other owners.

`MixSearchNode` and `MixSearchResult` names do not establish the search algorithm.
Inspect their actual bodies before using them as an algorithm reference.

## Geometry, authority, and interrupted saves

**BaseBuilder:** reported pure geometry helpers with finite-input checks; revisioned deltas
applied to a candidate before validation; sender/lobby checks; and a pending-intent/snapshot
recovery design. Symbols included `PlacementGeometry`, `WorldDelta`, `LobbySync`, and `SaveSafety`.
This does not establish protocol security or reliable recovery for another game/runtime.
Use [networked world state](../../schedule1-networked-world-state/SKILL.md) for adaptation.

## Runtime property content

**Greenhouse Island / PelicanKey:** earlier notes report raw game interop, procedural property
construction, a fixed network scene identifier, purchase-dialogue integration, guarded restore,
shore-connection probing, generated materials, and ownership-aware doors.
A fixed scene identifier alone does not establish multiplayer agreement: uniqueness,
registration, authority, late join, object lifecycle, and version compatibility also matter.
Check hard-coded world positions and identifier collisions. A zero price can be a design
choice; establish intended behavior before reporting it as a defect.

## Runtime GLB and station reuse

**Strain-o-nator / SeedWorks:** reported S1API plus SharpGLTF, numerous Harmony hooks,
static model templates, node animation driven by process time, and reuse of vanilla stations.
The inspected loader reportedly handled mesh attributes, reversed triangle winding, morph
targets, and image decoding while rejecting skinned assets. Those choices depend on the
full coordinate-conversion pipeline and are not a universal glTF conversion recipe.

Reported relevance gates used slot item IDs, feature flags, and exclusions. Verify how they
behave on cloned vanilla stations and other mods' items; a gate can still be incomplete.
Reported input receipts/snapshots are a useful transactional design lead, but do not prove
refund/recovery correctness without failure tests.
A persistent template cache and per-instance material clones require explicit ownership,
reload behavior, and retained-memory measurement.

## How to adopt a pattern

1. Obtain the relevant artifact/source with its provenance and inspect the exact body.
2. State the target mod's lifetime, authority, identity, and failure requirements.
3. Verify current APIs and existing shared helpers.
4. Implement the needed design independently; do not copy protocol constants or coordinates.
5. Exercise successful, interrupted, repeated, and cleanup paths.

A static decompile can be incomplete when dependencies or native implementations are missing.
Record that limit rather than turning a class name or reported earlier success into proof.
