# Third-Party Notices

This workspace builds on and distributes third-party components. The MIT license in [`LICENSE`](LICENSE) applies **only** to original workspace code (`Source/Mods/*`, `Source/Tests/*`, `Tools/`, `Skills/`, docs). Third-party code and assets remain under their respective licenses.

## Frameworks & Libraries

| Component | Version | License | Source / Notes |
|---|---|---|---|
| **S1API** (ifBars fork) | 3.2.0 | GPL-3.0-or-later | `ThirdParty/S1API/` — pinned git submodule; fork build deployed as `Mods/S1API.Il2Cpp.MelonLoader.dll` + `Plugins/S1APILoader.dll` |
| **S1MAPI** | — | GPL-3.0 | `ThirdParty/S1MAPI/` — pinned git submodule (optional world/mesh/GLTF API) |
| **MelonLoader** | 0.7.3 | Apache-2.0 | External runtime loader — not vendored, install from [melonwiki.xyz](https://melonwiki.xyz/) |
| **HarmonyX** | (transitive) | MIT | Ships with MelonLoader — used for runtime patching |

## Archived / Reference-Only Components

These live under `ThirdParty/Archive/` for reference and are **not** part of the product build:

| Component | License | Notes |
|---|---|---|
| **ScheduleOne-Hash** (DooDesch) | see `ThirdParty/Archive/ScheduleOne-Hash/` | Deprecated terminal replacement shim, reference-only |
| **PhoneScroll** (V4LEXL) | Closed source | Retired 2026-09-16; archive kept for reference only |
| **ScheduleIArcade** | see archive folder | Reference assets only |

## Assets

| Asset | License | Notes |
|---|---|---|
| MoreDrugs `heartpill.glb` | CC BY 4.0 | MoreDrugs itself was removed 2026-09-19; attribution retained for historical reference |

## Game Content

*Schedule I* (TVGS) game assemblies under `GameReferences/` are locally generated decompiles for research only, never committed, and remain the property of their respective owners. No game assets are distributed with this repository.

---

*Last updated: 2026-09-19. When adding a new third-party dependency, update this file, `ThirdParty/README.md`, and the deploy policy `ThirdParty/.deployignore` together.*
