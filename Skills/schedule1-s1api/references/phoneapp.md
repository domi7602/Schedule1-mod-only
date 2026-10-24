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

Create UI under the framework-provided container and identify who owns each root,
canvas, texture, sprite, and subscription. Reuse the owned UI when the phone closes
and reopens. Close should release temporary input focus and hide relevant UI;
destruction should release the owning instance's resources and subscriptions.

The inherited failure case destroyed UI or removed essential update handlers on
phone close, then expected OnCreated to run on the next open. Do not rely on that
recreation assumption. Test actual create/open/close/destroy counts in the target.

If scene reload creates another app instance, detach the old instance explicitly.
A new instance unsubscribing its own method does not remove the old handler.
Keep world model state independent of a canvas that can be recreated.

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

Test repeated phone open/close, switching apps, typing and hotkeys, save reload,
menu return, and new app instance creation. Check selected-control focus, canvas
visibility, callback counts, and resource counts. Confirm icons and model state
survive the intended lifetime without carrying another world's data.

Read [lifecycle](lifecycle.md) for callbacks and
[cross-runtime compatibility](cross-compat.md) for delegate/native boundaries.
