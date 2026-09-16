using System;
using System.IO;
using System.Text.Json;

namespace MelonLoader
{
    public static class MelonLogger
    {
        public static void Warning(string message) { }
        public static void Error(string message) { }
    }
}

namespace S1Mods.Shared
{
    public static class SaveSlots
    {
        public static int GetActiveSlotNumber() => 0;
    }

    public static class SafeStorage
    {
        public static string GetUserDataPath(string modName, string fileName)
            => Path.Combine(Path.GetTempPath(), modName, fileName);

        public static T? LoadSafe<T>(string filePath, T fallback, JsonSerializerOptions? options = null)
            => fallback;

        public static void SaveAtomic<T>(string filePath, T data, JsonSerializerOptions? options = null)
        {
        }
    }
}
