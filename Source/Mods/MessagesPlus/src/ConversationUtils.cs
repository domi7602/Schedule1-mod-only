using System;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppScheduleOne.Messaging;
using UnityEngine;

namespace MessagesPlus;

/// <summary>
/// Small IL2CPP-safe helpers shared by <see cref="InboxUI"/>,
/// <see cref="InboxView"/> and <see cref="LegacyRestore"/>: proxy liveness
/// (Pointer/WasCollected), stable conversation identity, and the conservative
/// category filter. Every member access on a game proxy is wrapped in
/// try/catch — proxies can throw at any time (WasCollected / uninitialized
/// native pointer).
/// </summary>
internal static class ConversationUtils
{
    /// <summary>
    /// IL2CPP liveness for non-UnityEngine game objects (MSGConversation, ...).
    /// UnityEngine.Object types use <see cref="S1Mods.Shared.NetworkGuard.IsAlive(UnityEngine.Object?)"/>.
    /// </summary>
    public static bool IsAlive(Il2CppObjectBase? obj)
    {
        if (obj == null) return false;
        try { return obj.Pointer != IntPtr.Zero && !obj.WasCollected; }
        catch { return false; }
    }

    /// <summary>Contact display name — never throws.</summary>
    public static string SafeName(MSGConversation conv)
    {
        try { return conv.ContactName ?? "?"; }
        catch { return "?"; }
    }

    /// <summary>
    /// Best-effort stable id: MSGConversation.ConversationId first (0.4.7f6),
    /// then the save file name, else a synthetic index+name key. Never throws.
    /// </summary>
    public static string GetConversationId(MSGConversation conv)
    {
        try
        {
            string id = conv.ConversationId ?? string.Empty;
            if (id.Length > 0) return id;
        }
        catch { /* fall through */ }
        try
        {
            string fileName = conv.SaveFileName ?? string.Empty;
            if (fileName.Length > 0) return fileName;
        }
        catch { /* fall through */ }
        string fallbackName = string.Empty;
        int fallbackIndex = -1;
        try { fallbackName = conv.ContactName ?? string.Empty; } catch { }
        try { fallbackIndex = conv.Index; } catch { }
        return $"idx:{fallbackIndex}|{fallbackName}";
    }

    /// <summary>
    /// Row rect of a conversation's inbox entry (null before the lazy
    /// MSGConversation.CreateUI has run, or when the proxy is dead). Never throws.
    /// </summary>
    public static RectTransform? SafeEntry(MSGConversation conv)
    {
        try
        {
            RectTransform entry = conv.entry;
            if (entry == null) return null;
            return S1Mods.Shared.NetworkGuard.IsAlive(entry) ? entry : null;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>Read flag of a conversation (MSGConversation.Read). false return = access error (unknown).</summary>
    public static bool TryGetRead(MSGConversation conv, out bool read)
    {
        read = false;
        try
        {
            read = conv.Read;
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// True when Categories contains the given category. Null/empty Categories
    /// or any access error means NO match. View-only filter — unlike the
    /// conservative <see cref="IsCustomer"/> delete rule, matching is allowed
    /// for multi-category threads here because nothing is mutated.
    /// </summary>
    public static bool HasCategory(MSGConversation conv, EConversationCategory wanted)
    {
        try
        {
            var categories = conv.Categories;
            if (categories == null) return false;
            int count = categories.Count;
            for (int i = 0; i < count; i++)
            {
                if (categories[i] == wanted) return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Conservative customer filter (the ONLY threads Clear All/Clear Read may
    /// hide): Categories contains Customer AND contains neither Supplier nor
    /// Dealer. Null/empty Categories or any access error means NON-customer —
    /// such threads are never touched.
    /// </summary>
    public static bool IsCustomer(MSGConversation conv)
    {
        try
        {
            var categories = conv.Categories;
            if (categories == null) return false;
            int count = categories.Count;
            if (count <= 0) return false;

            bool hasCustomer = false;
            for (int i = 0; i < count; i++)
            {
                EConversationCategory category = categories[i];
                if (category == EConversationCategory.Supplier || category == EConversationCategory.Dealer)
                    return false;
                if (category == EConversationCategory.Customer)
                    hasCustomer = true;
            }
            return hasCustomer;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// True when Categories contain Supplier or Dealer (legacy safety net).
    /// Null/empty Categories or any access error means false — never touch it.
    /// </summary>
    public static bool IsSupplierOrDealer(MSGConversation conv)
    {
        try
        {
            var categories = conv.Categories;
            if (categories == null) return false;
            int count = categories.Count;
            for (int i = 0; i < count; i++)
            {
                EConversationCategory category = categories[i];
                if (category == EConversationCategory.Supplier || category == EConversationCategory.Dealer)
                    return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
}
