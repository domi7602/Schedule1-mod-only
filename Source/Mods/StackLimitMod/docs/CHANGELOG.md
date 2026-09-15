# Changelog

## 0.1.4 (2026-09-15) — Agriculture-Only Mode & Weapon/Ammo Shield
- Neuer Standardmodus `AgricultureOnly = true`: Stack-Limits gelten strikt für Landwirtschafts- und Verarbeitungs-Items (Erde `SoilDefinition`, Samen `SeedDefinition`, Verpackungen wie Baggies/Jars `PackagingDefinition`, Dünger/Zusätze `AdditiveDefinition`, Pilzsporen/Spawn `SporeSyringeDefinition`/`ShroomSpawnDefinition` sowie Ernteprodukte `ProductDefinition`/`QualityItemDefinition`).
- Permanenter Schutz für Waffen, Munition, Kleidung und Geld: Waffen und Munition werden niemals gestackt (`IsWeaponOrAmmo` Guard), bestehende Overrides werden beim Laden sofort auf Vanilla-Limits zurückgesetzt. Behebt den UI-Bug mit chinesischen Schriftzeichen bei gestackten Waffen sowie den `-1 Munition`-Fehler beim Nachladen.
- Konsole: `stack stats` zeigt den `Agriculture Only`-Status an; neuer Toggle `stack set ag <true|false>`.

## 0.1.3 (2026-09-12) — Bug-Audit-Fixes Runde 3 (Audit 2026-09-12)
- `StackLimitEngine.RestoreAll()` neu: stellt jedes getrackte Original-`StackLimit` wieder her. Wird in `Mod.OnDeinitializeMelon` aufgerufen, sodass Mod-Disable/Unload die Definitionen nicht mit überschriebenen Limits zurücklässt.
- `OverrideNonStackable`-Default bleibt `true` (Backcompat), aber im Config-Kommentar dokumentiert: kann Quest-/Unique-Items stapelbar machen → bei Regressions via `stack set overridenonstackable false` deaktivieren.

## 0.1.2 (2026-09-11)
- Exclude stellt Original-Limit wieder her (kein Override-Rest nach Nachtrag).
- Decision-Cache mit 4096-Cap; kein Caching bei noch-unbekannter ID.

## 0.1.1 (2026-09-10)

- Decision-Cache (IntPtr-Key) im get_StackLimit-Hot-Path (0 Allokationen).
- OverrideNonStackable=false stellt Original-Limits wieder her.

## 0.1.0 (2026-08-20)

- **Initial release**: StackLimitMod for *Schedule I* (v0.4.6f13).
- **Engine**: Implemented `StackLimitEngine` scanning both `Resources.FindObjectsOfTypeAll<BaseItemDefinition>()` and `Registry.GetAllItems()`.
- **Runtime Hook**: Harmony postfix on `Registry.AddToRegistry` to dynamically apply stack limit rules to runtime-registered items.
- **Instance Patch**: Harmony postfix on `BaseItemInstance.get_StackLimit` with fallback to original limits and exclusion support.
- **Console & Terminal Integration**: Added `stack` and `stacklimit` commands supporting `stats`, `set <amount>`, `reload`, and `help`. Registered with DooDesch's `hash` terminal bridge.
- **Resilience**: Integrated `PatchGuard` and `SafeStorage` atomic persistence (`UserData/StackLimitMod/config.json`).
