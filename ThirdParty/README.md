# Third-Party Dependencies

This directory contains dependencies that are not part of the workspace product code. Active mods must not take project references to another mod or to an arbitrary third-party source tree.

| Entry | Management | Purpose |
|---|---|---|
| `S1API/` | Git submodule, pinned commit | Primary Schedule I modding API |
| `S1MAPI/` | Git submodule, pinned commit | Optional world, mesh, and GLTF API |
| `S1MCPServer-master/` | Vendored workspace tool | Local MCP bridge; not part of `S1Mods.sln` |
| `Workspace/` | Vendored build props | Shared `ModVersion.props` for the S1API fork build |
| `Herer's Minimap/` | External NexusMod #899 | `HererMiniMap.dll` (v2.0.1 by JackHerer1820), deployed to `<Game>\Mods\` |
| `Archive/` | Archived releases & retired third-party mods | `ScheduleOne-Hash/` (deprecated terminal shim, reference-only), `PhoneScroll/` (retired 2026-09-16, closed source by V4LEXL), `ScheduleIArcade/` (reference assets) — none part of the product build |

Clone with dependencies:

```pwsh
git clone --recurse-submodules <repository-url>
```

For an existing clone:

```pwsh
git submodule update --init --recursive
```

`Tools/deploy-thirdparty.ps1` deploys only explicitly allowed runtime DLLs. Its allow/deny policy is `ThirdParty/.deployignore`; adding a DLL here must not make it deployable by default.
