using System;
using System.Globalization;
using System.IO;
using MelonLoader;
using S1API.Lifecycle;
using S1Mods.Shared;
using UnityEngine;

[assembly: MelonInfo(typeof(MoonRocks.Mod), "MoonRocks", "0.1.0", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace MoonRocks;

/// <summary>
/// MoonRocks Milestone 1: Earth launch pad, Planet-Crafter-style live rocket flight,
/// playable cratered Moon terrain with low gravity, and return flight.
/// </summary>
public sealed class Mod : MelonMod
{
    // Exact dock pier position & angle derived from Schedule_I_Main_FULL_DETAIL.blend (OutdoorFence line at X=-36.02, Z=6.33..18.33).
    private static readonly Vector3 DefaultLaunchPadPos = new(-41.50f, -2.30f, 12.33f);
    private const float DefaultLaunchPadYaw = 0f;

    internal static Mod? Instance { get; private set; }

    private readonly MoonMaterials _materials = new();
    private readonly MoonTerrainBuilder _moon = new();
    private readonly LaunchPadBuilder _pad = new();
    private FlightDirector _flight = null!;

    internal MoonTerrainBuilder Moon => _moon;
    internal LaunchPadBuilder Pad => _pad;
    internal FlightDirector Flight => _flight;
    internal bool IsWorldReady { get; private set; }
    internal bool CoordPickerEnabled { get; private set; } = false;

    private string? _lastPickBanner;
    private float _pickBannerUntil;

    internal void ToggleCoordPicker()
    {
        CoordPickerEnabled = !CoordPickerEnabled;
        Log($"Koordinaten-Picker (Mausrad): {(CoordPickerEnabled ? "AN" : "AUS")}");
    }

    internal static void Log(string msg) => MelonLogger.Msg("[MoonRocks] " + msg);

    public override void OnInitializeMelon()
    {
        Instance = this;
        _flight = new FlightDirector(_pad, _moon);

        GameLifecycle.OnLoadComplete += OnWorldLoaded;
        GameLifecycle.OnPreLoad += OnWorldLeaving;
        GameLifecycle.OnPreSceneChange += OnWorldLeaving;

        Log("Initialisiert (v0.1.0 — Meilenstein 1: Startrampe, Raketenflug & Mond).");
    }

    public override void OnDeinitializeMelon()
    {
        GameLifecycle.OnLoadComplete -= OnWorldLoaded;
        GameLifecycle.OnPreLoad -= OnWorldLeaving;
        GameLifecycle.OnPreSceneChange -= OnWorldLeaving;
        TeardownWorld();
        Instance = null;
    }

    public override void OnUpdate()
    {
        if (!IsWorldReady) return;
        _flight.Update(Time.deltaTime);

        if (CoordPickerEnabled && !_flight.IsInFlight)
        {
            if (Input.GetMouseButtonDown(2))
            {
                PickCrosshairCoordinates();
            }

            if (_flight.Phase == FlightPhase.IdleOnEarth)
            {
                float scroll = Input.mouseScrollDelta.y;
                if (Mathf.Abs(scroll) > 0.01f)
                {
                    NudgePadByScroll(scroll);
                }
            }
        }
    }

    private void NudgePadByScroll(float scroll)
    {
        float dir = Mathf.Sign(scroll);
        float step = (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) ? 0.25f : 1.0f;
        bool shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        Vector3 delta;
        if (shift)
        {
            delta = new Vector3(0f, dir * step, 0f);
        }
        else
        {
            var cam = Camera.main;
            Vector3 forward = cam != null ? cam.transform.forward : Vector3.forward;
            forward.y = 0f;
            if (forward.sqrMagnitude < 0.001f) forward = Vector3.forward;
            forward.Normalize();
            delta = forward * (dir * step);
        }

        Vector3 next = _pad.EarthPadCenter + delta;
        _pad.RepositionPad(next);

        string formatted = string.Format(
            CultureInfo.InvariantCulture,
            "new Vector3({0:F2}f, {1:F2}f, {2:F2}f)",
            next.x, next.y, next.z);
        try { GUIUtility.systemCopyBuffer = formatted; } catch { }
        SavePadCenterRecord(formatted);
        _lastPickBanner = $"[Rampe] {formatted} (Shift=Hoch/Runter, Strg=Fein)";
        _pickBannerUntil = Time.unscaledTime + 4.0f;
    }

    private static void SavePadCenterRecord(string formatted)
    {
        try
        {
            string outPath = Path.Combine(Application.dataPath, "..", "UserData", "MoonRocks_PadCenter.txt");
            File.WriteAllText(Path.GetFullPath(outPath), formatted + Environment.NewLine);
            Log("[PadCenter] " + formatted);
        }
        catch
        {
            // Non-fatal debug helper.
        }
    }

    public override void OnGUI()
    {
        if (!IsWorldReady) return;
        _flight.DrawPromptIfNeeded();

        if (!string.IsNullOrEmpty(_lastPickBanner) && Time.unscaledTime <= _pickBannerUntil)
        {
            GUI.Box(new Rect(20f, 20f, 520f, 34f), _lastPickBanner);
        }
    }

    private void PickCrosshairCoordinates()
    {
        var cam = Camera.main;
        if (cam == null) return;

        var ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (!Physics.Raycast(ray, out RaycastHit hit, 500f))
        {
            _lastPickBanner = "[Pick] Kein Ziel in Reichweite (500 m).";
            _pickBannerUntil = Time.unscaledTime + 3f;
            return;
        }

        Vector3 p = hit.point;
        string hitName = hit.collider != null ? hit.collider.gameObject.name : "?";
        string formatted = string.Format(
            CultureInfo.InvariantCulture,
            "new Vector3({0:F2}f, {1:F2}f, {2:F2}f)",
            p.x, p.y, p.z);

        try { GUIUtility.systemCopyBuffer = formatted; } catch { }

        string line = $"[{DateTime.Now:HH:mm:ss}] {formatted} // hit={hitName}";
        Log("[Pick] " + line);

        try
        {
            string outPath = Path.Combine(Application.dataPath, "..", "UserData", "MoonRocks_PickedCoords.txt");
            File.AppendAllText(Path.GetFullPath(outPath), line + Environment.NewLine);
        }
        catch (Exception ex)
        {
            Log("Konnte PickedCoords nicht schreiben: " + ex.Message);
        }

        if (Input.GetKey(KeyCode.LeftShift) && _flight.Phase == FlightPhase.IdleOnEarth)
        {
            _pad.RepositionPad(p);
            SavePadCenterRecord(formatted);
            _lastPickBanner = $"[Pick + Rampe versetzt] {formatted} ({hitName})";
        }
        else
        {
            _lastPickBanner = $"[Kopiert & gespeichert] {formatted} ({hitName})";
        }
        _pickBannerUntil = Time.unscaledTime + 4.5f;
    }

    private void OnWorldLoaded()
    {
        if (!NetworkGuard.IsHostOrSingleplayer())
        {
            Log("Nur Einzelspieler wird aktuell unterstuetzt; MoonRocks bleibt im Multiplayer deaktiviert.");
            return;
        }

        try
        {
            TeardownWorld();
            _materials.Initialize();
            _moon.Build(_materials);
            _pad.BuildAtExactPosition(DefaultLaunchPadPos, DefaultLaunchPadYaw, _materials);
            IsWorldReady = true;
            Log($"Welt bereit. Startrampe auf {_pad.EarthPadCenter}, Mond auf {_moon.LandingPadCenter}. Konsole: 'moon help'.");
        }
        catch (Exception ex)
        {
            Log("Fehler beim Aufbau der Mondwelt: " + ex);
            TeardownWorld();
        }
    }

    private void OnWorldLeaving()
    {
        TeardownWorld();
    }

    private void TeardownWorld()
    {
        IsWorldReady = false;
        _flight?.ResetAllState();
        _pad.Destroy();
        _moon.Destroy();
        _materials.DisposeAll();
    }
}
