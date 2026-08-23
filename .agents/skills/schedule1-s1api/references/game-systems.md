# S1API — Game Systems Quick Reference

One-page navigators for the smaller game-system namespaces. For each, the pattern is: **read the decompile, then use the wrapper** (or fall back to vanilla if no wrapper exists).

---

## 1. Doors — `S1API.Doors`

```csharp
using S1API.Doors;

var door = DoorManager.GetDoor(transform);   // door at a transform
door.IsOpen = true;
door.Lock();
```

Interior doors vs property doors: covered by `S1API.Building`.

---

## 2. Building — `S1API.Building`

```csharp
using S1API.Building;

var building = BuildingManager.GetBuildingByID("barn");
building.InteriorLightsEnabled = true;
```

For **building new rooms/areas** in the world, use `S1MAPI` (the sister framework) — see the `schedule1-s1mapi` skill.

---

## 3. Vehicles — `S1API.Vehicles`

```csharp
using S1API.Vehicles;

var vehicle = VehicleManager.GetVehicleByID("vehicle_id");
vehicle.LockDoors();
vehicle.SetOwner(Player.Local);
```

`VehicleManager.AllVehicles` lists all vehicles in the world.

---

## 4. Growing — `S1API.Growing`

```csharp
using S1API.Growing;

var plant = PotScanner.GetPlant(potGameObject);   // wraps Plant
plant.Water();                                    // instant water
plant.Harvest();                                  // returns product
```

For full Plant/Cycle control, see the decompile. The `schedule1-s1api/references/entities.md` mentions plants in the context of NPCs.

---

## 5. Stations — `S1API.Stations`

Mixing stations, packaging, etc. Used by the mixing system.

```csharp
using S1API.Stations;

var station = StationManager.GetStationByID("station_id");
station.SetMixing(true);
```

---

## 6. DeadDrops — `S1API.DeadDrops`

```csharp
using S1API.DeadDrops;

var drop = DeadDropManager.GetDeadDropByID("drop_id");
drop.SetCashAmount(500f);
```

For the underlying game types, see `S1API.DeadDrops.Native`.

---

## 7. Cartel — `S1API.Cartel`

Cartel rank / influence / region unlock. Used by the unlocking-system progression.

```csharp
using S1API.Cartel;

var cartel = CartelManager.Instance;
cartel.UnlockRegion(Region.Docks);
```

---

## 8. Casino — `S1API.Casino`

Casino games (slots, blackjack). Mods can place custom casino tables.

```csharp
using S1API.Casino;

var casino = CasinoManager.GetCasinoByID("casino_id");
casino.Open();
```

---

## 9. Weather / Temperature / Time

Covered in `references/money-economy.md` (GameTime) and these one-liners:

```csharp
WeatherManager.SetWeather(Weather.Sunny);
TemperatureManager.SetTemperature(20f);   // Celsius
LevelingManager.GetXP(Player.Local);
```

---

## 10. Misc / Dialogues / Messaging

```csharp
using S1API.Dialogues;
DialogueManager.ShowDialogue(npc, lineKey);

using S1API.Messaging;
MessageManager.SendSMS(contact, "Hello!");
```

For these less-common APIs, the decompile is the source of truth:
`Knowledge/Frameworks/S1API/Decompiles/3.2.0/<Namespace>/`

---

## 11. When No S1API Wrapper Exists

Some vanilla game systems have no S1API wrapper yet (rendering, exotic managers). For those:

1. **Direct `Il2CppScheduleOne.*` access** — IL2CPP-bridge classes, see `Knowledge/Game-Reference/Decompiles/Assembly-CSharp/`
2. **Isolate in `internal` module** — keep public mod API clean
3. **Wrap in an `S1API.Internal.*` namespace** if you want to share with other mods

For branch compatibility concerns, see [cross-compat.md](cross-compat.md).
