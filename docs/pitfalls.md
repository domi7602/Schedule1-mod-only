# Battle-Tested Gotchas & IL2CPP Pitfalls (Cheat Sheet)

> **Single Source of Truth for Schedule I Modding Edge Cases & Architecture Guardrails**  
> Basiert auf der Praxiserfahrung aus 15 aktiven Mods, Hunderten von Bugfixes und IL2CPP-Crash-Analysen.

---

## 1. Core Architecture Standards (Non-Negotiable Guardrails)

### 1.1 IL2CPP Pointer Validation & Lifecycle Safety
- **Pointer Validation:** `obj != null` reicht in IL2CPP niemals aus — immer `obj != null && obj.Pointer != IntPtr.Zero && !obj.WasCollected` prüfen (Managed-Proxies überleben die nativen C++-Objekte).
- **Public `IntPtr` Constructors:** Jede `[RegisterTypeInIl2Cpp]`-MonoBehaviour **MUSS** einen `public X(IntPtr ptr) : base(ptr) { }` Konstruktor deklarieren — sonst stürzt die native IL2CPP-Bridge beim `AddComponent` hart ab.
- **Kein `foreach`/LINQ auf `Il2CppSystem.Collections.Generic.List<T>`:** Nur indizierte `for`-Schleifen verwenden (verhindert massive per-Frame GC-Allokationen).
- **Scene-Transition Cleanup:** Statische UI- und GameObject-Caches in `OnSceneWasUnloaded` / `OnPreLoad` explizit leeren (`.Clear()`), um Dangling-Pointer-Crashes bei Szenenwechseln zu verhindern.
- **`UnityEngine.TextMesh` ist gestript:** `AddComponent(Il2CppType.Of<TextMesh>())` wirft in IL2CPP eine `ArgumentException`. Stattdessen 0-Allocation-`OnGUI()`-HUDs mit statisch gecachten `GUIStyle`s oder TextMeshPro (`TextMeshProUGUI`) nutzen.

### 1.2 Responsive UI ("Method 3": `UITheme`)
- S1API-Phone-Canvases sind High-DPI und 90° rotiert — feste Pixelgrößen sind auf verschiedenen Auflösungen unlesbar.
- **Single Source of Truth:** `S1Mods.Shared.UITheme` (`Source/Mods/Shared/src/UITheme.cs`). Mod-UI-Elemente nutzen `UITheme.Sp(...)`, `UITheme.Dp(...)` und `UITheme.Scale`.
- **Non-Destructive PhoneApp-Lifecycle:** Niemals `Object.Destroy()` oder destruktive UI-Clears in `OnPhoneClosed()` aufrufen — nur `_mainBG.SetActive(false)`. Andernfalls tritt der gefürchtete **"Transparent Phone"**-Bug beim erneuten Öffnen auf.
- **Rule 10/11:** `OnCreated()` feuert nur 1× pro Szene (S1API `HomeScreen_Start_Patch`), `OnPhoneClosed()` bei jedem Schließen. **Niemals** `MelonEvents.OnUpdate`-Subscriptions in `OnPhoneClosed()` abmelden, da die App beim zweiten Öffnen sonst unbedienbar wird.

### 1.3 Safe Persistence & Savegame-Synchronisation
- **Atomic Writes:** Nur `SafeStorage.SaveAtomic` / `SaveTextAtomic` (`.tmp` → atomic Move → `.bak`) verwenden, niemals ungeschütztes `File.WriteAllText`.
- **Slot Isolation:** Mod-Saves strikt mit der Slot-Nummer suffixen (`<name>_slot_{n}.json`) via `SaveSlots.GetActiveSlotNumber()` (mit `>= 0`-Guard gegen `slot_-1.json`).
- **Transaktionale Save-Integrität:** Runtime-Zustände bleiben im RAM; Serialisierung auf Disk erfolgt erst bei `S1API.Lifecycle.GameLifecycle.OnSaveComplete`. State-Reset erfolgt bei `OnPreLoad` / Scene-Unload.
- **Culture-Safe Parsing:** `float` und `decimal` immer mit `NumberStyles.Float + CultureInfo.InvariantCulture` parsen und formatieren (deutsches Komma bricht Bank-Transaktionen und Saves).

### 1.4 Resilient Harmony Patching (`PatchGuard`)
- **Graceful Degradation:** Patches immer über `S1Mods.Shared.PatchGuard.TryPatch` registrieren — Signatur-Änderungen nach Spiel-Updates führen dann zu sauberem Logging statt zum Mod-Crash.
- **Nie `ref <Il2CppType> __result` in Prefixes mit `return false`:** Dies führt zu `0xc0000005`-Access-Violations in `UnityPlayer.dll`. Manipulationen stattdessen direkt an der UI/Liste vornehmen.
- **Field-Accessors sind nicht patchbar:** IL2CPP-Eigenschaften wie `BaseItemDefinition.get_DefaultStackLimit` können nicht gepatcht werden (`Il2CppInterop can't be patched`). Stattdessen Instanz-Methoden wie `BaseItemInstance.get_StackLimit` postfixen.

### 1.5 Multiplayer Host Authority
- Alle wirtschaftlichen und weltverändernden Aktionen strikt mit `NetworkGuard.IsHostOrSingleplayer()` absichern.
- **FishNet Singleplayer Trap:** Im Singleplayer ist `InstanceFinder.IsServer == false`, da der NetworkManager inaktiv ist. Der Host-Check muss prüfen: `NetworkManager == null || (Object)NetworkManager == null || InstanceFinder.IsServer`.

### 1.6 Decoupled Runtime Pattern (MonoBehaviour Boundary)
- Managed C#-Typen (`List<string>`, DTOs) in den Signaturen von `[RegisterTypeInIl2Cpp]`-MonoBehaviours führen dazu, dass `ClassInjector` die **gesamte Assembly** abweist.
- **Lösung:** MonoBehaviours strikt primitiv halten (Unity-Lifecycle + `string Guid`); komplexe Datenmodelle über statische, pure C#-Manager verwalten.

---

## 2. Battle-Tested Gotchas & Solutions Matrix

| Kontext / Mod | Problem / Gotcha | Ursache | Verifizierte Lösung |
|---|---|---|---|
| **HomelessMod** (Outdoor) | `DestroyImmediate(GridItem)` → Hard Crash | Native Component-Tables von Unity werden korrumpiert | **Nie `GridItem` zerstören:** `gi.enabled = false; gi.SetFootprintTileVisiblity(false);` und Harmony-Prefix auf `GridItem.Destroy` mit `return false` bei `IsOutdoorItem`. |
| **HomelessMod** | FishNet Clone-Desync | Vanilla-Prefabs erwarten FishNet-Server-Authority | Sofort nach Instanziierung: `BuildManager.Instance.DisableNetworking(obj)` + `DisableNavigation(obj)`. |
| **HomelessMod** | Unsichtbare Objekte nach Interieur-Exit | `BuildableItem.SetCulled`/`Start` erwartet Vanilla-`Property`-Parent | Harmony-Prefix auf `BuildableItem.Start` + `SetCulled`: wenn Outdoor-Item, `return false; b.enabled = false;`. |
| **HomelessMod** | `Property` auf `_streetRoot` zerstört Real-Estate-Apps | Real-Estate-Logik iteriert alle Properties der Szene | **Nie `Property` auf `_streetRoot` platzieren** — `DontDestroyOnLoad` nutzen, Verwaltung via `StreetPropertyManager`. |
| **HomelessMod** | Floating Grid-Tiles auf Asphalt | Vanilla-Grid rendert Tile-Decals | `FootprintTile`-Child-GameObjects deaktivieren, Mesh-Renderer aktiv lassen. |
| **HomelessMod** | Dupe-Exploit bei Alt+F4 nach Platzieren | Disk-Save erfolgte sofort, Inventar-Save erst regulär | Runtime-Objekte strikt im RAM (`_activeStreetObjects`) halten; Speichern erst auf `OnSaveComplete`. |
| **HomelessMod** | Raycast fällt durch Terrain | Straßen-Meshes liegen auf `Grid`-Layer | Layer-Maske: `~LayerMask.GetMask("Ignore Raycast", "Player")` inkl. `Grid`, `QueryTriggerInteraction.Ignore`. |
| **CustomSkateboard** | Memory-Leak über `renderer.material` | Zugriff auf `.material` instanziiert Kopie im VRAM | Konsequent `r.sharedMaterial` verwenden + `static readonly` Material-Caches. |
| **CustomSkateboard** | Avatar-Visual-Corruption | Deck-Swap manipulierte versehentlich Kleidung/Haare | Renderer strikt nach Mesh-Name (`"deck"` / `"board"`) auf Child-Transforms filtern. |
| **CustomSkateboard** | Duplicate Dialogue Injection bei Jeff | Dialogue-Tree wird bei Reloads erneut durchlaufen | Vor Injektion mit `StringComparison.OrdinalIgnoreCase` auf vorhandene Knoten prüfen. |
| **CustomSkateboard** | Double-Tuning Allocation Churn | Awake- und Mount-Hooks feuern mehrfach | Idempotenz-Filter `HashSet<IntPtr> _tunedBoards`, Reset in `OnSceneWasLoaded`. |
| **MoreSaveSlots** | Slot-Allocation-Timing-Crash | Native Arrays werden vor Slot-Patch allokiert | `SaveManager.Awake`-Prefix injiziert `SAVE_SLOT_COUNT` vor der internen Array-Allokation. |
| **MoreSaveSlots** | Szene-Guard verkehrt herum | `IsInMainScene() == "MainMenu"` invertiert Logik | `IsInGameplayScene()` prüft strikt auf den Szenennamen `"Main"`. |
| **PhoneApps** | WASD-Laufen während der Texteingabe | Unity InputField blockiert Vanilla-Player-Controls nicht | `[RegisterTypeInIl2Cpp]`-Komponente toggelt bei Focus/Blur `S1API.Input.Controls.IsTyping`. |
| **PhoneApps** | `UnityEvent.AddListener` IL2CPP-Crash | Native Delegate-Bridge bricht bei Standard-Action | Immer `S1API.Utils.EventHelper.AddListener(...)` oder `ButtonUtils.AddListener(...)` nutzen. |
| **PhoneApps** | Globaler State-File Leak | Globales JSON leakt Daten zwischen Save-Slots | Konsequent `<name>_slot_{n}.json` via `SaveSlots.GetActiveSlotNumber()` + `TryMigrateLegacy()`. |
| **PocketShop** | Leerer Shop-Katalog bei Fast-Load | `ShopInterface.AllShops` ist beim Start noch unbefüllt | Resiliente Retry-Loop (20 × 1.5s) auf Registry-Population. |
| **PocketShop** | Nicht-atomarer Kauf (Geld weg, kein Item) | Geld wurde vor Item-Übergabe abgebucht | Item-Instanzierung vor Abbuchung, Try/Catch mit automatischem Refund bei Inventar-Voll. |
| **BankApp** | Bargeld-Verlust bei vollem Inventar | Abhebung bucht Bank ab, Bargeld passt nicht in Taschen | Vor Abbuchung freie Kapazität ($1.000 pro freiem Slot) prüfen. |
| **BusinessIncome** | Doppelte Payouts im Koop | Host und Client rechnen beide separat ab | Strikt über `NetworkGuard.IsHostOrSingleplayer()` absichern. |
| **Minimap** | GC-Stutter durch Blip Instantiate/Destroy | Tausende Objekte werden per Frame erzeugt/zerstört | Fester 64-Blip-Objekt-Pool mit Shape-aware Edge-Clamping (0 GC Allocations/Frame). |
| **PotScanner** | `WaterSinglePot` ohne Eigentums-Prüfung | Spieler konnte Töpfe unbesessener Immobilien gießen | Vor Wasser-Abzug `PotTracker.FindByPtr` + `IsOwnedProperty` prüfen. |
| **AutoPackagingStation** | Kessel-Mesh sichtbar nach Platzierung | Base-Prefab-Renderer blieben aktiv | `HideBaseRenderers()` im Ghost-Placement und beim Laden aufrufen. |
| **AutoPackagingStation** | Hängenbleiben in `Blocked` / `NoPackaging` | State-Machine deckte Grenzfälle nach Reload nicht ab | `ApplySaveData` erzwingt `Idle`, wenn Inputs vorhanden sind. |
| **AutoPackagingStation** | Probe-Verweigerung bei Mengen > 1 | `CanItemFitInInventory` multiplizierte Mengen quadratisch | Immer 1-Unit-Probe (`GetDefaultInstance(1)`), Hinzufügen in Schleife. |
| **S1MCP** | Freeze bei großen Logfiles (ANR) | 700k Zeilen `Latest.log` synchron gelesen | `Stream.Seek(-Math.Min(Length, 256*1024), SeekOrigin.End)` — nur letztes 256-KB-Fenster lesen. |
| **Build-System** | 16x paralleler PowerShell-Spawn | `Directory.Build.targets` feuerte `DeployThirdParty` auf jedem Mod | Bindung an `Shared.dll`, Mutex auf sitzungslokal umgestellt. |
