// Deliberately narrow fake native boundary for executing the real Mod/SceneTargets code.
// This proves managed decisions and callback wiring, NOT IL2CPP signatures or gameplay.
namespace GoldenHour.Tests
{
    internal sealed class NativeColor
    {
        public UnityEngine.Color Value = new(0.6f, 0.7f, 0.8f, 1f);
        public int Reads;
        public int Writes;
        public int ThrowAfterWrites;
        public bool ThrowBeforeWrite;
        public bool IgnoreWrite;
        public UnityEngine.Color Read()
        {
            Reads++;
            return Value;
        }
        public void Write(UnityEngine.Color value)
        {
            Writes++;
            if (ThrowBeforeWrite) throw new InvalidOperationException("injected setter failure");
            if (!IgnoreWrite) Value = value;
            if (ThrowAfterWrites > 0)
            {
                ThrowAfterWrites--;
                throw new InvalidOperationException("injected post-mutation failure");
            }
        }
    }
}

namespace UnityEngine
{
    public class Object
    {
        private static int _next;
        public IntPtr Pointer { get; } = new(++_next);
        public bool Alive { get; set; } = true;
    }

    public readonly record struct Color(float r, float g, float b, float a);

    public sealed class Material : Object
    {
        internal GoldenHour.Tests.NativeColor Slot { get; } = new();
        public bool HasSunShine { get; set; } = true;
        public bool HasProperty(string name) => HasSunShine && name == "_SunShine";
        public Color GetColor(string name)
        {
            if (!HasProperty(name)) throw new InvalidOperationException("unknown material property");
            return Slot.Read();
        }
        public void SetColor(string name, Color value)
        {
            if (!HasProperty(name)) throw new InvalidOperationException("unknown material property");
            Slot.Write(value);
        }
    }

    public sealed class Light : Object
    {
        internal GoldenHour.Tests.NativeColor Slot { get; } = new();
        public Color color { get => Slot.Read(); set => Slot.Write(value); }
    }

    public static class Time
    {
        public static float unscaledTime { get; set; }
    }

    public static class RenderSettings
    {
        public static Material? skybox { get; set; }
        public static Light? sun { get; set; }
        public static Rendering.AmbientMode ambientMode { get; set; }
        internal static GoldenHour.Tests.NativeColor Sky = new();
        internal static GoldenHour.Tests.NativeColor Equator = new();
        internal static GoldenHour.Tests.NativeColor Ground = new();
        public static Color ambientSkyColor { get => Sky.Read(); set => Sky.Write(value); }
        public static Color ambientEquatorColor { get => Equator.Read(); set => Equator.Write(value); }
        public static Color ambientGroundColor { get => Ground.Read(); set => Ground.Write(value); }
    }
}

namespace UnityEngine.Rendering
{
    public enum AmbientMode { Skybox, Trilight, Flat }
}

namespace UnityEngine.SceneManagement
{
    public readonly record struct Scene(int handle, string name, bool isLoaded = true)
    {
        public bool IsValid() => handle != 0;
    }

    public static class SceneManager
    {
        public static Scene Active { get; set; }
        public static Scene GetActiveScene() => Active;
    }
}

namespace Il2CppScheduleOne.GameTime
{
    public sealed class TimeManager : UnityEngine.Object
    {
        public int RawHhmm { get; set; } = 1800;
        public bool ThrowOnRead { get; set; }
        public int CurrentTime => ThrowOnRead ? throw new InvalidOperationException("unreadable clock") : RawHhmm;
    }
}

namespace Il2CppScheduleOne.DevUtilities
{
    public static class NetworkSingleton<T> where T : class
    {
        public static T? Instance { get; set; }
    }
}

namespace S1API.Lifecycle
{
    public static class GameLifecycle
    {
        public static event Action? OnPreLoad;
        public static event Action? OnPreSceneChange;
        public static event Action? OnLoadComplete;
        public static int Subscribers => (OnPreLoad?.GetInvocationList().Length ?? 0)
            + (OnPreSceneChange?.GetInvocationList().Length ?? 0) + (OnLoadComplete?.GetInvocationList().Length ?? 0);
        public static void PreLoad() => OnPreLoad?.Invoke();
        public static void PreSceneChange() => OnPreSceneChange?.Invoke();
        public static void LoadComplete() => OnLoadComplete?.Invoke();
    }
}

namespace S1Mods.Shared
{
    public static class NetworkGuard
    {
        public static bool IsAlive(UnityEngine.Object? target) => target?.Alive == true;
    }

    public sealed class ModLogger
    {
        public ModLogger(string name) { }
        public void Info(string text) { }
        public void Warn(string text) { }
        public void Warn(string text, Exception ex) { }
    }
}
