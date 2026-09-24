# Changelog

## 0.1.6 (2026-09-19) — IL2CPP type-check fix (TryCast)
- **CRITICAL FIX — `is`-checks fail on IL2CPP proxies:** `IsAgricultureItem`/`IsWeaponOrAmmo` checked the managed wrapper type (always `BaseItemDefinition`), never the real IL2CPP class hierarchy. Consequence (apply_report.json 2026-09-19): `ogkush`/`sourdiesel`/`greencrack`/`granddaddypurple` (WeedDefinition), `meth`, `cocaine` were classified as "not-agriculture" and NEVER raised to the stack limit — only ID keyword hits (soil, baggie, jar, defaultweed, ...) were modified. Packaged AND unpackaged real products therefore only stacked with the vanilla limit (20).
- Fix: all type checks switched to `TryCast<T>()` (real IL2CPP class hierarchy; same pattern as AutoPackagingStation). Affects both directions: real products are now stacked; cash/clothing/equippable are now reliably protected (before, `cash` slipped through as "not-agriculture", `mushroomhat` was incorrectly stacked).
- Side effect: the v0.1.5 timing fix stayed inconspicuous in live testing, because the report made the actual misclassification visible. The v0.1.5 diagnostic tools have proven their purpose.


## 0.1.5 (2026-09-19) — Timing fix & diagnostic tools
- **Timing fix (packaged products do not stack):** If an apply ran before the game had registered its items, the result was `Registry:0` — and the 1500 ms dedupe in `OnLoadComplete` swallowed the canonical post-load apply (log finding 2026-09-19: a single apply with `Resources:23 Registry:0`, a large part of the definitions stayed on the vanilla limit). `OnLoadComplete` now reapplies when the last apply had `RegistryCount==0` with `ModifiedItemCount>0`.
- **Apply report:** Every scan writes `UserData/StackLimitMod/apply_report.json` (id, type, original limit, decision, source per item) — independent of MelonLogger debug gating. New: `LastRegistryCount` as a reliable timing signal.
- **Console:** `stack check <itemId>` (live diagnosis per item: weapon guard, agriculture type/ID match, exclude, captured original, eligibility, verdict), `stack report` (summary of the most recent apply decisions), help/stats extended.
- **Config:** New flag `LogDecisions` (default `false`) — records uncalculated hot-path decisions of the `get_StackLimit` postfix (max 512) in the report. Only enable for diagnostics.
- Static analysis (decompiles + Interop assemblies): Packaged products carry the product definition ID; packaging lives in the instance field `PackagingID` (`ProductItemInstance`). No `get_StackLimit` override in `ItemInstance`/`QualityItemInstance`/`ProductItemInstance` — the postfix on `BaseItemInstance` covers them. Eligibility heuristics (`baggie`/`jar`/`weed`/`bud`...) and `ProductDefinition` type match cover both cases.


## 0.1.4 (2026-09-15) — Agriculture-only mode & weapon/ammo shield
- New default mode `AgricultureOnly = true`: stack limits apply strictly to agricultural and processing items (soil `SoilDefinition`, seeds `SeedDefinition`, packaging like baggies/jars `PackagingDefinition`, fertilizer/additives `AdditiveDefinition`, mushroom spores/spawn `SporeSyringeDefinition`/`ShroomSpawnDefinition` as well as harvested products `ProductDefinition`/`QualityItemDefinition`).
- Permanent protection for weapons, ammo, clothing, and money: weapons and ammo are never stacked (`IsWeaponOrAmmo` guard); existing overrides are immediately reset to vanilla limits on load. Fixes the UI bug with Chinese characters on stacked weapons as well as the `-1 ammo` reload error.
- Console: `stack stats` shows the `Agriculture Only` status; new toggle `stack set ag <true|false>`.

## 0.1.3 (2026-09-12) — Bug-audit fixes round 3 (audit 2026-09-12)
- New `StackLimitEngine.RestoreAll()`: restores each tracked original `StackLimit`. Called in `Mod.OnDeinitializeMelon`, so mod disable/unload doesn't leave the definitions with overwritten limits.
- `OverrideNonStackable` default stays `true` (backcompat), but documented in the config comment: can make quest/unique items stackable → disable on regressions via `stack set overridenonstackable false`.

## 0.1.2 (2026-09-11)
- Exclude restores the original limit (no override remnant after append).
- Decision cache with 4096 cap; no caching for still-unknown IDs.

## 0.1.1 (2026-09-10)

- Decision cache (IntPtr key) in the get_StackLimit hot path (0 allocations).
- OverrideNonStackable=false restores original limits.

## 0.1.0 (2026-08-20)

- **Initial release**: StackLimitMod for *Schedule I* (v0.4.6f13).
- **Engine**: Implemented `StackLimitEngine` scanning both `Resources.FindObjectsOfTypeAll<BaseItemDefinition>()` and `Registry.GetAllItems()`.
- **Runtime hook**: Harmony postfix on `Registry.AddToRegistry` to dynamically apply stack limit rules to runtime-registered items.
- **Instance patch**: Harmony postfix on `BaseItemInstance.get_StackLimit` with fallback to original limits and exclusion support.
- **Console & terminal integration**: Added `stack` and `stacklimit` commands supporting `stats`, `set <amount>`, `reload`, and `help`. Registered with DooDesch's `hash` terminal bridge.
- **Resilience**: Integrated `PatchGuard` and `SafeStorage` atomic persistence (`UserData/StackLimitMod/config.json`).