using System;
using System.Collections.Generic;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppScheduleOne.Messaging;
using Il2CppScheduleOne.UI.Phone.Messages;
using S1Mods.Shared;
using Il2CppTMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MessagesPlus;

/// <summary>
/// Dark mode — recolors the VANILLA Messages app shell so the app (not every
/// popup it can open) goes dark: page backgrounds, inbox rows, chat bubbles, and
/// the dialogue header. Order and counter-offer menus retain their vanilla colors.
/// Graphics are themed once per instance ID and original colors are cached — a
/// light restore can still return the game's exact appearance.
///
/// Only colours are touched — never layouts, raycasts or save state. Guards:
///  - our own injected UI (band/menu/dialog) is excluded from every sweep;
///  - avatars (small, roughly square) are never tinted (they are content);
///  - coloured elements (status bars, badges, the unread dot, the player's
///    accent bubbles) stay as they are.
/// </summary>
internal static class AppTheme
{
    private sealed class GraphicThemeState
    {
        public Graphic Graphic { get; }
        public Color Original { get; }
        public Color LastTheme { get; set; }
        public ThemeRole Role { get; set; }

        public GraphicThemeState(Graphic graphic, Color original, Color lastTheme, ThemeRole role)
        {
            Graphic = graphic;
            Original = original;
            LastTheme = lastTheme;
            Role = role;
        }
    }

    private sealed class SelectableThemeState
    {
        public Selectable Selectable { get; }
        public ColorBlock Original { get; }
        public ColorBlock LastTheme { get; set; }
        public ThemeRole Role { get; set; }

        public SelectableThemeState(Selectable selectable, ColorBlock original, ColorBlock lastTheme, ThemeRole role)
        {
            Selectable = selectable;
            Original = original;
            LastTheme = lastTheme;
            Role = role;
        }
    }

    private static readonly Dictionary<int, GraphicThemeState> _graphics = new();
    private static readonly Dictionary<int, SelectableThemeState> _selectables = new();
    private static readonly Dictionary<int, Graphic> _protectedGraphics = new();
    private static readonly List<Transform> _ownRoots = new();
    private static readonly List<Transform> _vanillaRoots = new();
    private static readonly List<int> _scratchIds = new();

    private static float _nextCachePruneTime;
    private static int _fullPassCount;
    private static int _passNew;
    private static int _passRepaired;
    private static int _passButtons;
    private static int _passProtected;
    private static int _passCleaned;

    // --- Chat-body diagnostics (one summary line per pass, never per frame) ---
    private static int _diagChatCandidates;
    private static int _diagChatTinted;
    private static int _diagChatNoGraphic;
    private static int _diagChatSkipped;
    private static int _diagChatLost;
    private static int _diagPageBackgroundsTinted;
    private static string _diagLastChatNode = string.Empty;

    // ------------------------------------------------------------------
    // Apply / restore
    // ------------------------------------------------------------------

    /// <summary>Recolours the known vanilla app surfaces; full scans are event/fallback driven.</summary>
    public static void Apply(MessagesApp app)
    {
        if (!NetworkGuard.IsAlive(app)) { return; }
        if (!IsPageVisible(app)) { return; }

        try
        {
            _passNew = 0;
            _passRepaired = 0;
            _passButtons = 0;
            _passProtected = 0;
            _passCleaned = 0;
            _fullPassCount++;
            ResetDiagCounters();

            PruneTrackedCaches();
            PruneRoots();
            RegisterConversationProtections();

            MSGConversation? current = null;
            try { current = app.currentConversation; } catch { /* not ready */ }
            if (ConversationUtils.IsAlive(current))
            {
                ProtectConversationStatus(current!);
                PreserveResponseArea(current!);
            }

            ThemePageBackgrounds(app);

            try
            {
                var conversations = MessagesApp.ActiveConversations;
                if (conversations != null)
                {
                    int count = conversations.Count;
                    for (int i = 0; i < count; i++)
                    {
                        MSGConversation? conv = conversations[i];
                        if (ConversationUtils.IsAlive(conv)) TintEntry(conv!);
                    }
                }
            }
            catch (Exception ex) { Mod.Log?.Debug($"AppTheme entries: {ex.Message}"); }

            try { TintDialogueHeader(app); }
            catch (Exception ex) { Mod.Log?.Debug($"AppTheme header: {ex.Message}"); }

            if (ConversationUtils.IsAlive(current))
            {
                try { TintBubbles(current!); }
                catch (Exception ex) { Mod.Log?.Debug($"AppTheme bubbles: {ex.Message}"); }
            }

            // The open conversation's own chat surface (the large light area behind
            // the bubbles). Vanilla never rebuilds this per message, so one pass
            // fills it and the 1 s fallback keeps it filled.
            if (ConversationUtils.IsAlive(current))
            {
                try { TintChatBodyBackground(current!); }
                catch (Exception ex) { Mod.Log?.Debug($"AppTheme chat body: {ex.Message}"); }
            }

            LogPassSummary();
            LogChatDiagnostics();
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"AppTheme.Apply failed: {ex.Message}");
        }
    }

    /// <summary>Immediately refreshes a conversation after vanilla creates or renders its UI.</summary>
    public static void RefreshConversation(MSGConversation conv)
    {
        if (!ConversationUtils.IsAlive(conv)) return;
        try
        {
            PruneTrackedCaches();
            PruneRoots();
            ProtectConversationStatus(conv);
            TintEntry(conv);
            TintBubbles(conv);
            TintChatBodyBackground(conv);
        }
        catch (Exception ex)
        {
            Mod.Log?.Debug($"AppTheme conversation refresh: {ex.Message}");
        }
    }

    /// <summary>Registers response/counter-offer UI immediately after vanilla creates it.</summary>
    public static void PreserveConversationResponseArea(MSGConversation conv)
    {
        if (!ConversationUtils.IsAlive(conv)) return;
        try
        {
            ProtectConversationStatus(conv);
            PreserveResponseArea(conv);
        }
        catch (Exception ex)
        {
            Mod.Log?.Debug($"AppTheme response protection: {ex.Message}");
        }
    }

    private static void ProtectConversationStatus(MSGConversation conv)
    {
        try
        {
            Image? fill = conv.sliderFill;
            if (!NetworkGuard.IsAlive(fill)) return;
            int id = fill!.GetInstanceID();
            if (!_protectedGraphics.TryGetValue(id, out Graphic? existing) ||
                !NetworkGuard.IsAlive(existing) || !SameObject(existing, fill))
            {
                _protectedGraphics[id] = fill;
                _passProtected++;
            }
        }
        catch { /* status UI may not exist yet */ }
    }

    private static void RegisterConversationProtections()
    {
        try
        {
            var conversations = MessagesApp.Conversations;
            if (conversations == null) return;
            int count = conversations.Count;
            for (int i = 0; i < count; i++)
            {
                MSGConversation? conv = conversations[i];
                if (ConversationUtils.IsAlive(conv)) ProtectConversationStatus(conv!);
            }
        }
        catch (Exception ex) { Mod.Log?.Debug($"AppTheme status registration: {ex.Message}"); }
    }

    /// <summary>Restores and excludes one vanilla menu/subtree from all later theme passes.</summary>
    public static void PreserveVanillaSubtree(GameObject root)
    {
        if (!NetworkGuard.IsAlive(root)) return;
        try
        {
            Transform rootTransform = root.transform;
            if (!RegisterVanillaRoot(rootTransform)) return;
            RestoreTrackedWithin(rootTransform);
            _passProtected++;
        }
        catch (Exception ex)
        {
            Mod.Log?.Debug($"AppTheme popup preservation failed: {ex.Message}");
        }
    }

    private static bool RegisterVanillaRoot(Transform candidate)
    {
        for (int i = _vanillaRoots.Count - 1; i >= 0; i--)
        {
            Transform? existing = _vanillaRoots[i];
            if (!NetworkGuard.IsAlive(existing))
            {
                _vanillaRoots.RemoveAt(i);
                _passCleaned++;
                continue;
            }

            if (SameObject(existing, candidate) || IsWithin(candidate, existing!)) return false;
            if (IsWithin(existing!, candidate)) _vanillaRoots.RemoveAt(i);
        }

        _vanillaRoots.Add(candidate);
        return true;
    }

    private static void RestoreTrackedWithin(Transform root)
    {
        _scratchIds.Clear();
        foreach (KeyValuePair<int, GraphicThemeState> pair in _graphics)
        {
            GraphicThemeState state = pair.Value;
            if (!NetworkGuard.IsAlive(state.Graphic) || IsWithin(state.Graphic.transform, root))
                _scratchIds.Add(pair.Key);
        }
        for (int i = 0; i < _scratchIds.Count; i++)
        {
            int id = _scratchIds[i];
            if (!_graphics.TryGetValue(id, out GraphicThemeState? state)) continue;
            if (NetworkGuard.IsAlive(state.Graphic)) RestoreGraphicIfThemeOwned(state, preserveCurrentAlpha: true);
            _graphics.Remove(id);
            _passCleaned++;
        }

        _scratchIds.Clear();
        foreach (KeyValuePair<int, SelectableThemeState> pair in _selectables)
        {
            SelectableThemeState state = pair.Value;
            if (!NetworkGuard.IsAlive(state.Selectable))
            {
                _scratchIds.Add(pair.Key);
                continue;
            }
            try
            {
                Graphic? target = state.Selectable.targetGraphic;
                if (NetworkGuard.IsAlive(target) && IsWithin(target!.transform, root)) _scratchIds.Add(pair.Key);
            }
            catch { _scratchIds.Add(pair.Key); }
        }
        for (int i = 0; i < _scratchIds.Count; i++)
        {
            int id = _scratchIds[i];
            if (!_selectables.TryGetValue(id, out SelectableThemeState? state)) continue;
            if (NetworkGuard.IsAlive(state.Selectable)) RestoreSelectableIfThemeOwned(state);
            _selectables.Remove(id);
            _passCleaned++;
        }

        _scratchIds.Clear();
        foreach (KeyValuePair<int, Graphic> pair in _protectedGraphics)
        {
            if (!NetworkGuard.IsAlive(pair.Value) || IsWithin(pair.Value.transform, root)) _scratchIds.Add(pair.Key);
        }
        for (int i = 0; i < _scratchIds.Count; i++)
        {
            if (_protectedGraphics.Remove(_scratchIds[i])) _passCleaned++;
        }
        _scratchIds.Clear();
    }

    private static bool IsVanillaSubtree(Transform? target)
    {
        if (!NetworkGuard.IsAlive(target)) return false;
        for (int i = _vanillaRoots.Count - 1; i >= 0; i--)
        {
            Transform? root = _vanillaRoots[i];
            if (!NetworkGuard.IsAlive(root))
            {
                _vanillaRoots.RemoveAt(i);
                _passCleaned++;
                continue;
            }
            try
            {
                if (SameObject(target, root) || target!.IsChildOf(root!)) return true;
            }
            catch { return true; } // fail closed if native ancestry is uncertain
        }
        return false;
    }

    private static bool IsWithin(Transform target, Transform root)
    {
        try { return SameObject(target, root) || target.IsChildOf(root); }
        catch { return false; }
    }

    /// <summary>Restores tracked values only when they still match the last theme assignment.</summary>
    public static void RestoreAll()
    {
        foreach (GraphicThemeState state in _graphics.Values)
            RestoreGraphicIfThemeOwned(state, preserveCurrentAlpha: false);
        foreach (SelectableThemeState state in _selectables.Values)
            RestoreSelectableIfThemeOwned(state);
        ClearTracking();
    }

    /// <summary>Scene unload: objects die with the scene, so drop references without touching them.</summary>
    public static void HandleSceneUnload() => ClearTracking();

    private static void ClearTracking()
    {
        _graphics.Clear();
        _selectables.Clear();
        _protectedGraphics.Clear();
        _ownRoots.Clear();
        _vanillaRoots.Clear();
        _scratchIds.Clear();
        _nextCachePruneTime = 0f;
    }

    private static void RestoreGraphicIfThemeOwned(GraphicThemeState state, bool preserveCurrentAlpha)
    {
        try
        {
            Graphic graphic = state.Graphic;
            if (!NetworkGuard.IsAlive(graphic)) return;
            Color current = graphic.color;
            if (!SameRgb(current, state.LastTheme)) return;
            Color restore = preserveCurrentAlpha
                ? new Color(state.Original.r, state.Original.g, state.Original.b, current.a)
                : state.Original;
            if (!SameColor(current, restore)) graphic.color = restore;
        }
        catch { /* destroyed during restore */ }
    }

    private static void RestoreSelectableIfThemeOwned(SelectableThemeState state)
    {
        try
        {
            if (!NetworkGuard.IsAlive(state.Selectable)) return;
            if (SameColorBlock(state.Selectable.colors, state.LastTheme))
                state.Selectable.colors = state.Original;
        }
        catch { /* destroyed during restore */ }
    }

    // ------------------------------------------------------------------
    // Own-UI registration (called by InboxUI around every build)
    // ------------------------------------------------------------------

    /// <summary>Registers the roots of our injected UI; every sweep skips their subtrees.</summary>
    public static void SetOwnRoots(Transform? toolbar, Transform? menu, Transform? modal, Transform? hidden)
    {
        _ownRoots.Clear();
        AddOwnRoot(toolbar);
        AddOwnRoot(menu);
        AddOwnRoot(modal);
        AddOwnRoot(hidden);
    }

    /// <summary>Forgets the own-UI roots (a rebuild re-registers them).</summary>
    public static void ClearOwnRoots()
    {
        _ownRoots.Clear();
    }

    private static void AddOwnRoot(Transform? root)
    {
        if (!NetworkGuard.IsAlive(root)) return;
        for (int i = 0; i < _ownRoots.Count; i++)
        {
            Transform? existing = _ownRoots[i];
            if (NetworkGuard.IsAlive(existing) && SameObject(existing, root)) return;
        }
        _ownRoots.Add(root!);
    }

    private static bool IsOwnedByMod(Transform t)
    {
        for (int i = _ownRoots.Count - 1; i >= 0; i--)
        {
            Transform? root = _ownRoots[i];
            if (!NetworkGuard.IsAlive(root))
            {
                _ownRoots.RemoveAt(i);
                _passCleaned++;
                continue;
            }
            try
            {
                if (SameObject(t, root) || t.IsChildOf(root)) return true;
            }
            catch { return true; } // fail closed: never theme our UI on uncertain ancestry
        }
        return false;
    }

    // ------------------------------------------------------------------
    // Specific surfaces
    // ------------------------------------------------------------------

    private static void TintEntry(MSGConversation conv)
    {
        RectTransform? entry = ConversationUtils.SafeEntry(conv);
        if (!NetworkGuard.IsAlive(entry)) return;
        GameObject go = entry!.gameObject;

        Image? background = FindEntryBackground(entry);
        if (!NetworkGuard.IsAlive(background)) return;
        if (IsNeutralLightSurface(background!.color) || IsTrackedGraphic(background))
            TintGraphic(background, GamePalette.Card, ThemeRole.InboxRowBackground);
        if (!IsThemeSurfaceDark(background, ThemeRole.InboxRowBackground, GamePalette.Card)) return;

        Graphic? preview = null;
        try { preview = AsGraphic(conv.entryPreviewText); } catch { /* keep null */ }
        string contactName = ConversationUtils.SafeName(conv).Trim();

        Text[] legacyTexts = go.GetComponentsInChildren<Text>(true);
        if (legacyTexts != null)
        {
            for (int i = 0; i < legacyTexts.Length; i++)
            {
                Text? text = legacyTexts[i];
                if (!NetworkGuard.IsAlive(text) || IsOwnedByMod(text!.transform) || IsVanillaSubtree(text.transform) || IsProtectedText(text)) continue;
                bool isPreview = preview != null && SameObject(text, preview);
                string value;
                try { value = (text.text ?? string.Empty).Trim(); } catch { continue; }
                if (isPreview && IsDarkNeutral(text.color))
                    TintGraphic(text, GamePalette.TextMuted, ThemeRole.SecondaryText);
                else if (!isPreview && IsConversationName(value, contactName) && IsDarkNeutral(text.color))
                    TintGraphic(text, GamePalette.TextPrimary, ThemeRole.PrimaryText);
            }
        }

        TextMeshProUGUI[] tmpTexts = go.GetComponentsInChildren<TextMeshProUGUI>(true);
        if (tmpTexts != null)
        {
            for (int i = 0; i < tmpTexts.Length; i++)
            {
                TextMeshProUGUI? text = tmpTexts[i];
                if (!NetworkGuard.IsAlive(text) || IsOwnedByMod(text!.transform) || IsVanillaSubtree(text.transform) || IsProtectedText(text)) continue;
                bool isPreview = preview != null && SameObject(text, preview);
                string value;
                try { value = (text.text ?? string.Empty).Trim(); } catch { continue; }
                if (isPreview && IsDarkNeutral(text.color))
                    TintGraphic(text, GamePalette.TextMuted, ThemeRole.SecondaryText);
                else if (!isPreview && IsConversationName(value, contactName) && IsDarkNeutral(text.color))
                    TintGraphic(text, GamePalette.TextPrimary, ThemeRole.PrimaryText);
            }
        }
    }

    private static Image? FindEntryBackground(RectTransform entry)
    {
        float width;
        float height;
        try { width = Mathf.Abs(entry.rect.width); height = Mathf.Abs(entry.rect.height); }
        catch { return null; }
        if (width <= 1f || height <= 1f) return null;

        Image? rootImage = null;
        try { rootImage = entry.GetComponent<Image>(); } catch { /* not an image row */ }
        if (IsEntryBackgroundCandidate(rootImage, entry, width, height, rootImage: true)) return rootImage;

        Image[] images = entry.GetComponentsInChildren<Image>(true);
        if (images == null) return null;
        Image? best = null;
        float bestArea = 0f;
        for (int i = 0; i < images.Length; i++)
        {
            Image? image = images[i];
            if (!IsEntryBackgroundCandidate(image, entry, width, height, rootImage: false)) continue;
            float area;
            try { area = Mathf.Abs(image!.rectTransform.rect.width * image.rectTransform.rect.height); }
            catch { continue; }
            if (area <= bestArea) continue;
            best = image;
            bestArea = area;
        }
        return best;
    }

    private static bool IsEntryBackgroundCandidate(Image? image, RectTransform entry, float width, float height, bool rootImage)
    {
        if (!NetworkGuard.IsAlive(image)) return false;
        try
        {
            if (IsOwnedByMod(image!.transform) || IsVanillaSubtree(image.transform) || IsProtectedContentName(image.name)) return false;
            if (!Covers(image.rectTransform, width * 0.90f, height * 0.90f)) return false;
            if (!IsNeutralSurfaceColor(image.color) && !IsTrackedGraphic(image)) return false;
            if (rootImage) return true; // the referenced inbox row's own Graphic
            if (image.sprite != null && !IsBackgroundName(image.name)) return false;
            return IsBackgroundName(image.name) ||
                   (image.sprite == null && SameObject(image.transform.parent, entry.transform));
        }
        catch { return false; }
    }

    private static bool IsNeutralSurfaceColor(Color color) =>
        IsNeutralLightSurface(color) || IsDarkNeutralSurface(color);

    private static void TintBubbles(MSGConversation conv)
    {
        var bubbles = conv._bubbles; // 0.4.7f6: the field is underscored
        if (bubbles == null) return;
        int count = bubbles.Count;
        for (int i = 0; i < count; i++)
        {
            MessageBubble? bubble = bubbles[i];
            if (bubble == null || !NetworkGuard.IsAlive(bubble)) continue;

            Graphic? bubbleGraphic = null;
            try { bubbleGraphic = AsGraphic(bubble.bubble); } catch { /* skip */ }
            if (bubbleGraphic == null) continue;

            Graphic? content = null;
            try { content = AsGraphic(bubble.content); } catch { /* skip */ }

            // Alignment is semantic and remains stable after theming changes the bubble colour.
            // Only incoming left bubbles are themed; player/centered bubbles stay vanilla.
            if (bubble.alignment != MessageBubble.Alignment.Left) continue;

            if (IsNeutralLightSurface(bubbleGraphic.color) || IsTrackedGraphic(bubbleGraphic))
                TintGraphic(bubbleGraphic, GamePalette.CardAlt, ThemeRole.IncomingBubble);
            if (!IsThemeSurfaceDark(bubbleGraphic, ThemeRole.IncomingBubble, GamePalette.CardAlt)) continue;

            // The little bubble triangles echo the bubble colour — only neutral
            // triangle graphics may follow the known incoming-bubble background.
            try
            {
                Graphic? left = AsGraphic(bubble.triangle_Left);
                if (NetworkGuard.IsAlive(left) && (IsNeutralLightSurface(left!.color) || IsTrackedGraphic(left)))
                    TintGraphic(left, GamePalette.CardAlt, ThemeRole.IncomingBubble);
            }
            catch { /* skip */ }
            try
            {
                Graphic? right = AsGraphic(bubble.triangle_Right);
                if (NetworkGuard.IsAlive(right) && (IsNeutralLightSurface(right!.color) || IsTrackedGraphic(right)))
                    TintGraphic(right, GamePalette.CardAlt, ThemeRole.IncomingBubble);
            }
            catch { /* skip */ }

            // MessageBubble.content is a verified legacy Text component. Only dark
            // neutral text is lifted; colored status text and content art are untouched.
            if (content != null && !IsProtectedText(content) && IsDarkNeutral(content.color))
                TintGraphic(content, GamePalette.TextPrimary, ThemeRole.PrimaryText);
        }
    }

    /// <summary>
    /// Themes the open conversation's chat body — the large vanilla-white area
    /// behind the message bubbles.
    ///
    /// Verified against the vanilla prefab hierarchy (Schedule I 0.4.7f12,
    /// sharedassets0 "Conversation"): the white fill is the ScrollRect's
    /// <c>viewport</c> Graphic — rgb(1,1,1,1) opaque, raycast target, the node
    /// that actually covers the chat area. <c>scrollRectContainer</c> and
    /// <c>bubbleContainer</c> are bare <see cref="RectTransform"/>s with no
    /// Graphic of their own, so the old code silently tinted nothing here.
    ///
    /// Resolution order (each guarded, first hit wins):
    ///  1. <c>conv.scrollRect.viewport</c> — the verified vanilla surface.
    ///  2. The ScrollRect's own Graphic (reference lost / viewport not ready).
    ///  3. The legacy container fields (kept as fallback + diagnostic proof).
    /// </summary>
    private static void TintChatBodyBackground(MSGConversation conv)
    {
        bool handled = false;

        // (1) Verified path: the ScrollRect viewport paints the chat body.
        var scroll = conv.scrollRect;
        if (NetworkGuard.IsAlive(scroll))
        {
            RectTransform? viewport = null;
            try { viewport = scroll!.viewport; } catch { /* not ready */ }
            if (NetworkGuard.IsAlive(viewport))
            {
                handled = true;
                TintChatBodyCandidate(viewport!.gameObject, "scrollRect.viewport");
            }
            else
            {
                handled = true;
                TintChatBodyCandidate(scroll!.gameObject, "scrollRect");
            }
        }

        if (!handled)
        {
            // (2) The conversation reference was not populated yet — locate the
            //     ScrollRect inside the conversation's own container subtree
            //     (one small, bounded scan — NOT a global canvas scan).
            var found = FindChatScrollRect(conv);
            if (NetworkGuard.IsAlive(found))
            {
                handled = true;
                RectTransform? viewport = null;
                try { viewport = found!.viewport; } catch { /* not ready */ }
                if (NetworkGuard.IsAlive(viewport))
                    TintChatBodyCandidate(viewport!.gameObject, "scrollRect.viewport");
                else
                    TintChatBodyCandidate(found!.gameObject, "scrollRect");
            }
        }

        // (3) Legacy container references — in the vanilla prefab these are bare
        //     RectTransforms (no Graphic), which is exactly the diagnostic that
        //     proves the viewport above is the real surface.
        TintChatBodyCandidate(AsGameObject(conv.scrollRectContainer), "scrollRectContainer");
        TintChatBodyCandidate(AsGameObject(conv.bubbleContainer), "bubbleContainer");

        VerifyChatBodyVisible(conv);
    }

    /// <summary>
    /// Finds the chat ScrollRect inside the conversation's own subtree. Bounded to
    /// the conversation container (a handful of nodes) — never a full canvas scan,
    /// and only used when the direct field reference is not populated yet.
    /// </summary>
    private static ScrollRect? FindChatScrollRect(MSGConversation conv)
    {
        try
        {
            GameObject? root = AsGameObject(conv.container);
            if (!NetworkGuard.IsAlive(root)) return null;
            ScrollRect[] all = root!.GetComponentsInChildren<ScrollRect>(true);
            if (all == null || all.Length == 0) return null;
            // Prefer the one that is active and actually has a viewport.
            for (int i = 0; i < all.Length; i++)
            {
                ScrollRect? candidate = all[i];
                if (!NetworkGuard.IsAlive(candidate)) continue;
                try
                {
                    if (candidate!.viewport != null && NetworkGuard.IsAlive(candidate!.viewport))
                        return candidate;
                }
                catch { /* not ready */ }
            }
            return all.Length > 0 && NetworkGuard.IsAlive(all[0]) ? all[0] : null;
        }
        catch { return null; }
    }

    // Absolute floor for a chat-body surface (px). A frame whose layout is not
    // built yet reports 0 — that is "unknown", never a rejection (handled by Covers).
    private const float MinChatWidthPx = 40f;
    private const float MinChatHeightPx = 40f;

    /// <summary>
    /// Tints one identified chat-body surface: a full-size, near-neutral,
    /// NON-selectable Graphic that belongs to the live conversation viewport.
    /// Every decision is recorded for the throttled pass diagnostics.
    /// </summary>
    private static void TintChatBodyCandidate(GameObject? node, string label)
    {
        if (!NetworkGuard.IsAlive(node)) { NoteChatCandidate(label, ChatSkip.NoGraphic); return; }
        try
        {
            if (IsOwnedByMod(node!.transform)) { NoteChatCandidate(label, ChatSkip.OwnedByMod); return; }
            if (IsVanillaSubtree(node.transform)) { NoteChatCandidate(label, ChatSkip.VanillaSubtree); return; }

            Graphic? graphic = null;
            try { graphic = node.GetComponent<Graphic>(); } catch { /* not ready */ }
            if (!NetworkGuard.IsAlive(graphic)) { NoteChatCandidate(label, ChatSkip.NoGraphic); return; }

            if (IsProtectedContentName(graphic!.name))
            { FinishCandidate(label, node, ChatSkip.ProtectedName, graphic, graphic.color, 0f, 0f); return; }

            // A chat body is never a click target; skip selectable rows/buttons
            // so a row background can never be mistaken for the chat surface.
            if (HasSelectableTarget(graphic))
            { FinishCandidate(label, node, ChatSkip.Selectable, graphic, graphic.color, 0f, 0f); return; }

            RectTransform rt = graphic.rectTransform;
            float w = 0f;
            float h = 0f;
            try { w = Mathf.Abs(rt.rect.width); h = Mathf.Abs(rt.rect.height); }
            catch { /* deferred layout */ }

            bool tracked = IsTrackedGraphic(graphic);
            if (!IsNeutralLightSurface(graphic.color) && !tracked)
            { FinishCandidate(label, node, ChatSkip.NotNeutralColor, graphic, graphic.color, w, h); return; }
            // Size gate only for the generic fallback labels. The verified viewport
            // reference is trusted on its own: Unity computes its rect at layout
            // time (the prefab stores 0x0), so a 0 reading means "not laid out
            // yet", never "too small".
            if (!tracked && label != "scrollRect.viewport" && !Covers(rt, MinChatWidthPx, MinChatHeightPx))
            { FinishCandidate(label, node, ChatSkip.WrongSize, graphic, graphic.color, w, h); return; }

            Color before = graphic.color;
            TintGraphic(graphic, GamePalette.Bg, ThemeRole.PageBackground);
            _diagChatTinted++;
            FinishCandidate(label, node, ChatSkip.Tinted, graphic, before, w, h);
        }
        catch (Exception ex)
        {
            Mod.Log?.Debug($"AppTheme {label}: {ex.Message}");
            NoteChatCandidate(label, ChatSkip.NoGraphic);
        }
    }

    /// <summary>Counts one candidate and emits detail only when it is worth a line.</summary>
    private static void FinishCandidate(string label, GameObject? node, ChatSkip reason, Graphic? graphic, Color before, float w, float h)
    {
        NoteChatCandidate(label, reason);
        if (label == "scrollRect.viewport" && NetworkGuard.IsAlive(node)) _diagLastChatNode = DescribeNode(node);
        LogChatCandidateDetail(label, node, reason, graphic, before, w, h);
    }

    /// <summary>True when any Selectable up the hierarchy targets this Graphic (a button/row — never a body).</summary>
    private static bool HasSelectableTarget(Graphic graphic)
    {
        try
        {
            Selectable[] selectables = graphic.GetComponentsInParent<Selectable>(true);
            if (selectables == null || selectables.Length == 0) return false;
            int id = graphic.GetInstanceID();
            for (int i = 0; i < selectables.Length; i++)
            {
                Selectable? selectable = selectables[i];
                if (!NetworkGuard.IsAlive(selectable)) continue;
                Graphic? target = null;
                try { target = selectable!.targetGraphic; } catch { continue; }
                if (!NetworkGuard.IsAlive(target) || target!.GetInstanceID() != id || !SameObject(target, graphic)) continue;
                return true;
            }
            return false;
        }
        catch { return true; } // fail closed: uncertain ancestry is never a chat body
    }

    /// <summary>
    /// Re-reads the verified surface after the pass. If it is light again although
    /// we own it, vanilla UI logic overwrote our colour this frame (diagnostic 5).
    /// </summary>
    private static void VerifyChatBodyVisible(MSGConversation conv)
    {
        try
        {
            var scroll = conv.scrollRect;
            if (!NetworkGuard.IsAlive(scroll)) return;
            RectTransform? viewport = null;
            try { viewport = scroll!.viewport; } catch { /* not ready */ }
            if (!NetworkGuard.IsAlive(viewport)) return;
            Graphic? graphic = null;
            try { graphic = viewport!.gameObject.GetComponent<Graphic>(); } catch { return; }
            if (!NetworkGuard.IsAlive(graphic)) return;

            Color current = graphic!.color;
            if (IsDarkNeutralSurface(current) || SameRgb(current, GamePalette.Bg)) return;
            if (IsTrackedGraphic(graphic)) _diagChatLost++;
        }
        catch { /* diagnostics only */ }
    }

    private enum ChatSkip { NoGraphic, OwnedByMod, VanillaSubtree, ProtectedName, NotNeutralColor, Selectable, WrongSize, Tinted }

    private static void NoteChatCandidate(string label, ChatSkip reason)
    {
        _diagChatCandidates++;
        switch (reason)
        {
            case ChatSkip.NoGraphic: _diagChatNoGraphic++; break;
            case ChatSkip.Tinted: _diagChatTinted++; break;
            default: _diagChatSkipped++; break;
        }
    }

    /// <summary>
    /// Full per-candidate detail. Logged only for the VERIFIED surface
    /// (<c>scrollRect.viewport</c>) or for any rejected candidate — never for the
    /// two known-empty legacy containers, so a pass emits at most a couple of
    /// lines and never one per frame.
    /// </summary>
    private static void LogChatCandidateDetail(string label, GameObject? node, ChatSkip reason, Graphic? graphic, Color before, float w, float h)
    {
        bool verbose = label == "scrollRect.viewport" || reason != ChatSkip.NoGraphic;
        if (!verbose) return;

        string path = DescribeNode(node);
        string graphicInfo = "none";
        string colorInfo = "n/a";
        string after = "n/a";
        if (NetworkGuard.IsAlive(graphic))
        {
            graphicInfo = $"{graphic!.GetType().Name} '{graphic.name}' #{graphic.GetInstanceID()}";
            colorInfo = $"rgb=({before.r:F3},{before.g:F3},{before.b:F3}) a={before.a:F3}";
            try { Color now = graphic.color; after = $"rgb=({now.r:F3},{now.g:F3},{now.b:F3}) a={now.a:F3}"; }
            catch { /* destroyed */ }
        }
        string why = reason switch
        {
            ChatSkip.NoGraphic => "no Graphic on the node",
            ChatSkip.OwnedByMod => "inside our injected UI",
            ChatSkip.VanillaSubtree => "inside a preserved vanilla popup",
            ChatSkip.ProtectedName => "protected by name guard",
            ChatSkip.NotNeutralColor => "not a neutral light surface (and not tracked)",
            ChatSkip.Selectable => "is a selectable/button surface",
            ChatSkip.WrongSize => $"too small ({w:F0}x{h:F0}px)",
            ChatSkip.Tinted => "tinted (or re-applied)",
            _ => "unknown"
        };
        Mod.Log?.Debug($"AppTheme chat[{label}] {path} :: {graphicInfo} {colorInfo} size={w:F0}x{h:F0} :: {why} -> after={after}");
    }

    private static string DescribeNode(GameObject? node)
    {
        if (!NetworkGuard.IsAlive(node)) return "<dead>";
        try
        {
            Transform t = node!.transform;
            string path = t.name;
            for (int depth = 0; depth < 6; depth++)
            {
                Transform? parent = null;
                try { parent = t.parent; } catch { break; }
                if (!NetworkGuard.IsAlive(parent)) break;
                path = parent!.name + "/" + path;
                t = parent;
            }
            return path;
        }
        catch { return "<err>"; }
    }

    private static void ResetDiagCounters()
    {
        _diagChatCandidates = 0;
        _diagChatTinted = 0;
        _diagChatNoGraphic = 0;
        _diagChatSkipped = 0;
        _diagChatLost = 0;
        _diagPageBackgroundsTinted = 0;
        _diagLastChatNode = string.Empty;
    }

    /// <summary>One line per pass — never per frame, and only when something is worth reporting.</summary>
    private static void LogChatDiagnostics()
    {
        if (_diagChatCandidates == 0) return;
        bool interesting = _diagChatTinted > 0 || _diagChatNoGraphic == _diagChatCandidates || _diagChatLost > 0;
        if (!interesting) return;
        Mod.Log?.Debug($"AppTheme chat body: candidates={_diagChatCandidates} tinted={_diagChatTinted} " +
                       $"noGraphic={_diagChatNoGraphic} skipped={_diagChatSkipped} " +
                       $"overwritten={_diagChatLost} pageBgTinted={_diagPageBackgroundsTinted} " +
                       $"surface='{_diagLastChatNode}'");
    }

    /// <summary>Preserves the vanilla response/counter-offer menu and its original colors.</summary>
    private static void PreserveResponseArea(MSGConversation conv)
    {
        try
        {
            RectTransform? container = conv.responseContainer;
            if (container != null && NetworkGuard.IsAlive(container))
            {
                PreserveVanillaSubtree(container.gameObject);
            }
        }
        catch { /* response UI may not exist yet */ }
    }

    /// <summary>Theme the known dialogue header, its exact title reference, and monochrome back icon.</summary>
    private static void TintDialogueHeader(MessagesApp app)
    {
        GameObject? page = null;
        try { page = AsGameObject(app.dialoguePage); } catch { /* skip */ }
        if (!NetworkGuard.IsAlive(page)) return;

        RectTransform? pageRt = null;
        try { pageRt = page!.GetComponent<RectTransform>(); } catch { /* not ready */ }
        if (!NetworkGuard.IsAlive(pageRt)) return;
        float pageWidth = Mathf.Abs(pageRt!.rect.width);
        float pageHeight = Mathf.Abs(pageRt.rect.height);
        if (pageWidth <= 1f || pageHeight <= 1f) return;

        Text? title = null;
        try { title = app.dialoguePageNameText; } catch { /* optional */ }
        Image? header = null;

        // Prefer the known title reference and its nearby surface before using geometry.
        if (NetworkGuard.IsAlive(title))
        {
            Transform? ancestor = title!.transform;
            for (int depth = 0; depth < 5 && NetworkGuard.IsAlive(ancestor); depth++)
            {
                Image? candidate = null;
                try { candidate = ancestor!.GetComponent<Image>(); } catch { /* skip */ }
                if (IsHeaderCandidate(candidate, pageRt, pageWidth, pageHeight))
                {
                    header = candidate;
                    break;
                }
                if (SameObject(ancestor, page!.transform)) break;
                try { ancestor = ancestor!.parent; } catch { break; }
            }
        }

        if (!NetworkGuard.IsAlive(header))
        {
            Image[] images = page!.GetComponentsInChildren<Image>(true);
            if (images == null) return;
            float bestArea = 0f;
            for (int i = 0; i < images.Length; i++)
            {
                Image? image = images[i];
                if (!IsHeaderCandidate(image, pageRt, pageWidth, pageHeight)) continue;
                float area;
                try { area = Mathf.Abs(image!.rectTransform.rect.width * image.rectTransform.rect.height); }
                catch { continue; }
                if (area <= bestArea) continue;
                header = image;
                bestArea = area;
            }
        }
        if (!NetworkGuard.IsAlive(header)) return;

        TintGraphic(header!, GamePalette.Header, ThemeRole.Header);
        if (!IsThemeSurfaceDark(header, ThemeRole.Header, GamePalette.Header)) return;

        if (NetworkGuard.IsAlive(title) && IsDarkNeutral(title!.color))
            TintGraphic(title, GamePalette.TextPrimary, ThemeRole.PrimaryText);

        Transform? container = header!.transform.parent;
        if (!NetworkGuard.IsAlive(container)) return;
        TintSmallDarkIconsIn(container!.gameObject, pageWidth, pageHeight);
    }

    private static bool IsHeaderCandidate(Image? image, RectTransform pageRt, float pageWidth, float pageHeight)
    {
        if (!NetworkGuard.IsAlive(image)) return false;
        try
        {
            if (IsOwnedByMod(image!.transform) || IsVanillaSubtree(image.transform) || IsProtectedContentName(image.name)) return false;
            if (!IsNeutralSurfaceColor(image.color) && !IsTrackedGraphic(image)) return false;
            if (image.sprite != null && !IsBackgroundName(image.name)) return false;

            RectTransform rect = image.rectTransform;
            float width = Mathf.Abs(rect.rect.width);
            float height = Mathf.Abs(rect.rect.height);
            return width >= pageWidth * 0.70f && height >= pageHeight * 0.04f &&
                   height <= pageHeight * 0.30f && IsInTopHalf(pageRt, rect);
        }
        catch { return false; }
    }

    // ------------------------------------------------------------------
    // Known page backgrounds
    // ------------------------------------------------------------------

    private static void ThemePageBackgrounds(MessagesApp app)
    {
        try { ThemePageBackground(app.homePage); } catch { /* page may not exist */ }
        try { ThemePageBackground(AsGameObject(app.dialoguePage)); } catch { /* page may not exist */ }
    }

    /// <summary>
    /// Themes the single largest near-white page background of a vanilla page.
    ///
    /// Thresholds are derived from the real vanilla geometry, not guesses: on the
    /// DialogueScreen the page is stretched to the screen size while its
    /// conversation area is 100 px shorter (the Topbar). A hard 0.85 cut therefore
    /// rejected the real background — the coverage floor is 0.80 and the sprite
    /// guard stays. Icons, avatars and small elements can never pass 0.80 anyway.
    /// Selectable surfaces are excluded so a clickable row is never mistaken for
    /// the page background.
    /// </summary>
    private static void ThemePageBackground(GameObject? page)
    {
        if (!NetworkGuard.IsAlive(page) || IsOwnedByMod(page!.transform) || IsVanillaSubtree(page.transform)) return;
        RectTransform? pageRt = null;
        try { pageRt = page.GetComponent<RectTransform>(); } catch { /* not ready */ }
        if (!NetworkGuard.IsAlive(pageRt)) return;
        float width = Mathf.Abs(pageRt!.rect.width);
        float height = Mathf.Abs(pageRt.rect.height);
        if (width <= 1f || height <= 1f) return;

        Image[] images = page.GetComponentsInChildren<Image>(true);
        if (images == null) return;
        Image? best = null;
        float bestArea = 0f;
        for (int i = 0; i < images.Length; i++)
        {
            Image? image = images[i];
            if (!NetworkGuard.IsAlive(image)) continue;
            try
            {
                if (IsOwnedByMod(image!.transform) || IsVanillaSubtree(image.transform) || IsProtectedContentName(image.name)) continue;
                RectTransform rect = image.rectTransform;
                if (!Covers(rect, width * 0.80f, height * 0.80f)) continue;
                if (image.sprite != null && !IsBackgroundName(image.name) && !SameObject(image.transform, page.transform)) continue;
                if (!IsNeutralLightSurface(image.color) && !IsTrackedGraphic(image)) continue;
                // A page background is never a click target — never recolor a row/button here.
                if (HasSelectableTarget(image)) continue;

                float area = Mathf.Abs(rect.rect.width * rect.rect.height);
                if (area <= bestArea) continue;
                best = image;
                bestArea = area;
            }
            catch { /* skip invalid proxies */ }
        }

        if (NetworkGuard.IsAlive(best))
        {
            TintGraphic(best, GamePalette.Bg, ThemeRole.PageBackground);
            _diagPageBackgroundsTinted++;
        }
    }

    /// <summary>Only named monochrome back/arrow icons in the header are lightened.</summary>
    private static void TintSmallDarkIconsIn(GameObject root, float pageW, float pageH)
    {
        Image[] images = root.GetComponentsInChildren<Image>(true);
        if (images == null) return;
        for (int i = 0; i < images.Length; i++)
        {
            Image? image = images[i];
            if (!NetworkGuard.IsAlive(image)) continue;
            try
            {
                string name = image!.name.ToLowerInvariant();
                if (IsOwnedByMod(image.transform) || IsVanillaSubtree(image.transform) || IsProtectedContentName(name)) continue;
                if (!((name.Contains("back") && !name.Contains("background")) || name.Contains("arrow"))) continue;

                RectTransform rect = image.rectTransform;
                float width = Mathf.Abs(rect.rect.width);
                float height = Mathf.Abs(rect.rect.height);
                if (width > pageW * 0.15f || height > pageH * 0.15f) continue;
                if (!IsDarkNeutral(image.color)) continue;
                TintGraphic(image, GamePalette.TextPrimary, ThemeRole.MonochromeIcon);
            }
            catch { /* invalid proxy */ }
        }
    }

    // ------------------------------------------------------------------
    // Primitives
    // ------------------------------------------------------------------

    private static bool Covers(RectTransform? rt, float minW, float minH)
    {
        if (!NetworkGuard.IsAlive(rt)) return false;
        try { return Mathf.Abs(rt!.rect.width) >= minW && Mathf.Abs(rt.rect.height) >= minH; }
        catch { return false; }
    }

    private static bool IsNeutralLightSurface(Color color) =>
        color.a > 0.02f && Mathf.Min(color.r, Mathf.Min(color.g, color.b)) >= 0.60f && Spread(color) <= 0.08f;

    private static bool IsDarkNeutral(Color color) =>
        color.a > 0.02f && MaxChannel(color) <= 0.62f && Spread(color) <= 0.14f;

    private static bool IsDarkNeutralSurface(Color color) =>
        color.a > 0.02f && MaxChannel(color) <= 0.45f && Spread(color) <= 0.20f;

    private static float MaxChannel(Color color) => Mathf.Max(color.r, Mathf.Max(color.g, color.b));

    private static float Spread(Color color) => MaxChannel(color) - Mathf.Min(color.r, Mathf.Min(color.g, color.b));

    private static bool IsInTopHalf(RectTransform pageRt, RectTransform target)
    {
        try
        {
            float localY = pageRt.InverseTransformPoint(target.position).y;
            float centerY = (0.5f - pageRt.pivot.y) * pageRt.rect.height;
            return localY > centerY;
        }
        catch { return false; }
    }

    private static bool IsProtectedContentName(string name)
    {
        string lower = name.ToLowerInvariant();
        return lower.Contains("avatar") || lower.Contains("portrait") || lower.Contains("product") ||
               lower.Contains("item") || lower.Contains("status") || lower.Contains("relationship") ||
               lower.Contains("unread") || lower.Contains("badge");
    }

    private static bool IsProtectedText(Graphic? text)
    {
        if (!NetworkGuard.IsAlive(text)) return true;
        try
        {
            string name = text!.name.ToLowerInvariant();
            return IsProtectedContentName(name) || name.Contains("notice") || name.Contains("hint") ||
                   name.Contains("timestamp") || name.Contains("indicator");
        }
        catch { return true; }
    }

    private static bool IsBackgroundName(string name)
    {
        string lower = name.ToLowerInvariant();
        return lower.Contains("background") || lower == "bg" || lower.EndsWith("_bg", StringComparison.Ordinal) ||
               lower.Contains("header");
    }

    private static bool IsConversationName(string value, string contactName) =>
        value.Length > 0 && contactName.Length > 0 && string.Equals(value, contactName, StringComparison.Ordinal);

    private static bool IsThemeSurfaceDark(Graphic? graphic, ThemeRole role, Color target)
    {
        if (!NetworkGuard.IsAlive(graphic)) return false;
        try
        {
            Color current = graphic!.color;
            if (SameRgb(current, target) || IsDarkNeutralSurface(current)) return true;

            // During ColorTint fades, inspect the stable ColorBlock instead of
            // repeatedly assigning the interpolated Graphic color.
            Selectable[] selectables = graphic.GetComponentsInParent<Selectable>(true);
            if (selectables == null) return false;
            int graphicId = graphic.GetInstanceID();
            for (int i = 0; i < selectables.Length; i++)
            {
                Selectable? selectable = selectables[i];
                if (!NetworkGuard.IsAlive(selectable)) continue;
                Graphic? targetGraphic = null;
                try { targetGraphic = selectable!.targetGraphic; } catch { continue; }
                if (!NetworkGuard.IsAlive(targetGraphic) || targetGraphic!.GetInstanceID() != graphicId || !SameObject(targetGraphic, graphic)) continue;
                if (selectable!.transition != Selectable.Transition.ColorTint || !IsDarkThemeColorBlock(selectable.colors)) continue;

                int selectableId = selectable.GetInstanceID();
                if (_selectables.TryGetValue(selectableId, out SelectableThemeState? state) &&
                    NetworkGuard.IsAlive(state.Selectable) && SameObject(state.Selectable, selectable) && state.Role == role)
                    return true;
                return true; // an already-dark vanilla ColorBlock is also a safe text surface
            }
        }
        catch { /* uncertain native state: leave associated text untouched */ }
        return false;
    }

    private static bool IsDarkThemeColorBlock(ColorBlock block) =>
        IsDarkThemeStateColor(block.normalColor) && IsDarkThemeStateColor(block.highlightedColor) &&
        IsDarkThemeStateColor(block.pressedColor) && IsDarkThemeStateColor(block.selectedColor) &&
        IsDarkThemeStateColor(block.disabledColor);

    private static bool IsDarkThemeStateColor(Color color) =>
        MaxChannel(color) <= 0.45f && Spread(color) <= 0.20f;

    private static bool IsSupportedThemeColor(ThemeRole role, Color current) => role switch
    {
        ThemeRole.PageBackground or ThemeRole.Header or ThemeRole.InboxRowBackground or ThemeRole.IncomingBubble =>
            IsNeutralLightSurface(current),
        ThemeRole.PrimaryText or ThemeRole.SecondaryText or ThemeRole.MonochromeIcon => IsDarkNeutral(current),
        _ => false
    };

    private static void TintGraphic(Graphic? graphic, Color target, ThemeRole role)
    {
        if (!NetworkGuard.IsAlive(graphic) || role is ThemeRole.ProtectedContent or ThemeRole.StatusIndicator or ThemeRole.OutgoingContent)
            return;

        try
        {
            Transform transform = graphic!.transform;
            if (IsOwnedByMod(transform) || IsVanillaSubtree(transform)) return;
            int id = graphic.GetInstanceID();
            if (IsProtectedGraphic(id, graphic)) return;

            // Only known inbox-row ColorTint buttons are themed through ColorBlock.
            // All other transitions are left to Unity and never hard-set per frame.
            if (TryHandleSelectableTarget(graphic, id, role)) return;

            Color current = graphic.color;
            if (current.a <= 0.02f) return;
            bool hasState = TryGetGraphicState(id, graphic, out GraphicThemeState? state);
            if (!hasState && !IsSupportedThemeColor(role, current)) return;
            Color original = hasState ? state!.Original : current;
            Color lastTheme = hasState ? state!.LastTheme : default;
            if (!ThemeColorPolicy.ShouldWrite(hasState, role, ToThemeRgb(original), ToThemeRgb(lastTheme),
                    ToThemeRgb(current), ToThemeRgb(target)))
            {
                if (hasState && SameRgb(current, target))
                {
                    state!.Role = role;
                    state.LastTheme = WithAlpha(target, current.a);
                }
                return;
            }

            Color themed = WithAlpha(target, current.a);
            graphic.color = themed;
            if (hasState)
            {
                state!.Role = role;
                state.LastTheme = themed;
                _passRepaired++;
            }
            else
            {
                _graphics[id] = new GraphicThemeState(graphic, current, themed, role);
                _passNew++;
            }
        }
        catch { /* IL2CPP object may be destroyed between liveness and access */ }
    }

    /// <summary>
    /// Themes a graphic that is the target of a vanilla <see cref="Selectable"/>.
    ///
    /// Returns false in the normal case so the caller assigns the graphic colour
    /// itself. Bailing out here (the old behaviour) left every selectable surface
    /// that is NOT a themeable inbox-row ColorTint button in its vanilla light
    /// colour permanently — those were the white artifacts no later pass fixed.
    ///
    /// Only the known inbox-row ColorTint buttons additionally get a themed
    /// ColorBlock, and even for those the graphic colour is written directly:
    /// Unity only pushes <c>colors.normalColor</c> onto the graphic during a
    /// state transition, so a ColorBlock-only theme left the row vanilla-white
    /// until the pointer happened to hover/click it ("applies later").
    /// </summary>
    private static bool TryHandleSelectableTarget(Graphic graphic, int graphicId, ThemeRole role)
    {
        Selectable[] selectables;
        try { selectables = graphic.GetComponentsInParent<Selectable>(true); }
        catch { return false; } // uncertain ancestry: let the normal tint path decide

        if (selectables == null || selectables.Length == 0) return false;

        bool ownsGraphic = false;
        for (int i = 0; i < selectables.Length; i++)
        {
            Selectable? selectable = selectables[i];
            if (!NetworkGuard.IsAlive(selectable)) continue;
            Graphic? target = null;
            try { target = selectable!.targetGraphic; } catch { continue; }
            if (!NetworkGuard.IsAlive(target) || target!.GetInstanceID() != graphicId || !SameObject(target, graphic)) continue;
            ownsGraphic = true;
            break;
        }
        if (!ownsGraphic) return false; // not a selectable target — normal tint path

        for (int i = 0; i < selectables.Length; i++)
        {
            Selectable? selectable = selectables[i];
            if (!NetworkGuard.IsAlive(selectable)) continue;
            try
            {
                Graphic? target = selectable!.targetGraphic;
                if (NetworkGuard.IsAlive(target) && target!.GetInstanceID() == graphicId && SameObject(target, graphic))
                    ThemeSelectable(selectable, role);
            }
            catch { /* leave malformed targets unchanged */ }
        }
        return false;
    }

    private static void ThemeSelectable(Selectable selectable, ThemeRole role)
    {
        try
        {
            if (!NetworkGuard.IsAlive(selectable) || selectable.transition != Selectable.Transition.ColorTint) return;
            int id = selectable.GetInstanceID();
            ColorBlock current = selectable.colors;
            bool hasState = TryGetSelectableState(id, selectable, out SelectableThemeState? state);
            ColorBlock original = hasState ? state!.Original : current;
            ColorBlock previous = hasState ? state!.LastTheme : default;
            if (!hasState && !IsNeutralColorBlock(current)) return;

            ColorBlock target = CreateThemedColorBlock(original);
            if (SameColorBlock(current, target)) return;
            if (hasState && !SameColorBlock(current, previous) && !SameColorBlock(current, original)) return;

            selectable.colors = target;
            if (hasState)
            {
                state!.Role = role;
                state.LastTheme = target;
                _passRepaired++;
            }
            else
            {
                _selectables[id] = new SelectableThemeState(selectable, current, target, role);
                _passButtons++;
            }
        }
        catch { /* destroyed or unavailable proxy */ }
    }

    private static ColorBlock CreateThemedColorBlock(ColorBlock original)
    {
        ColorBlock themed = original; // preserves fade duration and multiplier
        themed.normalColor = WithAlpha(GamePalette.Card, original.normalColor.a);
        themed.highlightedColor = WithAlpha(GamePalette.CardHover, original.highlightedColor.a);
        themed.pressedColor = WithAlpha(GamePalette.CardPressed, original.pressedColor.a);
        themed.selectedColor = WithAlpha(GamePalette.CardAlt, original.selectedColor.a);
        themed.disabledColor = WithAlpha(GamePalette.Bg, original.disabledColor.a); // distinct disabled state; preserve its alpha
        return themed;
    }

    private static bool IsNeutralColorBlock(ColorBlock block) =>
        IsNeutralStateColor(block.normalColor) && IsNeutralStateColor(block.highlightedColor) &&
        IsNeutralStateColor(block.pressedColor) && IsNeutralStateColor(block.selectedColor) &&
        IsNeutralStateColor(block.disabledColor);

    private static bool IsNeutralStateColor(Color color) => Spread(color) <= 0.08f;

    private static bool SameColorBlock(ColorBlock left, ColorBlock right) =>
        SameColor(left.normalColor, right.normalColor) &&
        SameColor(left.highlightedColor, right.highlightedColor) &&
        SameColor(left.pressedColor, right.pressedColor) &&
        SameColor(left.selectedColor, right.selectedColor) &&
        SameColor(left.disabledColor, right.disabledColor) &&
        Mathf.Abs(left.colorMultiplier - right.colorMultiplier) <= 0.002f &&
        Mathf.Abs(left.fadeDuration - right.fadeDuration) <= 0.002f;

    private static bool TryGetGraphicState(int id, Graphic graphic, out GraphicThemeState? state)
    {
        if (_graphics.TryGetValue(id, out state))
        {
            if (NetworkGuard.IsAlive(state.Graphic) && SameObject(state.Graphic, graphic)) return true;
            _graphics.Remove(id);
            _passCleaned++;
        }
        state = null;
        return false;
    }

    private static bool TryGetSelectableState(int id, Selectable selectable, out SelectableThemeState? state)
    {
        if (_selectables.TryGetValue(id, out state))
        {
            if (NetworkGuard.IsAlive(state.Selectable) && SameObject(state.Selectable, selectable)) return true;
            _selectables.Remove(id);
            _passCleaned++;
        }
        state = null;
        return false;
    }

    private static bool IsTrackedGraphic(Graphic graphic)
    {
        try { return TryGetGraphicState(graphic.GetInstanceID(), graphic, out _); }
        catch { return false; }
    }

    private static bool IsProtectedGraphic(int id, Graphic graphic)
    {
        if (!_protectedGraphics.TryGetValue(id, out Graphic? existing)) return false;
        if (NetworkGuard.IsAlive(existing) && SameObject(existing, graphic)) return true;
        _protectedGraphics.Remove(id);
        _passCleaned++;
        return false;
    }

    private static bool SameObject(Il2CppObjectBase? left, Il2CppObjectBase? right)
    {
        if (left == null || right == null) return false;
        try { return left.Pointer != IntPtr.Zero && left.Pointer == right.Pointer; }
        catch { return false; }
    }

    private static bool SameObject(UnityEngine.Object? left, UnityEngine.Object? right)
    {
        if (!NetworkGuard.IsAlive(left) || !NetworkGuard.IsAlive(right)) return false;
        try { return left!.GetInstanceID() == right!.GetInstanceID(); }
        catch { return false; }
    }

    private static ThemeRgb ToThemeRgb(Color color) => new(color.r, color.g, color.b);

    private static Color WithAlpha(Color color, float alpha) => new(color.r, color.g, color.b, alpha);

    private static bool SameRgb(Color left, Color right) =>
        Mathf.Abs(left.r - right.r) <= 0.002f &&
        Mathf.Abs(left.g - right.g) <= 0.002f &&
        Mathf.Abs(left.b - right.b) <= 0.002f;

    private static bool SameColor(Color left, Color right) =>
        SameRgb(left, right) && Mathf.Abs(left.a - right.a) <= 0.002f;

    private static void PruneTrackedCaches()
    {
        float now;
        try { now = Time.unscaledTime; } catch { return; }
        if (now < _nextCachePruneTime) return;
        _nextCachePruneTime = now + 20f;

        _scratchIds.Clear();
        foreach (KeyValuePair<int, GraphicThemeState> pair in _graphics)
            if (!NetworkGuard.IsAlive(pair.Value.Graphic)) _scratchIds.Add(pair.Key);
        for (int i = 0; i < _scratchIds.Count; i++)
            if (_graphics.Remove(_scratchIds[i])) _passCleaned++;

        _scratchIds.Clear();
        foreach (KeyValuePair<int, SelectableThemeState> pair in _selectables)
            if (!NetworkGuard.IsAlive(pair.Value.Selectable)) _scratchIds.Add(pair.Key);
        for (int i = 0; i < _scratchIds.Count; i++)
            if (_selectables.Remove(_scratchIds[i])) _passCleaned++;

        _scratchIds.Clear();
        foreach (KeyValuePair<int, Graphic> pair in _protectedGraphics)
            if (!NetworkGuard.IsAlive(pair.Value)) _scratchIds.Add(pair.Key);
        for (int i = 0; i < _scratchIds.Count; i++)
            if (_protectedGraphics.Remove(_scratchIds[i])) _passCleaned++;
        _scratchIds.Clear();
    }

    private static void PruneRoots()
    {
        for (int i = _vanillaRoots.Count - 1; i >= 0; i--)
        {
            if (NetworkGuard.IsAlive(_vanillaRoots[i])) continue;
            _vanillaRoots.RemoveAt(i);
            _passCleaned++;
        }
        for (int i = _ownRoots.Count - 1; i >= 0; i--)
        {
            if (NetworkGuard.IsAlive(_ownRoots[i])) continue;
            _ownRoots.RemoveAt(i);
            _passCleaned++;
        }
    }

    private static void LogPassSummary()
    {
        if (_passNew == 0 && _passRepaired == 0 && _passButtons == 0 && _passProtected == 0 && _passCleaned == 0) return;
        Mod.Log?.Debug($"AppTheme pass {_fullPassCount}: new={_passNew}, repaired={_passRepaired}, buttons={_passButtons}, protected={_passProtected}, cleaned={_passCleaned}.");
    }

    /// <summary>
    /// Anything Graphic-shaped (TMP text, Image, ...). Accepts GameObjects AND any
    /// other Unity <see cref="Component"/>: a vanilla field can point at a
    /// RectTransform that has no Graphic of its own, and only walking through its
    /// GameObject reaches the graphic the object actually draws with.
    /// </summary>
    private static Graphic? AsGraphic(Il2CppObjectBase? obj)
    {
        if (obj == null) return null;
        try
        {
            Graphic? graphic = obj.TryCast<Graphic>();
            if (graphic != null) return graphic;
            GameObject? go = obj.TryCast<GameObject>();
            if (go != null) return go.GetComponent<Graphic>();
            // General Component (RectTransform, Behaviour, ScrollRect, ...):
            // resolve to its GameObject, then read the Graphic from there.
            // IL2CPP-safe via TryCast — never a C# 'is'/'as' check on a proxy.
            Component? component = obj.TryCast<Component>();
            if (component != null)
            {
                GameObject? owner = component.gameObject;
                return owner != null ? owner.GetComponent<Graphic>() : null;
            }
            return null;
        }
        catch { return null; }
    }

    /// <summary>True while the app's home or dialogue page is actually shown (phone open).</summary>
    public static bool IsPageVisible(MessagesApp app)
    {
        if (!NetworkGuard.IsAlive(app)) return false;
        try
        {
            GameObject home = app.homePage;
            if (home != null && home.activeInHierarchy) return true;
        }
        catch { /* fall through */ }

        try
        {
            GameObject? dialogue = AsGameObject(app.dialoguePage);
            if (dialogue != null && dialogue.activeInHierarchy) return true;
        }
        catch { /* fall through */ }

        return false;
    }

    private static GameObject? AsGameObject(Il2CppObjectBase? obj)
    {
        if (obj == null) return null;
        try
        {
            GameObject? go = obj.TryCast<GameObject>();
            if (go != null) return go;
            Component? comp = obj.TryCast<Component>();
            return comp != null ? comp.gameObject : null;
        }
        catch { return null; }
    }
}
