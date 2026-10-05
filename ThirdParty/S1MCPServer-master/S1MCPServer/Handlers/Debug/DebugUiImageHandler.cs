using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using S1MCPServer.Core;
using S1MCPServer.Models;
using S1MCPServer.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace S1MCPServer.Handlers.Debug;

/// <summary>
/// UI-focused debug tools for uGUI theming / dark-mode triage.
///
///  - inspect_ui_image: resolves GameObjects by name or full path (inactive
///    objects included, bounded scene traversal - never
///    Resources.FindObjectsOfTypeAll) and dumps everything that decides how an
///    Image RENDERS: colour vs canvas-renderer colour (state multiplies),
///    plain sprite vs override sprite vs the ACTIVE sprite, sprite
///    rect/pivot/border/packing and texture info, plus the nearest Selectable
///    and its ColorBlock.
///
///  - read_sprite_pixels: reads actual sprite texels through a RenderTexture
///    copy (works for non-readable/atlas textures) and returns RGBA samples
///    with max channel. This settles "is the art dark or is it a tint"
///    without screenshots.
/// </summary>
public class DebugUiImageHandler : DebugHandlerBase
{
    public DebugUiImageHandler(ResponseQueue responseQueue) : base(responseQueue)
    {
    }

    public override void Handle(Request request)
    {
        switch (request.Method)
        {
            case "inspect_ui_image":
                HandleInspectUiImage(request);
                break;
            case "read_sprite_pixels":
                HandleReadSpritePixels(request);
                break;
            default:
                _responseQueue.EnqueueResponse(ProtocolHandler.CreateErrorResponse(
                    request.Id, -32601, $"Unknown UI method: {request.Method}"));
                break;
        }
    }

    // ------------------------------------------------------------------
    // inspect_ui_image
    // ------------------------------------------------------------------

    private void HandleInspectUiImage(Request request)
    {
        try
        {
            string query = GetStringParam(request.Params, "object_name") ?? GetStringParam(request.Params, "path_pattern") ?? "";
            if (string.IsNullOrEmpty(query))
            {
                _responseQueue.EnqueueResponse(ProtocolHandler.CreateErrorResponse(
                    request.Id, -32602, "object_name (name or path substring) is required"));
                return;
            }

            int maxResults = GetIntParam(request.Params, "max_results") ?? 10;
            int maxDepth = GetIntParam(request.Params, "max_depth") ?? 20;
            bool includeChildren = GetBoolParam(request.Params, "include_children") ?? true;

            var targets = FindTargets(query, maxDepth, includeChildren ? maxResults : 1);
            if (targets.Count == 0)
            {
                _responseQueue.EnqueueResponse(ProtocolHandler.CreateErrorResponse(
                    request.Id, -32000, "GameObject not found",
                    new { object_name = query, hint = "name and full path (inactive included) are matched as case-insensitive substrings" }));
                return;
            }

            var entries = new List<Dictionary<string, object>>();
            foreach (var (go, path) in targets)
            {
                var images = go.GetComponents<Image>();
                if (images == null || images.Length == 0) continue;
                var imageEntries = new List<Dictionary<string, object>>();
                for (int i = 0; i < images.Length; i++)
                {
                    Image img = images[i];
                    if (img == null) continue;
                    imageEntries.Add(SerializeImage(img));
                }
                if (imageEntries.Count == 0) continue;
                entries.Add(new Dictionary<string, object>
                {
                    ["path"] = path,
                    ["name"] = go.name,
                    ["active"] = go.activeSelf,
                    ["active_in_hierarchy"] = go.activeInHierarchy,
                    ["images"] = imageEntries
                });
            }

            var result = new Dictionary<string, object>
            {
                ["query"] = query,
                ["matches"] = entries,
                ["count"] = entries.Count
            };
            _responseQueue.EnqueueResponse(ProtocolHandler.CreateSuccessResponse(request.Id, result));
        }
        catch (Exception ex)
        {
            ModLogger.Error($"Error in HandleInspectUiImage: {ex.Message}");
            _responseQueue.EnqueueResponse(ProtocolHandler.CreateErrorResponse(
                request.Id, -32000, "Failed to inspect UI image", new { details = ex.Message }));
        }
    }

    private Dictionary<string, object> SerializeImage(Image img)
    {
        var d = new Dictionary<string, object>();

        // Plain vs override vs ACTIVE sprite. Unity semantics (important):
        // Image.sprite returns the plain slot (m_Sprite, what the game writes),
        // Image.overrideSprite returns the ACTIVE sprite and falls back to the
        // plain one when no override is set. The m_* members show the truth.
        d["field_m_Sprite"] = NameOf(GetMember(img, "m_Sprite"));
        d["field_m_OverrideSprite"] = NameOf(GetMember(img, "m_OverrideSprite"));
        try { d["prop_sprite"] = NameOf(img.sprite); } catch { d["prop_sprite"] = "?"; }
        try { d["prop_overrideSprite_active"] = NameOf(img.overrideSprite); } catch { d["prop_overrideSprite_active"] = "?"; }

        try { d["type"] = img.type.ToString(); } catch { d["type"] = "?"; }
        try { d["fill_amount"] = Math.Round((double)img.fillAmount, 4); } catch { }
        try { d["fill_method"] = img.fillMethod.ToString(); } catch { }
        try { d["fill_origin"] = img.fillOrigin; } catch { }
        try { d["preserve_aspect"] = img.preserveAspect; } catch { }
        try { d["fill_center"] = img.fillCenter; } catch { }
        try { d["use_sprite_mesh"] = img.useSpriteMesh; } catch { }
        try { d["raycast_target"] = img.raycastTarget; } catch { }
        try { d["pixels_per_unit_multiplier"] = img.pixelsPerUnitMultiplier; } catch { }

        try { d["color"] = ColorDict(img.color); } catch { }
        try
        {
            var cr = img.canvasRenderer;
            d["canvas_renderer"] = new Dictionary<string, object>
            {
                ["color"] = ColorDict(cr.GetColor()),
                ["alpha"] = Math.Round((double)cr.GetAlpha(), 4),
                ["cull"] = cr.cull
            };
        }
        catch { }

        Sprite active = null;
        try { active = img.overrideSprite; } catch { }
        try { d["sprite_active"] = SpriteInfo(active); } catch { }

        // Rect of the image itself - fill/handle geometry lives here.
        try
        {
            var rt = img.rectTransform;
            var r = rt.rect;
            d["rect"] = new Dictionary<string, object> { ["x"] = r.x, ["y"] = r.y, ["w"] = r.width, ["h"] = r.height };
        }
        catch { }

        d["selectable"] = DescribeSelectable(img);
        return d;
    }

    private Dictionary<string, object> SpriteInfo(Sprite sp)
    {
        if (sp == null) return new Dictionary<string, object> { ["name"] = "null" };
        var d = new Dictionary<string, object> { ["name"] = sp.name };
        try { d["rect"] = RectDict(sp.rect); } catch { }
        try { d["texture_rect"] = RectDict(sp.textureRect); } catch { d["texture_rect"] = "throws (tight mesh?)"; }
        try { var off = sp.textureRectOffset; d["texture_rect_offset"] = new Dictionary<string, object> { ["x"] = off.x, ["y"] = off.y }; } catch { }
        try { var p = sp.pivot; d["pivot"] = new Dictionary<string, object> { ["x"] = p.x, ["y"] = p.y }; } catch { }
        try { var b = sp.border; d["border"] = new Dictionary<string, object> { ["x"] = b.x, ["y"] = b.y, ["z"] = b.z, ["w"] = b.w }; } catch { }
        try { d["pixels_per_unit"] = sp.pixelsPerUnit; } catch { }
        try { d["packed"] = sp.packed; } catch { }
        try { d["packing_rotation"] = sp.packingRotation.ToString(); } catch { }
        try
        {
            var tex = sp.texture;
            if (tex != null)
            {
                d["texture"] = new Dictionary<string, object>
                {
                    ["name"] = tex.name,
                    ["width"] = tex.width,
                    ["height"] = tex.height,
                    ["format"] = tex.format.ToString(),
                    ["is_readable"] = tex.isReadable,
                    ["filter_mode"] = tex.filterMode.ToString(),
                    ["wrap_mode"] = tex.wrapMode.ToString()
                };
            }
        }
        catch { }
        return d;
    }

    private Dictionary<string, object> DescribeSelectable(Image img)
    {
        try
        {
            Transform t = img.transform;
            for (int i = 0; i < 3 && t != null; i++)
            {
                Selectable sel = t.GetComponent<Selectable>();
                if (sel != null)
                {
                    bool target = false;
                    try { target = sel.targetGraphic != null && sel.targetGraphic.Pointer == img.Pointer; } catch { }
                    Color n = sel.colors.normalColor;
                    return new Dictionary<string, object>
                    {
                        ["type"] = sel.GetType().Name,
                        ["transition"] = sel.transition.ToString(),
                        ["normal_color"] = ColorDict(n),
                        ["target_is_this_image"] = target
                    };
                }
                t = t.parent;
            }
        }
        catch { }
        return new Dictionary<string, object> { ["type"] = "none" };
    }

    // ------------------------------------------------------------------
    // read_sprite_pixels
    // ------------------------------------------------------------------

    private void HandleReadSpritePixels(Request request)
    {
        Texture2D copy = null;
        try
        {
            string query = GetStringParam(request.Params, "object_name") ?? "";
            if (string.IsNullOrEmpty(query))
            {
                _responseQueue.EnqueueResponse(ProtocolHandler.CreateErrorResponse(
                    request.Id, -32602, "object_name is required"));
                return;
            }

            int maxDepth = GetIntParam(request.Params, "max_depth") ?? 20;
            string which = GetStringParam(request.Params, "which") ?? "active"; // active | plain | override
            var targets = FindTargets(query, maxDepth, 1);
            if (targets.Count == 0)
            {
                _responseQueue.EnqueueResponse(ProtocolHandler.CreateErrorResponse(
                    request.Id, -32000, "GameObject not found", new { object_name = query }));
                return;
            }

            var (go, path) = targets[0];
            var img = go.GetComponent<Image>();
            if (img == null)
            {
                _responseQueue.EnqueueResponse(ProtocolHandler.CreateErrorResponse(
                    request.Id, -32000, "No Image component on GameObject", new { path }));
                return;
            }

            Sprite sp = null;
            try
            {
                if (which == "plain") sp = img.sprite;
                else if (which == "override") sp = GetMember(img, "m_OverrideSprite") as Sprite;
                else sp = img.overrideSprite; // active sprite
            }
            catch { }
            if (sp == null)
            {
                _responseQueue.EnqueueResponse(ProtocolHandler.CreateErrorResponse(
                    request.Id, -32000, "No sprite in the requested slot", new { path, which }));
                return;
            }

            var tex = sp.texture;
            int w = tex.width;
            int h = tex.height;
            if (w <= 0 || h <= 0 || w > 4096 || h > 4096)
            {
                _responseQueue.EnqueueResponse(ProtocolHandler.CreateErrorResponse(
                    request.Id, -32000, "Texture size outside 1..4096 - refusing to sample",
                    new { width = w, height = h }));
                return;
            }

            Rect r;
            try { r = sp.textureRect; }
            catch (Exception ex)
            {
                _responseQueue.EnqueueResponse(ProtocolHandler.CreateErrorResponse(
                    request.Id, -32000, "textureRect not available", new { details = ex.Message }));
                return;
            }

            // Texel readback via RenderTexture copy - works for non-readable textures.
            copy = new Texture2D(w, h, TextureFormat.RGBA32, false);
            var rt = RenderTexture.GetTemporary(w, h, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
            var prev = RenderTexture.active;
            try
            {
                Graphics.Blit(tex, rt);
                RenderTexture.active = rt;
                copy.ReadPixels(new Rect(0f, 0f, w, h), 0, 0);
                copy.Apply();
            }
            finally
            {
                RenderTexture.active = prev;
                RenderTexture.ReleaseTemporary(rt);
            }

            var samples = ParseSamples(request.Params);
            var sampleEntries = new List<Dictionary<string, object>>();
            foreach (var (u, v) in samples)
            {
                int x = Mathf.Clamp(Mathf.RoundToInt(r.x + (u * (r.width - 1f))), 0, w - 1);
                int y = Mathf.Clamp(Mathf.RoundToInt(r.y + (v * (r.height - 1f))), 0, h - 1);
                Color c = copy.GetPixel(x, y);
                float maxChannel = Mathf.Max(c.r, Mathf.Max(c.g, c.b));
                sampleEntries.Add(new Dictionary<string, object>
                {
                    ["u"] = u,
                    ["v"] = v,
                    ["x"] = x,
                    ["y"] = y,
                    ["rgba_hex"] = Hex(c),
                    ["rgba"] = ColorDict(c),
                    ["max_channel"] = Math.Round((double)maxChannel, 4)
                });
            }

            var result = new Dictionary<string, object>
            {
                ["path"] = path,
                ["which"] = which,
                ["sprite"] = sp.name,
                ["texture"] = new Dictionary<string, object> { ["name"] = tex.name, ["width"] = w, ["height"] = h },
                ["texture_rect"] = RectDict(r),
                ["samples"] = sampleEntries
            };
            _responseQueue.EnqueueResponse(ProtocolHandler.CreateSuccessResponse(request.Id, result));
        }
        catch (Exception ex)
        {
            ModLogger.Error($"Error in HandleReadSpritePixels: {ex.Message}");
            _responseQueue.EnqueueResponse(ProtocolHandler.CreateErrorResponse(
                request.Id, -32000, "Failed to sample sprite pixels", new { details = ex.Message }));
        }
        finally
        {
            try { if (copy != null) UnityEngine.Object.Destroy(copy); } catch { }
        }
    }

    /// <summary>Normalized (u, v) sample points inside the sprite's textureRect; default: 7 points across the middle.</summary>
    private List<(float u, float v)> ParseSamples(Dictionary<string, object> parameters)
    {
        var samples = new List<(float, float)>
        {
            (0.5f, 0.5f), (0.1f, 0.5f), (0.25f, 0.5f), (0.4f, 0.5f), (0.6f, 0.5f), (0.75f, 0.5f), (0.9f, 0.5f)
        };
        if (parameters == null || !parameters.TryGetValue("samples", out var samplesObj) || samplesObj == null)
            return samples;

        try
        {
            var parsed = new List<(float, float)>();
            if (samplesObj is JsonElement element && element.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in element.EnumerateArray())
                {
                    if (item.ValueKind != JsonValueKind.Array) continue;
                    var nums = item.EnumerateArray();
                    if (!nums.MoveNext()) continue;
                    float u = nums.Current.GetSingle();
                    if (!nums.MoveNext()) continue;
                    float v = nums.Current.GetSingle();
                    parsed.Add((Mathf.Clamp01(u), Mathf.Clamp01(v)));
                }
            }
            if (parsed.Count > 0) return parsed;
        }
        catch { }
        return samples;
    }

    // ------------------------------------------------------------------
    // Target resolution and shared helpers
    // ------------------------------------------------------------------

    /// <summary>
    /// Bounded scene traversal (inactive included) matching name or full path
    /// as case-insensitive substrings. Never touches
    /// Resources.FindObjectsOfTypeAll (IL2CPP freeze risk).
    /// </summary>
    private List<(GameObject go, string path)> FindTargets(string query, int maxDepth, int maxResults)
    {
        var matches = new List<(int score, int pathLen, GameObject go, string path)>();
        try
        {
            var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            if (scene.IsValid() && scene.isLoaded)
            {
                var roots = scene.GetRootGameObjects();
                for (int i = 0; i < roots.Length; i++)
                {
                    if (roots[i] == null) continue;
                    CollectTargets(roots[i].transform, "", 0, maxDepth, query, matches, maxResults * 4);
                }
            }
        }
        catch (Exception ex) { ModLogger.Debug($"FindTargets: {ex.Message}"); }

        matches.Sort((a, b) =>
        {
            int byScore = b.score.CompareTo(a.score);
            return byScore != 0 ? byScore : a.pathLen.CompareTo(b.pathLen);
        });

        var result = new List<(GameObject, string)>();
        for (int i = 0; i < matches.Count && result.Count < maxResults; i++)
            result.Add((matches[i].go, matches[i].path));
        return result;
    }

    private void CollectTargets(Transform t, string parentPath, int depth, int maxDepth, string query,
        List<(int score, int pathLen, GameObject go, string path)> matches, int scanCap)
    {
        if (t == null || depth > maxDepth || matches.Count >= scanCap) return;
        string path = parentPath.Length == 0 ? t.name : parentPath + "/" + t.name;

        int score = 0;
        if (string.Equals(t.name, query, StringComparison.OrdinalIgnoreCase)) score = 3;
        else if (t.name.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0) score = 2;
        else if (path.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0) score = 1;
        if (score > 0) matches.Add((score, path.Length, t.gameObject, path));

        for (int i = 0; i < t.childCount; i++)
        {
            if (matches.Count >= scanCap) return;
            CollectTargets(t.GetChild(i), path, depth + 1, maxDepth, query, matches, scanCap);
        }
    }

    private static object GetMember(object obj, string name)
    {
        try
        {
            Type t = obj.GetType();
            var prop = t.GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (prop != null) return prop.GetValue(obj);
            var field = t.GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null) return field.GetValue(obj);
        }
        catch { }
        return null;
    }

    private static string NameOf(object obj)
    {
        try
        {
            var unityObj = obj as UnityEngine.Object;
            return unityObj == null ? "null" : unityObj.name;
        }
        catch { return "?"; }
    }

    private static string Hex(Color c)
    {
        int r = Mathf.Clamp(Mathf.RoundToInt(c.r * 255f), 0, 255);
        int g = Mathf.Clamp(Mathf.RoundToInt(c.g * 255f), 0, 255);
        int b = Mathf.Clamp(Mathf.RoundToInt(c.b * 255f), 0, 255);
        int a = Mathf.Clamp(Mathf.RoundToInt(c.a * 255f), 0, 255);
        return $"#{r:X2}{g:X2}{b:X2}{a:X2}";
    }

    private static Dictionary<string, object> ColorDict(Color c)
    {
        return new Dictionary<string, object>
        {
            ["r"] = Math.Round((double)c.r, 4),
            ["g"] = Math.Round((double)c.g, 4),
            ["b"] = Math.Round((double)c.b, 4),
            ["a"] = Math.Round((double)c.a, 4)
        };
    }

    private static Dictionary<string, object> RectDict(Rect r)
    {
        return new Dictionary<string, object> { ["x"] = r.x, ["y"] = r.y, ["w"] = r.width, ["h"] = r.height };
    }

    private string GetStringParam(Dictionary<string, object> parameters, string key)
    {
        if (parameters != null && parameters.TryGetValue(key, out var value) && value != null)
        {
            if (value is JsonElement element && element.ValueKind == JsonValueKind.String) return element.GetString();
            return value.ToString();
        }
        return null;
    }

    private int? GetIntParam(Dictionary<string, object> parameters, string key)
    {
        if (parameters != null && parameters.TryGetValue(key, out var value) && value != null)
        {
            if (value is JsonElement element && element.ValueKind == JsonValueKind.Number) return element.GetInt32();
            int parsed;
            if (int.TryParse(value.ToString(), out parsed)) return parsed;
        }
        return null;
    }

    private bool? GetBoolParam(Dictionary<string, object> parameters, string key)
    {
        if (parameters != null && parameters.TryGetValue(key, out var value) && value != null)
        {
            if (value is JsonElement element && element.ValueKind == JsonValueKind.True) return true;
            if (value is JsonElement element2 && element2.ValueKind == JsonValueKind.False) return false;
            bool parsed;
            if (bool.TryParse(value.ToString(), out parsed)) return parsed;
        }
        return null;
    }
}
