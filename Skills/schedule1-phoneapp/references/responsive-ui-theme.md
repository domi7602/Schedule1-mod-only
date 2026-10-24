# Responsive Phone Layout and Theme Reconciliation

> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 18 of 18 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

## Evidence and use

Use for unreadable text, clipped layouts, mockup matching, phone icon sizing, and dark-theme flashing.
Historical workspace helpers lived in `Source/Mods/Shared/src/UITheme.cs`, with wrappers in NotesApp,
PotScanner, CalculatorApp, and PocketShop. Those files are external source anchors, not bundled APIs.
Inspect the supplied checkout and helper signatures before depending on them.

## Measure before scaling

Read the actual container rect after layout has valid dimensions, plus its canvas/scaler and rotation.
Use the relevant local coordinate space; screenshot pixels and local UI units are not interchangeable.
A 90-degree historical phone orientation is a clue to inspect, not permission to swap width/height
unconditionally. Recompute or invalidate metrics when the container or orientation changes.

The existing shared theme helper is preferable to another copied implementation when it is present
and suitable. Check whether its metrics are global mutable state. If two apps can initialize different
curves in the same process, one must not silently change the other's sizing; use the repository's
supported per-view metrics or take a stable app-specific snapshot rather than inventing a conflicting
second global theme class.

Historical tuning examples used a reference height of 750 with a 0.85–2.0 clamp for text-heavy apps,
and 900 with 0.75–1.20 for dense dashboards. These are design presets from that workspace, not Unity
requirements or proven defaults for every phone. Confirm readability and density in the actual UI.

## Layout ownership

Choose which component controls each dimension. Avoid two layout systems fighting over the same
axis. Scale fonts, padding, spacing, and explicit minimum sizes consistently with the chosen theme.

| Element | Practical contract |
|---|---|
| Full-screen app root | Stretch to the app container without resizing shared phone objects |
| Vertical list content | Top pivot; content height follows preferred row height |
| Scroll-list width | Content/row width fills the viewport; check child-control and expansion settings |
| Fixed tag plus flexible title | Fixed tag preferred width; flexible title receives remaining width |
| Inline picker | Built once, toggled; layout participation matches visible state |
| Modal | Bounded card inside a full-screen owned overlay; usable at the smallest supported rect |

The historical collapsed-row fix set a vertical layout's `childControlWidth` and
`childForceExpandWidth` true. For a horizontal row with a fixed tag and flexible title, it enabled
child width control but disabled forced equal expansion. Re-derive these settings from the current
child `LayoutElement` contracts rather than applying them to every hierarchy.

Trigger a targeted layout rebuild after structural changes when necessary. Do not force all canvases
every frame. A hidden inline picker should not retain an unintended fixed blank height; an expanded
picker must expose its actual preferred height instead of a remembered 100-unit constant.

## Match a reference image

Measure proportions and hierarchy: content margins, header/list share, font hierarchy, row spacing,
button sizes, and alignment. Translate them into the real container coordinate system and check the
chosen font's metrics. Browser/headless previews can help compare proportions but do not establish
Unity text wrapping, masks, clipping, or input behavior. Use an in-game screenshot for acceptance.

Do not rely on a particular local browser path, handwritten PNG decoding, sandbox-disabling switches,
or fixed font-height ratios as a UI method. Use available image tools for measurement and document
any remaining approximation.

### Font parity with a browser concept

The local S1API `UIFactory.Text` and `ButtonWithLabel` implementations assign the built-in
`Arial.ttf`; a browser's Segoe UI stack therefore does not automatically match native text metrics.
For a close visual port, verify the actual native font API and font availability, then compare
baseline/weight/line height in game rather than compensating with arbitrary smaller text.

On 2026-10-22 the LightControl graphite-port preflight read the installed
`UnityEngine.TextRenderingModule.dll` metadata and confirmed
`Font.CreateDynamicFontFromOSFont(System.String, System.Int32)` plus its `Il2CppStringArray`
overload; Windows listed Segoe UI and Segoe UI Semibold. This is metadata/OS evidence only,
not a rendered-font verification. Subsequent wrapper-body inspection found the string overload's
`Method unstripping failed` stub; the port therefore used the `Il2CppStringArray` overload. The
owner's subsequent native session on the same date showed that this overload also fails: its body
calls a missing `Font(Il2CppStringArray, Int32)` constructor (`MissingMethodException`), and the
guarded Arial fallback is active. Neither a callable-looking wrapper body nor OS font enumeration
proved a usable native font-creation path. Preserve the fallback and verify the complete call chain
in game before claiming Segoe UI parity. A created font is owned runtime state: track its consumers and
release it with the owning view; a borrowed built-in fallback must not be destroyed.

## Theme vanilla UI narrowly

Resolve targets by their verified hierarchy and role, not by a broad global name search. Cache targets
for the current canvas generation and invalidate destroyed/rebuilt objects. Treat text and images
separately; tint multiplies sprite artwork, so a broad dark tint can make icons illegible. Preserve
original tint/alpha or use white when the asset contract calls for untinted artwork.

When vanilla update code overwrites a colour, inspect its owner and update order. A scoped `LateUpdate`
pass can run after `Update`, but it does not guarantee priority over other `LateUpdate` methods,
animations, canvas rebuilds, or later writers. Prefer the correct lifecycle hook or change point and
make any reconciliation bounded and idempotent. Restore original values only while still owning the
override.

## Icon and hover diagnostics

For an unexpectedly full-screen or invisible phone hover/icon element, inspect the actual sprite,
RectTransform, mask/maskable configuration, `CanvasRenderer` alpha, and active state. A historical
Outline element was a filled sprite, so changing only a presumed border colour was ineffective.
Old 620×1000 and 655×1201 measurements described different observed containers, not canonical sizes.

Use a temporary bounded change watcher if a field's writer is unclear; remove its subscription and
logging after diagnosis. Never leave per-frame full-hierarchy dumps in the shipping mod.

## Verify

Test smallest/largest supported display settings, text length, empty/full lists, inline expansion,
modal overlap, orientation, icon hover, and a second open. Verify no app modifies another app's theme
metrics and no hidden element intercepts input.

## Primary source

- [Unity LateUpdate](https://docs.unity3d.com/ScriptReference/MonoBehaviour.LateUpdate.html).
