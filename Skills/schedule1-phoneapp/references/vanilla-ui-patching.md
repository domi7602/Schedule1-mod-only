# Patching & Theming Vanilla Phone UI — Field Notes (MessagesPlus)

> Verified against MessagesPlus v0.3.0–v0.4.1 (in-game round 2026-09-30, plus the S1API 3.2.1-beta.7 / game 0.4.7f6-7 line).
> This covers the *vanilla-surfaces* side of a PhoneApp-style mod: a patch-only mod that restyles or extends the game's own screens instead of hosting its own `_mainBG`.

---

## 1. When to patch vs inject

* **Inject** your own controls as a new subtree (band, toolbar, modal) — full control, no impact on vanilla layout.
* **Patch** vanilla methods when you must (a) restyle vanilla surfaces, (b) make room for injected UI, or (c) react to a popup opening in the same frame.
* Keep the ownership rule: only hide/show, restyle or relocate what the mod itself owns; treated vanilla state (e.g. hidden conversations) is left alone unless the feature explicitly mutates it (and then host-only + save-safe).

## 2. Same-frame theme refresh on popup open

**Problem:** a freshly shown vanilla popup (e.g. `DealWindowSelector` — the Morning/Afternoon/Night/LateNight picker) appeared in the light theme and only darkened on the next 1 s theme tick.

**Fix (`DealWindowSelectorPatch`, MessagesPlus v0.4.1):**
* Postfix `SetIsOpen` — one shared postfix for **both overloads** — and force-refresh the popup's subtree in the same frame.
* The forced refresh must not overwrite the cached originals (restore must stay exact).
* Generalise: any vanilla surface that appears on demand needs either a same-frame hook on its show method or a forced re-tint after it re-colours itself.

## 3. Whole-app theming (`AppTheme`)

* One theme applier recolours the mod's injected surfaces **and** selected vanilla ones: page backgrounds, inbox rows + their texts, chat bubbles + tails, dialogue header + response panel, plus a conservative near-white sweep for leftovers.
* **Colours only** — never touch layouts, listeners or `raycastTarget` in the theme applier.
* **One-time per graphic with cached originals** so light mode restores the game's exact look; re-tint graphics the vanilla code re-colours.
* **Never theme avatars, badges or the unread dot** — they carry identity/state.
* Permanently-ON mode (v0.4.1): config defaults to ON and a stale `false` self-heals at startup; keep the config field for schema stability instead of deleting it.

## 4. Making room in vanilla layouts

* Reserve space for an injected band by taking its height off the top of the vanilla viewport: `ScrollRect.viewport.offsetMax.y -= band` (`TryMakeRoom`), and **hand the space back before every rebuild** (`UndoMakeRoom`) so repeated rebuilds don't shorten the list cumulatively.
* Re-assert the reservation from a throttled (1 s) tick — vanilla re-lays out the list on its own events (`ReassertRoom`).
* If no `ScrollRect` exists, fall back to a fixed offset under the title and log it — never guess silently.

## 5. Rebuild safety

* Rebuilt controls need **defensive Remove-before-Add** listener wiring: `EventHelper`/`ButtonUtils` dedupe globally per delegate instance, so a rebuilt button would silently get no listener (see `schedule1-modding` Key Rule 19).
* Destroy the mod's own managed UI roots before re-injecting (no orphaned/duplicate toolbar after a mid-build failure); liveness-check buttons and self-heal on the next open/tick.
* `raycastTarget`: false on all decorative graphics; true on anything that must receive clicks — especially the visible surface of a search/`InputField` (a blanket-off decor helper made the field dead in MessagesPlus v0.4.0; see `schedule1-phoneapp` Rule 17).
* Modal confirmations: a no-op click catcher on the card background stops backdrop click-through; reset the pending action on app close.
* View-only filtering (search/chips) must only toggle `SetActive` on entries the mod itself owns and re-apply from the throttled tick while active; destructive actions stay host-only + category-restricted, never on a multiplayer client.

## 6. Config self-heal

When a setting becomes permanent (dark mode ON), keep the field, ignore/rewrite stale values at startup, and log the self-heal once. Users who had toggled the old value get consistent behaviour without a config migration.

---

**Reference implementation:** `Source/Mods/MessagesPlus/src/` (`AppTheme`, `InboxUI`, `DealWindowSelectorPatch`, `ConversationUtils`).
**Related:** `schedule1-phoneapp` SKILL Rules 16–18; `schedule1-modding` Key Rule 19 (EventHelper dedupe).
