# S1API NPC integration map

> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 31/36 identifier-shaped tokens resolve (0 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.

Source scope: official online documentation reviewed on 2026-10-21; installed
S1API 3.2.1 behavior has not been tested by this package. Use the <!-- skills-health: version-evidence (S1API target requested by the owner; installed build state lives in AGENTS.md) -->
[custom-NPC skill](../../schedule1-custom-npcs/SKILL.md) as the implementation home.

## Choose the entity model

The official [NPC reference](https://ifbars.github.io/S1API/api/S1API.Entities.NPC.html)
distinguishes physical world NPCs from non-physical phone contacts. It presents
`IsPhysical` as a public virtual property and `ConfigurePrefab` as the prefab
configuration hook. Identity and initial customer/relationship/schedule setup
belong to that builder phase; runtime initialization belongs to OnCreated.
Match these members and the builder's overloads to the exact target assembly.

Do not restore the old mandatory `SpawnRegions` example or invented region enums.
Determine the actual spawn, region, navigation, and readiness contracts from the
current NPC builder and world implementation. A region name in game design does
not establish a property or enum member in S1API.

## Divide responsibilities

| Concern | Responsibility |
|---|---|
| Identity, initial components and defaults | Target-supported prefab configuration |
| Live subscriptions and gameplay behavior | Owning runtime NPC instance |
| Persisted mod data | Validated save fields with stable identity |
| Response callbacks after restore | Rebind through the supported restore lifecycle |
| Appearance | Verified customization fields, ranges, assets, and ownership |
| World placement and movement | Ready navigation, valid spawn point, correct authority |

Do not overwrite restored customer or relationship state with defaults every
OnLoaded. Verify whether builder defaults apply only to newly created state.
Avoid adding customer/dealer/supplier components twice or combining roles the
target framework treats as mutually exclusive.

## Lifetime and lookup

Keep IDs rather than wrappers across world reload. Resolve the current NPC through
a verified lookup route and handle not-yet-ready, missing, and destroyed states
separately. The online API has readiness and lookup surfaces, but their existence
alone does not prove that every optional subsystem is initialized.

Store exact delegate instances when needed for removal. Tear down the old NPC's
subscriptions on destruction/world transition. Repeated registration should not
create duplicate contacts, dialogues, schedules, or callbacks.

## Appearance and behavior verification

Inspect the exact customization field type before choosing numeric ranges, asset
paths, or color representations. Do not substitute an unverified preset enum for
an appearance builder. Validate the expected skeleton and asset dependencies.
For schedules, check that actions are enabled, navigation exists, and destinations
remain reachable after reload; a scheduler flag alone cannot establish pathfinding.

## Tests

Test first spawn, existing-save restore, stable identity, repeated reload, physical
and contact-only variants, messaging after load, appearance persistence, and
teardown. For networked NPCs include host, client, and late join. Keep callback,
contact, and native-object counts to detect duplicate initialization.

Use [saveables](saveables.md) for field serialization and
[lifecycle verification](../../schedule1-lifecycle-verify/SKILL.md) when order is unclear.
