extern alias il2cpp;

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppScheduleOne;
using Il2CppScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using NativeObject = il2cpp::Il2CppSystem.Object;
using NativeAction = il2cpp::Il2CppSystem.Action;

namespace TownHallInterior;

// Main-thread only. The caller must call NativeDoorModalHost.Tick on subsequent
// updates and Shutdown before invalidating the scene generation.
public sealed class NativeContextMenuRuntime : IDoorModalRuntime
{
    private readonly IExclusiveModalFocus _focus;
    private readonly Func<int> _generation;
    private readonly int _sceneGeneration, _sceneHandle;
    private UIScreenManager? _manager;
    private PlayerMovement? _player;
    private UIPopupScreen_ContextMenu? _popup;
    private int _managerId, _playerId, _popupId;
    private int[]? _before;
    private bool _mouseAttempted, _movementAttempted, _openAttempted, _identifiedOwned;
    private NativeAction[]? _actions;
    private Il2CppReferenceArray<UIPopupScreen_ContextMenu.ContextMenuOption>? _options;
    private Il2CppReferenceArray<NativeObject>? _args;
    // Native cached screens may retain old listeners after close. Keep every
    // marshalled delegate/payload rooted for this scene lifetime; cap the count.
    private readonly List<object> _nativeRoots = new();
    private const int MaximumOpenAttempts = 128;

    public NativeContextMenuRuntime(IExclusiveModalFocus focus, Func<int> generation)
    {
        _focus = focus ?? throw new ArgumentNullException(nameof(focus));
        _generation = generation ?? throw new ArgumentNullException(nameof(generation));
        _sceneGeneration = generation();
        _sceneHandle = SceneManager.GetActiveScene().handle;
    }

    public bool IsCurrent => _generation() == _sceneGeneration &&
        SceneManager.GetActiveScene().handle == _sceneHandle && SceneManager.GetActiveScene().name == "Main";
    public bool IsExclusive => IsCurrent && _focus.IsHeld;

    private bool ValidOwners()
    {
        if (!IsExclusive || _manager == null || _player == null) return false;
        var managers = Resources.FindObjectsOfTypeAll<UIScreenManager>();
        var players = UnityEngine.Object.FindObjectsOfType<PlayerMovement>();
        return managers.Length == 1 && players.Length == 1 && managers[0] != null && players[0] != null &&
            managers[0].Pointer == _manager.Pointer && players[0].Pointer == _player.Pointer &&
            _manager.GetInstanceID() == _managerId && _player.GetInstanceID() == _playerId &&
            _manager.gameObject.activeInHierarchy && _manager.enabled;
    }

    private static int[] Stack(UIScreenManager manager)
    {
        var ids = new List<int>();
        foreach (var entry in manager.screenStack) ids.Add(entry.screen == null ? 0 : entry.screen.GetInstanceID());
        return ids.ToArray();
    }
    private static bool Same(int[] a, int[] b)
    {
        if (a.Length != b.Length) return false;
        for (int i = 0; i < a.Length; i++) if (a[i] != b[i]) return false;
        return true;
    }
    private bool BaselineStack() => ValidOwners() && _before != null && !_manager!.IsAnyPopupScreenActive() && Same(_before, Stack(_manager));
    private bool OwnedTop()
    {
        if (!ValidOwners() || _before == null || _popup == null) return false;
        var top = _manager!.TopScreen;
        return _manager.IsAnyPopupScreenActive() && top != null && _popup.GetInstanceID() == _popupId &&
            top.Pointer == _popup.Pointer && ModalOwnership.OwnsTop(_before, Stack(_manager), _managerId,
                _manager.GetInstanceID(), _popupId, top.GetInstanceID(), _popup.GetInstanceID());
    }
    private bool ReconcilePopup()
    {
        if (!ValidOwners()) return false;
        int count = 0;
        UIPopupScreen_ContextMenu? found = null;
        foreach (var item in _manager!.popupScreenInstances)
            if (item != null && item.popupID == "ContextMenu") { count++; found = item.TryCast<UIPopupScreen_ContextMenu>(); }
        if (count != 1 || found == null || !found.gameObject.scene.IsValid() || !found.gameObject.scene.isLoaded) return false;
        _popup = found;
        _popupId = found.GetInstanceID();
        return OwnedTop();
    }
    private static NativeObject BoxVector(Vector2 value)
    {
        var type = Il2CppClassPointerStore<Vector2>.NativeClassPtr;
        if (type == IntPtr.Zero || Marshal.PtrToStringAnsi(IL2CPP.il2cpp_class_get_name(type)) != "Vector2" ||
            Marshal.PtrToStringAnsi(IL2CPP.il2cpp_class_get_namespace(type)) != "UnityEngine") throw new InvalidOperationException("Unexpected Vector2 class");
        var memory = Marshal.AllocHGlobal(Marshal.SizeOf<Vector2>());
        try
        {
            Marshal.StructureToPtr(value, memory, false);
            var boxed = IL2CPP.il2cpp_value_box(type, memory);
            if (boxed == IntPtr.Zero) throw new InvalidOperationException("Vector2 boxing failed");
            return new NativeObject(boxed);
        }
        finally { Marshal.FreeHGlobal(memory); }
    }

    public bool TryOpen(string[] options, Action<int?> selected)
    {
        if (!IsExclusive || options.Length != 3 || _mouseAttempted || _movementAttempted || _openAttempted ||
            _nativeRoots.Count >= MaximumOpenAttempts * 3) return false;
        var managers = Resources.FindObjectsOfTypeAll<UIScreenManager>();
        var players = UnityEngine.Object.FindObjectsOfType<PlayerMovement>();
        if (managers.Length != 1 || players.Length != 1 || managers[0] == null || players[0] == null) return false;
        _manager = managers[0]; _player = players[0];
        _managerId = _manager.GetInstanceID(); _playerId = _player.GetInstanceID();
        if (!ValidOwners() || _manager.transform.name != "@UIScreenManager" || _manager.transform.parent != null ||
            _manager.IsAnyPopupScreenActive() || _manager.TopScreen == null || _manager.TopScreen.name != "FilterPanel" ||
            !_player.CanMove || Cursor.lockState != CursorLockMode.Locked || Cursor.visible || MouseController.IsMouseVisible || GameInput.IsTyping) return false;
        _before = Stack(_manager);
        if (_before.Length == 0 || _before[0] != _manager.TopScreen.GetInstanceID()) return false;
        int prefabs = 0;
        foreach (var p in _manager.popupScreenPrefabs)
            if (p != null && p.popupID == "ContextMenu" && p.TryCast<UIPopupScreen_ContextMenu>() != null) prefabs++;
        if (prefabs != 1) return false;
        _actions = new NativeAction[3];
        _options = new Il2CppReferenceArray<UIPopupScreen_ContextMenu.ContextMenuOption>(3);
        for (int i = 0; i < 3; i++)
        {
            int index = i;
            _actions[i] = DelegateSupport.ConvertDelegate<NativeAction>(new Action(() => selected(index)))
                ?? throw new InvalidOperationException("Native action conversion failed");
            _options[i] = new UIPopupScreen_ContextMenu.ContextMenuOption(9001 + i, options[i], _actions[i]);
        }
        _args = new Il2CppReferenceArray<NativeObject>(2);
        _args[0] = new NativeObject(_options.Pointer);
        _args[1] = BoxVector(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f));
        _nativeRoots.Add(_actions); _nativeRoots.Add(_options); _nativeRoots.Add(_args);
        if (!BaselineStack() || !_player.CanMove || GameInput.IsTyping || Cursor.lockState != CursorLockMode.Locked || Cursor.visible || MouseController.IsMouseVisible) return false;
        _mouseAttempted = true;
        MouseController.FreeMouse(true);
        if (!BaselineStack() || !_player.CanMove || GameInput.IsTyping || Cursor.lockState != CursorLockMode.None || !Cursor.visible || !MouseController.IsMouseVisible) return false;
        _openAttempted = true;
        _manager.OpenPopupScreen("ContextMenu", _args);
        if (!ReconcilePopup()) return false;
        _identifiedOwned = true;
        if (!_popup!.gameObject.activeInHierarchy || !_player.CanMove || GameInput.IsTyping) return false;
        _movementAttempted = true;
        _player.CanMove = false;
        return OwnedTop() && !_player.CanMove && !GameInput.IsTyping && Cursor.lockState == CursorLockMode.None && Cursor.visible && MouseController.IsMouseVisible;
    }

    public bool IsClosed => BaselineStack();

    private static bool RefuseRelease(bool mutated)
    {
        if (mutated) throw new InvalidOperationException("Native cleanup outcome is uncertain; retry disabled");
        return false;
    }

    public bool TryRelease(bool closeOwnedPopup)
    {
        bool mutated = false;
        if (!IsExclusive) return false;
        if (!_mouseAttempted && !_movementAttempted && !_openAttempted) return true;
        if (!ValidOwners() || _before == null) return false;
        if (_manager!.IsAnyPopupScreenActive())
        {
            if (!closeOwnedPopup || !_openAttempted || !_identifiedOwned || !OwnedTop()) return false;
            mutated = true;
            _manager.ClosePopupScreen("ContextMenu");
        }
        if (!BaselineStack() || GameInput.IsTyping) return RefuseRelease(mutated);
        if (_mouseAttempted)
        {
            if (Cursor.lockState == CursorLockMode.None && Cursor.visible && MouseController.IsMouseVisible)
            {
                if (!BaselineStack() || GameInput.IsTyping) return RefuseRelease(mutated);
                mutated = true;
                MouseController.LockMouse(true);
            }
            if (!BaselineStack() || Cursor.lockState != CursorLockMode.Locked || Cursor.visible || MouseController.IsMouseVisible || GameInput.IsTyping) return RefuseRelease(mutated);
            _mouseAttempted = false;
        }
        if (_movementAttempted)
        {
            if (!_player!.CanMove)
            {
                if (!BaselineStack() || GameInput.IsTyping || Cursor.lockState != CursorLockMode.Locked || Cursor.visible) return RefuseRelease(mutated);
                mutated = true;
                _player.CanMove = true;
            }
            if (!BaselineStack() || !_player.CanMove) return RefuseRelease(mutated);
            _movementAttempted = false;
        }
        _openAttempted = false; _identifiedOwned = false;
        _popup = null; _popupId = 0; _args = null; _options = null; _actions = null;
        _before = null; _manager = null; _player = null; _managerId = 0; _playerId = 0;
        return true;
    }
}
