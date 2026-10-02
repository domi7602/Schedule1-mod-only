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

For full Plant/Cycle control, see the decompile. The `entities.md` in this skill mentions plants in the context of NPCs.

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

**`S1API.Weather` is read-only by design** (verified against the deployed 3.2.1-beta.7 assembly and its source doc `ThirdParty/S1API/S1API/docs/weather.md`). The complete surface is three members — there is **no** `SetWeather`, no `TemperatureManager`:

```csharp
using S1API.Weather;

WeatherManager.OnWeatherChanged += state => { if (state.Rainy > 0.5f) { /* react */ } };
WeatherState? current = WeatherManager.Current;               // null outside gameplay
IReadOnlyList<string> ids = WeatherManager.KnownSequenceIds;  // configured sequence ids (game order/casing)
```

`WeatherState` is a readonly struct with nine float properties: `Sunny, Cloudy, Rainy, Stormy, Snowy, Foggy, Windy, Hail, Sleet`. Internally S1API polls `EnvironmentManager._currentWeatherConditions` every 0.1 s (the native change callback is not raised by the current weather implementation) and re-publishes distinct snapshots.

**Writing weather requires direct vanilla interop** on `Il2CppScheduleOne.Weather.EnvironmentManager` (`NetworkSingleton` → wrap in `NetworkGuard.IsHostOrSingleplayer()`). Verified members on the live 0.4.7f7 assembly:

- `SetWeather(string type)`, `SetWeatherSequence(string sequenceId)` — valid ids come from `WeatherSequences` / `WeatherProfile.Id`
- `TriggerLightningEvent()`, `TriggerTargetedLightningEvent(Vector3)`, `TriggerDistantThunder()`
- `OverrideTimeOfDay(int)` / `ClearTimeOfDayOverride()`
- `GetActiveWeatherConditionsFromPosition(Vector3)`, `GetWeatherProfile(string id)`, `IsPositionUnderCover(Vector3)`
- `WeatherSequences` (`List<WeatherSequence>`), `_currentWeatherConditions` (`WeatherConditions`, nine public float fields + `Set`)

**Temperature (`S1API.Temperature`) IS writable:** `TemperatureEmitter.GetOrAddComponent(go)` plus `SetTemperature / SetRange / SetPosition / NotifyChanged`, `TemperatureUtility.TemperatureSystemEnabled`, and the pure `TemperatureAlgorithm.GetTemperatureAtPoint(ambient, origin, point, emitters)`. Native limits: ambient 20 °C, emitter 0–40 °C, range 0.1–100 units (default 5, 0.1 min).

**Time / XP:** time via `S1API.GameTime`; `LevelManager.AddXP(int)` exists (`S1API.Leveling`), a `GetXP`/`LevelingManager` does not — read `LevelManager.XP / Rank / Tier`.

---

## 10. Misc / Dialogues / Messaging

```csharp
using S1API.Dialogues;
DialogueManager.ShowDialogue(npc, lineKey);

using S1API.Messaging;
MessageManager.SendSMS(contact, "Hello!");
```

For these less-common APIs, the decompile is the source of truth:
`ThirdParty/S1API/` (S1API-Source, Submodule)

---

## 11. When No S1API Wrapper Exists

Some vanilla game systems have no S1API wrapper yet (rendering, exotic managers). For those:

1. **Direct `Il2CppScheduleOne.*` access** — IL2CPP-bridge classes, see `GameReferences/ (locally generated decompiles, see GameReferences/README.md)`
2. **Isolate in `internal` module** — keep public mod API clean
3. **Wrap in an `S1API.Internal.*` namespace** if you want to share with other mods

For branch compatibility concerns, see [cross-compat.md](cross-compat.md).
