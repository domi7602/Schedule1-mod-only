# Select a mod architecture

> verified: patterns established 2026-08-20 → 2026-09-11; §4 lifecycle advice re-verified 2026-09-29 (OnSaveInfoLoaded = 0 firings in the instrumented session); Pattern-1 skeleton modernized to the Shared UITheme API 2026-10-05.
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 45 of 45 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

These are reusable architectures, not compiled templates. Named mods below are historical lookup leads from the uploaded workspace; verify their current paths/status and implementation before reuse.

| Pattern | Choose it for | Required boundaries | Historical source leads |
|---|---|---|---|
| Phone application | User-driven interaction inside the phone | Domain state, UI instance lifetime, focus ownership, persistence | NotesApp, CalculatorApp, BankApp, PocketShop, PotScanner |
| HUD overlay | Continuous presentation of small status data | Cached snapshot, bounded refresh, layout/configuration, teardown | Minimap, DayCounter, ProfitTracker |
| Polling service | Data without reliable push notifications | Bounded cadence, incremental work, invalidation, diagnostics | PotScanner, product scanners |
| Native save integration | State with an established framework save owner | Stable identity, serialization contract, load order, migration | Saveable-based project implementations |
| Sidecar persistence | State requiring an independently owned file | Confirmed save binding, read status, atomic write, recovery | CalculatorApp, BankApp |
| Harmony patch | A verified native behavior needs interception | Exact target, ownership gate, result/restoration, lifecycle | TaxiDriver, CustomSkateboard, MoreSaveSlots |
| Configuration service | User-tunable global preferences | Validation, last-known-good state, actual serializer contract | ModConfig users in Shared-dependent mods |
| Diagnostic command | Targeted readback or controlled test operation | Exact registration API, arguments, bounded work, authorization | Existing S1API console command implementations |

## Combine deliberately

A phone app may consume a polling service and a persistence service. A world extension may need authoritative state, staged construction and local visual resources. Give each responsibility one owner; do not attach independent save writers to every view.

For an initial mod, use only the boundaries needed by the current feature. Do not generate a large service framework from this table.

## Lookup procedure

1. Identify the desired observable behavior and lifecycle.
2. Find a live implementation with similar semantics.
3. Read its dependencies and failure paths, not just its class names.
4. Compare the intended multiplayer and save identity assumptions.
5. Extract the general mechanism; do not copy project-specific IDs, paths or timing constants without evidence.

## Load for the chosen responsibility

- Phone UI: [PhoneApp](../../schedule1-phoneapp/SKILL.md).
- Storage: [Persistence](../../schedule1-persistence/SKILL.md).
- Purchases/income: [Economy](../../schedule1-economy/SKILL.md).
- Owned Unity resources: [Runtime cache](../../schedule1-runtime-unity-cache/SKILL.md); file-change scheduling: [reload](../../schedule1-debounced-reload/SKILL.md). Polling alone does not require either mechanism.
- World additions: [World expansion](../../schedule1-world-expansion/SKILL.md); explicitly requested multiplayer: [networked state](../../schedule1-networked-world-state/SKILL.md).

## Verify the chosen design

A reviewer should be able to locate the entry point, state owner, external API boundary, save destination, cleanup path and observable acceptance result. Large amounts of boilerplate do not establish those properties.
