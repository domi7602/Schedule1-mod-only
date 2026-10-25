# S1API PhoneApp integration map

> verified: §5 UIFactory signatures checked against `UIFactory.cs` ( source) 2026-10-05; lifecycle rules empirical 2026-08-20.
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 42 of 42 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

Source scope: official online documentation reviewed on 2026-10-21; target-assembly
and in-game validation remain required. Use the
[phone-app skill](../../schedule1-phoneapp/SKILL.md) for the implementation workflow.

## Base class contract

The official [PhoneApp reference](https://ifbars.github.io/S1API/api/S1API.PhoneApp.PhoneApp.html)
separates app identity (`AppName`), display text (`AppTitle`, `IconLabel`), icon
source, and orientation. It documents creation, UI construction, phone close,
and destruction as separate callbacks. Match accessibility and signature to the
target before overriding; do not copy guessed base members.

`IconSprite` takes precedence over file-based icon lookup in the online reference.
`IsOpen` reports this app's open state. Confirm exact behavior in the target;
do not substitute the entire phone's open flag for the app's state.

## Lifetime design

Use the canonical [PhoneApp lifetime contract](../../schedule1-phoneapp/references/lifecycle-and-canvas.md#resource-lifetime-table)
for container ownership, close versus destruction, instance-bound subscriptions
and save-bound models. Its historical blank-second-open case explains why ordinary
phone closure must not destroy the reopen mechanism.

## Icons and UI factories

Cache an owned generated icon for its intended lifetime. Do not allocate a new
Texture2D/Sprite on every property read. Destroy only resources the mod owns and
only after all users release them; borrowed framework assets are not yours to destroy.
Check file lookup location and missing-icon behavior in the deployed framework.

Inspect the actual UIFactory return types and component ownership before composing
controls. Do not add a second Image to a panel that already owns one, or treat a
Text component return as a GameObject. Use the project's supported event bridge
for IL2CPP delegates; do not assume a managed listener overload is compatible.

## Discovery and failure handling

Inspect the framework's discovery path, candidate requirements, and exception
handling. Verify the app appears once and an unrelated broken app does not silently
mask your own registration failure. Keep a useful unavailable/dependency status
when the world is not ready. Do not rebuild the full UI each frame to compensate
for missing lifecycle ownership.

## Verification

Run the [reopen/reload checks](../../schedule1-phoneapp/references/lifecycle-and-canvas.md#reopen-and-reload-verification),
including focus, canvas visibility and callback counts. Also confirm icon resource
counts and lifetime; test configured hotkeys without leaking input into gameplay.

Read [lifecycle](lifecycle.md) for callbacks and
[cross-runtime compatibility](cross-compat.md) for delegate/native boundaries.
