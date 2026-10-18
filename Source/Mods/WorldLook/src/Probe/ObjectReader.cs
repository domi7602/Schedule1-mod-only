extern alias il2cpp;

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace GraphicsProbe;

internal static class ObjectReader
{
    public static string NativeType(il2cpp::Il2CppSystem.Object value) => ReportPolicy.Text(value.GetIl2CppType().FullName);
    public static object Vec(Vector3 v) => new { x = v.x, y = v.y, z = v.z };
    public static object ColorValue(Color c) => new { r = c.r, g = c.g, b = c.b, a = c.a };
    public static object BoundsValue(Bounds b) => new { center = Vec(b.center), size = Vec(b.size) };

    public static string HierarchyPath(Transform transform)
    {
        var parts = new List<string>();
        int depth = 0;
        while (transform != null && depth++ < ReportPolicy.MaxHierarchyDepth)
        {
            parts.Add(ReportPolicy.Text(transform.name) + "[" + transform.GetSiblingIndex() + "]");
            transform = transform.parent;
        }
        parts.Reverse();
        return ReportPolicy.Text((transform != null ? "…/" : "") + string.Join("/", parts), ReportPolicy.MaxPath);
    }

    public static ObservedRecord Base(Component component)
    {
        var row = new ObservedRecord();
        row.Read("nativeType", () => NativeType(component));
        row.Read("name", () => ReportPolicy.Text(component.name));
        row.Read("hierarchyPath", () => HierarchyPath(component.transform));
        row.Read("scene", () => new { name = ReportPolicy.Text(component.gameObject.scene.name), handle = component.gameObject.scene.handle });
        row.Read("activeSelf", () => component.gameObject.activeSelf);
        row.Read("activeInHierarchy", () => component.gameObject.activeInHierarchy);
        row.Read("layer", () => component.gameObject.layer);
        return row;
    }

    public static object ComponentNames(GameObject go)
    {
        var all = go.GetComponents<Component>();
        var kept = new List<string>();
        int scanned = Math.Min(all.Length, ReportPolicy.MaxComponents);
        for (int i = 0; i < scanned; i++)
        {
            try { kept.Add(all[i] == null ? "<missing>" : NativeType(all[i])); }
            catch (Exception ex) { kept.Add("<unreadable:" + ReportPolicy.Text(ex.GetType().Name) + ">"); }
        }
        return new { totalObserved = all.Length, scanned, kept = kept.Count, truncated = all.Length > scanned, types = kept };
    }

    public static ObservedRecord CameraDetails(Camera camera, string discoverySource)
    {
        var row = Base(camera);
        row["discoverySource"] = discoverySource;
        row.Read("instanceId", () => camera.GetInstanceID());
        row.Read("enabled", () => camera.enabled);
        row.Read("depth", () => camera.depth);
        row.Read("cameraType", () => camera.cameraType.ToString());
        row.Read("cullingMask", () => camera.cullingMask);
        row.Read("clearMode", () => camera.clearFlags.ToString());
        row.Read("backgroundColor", () => ColorValue(camera.backgroundColor));
        row.Read("allowHDR", () => camera.allowHDR);
        row.Read("allowMSAA", () => camera.allowMSAA);
        row.Read("depthTextureModeRequest", () => camera.depthTextureMode.ToString());
        row.Read("nearClipPlane", () => camera.nearClipPlane);
        row.Read("farClipPlane", () => camera.farClipPlane);
        row.Read("fieldOfView", () => camera.fieldOfView);
        row.Read("orthographic", () => camera.orthographic);
        row.Read("orthographicSize", () => camera.orthographicSize);
        row.Read("targetDisplay", () => camera.targetDisplay);
        row.Read("pixelWidth", () => camera.pixelWidth);
        row.Read("pixelHeight", () => camera.pixelHeight);
        row.Read("targetTexture", () => TextureDetails(camera.targetTexture));
        row.Read("components", () => ComponentNames(camera.gameObject));
        row.Read("urpAdditionalCameraData", () => AdditionalCamera(camera.GetComponent<UniversalAdditionalCameraData>()));
        row["interpretation"] = "Observed settings only; does not identify the gameplay/main camera or prove a usable depth buffer.";
        return row;
    }

    private static object? AdditionalCamera(UniversalAdditionalCameraData data)
    {
        if (data == null) return null;
        var row = new ObservedRecord();
        row.Read("nativeType", () => NativeType(data));
        // Serialized backing fields only: no renderer resolution, stack cleanup or defaults creation.
        row.Read("renderType", () => data.m_CameraType.ToString());
        row.Read("rendererIndex", () => data.m_RendererIndex);
        row.Read("requiresDepthOption", () => data.m_RequiresDepthTextureOption.ToString());
        row.Read("requiresOpaqueOption", () => data.m_RequiresOpaqueTextureOption.ToString());
        row.Read("runtimeDepthRequestField", () => data.m_RequiresDepthTexture);
        row.Read("runtimeColorRequestField", () => data.m_RequiresColorTexture);
        row.Read("clearDepth", () => data.m_ClearDepth);
        row.Read("renderShadows", () => data.m_RenderShadows);
        row.Read("renderPostProcessing", () => data.m_RenderPostProcessing);
        row.Read("antialiasing", () => data.m_Antialiasing.ToString());
        row.Read("volumeLayerMask", () => data.m_VolumeLayerMask.value);
        row.Read("stackCountField", () => data.m_Cameras == null ? 0 : data.m_Cameras.Count);
        row["rendererAndPassResolution"] = "unresolved: intentionally not invoking lazy renderer/stack accessors";
        return row;
    }

    public static object? TextureDetails(RenderTexture texture)
    {
        if (texture == null) return null;
        var row = new ObservedRecord();
        row.Read("name", () => ReportPolicy.Text(texture.name));
        row.Read("nativeType", () => NativeType(texture));
        row.Read("width", () => texture.width);
        row.Read("height", () => texture.height);
        row.Read("depthBits", () => texture.depth);
        row.Read("antiAliasing", () => texture.antiAliasing);
        row.Read("format", () => texture.format.ToString());
        row.Read("graphicsFormat", () => texture.graphicsFormat.ToString());
        row.Read("dimension", () => texture.dimension.ToString());
        row.Read("created", () => texture.IsCreated());
        return row;
    }

    public static object? MaterialDetails(Material material, bool includeProperties = false)
    {
        if (material == null) return null;
        var row = new ObservedRecord();
        row.Read("name", () => ReportPolicy.Text(material.name));
        row.Read("shader", () => material.shader == null ? null : ReportPolicy.Text(material.shader.name));
        row.Read("renderQueue", () => material.renderQueue);
        if (includeProperties) row.Read("shaderProperties", () => ShaderProperties(material));
        return row;
    }

    private static object? ShaderProperties(Material material)
    {
        var shader = material.shader;
        if (shader == null) return null;
        int total = shader.GetPropertyCount();
        var planned = IntrospectionPolicy.PlanIndices(total, IntrospectionPolicy.MaxShaderProperties);
        var items = new List<ObservedRecord>();
        foreach (int index in planned)
        {
            int i = index;
            var entry = new ObservedRecord();
            string name = "";
            string shape = "unknown";
            entry.Read("name", () => name = ReportPolicy.Text(shader.GetPropertyName(i)));
            entry.Read("kind", () => shape = IntrospectionPolicy.ShapeOf(shader.GetPropertyType(i).ToString()));
            switch (shape)
            {
                case "color":
                    entry.Read("value", () =>
                    {
                        Color c = material.GetColor(name);
                        var n = IntrospectionPolicy.NormalizeComponents(new[] { (double)c.r, c.g, c.b, c.a });
                        return n == null ? null : new { r = n[0], g = n[1], b = n[2], a = n[3] };
                    });
                    break;
                case "vector":
                    entry.Read("value", () =>
                    {
                        Vector4 v = material.GetVector(name);
                        var n = IntrospectionPolicy.NormalizeComponents(new[] { (double)v.x, v.y, v.z, v.w });
                        return n == null ? null : new { x = n[0], y = n[1], z = n[2], w = n[3] };
                    });
                    break;
                case "float":
                    entry.Read("value", () => IntrospectionPolicy.Normalize(material.GetFloat(name)));
                    break;
                case "texture":
                    entry.Read("value", () =>
                    {
                        Texture t = material.GetTexture(name);
                        return t == null ? null : new { name = ReportPolicy.Text(t.name), dimension = t.dimension.ToString() };
                    });
                    break;
                default:
                    entry["value"] = null;
                    entry["valueNote"] = "value not read: unknown property kind";
                    break;
            }
            items.Add(entry);
        }
        return new { totalObserved = total, scanned = planned.Count, kept = items.Count, truncated = total > planned.Count, items };
    }

    internal sealed class RendererClues
    {
        public List<string> Evidence { get; } = new();
        public ObservedRecord Materials { get; } = new();
        public bool Unreadable { get; set; }
    }

    public static RendererClues ScanRenderer(Renderer renderer)
    {
        var result = new RendererClues();
        result.Materials.Read("rendererName", () => ReportPolicy.Text(renderer.name));
        string name = result.Materials.TryGetValue("rendererName", out var n) ? n as string ?? "" : "";
        result.Evidence.AddRange(WaterHeuristics.Evidence(name, null, null));
        result.Materials.Read("slots", () =>
        {
            var materials = renderer.sharedMaterials;
            int scanned = Math.Min(materials.Length, ReportPolicy.MaxMaterials);
            if (scanned < materials.Length) result.Unreadable = true;
            var slots = new List<ObservedRecord>();
            for (int i = 0; i < scanned; i++)
            {
                int index = i;
                var slot = new ObservedRecord { ["slot"] = index };
                slot.Read("material", () => MaterialDetails(materials[index]));
                if (slot["material"] is ObservedRecord description)
                {
                    string? mat = description.TryGetValue("name", out var m) ? m as string : null;
                    string? shader = description.TryGetValue("shader", out var s) ? s as string : null;
                    foreach (string clue in WaterHeuristics.Evidence(null, mat, shader))
                        result.Evidence.Add("slot:" + index + "/" + clue);
                    if (description.Errors.Count > 0) result.Unreadable = true;
                }
                if (slot.Errors.Count > 0) result.Unreadable = true;
                slots.Add(slot);
            }
            return new { totalObserved = materials.Length, scanned, kept = slots.Count, truncated = materials.Length > scanned, items = slots };
        });
        if (result.Materials.Errors.Count > 0) result.Unreadable = true;
        return result;
    }

    public static ObservedRecord WaterCandidate(Renderer renderer, RendererClues clues, bool effect)
    {
        var row = Base(renderer);
        row["classification"] = effect ? "unconfirmed particle/line/trail lexical match, not a surface-water candidate"
            : "unconfirmed non-particle lexical candidate; renderer category is not proof of water";
        row["evidence"] = clues.Evidence;
        row["sharedMaterialSlots"] = clues.Materials;
        row.Read("enabled", () => renderer.enabled);
        row.Read("bounds", () => BoundsValue(renderer.bounds));
        row.Read("components", () => ComponentNames(renderer.gameObject));
        row.Read("parentComponents", () => renderer.transform.parent == null ? null : ComponentNames(renderer.transform.parent.gameObject));
        row.Read("mesh", () => MeshDetails(renderer));
        return row;
    }

    private static object? MeshDetails(Renderer renderer)
    {
        Mesh mesh;
        var skinned = renderer.TryCast<SkinnedMeshRenderer>();
        if (skinned != null) mesh = skinned.sharedMesh;
        else
        {
            var filter = renderer.GetComponent<MeshFilter>();
            if (filter == null) return null;
            mesh = filter.sharedMesh;
        }
        if (mesh == null) return null;
        var row = new ObservedRecord();
        row.Read("name", () => ReportPolicy.Text(mesh.name));
        row.Read("vertexCount", () => mesh.vertexCount);
        row.Read("subMeshCount", () => mesh.subMeshCount);
        row.Read("bounds", () => BoundsValue(mesh.bounds));
        row["geometry"] = "not read: no vertices, indices, texture pixels or extracted assets";
        return row;
    }

    public static ObservedRecord LightDetails(Light light)
    {
        var row = Base(light);
        row.Read("enabled", () => light.enabled);
        row.Read("type", () => light.type.ToString());
        row.Read("color", () => ColorValue(light.color));
        row.Read("intensity", () => light.intensity);
        row.Read("range", () => light.range);
        row.Read("spotAngle", () => light.spotAngle);
        row.Read("shadows", () => light.shadows.ToString());
        row.Read("shadowStrength", () => IntrospectionPolicy.Normalize(light.shadowStrength));
        row.Read("cullingMask", () => light.cullingMask);
        row["interpretation"] = "Observed light state at capture time; the game may rewrite these values over its day/night cycle.";
        return row;
    }

    public static ObservedRecord ProbeDetails(Component probe)
    {
        var row = Base(probe);
        var reflection = probe.TryCast<ReflectionProbe>();
        if (reflection != null)
        {
            row.Read("enabled", () => reflection.enabled);
            row.Read("mode", () => reflection.mode.ToString());
            row.Read("refreshMode", () => reflection.refreshMode.ToString());
            row.Read("bounds", () => BoundsValue(reflection.bounds));
            row.Read("resolution", () => reflection.resolution);
            row.Read("intensity", () => reflection.intensity);
            row.Read("boxProjection", () => reflection.boxProjection);
        }
        else row["probePositions"] = "not read: descriptor only, no bulk position allocation";
        return row;
    }

    public static ObservedRecord VolumeDetails(Volume volume)
    {
        var row = Base(volume);
        row.Read("enabled", () => volume.enabled);
        row.Read("isGlobal", () => volume.m_IsGlobal);
        row.Read("priority", () => volume.priority);
        row.Read("weight", () => volume.weight);
        row.Read("blendDistance", () => volume.blendDistance);
        row.Read("sharedProfile", () =>
        {
            var profile = volume.sharedProfile;
            if (profile == null) return null;
            var p = new ObservedRecord();
            p.Read("name", () => ReportPolicy.Text(profile.name));
            p.Read("components", () =>
            {
                var all = profile.components;
                int count = all == null ? 0 : all.Count;
                int scanned = Math.Min(count, ReportPolicy.MaxComponents);
                var components = new List<ObservedRecord>();
                for (int i = 0; i < scanned; i++)
                {
                    int index = i;
                    var component = new ObservedRecord();
                    component.Read("nativeType", () => all![index] == null ? "<missing>" : NativeType(all![index]));
                    component.Read("active", () => all![index] == null ? null : all![index].active);
                    components.Add(component);
                }
                return new { totalObserved = count, scanned, kept = components.Count, truncated = count > scanned, items = components };
            });
            return p;
        });
        row["interpretation"] = "Shared profile descriptors only; runtime blended/instanced values and final post-processing are unresolved.";
        return row;
    }
}
