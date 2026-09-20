# StackLimit Engine

`StackLimitEngine.cs:112 ApplyToDefinition` — snapshot originalLimits once, then overwrite.

```csharp
bool ApplyToDefinition(BaseItemDefinition def, Config cfg, HashSet processed){
  if(def==null||def.Pointer==0)return false;
  string id=def.ID; if(string.IsNullOrEmpty(id))return false;
  if(processed!=null && !processed.Add(id))return false;
  int orig; lock(_lock) if(!_originalLimits.TryGetValue(id,out orig)){orig=def.StackLimit; _originalLimits[id]=orig;}
  if(cfg.ExcludedItemIds?.Contains(id)==true) return false;
  if(!cfg.OverrideNonStackable && orig==1) return false;
  try{ def.StackLimit=cfg.StackLimit; }catch(Exception ex){ Log.Warn(ex); return false; }
  return true;
}
```

Instance fallback `BaseItemInstance_GetStackLimit_Postfix:26` enforces limit even if definition missed (e.g. late-registered item) — checks Excluded + orig==1.

NOT patched: `BaseItemDefinition.get_DefaultStackLimit` field accessor can't be patched: `Latest.log:17:43:04.438` — removed `Mod.cs:109`.

## Apply-Timing-Trap (v0.1.5, 2026-09-19)

**Symptom:** einzelne Item-Kategorien (z. B. verpackte Produkte) stapeln nicht, `stack stats` zeigt zu wenig Modified Items.

**Ursache:** `OnSaveInfoLoaded` kann feuern, BEVOR das Spiel `Registry.AddToRegistry` für alle Items gerufen hat. Ergebnis: Apply mit `Resources:23 Registry:0` (zu wenig Treffer). Der 1500-ms-Dedupe in `OnLoadComplete` verschlang danach den kanonischen Post-Load-Apply — die meisten Definitionen blieben dauerhaft auf Vanilla-Limit.

**Fix:** OnLoadComplete prüft `StackLimitEngine.LastRegistryCount == 0 && ModifiedItemCount > 0` ("blinder Apply") und wendet trotz Dedupe erneut an.

**Diagnose-Werkzeuge (seit v0.1.5):**
- `UserData/StackLimitMod/apply_report.json` — je Item Id/Typ/Original-Limit/Entscheidung/Quelle, unabhängig vom Debug-Gating des MelonLoggers.
- `stack check <itemId>` — Live-Diagnose: Waffen-Guard, Agriculture-Typ/ID-Match, Exclude, Eligibility + Verdict.
- `stack report` — Zusammenfassung der letzten Apply-Entscheidungen (nicht-modifizierte zuerst).
- Config `LogDecisions: true` — zeichnet Hot-Path-Postfix-Entscheidungen (max. 512) in den Report.

**Verpackte Produkte (Fakten, verifiziert gegen Interop-Decompiles):**
- Packaging ist KEIN eigener Item-Typ am Produkt: `ProductItemInstance.PackagingID` (String-Feld) referenziert die `PackagingDefinition` (z. B. `baggie`, `jar`). Die Definition/ID bleibt die des Produkts (`ogkush`).
- Kein `get_StackLimit`-Override in `ItemInstance`/`QualityItemInstance`/`ProductItemInstance` — ein Harmony-Postfix auf `BaseItemInstance.get_StackLimit` feuert für alle.
- `BaseItemInstance`/`BaseItemDefinition` liegen NICHT in Assembly-CSharp, sondern in `Il2CppScheduleOne.Core.dll` (Namespace `Core.Items.Framework`) — für Analyse die Interop-Assembly aus `MelonLoader/Il2CppAssemblies/` decompilieren, nicht die GameReferences-Decompiles.

## Update-Zyklus-Lektion (2026-09-19)

Repo-`mod.json` (in `docs/`) wird von `bump-version.ps1` aktualisiert, aber das DEPLOYTE `UserData/<Mod>/mod.json` nur durch den Build-Deploy — nach manuellem json-Kopieren neu bauen. Quality Gates nach jedem Bump: `check-version-sync.ps1`, `check-doc-paths.ps1`, `dotnet format --verify-no-changes`.
