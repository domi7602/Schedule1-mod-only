# Third-Party Dependencies

This directory contains dependencies that are not part of the workspace product code. Active mods must not take project references to another mod or to an arbitrary third-party source tree.

| Dependency | Management | Purpose |
|---|---|---|
| `S1API/` | Git submodule, pinned commit | Primary Schedule I modding API |
| `S1MAPI/` | Git submodule, pinned commit | Optional world, mesh, and GLTF API |
| `PhoneScroll/` | Pinned release binary | Closed-source phone home-screen utility |
| `ScheduleOne-Hash/` | Reference-only release archive | Deprecated terminal compatibility shim |
| `S1MCPServer-master/` | Vendored workspace tool | Local MCP bridge; not part of `S1Mods.sln` |
| `ScheduleIArcade/` | Vendored reference assets | Not part of the product build |

Clone with dependencies:

```pwsh
git clone --recurse-submodules <repository-url>
```

For an existing clone:

```pwsh
git submodule update --init --recursive
```

`Tools/deploy-thirdparty.ps1` deploys only explicitly allowed runtime DLLs. Its allow/deny policy is `ThirdParty/.deployignore`; adding a DLL here must not make it deployable by default.
