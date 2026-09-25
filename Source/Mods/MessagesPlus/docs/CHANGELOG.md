# Changelog

## 0.1.1
- Review fixes (behavior-preserving hardening; no feature changes).
- Fixed a skip bug in the purge re-apply (`ApplyToConversations`): list removal during forward iteration skipped adjacent purged threads, leaving them visible in the inbox. Both `Conversations` and `ActiveConversations` are now walked backwards.
- The trash UI now subscribes to `TrashService.OnTrashChanged` (once, static guard): Clear All / Restore / Empty trash / slot load immediately rebuild the "Trash (N)" row list instead of showing stale rows.
- Confirmation dialog: the modal root is part of the UI liveness check (dead buttons self-heal on the next open/tick), a no-op button on the dialog card stops backdrop click-through, and the dialog is reset on app close (no stale pending action reappears).
- Rebuild path destroys our own managed UI roots before re-injecting (no orphaned/duplicate toolbar after a mid-build failure).
- Purged threads are re-suppressed from the throttled tick, so a purged thread that receives a new message cannot resurrect its inbox entry (in-game test note in the README).
- Matching hardening: name-only fallback is restricted to records without a captured index (no cross-matching of duplicate contact names), and restore removes records by identity match instead of reference equality.
- Invisible panels/labels no longer swallow clicks meant for the vanilla UI (`raycastTarget` disabled on non-interactive graphics).
- Multiplayer clients now get disabled mutation buttons (read-only trash) instead of buttons that silently no-op.
- Slot probe fallback logs a warning (stale-slot-file risk is visible in the log).
- UI strings unified in English ("Trash (N)", "Empty trash").

## 0.1.0
- Initial release (Phase 1: Clear All + Trash/Restore).
- Clear All button injected into the vanilla MessagesApp toolbar with a confirmation popup; all visible conversations move to the trash (hidden via `MSGConversation.SetEntryVisibility(false)`).
- Collapsible "Trash (N)" trash section below the conversation list with per-thread restore buttons (`SetEntryVisibility(true)` + `MoveToTop()` + `RepositionEntries()`).
- "Empty trash" permanent delete with a second confirmation; purged threads are re-removed after every save load.
- Slot-isolated persistence via `SafeStorage.SaveAtomic` in `trash_slot_{n}.json` (triple-guarded slot suffix, never `slot_-1`).
- Host-only mutations for multiplayer (clients get a read-only trash); graceful Harmony degradation via `PatchGuard`.
