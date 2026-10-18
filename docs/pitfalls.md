# Battle-Tested Gotchas & IL2CPP Pitfalls (Cheat Sheet)

> **Single source of truth for Schedule I modding edge cases & architecture guardrails**  
> Based on practical experience from the active and archived mods in this repository, hundreds of bugfixes, and IL2CPP crash analyses.

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
- **Typed MelonPreferences Entries Only:** Create preference entries via `ModConfig<T>` (which creates typed `MelonPreferences_Entry<T>`), never raw `MelonPreferences_Entry<object>`. Tomlet bug (verified 2026-10-09): `TomlSerializationMethods.Register(Type, …)` ignores its `Type` argument and registers the delegate under `typeof(object)` — so the last composite/enum type deserialized (e.g. `UnityEngine.KeyCode`) hijacks ALL object-typed entry loads. Integer values become nameless enum values and crash `MelonPreferences.Save()` at quit (`Tomlet: Cannot serialize 10 as an enum of type UnityEngine.KeyCode`); strings/floats/bools fail to parse and silently fall back to their defaults (value loss).

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

### 1.7 Diagnostic Coverage Must Fail Closed

A field-level catch must not leave an empty discovery result labeled complete. In the
GraphicsProbe static review, a throwing outer scene-count provider retained
`fieldErrors.scenes` but initially bypassed coverage/error accounting. Forward such
failures into the bounded discovery-error path as well as retaining field evidence;
keep unrelated settings/environment observations. The linked policy regression is
executable evidence for that boundary, **not native Unity runtime verification**.
Use shared material/profile references for diagnostics; do not call lazy instancing
or renderer-resolution accessors merely to inventory rendering state.

### 1.8 Bounded Discovery Can Still Starve Important Evidence

A 50,000-node cap does not guarantee useful coverage: the 2026-10-16 GraphicsProbe
0.1.1 gameplay report exhausted it in two early NPC roots, missed all cameras and
retained only particle false positives. Read scene/root admission and native renderer
types before interpreting empty sections. Admit roots before descendants, rotate
bounded breadth work fairly and reserve separate scan/record allowances for surface-like
and particle/line/trail matches. Keep both categories explicitly unconfirmed. Spread
requested captures over cooperative main-thread slices, cancel retained references on
scene transitions, and measure active work separately from wall/finalization duration.
Neither a pure traversal test nor a requested millisecond budget proves native frame
performance; individual native getters and final report IO can still hitch. Test
acquisition failures through actual managed collector orchestration, not source strings
alone: clear stale array/cursor bounds before acquiring a new batch, and distinguish
unknown scene counts from successfully observed zero scenes in aggregate sections.

### 1.9 Rendering State Is Rewritten Every Frame (GoldenHour, measured 2026-10-17)
The game's day/night grading rewrites `RenderSettings.sun.color` (and ambient/fog state)
**every frame** — a mod write is overwritten within ~16 ms. Measured via S1MCP: the sun color
drifts continuously with the time-of-day curve (e.g. `0.985047 → 0.985153` over 0.3 s at noon).
Consequences for any mod that writes lighting/render state:
- A once-per-second reapply cadence **loses every frame race** and the change never shows on
  screen. Write in `OnLateUpdate` every frame (after the game's Update writes) or patch the
  game's own writer.
- Never blend from a stale snapshot of the value: the game drifts it, so rebase onto its
  current value (detect your own writes by value) or the parameter freezes at toggle-time
  values and a later restore drags it back hours. A strength change is not game drift: the
  blend at the previous strength is still our write. Adopting it freezes the old tint.
  See `Source/Mods/WorldLook/src/Lighting/BaselinePolicy.cs`.
- `Sun.intensity` follows the day/night curve too (0 at 06:00, ~1.7 at noon, ~2.6 at 18:00 in
  the Main scene) — a biased sun **color** is invisible while intensity is 0 (dawn/dusk).
- URP `Volume` `colorFilter` is only writable when the game instantiated its own profile
  (`volume.profile != volume.sharedProfile`); otherwise the shared asset would be mutated.
  `RenderSettings.fog == false` in the Main scene — fog writes have no target.

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
| **Shared / all mods** | Quit-save crash `Tomlet: Cannot serialize 10 as an enum of type UnityEngine.KeyCode` | Tomlet bug: `TomlSerializationMethods.Register(Type, …)` stores composite deserializers under `typeof(object)`; the last enum type loaded (here `UnityEngine.KeyCode` via a third-party mod's typed entries) hijacks `MelonPreferences_Entry<object>` loads — ints become nameless KeyCode values (confirms: `MaxBatchSize=10`→`KeyCode 10`, `PayoutHour=0`→`None`, `LastShopIndex=12`→`Clear`), other types silently fall back to defaults | `ModConfig.GetOrCreateEntry` creates **typed** entries (`CreateEntry<T>` via reflection with the property type). Never create raw object-typed MelonPreferences entries. (Root-caused 2026-10-09 via the temporary `PrefProbe` diagnostic mod; probe removed afterwards.) |
| **PhoneApps** | `UnityEvent.AddListener` IL2CPP crash | Native delegate bridge breaks with standard Action | Always use `S1API.Utils.EventHelper.AddListener(...)` or `ButtonUtils.AddListener(...)`. |
| **PhoneApps** | Global state file leak | Global JSON leaks data between save slots | Consistently `<name>_slot_{n}.json` via `SaveSlots.GetActiveSlotNumber()` + `TryMigrateLegacy()`. |
| **PocketShop** | Empty shop catalog on fast load | `ShopInterface.AllShops` is not yet populated at start | Resilient retry loop (20 × 1.5s) on registry population. |
| **PocketShop** | Non-atomic purchase (money gone, no item) | Money was deducted before item handover | Instantiate item before deduction, try/catch with automatic refund on full inventory. |
| **BankApp** | Cash loss on full inventory | Withdrawal debits bank, cash doesn't fit in pockets | Before debit check free capacity ($1,000 per free slot). |
| **BusinessIncome** | Double payouts in co-op | Host and client both settle separately | Strictly guard via `NetworkGuard.IsHostOrSingleplayer()`. |
| **Minimap** | GC stutter from blip instantiate/destroy | Thousands of objects are created/destroyed per frame | Fixed 64-blip object pool with shape-aware edge clamping (0 GC allocations/frame). |
| **PotScanner** | `WaterSinglePot` without ownership check | Player could water pots of unowned properties | Before water deduction check `PotTracker.FindByPtr` + `IsOwnedProperty`. |
| **AutoPackagingStation** | `Use/SetPlayerUser` throws in `IsClientInitialized` after cloning cauldron | Adding `PackagingStation` after spawn creates a native NetworkBehaviour without the packaging prefab's serialized graph | Preserve the complete packaging donor; transfer only owned cauldron footprint geometry before publication. Keep templates active-self under an inactive cache, never scene-active just to prime Awake. Native setup/canvas/processing are log-backed on v0.4.7 (2026-10-15), with subsequent owner-confirmed gameplay acceptance for the tested session; multiplayer remains unverified. |
| **AutoPackagingStation registration** | Retry accepts a partially published/default prefab; repeated failures retain extra templates | Builder publishes before returning; item-ID existence is not completion, and composition had no bounded owner | Prepare prefab and ghost before Build; validate exact prepared identity on retries; reserve one template and release its inactive cache on deinit. Source/policy tests do not exercise native initialization. |
| **AutoPackagingStation native interaction** | `IntObj` resolves and setup succeeds, but E prompt is absent | A nearer placed root collider intercepts selection before the native child IntObj | Native f12 `CheckHover` uses `GetComponentInParent` (no child lookup) and cuts sphere candidates by nearest blocking ray distance. The 2026-10-15 v0.4.6 session restored native E/canvas interaction on two stations after removing root-collider manipulation. Retain inherited native geometry; do not add a replacement E handler, duplicate listeners or invoke Awake. Native tracing found no IntObj listener registration in the Awake UserLogic body; serialized event counts alone do not prove execution. Version 0.4.7 retires the read-only observer/dumps/bookkeeping; its own native canvas and Pack/Unpack path are log-backed (2026-10-15), with subsequent owner confirmation that tested gameplay worked. The general report did not itemize additional checks or establish separate multiplayer/migration coverage. |
| **AutoPackagingStation** | Kettle mesh visible after placement | Base prefab renderers stayed active | Call `HideBaseRenderers()` on ghost placement and on load. |
| **AutoPackagingStation** | Stuck in `Blocked` / `NoPackaging` | State machine didn't cover edge cases after reload | `ApplySaveData` forces `Idle` when inputs are present. |
| **AutoPackagingStation** | Probe rejection at quantities > 1 | `CanItemFitInInventory` multiplied quantities quadratically | Always 1-unit probe (`GetDefaultInstance(1)`), add in loop. |
| **S1MCP** | Freeze on large log files (ANR) | 700k lines `Latest.log` read synchronously | `Stream.Seek(-Math.Min(Length, 256*1024), SeekOrigin.End)` — read only the last 256 KB window. |
| **S1MCP** | Bridge resets/timeouts after a handful of calls; methods "not found" | **Fixed in 1.0.2/1.0.3 (verified 2026-10-17, 7/7 integration checks in the main menu):** dead/half-open clients no longer block new connections (accept loop replaces them, EOF is EOF, `.Connected` no longer used as liveness), and `handshake`/`status`/`list_methods`/`heartbeat` answer on the server thread (`IThreadSafeCommandHandler` fast path) without game focus — proven by `inline_requests` in `status`. Legacy rule still applies: flat method names (`list_npcs`, no `tools/call`), handshake first. |
| **Build system** | 16x parallel PowerShell spawn | `Directory.Build.targets` fired `DeployThirdParty` on every mod | Bound to `Shared.dll`, mutex switched to session-local. |
| **Build system** | `dotnet format a.csproj b.csproj` → "Unrecognized command or argument" | `dotnet format` accepts exactly one project per call | Run it per project; the sln-level `--verify-no-changes` call is fine. |
| **Build system** | `package-release.ps1` for several mods in parallel fails with file locks | All mods share the `Shared` build outputs | Package serially per mod, or one run with `-Mod All` (internal serial). |
| **Deploy** | Game quit leaves `Schedule I.exe` as an unkillable zombie; mod DLLs stay file-locked | IL2CPP process stuck in uninterruptible kernel wait (Task Manager/`taskkill`/`Stop-Process` all fail) | Rename trick: `<Mod>.dll` → `<Mod>.dll.zombie`, copy the new DLL in; clean `.zombie` residue after reboot. On a hanging console **restart**, never shut down — Fast Startup hibernates the zombie and can bugcheck `0x12B` on next boot. |
| **Deploy** | `-c Release` silently overwrites the deployed DEBUG DLL in the game dir | Auto-deploy always force-copies (`SkipUnchangedFiles=false`) | Only build Release with `-p:S1NoDeploy=true` when the Debug DLL must survive. |
| **IL2CPP code** | `Il2CppSystem.Collections.List<T>` unresolved in mod code | Lives in the il2cpp assembly, outside the default alias | `extern alias il2cpp;` — or keep the data on the managed side as `List<T>`. |
| **URP volume (IL2CPP)** | The mod's volume parameter never applies and the log blames a missing component | The `GlobalVolume` was looked up by filtering scene **roots** on `EnvironmentFX` — but the live path is `@Managers/@EnvironmentFX/GlobalVolume`, i.e. `EnvironmentFX` is a child of `@Managers`, not a root. The filter matched nothing, so the volume was never reached and the "no ColorAdjustments" warning was a red herring | Filter roots on a token that actually is a root (`Managers`) and keep the deep child lookup for the rest. Verified live 2026-10-16 via S1MCP `inspect_component`: `game_object_path = @Managers/@EnvironmentFX/GlobalVolume`. |
| **URP volume (IL2CPP)** | `HasInstantiatedProfile()` reports `unknown` / the profile gate never runs | Same root cause as above — the gate sits *after* the lookup, so an unreachable volume means the gate is never evaluated and the diagnostic stays empty | Read the gate immediately after the volume is resolved, and let the failure message name the path actually searched so a dead lookup cannot masquerade as a policy skip. |
| **URP volume (IL2CPP)** | Is it safe to write `volume.profile`? | In this game **no**: live evidence shows `m_InternalProfile == null` and `profile == sharedProfile`, i.e. the game never instantiates its own profile. Reading `.profile` would create the clone and decouple the game's day/night grading | Gate on `Volume.HasInstantiatedProfile()` and skip the volume key entirely when the game did not instantiate it — the other parameters carry the look. |
| **Console commands** | A `^` character in a mod console command never appears | `^` (VK 220/0xDC) is a dead key: `^`+Space emits Backspace | Use an alias without `^`, or paste the command. |
| **URP volume (IL2CPP)** | A mod's `ColorAdjustments` write has no visible effect | `VolumeParameter` values only contribute when the component is `active` **and** the parameter's `overrideState` is set — otherwise the write is a silent no-op | Force `adjustments.active = true` and `adjustments.colorFilter.overrideState = true` before writing, capture both first, and restore them on the way out. |
| **URP volume (IL2CPP)** | Day/night grading freezes after the mod touches a `GlobalVolume` | Reading `Volume.profile` **instantiates** the profile (`m_InternalProfile`); the VolumeManager then blends from that clone instead of `sharedProfile`, decoupling the game's own per-frame colorFilter writes | Only use `volume.profile` when `volume.HasInstantiatedProfile()` is true (then the clone is what the game itself writes into). Otherwise skip the volume key entirely — never let the mod create the clone. `HasInstantiatedProfile()` exists in the installed `Unity.RenderPipelines.Core.Runtime.dll` (verified 2026-10-16). |
| **URP volume (IL2CPP)** | Volume cache silently loses the restore target | Gating "is this volume usable" on `Volume.weight > 0` drops the cached component while the profile instance still carries the mod's tint | Keep the cache valid as long as the object lives and is enabled/global/active; do not gate on `weight`. |