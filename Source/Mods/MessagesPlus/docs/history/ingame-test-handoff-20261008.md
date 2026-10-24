# Historical snapshot — 2026-10-24

This is the 2026-10-08 handoff, including superseded visual requirements, then-open checks and the former user-local SDK installation. It is not current setup guidance or an open task list. Current acceptance is recorded in [compatibility](../../../../../docs/compatibility.md#verification-matrix); current setup is in [DEVELOPERS](../../../../../DEVELOPERS.md#requirements).

Original: `Notes/MessagesPlus-InGame-Test-Handoff.md`. Original SHA-256: `dcc959c980cce43fae5608d6d6deff99c8d11fa49892c8a1f6e90fe7edeb0eeb`.
The original document bytes are retained after the marker below.

<!-- preserved-document:start -->
# MessagesPlus In-Game Test Handoff

**Updated:** 2026-10-08
**Project:** `C:\Users\pc\Schedule1-mod-only`

## User goal

Keep the Messages app shell in dark mode, but preserve vanilla colors in the Dead Drop/order popup and counter-offer menu. Keep Seed and Pseudo artwork unchanged and recognizable.

## Changes made

- `Source/Mods/MessagesPlus/src/AppTheme.cs`
  - Added `PreserveVanillaSubtree(GameObject)`: restores cached original graphic colors, then excludes that UI subtree from later image/text sweeps.
  - Uses the verified `MSGConversation.responseContainer` reference for counter-offer UI.
  - Generic image and text sweeps skip registered vanilla roots.
  - Does not invoke `TryLiftDarkIcon`; Seed/Pseudo sprite or texture pixels are not edited by the active theme path.
- `Source/Mods/MessagesPlus/src/MessagesAppPatch.cs`
  - Added a Harmony postfix on `MSGConversation.CreateResponseUI(Response)` so a newly created counter-offer menu is restored immediately.
- `Source/Mods/MessagesPlus/src/DealWindowSelectorPatch.cs`
  - Uses the verified `DealWindowSelector.Container` root and preserves its vanilla appearance instead of applying dark theme colors.
- Updated MessagesPlus initialization text, README, changelog, and `AGENTS.md` to describe the intended behavior.

## Verification done

- Source-contract checks passed for popup restoration/exclusion and existing message-theme behavior.
- Focused `git diff --check` passed.
- Build passed with **.NET SDK 8.0.425** targeting the project's .NET 6 setup: **0 warnings, 0 errors**.
- Copied the built `MessagesPlus.dll` into the game's `Mods` folder. Source and deployed DLL SHA-256 matched: `80dd76363a5abbcf1dc36f2baf28e0c41218bb0f080be629f2212697023d1491`.
- The game was not running during the copy.

## Still to test in game

1. Open the Messages app and verify its shell remains dark.
2. Open a Dead Drop/order popup; check its vanilla colors and Seed/Pseudo visibility.
3. Open a counter-offer menu and verify its vanilla colors.
4. Check the MelonLoader log for Harmony warnings or IL2CPP exceptions.

No in-game visual test has been performed yet. Keep unrelated existing working-tree changes; do not reset or overwrite them.

## Toolchain reminder

`C:\Users\pc\.dotnet\dotnet.exe` provides SDK 8.0.425. The default `C:\Program Files\dotnet\dotnet.exe` only reports the .NET 6.0.36 runtime and no SDK. For builds, use the per-user executable explicitly.
