using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Il2CppScheduleOne.Map;
using Il2CppScheduleOne.Networking;
using Il2CppScheduleOne.Persistence;
using S1API.Lifecycle;
using S1Mods.Shared;
using UnityEngine;

namespace Minimap;

/// <summary>
/// Explicit, bounded, read-only development observations. This is not a search quest or proof
/// of session eligibility. No native setters, save loads, pickups or world mutations are used.
/// </summary>
public sealed class MinimapSearchDevelopment : IDisposable
{
    private readonly ModLogger _log;
    private readonly Stopwatch _clock = Stopwatch.StartNew();
    private readonly SearchObservationGate _gate = new();
    private bool _initialized;
    private bool _loadCompleted;
    private string _lastSavePath = "";
    private string _pathFingerprint = "";

    public MinimapSearchDevelopment(ModLogger log) => _log = log;

    public void Initialize()
    {
        if (_initialized) return;
        GameLifecycle.OnPreSceneChange += OnPreSceneChange;
        GameLifecycle.OnPreLoad += OnPreLoad;
        GameLifecycle.OnLoadComplete += OnLoadComplete;
        _initialized = true;
        _log.Info("[MinimapSearchDev] Read-only developer observer ready, OFF by default; use 'minimap searchdev watch'. Search gameplay is not installed.");
    }

    public void Dispose()
    {
        _gate.Stop();
        if (_initialized)
        {
            GameLifecycle.OnPreSceneChange -= OnPreSceneChange;
            GameLifecycle.OnPreLoad -= OnPreLoad;
            GameLifecycle.OnLoadComplete -= OnLoadComplete;
        }
        _initialized = false;
        _loadCompleted = false;
        _lastSavePath = "";
        _pathFingerprint = "";
    }

    public void Tick()
    {
        if (_gate.Enabled && _gate.ShouldSample(_clock.Elapsed.TotalSeconds))
        {
            string observation = Observe();
            if (_gate.ShouldReport(observation))
                _log.Info($"[MinimapSearchDev] epoch={_gate.Epoch}; {observation}");
        }
        if (_gate.TryConsumeAutoStop())
            _log.Info("[MinimapSearchDev] Watch stopped at time/report budget; observations do not mark a key found.");
    }

    public void Command(List<string> args)
    {
        string op = args.Count > 1 ? args[1].ToLowerInvariant() : "status";
        switch (op)
        {
            case "watch":
                _gate.Start(_clock.Elapsed.TotalSeconds);
                _log.Info("[MinimapSearchDev] Watch ON for at most 10 minutes / 256 changed observations, 4 checks per second. Exact positions require separate 'sites' command.");
                Tick();
                break;
            case "stop":
                _gate.Stop();
                _log.Info("[MinimapSearchDev] Watch OFF.");
                break;
            case "status":
                _log.Info($"[MinimapSearchDev] watch={_gate.Enabled}; reports={_gate.Reports}; epoch={_gate.Epoch}; {Observe()}");
                break;
            case "sites":
                if (!_gate.CanObserve(_clock.Elapsed.TotalSeconds))
                {
                    _log.Warn("[MinimapSearchDev] Watch is OFF or expired; restart with 'minimap searchdev watch' before an exact-position development dump.");
                    return;
                }
                int offset = 0;
                if (args.Count > 2 && !int.TryParse(args[2], NumberStyles.None, CultureInfo.InvariantCulture, out offset))
                {
                    _log.Warn("[MinimapSearchDev] Usage: minimap searchdev sites [nonnegative offset]");
                    return;
                }
                DumpSites(offset);
                break;
            default:
                _log.Info("[MinimapSearchDev] Usage: minimap searchdev <watch|stop|status|sites [offset]>. Development reads only; no rewards or collection commands.");
                break;
        }
    }

    private void OnPreSceneChange() => Boundary("pre-scene-change", false);
    private void OnPreLoad() => Boundary("pre-load", false);
    private void OnLoadComplete() => Boundary("load-complete", true);

    private void Boundary(string name, bool completed)
    {
        _loadCompleted = completed;
        _gate.ResetEpoch();
        _lastSavePath = "";
        _pathFingerprint = "";
        if (_gate.CanObserve(_clock.Elapsed.TotalSeconds) && _gate.ShouldReport("boundary=" + name))
            _log.Info($"[MinimapSearchDev] epoch={_gate.Epoch}; boundary={name}; next observation is a baseline, NOT a collection event.");
    }

    private string Observe()
    {
        var result = new StringBuilder();
        result.Append("loadCompleteEvent=").Append(_loadCompleted);
        bool ready = false;
        bool worldCandidateValid = false;
        try
        {
            var load = LoadManager.Instance;
            if (NetworkGuard.IsAlive(load))
            {
                bool loading = load.IsLoading;
                bool loaded = load.IsGameLoaded;
                result.Append("; loading=").Append(loading).Append("; loaded=").Append(loaded);
                ready = !loading && loaded && NetworkGuard.IsInMainScene;
                var info = load.ActiveSaveInfo;
                if (info != null && !info.WasCollected && info.Pointer != IntPtr.Zero)
                {
                    string path = info.SavePath ?? "";
                    int slot = info.SaveSlotNumber;
                    long createdTicks = info.DateCreated.Ticks;
                    worldCandidateValid = !string.IsNullOrWhiteSpace(path) && slot >= 0 && createdTicks > 0;
                    if (worldCandidateValid)
                    {
                        if (!string.Equals(path, _lastSavePath, StringComparison.Ordinal))
                        {
                            _lastSavePath = path;
                            _pathFingerprint = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(path)));
                        }
                        result.Append("; candidatePathHash=").Append(_pathFingerprint);
                    }
                    else
                    {
                        _lastSavePath = "";
                        _pathFingerprint = "";
                        result.Append("; candidatePathHash=unavailable");
                    }
                    result.Append("; slot=").Append(slot)
                        .Append("; candidateCreatedTicks=").Append(createdTicks);
                    var metadata = info.MetaData;
                    if (metadata != null && !metadata.WasCollected && metadata.Pointer != IntPtr.Zero)
                    {
                        var created = metadata.CreationDate;
                        if (created != null && !created.WasCollected && created.Pointer != IntPtr.Zero)
                        {
                            // Constructing a managed date also rejects corrupt calendar fields.
                            var creation = new DateTime(created.Year, created.Month, created.Day,
                                created.Hour, created.Minute, created.Second, DateTimeKind.Unspecified);
                            result.Append("; candidateMetadataCreation=").Append(creation.ToString("O", CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            worldCandidateValid = false;
                            result.Append("; metadataCreation=unavailable");
                        }
                    }
                    else
                    {
                        worldCandidateValid = false;
                        result.Append("; metadata=unavailable");
                    }
                }
                else result.Append("; worldCandidate=unavailable");
            }
            else result.Append("; loadManager=unavailable");
        }
        catch (Exception ex)
        {
            ready = false;
            worldCandidateValid = false;
            result.Append("; loadReadError=").Append(ex.GetType().Name);
        }

        try
        {
            var lobby = Lobby.Instance;
            if (NetworkGuard.IsAlive(lobby))
                result.Append("; lobbyHost=").Append(lobby.IsHost)
                    .Append("; inLobby=").Append(lobby.IsInLobby)
                    .Append("; lobbyPlayers=").Append(lobby.PlayerCount);
            else result.Append("; lobby=unavailable");
        }
        catch (Exception ex) { result.Append("; sessionReadError=").Append(ex.GetType().Name); }

        if (!ready || !worldCandidateValid || !_loadCompleted) return result.Append("; keyRead=deferred-until-ready-world").ToString();
        try
        {
            var manager = SewerManager.Instance;
            if (NetworkGuard.IsAlive(manager))
            {
                result.Append("; collected=").Append(manager.IsRandomWorldKeyCollected)
                    .Append("; locationIndex=").Append(manager.RandomSewerKeyLocationIndex);
                try { result.Append("; pickupAlive=").Append(NetworkGuard.IsAlive(manager.RandomWorldSewerKeyPickup)); }
                catch (Exception ex) { result.Append("; pickupReadError=").Append(ex.GetType().Name); }
                try
                {
                    var locations = manager.RandomSewerKeyLocations;
                    result.Append("; locationCount=").Append(locations == null ? -1 : locations.Length);
                }
                catch (Exception ex) { result.Append("; locationsReadError=").Append(ex.GetType().Name); }
            }
            else result.Append("; sewerManager=unavailable");
        }
        catch (Exception ex) { result.Append("; keyReadError=").Append(ex.GetType().Name); }
        return result.ToString();
    }

    private void DumpSites(int offset)
    {
        try
        {
            var load = LoadManager.Instance;
            if (!_loadCompleted || !NetworkGuard.IsInMainScene || !NetworkGuard.IsAlive(load) || load.IsLoading || !load.IsGameLoaded)
            {
                _log.Warn("[MinimapSearchDev] Sites unavailable until a confirmed loaded world.");
                return;
            }
            var manager = SewerManager.Instance;
            if (!NetworkGuard.IsAlive(manager)) return;
            var locations = manager.RandomSewerKeyLocations;
            if (locations == null || locations.Length > 1024 || offset < 0 || offset >= locations.Length)
            {
                _log.Warn("[MinimapSearchDev] Location array/offset unavailable or outside the admitted bounds.");
                return;
            }
            int end = Math.Min(locations.Length, offset + 16);
            int successful = 0;
            var timer = Stopwatch.StartNew();
            for (int i = offset; i < end; i++)
            {
                if (timer.Elapsed.TotalMilliseconds >= 20) break;
                try
                {
                    var site = locations[i];
                    if (!NetworkGuard.IsAlive(site)) continue;
                    Vector3 p = site.position;
                    if (!float.IsFinite(p.x) || !float.IsFinite(p.y) || !float.IsFinite(p.z)) continue;
                    _log.Info(FormattableString.Invariant($"[MinimapSearchDev] EXACT-DEVELOPMENT site={i}/{locations.Length}; xyz=({p.x:R},{p.y:R},{p.z:R}); curated=false"));
                    successful++;
                }
                catch (Exception ex) { _log.Warn($"[MinimapSearchDev] site={i}; unavailable={ex.GetType().Name}"); }
            }
            _log.Info($"[MinimapSearchDev] Site dump observed {successful} entries in requested [{offset},{end}); count={locations.Length}; max16/time20ms budget; incomplete if fewer entries. This is not an accessibility or coverage verification.");
        }
        catch (Exception ex) { _log.Warn($"[MinimapSearchDev] Site dump unavailable: {ex.GetType().Name}"); }
    }
}
