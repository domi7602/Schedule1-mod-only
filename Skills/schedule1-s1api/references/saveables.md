# S1API Saveables and serialization

> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 14/16 identifier-shaped tokens resolve (0 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.

Source scope: official online documentation reviewed on 2026-10-21; this is not a
compile or runtime verification against the user's installed S1API 3.2.1. <!-- skills-health: version-evidence (S1API target requested by the owner; installed build state lives in AGENTS.md) -->

## Supported surface to match to the target

The official [Saveable reference](https://ifbars.github.io/S1API/api/S1API.Internal.Abstraction.Saveable.html)
identifies `S1API.Internal.Abstraction.Saveable` as a supported mod-facing base
class despite its namespace. It documents `OnLoaded`, `OnSaved`, `LoadOrder`, and
`RequestGameSave`. The request returns whether a save was requested; that Boolean
is not a durable-write receipt. The default load order is after base-game entities;
choose earlier loading only for a demonstrated dependency.

The [SaveableField reference](https://ifbars.github.io/S1API/api/S1API.Saveables.SaveableField.html)
targets fields, not properties. Use fields with explicit persisted names and
initialized defaults. The documentation warns against the reserved field name
`QuestData`; check the target source when choosing names and migrations.

## Design the persisted model

Keep versioned domain data separate from transient wrappers, delegates, Unity
objects, and UI projections. Persist stable identifiers for runtime objects and
resolve them after the appropriate restore boundary.
Use bounded collections and a schema migration policy. Check the actual serializer
for supported nested structures, collection types, null handling, private fields,
and cycles; do not infer restrictions or capabilities from another JSON library.
Do not add attributes to a cyclic object graph as a substitute for designing a
serializable data model.

## Discovery and lifecycle

Inspect the target's registration/discovery path and constructor requirements.
Verify exactly one intended state owner is created and that consumers receive that
instance. Do not create an additional local instance and assume it is the one the
framework saves. A nested state DTO need not be an independently registered
Saveable; establish the target's behavior before inheriting it for nested records.

Restore fields first, validate the candidate, then rebuild runtime projections.
If a UI subscribes after OnLoaded, provide the current validated state as well as
future notifications; otherwise initial data can remain invisible.
Clean up transient subscriptions without clearing persisted data prematurely.

## Save requests and failure

Trigger saves according to the feature's persistence policy and the game's
accepted cadence. Do not force a full game save for every keystroke or every
repaint. Handle an unaccepted request and keep unsaved intent visible.
Trace the target's actual failure reporting before treating OnSaved as complete
proof of all native and mod data durability.

If custom IO is needed instead of framework persistence, use
[the persistence workflow](../../schedule1-persistence/SKILL.md). Do not wrap an
unreadable file in a default object and then silently overwrite its source.

## Verification

Compile the actual attributed fields against the target assembly. Roundtrip
non-empty strings, multiple collection entries, numeric boundaries, null/default
values, and schema migration. Check actual saved content rather than success logs
alone. Test reload, slot switch, rejected save request, missing/corrupt data, and
UI subscription after restore. Label native acceptance tests separately from pure
serializer tests.

Research anchor: the matching source under `ThirdParty/S1API/`, if available.
