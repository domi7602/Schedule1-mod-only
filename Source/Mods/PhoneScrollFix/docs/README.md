# PhoneScrollFix v2.0.0

Fixes the white hover plate on phone home-screen app icons in **Schedule I v0.4.7** (TVGS, IL2CPP, MelonLoader 0.7.3, S1API 3.2.1).

## Problem

Once many mods add phone apps, S1API makes the home-screen icon grid scrollable
(stencil viewport `Mask`). Then hovering an app icon looks broken:

- the hover highlight reads as a **big white plate** covering the icon, and
- on a **half-scrolled icon** the plate floats over the clipped area
  (it even covers the status bar).

## Root cause (live-confirmed 2026-10-11)

Diagnosed with S1MCP plus an in-mod field-level watcher (every field of every
icon subtree diffed every 0.5 s):

1. The hover highlight is the icon's `Outline` child — a **filled white
   rounded-rect** (`Rectangle_RoundedEdges`, 184x185 on a 190 cell while the
   icon art is only 170x170), faded in via `CanvasRenderer` alpha 0 -> 1.
2. That graphic is **non-maskable** in vanilla, so it **escapes the scroll
   viewport's clipping** — the classic "white plate over a half-clipped icon".
   Root scale (1.0) and every other field stay untouched; dimming the colour
   only turned the plate grey, which pinpointed the sprite itself.

## Fix (visuals only — no gameplay, config or saves touched)

- Hover `Outline` sprite replaced with a procedurally generated **slim rounded
  ring** (transparent centre) -> highlight reads as a thin frame.
- `Image.maskable = true` -> the frame is **clipped exactly like the icon art**.
- Frame colour alpha 0.85 -> soft, vanilla-looking highlight.
- Scroll viewport `Mask` (+ its near-transparent white graphic) replaced with a
  stencil-free `RectMask2D`, which does not misrender at the scroll boundary.

- Ordering-proof: polls until applied, so it runs after S1API's
  `HomeScreenScrollPatch` regardless of mod load order; re-checks after scene
  loads.
- Removal: delete `PhoneScrollFix.dll` — everything returns to stock behavior.

## Verification

- [x] Release build, 0 warnings / 0 errors, deployed to `Mods/` (v2.0.0)
- [x] Hover on a half-scrolled icon: no white plate, thin frame clipped with
      the icon — **user-confirmed in-game 2026-10-11** ("jetzt ist das vanilla")
- [x] Log evidence: `hover frame applied ... (maskable False -> true)` — the
      non-maskable highlight was the confirmed culprit
- [ ] Long-session check: scene reload / phone reopen (mechanism re-applies per
      scene; not separately re-tested)
