using System;
using System.IO;
using CustomSkateboard.Config;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppScheduleOne.Equipping;
using Il2CppScheduleOne.Skating;
using UnityEngine;

namespace CustomSkateboard.Visuals;

/// <summary>
/// Handles applying custom cyberpunk materials, procedural textures, neon underglow, and trail effects to the Pro Cyber Skateboard.
/// </summary>
public static class CyberSkateboardVisualizer
{
    private static Texture2D? _cyberDeckTexture;
    private static Texture2D? _cyberWheelsTexture;
    private static Texture2D? _cyberTrucksTexture;

    private static Material? _cachedDeckMaterial;
    private static Material? _cachedWheelsMaterial;
    private static Material? _cachedTrucksMaterial;

    private static readonly Gradient _cyanTrailGradient = BuildCyanTrailGradient();

    // Log-noise fix 2026-09-01 (v3): per-instance dedup was ineffective (Unity spawns
    // a fresh board on every mount); a 10s time-cooldown was also ineffective because
    // typical remounts are >10s apart. Both attempts left the line firing on every
    // mount. Correct fix: this is purely informational — Debug-only is the right level.
    internal static void ClearStyledCache() { }

    private static Gradient BuildCyanTrailGradient()
    {
        Gradient g = new Gradient();
        g.SetKeys(
            new GradientColorKey[] {
                new(new Color(0.0f, 0.95f, 1.0f), 0.0f),
                new(new Color(1.0f, 0.85f, 0.2f), 1.0f)
            },
            new GradientAlphaKey[] {
                new(0.95f, 0.0f),
                new(0.0f, 1.0f)
            }
        );
        return g;
    }

    public static void InitializeAssets(Material? baseTemplate = null)
    {
        try
        {
            if (_cyberDeckTexture == null)
            {
                _cyberDeckTexture = GenerateCyberDeckTexture();
            }
            if (_cyberWheelsTexture == null)
            {
                _cyberWheelsTexture = GenerateCyberWheelsTexture();
            }
            if (_cyberTrucksTexture == null)
            {
                _cyberTrucksTexture = GenerateCyberTrucksTexture();
            }

            if (_cachedDeckMaterial == null && _cyberDeckTexture != null)
            {
                _cachedDeckMaterial = CreateCyberMaterial(baseTemplate, _cyberDeckTexture, new Color(0.0f, 0.85f, 1.0f) * 2.5f, 0.8f, 0.85f);
            }
            if (_cachedWheelsMaterial == null && _cyberWheelsTexture != null)
            {
                _cachedWheelsMaterial = CreateCyberMaterial(baseTemplate, _cyberWheelsTexture, new Color(0.0f, 0.95f, 1.0f) * 3.0f, 0.3f, 0.9f);
            }
            if (_cachedTrucksMaterial == null && _cyberTrucksTexture != null)
            {
                _cachedTrucksMaterial = CreateCyberMaterial(baseTemplate, _cyberTrucksTexture, new Color(1.0f, 0.8f, 0.2f) * 1.5f, 0.95f, 0.9f);
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"Asset initialization notice: {ex.Message}");
        }
    }

    private static Shader? FindSafeShader()
    {
        try
        {
            return Shader.Find("Universal Render Pipeline/Lit")
                ?? Shader.Find("Standard")
                ?? Shader.Find("Diffuse")
                ?? Shader.Find("Unlit/Texture")
                ?? Shader.Find("Sprites/Default");
        }
        catch
        {
            return null;
        }
    }

    private static Texture2D GenerateCyberDeckTexture()
    {
        int w = 256, h = 256;
        Texture2D tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
        tex.name = "CyberDeckTex";
        Color carbonDark = new Color(0.06f, 0.07f, 0.09f, 1f);
        Color carbonLight = new Color(0.12f, 0.14f, 0.18f, 1f);
        Color neonCyan = new Color(0.0f, 0.95f, 1.0f, 1f);
        Color goldAccent = new Color(1.0f, 0.82f, 0.2f, 1f);

        Color[] pixels = new Color[w * h];
        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                // Carbon weave pattern
                bool weave = ((x / 4) + (y / 4)) % 2 == 0;
                Color c = weave ? carbonDark : carbonLight;

                // Dual Neon Cyan Racing Stripes down the middle
                if ((x >= 118 && x <= 124) || (x >= 132 && x <= 138))
                {
                    c = neonCyan;
                }
                // Gold edge lines
                else if (x == 115 || x == 141 || x <= 4 || x >= 251)
                {
                    c = goldAccent;
                }
                // Cyber circuit diagonal accents
                else if ((x + y) % 32 == 0 && (x < 80 || x > 176))
                {
                    c = neonCyan * 0.8f;
                }

                pixels[y * w + x] = c;
            }
        }
        tex.SetPixels(pixels);
        tex.Apply();
        return tex;
    }

    private static Texture2D GenerateCyberWheelsTexture()
    {
        int w = 64, h = 64;
        Texture2D tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
        tex.name = "CyberWheelsTex";
        Color cyanCore = new Color(0.0f, 0.95f, 1.0f, 1f);
        Color cyanRim = new Color(0.1f, 0.5f, 0.8f, 1f);

        Color[] pixels = new Color[w * h];
        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                float dx = x - 32f, dy = y - 32f;
                float r = Mathf.Sqrt(dx * dx + dy * dy);
                pixels[y * w + x] = (r > 20f && r < 28f) ? cyanCore : cyanRim;
            }
        }
        tex.SetPixels(pixels);
        tex.Apply();
        return tex;
    }

    private static Texture2D GenerateCyberTrucksTexture()
    {
        int w = 32, h = 32;
        Texture2D tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
        tex.name = "CyberTrucksTex";
        Color gold = new Color(1.0f, 0.8f, 0.2f, 1f);
        Color darkGold = new Color(0.7f, 0.5f, 0.1f, 1f);

        Color[] pixels = new Color[w * h];
        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                pixels[y * w + x] = (x % 4 == 0) ? gold : darkGold;
            }
        }
        tex.SetPixels(pixels);
        tex.Apply();
        return tex;
    }

    private static Material? CreateCyberMaterial(Material? baseTemplate, Texture2D customTexture, Color emissionColor, float metallic = 0.8f, float smoothness = 0.85f)
    {
        try
        {
            Material mat;
            if (baseTemplate != null && baseTemplate.Pointer != IntPtr.Zero)
            {
                mat = new Material(baseTemplate);
            }
            else
            {
                Shader? shader = FindSafeShader();
                if (shader == null)
                {
                    Mod.Log.Warn("Could not find a valid shader to create cyber material.");
                    return null;
                }
                mat = new Material(shader);
            }

            mat.name = $"M_CyberSkateboard_{customTexture.name}";

            // Assign textures for both Standard and URP shaders
            mat.mainTexture = customTexture;
            if (mat.HasProperty("_MainTex")) mat.SetTexture("_MainTex", customTexture);
            if (mat.HasProperty("_BaseMap")) mat.SetTexture("_BaseMap", customTexture);

            // Assign colors
            if (mat.HasProperty("_Color")) mat.SetColor("_Color", Color.white);
            if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", Color.white);

            // Metallic & Smoothness
            if (mat.HasProperty("_Metallic")) mat.SetFloat("_Metallic", metallic);
            if (mat.HasProperty("_Glossiness")) mat.SetFloat("_Glossiness", smoothness);
            if (mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness", smoothness);

            // Emission
            if (mat.HasProperty("_EmissionColor")) mat.EnableKeyword("_EMISSION");
            if (mat.HasProperty("_EmissionColor")) mat.SetColor("_EmissionColor", emissionColor);
            if (mat.HasProperty("_EmissionMap")) mat.SetTexture("_EmissionMap", customTexture);

            return mat;
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"Failed to create cyber material: {ex.Message}");
            return null;
        }
    }

    private static bool IsSkateboardPart(string name)
    {
        string n = name.ToLowerInvariant();
        return n.Contains("wheel") || n.Contains("truck") || n.Contains("deck")
            || n.Contains("board") || n.Contains("axle") || n.Contains("hanger")
            || n.Contains("grip") || n.Contains("skate");
    }

    private static bool IsPlayerAvatarPart(string name)
    {
        string n = name.ToLowerInvariant();
        if (IsSkateboardPart(n)) return false;
        return n.Contains("hair") || n.Contains("eye") || n.Contains("head")
            || n.Contains("face") || n.Contains("body") || n.Contains("arm")
            || n.Contains("hand") || n.Contains("leg") || n.Contains("foot")
            || n.Contains("cloth") || n.Contains("skin") || n.Contains("avatar")
            || n.Contains("player") || n.Contains("character") || n.Contains("mesh_character");
    }

    // Gatekeeper-fix B7: robust deck mesh heuristic — prefer exact "deck" name with sanity check, fallback to Contains.
    private static bool IsExactDeckName(string name)
    {
        return string.Equals(name, "deck", StringComparison.OrdinalIgnoreCase)
            || string.Equals(name, "deck_mesh", StringComparison.OrdinalIgnoreCase)
            || name.EndsWith("_deck", StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsDeckMeshSane(MeshFilter mf)
    {
        try
        {
            var m = mf.sharedMesh;
            if (m == null) return false;
            // Sanity: deck must have vertices and non-degenerate bounds; reject Combined/Root mega-meshes.
            if (m.vertexCount < 20 || m.vertexCount > 50000) return false;
            var b = m.bounds;
            float maxExtent = Mathf.Max(b.extents.x, Mathf.Max(b.extents.y, b.extents.z));
            // Deck is ~2m long, so extents ~1m; Combined root mesh often has >>5m extents.
            if (maxExtent > 5f || maxExtent < 0.05f) return false;
            return true;
        }
        catch { return false; }
    }

    private static bool TrySwapDeckMesh(Transform searchRoot, Mesh customMesh)
    {
        var meshFilters = searchRoot.GetComponentsInChildren<MeshFilter>(true);
        if (meshFilters == null) return false;

        // Pass 1: prefer exact "deck" name + sanity check (avoids hitting "Combined" or "Root/board_collision").
        foreach (var mf in meshFilters)
        {
            if (mf == null || mf.gameObject == null) continue;
            string mfName = mf.gameObject.name.ToLowerInvariant();
            string sharedMeshName = (mf.sharedMesh != null) ? mf.sharedMesh.name.ToLowerInvariant() : "";
            bool exact = IsExactDeckName(mf.gameObject.name) || IsExactDeckName(sharedMeshName);
            bool isAvatarPart = IsPlayerAvatarPart(mfName) || IsPlayerAvatarPart(sharedMeshName);
            if (exact && !isAvatarPart && IsDeckMeshSane(mf))
            {
                if (mf.sharedMesh != customMesh) mf.sharedMesh = customMesh;
                return true;
            }
        }
        // Pass 2: fallback Contains("deck") / Contains("board") — original heuristic for variants.
        foreach (var mf in meshFilters)
        {
            if (mf == null || mf.gameObject == null) continue;
            string mfName = mf.gameObject.name.ToLowerInvariant();
            string sharedMeshName = (mf.sharedMesh != null) ? mf.sharedMesh.name.ToLowerInvariant() : "";
            bool isDeckCandidate = mfName.Contains("deck", StringComparison.OrdinalIgnoreCase) || mfName.Contains("board", StringComparison.OrdinalIgnoreCase)
                || sharedMeshName.Contains("deck", StringComparison.OrdinalIgnoreCase) || sharedMeshName.Contains("board", StringComparison.OrdinalIgnoreCase)
                || mfName.EndsWith("_deck", StringComparison.OrdinalIgnoreCase) || mfName.EndsWith("_board", StringComparison.OrdinalIgnoreCase);
            bool isAvatarPart = IsPlayerAvatarPart(mfName) || IsPlayerAvatarPart(sharedMeshName);
            if (isDeckCandidate && !isAvatarPart)
            {
                if (mf.sharedMesh != customMesh) mf.sharedMesh = customMesh;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Applies the custom glowing materials, underglow light, and trails strictly to the skateboard model (never to player avatar).
    /// </summary>
    public static void ApplyToSkateboard(Skateboard skateboard, SkateboardConfig config)
    {
        try
        {
            if (skateboard == null || skateboard.Pointer == IntPtr.Zero) return;

            var visuals = skateboard.GetComponentInChildren<SkateboardVisuals>();
            Transform? boardRoot = (visuals != null && visuals.Board != null) ? visuals.Board : null;

            // If Board transform is found, ONLY search within boardRoot to avoid player avatar
            MeshRenderer[]? renderers = boardRoot != null
                ? boardRoot.GetComponentsInChildren<MeshRenderer>(true)
                : skateboard.GetComponentsInChildren<MeshRenderer>(true);

            // Find a base template material BEFORE initializing cached assets, so materials can clone the right shader
            Material? baseTemplate = null;
            if (renderers != null)
            {
                foreach (var r in renderers)
                {
                    if (r != null && r.sharedMaterial != null)
                    {
                        baseTemplate = r.sharedMaterial;
                        break;
                    }
                }
            }
            InitializeAssets(baseTemplate);

            // Gatekeeper-fix B7: robust two-pass heuristic (exact "deck" + sanity, then Contains fallback) to avoid hitting Combined/Root.
            Mesh? customMesh = ObjLoader.TryGetOrLoadDeckMesh();
            if (customMesh != null)
            {
                Transform searchRoot = boardRoot ?? skateboard.transform;
                TrySwapDeckMesh(searchRoot, customMesh);
            }

            if (renderers != null && renderers.Length > 0)
            {
                foreach (var r in renderers)
                {
                    if (r == null || r.gameObject == null) continue;
                    string n = r.gameObject.name.ToLowerInvariant();

                    // Explicitly ignore any player parts!
                    if (IsPlayerAvatarPart(n)) continue;

                    Material? targetMat = null;
                    if (n.Contains("wheel"))
                    {
                        targetMat = _cachedWheelsMaterial;
                    }
                    else if (n.Contains("truck") || n.Contains("axle") || n.Contains("hanger"))
                    {
                        targetMat = _cachedTrucksMaterial;
                    }
                    else if (n.Contains("board") || n.Contains("deck") || n.Contains("grip") || n.Contains("skate"))
                    {
                        targetMat = _cachedDeckMaterial;
                    }
                    else
                    {
                        continue; // Unknown or non-skateboard renderer, skip safely
                    }

                    if (targetMat == null) continue;

                    // Replace all material slots
                    var count = r.sharedMaterials.Length;
                    if (count > 0)
                    {
                        var newMats = new Il2CppReferenceArray<Material>(count); // Acceptable: one-time styling on mount
                        for (int i = 0; i < count; i++)
                        {
                            newMats[i] = targetMat;
                        }
                        r.sharedMaterials = newMats;
                    }
                    else
                    {
                        r.sharedMaterial = targetMat;
                    }
                }
            }

            // Tune dynamic visuals (lean angle)
            if (visuals != null)
            {
                visuals.MaxBoardLean = config.MaxBoardLean;
                visuals.BoardLeanRate = config.BoardLeanRate;
            }

            // Attach Neon Underglow Light (check idempotency to prevent duplicates)
            Transform underglowParent = boardRoot ?? skateboard.transform;
            Light? existingLight = null;
            var allLights = underglowParent.GetComponentsInChildren<Light>(true);
            if (allLights != null)
            {
                foreach (var l in allLights)
                {
                    if (l != null && l.gameObject != null && l.gameObject.name == "CyberUnderglowLight")
                    {
                        existingLight = l;
                        break;
                    }
                }
            }

            if (existingLight == null)
            {
                GameObject underglowObj = new GameObject("CyberUnderglowLight");
                underglowObj.transform.SetParent(underglowParent, false);
                underglowObj.transform.localPosition = new Vector3(0f, 0.08f, 0f);

                Light light = underglowObj.AddComponent<Light>();
                light.type = LightType.Point;
                light.color = new Color(0.0f, 0.95f, 1.0f);
                light.range = 3.5f;
                light.intensity = 3.0f;
                light.shadows = LightShadows.None;
                light.renderMode = LightRenderMode.ForcePixel;
            }

            // Tint speed trails in SkateboardEffects
            var effects = skateboard.GetComponentInChildren<SkateboardEffects>();
            if (effects != null && effects.Trails != null)
            {
                foreach (var trail in effects.Trails)
                {
                    if (trail != null)
                    {
                        trail.colorGradient = _cyanTrailGradient;
                        trail.startWidth = 0.09f;
                    }
                }
            }

            // Log-noise fix 2026-09-01 (v3): debug-level (silent in Release builds).
            Mod.Log.Debug("In-world skateboard cleanly styled (Player avatar strictly untouched).");
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"Visual application on in-world board: {ex.Message}");
        }
    }

    /// <summary>
    /// Applies custom materials strictly to the viewmodel held in the player's hands.
    /// Gatekeeper-fix B17: now also swaps deck mesh (same two-pass heuristic as world board) — previously materials only.
    /// Viewmodel mesh swap is intentional to keep first-person and world appearance consistent.
    /// </summary>
    public static void ApplyToViewmodel(GameObject viewmodelObj)
    {
        try
        {
            if (viewmodelObj == null) return;

            var renderers = viewmodelObj.GetComponentsInChildren<MeshRenderer>(true);
            Material? baseTemplate = null;
            if (renderers != null)
            {
                foreach (var r in renderers)
                {
                    if (r != null && r.sharedMaterial != null)
                    {
                        baseTemplate = r.sharedMaterial;
                        break;
                    }
                }
            }
            InitializeAssets(baseTemplate);

            // Gatekeeper-fix B17: swap deck mesh in viewmodel as well (was materials-only before).
            Mesh? customMesh = ObjLoader.TryGetOrLoadDeckMesh();
            if (customMesh != null)
            {
                TrySwapDeckMesh(viewmodelObj.transform, customMesh);
            }

            if (renderers != null && renderers.Length > 0)
            {
                foreach (var r in renderers)
                {
                    if (r == null || r.gameObject == null) continue;
                    string n = r.gameObject.name.ToLowerInvariant();

                    if (IsPlayerAvatarPart(n)) continue;

                    Material? targetMat = null;
                    if (n.Contains("wheel"))
                    {
                        targetMat = _cachedWheelsMaterial;
                    }
                    else if (n.Contains("truck") || n.Contains("axle") || n.Contains("hanger"))
                    {
                        targetMat = _cachedTrucksMaterial;
                    }
                    else if (n.Contains("board") || n.Contains("deck") || n.Contains("grip") || n.Contains("skate"))
                    {
                        targetMat = _cachedDeckMaterial;
                    }
                    else
                    {
                        continue;
                    }

                    if (targetMat == null) continue;

                    var count = r.sharedMaterials.Length;
                    if (count > 0)
                    {
                        var newMats = new Il2CppReferenceArray<Material>(count); // Acceptable: one-time styling on mount
                        for (int i = 0; i < count; i++) newMats[i] = targetMat;
                        r.sharedMaterials = newMats;
                    }
                    else
                    {
                        r.sharedMaterial = targetMat;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"Viewmodel visual application: {ex.Message}");
        }
    }
}
