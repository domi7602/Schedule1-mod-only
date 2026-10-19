namespace LightControl.Core;

/// <summary>
/// Thread-safe pure model. BeginSession is mandatory. All authoritative mutations are
/// serialized; external authorization sees a frozen target set and cannot reenter a
/// mutation. Returned deltas are data to be delivered by a separately verified transport.
/// </summary>
public sealed class LightStateStore
{
    private readonly object _gate = new();
    private readonly Dictionary<string, LampRegistration> _registrations = new(StringComparer.Ordinal);
    private readonly Dictionary<string, string> _runtimeToLogical = new(StringComparer.Ordinal);
    private Dictionary<string, LampStateEntry> _states = new(StringComparer.Ordinal);
    private readonly Dictionary<(string Sender, Guid Command), long> _seenCommands = new();
    private readonly Queue<(string Sender, Guid Command)> _commandOrder = new();
    private EffectState _effects = EffectState.Empty;
    private Guid _sessionId;
    private string? _saveId;
    private SessionMode _mode;
    private long _revision;
    private long _minimumSnapshotRevision;
    private double _lastHostTime;
    private bool _hasSession;
    private bool _hasSnapshot;
    private bool _restored;
    private bool _insideAuthorization;

    public Guid SessionId { get { lock (_gate) { EnsureSession(); return _sessionId; } } }
    public string SaveId { get { lock (_gate) { EnsureSession(); return _saveId!; } } }
    public SessionMode Mode { get { lock (_gate) { EnsureSession(); return _mode; } } }
    public long Revision { get { lock (_gate) { EnsureSession(); return _revision; } } }
    public EffectState Effects { get { lock (_gate) { EnsureSession(); return _effects; } } }
    public bool NeedsSnapshot { get { lock (_gate) { EnsureSession(); return _mode == SessionMode.Client && (!_hasSnapshot || _minimumSnapshotRevision > _revision); } } }
    public double LastHostTimeSeconds { get { lock (_gate) { EnsureSession(); return _lastHostTime; } } }

    /// <summary>New connection/save/mode requires a fresh session ID. Local lamp values are discarded.</summary>
    public void BeginSession(Guid sessionId, string saveId, SessionMode mode)
    {
        LightLimits.Session(sessionId);
        LightLimits.Identifier(saveId, nameof(saveId));
        LightLimits.EnumValue(mode, nameof(mode));
        lock (_gate)
        {
            EnsureNoAuthorization();
            if (_hasSession && _sessionId == sessionId)
                throw new ArgumentException("A session or mode transition must use a fresh session ID.", nameof(sessionId));
            Clear();
            _sessionId = sessionId;
            _saveId = saveId;
            _mode = mode;
            _hasSession = true;
        }
    }

    public void EndSession()
    {
        lock (_gate) { EnsureNoAuthorization(); Clear(); }
    }

    private void Clear()
    {
        _registrations.Clear();
        _runtimeToLogical.Clear();
        _states.Clear();
        _seenCommands.Clear();
        _commandOrder.Clear();
        _effects = EffectState.Empty;
        _sessionId = Guid.Empty;
        _saveId = null;
        _revision = 0;
        _minimumSnapshotRevision = 0;
        _lastHostTime = 0;
        _hasSession = false;
        _hasSnapshot = false;
        _restored = false;
    }

    /// <summary>
    /// Registers one logical fixture, not each renderer. Clients may bind before a
    /// snapshot; host/local registrations introduce default state only if none is pending.
    /// </summary>
    public StateDelta? Register(LampRegistration lamp, double hostTimeSeconds = 0)
    {
        if (lamp is null) throw new ArgumentNullException(nameof(lamp));
        LightLimits.Time(hostTimeSeconds, nameof(hostTimeSeconds));
        lock (_gate)
        {
            EnsureMutable();
            lamp.Identity.ValidateSession(_sessionId);
            if (_registrations.TryGetValue(lamp.Identity.Key, out var old))
            {
                if (old.RuntimeInstanceId != lamp.RuntimeInstanceId)
                    throw new InvalidOperationException("Persistent/logical identity collision. Explicitly rebind a verified unloaded instance.");
                if (old.PropertyId != lamp.PropertyId || old.GroupId != lamp.GroupId)
                    throw new InvalidOperationException("Move an existing instance explicitly before updating its registration.");
                _registrations[lamp.Identity.Key] = old.SetPresence(lamp.Presence);
                return null;
            }
            if (_runtimeToLogical.ContainsKey(lamp.RuntimeInstanceId))
                throw new InvalidOperationException("Runtime instance is already assigned to another logical fixture.");
            if (_registrations.Count >= LightLimits.MaxLamps)
                throw new InvalidOperationException("Lamp registry capacity reached.");
            bool introduceState = _mode != SessionMode.Client && !_states.ContainsKey(lamp.Identity.Key);
            if (introduceState)
            {
                CheckAuthorityTime(hostTimeSeconds);
                if (_states.Count >= LightLimits.MaxLamps || _revision == long.MaxValue)
                    throw new InvalidOperationException("Authoritative state or revision capacity reached.");
            }
            _registrations.Add(lamp.Identity.Key, lamp);
            _runtimeToLogical.Add(lamp.RuntimeInstanceId, lamp.Identity.Key);
            if (!introduceState) return null;
            var entry = new LampStateEntry(lamp.Identity, LampBaseState.Default);
            _states.Add(lamp.Identity.Key, entry);
            return CommitDelta(hostTimeSeconds, new[] { entry }, Array.Empty<LampIdentity>(), null);
        }
    }

    public void MarkTemporarilyUnloaded(LampIdentity identity)
    {
        lock (_gate)
        {
            EnsureMutable();
            var existing = RequiredRegistration(identity);
            _registrations[identity.Key] = existing.SetPresence(LampPresence.TemporarilyUnloaded);
        }
    }

    /// <summary>
    /// Adapter must prove this is the same continuing logical fixture. This operation
    /// preserves state; actual pickup/removal must use RemovePermanently instead.
    /// </summary>
    public void RebindTemporarilyUnloaded(LampIdentity identity, string newRuntimeInstanceId)
    {
        LightLimits.Identifier(newRuntimeInstanceId, nameof(newRuntimeInstanceId));
        lock (_gate)
        {
            EnsureMutable();
            var existing = RequiredRegistration(identity);
            if (existing.Presence != LampPresence.TemporarilyUnloaded)
                throw new InvalidOperationException("Only a known temporarily unloaded fixture can be rebound.");
            if (_runtimeToLogical.TryGetValue(newRuntimeInstanceId, out var other) && other != identity.Key)
                throw new InvalidOperationException("New runtime identity already belongs to another fixture.");
            var replacement = new LampRegistration(identity, newRuntimeInstanceId, existing.PropertyId, existing.GroupId);
            _runtimeToLogical.Remove(existing.RuntimeInstanceId);
            _runtimeToLogical[newRuntimeInstanceId] = identity.Key;
            _registrations[identity.Key] = replacement;
        }
    }

    public void Move(LampIdentity identity, string? propertyId, string? groupId = null)
    {
        lock (_gate)
        {
            EnsureMutable();
            var existing = RequiredRegistration(identity);
            var moved = existing.Relocate(propertyId, groupId);
            _registrations[identity.Key] = moved;
        }
    }

    /// <summary>Requires confirmed logical deletion. A missing renderer is insufficient evidence.</summary>
    public StateDelta? RemovePermanently(LampIdentity identity, double hostTimeSeconds)
    {
        if (identity is null) throw new ArgumentNullException(nameof(identity));
        lock (_gate)
        {
            EnsureAuthority();
            identity.ValidateSession(_sessionId);
            CheckAuthorityTime(hostTimeSeconds);
            if (!_states.ContainsKey(identity.Key)) return null;
            if (_revision == long.MaxValue) throw new InvalidOperationException("Revision capacity reached.");
            if (_registrations.Remove(identity.Key, out var registration))
                _runtimeToLogical.Remove(registration.RuntimeInstanceId);
            _states.Remove(identity.Key);
            return CommitDelta(hostTimeSeconds, Array.Empty<LampStateEntry>(), new[] { identity }, null);
        }
    }

    /// <summary>Detach a client's local object without pretending to delete the host's state.</summary>
    public void UnbindClientView(LampIdentity identity)
    {
        if (identity is null) throw new ArgumentNullException(nameof(identity));
        lock (_gate)
        {
            EnsureMutable();
            if (_mode != SessionMode.Client) throw new InvalidOperationException("This operation is only for a client view.");
            identity.ValidateSession(_sessionId);
            if (_registrations.Remove(identity.Key, out var registration))
                _runtimeToLogical.Remove(registration.RuntimeInstanceId);
        }
    }

    public IReadOnlyList<LampRegistration> GetRegistrations()
    {
        lock (_gate)
        {
            EnsureSession();
            return Array.AsReadOnly(_registrations.Values.OrderBy(lamp => lamp.Identity.Key, StringComparer.Ordinal).ToArray());
        }
    }

    public LampBaseState GetBaseState(LampIdentity identity)
    {
        if (identity is null) throw new ArgumentNullException(nameof(identity));
        lock (_gate)
        {
            EnsureSession();
            identity.ValidateSession(_sessionId);
            if (_states.TryGetValue(identity.Key, out var state)) return state.State;
            if (_registrations.ContainsKey(identity.Key)) return LampBaseState.Default;
            throw new KeyNotFoundException("Unknown logical fixture.");
        }
    }

    public VisualInstruction ResolveVisual(LampIdentity identity, double synchronizedTimeSeconds, bool modEnabled = true,
        double rainbowPeriodSeconds = LightLimits.RainbowPeriodSeconds)
    {
        lock (_gate)
        {
            EnsureSession();
            var lamp = RequiredRegistration(identity);
            var state = _states.TryGetValue(identity.Key, out var entry) ? entry.State : LampBaseState.Default;
            return VisualResolver.Resolve(identity.Key, lamp.PropertyId, state, _effects, synchronizedTimeSeconds, modEnabled, rainbowPeriodSeconds);
        }
    }

    public CommandResult ExecuteHost(ClientCommand request, string verifiedSenderId,
        Func<CommandAuthorizationContext, bool> authorize, double hostTimeSeconds)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));
        if (authorize is null) throw new ArgumentNullException(nameof(authorize));
        LightLimits.Identifier(verifiedSenderId, nameof(verifiedSenderId));
        LightLimits.Time(hostTimeSeconds, nameof(hostTimeSeconds));
        lock (_gate)
        {
            EnsureMutable();
            if (_mode != SessionMode.Host) return new(CommandStatus.WrongMode, _revision);
            if (request.ProtocolVersion != LightLimits.ProtocolVersion) return new(CommandStatus.WrongProtocol, _revision);
            if (request.SessionId != _sessionId) return new(CommandStatus.WrongSession, _revision);
            if (_seenCommands.ContainsKey((verifiedSenderId, request.CommandId))) return new(CommandStatus.Duplicate, _revision);
            if (request.ExpectedRevision != _revision) return new(CommandStatus.StaleRevision, _revision);
            CheckAuthorityTime(hostTimeSeconds);
            var targets = SelectTargets(request.Command.Selection);
            if (NeedsTargets(request.Command) && targets.Length == 0) return new(CommandStatus.UnknownTarget, _revision);
            var context = new CommandAuthorizationContext(verifiedSenderId, request.Command, targets);
            bool permitted;
            try
            {
                _insideAuthorization = true;
                permitted = authorize(context);
            }
            catch (Exception)
            {
                return new(CommandStatus.AuthorizationFailed, _revision);
            }
            finally { _insideAuthorization = false; }
            if (!permitted) return new(CommandStatus.Denied, _revision);
            var result = ApplyCommand(request.Command, targets, hostTimeSeconds);
            if (result.Status == CommandStatus.Applied)
            {
                var key = (verifiedSenderId, request.CommandId);
                _seenCommands.Add(key, result.Revision);
                _commandOrder.Enqueue(key);
                while (_commandOrder.Count > LightLimits.MaxRememberedCommands)
                    _seenCommands.Remove(_commandOrder.Dequeue());
            }
            return result;
        }
    }

    public CommandResult ExecuteLocal(ControlCommand command, double localTimeSeconds)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));
        lock (_gate)
        {
            EnsureMutable();
            if (_mode != SessionMode.Local) return new(CommandStatus.WrongMode, _revision);
            CheckAuthorityTime(localTimeSeconds);
            var targets = SelectTargets(command.Selection);
            if (NeedsTargets(command) && targets.Length == 0) return new(CommandStatus.UnknownTarget, _revision);
            return ApplyCommand(command, targets, localTimeSeconds);
        }
    }

    private CommandResult ApplyCommand(ControlCommand command, LampRegistration[] targets, double time)
    {
        if (_revision == long.MaxValue) return new(CommandStatus.RevisionExhausted, _revision);
        LampStateEntry[] updates = Array.Empty<LampStateEntry>();
        EffectState? effects = null;
        if (NeedsTargets(command))
        {
            // Construct every update before changing any state. Later arrivals are absent.
            updates = targets.Select(target =>
            {
                var previous = _states.TryGetValue(target.Identity.Key, out var existing) ? existing.State : LampBaseState.Default;
                var next = command.Kind == CommandKind.CycleColor ? previous.NextColor() : command.Change!.Apply(previous);
                return new LampStateEntry(target.Identity, next);
            }).ToArray();
        }
        else
        {
            if (command.Kind == CommandKind.StartDisco && command.Selection.Kind == SelectionKind.Property &&
                !_effects.Properties.ContainsKey(command.Selection.PropertyId!) && _effects.Properties.Count >= LightLimits.MaxPropertyEffects)
                return new(CommandStatus.CapacityExceeded, _revision);
            var effect = command.Kind == CommandKind.StartDisco
                ? new DiscoEffect(time, command.DiscoStepSeconds!.Value, command.DiscoSeed) : null;
            effects = _effects.Change(command.Selection, effect);
        }
        foreach (var update in updates) _states[update.Identity.Key] = update;
        if (effects is not null) _effects = effects;
        return new(CommandStatus.Applied, _revision + 1,
            CommitDelta(time, updates, Array.Empty<LampIdentity>(), effects));
    }

    private static bool NeedsTargets(ControlCommand command) => command.Kind is CommandKind.SetState or CommandKind.CycleColor;

    private LampRegistration[] SelectTargets(LampSelection selection) =>
        _registrations.Values.Where(selection.Includes).OrderBy(lamp => lamp.Identity.Key, StringComparer.Ordinal).ToArray();

    private StateDelta CommitDelta(double time, IEnumerable<LampStateEntry> upserts, IEnumerable<LampIdentity> removed, EffectState? effects)
    {
        var delta = new StateDelta(LightLimits.ProtocolVersion, _sessionId, _revision, _revision + 1, time, upserts, removed, effects);
        _revision++;
        _lastHostTime = time;
        return delta;
    }

    public StateSnapshot CreateHostSnapshot(double hostTimeSeconds)
    {
        lock (_gate)
        {
            EnsureMutable();
            if (_mode != SessionMode.Host) throw new InvalidOperationException("Only a mod host exports authoritative snapshots.");
            CheckAuthorityTime(hostTimeSeconds);
            _lastHostTime = hostTimeSeconds;
            return new(LightLimits.ProtocolVersion, _sessionId, _revision, hostTimeSeconds,
                _states.Values.OrderBy(state => state.Identity.Key, StringComparer.Ordinal), _effects);
        }
    }

    public SynchronizationResult ApplyHostSnapshot(StateSnapshot snapshot)
    {
        if (snapshot is null) throw new ArgumentNullException(nameof(snapshot));
        lock (_gate)
        {
            EnsureMutable();
            var mismatch = CheckEnvelope(snapshot.ProtocolVersion, snapshot.SessionId);
            if (mismatch is not null) return new(mismatch.Value, _revision);
            if (_hasSnapshot && snapshot.Revision <= _revision) return new(SynchronizationStatus.IgnoredStale, _revision);
            if (snapshot.Revision < _minimumSnapshotRevision) return new(SynchronizationStatus.NeedSnapshot, _revision);
            if (snapshot.HostTimeSeconds < _lastHostTime || !ValidEffects(snapshot.Effects, snapshot.HostTimeSeconds))
                return new(SynchronizationStatus.InvalidPayload, _revision);
            var replacement = ValidateEntries(snapshot.Lamps);
            if (replacement is null) return new(SynchronizationStatus.InvalidPayload, _revision);
            _states = replacement;
            _effects = snapshot.Effects;
            _revision = snapshot.Revision;
            _minimumSnapshotRevision = snapshot.Revision;
            _lastHostTime = snapshot.HostTimeSeconds;
            _hasSnapshot = true;
            return new(SynchronizationStatus.Applied, _revision);
        }
    }

    public SynchronizationResult ApplyHostDelta(StateDelta delta)
    {
        if (delta is null) throw new ArgumentNullException(nameof(delta));
        lock (_gate)
        {
            EnsureMutable();
            var mismatch = CheckEnvelope(delta.ProtocolVersion, delta.SessionId);
            if (mismatch is not null) return new(mismatch.Value, _revision);
            if (_hasSnapshot && delta.Revision <= _revision) return new(SynchronizationStatus.IgnoredStale, _revision);
            var updates = ValidateEntries(delta.Upserts);
            var removed = new HashSet<string>(StringComparer.Ordinal);
            if (updates is null || !ValidEffects(delta.Effects, delta.HostTimeSeconds))
                return new(SynchronizationStatus.InvalidPayload, _revision);
            foreach (var identity in delta.Removed)
            {
                if (!IdentityInSession(identity) || !removed.Add(identity.Key) || updates.ContainsKey(identity.Key))
                    return new(SynchronizationStatus.InvalidPayload, _revision);
            }
            if (!_hasSnapshot || delta.PreviousRevision != _revision)
            {
                _minimumSnapshotRevision = Math.Max(_minimumSnapshotRevision, delta.Revision);
                return new(SynchronizationStatus.NeedSnapshot, _revision);
            }
            if (delta.HostTimeSeconds < _lastHostTime) return new(SynchronizationStatus.InvalidPayload, _revision);
            var next = new Dictionary<string, LampStateEntry>(_states, StringComparer.Ordinal);
            foreach (var key in removed) next.Remove(key);
            foreach (var entry in updates) next[entry.Key] = entry.Value;
            if (next.Count > LightLimits.MaxLamps) return new(SynchronizationStatus.InvalidPayload, _revision);
            _states = next;
            if (delta.Effects is not null) _effects = delta.Effects;
            _revision = delta.Revision;
            _lastHostTime = delta.HostTimeSeconds;
            return new(SynchronizationStatus.Applied, _revision);
        }
    }

    private SynchronizationStatus? CheckEnvelope(int protocol, Guid session)
    {
        if (_mode != SessionMode.Client) return SynchronizationStatus.WrongMode;
        if (protocol != LightLimits.ProtocolVersion) return SynchronizationStatus.WrongProtocol;
        if (session != _sessionId) return SynchronizationStatus.WrongSession;
        return null;
    }

    private bool IdentityInSession(LampIdentity identity) => identity.SessionId is null || identity.SessionId == _sessionId;

    private Dictionary<string, LampStateEntry>? ValidateEntries(IEnumerable<LampStateEntry> entries)
    {
        var result = new Dictionary<string, LampStateEntry>(StringComparer.Ordinal);
        foreach (var entry in entries)
            if (!IdentityInSession(entry.Identity) || !result.TryAdd(entry.Identity.Key, entry)) return null;
        return result;
    }

    private static bool ValidEffects(EffectState? effects, double hostTime) => effects is null ||
        ((effects.World is null || effects.World.StartTimeSeconds <= hostTime) &&
         effects.Properties.Values.All(effect => effect.StartTimeSeconds <= hostTime));

    internal void RestorePersisted(Guid expectedSession, string expectedSave, IReadOnlyList<PersistedLampState> values)
    {
        lock (_gate)
        {
            EnsureMutable();
            if (_mode != SessionMode.Host || expectedSession != _sessionId || expectedSave != _saveId)
                throw new InvalidOperationException("Persistence belongs to this host session and this save only.");
            if (_restored || _revision != 0 || _states.Count != 0 || _registrations.Count != 0)
                throw new InvalidOperationException("Restore once, before registering fixtures or accepting commands.");
            var replacement = new Dictionary<string, LampStateEntry>(StringComparer.Ordinal);
            foreach (var value in values)
            {
                var identity = LampIdentity.Persistent(value.Identity);
                if (!replacement.TryAdd(identity.Key, new(identity, value.State)))
                    throw new ArgumentException("Duplicate persistent identity.", nameof(values));
            }
            if (replacement.Count > LightLimits.MaxLamps) throw new ArgumentException("Too many persistent lamps.", nameof(values));
            _states = replacement;
            _restored = true;
        }
    }

    internal IReadOnlyList<PersistedLampState> ExportPersisted(Guid expectedSession, string expectedSave)
    {
        lock (_gate)
        {
            EnsureSession();
            if (_mode != SessionMode.Host || expectedSession != _sessionId || expectedSave != _saveId || !_restored)
                throw new InvalidOperationException("Only the initialized current host save may export durable lamp data.");
            return Array.AsReadOnly(_states.Values.Where(entry => entry.Identity.PersistentId is not null)
                .OrderBy(entry => entry.Identity.Key, StringComparer.Ordinal)
                .Select(entry => new PersistedLampState(entry.Identity.PersistentId!, entry.State)).ToArray());
        }
    }

    private LampRegistration RequiredRegistration(LampIdentity identity)
    {
        if (identity is null) throw new ArgumentNullException(nameof(identity));
        identity.ValidateSession(_sessionId);
        return _registrations.TryGetValue(identity.Key, out var registration) ? registration :
            throw new KeyNotFoundException("Logical fixture is not currently registered.");
    }

    private void CheckAuthorityTime(double value)
    {
        LightLimits.Time(value, nameof(value));
        if (value < _lastHostTime) throw new ArgumentOutOfRangeException(nameof(value), "Authority time must not move backwards.");
    }

    private void EnsureSession()
    {
        if (!_hasSession) throw new InvalidOperationException("Begin a session before using lamp data.");
    }

    private void EnsureNoAuthorization()
    {
        if (_insideAuthorization) throw new InvalidOperationException("Authorization may inspect but cannot mutate the model.");
    }

    private void EnsureMutable() { EnsureSession(); EnsureNoAuthorization(); }
    private void EnsureAuthority()
    {
        EnsureMutable();
        if (_mode == SessionMode.Client) throw new InvalidOperationException("A client cannot mutate authoritative lamp state.");
    }
}
