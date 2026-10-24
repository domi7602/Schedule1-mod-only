// Only loader logging/path and the native slot probe are replaced. SafeStorage and the
// LightPersistence adapter are linked verbatim from production and perform real fixture I/O.
namespace MelonLoader.Utils
{
    public static class MelonEnvironment
    {
        public static string UserDataDirectory { get; set; } = string.Empty;
    }
}
namespace MelonLoader
{
    public static class MelonLogger
    {
        public static void Msg(string message) { }
        public static void Warning(string message) { }
        public static void Error(string message) { }
    }
}
namespace S1Mods.Shared
{
    public static class SaveSlots
    {
        public static int ActiveSlot { get; set; } = 0;
        public static bool ThrowOnProbe { get; set; }
        public static int GetActiveSlotNumber() => ThrowOnProbe ? throw new IOException("injected slot probe failure") : ActiveSlot;
    }
}
