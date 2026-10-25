using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using Il2CppScheduleOne.Networking;
using Il2CppScheduleOne.Persistence;
using Il2CppScheduleOne.PlayerScripts;
using MelonLoader;
using S1Mods.Shared;
using UnityEngine;
using UnityEngine.SceneManagement;

[assembly: MelonInfo(typeof(WorldLook.WorldLookMod), "WorldLook", "0.3.0-dev.5", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace WorldLook;

/// <summary>
/// One coordinator owns the mild native-light adjustment and clear water. The game
/// owns its sky, clouds and celestial bodies. F7 changes only this session's state.
/// </summary>
public sealed class WorldLookMod : MelonMod
{
    private readonly GoldenHour.Mod _lighting = new();
    private readonly WaterShader.WaterShaderMod _water = new();
    private readonly ModLogger _log = new("WorldLook");
    private WorldLookSettings _settings = new();
    private bool _sessionEnabled;
    private bool _lightingInitialized;
    private bool _waterInitialized;
    private bool _lightingFaulted;
    private bool _waterFaulted;
    private bool _inputErrorLogged;
    private bool _closed;
    private bool _menuRequested;
    private int _mainThreadId;

    public override void OnInitializeMelon()
    {
        _mainThreadId = Thread.CurrentThread.ManagedThreadId;
        try
        {
            string configPath = SafeStorage.GetUserDataPath("WorldLook", "WorldLook.json");
            _settings = WorldLookSettings.Load(() => File.ReadAllText(configPath),
                json => SafeStorage.SaveTextAtomic(configPath, json, _log), message => _log.Warn(message));
        }
        catch (Exception ex)
        {
            _settings = new WorldLookSettings { Enabled = false };
            _log.Warn("Profile unavailable; look disabled for this session", ex);
        }
        _sessionEnabled = _settings.Enabled;
        try
        {
            _lighting.OnInitializeMelon();
            _lightingInitialized = true;
            _lighting.SetEnabled(_sessionEnabled && _settings.LightingEnabled, _settings.Warmth);
        }
        catch (Exception ex) { LightingFault("initialization", ex); }
        _log.Info($"v{Info.Version} initialized. Ctrl+Shift+F7: look on/off (session only).");
    }

    public override void OnLateInitializeMelon()
    {
        if (_closed) return;
        try
        {
            _water.SetEnabled(_sessionEnabled && _settings.WaterEnabled);
            _water.OnLateInitializeMelon();
            _waterInitialized = true;
        }
        catch (Exception ex) { WaterFault("initialization", ex); }
        try { WorldLookAutomationHost.Create(this); }
        catch (Exception ex) { _log.Warn("Automation endpoint unavailable; normal look controls remain available", ex); }
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        if (sceneName == "Main") _menuRequested = false;
        if (_closed || !_waterInitialized) return;
        try { _water.OnSceneWasLoaded(buildIndex, sceneName); }
        catch (Exception ex) { WaterFault("scene load", ex); }
    }

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        if (_closed) return;
        try { _lighting.OnSceneWasUnloaded(buildIndex, sceneName); }
        catch (Exception ex) { LightingFault("scene unload", ex); }
        try { _water.OnSceneWasUnloaded(buildIndex, sceneName); }
        catch (Exception ex) { WaterFault("scene unload", ex); }
    }

    public override void OnUpdate()
    {
        if (_closed) return;
        HandleShortcuts();
        if (_lightingInitialized && !_lightingFaulted)
        {
            try { _lighting.OnUpdate(); }
            catch (Exception ex) { LightingFault("update", ex); }
        }
        if (_waterInitialized && !_waterFaulted)
        {
            try { _water.OnUpdate(); }
            catch (Exception ex) { WaterFault("update", ex); }
        }
    }

    public override void OnLateUpdate()
    {
        if (_closed || !_lightingInitialized || _lightingFaulted) return;
        try { _lighting.OnLateUpdate(); }
        catch (Exception ex) { LightingFault("late update", ex); }
    }

    private void HandleShortcuts()
    {
        try
        {
            if (!Application.isFocused || HotkeyManager.IsInputFieldFocused()) return;
            bool control = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            bool shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            if (!control || !shift) return;
            if (Input.GetKeyDown(KeyCode.F7) && NetworkGuard.IsInMainScene)
            {
                // A failed optional module must not remove the manual OFF path
                // for the other, functioning module.
                var context = ReadAutomationContext(requireInitializedModules: false);
                if (context.RefusalReason == null) SetSessionEnabled(!_sessionEnabled);
            }
        }
        catch (Exception ex)
        {
            if (_inputErrorLogged) return;
            _inputErrorLogged = true;
            _log.Warn("Shortcut handling unavailable", ex);
        }
    }

    // Explicit desired state makes retries idempotent. A fault is never cleared by
    // a same-state retry; OFF -> ON remains the deliberate recovery sequence.
    private bool SetSessionEnabled(bool enabled)
    {
        if (_sessionEnabled == enabled) return false;
        _sessionEnabled = enabled;
        if (_lightingInitialized)
        {
            try
            {
                _lighting.SetEnabled(enabled && _settings.LightingEnabled, _settings.Warmth);
                _lightingFaulted = false;
            }
            catch (Exception ex) { LightingFault("set state", ex); }
        }
        if (_waterInitialized)
        {
            try
            {
                _water.SetEnabled(enabled && _settings.WaterEnabled);
                _waterFaulted = false;
            }
            catch (Exception ex) { WaterFault("set state", ex); }
        }
        _log.Info("Look requested " + (enabled ? "ON" : "OFF") +
            $" (session only; lightingFault={_lightingFaulted}, waterFault={_waterFaulted}).");
        return true;
    }

    internal string AutomationRequest(string action, bool? enabled)
    {
        var context = ReadAutomationContext();
        bool accepted = false;
        bool changed = false;
        string? error = null;
        if (action == "status") accepted = context.RefusalReason != "wrong-thread" && !_closed;
        else if (context.RefusalReason != null) error = context.RefusalReason;
        else
        {
            try
            {
                if (enabled.HasValue)
                {
                    changed = SetSessionEnabled(enabled.Value);
                    accepted = true;
                }
                else if (action == "return-to-menu")
                {
                    // Latch before invoking the asynchronous native transition;
                    // an unknown/partial outcome must not cause duplicate exits.
                    _menuRequested = true;
                    context.Manager!.ExitToMenu(null, null, false);
                    accepted = true;
                    changed = true;
                }
                else error = "unknown-action";
            }
            catch (Exception ex) { error = ex.GetType().Name + ": " + ex.Message; }
        }
        context = ReadAutomationContext();
        return JsonSerializer.Serialize(new
        {
            schemaVersion = 1,
            action,
            accepted,
            changed,
            error,
            ready = context.RefusalReason == null,
            refusalReason = context.RefusalReason,
            requestedEnabled = _sessionEnabled,
            lightingInitialized = _lightingInitialized,
            waterInitialized = _waterInitialized,
            lightingFaulted = _lightingFaulted,
            waterFaulted = _waterFaulted,
            waterState = _water.AutomationState,
            referenceWaterReady = _water.ReferenceReady,
            scene = context.Scene,
            gameLoaded = context.GameLoaded,
            loading = context.Loading,
            saveName = context.SaveName,
            savePath = context.SavePath,
            hostOrSingleplayer = context.HostOrSingleplayer,
            singleplayer = context.Singleplayer,
            lobbyAvailable = context.LobbyAvailable,
            inLobby = context.InLobby,
            menuRequested = _menuRequested
        });
    }

    private AutomationContext ReadAutomationContext(bool requireInitializedModules = true)
    {
        var context = new AutomationContext();
        if (Thread.CurrentThread.ManagedThreadId != _mainThreadId) { context.RefusalReason = "wrong-thread"; return context; }
        if (_closed) { context.RefusalReason = "closed"; return context; }
        try
        {
            var scene = SceneManager.GetActiveScene();
            context.Scene = scene.name;
            if (!scene.IsValid() || !scene.isLoaded || scene.name != "Main")
                context.RefusalReason = "not-in-main";
            var manager = LoadManager.Instance;
            if (NetworkGuard.IsAlive(manager))
            {
                context.Manager = manager;
                context.GameLoaded = manager.IsGameLoaded;
                context.Loading = manager.IsLoading;
                if (context.GameLoaded && !context.Loading && context.RefusalReason == null)
                {
                    context.SavePath = manager.LoadedGameFolderPath ?? string.Empty;
                    context.SaveName = manager.ActiveSaveInfo?.OrganisationName ?? string.Empty;
                }
            }
            if (!context.GameLoaded || context.Loading || context.SavePath.Length == 0)
                context.RefusalReason ??= "world-not-ready";
            context.HostOrSingleplayer = NetworkGuard.IsHostOrSingleplayer();
            var lobby = Lobby.Instance;
            context.LobbyAvailable = NetworkGuard.IsAlive(lobby);
            context.InLobby = context.LobbyAvailable && lobby.IsInLobby;
            context.Singleplayer = context.HostOrSingleplayer && context.LobbyAvailable && !context.InLobby
                && Player.PlayerList != null && Player.PlayerList.Count == 1;
            if (!context.Singleplayer) context.RefusalReason ??= "singleplayer-required";
            if (requireInitializedModules && (!_lightingInitialized || !_waterInitialized)) context.RefusalReason ??= "modules-not-initialized";
            if (_menuRequested) context.RefusalReason ??= "menu-request-pending";
        }
        catch (Exception ex) { context.RefusalReason = "readiness-unavailable: " + ex.GetType().Name; }
        return context;
    }

    private sealed class AutomationContext
    {
        internal string? RefusalReason;
        internal string Scene = string.Empty;
        internal bool GameLoaded;
        internal bool Loading;
        internal bool HostOrSingleplayer;
        internal bool Singleplayer;
        internal bool LobbyAvailable;
        internal bool InLobby;
        internal string SaveName = string.Empty;
        internal string SavePath = string.Empty;
        internal LoadManager? Manager;
    }

    private void LightingFault(string phase, Exception ex)
    {
        if (!_lightingFaulted) _log.Warn("Lighting " + phase + " failed; module paused until an explicit toggle", ex);
        _lightingFaulted = true;
        try { _lighting.SetEnabled(false, _settings.Warmth); } catch { }
    }

    private void WaterFault(string phase, Exception ex)
    {
        if (!_waterFaulted) _log.Warn("Water " + phase + " failed; module paused until an explicit toggle", ex);
        _waterFaulted = true;
        try { _water.SetEnabled(false); } catch { }
    }

    private void Shutdown()
    {
        if (_closed) return;
        _closed = true;
        try { WorldLookAutomationHost.Dispose(this); } catch (Exception ex) { _log.Warn("Automation cleanup incomplete", ex); }
        try { _lighting.OnDeinitializeMelon(); } catch (Exception ex) { _log.Warn("Lighting cleanup incomplete", ex); }
        try { _water.OnDeinitializeMelon(); } catch (Exception ex) { _log.Warn("Water cleanup incomplete", ex); }
        try { ShaderAssets.Dispose(); } catch (Exception ex) { _log.Warn("Shader bundle cleanup incomplete", ex); }
    }

    public override void OnApplicationQuit() => Shutdown();
    public override void OnDeinitializeMelon() => Shutdown();
}
