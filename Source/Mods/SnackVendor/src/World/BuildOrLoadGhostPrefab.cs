extern alias il2cpp;

using System;
using System.IO;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Attributes;
using MelonLoader;
using UnityEngine;
using S1Mods.Shared;

namespace SnackVendor.World;

/// <summary>
/// Builds the Ghost-Prefab for place-mode (low-res preview shown while the
/// player picks a tile) AND the thumbnail icon shown in shop menus. Mirrors
/// AutoPackMeshBuilder's pattern: one tiny renderer-icon sprite plus a
/// full-fidelity prefab for the GLB swap-in (used later by the controller).
/// </summary>
public static class BuildOrLoadGhostPrefab
{
    public const string ModName = "SnackVendor";
    private static Sprite? _cachedIconSprite;
    private static byte[]? _cachedGlbBytes;
    private static GameObject? _cachedGhost;

    /// <summary>
    /// 1×1 thumbnail sprite used in shop UIs. Built off a procedural rect
    /// (snack-bar horizontal stripe, Schedule-I furniture card proportions
    /// ~1:1.25). Falls back to a 32×32 flat block if Sprite.Create fails.
    /// </summary>
    public static Sprite GetOrCreateIconSprite()
    {
        if (_cachedIconSprite != null && _cachedIconSprite.Pointer != IntPtr.Zero) return _cachedIconSprite;
        try
        {
            const int W = 64, H = 80;
            var tex = new Texture2D(W, H, TextureFormat.RGBA32, false)
            {
                wrapMode = TextureWrapMode.Clamp,
                filterMode = FilterMode.Point,
            };
            var pixels = new Color32[W * H];
            // Snack Vendor red dominator + glass-pane horizontal stripe.
            for (int y = 0; y < H; y++)
                for (int x = 0; x < W; x++)
                {
                    bool glass = (y >= H / 2 - 12 && y <= H / 2 + 12) && x > 10 && x < W - 10;
                    bool logo = (y >= 8 && y <= 22) && (x % 16 < 10);
                    pixels[y * W + x] = glass switch
                    {
                        true => new Color32(120, 170, 200, 220),
                        false when logo => new Color32(250, 200, 35, 255),
                        _ => new Color32(200, 35, 30, 255),
                    };
                }
            tex.SetPixels32(pixels);
            tex.Apply();

            var rect = new Rect(0, 0, W, H);
            var pivot = new Vector2(0.5f, 0.5f);
            var s = Sprite.Create(tex, rect, pivot, 100f, 0, SpriteMeshType.FullRect);
            s.name = "SnackVendor_Icon";
            _cachedIconSprite = s;
            return s;
        }
        catch (Exception ex)
        {
            Mod.Log.Warn("icon sprite build failed, fallback", ex);
            _cachedIconSprite = Sprite.Create(new Texture2D(2, 2), new Rect(0, 0, 2, 2), Vector2.one * 0.5f);
            return _cachedIconSprite;
        }
    }

    /// <summary>
    /// Place-mode ghost: vanilla BoxCollider visual proxy. The real GLB
    /// is swapped in after placement by the controller; until then the
    /// ghost looks like a red 0.95×1.85×0.72 m box — close enough for
    /// grid-snapping visualization and *zero* IL2CPP construction risk
    /// during the vulnerable Build() window.
    /// </summary>
    public static GameObject CreateGhostPrefab()
    {
        if (_cachedGhost != null && _cachedGhost.Pointer != IntPtr.Zero) return _cachedGhost;
        try
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = "SnackVendor_GhostProxy";
            // Glb swap-in target size (from Blender model).
            go.transform.localScale = new Vector3(0.95f, 1.85f, 0.72f);
            // Remove collider — BuildableItem brings its own.
            var col = go.GetComponent<Collider>();
            if (col != null && col.Pointer != IntPtr.Zero) UnityEngine.Object.DestroyImmediate(col);
            // Snackbar-red material so it pops in place-mode.
            var mr = go.GetComponent<MeshRenderer>();
            if (mr != null && mr.Pointer != IntPtr.Zero)
            {
                mr.material = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard"))
                {
                    color = new Color(0.78f, 0.12f, 0.10f, 1f),
                };
            }
            _cachedGhost = go;
            go.SetActive(false); // kept as template, instantiated by S1API
            UnityEngine.Object.DontDestroyOnLoad(go);
            return go;
        }
        catch (Exception ex)
        {
            Mod.Log.Warn("ghost prefab failed", ex);
            // Fallback: empty placeholder so builder still gets *something*.
            var empty = new GameObject("SnackVendor_GhostFallback");
            _cachedGhost = empty;
            return empty;
        }
    }

    /// <summary>
    /// Loads the SnackVendor GLB from Mods/SnackVendor/model.glb (path
    /// matches AutoPack pattern). Lazy-loaded so path-not-found during
    /// icon-only mode doesn't throw.
    /// </summary>
    public static byte[]? GetGlbBytes()
    {
        if (_cachedGlbBytes != null) return _cachedGlbBytes;
        try
        {
            var path = Path.Combine(MelonLoader.Utils.MelonEnvironment.ModsDirectory, ModName, "model.glb");
            if (!File.Exists(path))
            {
                Mod.Log.Warn($"GLB not found at {path} (skipping custom mesh load)");
                return null;
            }
            _cachedGlbBytes = File.ReadAllBytes(path);
            return _cachedGlbBytes;
        }
        catch (Exception ex)
        {
            Mod.Log.Warn("glb read failed", ex);
            return null;
        }
    }
}
