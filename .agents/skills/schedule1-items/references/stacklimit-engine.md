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
