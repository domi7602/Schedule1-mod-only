using System;
using System.Text.Json;

namespace PropertyLEDs.Core
{
    public enum StateLoadStatus { Missing, Loaded, Invalid, UnsupportedVersion }

    public sealed class StateLoadResult
    {
        internal StateLoadResult(LEDState state, StateLoadStatus status, string? originalJson)
        { State = state; Status = status; OriginalJson = originalJson; }
        public LEDState State { get; }
        public StateLoadStatus Status { get; }
        public string? OriginalJson { get; }
        public bool CanWrite => Status == StateLoadStatus.Missing || Status == StateLoadStatus.Loaded;
    }

    /// <summary>Pure read decisions. Invalid/future input must not be overwritten by the I/O adapter.</summary>
    public static class StateCodec
    {
        public static StateLoadResult Read(string? json)
        {
            if (json == null) return new StateLoadResult(new LEDState(), StateLoadStatus.Missing, null);
            try
            {
                using var document = JsonDocument.Parse(json);
                if (document.RootElement.ValueKind != JsonValueKind.Object) return Invalid(json);
                int versions = 0, version = 0;
                foreach (var member in document.RootElement.EnumerateObject())
                {
                    if (!string.Equals(member.Name, "FormatVersion", StringComparison.OrdinalIgnoreCase)) continue;
                    versions++;
                    if (!member.Value.TryGetInt32(out version)) return Invalid(json);
                }
                if (versions != 1) return Invalid(json);
                if (version != LEDState.CurrentFormatVersion)
                    return new StateLoadResult(new LEDState(), StateLoadStatus.UnsupportedVersion, json);
                var state = JsonSerializer.Deserialize<LEDState>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (state == null || !state.Normalize()) return Invalid(json);
                return new StateLoadResult(state, StateLoadStatus.Loaded, json);
            }
            catch (JsonException) { return Invalid(json); }
            catch (InvalidOperationException) { return Invalid(json); }
        }

        private static StateLoadResult Invalid(string json)
            => new StateLoadResult(new LEDState(), StateLoadStatus.Invalid, json);
    }
}
