# S1API money, time, property, and law

> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 21 of 21 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

Source scope: official online API pages reviewed on 2026-10-21. These findings
must be matched to the exact reference/deployed S1API assembly before use.

## Money surface

The official [Money reference](https://ifbars.github.io/S1API/api/S1API.Money.Money.html)
uses `GetCashBalance`, `GetOnlineBalance`, and `GetNetWorth` methods, and exposes
`OnBalanceChanged`. Do not assume same-named balance properties from an older sample.
The documented cash mutation accepts amount, visualization, and sound options.
The online transaction's third parameter is quantity, not days; its amount is
expressed per unit. Verify provider effects and return contracts independently.

Use [economy operations](../../schedule1-economy/SKILL.md) for settlement. Cash and
bank calls are separate effects. Never catch both transfer legs and unconditionally
refund: the first leg may have failed before mutation, or a later exception may
have occurred after a successful mutation. Track confirmed, partial, and unresolved
effects and settle only the proven outstanding amount.

Own balance subscriptions by app/mod lifetime. Close/reopen should not permanently
remove a listener that is only installed at creation. Destruction/world transition
must detach the exact prior instance's handler.

## Time representation

The official [TimeManager reference](https://ifbars.github.io/S1API/api/S1API.GameTime.TimeManager.html)
defines `CurrentTime` as 24-hour HHMM. A value such as 1330 represents 13:30;
12-hour AM/PM is a formatting choice. The reference provides conversions between
minutes and HHMM and a separate formatted-time method.

For validated HHMM, integer division by 100 extracts the hour. Do not subtract
HHMM values to obtain elapsed minutes; convert to minutes and account for day
boundaries. Clarify whether a range crosses midnight and how the game's day/sleep
boundary affects the feature. Verify SetTime authority and network effects before
using it for testing or gameplay changes.

The documented time callbacks include delegate fields as well as other surfaces;
inspect member kind and delegate signature instead of assuming every callback is
a C# event. Unsubscribe only your delegate; do not replace a shared callback field.

## Property and law research

For properties, establish the actual ownership, business/property distinction,
persistent identifier, and readiness before changing or displaying ownership.
For law, determine which native/wrapper state drives pursuit, heat, wanted display,
and consequences. Do not infer a universal 0-to-5 wanted scale or fabricate a
GetCurrentWantedLevel helper from the UI concept.

Locate the supported member in the target source/API index, then trace its native
owner and authority if necessary. Use the relevant
[game-system guide](../../schedule1-game-systems/SKILL.md) as a research map;
its inherited signatures still require exact-target verification.

## Verification

Test money failure before/after mutation, repeated subscriptions, and world switch.
For time test midnight, noon, minute rollover, the game's sleep/day boundary, and
cross-midnight intervals. For property/law test readiness and authority on every
peer the mod supports. Distinguish formatting tests from native state-change tests.
