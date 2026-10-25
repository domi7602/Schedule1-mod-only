// Native-boundary doubles for the linked production ShaderAssets cache. They model observable
// native destruction separately from managed wrapper lifetime; they do not execute Unity or GPUs.
namespace UnityEngine
{
    [Flags]
    public enum HideFlags
    {
        None = 0,
        HideInHierarchy = 1,
        HideInInspector = 2,
        DontSaveInEditor = 4,
        DontUnloadUnusedAsset = 32
    }

    public class Object
    {
        public bool WasCollected { get; set; }
        public IntPtr Pointer { get; set; } = new(1);
        public bool NativeAlive { get; set; } = true;
    }

    public sealed class Shader : Object
    {
        private HideFlags _flags;
        public bool Supported { get; set; } = true;
        public bool ThrowOnSupportRead { get; set; }
        public bool IgnoreFlagWrite { get; set; }
        public bool isSupported => ThrowOnSupportRead ? throw new InvalidOperationException("native shader inaccessible") : Supported;
        public HideFlags hideFlags
        {
            get => _flags;
            set { if (!IgnoreFlagWrite) _flags = value; }
        }

        public void SimulateUnusedAssetCleanup()
        {
            if ((_flags & HideFlags.DontUnloadUnusedAsset) == 0) NativeAlive = false;
        }
    }

    public sealed class AssetBundle : Object
    {
        public static Func<AssetBundle?> MemoryLoader { get; set; } = () => null;
        public static Func<AssetBundle?> FileLoader { get; set; } = () => null;
        public static int MemoryLoads { get; set; }
        public static int FileLoads { get; set; }
        public Func<string, Shader?> AssetLoader { get; set; } = _ => null;
        public int AssetLoads { get; private set; }
        public List<bool> Unloads { get; } = new();

        public static AssetBundle? LoadFromMemory(byte[] bytes)
        {
            if (bytes.Length < 16) throw new InvalidDataException("test payload missing");
            MemoryLoads++;
            return MemoryLoader();
        }

        public static AssetBundle? LoadFromFile(string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException("cache not published", path);
            FileLoads++;
            return FileLoader();
        }

        public T? LoadAsset<T>(string path) where T : Object
        {
            AssetLoads++;
            return AssetLoader(path) as T;
        }

        public void Unload(bool unloadAllLoadedObjects) { Unloads.Add(unloadAllLoadedObjects); }
    }
}

namespace WaterShader
{
    // Mirrors the existing WaterEnvironment optional probe at the native boundary. Cache tests
    // select each failure independently instead of treating a non-null wrapper as a live asset.
    internal static class WaterEnvironment
    {
        internal static bool IsUsable(UnityEngine.Object? value)
            => value != null && !value.WasCollected && value.Pointer != IntPtr.Zero && value.NativeAlive;
    }
}

namespace S1Mods.Shared
{
    internal static class SafeStorage
    {
        internal static string CacheDirectory { get; } = Path.Combine(Path.GetTempPath(), "WorldLook-cache-tests-" + Guid.NewGuid().ToString("N"));
        internal static string GetUserDataPath(string mod, string child) => CacheDirectory;
    }
}
