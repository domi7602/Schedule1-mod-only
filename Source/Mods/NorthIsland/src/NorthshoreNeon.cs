using System;
using System.Collections.Generic;
using Il2CppTMPro;
using MelonLoader;
using UnityEngine;
using UnityEngine.Rendering;

namespace NorthIsland;

/// <summary>
/// "Northshore Nights" decor layer (Phase 3, package A): illuminated signage lettering on the
/// existing welcome/store/diner panels, pink/cyan neon tubes on the diner roof edge, warm
/// window glow on the coastal houses and storefronts, sagging string lights between the
/// existing north-boardwalk lamp poles, and small lanterns on the coast guardrail posts.
/// Everything is additive: accepted Phase 1 geometry is only referenced, never reshaped.
/// Glow uses unlit materials (no extra realtime lights) to respect URP per-object light limits.
/// </summary>
internal sealed class NorthshoreNeon
{
    private readonly List<Material> _materials = new();
    private Material? _neonPinkMat;
    private Material? _neonCyanMat;
    private Material? _bulbMat;
    private Material? _windowMat;
    private Material? _darkPanelMat;
    private Material? _wireMat;
    private Material? _darkMetalMat;

    internal int TextCount { get; private set; }
    internal int SpanCount { get; private set; }
    internal int BulbCount { get; private set; }
    internal int GlowCount { get; private set; }

    internal void Build(Transform townRoot)
    {
        TextCount = 0;
        SpanCount = 0;
        BulbCount = 0;
        GlowCount = 0;

        try
        {
            EnsureMaterials();
            TMP_FontAsset? font = FindDonorFont();
            if (font == null)
                MelonLogger.Warning("[NorthIsland:Neon] Keine TMP-Font-Spender gefunden (Barn-Kaufschild) – Schriftzüge werden übersprungen, Glow bleibt.");

            AddSignage(townRoot, font);
            AddDinerNeonTubes(townRoot);
            AddWindowGlow(townRoot);
            BuildStringLights(townRoot);
            BuildRailLanterns(townRoot);

            MelonLogger.Msg(
                $"[NorthIsland:Neon] Northshore Nights aufgebaut: {TextCount} Schriftzüge, {SpanCount} Lichterketten-Spannen ({BulbCount} Birnen), {GlowCount} Glow-Elemente.");
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[NorthIsland:Neon] Dekor-Aufbau fehlgeschlagen (Basistadt bleibt intakt): {ex}");
        }
    }

    internal void DisposeAssets()
    {
        foreach (var mat in _materials)
        {
            if (mat != null) UnityEngine.Object.Destroy(mat);
        }
        _materials.Clear();
        _neonPinkMat = _neonCyanMat = _bulbMat = _windowMat = _darkPanelMat = _wireMat = _darkMetalMat = null;
    }

    // =========================================================================
    // SIGNAGE LETTERING (TMP on existing panels; font borrowed from the Barn sign)
    // =========================================================================

    private static TMP_FontAsset? FindDonorFont()
    {
        try
        {
            var props = Il2CppScheduleOne.Property.Property.Properties;
            if (props == null) return null;
            for (int i = 0; i < props.Count; i++)
            {
                var property = props[i];
                if (property == null || property.ForSaleSign == null) continue;
                foreach (var text in property.ForSaleSign.GetComponentsInChildren<TMP_Text>(true))
                {
                    if (text != null && text.font != null)
                        return text.font;
                }
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[NorthIsland:Neon] Font-Spendersuche fehlgeschlagen: {ex.Message}");
        }
        return null;
    }

    private void AddSignage(Transform townRoot, TMP_FontAsset? font)
    {
        // Welcome sign faces SOUTH toward the off-ramp; its glow panel sits on the board's south face.
        // TMP lettering reads from -Z at identity rotation (observed 2026-11-16: 180° yaw mirrored it).
        var welcomePanel = townRoot.Find("Town_Props_And_Nature/WelcomeSign_GlowPanel");
        if (welcomePanel != null)
        {
            AddText(welcomePanel.parent, "WelcomeSign_Lettering",
                welcomePanel.position + new Vector3(0f, 0f, -0.05f),
                Quaternion.identity,
                "WELCOME TO NORTHSHORE", new Color(0.28f, 0.17f, 0.08f),
                new Vector2(5.4f, 0.72f), 0.62f, font);
        }

        var store = townRoot.Find("Town_Buildings/Northshore_GeneralStore");
        if (store != null)
        {
            var sign = store.Find("Store_Sign");
            if (sign != null)
            {
                // Storefront faces NORTH toward the boulevard: yaw 180° so lettering reads from the road.
                AddText(store, "Store_Sign_Lettering",
                    sign.position + new Vector3(0f, 0f, 0.08f),
                    Quaternion.Euler(0f, 180f, 0f),
                    "GENERAL STORE", new Color(0.28f, 0.17f, 0.08f),
                    new Vector2(6.2f, 0.46f), 0.44f, font);
            }

            // Small OPEN sign on the east storefront window (panel proud of the wall face).
            Vector3 openPos = store.position + new Vector3(3.5f, 1.8f, 4.22f);
            TownBuilder.CreateBox(store, "Store_OpenPanel", openPos, new Vector3(1.05f, 0.5f, 0.05f), Quaternion.identity, _darkPanelMat, false);
            AddText(store, "Store_Open_Lettering", openPos + new Vector3(0f, 0f, 0.05f), Quaternion.Euler(0f, 180f, 0f),
                "OPEN", new Color(1.0f, 0.30f, 0.55f), new Vector2(0.9f, 0.36f), 0.34f, font);
            GlowCount++;
        }

        var diner = townRoot.Find("Town_Buildings/Northshore_BaysideDiner");
        if (diner != null)
        {
            Vector3 panelPos = diner.position + new Vector3(0f, 3.2f, 3.98f);
            TownBuilder.CreateBox(diner, "Diner_SignPanel", panelPos, new Vector3(7.6f, 0.8f, 0.10f), Quaternion.identity, _darkPanelMat, false);
            AddText(diner, "Diner_Sign_Lettering", panelPos + new Vector3(0f, 0f, 0.08f), Quaternion.Euler(0f, 180f, 0f),
                "BAYSIDE DINER", new Color(0.30f, 0.95f, 1.0f), new Vector2(7.2f, 0.62f), 0.6f, font);
            GlowCount++;
        }
    }

    private void AddText(Transform parent, string name, Vector3 pos, Quaternion rot,
        string text, Color color, Vector2 rectSize, float fontSizeMax, TMP_FontAsset? font)
    {
        if (font == null) return;
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        go.transform.position = pos;
        go.transform.rotation = rot;

        var tmp = go.AddComponent<TextMeshPro>();
        tmp.font = font;
        tmp.text = text;
        tmp.color = color;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.enableWordWrapping = false;
        tmp.enableAutoSizing = true;
        tmp.fontSizeMin = 0.08f;
        tmp.fontSizeMax = fontSizeMax;
        tmp.isOrthographic = true;
        tmp.rectTransform.sizeDelta = rectSize;
        TextCount++;
    }

    // =========================================================================
    // DINER NEON TUBES (pink above / cyan below the existing warm roof trim)
    // =========================================================================

    private void AddDinerNeonTubes(Transform townRoot)
    {
        var diner = townRoot.Find("Town_Buildings/Northshore_BaysideDiner");
        if (diner == null) return;

        // Existing Diner_NeonTrim: local (0, h+0.22=4.12, d*0.5+1.12=4.87), width w+0.85=11.85.
        TownBuilder.CreateBox(diner, "Diner_NeonPink", diner.position + new Vector3(0f, 4.34f, 4.87f),
            new Vector3(11.85f, 0.07f, 0.09f), Quaternion.identity, _neonPinkMat, false);
        TownBuilder.CreateBox(diner, "Diner_NeonCyan", diner.position + new Vector3(0f, 3.92f, 4.87f),
            new Vector3(11.85f, 0.07f, 0.09f), Quaternion.identity, _neonCyanMat, false);
        GlowCount += 2;
    }

    // =========================================================================
    // WARM WINDOW GLOW (houses use their visible window panels; storefronts sit
    // proud of the wall face because their glass panels are recessed)
    // =========================================================================

    private void AddWindowGlow(Transform townRoot)
    {
        foreach (string houseName in new[] { "CoastalHouse_West", "CoastalHouse_East" })
        {
            var house = townRoot.Find($"Town_Buildings/{houseName}");
            if (house == null) continue;
            foreach (string winName in new[] { "Win1_-2.8", "Win1_2.8", "Win2_-2.8", "Win2_2.8", "Win2_Center" })
            {
                var win = house.Find(winName);
                if (win == null) continue;
                Vector3 s = win.localScale;
                TownBuilder.CreateBox(house, $"{winName}_Glow",
                    win.position + new Vector3(0f, 0f, 0.07f),
                    new Vector3(s.x * 0.86f, s.y * 0.86f, 0.02f),
                    Quaternion.identity, _windowMat, false);
                GlowCount++;
            }
        }

        var store = townRoot.Find("Town_Buildings/Northshore_GeneralStore");
        if (store != null)
        {
            foreach (float wx in new[] { -3.5f, 3.5f })
            {
                TownBuilder.CreateBox(store, $"Store_WindowGlow_{wx:F1}",
                    store.position + new Vector3(wx, 1.8f, 4.18f),
                    new Vector3(3.0f, 1.6f, 0.02f), Quaternion.identity, _windowMat, false);
                GlowCount++;
            }
        }

        var diner = townRoot.Find("Town_Buildings/Northshore_BaysideDiner");
        if (diner != null)
        {
            foreach (float wx in new[] { -3.3f, 3.3f })
            {
                TownBuilder.CreateBox(diner, $"Diner_WindowGlow_{wx:F1}",
                    diner.position + new Vector3(wx, 1.7f, 3.93f),
                    new Vector3(2.8f, 1.4f, 0.02f), Quaternion.identity, _windowMat, false);
                GlowCount++;
            }
        }
    }

    // =========================================================================
    // STRING LIGHTS (LineRenderer wire + unlit bulbs between existing lamp poles)
    // =========================================================================

    private void BuildStringLights(Transform parent)
    {
        float[] xs = NorthIslandGeometry.NorthLampXs;
        const float z = 436.0f;
        const int samples = 24;

        for (int i = 0; i + 1 < xs.Length; i++)
        {
            float ax = xs[i];
            float bx = xs[i + 1];
            float ay = NorthIslandGeometry.GetBoulevardSurfaceY(ax) + 0.10f + NorthIslandGeometry.StringLightAttachHeight;
            float by = NorthIslandGeometry.GetBoulevardSurfaceY(bx) + 0.10f + NorthIslandGeometry.StringLightAttachHeight;

            var wireGo = new GameObject($"StringLight_Wire_{i}");
            wireGo.transform.SetParent(parent, false);
            var lr = wireGo.AddComponent<LineRenderer>();
            lr.useWorldSpace = true;
            lr.widthMultiplier = 0.025f;
            lr.sharedMaterial = _wireMat;
            lr.startColor = new Color(0.10f, 0.10f, 0.12f);
            lr.endColor = new Color(0.10f, 0.10f, 0.12f);
            lr.shadowCastingMode = ShadowCastingMode.Off;
            lr.receiveShadows = false;
            lr.positionCount = samples + 1;
            for (int s = 0; s <= samples; s++)
            {
                float t = (float)s / samples;
                lr.SetPosition(s, new Vector3(
                    Mathf.Lerp(ax, bx, t),
                    NorthIslandGeometry.GetStringLightY(ay, by, t),
                    z));
            }

            int bulbs = Mathf.Max(2, Mathf.RoundToInt((bx - ax) / 1.2f));
            for (int b = 1; b < bulbs; b++)
            {
                float t = (float)b / bulbs;
                TownBuilder.CreateBox(parent, $"StringLight_Bulb_{i}_{b}",
                    new Vector3(Mathf.Lerp(ax, bx, t), NorthIslandGeometry.GetStringLightY(ay, by, t) - 0.08f, z),
                    new Vector3(0.10f, 0.13f, 0.10f), Quaternion.identity, _bulbMat, false);
                BulbCount++;
            }
            SpanCount++;
        }
    }

    // =========================================================================
    // COAST GUARDRAIL LANTERNS (every second post, pier gap stays open)
    // =========================================================================

    private void BuildRailLanterns(Transform parent)
    {
        const float minX = NorthIslandGeometry.BoulevardMinX;
        const float maxX = NorthIslandGeometry.BoulevardMaxX;
        const int steps = 35;

        for (int i = 0; i < steps; i += 2)
        {
            float xm = minX + (maxX - minX) * (i + 0.5f) / steps;
            if (xm > NorthIslandGeometry.PierMinX - 0.6f && xm < NorthIslandGeometry.PierMaxX + 0.6f) continue;

            float ym = NorthIslandGeometry.GetBoulevardSurfaceY(xm);
            TownBuilder.CreateBox(parent, $"CoastLantern_{i}",
                new Vector3(xm, ym + 1.14f, 436.5f),
                new Vector3(0.15f, 0.22f, 0.15f), Quaternion.identity, _bulbMat, false);
            TownBuilder.CreateBox(parent, $"CoastLanternCap_{i}",
                new Vector3(xm, ym + 1.28f, 436.5f),
                new Vector3(0.22f, 0.05f, 0.22f), Quaternion.identity, _darkMetalMat, false);
            GlowCount += 2;
        }
    }

    // =========================================================================
    // MATERIALS (same URP Lit/Unlit fallback chain as TownBuilder)
    // =========================================================================

    private void EnsureMaterials()
    {
        if (_bulbMat != null) return;

        Shader lit = Shader.Find("Universal Render Pipeline/Lit")
                     ?? Shader.Find("Standard")
                     ?? Shader.Find("Diffuse")
                     ?? Shader.Find("Unlit/Color");
        Shader unlit = Shader.Find("Universal Render Pipeline/Unlit")
                       ?? Shader.Find("Unlit/Color")
                       ?? lit;

        _neonPinkMat = MakeMat(unlit, "NI_NeonPink", new Color(1.0f, 0.22f, 0.55f));
        _neonCyanMat = MakeMat(unlit, "NI_NeonCyan", new Color(0.25f, 0.95f, 1.0f));
        _bulbMat = MakeMat(unlit, "NI_StringBulb", new Color(1.0f, 0.85f, 0.55f));
        _windowMat = MakeMat(unlit, "NI_WindowWarm", new Color(0.95f, 0.78f, 0.50f));
        _wireMat = MakeMat(unlit, "NI_StringWire", new Color(0.10f, 0.10f, 0.12f));
        _darkPanelMat = MakeMat(lit, "NI_SignPanelDark", new Color(0.08f, 0.09f, 0.12f));
        _darkMetalMat = MakeMat(lit, "NI_NeonDarkMetal", new Color(0.16f, 0.18f, 0.20f));
    }

    private Material MakeMat(Shader shader, string name, Color color)
    {
        var m = new Material(shader) { name = name, color = color };
        if (m.HasProperty("_BaseColor")) m.SetColor("_BaseColor", color);
        if (m.HasProperty("_Color")) m.SetColor("_Color", color);
        _materials.Add(m);
        return m;
    }
}
