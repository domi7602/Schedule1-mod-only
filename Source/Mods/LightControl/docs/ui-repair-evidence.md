# LightControl UI repair evidence (LC08-LC10)

## Scope

Owner-approved local source, test and documentation work only. No deployment, game launch, live save,
Shared, other mod, dependency or schema change. Production budget unchanged: the same 19 `.cs` files
and one `.csproj`; no production file was added. This work edits exactly:

- `src/LightControlApp.cs`
- `src/UiSelectionPolicy.cs`

New supporting files:

- `Source/Tests/LightControl.Tests/UiRepairChecks.cs` (registered by the parent in `Program.cs` via
  `tests = tests.Concat(UiRepairChecks.Cases).ToArray();`; the SDK test project already globs it, so
  no `.csproj` edit is needed)
- this document

Runtime and other test files are owned by parallel work and were not edited here.

## Behavior delivered

### LC08 selection

- `UiSelectionPolicy.IsPermanentlyRemoved` only clears a selected key when a **completed** scan no
  longer knows it (`!known && !scanInProgress`). A temporarily unloaded lamp stays known, so its key
  and its last stable label survive. An unknown key during a scan is retained.
- `ResolveSelection` keeps the key when the visible list is empty; `LightRuntime.IsKnownLamp` is the
  only permanent-removal source. A first open with no key still takes the first visible key.
- `ClassifyEditor`/`CanEditSelection` make a filtered, hidden, unloaded or unknown selection
  **unavailable but retained**. The editor stays visible and non-interactable; actions never run
  against it.

### LC09 editor modes

- Three modes only: `Original`, `Fixed`, `Rainbow`. `ModeOf` classifies all nine solid colors as
  `Fixed`; `FixedColor` keeps the current solid color, else the last remembered solid pick, else
  white, and never remembers `Original`/`Rainbow`.
- Active mode is painted with the blue accent; active brightness step uses the same marker.
- `SetEditorEnabled` is the single enable/disable path for every mode, swatch, brightness and action
  button (`Button.interactable`, plus one `CanvasGroup` alpha). With no valid target the whole editor
  is disabled instead of being hidden.
- Brightness remains seven direct step buttons, labelled `10%` through `200%`, not a slider.
  `Original` changes only the color, so the selected intensity is preserved; `Reset (original, 100%)`
  restores both.

### LC10 status and messages

- `StatusLine` keeps the actual `LightRuntime.StatusMessage` reason before a session exists. Once
  ready it composes scan state (`ScanInProgress`, `ScanTruncated`), the lamp count (including the
  empty-list reason), the unsupported-lamp count and the persistence notice as separate facts. The
  empty list therefore still shows both its reason and `Saved.`/`Save pending.` etc.
- The two-line status band and the one-line action band are permanently allocated. Expiry clears the
  action **text** instead of deactivating its object, so no message arrival moves the body.

### Copy wording

- Property copy label reads `Copy to <property> (<n> incl. source)`; the target count is the runtime
  preview count, which includes the source lamp.
- The global copy is now explicit: pressing `All properties (n incl. source)` opens a confirmation
  naming the source lamp, source property, source color, source brightness and exact target count,
  with `including the source`. `Copy all` executes the frozen preview; `Cancel` closes it. A changed
  preview still rejects and refreshes through `ApplyPreview`.
- Action results use `ActionMessage`: `Applied to n lamps.` only for a positive changed count,
  `No lamps changed.` for zero, `Already set.` for unchanged, and an explicit not-applied sentence
  for stale/rejected results.

### Visual/layout

- Left: grouped property list with stable `LampView.Label` rows. Right: sectioned editor
  (`COLOR`, `MODE`, `BRIGHTNESS`, `ACTIONS`) inside `EditorScroll`, so a short canvas scrolls instead
  of clipping the last action.
- Palette is exactly nine real swatches, White through Pink; Original and Rainbow are mode buttons.
- Compact pane below `UiSelectionPolicy.CompactWidth` (360): list or editor, with a Back action.
  Normal width keeps both side by side.
- A deferred layout breadcrumb logs the first nonzero measured size and compact flag once. A zero
  rect is retried, never spammed. `PinListWidth` runs on creation and on a measured size change, not
  every open frame. The dev.15 clipping fix and exact `EventHelper.RemoveListener(handler, unityEvent)`
  cleanup are preserved.

## RED evidence (policy before implementation)

Isolated harness:
`C:\Users\pc\AppData\Local\hermes\cache\scratch\lc-ui-repair-harness` compiles `Domain.cs`,
`Protocol.cs`, `UiSelectionPolicy.cs` and the new `UiRepairChecks.cs` only. Command:

```
LIGHTCONTROL_SRC=C:/Users/pc/Schedule1-mod-only/Source/Mods/LightControl/src \
  dotnet run --project Harness.csproj -c Release
```

First run, with the new members present as real-signature stubs (so every failure is behavioral, not
a missing-type compile error):

```
PASS UI01 Temporary unload keeps the selected key, including an empty list
FAIL UI02 Only a completed scan may treat an unknown key as permanent removal
FAIL UI03 A filtered or unloaded selection keeps state but is not editable
FAIL UI04 Original, fixed color and rainbow are the only three modes
FAIL UI05 Fixed mode remembers the last solid color and falls back safely
FAIL UI06 Copy labels and the global confirmation name the source and exact count
FAIL UI07 An unchanged action never claims that lamps changed
FAIL UI08 Status keeps the actual reason, scan state and persistence even when empty
FAIL UI09 Compact layout is a pure width decision
FAIL UI10 Phone app wires LC08 selection, LC09 editor state and LC10 status
FAIL UI11 Phone app shows three modes, nine real swatches and seven readable steps
FAIL UI12 Phone app keeps the clipping fix, stable labels, copy confirmation and cleanup
```

UI01 passed on the first run because `ResolveSelection` already retained an all-absent key; the new
gate is UI02. It remains as a regression check for the preservation contract.

## GREEN evidence

Same isolated harness after the policy implementation and app rewrite:

```
PASS UI01 ... PASS UI12
12/12 UI repair cases passed
```

Full repository test project (parent registration of `UiRepairChecks.Cases` was still pending at this
run, so the 12 UI cases above are not yet included in this total):

```
dotnet run --project Source/Tests/LightControl.Tests/LightControl.Tests.csproj -c Release \
  -p:S1NoDeploy=true -p:BuildProjectReferences=false

RESULT: 162/164 passed; 2 failed. Pure-model simulation only.
FAIL C125 Runtime uses the ownership helper and retains unproven releases
FAIL C134 Runtime decides material identity by native id only
```

Both remaining failures are runtime source-wiring checks in `LightRuntime.cs`, owned and being
edited concurrently by the parent/other child. They are unrelated to `LightControlApp.cs` or
`UiSelectionPolicy.cs`; the previously failing C99 app check passes after the documented rejection
fallback. Re-run the full suite after the runtime work settles and after registering
`UiRepairChecks.Cases`.

Production compile:

```
dotnet build Source/Mods/LightControl/src/LightControl.csproj -c Release --no-incremental \
  -p:S1NoDeploy=true -p:BuildProjectReferences=false

Build succeeded.
    0 Warning(s)
    0 Error(s)
```

## Evidence boundary

These checks prove pure policy behavior and source wiring only. There is deliberately no fake Unity
visual test: it would assert a stub, not the phone. Actual PhoneApp-container measurement,
compact-mode transition, text fit, palette touch targets and scrolling remain **OPEN** and require a
separately approved in-game session. The deferred breadcrumb records the real measured size once
when such a session is run; it is not a substitute for seeing the app.
