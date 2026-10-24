# Compact color-picker verification

> Historical v3/V4 evidence (dev.17/dev.18 iteration). Current status: **0.1.0, owner-confirmed
> 2026-10-24** — see [graphite-ui-verification.md](graphite-ui-verification.md). "Open"
> statements below describe their original snapshot dates.

The subsequent dev.19 graphite native port is documented in
[graphite-ui-verification.md](graphite-ui-verification.md). This file retains v3/V4 evidence.

**Historical dev.18 deployment:** the exact retained V4 candidate was installed on 2026-10-22 after
owner approval, without rebuilding; all three targets and previous-file backups were hash-verified.
It replaces dev.17, installed on 2026-10-21. See
[phone-color-picker-deployment.md](phone-color-picker-deployment.md) for both deployment records.
The compile-only scope and artifact metadata below describe their earlier creation/verification
slices, not current install status. Native UI/gameplay acceptance remains open; no game was launched
by the assistant during deployment.

## Scope

Approved compact-v3 LightControl PhoneApp redesign, with local code/tests/documentation and
compile-only verification. No deployment, game launch, native-save mutation, Shared/native-light/
persistence change, dependency installation, commit or push is authorized in this slice.
The accepted behavior and visual contract are in [phone-color-picker-design.md](phone-color-picker-design.md).

## Baseline actually executed by the parent

- Existing working-tree source/tests/docs preserved in a task-specific archive before editing;
  no stash/reset or replacement of the owner's earlier work.
- SDK 8.0.425, test host runtime 6.0.36.
- `dotnet run --project Source/Tests/LightControl.Tests/LightControl.Tests.csproj -c Release -p:S1NoDeploy=true -p:BuildProjectReferences=false -p:CheckEolTargetFramework=false`:
  **233/233 passed, 0 failed**.
- Forced compile-only Release: **0 warnings / 0 errors**.
- Built/installed Shared matched. The installed LightControl dev.16 DLL initially hashed
  `a0697ef72100891f31149dbd341af7f136e02d02678c05f696203a561791f344`.
- A witness manifest captured 72 installed mod/library/bundle/LightControl-userdata files;
  the parent later re-read them and observed no changes during the first implementation/review.

## First implementation evidence (not the final release verdict)

The first integrated draft changed only `LightControlApp.cs`, `UiSelectionPolicy.cs`, the test
registration, reconciled UI source contracts and a new `ColorPickerChecks.cs`. The parent compared
all existing production files against the saved pre-task working tree: every runtime, native-light,
model, persistence and project file outside the two UI files was byte-identical. No production
file/assembly/dependency was added; the existing 19 C# files plus project remain the source budget.

Observed implementation evidence:

- The original stub/UI state reported **232/245 passed, 13 failed**. These failures include behavioral
  policy checks **and supplementary source contracts**; do not call all 13 native/behavioral failures.
- The policy-only intermediate state reported **242/245**, with three remaining UI source contracts.
- The parent independently reran the integrated suite: **245/245 passed, 0 failed**.
- Parent forced compile-only Release: **0 warnings / 0 errors**.
- Parent project-relative scoped production/test `dotnet format --verify-no-changes --no-restore --include ...`:
  **exit 0** for both projects.
- CP03's original closed-state assertions passed against the trivial stub. A later deliberate
  regression injection made CP03 fail on accepting a pick after cancellation, followed by a passing
  rerun after restoring the guard. This is a sensitivity check, **not original test-first evidence**.

The accepted browser preview separately passed 15 layout/interaction checks. Its real rendered
PNG/HTML and hashes were retained outside prunable scratch in the local verified-builds directory.
This does not establish native uGUI fit, raycast ordering, event cleanup or gameplay acceptance.

## Independent review of the first draft — FAIL

Read-only review froze the two production hashes and inspected the complete UI/policy delta,
actual S1API `UIFactory`, `PhoneApp`, `EventHelper`, unchanged runtime apply/copy contracts and
existing repository modal patterns. It found no security concerns, but four actionable issues:

1. **Outside dismissal:** a full-root raycast-target CardLayer sat above the sibling backdrop
   Button, so outside clicks could never reach the dismiss handler.
2. **Modal scroll width:** only the main list received the proven zero-width scroll-content fix.
   All three newly built modal bodies inherited the factory's default width overhang.
3. **Actions-session guard:** More actions bound a key but checked the session only during the
   next refresh. A new session containing the same lamp identity key could receive a stale action.
4. **Stale brightness wording:** after observing a changed intensity, a second Select without a
   fresh pick could commit the freshly read value. This was a no-op at runtime, not a stale overwrite,
   but contradicted the message requiring a new pick.

Compact-width/footer behavior was a documented additional risk: the half/40-percent widths did
not become near-full-width on a narrow host. The only native measurement so far remains 1201 x 655.
The review also noted small string allocations while a modal is open; these were not considered
material or a reason for an unrelated optimization.

The four issues were corrected before the version bump or release. The parent independently
reran the repaired suite: **251/251 passed, 0 failed**. A separate bounded delta review is required;
the earlier draft never received a PASS.

### Repair evidence

- Outside dismissal: the full-root CardLayer no longer answers raycasts. Each card consumes its
  own interior clicks, while outside hits reach the sibling backdrop Button. The new source
  contract checks this exact hierarchy; it does not execute native uGUI hit testing.
- All scroll bodies: one width-pinning helper is called for both factory sites, including the
  shared constructor for the color, intensity and More cards. It zeroes the horizontal sizeDelta
  and retains the established anchors/pivot and unconstrained horizontal fitter.
- Action-time session gate: color/intensity commits carry the captured session and exact key;
  More actions use the captured modal session. Every action passes the runtime-used pure gate
  against current readiness/session/known/visible state immediately before the existing runtime
  call. Hidden More handlers cannot act while a different modal is open.
- The first repair made the draft's staged-brightness confirmation internally consistent. The
  delta review then correctly rejected the confirmation itself as an unapproved behavior change;
  see the immediate-brightness correction below. This intermediate repair is not the final UX.
- Bounded compact sizing: card width has a tested minimum/available-width clamp; footer buttons
  adapt within a bounded range and the optional hint hides if needed. The normal 1201 x 655
  composition is retained. Tiny containers below roughly 230 device pixels and short native
  layouts remain unverified; this is **not a universal-fit guarantee**.

The first repair regression run reported **244/251**, with seven assertion failures including
supplementary source contracts and the real action/session, stale-intensity and width decisions.
Two later corrected test expectations were verified by temporarily restoring the old behavior:
**249/251**, failing stale brightness and compact width, followed by **251/251** when the repairs
were restored. That later deliberate restoration is a sensitivity check, not a new claim that
the original implementation was test-first. The parent read the actual failing/passing outputs.

Source/policy tests and code review do not establish rendered pixels, native click routing or
actual save/scene transition timing. The in-game checklist below remains separate.

## Delta review — structural fixes accepted, brightness scope rejected

The fresh reviewer verified the exact repaired source hashes and traced the real construction:
outside dismissal, all scroll-content widths and action-time session/key binding were resolved.
No security concern, new clipping or disruption of the frozen copy previews was found.

The verdict was nevertheless **FAIL**: the draft had moved the seven existing immediate brightness
steps behind an additional Select confirmation. Only the color chooser was approved for staged
selection. The parent confirmed the actual saved pre-task working tree called SetIntensity directly
from each step button; the correction must preserve that behavior rather than adapt the requirement
to the new tests. The obsolete staged-brightness tests are not acceptance evidence for this contract.

The final bounded correction applies an intensity step on click through the same captured
session/key gate, keeps the live current-step highlight and a Close-only footer, and leaves the
color chooser's pending/Back/Select path unchanged. The menu remains open for immediate step previews;
obsolete staged-intensity state/confirmation was removed, rather than left as a second user path.

The pre-fix run genuinely failed three updated **source contracts**, with no compilation failure:
**248/251** (staged brightness still wired, no immediate handler and no immediate session-gate path).
The parent read that original output and independently reran the returned source: **251/251**, then
scoped production/test format checks and `git diff --check`, all **exit 0**. These checks prove source
wiring and pure decisions, not an executed native click.

## Final independent delta review — PASS

A fresh read-only reviewer verified the exact final hashes below and traced button -> immediate
handler -> current session/key gate -> SetIntensity, live highlight refresh, null-safe single-button
footer resizing, unchanged paired color/actions footers and unchanged staged-color/copy paths.
The verdict was **PASS**, with no logic errors or security concerns. Cosmetic/vestigial-condition
suggestions were non-blocking; no unrelated cleanup was added to the release.

| Final reviewed source | SHA-256 |
|---|---|
| LightControlApp.cs | `876a895a524f8d9ba21cb5394c28ae7b267e0c268bf561e4f74197bad85da6c0` |
| UiSelectionPolicy.cs | `fb2bde33206c6e4d120fb640b0fb2a87017233853002a1baa9ac95fd280c3239` |
| ColorPickerChecks.cs | `e9ee6423bfe44957bac6edbe9b182797eb4c1dd9d9b8ca675e14cecca1285a71` |

## Final compiled candidate — 0.1.0-dev.17, not installed

The parent bumped once using the repository helper after the final review. The helper changed the
MelonInfo version/metadata/inventory rows; its stale installed-state prose and changelog placeholder
were reconciled separately. The three reviewed files remained hash-identical after the bump.

- Final versioned suite: **251/251**, 0 failed.
- Final `dotnet build` with `--no-incremental -p:S1NoDeploy=true -p:BuildProjectReferences=false
  -p:TreatWarningsAsErrors=true`: **0 warnings / 0 errors**.
- The compiled DLL was read using the existing Mono.Cecil metadata reader, without executing the mod:
  exactly one MelonInfo, name `LightControl`, version **0.1.0-dev.17**.
- Scoped format verification and diff whitespace check: **exit 0**.
- Repository version sync: **21 mods, 0 drift**. Doc-path check: **exit 0**, all referenced paths exist.
  The compatibility row retains the header's seven columns.
- The resolved **184 reference files** were pinned by path/SHA-256 for later source/dependency checks;
  these hashes identify build inputs, not a claim of native-game acceptance.

| Retained artifact | SHA-256 |
|---|---|
| LightControl.dll | `f0d41f141ffb74da7f2de9cab028a57aafe5f547e77d8117a8061a8df82a4b21` |
| LightControl.pdb | `742e644009df7c4366ec45ecb2a9e15650aa7b9073406db0a983ab6951626e28` |
| mod.json | `812674619d6131ecddba65c9cb2eb69dde92a6597b2b55ff168a511c41c1771a` |

The frozen artifact, source snapshot, dependency hashes and original evidence are retained outside
prunable scratch under `%LOCALAPPDATA%/Schedule1Tools/VerifiedBuilds/LightControl/0.1.0-dev.17-compact-picker/`.
Any later deployment must use that exact retained candidate or rebuild/reverify the same source and
dependency stand; a mutable bin folder or a matching version string alone is insufficient.

## V4 professional-list pass — 0.1.0-dev.18 compile-only candidate (not installed)

The owner's two in-game screenshots of the installed dev.17 build showed the accepted dialog and
list structure but also two defects: large circular color markers that read as extra buttons, and a
color dialog whose hint text rendered while **Back** and **Select** did not. The owner approved the
V4 direction ("plan ist grün") after a browser preview; implementation stayed inside LightControl UI,
tests and release documentation.

### Diagnosed cause of the missing footer

Source inspection showed `UIFactory.ButtonWithLabel` creates a `RectTransform` with a `sizeDelta`
but **no `LayoutElement`**, while the footer's `HorizontalLayoutGroup` runs with
`childControlHeight = true` and `childForceExpandHeight = false`. The layout group therefore sized
both buttons from a missing preferred/minimum height, collapsing them, while the sibling hint text
kept its own reserved height - exactly the hint-only footer in the screenshot. The fix gives every
footer button (`Back`, `Select`, single-button `Close`) its own preferred and minimum height; the
existing width-only footer resizing is unchanged. This is a layout-contract fix, not a rendering
workaround, and the native result still needs a game session.

### Test-first evidence

- Two new source contracts were added before the change: rows must use one horizontal layout with an
  explicit 18 dp square marker, and every footer button must reserve preferred and minimum height.
  Against the pre-V4 source they failed with **251/253** (CP19 and CP20), then passed after the
  change: **253/253**.
- After the review finding below, a deliberate mutant source (a second `CreateColumnHeader` call
  injected in an external copy, selected through `LIGHTCONTROL_SRC`) reproduced the defect as
  **252/253** with `FAIL CP19 ... Expected [1]; actual [2]`, then the real tree passed again at
  **253/253**. That is a sensitivity check, not a second test-first claim.
- Parent reran on the final sources: **253/253 passed, 0 failed**; forced `dotnet build`
  (`--no-incremental -p:S1NoDeploy=true -p:BuildProjectReferences=false
  -p:TreatWarningsAsErrors=true`): **0 warnings / 0 errors**; scoped production/test
  `dotnet format --verify-no-changes` and `git diff --check`: **exit 0**.

### Independent review of the frozen V4 delta — one blocking defect, fixed

The read-only reviewer traced the exact hashes and confirmed the footer height correction, the
row/column geometry at the measured 1201 x 655 root, the staged-color contract, the unchanged
runtime calls and the safety of the one-shot geometry probe (no per-frame work, no layout mutation).
It returned **failed** for one blocking defect: a second `CreateColumnHeader(_root.transform)` call
created an untracked heading row outside the scroll list that would never be hidden, so the shipped
UI would show two "Light / Brightness / Actions" rows - and one even above the empty state.

Corrections applied after that review:

- the duplicate call site was removed; `CreateColumnHeader` now runs exactly once, inside the scroll
  content, and a test asserts one build-site and its ordering after the first property heading;
- heading and rows share one `LightColumnMinWidth` constant, so the flexible column cannot drift
  between the labels and the buttons;
- the status band reserves two lines, so a multi-segment warning cannot paint over the table heading.

The reviewer's remaining notes were handled or recorded: the post-dispatch sibling-ordering change
was re-covered by a fresh suite/build run on the final hash, and native rendering remains unproven by
source tests by design.

| Final reviewed source | SHA-256 |
|---|---|
| LightControlApp.cs | `8f221a25eb5596f55f11003ed926b01f0bfb822ac9f65dc17fc78e2b1989113d` |
| UiSelectionPolicy.cs | `fb2bde33206c6e4d120fb640b0fb2a87017233853002a1baa9ac95fd280c3239` (unchanged) |
| ColorPickerChecks.cs | `6fa636c56baba38edde2c495e53036c94805f03a0d623d7fbc386b0a9e63f7fa` |

### Candidate state at build completion (historical)

`0.1.0-dev.18` is a **compile-only candidate**. The installed game copy is still `0.1.0-dev.17`
(DLL `f0d41f14…`, PDB `742e6440…`, metadata `81267461…`), and Shared, the 76 unrelated
mod/library/plugin files, the four LightControl sidecars and the 125 native save files were not
touched in this slice. No game launch, deployment, save mutation or commit was performed.

**Subsequent installation, 2026-10-22:** dev.18 is now installed from that exact retained artifact.
Source/project and snapshot hashes matched the build manifest; destination and previous-file backup
hashes were independently read back. See the deployment record above. This does not establish native
footer visibility, hit testing or rendering acceptance.

## Final acceptance boundary

Implementation, review and compile-only acceptance are closed for the retained dev.18 candidate.
Installation was outside that slice and was separately approved and completed on 2026-10-22;
see the deployment update.
Native appearance and hit testing, outside
click consumption, the first/last modal controls, repeated close/reopen, save-slot transitions and
actual light output remain **In-Game-Verify open** until tested in game.

## Singleplayer in-game checklist (dev.18 installed; gameplay still open)

1. Open LightControl in the backed-up test save: landscape full-width list, property headers and
   stable lamp labels; no dialog auto-opens. Check the empty/unavailable states and warning band.
2. Open a color picker. Check all nine square fields, White and Pink at the masked edges, the
   unmistakable white selection frame, readable color name and reachable Back/Select footer.
3. Pick another color: nothing applies yet. Back and a dimmed-area click both discard it; outside
   clicks do not select a background row. A click inside the card does not close it.
4. Select a color: only the exact lamp changes, intensity stays unchanged; opening a second lamp
   starts from that lamp's own live value. Original/Rainbow sources do not silently become White.
5. Open intensity and click each relevant step: it applies immediately, without Select; all seven
   documented percentages remain reachable. Close/reopen keeps the current live highlight.
6. Under More, check Original preserves intensity, Reset restores Original and 100 percent, and
   property/all-property copy names/counts/explicit global confirmation match the intended targets.
7. Close the phone and reopen repeatedly; then test load/slot return without a stale dialog/action.
   Temporarily missing lamps retain settings but cannot be edited. Applied and saved notices remain
   distinct; do not infer save success from the visible color change.
8. Check panel material and actual emitted-light color/intensity, normal transitions/quit cleanup,
   and phone fit at the real container. This checklist does not require multiplayer acceptance.
