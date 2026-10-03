using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppScheduleOne.Messaging;
using Il2CppScheduleOne.UI.Phone;
using Il2CppScheduleOne.UI.Phone.Messages;
using MelonLoader;
using S1Mods.Shared;
using Il2CppTMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MessagesPlus;

/// <summary>
/// Dark mode — recolors the VANILLA Messages surfaces so the WHOLE app (not just
/// the injected band) goes dark: page backgrounds, inbox rows, chat bubbles, the
/// dialogue header bar and the response panel. Everything is applied once per
/// graphic (instance-id tracked; vanilla re-colours are never fought again).
///
/// Only colours are touched — never layouts, raycasts or save state. Guards:
///  - our own injected UI (band/menu/dialog) is excluded from every sweep;
///  - avatars (small, roughly square) are never tinted (they are content);
///  - coloured elements (status bars, badges, the unread dot, the player's
///    accent bubbles) stay as they are.
/// </summary>
internal static class AppTheme
{
    /// <summary>Instance ids already themed once — first-wins order between the passes stays intact.</summary>
    private static readonly HashSet<int> _themed = new();

    /// <summary>The dark target per themed graphic — the verification pass re-applies it when vanilla re-colours a tracked surface.</summary>
    private static readonly Dictionary<int, TrackedGraphic> _tracked = new();

    /// <summary>Rebuild watchers attached to the app/pages (deduped by pointer, re-attached lazily).</summary>
    private static readonly List<ThemeRebuildWatcher> _watchers = new();

    /// <summary>Timestamp of the last full sweep (drives the heartbeat while latched).</summary>
    private static float _lastSweepTime;

    /// <summary>Heartbeat interval while the sweep is latched — stretches (up to 30 s) while nothing changes.</summary>
    private static float _heartbeatInterval = 5f;

    /// <summary>Frame of the last hierarchy-triggered sweep (coalesces watcher bursts).</summary>
    private static int _lastHierarchySweepFrame = -1;

    /// <summary>Rate limit for the verify-pass log line.</summary>
    private static float _lastRestoreLog;

    /// <summary>Graphic instance ids the sweeps must leave alone (texts of coloured bubbles).</summary>
    private static readonly HashSet<int> _skip = new();

    /// <summary>Roots of OUR injected UI — the vanilla sweeps must never touch their subtrees.</summary>
    private static readonly List<Transform> _ownRoots = new();

    /// <summary>Transient force scope: while set, already-themed graphics under this
    /// root may be re-tinted (a popup that just opened re-set some colours).</summary>
    private static Transform? _forceRoot;

    // Perf (2026-10-02): the 1 s tick keeps running the specific passes (bubbles /
    // rows / header), but the full-app generic sweep is latched off once a few
    // consecutive passes found no new graphics. It re-arms on page rebuilds
    // (SetOwnRoots), popup theming (ApplyToSubtree), scene unload and conversation
    // changes (checked in Apply).
    private static bool _sweepLatched;
    private static int _cleanSweepPasses;
    private static int _lastConversationCount = -1;
    private static IntPtr _lastCurrentConversation;

    /// <summary>The open conversation's friendship slider root - kept vanilla, never themed
    /// (reference-based; the name/type guard in the sweep can miss it).</summary>
    private static Transform? _friendshipSliderRoot;

    /// <summary>The VISIBLE relationship bar (Topbar/Name/Relationship - a Scrollbar
    /// subtree, not conv.slider). Guarded from every sweep; restored to vanilla white.</summary>
    private static Transform? _relationshipBarRoot;

    // Instrumentation for the popup pass (ApplyToSubtree): surfaces tinted vs
    // content squares kept — one log line per popup open is the test evidence.
    private static int _passThemed;
    private static int _passProtected;

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

            EnsureWatchers(app);

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

                // The friendship slider: protect via the game's own reference + keep it readable.
                try { TintFriendshipBar(current!); }
                catch (Exception ex) { Mod.Log?.Debug($"AppTheme friendship bar: {ex.Message}"); }
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

            // 3c. The header's friendship/relationship bar: hue-preserving sprite
            //     lift so the dark jewel-tone spectrum reads on the dark header
            //     (no white overlay, no outline). 2026-10-03.
            try { LiftHeaderBars(app); }
            catch (Exception ex) { Mod.Log?.Debug($"AppTheme bar lift: {ex.Message}"); }

            // 3a. The visible relationship bar stays vanilla: guarded in every sweep,
            //     no colour writes from us at all (2026-10-03).

            // 3b. Restore the dark target on tracked graphics vanilla re-coloured
            //     (hover, pooling, rebuilds) — the fallback this class used to miss.
            VerifyTracked();

            // 4. Generic sweep: every remaining near-white surface + dark neutral text.
            //    Latched off after a few clean passes (see field comment); a conversation
            //    change re-arms it so chat surfaces are never missed.
            int conversationCount = 0;
            try
            {
                var conversations = MessagesApp.ActiveConversations;
                conversationCount = conversations?.Count ?? 0;
            }
            catch { }
            IntPtr currentPtr = IntPtr.Zero;
            try { currentPtr = current?.Pointer ?? IntPtr.Zero; } catch { }
            if (conversationCount != _lastConversationCount || currentPtr != _lastCurrentConversation)
            {
                _lastConversationCount = conversationCount;
                _lastCurrentConversation = currentPtr;
                _sweepLatched = false;
                _cleanSweepPasses = 0;
            }

            // A latched sweep still runs a heartbeat so surfaces that appear
            // without a known event cannot stay light forever; the interval
            // stretches while nothing changes.
            bool heartbeat = _sweepLatched && (Time.unscaledTime - _lastSweepTime) >= _heartbeatInterval;
            if (!_sweepLatched || heartbeat)
            {
                int themedBefore = _themed.Count;
                bool sweepFailed = false;
                try { SweepAppSurfaces(app); }
                catch (Exception ex)
                {
                    // An aborted pass must never count as clean - that latched
                    // the sweep off with surfaces still unthemed.
                    sweepFailed = true;
                    _cleanSweepPasses = 0;
                    Mod.Log?.Debug($"AppTheme sweep: {ex.Message}");
                }
                _lastSweepTime = Time.unscaledTime;

                if (!_sweepLatched)
                {
                    if (!sweepFailed && _themed.Count == themedBefore)
                    {
                        if (++_cleanSweepPasses >= 3) _sweepLatched = true;
                    }
                    else
                    {
                        _cleanSweepPasses = 0;
                    }
                }
                else
                {
                    _heartbeatInterval = !sweepFailed && _themed.Count == themedBefore
                        ? Mathf.Min(_heartbeatInterval * 1.5f, 30f)
                        : 5f;
                }
            }
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
            // Perf (2026-10-02): the popup brought fresh surfaces — re-arm the sweep.
            RequestSweep();
            _forceRoot = root.transform;
            _passThemed = 0;
            _passProtected = 0;
            SweepRoot(root, aggressiveSquares: true); // ends with the dark-text pass over the same subtree
            Mod.Log?.Info($"AppTheme popup pass: {_passThemed} surface(s) tinted, {_passProtected} content square(s) kept.");
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
    private static bool IsInForceScope(Graphic g)
    {
        if (_forceRoot == null) return false;
        try
        {
            // The transform read is an IL2CPP proxy access and must stay inside
            // the guard - a throw here used to abort the whole verify pass.
            Transform t = g.transform;
            if (!NetworkGuard.IsAlive(_forceRoot) || !NetworkGuard.IsAlive(t)) return false;
            return t.IsChildOf(_forceRoot) || t.Pointer == _forceRoot.Pointer;
        }
        catch { return false; }
    }

    /// <summary>Scene unload: the objects die with the scene — drop the tracking without touching them.</summary>
    public static void HandleSceneUnload()
    {
        _themed.Clear();
        _tracked.Clear();
        _watchers.Clear();
        _skip.Clear();
        _ownRoots.Clear();
        _sweepLatched = false;
        _cleanSweepPasses = 0;
        _lastConversationCount = -1;
        _lastCurrentConversation = IntPtr.Zero;
        _lastSweepTime = 0f;
        _heartbeatInterval = 5f;
        _lastHierarchySweepFrame = -1;

        // Guard roots die with the scene (both re-resolved on the next pass).
        _relationshipBarRoot = null;
        _friendshipSliderRoot = null;

        // Our lifted sprites/textures are IL2CPP objects the GC never frees.
        BarLift.ReleaseAll();
    }

    // ------------------------------------------------------------------
    // Sweep watchers + verification pass (dark-mode fallback hardening)
    // ------------------------------------------------------------------

    /// <summary>Re-arms the generic sweep — a rebuild may have brought new surfaces.</summary>
    internal static void RequestSweep()
    {
        _sweepLatched = false;
        _cleanSweepPasses = 0;
        _heartbeatInterval = 5f;
    }

    /// <summary>Hierarchy change: re-arm the sweep and run one sweep — bursts coalesce to one per frame.</summary>
    internal static void NotifyHierarchyChanged(MessagesApp app)
    {
        RequestSweep();
        if (Time.frameCount == _lastHierarchySweepFrame) return;
        _lastHierarchySweepFrame = Time.frameCount;
        Apply(app);
    }

    /// <summary>
    /// Attaches/dedupes the tiny rebuild watchers on the app and its two pages.
    /// Called from every tick; cheap (a few pointer compares).
    /// </summary>
    private static void EnsureWatchers(MessagesApp app)
    {
        try
        {
            AttachWatcher(app.gameObject, app);
        }
        catch { /* watcher is best effort */ }

        try
        {
            GameObject home = app.homePage;
            if (home != null) AttachWatcher(home, app);
        }
        catch { /* keep going */ }

        try
        {
            GameObject? dialogue = AsGameObject(app.dialoguePage);
            if (dialogue != null) AttachWatcher(dialogue, app);
        }
        catch { /* keep going */ }
    }

    private static void AttachWatcher(GameObject go, MessagesApp app)
    {
        if (go == null || !NetworkGuard.IsAlive(go)) return;

        try
        {
            for (int i = _watchers.Count - 1; i >= 0; i--)
            {
                ThemeRebuildWatcher? watcher = _watchers[i];
                if (watcher == null || !NetworkGuard.IsAlive(watcher))
                {
                    _watchers.RemoveAt(i);
                    continue;
                }
                if (watcher.gameObject.Pointer == go.Pointer)
                {
                    watcher.app = app;
                    return;
                }
            }

            ThemeRebuildWatcher fresh = go.AddComponent<ThemeRebuildWatcher>();
            if (fresh != null)
            {
                fresh.app = app;
                _watchers.Add(fresh);
                Mod.Log?.Info($"AppTheme watcher attached: {PathOf(go.transform)}");
            }
        }
        catch (Exception ex)
        {
            Mod.Log?.Debug($"AppTheme watcher attach failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Restores the dark target on tracked graphics whose colour the game
    /// re-set (hover, pooling, rebuilds). Only tracked instances are checked —
    /// no extra scans — and dead objects are dropped along the way.
    /// </summary>
    private static void VerifyTracked()
    {
        if (_tracked.Count == 0) return;

        int checkedCount = 0;
        int restored = 0;
        List<int>? dead = null;

        foreach (KeyValuePair<int, TrackedGraphic> pair in _tracked)
        {
            TrackedGraphic entry = pair.Value;
            Graphic? g = entry.Graphic;
            if (g == null || !NetworkGuard.IsAlive(g))
            {
                (dead ??= new List<int>()).Add(pair.Key);
                continue;
            }

            // ColorTint Selectables carry the palette in their ColorBlock (the
            // graphic stays white) - watch the block instead of the colour, or a
            // game ColorBlock reset stays invisible and the surface stays light.
            if (entry.BlockCarried)
            {
                Selectable? sel = entry.Selectable;
                if (sel == null || !NetworkGuard.IsAlive(sel))
                {
                    (dead ??= new List<int>()).Add(pair.Key);
                    continue;
                }
                checkedCount++;
                Color blockNormal;
                try { blockNormal = sel.colors.normalColor; }
                catch { (dead ??= new List<int>()).Add(pair.Key); continue; }
                bool drifted = Mathf.Abs(blockNormal.r - entry.Target.r) > 0.04f
                    || Mathf.Abs(blockNormal.g - entry.Target.g) > 0.04f
                    || Mathf.Abs(blockNormal.b - entry.Target.b) > 0.04f;
                if (drifted && IsLightSurface(blockNormal))
                {
                    try
                    {
                        ApplyDarkTransitions(sel, entry.Target);
                        g.color = new Color(1f, 1f, 1f, g.color.a);
                        restored++;
                    }
                    catch { (dead ??= new List<int>()).Add(pair.Key); }
                }
                continue;
            }

            Color current;
            try { current = g.color; }
            catch { (dead ??= new List<int>()).Add(pair.Key); continue; }
            if (current.a <= 0.02f) continue; // hidden right now — do not fight transparency
            checkedCount++;

            if (Mathf.Abs(current.r - entry.Target.r) <= 0.04f
                && Mathf.Abs(current.g - entry.Target.g) <= 0.04f
                && Mathf.Abs(current.b - entry.Target.b) <= 0.04f)
            {
                continue;
            }

            // Only a LIGHT fallback is a bug to fix; a deliberate dark or
            // coloured re-colour (a pooled bubble reused as an accent, semantic
            // states) is left alone.
            if (!IsLightSurface(current)) continue;

            try
            {
                g.color = new Color(entry.Target.r, entry.Target.g, entry.Target.b, current.a);
                restored++;
            }
            catch { (dead ??= new List<int>()).Add(pair.Key); }
        }

        if (dead != null)
        {
            for (int i = 0; i < dead.Count; i++)
            {
                _tracked.Remove(dead[i]);
                _themed.Remove(dead[i]);
            }
        }

        if (restored > 0 && Time.unscaledTime - _lastRestoreLog >= 5f)
        {
            _lastRestoreLog = Time.unscaledTime;
            Mod.Log?.Info($"AppTheme verify pass: {checkedCount} checked, {restored} restored.");
        }
    }

    /// <summary>Diagnostic (debug): names a still-light image the sweep left alone. Capped per pass.</summary>
    private static void LogStillLight(Image img, Color c, ref int budget)
    {
        if (budget <= 0) return;
        if (c.a <= 0.02f || Lum(c) <= 0.5f) return;
        budget--;

        try
        {
            RectTransform rt = img.rectTransform;
            float iw = rt != null ? Mathf.Abs(rt.rect.width) : 0f;
            float ih = rt != null ? Mathf.Abs(rt.rect.height) : 0f;
            string sprite = "?";
            try { sprite = img.sprite != null ? "yes" : "no"; }
            catch { /* keep "?" */ }
            Mod.Log?.Debug($"AppTheme still light: {PathOf(img.transform)} colour=({c.r:0.00},{c.g:0.00},{c.b:0.00}) size=({iw:0}x{ih:0}) sprite={sprite}");
        }
        catch { /* diagnostics only */ }
    }

    /// <summary>Short hierarchy path for log lines (best effort, depth-capped).</summary>
    private static string PathOf(Transform t)
    {
        try
        {
            string path = t.name;
            Transform? parent = t.parent;
            for (int i = 0; i < 6 && parent != null; i++)
            {
                path = parent.name + "/" + path;
                parent = parent.parent;
            }
            return path;
        }
        catch { return "?"; }
    }

    /// <summary>Dark target of one themed graphic, kept for the verification pass.</summary>
    private sealed class TrackedGraphic
    {
        public readonly Graphic? Graphic;
        public readonly Color Target;

        // ColorTint Selectables keep the palette in their ColorBlock (graphic
        // white) - remembered so VerifyTracked can watch the block too.
        public readonly Selectable? Selectable;
        public readonly bool BlockCarried;

        public TrackedGraphic(Graphic graphic, Color target, Selectable? selectable = null, bool blockCarried = false)
        {
            Graphic = graphic;
            Selectable = selectable;
            BlockCarried = blockCarried;
            Target = target;
        }
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
        // Perf (2026-10-02): a (re)build can bring new surfaces — re-arm the sweep.
        RequestSweep();
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
            RectTransform? coverRt = null;
            try { coverRt = img.rectTransform; } catch { continue; }
            if (!Covers(coverRt, w * 0.9f, h * 0.9f)) continue;
            TintGraphic(img, GamePalette.Card);
        }
    }

    /// <summary>True when this graphic is one of our themed surfaces (tracked with identity).</summary>
    private static bool IsTracked(Graphic g)
    {
        try
        {
            int id = g.GetInstanceID();
            if (_tracked.TryGetValue(id, out TrackedGraphic? entry) && entry != null && entry.Graphic != null)
            {
                return NetworkGuard.IsAlive(entry.Graphic) && entry.Graphic.Pointer == g.Pointer;
            }
        }
        catch { }
        return false;
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
            // OURS first: a bubble we already themed now reads "dark" and must
            // not be re-classified as a player accent - that poisoned _skip and
            // blocked content text repairs from the second pass on.
            bool ours = IsTracked(bubbleGraphic);
            bool nearWhite = IsLightSurface(original);
            if (!nearWhite && !ours)
            {
                // Coloured bubble (player's): keep the accent AND its text - protect
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
            if (IsInFriendshipBar(img.transform) || IsInRelationshipBar(img.transform)) continue; // friendship slider + visible relationship bar stay vanilla

            Color c;
            try { c = img.color; } catch { continue; }
            if (c.a <= 0.02f || !IsLightSurface(c)) continue;

            RectTransform rt;
            try { rt = img.rectTransform; } catch { continue; }
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

    /// <summary>
    /// The chat header's friendship slider. Its images are never themed (state
    /// display); a track that would blend dark-on-dark is lifted to a readable
    /// grey while the game-coloured fill stays exactly as the game set it.
    /// Uses the conversation's own slider reference - the name/type guard can
    /// miss the bar depending on the prefab's object names (2026-10-03).
    /// </summary>
    private static void TintFriendshipBar(MSGConversation conv)
    {
        Slider? slider = null;
        try { slider = conv.slider; } catch { /* skip */ }
        if (slider == null || !NetworkGuard.IsAlive(slider))
        {
            return; // keep the last known root - a missing ref must not open the guard
        }

        try { _friendshipSliderRoot = slider.transform; } catch { /* keep previous root */ }

        IntPtr fillPtr = IntPtr.Zero;
        try
        {
            Image? fill = conv.sliderFill;
            if (fill != null && NetworkGuard.IsAlive(fill)) fillPtr = fill.Pointer;
        }
        catch { /* skip */ }

        GameObject root = slider.gameObject;
        var images = root.GetComponentsInChildren<Image>(true);
        if (images == null) return;

        // One-time diagnostic per conversation (Info reaches Latest.log):
        // the subtree structure under the slider ref decides the strategy.
        try { LogFriendshipBarOnce(conv, slider, images); } catch { /* diagnostics only */ }

        // The bar is a fixed colour spectrum (bad red to good green) plus a
        // marker showing the current standing: segments, fill and marker stay
        // exactly as the game painted them. Only the resolved track is ours.
        Image? track = null;
        try { track = ResolveTrackImage(images, fillPtr); } catch { /* skip */ }

        for (int i = 0; i < images.Length; i++)
        {
            Image? img = images[i];
            if (img == null || !NetworkGuard.IsAlive(img)) continue;
            if (IsOwnedByMod(img.transform)) continue;

            // Only the resolved track is themed - everything else stays
            // vanilla so the standing reads truthfully.
            bool isTrack = false;
            if (track != null)
            {
                try { isTrack = img.Pointer == track.Pointer; } catch { continue; }
            }

            if (!isTrack)
            {
                // Purge stale dark targets: anything themed here by an earlier
                // revision must not be restored back to dark by the verify pass.
                // The track is exempt - it keeps its tracking so VerifyTracked
                // protects its readability tint across passes.
                try
                {
                    int dropId = img.GetInstanceID();
                    _tracked.Remove(dropId);
                    _themed.Remove(dropId);
                }
                catch { /* skip */ }
                continue;
            }

            Color c;
            try { c = img.color; } catch { continue; }
            if (c.a <= 0.02f) continue;
            if (MaxChannel(c) >= 0.35f) continue; // already readable on the dark bar

            // Track would blend into the dark header - theme it (tracked), so
            // the verify pass keeps it readable instead of fighting it.
            TintGraphic(img, GamePalette.TextMuted);
        }

        // Obsolete (2026-10-03, evening): the sprite lift aimed at conv.slider, which is
        // not the visible bar - removed. The visible bar (Topbar/Name/Relationship)
        // stays vanilla via the IsInRelationshipBar guards; we write no colours there.
    }

    /// <summary>
    /// Picks the track image behind the friendship spectrum. Prefers an
    /// explicit background name, falls back to the widest non-fill,
    /// non-handle image, and returns null instead of a wrong object when
    /// nothing qualifies.
    /// </summary>
    private static Image? ResolveTrackImage(Image[] images, IntPtr fillPtr)
    {
        Image? best = null;
        float bestW = 0f;
        for (int i = 0; i < images.Length; i++)
        {
            Image? img = images[i];
            if (img == null || !NetworkGuard.IsAlive(img)) continue;
            if (IsOwnedByMod(img.transform)) continue;
            try { if (fillPtr != IntPtr.Zero && img.Pointer == fillPtr) continue; } catch { continue; }

            string name = "";
            try { name = img.gameObject.name.ToLowerInvariant(); } catch { continue; }
            if (name.Contains("fill")) continue;
            if (name.Contains("handle")) continue;
            if (name.Contains("background")) return img;

            float w = 0f;
            try { w = Mathf.Abs(img.rectTransform.rect.width); } catch { continue; }
            if (best == null || w > bestW) { best = img; bestW = w; }
        }
        return best;
    }

    /// <summary>
    /// The visible friendship/relationship bar in the dialogue header. The game
    /// paints its spectrum in dark jewel tones for the vanilla LIGHT header, so it
    /// sits dark-on-dark on ours. Every colour sweep keeps it vanilla; readability
    /// comes from a hue-preserving sprite lift (<see cref="BarLift"/>), plus a reset
    /// of any stale dark ColorBlock on the bar's Selectable (2026-10-03).
    /// </summary>
    private static void LiftHeaderBars(MessagesApp app)
    {
        GameObject? page = null;
        try { page = AsGameObject(app.dialoguePage); } catch { /* skip */ }
        if (page == null || !NetworkGuard.IsAlive(page)) return;

        // Resolve the VISIBLE bar (Topbar/Name/Relationship). This was a dead
        // method before - its exclusion guard never ran; now it is the lift target.
        try { EnsureRelationshipRoot(page); } catch { /* retried next pass */ }

        Transform? rel = _relationshipBarRoot;
        if (rel != null && NetworkGuard.IsAlive(rel))
        {
            BarLift.Lift(rel, "relationship");
        }

        // Hedge: the research dump pointed at conv.slider as the bar; the code
        // earlier concluded the visible one is the Scrollbar above. Lift both - a
        // lift on the non-visible one is a harmless no-op.
        Transform? fri = _friendshipSliderRoot;
        if (fri != null && NetworkGuard.IsAlive(fri))
        {
            BarLift.Lift(fri, "friendship");
        }
    }

    /// <summary>
    /// Dark-mode rescue for the friendship bar (2026-10-03): the game's spectrum
    /// is painted in dark jewel tones for the vanilla light header, so it sits
    /// dark-on-dark on ours. Two independent steps, correct under either cause:
    /// (1) neutralize a stale dark state colour on the slider (uGUI multiplies
    /// it into the canvas renderer), (2) lift the sprite texels to a readable
    /// peak while keeping channel ratios - red stays red, green stays green, and
    /// the standing still reads.
    /// </summary>
    /// <summary>
    /// Resolves the VISIBLE relationship bar (Topbar/Name/Relationship - a Scrollbar
    /// subtree, not conv.slider). Cached; re-resolved when the cached root dies.
    /// </summary>
    private static void EnsureRelationshipRoot(GameObject page)
    {
        if (_relationshipBarRoot != null && NetworkGuard.IsAlive(_relationshipBarRoot)) return;
        _relationshipBarRoot = null;
        try
        {
            Transform? direct = page.transform.Find("Topbar/Name/Relationship");
            if (direct != null && NetworkGuard.IsAlive(direct.gameObject))
            {
                _relationshipBarRoot = direct;
                return;
            }
        }
        catch { /* fall through to the scrollbar walk */ }
        try
        {
            var scrollbars = page.GetComponentsInChildren<Scrollbar>(true);
            if (scrollbars == null) return;
            for (int i = 0; i < scrollbars.Length; i++)
            {
                Scrollbar? sb = scrollbars[i];
                if (sb == null || !NetworkGuard.IsAlive(sb)) continue;
                Transform? t = null;
                try { t = sb.transform; } catch { continue; }
                for (int d = 0; d < 5 && t != null; d++)
                {
                    string n = "";
                    try { n = t.name; } catch { break; }
                    if (n == "Relationship")
                    {
                        _relationshipBarRoot = t;
                        return;
                    }
                    try { t = t.parent; } catch { break; }
                }
            }
        }
        catch { /* unresolved this pass - retried next pass */ }
    }

    /// <summary>True for images inside the visible relationship bar (guarded + restored).</summary>
    private static bool IsInRelationshipBar(Transform t)
    {
        Transform? root = _relationshipBarRoot;
        if (root == null || !NetworkGuard.IsAlive(root)) return false;
        try
        {
            if (t.Pointer == root.Pointer) return true;
            return t.IsChildOf(root);
        }
        catch { return false; }
    }

    /// <summary>
    /// <summary>Nearest Selectable around a bar image (possible state-colour multiply source), for the one-time dump.</summary>
    private static string DescribeSelectable(Image img)
    {
        try
        {
            Selectable? sel = img.GetComponentInParent<Selectable>();
            if (sel == null || !NetworkGuard.IsAlive(sel)) return "sel=none";
            bool target = false;
            try { target = sel.targetGraphic != null && sel.targetGraphic.Pointer == img.Pointer; } catch { /* keep */ }
            Color n = sel.colors.normalColor;
            return $"sel={sel.GetType().Name} trans={sel.transition} tgt={target} block=({n.r:0.00},{n.g:0.00},{n.b:0.00},{n.a:0.00})";
        }
        catch { return "sel=?"; }
    }

    /// <summary>Conversations already covered by the one-time slider dump.</summary>
    private static readonly HashSet<IntPtr> _friendshipBarLogged = new();

    /// <summary>
    /// One-time hierarchy dump of the friendship slider per conversation (Info
    /// level so it reaches Latest.log). No behaviour change - answers which
    /// image is the track and whether alpha eats the lift.
    /// </summary>
    private static void LogFriendshipBarOnce(MSGConversation conv, Slider slider, Image[] images)
    {
        IntPtr key = IntPtr.Zero;
        try { key = conv.Pointer; } catch { return; }
        if (key == IntPtr.Zero || _friendshipBarLogged.Contains(key)) return;
        _friendshipBarLogged.Add(key);

        IntPtr fillRef = IntPtr.Zero;
        try
        {
            Image? fill = conv.sliderFill;
            if (fill != null && NetworkGuard.IsAlive(fill)) fillRef = fill.Pointer;
        }
        catch { /* skip */ }

        for (int i = 0; i < images.Length; i++)
        {
            Image? img = images[i];
            if (img == null || !NetworkGuard.IsAlive(img)) continue;
            string name = "?";
            float w = 0f;
            Color c = new Color(0f, 0f, 0f, 0f);
            float rendererAlpha = -1f;
            try { name = img.gameObject.name; } catch { /* keep */ }
            try { w = Mathf.Abs(img.rectTransform.rect.width); } catch { /* keep */ }
            try { c = img.color; } catch { /* keep */ }
            try { rendererAlpha = img.canvasRenderer.GetAlpha(); } catch { /* keep */ }
            bool isFillRef = false;
            try { isFillRef = fillRef != IntPtr.Zero && img.Pointer == fillRef; } catch { /* keep */ }
            float groupAlpha = 1f;
            try { groupAlpha = NearestCanvasGroupAlpha(img.transform); } catch { /* keep */ }
            string cr = "?";
            try
            {
                Color crc = img.canvasRenderer.GetColor();
                cr = $"({crc.r:0.00},{crc.g:0.00},{crc.b:0.00},{crc.a:0.00})";
            }
            catch { /* keep */ }
            string type = "?";
            float fill = -1f;
            try { type = img.type.ToString(); fill = img.fillAmount; } catch { /* keep */ }
            Mod.Log?.Info($"FriendshipBar: name='{name}' w={w:0.0} colour=({c.r:0.00},{c.g:0.00},{c.b:0.00}) a={c.a:0.00} crColor={cr} rendererAlpha={rendererAlpha:0.00} groupAlpha={groupAlpha:0.00} isFillRef={isFillRef} type={type} fill={fill:0.00} {DescribeSelectable(img)}");
        }
    }

    /// <summary>Nearest CanvasGroup alpha above a transform, 1 when none.</summary>
    private static float NearestCanvasGroupAlpha(Transform t)
    {
        try
        {
            Transform? parent = t.parent;
            for (int i = 0; i < 8 && parent != null; i++)
            {
                CanvasGroup? group = null;
                try { group = parent.GetComponent<CanvasGroup>(); } catch { /* keep climbing */ }
                if (group != null && NetworkGuard.IsAlive(group))
                {
                    try { return group.alpha; } catch { return 1f; }
                }
                parent = parent.parent;
            }
        }
        catch { /* fall through */ }
        return 1f;
    }

    /// <summary>True for images inside the open conversation's friendship slider (kept vanilla).</summary>
    private static bool IsInFriendshipBar(Transform t)
    {
        Transform? root = _friendshipSliderRoot;
        if (root == null || !NetworkGuard.IsAlive(root)) return false;
        try { return t.IsChildOf(root); } catch { return false; }
    }

    // ------------------------------------------------------------------
    // Generic sweep
    // ------------------------------------------------------------------

    /// <summary>
    /// Generic pass over the whole app: near-white surfaces get the dark treatment
    /// (full-page → Bg, wide top band → Header, everything else → Card) and dark
    /// neutral texts turn light. Avatars (small, roughly square) and our own UI
    /// are skipped. Conservative square rules - the aggressive control-square
    /// detection stays popup-only (2026-10-03: app-wide it darkened the dialogue
    /// header avatar and friendship bar).
    /// </summary>
    private static void SweepAppSurfaces(MessagesApp app)
    {
        SweepRoot(app.gameObject, aggressiveSquares: false);
    }

    /// <summary>Generic sweep over an arbitrary subtree, sized by its own rect.
    /// <paramref name="aggressiveSquares"/> = true only for popup passes: there
    /// the small-square rules also treat interactive / control-named squares as
    /// surfaces (quantity boxes), while the app-wide sweep stays conservative so
    /// identity art (header avatar) is never darkened.</summary>
    private static void SweepRoot(GameObject root, bool aggressiveSquares)
    {
        RectTransform? rootRt = root.GetComponent<RectTransform>();
        if (rootRt == null) return;
        float w = Mathf.Abs(rootRt.rect.width);
        float h = Mathf.Abs(rootRt.rect.height);
        if (w <= 1f || h <= 1f) return;

        int stillLightBudget = 20;
        var images = root.GetComponentsInChildren<Image>(true);
        if (images != null)
        {
            for (int i = 0; i < images.Length; i++)
            {
                Image? img = images[i];
                if (img == null || !NetworkGuard.IsAlive(img)) continue;
                if (IsOwnedByMod(img.transform)) continue;
                if (IsUnderSlider(img) || IsInFriendshipBar(img.transform) || IsInRelationshipBar(img.transform)) continue; // identity/state sliders stay vanilla (friendship + relationship bars)

                Color c;
                try { c = img.color; } catch { continue; }
                if (c.a <= 0.02f || !IsLightSurface(c))
                {
                    // Surfaces the rules deliberately leave alone are logged for
                    // the test round (debug; capped per pass).
                    LogStillLight(img, c, ref stillLightBudget);
                    continue;
                }

                RectTransform rt;
                try { rt = img.rectTransform; } catch { continue; }
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
                        LogStillLight(img, c, ref stillLightBudget);
                        continue;
                    }
                    _passThemed++;
                    TintGraphic(img, GamePalette.CardAlt);
                    continue;
                }

                if (iw >= w * 0.85f && ih >= h * 0.85f)
                {
                    _passThemed++;
                    TintGraphic(img, GamePalette.Bg);
                }
                else if (iw >= w * 0.7f && ih <= h * 0.3f && IsInTopHalf(rootRt, rt))
                {
                    _passThemed++;
                    TintGraphic(img, GamePalette.Header);
                }
                else
                {
                    _passThemed++;
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
        if (c.a <= 0.02f) return;
        if (Lum(c) >= 0.45f) return; // not dark enough to need lightening

        bool saturated = Spread(c) > 0.30f;
        if (saturated && IsOnLightSurface(text.transform)) return; // readable where it is

        // Neutral dark text becomes the primary ink; coloured dark text keeps its
        // hue and is only brightened until it is readable on the dark surface.
        TintGraphic(text, saturated ? BrightenToReadable(c) : GamePalette.TextPrimary);
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
            if (IsInFriendshipBar(img.transform) || IsInRelationshipBar(img.transform)) continue; // bar art is state, not icons

            RectTransform rt;
            try { rt = img.rectTransform; } catch { continue; }
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

    /// <summary>Light SURFACE candidate: clearly near-white, or a light, low-saturation
    /// tint (light grey / light blue) the old per-channel test used to miss.</summary>
    private static bool IsLightSurface(Color c)
        => (c.r >= 0.6f && c.g >= 0.6f && c.b >= 0.6f) || (Lum(c) > 0.55f && Spread(c) < 0.32f);

    private static float MaxChannel(Color c) => Mathf.Max(c.r, Mathf.Max(c.g, c.b));

    private static float Spread(Color c) => MaxChannel(c) - Mathf.Min(c.r, Mathf.Min(c.g, c.b));

    /// <summary>Perceptual luminance (0.299/0.587/0.114) — base for surface/text classification.</summary>
    private static float Lum(Color c) => (0.299f * c.r) + (0.587f * c.g) + (0.114f * c.b);

    /// <summary>Lerps a dark colour toward white until it reads on a dark surface (hue kept).</summary>
    private static Color BrightenToReadable(Color c)
    {
        float lum = Lum(c);
        float t = Mathf.Clamp01((0.62f - lum) / (1f - lum));
        return new Color(Mathf.Lerp(c.r, 1f, t), Mathf.Lerp(c.g, 1f, t), Mathf.Lerp(c.b, 1f, t), c.a);
    }

    /// <summary>Walks up a few ancestors to find the surface under a text (best effort).</summary>
    private static bool IsOnLightSurface(Transform t)
    {
        try
        {
            Transform? parent = t.parent;
            for (int i = 0; i < 5 && parent != null; i++)
            {
                Graphic? g = parent.GetComponent<Graphic>();
                if (g != null && NetworkGuard.IsAlive(g))
                {
                    Color c = g.color;
                    if (c.a > 0.05f) return IsLightSurface(c);
                }
                parent = parent.parent;
            }
        }
        catch { /* treat as unknown */ }
        return false;
    }

    /// <summary>Roughly square and small → candidate for avatar/portrait content.</summary>
    private static bool IsAvatarLike(float iw, float ih, float pageW)
    {
        float max = Mathf.Max(iw, ih);
        if (max <= 1f) return false;
        return Mathf.Abs(iw - ih) <= 0.25f * max && iw <= 0.3f * pageW;
    }

    /// <summary>
    /// Decides whether a small, roughly square image is protected CONTENT (avatar /
    /// item art) or a SURFACE that merely looks like one (input background, solid
    /// control/decor fill). Content carries a sprite; input boxes are themed like
    /// any surface. In aggressive mode (popup passes only) interactive and
    /// control-named squares are themed too - that fixes the supplier dead-drop
    /// popup's quantity boxes; app-wide the conservative rule keeps identity art
    /// (header avatar) untouched (2026-10-03: aggressive rules applied app-wide
    /// darkened the header avatar and friendship bar).
    /// </summary>
    private static bool IsContentSquare(Image img, bool aggressive)
    {
        try
        {
            if (img.GetComponentInParent<TMP_InputField>() != null) return false;
        }
        catch { /* unreadable — fall through to the other signals */ }

        try
        {
            if (img.GetComponentInParent<InputField>() != null) return false;
        }
        catch { /* unreadable — fall through to the other signals */ }

        if (aggressive)
        {
            // Interactive surfaces (quantity inputs, stepper buttons) are controls,
            // not content - but vanilla marks plenty of identity art raycastable,
            // so this signal must stay popup-scoped.
            try
            {
                if (img.raycastTarget) return false;
            }
            catch { /* unreadable — fall through */ }

            // Control-ish names beat the sprite heuristic (the box may be a plain
            // image with a text label and no input component).
            try
            {
                Transform? t = img.transform;
                for (int i = 0; i < 3 && t != null; i++)
                {
                    string name = t.name.ToLowerInvariant();
                    if (name.Contains("input") || name.Contains("quantity") || name.Contains("amount")
                        || name.Contains("stepper") || name.Contains("plus") || name.Contains("minus"))
                    {
                        return false;
                    }
                    t = t.parent;
                }
            }
            catch { /* unreadable — fall through */ }
        }

        try { return img.sprite != null; } catch { return true; }
    }

    /// <summary>
    /// True when the image belongs to a Slider (e.g. the dialogue header's
    /// friendship bar). Sliders carry state - their track/fill/handle stay
    /// vanilla so the level stays readable on the dark surfaces (2026-10-03).
    /// </summary>
    private static bool IsUnderSlider(Image img)
    {
        try { if (img.GetComponentInParent<Slider>() != null) return true; } catch { /* fall through */ }

        try
        {
            Transform? t = img.transform;
            for (int i = 0; i < 4 && t != null; i++)
            {
                if (t.name.ToLowerInvariant().Contains("slider")) return true;
                t = t.parent;
            }
        }
        catch { /* fall through */ }

        return false;
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
    /// One-time tint per graphic (instance-id tracked); stores the dark target so
    /// the verification pass can restore it after vanilla re-colours (hover,
    /// pooling, rebuilds). Alpha stays live — only RGB swaps. Inside a force scope
    /// (ApplyToSubtree) the target is re-applied immediately, too.
    /// </summary>
    private static void TintGraphic(Graphic? graphic, Color dark)
    {
        if (graphic == null || !NetworkGuard.IsAlive(graphic)) return;

        int id;
        try { id = graphic.GetInstanceID(); } catch { return; }

        Color original;
        try { original = graphic.color; } catch { return; }
        if (original.a <= 0.02f) return; // invisible hit-area / decor — nothing to tint

        if (_themed.Contains(id))
        {
            // The id is known — but the OBJECT may be a new one (Unity reuses
            // instance ids after a destroy). Only the same living object counts
            // as "already themed".
            if (_tracked.TryGetValue(id, out TrackedGraphic? entry) && entry != null && entry.Graphic != null)
            {
                Graphic tracked = entry.Graphic;
                if (NetworkGuard.IsAlive(tracked) && tracked.Pointer == graphic.Pointer)
                {
                    // Vanilla re-colours stay — EXCEPT inside a force scope (a popup
                    // that just opened re-set some colours): re-apply the target.
                    if (!IsInForceScope(graphic)) return;
                    try
                    {
                        if (entry.BlockCarried && entry.Selectable != null && NetworkGuard.IsAlive(entry.Selectable))
                        {
                            ApplyDarkTransitions(entry.Selectable, entry.Target);
                            graphic.color = new Color(1f, 1f, 1f, graphic.color.a);
                        }
                        else
                        {
                            graphic.color = new Color(entry.Target.r, entry.Target.g, entry.Target.b, graphic.color.a);
                        }
                    }
                    catch { /* dead */ }
                    return;
                }
            }
            _tracked.Remove(id); // stale id of a destroyed object — treat as new
        }
        else
        {
            _themed.Add(id);
        }

        var target = new Color(dark.r, dark.g, dark.b, 1f);
        if (!SetDark(graphic, target, out Color applied, out Selectable? blockHolder))
        {
            _themed.Remove(id);
            _tracked.Remove(id);
            return;
        }
        _tracked[id] = blockHolder != null
            ? new TrackedGraphic(graphic, target, blockHolder, true)
            : new TrackedGraphic(graphic, applied);
    }

    /// <summary>
    /// Applies the dark RGB (the graphic's alpha stays live) and darkens the
    /// button transitions when this graphic is the target of a colour-tint
    /// Selectable. uGUI MULTIPLIES the graphic colour with the state colour, so
    /// in that case the graphic is set white and the palette is carried entirely
    /// in the ColorBlock. Returns the colour actually written to the graphic.
    /// </summary>
    private static bool SetDark(Graphic graphic, Color target, out Color applied, out Selectable? blockHolder)
    {
        applied = target;
        blockHolder = null;
        try
        {
            Selectable? sel = graphic.GetComponent<Selectable>();
            if (sel != null
                && sel.transition == Selectable.Transition.ColorTint
                && sel.targetGraphic != null
                && sel.targetGraphic.Pointer == graphic.Pointer)
            {
                applied = Color.white;
                blockHolder = sel; // the palette lives in the ColorBlock, not the graphic
                ApplyDarkTransitions(sel, target);
                LightenDarkIconsIn(graphic.gameObject);
            }
        }
        catch { /* transitions are polish, never fatal */ }

        try
        {
            graphic.color = new Color(applied.r, applied.g, applied.b, graphic.color.a);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Recolours a colour-tint Selectable's states into the dark family (feedback
    /// stays visible, nothing flashes back to light) and forces the normal state
    /// onto the renderer immediately — otherwise it would keep its previous
    /// (light) state colour until the next hover. Never throws.
    /// </summary>
    private static void ApplyDarkTransitions(Selectable sel, Color target)
    {
        try
        {
            ColorBlock block = sel.colors;
            block.colorMultiplier = 1f;
            block.normalColor = new Color(target.r, target.g, target.b, 1f);
            block.highlightedColor = new Color(GamePalette.CardHover.r, GamePalette.CardHover.g, GamePalette.CardHover.b, 1f);
            block.pressedColor = new Color(GamePalette.CardPressed.r, GamePalette.CardPressed.g, GamePalette.CardPressed.b, 1f);
            block.selectedColor = block.highlightedColor;
            block.disabledColor = new Color(target.r, target.g, target.b, 0.5f);
            block.fadeDuration = 0.1f;
            sel.colors = block;

            sel.targetGraphic.CrossFadeColor(block.normalColor, 0f, true, true);
        }
        catch { /* transitions are polish, never fatal */ }
    }

    /// <summary>
    /// Dark neutral icons inside a control we just darkened (stepper glyphs like
    /// the +/- on the quantity boxes) turn light so they stay visible. No
    /// page-size limit — this runs on a single small control, not a page.
    /// </summary>
    private static void LightenDarkIconsIn(GameObject root)
    {
        var images = root.GetComponentsInChildren<Image>(true);
        if (images == null) return;
        for (int i = 0; i < images.Length; i++)
        {
            Image? img = images[i];
            if (img == null || !NetworkGuard.IsAlive(img)) continue;
            if (IsOwnedByMod(img.transform)) continue;

            Color c;
            try { c = img.color; } catch { continue; }
            if (c.a <= 0.02f) continue;
            if (MaxChannel(c) > 0.45f) continue;              // only dark icons
            if (Spread(c) > 0.25f) continue;                  // ...and neutral ones
            TintGraphic(img, GamePalette.TextPrimary);
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
    internal static bool IsPageVisible(MessagesApp app)
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

// ---------------------------------------------------------------------------
// Merged from DealWindowSelectorPatch.cs (2026-10-02) — instant popup theming.
// ---------------------------------------------------------------------------

/// <summary>
/// Immediate dark-theme refresh for the deal-window popup (<see cref="DealWindowSelector"/> —
/// the Morning/Afternoon/Night/LateNight picker that opens from a conversation
/// response). Without this, the popup keeps its vanilla colours until the next
/// 1-second theme tick — the "popup flashes light for a moment" report.
///
/// The postfix runs in the same frame as the open call (before the first render)
/// and force-refreshes the popup's subtree, so colours the game re-sets while
/// opening also end up dark.
/// </summary>
public static class DealWindowSelectorPatch
{
    public static void ApplyAll(HarmonyLib.Harmony harmony, ModLogger log)
    {
        // One shared postfix for both overloads below — identical behaviour.
        HarmonyMethod postfix = new(typeof(DealWindowSelectorPatch), nameof(SetIsOpen_Postfix));

        // The app's real open path: SetIsOpen(bool, MSGConversation, Action<EDealWindow>).
        // Found by SHAPE (name + arity + first parameter) instead of naming the
        // IL2CPP delegate type in a parameter list — the proxy delegate type
        // (Il2CppSystem.Action<EDealWindow>) is not referenceable from mod code
        // (CS0305: Il2CppSystem.Action is generated as a 9-arity generic only).
        MethodInfo? threeArg = FindThreeArgSetIsOpen();
        if (threeArg != null)
        {
            PatchGuard.TryPatch(harmony, threeArg, postfix: postfix, log: log);
        }
        else
        {
            log.Warn("DealWindowSelectorPatch: SetIsOpen(bool, MSGConversation, Action<EDealWindow>) not found — popup hook skipped (the 1 s tick stays as fallback).");
        }

        // Defensive: the bool-only overload (no managed callers in 0.4.7f7, but
        // patched so every edge call path is covered as well).
        PatchGuard.TryPatch(
            harmony,
            typeof(DealWindowSelector),
            nameof(DealWindowSelector.SetIsOpen),
            postfix: postfix,
            parameterTypes: new[] { typeof(bool) },
            log: log);
    }

    /// <summary>Locates SetIsOpen(bool, MSGConversation, Action&lt;EDealWindow&gt;) by its shape.</summary>
    private static MethodInfo? FindThreeArgSetIsOpen()
    {
        try
        {
            foreach (MethodInfo m in typeof(DealWindowSelector).GetMethods(
                         BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (m.Name != nameof(DealWindowSelector.SetIsOpen)) continue;
                ParameterInfo[] ps = m.GetParameters();
                if (ps.Length == 3 && ps[0].ParameterType == typeof(bool))
                    return m;
            }
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"DealWindowSelectorPatch: overload lookup failed: {ex.Message}");
        }

        return null;
    }

    [HarmonyPostfix]
    public static void SetIsOpen_Postfix(DealWindowSelector __instance, bool __0)
    {
        // "__0" instead of a named parameter — immune to future renames.
        if (!__0) return; // closing — nothing to theme
        Refresh(__instance);
    }

    private static void Refresh(DealWindowSelector selector)
    {
        try
        {
            MessagesPlusConfig? cfg = ModConfig<MessagesPlusConfig>.Instance;
            if (cfg == null || !cfg.DarkMode) return;
            if (selector == null || !NetworkGuard.IsAlive(selector)) return;

            AppTheme.ApplyToSubtree(selector.gameObject);
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"DealWindowSelector theme refresh failed: {ex.Message}");
        }
    }
}

// ---------------------------------------------------------------------------
// Added 2026-10-02 — instant theme for the supplier dead-drop order popup.
// ---------------------------------------------------------------------------

/// <summary>
/// Same-frame dark theme for the supplier dead-drop order popup
/// (<see cref="PhoneShopInterface"/> — "Request Dead Drop": item list with
/// quantity boxes, Order/Debt/limit footer and Send). The popup is built when
/// Open(...) runs, so its rows must be refreshed immediately — the 1 s tick may
/// be latched off and the fresh rows would stay light ("seeds order is not fully
/// dark", 2026-10-02). Mirrors <see cref="DealWindowSelectorPatch"/>.
/// </summary>
public static class PhoneShopInterfacePatch
{
    public static void ApplyAll(HarmonyLib.Harmony harmony, ModLogger log)
    {
        MethodInfo? open = FindOpenMethod();
        if (open != null)
        {
            PatchGuard.TryPatch(harmony, open, postfix: new HarmonyMethod(typeof(PhoneShopInterfacePatch), nameof(Open_Postfix)), log: log);
        }
        else
        {
            log.Warn("PhoneShopInterfacePatch: Open(...) not found — popup hook skipped (the 1 s tick stays as fallback).");
        }
    }

    /// <summary>Locates Open(string, string, MSGConversation, List&lt;Listing&gt;, float, float, Action&lt;...&gt;) by shape.</summary>
    private static MethodInfo? FindOpenMethod()
    {
        try
        {
            foreach (MethodInfo m in typeof(PhoneShopInterface).GetMethods(
                         BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (m.Name != nameof(PhoneShopInterface.Open)) continue;
                ParameterInfo[] ps = m.GetParameters();
                if (ps.Length == 7 && ps[0].ParameterType == typeof(string))
                    return m;
            }
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"PhoneShopInterfacePatch: overload lookup failed: {ex.Message}");
        }

        return null;
    }

    [HarmonyPostfix]
    public static void Open_Postfix(PhoneShopInterface __instance)
    {
        try
        {
            MessagesPlusConfig? cfg = ModConfig<MessagesPlusConfig>.Instance;
            if (cfg == null || !cfg.DarkMode) return;
            if (__instance == null || !NetworkGuard.IsAlive(__instance)) return;

            AppTheme.ApplyToSubtree(__instance.gameObject);
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"PhoneShopInterface theme refresh failed: {ex.Message}");
        }
    }
}

// ---------------------------------------------------------------------------
// Added 2026-10-02 — rebuild watcher + same-frame theming for the remaining
// Messages-app popups (vanilla confirmation dialog, counter-offer flow).
// ---------------------------------------------------------------------------

/// <summary>
/// Re-arms and re-runs the theme sweep when a watched page adds or removes a
/// direct child (page rebuilds). Attached per scene by <see cref="AppTheme"/>;
/// Unity only calls this on hierarchy changes — no per-frame work.
/// </summary>
[RegisterTypeInIl2Cpp]
internal sealed class ThemeRebuildWatcher : MonoBehaviour
{
    public ThemeRebuildWatcher(IntPtr ptr) : base(ptr) { }

    /// <summary>The app this watcher belongs to (set right after AddComponent).</summary>
    public MessagesApp? app;

    private int _fires;

    private void OnTransformChildrenChanged()
    {
        _fires++;
        if (_fires == 1)
        {
            Mod.Log?.Info($"AppTheme watcher fired on {name} (first time).");
        }
        else
        {
            Mod.Log?.Debug($"AppTheme watcher fired on {name} ({_fires}).");
        }

        MessagesApp? target = app;
        if (target != null && NetworkGuard.IsAlive(target))
        {
            AppTheme.NotifyHierarchyChanged(target);
        }
        else
        {
            AppTheme.RequestSweep();
        }
    }
}

/// <summary>
/// Same-frame dark theme for the vanilla confirmation dialog
/// (<see cref="ConfirmationPopup"/> — used by the Messages app for destructive
/// confirmations). Mirrors <see cref="PhoneShopInterfacePatch"/>.
/// </summary>
public static class ConfirmationPopupPatch
{
    public static void ApplyAll(HarmonyLib.Harmony harmony, ModLogger log)
    {
        MethodInfo? open = FindOpenMethod();
        if (open != null)
        {
            PatchGuard.TryPatch(harmony, open, postfix: new HarmonyMethod(typeof(ConfirmationPopupPatch), nameof(Open_Postfix)), log: log);
        }
        else
        {
            log.Warn("ConfirmationPopupPatch: Open(string, string, MSGConversation, ...) not found — popup hook skipped (the 1 s tick stays as fallback).");
        }
    }

    /// <summary>Locates Open(string, string, MSGConversation, Action&lt;EResponse&gt;) by shape.</summary>
    private static MethodInfo? FindOpenMethod()
    {
        try
        {
            foreach (MethodInfo m in typeof(ConfirmationPopup).GetMethods(
                         BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (m.Name != nameof(ConfirmationPopup.Open)) continue;
                ParameterInfo[] ps = m.GetParameters();
                if (ps.Length == 4 && ps[0].ParameterType == typeof(string))
                    return m;
            }
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"ConfirmationPopupPatch: overload lookup failed: {ex.Message}");
        }

        return null;
    }

    [HarmonyPostfix]
    public static void Open_Postfix(ConfirmationPopup __instance)
    {
        try
        {
            MessagesPlusConfig? cfg = ModConfig<MessagesPlusConfig>.Instance;
            if (cfg == null || !cfg.DarkMode) return;
            if (__instance == null || !NetworkGuard.IsAlive(__instance)) return;

            AppTheme.ApplyToSubtree(__instance.gameObject);
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"ConfirmationPopup theme refresh failed: {ex.Message}");
        }
    }
}

/// <summary>
/// Same-frame dark theme for the counter-offer popup
/// (<see cref="CounterofferInterface"/> — opened from a conversation response).
/// </summary>
public static class CounterofferInterfacePatch
{
    public static void ApplyAll(HarmonyLib.Harmony harmony, ModLogger log)
    {
        MethodInfo? open = FindOpenMethod();
        if (open != null)
        {
            PatchGuard.TryPatch(harmony, open, postfix: new HarmonyMethod(typeof(CounterofferInterfacePatch), nameof(Open_Postfix)), log: log);
        }
        else
        {
            log.Warn("CounterofferInterfacePatch: Open(ProductDefinition, int, float, MSGConversation, ...) not found — popup hook skipped (the 1 s tick stays as fallback).");
        }
    }

    /// <summary>Locates Open(ProductDefinition, int, float, MSGConversation, Action&lt;...&gt;) by shape.</summary>
    private static MethodInfo? FindOpenMethod()
    {
        try
        {
            foreach (MethodInfo m in typeof(CounterofferInterface).GetMethods(
                         BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (m.Name != nameof(CounterofferInterface.Open)) continue;
                ParameterInfo[] ps = m.GetParameters();
                if (ps.Length == 5 && ps[1].ParameterType == typeof(int) && ps[2].ParameterType == typeof(float))
                    return m;
            }
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"CounterofferInterfacePatch: overload lookup failed: {ex.Message}");
        }

        return null;
    }

    [HarmonyPostfix]
    public static void Open_Postfix(CounterofferInterface __instance)
    {
        try
        {
            MessagesPlusConfig? cfg = ModConfig<MessagesPlusConfig>.Instance;
            if (cfg == null || !cfg.DarkMode) return;
            if (__instance == null || !NetworkGuard.IsAlive(__instance)) return;

            AppTheme.ApplyToSubtree(__instance.gameObject);
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"CounterofferInterface theme refresh failed: {ex.Message}");
        }
    }
}

/// <summary>
/// Same-frame dark theme for the product picker inside the counter-offer flow
/// (<see cref="CounterOfferProductSelector"/> — opens on top of the counter-offer
/// popup). No-argument Open().
/// </summary>
public static class CounterOfferProductSelectorPatch
{
    public static void ApplyAll(HarmonyLib.Harmony harmony, ModLogger log)
    {
        MethodInfo? open = FindOpenMethod();
        if (open != null)
        {
            PatchGuard.TryPatch(harmony, open, postfix: new HarmonyMethod(typeof(CounterOfferProductSelectorPatch), nameof(Open_Postfix)), log: log);
        }
        else
        {
            log.Warn("CounterOfferProductSelectorPatch: Open() not found — hook skipped (the 1 s tick stays as fallback).");
        }
    }

    /// <summary>Locates the no-argument Open() by shape.</summary>
    private static MethodInfo? FindOpenMethod()
    {
        try
        {
            foreach (MethodInfo m in typeof(CounterOfferProductSelector).GetMethods(
                         BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (m.Name != nameof(CounterOfferProductSelector.Open)) continue;
                ParameterInfo[] ps = m.GetParameters();
                if (ps.Length == 0)
                    return m;
            }
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"CounterOfferProductSelectorPatch: overload lookup failed: {ex.Message}");
        }

        return null;
    }

    [HarmonyPostfix]
    public static void Open_Postfix(CounterOfferProductSelector __instance)
    {
        try
        {
            MessagesPlusConfig? cfg = ModConfig<MessagesPlusConfig>.Instance;
            if (cfg == null || !cfg.DarkMode) return;
            if (__instance == null || !NetworkGuard.IsAlive(__instance)) return;

            AppTheme.ApplyToSubtree(__instance.gameObject);
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"CounterOfferProductSelector theme refresh failed: {ex.Message}");
        }
    }
}
