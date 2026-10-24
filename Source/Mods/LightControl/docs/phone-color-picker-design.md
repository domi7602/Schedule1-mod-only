# Compact phone color picker — approved design

## Current target: Graphite native port (approved 2026-10-22)

Dominik approved [the interactive HTML concept](ui-mockup.html), then requested Rainbow as the
tenth palette tile to complete a **5 x 2** grid. He subsequently approved the native port, focused
verification, version step to dev.19 and installation with backup/hash verification. The earlier
v3/V4 sections below document their respective historical designs.

The HTML app area is the visual reference at **1201 x 655**: graphite surfaces, `Lights` heading,
property/count summary, four aligned `Light / Color / Brightness / More` columns, lamp glyphs,
explicit labelled color controls, percentage meters and ellipsis actions. Real lamp/property and
storage/scan state replace browser fixtures. The surrounding preview toolbar is browser-only.

The color card is **600 x 456**, with 80-unit tiles, 14-unit gaps and 54-unit footer buttons.
Nine fixed colors retain their actual emitted RGB values; Rainbow is the bottom-right color-cycle
choice, staged and committed through the same **Cancel / Apply** path. Opening an existing Rainbow
lamp highlights its own tile; Original opens with no pending choice. All choices remain bound to
the captured session and exact lamp key, and stale/unavailable sources cannot apply.

Brightness retains the seven immediate steps in one row, with a 200-percent meter maximum.
The More card keeps Original, Rainbow, Reset, immediate property copy and confirmed all-property
copy. Native appearance, hit testing, font rendering and close/reopen acceptance require actual
game observation. Build and installation evidence belongs in
[graphite-ui-verification.md](graphite-ui-verification.md).

## Approval and boundary

Dominik accepted the compact v3 preview on 2026-10-21 and approved its implementation.
The approved implementation scope is LightControl UI, its runtime-used UI policy, tests and
release documentation, with compile-only verification. Installation requires a separate go.
No game launch, save modification, Shared/native-rendering/persistence change, new dependency,
commit or push is included.

This is an **Operate** surface: choose an existing lamp, then choose its color. It is not a
permanently visible general-purpose settings inspector.

## Visual target

The installed dev.16 log measured the actual horizontal PhoneApp container at **1201 x 655**.
That is a measurement of the previous UI, not native acceptance of this redesign.

The accepted browser concept uses:

- One full-width scrolling list, grouped by the existing property identity and display name.
- Stable lamp/tent labels, the current color and current intensity. Existing individual grow
  lamps remain supported; this is not a tents-only filter.
- A centered color-only dialog, approximately **600 x 456** at the measured reference size.
- Nine actual emitted-light colors, arranged as five square fields followed by four. Their
  RGB values are the existing `ColorMode.White` through `ColorMode.Pink` values, not UI tints.
- A strong white frame on the selected color, its English name below the grid, and clearly
  separated **Back** and **Select** actions.
- Smaller fields/spacing than the rejected larger draft, without reducing the accepted text
  and button sizes or uniformly scaling down the complete dialog to hide overflow.

The standalone browser preview passed 15 layout/interaction checks. Those checks are not
native uGUI execution, game-input verification or gameplay acceptance.

## Interaction contract

1. The app opens to the list. A scan, selection fallback or periodic refresh never opens a dialog.
2. Clicking a lamp's main/color area explicitly opens its color picker.
3. Picking a tile changes only the pending selection. The model and emitted light remain unchanged.
4. **Select** validates the captured session and exact source key, claims the pending choice once,
   and calls the existing `LightRuntime.SetColor` path. It does not set intensity or retry unknown
   outcomes. The resulting state and persistence status are reported independently.
5. **Back**, an outside click or app/phone closure discards the pending selection. An outside click
   is consumed by the overlay; it must not also select a background row.
6. Opening a source currently in Original or Rainbow mode never silently applies a fixed color.
7. Temporarily unavailable sources retain their settings but cannot be edited. Permanent removal,
   a new session or a changed selection invalidates the old dialog and pending confirmations.
8. A source color that changed after opening cannot be silently overwritten by an old pending
   choice. Revalidation must reject stale confirmation or require a visibly fresh selection.

The native Escape/Home behavior stays intact. This change does not introduce custom key bindings.

## Existing functions outside the color-only dialog

- The row's intensity control opens a separate seven-step selector: 10, 25, 50, 75, 100, 150,
  200 percent. A step applies immediately through the existing intensity path, as it did before
  this redesign; moving the controls does not authorize an extra brightness confirmation. The
  new pending/Back/Select contract belongs only to the approved color chooser.
- A separate More/actions view keeps Original, Rainbow, full lamp reset, property copy and global
  copy reachable without adding those controls to the accepted color dialog.
- Original preserves intensity. Full reset restores Original and 100 percent.
- Property/world copy still uses available exact targets, including the source when applicable.
  Global copy retains the explicit source/settings/count confirmation and frozen preview checks.
- Storage, scan/truncation, material-conflict and retained-cleanup warnings remain available even
  when the list is empty. Applied does not imply saved.

## Engineering constraints

Build and wire overlay controls once. Keep the overlay above the list and excluded from its
layout group, and block background raycasts. Keep the confirmed list-width/ContentSizeFitter fix,
exact EventHelper handler/event cleanup, open-direction visibility synchronization and row pooling.
Closing the phone hides/reset menus; it never destroys UI or unsubscribes the app update loop.
Recalculate responsive geometry after real rect changes, not from a stale first-open size. Narrow
or short screens need explicit reflow/scrolling, not clipped controls or arbitrarily tiny fonts.

Pending UI state belongs in the existing Unity-free `UiSelectionPolicy` and must be used by the
actual app. New behavioral tests require observed failing assertions before implementation;
source contracts are supplementary. There are no artificial tests claiming native visual proof.

## V4 professional list (accepted 2026-10-21)

After the installed dev.17 build was seen in game, the owner rejected the large circular color dots
as ugly and asked for a more professional screen. The accepted direction replaces presentation, not
behaviour:

- **Rows are a table, not cards.** One horizontal layout per lamp: a fixed **18 dp square** marker in
  the light's real color, the lamp name as the primary bold label, the color name below it in muted
  text, then right-aligned **Brightness** and **Actions** columns. All three columns share one heading
  row (`Light`, `Brightness`, `Actions`) that appears once, after the first property heading.
- **Selection is a quiet background tint**, not a `>` prefix. Property groups are plain text
  headings with a right-aligned lamp count; the raised header tile is gone.
- **Header:** `Light Control` on the left, the existing status line on the right, with two reserved
  lines so scan/save/warning suffixes cannot paint over the table. The permanent instruction
  paragraph is replaced by one compact action-feedback line.
- **Color chooser:** unchanged in substance - nine true-color squares in a 5 + 4 grid, one narrow
  white ring, the color name beneath, and a reserved footer with **Back** left and **Select** right.
  The center hint is intentionally empty in the normal case; the footer buttons now own their height,
  so the dialog cannot render as hint-only text.
- **Dialogs:** darker backdrop, consistent card padding, one title size and one button size; Close
  stays the single action for the brightness and More views.

Hard constraints that did not change: brightness steps apply immediately on click (no extra
confirmation), More keeps Original/Rainbow/reset/property copy/all-properties copy with the explicit
global confirmation, the dialog stays bound to the exact lamp identity plus session, and no new
runtime, schema, persistence or multiplayer behavior was added.

## Verification boundary

Before the redesign, the parent observed **233/233** baseline core/delegate/real-disk/source checks
and a forced compile-only Release with **0 warnings / 0 errors**. The pre-change working-tree files
were preserved without resetting or stashing existing work. Built and installed Shared matched.

Implementation, final tests, review and versioned artifact evidence are recorded separately at
completion. Native PhoneApp fit, hit testing, reopen behavior and rendering remain **In-Game-Verify
open** until the candidate is separately installed and exercised in the game. Earlier dev.15 UI
acceptance and dev.16 deployment/audit evidence remain historical for their respective builds.
