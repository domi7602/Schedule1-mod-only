# Changelog

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
