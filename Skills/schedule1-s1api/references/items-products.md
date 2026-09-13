# S1API — Items & Products

Two related but distinct APIs:
- **`S1API.Items`** — define **custom items** (anything that goes in inventory: tools, consumables, packaging, etc.)
- **`S1API.Products`** — register products with the **mixing system** (drugs, packaging, etc.)

---

## 1. Items — Custom Storable Items

```csharp
using S1API.Items;
using S1API.Items.Storable;

public sealed class MyCustomItem : StorableItemDefinition
{
    public MyCustomItem() : base("my_custom_item_id")
    {
        // Set display properties
        Name = "My Item";
        Description = "A custom item I made.";
        Category = EItemCategory.Tools;
        StackLimit = 50;
        Price = 99f;
    }
}
```

### Registration

```csharp
public override void OnInitializeMelon()
{
    ItemManager.RegisterItem(new MyCustomItem());
}
```

### Sub-Types

| Type | Use for |
|---|---|
| `StorableItemDefinition` | Base — anything in inventory |
| `BuildableItemDefinition` | Furniture (beds, lamps, etc.) |
| `ClothingItemDefinition` | Wearable clothing |
| `IngredientItemDefinition` | Mixing ingredient |
| `QualityItemDefinition` | Quality-tier item |
| `AdditiveItemDefinition` | Plant-grow additive |

---

## 2. Quality Tier System

```csharp
public enum EItemQuality
{
    Trash,        // 0.0
    Common,       // 0.2
    Uncommon,     // 0.4
    Rare,         // 0.6
    Epic,         // 0.8
    Legendary     // 1.0
}
```

Quality affects price and gameplay. Use `QualityItemDefinition` for items that should have a quality tier.

---

## 3. Products — Mixing System Integration

```csharp
using S1API.Products;

[ProductDefinition("my_product")]
public class MyProduct : ProductDefinition
{
    public MyProduct() : base("my_product")
    {
        Name = "My Product";
        Description = "Best product ever";
        Category = EProductCategory.Drug;
    }
}
```

S1API auto-discovers `[ProductDefinition]`-decorated classes.

### Custom Product Save Provider

```csharp
public class MyProductSaveProvider : ICustomProductSaveProvider
{
    public string ModId => "MyMod";
    public void Save(SaveData data) { /* your save logic */ }
    public void Load(SaveData data) { /* your load logic */ }
}

// Register
ProductRegistry.RegisterSaveProvider(new MyProductSaveProvider());
```

This integrates with the save system so your custom products persist with the same save format as base game products.

---

## 4. Item Slots / Inventory

```csharp
private static PlayerInventory Inventory => PlayerSingleton<PlayerInventory>.Instance;

public void GiveItemToPlayer()
{
    var instance = ItemManager.GetInstance("my_custom_item_id", quantity: 1);
    Inventory.AddItemToInventory(instance);
}
```

For defining the in-game slot:
- `HotbarSlot` for hotbar (max 9 slots)
- `CashSlot` for cash (max $1,000)
- `InventorySlot` for general inventory

See the `S1Mods.Shared` `EconomyHelper.GetMaxHoldableCashCapacity()` for slot-aware cash handling.

---

## 5. Common Recipes

### Recipe A: Simple Currency

```csharp
public sealed class CoinItem : StorableItemDefinition
{
    public CoinItem() : base("my_coin")
    {
        Name = "My Coin";
        Description = "Tokens earned by my mod";
        Category = EItemCategory.Cash;
        StackLimit = 10000;
        Price = 1f;
    }
}
```

### Recipe B: Stackable Consumable

```csharp
public sealed class MyPotion : StorableItemDefinition
{
    public MyPotion() : base("my_potion")
    {
        Name = "Healing Potion";
        Description = "Heals 100 HP.";
        Category = EItemCategory.Consumable;
        StackLimit = 99;
        Price = 50f;
    }
}
```

### Recipe C: Buildable Furniture

```csharp
public sealed class MyBed : BuildableItemDefinition
{
    public MyBed() : base("my_bed")
    {
        Name = "Custom Bed";
        Description = "Sleep in your own bed.";
        Category = EItemCategory.Furniture;
    }
}
```

Buildables also need a prefab (or `MeshVault`). If you don't have one, use a vanilla prefab with a `StorableItemDefinition` wrapper.

---

## 6. Workspace Reference

`HomelessMod` uses `S1API.Items.Buildable` for the procedural SleepingBag. See `Source/Mods/HomelessMod/src/Items/SleepingBagItemFactory.cs` for a production pattern.

`MoreDrugs` (ThirdParty/) uses `S1API.Products` with `ICustomProductSaveProvider` for full save integration.

For decompile: `Knowledge/Frameworks/S1API/Decompiles/3.2.0/S1API.Items/` and `S1API.Products/`.
