using System;
using Il2CppScheduleOne.PlayerScripts;
using S1Mods.Shared;
using UnityEngine;

namespace MoonRocks;

internal enum FlightPhase
{
    IdleOnEarth,
    LaunchingToMoon,
    OnMoon,
    ReturningToEarth
}

/// <summary>
/// Orchestrates the live Planet-Crafter-style rocket ascent/descent, camera takeover,
/// atmosphere-to-space transition, and low-gravity lunar physics.
/// </summary>
internal sealed class FlightDirector
{
    private const float AscentDuration = 14.0f;
    private const float ReturnDuration = 12.0f;
    private const float InteractDistance = 6.5f;

    private readonly LaunchPadBuilder _pad;
    private readonly MoonTerrainBuilder _moon;

    private FlightPhase _phase = FlightPhase.IdleOnEarth;
    private float _flightElapsed;
    private bool _switchedToMoonSpace;
    private bool _switchedToEarthSpace;
    private bool _playerNearRocket;

    // Saved Earth atmosphere settings to restore cleanly on return or teardown.
    private bool _hasSavedAtmosphere;
    private Color _savedAmbientLight;
    private Color _savedFogColor;
    private float _savedFogDensity;
    private bool _savedFogEnabled;
    private Material? _savedSkybox;
    private CameraClearFlags _savedClearFlags;
    private Color _savedBgColor;
    private float _savedBaseGravity = 1f;

    internal FlightPhase Phase => _phase;
    internal bool IsInFlight => _phase is FlightPhase.LaunchingToMoon or FlightPhase.ReturningToEarth;
    internal bool IsNearRocket => _playerNearRocket && !IsInFlight;

    internal FlightDirector(LaunchPadBuilder pad, MoonTerrainBuilder moon)
    {
        _pad = pad;
        _moon = moon;
    }

    internal void Update(float dt)
    {
        if (!NetworkGuard.IsHostOrSingleplayer()) return;

        UpdateProximity();

        if (_playerNearRocket && !IsInFlight && Input.GetKeyDown(KeyCode.E))
        {
            if (_phase == FlightPhase.IdleOnEarth) StartLaunchToMoon();
            else if (_phase == FlightPhase.OnMoon) StartReturnToEarth();
        }

        if (_phase == FlightPhase.LaunchingToMoon)
        {
            TickLaunchToMoon(dt);
        }
        else if (_phase == FlightPhase.ReturningToEarth)
        {
            TickReturnToEarth(dt);
        }
        else if (_phase == FlightPhase.OnMoon)
        {
            // Keep lunar gravity enforced while on the moon.
            if (Mathf.Abs(PlayerMovement.GravityMultiplier - MoonMath.MoonGravityMultiplier) > 0.01f)
            {
                PlayerMovement.GravityMultiplier = MoonMath.MoonGravityMultiplier;
            }
        }
    }

    internal bool StartLaunchToMoon()
    {
        if (IsInFlight || _pad.RocketRoot == null) return false;
        CaptureEarthAtmosphere();

        _phase = FlightPhase.LaunchingToMoon;
        _flightElapsed = 0f;
        _switchedToMoonSpace = false;

        _pad.PlaceRocketOnEarth();
        _pad.SetThrustersActive(true);
        LockPlayerForFlight();
        Mod.Log("Raketenstart eingeleitet: Auf zum Mond!");
        return true;
    }

    internal bool StartReturnToEarth()
    {
        if (IsInFlight || _pad.RocketRoot == null) return false;

        _phase = FlightPhase.ReturningToEarth;
        _flightElapsed = 0f;
        _switchedToEarthSpace = false;

        _pad.PlaceRocketOnMoon(_moon.LandingPadCenter);
        _pad.SetThrustersActive(true);
        LockPlayerForFlight();
        Mod.Log("Rueckflug zur Erde eingeleitet!");
        return true;
    }

    internal void TeleportDirectToMoon()
    {
        if (IsInFlight) return;
        CaptureEarthAtmosphere();
        _moon.SetActive(true);
        ApplySpaceAtmosphere(1f);
        _phase = FlightPhase.OnMoon;
        _pad.PlaceRocketOnMoon(_moon.LandingPadCenter);
        TeleportPlayer(_moon.PlayerArrivalPoint, Quaternion.Euler(0f, -90f, 0f));
        PlayerMovement.GravityMultiplier = MoonMath.MoonGravityMultiplier;
        Mod.Log("Direkt zum Mond teleportiert (Low-Gravity aktiv).");
    }

    internal void TeleportDirectToEarth()
    {
        if (IsInFlight) return;
        _moon.SetActive(false);
        RestoreEarthAtmosphere();
        _phase = FlightPhase.IdleOnEarth;
        _pad.PlaceRocketOnEarth();
        TeleportPlayer(_pad.EarthArrivalPoint, Quaternion.Euler(0f, -90f, 0f));
        PlayerMovement.GravityMultiplier = _savedBaseGravity;
        Mod.Log("Zurueck zur Startrampe auf der Erde teleportiert.");
    }

    private void TickLaunchToMoon(float dt)
    {
        _flightElapsed += dt;
        _pad.AnimateThrusters(_flightElapsed);
        float t = MoonMath.Clamp01(_flightElapsed / AscentDuration);

        var rocket = _pad.RocketRoot;
        if (rocket == null)
        {
            AbortFlightToEarth();
            return;
        }

        if (t < 0.72f)
        {
            // Phase 1 & 2: Ascend live above Hyland Point while the sky darkens into space.
            float ascentT = t / 0.72f;
            float climb = MoonMath.ComputeAscentProgress(ascentT) * 950f;
            Vector3 rocketPos = _pad.EarthPadCenter + new Vector3(0f, 0.32f + climb, 0f);
            rocket.transform.position = rocketPos;

            float darkness = MoonMath.ComputeSpaceDarkness(ascentT);
            ApplySpaceAtmosphere(darkness);

            // Dramatic Planet-Crafter-style camera: tracks rocket while looking slightly down at shrinking Hyland Point.
            float shake = (1f - ascentT * 0.5f) * 0.18f;
            Vector3 shakeOffset = new(
                Mathf.Sin(_flightElapsed * 37f) * shake,
                Mathf.Cos(_flightElapsed * 43f) * shake,
                Mathf.Sin(_flightElapsed * 29f) * shake
            );
            float camDist = Mathf.Lerp(14f, 22f, ascentT);
            float camHeight = Mathf.Lerp(4f, 11f, ascentT);
            Vector3 camPos = rocketPos + new Vector3(camDist * 0.7f, camHeight, -camDist * 0.7f) + shakeOffset;
            Vector3 lookTarget = rocketPos + new Vector3(0f, Mathf.Lerp(4f, -12f, ascentT), 0f);
            OverrideCamera(camPos, Quaternion.LookRotation(lookTarget - camPos, Vector3.up));
        }
        else
        {
            // Phase 3: Cross into lunar space and retro-burn down onto the Landing Base.
            if (!_switchedToMoonSpace)
            {
                _switchedToMoonSpace = true;
                _moon.SetActive(true);
                ApplySpaceAtmosphere(1f);
            }

            float landT = (t - 0.72f) / 0.28f;
            float descent = MoonMath.ComputeDescentProgress(landT);
            float height = Mathf.Lerp(260f, 0.10f, descent);
            Vector3 rocketPos = _moon.LandingPadCenter + new Vector3(0f, height, 0f);
            rocket.transform.position = rocketPos;

            Vector3 camPos = rocketPos + new Vector3(16f, Mathf.Lerp(12f, 5.5f, descent), -16f);
            Vector3 lookTarget = rocketPos + new Vector3(0f, 3.5f, 0f);
            OverrideCamera(camPos, Quaternion.LookRotation(lookTarget - camPos, Vector3.up));
        }

        if (t >= 1f)
        {
            _phase = FlightPhase.OnMoon;
            _pad.PlaceRocketOnMoon(_moon.LandingPadCenter);
            TeleportPlayer(_moon.PlayerArrivalPoint, Quaternion.Euler(0f, -90f, 0f));
            UnlockPlayerAfterFlight(MoonMath.MoonGravityMultiplier);
            Mod.Log("Landung auf dem Mond abgeschlossen! Willkommen auf der Landing Base.");
        }
    }

    private void TickReturnToEarth(float dt)
    {
        _flightElapsed += dt;
        _pad.AnimateThrusters(_flightElapsed);
        float t = MoonMath.Clamp01(_flightElapsed / ReturnDuration);

        var rocket = _pad.RocketRoot;
        if (rocket == null)
        {
            AbortFlightToEarth();
            return;
        }

        if (t < 0.55f)
        {
            float ascentT = t / 0.55f;
            float climb = MoonMath.ComputeAscentProgress(ascentT) * 280f;
            Vector3 rocketPos = _moon.LandingPadCenter + new Vector3(0f, 0.10f + climb, 0f);
            rocket.transform.position = rocketPos;

            Vector3 camPos = rocketPos + new Vector3(15f, 8f, -15f);
            OverrideCamera(camPos, Quaternion.LookRotation((rocketPos + new Vector3(0f, 3f, 0f)) - camPos, Vector3.up));
        }
        else
        {
            if (!_switchedToEarthSpace)
            {
                _switchedToEarthSpace = true;
                _moon.SetActive(false);
            }

            float landT = (t - 0.55f) / 0.45f;
            float descent = MoonMath.ComputeDescentProgress(landT);
            float height = Mathf.Lerp(650f, 0.32f, descent);
            Vector3 rocketPos = _pad.EarthPadCenter + new Vector3(0f, height, 0f);
            rocket.transform.position = rocketPos;

            float darkness = 1f - MoonMath.SmoothStep(0.15f, 0.85f, landT);
            ApplySpaceAtmosphere(darkness);

            Vector3 camPos = rocketPos + new Vector3(15f, Mathf.Lerp(11f, 5f, descent), -15f);
            OverrideCamera(camPos, Quaternion.LookRotation((rocketPos + new Vector3(0f, 3.5f, 0f)) - camPos, Vector3.up));
        }

        if (t >= 1f)
        {
            RestoreEarthAtmosphere();
            _phase = FlightPhase.IdleOnEarth;
            _pad.PlaceRocketOnEarth();
            TeleportPlayer(_pad.EarthArrivalPoint, Quaternion.Euler(0f, -90f, 0f));
            UnlockPlayerAfterFlight(_savedBaseGravity);
            Mod.Log("Rueckkehr nach Hyland Point abgeschlossen!");
        }
    }

    internal void DrawPromptIfNeeded()
    {
        if (IsInFlight)
        {
            string label = _phase == FlightPhase.LaunchingToMoon
                ? "Rakete steigt auf — Auf zum Mond..."
                : "Wiedereintritt — Rueckflug nach Hyland Point...";
            DrawBanner(label);
            return;
        }

        if (_playerNearRocket)
        {
            string prompt = _phase == FlightPhase.OnMoon
                ? "[E] Rueckflug zur Erde starten"
                : "[E] Rakete starten — Flug zum Mond";
            DrawBanner(prompt);
        }
    }

    private static void DrawBanner(string text)
    {
        float w = 380f;
        float h = 38f;
        Rect box = new((Screen.width - w) * 0.5f, Screen.height * 0.78f, w, h);
        GUI.Box(box, text);
    }

    private void UpdateProximity()
    {
        _playerNearRocket = false;
        if (_pad.RocketRoot == null || IsInFlight) return;
        var player = Player.Local;
        if (player == null) return;
        float sqr = (player.transform.position - _pad.RocketRoot.transform.position).sqrMagnitude;
        _playerNearRocket = sqr <= InteractDistance * InteractDistance;
    }

    private void CaptureEarthAtmosphere()
    {
        if (_hasSavedAtmosphere) return;
        _savedAmbientLight = RenderSettings.ambientLight;
        _savedFogColor = RenderSettings.fogColor;
        _savedFogDensity = RenderSettings.fogDensity;
        _savedFogEnabled = RenderSettings.fog;
        _savedSkybox = RenderSettings.skybox;
        var cam = Camera.main;
        if (cam != null)
        {
            _savedClearFlags = cam.clearFlags;
            _savedBgColor = cam.backgroundColor;
        }
        _savedBaseGravity = PlayerMovement.BaseGravityMultiplier > 0.01f
            ? PlayerMovement.BaseGravityMultiplier
            : 1f;
        _hasSavedAtmosphere = true;
    }

    private void ApplySpaceAtmosphere(float darkness01)
    {
        if (!_hasSavedAtmosphere) return;
        RenderSettings.ambientLight = Color.Lerp(_savedAmbientLight, new Color(0.22f, 0.24f, 0.30f), darkness01);
        RenderSettings.fogColor = Color.Lerp(_savedFogColor, new Color(0.01f, 0.012f, 0.025f), darkness01);
        RenderSettings.fogDensity = Mathf.Lerp(_savedFogDensity, 0.0002f, darkness01);

        if (darkness01 >= 0.85f)
        {
            RenderSettings.skybox = null;
            var cam = Camera.main;
            if (cam != null)
            {
                cam.clearFlags = CameraClearFlags.SolidColor;
                cam.backgroundColor = new Color(0.01f, 0.012f, 0.025f);
            }
        }
    }

    private void RestoreEarthAtmosphere()
    {
        if (!_hasSavedAtmosphere) return;
        RenderSettings.ambientLight = _savedAmbientLight;
        RenderSettings.fogColor = _savedFogColor;
        RenderSettings.fogDensity = _savedFogDensity;
        RenderSettings.fog = _savedFogEnabled;
        RenderSettings.skybox = _savedSkybox;
        var cam = Camera.main;
        if (cam != null)
        {
            cam.clearFlags = _savedClearFlags;
            cam.backgroundColor = _savedBgColor;
        }
    }

    private static void LockPlayerForFlight()
    {
        try
        {
            if (PlayerMovement.InstanceExists) PlayerMovement.Instance.CanMove = false;
            if (PlayerCamera.InstanceExists) PlayerCamera.Instance.SetCanLook(false);
        }
        catch (Exception ex)
        {
            Mod.Log("Warnung beim Sperren der Steuerung: " + ex.Message);
        }
    }

    private static void UnlockPlayerAfterFlight(float gravityMultiplier)
    {
        try
        {
            PlayerMovement.GravityMultiplier = gravityMultiplier;
            if (PlayerCamera.InstanceExists)
            {
                PlayerCamera.Instance.StopTransformOverride(0.4f, true, true);
                PlayerCamera.Instance.SetCanLook(true);
            }
            if (PlayerMovement.InstanceExists) PlayerMovement.Instance.CanMove = true;
        }
        catch (Exception ex)
        {
            Mod.Log("Warnung beim Freigeben der Steuerung: " + ex.Message);
        }
    }

    private static void OverrideCamera(Vector3 pos, Quaternion rot)
    {
        if (PlayerCamera.InstanceExists)
        {
            PlayerCamera.Instance.OverrideTransform(pos, rot, 0f, false);
        }
    }

    private static void TeleportPlayer(Vector3 pos, Quaternion rot)
    {
        if (PlayerMovement.InstanceExists)
        {
            PlayerMovement.Instance.Teleport(pos, rot, true);
        }
    }

    private void AbortFlightToEarth()
    {
        RestoreEarthAtmosphere();
        _phase = FlightPhase.IdleOnEarth;
        UnlockPlayerAfterFlight(_savedBaseGravity);
    }

    internal void ResetAllState()
    {
        _pad.SetThrustersActive(false);
        RestoreEarthAtmosphere();
        _hasSavedAtmosphere = false;
        _phase = FlightPhase.IdleOnEarth;
        _playerNearRocket = false;
        try
        {
            PlayerMovement.GravityMultiplier = _savedBaseGravity;
            if (PlayerCamera.InstanceExists)
            {
                PlayerCamera.Instance.StopTransformOverride(0f, true, true);
                PlayerCamera.Instance.SetCanLook(true);
            }
            if (PlayerMovement.InstanceExists) PlayerMovement.Instance.CanMove = true;
        }
        catch
        {
            // Scene may already be tearing down.
        }
    }
}
