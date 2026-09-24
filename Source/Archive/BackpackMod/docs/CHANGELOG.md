# Changelog


## 1.2.3 (2026-09-13) — StorageMenu sort-button overlay fix (live-game report 2026-09-13)
- **Giant overlay fixed (HIGH):** `SortUIInjector` cloned the FIRST button under `CloseButtonContainer` and inherited its unchecked Rect — on container panels the "Sort" button became fullscreen, covered the menu contents and swallowed clicks. Template is now the smallest button (by area; own clones + destroyed excluded); size is enforced via `SortButtonLayout.ClampButtonSize` (110–220 x 36–60, fallback 150x44), position explicitly to the left of the close cluster in the same anchor space.
- New pure file `SortButtonLayout.cs` (Unity-free, testable) + regression suite `BackpackMod.Tests` (xUnit).
- Observability: injection log now names template and final geometry (`template WxH → WxH at x,y`).

## 1.2.2 (2026-09-12) — Bug-audit fixes round 4 (audit 2026-09-12)
- **HUD sort-button raycast protection:** After cloning the vanilla close button, we set `img.raycastTarget = false` on the image shell (button itself stays raycast-capable). Prevents the sort button at its anchor position (-24, 150) from swallowing drag/click events onto the underlying slot grid.

## 1.2.1 (2026-09-12) — Bug-audit fixes round 3 (audit 2026-09-12)
- ObjLoader: `LoadMeshFromObj` now checks the file size (50 MB cap) before `File.ReadAllLines` and aborts the parse loop above 250,000 vertices. Protects against frame spikes on accidentally huge or corrupt OBJ files in `UserData/BackpackMod/models/`.
- `BackpackDefinitions` listing-snapshot dump now behind `#if DEBUG` (was active per load and flooded the MelonLoader log in release).

## 1.2.0 (2026-09-12) — Bug-audit fixes (audit 2026-09-12)
- **Sort commit atomic (HIGH):** Before the first write, all instances are prepared via `ItemInstance.GetCopy(quantity)` (preserves quality/packaging/instance state completely). If a preparation fails, abort before EVERY write — no loss, no duplication.
- **Clipboard slot protection (MEDIUM):** Player-inventory sort now filters via reference comparison against `inv.clipboardSlot`/`inv.cashSlot`; hotbar 0–7 is the only sort zone. Vanilla UI validation of special slots is no longer bypassed.
- **Overflow downgrade quality+packaging (HIGH):** On tier downgrade, the live instance is returned via `GetCopy` (no more default reset). Overflow items that don't fit in the inventory are persisted in `backpack_overflow_slot_{n}.json` and automatically returned on the next backpack open.
- **Cross-save protect (MEDIUM):** `ResetCache` no longer saves before destruction (OnPreLoad cross-save assumption); the main unload-path chain `OnSceneWasUnloaded → ResetForSceneUnload → SaveStorage` is explicit.
- Docs: the item description text "Drop it to access its inventory" contradicts the real ToggleStorage flow (info note; no change).

## 1.1.0 (2026-09-12)
- **B1 Sort (QoL Spec v1.1.0, button-only per user decision):** one-click sorting for backpack (StorageEntity), player inventory (hotbar 0–8, cash slot excluded) and opened vanilla storage containers. No hotkeys — buttons only.
- "Sort" button in the StorageMenu (cloned from the vanilla close button, inheriting styling) sorts the opened container.
- "Sort Inventory" button in the GameplayMenu character screen (inventory open) sorts the player inventory.
- Sort algorithm: merge by ID+quality+packaging up to StackLimit (respects StackLimitMod), order category → quality (descending) → name; atomic plan-then-commit (no item loss on capacity shortage).
- New classes: `BackpackInventorySorter` (sort engine, snapshot semantics), `SortUIInjector` (both buttons, idempotent, GameplayMenu sync via OnOpen/OnClose/SetScreen postfixes).
- StorageMenu.Open/Close + GameplayMenu.OnOpen/OnClose/SetScreen Harmony patches via PatchGuard; scene unload reset for button caches.
- Fix-3.1 pattern on sort instances: quality + packaging are preserved when reshuffled (GetDefaultInstance + Quality/SetPackaging restore).

## 1.0.2 (2026-09-11)
- slot_-1 guard: no more slot_-1 file in the menu; save skipped when slot is unresolved.
- ClothingItemUIPatch: message fallback only for Il2CppException (no swallowing foreign bugs).

## 1.0.1 (2026-09-10)
- Bone cache plus 2s throttle (fix per-frame scan and log spam).
- BackpackSlot reset on scene unload with pointer validation; material cache; index loop over ItemDictionary; empty-JSON guard.

## 1.0.0 (2026-08-24)
- First public release: 3D wearable backpacks with spine rig alignment on the player avatar.
- ⌨ **B-Hotkey** toggles backpack storage from anywhere in the game.
- Realistic harness system with chest sternum cross-strap, shoulder straps, and metal buckles via `BackpackVisualManager.cs`.
- Tier system: backpack_t1 / backpack_t2 / backpack_t3 with per-tier color palettes.
- `ObjLoader` runtime: zero-dependency Wavefront OBJ loader for custom `.obj` files under `UserData/BackpackMod/models/`.
- `backpacks.bundle` AssetBundle pipeline for prefab-based backpack meshes.
- Tier-based storage scaling (T1 small, T2 hiking, T3 tactical) via `BackpackStorageManager`.
- 360° mannequin inspection rotation in the Character menu (RMB-drag, Q/E keys).
- ClothingSlot 10 binding: backpack renders only when actively equipped.

## 0.1.0 (2026-08-21)
- Initial version — BackpackMod skeleton, bundle loader, F8 debug spawn.