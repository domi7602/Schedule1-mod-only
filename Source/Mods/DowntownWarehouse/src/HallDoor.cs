using System;
using System.Collections.Generic;
using Il2CppFishNet.Object;
using Il2CppScheduleOne.Interaction;
using Il2CppScheduleOne.PlayerScripts;
using MelonLoader;
using S1Mods.Shared;
using UnityEngine;
using UnityEngine.Events;

namespace DowntownWarehouse;

/// <summary>
/// Turns the native dummy blue door behind Hank's counter into a working entrance.
/// The shell door has inactive IntObj and no live leaf, so we clone interaction from
/// a functioning industrial door donor, hide the dummy renderers, and swing an owned
/// leaf. Locked until the hall is owned. Own implementation; the inspected third-party
/// binary only mapped the native hierarchy and interaction API.
/// </summary>
internal static class HallDoor
{
    private const string DonorPath = "Map/Hyland Point/Region_Northtown/Storage warehouse/Industrial Metal Door";
    private const string MsgOpen = "Open";
    private const string MsgClose = "Close";
    private const string MsgLocked = "Locked. Buy it at Ray's Realty";
    private const float OpenAngle = 100f;
    private const float SwingSpeed = 5f;

    private static GameObject? _root;
    private static Transform? _pivot;
    private static Quaternion _pivotBase;
    private static float _sign = -1f;
    private static BoxCollider? _leafCol;
    private static Mesh? _fallbackMesh;
    private static InteractableObject? _ioShop;
    private static InteractableObject? _ioHall;
    private static Transform? _dummyParts;
    private static readonly List<Renderer> _hiddenRenderers = new();
    private static readonly List<Collider> _disabledColliders = new();
    private static bool _applied;
    private static bool _open;
    private static float _angle;
    private static bool _owned;

    internal static bool Build()
    {
        Reset();
        try
        {
            Prepare();
            return true;
        }
        catch (Exception ex)
        {
            Reset();
            MelonLogger.Warning("[DowntownWarehouse] Blaue Tür nicht bereit: " + ex.Message);
            return false;
        }
    }

    private static void Prepare()
    {
        var door = GameObject.Find(HallPlan.BlueDoorPath);
        if (door == null)
            throw new InvalidOperationException("Native Tür fehlt.");

        var container = door.transform.Find("Container");
        _dummyParts = door.transform.Find("Container/IndustrialMetalDoor/industrialmetaldoor");
        var leafSrc = _dummyParts != null ? _dummyParts.Find("Door") : null;
        var handleSrc = _dummyParts != null ? _dummyParts.Find("Handle") : null;
        if (container == null || leafSrc == null)
            throw new InvalidOperationException("Unerwartetes Tür-Layout.");

        var donor = GameObject.Find(DonorPath);
        var donorLeaf = donor != null ? donor.transform.Find("Container/IndustrialMetalDoor/industrialmetaldoor/Door") : null;
        var donorHandle = donor != null ? donor.transform.Find("Container/IndustrialMetalDoor/industrialmetaldoor/Handle") : null;

        _root = new GameObject("DowntownWarehouse_BlueDoor");
        _root.SetActive(false);
        var pivotGo = new GameObject("Pivot");
        pivotGo.transform.SetParent(_root.transform, false);
        pivotGo.transform.position = container.position;
        pivotGo.transform.rotation = container.rotation;
        _pivot = pivotGo.transform;
        _pivotBase = container.rotation;

        var leafMat = leafSrc.GetComponent<MeshRenderer>()?.sharedMaterial
            ?? throw new InvalidOperationException("Material des Türblatts fehlt.");
        bool leafOk = CopyPart("Leaf", donorLeaf, leafSrc, leafMat);
        if (!leafOk)
        {
            var box = leafSrc.GetComponent<BoxCollider>();
            var fallback = new GameObject("LeafFallback");
            fallback.transform.SetParent(_pivot, false);
            fallback.transform.position = leafSrc.position;
            fallback.transform.rotation = leafSrc.rotation;
            var size = box != null ? box.size : new Vector3(0.06f, 2.1f, 1.05f);
            var mf = fallback.AddComponent<MeshFilter>();
            _fallbackMesh = new Mesh { name = "DowntownWarehouse_DoorLeaf" };
            BuildBoxMesh(_fallbackMesh, size);
            mf.sharedMesh = _fallbackMesh;
            fallback.AddComponent<MeshRenderer>().sharedMaterial = leafMat;
        }

        if (handleSrc != null)
            CopyPart("Handle", donorHandle, handleSrc, handleSrc.GetComponent<MeshRenderer>()?.sharedMaterial);

        var leafBox = leafSrc.GetComponent<BoxCollider>();
        var colGo = new GameObject("LeafCollider");
        colGo.transform.SetParent(_pivot, false);
        colGo.transform.position = leafSrc.position;
        colGo.transform.rotation = leafSrc.rotation;
        _leafCol = colGo.AddComponent<BoxCollider>();
        _leafCol.center = leafBox != null ? leafBox.center : Vector3.zero;
        _leafCol.size = leafBox != null ? leafBox.size : new Vector3(0.08f, 2.1f, 1.05f);

        var ioShopSrc = donor != null ? donor.transform.Find("Container/ExteriorIntObj") : null;
        var ioHallSrc = donor != null ? donor.transform.Find("Container/InteriorIntObj") : null;
        var ioShopPose = door.transform.Find("Container/ExteriorIntObj");
        var ioHallPose = door.transform.Find("Container/InteriorIntObj");
        _ioShop = CloneIo(ioShopSrc != null ? ioShopSrc : ioShopPose, ioShopPose, "IntObj_Shop");
        _ioHall = CloneIo(ioHallSrc != null ? ioHallSrc : ioHallPose, ioHallPose, "IntObj_Hall");

        var offset = leafSrc.position - container.position;
        var a = Quaternion.Euler(0f, 50f, 0f) * offset;
        var b = Quaternion.Euler(0f, -50f, 0f) * offset;
        _sign = a.x > b.x ? 1f : -1f;

        MelonLogger.Msg($"[DowntownWarehouse] Blaue Tür vorbereitet: Spenderblatt={leafOk}, beide Türseiten angebunden.");
    }

    internal static void Apply(bool owned)
    {
        if (_root == null || _dummyParts == null)
            return;
        try
        {
            if (!_applied)
            {
                HideDummy(_dummyParts.Find("Door"));
                HideDummy(_dummyParts.Find("Handle"));
                HideDummy(_dummyParts.Find("Quad"));
                HideDummy(_dummyParts.Find("Quad (1)"));
                _root.SetActive(true);
                _applied = true;
            }
            _owned = owned;
            _open = false;
            _angle = 0f;
            if (_pivot != null) _pivot.rotation = _pivotBase;
            if (_leafCol != null) _leafCol.enabled = true;
            UpdatePrompt(_ioShop);
            UpdatePrompt(_ioHall);
        }
        catch
        {
            Reset();
            throw;
        }
    }

    internal static void Tick()
    {
        if (!_applied || _pivot == null)
            return;
        float target = _open ? OpenAngle : 0f;
        if (Mathf.Abs(_angle - target) < 0.01f)
            return;
        _angle = Mathf.MoveTowards(_angle, target, SwingSpeed * Time.deltaTime * 60f);
        _pivot.rotation = _pivotBase * Quaternion.Euler(0f, _sign * _angle, 0f);
        if (_leafCol != null)
            _leafCol.enabled = _angle < 5f;
    }

    internal static void Reset()
    {
        _applied = false;
        if (NetworkGuard.IsAlive(_root))
        {
            try { _root!.SetActive(false); } catch { }
            try { UnityEngine.Object.Destroy(_root!); } catch { }
        }
        if (NetworkGuard.IsAlive(_fallbackMesh))
        {
            try { UnityEngine.Object.Destroy(_fallbackMesh!); } catch { }
        }
        foreach (var renderer in _hiddenRenderers)
        {
            if (NetworkGuard.IsAlive(renderer))
            {
                try
                {
                    if (!renderer.enabled)
                        renderer.enabled = true;
                }
                catch { }
            }
        }
        foreach (var collider in _disabledColliders)
        {
            if (NetworkGuard.IsAlive(collider))
            {
                try
                {
                    if (!collider.enabled)
                        collider.enabled = true;
                }
                catch { }
            }
        }
        _hiddenRenderers.Clear();
        _disabledColliders.Clear();
        _dummyParts = null;
        _fallbackMesh = null;
        _root = null;
        _pivot = null;
        _leafCol = null;
        _ioShop = null;
        _ioHall = null;
        _open = false;
        _angle = 0f;
        _owned = false;
    }

    private static void OnInteract()
    {
        if (!_applied)
            return;
        if (_open)
        {
            _open = false;
        }
        else if (MayOpen())
        {
            _open = true;
        }

        UpdatePrompt(_ioShop);
        UpdatePrompt(_ioHall);
    }

    private static bool MayOpen()
    {
        if (_owned)
            return true;
        var local = Player.Local;
        if (local == null)
            return false;
        var p = local.transform.position;
        return p.x > 108.75f && HallPlan.ContainsInterior(p.x, p.z);
    }

    private static void UpdatePrompt(InteractableObject? io)
    {
        if (io == null)
            return;
        if (_open)
        {
            io.SetMessage(MsgClose);
            io.SetInteractableState(InteractableObject.EInteractableState.Default);
        }
        else if (MayOpen())
        {
            io.SetMessage(MsgOpen);
            io.SetInteractableState(InteractableObject.EInteractableState.Default);
        }
        else
        {
            io.SetMessage(MsgLocked);
            io.SetInteractableState(InteractableObject.EInteractableState.Invalid);
        }
    }

    private static bool CopyPart(string name, Transform? meshSrc, Transform poseSrc, Material? mat)
    {
        if (meshSrc == null)
            return false;
        var mesh = meshSrc.GetComponent<MeshFilter>()?.sharedMesh;
        if (mesh == null || mesh.vertexCount == 0 || mesh.name.StartsWith("Combined", StringComparison.Ordinal))
            return false;
        var go = new GameObject(name);
        go.transform.SetParent(_pivot, false);
        go.transform.position = poseSrc.position;
        go.transform.rotation = poseSrc.rotation;
        go.transform.localScale = meshSrc.lossyScale;
        go.AddComponent<MeshFilter>().sharedMesh = mesh;
        go.AddComponent<MeshRenderer>().sharedMaterial = mat;
        return true;
    }

    private static void HideDummy(Transform? part)
    {
        if (part == null)
            return;
        foreach (var renderer in part.GetComponents<Renderer>())
        {
            if (renderer == null || !renderer.enabled) continue;
            _hiddenRenderers.Add(renderer);
            renderer.enabled = false;
        }
        foreach (var collider in part.GetComponents<Collider>())
        {
            if (collider == null || !collider.enabled) continue;
            _disabledColliders.Add(collider);
            collider.enabled = false;
        }
    }

    private static InteractableObject CloneIo(Transform? src, Transform? pose, string name)
    {
        if (src == null || pose == null)
            throw new InvalidOperationException(name + ": Interaktionsvorlage oder Zielposition fehlt.");
        // The pivot is under the inactive staging root, including during Instantiate.
        var go = UnityEngine.Object.Instantiate(src.gameObject, _pivot);
        go.name = name;
        go.transform.position = pose.position;
        go.transform.rotation = pose.rotation;
        go.transform.localScale = pose.lossyScale;
        foreach (var nb in go.GetComponentsInChildren<NetworkBehaviour>(true))
            if (nb != null) UnityEngine.Object.DestroyImmediate(nb);
        foreach (var no in go.GetComponentsInChildren<NetworkObject>(true))
            if (no != null) UnityEngine.Object.DestroyImmediate(no);
        foreach (var mr in go.GetComponentsInChildren<MeshRenderer>(true))
            if (mr != null) mr.enabled = false;
        var io = go.GetComponent<InteractableObject>();
        if (io == null || go.GetComponent<Collider>() == null)
            throw new InvalidOperationException(name + ": Interaktion oder Treffer-Collider fehlt.");
        foreach (var col in go.GetComponentsInChildren<Collider>(true))
            if (col != null) col.enabled = true;
        io.enabled = true;
        io.onHovered = new UnityEvent();
        io.onInteractStart = new UnityEvent();
        io.onInteractEnd = new UnityEvent();
        io.SetMessage(MsgOpen);
        io.SetInteractionType(InteractableObject.EInteractionType.Key_Press);
        io.SetInteractableState(InteractableObject.EInteractableState.Default);
        // Keep the cloned event's native delegate free of references to the clone.
        io.onHovered.AddListener((Action)(() =>
        {
            if (!_applied) return;
            try
            {
                UpdatePrompt(_ioShop);
                UpdatePrompt(_ioHall);
            }
            catch { }
        }));
        io.onInteractStart.AddListener((Action)(() => { try { OnInteract(); } catch { } }));
        go.SetActive(true);
        return io;
    }

    private static void BuildBoxMesh(Mesh mesh, Vector3 size)
    {
        var h = size * 0.5f;
        var v = new Vector3[]
        {
            new(-h.x,-h.y,-h.z), new(h.x,-h.y,-h.z), new(h.x,h.y,-h.z), new(-h.x,h.y,-h.z),
            new(-h.x,-h.y,h.z), new(h.x,-h.y,h.z), new(h.x,h.y,h.z), new(-h.x,h.y,h.z)
        };
        var t = new int[]
        {
            0,2,1, 0,3,2, 4,5,6, 4,6,7,
            0,1,5, 0,5,4, 2,3,7, 2,7,6,
            0,4,7, 0,7,3, 1,2,6, 1,6,5
        };
        mesh.vertices = v;
        mesh.triangles = t;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}
