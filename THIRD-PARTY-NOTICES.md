# Third-Party Notices

This repository builds on, and in some places vendors, third-party components. The MIT license in [`LICENSE`](LICENSE) applies **only** to the original code and documentation in this repository (`Source/Mods/*`, `Source/Tests/*`, `Source/Archive/*`, `Tools/`, `Skills/`, `docs/`, `assets/`). Third-party code and assets remain under their respective licenses, reproduced in the locations given below.

## Frameworks and libraries

| Component | Version in repository | License | Location / notes |
|---|---|---|---|
| **S1API** by ifBars and contributors | 3.2.0 (+4 commits, submodule pin `1bbf3f0`) | MIT — see `ThirdParty/S1API/LICENSE` | Git submodule `ThirdParty/S1API/` (source: [ifBars/S1API](https://github.com/ifBars/S1API)). Runtime deployed as `Mods/S1API.Il2Cpp.MelonLoader.dll` + `Plugins/S1APILoader.dll`; the deployed runtime at the time of the last verification was the prebuilt 3.2.1-beta.5. Not redistributed in release ZIPs — players install it separately. |
| **S1MAPI** | 2.0.1 (submodule pin `89b8d01`) | GPL-3.0 — see `ThirdParty/S1MAPI/LICENSE` | Git submodule `ThirdParty/S1MAPI/` (fork [domi7602/S1MAPI](https://github.com/domi7602/S1MAPI)). Runtime `UserLibs/S1MAPI_Il2Cpp.dll`, used by AutoPackagingStation and TaxiDriver. Not redistributed in release ZIPs. |
| **MelonLoader** | 0.7.3 | Apache-2.0 | External mod loader, not vendored — install from [melonwiki.xyz](https://melonwiki.xyz/). |
| **HarmonyX** (`0Harmony.dll`) | ships with MelonLoader 0.7.3 | MIT | Referenced from `<GameDir>\MelonLoader\net6\`, used for runtime patching. |
| **Il2CppInterop** | ships with MelonLoader 0.7.3 | LGPL-3.0 | Referenced from `<GameDir>\MelonLoader\net6\`. |
| **S1MCPServer** | vendored source | see `ThirdParty/S1MCPServer-master/README.md` | `ThirdParty/S1MCPServer-master/` — local live-debugging bridge (TCP :8765); not part of `S1Mods.sln` and not shipped. |
| **xunit**, **xunit.runner.visualstudio**, **Microsoft.NET.Test.Sdk** | 2.7.0 / 2.5.6 / 17.8.0 | Apache-2.0 / MIT | NuGet packages used only by the test projects in `Source/Tests/`. |

## Archived / reference-only components

Kept under `ThirdParty/Archive/` for reference. None of them is part of the product build or of any release ZIP.

| Component | License | Notes |
|---|---|---|
| **ScheduleOne-Hash** 1.0.5 by DooDesch | MIT — see `ThirdParty/Archive/ScheduleOne-Hash/Hash_extracted/LICENSE.md` | Terminal-replacement shim; deprecated 2026-09-10, reference only. |
| **PhoneScroll** 1.4 by V4LEXL | Closed source (NexusMods download) | Only a decompile summary (`ThirdParty/Archive/PhoneScroll/README.md`) is kept; no binaries or source are redistributed. Retired 2026-09-16. |
| **ScheduleIArcade** | Not stated in the archived files | Reference assets only (`ThirdParty/Archive/ScheduleIArcade/`). Treat as all-rights-reserved; do not reuse without clarifying the license. |

## Assets

| Asset | License | Notes |
|---|---|---|
| MoreDrugs `heartpill.glb` (ifBars) | CC BY 4.0 | The MoreDrugs mod was removed from the workspace on 2026-09-19 and its files are not in this repository; the attribution is retained for historical reference. |
| Phone-app icons (`assets/icon-sources/`, `Source/Mods/*/assets/*_icon.png`), `TaxiDriver/assets/taxi.glb`, `CustomSkateboard/assets/models/*` | MIT (workspace) | Created for this repository. |

## Game content

*Schedule I* is developed by TVGS. Game assemblies under `GameReferences/` are locally generated decompiles for research only, are never committed, and remain the property of their respective owners. No game assets are distributed with this repository. This is an unofficial fan project, not affiliated with TVGS.

---

*Last updated: 2026-10-01.* When adding a third-party dependency, update this file, `ThirdParty/README.md` and the deploy policy `ThirdParty/.deployignore` together.
