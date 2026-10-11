using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;
using Il2CppScheduleOne.PlayerScripts;
using MelonLoader;
using PropertyLEDs.Core;
using S1Mods.Shared;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;
using NativeProperty = Il2CppScheduleOne.Property.Property;
using NativeBusiness = Il2CppScheduleOne.Property.Business;

namespace PropertyLEDs;

/// <summary>Opt-in, bounded surveys of one runtime-owned property. No scene searches or gameplay save writes.</summary>
internal static class LEDDiagnostics
{
    internal const int MaxDumpNodes = 512;
    internal const int MaxDumpDepth = 12;
    internal const int MaxDumpCharacters = 131072;
    internal const int MaxPoints = 256;
    internal const int MaxPointsPerPath = 128;
    internal const int MaxPaths = 16;
    private const int MaxAnchorDepth = 32;
    private const int MaxAnchorChildren = 4096;
    private const float MaxLocalCoordinate = 200f;
    private const string BridgeName = "PropertyLEDs_Diagnostics";
    private static RoomLayout? _capture;
    private static Transform? _captureAnchor;
    private static IntPtr _propertyPointer;
    private static int _captureSlot = -1;
    private static GameObject? _bridgeRoot;
    private static GameObject? _markerRoot;
    private static Material? _markerMaterial;
    private static int _markerCount;
    private static bool _previewActive;
    private static bool _aimArmed;
    private static float _aimDistance, _aimExpires;

    /// <summary>Called by runtime after load. Exact owned-object reference; never searches for its name.</summary>
    internal static void EnsureBridge()
    {
        RequireReady();
        if (NetworkGuard.IsAlive(_bridgeRoot)) return;
        _bridgeRoot = new GameObject(BridgeName);
        _bridgeRoot.AddComponent<LEDDiagnosticBridge>();
    }

    /// <summary>Runtime calls this before scene changes/save reloads. Idempotent and fail-safe.</summary>
    internal static void Reset()
    {
        CancelAim();
        HidePreview();
        _capture = null;
        _captureAnchor = null;
        _propertyPointer = IntPtr.Zero;
        _captureSlot = -1;
        DestroyOwned(_bridgeRoot);
        _bridgeRoot = null;
    }

    internal static void RequireReady()
    {
        if (!LEDRuntime.Ready || !NetworkGuard.IsInMainScene || SaveSlots.GetActiveSlotNumber() < 0)
            throw new InvalidOperationException("LED runtime / gameplay scene / active save slot is not ready.");
    }

    internal static PropertyEntry Property(string id)
    {
        RequireReady();
        var properties = LEDRuntime.Properties;
        for (int i = 0; i < properties.Count; i++)
            if (string.Equals(properties[i].Id, id, StringComparison.Ordinal))
            {
                if (!NetworkGuard.IsAlive(properties[i].Anchor)) throw new InvalidOperationException("Property anchor is no longer alive.");
                return properties[i];
            }
        throw new ArgumentException("Unknown owned property ID. Use 'led properties'; IDs are exact, case-sensitive.");
    }

    internal static string Properties()
    {
        RequireReady();
        var properties = LEDRuntime.Properties;
        var text = new StringBuilder("Owned property registry (exact IDs):\n");
        int limit = Math.Min(properties.Count, 128);
        for (int i = 0; i < limit; i++)
            text.AppendLine($"{Clean(properties[i].Id)} | {Clean(properties[i].DisplayName)} | {(properties[i].IsBusiness ? "business" : "property")} | anchor alive={NetworkGuard.IsAlive(properties[i].Anchor)}");
        if (limit < properties.Count) text.AppendLine("TRUNCATED: first 128 registry entries only.");
        return text.ToString();
    }

    internal static string Rooms(string id)
    {
        var property = Property(id);
        var rooms = LEDRuntime.GetRooms(id);
        var text = new StringBuilder($"{Clean(property.DisplayName)} rooms (settings also support property default):\n");
        for (int i = 0; i < Math.Min(rooms.Count, 128); i++)
            text.AppendLine($"{Clean(rooms[i].RoomId)} | {Clean(rooms[i].DisplayName)} | verified={rooms[i].Verified} | anchor={Clean(rooms[i].AnchorPath)} | paths={rooms[i].Paths.Count}");
        if (rooms.Count > 128) text.AppendLine("TRUNCATED: first 128 room entries only.");
        return text.ToString();
    }

    /// <summary>
    /// Read-only audit of every native property/business (owned or not) against the layout catalog.
    /// Proves which catalog ids match live PropertyCode values; builds nothing and changes no state.
    /// </summary>
    internal static string CatalogAudit()
    {
        RequireReady();
        var native = new List<(string Code, string Name, bool Owned, bool Business)>();
        try
        {
            var properties = NativeProperty.Properties;
            if (properties != null)
                for (int i = 0; i < Math.Min(properties.Count, 512); i++)
                {
                    try
                    {
                        var entry = properties[i];
                        if (!PropertyRegistry.NativeAlive(entry)) continue;
                        string code = entry.PropertyCode;
                        string name = entry.PropertyName;
                        native.Add((code == null ? "" : code, name == null ? "" : name, entry.IsOwned, false));
                    }
                    catch (Exception ex) { Mod.Log.Warn($"Catalog audit: skipped property entry {i}: {ex.Message}"); }
                }
        }
        catch (Exception ex) { Mod.Log.Warn("Catalog audit: property list unavailable: " + ex.Message); }
        try
        {
            var businesses = NativeBusiness.Businesses;
            if (businesses != null)
                for (int i = 0; i < Math.Min(businesses.Count, 512); i++)
                {
                    try
                    {
                        var entry = businesses[i];
                        if (!PropertyRegistry.NativeAlive(entry)) continue;
                        string code = entry.PropertyCode;
                        string name = entry.PropertyName;
                        native.Add((code == null ? "" : code, name == null ? "" : name, entry.IsOwned, true));
                    }
                    catch (Exception ex) { Mod.Log.Warn($"Catalog audit: skipped business entry {i}: {ex.Message}"); }
                }
        }
        catch (Exception ex) { Mod.Log.Warn("Catalog audit: business list unavailable: " + ex.Message); }
        var text = new StringBuilder($"Catalog audit: {native.Count} native entries (property/business)\n");
        var matched = new HashSet<string>(StringComparer.Ordinal);
        for (int i = 0; i < native.Count; i++)
        {
            var entry = native[i];
            matched.Add(entry.Code);
            text.AppendLine($"{Clean(entry.Code)} | {Clean(entry.Name)} | {(entry.Owned ? "owned" : "not owned")} | {(entry.Business ? "business" : "property")} | catalogRooms={LEDRuntime.CatalogRoomCount(entry.Code)}");
        }
        foreach (var id in LEDRuntime.CatalogPropertyIds())
            if (!matched.Contains(id)) text.AppendLine($"CATALOG ID WITHOUT LIVE MATCH: {Clean(id)}");
        return text.ToString();
    }

    /// <summary>Name paths stay compatible with layout AnchorPath. Duplicate siblings fail closed.</summary>
    private static Transform ResolveAnchor(Transform root, string path)
    {
        if (!NetworkGuard.IsAlive(root)) throw new InvalidOperationException("Dead property root.");
        if (path == ".") return root;
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Use '.' explicitly for the property root.");
        if (path.Length > 512 || path.StartsWith('/') || path.EndsWith('/') || path.Contains('\\'))
            throw new ArgumentException("Anchor must be a relative slash-separated child path, max 512 characters.");
        string[] segments = path.Split('/');
        if (segments.Length > MaxAnchorDepth) throw new ArgumentException("Anchor exceeds 32 path segments.");
        Transform current = root;
        int examined = 0;
        for (int i = 0; i < segments.Length; i++)
        {
            if (segments[i].Length == 0 || segments[i] == "." || segments[i] == "..")
                throw new ArgumentException("Empty/dot/parent path segments are not allowed.");
            Transform? found = null;
            int children = current.childCount;
            for (int childIndex = 0; childIndex < children; childIndex++)
            {
                if (++examined > MaxAnchorChildren) throw new InvalidOperationException("Anchor resolution exceeded 4096 direct-child checks.");
                var child = current.GetChild(childIndex);
                if (!NetworkGuard.IsAlive(child) || child.name != segments[i]) continue;
                if (found != null) throw new InvalidOperationException("Ambiguous sibling name in anchor path; use a different unique ancestor or the property root.");
                found = child;
            }
            current = found ?? throw new ArgumentException("Anchor child not found: " + segments[i]);
        }
        return current;
    }

    internal static string Dump(string propertyId, string anchorPath, int maxNodes, int maxDepth)
    {
        var property = Property(propertyId);
        if (maxNodes < 1 || maxNodes > MaxDumpNodes || maxDepth < 0 || maxDepth > MaxDumpDepth)
            throw new ArgumentException("Dump limits: nodes 1..512; depth 0..12.");
        var anchor = ResolveAnchor(property.Anchor, anchorPath);
        var output = new StringBuilder();
        output.AppendLine($"PropertyLEDs bounded hierarchy survey; property={Clean(propertyId)}; anchor={Clean(anchorPath)}; slot={SaveSlots.GetActiveSlotNumber()}");
        output.AppendLine("Positions/rotations/scales are parent-local and world; propertyLocal and anchorLocal use InverseTransformPoint. Renderer bounds are WORLD axis-aligned bounds; local bounds are renderer-local.");
        var stack = new Stack<DumpFrame>();
        stack.Push(new DumpFrame(anchor, ".", 0));
        int visited = 0, examinedChildren = 0;
        bool truncated = false;
        while (stack.Count > 0)
        {
            var frame = stack.Peek();
            if (!NetworkGuard.IsAlive(frame.Node)) { stack.Pop(); continue; }
            if (!frame.Written)
            {
                if (visited >= maxNodes || output.Length >= MaxDumpCharacters - 4096) { truncated = true; break; }
                frame.Written = true;
                visited++;
                var node = frame.Node;
                output.AppendLine($"[{visited}] {Clean(frame.Path)} active={node.gameObject.activeSelf}/{node.gameObject.activeInHierarchy} children={node.childCount}");
                output.AppendLine($"  local pos={V(node.localPosition)} rot={Q(node.localRotation)} scale={V(node.localScale)}");
                output.AppendLine($"  world pos={V(node.position)} rot={Q(node.rotation)} lossyScale={V(node.lossyScale)}");
                output.AppendLine($"  propertyLocal={V(property.Anchor.InverseTransformPoint(node.position))} anchorLocal={V(anchor.InverseTransformPoint(node.position))}");
                var renderers = node.GetComponents<Renderer>(); // Only this node, never recursive native collection.
                int rendererLimit = Math.Min(renderers.Length, 16);
                for (int r = 0; r < rendererLimit; r++)
                {
                    if (output.Length >= MaxDumpCharacters - 1024) { truncated = true; break; }
                    var renderer = renderers[r];
                    if (!NetworkGuard.IsAlive(renderer)) continue;
                    var bounds = renderer.bounds;
                    var localBounds = renderer.localBounds;
                    output.AppendLine($"  renderer[{r}] enabled={renderer.enabled} worldBounds center={V(bounds.center)} size={V(bounds.size)} min={V(bounds.min)} max={V(bounds.max)} localBounds center={V(localBounds.center)} size={V(localBounds.size)}");
                }
                if (renderers.Length > rendererLimit) { truncated = true; output.AppendLine("  renderers TRUNCATED at 16 per node."); }
            }
            if (frame.Depth >= maxDepth)
            {
                if (frame.Node.childCount > 0) truncated = true;
                stack.Pop();
                continue;
            }
            if (frame.ChildIndex >= frame.Node.childCount) { stack.Pop(); continue; }
            int index = frame.ChildIndex++;
            if (++examinedChildren > MaxDumpNodes * 4) { truncated = true; break; }
            var child = frame.Node.GetChild(index);
            if (!NetworkGuard.IsAlive(child)) continue;
            var markerRoot = _markerRoot;
            if (NetworkGuard.IsAlive(markerRoot) && child.Pointer == markerRoot!.transform.Pointer) continue;
            string childPath = frame.Path == "." ? child.name : frame.Path + "/" + child.name;
            if (childPath.Length > 1024) { truncated = true; continue; }
            stack.Push(new DumpFrame(child, childPath, frame.Depth + 1));
        }
        output.AppendLine($"Survey complete: nodes={visited}, truncated={truncated}, maxNodes={maxNodes}, maxDepth={maxDepth}. No geometry inferred.");
        string file = SafeStorage.GetUserDataPath("PropertyLEDs", $"diagnostics/hierarchy_slot_{SaveSlots.GetActiveSlotNumber()}.txt");
        if (!SafeStorage.SaveTextAtomic(file, output.ToString(), Mod.Log)) throw new IOException("Hierarchy export failed; see mod log.");
        return $"Surveyed {visited} nodes; truncated={truncated}. Saved {file}. Read this file for transforms and renderer bounds.";
    }

    internal static string Begin(string propertyId, string roomId, string anchorPath, string displayName)
    {
        var property = Property(propertyId);
        ValidateToken(roomId, "room ID");
        var anchor = ResolveAnchor(property.Anchor, anchorPath);
        ResetCapture();
        _capture = new RoomLayout
        {
            PropertyId = propertyId,
            RoomId = roomId,
            DisplayName = string.IsNullOrWhiteSpace(displayName) ? roomId : displayName,
            AnchorPath = anchorPath,
            Verified = false,
            Width = 0.035f,
            Paths = new List<LEDPath> { new() { Closed = false, Points = new List<LEDPoint>() } }
        };
        _captureAnchor = anchor;
        _propertyPointer = property.Anchor.Pointer;
        _captureSlot = SaveSlots.GetActiveSlotNumber();
        return $"Capture begun: {Clean(propertyId)}/{Clean(roomId)}; anchor={Clean(_capture.AnchorPath)}; Verified=false. No points invented. Existing capture replaced in memory only.";
    }

    private static RoomLayout Capture()
    {
        RequireReady();
        if (_capture == null) throw new InvalidOperationException("Begin a capture first: led begin <propertyId> <roomId> [anchorPath|.].");
        var property = Property(_capture.PropertyId);
        if (SaveSlots.GetActiveSlotNumber() != _captureSlot || property.Anchor.Pointer != _propertyPointer || !NetworkGuard.IsAlive(_captureAnchor))
        {
            ResetCapture();
            throw new InvalidOperationException("Capture invalidated by slot or anchor change; begin again.");
        }
        return _capture;
    }

    internal static string Point(Vector3 value, bool world)
    {
        var layout = Capture();
        if (!Finite(value)) throw new ArgumentException("Point coordinates must be finite.");
        var point = world ? _captureAnchor!.InverseTransformPoint(value) : value;
        if (!Finite(point) || Math.Abs(point.x) > MaxLocalCoordinate || Math.Abs(point.y) > MaxLocalCoordinate || Math.Abs(point.z) > MaxLocalCoordinate)
            throw new ArgumentException("Point is outside the +/-200 metre anchor-local measurement bound.");
        var path = layout.Paths[layout.Paths.Count - 1];
        if (path.Closed) throw new InvalidOperationException("Current path is closed; use 'led path open' or 'led closed false'.");
        if (path.Points.Count > 0)
        {
            var previous = path.Points[path.Points.Count - 1];
            if ((point - new Vector3(previous.X, previous.Y, previous.Z)).sqrMagnitude < 1e-10f)
                throw new ArgumentException("Consecutive measured points must be distinct.");
        }
        if (path.Points.Count >= MaxPointsPerPath || TotalPoints(layout) >= MaxPoints)
            throw new InvalidOperationException("Point cap reached: 128 per path / 256 total.");
        HidePreview();
        path.Points.Add(new LEDPoint { X = point.x, Y = point.y, Z = point.z });
        return $"Captured path {layout.Paths.Count}, point {path.Points.Count}: anchorLocal={V(point)}, world={V(_captureAnchor!.TransformPoint(point))}. Verified=false; preview cleared (run led preview).";
    }

    internal static string Aim(float distance)
    {
        Capture();
        if (!float.IsFinite(distance) || distance <= 0 || distance > 30f) throw new ArgumentException("Aim distance: finite, >0..30 metres.");
        var cameraOwner = PlayerCamera.Instance;
        if (!NetworkGuard.IsAlive(cameraOwner) || !NetworkGuard.IsAlive(cameraOwner.Camera))
            throw new InvalidOperationException("Local player's camera is unavailable.");
        var transform = cameraOwner.Camera.transform;
        int mask = ~LayerMask.GetMask("Ignore Raycast", "Player", "Overlay", "UI");
        if (!Physics.Raycast(new Ray(transform.position, transform.forward), out var hit, distance, mask, QueryTriggerInteraction.Ignore))
            throw new InvalidOperationException("No non-trigger hit within the bounded aim ray.");
        var property = Property(_capture!.PropertyId);
        if (!NetworkGuard.IsAlive(hit.transform) || (hit.transform.Pointer != property.Anchor.Pointer && !hit.transform.IsChildOf(property.Anchor)))
            throw new InvalidOperationException("First ray hit is outside the selected property subtree; point rejected (no ray-through-other-property fallback).");
        return Point(hit.point, world: true);
    }

    /// <summary>Optional console-safe delayed aim: restore camera look, then capture with primary click.</summary>
    internal static string ArmAim(float distance)
    {
        Capture();
        if (!float.IsFinite(distance) || distance <= 0 || distance > 30f) throw new ArgumentException("Aim distance: >0..30 metres.");
        CancelAim();
        _aimArmed = true;
        _aimDistance = distance;
        _aimExpires = Time.unscaledTime + 30f;
        MelonEvents.OnUpdate.Subscribe(TickAim);
        return "Aim capture armed for 30 seconds. Close console and phone, aim at this property's collider, press left mouse once. Escape cancels. No gameplay click is suppressed.";
    }

    private static void TickAim()
    {
        if (!_aimArmed) return;
        if (!LEDRuntime.Ready || !NetworkGuard.IsInMainScene || Time.unscaledTime > _aimExpires || UnityEngine.Input.GetKeyDown(KeyCode.Escape))
        { CancelAim(); return; }
        if (S1API.Input.Controls.IsTyping || !UnityEngine.Input.GetMouseButtonDown(0)) return;
        float distance = _aimDistance;
        CancelAim();
        try { Mod.Log.Info(Aim(distance)); }
        catch (Exception ex) { Mod.Log.Warn("Aim capture rejected: " + ex.Message); }
    }

    private static void CancelAim()
    {
        _aimArmed = false;
        MelonEvents.OnUpdate.Unsubscribe(TickAim);
    }

    internal static string NewPath()
    {
        var layout = Capture();
        if (layout.Paths.Count >= MaxPaths) throw new InvalidOperationException("Path cap reached (16).");
        if (layout.Paths[layout.Paths.Count - 1].Points.Count < 2) throw new InvalidOperationException("Current path needs at least 2 points before a new path.");
        HidePreview();
        layout.Paths.Add(new LEDPath { Closed = false, Points = new List<LEDPoint>() });
        return $"Started open path {layout.Paths.Count}.";
    }

    internal static string Closed(bool closed)
    {
        var layout = Capture();
        var path = layout.Paths[layout.Paths.Count - 1];
        if (closed && path.Points.Count < 3) throw new InvalidOperationException("Closed path requires at least 3 measured points.");
        HidePreview();
        path.Closed = closed;
        return "Current path Closed=" + closed;
    }

    internal static string Undo()
    {
        var layout = Capture();
        var path = layout.Paths[layout.Paths.Count - 1];
        HidePreview();
        if (path.Points.Count == 0 && layout.Paths.Count > 1) layout.Paths.RemoveAt(layout.Paths.Count - 1);
        else if (path.Points.Count > 0) { path.Closed = false; path.Points.RemoveAt(path.Points.Count - 1); }
        else return "No points to undo.";
        return $"Undo complete: paths={layout.Paths.Count}, points={TotalPoints(layout)}. Nothing written.";
    }

    internal static string Width(float width)
    {
        var layout = Capture();
        if (!float.IsFinite(width) || width < 0.005f || width > 0.25f) throw new ArgumentException("Width: finite 0.005..0.25 metres.");
        HidePreview();
        layout.Width = width;
        return "Capture width=" + width.ToString("0.###", CultureInfo.InvariantCulture);
    }

    internal static string Preview()
    {
        var layout = Capture();
        ValidatePaths(layout);
        HidePreview();
        var shader = Shader.Find("Universal Render Pipeline/Unlit");
        if (!NetworkGuard.IsAlive(shader)) throw new InvalidOperationException("URP/Unlit unavailable; no unsafe fallback shader used.");
        try
        {
            _markerMaterial = new Material(shader);
            _markerMaterial.name = "PropertyLEDs_DiagnosticMarkerMaterial";
            _markerMaterial.SetColor("_BaseColor", new Color(1f, 0.3f, 0.1f, 1f));
            _markerRoot = new GameObject("PropertyLEDs_MeasurementMarkers");
            _markerRoot.transform.SetParent(_captureAnchor, false);
            for (int i = 0; i < layout.Paths.Count; i++)
                for (int j = 0; j < layout.Paths[i].Points.Count; j++)
                {
                    var point = layout.Paths[i].Points[j];
                    var marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    // Track before touching native components so cleanup also covers partial failures.
                    marker.transform.SetParent(_markerRoot.transform, false);
                    marker.name = $"LEDPoint_{i + 1}_{j + 1}";
                    var collider = marker.GetComponent<Collider>();
                    if (NetworkGuard.IsAlive(collider)) { collider.enabled = false; Object.Destroy(collider); }
                    marker.transform.localPosition = new Vector3(point.X, point.Y, point.Z);
                    marker.transform.localScale = Vector3.one * 0.065f;
                    var renderer = marker.GetComponent<Renderer>();
                    renderer.sharedMaterial = _markerMaterial;
                    renderer.shadowCastingMode = ShadowCastingMode.Off;
                    renderer.receiveShadows = false;
                    _markerCount++;
                }
            LEDRuntime.PreviewLayout(Snapshot(layout));
            _previewActive = true;
            return $"Preview requested: {_markerCount} temporary collider-free markers and LED polylines. Verified remains false; normal property settings unchanged. 'led hide' removes preview.";
        }
        catch { HidePreview(); throw; }
    }

    internal static void HidePreview()
    {
        try { LEDRuntime.ClearPreview(); } catch (Exception ex) { Mod.Log.Warn("Preview cleanup: " + ex.Message); }
        DestroyOwned(_markerRoot);
        DestroyOwned(_markerMaterial);
        _markerRoot = null;
        _markerMaterial = null;
        _markerCount = 0;
        _previewActive = false;
    }

    internal static string Clear()
    {
        ResetCapture();
        return "Measurement capture, armed aim, markers and LED preview cleared. Exported files untouched.";
    }

    private static void ResetCapture()
    {
        CancelAim();
        HidePreview();
        _capture = null;
        _captureAnchor = null;
        _propertyPointer = IntPtr.Zero;
        _captureSlot = -1;
    }

    internal static string Export(string name)
    {
        var layout = Capture();
        ValidatePaths(layout);
        ValidateToken(name, "export name");
        var catalog = new LayoutCatalog { FormatVersion = 1, Rooms = new List<RoomLayout> { Snapshot(layout) } };
        string file = SafeStorage.GetUserDataPath("PropertyLEDs", "diagnostics/" + name + ".json");
        if (!SafeStorage.SaveAtomic(file, catalog, Mod.Log)) throw new IOException("Layout export failed; see mod log. Capture kept in memory.");
        return $"Exported candidate to {file}; Verified=false. This does NOT overwrite runtime layouts. Inspect and explicitly promote the measured room into layouts.json, then 'led reload'. No compile needed.";
    }

    internal static string Reload()
    {
        RequireReady();
        HidePreview();
        LEDRuntime.ReloadLayouts();
        return "Runtime layout reload requested. Inspect led rooms / led status and the mod log for accepted layouts; no compilation or automatic verification.";
    }

    private static RoomLayout Snapshot(RoomLayout layout)
    {
        var result = new RoomLayout
        {
            PropertyId = layout.PropertyId,
            RoomId = layout.RoomId,
            DisplayName = layout.DisplayName,
            AnchorPath = layout.AnchorPath,
            Verified = false,
            Width = layout.Width,
            Paths = new List<LEDPath>()
        };
        for (int i = 0; i < layout.Paths.Count; i++)
        {
            var path = new LEDPath { Closed = layout.Paths[i].Closed, Points = new List<LEDPoint>() };
            for (int j = 0; j < layout.Paths[i].Points.Count; j++)
            {
                var p = layout.Paths[i].Points[j];
                path.Points.Add(new LEDPoint { X = p.X, Y = p.Y, Z = p.Z });
            }
            result.Paths.Add(path);
        }
        return result;
    }

    private static void ValidatePaths(RoomLayout layout)
    {
        if (layout.Paths.Count < 1 || layout.Paths.Count > MaxPaths || TotalPoints(layout) > MaxPoints)
            throw new InvalidOperationException("Capture path/point budget invalid.");
        for (int i = 0; i < layout.Paths.Count; i++)
        {
            if (layout.Paths[i].Points.Count < (layout.Paths[i].Closed ? 3 : 2) || layout.Paths[i].Points.Count > MaxPointsPerPath)
                throw new InvalidOperationException($"Path {i + 1} needs {(layout.Paths[i].Closed ? 3 : 2)}..128 measured points. Finish or undo the empty path.");
            if (!layout.Paths[i].Validate(out string error)) throw new InvalidOperationException($"Path {i + 1}: {error}");
        }
    }

    private static int TotalPoints(RoomLayout layout)
    {
        int count = 0;
        for (int i = 0; i < layout.Paths.Count; i++) count += layout.Paths[i].Points.Count;
        return count;
    }

    internal static void ValidateToken(string token, string label)
    {
        if (string.IsNullOrWhiteSpace(token) || token.Length > 64) throw new ArgumentException(label + ": use 1..64 ASCII letters, digits, '-' or '_'.");
        for (int i = 0; i < token.Length; i++)
            if (!(token[i] >= 'a' && token[i] <= 'z') && !(token[i] >= 'A' && token[i] <= 'Z') &&
                !(token[i] >= '0' && token[i] <= '9') && token[i] != '-' && token[i] != '_')
                throw new ArgumentException(label + ": use ASCII letters, digits, '-' or '_'; token was not modified.");
    }

    /// <summary>Plain JSON only, bounded registry view with no Unity objects serialized.</summary>
    internal static string StatusJson()
    {
        bool ready = LEDRuntime.Ready && NetworkGuard.IsInMainScene;
        var entries = new List<object>();
        var properties = LEDRuntime.Properties;
        int limit = ready ? Math.Min(properties.Count, 128) : 0;
        for (int i = 0; i < limit; i++)
        {
            var property = properties[i];
            var roomStatus = new List<object>();
            var rooms = LEDRuntime.GetRooms(property.Id);
            for (int j = 0; j < Math.Min(rooms.Count, 128); j++)
            {
                var room = rooms[j];
                roomStatus.Add(new
                {
                    room.RoomId,
                    room.DisplayName,
                    room.AnchorPath,
                    room.Verified,
                    LayoutPathCount = room.Paths.Count,
                    LayoutPointCount = TotalPoints(room),
                    HasOverride = LEDRuntime.HasOverride(property.Id, room.RoomId),
                    Settings = LEDRuntime.GetSettings(property.Id, room.RoomId).Clone()
                });
            }
            entries.Add(new
            {
                property.Id,
                property.DisplayName,
                property.IsBusiness,
                AnchorAlive = NetworkGuard.IsAlive(property.Anchor),
                Settings = LEDRuntime.GetSettings(property.Id, null).Clone(),
                RoomCount = rooms.Count,
                RoomsTruncated = rooms.Count > 128,
                Rooms = roomStatus
            });
        }
        return JsonSerializer.Serialize(new
        {
            Ready = ready,
            Slot = SaveSlots.GetActiveSlotNumber(),
            Revision = LEDRuntime.Revision,
            RegistryCount = properties.Count,
            RegistryTruncated = properties.Count > limit,
            Properties = entries,
            DiagnosticResources = new { BridgeAlive = NetworkGuard.IsAlive(_bridgeRoot), MarkerRootAlive = NetworkGuard.IsAlive(_markerRoot), MarkerCount = _markerCount, PreviewRequested = _previewActive, AimArmed = _aimArmed },
            Capture = _capture == null ? null : new
            {
                _capture.PropertyId,
                _capture.RoomId,
                _capture.AnchorPath,
                _capture.Width,
                Verified = false,
                Slot = _captureSlot,
                AnchorAlive = NetworkGuard.IsAlive(_captureAnchor),
                PathCount = _capture.Paths.Count,
                PointCount = TotalPoints(_capture)
            }
        });
    }

    private static bool Finite(Vector3 p) => float.IsFinite(p.x) && float.IsFinite(p.y) && float.IsFinite(p.z);
    private static string V(Vector3 value) => $"({F(value.x)}, {F(value.y)}, {F(value.z)})";
    private static string Q(Quaternion value) => $"({F(value.x)}, {F(value.y)}, {F(value.z)}, {F(value.w)})";
    private static string F(float value) => value.ToString("0.#####", CultureInfo.InvariantCulture);
    private static string Clean(string value) => value.Replace('\r', ' ').Replace('\n', ' ').Replace('<', '[').Replace('>', ']');
    private static void DestroyOwned(Object? value)
    {
        try { if (NetworkGuard.IsAlive(value)) Object.Destroy(value); }
        catch (Exception ex) { Mod.Log.Warn("Diagnostic resource cleanup: " + ex.Message); }
    }

    private sealed class DumpFrame
    {
        internal readonly Transform Node;
        internal readonly string Path;
        internal readonly int Depth;
        internal int ChildIndex;
        internal bool Written;
        internal DumpFrame(Transform node, string path, int depth) { Node = node; Path = path; Depth = depth; }
    }
}

/// <summary>
/// Primitive-only IL2CPP bridge for tooling. The installed S1MCP cannot pass arguments to an
/// injected method (a string parameter and an int property both fail silently), so every entry
/// point here is parameterless: the selection is tracked internally and actions return their text
/// result. Humans keep the full console syntax through <see cref="Execute"/>.
/// </summary>
[RegisterTypeInIl2Cpp]
public sealed class LEDDiagnosticBridge : MonoBehaviour
{
    private int _index;

    public LEDDiagnosticBridge(IntPtr pointer) : base(pointer) { }

    /// <summary>Currently selected 0-based registry index (see <see cref="Properties"/>).</summary>
    public int Index => _index;

    /// <summary>Selected property/business id, empty when none is selected yet.</summary>
    public string Selected => SelectedId() ?? "";

    public string Execute(string command) => LEDConsoleCommand.ExecuteText(command);

    public string Status() => Guard(() => LEDDiagnostics.StatusJson());

    public string Properties() => Run("properties");

    /// <summary>Selects the next owned property/business (wraps) and returns the list.</summary>
    public string NextProperty()
    {
        _index++;
        return Properties();
    }

    /// <summary>Selects the previous owned property/business (wraps) and returns the list.</summary>
    public string PrevProperty()
    {
        _index--;
        return Properties();
    }

    public string Rooms() => Run("rooms", SelectedId());

    /// <summary>Bounded hierarchy dump (256 nodes, depth 8) of the selected property into the diagnostics folder.</summary>
    public string Dump() => Run("dump", SelectedId(), ".", "256", "8");

    public string Preview() => Run("preview");
    public string HidePreview() => Run("hide");
    public string ClearCapture() => Run("clear");
    public string ExportCapture() => Run("export", "capture_" + (_index < 0 ? 0 : _index));
    public string ReloadLayouts() => Run("reload");
    public string RefreshProperties() => Run("refresh");

    /// <summary>Read-only: every native property/business vs catalog ids (code match + room counts).</summary>
    public string CatalogAudit() => Guard(() => LEDDiagnostics.CatalogAudit());

    private string? SelectedId()
    {
        var properties = LEDRuntime.Properties;
        if (properties.Count == 0) return null;
        if (_index < 0) _index += properties.Count;
        if (_index >= properties.Count) _index %= properties.Count;
        return properties[_index].Id;
    }

    /// <summary>Builds a 'led ...' command from already-validated tokens (never raw caller text).</summary>
    private static string Run(params string?[] tokens) => Guard(() =>
    {
        var args = new List<string>(tokens.Length);
        for (int i = 0; i < tokens.Length; i++)
        {
            if (tokens[i] == null) return "ERROR: no property/business is selected yet (call NextProperty first).";
            args.Add(tokens[i]!);
        }
        return LEDConsoleCommand.ExecuteArgs(args);
    });

    private static string Guard(Func<string> action)
    {
        try { return action(); }
        catch (Exception ex) { return "ERROR: " + ex.Message; }
    }
}
