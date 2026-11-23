namespace UnityEngine
{
    public static class Time { public static float unscaledTime; }
}
namespace S1API.Lifecycle
{
    public static class GameLifecycle
    {
        public static event Action? OnLoadComplete;
        public static event Action? OnPreLoad;
        public static event Action? OnPreSceneChange;
        public static void Loaded() => OnLoadComplete?.Invoke();
        public static void PreLoad() => OnPreLoad?.Invoke();
        public static void PreSceneChange() => OnPreSceneChange?.Invoke();
    }
}
namespace Il2CppScheduleOne.DevUtilities
{
    public static class Singleton<T> { public static T? Instance; }
}
namespace Il2CppScheduleOne.Networking
{
    public class Lobby : UnityEngine.Object { public bool IsInLobby; }
}
namespace Il2CppScheduleOne.PlayerScripts
{
    public class Player { public static List<Player>? PlayerList; }
}
namespace MelonLoader
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class MelonInfoAttribute : Attribute
    {
        public MelonInfoAttribute(Type type, string name, string version, string author) { }
    }
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class MelonGameAttribute : Attribute
    {
        public MelonGameAttribute(string developer, string game) { }
    }
    public class MelonMod
    {
        public virtual void OnInitializeMelon() { }
        public virtual void OnDeinitializeMelon() { }
        public virtual void OnUpdate() { }
        public virtual void OnSceneWasUnloaded(int index, string name) { }
    }
    public static class MelonLogger
    {
        public static readonly List<string> Lines = new();
        public static void Msg(string text) => Lines.Add(text);
        public static void Warning(string text) => Lines.Add(text);
        public static void Error(string text) => Lines.Add(text);
    }
}
