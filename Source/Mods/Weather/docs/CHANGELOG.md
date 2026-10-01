# Changelog

## 0.4.0 (2026-10-01) - design overhaul: rebuilt to the approved mockup

- **Flat canvas, no header chrome:** "WEATHER" (Sp21) top left, a pulsing amber dot plus a muted `LIVE CONDITIONS` (Sp10) on the same line. The header bar, the accent tick and the divider rule are gone.
- **Hero card = accent-bordered panel:** Dp2 rim (radius 12) with a soft accent halo around the design's 0.052-0.948 / 0.7723-0.9330 band; inside it the giant attention name (Sp32) and percentage (Sp24), a **solid accent pill with dark text** (Sp10, HEAVY/MODERATE/LIGHT) and the `N OF 9 ACTIVE` meta line (Sp10, white 58%). The old overline, glass pill and dark drop shadow are gone.
- **Ring gauge re-proportioned:** side = 0.757 x card height (centre 0.833/0.496), thicker 10% annulus, fully opaque accent arc, plus a new **round arc cap** that follows the eased fill.
- **`ALL CONDITIONS` caption** (Sp14, white 68%) without the former rule line.
- **Rows rebuilt:** uniform Dp6 gaps across the 0.7164-0.0606 band, radius-10 cards with a Dp2 rim; the icon (Dp22) stays left, the name (Sp12) now sits **above its own capsule bar** (Dp7.5), and the percentage (Sp13) is right-aligned and vertically centred on the row.
- **Active conditions tint as a whole row:** base = accent blended 14% into the canvas, rim = the accent, icon and bar in the accent colour. Inactive rows keep the neutral base, the muted rim, a white-10% bar track and grey icons — the previous per-row `CanvasGroup` dimming is gone.
- **Palette aligned to the design:** Sunny `#FDD02E`, Cloudy `#8EB5E1`, Foggy `#ACA496`, Windy `#22A6B9` (measured off the reference), canvas `#13171D`, hero base `#191C1D`; the five components that sit inactive in the reference keep the house accents.
- **Empty state** restyled to the new palette (dark card, muted text).
- **Kept from the previous unreleased iteration:** eased bars/ring, animated hero icons (sun rotation, gentle bobbing) and MIXED on ties.
- **Behaviour unchanged:** lifecycle (single `_active` instance, `WeatherManager.OnWeatherChanged` dispatched once, per-frame snapshot poll, nothing destroyed on close), nine-row no-scroll layout and the no-persistence, no-gameplay-impact scope.
- All anchors are documented as canvas fractions measured off the reference design (400:750 aspect), so the layout maps 1:1 onto the phone canvas at any resolution.

## 0.3.0 (2026-09-26)
- **Minimal dark/glass redesign (visual only):** neutral charcoal canvas with no dominant-colour tint anywhere — the hero is a plain two-layer glass card (outer rounded rect in white 14%, base inset 2 Dp in white 6%, radius 12) plus fake elevation (a slightly larger dark rounded shadow rect at 25% alpha) and a thin light rim. The header accent strip, the sky-gradient sprite and the icon glow of the previous iteration are all gone.
- **Monochrome type hierarchy:** `DOMINANT CONDITION` overline (Sp13, white 48%), hero name Sp38 bold in pure white, big percentage Sp40 in pure white, intensity chip as a dark glass pill (white 10% background, white 90% text) and the `N OF 9 ACTIVE` meta line at white 58%.
- **Radial ring gauge (the only colour on screen):** the hero capsule bar is replaced by a generated anti-aliased donut around the condition icon — a white 10% track plus an `Image.Type.Filled` / `Radial360` arc that carries the dominant condition colour at reduced alpha 0.62 (clockwise from top, `fillAmount` = dominant weight), with the shape-drawn icon centred inside the ring bore.
- **Glass condition rows:** rounded (radius 12) cards with white 6% fill and white 12% rims, each showing a small shape icon in flat white 70%, a bold uppercase name, a Dp8 capsule bar with a white 8% track and white 65% fill, and a right-aligned percentage at white 85%; zero-weight rows dim to 38% as a whole via a per-row `CanvasGroup`.
- **No footer/status row (user-requested):** the app deliberately has no footer band — the old LIVE indicator (accent dot + `LIVE` micro-label), the live/auto-refresh hint and the MelonInfo version pill are gone entirely (the `GetVersionString()` helper was removed with the pill).
- **Overlap-proof hero band math** documented in one comment block (x/y spans per element with explicit clear gaps): the pct-vs-icon collision zone is a 0.05-wide empty gutter between the left text column (ends x 0.66) and the ring box (starts x 0.71).
- **Generated sprites:** capsule, circle, radius-12 glass-card rect and the donut annulus; the vertical gradient and radial glow sprite helpers were deleted with the restyle. All non-interactive graphics keep `raycastTarget = false`.
- **Behaviour unchanged:** lifecycle, per-frame polling, `WeatherManager` dispatch through the single `_active` instance, empty-state text and the nine-row no-scroll layout (worst-case row stack recomputed against the extended list band at every supported scale).

## 0.2.0 (2026-09-26)
- **UI redesign (visual polish only):** prominent hero card for the dominant condition — big name + percentage, left accent strip in the condition's colour, rounded progress bar, intensity chip (HEAVY/MODERATE/LIGHT) and an active-count readout.
- **Subtle background tint** derived from the dominant condition colour (plus a matching header accent tick and a soft glow behind the hero icon).
- **Shape-drawn condition icons** per weather type (sun disc with rays, clouds, drop bars, violet bolt zigzag, snow dots, layered fog bars, wind curls, hail stones) built from plain UI Images — no glyphs, since legacy Arial Text cannot render emoji.
- **Clean component rows:** rounded cards with colour chips, rounded (capsule) progress bars in each condition's colour and percentage labels; zero-weight rows stay dimmed.
- **Generated rounded sprites** (rounded rect / capsule / circle / radial glow) for cards, bars and badges — consistent rounded look at any canvas size.
- **Section spacing + footer version badge:** "ALL CONDITIONS" label with rule line, live/auto-refresh hint and a MelonInfo version pill moved to the footer.
- Explicit anchor math throughout; the nine rows share the list band exactly (VerticalLayoutGroup + flexible heights), so nothing overlaps or clips at any supported phone-canvas size.


All notable changes to **Weather** are documented in this file.

## 0.1.0 (2026-09-24)
- **Initial release** — read-only weather dashboard PhoneApp (`WeatherApp : PhoneApp`, `EOrientation.Vertical`).
- **Dominant-condition hero readout:** the highest-weighted component is rendered large (`RAINY  62%`) with its own accent colour, an intensity line (`Heavy` > 0.66 / `Moderate` 0.33–0.66 / `Light` > 0 / `Clear` at 0) and a wide progress bar.
- **All nine components listed** (Sunny, Cloudy, Rainy, Stormy, Snowy, Foggy, Windy, Hail, Sleet) with name, horizontal progress bar and percentage; zero-weight rows are dimmed so active conditions stand out.
- **Live rendering** on `WeatherManager.OnWeatherChanged` (cache-while-closed, render-on-open) plus a per-frame `WeatherManager.Current` comparison as a safety net.
- **Graceful empty state** when `WeatherManager.Current` is `null` ("No weather data available" + in-game hint).
- **Responsive layout** via `S1Mods.Shared.UITheme` (`Sp`/`Dp`); the nine rows share the available height, so no scrolling is needed at any resolution.
- **Lifecycle per runbook:** isolated `_mainBG` (`fullAnchor: true`, starts inactive), `OnUpdate` subscribed exactly once (defensive `Unsubscribe`-before-`Subscribe` in `OnCreated`), nothing destroyed/unsubscribed in `OnPhoneClosed` (Rules 2/3/10), single static dispatcher for `WeatherManager.OnWeatherChanged` (no subscriber leaks per scene load), <kbd>Escape</kbd> closes the app.
- No persistence, no gameplay influence — pure status display.
