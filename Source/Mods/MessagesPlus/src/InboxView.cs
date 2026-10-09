using System;
using System.Collections.Generic;
using Il2CppScheduleOne.Messaging;
using Il2CppScheduleOne.UI.Phone.Messages;
using UnityEngine;

namespace MessagesPlus;

/// <summary>Active category filter for the inbox view.</summary>
internal enum InboxFilter
{
    All,
    Customer,
    Dealer,
    Supplier
}

/// <summary>
/// Temporary VIEW state for the vanilla inbox (search text + category filter).
///
/// Hides non-matching conversation entries via `entry.gameObject.SetActive(false)`
/// — NEVER via `MSGConversation.SetEntryVisibility` (that persists `IsHidden`
/// into the save and is the Clear All / Clear Read mutation path).
///
/// Ownership model: only entries this engine hid are ever re-shown. Rows hidden
/// by vanilla itself (its own category filter, SetEntryVisibility) are left
/// alone even when they match. Vanilla can re-show entries on its own events
/// (message arrival, lazy CreateUI — the v0.1.x TrashUI W5 lesson), so the view
/// is re-applied from InboxUI.Tick while it is active. With no search text and
/// filter == All the engine is a no-op (zero-cost default state).
/// </summary>
internal static class InboxView
{
    private static readonly HashSet<string> HiddenIds = new();

    private static string _search = string.Empty;
    private static InboxFilter _filter = InboxFilter.All;
    private static bool _active;
    private static int _lastVisible;
    private static int _lastTotal;

    /// <summary>True while a search text or category filter narrows the inbox.</summary>
    public static bool IsActive => _active;

    /// <summary>True while a filter/search is active but no inbox row matches (empty-state hint).</summary>
    public static bool NoMatches => _active && _lastTotal > 0 && _lastVisible == 0;

    /// <summary>Live name filter (case-insensitive substring). Empty = no search.</summary>
    public static void SetSearch(string? value)
    {
        string normalized = (value ?? string.Empty).Trim();
        if (normalized == _search) return;
        _search = normalized;
        RefreshState();
    }

    public static void SetFilter(InboxFilter filter)
    {
        if (filter == _filter) return;
        _filter = filter;
        RefreshState();
    }

    /// <summary>Back to the unfiltered inbox (called on app close — W12 analogue).</summary>
    public static void ResetView()
    {
        if (!_active && _search.Length == 0 && _filter == InboxFilter.All) return;
        _search = string.Empty;
        _filter = InboxFilter.All;
        RefreshState();
    }

    /// <summary>
    /// Drops all view state WITHOUT touching entries (scene unload — the
    /// entries die with the scene and must never be SetActive'ed).
    /// </summary>
    public static void DropState()
    {
        _search = string.Empty;
        _filter = InboxFilter.All;
        _active = false;
        _lastVisible = 0;
        _lastTotal = 0;
        HiddenIds.Clear();
    }

    private static void RefreshState()
    {
        _active = _search.Length > 0 || _filter != InboxFilter.All;
        (int visible, int total) = Apply();
        Mod.Log?.Info($"InboxView: applied search='{_search}' filter={_filter} visible={visible}/{total}");
    }

    /// <summary>
    /// Re-applies the current view to every vanilla-visible conversation entry
    /// (idempotent, self-healing). Only touches entries whose conversation is
    /// alive and `EntryVisible`, and only ever restores hides this engine owns.
    /// Returns (matching, considered).
    /// </summary>
    public static (int visible, int total) Apply()
    {
        int visible = 0;
        int total = 0;
        try
        {
            var conversations = MessagesApp.ActiveConversations;
            if (conversations == null)
            {
                _lastVisible = 0;
                _lastTotal = 0;
                return (0, 0);
            }
            int count = conversations.Count;
            for (int i = 0; i < count; i++)
            {
                MSGConversation? conv = conversations[i];
                if (!ConversationUtils.IsAlive(conv)) continue;
                try
                {
                    if (!conv!.EntryVisible) continue;
                    RectTransform? entry = ConversationUtils.SafeEntry(conv);
                    if (entry == null) continue;

                    total++;
                    string id = ConversationUtils.GetConversationId(conv);
                    bool match = Matches(conv);
                    bool ours = HiddenIds.Contains(id);
                    GameObject go = entry.gameObject;

                    if (match)
                    {
                        visible++;
                        if (ours)
                        {
                            // Our own hide from an earlier filter — restore it.
                            HiddenIds.Remove(id);
                            if (!go.activeSelf) go.SetActive(true);
                        }
                        // not ours: vanilla-hidden or already visible — leave alone
                    }
                    else if (ours)
                    {
                        // Vanilla re-showed it (W5) — re-apply our hide.
                        if (go.activeSelf) go.SetActive(false);
                    }
                    else if (go.activeSelf)
                    {
                        // New non-matching row — hide it and claim ownership.
                        go.SetActive(false);
                        HiddenIds.Add(id);
                    }
                    // inactive + not ours = vanilla-hidden → leave alone
                }
                catch (Exception ex)
                {
                    Mod.Log?.Warn($"InboxView: entry {i} skipped: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"InboxView.Apply failed: {ex.Message}");
        }
        _lastVisible = visible;
        _lastTotal = total;
        return (visible, total);
    }

    /// <summary>Throttled re-apply while a view is active (vanilla may re-show entries).</summary>
    public static void Reapply()
    {
        if (!_active) return;
        Apply();
    }

    /// <summary>
    /// Unread conversation count among visible inbox threads (the per-thread
    /// MSGConversation.Read flag — vanilla has no per-message read state).
    /// Threads with unknown read state are skipped.
    /// </summary>
    public static int CountUnread()
    {
        int unread = 0;
        try
        {
            var conversations = MessagesApp.ActiveConversations;
            if (conversations == null) return 0;
            int count = conversations.Count;
            for (int i = 0; i < count; i++)
            {
                MSGConversation? conv = conversations[i];
                if (!ConversationUtils.IsAlive(conv)) continue;
                try
                {
                    if (!conv!.EntryVisible) continue;
                    if (!ConversationUtils.TryGetRead(conv, out bool read)) continue;
                    if (!read) unread++;
                }
                catch
                {
                    // skip unreadable entries
                }
            }
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"InboxView.CountUnread failed: {ex.Message}");
        }
        return unread;
    }

    private static bool Matches(MSGConversation conv)
    {
        if (_filter != InboxFilter.All &&
            !ConversationUtils.HasCategory(conv, ToCategory(_filter)))
        {
            return false;
        }

        if (_search.Length > 0 &&
            !ConversationUtils.MatchesSearch(conv, _search))
        {
            return false;
        }

        return true;
    }

    private static EConversationCategory ToCategory(InboxFilter filter) => filter switch
    {
        InboxFilter.Customer => EConversationCategory.Customer,
        InboxFilter.Dealer => EConversationCategory.Dealer,
        InboxFilter.Supplier => EConversationCategory.Supplier,
        _ => EConversationCategory.Customer
    };
}
