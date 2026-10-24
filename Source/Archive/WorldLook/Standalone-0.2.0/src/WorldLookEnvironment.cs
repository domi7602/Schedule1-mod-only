using System;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.GameTime;
using Il2CppScheduleOne.Weather;
using UnityEngine;

namespace WorldLook;

/// <summary>
/// The current inputs of the sky that WorldLook actually owns. These are the same
/// Color values sent to its material, so the water fallback does not invent a
/// second palette. A valid snapshot is not proof that a camera drew the material.
/// </summary>
internal readonly struct WorldLookEnvironmentState
{
    internal readonly bool IsValid, HasWeather, HasSun, HasMoon;
    internal readonly float Daylight, SunElevation, DayFraction;
    internal readonly float CloudCoverage, CloudDensity, Windy;
    internal readonly Vector3 SunDirection, MoonDirection;
    internal readonly Vector2 WindDirection;
    internal readonly Color SunColor, SkyTopColor, SkyHorizonColor, SkyGroundColor;
    internal readonly Color CloudLightColor, CloudShadowColor;
    internal readonly Color AmbientSky, AmbientEquator, AmbientGround;

    internal WorldLookEnvironmentState(bool hasWeather, bool hasSun, bool hasMoon,
        float daylight, float sunElevation, float dayFraction,
        float cloudCoverage, float cloudDensity, float windy,
        Vector3 sunDirection, Vector3 moonDirection, Vector2 windDirection,
        Color sunColor, Color skyTopColor, Color skyHorizonColor, Color skyGroundColor,
        Color cloudLightColor, Color cloudShadowColor,
        Color ambientSky, Color ambientEquator, Color ambientGround)
    {
        IsValid = true;
        HasWeather = hasWeather; HasSun = hasSun; HasMoon = hasMoon;
        Daylight = daylight; SunElevation = sunElevation; DayFraction = dayFraction;
        CloudCoverage = cloudCoverage; CloudDensity = cloudDensity; Windy = windy;
        SunDirection = sunDirection; MoonDirection = moonDirection; WindDirection = windDirection;
        SunColor = sunColor; SkyTopColor = skyTopColor; SkyHorizonColor = skyHorizonColor;
        SkyGroundColor = skyGroundColor;
        CloudLightColor = cloudLightColor; CloudShadowColor = cloudShadowColor;
        AmbientSky = ambientSky; AmbientEquator = ambientEquator; AmbientGround = ambientGround;
    }
}

/// <summary>Unmodified light values, observed before WorldLook applies its bias.</summary>
internal readonly struct WorldLookNativeLighting
{
    internal readonly Light? Sun;
    internal readonly Color SunColor, AmbientSky, AmbientEquator, AmbientGround;
    internal readonly float SunIntensity, SunShadowStrength;

    internal WorldLookNativeLighting(Light? sun, Color sunColor, float sunIntensity,
        float sunShadowStrength, Color ambientSky, Color ambientEquator, Color ambientGround)
    {
        Sun = sun; SunColor = sunColor; SunIntensity = sunIntensity;
        SunShadowStrength = sunShadowStrength;
        AmbientSky = ambientSky; AmbientEquator = ambientEquator; AmbientGround = ambientGround;
    }
}

/// <summary>
/// Reads the existing game clock, native sky state and S1API weather. It never
/// selects weather, evaluates native effect methods or writes a native SkyState.
/// </summary>
internal static class WorldLookEnvironment
{
    internal static WorldLookEnvironmentState Current { get; private set; }

    private static bool _weatherInitialized;
    private static float _cloudiness = .3f;
    private static float _windy;
    private static Vector2 _windDirection = new(.94f, .34f);

    internal static EnvironmentManager? TryGetManager()
    {
        try
        {
            if (!NetworkSingleton<EnvironmentManager>.InstanceExists) return null;
            var manager = NetworkSingleton<EnvironmentManager>.Instance;
            return IsAlive(manager) ? manager : null;
        }
        catch { return null; }
    }

    internal static DayNightController? TryGetController(EnvironmentManager? manager)
    {
        try
        {
            if (!IsAlive(manager)) return null;
            var controller = manager!._dayNightController;
            return IsAlive(controller) ? controller : null;
        }
        catch { return null; }
    }

    internal static WorldLookEnvironmentState Read(CloudSkySettings settings,
        EnvironmentManager? manager, DayNightController? controller,
        WorldLookNativeLighting native, float elapsed)
    {
        float fraction = ReadDayFraction();
        Vector3 sunDirection = Vector3.up;
        Vector3 moonDirection = Vector3.down;
        bool hasSun = TryDirection(native.Sun, out sunDirection);
        bool hasMoon = false;
        try { if (IsAlive(controller)) hasMoon = TryDirection(controller!._moonLight, out moonDirection); }
        catch { }

        // The native transform is authoritative, including native time overrides.
        // A clock-only fallback supplies the gradient, never a guessed sun disc.
        float elevation = hasSun ? sunDirection.y : Mathf.Sin((fraction - .25f) * Mathf.PI * 2f);
        float daylight = Smooth(-.14f, .20f, elevation);
        float twilight = (1f - Smooth(.02f, .38f, Math.Abs(elevation))) * daylight;

        bool hasWeather = TryReadWeather(out float cloudiness, out float windy);
        float smoothing = 1f - Mathf.Exp(-Mathf.Clamp(elapsed, 0f, 2f) / 4f);
        if (!_weatherInitialized)
        {
            _cloudiness = cloudiness; _windy = windy; _weatherInitialized = true;
        }
        else
        {
            _cloudiness = Mathf.Lerp(_cloudiness, cloudiness, smoothing);
            _windy = Mathf.Lerp(_windy, windy, smoothing);
        }

        // The configuration sets the art direction; native weather shifts it.
        // Missing API state is a moderate .3 input, not an invented weather event.
        float baseCoverage = Mathf.Clamp01(Finite(settings.Coverage, .55f));
        // Preserve useful slider endpoints: zero is clear even in native rain;
        // one keeps maximum configured coverage. Intermediate values follow weather.
        float weatherInfluence = 4f * baseCoverage * (1f - baseCoverage);
        float coverage = Mathf.Clamp01(baseCoverage + (_cloudiness - .3f) * .55f * weatherInfluence);
        float density = Mathf.Clamp(Finite(settings.Density, .75f) * (.90f + .25f * _cloudiness), .05f, 1.5f);

        Color nativeTop = native.AmbientSky;
        Color nativeHorizon = native.AmbientEquator;
        Color nativeGround = native.AmbientGround;
        try
        {
            if (IsAlive(manager))
            {
                var sky = manager!._currentSkyState;
                if (sky != null && sky.Pointer != IntPtr.Zero && !sky.WasCollected)
                {
                    nativeTop = SafeColor(sky.SkyUpperColor, nativeTop);
                    nativeHorizon = SafeColor(sky.SkyMiddleColor, nativeHorizon);
                    nativeGround = SafeColor(sky.SkyLowerColor, nativeGround);
                }

                var wind = manager._windVelocity;
                var horizontal = new Vector2(wind.x, wind.z);
                if (IsFinite(horizontal.x) && IsFinite(horizontal.y) && horizontal.sqrMagnitude > .001f)
                {
                    var blended = Vector2.Lerp(_windDirection, horizontal.normalized, smoothing);
                    if (blended.sqrMagnitude > .001f) _windDirection = blended.normalized;
                }
            }
        }
        catch { /* A stale native proxy only loses its optional color/wind input. */ }

        Color clearTop = Color.Lerp(new Color(.009f, .016f, .035f, 1f),
            new Color(.24f, .40f, .61f, 1f), daylight);
        Color clearHorizon = Color.Lerp(new Color(.020f, .029f, .050f, 1f),
            new Color(.64f, .71f, .77f, 1f), daylight);
        Color nativeSun = SafeColor(native.SunColor, Color.white);
        Color sunset = Color.Lerp(new Color(.86f, .59f, .40f, 1f), NormalizeColor(nativeSun), .45f);
        clearHorizon = Color.Lerp(clearHorizon, sunset, twilight * .28f);

        // Keep the game's day/night and weather palette present, but rein in
        // very saturated daytime colors instead of tinting the entire screen.
        float nativeInfluence = Mathf.Lerp(.65f, .28f, daylight);
        Color top = Color.Lerp(clearTop, SafeColor(nativeTop, clearTop), nativeInfluence);
        Color horizon = Color.Lerp(clearHorizon, SafeColor(nativeHorizon, clearHorizon), nativeInfluence);
        float overcast = Smooth(.55f, .95f, coverage);
        Color overcastTop = Color.Lerp(new Color(.018f, .025f, .039f, 1f),
            new Color(.54f, .59f, .65f, 1f), daylight);
        top = Color.Lerp(top, overcastTop, overcast * .72f);
        horizon = Color.Lerp(horizon, Color.Lerp(overcastTop, horizon, .35f), overcast * .55f);
        Color ground = Color.Lerp(SafeColor(nativeGround, native.AmbientGround), horizon, .22f);

        Color cloudLight = Color.Lerp(new Color(.055f, .069f, .096f, 1f),
            new Color(.96f, .97f, .98f, 1f), daylight);
        Color cloudShadow = Color.Lerp(new Color(.017f, .024f, .038f, 1f),
            new Color(.46f, .51f, .58f, 1f), daylight);
        cloudLight = Color.Lerp(cloudLight, sunset, twilight * .30f * (1f - overcast * .6f));
        cloudLight = Color.Lerp(cloudLight, new Color(.73f, .76f, .80f, 1f), overcast * daylight * .30f);
        cloudShadow = Color.Lerp(cloudShadow, horizon, .12f);

        Current = new WorldLookEnvironmentState(hasWeather, hasSun, hasMoon,
            daylight, elevation, fraction, coverage, density, _windy,
            sunDirection, moonDirection, _windDirection, nativeSun,
            top, horizon, ground, cloudLight, cloudShadow,
            native.AmbientSky, native.AmbientEquator, native.AmbientGround);
        return Current;
    }

    private static bool TryReadWeather(out float cloudiness, out float windy)
    {
        cloudiness = .3f; windy = 0f;
        try
        {
            var snapshot = S1API.Weather.WeatherManager.Current;
            if (!snapshot.HasValue) return false;
            var weather = snapshot.Value;
            // The API does not promise that its weights sum to one.
            cloudiness = Mathf.Max(Unit(weather.Cloudy), .9f * Unit(weather.Rainy));
            cloudiness = Mathf.Max(cloudiness, Unit(weather.Stormy));
            cloudiness = Mathf.Max(cloudiness, .8f * Unit(weather.Snowy));
            cloudiness = Mathf.Max(cloudiness, .65f * Unit(weather.Foggy));
            cloudiness = Mathf.Max(cloudiness, .85f * Mathf.Max(Unit(weather.Hail), Unit(weather.Sleet)));
            windy = Unit(weather.Windy);
            return true;
        }
        catch { return false; }
    }

    private static float ReadDayFraction()
    {
        try
        {
            var clock = NetworkSingleton<TimeManager>.Instance;
            if (IsAlive(clock))
            {
                int minutes = clock.CurrentTime;
                if (minutes >= 0 && minutes < 1440) return minutes / 1440f;
            }
        }
        catch { }
        return .5f;
    }

    private static bool TryDirection(Light? light, out Vector3 direction)
    {
        direction = Vector3.up;
        try
        {
            if (!IsAlive(light)) return false;
            var value = -light!.transform.forward;
            if (!IsFinite(value.x) || !IsFinite(value.y) || !IsFinite(value.z) || value.sqrMagnitude < .001f) return false;
            direction = value.normalized;
            return true;
        }
        catch { return false; }
    }

    internal static bool IsAlive(UnityEngine.Object? value)
    {
        if (ReferenceEquals(value, null)) return false;
        try { return !value.WasCollected && value.Pointer != IntPtr.Zero && value != null; }
        catch { return false; }
    }

    internal static bool SameObject(UnityEngine.Object? a, UnityEngine.Object? b)
    {
        if (!IsAlive(a) || !IsAlive(b)) return false;
        try { return a!.GetInstanceID() == b!.GetInstanceID(); }
        catch { return false; }
    }

    internal static bool IsFinite(float value) => !float.IsNaN(value) && !float.IsInfinity(value);
    internal static float Finite(float value, float fallback) => IsFinite(value) ? value : fallback;
    internal static float Unit(float value) => Mathf.Clamp01(Finite(value, 0f));
    internal static float Smooth(float start, float end, float value)
    {
        float t = Mathf.Clamp01((value - start) / Math.Max(.0001f, end - start));
        return t * t * (3f - 2f * t);
    }

    internal static Color SafeColor(Color value, Color fallback)
        => IsFinite(value.r) && IsFinite(value.g) && IsFinite(value.b)
            ? new Color(Math.Max(0f, value.r), Math.Max(0f, value.g), Math.Max(0f, value.b), 1f)
            : fallback;

    private static Color NormalizeColor(Color value)
    {
        float maximum = Math.Max(.001f, Math.Max(value.r, Math.Max(value.g, value.b)));
        return new Color(value.r / maximum, value.g / maximum, value.b / maximum, 1f);
    }

    internal static void Reset()
    {
        Current = default;
        _weatherInitialized = false;
        _cloudiness = .3f;
        _windy = 0f;
        _windDirection = new Vector2(.94f, .34f).normalized;
    }
}
