# S1API quests, identity, and restoration

> **Canonical home of the Quest-Stale-Reference gotcha** (§ gotchas) — other files reference here. verified: gotchas empirical; API surface vs source 2026-10-05.
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 20 of 20 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

Source scope: official online documentation reviewed on 2026-10-21. Reconcile the
published API with the exact S1API 3.2.1 reference/deployed assembly before coding. <!-- skills-health: version-evidence (S1API target requested by the owner; installed build state lives in AGENTS.md) -->

## Resolve the correct quest system

The official [QuestManager reference](https://ifbars.github.io/S1API/api/S1API.Quests.QuestManager.html)
distinguishes custom-quest lookup from base-game quest access. It documents
`CreateQuest`, `GetQuestByGuid`, and `GetQuestByName`; the name lookup is described
for custom mod quests. Do not substitute the old unverified `GetQuestByID` name or
assume one lookup covers every vanilla and mod quest.

The [Quest reference](https://ifbars.github.io/S1API/api/S1API.Quests.Quest.html)
derives from Saveable and exposes its own entry collection and state contracts.
Read the actual property/field names, enum values, callback signatures, and AddEntry
overloads. Do not invent an `EQuestState` enum or reuse native enum names as wrapper
names merely because their concepts are similar.

## Creation and stable identity

Define a persistent mod quest identity and an idempotent creation policy. Determine
which quests the framework restores automatically and when restoration is complete
before deciding to create a replacement. A missing lookup during load is not proof
that the quest never existed or should be recreated.

For entries use a stable domain mapping under your control. Avoid index-based
assumptions when entry order may change, but do not replace them with unqualified
localized-title matching either. If only titles are available, document uniqueness,
localization, and migration constraints and validate matches explicitly.

## Reload and completion

Reacquire current quest and entry wrappers after restore; cached references can
belong to the previous world generation. Restore subscriptions and UI against the
new instances, and remove the previous instance's handlers.

The inherited notes report completed-quest lookup gaps in one workspace. Treat
this as a test case, not a universal claim that S1API cannot persist completion.
Distinguish not loaded, not found, completed, failed, and incompatible state.
If an independent completion store is needed, bind it to the same world/checkpoint
and preserve an explicit once-only reward state. Do not award again merely because
a callback repeats or a UI lookup returns null.

## Rewards and save data

Use [Saveable fields](saveables.md) for supported framework persistence. For money
or item rewards, use the [economy workflow](../../schedule1-economy/SKILL.md): latch
an attempt, establish actual effects, and retain unresolved outcomes. Setting a
quest complete and paying a reward are separate effects unless the native contract
proves a common transaction.

Keep notifications outside reward retry decisions. Preserve user cancellation or
accepted intent separately from native objective observations when rollback can
change those observations.

## Verification

Test new and existing saves, incomplete/completed/failed quests, repeated completion
callbacks, missing lookup during load, save rollback, changed entry order, localized
titles, and world switch. Verify one quest instance, correct entry mapping, and
exactly the intended retained reward. Check the production serializer's actual
saved content, not only the quest list displayed in the UI.
