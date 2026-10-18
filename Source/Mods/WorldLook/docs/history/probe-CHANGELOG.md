# Changelog

## 0.1.3 (2026-10-16)
- Evidence slice for the golden-hour atmosphere work (docs/graphics-golden-hour-plan.md): enumerate the skybox material's shader properties (name, kind and current value per read strategy; 64-property prefix cap with explicit truncation) and record sun light details (type, color, intensity, shadows, shadowStrength) as point-in-time observations.
- Add an in-game time anchor (TimeManager current time, current day, elapsed days, display string) guarded by NetworkGuard.IsAlive so light/sky observations can be correlated with the day/night cycle; unavailable in menus without blocking capture.
- New Unity-free linked IntrospectionPolicy: shader-property kind mapping to read strategies (color/vector/float/texture/unknown), bounded prefix planning and non-finite normalization (a single non-finite component rejects the whole tuple; JSON validity itself is guaranteed by the report's finite converters). 21 new RED→GREEN policy and source-contract tests (80 total in GraphicsProbe.Tests).
- Independent scoped pre-deploy review (2026-10-16, verdict PASS-with-nits, no P1) fixed through before deployment: source contracts pin the capped/gated/error-isolated introspection and the guarded TimeManager reads; per-property error isolation keeps one bad shader property from nulling the whole list; the 64-property cap is pinned by test.
- Scope bound: volume profile components/weights are already covered by 0.1.2 reports; per-parameter value introspection is deliberately out of this slice. No render, scene or save-state mutations.

## 0.1.2 (2026-10-16)
- Repair early-hierarchy starvation with bounded, resumable root-first breadth traversal: scene-round-robin admitted roots, one child attempt per parent turn, identity deduplication and explicit frontier/omission accounting.
- Collect only requested captures across frames using 2 ms cooperative slice targets, 128 steps/update, 1,500 ms active budget and a 30 s wall deadline; log separate synchronous finalization cost. Targets cannot preempt slow native calls or prove hitch-free behavior.
- Cancel/discard in-flight capture on scene load/unload/initialization and deinitialization; release native-reference queues/arrays before rescheduling. Preserve the prior report on collection failure/cancellation.
- Separate particle/line/trail lexical matches from possible surface matches: independent scan budgets 512/19,488 (20,000 total) and retained capacities 32/128. Neither category confirms actual water.
- Read one early, deduplicated Camera.main hint with explicit provenance; retain inactive/disabled scene-camera discovery and never infer gameplay-camera identity from the tag.
- Schema 2 timestamps a multi-frame observation window and exposes active/wall/step/frame timing, root admissions/visits, pending work, renderer types and omissions. Independent review found and reproduced two P2 failure paths before deployment: stale component bounds after an unreadable array and a falsely complete empty scene subsection after count failure. Fixed both with assertion RED→GREEN tests executing the actual collector against managed adapters; successful zero scenes are tested separately. 59 policy/source/managed-adapter tests pass; deployed 2026-10-16 after a PASS delta re-review with verified install hashes; native camera/water coverage and actual timing remain In-Game-Verify open.

## 0.1.1 (2026-10-15)
- Correct top-level scene-discovery exception reporting: preserve field-level evidence and mark coverage incomplete through the bounded discovery error accounting, rather than emitting a misleading untruncated snapshot.
- Keep startup/report version diagnostics derived from the mod identity instead of stale duplicated literals.
- Runtime inspection 2026-10-16: reviewed DLL loaded; manual JSON reports and prior backup observed; active URP/URP_3Ultra confirmed on 0.4.7f12. Gameplay discovery exhausts 50,000 nodes in early hierarchies, misses cameras and retains particle false positives rather than confirmed water. About 1.04 seconds collection time; full In-Game-Verify remains open. No code or rendering changes in this inspection; see verification.md.
- Independent review identified this static failure path before deployment; linked-policy regressions and a delta re-review are required. Native behavior remains In-Game-Verify open.

## 0.1.0
- Initial experimental read-only rendering snapshot: delayed/coalesced startup, scene and Ctrl+Shift+F8 requests; cancellation on shutdown and retry after failure.
- Bounded loaded-scene traversal including inactive descendants, camera/pipeline/environment descriptors and renderer counts with unconfirmed lexical water candidates only.
- SafeStorage latest.json/backup output, finite JSON numbers, explicit scope/truncation/errors and no render/scene/save-state mutations.
- Unity-free linked policy tests and supplementary source contracts observed RED to GREEN; compile-only Release gate. In-Game-Verify remains open; no deployment/gameplay test in this implementation slice.
