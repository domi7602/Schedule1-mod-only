# PropertyLEDs visual comparisons

**Prior 0.1.4 evidence (historical): [full-height responsive preview](README-0.1.4.md)** — footer removed,
measured control stretching, collapsed/expanded/overflow screenshots and 15 passing browser checks.
Those constants are not the 0.1.5 phone sizes. No new browser comparison was rendered for 0.1.5.

## Historical 0.1.3 rendered comparison

The report below describes the previous layout, not the current responsive sizing policy.

The primary surface is **Configure**: property/scope selection, color editing, power and inheritance.
This is an offline comparison artifact, **not native Unity acceptance**.

## Artifacts

- [Self-contained HTML comparison](propertyleds-0.1.3.html): original cropped mockup beside the
  new view, with embedded raster icons and no network dependencies. Demonstration values only.
- [Rendered screenshot](comparison.png): captured by the installed Helium Chromium browser via CDP.
- [Reference crop](mockup-crop.png): screen rectangle x=691..982, y=188..780 from the user PNG.
- [Source measurements](measurements.json): image bounds, sampled palette and native AppLayout constants.
- [Browser geometry](rendered-metrics.json): actual browser bounding rectangles, not estimates.
- [Interaction checks](browser-verification.json): seven passing demonstration checks.

The preview parses AppLayout.cs constants when generated. It uses the same white-alpha raster icon
predicates as the native implementation, at the reference 291 x 592 screen size, with a 750-unit
reference-height canvas. The production layout stays responsive to its native phone width.
The supplied PNG is 1672 x 941; its isolated screen aspect is not assumed to equal the native canvas.

## Measured comparison

All coordinates below are pixels relative to the cropped phone screen. Reference boundaries were
identified in the supplied image and sampled from decoded pixels; preview rectangles were read
from the rendered browser DOM. Subpixel values are rounded to two decimals.

| Element | Reference top / height | Rendered top / height | Difference |
|---|---|---|---|
| Property card | 59 / 50 | 59.20 / 49.73 | top +0.20, height -0.27 |
| Scope card | 115 / 49 | 115.24 / 49.73 | top +0.24, height +0.73 |
| Color panel | 174 / 43 | 171.29 / 42.62 | top -2.71, height -0.38 |
| Power card | 391 / 49 | 381.25 / 47.36 | top -9.75, height -1.64 |
| Presets | 452 / 38 | 434.92 / 37.89 | top -17.08, height -0.11 |

Selector/power card left edges are 13.09 versus reference 13; widths 264.82 versus 264.
The preview color panel is aligned with the cards (width 264.82), whereas the reference's inset
panel is 260 wide. Initial rendering placed the scope/color panel too high; a second RED -> GREEN
sizing cycle increased selector height to 63 dp and outer gaps to 8 dp. Final content budgets are
524 dp collapsed and 580 dp expanded. Native late-layout breadcrumbs must validate actual sizes.

## What matches, and what intentionally differs

- Matched structure: bulb/title/subtitle, outlined dark-navy property/scope cards, drawn house/layers
  tiles, large rounded selected-color panel, rainbow and white-to-color tracks, round handles,
  row icons, bottom power-state card with a working switch, and compact White/inheritance actions.
- Intentional simplification: Color has no degree value; Color intensity is hidden under Advanced.
  Brightness remains 0..5. There is no dead gear or second Turn LEDs Off action. Two selector arrows
  preserve actual existing navigation instead of pretending there is a dropdown.
- Inheritance is disabled in the default-scope demonstration, so its button is subdued instead of
  cyan. Native code accents it only when an actual room override can be removed. Text/icons remain
  readable together. Footer coverage is actual native selected-property data; HTML 2/2 is fixture data.
- Remaining visual differences: flatter surfaces/no glow, simpler bulb/layers/info icon treatment,
  extra small selector index/context line, values aligned with slider headings instead of track
  centers, and more space above the footer after removing redundant controls. The selected-color
  panel uses the unchanged effective HSV/intensity values; its fixture amber is darker than the
  mockup's stylized glowing amber. Do not brighten it by changing persisted color semantics.
- No full mockup-match claim: the browser screenshot checks composition and measured geometry only.
  Browser CSS text and masking are not Unity Text, uGUI masks or IL2CPP pointer input.

## Verification and limits

Seven browser checks passed: power switch state, Advanced expansion, collapse retaining intensity,
color-edit auto-enable, zero-brightness edit preserving explicit off, one power control, and no
redundant power button. Runtime.exceptionThrown count was zero in the CDP session.

The straightforward headless CLI dump-DOM path returned success without usable output on this
host. An isolated browser with remote debugging plus a stdlib WebSocket/CDP client successfully
ran verification, read geometry and captured the screenshot; only that owned browser was closed.
No dependency install or game launch was needed.

Native acceptance remains pending: first-open/reopen sizing, drawn sprites, rounded gradient alpha,
hue endpoints, saturation hue refresh, real selector clicks, slider dragging, power changes,
disabled inheritance, auto-enable/brightness 0, property edits clearing overrides, room inheritance,
slot persistence, scrolling at small sizes, listener uniqueness and destruction cleanup.

Design self-audit: no wrong-surface composition, hero, fake metrics, feature-card grid, glass blur,
icon toppers or decorative gradient background. The rainbow is a functional hue track, not decoration.
The artifact retains Arial deliberately because the native view uses legacy Unity Text.
