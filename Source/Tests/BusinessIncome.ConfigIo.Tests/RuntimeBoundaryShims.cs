using System;
using System.Collections.Generic;
using BusinessIncome.Services;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

// Only external game/environment boundaries are replaced. Storage, facade, calculator
// and settlement implementations are linked directly from production source.
namespace MelonLoader.Utils
{
    public static class MelonEnvironment
    {
        public static string UserDataDirectory { get; set; } = Environment.GetEnvironmentVariable("TMPDIR")!;
    }
}
namespace MelonLoader
{
    public static class MelonLogger { public static void Warning(string message) { } }
}
namespace S1Mods.Shared
{
    public static class NetworkGuard
    {
        public static bool Host { get; set; } = true;
        public static bool IsHostOrSingleplayer() => Host;
    }
    public sealed class TestSaveInfo
    {
        public int SlotNumber { get; set; }
        public string SavePath { get; set; } = "";
    }
    public static class SaveSlots
    {
        public static TestSaveInfo? Active { get; set; }
        public static TestSaveInfo? TryGetActiveSaveInfo() => Active;
        public static string? TryExtractSlotTokenFromSavePath(string? path) => null;
    }
}
namespace BusinessIncome.Services
{
    // The procedural Unity sprite is a native presentation boundary, not payout logic.
    internal static class BusinessIcon
    {
        internal static object Get() => new object();
    }

    public static class BusinessResolver
    {
        public static bool Readable { get; set; } = true;
        public static List<BusinessData> Owned { get; set; } = new();
        public static bool TryGetOwnedBusinesses(Dictionary<string, string> names, out List<BusinessData> owned)
        {
            owned = new List<BusinessData>(Owned);
            return Readable;
        }
    }
}
namespace UnityEngine { public class Object { } }
namespace Il2CppScheduleOne.UI
{
    public class NotificationsManager : UnityEngine.Object
    {
        public static NotificationsManager? Instance => null;
        public void SendNotification(string title, string text, object icon, float duration, bool sound) { }
    }
}
namespace Il2CppScheduleOne.Money
{
    public sealed class MoneyManager
    {
        public static MoneyManager? Instance { get; set; } = new();
        public IntPtr Pointer { get; set; } = new(1);
        public bool WasCollected { get; set; }
    }
}
namespace S1API.Money
{
    public static class Money
    {
        public static int Requests { get; set; }
        public static void CreateOnlineTransaction(string name, float amount, float days, string note) => Requests++;
    }
}
namespace S1API.GameTime
{
    public enum Day { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday }
    public static class TimeManager { public static Day CurrentDay => Day.Monday; }
}
