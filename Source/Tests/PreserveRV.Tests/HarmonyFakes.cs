using System.Reflection;

namespace HarmonyLib
{
    public sealed class HarmonyMethod
    {
        public readonly MethodInfo method;
        public HarmonyMethod(Type type, string name) => method = type.GetMethod(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)!;
    }
    public sealed class Patch { public string owner = ""; public MethodInfo PatchMethod = null!; }
    public sealed class Patches
    {
        public List<Patch> Prefixes = new();
        public IEnumerable<string> Owners => Prefixes.Select(p => p.owner).Distinct();
    }
    public sealed class Harmony(string id)
    {
        public static Dictionary<MethodBase, Patches> Registry = new();
        public static string? MissingMetadataFor;
        public static bool ThrowOnRemove;
        public string Id => id;
        public static Patches? GetPatchInfo(MethodBase method) => Registry.GetValueOrDefault(method);
        public void Patch(MethodBase original, HarmonyMethod prefix)
        {
            if (original.Name == MissingMetadataFor) return;
            if (!Registry.ContainsKey(original)) Registry[original] = new();
            Registry[original].Prefixes.Add(new() { owner = id, PatchMethod = prefix.method });
        }
        public void UnpatchSelf()
        {
            if (ThrowOnRemove) throw new InvalidOperationException("remove failed");
            foreach (var metadata in Registry.Values) metadata.Prefixes.RemoveAll(p => p.owner == id);
        }
        public static bool RunPrefix(string method, Il2CppScheduleOne.Quests.Quest_WelcomeToHylandPoint quest)
        {
            var target = typeof(Il2CppScheduleOne.Quests.Quest_WelcomeToHylandPoint).GetMethod(method)!;
            var prefix = Registry[target].Prefixes.Single(p => p.owner == "Dominik.PreserveRV");
            return (bool)prefix.PatchMethod.Invoke(null, new object[] { quest })!;
        }
    }
}
namespace S1Mods.Shared
{
    public static class PatchGuard
    {
        public static int Calls;
        public static bool Refuse;
        public static bool TryPatch(HarmonyLib.Harmony harmony, MethodBase original, HarmonyLib.HarmonyMethod prefix)
        {
            Calls++;
            if (Refuse) return false;
            harmony.Patch(original, prefix); return true;
        }
    }
}
