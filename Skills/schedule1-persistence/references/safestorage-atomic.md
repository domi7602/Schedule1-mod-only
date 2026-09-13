# SafeStorage Atomic

`Shared/src/SafeStorage.cs:73` SaveAtomic → EnsureDirectoryForFile → Serialize → SaveTextAtomic.
`SafeStorage.cs:91` SaveTextAtomic → Write temp `.tmp` → Copy `.bak` if target exists → Move `.tmp`→target (overwrite true).

```csharp
bool ok = SafeStorage.SaveAtomic(path, obj, log); // JSON
bool ok = SafeStorage.SaveTextAtomic(path, json, log); // text (Game.json)
if (!ok) MelonLogger.Warning("atomic save failed");
var loaded = SafeStorage.LoadSafe<T>(path, fallback: new T(), log); // tries .bak if primary corrupt/missing:127
```

**Migrate from raw File I/O:**
```csharp
// Before (broken) MoreSaveSlotsConfig.cs:57
string json = JsonSerializer.Serialize(this, opts); File.WriteAllText(ConfigPath, json);

// After (2026-08-21)
string json = JsonSerializer.Serialize(this, opts); bool ok = SafeStorage.SaveTextAtomic(ConfigPath, json);

// Rename Game.json: SaveRenameService.cs:69 now SafeStorage.SaveTextAtomic(gameJsonPath, newJson)
```
