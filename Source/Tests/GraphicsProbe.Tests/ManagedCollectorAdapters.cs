// TEST ONLY: managed boundary stand-ins for the linked production RenderingSnapshot.
// These model only the collector's required API surface, not Unity/IL2CPP execution,
// fake-null/lifetime behavior, native timings, or the real ObjectReader's field reads.
namespace GraphicsProbe.Tests.Adapters
{
    internal static class ManagedNativeReads
    {
        // Counts only instrumented adapter reads; never a measurement of real native calls.
        public static int Count { get; private set; }
        public static void Record() => Count++;
        public static void Reset()
        {
            Count = 0;
            UnityEngine.Camera.MainGetter = () => null;
            UnityEngine.SceneManagement.SceneManager.Scenes = Array.Empty<UnityEngine.SceneManagement.Scene>();
            UnityEngine.SceneManagement.SceneManager.CountGetter = () => UnityEngine.SceneManagement.SceneManager.Scenes.Length;
        }
    }
}

namespace Il2CppInterop.Runtime.InteropTypes.Arrays
{
    internal class Il2CppArrayBase<T>
    {
        private readonly T[] _items;
        public Func<int>? LengthGetter { get; init; }
        public Func<int, T>? ItemGetter { get; init; }
        public int LengthReads { get; private set; }
        public int IndexReads { get; private set; }
        public Il2CppArrayBase(params T[] items) => _items = items;
        public int Length
        {
            get
            {
                GraphicsProbe.Tests.Adapters.ManagedNativeReads.Record();
                LengthReads++;
                return LengthGetter == null ? _items.Length : LengthGetter();
            }
        }
        public T this[int index]
        {
            get
            {
                GraphicsProbe.Tests.Adapters.ManagedNativeReads.Record();
                IndexReads++;
                return ItemGetter == null ? _items[index] : ItemGetter(index);
            }
        }
    }

    internal sealed class Il2CppReferenceArray<T> : Il2CppArrayBase<T>
    {
        public Il2CppReferenceArray(params T[] items) : base(items) { }
    }
}

namespace UnityEngine
{
    using GraphicsProbe.Tests.Adapters;
    using Il2CppInterop.Runtime.InteropTypes.Arrays;
    using UnityEngine.SceneManagement;

    internal class Component
    {
        public int InstanceId { get; init; }
        public int GetInstanceID() { ManagedNativeReads.Record(); return InstanceId; }
        public T? TryCast<T>() where T : Component => this as T;
    }

    internal sealed class Transform : Component
    {
        public GameObject gameObject { get; init; } = null!;
        public int childCount => 0;
        public Transform GetChild(int index) => throw new ArgumentOutOfRangeException(nameof(index));
    }

    internal sealed class GameObject
    {
        public Scene scene { get; init; } = null!;
        public Transform transform { get; }
        public Func<Il2CppArrayBase<Component>> ComponentsGetter { get; init; } = () => new();
        public GameObject(int id) => transform = new Transform { InstanceId = id, gameObject = this };
        public Il2CppArrayBase<T> GetComponents<T>() where T : Component
        {
            ManagedNativeReads.Record();
            return (Il2CppArrayBase<T>)(object)ComponentsGetter();
        }
    }

    internal sealed class Camera : Component
    {
        public static Func<Camera?> MainGetter { get; set; } = () => null;
        public static Camera? main { get { ManagedNativeReads.Record(); return MainGetter(); } }
    }
    internal class Renderer : Component { }
    internal sealed class Light : Component { }
    internal sealed class ReflectionProbe : Component { }
    internal sealed class LightProbeGroup : Component { }
}

namespace UnityEngine.Rendering
{
    internal sealed class Volume : UnityEngine.Component { }
}

namespace UnityEngine.SceneManagement
{
    using GraphicsProbe.Tests.Adapters;
    using Il2CppInterop.Runtime.InteropTypes.Arrays;

    internal sealed class Scene
    {
        public string name => "managed test scene";
        public int handle => 1;
        public int buildIndex => 0;
        public bool isLoaded => true;
        public bool IsValid() => true;
        public Il2CppReferenceArray<GameObject> Roots { get; set; } = new();
        public Il2CppReferenceArray<GameObject> GetRootGameObjects() { ManagedNativeReads.Record(); return Roots; }
    }

    internal static class SceneManager
    {
        public static Scene[] Scenes { get; set; } = Array.Empty<Scene>();
        public static Func<int> CountGetter { get; set; } = () => Scenes.Length;
        public static int sceneCount { get { ManagedNativeReads.Record(); return CountGetter(); } }
        public static Scene GetSceneAt(int index) { ManagedNativeReads.Record(); return Scenes[index]; }
    }
}

namespace GraphicsProbe
{
    using UnityEngine;
    using UnityEngine.Rendering;
    using GraphicsProbe.Tests.Adapters;

    internal static class EnvironmentReader
    {
        public static ObservedRecord Settings() { ManagedNativeReads.Record(); return new(); }
        public static ObservedRecord Environment() { ManagedNativeReads.Record(); return new(); }
    }

    internal static class ObjectReader
    {
        public static ObservedRecord CameraDetails(Camera camera, string source)
        {
            ManagedNativeReads.Record();
            return new() { ["instanceId"] = camera.GetInstanceID(), ["discoverySource"] = source };
        }
        // Unexercised reader branches fail loudly rather than returning invented observations.
        public static ObservedRecord LightDetails(Light light) => throw new NotSupportedException();
        public static ObservedRecord ProbeDetails(Component probe) => throw new NotSupportedException();
        public static ObservedRecord VolumeDetails(Volume volume) => throw new NotSupportedException();
        public static string NativeType(Renderer renderer) => throw new NotSupportedException();
        public static RendererClues ScanRenderer(Renderer renderer) => throw new NotSupportedException();
        public static ObservedRecord WaterCandidate(Renderer renderer, RendererClues clues, bool effect) => throw new NotSupportedException();
        internal sealed class RendererClues
        {
            public bool Unreadable => throw new NotSupportedException();
            public List<string> Evidence => throw new NotSupportedException();
        }
    }
}
