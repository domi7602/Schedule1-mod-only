# Registry Dual-Layer Scan

`StackLimitEngine.cs:46` Resources + `StackLimitEngine.cs:69` Registry → both needed.

```csharp
// Layer 1
var defs = Resources.FindObjectsOfTypeAll<BaseItemDefinition>();
for(int i=0;i<defs.Length;i++) ApplyToDefinition(defs[i], config, processedIds);

// Layer 2
var all = Registry.Instance.GetAllItems(); // Il2Cpp IList
for(int i=0;i<all.Count;i++) ApplyToDefinition(all[i], config, processedIds);

// Runtime — new items after scan
[HarmonyPostfix] Registry_AddToRegistry_Postfix(ItemDefinition item) => ApplyToItem(item, Config);

// Atomic lookup (no TOCTOU)
var def = Registry.Instance.GetItem(id); if(def==null) return; // don't Exists→Get
```

processedIds HashSet prevents double-count across layers.

---

## ProductItemInstance — Real API (verified Reflection 2026-08-22)

`Il2CppScheduleOne.Product.ProductItemInstance` actual properties:

```
PackagingID, packaging, AppliedPackaging, Amount, Name, Equippable,
StoredItem, Icon, Quality (EQuality), Definition, ID, Quantity,
Description, Category, StackLimit, EquippableData, ObjectClass, Pointer, WasCollected
```

**⚠️ GOTCHA: `MixEffects` does NOT exist on `ProductItemInstance`** — LLM hallucination. Quality → `EQuality Quality` on `QualityItemInstance`.

```csharp
// Correct: detect packaging via Registry cast (NOT substring matching)
bool isPackaging = def.TryCast<Il2CppScheduleOne.Product.Packaging.PackagingDefinition>() != null;
var prodDef = def.TryCast<Il2CppScheduleOne.Product.ProductDefinition>();
bool isRawProduct = prodDef != null && prodDef.ValidPackaging != null && prodDef.ValidPackaging.Length > 0;

// WRONG (ogkush_baggie falsely detected as empty packaging):
bool isPackaging = id.Contains("bag") || id.Contains("jar"); // ← NEVER
```

**Reflect yourself** via `TempReflect` (workspace root): `dotnet run --project TempReflect\TempReflect.csproj`

