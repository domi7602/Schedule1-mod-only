using System;
using MelonLoader;

namespace S1Mods.Shared;

public class ModLogger
{
    public string ModName { get; }

    public ModLogger(string modName)
    {
        if (string.IsNullOrEmpty(modName))
            throw new ArgumentNullException(nameof(modName));
        ModName = modName;
    }

    public void Info(string msg) => MelonLogger.Msg($"[{ModName}] {msg}");
    public void Warn(string msg) => MelonLogger.Warning($"[{ModName}] {msg}");
    public void Warn(string context, Exception ex) => MelonLogger.Warning($"[{ModName}] {context}: {ex}");
    public void Warn(Exception ex) => MelonLogger.Warning($"[{ModName}] {ex}");
    public void Error(string msg) => MelonLogger.Error($"[{ModName}] {msg}");
    public void Error(string context, Exception ex) => MelonLogger.Error($"[{ModName}] {context}: {ex}");
    public void Error(Exception ex) => MelonLogger.Error($"[{ModName}] {ex}");
    public void Debug(string msg)
    {
#if DEBUG
        MelonLogger.Msg($"[{ModName}] [DEBUG] {msg}");
#endif
    }
}
