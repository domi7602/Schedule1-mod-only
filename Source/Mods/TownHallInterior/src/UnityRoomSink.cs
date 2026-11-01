using System;
using UnityEngine;

namespace TownHallInterior;

// Creates only owned primitives and materials. Publish() is called only after the
// collider-only hall swap succeeds. Renderers and the native StaticDoor are not touched.
internal sealed class UnityRoomSink : IRoomSink
{
    private GameObject? _root;
    private readonly Material?[] _materials = new Material?[3];

    public UnityRoomSink()
    {
        try
        {
            _root = new GameObject("TownHallInterior.Staged");
            _root.SetActive(false);
            var shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) throw new InvalidOperationException("URP/Lit shader unavailable");
            var colors = new[] { new Color(0.52f, 0.39f, 0.27f), new Color(0.73f, 0.70f, 0.64f), new Color(0.30f, 0.22f, 0.16f) };
            for (int i = 0; i < _materials.Length; i++)
            {
                _materials[i] = new Material(shader);
                _materials[i]!.color = colors[i];
            }
        }
        catch { Dispose(); throw; }
    }

    internal Transform? Root => _root == null ? null : _root.transform;

    internal void Publish()
    {
        if (_root == null) throw new ObjectDisposedException(nameof(UnityRoomSink));
        _root.SetActive(true);
    }

    public void Add(RoomBox box)
    {
        if (_root == null) throw new ObjectDisposedException(nameof(UnityRoomSink));
        GameObject? child = null;
        try
        {
            child = GameObject.CreatePrimitive(PrimitiveType.Cube);
            child.name = box.Name;
            child.transform.SetParent(_root.transform, false);
            child.transform.localPosition = new Vector3(box.X, box.Y, box.Z);
            child.transform.localScale = new Vector3(box.Width, box.Height, box.Depth);
            child.GetComponent<Renderer>().sharedMaterial = _materials[box.Style];
            if (!box.Solid) UnityEngine.Object.Destroy(child.GetComponent<Collider>());
            // These sit on the native shell or inside the brick trim and z-fight.
            // Keep their collision; the visible inner shell is separate.
            if (box.Name.StartsWith("west-") || box.Name.StartsWith("upper-") ||
                box.Name is "east" or "south" or "north")
                child.GetComponent<Renderer>().enabled = false;
        }
        catch
        {
            if (child != null) UnityEngine.Object.Destroy(child);
            throw;
        }
    }

    public void Dispose()
    {
        if (_root != null) { UnityEngine.Object.Destroy(_root); _root = null; }
        for (int i = 0; i < _materials.Length; i++)
        {
            if (_materials[i] != null) UnityEngine.Object.Destroy(_materials[i]);
            _materials[i] = null;
        }
    }
}
