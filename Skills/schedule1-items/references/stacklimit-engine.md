# StackLimit Engine

> verified: v0.1.5 lessons 2026-09-19; timing note re-verified 2026-10-05 (OnSaveInfoLoaded = 0 firings on 0.4.7f6+). Anchor: game v0.4.7f9 / S1API 3.2.1-beta.8.

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

Instance fallback `BaseItemInstance_GetStackLimit_Postfix:26` enforces the limit even if the definition was missed (e.g. late-registered item) — checks Excluded + orig==1.

NOT patched: `BaseItemDefinition.get_DefaultStackLimit` field accessor cannot be patched: `Latest.log:17:43:04.438` — removed `Mod.cs:109`.

## Apply-Timing Trap (v0.1.5, 2026-09-19)

**Symptom:** certain item categories (e.g. packaged products) do not stack; `stack stats` shows too few modified items.

**Cause (0.4.6-era analysis):** `OnSaveInfoLoaded` may fire BEFORE the game has called `Registry.AddToRegistry` for all items. Result: Apply with `Resources:23 Registry:0` (too few hits). The 1500 ms dedupe in `OnLoadComplete` then swallowed the canonical post-load apply — most definitions stayed on the vanilla limit.

**0.4.7f6+ note (re-verified 2026-10-05):** `OnSaveInfoLoaded` **fires 0×** on game 0.4.7f6+ (see `schedule1-troubleshooting/references/save-load-timing.md` §1) — the trigger-timing analysis above is historical. The operative fix stands: apply from `OnLoadComplete` + keep the `Registry_AddToRegistry` postfix (schedule1-items SKILL §2) so late registrations are caught whenever they happen.

**Fix:** `OnLoadComplete` checks `StackLimitEngine.LastRegistryCount == 0 && ModifiedItemCount > 0` ("blind apply") and reapplies even with dedupe.

**Diagnostic tools (since v0.1.5):**
- `UserData/StackLimitMod/apply_report.json` — per-item Id/Type/Original-Limit/Decision/Source, independent of the MelonLogger debug gating.
- `stack check <itemId>` — live diagnosis: weapon guard, agriculture type/ID match, exclude, eligibility + verdict.
- `stack report` — summary of the most recent apply decisions (unmodified items first).
- Config `LogDecisions: true` — records hot-path postfix decisions (max 512) in the report.

**Packaged Products (facts, verified against Interop decompiles):**
- Packaging is NOT a separate item type on the product: `ProductItemInstance.PackagingID` (string field) references the `PackagingDefinition` (e.g. `baggie`, `jar`). The definition/ID stays that of the product (`ogkush`).
- No `get_StackLimit` override in `ItemInstance`/`QualityItemInstance`/`ProductItemInstance` — a Harmony postfix on `BaseItemInstance.get_StackLimit` fires for all of them.
- `BaseItemInstance`/`BaseItemDefinition` do NOT live in Assembly-CSharp, but in `Il2CppScheduleOne.Core.dll` (namespace `Core.Items.Framework`) — for analysis, decompile the Interop assembly from `MelonLoader/Il2CppAssemblies/`, not the GameReferences decompiles.

## Update Cycle Lesson (2026-09-19)

The repo's `mod.json` (in `docs/`) is updated by `bump-version.ps1`, but the DEPLOYED `UserData/<Mod>/mod.json` is only updated by the build deploy — after manually copying the json, rebuild. Quality gates after each bump: `check-version-sync.ps1`, `check-doc-paths.ps1`, `dotnet format --verify-no-changes`.