// Minimal managed boundary doubles, NOT a Unity/IL2CPP runtime emulation.
namespace UnityEngine
{
    public class Object { public bool Alive = true; }
    public class GameObject : Object
    {
        public bool activeSelf = true;
        public int ActiveWrites;
        public void SetActive(bool value) { activeSelf = value; ActiveWrites++; }
    }
    public class Transform : Object
    {
        public string name;
        public GameObject gameObject = new();
        public Transform? parent;
        public List<Transform> Children = new();
        public int ParentWrites;
        public bool? LastWorldPositionStays;
        public string WorldPose = "original position/rotation/scale";
        public Transform(string name) { this.name = name; }
        public int childCount => Children.Count;
        public Transform GetChild(int i) => Children[i];
        public int GetInstanceID() => System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(this);
        public void SetParent(Transform root, bool worldPositionStays)
        {
            parent?.Children.Remove(this); parent = root; root.Children.Add(this);
            ParentWrites++; LastWorldPositionStays = worldPositionStays;
            if (!worldPositionStays) WorldPose = "CHANGED";
        }
        public Transform Add(string name, bool active = true)
        {
            var child = new Transform(name); child.gameObject.activeSelf = active;
            child.SetParent(this, true); child.ParentWrites = 0; return child;
        }
    }
}
namespace Il2CppScheduleOne.Property
{
    public class RV : UnityEngine.Object
    {
        public UnityEngine.Transform transform = new("RV property");
        public bool IsDestroyed;
        public bool _exploded;
    }
}
namespace Il2CppScheduleOne.Quests
{
    public enum EQuestState { Inactive, Active, Completed, Failed, Expired, Cancelled }
    public class QuestEntry : UnityEngine.Object
    {
        public string EntryTitle = "";
        public EQuestState State;
    }
    public class Quest : UnityEngine.Object
    {
        public static List<Quest> Quests = new();
        public T? TryCast<T>() where T : class => this as T;
    }
    public class Quest_WelcomeToHylandPoint : Quest
    {
        public void BlowupRV() { }
        public void SetRVDestroyed() { }
        public Il2CppScheduleOne.Property.RV RV = new();
        public List<QuestEntry> Entries = new();
    }
}
namespace S1Mods.Shared
{
    public static class NetworkGuard
    {
        public static bool Main = true, Host = true, Throw;
        public static bool IsInMainScene => Throw ? throw new InvalidOperationException("unknown scene") : Main;
        public static bool IsHostOrSingleplayer() => Host;
        public static bool IsAlive(UnityEngine.Object? obj) => obj != null && obj.Alive;
    }
}
