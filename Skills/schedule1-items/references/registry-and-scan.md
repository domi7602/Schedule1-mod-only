# Registry lookup and bounded discovery

> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 10 of 10 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

Inherited StackLimitMod and custom-item notes; not revalidated against the
installed game in this package. Distinguish S1API wrappers from native registry
objects and from local Harmony handler names.

## Resolve and register

Prefer one definition lookup followed by a validity check to an exists-then-get
sequence. This reduces redundant lookup and avoids treating an earlier existence
check as a guarantee. It does not make arbitrary later operations transactional.
Use stable canonical item IDs and respect the registry's actual case/alias rules.

Register custom definitions at the lifecycle point required before saved instances
load. Check the active registry, handle ID conflicts deliberately, and verify scene
cleanup/preservation. Do not append the same definition on every load callback.

## Discovery layers

The inherited StackLimitMod scanned both loaded definition resources and the active
registry because runtime registration and resource discovery exposed different
populations. Verify whether both are needed for the current target and feature.

1. Scan only at a bounded initialization/reload boundary.
2. Track object identity and stable item ID; distinguish a replacement definition
   from a duplicate reference to the same definition.
3. Separate discovered, eligible, successfully applied, skipped, and failed counts.
   A failed mutation must not be permanently hidden by the discovery dedupe set.
4. Use a verified registration hook for definitions that arrive later. The name
   `Registry_AddToRegistry_Postfix` is an example mod handler, not a native API name.
5. Avoid expensive world/resource enumeration in a per-frame hot path.

## Classification

Determine packaging by the actual definition type/contract, not `bag` or `jar` in
its ID. A packaged product's identity and packaging metadata are separate concepts.
Likewise, inspect quality and effect ownership on their actual definition/instance
layers; do not invent a `MixEffects` property because the concept exists in the UI.

The inherited research located `PackagingID`, quality, and packaging definition
relationships in native product classes. Recheck those exact members in the target
build, then preserve their semantic distinction in mod domain models.

## Verification

Cover definitions discovered in each layer, late registration, duplicate references,
same-ID replacement, malformed definitions, failed application followed by retry,
and reload. Use counts and sample IDs to prove scan coverage; a green patch log
alone does not prove that late items were handled.

Read [stack-limit policy](stacklimit-engine.md) for mutation ownership, and
[S1API item integration](../../schedule1-s1api/references/items-products.md) for
wrapper creation and registration research.
