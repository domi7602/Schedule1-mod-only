# Buildable restore and duplicate prevention

> verified: 2026-08-22 (AutoPackagingStation duplicate-station bug); cross-checked against `common-errors.md` §20 2026-10-05.
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 11 of 11 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

Inherited source notes; not revalidated against the installed game in this package.
Historical case: AutoPackagingStation duplicated stations after save/load because
both native persistence and mod restoration created the same placed object.

## Assign exactly one restoration owner

| Object category | Restoration policy |
|---|---|
| Native-persisted registered BuildableItem | Let native persistence recreate it; attach mod data to that instance |
| Pure mod-owned object with confirmed no native persistence | Recreate through the mod's validated restore path |
| Unclear native/network restore behavior | Determine ownership first; do not optimistically run both paths |

Registration or FishNet spawning alone does not prove persistence behavior.
Test native save and reload with the mod's spawn path disabled to establish what
already returns, using stable identity and object counts.

## Restore metadata without spawning duplicates

1. Read and validate the saved mod metadata under the confirmed world identity.
2. Index candidate metadata by the object's persistent native GUID, or another
   identifier proven stable across that object's native save cycle.
3. Reconcile already restored objects and objects restored later. Apply metadata
   immediately to existing instances; retain bounded staged metadata for later ones.
4. Attach through a verified native lifecycle hook and make attachment idempotent
   for object identity plus load generation. Do not assume Start always occurs
   after the mod's load-complete handler.
5. Report unmatched or duplicate identities. Do not silently spawn a replacement
   for an object whose native restore outcome is still unknown.

Do not generate a new persistent GUID during every Awake or scene load. Runtime
instance IDs and addresses are unsuitable as cross-session persistence keys.

## Cleanup and save coordination

Clear the active-world registry on a confirmed world transition such as the
verified preload path. Do not clear it for every unrelated scene unload.
Destroy only mod-owned clones and detach only mod-owned components/subscriptions.
Retain pending writes separately from scene references.

For objects exchanged with native inventory, coordinate placement, pack-up, and
save checkpoints. Immediate sidecar persistence while native inventory remains
unsaved can create rollback duplication or loss. Deferring to a save callback is
necessary in some designs but is not by itself a cross-file transaction.

## Verification

Place one object, save, reload twice, and assert one native instance plus one mod
attachment each time. Test metadata loaded before and after native object creation,
missing metadata, duplicate GUIDs, corrupted sidecar, slot switch, unrelated scene
unload, pack-up followed by rollback, and host/client loading.

Read [slot identity](slot-isolation.md), [atomic storage](safestorage-atomic.md), and
[lifecycle verification](../../schedule1-lifecycle-verify/SKILL.md) when those
boundaries are uncertain. Historical implementation anchors belong in the owning
repository; do not assume the archived HomelessMod uses current native behavior.
