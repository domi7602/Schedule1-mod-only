using System;
using System.Collections.Generic;
using System.IO;
using Il2CppScheduleOne.Messaging;
using Il2CppScheduleOne.UI.Phone.Messages;
using S1Mods.Shared;

namespace MessagesPlus;

/// <summary>
/// Identity record from the v0.1.x trash files. Three keys because the vanilla
/// game offers no guaranteed stable handle across sessions: <see cref="Id"/>
/// (MSGConversation.ConversationId, else SaveFileName), and
/// <see cref="Index"/> + <see cref="ContactName"/> as the fallback pair.
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
/// Legacy trash document shape (v0.1.x: trash_slot_{n}.json + .bak siblings).
/// Read-only — v0.2.0 never writes trash records anymore.
/// </summary>
public sealed class TrashState
{
    public List<TrashEntry> Trashed { get; set; } = new();
    public List<TrashEntry> Purged { get; set; } = new();
}

/// <summary>
/// One-time legacy restore that heals save data written by MessagesPlus
/// v0.1.x. CRITICAL PERSISTENCE FACT: SetEntryVisibility(false) IS persisted
/// into the game save (MSGConversationData.IsHidden) — threads hidden by the
/// old mod are STILL hidden in the player's save and must be un-hidden here.
///
/// Two passes (idempotent — safe to run from every hook):
///   (a) Legacy files: scan UserData/MessagesPlus/ for trash_*.json and
///       trash_*.json.bak OF THE ACTIVE SAVE SLOT (plus the legacy "default"
///       suffix), un-hide every conversation recorded in their Trashed AND
///       Purged lists, then rename each FULLY restored file to
///       &lt;name&gt;.restored (only when every record matched — unmatched or
///       unreadable files stay for a later load; *.restored is never scanned
///       again, so renaming early would burn the records).
///   (b) Safety net: walk MessagesApp.Conversations/ActiveConversations and
///       un-hide every hidden Supplier/Dealer conversation. Customer threads
///       are NEVER un-hidden here (they stay hidden on purpose).
///
/// Runs on GameLifecycle.OnSaveInfoLoaded and on the MessagesApp.Loaded
/// postfix. When the conversation lists are still empty (the usual case at
/// OnSaveInfoLoaded — MessagesApp.Loaded populates them later) the run is
/// DEFERRED without renaming anything, so no record can be lost to an early
/// call. Multiplayer: restore mutations are host-only (NetworkGuard).
/// </summary>
public static class LegacyRestore
{
    /// <summary>
    /// Executes both restore passes. Idempotent: processed files are renamed to
    /// *.restored (never scanned again) and pass (b) only flips visibility.
    /// </summary>
    public static void Run()
    {
        try
        {
            if (!NetworkGuard.IsHostOrSingleplayer())
            {
                Mod.Log?.Debug("LegacyRestore: skipped — restore mutations are host-only (multiplayer client).");
                return;
            }

            // Deferral guard: without a populated conversation list nothing can
            // be matched — renaming the files anyway would lose the records.
            // The MessagesApp.Loaded postfix retries once the list is ready.
            if (!ConversationsAvailable())
            {
                Mod.Log?.Debug("LegacyRestore: conversation list not populated yet — deferring to the MessagesApp.Loaded hook.");
                return;
            }

            int fromFiles = RestoreFromLegacyFiles();
            int fromSafetyNet = RestoreHiddenNonCustomers();

            if (fromFiles + fromSafetyNet > 0)
            {
                Mod.Log?.Info($"LegacyRestore: restored {fromFiles} conversation(s) from legacy trash files, {fromSafetyNet} hidden non-customer conversation(s) via safety net.");
            }
            else
            {
                Mod.Log?.Debug("LegacyRestore: nothing to restore.");
            }
        }
        catch (Exception ex)
        {
            Mod.Log?.Error($"LegacyRestore failed: {ex.Message}");
        }
    }

    private static bool ConversationsAvailable()
    {
        try
        {
            var conversations = MessagesApp.Conversations;
            return conversations != null && conversations.Count > 0;
        }
        catch
        {
            return false;
        }
    }

    // ------------------------------------------------------------------
    // Pass (a): legacy trash files
    // ------------------------------------------------------------------

    private static int RestoreFromLegacyFiles()
    {
        string dir = SafeStorage.GetUserDataPath("MessagesPlus");
        if (!Directory.Exists(dir)) return 0;

        List<string> files = new();
        try
        {
            files.AddRange(Directory.GetFiles(dir, "trash_*.json"));
            files.AddRange(Directory.GetFiles(dir, "trash_*.json.bak"));
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"LegacyRestore: trash file scan failed: {ex.Message}");
            return 0;
        }

        if (files.Count == 0) return 0;

        // Ordinal sort so a plain .json is processed before its .bak sibling.
        files.Sort(StringComparer.Ordinal);

        int restored = 0;
        foreach (string file in files) // managed string list — plain foreach is fine
        {
            string name = Path.GetFileName(file);
            if (name.EndsWith(".restored", StringComparison.OrdinalIgnoreCase)) continue;

            // Review-Major (2026-09-26): only touch the ACTIVE save slot's files.
            // Matching runs against the loaded save only — records of other slots
            // cannot match and must never be renamed away (they are needed when
            // that slot is loaded). "default" is the legacy fallback suffix and is
            // always processed.
            if (!IsForActiveSlot(name))
            {
                Mod.Log?.Debug($"LegacyRestore: '{name}' belongs to another save slot — kept untouched.");
                continue;
            }

            // Review-Blocker (2026-09-26): rename ONLY after a complete restore
            // (file parsed AND every record matched + un-hidden). An incomplete run
            // keeps the file for the next load — renaming it would burn the records
            // forever (*.restored is never scanned again).
            (int restoredHere, bool complete) = RestoreFromFile(file);
            restored += restoredHere;
            if (complete)
            {
                RenameProcessed(file);
            }
            else
            {
                Mod.Log?.Warn($"LegacyRestore: '{name}' not fully restored (unmatched/failed records above) — kept for the next load.");
            }
        }
        return restored;
    }

    /// <summary>
    /// True when a legacy file name belongs to the active save slot or is the
    /// legacy "default" fallback (v0.1.x wrote trash_{suffix}.json with
    /// slot_{n} or "default"). Anything else is left untouched. Never throws —
    /// an unknown slot only processes "default" files.
    /// </summary>
    private static bool IsForActiveSlot(string fileName)
    {
        string infix = fileName
            .Substring("trash_".Length)
            .Replace(".json.bak", string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace(".json", string.Empty, StringComparison.OrdinalIgnoreCase);

        if (string.Equals(infix, "default", StringComparison.OrdinalIgnoreCase)) return true;

        int slot = -1;
        try { slot = SaveSlots.GetActiveSlotNumber(); } catch { /* unknown slot below */ }
        return slot >= 0 && string.Equals(infix, $"slot_{slot}", StringComparison.OrdinalIgnoreCase);
    }

    private static (int Restored, bool Complete) RestoreFromFile(string file)
    {
        TrashState state;
        try
        {
            // Strict parse (review-blocker 2026-09-26): a corrupt file must NOT
            // look like an empty record set — that would rename it and burn the
            // records. Only a cleanly parsed document counts as processed;
            // anything else is kept for manual recovery.
            string json = File.ReadAllText(file);
            TrashState? parsed = System.Text.Json.JsonSerializer.Deserialize<TrashState>(json);
            if (parsed == null)
            {
                Mod.Log?.Warn($"LegacyRestore: '{Path.GetFileName(file)}' contains no document — kept.");
                return (0, false);
            }
            state = parsed;
            state.Trashed ??= new List<TrashEntry>();
            state.Purged ??= new List<TrashEntry>();
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"LegacyRestore: '{Path.GetFileName(file)}' unreadable ({ex.Message}) — kept for manual recovery.");
            return (0, false);
        }

        (int restoredTrashed, int unresolvedTrashed) = RestoreEntries(state.Trashed);
        (int restoredPurged, int unresolvedPurged) = RestoreEntries(state.Purged);
        bool complete = unresolvedTrashed + unresolvedPurged == 0;
        return (restoredTrashed + restoredPurged, complete);
    }

    private static (int Restored, int Unresolved) RestoreEntries(List<TrashEntry> entries)
    {
        int restored = 0;
        int unresolved = 0;
        for (int i = 0; i < entries.Count; i++)
        {
            TrashEntry? entry = entries[i];
            if (entry == null) continue; // garbage record — nothing a later run could resolve
            entry.Id ??= string.Empty;
            entry.ContactName ??= string.Empty;

            MSGConversation? conv = FindConversation(entry);
            if (conv == null)
            {
                // Unmatched records keep the file alive (review-blocker): the
                // thread may only be created later (or on another load), and the
                // customer safety net deliberately never un-hides customers.
                unresolved++;
                Mod.Log?.Warn($"LegacyRestore: no live conversation for '{entry.ContactName}' (id '{entry.Id}') — record kept for a later load.");
                continue;
            }

            try
            {
                conv.SetEntryVisibility(true);
                restored++;
                Mod.Log?.Info($"LegacyRestore: un-hid '{ConversationUtils.SafeName(conv)}' (record '{entry.ContactName}').");
            }
            catch (Exception ex)
            {
                unresolved++;
                Mod.Log?.Warn($"LegacyRestore: un-hide failed for '{entry.ContactName}': {ex.Message}");
            }
        }
        return (restored, unresolved);
    }

    /// <summary>
    /// Finds the live conversation for a legacy trash record. Match order (same
    /// as the v0.1.x trash code): Id == ConversationId ?? SaveFileName; else
    /// Index &gt;= 0 AND Index == conv.Index AND ContactName equals
    /// ordinal-ignore-case; else a name-only fallback restricted to records
    /// with Index &lt; 0 (a broader name fallback cross-matches duplicate
    /// contact names such as several "Unknown Number").
    /// </summary>
    private static MSGConversation? FindConversation(TrashEntry entry)
    {
        var conversations = MessagesApp.Conversations;
        if (conversations == null) return null;

        MSGConversation? byName = null;
        int count = conversations.Count;
        for (int i = 0; i < count; i++)
        {
            try
            {
                MSGConversation? conv = conversations[i];
                if (!ConversationUtils.IsAlive(conv)) continue;

                string id = ConversationUtils.GetConversationId(conv!);
                if (entry.Id.Length > 0 && id.Length > 0 && string.Equals(entry.Id, id, StringComparison.Ordinal))
                    return conv;

                string convName = string.Empty;
                int convIndex = -1;
                try { convName = conv!.ContactName ?? string.Empty; } catch { }
                try { convIndex = conv!.Index; } catch { }

                if (entry.Index >= 0 && entry.Index == convIndex &&
                    string.Equals(entry.ContactName, convName, StringComparison.OrdinalIgnoreCase))
                    return conv;

                if (byName == null && entry.Index < 0 && entry.ContactName.Length > 0 &&
                    string.Equals(entry.ContactName, convName, StringComparison.OrdinalIgnoreCase))
                    byName = conv;
            }
            catch (Exception ex)
            {
                // Review-minor 2026-09-26: one throwing proxy must not abort the
                // whole match walk (the outer catch would end the run early).
                Mod.Log?.Warn($"LegacyRestore: FindConversation entry {i} skipped: {ex.Message}");
            }
        }
        return byName;
    }

    /// <summary>
    /// Renames a fully processed legacy file to &lt;name&gt;.restored (skips files
    /// already ending in .restored). overwrite:false — an existing .restored
    /// marker holds the only copy of its records and must never be replaced
    /// (review-minor 2026-09-26); on a collision the source file stays and is
    /// retried on the next load. Wrapped in try/catch — a locked/read-only file
    /// must never break the restore.
    /// </summary>
    private static void RenameProcessed(string file)
    {
        try
        {
            if (file.EndsWith(".restored", StringComparison.OrdinalIgnoreCase)) return;
            string target = file + ".restored";
            File.Move(file, target, overwrite: false);
            Mod.Log?.Info($"LegacyRestore: '{Path.GetFileName(file)}' -> '{Path.GetFileName(target)}'.");
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"LegacyRestore: rename of '{Path.GetFileName(file)}' failed: {ex.Message}");
        }
    }

    // ------------------------------------------------------------------
    // Pass (b): safety net for hidden non-customer conversations
    // ------------------------------------------------------------------

    /// <summary>
    /// Un-hides every hidden Supplier/Dealer conversation in both app lists.
    /// Customer threads are NEVER un-hidden here (they stay hidden on purpose).
    /// Unknown/empty categories are treated as non-supplier/dealer and left
    /// untouched.
    /// </summary>
    private static int RestoreHiddenNonCustomers()
    {
        int restored = RestoreHiddenNonCustomersIn(useActiveList: false);
        restored += RestoreHiddenNonCustomersIn(useActiveList: true);
        return restored;
    }

    private static int RestoreHiddenNonCustomersIn(bool useActiveList)
    {
        // "var" is deliberate: the Il2Cpp list type must not be named explicitly
        // (it differs across interop assemblies).
        var conversations = useActiveList ? MessagesApp.ActiveConversations : MessagesApp.Conversations;
        if (conversations == null) return 0;

        // Plan-then-commit: SetEntryVisibility can rebuild the lists under a
        // live walk — collect matches first, then mutate.
        List<MSGConversation> targets = new();
        try
        {
            int count = conversations.Count;
            for (int i = 0; i < count; i++)
            {
                MSGConversation? conv = conversations[i];
                if (!ConversationUtils.IsAlive(conv)) continue;
                try
                {
                    if (conv!.EntryVisible) continue;
                    if (!ConversationUtils.IsSupplierOrDealer(conv)) continue; // only Supplier/Dealer — never customers/unknown
                    targets.Add(conv);
                }
                catch (Exception ex)
                {
                    Mod.Log?.Warn($"LegacyRestore safety net: entry {i} skipped: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"LegacyRestore safety net walk failed: {ex.Message}");
        }

        int restored = 0;
        for (int i = 0; i < targets.Count; i++)
        {
            MSGConversation conv = targets[i];
            if (!ConversationUtils.IsAlive(conv)) continue;
            try
            {
                conv.SetEntryVisibility(true);
                restored++;
                Mod.Log?.Info($"LegacyRestore safety net: un-hid hidden '{ConversationUtils.SafeName(conv)}' thread.");
            }
            catch (Exception ex)
            {
                Mod.Log?.Warn($"LegacyRestore safety net: un-hide failed for '{ConversationUtils.SafeName(conv)}': {ex.Message}");
            }
        }
        return restored;
    }
}
