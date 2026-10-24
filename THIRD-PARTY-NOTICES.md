# Third-Party Notices

This repository builds on, and in some places vendors, third-party components. The MIT license in [`LICENSE`](LICENSE) applies **only** to the original code and documentation in this repository (`Source/Mods/*`, `Source/Tests/*`, `Source/Archive/*`, `Tools/`, `Skills/`, `docs/`, `assets/`). Third-party code and assets remain under their respective licenses, reproduced in the locations given below.

## Frameworks and libraries

| Component | Version in repository | License | Location / notes |
|---|---|---|---|
| **S1API** by ifBars and contributors | 3.2.1 stable (submodule pin `a4cbdd9`) | MIT — see `ThirdParty/S1API/LICENSE` | Git submodule `ThirdParty/S1API/` (source: [ifBars/S1API](https://github.com/ifBars/S1API)). Runtime deployed as `Mods/S1API.Il2Cpp.MelonLoader.dll` + `Plugins/S1APILoader.MelonLoader.dll`. Runtime evidence is tracked in `AGENTS.md` and `docs/compatibility.md`. Not redistributed in release ZIPs — players install it separately. |
| **S1MAPI** | 2.0.1 (submodule pin `89b8d01`) | GPL-3.0 — see `ThirdParty/S1MAPI/LICENSE` | Git submodule `ThirdParty/S1MAPI/` (fork [domi7602/S1MAPI](https://github.com/domi7602/S1MAPI)). Runtime `UserLibs/S1MAPI_Il2Cpp.dll`, used by AutoPackagingStation and TaxiDriver. Not redistributed in release ZIPs. |
| **MelonLoader** | 0.7.3 | Apache-2.0 | External mod loader, not vendored — install from [melonwiki.xyz](https://melonwiki.xyz/). |
| **HarmonyX** (`0Harmony.dll`) | ships with MelonLoader 0.7.3 | MIT | Referenced from `<GameDir>\MelonLoader\net6\`, used for runtime patching. |
| **Il2CppInterop** | ships with MelonLoader 0.7.3 | LGPL-3.0 | Referenced from `<GameDir>\MelonLoader\net6\`. |
| **S1MCPServer** | vendored source | see `ThirdParty/S1MCPServer-master/README.md` | `ThirdParty/S1MCPServer-master/` — local live-debugging bridge (TCP :8765); not part of `S1Mods.sln` and not shipped. |
| **xunit**, **xunit.runner.visualstudio**, **Microsoft.NET.Test.Sdk** | 2.7.0 / 2.5.6 / 17.8.0 | Apache-2.0 / MIT | NuGet packages used only by the test projects in `Source/Tests/`. |

## Archived / reference-only components

Tracked reference material is kept under `ThirdParty/Archive/`; local-only assets are identified below. No archived project or DLL is built or deployed. The MIT Hash command-registration source shim is the explicit exception: it is compiled into BusinessIncome and PotScanner.

| Component | License | Notes |
|---|---|---|
| **ScheduleOne-Hash** 1.0.5 by DooDesch | MIT — see `ThirdParty/Archive/ScheduleOne-Hash/Hash_extracted/LICENSE.md` | Terminal-replacement archive, deprecated 2026-09-10. Its optional `Hash.Api/HashCommands.cs` source shim is still compiled into BusinessIncome and PotScanner; no archived DLL is deployed. |
| **PhoneScroll** 1.4 by V4LEXL | Closed source (NexusMods download) | Only a decompile summary (`ThirdParty/Archive/PhoneScroll/README.md`) is kept; no binaries or source are redistributed. Retired 2026-09-16. |
| **ScheduleIArcade** | Not stated in the archived files | Local-only reference assets (`ThirdParty/Archive/ScheduleIArcade/`), excluded from Git since 2026-10-07 and not redistributed. Absent from fresh checkouts. Treat as all-rights-reserved; do not reuse without clarifying the license. | <!-- doc-paths: historical-reference (local-only assets excluded from Git since 2026-10-07) -->

## Assets

| Asset | License | Notes |
|---|---|---|
| MoreDrugs `heartpill.glb` (ifBars) | CC BY 4.0 | The MoreDrugs mod was removed from the workspace on 2026-09-19 and its files are not in this repository; the attribution is retained for historical reference. |
| Phone-app icons (`assets/icon-sources/`, `Source/Mods/*/assets/*_icon.png`), `TaxiDriver/assets/taxi.glb`, `CustomSkateboard/assets/models/*` | MIT (workspace) | Created for this repository. |

## Game content

*Schedule I* is developed by TVGS. Game assemblies under `GameReferences/` are locally generated decompiles for research only, are never committed, and remain the property of their respective owners. No game assets are distributed with this repository. This is an unofficial fan project, not affiliated with TVGS.

---

*Last updated: 2026-10-24.* When adding a third-party dependency, update this file, `ThirdParty/README.md` and the deploy policy `ThirdParty/.deployallow` together.
