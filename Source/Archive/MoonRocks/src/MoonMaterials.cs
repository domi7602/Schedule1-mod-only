using System.Collections.Generic;
using UnityEngine;

namespace MoonRocks;

/// <summary>
/// Owns all URP-compatible runtime materials created by MoonRocks and destroys them cleanly on teardown.
/// </summary>
internal sealed class MoonMaterials
{
    private readonly List<Material> _owned = new();

    internal Material MoonGround { get; private set; } = null!;
    internal Material ConcretePad { get; private set; } = null!;
    internal Material RocketBody { get; private set; } = null!;
    internal Material RocketAccent { get; private set; } = null!;
    internal Material MetalDark { get; private set; } = null!;
    internal Material GoldFoil { get; private set; } = null!;
    internal Material GlassCyan { get; private set; } = null!;
    internal Material BeaconGlow { get; private set; } = null!;
    internal Material FlameGlow { get; private set; } = null!;
    internal Material SpaceDome { get; private set; } = null!;
    internal Material StarGlow { get; private set; } = null!;
    internal Material EarthOcean { get; private set; } = null!;
    internal Material EarthLand { get; private set; } = null!;

    internal void Initialize()
    {
        if (_owned.Count > 0) return;

        MoonGround = CreateLit("MoonRocks_Ground", new Color(0.56f, 0.57f, 0.60f), 0.05f, 0.12f);
        ConcretePad = CreateLit("MoonRocks_Concrete", new Color(0.32f, 0.34f, 0.37f), 0.1f, 0.2f);
        RocketBody = CreateLit("MoonRocks_RocketBody", new Color(0.92f, 0.93f, 0.96f), 0.25f, 0.55f);
        RocketAccent = CreateLit("MoonRocks_RocketAccent", new Color(0.86f, 0.22f, 0.14f), 0.2f, 0.45f);
        MetalDark = CreateLit("MoonRocks_MetalDark", new Color(0.18f, 0.20f, 0.24f), 0.7f, 0.5f);
        GoldFoil = CreateLit("MoonRocks_GoldFoil", new Color(0.88f, 0.68f, 0.18f), 0.85f, 0.7f);
        GlassCyan = CreateLit("MoonRocks_GlassCyan", new Color(0.28f, 0.75f, 0.95f), 0.1f, 0.9f);
        EarthOcean = CreateLit("MoonRocks_EarthOcean", new Color(0.12f, 0.38f, 0.82f), 0.1f, 0.5f);
        EarthLand = CreateLit("MoonRocks_EarthLand", new Color(0.24f, 0.58f, 0.30f), 0.05f, 0.2f);

        BeaconGlow = CreateUnlit("MoonRocks_BeaconGlow", new Color(0.20f, 0.95f, 1.0f));
        FlameGlow = CreateUnlit("MoonRocks_FlameGlow", new Color(1.0f, 0.58f, 0.14f));
        SpaceDome = CreateUnlit("MoonRocks_SpaceDome", new Color(0.01f, 0.012f, 0.025f));
        StarGlow = CreateUnlit("MoonRocks_StarGlow", new Color(0.95f, 0.97f, 1.0f));
    }

    private Material CreateLit(string name, Color color, float metallic, float smoothness)
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit")
            ?? Shader.Find("Standard")
            ?? Shader.Find("Diffuse")
            ?? Shader.Find("Unlit/Color");

        var mat = new Material(shader) { name = name };
        if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", color);
        if (mat.HasProperty("_Color")) mat.SetColor("_Color", color);
        if (mat.HasProperty("_Metallic")) mat.SetFloat("_Metallic", metallic);
        if (mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness", smoothness);
        _owned.Add(mat);
        return mat;
    }

    private Material CreateUnlit(string name, Color color)
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Unlit")
            ?? Shader.Find("Unlit/Color")
            ?? Shader.Find("Sprites/Default");

        var mat = new Material(shader) { name = name };
        if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", color);
        if (mat.HasProperty("_Color")) mat.SetColor("_Color", color);
        _owned.Add(mat);
        return mat;
    }

    internal void DisposeAll()
    {
        for (int i = 0; i < _owned.Count; i++)
        {
            if (_owned[i] != null) Object.Destroy(_owned[i]);
        }
        _owned.Clear();
    }
}
