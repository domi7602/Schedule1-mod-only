using System;
using System.Collections.Generic;
using S1Mods.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace MessagesPlus;

/// <summary>
/// Hue-preserving brightness lift for game art that encodes meaning but was
/// painted for a LIGHT surface - the dialogue header's friendship/relationship
/// bar. The dark theme darkens the header, so the bar's dark jewel tones sit
/// dark-on-dark. This copies the sprite texels, scales each pixel so its max
/// channel reaches <see cref="TargetPeak"/> while keeping the channel RATIOS
/// (hue is exact), damps the scale by the texel alpha (anti-aliased edges stay
/// fringe - no white halo), and installs the copy into the Image's PLAIN sprite
/// slot (never overrideSprite). A stale dark ColorBlock on the bar's Selectable
/// is reset toward white so its state multiply stops crushing the art.
///
/// Never throws; every skip is logged at Info/Warn so a screenshot report can be
/// attributed from Latest.log.
/// </summary>
internal static class BarLift
{
    private const float TargetPeak = 0.5f;    // target max channel of the lifted art
    private const float ScaleCap = 25f;       // cap: near-black fringe must not blow up
    private const float Eps = 1f / 255f;
    private const float FrameMutePeak = 0.30f;   // near-white track/frame -> muted grey
    private const float FrameMuteAbove = 0.72f;  // min channel above this = near-white
    private const float FrameMuteSpread = 0.12f; // ...and near-neutral

    private static readonly HashSet<int> _lifted = new();           // Image ids we lifted
    private static readonly Dictionary<int, Sprite> _owned = new(); // our sprites (freed on scene unload)
    private static readonly HashSet<int> _blockFixed = new();       // Selectable ids already reset

    /// <summary>Lifts every sprite-bearing Image under a bar root and resets a stale dark ColorBlock.</summary>
    public static void Lift(Transform root, string label)
    {
        if (root == null || !NetworkGuard.IsAlive(root)) return;

        Image[]? images = null;
        try { images = root.GetComponentsInChildren<Image>(true); } catch { return; }
        if (images == null) return;

        for (int i = 0; i < images.Length; i++)
        {
            Image? img = images[i];
            if (img == null || !NetworkGuard.IsAlive(img)) continue;
            try { ResetBlockIfDark(img, label); } catch { /* insurance only */ }
            try { LiftImage(img, label); }
            catch (Exception ex) { Log($"{label}: lift failed: {ex.Message}"); }
        }
    }

    /// <summary>Destroys our lifted sprites/textures on scene unload (the IL2CPP GC never frees them).</summary>
    public static void ReleaseAll()
    {
        foreach (KeyValuePair<int, Sprite> kv in _owned)
        {
            try
            {
                Sprite? sp = kv.Value;
                if (sp == null) continue;
                Texture2D? tex = null;
                try { tex = sp.texture; } catch { /* keep going */ }
                UnityEngine.Object.Destroy(sp);
                if (tex != null) UnityEngine.Object.Destroy(tex);
            }
            catch { /* best effort */ }
        }
        _owned.Clear();
        _lifted.Clear();
        _blockFixed.Clear();
    }

    /// <summary>Lifts one image's sprite (hue-preserving; near-white frame muted).</summary>
    public static void LiftImage(Image img, string label)
    {
        int id;
        try { id = img.GetInstanceID(); } catch { return; }

        if (_lifted.Contains(id))
        {
            // Only a still-installed copy counts; a reused id of a new object does not.
            if (_owned.TryGetValue(id, out Sprite? mine) && mine != null)
            {
                try { if (img.sprite != null && img.sprite.Pointer == mine.Pointer) return; } catch { /* re-lift */ }
            }
            _lifted.Remove(id);
        }

        Sprite? src;
        try { src = img.sprite; } catch { return; }
        if (src == null) return; // no art - nothing to lift

        Rect rect, tr;
        try { rect = src.rect; tr = src.textureRect; } catch { Log($"{label}: skip (rect read failed)"); return; }
        if (Mathf.Abs(rect.width - tr.width) > 0.5f || Mathf.Abs(rect.height - tr.height) > 0.5f)
        {
            Log($"{label}: skip trimmed sprite ({rect.width:0}x{rect.height:0} vs {tr.width:0}x{tr.height:0})");
            return;
        }

        Texture2D? tex;
        try { tex = src.texture; } catch { return; }
        if (tex == null) { Log($"{label}: skip (no texture)"); return; }

        int tw = Mathf.RoundToInt(tr.width);
        int th = Mathf.RoundToInt(tr.height);
        if (tw <= 0 || th <= 0 || tex.width <= 0 || tex.height <= 0) { Log($"{label}: skip (bad rect)"); return; }

        Color32[]? px = null;
        string path;
        try { px = ReadTexels(tex, tr, tw, th, out path); }
        catch (Exception ex) { Log($"{label}: skip (copy failed: {ex.Message})"); return; }
        if (px == null) { Log($"{label}: skip (no texels)"); return; }

        try
        {
            int changed = 0;
            int frameMuted = 0;
            for (int i = 0; i < px.Length; i++)
            {
                Color32 c = px[i];
                if (c.a == 0) continue;
                float r = c.r / 255f, g = c.g / 255f, b = c.b / 255f;
                float mx = Mathf.Max(r, Mathf.Max(g, b));
                float mn = Mathf.Min(r, Mathf.Min(g, b));
                if (mx <= Eps) continue; // pure black has no hue to preserve

                // Near-white + neutral = the bar's light track/frame, painted for the
                // vanilla LIGHT header. Mute it toward the dark theme instead of
                // lifting it; a coloured segment lifts (max channel to the target
                // peak, ratios preserved). Both are damped by alpha so edges stay
                // smooth and no halo appears.
                bool isFrame = mn > FrameMuteAbove && (mx - mn) < FrameMuteSpread;
                float factor = isFrame
                    ? FrameMutePeak / mx
                    : Mathf.Clamp(TargetPeak / mx, 1f, ScaleCap);
                float eff = 1f + ((factor - 1f) * (c.a / 255f));
                byte nr = ToByte(r * eff), ng = ToByte(g * eff), nb = ToByte(b * eff);
                if (nr != c.r || ng != c.g || nb != c.b)
                {
                    changed++;
                    if (isFrame) frameMuted++;
                }
                px[i] = new Color32(nr, ng, nb, c.a);
            }

            if (changed == 0)
            {
                Log($"{label}: {tw}x{th} sprite already themed ({path}) - no change");
                return;
            }

            Texture2D readable = new(tw, th, TextureFormat.RGBA32, false);
            readable.SetPixels32(px);
            readable.Apply(false, true);

            Vector2 pivot;
            try { pivot = src.pivot; } catch { pivot = new Vector2(tw * 0.5f, th * 0.5f); }
            float ppu;
            try { ppu = src.pixelsPerUnit; } catch { ppu = 100f; }
            Vector4 border;
            try { border = src.border; } catch { border = Vector4.zero; }

            Sprite lifted = Sprite.Create(readable, new Rect(0, 0, tw, th), pivot, ppu, 0,
                SpriteMeshType.FullRect, border);
            img.sprite = lifted; // PLAIN slot - never overrideSprite
            _lifted.Add(id);
            _owned[id] = lifted;
            Log($"{label}: themed {tw}x{th} sprite ({changed} px, {frameMuted} frame px muted, peak={TargetPeak:0.00}, {path})");
        }
        catch (Exception ex)
        {
            Log($"{label}: skip (lift failed: {ex.Message})");
        }
    }

    /// <summary>
    /// Copies the sprite's textureRect region into a Color32 array. Reads a
    /// readable texture directly (exact orientation); otherwise blits the region
    /// into a temporary RenderTexture and reads it back (atlas-packed sprites are
    /// not CPU-readable). <paramref name="path"/> reports which route ran.
    /// </summary>
    private static Color32[]? ReadTexels(Texture2D tex, Rect tr, int tw, int th, out string path)
    {
        if (tex.isReadable)
        {
            path = "readable";
            Color32[] all = tex.GetPixels32();
            Color32[] px = new Color32[tw * th];
            int x0 = Mathf.RoundToInt(tr.x);
            int y0 = Mathf.RoundToInt(tr.y);
            for (int y = 0; y < th; y++)
            {
                Array.Copy(all, ((y0 + y) * tex.width) + x0, px, y * tw, tw);
            }
            return px;
        }

        path = "blit";
        RenderTexture? rt = null;
        RenderTexture? prev = null;
        try
        {
            rt = RenderTexture.GetTemporary(tw, th, 0, RenderTextureFormat.ARGB32);
            if (rt == null) return null;
            Graphics.Blit(tex, rt, new Vector2((float)tw / tex.width, (float)th / tex.height),
                new Vector2(tr.x / tex.width, tr.y / tex.height));

            prev = RenderTexture.active;
            RenderTexture.active = rt;
            Texture2D readable = new(tw, th, TextureFormat.RGBA32, false);
            readable.ReadPixels(new Rect(0, 0, tw, th), 0, 0);
            readable.Apply(false, false);
            Color32[] px = readable.GetPixels32();
            UnityEngine.Object.Destroy(readable);
            return px;
        }
        finally
        {
            try { if (prev != null) RenderTexture.active = prev; } catch { /* ignore */ }
            try { if (rt != null) RenderTexture.ReleaseTemporary(rt); } catch { /* ignore */ }
        }
    }

    private static void ResetBlockIfDark(Image img, string label)
    {
        Selectable? sel;
        try { sel = img.GetComponentInParent<Selectable>(); } catch { return; }
        if (sel == null || !NetworkGuard.IsAlive(sel)) return;

        int id;
        try { id = sel.GetInstanceID(); } catch { return; }
        if (_blockFixed.Contains(id)) return;

        try
        {
            if (sel.transition != Selectable.Transition.ColorTint) return;
            ColorBlock block = sel.colors;
            Color n = block.normalColor;
            if (MaxChannel(n) >= 0.5f) return; // not crushed by the state colour

            block.colorMultiplier = 1f;
            block.normalColor = Color.white;
            block.highlightedColor = Color.white;
            block.pressedColor = Color.white;
            block.selectedColor = Color.white;
            block.disabledColor = new Color(1f, 1f, 1f, 0.5f);
            sel.colors = block;
            try { if (sel.targetGraphic != null) sel.targetGraphic.CrossFadeColor(Color.white, 0f, true, true); } catch { /* ignore */ }
            _blockFixed.Add(id);
            Log($"{label}: reset dark ColorBlock (normal was {n.r:0.00},{n.g:0.00},{n.b:0.00})");
        }
        catch { /* insurance only */ }
    }

    private static byte ToByte(float v) => (byte)Mathf.Clamp(Mathf.RoundToInt(v * 255f), 0, 255);

    private static float MaxChannel(Color c) => Mathf.Max(c.r, Mathf.Max(c.g, c.b));

    private static void Log(string msg) => Mod.Log?.Info($"BarLift: {msg}");
}
