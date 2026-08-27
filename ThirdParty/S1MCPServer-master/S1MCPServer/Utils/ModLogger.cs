using MelonLoader;
using System;

namespace S1MCPServer.Utils
{
    public static class ModLogger 
    {
        /// <summary>
        /// Debug logging is OFF by default. Enable via MelonPreferences
        /// (S1MCPServer → DebugLogging) or UserData/S1MCPServer.cfg.
        /// </summary>
        public static bool DebugEnabled { get; set; } = false;

        /// <summary>Max characters of a payload (raw JSON) written per debug line.</summary>
        private const int MaxPayloadLogChars = 500;

        public static void Info(string message)
        {
            MelonLogger.Msg(message);
        }

        public static void Debug(string message)
        {
            if (!DebugEnabled) return;
            MelonLogger.Msg($"[DEBUG] {message}");
        }

        /// <summary>
        /// Logs a potentially huge payload (raw JSON request/response) in truncated form.
        /// No-op unless DebugEnabled.
        /// </summary>
        public static void DebugPayload(string label, string payload)
        {
            if (!DebugEnabled) return;
            if (payload == null)
            {
                MelonLogger.Msg($"[DEBUG] {label}: <null>");
                return;
            }
            var trimmed = payload.Length <= MaxPayloadLogChars
                ? payload
                : payload.Substring(0, MaxPayloadLogChars) + $" … [+{payload.Length - MaxPayloadLogChars} chars truncated]";
            MelonLogger.Msg($"[DEBUG] {label} ({payload.Length} chars): {trimmed}");
        }

        public static void Error(string message)
        {
            MelonLogger.Msg($"[ERROR] {message}");
        }

        public static void Error(string message, Exception exception)
        {
            MelonLogger.Error($"{message}: {exception.Message}");
            MelonLogger.Error($"Stack trace: {exception.StackTrace}");
        }
        
        public static void Warn(string message)
        {
            MelonLogger.Warning(message);
        }
    }
}