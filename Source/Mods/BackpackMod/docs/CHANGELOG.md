# Changelog


## 1.0.2 (2026-09-11)
- slot_-1-Guard: kein slot_-1-File mehr im Menue; Save bei unaufgeloestem Slot geskippt.
- ClothingItemUIPatch: Message-Fallback nur noch fuer Il2CppException (kein Swallow fremder Bugs).

## 1.0.1 (2026-09-10)
- Bone-Cache plus 2s-Throttle (Fix Per-Frame-Scan und Log-Spam).
- BackpackSlot-Reset bei Scene-Unload mit Pointer-Validierung; Material-Cache; Index-Loop ueber ItemDictionary; Empty-JSON-Guard.

## 1.0.0 (2026-08-24)
- First public release: 3D Wearable Backpacks with Spine Rig Alignment on the player avatar.
- ⌨ **B-Hotkey** toggles backpack storage from anywhere in the game.
- Realistic Harness system with chest sternum cross-strap, shoulder straps, and metal buckles via `BackpackVisualManager.cs`.
- Tier system: backpack_t1 / backpack_t2 / backpack_t3 with per-tier color palettes.
- `ObjLoader` runtime: zero-dependency Wavefront OBJ loader for custom `.obj` files under `UserData/BackpackMod/models/`.
- `backpacks.bundle` AssetBundle pipeline for prefab-based backpack meshes.
- Tier-based storage scaling (T1 small, T2 hiking, T3 tactical) via `BackpackStorageManager`.
- 360° mannequin inspection rotation in the Character menu (RMB-drag, Q/E keys).
- ClothingSlot 10 binding: backpack renders only when actively equipped.

## 0.1.0 (2026-08-21)
- Initiale Version — BackpackMod-Grundgerüst, Bundle-Loader, F8-Debug-Spawn.
