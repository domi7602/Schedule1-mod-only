# Changelog

## 0.1.0
- Initial release (Phase 1: Clear All + Trash/Restore).
- Clear All button injected into the vanilla MessagesApp toolbar with a confirmation popup; all visible conversations move to the trash (hidden via `MSGConversation.SetEntryVisibility(false)`).
- Collapsible "Papierkorb (N)" trash section below the conversation list with per-thread restore buttons (`SetEntryVisibility(true)` + `MoveToTop()` + `RepositionEntries()`).
- "Trash leeren" permanent delete with a second confirmation; purged threads are re-removed after every save load.
- Slot-isolated persistence via `SafeStorage.SaveAtomic` in `trash_slot_{n}.json` (triple-guarded slot suffix, never `slot_-1`).
- Host-only mutations for multiplayer (clients get a read-only trash); graceful Harmony degradation via `PatchGuard`.
