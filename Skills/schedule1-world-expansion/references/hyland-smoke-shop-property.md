# Hyland Smoke Shop — Custom-Property & Map-Expansion Pattern Notes

> UNVERIFIED in game. Evidence scope: static decompile of a user-provided third-party assembly (`HylandSmokeShop.dll`),
> SHA-256 `0a76e5651c60c4e033c77828a445aaa80d8564a7af573059c4dd4781ac422896` (6,845,952 bytes),
> decompiled 2026-10-18 with `ilspycmd` 9.1.0.7988 (ICSharpCode.Decompiler 9.1.0.7988) to a scratch
> directory only. **None of this was run in game**, no DLL was loaded into the game, and no runtime
> behaviour is claimed. ILSpy emitted unresolved-IL annotations on many bodies, so a decompiled
> method may be incomplete. Identifiers, field names and coordinates below are read from that one
> binary; treat them as evidence about that binary, not as current game truth, and re-verify every
> API against the live game before adapting anything. No third-party code or assets are copied here.

This note generalises the observed structure into checks for the staged world-expansion pipeline
(`SKILL.md` → Procedure). It is a companion to `world-expansion-pipeline.md`.

## Assembly identity (observed)

- Single MelonLoader host: `HylandSmokeShop.Main : MelonMod` (`[assembly: MelonInfo(typeof(Main), "Hyland Smoke Shop", "0.3.0-rc.15", "Hyland Mods", null)]`, `[assembly: MelonGame("TVGS", "Schedule I")]`). <!-- skills-health: version-evidence (observed MelonInfo metadata from the decompiled third-party assembly) -->
- `[assembly: HarmonyDontPatchAll]` is declared, so MelonLoader does **not** auto-patch; the host calls
  `HarmonyInstance.PatchAll(typeof(Main).Assembly)` itself in `OnLateInitializeMelon` (plus an explicit
  `CustomerSafetyHooks.Install(...)`). A mod that declares `HarmonyDontPatchAll` must install its own
  patches or none apply.
- Build shape: `netcoreapp6.0`, `LangVersion 12.0`, `AllowUnsafeBlocks`, ~358 types, one root namespace
  `HylandSmokeShop`.
- Metadata is internally inconsistent: `MelonInfo` version `0.3.0-rc.15` vs `AssemblyInformationalVersion` <!-- skills-health: version-evidence (observed conflicting assembly metadata; not a dependency pin) -->
  `0.3.0-rc.7` — do not read a single "the version" from one field.

## Dependency surface (observed)

References: MelonLoader, Assembly-CSharp, UnityEngine.CoreModule, 0Harmony, Il2Cppmscorlib,
UnityEngine.PhysicsModule, Il2CppInterop.Runtime, Unity.TextMeshPro, **S1API**, Il2CppRootMotion,
UnityEngine.AIModule, Il2CppFishNet.Runtime, UnityEngine.AudioModule, UnityEngine.UI, UnityEngine.UIModule,
Il2CppScheduleOne.Core, UnityEngine.VehiclesModule, UnityEngine.ImageConversionModule, UnityEngine.AnimationModule,
Unity.InputSystem.

- Uses **S1API only** — namespaces `S1API.Entities`, `S1API.Entities.Schedule`, `S1API.Entities.Supplier`,
  `S1API.Entities.Appearances.*`, `S1API.Items`, `S1API.Lifecycle`, `S1API.Shops`. **No S1MAPI** (no
  `using S1MAPI`, no S1MAPI types).
- **No `AssetBundle` usage anywhere** and no `LoadFromMemory`/`LoadAsset`. Native game types come from
  `Il2CppScheduleOne.*` / `Il2CppFishNet.*`; world geometry is runtime-built primitives plus embedded
  binary meshes (see below).

## Lifecycle and ordering (observed, `Main.cs`)

- `GameLifecycle.OnPreLoad` → register items, dispose old runtime, `new ShopRuntime().Initialize(preLoad: true)`.
- `GameLifecycle.OnLoadComplete` → `Runtime.Initialize()`.
- `GameLifecycle.OnSaveStart` → `Runtime.Save()`.
- `GameLifecycle.OnPreSceneChange` → dispose runtime (nulls the field first).
- `OnUpdate` → `Tick(Time.deltaTime)`; on exception it logs and sets `Runtime.Ready = false`.
- `OnLateUpdate` → `LateTick()` only while `Ready`.
- `ShopRuntime.Initialize` builds only once: if `World.Root` already exists it runs the "finish" pass
  (`ClearNativeClutter`, `ApplyNativeFinishes`, `ConnectRoad`, `Physics.SyncTransforms()`, parking,
  placement init, net start); otherwise it does `Placement.Load()` → `World.Build()` →
  `Integration.MakeProperty(World, State)` → nav rebuild → `World.Refresh(State)`, and on exception
  disposes the half-built world and rethrows. This is the "one owned root, retry-safe phases" pattern.

## Custom property registration (observed, `Integration.MakeProperty`)

A runtime `Il2CppScheduleOne.Property.Property` is created from scratch, not cloned:

- New inactive `GameObject` parented to the mod root, `AddComponent<Property>()`, then set
  `propertyName` ("Hyland Smoke Shop"), `propertyCode` (`hyland_smoke_shop`), `Price` (5000),
  `EmployeeCapacity` (2), `OwnedByDefault=false`, `ContentCullingEnabled=false`.
- Ownership is written through the **compiler backing field** `_IsOwned_k__BackingField` (there is no
  public setter used) plus `IsOwnedVariable=""`.
- Every collection/event the vanilla loader would have populated is initialised by hand or left null:
  `onThisPropertyAcquired` (`UnityEvent`), `_LocalExtraFiles_k__BackingField` / `_LocalExtraFolders_k__BackingField`,
  `savedObjectPaths`, `savedEmployeePaths`, `_Employees_k__BackingField`, `BuildableItems`,
  `Configurables`, `Grids`, `Switches`, `Toggleables`.
- Points/bounds: `SpawnPoint`, `InteriorSpawnPoint`, `NPCSpawnPoint`, `EmployeeIdlePoints`
  (`Il2CppReferenceArray<Transform>`), `ObjectsToCull`, a `BoundingBox` object with a **trigger**
  `BoxCollider` (12×3×16) plus the `propertyBoundsColliders` array.
- Contents container: a `PropertyContentsContainer` is created and bound with `SetProperty(property)`,
  and its backing field `_Container_k__BackingField` is assigned.
- Worldspace UI: a new `GameObject` with a `RectTransform` becomes `_WorldspaceUIContainer_k__BackingField`.
- Loading docks are **cloned from an existing vanilla dock**, not built: the code scans `Property.Properties`
  for the `storageunit` property's first `LoadingDock`, instantiates it twice under staging objects, sets
  `ParentProperty`, `BakedGUID` (from a `DeliveryBayPlan.DockGuid(i)`), `Parking.BakedGUID`, clears
  occupants, and re-`SetGUID(new Guid(BakedGUID))` on dock and parking.
- A parallel `Il2CppScheduleOne.Property.Business` ("realty listing") object mirrors all property fields
  plus `LaunderCapacity` (10000) and `LaunderingOperations`; this is what the estate-agent dialogue lists.
- Registration is manual: `Property.Properties.Add(property)`, `Business.Businesses.Add(business)`, then
  `SmokeRealty.SyncOwnership(world, state)`.

**Take-away:** a runtime `Property` must have its backing fields, colliders, contents container,
spawn points, docks and the static registry lists populated explicitly; the vanilla serialised prefab
does this in the editor and does not run for a code-created property.

## Ownership lists and load hooks (observed, `SmokeRealty` + patches)

`SmokeRealty.SyncOwnership` sets `_IsOwned_k__BackingField` on both the `Property` and its `Business`,
then reconciles the **static ownership sets**: it removes the pair from `Property.Properties`,
`Property.OwnedProperties`, `Property.UnownedProperties`, `Business.OwnedBusinesses`,
`Business.UnownedBusinesses`, and adds them to `Owned*` when owned or `Unowned*` when not. Forgetting
these lists leaves the property in an inconsistent owned/unowned state even when the boolean is right.

Harmony hooks:

- `[HarmonyPatch(typeof(Property), "Load")]` prefix (`SmokePropertyLoadPatch`): if the instance is the
  shop, set `_IsOwned_k__BackingField = state.Owned` and `_hasBeenLoaded = true`, then **skip the
  original** (`return false`) so the vanilla loader cannot clobber the runtime property.
- `SmokeRealtyPatches`: same prefix on `Business.Load`; a prefix on `DialogueHandler.StartDialogue`
  (both overloads) swaps in a cloned `Conversation` that has an extra `hyland_smoke_shop` choice on the
  `BUSINESS_CHOICE` node; `DialogueHandler_EstateAgent.CheckChoice` postfix gates the choice on
  `onlineBalance >= 5000` and not-already-owned; `DialogueCallback` prefix intercepts
  `CONFIRM_BUY_BUSINESS` and routes to `ShopRuntime.PurchaseProperty()`.
- `ShopSaveBarrier`: prefixes `SaveManager.Save()` and `Save(string)`; when the network layer wants to
  defer, it sets `_IsSaving_k__BackingField = true` and skips the native save.

## Save scope (observed, `ShopSaveScope`)

Mod state is written to `<LoadedGameFolderPath>/Modded/HylandSmokeShop.json` (the vanilla `Modded`
subfolder convention). Reads/writes are guarded by comparing the normalised loaded-folder path so a
different world's progress cannot be loaded; writes go through a `.tmp` sibling then `File.Move(...,
overwrite: true)`. Caution: this is **not** the workspace's atomic `SafeStorage` (no `.bak`), and the
state file is separate from the vanilla property save.

## World build and map expansion (observed, `ShopWorld.Build`)

- One owned `GameObject` root ("HylandSmokeShop") at a fixed world position (approx `(-41, -3.98, 180.2)`),
  created inactive, fully built, then activated. All geometry is placed as local children.
- Geometry is generated from Unity primitives: `Geometry.Box` wraps `GameObject.CreatePrimitive(Cube)`,
  assigns a cached `Material`, and (for wall/floor/ceiling/divider names) recomputes UVs for tiling.
  `Geometry.Point` makes empty transform markers used as anchors. `Geometry.ConnectRoad(root, approach)`
  ties the plot to the road network; `Physics.SyncTransforms()` is called after moving colliders.
- Materials for the build are **native game materials looked up by name at runtime**, not shipped assets
  (see next section).
- "Map expansion" here is a single self-contained plot placed in the open world; it does not edit vanilla
  scene files or merge into an existing interior.

## Native material / model cloning and embedded meshes (observed, `Geometry`)

- `Geometry.NativeMaterial(params string[] names)` scans `Resources.FindObjectsOfTypeAll<Material>()`,
  caches by `name` (preferring instances that carry a `mainTexture`), and returns the first name hit.
  `NativeVisual` does the same over `Resources.FindObjectsOfTypeAll<Transform>()` to find a native model
  by name, then `Geometry.CloneVisual` copies its `MeshFilter` meshes (and bakes `SkinnedMeshRenderer`
  via `BakeMesh`), filtering out children whose names contain `lod1/2/3`, `buildbounds`, `bounding`,
  `footprint`, `ghost` or `collision`. This is the "borrow the game's own look" route and depends on
  current object/material **names**, so it breaks silently if the game renames them.
- Meshes the mod cannot borrow are shipped as **embedded binary resources** and rebuilt at runtime:
  `HylandSmokeShop.Assets.NativeRegister.bin` and `NativeWaterfront.bin` are read with a `BinaryReader`
  (`Assembly.GetManifestResourceStream`) using a custom format (part count; per part: name, vertex count,
  triangle count, vertices/normals/UVs, indices) into `new Mesh { vertices=..., normals=..., uv=...,
  triangles=... }`. PNG icons are likewise embedded and decoded to `Sprite`.
- Net effect: **no AssetBundle** is needed for procedural/interior geometry; the cost is a bespoke binary
  mesh format and reliance on runtime name lookups for native assets.

## Doors and interaction prompts (observed, `ShopDoor`, `ShopInteractions`)

- Doors are a **custom class** (`ShopDoor`), not vanilla `StaticDoor`/`DoorKnocker`. Each builds door-leaf
  geometry plus a `BoxCollider`, and `Tick` runs a proximity probe against workers, visitors and
  `Player.PlayerList` to auto-open (automatic doors) or apply a manual toggle; open state is a 0..1 float
  exposed as `NetworkOpen` for replication. Manual doors get an `Interactions` entry `("door:" + Name)`.
- `ShopInteractions.Install` turns each `(target, text, action)` entry into an `InteractableObject`:
  it ensures a collider (BoxCollider from `Geometry.LocalVisualBounds` when none), forces the object onto
  a layer present in `Singleton<InteractionManager>.Instance.Interaction_SearchMask`, sets message/range,
  and wires `onInteractStart` → `runtime.UI.Interact(action)`.
- `NativePromptPatches` prefixes `InteractableObject.Hovered` / `StartInteract` and routes to
  `ShopInteractions.Hover`/`Interact`, which gate on ownership, build-mode, open UI and a physics
  line-of-sight raycast (`ClearLine`).

## Storage containers and slot authority (observed, `NativeContainer`, `LocalStoragePatches`)

- `NativeContainer` clones a native `StorageEntity` template's visuals and rebuilds its storage graph by
  instantiating the template's `StorageGrid` children, re-fitting bounds/scale, creating a fresh
  `StorageEntity` (disabled) with its own `ItemSlot`s, and registering a `StorageVisualizer`. Slots it
  owns are tracked by native pointer in `LocalStoragePatches.Slots` (`HashSet<IntPtr>`).
- `LocalStoragePatches` prefixes `ItemSlot.SetStoredItem/SetQuantity/ChangeQuantity/SetPlayerFilter/
  RemoveLock/ClearStoredInstance/ApplyLock` for owned slots and gates mutation through
  `ShopReplication.CanEditSlot(ptr)` (host authority), with a postfix calling `Net.StockChanged()`.
  This is the same "pointer-keyed ownership + host gate" idea as the workspace's networked-world-state
  rules.

## IDs, names and phone/shop display evidence (observed)

- Property: code `hyland_smoke_shop`, name "Hyland Smoke Shop", price 5000; save schema int `1`.
- Products: 24 ids `hs_*` (`hs_vape, hs_bong, hs_pipe, hs_hookah, hs_lighter, hs_papers, hs_tray,
  hs_grinder, hs_torch, hs_tips, hs_cleaner, hs_incense, hs_swiss_bong, hs_lung_buster,
  hs_silicone_bong, hs_sackwoods, hs_square_grinder, hs_crack_pipe, hs_dab_rig, hs_banger,
  hs_terp_slurper, hs_silicone_puck, hs_dab_tool, hs_510_battery`) each built through the S1API
  `ItemCreator.CreateBuilder()...Build()` chain with `WithBasicInfo/WithStackLimit/WithIcon/WithPricing/
  WithLegalStatus/WithStoredItem/WithoutStationItem`.
- Fixtures: ids `hs_fixture_<key>` cloned from native `BuildableItemDefinition`s whose built prefabs are
  `StorageRack_Medium` / `HugeStorageCloset`.
- NPCs (S1API): `hyland_smoke_wholesaler` ("Casey","Supply"), `hyland_smoke_wholesaler_two`
  ("Casey","Dispatch"), `hyland_smoke_clerk` ("Riley","Brooks"). The two wholesalers set
  `IsSupplier => true` and configure `SupplierDataBuilder.WithPersistentId(...).WithDeliveryItem(productId)`
  for every catalogue id.
- **Phone/shop display names:** the mod does **not** create its own PhoneApp or `ShopInterface`. Its own
  storefront is an in-world computer UI (`PlayerCamera.AddActiveUIElement("HylandSmokeShopComputer")`).
  Supplier shops reach the phone through the S1API supplier NPCs; the mod then matches
  `NPC.Supplier.Shop.Name` to `ShopInterface.ShopName` (over `ShopInterface.AllShops`) to find the
  delivery van. So the phone-visible supplier shop name is whatever S1API/the supplier defaults produce
  for those NPCs, not a name the mod sets directly.
- Deliveries: `new DeliveryInstance(id, ShopName, "hyland_smoke_shop", dockIndex, StringIntPair[],
  EDeliveryStatus, 60)` sent via `NetworkSingleton<DeliveryManager>.Instance.SendDelivery(...)`, with
  `IsLoadingBayFree`/`SetDeliveryState` used to track bays and completion.

## Adaptation cautions (unverified / fragile)

- The `_IsOwned_k__BackingField`, `_hasBeenLoaded`, `_IsSaving_k__BackingField`, `_Container_k__BackingField`,
  `_WorldspaceUIContainer_k__BackingField`, `_ItemSlots_k__BackingField`, `_Employees_k__BackingField`,
  `_LocalExtraFiles_k__BackingField`, `_LocalExtraFolders_k__BackingField` names are **compiler-generated
  auto-property backing fields**. They are not a supported API and change if the game renames the
  properties; a mod relying on them must re-verify the names per game build.
- `Resources.FindObjectsOfTypeAll` name lookups for native materials/models and the `"storageunit"`
  loading-dock scan are string/keyed against the current game content.
- `ShopSaveScope` uses raw `File.WriteAllText`/`File.Move` rather than the workspace's `SafeStorage`;
  adopt the atomic + `.bak` pattern instead.
- ILSpy unresolved-IL annotations mean some bodies (notably `Geometry.Product`, `ShopUI` rendering and
  several `ShopRuntime` paths) are only partially reconstructed here. Nothing above is runtime-verified;
  confirm placement, ownership, doors, saves, host/client replication and cleanup in a controlled test.

## External-reference identifiers (not resolvable against this repo)

`HylandSmokeShop`, `Main`, `ShopRuntime`, `ShopWorld`, `Integration`, `SmokeRealty`, `SmokePropertyLoadPatch`,
`ShopSaveScope`, `ShopSaveBarrier`, `NativeContainer`, `LocalStoragePatches`, `NativePromptPatches`,
`Geometry.NativeMaterial`, `Geometry.NativeVisual`, `Geometry.CloneVisual`, `ShopDoor`, `NativeFurniture`.
