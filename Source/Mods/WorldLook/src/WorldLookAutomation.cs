using System;
using System.Threading;
using Il2CppInterop.Runtime.Attributes;
using MelonLoader;
using S1Mods.Shared;
using UnityEngine;

namespace WorldLook;

/// <summary>
/// Primitive-only, on-demand adapter for the existing MCP component API. Explicit
/// ON/OFF wrappers avoid the bridge's parameter-marshalling limitation. No Update,
/// polling, profile writes or references to the optional MCP mod are needed.
/// </summary>
[RegisterTypeInIl2Cpp]
public sealed class WorldLookAutomation : MonoBehaviour
{
    public WorldLookAutomation(IntPtr pointer) : base(pointer) { }

    public string GetStatus() => Invoke("status", null);
    public string EnableLook() => SetLookEnabled(true);
    public string DisableLook() => SetLookEnabled(false);
    public string SetLookEnabled(bool enabled) => Invoke(enabled ? "enable" : "disable", enabled);
    public string ReturnToMenu() => Invoke("return-to-menu", null);

    [HideFromIl2Cpp]
    private string Invoke(string action, bool? enabled)
    {
        // No native access is attempted before ownership and main-thread checks.
        WorldLookMod? owner = WorldLookAutomationHost.Resolve(this);
        return owner == null
            ? "{\"schemaVersion\":1,\"accepted\":false,\"changed\":false,\"ready\":false,\"error\":\"endpoint-unavailable-or-wrong-thread\"}"
            : owner.AutomationRequest(action, enabled);
    }

    private void OnDestroy() => WorldLookAutomationHost.Detach(this);
}

/// <summary>Managed ownership stays outside the IL2CPP-injected component.</summary>
internal static class WorldLookAutomationHost
{
    internal const string ObjectName = "WorldLook.Automation";
    private static WorldLookMod? _owner;
    private static GameObject? _object;
    private static WorldLookAutomation? _component;
    private static int _threadId;

    internal static void Create(WorldLookMod owner)
    {
        if (_owner != null || NetworkGuard.IsAlive(GameObject.Find(ObjectName)))
            throw new InvalidOperationException("Automation endpoint name or ownership is already occupied.");
        GameObject? created = null;
        try
        {
            created = new GameObject(ObjectName);
            UnityEngine.Object.DontDestroyOnLoad(created);
            var component = created.AddComponent<WorldLookAutomation>();
            if (!NetworkGuard.IsAlive(component)) throw new InvalidOperationException("Automation component creation failed.");
            _threadId = Thread.CurrentThread.ManagedThreadId;
            _object = created;
            _component = component;
            _owner = owner;
        }
        catch
        {
            if (NetworkGuard.IsAlive(created)) UnityEngine.Object.Destroy(created!);
            throw;
        }
    }

    internal static WorldLookMod? Resolve(WorldLookAutomation component)
    {
        if (_owner == null || Thread.CurrentThread.ManagedThreadId != _threadId) return null;
        if (!NetworkGuard.IsAlive(_component) || !NetworkGuard.IsAlive(component)
            || component.Pointer != _component!.Pointer) return null;
        return _owner;
    }

    internal static void Detach(WorldLookAutomation component)
    {
        if (Thread.CurrentThread.ManagedThreadId != _threadId || _component == null
            || component.Pointer != _component.Pointer) return;
        _owner = null;
        _component = null;
        _object = null;
    }

    internal static void Dispose(WorldLookMod owner)
    {
        if (!ReferenceEquals(owner, _owner)) return;
        var owned = _object;
        _owner = null;
        _component = null;
        _object = null;
        if (NetworkGuard.IsAlive(owned)) UnityEngine.Object.Destroy(owned!);
    }
}
