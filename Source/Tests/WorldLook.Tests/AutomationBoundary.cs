// Narrow fake native boundary: these tests execute the real coordinator and
// adapter, but do not claim IL2CPP signature or gameplay verification.
using System;
using System.Collections.Generic;

namespace Il2CppInterop.Runtime.Attributes { public sealed class HideFromIl2CppAttribute : Attribute { } }

namespace UnityEngine
{
    public class Object
    {
        private static int _next;
        public IntPtr Pointer { get; protected set; } = new(++_next);
        public bool Alive { get; set; } = true;
        public static void DontDestroyOnLoad(Object value) { }
        public static void Destroy(Object value)
        {
            value.Alive = false;
            if (value is GameObject obj) GameObject.Objects.Remove(obj.Name);
        }
    }
    public class MonoBehaviour : Object { public MonoBehaviour(IntPtr pointer) { Pointer = pointer; } }
    public sealed class GameObject : Object
    {
        internal static readonly Dictionary<string, GameObject> Objects = new();
        public string Name { get; }
        private object? _component;
        public GameObject(string name) { Name = name; Objects.Add(name, this); }
        public static GameObject? Find(string name) => Objects.GetValueOrDefault(name);
        public T AddComponent<T>()
        {
            _component = Activator.CreateInstance(typeof(T), new IntPtr(Pointer.ToInt64() + 10000));
            return (T)_component!;
        }
        public T GetComponent<T>() => (T)_component!;
    }
    public static class Application { public static bool isFocused = true; }
    public enum KeyCode { LeftControl, RightControl, LeftShift, RightShift, F7 }
    public static class Input
    {
        public static bool Shortcut;
        public static bool GetKey(KeyCode key) => Shortcut;
        public static bool GetKeyDown(KeyCode key) => Shortcut;
    }
}

namespace UnityEngine.SceneManagement
{
    public readonly record struct Scene(string name, bool isLoaded = true) { public bool IsValid() => name.Length > 0; }
    public static class SceneManager
    {
        public static Scene Active = new("Main");
        public static int Reads;
        public static Scene GetActiveScene() { Reads++; return Active; }
    }
}

namespace MelonLoader
{
    public sealed class RegisterTypeInIl2CppAttribute : Attribute { }
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class MelonInfoAttribute : Attribute { public MelonInfoAttribute(Type type, string name, string version, string author) { } }
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class MelonGameAttribute : Attribute { public MelonGameAttribute(string company, string game) { } }
    public class MelonMod
    {
        public ModInfo Info { get; } = new();
        public virtual void OnInitializeMelon() { }
        public virtual void OnLateInitializeMelon() { }
        public virtual void OnSceneWasLoaded(int index, string name) { }
        public virtual void OnSceneWasUnloaded(int index, string name) { }
        public virtual void OnUpdate() { }
        public virtual void OnLateUpdate() { }
        public virtual void OnApplicationQuit() { }
        public virtual void OnDeinitializeMelon() { }
    }
    public sealed class ModInfo { public string Version => "test"; }
}

namespace S1Mods.Shared
{
    public sealed class ModLogger
    {
        public ModLogger(string name) { }
        public void Info(string message) { }
        public void Warn(string message) { }
        public void Warn(string message, Exception ex) { }
    }
    public static class SafeStorage
    {
        public static int Writes;
        public static string GetUserDataPath(string mod, string file) => System.IO.Path.Combine(System.IO.Path.GetTempPath(), "worldlook-test-" + Guid.NewGuid(), file);
        public static bool SaveTextAtomic(string path, string text, ModLogger log) { Writes++; return true; }
    }
    public static class HotkeyManager { public static bool IsInputFieldFocused() => false; }
    public static class NetworkGuard
    {
        public static bool Host = true;
        public static bool IsHostOrSingleplayer() => Host;
        public static bool IsInMainScene => UnityEngine.SceneManagement.SceneManager.Active.name == "Main";
        public static bool IsAlive(UnityEngine.Object? value) => value?.Alive == true;
    }
}

namespace Il2CppScheduleOne.Persistence
{
    public sealed class SaveInfo { public string OrganisationName { get; set; } = "Dominik"; }
    public sealed class LoadManager : UnityEngine.Object
    {
        public static LoadManager Instance { get; set; } = new();
        public bool IsGameLoaded { get; set; } = true;
        public bool IsLoading { get; set; }
        public string LoadedGameFolderPath { get; set; } = "SaveGame_1";
        public SaveInfo ActiveSaveInfo { get; set; } = new();
        public int MenuCalls;
        public bool ThrowMenu;
        public void ExitToMenu(SaveInfo? save, object? popup, bool leave)
        {
            MenuCalls++;
            if (ThrowMenu) throw new InvalidOperationException("unknown transition outcome");
        }
    }
}
namespace Il2CppScheduleOne.PlayerScripts { public static class Player { public static List<object> PlayerList = new() { new object() }; } }
namespace Il2CppScheduleOne.Networking
{
    public sealed class Lobby : UnityEngine.Object
    {
        public static Lobby Instance { get; set; } = new();
        public bool IsInLobby { get; set; }
    }
}

namespace GoldenHour
{
    public sealed class Mod
    {
        public static Mod Latest = null!;
        public int SetCalls;
        public bool Enabled;
        public bool ThrowUpdate;
        public Mod() { Latest = this; }
        public void OnInitializeMelon() { }
        public void OnDeinitializeMelon() { }
        public void OnUpdate() { if (ThrowUpdate) { ThrowUpdate = false; throw new InvalidOperationException("lighting failed"); } }
        public void OnLateUpdate() { }
        public void OnSceneWasUnloaded(int index, string scene) { }
        public void SetEnabled(bool enabled, double warmth) { SetCalls++; Enabled = enabled; }
    }
}
namespace WaterShader
{
    public sealed class WaterShaderMod
    {
        public static WaterShaderMod Latest = null!;
        public static bool ThrowInitialize;
        public int SetCalls;
        public bool Enabled;
        public WaterShaderMod() { Latest = this; }
        public string AutomationState => Enabled ? "Applied" : "Stopped";
        public bool ReferenceReady => Enabled;
        public void SetEnabled(bool enabled) { SetCalls++; Enabled = enabled; }
        public void OnLateInitializeMelon() { if (ThrowInitialize) throw new InvalidOperationException("water initialization failed"); }
        public void OnSceneWasLoaded(int index, string scene) { }
        public void OnSceneWasUnloaded(int index, string scene) { }
        public void OnDeinitializeMelon() { }
        public void OnUpdate() { }
    }
}
namespace WorldLook { internal static class ShaderAssets { internal static void Dispose() { } } }
