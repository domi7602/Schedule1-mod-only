# Workspace Evidence Inventory

> verified: repository inventory re-measured 2026-10-05; decompiles regenerated 2026-10-05 with ilspycmd in project mode; spot-check set passed.
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 68 of 68 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

> Evidence scope: directory names are inherited from the uploaded workspace notes.
> This package contains skills and references, not the game decompiles, framework sources,
> active mod repository, or original evidence files described below.

## Locate before reading

Identify the workspace root through the current task/environment. Check the relevant path
before using it; do not search unrelated user locations to compensate for missing inputs.
Record source commit, binary identity/hash, backend, and decompile provenance when available.

| Expected path | Useful evidence | Limit |
|---|---|---|
| `GameReferences/decompiled/Assembly-CSharp/` | Game/proxy types and member signatures | Generated IL2CPP bodies usually hide native behavior |
| `ThirdParty/S1API/` | Framework implementation at a known commit | May differ from deployed framework DLLs |
| `ThirdParty/S1MAPI/` | Mapping/building/import implementation | Availability and current API require inspection |
| `ThirdParty/S1MCPServer-master/` | Bridge schemas and handlers | Local modifications may differ from public source and running bridge |
| `Source/Mods/` | Current workspace implementations and tests | A copied pattern is not automatically safe for a new lifetime/authority model |
| `Source/Archive/` | Historical implementations | Not an active compatibility guarantee |
| `AGENTS.md`, `docs/compatibility.md` | Workspace environment and dependency choices | Not included by default in this skill package |
| Actual game's `MelonLoader/Il2CppAssemblies/` | Generated interop bindings for that installation | Match their generation to the running game |
| Actual game's `Mods/`, `Plugins/`, `UserLibs/` | Deployed artifacts | File presence is distinct from loaded-module identity |

## Curated system entry points

Read the [systems index](../../schedule1-game-systems/references/_index.md) before opening
individual notes. The collection groups 64 system topics; it is not proof that every
possible system or every native implementation detail is covered.

| Topic | Examples in the collection |
|---|---|
| Production and growing | Growing, mixing, products, drug effects |
| Money and trade | Money, businesses, customers, dark market |
| World and navigation | Regions, property, doors, map, vehicles |
| NPCs and communication | Behavior, employees, dialogue/quest, messaging |
| Player and inventory | Player, item framework, equipping, storage |
| Environment | Weather, lighting, audio, temperature |
| Infrastructure | Networking, persistence, time, configuration, variables |

## Evidence provenance

For each inspected file record whether it is original source, a generated proxy, a native
analysis output, a static note, or a runtime trace. A timestamp on a note is not sufficient
without the identity of the artifact it describes.
A symbol-count sweep checks lexical coverage only; it does not verify overloads, relationships,
call order, authorization, or gameplay behavior.

## Tool discovery

Locate the actual `ilspycmd` executable and inspect its help/version before use.
Treat `s1interop`, diagnostic mods, and a live S1MCP connector as optional capabilities.
Use [search recipes](search-recipes.md) for targeted reads and
[bridge guidance](../../schedule1-mcp/SKILL.md) for session/schema checks.

## Missing input

If a necessary referenced file is absent, identify the exact missing evidence and continue
independent analysis. Do not manufacture APIs or claim verification from the curated note.
