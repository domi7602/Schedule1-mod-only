# Inherited Case Notes — Third-Party Mod Reviews

> verified 2026-10-20 — three worksteps recorded from actual decompilations; the sizes, hook counts and code references were read out of the decompiled sources. Behaviour claims were not tested in-game.

> Evidence scope: the original upload reports static decompilations on 2026-10-20.
> The binaries, hashes, full decompiles, and runtime traces are not bundled here.
> Values below preserve that historical report; they were not independently remeasured.

## Reported artifact overview

| Reported field | RealEstonia.Roguelike | FaleHouseKeys | CustomBrands |
|---|---|---|---|
| Version label in prior review | 0.2.1.0 | 1.0.0.0 | 1.1.1.0 |
| DLL size | 72 KB | 192 KB | 1.87 MB |
| Decompiled C# lines | 3,078 | 4,165 | 21,414 |
| Target framework | net472 | netcoreapp6.0 | netcoreapp6.0 |
| Hook description | 8 fixed | Dynamic name-pattern scan | 35 fixed plus 3 generated |
| Installation at time of review | Reported absent | Reported absent | Reported absent |

These labels do not establish current release identity, support, or installed state.

## RealEstonia.Roguelike

The prior notes describe a game-mode layer with perks, events, and cartel-goon waves.
Domain rules were separated into pure classes while an engine-facing session layer handled
Unity interaction. That separation makes isolated rule tests practical without claiming
all other layers are untestable.

Reported ideas: deterministic pricing salt from item identity, a hashed state key combining
save folder/seed/mode, pool reuse, and NavMesh checks for spawn positions.
Each idea needs its own validation: stable input identity and serialization, seed/world
replacement behavior, pool ownership, and actual path completeness for the relevant agent.
A deterministic hash does not by itself guarantee no multiplayer desynchronization.

Reported risks: a listing-update hook on a hot path, an offer-state gate that could block
wave start, strict day-jump handling, and reuse of the game's cartel pool.

## FaleHouseKeys

The prior notes describe physical property keys and a dynamic scan for door/gate/property
methods. Dynamic lookup can tolerate some type changes while still depending on names,
signatures, and relevance gates. It is not generally robust against renaming.
The mod reportedly remembered trigger collider enabled states and restored them later;
that ownership/restoration pattern is a useful review target.

Reported issues: conditional diagnostic calls, broad swallowed exceptions, periodic inventory
sidecars, and a shared fallback when slot identity could not be read.
Reinspect the actual binary before asserting release logging coverage or serialization support.
Periodic snapshots and ambiguous world identity require explicit loss/duplication tests.

## CustomBrands

The prior notes describe a branding/editor/packaging system with lazy item lookup registration,
runtime-generated meshes/icons/audio, configuration migrations, and structured diagnostics.
Lazy registration requires recursion, idempotency, and save/registry-order checks; it is not
an automatic guarantee of old-save compatibility.

Reported risks: generated iterator targets, hot-path visual/name hooks, periodic reinjection
into shops, and NPC joint changes that could interact with animation mods.
Generated names may change after relevant updates; they are not certain to change on every build.

The earlier scan reported a user-initiated image-import network path and did not find telemetry.
That is a statement about the earlier inspection scope, not a security guarantee for a new DLL.

## Transferable lessons

1. Separate domain rules and engine adapters where that improves testability.
2. Record actual hook outcomes and meaningful failures.
3. Validate world identity and distinguish atomic writes from backup/recovery policy.
4. Treat resource, collider, and mutable-state ownership as explicit contracts.
5. Revalidate every adopted pattern against current dependencies and its own failure cases.

Use [the runbook](reversing-runbook.md) to reproduce a review from the supplied artifact.
