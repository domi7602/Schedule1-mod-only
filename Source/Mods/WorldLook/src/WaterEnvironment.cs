using System;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Weather;
using UnityEngine;
using UnityEngine.Rendering;

namespace WaterShader
{
    /// <summary>Read-only native colors/weather. No clock model, custom sky palette or sky writes.</summary>
    internal static class WaterEnvironment
    {
        internal static bool Read(out Color sky, out Color horizon, out float wind, out string source)
        {
            (float r, float g, float b, float a)? nativeSky = null, nativeHorizon = null;
            (float r, float g, float b, float a)? ambientSky = null, ambientHorizon = null;
            try
            {
                if (NetworkSingleton<EnvironmentManager>.InstanceExists)
                {
                    var manager = NetworkSingleton<EnvironmentManager>.Instance;
                    if (IsUsable(manager))
                    {
                        var state = manager._currentSkyState;
                        if (state != null && state.Pointer != IntPtr.Zero && !state.WasCollected)
                        {
                            nativeSky = Channels(state.SkyUpperColor);
                            nativeHorizon = Channels(state.SkyMiddleColor);
                        }
                    }
                }
            }
            catch { /* Optional native input: use current ambient colors when unavailable. */ }
            try
            {
                if (RenderSettings.ambientMode == AmbientMode.Flat)
                    ambientSky = ambientHorizon = Channels(RenderSettings.ambientLight);
                else
                {
                    ambientSky = Channels(RenderSettings.ambientSkyColor);
                    ambientHorizon = Channels(RenderSettings.ambientEquatorColor);
                }
            }
            catch { }

            wind = 0f;
            try
            {
                var weather = S1API.Weather.WeatherManager.Current;
                if (weather.HasValue) wind = WaterEnvironmentPolicy.WindActivity(weather.Value.Windy);
            }
            catch { }

            var top = WaterEnvironmentPolicy.SelectColor(nativeSky, ambientSky);
            var middle = WaterEnvironmentPolicy.SelectColor(nativeHorizon, ambientHorizon);
            sky = top.HasValue ? ColorOf(top.Value) : default;
            horizon = middle.HasValue ? ColorOf(middle.Value) : default;
            if (!top.HasValue || !middle.HasValue) { source = "unavailable; last finite colors retained"; return false; }
            bool nativeTop = nativeSky.HasValue && WaterEnvironmentPolicy.IsFinite(nativeSky.Value);
            bool nativeMiddle = nativeHorizon.HasValue && WaterEnvironmentPolicy.IsFinite(nativeHorizon.Value);
            source = nativeTop && nativeMiddle ? "native SkyState"
                : nativeTop || nativeMiddle ? "native SkyState / ambient fallback" : "ambient fallback";
            return true;
        }

        private static (float r, float g, float b, float a) Channels(Color value)
            => (value.r, value.g, value.b, value.a);
        private static Color ColorOf((float r, float g, float b, float a) value)
            => new(value.r, value.g, value.b, value.a);

        // Unknown ownership must throw; optional input probes may use the soft wrapper.
        internal static bool IsAlive(UnityEngine.Object? value)
        {
            if (ReferenceEquals(value, null)) return false;
            if (value.WasCollected) throw new InvalidOperationException("Collected water proxy; ownership cannot be read safely.");
            return value.Pointer != IntPtr.Zero && value != null;
        }

        internal static bool IsUsable(UnityEngine.Object? value)
        {
            try { return IsAlive(value); }
            catch { return false; }
        }

        internal static bool SameObject(UnityEngine.Object? left, UnityEngine.Object right)
            => IsAlive(left) && IsAlive(right) && left!.Pointer == right.Pointer;
    }
}
