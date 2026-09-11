using System;
using System.IO;
using System.Reflection;
using MelonLoader;
using S1Mods.Shared;

[assembly: MelonInfo(typeof(_DiagPerfCounter.Mod), "_DiagPerfCounter", "0.3.2", "Diag")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace _DiagPerfCounter;

public sealed class Mod : MelonMod
{
    public override void OnInitializeMelon()
    {
#if !DEBUG
        // Dev-tool: never dump in release builds shipped to players.
        return;
#else
        string path = SafeStorage.GetUserDataPath("_DiagPerfCounter", "dump.txt");
        SafeStorage.EnsureDirectoryForFile(path);
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("=== Dumping StorageEntity ===");
        try
        {
            Type[] typesToDump = new[] {
                typeof(Il2CppScheduleOne.Storage.StorageEntity)
            };
            foreach (var t in typesToDump)
            {
                sb.AppendLine($"=== Type: {t.Name} ===");
                foreach (var m in t.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly))
                {
                    sb.AppendLine($"Method: {m.ReturnType.Name} {m.Name}({string.Join(", ", System.Linq.Enumerable.Select(m.GetParameters(), p => p.ParameterType.Name + " " + p.Name))})");
                }
            }
        }
        catch (Exception ex)
        {
            sb.AppendLine(ex.ToString());
        }
        SafeStorage.SaveTextAtomic(path, sb.ToString(), null);
        MelonLogger.Msg("Dumped methods to UserData/_DiagPerfCounter/dump.txt");
#endif
    }
}
