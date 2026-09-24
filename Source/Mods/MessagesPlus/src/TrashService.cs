using System;
using System.Collections.Generic;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppScheduleOne.Messaging;
using Il2CppScheduleOne.UI.Phone.Messages;
using S1Mods.Shared;

namespace MessagesPlus;

/// <summary>
/// Identity record for one trashed/purged conversation. Three keys because the
/// vanilla game offers no guaranteed stable handle across sessions:
/// <see cref="Id"/> (MSGConversation.ConversationId, else SaveFileName — best
/// effort), <see cref="Index"/> + <see cref="ContactName"/> as the reliable
/// fallback pair.
/// </summary>
public sealed class TrashEntry
{
    /// <summary>ConversationId, else SaveFileName, else a synthetic "idx:{Index}|{ContactName}" key.</summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>Contact display name at the time of the mutation (secondary key).</summary>
    public string ContactName { get; set; } = string.Empty;

    /// <summary>Conversation index at the time of the mutation; -1 when unknown.</summary>
    public int Index { get; set; } = -1;
}

/// <summary>
/// Persisted trash document (one per save slot). <see cref="Purged"/> keeps
/// permanently deleted threads suppressed across game reloads (see
/// <see cref="TrashService.EmptyTrash"/> for why the native save entry is kept).
/// </summary>
public sealed class TrashState
{
    public List<TrashEntry> Trashed { get; set; } = new();
    public List<TrashEntry> Purged { get; set; } = new();
}

/// <summary>
/// Core trash logic for the vanilla MessagesApp: move threads to trash (hidden
/// via MSGConversation.SetEntryVisibility(false)), restore them, permanently
/// empty the trash, and slot-isolated persistence via SafeStorage
/// (trash_slot_{n}.json, never slot_-1 — triple-guarded slot suffix, Rule 11).
///
/// Multiplayer: all mutations are host-only (NetworkGuard); clients get a
/// read-only trash view.
/// </summary>
public static class TrashService
{
    private static ModLogger? _log;
    private static TrashState _state = new();
    private static string _lastKnownSlot = "default";

    /// <summary>Raised after every trash mutation (static dispatcher — no subscriber leaks).</summary>
    public static event Action? OnTrashChanged;

    /// <summary>Wires the logger (call once from <c>Mod.OnInitializeMelon</c>).</summary>
    public static void Initialize(ModLogger log)
    {
        _log = log;
    }

    // ------------------------------------------------------------------
    // Persistence (slot-isolated, Rule 11)
    // ------------------------------------------------------------------

    /// <summary>
    /// Slot suffix "slot_{n}" with last-known fallback ("default"). The
    /// SaveSlots probe already refuses SaveSlotNumber &lt; 0, and the extra
    /// &gt;= 0 guard here guarantees no trash_slot_-1.json can ever be written.
    /// A transient slot &lt; 0 (e.g. right after a slot switch) falls back to the
    /// last known suffix and logs a warning — the stale-suffix risk is thereby
    /// visible in the log instead of silent.
    /// </summary>
    private static string GetSlotSuffix()
    {
        try
        {
            int slot = SaveSlots.GetActiveSlotNumber();
            if (slot >= 0)
            {
                _lastKnownSlot = $"slot_{slot}";
            }
            else
            {
                _log?.Warn($"Trash slot probe returned {slot} — writing to last-known file '{_lastKnownSlot}' (stale-slot risk after a slot switch).");
            }
        }
        catch (Exception ex)
        {
            // Keep the last known suffix (never fall back to slot_-1).
            _log?.Warn($"Trash slot probe failed ({ex.Message}) — writing to last-known file '{_lastKnownSlot}'.");
        }
        return _lastKnownSlot;
    }

    public static string GetTrashFilePath() =>
        SafeStorage.GetUserDataPath("MessagesPlus", $"trash_{GetSlotSuffix()}.json");

    /// <summary>
    /// Loads the trash document for the active slot (call on OnSaveInfoLoaded).
    /// Fires <see cref="OnTrashChanged"/> so the trash UI rebuilds its rows from
    /// the freshly loaded state (stale rows from the previous slot must not survive).
    /// </summary>
    public static void LoadForCurrentSlot()
    {
        try
        {
            string path = GetTrashFilePath();
            _state = SafeStorage.LoadSafe(path, new TrashState(), _log) ?? new TrashState();
            _state.Trashed ??= new List<TrashEntry>();
            _state.Purged ??= new List<TrashEntry>();
            _log?.Info($"Trash loaded: {_state.Trashed.Count} trashed, {_state.Purged.Count} purged ({System.IO.Path.GetFileName(path)}).");
        }
        catch (Exception ex)
        {
            _log?.Error($"LoadForCurrentSlot failed: {ex.Message}");
            _state = new TrashState();
        }
        finally
        {
            OnTrashChanged?.Invoke();
        }
    }

    private static void Save()
    {
        try
        {
            // GetTrashFilePath logs a warning when the slot probe returns < 0.
            SafeStorage.SaveAtomic(GetTrashFilePath(), _state, _log);
        }
        catch (Exception ex)
        {
            _log?.Error($"Trash save failed: {ex.Message}");
        }
    }

    // ------------------------------------------------------------------
    // Read model
    // ------------------------------------------------------------------

    /// <summary>Trashed (hidden, restorable) entries for the active slot.</summary>
    public static IReadOnlyList<TrashEntry> Trashed => _state.Trashed;

    /// <summary>Number of trashed entries (header counter).</summary>
    public static int TrashedCount => _state.Trashed.Count;

    /// <summary>True when the conversation is currently in the trash (hidden but restorable).</summary>
    public static bool IsTrashed(MSGConversation? conv) => FindMatch(_state.Trashed, conv) != null;

    /// <summary>True when the conversation was permanently deleted (kept out of the app lists).</summary>
    public static bool IsPurged(MSGConversation? conv) => FindMatch(_state.Purged, conv) != null;

    // ------------------------------------------------------------------
    // Mutations (host-only)
    // ------------------------------------------------------------------

    /// <summary>
    /// Moves every visible conversation into the trash (hides its entry via
    /// SetEntryVisibility(false)). Purged threads are skipped.
    /// </summary>
    public static void ClearAll(MessagesApp? app)
    {
        if (!CanMutate("ClearAll")) return;
        try
        {
            var conversations = MessagesApp.Conversations;
            if (conversations == null)
            {
                _log?.Warn("ClearAll: Conversations list unavailable.");
                return;
            }
            int moved = 0;
            for (int i = 0; i < conversations.Count; i++)
            {
                MSGConversation? conv = conversations[i];
                if (!IsAlive(conv)) continue;
                if (IsPurged(conv) || IsTrashed(conv)) continue;
                try
                {
                    if (!conv!.EntryVisible) continue;
                    conv.SetEntryVisibility(false);
                    _state.Trashed.Add(MakeEntry(conv));
                    moved++;
                }
                catch (Exception ex)
                {
                    _log?.Warn($"ClearAll: conversation {i} skipped: {ex.Message}");
                }
            }

            Save();
            RefreshApp(app);
            OnTrashChanged?.Invoke();
            _log?.Info($"ClearAll: {moved} conversation(s) moved to trash.");
        }
        catch (Exception ex)
        {
            _log?.Error($"ClearAll failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Restores one trashed thread: SetEntryVisibility(true) + MoveToTop() +
    /// RepositionEntries().
    /// </summary>
    public static void Restore(TrashEntry entry, MessagesApp? app)
    {
        if (entry == null || !CanMutate("Restore")) return;
        try
        {
            MSGConversation? conv = FindConversation(entry);
            if (conv == null)
            {
                // Conversation no longer exists (reload/slot switch) — drop the
                // stale record instead of keeping a dead row forever. Identity
                // match (not reference equality): the row capture can be stale
                // after LoadForCurrentSlot replaced _state.
                RemoveEntryIdentity(_state.Trashed, entry);
                Save();
                OnTrashChanged?.Invoke();
                _log?.Warn($"Restore: no live conversation for '{entry.ContactName}' — record dropped.");
                return;
            }

            conv.SetEntryVisibility(true);
            conv.MoveToTop();
            RemoveEntryIdentity(_state.Trashed, entry);
            Save();
            RefreshApp(app);
            OnTrashChanged?.Invoke();
            _log?.Info($"Restore: '{entry.ContactName}' restored to inbox.");
        }
        catch (Exception ex)
        {
            _log?.Error($"Restore failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Permanently empties the trash: every trashed thread is removed from
    /// MessagesApp.Conversations / ActiveConversations (it disappears from the
    /// app entirely) and recorded in the purge list.
    ///
    /// Design note: the native MSGConversation object is deliberately NOT
    /// destroyed — it is still referenced by the game's own save system, and
    /// destroying its UI would make vanilla callbacks (RenderMessage,
    /// RepositionEntry) hit collected objects. Instead the purge list re-applies
    /// the removal after every save load (see ApplyToConversations), so purged
    /// threads stay gone across sessions.
    /// </summary>
    public static void EmptyTrash(MessagesApp? app)
    {
        if (!CanMutate("EmptyTrash")) return;
        try
        {
            int purged = 0;
            foreach (TrashEntry entry in _state.Trashed.ToArray())
            {
                MSGConversation? conv = FindConversation(entry);
                if (conv != null)
                {
                    try
                    {
                        conv.SetEntryVisibility(false);
                        RemoveFromAppLists(conv);
                        purged++;
                    }
                    catch (Exception ex)
                    {
                        _log?.Warn($"EmptyTrash: '{entry.ContactName}' removal failed: {ex.Message}");
                    }
                }
                _state.Purged.Add(entry);
            }

            _state.Trashed.Clear();
            Save();
            RefreshApp(app);
            OnTrashChanged?.Invoke();
            _log?.Info($"EmptyTrash: {purged} conversation(s) permanently removed.");
        }
        catch (Exception ex)
        {
            _log?.Error($"EmptyTrash failed: {ex.Message}");
        }
    }

    // ------------------------------------------------------------------
    // State re-application (after save load + throttled tick)
    // ------------------------------------------------------------------

    /// <summary>
    /// Re-applies trash/purge state to the freshly loaded conversation list:
    /// trashed threads get hidden again, purged threads get hidden AND removed
    /// from the app's static lists. Idempotent — safe to call from every hook
    /// and from the throttled tick (purged MSGConversation objects stay alive
    /// and can receive new messages, which lets vanilla callbacks re-show the
    /// entry; the tick re-apply counters that). Cheap: early-returns when both
    /// lists are empty.
    /// </summary>
    public static void ApplyToConversations()
    {
        try
        {
            if (_state.Trashed.Count == 0 && _state.Purged.Count == 0) return;

            // Backwards iteration: RemoveFromAppLists shrinks the list under the
            // walk. A forward loop skips the element that shifts into the freed
            // slot (adjacent purged threads — second one stayed visible).
            var conversations = MessagesApp.Conversations;
            if (conversations != null)
            {
                for (int i = conversations.Count - 1; i >= 0; i--)
                {
                    MSGConversation? conv = conversations[i];
                    if (!IsAlive(conv)) continue;
                    try
                    {
                        if (IsPurged(conv))
                        {
                            conv!.SetEntryVisibility(false);
                            RemoveFromAppLists(conv);
                        }
                        else if (IsTrashed(conv))
                        {
                            conv!.SetEntryVisibility(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        _log?.Warn($"ApplyToConversations: entry {i} skipped: {ex.Message}");
                    }
                }
            }

            var active = MessagesApp.ActiveConversations;
            if (active != null)
            {
                for (int i = active.Count - 1; i >= 0; i--)
                {
                    MSGConversation? conv = active[i];
                    if (!IsAlive(conv)) continue;
                    try
                    {
                        if (IsPurged(conv))
                        {
                            conv!.SetEntryVisibility(false);
                            RemoveFromAppLists(conv);
                        }
                        else if (IsTrashed(conv))
                        {
                            conv!.SetEntryVisibility(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        _log?.Warn($"ApplyToConversations (active): entry {i} skipped: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _log?.Warn($"ApplyToConversations failed: {ex.Message}");
        }
    }

    // ------------------------------------------------------------------
    // Helpers
    // ------------------------------------------------------------------

    private static bool CanMutate(string context)
    {
        if (!NetworkGuard.IsHostOrSingleplayer())
        {
            _log?.Warn($"{context}: ignored — multiplayer clients have a read-only trash (host-only).");
            return false;
        }
        return true;
    }

    private static void RefreshApp(MessagesApp? app)
    {
        try
        {
            if (app == null || !NetworkGuard.IsAlive(app)) return;
            app.RepositionEntries();
            app.RefreshNotifications();
        }
        catch (Exception ex)
        {
            _log?.Warn($"RefreshApp failed: {ex.Message}");
        }
    }

    private static void RemoveFromAppLists(MSGConversation conv)
    {
        try
        {
            var all = MessagesApp.Conversations;
            all?.Remove(conv);
            var active = MessagesApp.ActiveConversations;
            active?.Remove(conv);
        }
        catch (Exception ex)
        {
            _log?.Warn($"RemoveFromAppLists failed: {ex.Message}");
        }
    }

    private static TrashEntry MakeEntry(MSGConversation conv)
    {
        return new TrashEntry
        {
            Id = GetConversationId(conv),
            ContactName = SafeString(conv.ContactName),
            Index = SafeIndex(conv)
        };
    }

    /// <summary>
    /// Best-effort stable id: MSGConversation.ConversationId first (0.4.7f6),
    /// then the save file name, else a synthetic index+name key. Never throws.
    /// </summary>
    private static string GetConversationId(MSGConversation conv)
    {
        try
        {
            string id = SafeString(conv.ConversationId);
            if (id.Length > 0) return id;
        }
        catch { /* fall through */ }

        try
        {
            string fileName = SafeString(conv.SaveFileName);
            if (fileName.Length > 0) return fileName;
        }
        catch { /* fall through */ }

        return $"idx:{SafeIndex(conv)}|{SafeString(conv.ContactName)}";
    }

    /// <summary>
    /// Finds the trash/purge record for a live conversation. Match order: Id,
    /// then Index+Name, then a name-only fallback that is restricted to records
    /// with Index &lt; 0 (never captured a valid index) — a broader name fallback
    /// cross-matches duplicate contact names (e.g. several "Unknown") and made
    /// ClearAll skip visible threads or Restore show the wrong one.
    /// </summary>
    private static TrashEntry? FindMatch(List<TrashEntry> list, MSGConversation? conv)
    {
        if (conv == null || !IsAlive(conv)) return null;
        string id = GetConversationId(conv);
        string name = SafeString(conv.ContactName);
        int index = SafeIndex(conv);

        TrashEntry? byName = null;
        foreach (TrashEntry entry in list)
        {
            if (entry == null) continue;
            if (entry.Id.Length > 0 && id.Length > 0 && string.Equals(entry.Id, id, StringComparison.Ordinal))
                return entry;
            if (entry.Index >= 0 && entry.Index == index &&
                string.Equals(entry.ContactName, name, StringComparison.OrdinalIgnoreCase))
                return entry;
            if (byName == null && entry.Index < 0 && name.Length > 0 &&
                string.Equals(entry.ContactName, name, StringComparison.OrdinalIgnoreCase))
                byName = entry;
        }
        return byName;
    }

    /// <summary>
    /// Finds the live conversation for a trash/purge record. Same match order and
    /// same restricted name fallback as <see cref="FindMatch"/> (see there for why).
    /// </summary>
    private static MSGConversation? FindConversation(TrashEntry entry)
    {
        var conversations = MessagesApp.Conversations;
        if (conversations == null) return null;
        MSGConversation? byName = null;
        for (int i = 0; i < conversations.Count; i++)
        {
            MSGConversation? conv = conversations[i];
            if (!IsAlive(conv)) continue;

            string id = GetConversationId(conv!);
            if (entry.Id.Length > 0 && id.Length > 0 && string.Equals(entry.Id, id, StringComparison.Ordinal))
                return conv;

            if (entry.Index >= 0 && entry.Index == SafeIndex(conv!) &&
                string.Equals(entry.ContactName, SafeString(conv!.ContactName), StringComparison.OrdinalIgnoreCase))
                return conv;

            if (byName == null && entry.Index < 0 && entry.ContactName.Length > 0 &&
                string.Equals(entry.ContactName, SafeString(conv!.ContactName), StringComparison.OrdinalIgnoreCase))
                byName = conv;
        }
        return byName;
    }

    /// <summary>
    /// Removes the record that matches <paramref name="entry"/> by identity
    /// (reference, else Id, else Index+Name) — never by List.Remove(reference)
    /// alone: a row's captured entry can be stale after LoadForCurrentSlot
    /// replaced _state, and a silent no-op removal would leave the record to
    /// re-hide the just-restored thread on the next apply.
    /// </summary>
    private static bool RemoveEntryIdentity(List<TrashEntry> list, TrashEntry entry)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (ReferenceEquals(list[i], entry))
            {
                list.RemoveAt(i);
                return true;
            }
        }
        for (int i = 0; i < list.Count; i++)
        {
            TrashEntry? e = list[i];
            if (e == null) continue;
            if (entry.Id.Length > 0 && e.Id.Length > 0 && string.Equals(e.Id, entry.Id, StringComparison.Ordinal))
            {
                list.RemoveAt(i);
                return true;
            }
            if (entry.Index >= 0 && e.Index == entry.Index &&
                string.Equals(e.ContactName, entry.ContactName, StringComparison.OrdinalIgnoreCase))
            {
                list.RemoveAt(i);
                return true;
            }
        }
        _log?.Warn($"RemoveEntryIdentity: no record matched '{entry.ContactName}' (id '{entry.Id}') — nothing removed.");
        return false;
    }

    private static string SafeString(string? value) => value ?? string.Empty;

    private static int SafeIndex(MSGConversation conv)
    {
        try { return conv.Index; }
        catch { return -1; }
    }

    /// <summary>IL2CPP liveness for non-UnityEngine game objects (MSGConversation, save data). Shared by TrashUI.</summary>
    public static bool IsAlive(Il2CppObjectBase? obj)
    {
        if (obj == null) return false;
        try { return obj.Pointer != IntPtr.Zero && !obj.WasCollected; }
        catch { return false; }
    }
}
