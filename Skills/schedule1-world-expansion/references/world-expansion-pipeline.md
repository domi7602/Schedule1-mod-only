> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 5/20 identifier-shaped tokens resolve (9 documented as absent). External-reference symbols are listed at the end of this file. Runtime behaviour is not covered by this sweep.

# World Expansion Pipeline

This reference converts patterns observed in the user-provided `PierExpansion.dll` and `DaffysHills.dll` decompiles into implementation checks. The examples are static evidence only; do not treat their constants, APIs, or code as compatible with the current runtime.

## Staged ownership

Keep one mod-owned root and an explicit state for each build phase: geometry, property templates/registration, authoritative spawn, dependent decor, navigation, and NPC/runtime helpers. Set a phase complete only after its work succeeds. Reset state and owned resources on scene unload. Make each phase idempotent so scene callbacks and readiness retries cannot duplicate objects.

Observed example: `PierExpansion.Core` separates `Build`, `Prepare`/`SpawnAll`, decor, and navigation, gates property spawning on server authority, retries when prerequisites are ready, and clears systems on unload. `ClifftopAccess.Core` delays its worker until world data is ready.

## Data-driven property templates

Represent each property as declarative data: stable code/name, source template, transform/orientation, dimensions, employee capacity, docks, price, and explicit children to remove. Resolve the source first and abort that definition cleanly if required objects are absent. Clone from a known-good template, then reconfigure identity and dependent components rather than scattering coordinates and mutation logic across lifecycle callbacks.

Observed example: `PierExpansion.World.PierProperties.MakeDefs`, `Prepare`, and `MakeTemplate` build from property definitions and sanitize cloned hierarchies. Its exact hierarchy assumptions are fragile; verify all paths, component lists, grids, save points, docks, and network state against the live game.

## Navigation build and teardown

1. Gather the final geometry/collision roots only after their transforms are stable.
2. Collect sources while excluding triggers and invalid/disabled collision.
3. Derive bounds from actual colliders, with deliberate padding.
4. Build each required agent type independently; catch/report one agent failure without hiding all results.
5. Add explicit links for gaps, bridges, or doors; record link endpoints for diagnostics.
6. Keep every `NavMeshDataInstance` and `NavMeshLinkInstance` created by the mod and remove them on unload/rebuild.
7. Log source counts, agent types, links created/failed, and elapsed time.

Observed example: `PierExpansion.World.NavBuilder.Build`, `JoinEnds`, `AddDoorLinks`, and `Clear` build per agent and keep handles for teardown. Tune settings for the target area and test every relevant NPC type; do not reuse its numerical settings blindly.

## Rendering optimization boundaries

Before merging static render meshes, exclude all renderers referenced by an `LODGroup`; group remaining meshes by material, layer, shadow mode, and spatial cell; preserve transform matrices and submesh mapping; cap output chunk size; skip unreadable/missing meshes; and retain colliders and gameplay objects. Check the result visually at distance and in all scene lighting.

Observed example: `PierExpansion.World.MeshMerge.Merge` keys groups by material/shadow/layer/cell and skips LOD renderers. A similar partition is a design idea, not proof that mesh merging is safe for a different shader, LOD, or platform.

## Layout-driven residents

Use an explicit layout record for building paths, transforms, doors, hangouts, shops, and removed locations. Parse with `CultureInfo.InvariantCulture`, validate field counts and finite coordinates, sort deterministically, and handle missing, malformed, or partial data without indexing empty lists. Resolve occupied/removed homes to a safe available alternative, then validate schedule destinations and NavMesh reachability.

Observed example: `ClifftopAccess.StreetLayout.Load` supports embedded and user-provided layout text, marks removed homes, and maps NPC choices through `HomeSlot`. The embedded positions and hard-coded fallbacks need current-map verification.

## Exit criteria

- Re-entering the gameplay scene does not duplicate properties, decor, or nav data.
- Each registered location has stable identity and the expected grid/interaction/save behavior.
- Only the host/server performs authoritative world spawning.
- Every required NPC agent can reach its destinations through doors and links.
- Unload removes all mod-owned nav instances, subscriptions, roots, and cached references.
- Missing hierarchy nodes or layout rows produce a clear, bounded warning and a safe partial result.

---

---

---

---

---

---

## Unresolved identifiers (f12 static check 2026-10-08)

The identifiers below come from mod assemblies that are **not part of this repository**. They were read from external decompiles, so they cannot be resolved against the game assemblies, the checked-in S1API/S1MAPI source, or the workspace source. Treat them as external-reference symbols, not as game-API claims, and re-verify them against those mods before relying on them.

- `JoinEnds`
- `NavBuilder`
- `AddDoorLinks`
- `MeshMerge`
- `DaffysHills`
- `SpawnAll`
