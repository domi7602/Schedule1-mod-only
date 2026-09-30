# Battle-Tested Gotchas & IL2CPP Pitfalls (Cheat Sheet)

> **Single source of truth for Schedule I modding edge cases & architecture guardrails**  
> Based on practical experience from 15 active mods, hundreds of bugfixes, and IL2CPP crash analyses.

---

## 1. Core Architecture Standards (Non-Negotiable Guardrails)

### 1.1 IL2CPP Pointer Validation & Lifecycle Safety
- **Pointer Validation:** `obj != null` is never sufficient in IL2CPP — always check `obj != null && obj.Pointer != IntPtr.Zero && !obj.WasCollected` (managed proxies outlive the native C++ objects).
- **Public `IntPtr` Constructors:** Every `[RegisterTypeInIl2Cpp]` MonoBehaviour **MUST** declare a `public X(IntPtr ptr) : base(ptr) { }` constructor — otherwise the native IL2CPP bridge crashes hard on `AddComponent`.
- **No `foreach`/LINQ on `Il2CppSystem.Collections.Generic.List<T>`:** Use indexed `for` loops only (prevents massive per-frame GC allocations).
- **Scene-Transition Cleanup:** Explicitly clear static UI and GameObject caches (`.Clear()`) in `OnSceneWasUnloaded` / `OnPreLoad` to prevent dangling-pointer crashes on scene changes.
- **`UnityEngine.TextMesh` is stripped:** `AddComponent(Il2CppType.Of<TextMesh>())` throws `ArgumentException` in IL2CPP. Instead use zero-allocation `OnGUI()` HUDs with statically cached `GUIStyle`s or TextMeshPro (`TextMeshProUGUI`).

### 1.2 Responsive UI ("Method 3": `UITheme`)
- S1API phone canvases are high-DPI and rotated 90° — fixed pixel sizes are unreadable on different resolutions.
- **Single source of truth:** `S1Mods.Shared.UITheme` (`Source/Mods/Shared/src/UITheme.cs`). Mod UI elements use `UITheme.Sp(...)`, `UITheme.Dp(...)`, and `UITheme.Scale`.
- **Non-Destructive PhoneApp Lifecycle:** Never call `Object.Destroy()` or destructive UI clears in `OnPhoneClosed()` — only `_mainBG.SetActive(false)`. Otherwise the dreaded **"Transparent Phone"** bug occurs on the next open.
- **Rule 10/11:** `OnCreated()` fires only 1× per scene (S1API `HomeScreen_Start_Patch`), `OnPhoneClosed()` on every close. **Never** unsubscribe `MelonEvents.OnUpdate` subscriptions in `OnPhoneClosed()`, otherwise the app becomes unoperable on the second open.

### 1.3 Safe Persistence & Savegame Synchronization
- **Atomic Writes:** Use only `SafeStorage.SaveAtomic` / `SaveTextAtomic` (`.tmp` → atomic move → `.bak`), never unprotected `File.WriteAllText`.
- **Slot Isolation:** Strictly suffix mod saves with the slot number (`<name>_slot_{n}.json`) via `SaveSlots.GetActiveSlotNumber()` (with `>= 0` guard against `slot_-1.json`).
- **Transactional Save Integrity:** Runtime states stay in RAM; serialization to disk happens only on `S1API.Lifecycle.GameLifecycle.OnSaveComplete`. State reset happens on `OnPreLoad` / scene unload.
- **Culture-Safe Parsing:** Always parse and format `float` and `decimal` with `NumberStyles.Float + CultureInfo.InvariantCulture` (German comma breaks bank transactions and saves).

### 1.4 Resilient Harmony Patching (`PatchGuard`)
- **Graceful Degradation:** Always register patches via `S1Mods.Shared.PatchGuard.TryPatch` — signature changes after game updates then lead to clean logging instead of mod crashes.
- **Never `ref <Il2CppType> __result` in prefixes with `return false`:** This leads to `0xc0000005` access violations in `UnityPlayer.dll`. Apply manipulations directly to the UI/list instead.
- **Field accessors are not patchable:** IL2CPP properties like `BaseItemDefinition.get_DefaultStackLimit` cannot be patched (`Il2CppInterop can't be patched`). Postfix instance methods like `BaseItemInstance.get_StackLimit` instead.

### 1.5 Multiplayer Host Authority
- Strictly guard all economy and world-changing actions with `NetworkGuard.IsHostOrSingleplayer()`.
- **FishNet Singleplayer Trap:** In singleplayer `InstanceFinder.IsServer == false` because the NetworkManager is inactive. The host check must check: `NetworkManager == null || (Object)NetworkManager == null || InstanceFinder.IsServer`.

### 1.6 Decoupled Runtime Pattern (MonoBehaviour Boundary)
- Managed C# types (`List<string>`, DTOs) in the signatures of `[RegisterTypeInIl2Cpp]` MonoBehaviours cause `ClassInjector` to **reject the entire assembly**.
- **Solution:** Keep MonoBehaviours strictly primitive (Unity lifecycle + `string Guid`); manage complex data models via static, pure C# managers.

---

## 2. Battle-Tested Gotchas & Solutions Matrix

| Context / Mod | Problem / Gotcha | Cause | Verified Solution |
|---|---|---|---|
| **HomelessMod** (Outdoor) | `DestroyImmediate(GridItem)` → hard crash | Unity's native component tables get corrupted | **Never destroy `GridItem`:** `gi.enabled = false; gi.SetFootprintTileVisiblity(false);` and Harmony prefix on `GridItem.Destroy` with `return false` when `IsOutdoorItem`. |
| **HomelessMod** | FishNet clone desync | Vanilla prefabs expect FishNet server authority | Immediately after instantiation: `BuildManager.Instance.DisableNetworking(obj)` + `DisableNavigation(obj)`. |
| **HomelessMod** | Invisible objects after interior exit | `BuildableItem.SetCulled`/`Start` expects vanilla `Property` parent | Harmony prefix on `BuildableItem.Start` + `SetCulled`: if outdoor item, `return false; b.enabled = false;`. |
| **HomelessMod** | `Property` on `_streetRoot` destroys real-estate apps | Real-estate logic iterates all properties of the scene | **Never place `Property` on `_streetRoot`** — use `DontDestroyOnLoad`, manage via `StreetPropertyManager`. |
| **HomelessMod** | Floating grid tiles on asphalt | Vanilla grid renders tile decals | Disable `FootprintTile` child GameObjects, keep mesh renderer active. |
| **HomelessMod** | Dupe exploit on Alt+F4 after placement | Disk save happened immediately, inventory save only on regular cycle | Keep runtime objects strictly in RAM (`_activeStreetObjects`); only save on `OnSaveComplete`. |
| **HomelessMod** | Raycast falls through terrain | Street meshes are on `Grid` layer | Layer mask: `~LayerMask.GetMask("Ignore Raycast", "Player")` incl. `Grid`, `QueryTriggerInteraction.Ignore`. |
| **CustomSkateboard** | Memory leak via `renderer.material` | Accessing `.material` instantiates a copy in VRAM | Consistently use `r.sharedMaterial` + `static readonly` material caches. |
| **CustomSkateboard** | Avatar visual corruption | Deck swap accidentally manipulated clothing/hair | Strictly filter renderers by mesh name (`"deck"` / `"board"`) on child transforms. |
| **CustomSkateboard** | Duplicate dialogue injection on Jeff | Dialogue tree re-runs on reload | Before injection check existing nodes with `StringComparison.OrdinalIgnoreCase`. |
| **CustomSkateboard** | Double-tuning allocation churn | Awake and mount hooks fire multiple times | Idempotency filter `HashSet<IntPtr> _tunedBoards`, reset in `OnSceneWasLoaded`. |
| **MoreSaveSlots** | Slot allocation timing crash | Native arrays are allocated before slot patch | `SaveManager.Awake` prefix injects `SAVE_SLOT_COUNT` before internal array allocation. |
| **MoreSaveSlots** | Scene guard inverted | `IsInMainScene() == "MainMenu"` inverts logic | `IsInGameplayScene()` strictly checks scene name `"Main"`. |
| **PhoneApps** | WASD movement during text input | Unity InputField doesn't block vanilla player controls | `[RegisterTypeInIl2Cpp]` component toggles `S1API.Input.Controls.IsTyping` on focus/blur. |
| **PhoneApps** | `UnityEvent.AddListener` IL2CPP crash | Native delegate bridge breaks with standard Action | Always use `S1API.Utils.EventHelper.AddListener(...)` or `ButtonUtils.AddListener(...)`. |
| **PhoneApps** | Global state file leak | Global JSON leaks data between save slots | Consistently `<name>_slot_{n}.json` via `SaveSlots.GetActiveSlotNumber()` + `TryMigrateLegacy()`. |
| **PocketShop** | Empty shop catalog on fast load | `ShopInterface.AllShops` is not yet populated at start | Resilient retry loop (20 × 1.5s) on registry population. |
| **PocketShop** | Non-atomic purchase (money gone, no item) | Money was deducted before item handover | Instantiate item before deduction, try/catch with automatic refund on full inventory. |
| **BankApp** | Cash loss on full inventory | Withdrawal debits bank, cash doesn't fit in pockets | Before debit check free capacity ($1,000 per free slot). |
| **BusinessIncome** | Double payouts in co-op | Host and client both settle separately | Strictly guard via `NetworkGuard.IsHostOrSingleplayer()`. |
| **Minimap** | GC stutter from blip instantiate/destroy | Thousands of objects are created/destroyed per frame | Fixed 64-blip object pool with shape-aware edge clamping (0 GC allocations/frame). |
| **PotScanner** | `WaterSinglePot` without ownership check | Player could water pots of unowned properties | Before water deduction check `PotTracker.FindByPtr` + `IsOwnedProperty`. |
| **AutoPackagingStation** | Kettle mesh visible after placement | Base prefab renderers stayed active | Call `HideBaseRenderers()` on ghost placement and on load. |
| **AutoPackagingStation** | Stuck in `Blocked` / `NoPackaging` | State machine didn't cover edge cases after reload | `ApplySaveData` forces `Idle` when inputs are present. |
| **AutoPackagingStation** | Probe rejection at quantities > 1 | `CanItemFitInInventory` multiplied quantities quadratically | Always 1-unit probe (`GetDefaultInstance(1)`), add in loop. |
| **S1MCP** | Freeze on large log files (ANR) | 700k lines `Latest.log` read synchronously | `Stream.Seek(-Math.Min(Length, 256*1024), SeekOrigin.End)` — read only the last 256 KB window. |
| **S1MCP** | Bridge resets/timeouts after a handful of calls; methods "not found" | Bridge speaks **flat** JSON-RPC method names and gets flaky under rapid sequences | Use flat names (`list_npcs`, no `tools/call`); handshake first, don't build critical sequences on it. |
| **Build system** | 16x parallel PowerShell spawn | `Directory.Build.targets` fired `DeployThirdParty` on every mod | Bound to `Shared.dll`, mutex switched to session-local. |
| **Build system** | `dotnet format a.csproj b.csproj` → "Unrecognized command or argument" | `dotnet format` accepts exactly one project per call | Run it per project; the sln-level `--verify-no-changes` call is fine. |
| **Build system** | `package-release.ps1` for several mods in parallel fails with file locks | All mods share the `Shared` build outputs | Package serially per mod, or one run with `-Mod All` (internal serial). |
| **Deploy** | Game quit leaves `Schedule I.exe` as an unkillable zombie; mod DLLs stay file-locked | IL2CPP process stuck in uninterruptible kernel wait (Task Manager/`taskkill`/`Stop-Process` all fail) | Rename trick: `<Mod>.dll` → `<Mod>.dll.zombie`, copy the new DLL in; clean `.zombie` residue after reboot. On a hanging console **restart**, never shut down — Fast Startup hibernates the zombie and can bugcheck `0x12B` on next boot. |
| **Deploy** | `-c Release` silently overwrites the deployed DEBUG DLL in the game dir | Auto-deploy always force-copies (`SkipUnchangedFiles=false`) | Only build Release with `-p:S1NoDeploy=true` when the Debug DLL must survive. |
| **IL2CPP code** | `Il2CppSystem.Collections.List<T>` unresolved in mod code | Lives in the il2cpp assembly, outside the default alias | `extern alias il2cpp;` — or keep the data on the managed side as `List<T>`. |
| **Console commands** | A `^` character in a mod console command never appears | `^` (VK 220/0xDC) is a dead key: `^`+Space emits Backspace | Use an alias without `^`, or paste the command. |