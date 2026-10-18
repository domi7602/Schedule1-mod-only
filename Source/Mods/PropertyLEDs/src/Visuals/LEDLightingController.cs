using System;
using System.Collections.Generic;
using PropertyLEDs.Core;
using UnityEngine;
using UnityEngine.Rendering;

namespace PropertyLEDs;

/// <summary>One owned mesh/material per room, no colliders, no realtime Lights or global frame scans.</summary>
internal sealed class LEDLightingController
{
    private sealed class RenderedRoom : IDisposable
    {
        internal string PropertyId = "";
        internal string RoomId = "";
        internal GameObject? Root;
        internal Mesh? Mesh;
        internal Material? Material;
        internal MeshRenderer? Renderer;
        internal bool HasEmission;

        public void Dispose()
        {
            if (PropertyRegistry.UnityAlive(Root))
            {
                try { Root!.SetActive(false); } catch { }
            }
            DestroyOwned(Root);
            DestroyOwned(Mesh);
            DestroyOwned(Material);
            Root = null;
            Mesh = null;
            Material = null;
            Renderer = null;
        }
    }

    private readonly List<RenderedRoom> _rooms = new();
    private RenderedRoom? _preview;
    private Shader? _shader;
    private bool _shaderResolved;
    // Combined-colour multiplier matching the accepted reference look (its StripGlow default 2.5).
    private const float GlowMultiplier = 2.5f;

    internal int RoomCount => _rooms.Count;
    internal bool HasPreview => _preview != null;
    internal string ShaderName => PropertyRegistry.UnityAlive(_shader) ? _shader!.name : "unavailable";
    internal int RealtimeLightCount => 0; // deliberately disabled until an opt-in implementation is profiled

    internal void Rebuild(PropertyRegistry properties, RoomLayoutRegistry layouts,
        Func<string, string?, LEDSettings> getSettings)
    {
        Dispose();
        if (!ResolveShader()) return;
        var entries = properties.Entries;
        for (int i = 0; i < entries.Count; i++)
        {
            var property = entries[i];
            if (!PropertyRegistry.UnityAlive(property.Anchor)) continue;
            var rooms = layouts.GetRooms(property.Id);
            for (int j = 0; j < rooms.Count; j++)
            {
                var room = rooms[j];
                if (!room.Verified) continue; // unverified imports are NEVER installed as persistent room LEDs
                var rendered = Create(property, room, false);
                if (rendered == null) continue;
                try
                {
                    ApplySettings(rendered, getSettings(property.Id, room.RoomId));
                    _rooms.Add(rendered);
                }
                catch (Exception ex)
                {
                    rendered.Dispose();
                    Mod.Log.Warn($"Skipped LED room '{property.Id}/{room.RoomId}' during settings apply: {ex.Message}");
                }
            }
        }
        Mod.Log.Info($"Built {_rooms.Count} LED room groups; 0 colliders, 0 realtime lights (emissive visuals only).");
    }

    internal void Apply(string propertyId, string? roomId, Func<string, string?, LEDSettings> getSettings)
    {
        for (int i = 0; i < _rooms.Count; i++)
        {
            var room = _rooms[i];
            if (!StringComparer.Ordinal.Equals(propertyId, room.PropertyId) ||
                (roomId != null && !StringComparer.Ordinal.Equals(roomId, room.RoomId))) continue;
            try { ApplySettings(room, getSettings(room.PropertyId, room.RoomId)); }
            catch (Exception ex) { Mod.Log.Warn($"LED material update failed for '{room.PropertyId}/{room.RoomId}': {ex.Message}"); }
        }
    }

    internal void Preview(PropertyEntry property, RoomLayout layout)
    {
        ClearPreview();
        if (!RoomLayoutRegistry.ValidateGeometry(layout, out string error))
        { Mod.Log.Warn($"Preview rejected: {error}"); return; }
        if (!ResolveShader()) return;
        var rendered = Create(property, RoomLayoutRegistry.Copy(layout), true);
        if (rendered == null) return;
        try
        {
            ApplySettings(rendered, LEDSettings.White());
            _preview = rendered;
            Mod.Log.Info($"Preview '{layout.PropertyId}/{layout.RoomId}': verified={layout.Verified}. Preview changes neither catalog nor settings.");
        }
        catch
        {
            rendered.Dispose();
            throw;
        }
    }

    internal void ClearPreview()
    {
        _preview?.Dispose();
        _preview = null;
    }

    private RenderedRoom? Create(PropertyEntry property, RoomLayout room, bool preview)
    {
        if (!preview && !room.Verified) return null;
        if (!StringComparer.Ordinal.Equals(property.Id, room.PropertyId)) return null;
        var anchor = RoomLayoutRegistry.ResolveAnchor(property, room);
        if (!PropertyRegistry.UnityAlive(anchor))
        {
            Mod.Log.Warn($"Skipped layout '{room.PropertyId}/{room.RoomId}': exact owned-root anchor '{room.AnchorPath}' not found.");
            return null;
        }
        var rendered = new RenderedRoom { PropertyId = room.PropertyId, RoomId = room.RoomId };
        try
        {
            // No primitives: MeshFilter/MeshRenderer are the only added components, never colliders.
            rendered.Mesh = LEDGeometryBuilder.Build(room, preview);
            if (!PropertyRegistry.UnityAlive(rendered.Mesh)) return null;
            rendered.Material = new Material(_shader!);
            rendered.Material.name = $"PropertyLEDs_{room.PropertyId}_{room.RoomId}_Material";
            if (!rendered.Material.HasProperty("_BaseColor"))
                throw new InvalidOperationException("Supported shader lacks URP _BaseColor.");
            rendered.HasEmission = rendered.Material.HasProperty("_EmissionColor");
            if (rendered.HasEmission) rendered.Material.EnableKeyword("_EMISSION");
            if (rendered.Material.HasProperty("_Metallic")) rendered.Material.SetFloat("_Metallic", 0f);
            if (rendered.Material.HasProperty("_Smoothness")) rendered.Material.SetFloat("_Smoothness", 0.15f);

            rendered.Root = new GameObject(preview ? "PropertyLEDs_Preview" : $"PropertyLEDs_{room.RoomId}");
            rendered.Root.SetActive(false);
            rendered.Root.layer = 2; // Unity's built-in Ignore Raycast layer; collider-free regardless
            rendered.Root.transform.SetParent(anchor, false);
            rendered.Root.transform.localPosition = Vector3.zero;
            rendered.Root.transform.localRotation = Quaternion.identity;
            rendered.Root.transform.localScale = Vector3.one;
            var filter = rendered.Root.AddComponent<MeshFilter>();
            filter.sharedMesh = rendered.Mesh;
            rendered.Renderer = rendered.Root.AddComponent<MeshRenderer>();
            rendered.Renderer.enabled = false;
            rendered.Renderer.sharedMaterial = rendered.Material;
            rendered.Renderer.shadowCastingMode = ShadowCastingMode.Off;
            rendered.Renderer.receiveShadows = false;
            rendered.Renderer.lightProbeUsage = LightProbeUsage.Off;
            rendered.Renderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
            return rendered;
        }
        catch (Exception ex)
        {
            rendered.Dispose();
            Mod.Log.Warn($"Skipped LED room '{room.PropertyId}/{room.RoomId}': {ex.Message}");
            return null;
        }
    }

    private static void ApplySettings(RenderedRoom room, LEDSettings source)
    {
        if (!PropertyRegistry.UnityAlive(room.Root) || !PropertyRegistry.UnityAlive(room.Renderer) ||
            !PropertyRegistry.UnityAlive(room.Material)) return;
        var settings = source.Clone();
        settings.Normalize();
        bool enabled = settings.Enabled && settings.Brightness > 0f;
        var rgb = settings.ToRgb();
        var glow = settings.Brightness * GlowMultiplier;
        var tint = new Color(rgb.R, rgb.G, rgb.B, 1f);
        var emission = new Color(rgb.R * glow, rgb.G * glow, rgb.B * glow, 1f);
        room.Material!.SetColor("_BaseColor", room.HasEmission ? tint : emission);
        if (room.HasEmission) room.Material.SetColor("_EmissionColor", emission);
        room.Renderer!.enabled = enabled;
        room.Root!.SetActive(enabled);
    }

    private bool ResolveShader()
    {
        if (_shaderResolved) return PropertyRegistry.UnityAlive(_shader);
        _shaderResolved = true;
        // Built-in Standard is NOT a fallback: it is incompatible with the game's URP pipeline.
        string[] candidates = { "Universal Render Pipeline/Lit", "Universal Render Pipeline/Simple Lit", "Universal Render Pipeline/Unlit" };
        for (int i = 0; i < candidates.Length; i++)
        {
            try
            {
                var shader = Shader.Find(candidates[i]);
                if (!PropertyRegistry.UnityAlive(shader) || !shader.isSupported) continue;
                _shader = shader;
                Mod.Log.Info($"LED shader supported: '{shader.name}'. Realtime lighting remains disabled.");
                return true;
            }
            catch (Exception ex) { Mod.Log.Warn($"LED shader lookup '{candidates[i]}' failed: {ex.Message}"); }
        }
        Mod.Log.Warn("No supported URP LED shader found. Settings remain available; room rendering is disabled.");
        return false;
    }

    internal void Dispose()
    {
        ClearPreview();
        for (int i = 0; i < _rooms.Count; i++) _rooms[i].Dispose();
        _rooms.Clear();
        _shader = null; // shader is a shared game asset; never destroy it
        _shaderResolved = false;
    }

    private static void DestroyOwned(UnityEngine.Object? value)
    {
        if (!PropertyRegistry.UnityAlive(value)) return;
        try { UnityEngine.Object.Destroy(value); }
        catch (Exception ex) { Mod.Log.Warn($"LED resource disposal failed: {ex.Message}"); }
    }
}
