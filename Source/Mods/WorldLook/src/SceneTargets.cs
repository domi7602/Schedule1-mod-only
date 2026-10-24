using System;
using System.Collections.Generic;
using S1Mods.Shared;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace GoldenHour;

/// <summary>
/// Exactly five borrowed native color channels. Reading RenderSettings.skybox does
/// not instantiate a material. No renderer.material, Volume.profile, clone, skybox
/// assignment or destruction is involved. Each restore closure names its original
/// object; a replacement can never receive the old object's baseline.
/// </summary>
public sealed class SceneTargets
{
    private const string SunShine = "_SunShine";
    private readonly LightingChannel?[] _channels = new LightingChannel?[5];
    private readonly bool[] _detachRequested = new bool[5];
    private readonly HashSet<string> _reported = new();
    private readonly ModLogger _log;
    private Material? _skyMaterial;
    private Light? _sunLight;
    private int? _ambientScene;

    public SceneTargets(ModLogger log) => _log = log;

    public int ErrorCount { get; private set; }
    public string? LastError { get; private set; }
    public int PendingRestores
    {
        get
        {
            int count = 0;
            foreach (LightingChannel? channel in _channels)
                if (channel != null && channel.Releasing && channel.HasPendingWrite) count++;
            return count;
        }
    }

    /// <summary>LateUpdate only: constant-sized native access, no scene scans.</summary>
    public int Apply(Scene scene, double strength, double nowSeconds)
    {
        RefreshSky(nowSeconds);
        RefreshSun(nowSeconds);
        RefreshAmbient(scene, nowSeconds);

        int readable = 0;
        for (int i = 0; i < _channels.Length; i++)
        {
            LightingChannel? channel = _channels[i];
            if (channel == null) continue;
            if (channel.Faulted || channel.Releasing)
            {
                Release(i, nowSeconds);
                continue;
            }
            if (channel.Apply(strength)) readable++;
        }
        return readable;
    }

    public void RestoreAll(double nowSeconds)
    {
        for (int i = 0; i < _channels.Length; i++) Release(i, nowSeconds, detach: true);
    }

    public void RetryReleases(double nowSeconds)
    {
        for (int i = 0; i < _channels.Length; i++)
        {
            LightingChannel? channel = _channels[i];
            if (channel != null && (channel.Releasing || channel.Faulted)) Release(i, nowSeconds);
        }
    }

    private void Release(int index, double nowSeconds, bool detach = false)
    {
        _detachRequested[index] |= detach;
        LightingChannel? channel = _channels[index];
        if (channel != null && !channel.Release(nowSeconds)) return;
        // A broken setter must not be rebound and retried every frame. Keep this
        // released faulted lane dormant until OFF, a world reset or target replacement.
        if (channel != null && channel.Faulted && !_detachRequested[index]) return;
        _channels[index] = null;
        _detachRequested[index] = false;
        if (index == 0) _skyMaterial = null;
        if (index == 1) _sunLight = null;
        if (_channels[2] == null && _channels[3] == null && _channels[4] == null) _ambientScene = null;
    }

    private void RefreshSky(double nowSeconds)
    {
        try
        {
            Material? material = RenderSettings.skybox;
            if (_channels[0] != null && !SameObject(_skyMaterial, material)) Release(0, nowSeconds, detach: true);
            if (_channels[0] != null || !HasSunShine(material)) return;

            // Capture the exact borrowed instance, never re-resolve it inside the setter.
            Material target = material!;
            _skyMaterial = target;
            _channels[0] = new LightingChannel(
                () => HasSunShine(target),
                () => ColorFrom(target.GetColor(SunShine)),
                value =>
                {
                    if (!HasSunShine(target)) throw new InvalidOperationException("sky target disappeared");
                    target.SetColor(SunShine, ColorFrom(value));
                }, GoldenHourTargets.SkySunGlowTint, error => Report("sky.sunShine", error));
        }
        catch (Exception ex) { Report("sky lookup", ex.Message); }
    }

    private void RefreshSun(double nowSeconds)
    {
        try
        {
            Light? light = RenderSettings.sun;
            if (_channels[1] != null && !SameObject(_sunLight, light)) Release(1, nowSeconds, detach: true);
            if (_channels[1] != null || !NetworkGuard.IsAlive(light)) return;

            Light target = light!;
            _sunLight = target;
            _channels[1] = new LightingChannel(
                () => NetworkGuard.IsAlive(target),
                () => ColorFrom(target.color),
                value =>
                {
                    if (!NetworkGuard.IsAlive(target)) throw new InvalidOperationException("sun target disappeared");
                    target.color = ColorFrom(value);
                }, GoldenHourTargets.SunLightTint, error => Report("sun.color", error));
        }
        catch (Exception ex) { Report("sun lookup", ex.Message); }
    }

    private void RefreshAmbient(Scene scene, double nowSeconds)
    {
        if (_ambientScene.HasValue && _ambientScene != scene.handle)
        {
            for (int i = 2; i < _channels.Length; i++) Release(i, nowSeconds, detach: true);
            if (_channels[2] != null || _channels[3] != null || _channels[4] != null) return;
        }

        int handle = scene.handle;
        _ambientScene = handle;
        for (int i = 2; i < _channels.Length; i++)
        {
            if (_channels[i] != null) continue;
            int index = i;
            _channels[i] = new LightingChannel(
                () => IsCurrentScene(handle),
                () => RenderSettings.ambientMode == AmbientMode.Trilight ? ReadAmbient(index) : null,
                value =>
                {
                    if (!IsCurrentScene(handle) || RenderSettings.ambientMode != AmbientMode.Trilight)
                        throw new InvalidOperationException("ambient scene or mode changed");
                    WriteAmbient(index, value);
                }, GoldenHourTargets.AmbientTint, error => Report("ambient." + index, error));
        }
    }

    private static bool IsCurrentScene(int handle)
    {
        Scene scene = SceneManager.GetActiveScene();
        return scene.IsValid() && scene.isLoaded && scene.name == "Main" && scene.handle == handle;
    }

    private static bool SameObject(UnityEngine.Object? a, UnityEngine.Object? b) =>
        NetworkGuard.IsAlive(a) && NetworkGuard.IsAlive(b) && a!.Pointer == b!.Pointer;

    private static bool HasSunShine(Material? material) =>
        NetworkGuard.IsAlive(material) && material!.HasProperty(SunShine);

    private static Argb ReadAmbient(int index) => ColorFrom(index switch
    {
        2 => RenderSettings.ambientSkyColor,
        3 => RenderSettings.ambientEquatorColor,
        _ => RenderSettings.ambientGroundColor,
    });

    private static void WriteAmbient(int index, Argb value)
    {
        Color color = ColorFrom(value);
        switch (index)
        {
            case 2: RenderSettings.ambientSkyColor = color; break;
            case 3: RenderSettings.ambientEquatorColor = color; break;
            case 4: RenderSettings.ambientGroundColor = color; break;
        }
    }

    private void Report(string channel, string error)
    {
        string message = channel + ": " + error;
        LastError = message;
        if (!_reported.Add(message)) return;
        ErrorCount++;
        _log.Warn(message);
    }

    private static Argb ColorFrom(Color color) => new(color.r, color.g, color.b, color.a);
    private static Color ColorFrom(Argb color) => new(color.R, color.G, color.B, color.A);
}
