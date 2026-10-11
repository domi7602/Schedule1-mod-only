# PropertyLEDs Phase 0 — installed environment and API baseline

Audit time: **2026-10-10T19:41:36+02:00**. Repository: `C:/Users/pc/Schedule1-mod-only`.
Game root: `C:/Program Files (x86)/Steam/steamapps/common/Schedule I`.

## Evidence scope and boundaries

- Read-only inspection of installed binaries, Steam manifest, existing log, S1API source, shared documentation, and NotesApp/BankApp/Weather implementations. The only repository write by this audit is this document. No existing mod, Shared, root documentation, dependency, installed DLL, configuration, or save was changed. No game launch/stop, deployment, or in-game mutation was performed.
- `tasklist.exe /FI "IMAGENAME eq Schedule I.exe" /FO CSV` returned **no matching tasks**. `netstat.exe -ano -p TCP` contained **no port 8765 entry**. A socket connect probe did not establish a connection (Windows result 10035). There is no current live measurement/visual verification session.
- Static contracts below are from **scoped `ilspycmd -t` decompiles of installed DLLs**, cross-checked against checked-out source, not a Nexus mod. No source or assets from the original PropertyLEDs mod were accessed.
- IL2CPP generated proxies prove member existence/types, not native method behavior, registry population, actual room dimensions, lighting appearance, or event-raising order. Successful offline inspection is **not** in-game compatibility verification.

## Installed identities — measured, not copied from older docs

| Component | Installed evidence |
|---|---|
| Schedule I | **0.4.7f12**, IL2CPP, x64 — `MelonLoader/Latest.log:9-10,20-23` |
| Steam build | **25796331** — `C:/Program Files (x86)/Steam/steamapps/appmanifest_3164500.acf:13,21` |
| Steam branch evidence | `UserConfig/BetaKey = public`, `MountedConfig/BetaKey = beta` — manifest lines 33-42. Report both; do not silently equate them. |
| Unity | **2022.3.62f2** — latest log line 22 |
| MelonLoader | runtime banner **0.7.3 Open-Beta**, assembly/file **0.7.3.0**, product **0.7.3+982ed9981ac5b0619648a2e79bcc02fd8861db33** |
| S1API | runtime banner **3.2.1**, assembly/file **3.2.1.0**, product **3.2.1+a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b** |
| S1API loader plugin | runtime banner **S1APILoader 2.5.0**; assembly/file **1.0.0.0**, product **1.0.0+a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b**. Plugin display version is not its assembly version. |
| S1MCP | runtime banner **1.0.1** (`Latest.log:188`); assembly/file **1.0.0.0**, product **1.0.0+98b7aab000bc9713bc8a671ebb05d9f3455dee7c** |
| SDK / decompiler | user-local .NET SDK **8.0.425**; `C:/Users/pc/.dotnet/tools/ilspycmd.exe` **9.1.0.7988** |
| Client prerequisites | `C:/Users/pc/AppData/Local/Schedule1Tools/s1mcp-venv/Scripts/python.exe` executed successfully: Python **3.13.15**, installed `mcp=2.0.0`, `psutil=7.2.2`, `pydantic=2.13.4`. Imports/versions only; no client handshake was possible. |

S1API source checkout `ThirdParty/S1API`: `git rev-parse HEAD` =
`a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b`; `git describe --tags --always` = `v3.2.1`;
`git status --short` = clean. The superproject's **current HEAD** `git ls-tree HEAD ThirdParty/S1API` records this same commit. Product metadata agrees with the checkout; affected type bodies/signatures were additionally inspected. This is not a claim that every binary byte was rebuilt from source during the audit.

External primary-source corroboration: GitHub's official API `https://api.github.com/repos/ifBars/S1API/git/ref/tags/v3.2.1` returned tag `v3.2.1` at commit `a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b`. The release API `https://api.github.com/repos/ifBars/S1API/releases/tags/v3.2.1` returned release **S1API 3.2.1**, published **2026-10-09 12:03:46 UTC**, and documents the managed PhoneApp.Exit compatibility parameter in its compatibility section. Both responses were fetched successfully with Python urllib after the configured web-extraction backend failed and search returned no results. This corroborates provenance only; upstream validation is not a PropertyLEDs test on this machine.

Older root documentation contains conflicting beta.8/f11 statements (`docs/compatibility.md:13-14`, `DEVELOPERS.md:49`, older passages in `AGENTS.md`). These are historical/stale relative to measured files; left untouched per audit scope.

### SHA-256 of files actually read

Computed from file bytes with Python `hashlib.file_digest(..., 'sha256')`. Paths below are relative to the game root.

| File | SHA-256 |
|---|---|
| `Schedule I.exe` | `ec99366083ae5a4068d0c8091d80f857b65c57fb6ef603ab952d835469246ecf` |
| `GameAssembly.dll` | `61829050e56ca180d67be592e4bfe61ec97d2e9573f720d90a54e318a7e4381d` |
| `MelonLoader/net6/MelonLoader.dll` | `bdd43dc0f3893c208c95389b863ab61c967ae14204da0dd8051647e754c8709b` |
| `Mods/S1API.Il2Cpp.MelonLoader.dll` | `85b218ff0ac3d18ec03e2a959f13ce13c596d38d46ab78f7908ec9fa7533c6e3` |
| `Plugins/S1APILoader.MelonLoader.dll` | `d58b1d3ceefa8863d627ed395c55d824faad8559e3614b19b1b5ebc92d271f60` |
| `Mods/S1MCPServer-IL2CPP.dll` | `bae2659cb6ca78e249e5021d752c2b9bcdbd1a50f95ce89d6b92d09f18f0fe35` |
| `MelonLoader/Il2CppAssemblies/Assembly-CSharp.dll` | `441720a60c7729945773e050e4a92bf0ffbf062124626348407fef4c59471a6d` |
| `MelonLoader/Il2CppAssemblies/UnityEngine.CoreModule.dll` | `0aa474bc0e139c03953db8f22f1b38a1d3948d50394534681d0cc80661060829` |
| `MelonLoader/net6/Il2CppInterop.Runtime.dll` | `3878ce0ba0c60385f76f2ba37e07ad0ee3649d019703aec8695db3402abeedd7` |
| `MelonLoader/net6/0Harmony.dll` | `6c898933b52149e8bcf6722305c7e4d94add47c9e53b364096f91721624c0877` |
| `UserLibs/S1MAPI_Il2Cpp.dll` | `ef612a9055ff0b3c4ccaec4e3288fd7f54118cb4764a981233443ed44cc27998` |

The loader, S1API and S1MCP hashes agree with `Latest.log:7,38,98,100`. `Assembly-CSharp.dll` last modified **2026-10-08 13:01:14 +02:00**; `GameAssembly.dll` **2026-10-08 07:58:07 +02:00**. The later **2026-10-10** log says **Assembly is up to date. No Generation Needed** (line 65). This supports proxy freshness for this installed build; it does not recover native bodies.

## Exact API contract — property/business

Namespace is **`S1API.Property`** (singular). `S1API.Properties` is a different API; do not substitute it.
Managed collection returns below are `System.Collections.Generic.List<T>` snapshots, not live registry views.

```csharp
// S1API.Property.PropertyManager
public static List<PropertyWrapper> GetAllProperties();
public static List<PropertyWrapper> GetOwnedProperties();
public static PropertyWrapper? FindPropertyByName(string name);

// S1API.Property.BusinessManager
public static List<BusinessWrapper> GetAllBusinesses();
public static List<BusinessWrapper> GetOwnedBusinesses();
public static BusinessWrapper? FindBusinessByName(string name);

// PropertyWrapper : BaseProperty
public PropertyWrapper(Il2CppScheduleOne.Property.Property property);
public override string PropertyName { get; }
public override string PropertyCode { get; }
public override float Price { get; set; }
public override bool IsOwned { get; }
public override int EmployeeCapacity { get; set; }
public override void SetOwned();
public override bool IsPointInside(UnityEngine.Vector3 point);
public UnityEngine.Vector3 ExteriorSpawnPosition { get; }
public UnityEngine.Vector3 InteriorSpawnPosition { get; }
public bool IsContentCulled { get; set; }
public int EmployeeCount { get; }
public int BuildableItemCount { get; }

// BusinessWrapper : PropertyWrapper
public BusinessWrapper(Il2CppScheduleOne.Property.Business business);
public float LaunderCapacity { get; set; }
public List<S1API.Property.LaunderingOperation> LaunderingOperations { get; }
public float CurrentLaunderTotal { get; }
public float AppliedLaunderLimit { get; }
public int LaunderingOperationCount { get; }
public bool IsAtLaunderingCapacity { get; }
public float LaunderingCapacityUsagePercent { get; }
public void AddLaunderingOperation(float amount, int minutesSinceStarted);
```

**Source evidence:** `ThirdParty/S1API/S1API/Property/BaseProperty.cs:15-67`,
`PropertyManager.cs:18-70`, `PropertyWrapper.cs:35-124,131-201`,
`BusinessManager.cs:21-59`, `BusinessWrapper.cs:18-112`. Installed decompiles contain the same affected surface.

**Behavior visible in wrapper bodies:** manager calls allocate a fresh managed list and new wrappers per call; name lookup uses exact case-sensitive equality. They enumerate native registries internally. Avoid calling them every frame. `IsPointInside` delegates to `DoBoundsContainPoint`. `Price` setter changes the game property and its sale signage; `IsContentCulled` setter calls native `SetContentCulled`; these are mutations, not harmless readbacks. `InnerProperty` and `InnerBusiness` are **internal readonly** fields: a consuming mod cannot directly access them through the public S1API contract. No public LED/room-light registration API was found on these property/business wrappers.

### Native adapter surface — installed Assembly-CSharp

`Il2CppScheduleOne.Property.Property : Il2CppFishNet.Object.NetworkBehaviour`;
`Il2CppScheduleOne.Property.Business : Property`. Proxy public members relevant to discovery:

- Native **`Il2CppSystem.Collections.Generic.List<Property>`** static field-accessor properties: `Properties`, `UnownedProperties`, `OwnedProperties` (installed decompile lines 1182,1197,1212). Business has its own `List<Business>` `Businesses`, `UnownedBusinesses`, `OwnedBusinesses` (lines 183,198,213). Do not assume the two sets are disjoint without runtime evidence; merged enumeration must diagnose/deduplicate native identity.
- `string PropertyName { get; }`, `string PropertyCode { get; }`; `float Price { get; set; }`; `int EmployeeCapacity { get; set; }`. `bool IsOwned { get; set; }` exists in the generated proxy, but its native setter metadata says **protected**; prefer read-only ownership checks for this mod.
- `PropertyContentsContainer Container { get; set; }` (line 2066), whose native setter metadata says **private**. `PropertyContentsContainer : UnityEngine.MonoBehaviour` exposes `Property Property { get; set; }` and `void SetProperty(Property property)` (container decompile lines 38,77).
- `GameObject BoundingBox`, `Il2CppReferenceArray<BoxCollider> propertyBoundsColliders`, `Il2CppSystem.Collections.Generic.List<...Grid> Grids`, `Transform SpawnPoint`, `Transform InteriorSpawnPoint`, `Il2CppReferenceArray<GameObject> ObjectsToCull` are public generated accessors for native fields. These are available handles, **not measured placement geometry**.
- `bool DoBoundsContainPoint(Vector3 point)`; `bool IsPointInsideBox(Vector3 worldPoint, BoxCollider box)`; `virtual void SetContentCulled(bool culled)`; `virtual void OnDestroy()`; `virtual void RecieveOwned()` (**retain this exact spelling**); `void SetOwned()`.
- `static Property.PropertyChange onPropertyAcquired` and `UnityEngine.Events.UnityEvent onThisPropertyAcquired` exist as generated field accessors, not managed C# events. Nested delegate exposes `void Invoke(Property property)`, `PropertyChange(IntPtr pointer)`, `PropertyChange(Il2CppSystem.Object @object, IntPtr method)` and implicit conversion from `System.Action<Property>` (lines 57,69,106,111). Store the converted delegate for exact removal; do not recreate it during unsubscribe.
- There is no property-specific public light list/member in the inspected Property/Business surface. **Recommendation, not runtime proof:** use a narrow, pointer-guarded native adapter for roots/bounds, while using S1API for supported ownership/app/lifecycle APIs. Discover eligible child lights only under the verified owning root, never via a global scene scan.

Registry membership/counts, `Container` hierarchy coverage (static shell vs placed contents), ownership acquisition callback timing, property codes, light transforms, materials, and suitable strip mounting points remain **unmeasured**.

## Exact API contract — phone app and lifecycle

```csharp
// S1API.PhoneApp.PhoneApp : S1API.Internal.Abstraction.Registerable
protected abstract string AppName { get; }
protected abstract string AppTitle { get; }
protected abstract string IconLabel { get; }
protected abstract string IconFileName { get; }
protected virtual UnityEngine.Sprite? IconSprite { get; }
protected virtual PhoneApp.EOrientation Orientation { get; }
protected override void OnCreated();
protected abstract void OnCreatedUI(UnityEngine.GameObject container);
protected virtual void OnPhoneClosed();
protected override void OnDestroyed();
public bool IsOpen();
public void OpenApp();
public void CloseApp();
public virtual void Exit(S1API.PhoneApp.ExitAction exit);
// Nested EOrientation values: Horizontal = 0, Vertical = 1.

// S1API.Lifecycle.GameLifecycle
public static event System.Action? OnPreLoad;
public static event System.Action? OnLoadComplete;
public static event System.Action? OnPreSceneChange;
public static event System.Action? OnSaveInfoLoaded;
public static event System.Action? OnSaveStart;
public static event System.Action? OnSaveComplete;
```

`PhoneApp` defaults to **Horizontal**, so explicitly override orientation. Installed `ExitAction` is a **sealed managed class** with `bool Used { get; set; }` and an **internal** `(Func<bool>, Action<bool>)` constructor. **No `ExitAction.Default` or `ReturnToHomeScreen` enum value exists** in this installed type; use `CloseApp()` for a manual close. Likewise **no `GameLifecycle.OnSaveLoaded`** exists.

**Installed control flow (not inferred from old skill prose):**

1. `HomeScreen_Start_Patch.Postfix` applies to `HomeScreen.Start`, requires active scene `Main`, discovers `PhoneApp` subclasses, requires a **public parameterless constructor**, then calls `CreateInternal()` → `SpawnUI()` → `SpawnIcon()` (decompile lines 15,28-49). Base `OnCreated()` registers the app and its external-host catalog entry; an override must call `base.OnCreated()`.
2. UI parent is `homeScreen.transform.parent.Find("AppsCanvas")`; panel named `AppName`, then child **`Container`**. `CreateAppContainer` calls `OnCreatedUI(container)` and sets container inactive. The prefix above runs on HomeScreen.Start, **not every app open**; do not depend on OnCreated re-firing when reopening.
3. `IsOpen()` requires `_appPanel != null`, `_appPanel.activeInHierarchy`, and `Phone.ActiveApp == _appPanel`; it does **not simply return Phone.Instance.IsOpen**. `OpenApp` / `CloseApp` manage active app, AppsCanvas/HomeScreen visibility, orientation and container state. CloseApp does **not** directly invoke `OnPhoneClosed`; that hook is subscribed to native `Phone.onPhoneClosed` in SpawnUI.
4. Vertical layout swaps parent rect width/height, sets centered anchors, and rotates panel **90 degrees around Z** (`PhoneApp.cs:588-611`). Do not force the vertical panel back to unrotated full stretch; initialize Shared UITheme on the actual passed container.
5. `PhoneAppButtonHandler.OnDestroy()` calls `DestroyInternal()` unless already destroying; base `OnDestroyed` destroys its panel, unregisters external catalog and removes phone-close/app-close/exit delegates. Custom instance subscriptions/resources belong in true teardown with `base.OnDestroyed()`, never ordinary phone close.
6. `GameLifecycle.Initialize()` bridges native `LoadManager.onPreLoad`, `onLoadComplete`, `onPreSceneChange`, `onSaveInfoLoaded` and `SaveManager.onSaveStart`, `onSaveComplete` via stored UnityAction listeners. Its `Initialize`/`Reset` are **internal**, not mod APIs. Native fields are `UnityEvent` (LoadManager decompile lines 2824,2884-2914; SaveManager lines 689,704). `LoadManager.IsGameLoaded`, `IsLoading`, `ActiveSaveInfo` are available guard/readback members.
7. `InvokeOnLoadComplete` calls `DeferredMapResolver.ResolveAll()` **before** invoking managed subscribers. Subscriber invocation is inside one outer try/catch, so a throwing earlier subscriber can abort later subscribers; handle exceptions within each mod callback. Reset removes pre-load/load-complete/save-info/save listeners but deliberately retains the pre-scene-change manager/listener; Initialize replaces that listener when manager identity changes. Do not reproduce lifecycle initialization yourself.

**Sources:** `ThirdParty/S1API/S1API/PhoneApp/PhoneApp.cs:170-325,450-622,753-770`,
`ThirdParty/S1API/S1API/Lifecycle/GameLifecycle.cs:50-145,151-218,221-243`;
scoped installed `HomeScreen_Start_Patch`, `PhoneAppButtonHandler`, `PhoneApp`, `ExitAction`, `GameLifecycle`, `LoadManager`, `SaveManager` decompiles.

**Existing log, not a new test:** the latest log was last modified **2026-10-10 19:07:29 +02:00**. It logs CustomSkateboard `GameLifecycle.OnPreLoad` at **19:07:09.129** (line 335), app discovery at **19:07:12.067** (350), and its `OnLoadComplete` handler at **19:07:14.676** (384). NotesApp, BankApp, Weather initialization appears at lines 351,358,371. This proves those callbacks/app-discovery markers occurred in that earlier session, not full correctness of those apps or PropertyLEDs. `OnSaveInfoLoaded` firing/order on the current stack is **not established** by this uninstrumented log. Historical September negative findings must not be promoted to a universal guarantee.

## Existing app lifecycle patterns inspected (no files edited)

| App/source | Verified source behavior | What to reuse / not infer |
|---|---|---|
| `Source/Mods/NotesApp/src/NotesApp.cs:208-303`; `Mod.cs:18-24` | OnCreated loads slot-aware notes, calls base, assigns `_active`, installs static update + SaveInfoLoaded/LoadComplete dispatch once. LoadComplete refreshes path/data/list. UITheme initialized on supplied container; isolated background starts hidden; input focus helper attached. Close hides background only. Main unload clears `_active`. | Static dispatcher avoids retaining old per-scene instances. SaveInfoLoaded is supplementary, not the only restore hook. No OnDestroyed override in this class; do not claim its close path disposes instance subscriptions. |
| `Source/Mods/BankApp/src/BankApp.cs:108-181,967-1017`; `Mod.cs:32-85` | One static update/balance/history dispatcher; active instance assigned on creation, cleared on Main unload. UITheme/root-hidden/input-focus pattern. Update shows/hides by IsOpen, refreshes on first open and every 1.5 s while open. Close hides/reset form state, no unsubscribe/destroy. Mod's PreLoad clears transaction cache with slot retained; SaveInfoLoaded/LoadComplete handlers are **no-op**; deinit unsubscribes mod lifecycle handlers. | Do not copy no-op lifecycle callbacks as a property restore implementation. Reopening retains dispatcher subscriptions. Main scene load/unload cache clearing is separate from phone close. |
| `Source/Mods/Weather/src/WeatherApp.cs:229-315,406-423`; `Mod.cs:16-24` | Base OnCreated + `_active`, defensive unsubscribe/subscribe for update, one static WeatherChanged dispatcher. Subscription flag set only after update wiring. Closed events cache/mark dirty; render only if open. UITheme.InitializeForTextApp and isolated hidden root; no input field/persistence. Close hides only; Main unload clears active. | Good read-only dashboard lifetime example. Current snapshot polling while open covers missed change events. Source patterns/log initialization are not a visual rendering test. |

All three intentionally keep update/event dispatch alive across ordinary phone closure. Native proxies must still be pointer/WasCollected guarded; root/input/listener/layout patterns in older code are examples, not permission to copy every cast, silent catch, or mutation.

## S1MCP diagnostic capability audit — installed DLL takes precedence

The installed `S1MCPServer.Core.CommandRouter.InitializeHandlers` contains **46 distinct registrations**, matching `Latest.log:272`. Its available safe/introspection routes include:

- `handshake`, `list_methods`, `heartbeat`, `get_game_state`, `capture_logs`;
- `get_hierarchy`, `get_scene_hierarchy`, `list_scenes`, `get_scene_objects`;
- `inspect_object`, `inspect_component`, `list_components`, `get_component_by_type`, `get_member_value`, `inspect_type`, `list_members`;
- `get_field`, `get_component_property`, `get_transform`, `is_active`.

The protocol is flat JSON-RPC over TCP `127.0.0.1:8765`, 4-byte little-endian length prefix, single client, Unity work dispatched on the game main thread. Handshake/list_methods must be rechecked once a parent/user starts a live session. Full object paths are preferred over ambiguous names; use bounded/scoped hierarchy and cached targets. Do **not** use `find_gameobjects` or other unreviewed global searches as a light-count benchmark.

**Important missing/illusory capabilities verified in deployed code:**

- `list_properties` is registered but its handler returns **an unconditional empty list/count 0**. `get_property` validates/echoes the requested identifier/name, then returns **hard-coded position (0,0,0), empty NPC list/count**. They are **stubs**, not ownership or geometry evidence. No `list_buildings` route exists.
- `DebugUiImageHandler` exists both in source and installed type list, but **`inspect_ui_image` and `read_sprite_pixels` are not registered in the installed router or current local router source**. Presence of a handler class is not exposed capability. The installed router exposes no screenshot/camera capture, frame-time percentile, GC allocation, renderer/light budget, property-light mapping, or benchmark route.
- Generic field/property reads can inspect known component state but cannot prove visible illumination, emitted strip appearance, camera framing, rendering performance or allocation-free behavior. Generic `call_method`/set/teleport/load routes are mutations and were not used in this audit. `find_objects_by_type` delegates to reflection helpers and does not itself enforce a property-root/max-result bound; do not assume it is a cheap bounded scan.

Evidence: installed scoped `S1MCPServer.Core.CommandRouter.cs:23-79`,
`S1MCPServer.Handlers.PropertyCommandHandler.cs:38-100`,
`S1MCPServer.Handlers.Debug.DebugHierarchyHandler.cs:90-154`;
local `ThirdParty/S1MCPServer-master/S1MCPServer/Core/CommandRouter.cs:27-103`,
`Handlers/Debug/DebugUiImageHandler.cs:13-49`,
`Skills/schedule1-mcp/references/tool-signatures.md:13-52,261-265`.

### Outstanding prerequisites / measurements (explicitly NOT verified)

1. **Parent/user-controlled live session:** game running with S1MCP accepting a single client, correct process identified, Main save loaded, ownership/safe test save/slot documented. This subagent must not launch/stop the game.
2. **Scoped property survey:** native registry counts and actual codes, owning root + Container/bounds coverage, inactive/culled objects, ceiling heights/local transforms/material/shader choices for each eligible owned property/business. Record unsupported/missing geometry rather than invent coordinates.
3. **Diagnostics implemented before claiming budgets:** mod-local read-only counters/timers for eligible roots, discovered fixtures, created Light/Renderer/material objects, cached vs rescan calls, destroyed/stale references, event/create/destroy/restore counts, and sampled frame-time/GC deltas. S1MCP currently has no purpose-built measurement route. No replacement dependency or server changes are authorized by Phase 0.
4. **Visual capture path:** external screenshot/video or a deliberately implemented capture tool; none is available through the installed S1MCP router. Fixed resolution/camera/player pose, time-of-day/weather/exposure, graphics/shadow settings and before/after reference captures are needed. S1MCP component values alone are insufficient.
5. **Runtime matrix:** app first open/close/reopen, switching another app, Menu→Main→Menu and repeated reload, save A→B→A, ownership acquisition, unowned exclusion, culling/room entry/exit, original-light restoration/disable, save/restart settings, and host/client handling. None was executed for PropertyLEDs in Phase 0.
6. **Controlled performance A/B:** same scene/camera/time/graphics and settled load, disabled baseline vs enabled, warm-up then sample window, sample count and measured median/p95/p99 frame times plus allocation/resource deltas. No frame-time/GC numbers or passes are fabricated here.

## Reproduction and local evidence files

Scoped output directory (temporary; Hermes may prune idle scratch files after 24 hours):
`C:/Users/pc/AppData/Local/hermes/cache/scratch/propertyleds-phase0/`.
Files are named by fully-qualified type: `S1API.Property.PropertyManager.cs`,
`S1API.Property.PropertyWrapper.cs`, `S1API.Property.BusinessManager.cs`,
`S1API.Property.BusinessWrapper.cs`, `S1API.Property.BaseProperty.cs`,
`S1API.PhoneApp.PhoneApp.cs`, `S1API.PhoneApp.ExitAction.cs`,
`S1API.PhoneApp.PhoneAppButtonHandler.cs`, `S1API.Lifecycle.GameLifecycle.cs`,
`S1API.Internal.Patches.HomeScreen_Start_Patch.cs`,
`Il2CppScheduleOne.Property.Property.cs`, `Il2CppScheduleOne.Property.Business.cs`,
`Il2CppScheduleOne.Property.PropertyContentsContainer.cs`,
`Il2CppScheduleOne.Persistence.LoadManager.cs`, `Il2CppScheduleOne.Persistence.SaveManager.cs`,
`S1MCPServer.Core.CommandRouter.cs`, `S1MCPServer.Handlers.PropertyCommandHandler.cs`,
and the inspected debug handlers. `file-evidence.json` preserves measured hashes/sizes/timestamps;
`S1API-types.txt` / `S1MCP-types.txt` preserve class-only listings.

```bash
# Windows Bash: scope runtime selection to this command, do not persist user-level settings.
DOTNET_ROOT='C:/Users/pc/.dotnet' 'C:/Users/pc/.dotnet/tools/ilspycmd.exe'   -t S1API.PhoneApp.PhoneApp   'C:/Program Files (x86)/Steam/steamapps/common/Schedule I/Mods/S1API.Il2Cpp.MelonLoader.dll'
# Substitute the exact target type and DLL for each entry above; never dump the whole assembly.
git -C 'C:/Users/pc/Schedule1-mod-only/ThirdParty/S1API' rev-parse HEAD
git -C 'C:/Users/pc/Schedule1-mod-only/ThirdParty/S1API' describe --tags --always
```

Managed assembly identity was read without loading game logic using
`System.Reflection.AssemblyName.GetAssemblyName(path)` and product/file versions using
`System.Diagnostics.FileVersionInfo.GetVersionInfo(path)` from Windows PowerShell.
An attempted `-t S1API.VersionInfo` failed (type not present); **ProductVersion metadata**, not an invented VersionInfo class, supplied the full commit suffix. ILSpy's update-available notice is not an API result and was ignored; tools/dependencies were not updated.

Final audit readback: all **11** installed file SHA-256 values above were recomputed and remained unchanged; S1API source `git status --short` remained empty; a second tasklist check still reported no matching Schedule I process. The document write/patch tools confirmed the on-disk target. No game or dependency state changes were required.

**Phase 0 outcome:** installed identities/hashes and relevant static API/lifecycle contracts are established; live geometry, visuals, lifetime correctness, persistence and performance remain open. No claim of completed PropertyLEDs runtime verification is made.
