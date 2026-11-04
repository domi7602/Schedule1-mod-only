using System;
using System.Collections.Generic;
using System.Linq;

namespace TownHallInterior;

public sealed record ProbeEntity(string Kind, string Identity, string ParentIdentity = "",
    string Code = "", string Guid = "", string Path = "");

public sealed record ProbeSnapshot(string Phase, int Frame, long MainGeneration, bool Complete,
    IReadOnlyList<string> Errors, IReadOnlyList<ProbeEntity> Entities, long Sequence = 0);

// "observed" describes only the supplied reads; it never grants permission to bind or mutate.
public sealed record ProbeAssessment(string Status, IReadOnlyList<string> Findings);

/// <summary>Bounded, game-free capture. Pointer identities are meaningful only within one snapshot.</summary>
public sealed class ProbeTrace
{
    private readonly int _maxSnapshots, _maxEntitiesPerSnapshot, _maxErrorsPerSnapshot;
    private readonly List<ProbeSnapshot> _snapshots = new();
    public IReadOnlyList<ProbeSnapshot> Snapshots => _snapshots.AsReadOnly();
    public bool Truncated { get; private set; }

    public ProbeTrace(int maxSnapshots = 64, int maxEntitiesPerSnapshot = 512, int maxErrorsPerSnapshot = 32)
    {
        if (maxSnapshots < 1) throw new ArgumentOutOfRangeException(nameof(maxSnapshots));
        if (maxEntitiesPerSnapshot < 1) throw new ArgumentOutOfRangeException(nameof(maxEntitiesPerSnapshot));
        if (maxErrorsPerSnapshot < 1) throw new ArgumentOutOfRangeException(nameof(maxErrorsPerSnapshot));
        _maxSnapshots = maxSnapshots;
        _maxEntitiesPerSnapshot = maxEntitiesPerSnapshot;
        _maxErrorsPerSnapshot = maxErrorsPerSnapshot;
    }

    /// <returns>False only when the snapshot itself was dropped at the trace limit.</returns>
    public bool Append(ProbeSnapshot snapshot)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        if (_snapshots.Count == _maxSnapshots) { Truncated = true; return false; }
        bool missing = snapshot.Entities == null || snapshot.Errors == null;
        var sourceEntities = snapshot.Entities ?? Array.Empty<ProbeEntity>();
        var sourceErrors = snapshot.Errors ?? Array.Empty<string>();
        bool cut = sourceEntities.Count > _maxEntitiesPerSnapshot || sourceErrors.Count > _maxErrorsPerSnapshot;
        var errors = sourceErrors.Take(_maxErrorsPerSnapshot).ToList();
        if (missing || cut)
        {
            if (errors.Count == _maxErrorsPerSnapshot) errors.RemoveAt(errors.Count - 1);
            errors.Add(cut ? "capture-truncated" : "capture-list-unreadable");
        }
        Truncated |= cut;
        // Copies prevent the capture adapter from changing earlier evidence on its next scan.
        _snapshots.Add(snapshot with
        {
            Complete = snapshot.Complete && !missing && !cut && errors.Count == 0,
            Errors = Array.AsReadOnly(errors.ToArray()),
            Entities = Array.AsReadOnly(sourceEntities.Take(_maxEntitiesPerSnapshot).ToArray())
        });
        return true;
    }

    public ProbeAssessment Analyze(long? mainGeneration = null)
    {
        const int maxFindings = 128;
        var findings = new List<string>();
        void Add(string value)
        {
            if (findings.Count < maxFindings) findings.Add(value);
            else findings[maxFindings - 1] = "findings-truncated";
        }
        if (Truncated) Add("trace-truncated");
        int count = 0;
        foreach (var snapshot in _snapshots)
        {
            if (mainGeneration.HasValue && snapshot.MainGeneration != mainGeneration.Value) continue;
            count++;
            string scope = $"generation={snapshot.MainGeneration},phase={snapshot.Phase},frame={snapshot.Frame}";
            if (!snapshot.Complete) Add(scope + ":capture-incomplete");
            foreach (string error in snapshot.Errors) Add(scope + ":read-error:" + error);
            var codes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var guids = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var entity in snapshot.Entities)
            {
                if (entity == null || string.IsNullOrWhiteSpace(entity.Identity))
                {
                    Add(scope + ":entity-identity-unreadable");
                    continue;
                }
                if (string.Equals(entity.Kind, "business", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrWhiteSpace(entity.Code)) Add(scope + ":business-code-uninitialized:" + entity.Identity);
                    else Check(codes, entity.Code, entity.Identity, "duplicate-business-code");
                }
                string guid = entity.Guid ?? "";
                bool validGuid = System.Guid.TryParse(guid, out var parsed);
                bool requiresGuid = string.Equals(entity.Kind, "grid", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(entity.Kind, "station", StringComparison.OrdinalIgnoreCase);
                if (requiresGuid && (!validGuid || parsed == System.Guid.Empty))
                    Add(scope + ":required-guid-uninitialized:" + entity.Identity);
                if (validGuid)
                    guid = parsed == System.Guid.Empty ? "" : parsed.ToString("D");
                Check(guids, guid, entity.Identity, "duplicate-guid");
            }

            void Check(Dictionary<string, string> seen, string key, string identity, string issue)
            {
                if (string.IsNullOrWhiteSpace(key)) return;
                key = key.Trim();
                if (seen.TryGetValue(key, out string? previous))
                {
                    if (!string.Equals(previous, identity, StringComparison.OrdinalIgnoreCase))
                        Add($"{scope}:{issue}:{key}:{previous}/{identity}");
                }
                else seen.Add(key, identity);
            }
        }
        if (count == 0) Add("no-snapshots");
        return new ProbeAssessment(findings.Count == 0 ? "observed" : "incomplete", findings.AsReadOnly());
    }
}

/// <summary>Local marker cycles, not native world or save identities. Ambiguous ordering stays explicit.</summary>
public sealed class ProbeLoadGeneration
{
    private bool _active;
    public long Generation { get; private set; }
    public bool MainPresent { get; private set; }
    public bool PreLoadObserved { get; private set; }
    public bool LoadCompleteObserved { get; private set; }
    public bool TransitionPending { get; private set; }
    // The adapter must retain this as incomplete evidence; a later marker cannot repair an ambiguous trace.
    public string? Anomaly { get; private set; }

    public long MainSeen()
    {
        EnsureCycle();
        MainPresent = true;
        return Generation;
    }

    public long PreLoad()
    {
        if (_active && (PreLoadObserved || LoadCompleteObserved))
            Anomaly ??= "preload-overlap-without-main-unload";
        EnsureCycle();
        PreLoadObserved = true;
        return Generation;
    }

    public void MainUnloaded()
    {
        if (_active && !MainPresent) Anomaly ??= "main-unload-before-main-seen";
        _active = false;
        MainPresent = false;
        TransitionPending = false;
    }

    // PreSceneChange may prepare the next scene; it is not proof that Main has already unloaded.
    public void Leaving() => TransitionPending = true;

    public bool LoadComplete()
    {
        if (!_active) { Anomaly ??= "load-complete-without-active-cycle"; return false; }
        if (!MainPresent) Anomaly ??= "load-complete-before-main-seen";
        LoadCompleteObserved = true;
        return true;
    }

    private void EnsureCycle()
    {
        if (_active) return;
        _active = true;
        Generation++;
        MainPresent = false;
        PreLoadObserved = false;
        LoadCompleteObserved = false;
        TransitionPending = false;
    }
}

public sealed record ProbeLoaderEvent(long Sequence, string Phase, int Frame, long MainGeneration,
    long CallId, string Method);

/// <summary>One local load's bounded loader observations, independent of inventory snapshot limits.</summary>
public sealed class ProbeLoaderTrace
{
    private readonly int _maxEvents;
    private readonly List<ProbeLoaderEvent> _events = new();
    public IReadOnlyList<ProbeLoaderEvent> Events => Array.AsReadOnly(_events.ToArray());
    public long DroppedEvents { get; private set; }

    public ProbeLoaderTrace(int maxEvents = 4096)
    {
        if (maxEvents < 1) throw new ArgumentOutOfRangeException(nameof(maxEvents));
        _maxEvents = maxEvents;
    }

    public bool Record(ProbeLoaderEvent entry)
    {
        ArgumentNullException.ThrowIfNull(entry);
        if (_events.Count == _maxEvents)
        {
            if (DroppedEvents < long.MaxValue) DroppedEvents++;
            return false;
        }
        _events.Add(entry);
        return true;
    }

    // Completeness of observed call pairs is not factory readiness or proof that every hook ran.
    public ProbeAssessment Analyze()
    {
        const int maxFindings = 128;
        var findings = new List<string>();
        void Add(string issue)
        {
            if (findings.Count < maxFindings) findings.Add(issue);
            else findings[maxFindings - 1] = "loader-findings-truncated";
        }
        if (DroppedEvents != 0) Add("loader-events-dropped:" + DroppedEvents);
        if (_events.Count == 0) Add("no-loader-events");
        var entered = new Dictionary<long, ProbeLoaderEvent>();
        var open = new HashSet<long>();
        long previousSequence = 0;
        long? generation = null;
        foreach (var entry in _events)
        {
            string scope = $"sequence={entry.Sequence},generation={entry.MainGeneration},call={entry.CallId}:";
            if (entry.Sequence <= 0 || entry.Sequence <= previousSequence) Add(scope + "invalid-sequence");
            previousSequence = entry.Sequence;
            generation ??= entry.MainGeneration;
            if (generation.Value != entry.MainGeneration) Add(scope + "mixed-generations");
            if (entry.CallId <= 0) { Add(scope + "invalid-call-id"); continue; }
            if (string.IsNullOrWhiteSpace(entry.Method)) { Add(scope + "missing-method"); continue; }
            if (entry.Phase == "enter")
            {
                if (!entered.TryAdd(entry.CallId, entry)) Add(scope + "duplicate-enter");
                else open.Add(entry.CallId);
            }
            else if (entry.Phase == "exit")
            {
                if (!entered.TryGetValue(entry.CallId, out var start)) Add(scope + "unmatched-exit");
                else if (!open.Contains(entry.CallId)) Add(scope + "duplicate-exit");
                else if (start.MainGeneration != entry.MainGeneration || !string.Equals(start.Method, entry.Method, StringComparison.Ordinal))
                    Add(scope + "mismatched-exit");
                else open.Remove(entry.CallId);
            }
            else Add(scope + "invalid-phase");
        }
        foreach (long callId in open) Add($"open-call:{callId}:{entered[callId].Method}");
        return new ProbeAssessment(findings.Count == 0 ? "observed" : "incomplete", findings.AsReadOnly());
    }
}

/// <summary>Publishes a new recording only after the caller reports successful persistence of the previous one.</summary>
public sealed class ProbeRecording
{
    private readonly int _maxSnapshots, _maxLoaderEvents;
    private bool _advancing;
    public long Generation { get; private set; }
    public ProbeTrace Snapshots { get; private set; }
    public ProbeLoaderTrace Loaders { get; private set; }

    public ProbeRecording(int maxSnapshots = 64, int maxLoaderEvents = 4096)
    {
        _maxSnapshots = maxSnapshots;
        _maxLoaderEvents = maxLoaderEvents;
        Snapshots = new ProbeTrace(maxSnapshots);
        Loaders = new ProbeLoaderTrace(maxLoaderEvents);
    }

    public bool TryAdvance(long generation, Func<bool> persistCurrent)
    {
        if (generation < 0 || generation < Generation) return false;
        if (generation == Generation) return true;
        if (_advancing || persistCurrent == null) return false;
        _advancing = true;
        try
        {
            var snapshots = new ProbeTrace(_maxSnapshots);
            var loaders = new ProbeLoaderTrace(_maxLoaderEvents);
            if (!persistCurrent()) return false;
            Snapshots = snapshots;
            Loaders = loaders;
            Generation = generation;
            return true;
        }
        catch (Exception) { return false; }
        finally { _advancing = false; }
    }
}
