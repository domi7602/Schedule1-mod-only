using System;
using System.IO;
using System.Reflection;
using MelonLoader;
using S1Mods.Shared;

[assembly: MelonInfo(typeof(_DiagPerfCounter.Mod), "_DiagPerfCounter", "0.3.0", "Diag")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace _DiagPerfCounter;

public sealed class Mod : MelonMod
{
    public override void OnInitializeMelon()
    {
        string path = SafeStorage.GetUserDataPath("_DiagPerfCounter", "dump.txt");
        SafeStorage.EnsureDirectoryForFile(path);
        using var sw = new StreamWriter(path, append: false);
        sw.WriteLine("=== Dumping StorageEntity ===");
        try
        {
            Type[] typesToDump = new[] {
                typeof(Il2CppScheduleOne.Storage.StorageEntity)
            };
            foreach (var t in typesToDump)
            {
                sw.WriteLine($"=== Type: {t.Name} ===");
                foreach (var m in t.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
                {
                    sw.WriteLine($"Method: {m.Name}");
                    foreach (var p in m.GetParameters())
                    {
                        sw.WriteLine($"  param: {p.ParameterType.Name} {p.Name}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            sw.WriteLine(ex.ToString());
        }
        MelonLogger.Msg("Dumped methods to UserData/_DiagPerfCounter/dump.txt");
    }
}
