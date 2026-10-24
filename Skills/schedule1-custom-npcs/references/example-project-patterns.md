# Custom NPC Implementation Patterns

> verified: patterns consolidated 2026-10-05 (duplicates moved to canonical reference).
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 22 of 22 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

## Evidence and use

These patterns retain the original workspace's role separation while removing unverified complete
code examples. Read [API and lifecycle reference](s1api-custom-npc-reference.md) for current evidence,
member lookup anchors, and phase boundaries. Each recipe is an implementation contract to bind to the
installed API; it does not introduce replacement methods.

## Physical customer

Declare stable physical/customer roles. In prefab configuration, assign stable identity, appearance,
spawn position, customer defaults, relationship policy, and a route with the required deal-handling
support. Make the customer's spending, order frequency, quality standards, affinities, and unlock
conditions coherent with the intended progression.

After the framework creates the instance, perform its supported appearance finalization, register
owned dialogue/event handlers, and enable the ready schedule. Test a real purchase/contract, not only
presence in the world. Route completion, networking, and save restoration are separate checks.

## Event-driven customer

Keep persistent defaults out of event handlers. Store the actual publisher and removable delegate,
subscribe once per instance, and release on final teardown. A reward or message caused by an event
needs its own idempotency/persistence rule when duplicate delivery or reload is possible.

When a publisher changes on load, remove the old subscription before binding the new one. Do not use
subtract/add against the current publisher as proof that all old bindings were removed.

## Dealer

Declare a stable dealer role and configure its signing fee, cut, dealer type, valid home reference,
quality policy, and any completed-deal variable before native registration. Add the target runtime's
required contract/deal signal to the schedule; use an explicit handling slot only when the intended
behavior needs one.

Wire recruitment, contract acceptance, and recommendation behavior through verified events after the
dealer component exists. Test recruitment cost, actual delivery/contract handling, repeated callbacks,
late-joining clients, and reload. Do not infer function from a visible dealer badge alone.

## Non-physical story contact

Use the non-physical role for a phone/message-only contact. Provide a stable identity and an intentional
icon/fallback. Omit world movement and schedule setup when the API's non-physical path does not need it.
Clear conversation categories only if the role should have no customer/dealer/supplier badge.

Gate an introduction with saved progress or an idempotent message identity. Sending it unconditionally
from `OnCreated` can repeat it on reload. Restore callbacks for saved response choices using the
supported load hook, and validate missing or changed response IDs. If `SaveableField` is used, apply
it to fields and verify the installed serializer contract.

## NPC as a UI entry point

Register the dialogue route once for the instance. On the relevant choice, validate the service
preconditions, close/exit dialogue through a supported path, and open the UI after camera and input
ownership have transferred. A node-displayed callback that repeatedly stops its own override can
reenter dialogue code; use the supported choice/close transition instead.

The UI owns its own close/focus/cleanup path. Destroying or unloading the NPC must not leave its panel,
camera override, cursor state, or global input gate active. Read the PhoneApp/interior input references
when the service uses those systems.

## Authored narrative NPC

Keep route points and authored IDs centralized, with verified world-space locations. Match schedule,
dialogue, clothing, customer/dealer policy, and progression to the character's role. Avoid two competing
appearance definitions or two lifecycle owners for one NPC. Use namespaced IDs for dialogue and saved
state so another NPC cannot accidentally share them.

## Conditional presence

Distinguish presence from accessibility. An early return from `OnCreated` does **not** guarantee that
the framework has not already spawned/registered the physical NPC.

If the NPC should exist but be locked, prefer supported relationship, dialogue, or unlock conditions.
If the feature truly requires absence, inspect the framework's supported pre-spawn/presence lifecycle
and persistence/network consequences, then implement an explicit authoritative transition. If no
supported path is established, report that limit and use a reviewable gating design; do not leave a
partially initialized registered NPC or destroy it opportunistically.

For state changes during play, define what happens to active dialogue, scheduled actions, messages,
saved IDs, and client views before changing presence. Repeated activation must not duplicate the NPC.

## Project structure and implementation order

Keep a focused class/file per role where that matches the repository. Split large role implementations
into appearance, schedule, dialogue, economy, and lifecycle helpers without losing a single owner for
registration and cleanup. Do not move every role into a global entrypoint or scatter one role's state
across unrelated services.

1. Identify physical/contact role, stable identity, authority, and persistence ownership.
2. Verify required installed API members and native prerequisites.
3. Implement prefab configuration with coherent defaults.
4. Implement runtime wiring and cleanup together.
5. Implement load restoration and idempotency before adding one-time rewards/messages.
6. Validate the role's actual gameplay behavior, then reload and host/client behavior.

A compile result proves the referenced surface; a runtime test proves only the exercised scenario.
Report both with versions and leave untested paths explicit.
