# WorldLook MCP automation

The owner requested this control surface on 2026-10-25 to remove manual hotkeys and
screenshot uploads from acceptance checks. It uses the existing local S1MCP bridge;
WorldLook does not reference S1MCP and does not add a second server.

## Runtime contract

The owned, persistent GameObject `WorldLook.Automation` exposes the registered component
`WorldLook.WorldLookAutomation`. Use exact object/type names with S1MCP `call_method`.
The installed S1MCP 1.0.5 argument decoder cannot reliably pass parameters to injected
methods, so automation uses parameterless methods:

- `GetStatus()` returns a JSON status with requested state, readiness and errors.
- `EnableLook()` and `DisableLook()` request an explicit session state. Repeating the
  same request does not toggle or reapply it. Startup configuration is not rewritten.
- `ReturnToMenu()` starts the game's native menu transition. WorldLook issues no save request.

Status remains readable in the menu. Mutations require a ready, loaded singleplayer world
and the game thread; loading, multiplayer, absent ownership and shutdown are refused.
Even a one-player multiplayer lobby is refused, as is missing or unreadable lobby state.
An accepted request is not proof that every light/material is currently applied. Read the
status again and inspect the affected Sun/material separately. A renderer still using the
native water shader can coexist with requested ON and no coordinator fault flag.

Ctrl+Shift+F7 and MCP use the same session coordinator. An explicit OFF→ON remains the recovery path;
repeating ON after a fault is not an implicit retry. The existing F9/debug export stays retired.

## Local driver

Use `Tools/invoke-worldlook-mcp.ps1` from the repository root with explicit PowerShell:

```pwsh
pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/invoke-worldlook-mcp.ps1 -Action Status -EvidencePath .scratch/worldlook-status.jsonl
```

Mutation actions are `On`, `Off`, `Menu`, `Load` and `Time`. Supply the owner-selected
`-SaveName` and exact native `-SavePath`; `Time` additionally needs `-Hhmm` (for example
`1800`). Use a fresh `-EvidencePath` per action. Keep `-GameProcessId` and
`-GameStartedUtc` from the initial status when testing state preservation across a reload.
`Load` resolves the current slot from fresh metadata and refuses mismatched or ambiguous
identity. It runs only from the menu, avoiding the bridge's direct-scene-load fallback.

The helper checks the loopback listener's process, correlates response IDs, skips only the
bridge's heartbeat notifications, acknowledges replies and retains errors. It never retries
a mutation automatically. Completion is checked separately through fresh state/readiness
and save identity. A timeout leaves the result uncertain; inspect before another write.

## Automated acceptance sequence and images

1. Record installed DLL/profile hashes, game process/start time and exact save identity.
   Preserve the native save with the game closed before the test run.
2. Request OFF, verify requested state and sample the Sun at a controlled 18:00. Capture
   the game window through the Computer Use API and save the returned image unchanged.
3. Request Menu; verify menu readiness and capture that screen.
4. Load the same save once, await complete load, and verify OFF without another state request.
   Set 18:00 again, record the Sun/weather and capture the view.
5. Request ON, verify readback and repeat the matched-time measurement/image. Inspect the
   ocean renderer's `sharedMaterial.shader.name` without invoking the cloning `material` getter.
6. Repeat the menu roundtrip with ON if its lifecycle result is not already established for
   this candidate. Leave ON as the final requested state and record all remaining defects.

Images belong in a local evidence folder, not in the production source tree. Pair every
image with UTC capture time, filename/hash, process, requested state, visible HUD time and
the associated MCP evidence file. Record capture start/completion separately from saving
the artifact. The clock continues while tools run; a nearby numerical
sample is not an exact-frame pixel comparison. Save the newly returned screenshot bytes,
not a previous capture retained by a closure. Validate hashes and inspect the actual view;
an obscuring foreground app is not a game screenshot.

The script deliberately does not synthesize Windows input or start a background screenshot
service. The agent orchestrates the MCP actions and supported Computer Use captures in the
active task. No manual screenshot upload is needed. Gameplay evidence and current limits
remain in [the acceptance record](in-game-acceptance.md).
