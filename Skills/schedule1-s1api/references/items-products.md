# S1API item and product integration

> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 23/26 identifier-shaped tokens resolve (1 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.

Source scope: official online documentation reviewed on 2026-10-21; installed
S1API 3.2.1 and native item behavior have not been validated by this package. <!-- skills-health: version-evidence (S1API target requested by the owner; installed build state lives in AGENTS.md) -->

## Definitions, creation, and lookup

The official [ItemDefinition reference](https://ifbars.github.io/S1API/api/S1API.Items.ItemDefinition.html)
presents a wrapper with identity, display metadata, stack limits, and instance
creation. It points custom creation toward ItemCreator. Do not assume arbitrary
definition subclasses can be constructed with an item-ID string just because an
older example did so. Inspect the creation path and required native backing object.

The [ItemManager reference](https://ifbars.github.io/S1API/api/S1API.Items.ItemManager.html)
provides definition lookup, listing, and registration-related operations. Match
`GetDefinition`, registration/preservation behavior, and return values to the target
before use. Registration and preservation across scene transitions are separate
concerns; verify both for custom definitions loaded before save restoration.

## Choose the correct item model

Separate a definition from its instances and from the world object a buildable
creates. Distinguish ordinary storable items, quality-bearing products, packaging,
and native buildables using actual types and supported creation methods.
Do not copy the removed rarity-style `EItemQuality` enum or guessed category names.
Read the actual quality/category enum on the relevant wrapper/native owner.

For a new item, establish stable ID, display metadata, legal/category rules,
stackability, icon ownership, supported instance construction, registration phase,
and required shop listing behavior. Validate name collisions before mutation.

## Product integration

Product systems may add recipes, mixing effects, quality, packaging, and their own
save providers. Locate the target's real registration and serialization contracts.
Do not invent a `ProductDefinition` registration attribute or a save-provider
signature from a generic example. If using a third-party product mod as a pattern,
record its exact revision and recheck every framework/native call it relies on.

Persist enough identity and metadata to reconstruct the intended instance without
reusing a stale native object. Validate quantity, quality, and packaging together;
a product ID alone may not describe a transferable stack.

## Inventory and buildables

Use [inventory capacity](../../schedule1-items/references/inventory-capacity.md)
for fit and partial transfer. Do not assume a native inventory class or singleton
is runtime-neutral just because the surrounding code uses S1API wrappers.

A buildable needs the correct prefab, identity, native components, placement, and
persistence ownership. Reusing a vanilla storable wrapper alone does not establish
those contracts. Follow [buildable restore](../../schedule1-persistence/references/buildable-restore.md)
to avoid recreating objects native persistence already restores.

## Verification

Compile the actual creation/registration path. Test first registration, duplicate
ID, late registration, scene transition, saved instance restore, icon lifetime,
stacking with different metadata, missing dependency, and incompatible save data.
For products add recipe/mixing and packaging roundtrips; for buildables add place,
pack-up, save, reload, and native instance counts.

Historical research anchors: the current ItemCreator implementation and archived
mod examples such as SnackVendor/MoreDrugs, only if present in the owning workspace.
