using System;
using System.Text;
using MelonLoader;
using S1API.Lifecycle;
using S1Mods.Shared;
using UnityEngine;

[assembly: MelonInfo(typeof(NorthIsland.Mod), "NorthIsland", "0.1.0", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace NorthIsland;

/// <summary>
/// NorthIsland Milestone 1: Connects the incomplete Northeast Overpass to North Island Terrain,
/// adds a Northtown waterfront on-ramp and island off-ramp, and removes blocking barriers.
/// Includes a middle-mouse-button raycast picker to inspect any remaining invisible wall.
/// </summary>
public sealed class Mod : MelonMod
{
    internal static Mod? Instance { get; private set; }
    internal NorthIslandWorld World { get; } = new();
    internal bool IsWorldReady { get; private set; }
    internal bool RaycastToolEnabled { get; set; }

    private string? _lastHitBanner;
    private float _hitBannerUntil;
    private GUIStyle? _bannerStyle;

    public override void OnInitializeMelon()
    {
        Instance = this;
        GameLifecycle.OnPreLoad += HandleWorldTeardown;
        GameLifecycle.OnPreSceneChange += HandleWorldTeardown;
        GameLifecycle.OnLoadComplete += HandleWorldReady;
        MelonLogger.Msg("[NorthIsland] initialisiert (v0.1.0).");
    }

    public override void OnDeinitializeMelon()
    {
        GameLifecycle.OnPreLoad -= HandleWorldTeardown;
        GameLifecycle.OnPreSceneChange -= HandleWorldTeardown;
        GameLifecycle.OnLoadComplete -= HandleWorldReady;
        HideoutPropertyHost.DisposePatches();
        World.Teardown();
        IsWorldReady = false;
        Instance = null;
    }

    public override void OnUpdate()
    {
        if (!IsWorldReady) return;
        World.Tick(Time.deltaTime);
        bool ctrlHeld = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        if (Input.GetMouseButtonDown(2) && (RaycastToolEnabled || ctrlHeld))
        {
            RunWallRaycast();
        }
    }

    public override void OnGUI()
    {
        if (string.IsNullOrEmpty(_lastHitBanner) || Time.unscaledTime > _hitBannerUntil) return;
        _bannerStyle ??= new GUIStyle(GUI.skin.box)
        {
            fontSize = 14,
            alignment = TextAnchor.UpperLeft,
            wordWrap = true
        };
        GUI.Box(new Rect(20f, 20f, 760f, 110f), _lastHitBanner, _bannerStyle);
    }

    private void RunWallRaycast()
    {
        Camera? cam = Camera.main;
        if (cam == null)
        {
            MelonLogger.Warning("[NorthIsland:Raycast] Camera.main ist null.");
            return;
        }

        bool prevBackfaces = Physics.queriesHitBackfaces;
        Physics.queriesHitBackfaces = true;
        try
        {
            Ray eyeRay = new Ray(cam.transform.position, cam.transform.forward);
            Ray lowRay = new Ray(cam.transform.position + Vector3.down * 1.05f, cam.transform.forward);

            var eyeHits = Physics.RaycastAll(eyeRay, 25f, ~0, QueryTriggerInteraction.Collide);
            var lowHits = Physics.RaycastAll(lowRay, 25f, ~0, QueryTriggerInteraction.Collide);

            var sorted = new System.Collections.Generic.List<RaycastHit>();
            var seen = new System.Collections.Generic.HashSet<int>();

            void CollectHits(RaycastHit[]? arr)
            {
                if (arr == null) return;
                for (int i = 0; i < arr.Length; i++)
                {
                    var col = arr[i].collider;
                    if (col == null || col.gameObject.layer == 6 || col.gameObject.layer == 2 || col.gameObject.layer == 15) continue;
                    string rootName = col.transform.root.name;
                    if (rootName.Equals("_Temp", StringComparison.OrdinalIgnoreCase) ||
                        col.GetComponentInParent<Rigidbody>() != null) continue;
                    if (seen.Add(col.GetInstanceID()))
                    {
                        sorted.Add(arr[i]);
                    }
                }
            }

            CollectHits(eyeHits);
            CollectHits(lowHits);

            if (sorted.Count == 0)
            {
                _lastHitBanner = "[NorthIsland Raycast] Kein Collider vor dir (25m).";
                _hitBannerUntil = Time.unscaledTime + 6f;
                MelonLogger.Msg(_lastHitBanner);
                return;
            }

            sorted.Sort((a, b) => a.distance.CompareTo(b.distance));

            var sb = new StringBuilder();
            int shown = Math.Min(sorted.Count, 6);
            for (int i = 0; i < shown; i++)
            {
                var h = sorted[i];
                var col = h.collider;
                if (col == null) continue;
                string path = GetHierarchyPath(col.transform);
                string line = $"[{i + 1}] {h.distance:F2}m | '{col.gameObject.name}' | Layer={LayerMask.LayerToName(col.gameObject.layer)}({col.gameObject.layer}) | trig={col.isTrigger} | bounds={col.bounds.center} | size={col.bounds.size} | path={path}";
                MelonLogger.Msg("[NorthIsland:Raycast] " + line);
                if (i < 2) sb.AppendLine(line);
            }

            _lastHitBanner = sb.ToString().TrimEnd();
            _hitBannerUntil = Time.unscaledTime + 10f;
        }
        finally
        {
            Physics.queriesHitBackfaces = prevBackfaces;
        }
    }

    private static string GetHierarchyPath(Transform t)
    {
        var sb = new StringBuilder(t.name);
        var cur = t.parent;
        int depth = 0;
        while (cur != null && depth < 6)
        {
            sb.Insert(0, cur.name + "/");
            cur = cur.parent;
            depth++;
        }
        return sb.ToString();
    }

    private void HandleWorldReady()
    {
        if (!NetworkGuard.IsHostOrSingleplayer())
        {
            MelonLogger.Msg("[NorthIsland] Client-Session erkannt – Weltaufbau wird nur vom Host/Singleplayer ausgefuehrt.");
            return;
        }

        try
        {
            World.Build();
            IsWorldReady = true;
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[NorthIsland] Fehler beim Weltaufbau: {ex}");
        }
    }

    private void HandleWorldTeardown()
    {
        IsWorldReady = false;
        _lastHitBanner = null;
        _hitBannerUntil = 0f;
        World.Teardown();
    }
}
