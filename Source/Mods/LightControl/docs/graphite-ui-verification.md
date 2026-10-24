# Graphite UI port — verification record

## Approval and target

On 2026-10-22 Dominik approved the native implementation of
[ui-mockup.html](ui-mockup.html), including the subsequent Rainbow tenth-slot amendment, tests,
independent review, version step to dev.19 and installation with backups/hash verification.
Native list/picker screenshots and startup evidence were subsequently checked below.
**Verified, owner-confirmed 2026-10-24** (released as 0.1.0 with version metadata only): the owner
confirmed the work finished; the confirmation had been missing from the record and was supplied
retroactively. Exact font parity, interactions and lifecycle were not itemized, and the screenshots
do not establish them on their own. Browser rendering is design evidence, not native acceptance.

## Parent-observed baseline

- The deployment preflight confirmed the game was closed.
- 92 existing production/test/documentation files were preserved in `pre-task.zip` with a
  SHA-256 manifest under
  `%LOCALAPPDATA%/Temp/opencode/lightcontrol-graphite-dev19-20261022T052850Z/`.
  Existing working-tree changes were retained.
- Pure/adapter/source suite: **253/253 passed, 0 failed**.
- Forced compile-only Release with `S1NoDeploy=true`, `BuildProjectReferences=false` and
  `TreatWarningsAsErrors=true`: **0 warnings / 0 errors**.
- Five new behavioral Rainbow checks genuinely failed against the pre-port picker:
  **253/258 passed, 5 failed** (G01-G05: Rainbow rejected/not highlighted). These are state-machine
  failures, not compilation failures or assertions about rendered pixels.

## Browser reference measurements

The parent measured the real rendered mockup at its unscaled 1201 x 655 app area. Measurements
were recorded in `mockup-reference-geometry.json` in the task evidence directory above.

| Element | Reference units |
|---|---|
| Main app | 1201 x 655 |
| Outer padding | left/right 36; top 32; bottom 28 |
| Header | 84 high, followed by 24 spacing |
| Property / column headings | 52 / 36 high |
| Lamp row | 126 high |
| Fixed columns | Color 224; Brightness 182; More 64; 24 gaps |
| Color / brightness controls | 224 x 54 / 174 x 60 |
| Color card | 600 x 456 |
| Color tile / gap | 80 x 80 / 14; five columns and two rows |
| Dialog footer buttons | 54 high |
| Brightness card | 600 x 395 |
| Actions card | 530 x 554 |
| Global-copy card | 560 x 499 |

## Verified integration contracts

The parent inspected the installed `UnityEngine.TextRenderingModule.dll` with Mono.Cecil:
`Font.CreateDynamicFontFromOSFont(System.String, System.Int32)` is present, as is the overload
accepting `Il2CppStringArray`. Windows lists Segoe UI and Segoe UI Semibold. This proves metadata
and OS font availability, not the native renderer's eventual output.

Subsequent wrapper-body inspection found that the string overload throws `Method unstripping
failed`; the implemented view uses the **Il2CppStringArray overload**, whose wrapper invokes the
native dynamic-font constructor. The independent reviewer confirmed this against the installed
assembly. The fallback remains the borrowed built-in Arial font.

The later native startup below disproved the array overload's runtime usability: its downstream
font constructor is missing. Wrapper-body inspection did not establish that dependency's presence.

The installed S1API reports ProductVersion `3.2.1+a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b`,
matching the source checkout commit. Actual reference hashes for this build baseline:

| Reference | SHA-256 |
|---|---|
| S1API.Il2Cpp.MelonLoader.dll | `79d7af288b605f8fcd4d2582a59636670b6cafd6a8847bc31c21b2278a9f2bf3` |
| Shared.dll | `087b876818beaffcc95d834a9b6d353d08d8c1c8e932ef0fe747e03d3e060335` |
| UnityEngine.TextRenderingModule.dll | `a25039690fc52385bfe03926d9fb31defc237a360fb3fbdd9e54b8fae8d04950` |

## Candidate checks and installation

The parent reran the completed candidate after the review repair:

- **264/264 checks passed**, 0 failed: existing core/adapter checks, Rainbow staged/commit/cancel/
  session/availability checks, reference fit, palette order, brightness scale, listener identity/
  release and sticky-heading behavior. Source contracts are supplementary, not native execution.
- Forced Release, `S1NoDeploy=true`, `BuildProjectReferences=false`, `TreatWarningsAsErrors=true`:
  **0 warnings / 0 errors**.
- Scoped production/test `dotnet format --verify-no-changes --no-restore`: **exit 0**.
- Native rendering, actual font metrics, click routing, resource destruction timing and gameplay
  remain unverified.

### Review repair

The initial independent review found a P2: S1API's installed EventHelper keys registrations globally
by `Action` equality, so reusing `CloseModal` across several buttons silently omitted the later
listeners. Each widget now owns a `UiSelectionPolicy.ClickBinding` with a distinct callback target;
the exact forwarded Action/event pair is stored for removal. Release severs its target before native
unsubscription, so a retained late callback cannot act or retain the app through that target.

The parent introduced the binding seam with legacy identity behavior and observed **261/263**,
failing both distinct registration and late-callback release; unique forwarding then resolved both.
This is behavioral adapter-seam evidence, not a native click test. The old C149 palette source
expectation was reconciled to `PickerColorCount`/`PickerColorAt` after the explicitly approved change.
The native heading now sticks within the viewport with one layout spacer, and Rainbow uses the
mockup's eight discrete color bands.

Final bounded independent delta review: **PASS**, no remaining P1/P2 findings. Production hashes were
identical before/after that review:

| Reviewed source | SHA-256 |
|---|---|
| LightControlApp.cs | `13781eae2b88df210f6ed0c9d8ca7d1e673a9d2c2afaea4cc57b55b3e9b7c1e8` |
| UiSelectionPolicy.cs | `466cfce771575c36cc08f507ca4440433410255535ffb763c60826f8c7658952` |

### Installed candidate — 2026-10-22

The exact checked Release was retained under
`%LOCALAPPDATA%/Schedule1Tools/VerifiedBuilds/LightControl/0.1.0-dev.19-graphite/` together with its
source/test snapshot, approved HTML reference, pre-task archive, baseline/review regression evidence,
final suite/build logs and deployment receipt. The compiled MelonInfo version was read through
Mono.Cecil as **0.1.0-dev.19**, and the native `LightControl.LightControlApp` type was present.

The parent verified the frozen reviewed hashes and the 20 production source/project files. The
17 production inputs outside the two UI files and the versioned `Mod.cs` matched the pre-task
manifest byte-for-byte. Installed LightControl, Shared and S1API matched the pre-task installation
before copying; Shared/S1API were checked again after copying.

`Tools/deploy-mod.ps1` reported **Copied 3, Unchanged 0**. All three destination hashes and all
three previous dev.18 backups were independently read back. Preflight confirmed the game was
closed before and after installation.

| Installed target | SHA-256 |
|---|---|
| Mods/LightControl.dll | `d7af584c48869411515132d8bd4c332caa2fc0d3d16c94c98e5fbf5c00f97a6c` |
| UserData/LightControl/LightControl.pdb | `e6a957f56fc4f68c289f259a4019cb58b889ab6b4377a31887101fadd41646e8` |
| UserData/LightControl/mod.json | `abcc7ead744f7bcf1b8e9a340508b3f71e6f202935c38525398d71f67f08d973` |

- Game root: `C:/Program Files (x86)/Steam/steamapps/common/Schedule I`.
- Attempt: `20261022T061143772Z-a8cf2aaac422`.
- Receipt: `<Game>/UserData/.deploy/receipts/LightControl.json` and the attempt-specific
  `LightControl.20261022T061143772Z-a8cf2aaac422.json` in the same folder.
- Verified previous-file backup:
  `<Game>/UserData/.deploy/backups/20261022T061143815Z-LightControl-6d286eb0/`.

No game was launched or save edited by the agent during implementation/deployment. The owner's
subsequent game session is recorded below; installation alone is not native acceptance.

## Owner screenshots and native startup — 2026-10-22

Dominik supplied two 1920 x 1080 in-game screenshots in this conversation: the lamp list at
Wednesday 07:17 and the open color chooser at Wednesday 07:35. The images are conversation
attachments, not repository image files. This is visual evidence, not an owner statement that
every control works or that the design is finally accepted.

Observed against the approved reference:

- Three Motel Room grow tents render with readable primary names and secondary labels. Light,
  Color, Brightness and More headings align with their controls; the displayed rows and last
  action controls are not visibly clipped in this example.
- Each row shows Blue and 100 percent, with the brightness meter half-filled on the 200-percent
  scale. The lamp glyphs, graphite surfaces and row separators are visible.
- The centered chooser shows five columns and two rows, with Rainbow in the tenth/bottom-right
  position. Blue has a selection border/check and the selected-color line reads Blue / #0000FF.
- The title, source subtitle, explanatory line and both full Cancel/Apply buttons fit inside the
  card. The list is visibly dimmed behind it. This establishes footer rendering, not hit testing.

The captured `Latest.log` reports `LightControl v0.1.0-dev.19` at 08:19:47.607 and
`[ui-layout] root=1201x655, reference=1201x655, scale=1.000, initialTheme=1.200` at 08:20:41.048.
The installed DLL was read back again and matches the retained candidate SHA-256
`d7af584c48869411515132d8bd4c332caa2fc0d3d16c94c98e5fbf5c00f97a6c`.

### Runtime font limitation

At 08:20:22.698 the font setup logs `Could not create Segoe UI; using built-in Arial` with
`MissingMethodException: Method not found: 'Void UnityEngine.Font..ctor(Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppStringArray, Int32)'`.
The stack leads through `Font.CreateDynamicFontFromOSFont(Il2CppStringArray, Int32)` into
`LightControlApp.CreateFont`. Thus the reference font has **not** been achieved: the guarded Arial
fallback is active. The screenshots demonstrate a usable rendered fallback, not exact Segoe UI
parity. No font repair, source edit, rebuild or redeployment was performed during this review.

No `Phone UI update failed` or `Phone action failed` message occurs in this captured log. The
three native `[apply]` entries during startup show Blue/100-percent settings applied to the three
lamps, but do not prove a picker-button action. No modal geometry measurement appears in this
capture; card/footer fit above is judged from the screenshot only. Scan completion is not established.

Log snapshot:
`%LOCALAPPDATA%/Temp/opencode/lightcontrol-graphite-dev19-native-20261022T062339Z/Latest.log`,
49,076 bytes, SHA-256 `8849a54886356c10b83ed742bf36ce220f36d4833ec221437bfd27416d39b375`.
Its last captured entry is 08:20:41.048; findings do not cover later activity.

**Status:** native list and open-picker appearance checked for this shown resolution/data set;
Segoe UI remains unavailable. Cancel/Apply behavior, Rainbow staging/reopen, brightness actions,
copy confirmation, long-list sticky scrolling, app/phone reopen and resource teardown are
owner-accepted 2026-10-24 (not itemized); this screenshot review did not establish them.

## In-game acceptance

1. Confirm dev.19 loads, then compare the native lamp list with the approved HTML app area:
   font sizes, graphite surfaces, property counts, four columns and visible first/last controls.
2. Open the color chooser: five columns, two rows, Rainbow at bottom-right; footer visible and
   clickable. Cancel/outside/phone close discard; Apply affects only the selected lamp.
3. Reopen a Rainbow lamp and an Original lamp: Rainbow is selected in its own tile; Original
   does not silently become White or Rainbow. Brightness remains independent.
4. Check all seven brightness steps, immediate live percentage/meter updates, More actions,
   counted property copy and the separate source/settings/target-count global confirmation.
5. Scroll a long list and reopen the phone; compare after another app was opened. Inspect the
   one-shot layout measurements and check for clipped text, stale modals or UI update errors.
