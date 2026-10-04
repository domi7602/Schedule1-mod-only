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
/// Dark mode — recolors the VANILLA Messages surfaces so the WHOLE app (not just
/// the injected band) goes dark: page backgrounds, inbox rows, chat bubbles, the
/// dialogue header bar and the response panel. Everything is applied once per
/// graphic (instance-id tracked) and every original colour is cached — dormant
/// since v0.4.1 (dark mode is permanent), but a light restore would still hand
/// the game's own look back exactly.
///
/// Only colours are touched — never layouts, raycasts or save state. Guards:
///  - our own injected UI (band/menu/dialog) is excluded from every sweep;
///  - avatars (small, roughly square) are never tinted (they are content);
///  - coloured elements (status bars, badges, the unread dot, the player's
///    accent bubbles) stay as they are.
/// </summary>
internal static class AppTheme
{
    /// <summary>(instance id, graphic, original colour) of every vanilla graphic we touched.</summary>
    private static readonly List<(int Id, Graphic Graphic, Color Original)> _originals = new();

    /// <summary>Instance ids already themed once — we never fight vanilla re-colours after that.</summary>
    private static readonly HashSet<int> _themed = new();

    /// <summary>Graphic instance ids the sweeps must leave alone (texts of coloured bubbles).</summary>
    private static readonly HashSet<int> _skip = new();

    /// <summary>Roots of OUR injected UI — the vanilla sweeps must never touch their subtrees.</summary>
    private static readonly List<Transform> _ownRoots = new();

    /// <summary>Transient force scope: while set, already-themed graphics under this
    /// root may be re-tinted (a popup that just opened re-set some colours — the
    /// cached originals stay untouched, so light restore remains exact).</summary>
    private static Transform? _forceRoot;

    // ------------------------------------------------------------------
    // Apply / restore
    // ------------------------------------------------------------------

    /// <summary>Recolours the vanilla app surfaces (idempotent; safe to call from the tick).</summary>
    public static void Apply(MessagesApp app)
    {
        try
        {
            if (!NetworkGuard.IsAlive(app)) return;
            if (!IsPageVisible(app)) return; // phone closed / different app — nothing to do

            // 1. The open conversation: specific rules FIRST — TintGraphic is
            //    first-wins, so bubbles/response panel must beat the generic sweep.
            MSGConversation? current = null;
            try { current = app.currentConversation; } catch { /* skip */ }
            if (ConversationUtils.IsAlive(current))
            {
                try { TintBubbles(current!); }
                catch (Exception ex) { Mod.Log?.Debug($"AppTheme bubbles: {ex.Message}"); }

                try { TintResponseArea(current!); }
                catch (Exception ex) { Mod.Log?.Debug($"AppTheme responses: {ex.Message}"); }
            }

            // 2. Inbox rows (visible subset — lazily created entries theme on later ticks).
            try
            {
                var conversations = MessagesApp.ActiveConversations;
                if (conversations != null)
                {
                    int count = conversations.Count;
                    for (int i = 0; i < count; i++)
                    {
                        MSGConversation? conv = conversations[i];
                        if (!ConversationUtils.IsAlive(conv)) continue;
                        TintEntry(conv!);
                    }
                }
            }
            catch (Exception ex) { Mod.Log?.Debug($"AppTheme entries: {ex.Message}"); }

            // 3. The dialogue header bar (bar colour + its texts + small dark icons).
            try { TintDialogueHeader(app); }
            catch (Exception ex) { Mod.Log?.Debug($"AppTheme header: {ex.Message}"); }

            // 4. Generic sweep: every remaining near-white surface + dark neutral text.
            try { SweepAppSurfaces(app); }
            catch (Exception ex) { Mod.Log?.Debug($"AppTheme sweep: {ex.Message}"); }
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"AppTheme.Apply failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Immediate theme pass for a subtree that just became visible (e.g. the deal
    /// window popup). Runs the generic surface rules scoped to <paramref name="root"/>
    /// and force-re-applies colours the game re-set while opening/updating —
    /// without this, freshly shown vanilla surfaces stay light until the 1 s tick.
    /// </summary>
    public static void ApplyToSubtree(GameObject root)
    {
        if (!NetworkGuard.IsAlive(root)) return;

        Transform? previous = _forceRoot;
        try
        {
            _forceRoot = root.transform;
            SweepRoot(root); // ends with the dark-text pass over the same subtree
        }
        catch (Exception ex)
        {
            Mod.Log?.Debug($"AppTheme subtree failed: {ex.Message}");
        }
        finally
        {
            _forceRoot = previous;
        }
    }

    /// <summary>True while a force scope is active and the transform lives under it.</summary>
    private static bool IsInForceScope(Transform t)
    {
        if (_forceRoot == null) return false;
        try
        {
            if (!NetworkGuard.IsAlive(_forceRoot) || !NetworkGuard.IsAlive(t)) return false;
            return t.IsChildOf(_forceRoot) || t.Pointer == _forceRoot.Pointer;
        }
        catch { return false; }
    }

    /// <summary>Restores every vanilla graphic we touched and forgets them. Dormant
    /// since v0.4.1 (dark mode is permanent) — kept so a light mode could be
    /// re-enabled with one call.</summary>
    public static void RestoreAll()
    {
        for (int i = _originals.Count - 1; i >= 0; i--)
        {
            (int _, Graphic graphic, Color original) = _originals[i];
            try
            {
                if (graphic != null && NetworkGuard.IsAlive(graphic)) graphic.color = original;
            }
            catch { /* dead object — nothing to restore */ }
        }
        _originals.Clear();
        _themed.Clear();
        _skip.Clear();
    }

    /// <summary>Scene unload: the objects die with the scene — drop the tracking without touching them.</summary>
    public static void HandleSceneUnload()
    {
        _originals.Clear();
        _themed.Clear();
        _skip.Clear();
        _ownRoots.Clear();
    }

    // ------------------------------------------------------------------
    // Own-UI registration (called by InboxUI around every build)
    // ------------------------------------------------------------------

    /// <summary>Registers the roots of our injected UI; every sweep skips their subtrees.</summary>
    public static void SetOwnRoots(Transform? toolbar, Transform? menu, Transform? modal)
    {
        _ownRoots.Clear();
        AddOwnRoot(toolbar);
        AddOwnRoot(menu);
        AddOwnRoot(modal);
    }

    /// <summary>Forgets the own-UI roots (a rebuild re-registers them).</summary>
    public static void ClearOwnRoots()
    {
        _ownRoots.Clear();
    }

    private static void AddOwnRoot(Transform? root)
    {
        if (root != null && NetworkGuard.IsAlive(root)) _ownRoots.Add(root);
    }

    private static bool IsOwnedByMod(Transform t)
    {
        if (_ownRoots.Count == 0) return false;
        try
        {
            for (int i = 0; i < _ownRoots.Count; i++)
            {
                Transform? root = _ownRoots[i];
                if (root == null || !NetworkGuard.IsAlive(root)) continue;
                if (t == root || t.IsChildOf(root)) return true;
            }
        }
        catch { /* treat as vanilla on errors */ }
        return false;
    }

    // ------------------------------------------------------------------
    // Specific surfaces
    // ------------------------------------------------------------------

    private static void TintEntry(MSGConversation conv)
    {
        RectTransform? entry = ConversationUtils.SafeEntry(conv);
        if (entry == null) return;
        GameObject go = entry.gameObject;

        Graphic? preview = null;
        try { preview = AsGraphic(conv.entryPreviewText); } catch { /* keep null */ }

        // The preview line is a legacy Text (0.4.7f6); the name may be one too —
        // scan both text kinds. Preview → muted, everything else → primary.
        var legacyTexts = go.GetComponentsInChildren<Text>(true);
        if (legacyTexts != null)
        {
            for (int i = 0; i < legacyTexts.Length; i++)
            {
                Text? text = legacyTexts[i];
                if (text == null || !NetworkGuard.IsAlive(text)) continue;
                bool isPreview = preview != null && text.Pointer == preview.Pointer;
                TintGraphic(text, isPreview ? GamePalette.TextMuted : GamePalette.TextPrimary);
            }
        }

        var tmpTexts = go.GetComponentsInChildren<TextMeshProUGUI>(true);
        if (tmpTexts != null)
        {
            for (int i = 0; i < tmpTexts.Length; i++)
            {
                TextMeshProUGUI? text = tmpTexts[i];
                if (text == null || !NetworkGuard.IsAlive(text)) continue;
                TintGraphic(text, GamePalette.TextPrimary);
            }
        }

        float w = Mathf.Abs(entry.rect.width);
        float h = Mathf.Abs(entry.rect.height);
        if (w <= 1f || h <= 1f) return;

        var images = go.GetComponentsInChildren<Image>(true);
        if (images == null) return;
        for (int i = 0; i < images.Length; i++)
        {
            Image? img = images[i];
            if (img == null || !NetworkGuard.IsAlive(img)) continue;
            if (!Covers(img.rectTransform, w * 0.9f, h * 0.9f)) continue;
            TintGraphic(img, GamePalette.Card);
        }
    }

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

            Color original;
            try { original = bubbleGraphic.color; } catch { continue; }
            bool nearWhite = original.r >= 0.6f && original.g >= 0.6f && original.b >= 0.6f;
            if (!nearWhite)
            {
                // Coloured bubble (player's): keep the accent AND its text — protect
                // the text from the generic dark-text sweep.
                if (content != null)
                {
                    try { _skip.Add(content.GetInstanceID()); } catch { /* skip */ }
                }
                continue;
            }

            TintGraphic(bubbleGraphic, GamePalette.CardAlt);

            // The little bubble triangles echo the bubble colour — always follow it.
            try { TintGraphic(AsGraphic(bubble.triangle_Left), GamePalette.CardAlt); } catch { /* skip */ }
            try { TintGraphic(AsGraphic(bubble.triangle_Right), GamePalette.CardAlt); } catch { /* skip */ }

            if (content != null) TintGraphic(content, GamePalette.TextPrimary);
        }
    }

    /// <summary>
    /// The response panel (Yes / [Counter-offer] / No): the panel and every
    /// non-dark surface inside become dark cards, the dark button texts turn light.
    /// The container lives on MSGConversation and survives page switches.
    /// </summary>
    private static void TintResponseArea(MSGConversation conv)
    {
        RectTransform? container = null;
        try { container = conv.responseContainer; } catch { /* skip */ }
        if (container == null || !NetworkGuard.IsAlive(container)) return;
        GameObject root = container.gameObject;

        var images = root.GetComponentsInChildren<Image>(true);
        if (images != null)
        {
            for (int i = 0; i < images.Length; i++)
            {
                Image? img = images[i];
                if (img == null || !NetworkGuard.IsAlive(img)) continue;
                if (IsOwnedByMod(img.transform)) continue;

                Color c;
                try { c = img.color; } catch { continue; }
                if (c.a <= 0.02f) continue;
                if (MaxChannel(c) <= 0.4f) continue; // already dark — leave it
                TintGraphic(img, GamePalette.CardAlt);
            }
        }

        TintDarkTextsIn(root);
    }

    /// <summary>
    /// The chat header bar (back arrow, name, avatar, status bars). Found by
    /// geometry — wide, low, in the top half of the dialogue page — then the
    /// container's dark texts and small dark icons (e.g. an arrow sprite) are
    /// lightened so they stay readable on the dark bar. Avatars are never touched.
    /// </summary>
    private static void TintDialogueHeader(MessagesApp app)
    {
        GameObject? page = null;
        try { page = AsGameObject(app.dialoguePage); } catch { /* skip */ }
        if (page == null || !NetworkGuard.IsAlive(page)) return;

        RectTransform? pageRt = page.GetComponent<RectTransform>();
        if (pageRt == null) return;
        float pw = Mathf.Abs(pageRt.rect.width);
        float ph = Mathf.Abs(pageRt.rect.height);
        if (pw <= 1f || ph <= 1f) return;

        var images = page.GetComponentsInChildren<Image>(true);
        if (images == null) return;
        for (int i = 0; i < images.Length; i++)
        {
            Image? img = images[i];
            if (img == null || !NetworkGuard.IsAlive(img)) continue;
            if (IsOwnedByMod(img.transform)) continue;

            Color c;
            try { c = img.color; } catch { continue; }
            if (c.a <= 0.02f || !IsNearWhite(c)) continue;

            RectTransform rt = img.rectTransform;
            if (rt == null) continue;
            float iw = Mathf.Abs(rt.rect.width);
            float ih = Mathf.Abs(rt.rect.height);
            if (iw < pw * 0.7f || ih > ph * 0.3f) continue;
            if (!IsInTopHalf(pageRt, rt)) continue;

            TintGraphic(img, GamePalette.Header);

            // Container extras: dark texts + small dark icons (the back arrow etc.).
            Transform? container = img.transform.parent;
            if (container == null) continue;
            GameObject containerGo = container.gameObject;
            TintDarkTextsIn(containerGo);
            TintSmallDarkIconsIn(containerGo, pw, ph);
        }
    }

    // ------------------------------------------------------------------
    // Generic sweep
    // ------------------------------------------------------------------

    /// <summary>
    /// Generic pass over the whole app: near-white surfaces get the dark treatment
    /// (full-page → Bg, wide top band → Header, everything else → Card) and dark
    /// neutral texts turn light. Avatars (small, roughly square) and our own UI
    /// are skipped.
    /// </summary>
    private static void SweepAppSurfaces(MessagesApp app)
    {
        SweepRoot(app.gameObject);
    }

    /// <summary>Generic sweep over an arbitrary subtree, sized by its own rect —
    /// the app root and the popup refresh share the same rules.</summary>
    private static void SweepRoot(GameObject root)
    {
        RectTransform? rootRt = root.GetComponent<RectTransform>();
        if (rootRt == null) return;
        float w = Mathf.Abs(rootRt.rect.width);
        float h = Mathf.Abs(rootRt.rect.height);
        if (w <= 1f || h <= 1f) return;

        var images = root.GetComponentsInChildren<Image>(true);
        if (images != null)
        {
            for (int i = 0; i < images.Length; i++)
            {
                Image? img = images[i];
                if (img == null || !NetworkGuard.IsAlive(img)) continue;
                if (IsOwnedByMod(img.transform)) continue;

                Color c;
                try { c = img.color; } catch { continue; }
                if (c.a <= 0.02f || !IsNearWhite(c)) continue;

                RectTransform rt = img.rectTransform;
                if (rt == null) continue;
                float iw = Mathf.Abs(rt.rect.width);
                float ih = Mathf.Abs(rt.rect.height);
                if (IsAvatarLike(iw, ih, w))
                {
                    // Small squares are usually avatars/item art — but the supplier
                    // dead-drop order popup draws its quantity boxes the same way.
                    // Content stays protected; input/decor boxes get the control
                    // fill instead (aggressive = popup passes only).
                    if (IsContentSquare(img, aggressiveSquares))
                    {
                        _passProtected++;
                        // A protected content square (item-icon art with a light tint) can still
                        // read dark-on-dark when the ART itself was painted for the old light
                        // popup (supplier dead-drop seed icons). In a popup pass, lift the sprite
                        // hue-preserving as well - BarLift only brightens texels whose max
                        // channel is below the target peak, so bright art and sprite-less images
                        // are left untouched.
                        if (aggressiveSquares) TryLiftDarkIcon(img, w, h);
                        LogStillLight(img, c, ref stillLightBudget);
                        continue;
                    }
                    _passThemed++;
                    TintGraphic(img, GamePalette.CardAlt);
                    continue;
                }

                if (iw >= w * 0.85f && ih >= h * 0.85f)
                {
                    TintGraphic(img, GamePalette.Bg);
                }
                else if (iw >= w * 0.7f && ih <= h * 0.3f && IsInTopHalf(rootRt, rt))
                {
                    TintGraphic(img, GamePalette.Header);
                }
                else
                {
                    TintGraphic(img, GamePalette.Card);
                }
            }
        }

        TintDarkTextsIn(root);
    }

    /// <summary>Dark, near-neutral texts on the now-dark surfaces turn light.</summary>
    private static void TintDarkTextsIn(GameObject root)
    {
        var legacy = root.GetComponentsInChildren<Text>(true);
        if (legacy != null)
        {
            for (int i = 0; i < legacy.Length; i++)
            {
                Text? text = legacy[i];
                if (text == null || !NetworkGuard.IsAlive(text)) continue;
                if (IsOwnedByMod(text.transform)) continue;
                TintIfDarkText(text);
            }
        }

        var tmps = root.GetComponentsInChildren<TextMeshProUGUI>(true);
        if (tmps != null)
        {
            for (int i = 0; i < tmps.Length; i++)
            {
                TextMeshProUGUI? text = tmps[i];
                if (text == null || !NetworkGuard.IsAlive(text)) continue;
                if (IsOwnedByMod(text.transform)) continue;
                TintIfDarkText(text);
            }
        }
    }

    private static void TintIfDarkText(Graphic text)
    {
        int id;
        try { id = text.GetInstanceID(); } catch { return; }
        if (_skip.Contains(id)) return;

        Color c;
        try { c = text.color; } catch { return; }
        if (c.a <= 0.02f || !IsDarkNeutral(c)) return;
        TintGraphic(text, GamePalette.TextPrimary);
    }

    /// <summary>Small dark icons inside a themed bar (the back arrow) turn light.</summary>
    private static void TintSmallDarkIconsIn(GameObject root, float pageW, float pageH)
    {
        var images = root.GetComponentsInChildren<Image>(true);
        if (images == null) return;
        for (int i = 0; i < images.Length; i++)
        {
            Image? img = images[i];
            if (img == null || !NetworkGuard.IsAlive(img)) continue;
            if (IsOwnedByMod(img.transform)) continue;

            RectTransform rt = img.rectTransform;
            if (rt == null) continue;
            float iw = Mathf.Abs(rt.rect.width);
            float ih = Mathf.Abs(rt.rect.height);
            if (iw > pageW * 0.15f || ih > pageH * 0.15f) continue; // icons are small

            Color c;
            try { c = img.color; } catch { continue; }
            if (c.a <= 0.02f) continue;
            if (MaxChannel(c) > 0.45f) continue;              // only dark icons
            if (Spread(c) > 0.25f) continue;                  // …and neutral ones
            TintGraphic(img, GamePalette.TextPrimary);
        }
    }

    // ------------------------------------------------------------------
    // Primitives
    // ------------------------------------------------------------------

    private static bool Covers(RectTransform? rt, float minW, float minH)
    {
        if (rt == null || !NetworkGuard.IsAlive(rt)) return false;
        try { return Mathf.Abs(rt.rect.width) >= minW && Mathf.Abs(rt.rect.height) >= minH; }
        catch { return false; }
    }

    private static bool IsNearWhite(Color c) => c.r >= 0.6f && c.g >= 0.6f && c.b >= 0.6f;

    private static bool IsDarkNeutral(Color c) => MaxChannel(c) <= 0.62f && Spread(c) <= 0.25f;

    private static float MaxChannel(Color c) => Mathf.Max(c.r, Mathf.Max(c.g, c.b));

    private static float Spread(Color c) => MaxChannel(c) - Mathf.Min(c.r, Mathf.Min(c.g, c.b));

    /// <summary>Roughly square and small → avatar/portrait content, never tinted.</summary>
    private static bool IsAvatarLike(float iw, float ih, float pageW)
    {
        float max = Mathf.Max(iw, ih);
        if (max <= 1f) return false;
        return Mathf.Abs(iw - ih) <= 0.25f * max && iw <= 0.3f * pageW;
    }

    /// <summary>Top half relative to the page's own centre (pivot-safe, rotation-safe).</summary>
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

    /// <summary>
    /// One-time tint per graphic (instance-id tracked); stores the original colour
    /// so light mode can restore it exactly. Alpha is preserved — only RGB swaps.
    /// Vanilla re-colours after we themed an object (hover, rebuilds) stay — except
    /// inside a force scope (ApplyToSubtree), where they are re-applied immediately.
    /// </summary>
    private static void TintGraphic(Graphic? graphic, Color dark)
    {
        if (graphic == null || !NetworkGuard.IsAlive(graphic)) return;

        int id;
        try { id = graphic.GetInstanceID(); } catch { return; }

        Color original;
        try { original = graphic.color; } catch { return; }
        if (original.a <= 0.02f) return; // invisible hit-area / decor — nothing to tint

        if (!_themed.Add(id))
        {
            // Already themed once. Vanilla re-colours stay — EXCEPT inside a force
            // scope (a popup that just opened re-set some colours): re-apply the
            // dark target. The cached original is never overwritten, so the
            // light-mode restore stays exact.
            if (!IsInForceScope(graphic.transform)) return;
            try { graphic.color = new Color(dark.r, dark.g, dark.b, original.a); } catch { /* dead */ }
            return;
        }

        try
        {
            graphic.color = new Color(dark.r, dark.g, dark.b, original.a);
            _originals.Add((id, graphic, original));
        }
        catch
        {
            _themed.Remove(id);
        }
    }

    /// <summary>Anything Graphic-shaped (TMP text, Image, ...); accepts GameObjects too.</summary>
    private static Graphic? AsGraphic(Il2CppObjectBase? obj)
    {
        if (obj == null) return null;
        try
        {
            Graphic? graphic = obj.TryCast<Graphic>();
            if (graphic != null) return graphic;
            GameObject? go = obj.TryCast<GameObject>();
            return go != null ? go.GetComponent<Graphic>() : null;
        }
        catch { return null; }
    }

    /// <summary>True while the app's home or dialogue page is actually shown (phone open).</summary>
    private static bool IsPageVisible(MessagesApp app)
    {
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
