# Changelog

## 0.1.6 (2026-09-19) — IL2CPP-Typcheck-Fix (TryCast)
- **CRITICAL FIX — `is`-Checks versagen auf IL2CPP-Proxys:** `IsAgricultureItem`/`IsWeaponOrAmmo` prüften den Managed-Wrappertyp (immer `BaseItemDefinition`), nie die echte IL2CPP-Klassenhierarchie. Folge (apply_report.json 2026-09-19): `ogkush`/`sourdiesel`/`greencrack`/`granddaddypurple` (WeedDefinition), `meth`, `cocaine` wurden als "not-agriculture" klassifiziert und NIE auf das Stack-Limit angehoben — nur ID-Keyword-Treffer (soil, baggie, jar, defaultweed, …) wurden modified. Verpackte UND unverpackte echte Produkte stapelten deshalb nur mit Vanilla-Limit (20).
- Fix: Alle Typ-Checks auf `TryCast<T>()` umgestellt (echte IL2CPP-Klassenhierarchie; gleiche Pattern wie AutoPackagingStation). Betrifft beide Richtungen: echte Produkte werden jetzt gestackt; Cash/Clothing/Equippable werden jetzt zuverlässig geschützt (vorher lief `cash` als "not-agriculture" durch, `mushroomhat` wurde fälschlich gestackt).
- Nebeneffekt: der v0.1.5-Timing-Fix blieb im Live-Test unauffällig, weil der Report die eigentliche Fehlklassifizierung sichtbar machte. Die v0.1.5-Diagnose-Werkzeuge haben damit ihren Zweck bewiesen.


## 0.1.5 (2026-09-19) — Timing-Fix & Diagnose-Werkzeuge
- **Timing-Fix (verpackte Produkte stapeln nicht):** Lief ein Apply bevor das Spiel seine Items registriert hatte, resultierte `Registry:0` — und der 1500-ms-Dedupe in `OnLoadComplete` verschlang den kanonischen Post-Load-Apply (Log-Befund 2026-09-19: ein einziger Apply mit `Resources:23 Registry:0`, ein Großteil der Definitionen blieb dauerhaft auf Vanilla-Limit). `OnLoadComplete` wendet jetzt erneut an, wenn der letzte Apply `RegistryCount==0` bei `ModifiedItemCount>0` hatte.
- **Apply-Report:** Jeder Scan schreibt `UserData/StackLimitMod/apply_report.json` (Id, Typ, Original-Limit, Entscheidung, Quelle je Item) — unabhängig von MelonLogger-Debug-Gating. Neu: `LastRegistryCount` als belastbares Timing-Signal.
- **Konsole:** `stack check <itemId>` (Live-Diagnose je Item: Waffen-Guard, Agriculture-Typ/ID-Match, Exclude, gecapturetes Original, Eligibility, Verdict), `stack report` (Zusammenfassung der letzten Apply-Entscheidungen), Help/Stats erweitert.
- **Config:** Neues Flag `LogDecisions` (Default `false`) — zeichnet unkalkulierte Hot-Path-Entscheidungen des `get_StackLimit`-Postfix (max. 512) in den Report. Nur für Diagnose aktivieren.
- Statische Analyse (Decompiles + Interop-Assemblies): Verpackte Produkte tragen die Produkt-Definitions-ID; Packaging liegt im Instanz-Feld `PackagingID` (`ProductItemInstance`). Kein `get_StackLimit`-Override in `ItemInstance`/`QualityItemInstance`/`ProductItemInstance` — der Postfix auf `BaseItemInstance` deckt sie ab. Eligibility-Heuristiken (`baggie`/`jar`/`weed`/`bud`…) und `ProductDefinition`-Type-Match greifen für beide Fälle.


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
