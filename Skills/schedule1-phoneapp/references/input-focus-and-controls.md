# Input Focus, Keyboard Navigation, and Controller Support

> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 10 of 10 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

## Evidence and use

Use when typing moves the player, hotkeys leak into gameplay, a modal loses focus, or cloned controls
respond to a controller but not a mouse. `S1API.Input.Controls.IsTyping`, IL2CPP component registration,
and historical game input names are lookup leads requiring the installed API/interop contract.

## Establish ownership before changing a global gate

Prefer the game's or repository's supported input/focus ownership mechanism. Record which app,
control, or modal owns each acquisition and release. A control becoming unfocused must release only
its own ownership; it must not set a shared typing/movement gate false while another owner still
requires it.

If the only verified surface is a shared boolean, inspect all writers and integrate at the correct
coordination point. Reading the old value and restoring it later does not solve interleaved owners.
Do not invent a public lease API, and do not ship the old unconditional `Controls.IsTyping = false`
inside every `OnDisable` handler. Document any unresolved coordination limitation explicitly.

Typing ownership applies only to the active, interactable app and its currently focused supported
text field. Handle input-field selection, modal transfer, app switch, phone close, disable, destroyed
UI, and scene teardown. Make releases idempotent. A hidden but still-selected field must not retain
exclusive gameplay input.

## Runtime component and event wiring

For an IL2CPP-registered custom component, inspect the installed MelonLoader/interop registration
pattern and required constructors. The inherited pattern includes an `IntPtr` forwarding constructor;
verify the applicable base type and version rather than promising that one signature covers all
runtimes. Mono and IL2CPP event/collection bridges can differ.

Use the repository's verified listener adapter, historically `EventHelper` or `ButtonUtils`, and keep
an exact removable delegate/wrapper when the binding API requires one. Do not construct a guessed
`UnityAction` signature or repeatedly add another listener on each open.

## Navigation contract

Only the visible, focused owner processes UI shortcuts. Use the installed input system and binding
policy, including remapped controls and controller navigation.

| Intent | Priority and expected effect |
|---|---|
| Back/Escape | Dismiss top modal, then nested view, then app; consume at one layer |
| Focus search | Select an active, interactable search field in this app |
| Tab navigation | Move among the app's visible controls; preserve multiline editing policy |
| Save shortcut | Commit permitted app state through its persistence gate; avoid duplicate actions |
| Confirm/cancel | Act on the current modal/control only |

Historical M/T/J or Q/E bindings are debugging clues, not universally unused keys. Check actual
bindings before filtering or reusing them. Do not suppress every game input because one text field
needs keyboard focus.

## Mouse and controller parity

A cloned control can retain a gamepad-oriented component such as an input ramp without a working
pointer handler. Inspect the actual attached components, interfaces/adapters, event system, and
raycast path. A namespace or type name alone does not establish how the control is driven.

Prefer a supported pointer adapter. If a narrowly scoped polling fallback is necessary, require the
app/modal to be visible, hit-test the intended rect in the correct canvas camera, and acquire one
interaction owner on press. Cancel on release, pointer exit if appropriate, focus loss, app close,
modal replacement, and teardown. Guard against double execution when the original control also
handles the same input. Test drag capture, not only a single click.

## Text field presentation

Choose a single-line or multiline mode deliberately. For a notes editor, align both the text and its
placeholder to the intended top-left position through the supported uGUI/TMP component. Do not assume
that every factory default is centered or that every text field is the legacy `InputField` type.
Preserve editing shortcuts and caret behavior while suppressing gameplay actions.

## Verify

Type WASD, numbers, navigation keys and configured hotkeys; switch between fields and modals, close while typing/dragging,
lose application focus, reopen, and reload the scene. Verify controller and mouse parity and ensure
another game's/mod's input owner remains respected. Count listeners before and after repeated opens.

Read [Lifecycle and canvas](lifecycle-and-canvas.md) for lifetime boundaries and
[Components and uGUI](components-and-ugui.md) for modal hit testing and listener cleanup.
