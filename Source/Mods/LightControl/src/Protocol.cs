namespace LightControl.Core;

/// <summary>
/// Transport-neutral DTOs, not a network implementation. A transport must authenticate
/// the host/sender, decode bounded inputs, negotiate this protocol and synchronize time.
/// </summary>
public sealed record LampStateEntry
{
    public LampIdentity Identity { get; }
    public LampBaseState State { get; }

    public LampStateEntry(LampIdentity identity, LampBaseState state)
    {
        Identity = identity ?? throw new ArgumentNullException(nameof(identity));
        State = state ?? throw new ArgumentNullException(nameof(state));
    }
}

public sealed class StateSnapshot
{
    public int ProtocolVersion { get; }
    public Guid SessionId { get; }
    public long Revision { get; }
    public double HostTimeSeconds { get; }
    public IReadOnlyList<LampStateEntry> Lamps { get; }
    public EffectState Effects { get; }

    public StateSnapshot(int protocolVersion, Guid sessionId, long revision, double hostTimeSeconds,
        IEnumerable<LampStateEntry> lamps, EffectState effects)
    {
        LightLimits.Session(sessionId);
        if (revision < 0) throw new ArgumentOutOfRangeException(nameof(revision));
        ProtocolVersion = protocolVersion;
        SessionId = sessionId;
        Revision = revision;
        HostTimeSeconds = LightLimits.Time(hostTimeSeconds, nameof(hostTimeSeconds));
        Lamps = LightLimits.Copy(lamps, LightLimits.MaxLamps, nameof(lamps));
        Effects = effects ?? throw new ArgumentNullException(nameof(effects));
    }
}

public sealed class StateDelta
{
    public int ProtocolVersion { get; }
    public Guid SessionId { get; }
    public long PreviousRevision { get; }
    public long Revision { get; }
    public double HostTimeSeconds { get; }
    public IReadOnlyList<LampStateEntry> Upserts { get; }
    public IReadOnlyList<LampIdentity> Removed { get; }
    public EffectState? Effects { get; }

    public StateDelta(int protocolVersion, Guid sessionId, long previousRevision, long revision,
        double hostTimeSeconds, IEnumerable<LampStateEntry> upserts, IEnumerable<LampIdentity> removed, EffectState? effects)
    {
        LightLimits.Session(sessionId);
        if (previousRevision < 0 || previousRevision == long.MaxValue || revision != previousRevision + 1)
            throw new ArgumentException("A delta must cover exactly one consecutive revision.");
        ProtocolVersion = protocolVersion;
        SessionId = sessionId;
        PreviousRevision = previousRevision;
        Revision = revision;
        HostTimeSeconds = LightLimits.Time(hostTimeSeconds, nameof(hostTimeSeconds));
        Upserts = LightLimits.Copy(upserts, LightLimits.MaxLamps, nameof(upserts));
        Removed = LightLimits.Copy(removed, LightLimits.MaxLamps, nameof(removed));
        Effects = effects;
    }
}

public sealed class ControlCommand
{
    public CommandKind Kind { get; }
    public LampSelection Selection { get; }
    public LampChange? Change { get; }
    public double? DiscoStepSeconds { get; }
    public ulong DiscoSeed { get; }

    private ControlCommand(CommandKind kind, LampSelection selection, LampChange? change,
        double? stepSeconds, ulong seed)
    {
        Kind = kind;
        Selection = selection ?? throw new ArgumentNullException(nameof(selection));
        Change = change;
        DiscoStepSeconds = stepSeconds;
        DiscoSeed = seed;
    }

    public static ControlCommand Set(LampSelection selection, LampChange change) =>
        new(CommandKind.SetState, selection, change ?? throw new ArgumentNullException(nameof(change)), null, 0);
    public static ControlCommand Cycle(LampSelection selection) => new(CommandKind.CycleColor, selection, null, null, 0);
    public static ControlCommand StartDisco(LampSelection scope, double stepSeconds, ulong seed)
    {
        ValidateEffectScope(scope);
        _ = new DiscoEffect(0, stepSeconds, seed);
        return new(CommandKind.StartDisco, scope, null, stepSeconds, seed);
    }
    public static ControlCommand StopDisco(LampSelection scope)
    {
        ValidateEffectScope(scope);
        return new(CommandKind.StopDisco, scope, null, null, 0);
    }
    private static void ValidateEffectScope(LampSelection scope)
    {
        if (scope is null || (scope.Kind != SelectionKind.World && scope.Kind != SelectionKind.Property))
            throw new ArgumentException("Effects require a world or property scope.", nameof(scope));
    }
}

public sealed class ClientCommand
{
    public int ProtocolVersion { get; }
    public Guid SessionId { get; }
    public Guid CommandId { get; }
    public long ExpectedRevision { get; }
    public ControlCommand Command { get; }

    public ClientCommand(int protocolVersion, Guid sessionId, Guid commandId, long expectedRevision, ControlCommand command)
    {
        LightLimits.Session(sessionId);
        if (commandId == Guid.Empty) throw new ArgumentException("Command ID is required.", nameof(commandId));
        if (expectedRevision < 0) throw new ArgumentOutOfRangeException(nameof(expectedRevision));
        ProtocolVersion = protocolVersion;
        SessionId = sessionId;
        CommandId = commandId;
        ExpectedRevision = expectedRevision;
        Command = command ?? throw new ArgumentNullException(nameof(command));
    }
}

/// <summary>
/// Immutable snapshot to authorize as one operation. The integration checks the actual
/// sender and the verified shared-property model. No ownership flag is interpreted here.
/// </summary>
public sealed class CommandAuthorizationContext
{
    public string SenderId { get; }
    public ControlCommand Command { get; }
    public IReadOnlyList<LampRegistration> Targets { get; }

    internal CommandAuthorizationContext(string senderId, ControlCommand command, IEnumerable<LampRegistration> targets)
    {
        SenderId = senderId;
        Command = command;
        Targets = LightLimits.Copy(targets, LightLimits.MaxLamps, nameof(targets));
    }
}

public enum CommandStatus
{
    Applied, Duplicate, WrongMode, WrongProtocol, WrongSession, StaleRevision,
    UnknownTarget, Denied, AuthorizationFailed, CapacityExceeded, RevisionExhausted
}

public sealed record CommandResult(CommandStatus Status, long Revision, StateDelta? Delta = null);
public enum SynchronizationStatus { Applied, IgnoredStale, WrongMode, WrongProtocol, WrongSession, NeedSnapshot, InvalidPayload }
public sealed record SynchronizationResult(SynchronizationStatus Status, long Revision);
