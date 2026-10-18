using System;
using System.Diagnostics;
using MelonLoader;
using S1Mods.Shared;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Il2CppTMPro;

namespace GraphicsProbe;

public sealed class Mod
{
    private readonly CaptureSchedule _schedule = new(3, 5);
    private string _version = "";
    private readonly ModLogger _storageLog = new("GraphicsProbe");
    private readonly Stopwatch _clock = Stopwatch.StartNew();
    private bool _closed;
    private bool _inputErrorLogged;
    private CooperativeCapture? _capture;
    private RenderingSnapshot? _snapshot;
    private double _captureStartedMilliseconds;

    public void OnInitializeMelon(string version)
    {
        _version = version;
        MelonLogger.Msg("[GraphicsProbe] Manual capture only: Ctrl+Shift+F8. Scene load does not scan.");
    }

    public void OnSceneWasInitialized(int buildIndex, string sceneName) => CancelCapture();

    public void OnSceneWasLoaded(int buildIndex, string sceneName) => CancelCapture();

    public void OnSceneWasUnloaded(int buildIndex, string sceneName) => CancelCapture();

    public void OnUpdate()
    {
        if (_closed) return;
        // No scene discovery here unless a delayed, coalesced capture is actually due.
        try
        {
            if (Input.GetKeyDown(KeyCode.F8)
                && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                && InputFocusAllowsCapture())
                _schedule.Request(_clock.Elapsed.TotalSeconds, "manual");
        }
        catch (Exception ex)
        {
            if (!_inputErrorLogged)
            {
                _inputErrorLogged = true;
                MelonLogger.Warning("[GraphicsProbe] Manual input unavailable: " + ReportPolicy.Text(ex.Message));
            }
        }
        if (_capture == null && _schedule.TryBegin(_clock.Elapsed.TotalSeconds, out string reason)) Capture(reason);
        if (_capture != null)
        {
            _capture.StepFrame();
            if (_capture.Status != "running") FinalizeCapture();
        }
    }

    // Selected standard uGUI/TMP text input suppresses this chord. No global input state writes.
    private static bool InputFocusAllowsCapture()
    {
        if (!Application.isFocused) return false;
        var system = EventSystem.current;
        if (system == null) return true;
        var selected = system.currentSelectedGameObject;
        if (selected == null) return true;
        return selected.GetComponent<InputField>() == null && selected.GetComponent<TMP_InputField>() == null;
    }

    private void Capture(string reason)
    {
        _captureStartedMilliseconds = _clock.Elapsed.TotalMilliseconds;
        try
        {
            _snapshot = new RenderingSnapshot(reason, _version);
            _capture = new CooperativeCapture(_snapshot, () => _clock.Elapsed.TotalMilliseconds,
                activeBudget: ReportPolicy.DiscoveryMilliseconds);
        }
        catch (Exception ex)
        {
            _snapshot?.Dispose(); _snapshot = null;
            _capture?.Dispose(); _capture = null;
            _schedule.Complete(_clock.Elapsed.TotalSeconds);
            MelonLogger.Error("[GraphicsProbe] Capture initialization failed; previous report retained: " + ReportPolicy.Text(ex.Message));
        }
    }

    private void FinalizeCapture()
    {
        var capture = _capture!;
        var snapshot = _snapshot!;
        var finalization = Stopwatch.StartNew();
        bool success = false;
        try
        {
            if (capture.Failure != null) throw new InvalidOperationException("Collection failed; previous report retained.", capture.Failure);
            var report = snapshot.FinishReport(capture);
            string json = ReportPolicy.Serialize(report);
            string path = SafeStorage.GetUserDataPath("GraphicsProbe", "latest.json");
            if (!SafeStorage.SaveTextAtomic(path, json, _storageLog))
                throw new InvalidOperationException("SafeStorage write failed for " + path);
            // Validate the exact target, not only the helper's status.
            if (!string.Equals(System.IO.File.ReadAllText(path), json, StringComparison.Ordinal))
                throw new InvalidOperationException("Report readback mismatch for " + path);
            success = true;
            MelonLogger.Msg($"[GraphicsProbe] Captured cameras={snapshot.Cameras.Kept}/{snapshot.Cameras.Scanned}, renderers={snapshot.RenderersScanned}, unconfirmedWaterCandidates={snapshot.WaterCandidates.Kept}/{snapshot.WaterCandidates.Matched}, particleLineTrailMatches={snapshot.EffectMatches.Kept}/{snapshot.EffectMatches.Matched}, nodes={snapshot.NodesVisited}, discoveryTruncated={snapshot.DiscoveryTruncated}; {path}");
        }
        catch (Exception ex)
        {
            MelonLogger.Error("[GraphicsProbe] Capture failed; no success claimed; trigger again to retry: " + ReportPolicy.Text(ex.GetType().Name + ": " + ex.Message));
        }
        finally
        {
            snapshot.Dispose(); capture.Dispose();
            _capture = null; _snapshot = null;
            _schedule.Complete(_clock.Elapsed.TotalSeconds);
            MelonLogger.Msg($"[GraphicsProbe] Capture timing success={success}, status={capture.Status}, activeWorkMilliseconds={capture.ActiveMilliseconds:F3}, collectionWallMilliseconds={capture.WallMilliseconds:F3}, steps={capture.Steps}, frames={capture.Frames}, maxObservedStepMilliseconds={capture.MaxObservedStepMilliseconds:F3}, finalizationMilliseconds={finalization.Elapsed.TotalMilliseconds:F3}, totalWallMilliseconds={_clock.Elapsed.TotalMilliseconds - _captureStartedMilliseconds:F3}; cooperative slices are not hard frame-time limits; finalization is synchronous.");
        }
    }

    private void CancelCapture()
    {
        if (_capture == null) return;
        var capture = _capture;
        capture.Cancel();
        _snapshot?.Dispose(); _snapshot = null; _capture = null;
        _schedule.Complete(_clock.Elapsed.TotalSeconds);
        MelonLogger.Msg($"[GraphicsProbe] Capture cancelled/discarded; previous report retained, activeWorkMilliseconds={capture.ActiveMilliseconds:F3}, wallMilliseconds={capture.WallMilliseconds:F3}, steps={capture.Steps}, frames={capture.Frames}.");
    }

    public void OnDeinitializeMelon()
    {
        _closed = true;
        CancelCapture();
        _schedule.Shutdown();
    }
}
