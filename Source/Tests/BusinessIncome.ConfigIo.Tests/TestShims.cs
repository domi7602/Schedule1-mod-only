using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace S1Mods.Shared
{
    /// <summary>TEST-ONLY ModLogger that records to memory instead of MelonLogger.</summary>
    public class ModLogger
    {
        public string ModName { get; }
        public List<string> Messages { get; } = new();

        public ModLogger(string modName)
        {
            if (string.IsNullOrEmpty(modName)) throw new ArgumentNullException(nameof(modName));
            ModName = modName;
        }

        public void Info(string msg) => Messages.Add("INFO " + msg);
        public void Warn(string msg) => Messages.Add("WARN " + msg);
        public void Warn(string context, Exception ex) => Messages.Add($"WARN {context}: {ex}");
        public void Warn(Exception ex) => Messages.Add("WARN " + ex);
        public void Error(string msg) => Messages.Add("ERROR " + msg);
        public void Error(string context, Exception ex) => Messages.Add($"ERROR {context}: {ex}");
        public void Error(Exception ex) => Messages.Add("ERROR " + ex);
        public void Debug(string msg) { }
    }
}

namespace BusinessIncome
{
    /// <summary>TEST-ONLY shim for the mod entry point; ConfigJsonStore only reads Log.</summary>
    public static class Mod
    {
        public static S1Mods.Shared.ModLogger Log { get; } = new S1Mods.Shared.ModLogger("BusinessIncome");
    }
}
