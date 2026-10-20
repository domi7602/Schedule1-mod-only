# Third-Party Dependencies

This directory contains dependencies that are not part of the workspace product code. Active mods must not take project references to another mod or to an arbitrary third-party source tree.

| Entry | Management | Purpose |
|---|---|---|
| `S1API/` | Git submodule, pinned commit | Primary Schedule I modding API |
| `S1MAPI/` | Git submodule, pinned commit | Optional world, mesh, and GLTF API |
| `S1MCPServer-master/` | Vendored workspace tool | Local MCP bridge; not part of `S1Mods.sln` |
| `Workspace/` | Vendored build props | Shared `ModVersion.props` for the S1API fork build |
| `Herer's Minimap` | External NexusMod #899 | `HererMiniMap.dll` (v2.0.1 by JackHerer1820, retired 2026-09-17, moved to `UserData\_RemovedMods\`) |
| `Archive/` | Archived releases & retired third-party mods | `ScheduleOne-Hash/` (deprecated; only `Hash.Api/HashCommands.cs` is still source-linked by BusinessIncome and PotScanner), `PhoneScroll/` (retired 2026-09-16, closed source by V4LEXL), `ScheduleIArcade/` (unlicensed arcade/Pac-Man assets — local-only, untracked since 2026-10-07). No archived project or Hash DLL is built/deployed; the optional command-registration shim is the explicit source-only exception |

Clone with dependencies:

```pwsh
git clone --recurse-submodules <repository-url>
```

For an existing clone:

```pwsh
git submodule update --init --recursive
```

`Tools/deploy-thirdparty.ps1` deploys only explicitly allowed runtime DLLs. Its explicit allowlist is `ThirdParty/.deployallow` (empty by default); adding a DLL here never makes it deployable automatically. The old `.deployignore` is retained as history and is no longer read. Missing allowlists and unmatched nonempty patterns fail closed. Existing differing DLLs are refused, not overwritten; handle third-party upgrades explicitly with a verified backup. Workspace-mod names are reserved. Emptying the allowlist does not uninstall files.
