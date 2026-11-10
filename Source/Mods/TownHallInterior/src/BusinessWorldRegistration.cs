using System;
using System.IO;

namespace TownHallInterior;

/// <summary>
/// Lifecycle state of the TownHall business registration in the game world.
/// </summary>
public enum WorldRegistrationState
{
    Unregistered,
    Preparing,
    Prepared,
    Registered,
    Deregistering,
    Faulted
}

/// <summary>
/// Immutable identity parameters for world and business list registration.
/// </summary>
public sealed record WorldRegistrationIdentity(
    string PropertyName = "Hyland Town Hall",
    string PropertyCode = "hyland_town_hall",
    string HierarchyPath = "@Businesses/HylandTownHall",
    string TargetOrganisation = "TownHallTest",
    string TargetSlot = "SaveGame_2");

/// <summary>
/// Snapshot of presence in native static lists and registries.
/// </summary>
public sealed record WorldRegistrationSnapshot(
    bool InBusinessList,
    bool InUnownedBusinessList,
    bool InOwnedBusinessList,
    bool InPropertyList,
    bool InUnownedPropertyList,
    bool InOwnedPropertyList,
    bool InSaveablesList);

/// <summary>
/// Port abstraction for native engine world publication and registration actions.
/// </summary>
public interface IBusinessWorldPort
{
    WorldRegistrationSnapshot CaptureSnapshot();
    bool RegisterInWorld(WorldRegistrationIdentity identity, bool isOwned);
    bool DeregisterFromWorld(WorldRegistrationIdentity identity);
}

/// <summary>
/// Result of a world registration or deregistration operation.
/// </summary>
public sealed record WorldRegistrationResult(
    WorldRegistrationState State,
    string Status,
    string? Reason = null,
    WorldRegistrationSnapshot? BeforeSnapshot = null,
    WorldRegistrationSnapshot? AfterSnapshot = null);

/// <summary>
/// Manages the deterministic lifecycle and registration of the TownHall business
/// within the game world, ensuring fail-closed isolation and cleanup.
/// </summary>
public sealed class BusinessWorldRegistrationManager
{
    private readonly object _sync = new();
    private readonly WorldRegistrationIdentity _identity;
    private WorldRegistrationState _state = WorldRegistrationState.Unregistered;
    private bool _isOwned;

    public BusinessWorldRegistrationManager(WorldRegistrationIdentity? identity = null)
    {
        _identity = identity ?? new WorldRegistrationIdentity();
    }

    public WorldRegistrationState State
    {
        get
        {
            lock (_sync) return _state;
        }
    }

    public WorldRegistrationIdentity Identity => _identity;

    public bool IsRegistered
    {
        get
        {
            lock (_sync) return _state == WorldRegistrationState.Registered;
        }
    }

    public WorldRegistrationResult Register(ConstructionContext? context, IBusinessWorldPort port, bool isOwned = false)
    {
        ArgumentNullException.ThrowIfNull(port);

        lock (_sync)
        {
            if (_state == WorldRegistrationState.Registered)
            {
                var currentSnapshot = port.CaptureSnapshot();
                return new WorldRegistrationResult(
                    _state,
                    "already-registered",
                    "Business is already registered in the world.",
                    currentSnapshot,
                    currentSnapshot);
            }

            string? gateError = ConstructionGate.Check(context, _identity.TargetOrganisation, _identity.TargetSlot);
            if (gateError != null)
            {
                return new WorldRegistrationResult(
                    _state,
                    "skipped",
                    gateError);
            }

            _state = WorldRegistrationState.Preparing;
            var beforeSnapshot = port.CaptureSnapshot();

            _state = WorldRegistrationState.Prepared;
            _isOwned = isOwned;

            try
            {
                bool success = port.RegisterInWorld(_identity, _isOwned);
                if (!success)
                {
                    _state = WorldRegistrationState.Faulted;
                    // Attempt fail-closed cleanup
                    try { port.DeregisterFromWorld(_identity); } catch { /* preserve fault */ }
                    var failedSnapshot = port.CaptureSnapshot();
                    return new WorldRegistrationResult(
                        _state,
                        "failed",
                        "Port failed to complete world registration.",
                        beforeSnapshot,
                        failedSnapshot);
                }

                _state = WorldRegistrationState.Registered;
                var afterSnapshot = port.CaptureSnapshot();

                return new WorldRegistrationResult(
                    _state,
                    "registered",
                    null,
                    beforeSnapshot,
                    afterSnapshot);
            }
            catch (Exception ex)
            {
                _state = WorldRegistrationState.Faulted;
                try { port.DeregisterFromWorld(_identity); } catch { /* preserve fault */ }
                var errorSnapshot = port.CaptureSnapshot();
                return new WorldRegistrationResult(
                    _state,
                    "faulted",
                    ex.Message,
                    beforeSnapshot,
                    errorSnapshot);
            }
        }
    }

    public WorldRegistrationResult Deregister(IBusinessWorldPort port)
    {
        ArgumentNullException.ThrowIfNull(port);

        lock (_sync)
        {
            if (_state == WorldRegistrationState.Unregistered)
            {
                var currentSnapshot = port.CaptureSnapshot();
                return new WorldRegistrationResult(
                    _state,
                    "noop",
                    "Business is already unregistered.",
                    currentSnapshot,
                    currentSnapshot);
            }

            _state = WorldRegistrationState.Deregistering;
            var beforeSnapshot = port.CaptureSnapshot();

            try
            {
                bool success = port.DeregisterFromWorld(_identity);
                var afterSnapshot = port.CaptureSnapshot();

                _state = success ? WorldRegistrationState.Unregistered : WorldRegistrationState.Faulted;

                return new WorldRegistrationResult(
                    _state,
                    success ? "deregistered" : "failed",
                    success ? null : "Port failed to deregister from world.",
                    beforeSnapshot,
                    afterSnapshot);
            }
            catch (Exception ex)
            {
                _state = WorldRegistrationState.Faulted;
                var errorSnapshot = port.CaptureSnapshot();
                return new WorldRegistrationResult(
                    _state,
                    "faulted",
                    ex.Message,
                    beforeSnapshot,
                    errorSnapshot);
            }
        }
    }

    public void Reset()
    {
        lock (_sync)
        {
            _state = WorldRegistrationState.Unregistered;
            _isOwned = false;
        }
    }
}
