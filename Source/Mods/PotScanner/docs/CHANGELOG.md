# Changelog


## 0.5.2 (2026-09-11)
- WasCollected-Guards in Update/Handler/PhoneClosed; Row-Caches werden bei Unload geleert.

## 0.5.1 (2026-09-10)
- Statischer Event-Dispatcher; PotTracker.NotifyPotsScanned mit Per-Handler-try/catch.
- WaterAll/WaterSinglePot nur noch als Host (Fix Multiplayer-Geldverlust/Desync).

## 0.5.0
- **Modernized Terminal & Console Bridge (`pot` subcommands):**
  - **`pot summary` / `pot status` (NEW):** Outputs a compact 1-line overview per property showing pot count, ready crops, dry pots, growing plants, or moist status (e.g. `Barn: 12 pots (4 ready, 2 dry, 6 growing) | Bungalow: 8 pots (8 moist)`).
  - **`pot stats` (NEW):** Displays global analytics across the world including total pots, owned pots percentage, empty pots, active crops, harvest-ready plants, thirsty pots requiring water, total estimated watering cost, and current auto-water status.
  - **Updated `pot help`:** Documents all subcommands (`list`, `summary`, `stats`, `water-all`, `auto`, `refresh`, `help`) with aliases and usage examples.
  - **Standardized English Terminal Logging:** Fully refactored console feedback and log messages to clean, professional English.

- **Bug Fixes:**
  - **Double-Charge Fix in `WaterAllService`:** Resolved an issue where funds were deducted both upfront as a bulk total and iteratively inside the per-pot watering loop. Charging is now performed strictly once per successfully watered pot.
  - **Auto-Water Loop & Safeguard Hardening:** Ensured `AutoWaterService` strictly observes per-pot cooldowns (5s) and player cash limits, preventing infinite auto-water drain loops or negative balance glitches.
  - **German String Localization Clean-up:** Eliminated residual German fallback strings across `PotInfo`, `PotTracker`, and `WaterAllService` (`(leer)` → `(empty)`, `(unbenannt)` → `(unnamed)`, `für` → `for`, `bereits bewässert` → `already watered`).

- **Performance & Architectural Optimizations:**
  - **0-Allocation In-Place UI Updates:** `PotScannerApp.RefreshList` reuses existing `PotRowUIRef` and `PropertyGroupUIRef` structures without teardown allocations when pot counts match.
  - **Single Scene-Scan Cache Reuse:** Optimized `WaterAllService` and `AutoWaterService` to query `FindObjectsByType<GrowContainer>` once per action cycle, indexing by `IntPtr` in a fast dictionary lookup.
  - **Native Pointer Safety:** Robust `IntPtr` tracking survives scene transitions without garbage collection wrapper crashes.

- **UI & UX Refinements:**
  - **Single-Property Focus & Compact Hierarchy:** Accordion navigation allows drilling down into single properties without cluttering the screen.
  - **Dynamic `UITheme` Responsive Engine:** Precise canvas dimension scaling prevents over-inflated tiles on higher resolutions.
  - **Quality & Harvest Status Badges:** Visual clarity for plant strains, ready states (`★`), and actionable moisture percentages (`💧`).

## 0.4.1
- **Single-Property Focus Mode & Compact UI Redesign:**
  - **Single-Property Focus (Accordion/Focus Mode):** Tapping a property (e.g. "Barn") completely hides all other properties, so you only see the pots of the selected property. As soon as you click the header again to close the property, all property headers reappear in the overview.
  - **Compact UI Scaling:**
    - `UITheme.Scale` damped to a maximum of `1.20f` (reference height 900f), preventing oversized tiles on high-resolution phone canvases.
    - **Action Row:** Height reduced to slim `36px` (from 58px); font sizes optimised to `13pt` (title) and `10pt` (sublabel).
    - **Property Header:** Height reduced to `30px` (from 40px); font size `13pt` / `10pt`.
    - **Pot Rows:** Height optimised to compact `46px` (from 74px, almost 40% space saved!). 8-12 pots now fit on the screen simultaneously without scrolling.
    - **Progress Bars:** Slimmer bars (height 0.26, font size 9pt) for water (W), growth (G) and soil (S).
  - **Instant Open/Close (Instant Toggle):** Switching happens directly via GameObjects (`SetActive`) without UI teardown. 0 GC allocations and stable scroll position.
  - **Interactive Header Feedback:** Property header with tangible hover/pressed `ColorTint` and dynamic chevron arrows (`▼` / `▶`).

  - **Issue 1 (Layout Too Close to the Edge):** Y zones completely re-structured with a clear separation:
    - TopBar: y=0.92→1.00 (8% instead of 5%, paddingL/R=20 instead of 40)
    - ActionRow: y=0.81→0.91 (10% instead of 8%)
    - Status: y=0.78→0.81 (own row, centred)
    - ListPanel: y=0.03→0.77 (74% instead of 77%)
  - **Issue 2 (Property Header Title Clipped):** Completely switched to a 2-line layout — title on top, badge on the bottom, both full width. The previous 70/30 horizontal split (v0.4.0) was the root cause: long property names like "Hyland Point Farm" were visually clipped ("Hyland Point F" + "arm" → only "am" visible). 2-line layout consistent with pot rows.
  - **Issue 3 (Pot Row Title Shows "()"):** Anchors verified once more (Title 0.5→1.0, Subtitle 0.0→0.5, both 0→1 horizontally with 16px padding). Row height 52/56 → 56/60px increased. One-time debug log `[PotScanner-Diag] Pot @PTR: PlantName='X' (len=N), Title='Y'` (throttled to 4 samples) to see if `PlantName` is really empty, an unusual value, or if the title is very long and would need to be clipped.
  - **Issue 4 (Loading State):** `_initialScanComplete` flag. `OnCreatedUI` initially sets `_initialScanComplete = false` + `ApplyLoadingState()` → buttons disabled with sublabel "...", status "Loading...". `RefreshList` sets the flag to `true` as soon as it runs. Buttons stay disabled until the first `RefreshList` has passed — no more clicking on "Water All (0)". `OnCreatedUI` calls `PotTracker.Instance.RefreshNow()` at the end in case the tracker is already active — fires synchronously `OnPotsScanned` → handler calls `RefreshList` → UI shows real data immediately.
  - **Buttons: `HorizontalLayoutGroup` for Action Row:** `childForceExpandWidth/Height=true` → Water All and Auto-Water automatically 50/50 with 10px spacing, no more manual anchorMax.x=0.48/0.52. Title font 12→13pt, subtitle 9→10pt for better readability.
  - **Dead Code Removed:** v0.3.1 `LogRectDiagnostics` method removed.
  - **Verified:** Build clean (0 errors, 0 warnings). Source includes for Hash.Api untouched. `pot` console commands API unchanged.

## 0.4.0
- **Hash Terminal Integration (`pot` subcommands, hash + vanilla console):**
  - **Hash.Api Plugin Pattern:** DooDesch's `Hash.Api/HashCommands.cs` as a source include in `PotScanner.csproj` (`<Compile Include="...HashCommands.cs" Link="Hash.Api.cs" />`) — no HintPath reference, no compile coupling, builds and runs with or without hash deployed. When `Hash.dll` is missing, `Add()` is a no-op (queued for later, replayed when hash loads).
  - **`HashCommands.Add("pot", "...", "pot list")` in `Mod.OnInitializeMelon`:** registers the word in the game's own command list, so it shows up in vanilla autocomplete, hash terminal autocomplete, the `help` overlay, and anywhere else (not just in hash).
  - **`ConsoleBridge.cs` (NEW, 6.7 KB):** Central `[ConsoleCommand("pot", "...")]` attribute method (MelonLoader), routes subcommands to the existing services. Works in the vanilla terminal AND in the hash terminal (both send to the same `SubmitCommand` pipeline).
  - **Subcommands:**
    - `pot list` (alias `ls`, `l`) — compact list of all pots, grouped by property, with status icon (`ready`/`grow`/`empty`), water% and growth% as 3-char aligned columns. Also shows total stats in the header line (X pots, Y ready, Z dry).
    - `pot water-all` (alias `water`, `w`) — triggers `WaterAllService.WaterAll()`, returns the result message on a single line.
    - `pot auto` — toggle. `pot auto on` (alias `1`/`true`/`yes`/`enable`) / `pot auto off` (alias `0`/`false`/`no`/`disable`) — set explicitly. Persisted via `ModConfig.SetAndSave` like the PhoneApp toggle.
    - `pot refresh` (alias `r`) — `PotTracker.Instance.RefreshNow()`, bypasses 2s polling.
    - `pot help` (alias `h`, `?`) — lists all subcommands.
  - **Output via `MelonLogger.Msg()`** — appears in hash terminal (colourful), vanilla terminal (in-game log), and MelonLoader logfile.
  - **Defensive:** `try/catch` around the whole switch, `MelonLogger.Warning` instead of throw, so a broken subcommand doesn't take the whole line down with it.
  - **Verified:** Build clean (0 errors, 0 warnings against the real S1API stack + Hash.Api shim + Schedule I refs). Tested: `pot help` lists 5 subcommands, `pot list` shows pot data, `pot water-all` triggers the service, `pot auto on/off/toggle` persists.

## 0.3.1
- **Fix: Property Header & Pot Row Layout (Anchor Overlap Bug):**
  - **Root Cause:** `UIFactory.CreateTextBlock` does NOT set the `RectTransform` anchors of its two text children — both end up with default anchors `(0.5, 0.5)` inside the parent. The previous `HorizontalLayoutGroup` in `CreatePropertyHeaderRow`/`CreatePotRow` only overrode `sizeDelta.x`, but left anchors/position untouched → both texts overlap completely in the middle of the row, "later sibling renders on top" → subtitle covers title. Title `"▶ Barn"` and `"○ (leer)"` were completely invisible.
  - **Fix:** `HorizontalLayoutGroup` removed from the rows. Title and subtitle are now positioned manually with `rectTransform.anchorMin/anchorMax/offsetMin/offsetMax` (NotesApp pattern from `Source/Mods/NotesApp/src/NotesApp.cs:779-811`). Title on the left (`anchorMax.x=0.7` Header / `0.55` Pot), subtitle on the right.
  - **Text Overflow Safety:** `horizontalOverflow = HorizontalWrapMode.Overflow`, `verticalOverflow = VerticalWrapMode.Truncate` on both text components — long property/plant names now clip cleanly at the rect edge instead of wrapping into a second line (which would have blown up the layout).
  - **`raycastTarget = false`** on both text components — without this, text graphics could intercept the row's button raycast and silently degrade click reliability.
  - **Badge Colour Coding:** The header badge now gets different colours per status: `★ X ready` (gold), `💧 X dry` (light blue), `X pots` (neutral). Pot title icon (`★`/`●`/`○`) also colourised (gold/green/grey).
  - **Verification:** ilspycmd decompile of the deployed `S1API.Il2Cpp.MelonLoader.dll` 3.1.7 confirms: `UIFactory.Text()` only does `AddComponent<RectTransform>()` without anchor adjustment.

## 0.4.2
- **Fix for Clipped Text & Cramped Buttons:**
  - **No More Left-Letter Clipping:** `UIFactory.CreateQuestRow` replaced with clean direct `UIFactory.Panel` elements with explicit **`16px` inner padding**.
  - **Full Visibility:** Text like `▶ Barn`, `Motel Room` and status badges are now 100% readable without clipped first letters.
  - **Compact Button Labels:** `Water All (OK)` instead of the 38-character text with inactive button — prevents cramping.

## 0.4.1
- **Refined Mockup UI Integration:**
  - **Slimmer TopBar:** Reduced to `40px` height for maximum scroll area.
  - **Compact Action Buttons:** Text buttons without emojis (`Water All` / `Auto: OFF` / `Auto: ON`).
  - **Muted Colour Palette:** Restrained dark blue (`#142F4D`) & forest green (`#1D3025`).
  - **Structured Pot Rows:** Status labels & bars `W ━━━── 60%` and `G ━━━━━━ 100%`.

## 0.4.0
- **Modern UI Redesign (Mockup-Faithful Revision):**
  - **🍃 Rounded Header:** `🍃 Pot Scanner` title card at the very top in modern dark-slate style (`#1E293B`).
  - **💊 Pill-shaped Action Buttons:**
    - **`💧 Water All`:** Pill button in vibrant ocean blue (`#2563EB` / `#1E3A8A`).
    - **`⚙️ Auto`:** Pill button in emerald green tone (`#16A34A` / `#1F402B`).
  - **🏷️ Property Cards with Colour Badges:**
    - Dark blue cards (`#1E2638`) with property name on the left (`Motel Room`) and rounded status badges on the right (`★ 3 ready` gold / `💧 2 dry` red).
    - Chevron arrows (`▲` / `▼`) for expand/collapse.
  - **📊 Nested Pot Tiles with Progress Bars:**
    - Inner tiles (`#0F172A`) per pot with icon (`★` gold / `●` green / `○` grey) + plant name.
    - **Visual progress bars:** Coloured bars for water (blue `💧 ━━━── 60%`) and growth (green/gold `🌱 ━━━━━━ 100%`).

## 0.3.9
- **Instant Display (0ms Load Time) & No More Glaring White Buttons:**
  - **Instant Scan on App Open:** `PotTracker.Instance.RefreshNow()` is called directly when the app is created/opened. All pots are immediately there (0 ms delay, no more waiting for the next 2s scan).
  - **No Glaring White Fade:** `Selectable.Transition.None` disables Unity's automatic fade colour when `interactable = false`.
  - **Elegant Blue Tones:**
    - Active (pots to water): Ocean blue (`#3373B2`).
    - Disabled (all sufficiently watered): Dark slate blue (`#2E4766`). Guaranteed to stay dark and blue!

## 0.3.8
- **Custom Panel+Button Construction for Slim Buttons:**
  - S1API's `RoundedButtonWithLabel` replaced with direct Unity UI `Panel` + `Text` + `Button` components.
  - Prevents automatic inflation/stretching of buttons through internal S1API layouts.
  - Buttons now stick strictly to an elegant `32px` height.
  - Absolutely clean separation: `TopBar` (0.90–1.00), Buttons (0.82–0.88), Status (0.78–0.81), `PotList` (0.02–0.77).
  - No more overlaps.

## 0.3.7
- **Slim Button Height (~32px) & Increased Scroll Area:**
  - **Button Height Drastically Reduced:** `ActionRow` placed at Y `[0.83, 0.89]` (~32px height instead of previous ~150px giant tiles).
  - **Font Size 13pt:** Perfect fit for long text (*e.g. 💧 Water All (30 pots, 1500g)*) without vertical inflation.
  - **Scroll Area Expanded:** `PotList` now runs from Y `0.02` to `0.78` (was `0.67`). That gives 11% more screen space for pots.
  - **Zero Overlap:** `RectMask2D` cleanly clips pots scrolling upward at Y `0.78`, well below the buttons (`0.83`).

## 0.3.6
- **`RectMask2D` Clipping for the Pot Scroll Panel:**
  - `UnityEngine.UI.RectMask2D` added to the `listPanel` container.
  - On scroll, all upward-moving elements (like property headers) are strictly clipped at the top edge of the scroll area (Y = 0.67).
  - `actionPanel.transform.SetAsLastSibling()` called to permanently anchor the buttons as the topmost UI element.
  - Scrolling upward can now under no circumstances overlap the action buttons.

## 0.3.5
- **Dedicated `ActionRow` Container Panel for Buttons:**
  - The action buttons (*Water All* / *Auto*) now have their own isolated container panel (`ActionRow`) at Y `[0.72, 0.84]`.
  - The pot scroll panel (`PotList`) ends strictly at Y `0.67` — overlaps between the buttons and the property headers are structurally impossible.
  - Font size in the buttons slightly adjusted (`15pt`) to cleanly centre long numeric values (*e.g. 30 pots, 1500g*).

## 0.3.4
- **Button Positioning Below TopBar Fixed:**
  - Buttons (*Water All* / *Auto*) placed at Y `[0.78, 0.86]` — exactly in the free space below the black `TopBar` ("Pot Scanner") and above the pot scroll panel (`0.74`).
  - The buttons are now fully visible, perfectly readable, and not obscured by any header.
  - Property-header height adjusted to `40px` so the two-line property summary is displayed without text clipping.

## 0.3.3
- **Strict Row-Height Investigation & Button-Clipping Fix:**
  - **LayoutElement Override:** `LayoutElement.minHeight` and `preferredHeight` explicitly forced to `26px` (pots) and `34px` (headers) to override the default QuestRow minimum of 55px.
  - **Row Spacing Eliminated:** Vertical padding of the `HorizontalLayoutGroup` reduced to 1px — spacing between rows is now completely minimal.
  - **Button Clipping Fixed:** Action buttons (*Water All* / *Auto*) raised to Y `[0.88, 0.93]` and the scroll panel lowered to `0.83`, so buttons are no longer clipped by the black panel border.

## 0.3.2
- **Ultra-Compact Single-Line View & Layout Fix:**
  - **Square Icons Removed:** The large 64×64 quest-icon boxes are hidden (`iconPanel.SetActive(false)`), using the full row width for text.
  - **Single-Line Layout (~28-32px Row Height):** Each pot is displayed in a single slim row (`★ READY OG Kush Seed · W:90% G:100% Q:50%`).
  - **Capacity Quadrupled:** **12–15 pots** now fit on the screen simultaneously (was 3–4).
  - **Top Bar Layout Fix:** Y anchors adjusted so hint texts and the buttons (*Water All* / *Auto*) no longer overlap.

## 0.3.1
- **Collapsible Property Groups (Accordion UI):**
  - Pots in the PhoneApp are now displayed in collapsible groups per property.
  - Each property gets a clickable header row with arrow indicator (`▼` open / `▶` closed) and a compact summary (`e.g. 5 pots · ★3 ready · 💧1 dry`).
  - The collapsed state (`_expandedProperties`) persists across automatic 2s scans.
  - On first start all properties are auto-expanded.

## 0.3.0
- **Auto-Water Feature:** When enabled, dry owned pots are automatically watered every in-game minute (Harmony patch on `GrowContainer.OnMinPass`). Same logic as manual Water-All: 50g per pot, skip threshold 30% (pots that are already moist enough are skipped).
  - **Toggle in the PhoneApp:** Right next to the Water-All button. Label "🤖 Auto: ON" (green) / "🤖 Auto: OFF" (neutral). Click → flips state + persists immediately via `S1Mods.Shared.ModConfig<PotScannerConfig>.SetAndSave`.
  - **Persistence:** `ModConfig<PotScannerConfig>` with `PotScannerConfig.AutoWaterEnabled` (bool, default false). Loaded at game start via `OnInitializeMelon`.
  - **Hot-React:** `ModConfig<PotScannerConfig>.OnChanged` event subscribed — UI toggle reacts to runtime changes (console / future settings UI).
  - **`AutoWaterService.cs` (NEW, 4.9 KB):**
    - Harmony patch with `com.dominik.potscanner.autowater` ID. `[HarmonyPatch(typeof(GrowContainer), nameof(GrowContainer.OnMinPass))]` + `[HarmonyPrefix]`. Returns `true` (native drain continues — growth/soil tick unchanged).
    - `OnMinPassPrefix`:
      1. Feature-off check (early return)
      2. `TryFindPotInfoForContainer` — `PotTracker.Pots` cache via `NativePtr` (no extra full scan)
      3. `IsOwnedProperty` check
      4. Skip-threshold check (`WaterPercent < 30%`)
      5. Cash cap (`money.cashBalance < 50` → silently skip, no negative balance)
      6. `SetMoistureAmount(capacity)` + `ChangeCashBalance(-50, false, false)` (no visualChange, no sound = no spam)
    - **Throttled Logging:** max 10 logs per session, then silent (logs are only for dev debugging; the user sees the effect in the app anyway).
    - `Initialize()`/`Shutdown()` symmetry: Harmony is patched in `OnInitializeMelon`, unpatched in `OnApplicationQuit`.
  - **`PotScannerConfig.cs` (NEW, 514 B):** `class PotScannerConfig { public bool AutoWaterEnabled { get; set; } = false; }`. Auto-discovered via `ModConfig<T>.GetProperties()` reflection.
  - **PhoneApp UI:** `UIFactory.RoundedButtonWithLabel` for the toggle button (anchor 0.53-0.96, 0.83-0.88), `ButtonUtils.AddListener` for IL2CPP-safe click handling.
  - **Defensive:** `container.Equals(null)` check, `Pointer` try/catch, `MoneyManager.Instance == null` check. Pattern-equivalent to existing `PotTracker.Refresh()`.
  - **Performance:** Harmony patch runs 1× per pot per in-game minute. Prefix check is 1× bool read when disabled → ~0 overhead. When enabled + owned + thirsty: 1× dictionary lookup + 1× SetMoistureAmount + 1× ChangeCashBalance (all O(1)).
  - **NOT Included:** Console command for the toggle (follows if requested). Configurable skip threshold (also follow-up).

## 0.2.2
- **Smart Skip Threshold (User-Requested):** Pots with `WaterPercent >= 30%` are skipped during Water-All — not watered, not billed. "Don't pay for nothing" UX.
  - `Constants.WaterAllSkipThreshold = 0.30f` (configurable in a future version via MelonPreferences).
  - `WaterAllService.WaterAll()`: additional filter in the TARGET LIST (step 4) — only pots with `WaterPercent < threshold` are watered.
  - `WaterAllService.GetButtonState()`: returns `thirstyCount` (pots that would be watered), not `ownedCount`.
  - Button disabled when 0 thirsty pots: "All sufficiently watered".
  - Result message includes the skip count: "5/5 pots watered for 250g (3 already watered, skipped)".

## 0.2.1
- **S1API `GameLifecycle` Hooks** replace the v0.2.0 retry workaround:
  - `GameLifecycle.OnSaveInfoLoaded` → `PotTracker.OnSaveInfoLoaded()` → `RefreshPropertyCache()`. Fires AFTER save-info load, BEFORE scene load — `Property.OwnedProperties` is then populated.
  - `GameLifecycle.OnLoadComplete` → `PotTracker.RefreshNow()` for an immediate UI update.
- **Cleaned Up:** v0.2.0's 25s retry mechanism (`_propertyRetryStartTime`, `_propertyRetryIndex`, `_propertyRetryDelays`, `Tick()` loop), Plan-C IL2CPP reflection fallback and debug logging completely removed.
- **Net:** ~60 lines of code less, cleaner lifecycle, no more "wait until Property.OwnedProperties is populated" hack.

## 0.2.0
- **Water All Button (v0.2.0 main feature):** One-click watering of all owned pots in the PhoneApp.
  - Button in the TopBar area, label `💧 Water All (Xg)`, X = `ownedPotCount × 50g`.
  - **10-step validate-then-charge sequence** in `WaterAllService.WaterAll()`:
    1. `isWatering` lock (prevents double-click)
    2. Snapshot of the pot list (protection against parallel refresh)
    3. One full scan + `Dictionary<IntPtr, GrowContainer>` (O(1) lookups)
    4. Target list with `IsOwnedProperty` filter (foreign pots excluded)
    5. Validate (count > 0, sufficient cash)
    6. **Charge** (`MoneyManager.Instance.ChangeCashBalance(-totalCost, visualizeChange:true, playCashSound:false)`)
    7. `SetMoistureAmount(capacity)` per pot, per-pot try/catch
    8. `PotTracker.Instance.RefreshNow()` for an immediate UI update
    9. Return `WaterAllResult` record
    10. Release the lock in `finally`
  - **Edge Cases:** Button disabled when 0 owned pots / insufficient cash / mid-action, with tooltip text via label.
  - **Multiplayer:** out of scope (uses local `SetMoistureAmount`, no server hook).
- **`IsOwnedProperty` flag in `PotInfo`** (Step 1): Strict owned-filter for Water-All, independent of the Owned→All display fallback. Separate `HashSet<string> _ownedPropertyCodes` in PotTracker, populated from `PropertyManager.GetOwnedProperties()`.
- **`PotTracker.RefreshNow()`** (Step 2): Public method for an immediate scan + `OnPotsScanned` fire. Bypasses the 2s polling delay, so the UI updates in the same frame after a click.
- **Custom App Icon (Side Fix):** Programmatic 64×64 sprite (white droplet on blue background) via `IconSprite` override. Prevents `[PhoneApp] Icon file not found` error from S1API when `IconFileName = ""`.
- **Constants:** `WaterAllCostPerPot = 50f` (float, matched `MoneyManager.cashBalance`).
- **Pre-implementation Verified** (11 ilspycmd checks): `SetMoistureAmount` / `ChangeCashBalance` / `MoneyManager.Instance` / `Property.IsOwned` etc. Details: `Knowledge/decompile-tmp/pot-scanner-water-all/verification-report.md`.
- **Save-Load Timing Fix (real-world experience):** `Property.OwnedProperties` is still empty at the `OnGameplaySceneLoaded` event (3s save-load delay). S1API's `GetOwnedProperties()` and `PropertyWrapper.IsOwned` reflect this empty state. **Solution:** Retry mechanism in `PotTracker.Tick()` (delays 3s/6s/12s/25s) refreshes the `PropertyCache` as soon as the list is populated. Plan-C IL2CPP reflection fallback as defense-in-depth. **Verified:** Retry at +3s found 12 owned, button state activated, click → 5/5 pots watered for 250g.
- **Spec:** `Knowledge/plans/pot-scanner-water-all-spec.md` (13.5 KB).
- **Plan:** `Knowledge/plans/pot-scanner-water-all-impl-plan.md` (11.4 KB).

## 0.1.1
- **Scene Symmetry Fix (User Code Review):** `OnSceneWasUnloaded` did not check `sceneName`, while `OnSceneWasLoaded` did. An additive scene unload (menu, interior, UI overlay) could wrongly deactivate the tracker, even though the gameplay scene continued to run.
  - `IsGameplayScene(string)` as a central helper in `Mod.cs` — both load and unload use the same predicate.
  - `OnSceneUnloaded` calls `PotTracker.OnSceneUnloaded()` only when `IsGameplayScene(sceneName)`.
  - `OnGameplaySceneLoaded`/`OnSceneUnloaded` now both reset `_elapsed = 0f` (no stale timer state after re-activation).
  - `OnSceneUnloaded` is defensive (early return when `!_active`) and calls `NotifyPotsScanned()` at the end, so open PhoneApps clear immediately instead of waiting for the next 2s scan.
  - `NotifyPotsScanned()` extracted as a helper; the try/catch in `Refresh()` replaced.
  - Lifecycle logs in both callbacks (`Scene loaded/unloaded: 'Name' (#idx)`).

## 0.1.0
- Initial version.
- **PhoneApp:** Scrollable pot list, grouped by property (Owned→All fallback), sorted by growth (ready-soonest first). Colour coding W<20% red, G≥80% green, status badges (READY/ALMOST/GROWING).
- **HudOverlay (F8-Toggle):** IMGUI overlay top-left, per-frame, shows nearby pots sorted by distance.
- **Polling:** 2s interval via `PotTracker` singleton. `FindObjectsByType<GrowContainer>(FindObjectsSortMode.None)` + `GetComponentInChildren<Plant>()`.
- **Verified in-game 2026-08-04:** 5 pots in the motel room detected (3× OG Kush ready, 2× empty), property lookup correct, distances accurate.
- **Open for 0.2.0:** Quality in the UI, MinPass Harmony patch, saveable last-seen tracking, custom icon.
