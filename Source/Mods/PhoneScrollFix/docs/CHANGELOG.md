# Changelog

## 2.0.0 (2026-10-11)
- Clean release: debug watcher/baseline logging removed (diagnosis complete).
  Keeps the finished fix set — viewport `Mask` -> `RectMask2D`, hover `Outline`
  -> slim ring sprite, `Image.maskable = true`, frame alpha 0.85.
  User-confirmed in-game: the hover now looks like vanilla (thin frame, clipped
  with the icon, no white plate on half-scrolled icons).

## 1.5.0 (2026-10-11)
- Final piece (user screenshot): the hover highlight is NON-maskable in
  vanilla, so it escapes the scroll viewport's clipping and renders full-size
  even when its icon is partly scrolled out ("floating frame over a clipped
  icon"). `Image.maskable = true` is now applied, so the highlight is clipped
  exactly like the icon art. With the v1.4.0 ring sprite this yields a clean
  slim frame that always hugs the visible icon part.

## 1.4.0 (2026-10-11)
- Dimming only turned the plate grey (user check), so the fill sprite itself is
  the problem. The hover `Outline` sprite is now replaced with a procedurally
  generated rounded RING (transparent centre) — the hover reads as a slim white
  frame around the icon instead of a filled plate, on clipped and unclipped
  icons alike. Frame colour alpha 0.85; vanilla fade untouched.

## 1.3.0 (2026-10-11)
- Root cause confirmed live: hovering an icon fades in its `Outline` child
  (white sprite `Rectangle_RoundedEdges`, CanvasRenderer alpha 0 -> 1) — that
  is the white plate, and it sits over the status bar on the S1API scroll
  layout. Fix: cap the Outline's Image colour alpha (0.30) so the hover reads
  as a soft glow instead of a hard white plate; the vanilla fade itself is
  untouched. Also logs a one-time full geometry baseline of icon #0.

## 1.2.0 (2026-10-11)
- Field-level watcher: dumps every field of the full home-screen icon subtrees
  (active, scale, position, size, image colour, canvas alpha, sprite name) and
  the HomeScreen sibling subtrees every 0.5 s, logging only the fields that
  change with the owning icon name. v1.1.0's narrow 4-value watch caught no
  change on hover; this one cannot miss a hierarchy change. Mask->RectMask2D
  swap stays active.

## 1.1.0 (2026-10-11)
- Debug watcher: logs per-icon render values (root scale, Shade alpha,
  renderer alpha, Mask/Image scale) on every change, so a hovered white icon
  is named in Latest.log. Swap now also reports already-fixed/no-mask states.

## 1.0.0 (2026-10-11)
- Initial release: swap S1API's stencil viewport `Mask` on the phone
  home-screen icon grid for a `RectMask2D`, fixing the white focus glow on
  half-clipped icons. Ordering-proof polling, re-check after scene loads.
  In-game verification open.

## 0.1.0
- Initiale Version.
