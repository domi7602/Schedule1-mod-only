# PropertyLEDs 0.1.4 — full-height responsive preview

**Offline comparison, not native Unity acceptance.** The left image is the owner's current
0.1.3 screenshot, cropped to screen x=60..337, y=77..586. The right view applies the new policy
with existing icons, English captions and navy palette. Colors/settings are fixtures, not save data;
the amber preview versus pink screenshot does not represent a persisted-color change.

## Artifacts

- [Self-contained HTML](propertyleds-0.1.4.html)
- [Collapsed](comparison-0.1.4.png), [expanded](comparison-0.1.4-expanded.png),
  [small-expanded](comparison-0.1.4-small-expanded.png) screenshots
- [Measured browser geometry](rendered-metrics-0.1.4.json)
- [15 passing browser checks](browser-verification-0.1.4.json)
- [Native constants and policy source](responsive-policy.json)

## Policy

Header stays pinned. Scroll content starts at 10% and ends at a 1.5% bottom margin: 88.5% of the
measured native frame. At observed native height 1201 this gives 1062.9 units instead of 960.8.
Minimum content remains 524 dp collapsed / 580 dp expanded. Extra height stretches actual selector
cards, preview, slider breathing room, Advanced/power/actions and gaps proportionally. Native
priority-10 height contracts override text; ContentSizeFitter is unconstrained and content sized
from the pure policy, with no minimum-height/fitter feedback cycle.

Controls cap at 1.5x and gaps at 3x; exceptionally huge outliers stop growing instead of inventing
blank spacers. Smaller viewports retain usable minima and scrolling. Advanced adds its real slider
and gap; lower actions stay in scroll content. Inline state contributes 32 dp and one gap only while
visible. There is no normal footer/info icon or hidden footer reservation.

Measured collapsed browser geometry relative to the 277 x 509 screen:

| Element | Top | Height |
|---|---:|---:|
| Property | 50.90 | 54.18 |
| Scope | 111.95 | 54.18 |
| Preview | 173.00 | 46.44 |
| Power | 401.68 | 51.59 |
| Presets | 460.14 | 41.27 |

Presets end at 501.41, leaving the intended approximately 7.6-pixel margin. Selector text stays
centered. The fixed 23 dp handle parent and zero cross-axis height delta keep native circles from
stretching when Slider.UpdateVisuals drives anchors to 0..1.

## Verification and limits

Installed Helium Chromium ran with an isolated scratch profile and CDP port 9339. The stdlib
WebSocket client read actual DOM geometry, ran 15 checks and captured three screenshots; zero
Runtime.exceptionThrown events. Only the owned browser was closed. Checks cover fixture power/edit/
brightness-zero behavior, Advanced retention, full-height sizing, footer absence, small overflow,
growth caps, inline errors/space release and native-1201 budget.

Integer browser clientHeight gives 1063.3 versus native float 1062.9: a subpixel difference, not a
changed native constant. Browser text/masks/fixture inputs cannot prove Unity/IL2CPP behavior.
The preview retains its existing idealized rounded switch; the actual native switch silhouette
was not redesigned by this scope and still needs inspection.

Native acceptance remains pending: first open/convergence/reopen, Advanced collapse without reset,
round handles during value changes, real selector/sliders/power/inheritance input, expanded action
reachability, small-view scrolling, inline-state readability/recovery, brightness-zero edits, scope
navigation, slot persistence and cleanup. Next breadcrumb records viewport/content/expected,
Advanced/inline state, applied selector/preview/slider/power/actions/gap sizes and overflow.
Only material measured viewport/state changes trigger rebuilding; no per-frame discovery or force-update.
