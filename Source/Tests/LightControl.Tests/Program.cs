using System.Text.Json;
using LightControl.Core;

namespace LightControl.Core.Tests;

// Dependency-free core, delegate and disk-adapter checks plus supplementary source contracts.
// Fixture I/O uses the actual disk adapter/Shared writer with loader-path and slot-probe shims.
// No native Unity execution, game hook, gameplay, rendered layout or real transport is proven.
internal static class Program
{
    private static readonly Guid Session = Guid.Parse("78aca78a-3f15-45e4-9c66-352a65a2a407");
    private static int Main()
    {
        var tests = new (string Name, Action Run)[]
        {
            ("C01 Eleven-color cycle preserves brightness", ColorCycle),
            ("C02 Reject invalid color and intensity enums", InvalidStates),
            ("C03 Reject nonfinite/unsafe effect parameters", InvalidEffects),
            ("C04 Persistent identity has collision-safe encoding", IdentityEncoding),
            ("C05 Reject missing identities and invalid Unicode", InvalidIdentities),
            ("C06 Reject duplicate logical and runtime instances", RegistryCollisions),
            ("C07 Temporary unload and explicit rebind preserve state", TemporaryUnload),
            ("C08 Permanent removal and replacement use default state", Replacement),
            ("C09 Move preserves base state and changes effective scope", MoveScope),
            ("C10 New group members do not inherit previous group edits", GroupNewcomer),
            ("C11 Temporarily unloaded lamps are not state-command targets", UnloadedGroup),
            ("C12 Group authorization is atomic", DeniedGroup),
            ("C13 Authorization cannot reenter a mutation", ReentrantAuthorization),
            ("C14 Authorization exceptions preserve all state", FailingAuthorization),
            ("C15 Host rejects stale command revisions", StaleCommand),
            ("C16 Host rejects wrong protocol and session", WrongCommandEnvelope),
            ("C17 Duplicate command is not executed twice", DuplicateCommand),
            ("C18 Command IDs are scoped to verified sender", SenderScopedCommands),
            ("C19 Missing targets and unauthorized properties are rejected", UnknownTargets),
            ("C20 Client cannot become a local authority implicitly", ClientRestrictions),
            ("C21 Snapshot may precede object registration", DeferredSnapshot),
            ("C22 Early delta prevents older initial snapshot overwrite", InitialSnapshotRace),
            ("C23 Gap delta requires sufficiently new snapshot", DeltaGap),
            ("C24 Delayed old snapshot cannot roll back a newer delta", OldSnapshot),
            ("C25 Invalid duplicate snapshot is rejected atomically", InvalidSnapshot),
            ("C26 Invalid overlapping delta is rejected atomically", InvalidDelta),
            ("C27 Wrong-session ephemeral payload does not enter model", WrongIdentitySession),
            ("C28 Unknown remote lamps wait for local registration", DeferredDelta),
            ("C29 Snapshot/delta constructors enforce collection bounds", ProtocolBounds),
            ("C30 World/property disco precedence restores underlying effects", EffectPriority),
            ("C31 Color edit under disco changes durable base only", BaseColorDuringDisco),
            ("C32 Brightness edit under disco acts immediately", BrightnessDuringDisco),
            ("C33 New lamp starts default and joins active disco", NewLampDuringDisco),
            ("C34 Shared time and stable identity reproduce disco color", DeterministicDisco),
            ("C35 Rainbow wraps at configurable period", RainbowPeriod),
            ("C36 Disabled mode removes all visual influence", DisabledVisuals),
            ("C37 Original color leaves current game color intact", OriginalVisuals),
            ("C38 Intensity uses fresh baseline and leaves zero off", FreshNativeBaseline),
            ("C39 Nonfinite or overflowing native intensity is rejected", InvalidNativeIntensity),
            ("C40 Persistence cannot write before initial load", WriteBeforeLoad),
            ("C41 Corrupt data causes write lock", CorruptPersistence),
            ("C42 Missing declaration cannot bypass load-error lock", MissingAfterFailure),
            ("C43 Unknown future schema remains write locked", FutureSchema),
            ("C44 Duplicate JSON fields and lamp IDs are rejected", DuplicateJson),
            ("C45 Invalid persisted state is rejected before mutation", InvalidPersistedState),
            ("C46 Repeated load cycles preserve distinct identical fixtures", RepeatedPersistence),
            ("C47 Save A cannot read or overwrite save B", SaveIsolation),
            ("C48 Unregistered durable states survive saving", PendingPersistence),
            ("C49 Temporary effects and ephemeral IDs are not saved", PersistenceExclusions),
            ("C50 Local lamp state never becomes a persisted host state", LocalModeTransition),
            ("C51 Old messages are rejected after session transition", SessionTransition),
            ("C52 Loading after state mutation does not overwrite it", LatePersistenceLoad),
            ("C53 Actual deletion removes persisted state", PersistentDeletion),
            ("C54 Valid recovered JSON can unblock a failed initial load", RecoverInitialLoad),
            ("C55 Oversized/nested/null JSON fails safely", JsonBounds),
            ("C56 Protocol and model collections are defensive copies", DefensiveCopies),
            ("C57 Concurrent commands from same revision have one winner", ConcurrentCommands),
            ("C58 Repeated menu sessions clear registrations and effects", RepeatedSessions),
            ("C59 Authority time cannot run backwards", MonotonicTime),
            ("C60 Effect timestamps cannot originate after envelope time", FutureEffectTimestamp),
            ("C61 Duplicate successful load notification preserves write gate", RepeatedLoadNotification),
            ("C62 Later actual storage read failure blocks writes", LaterReadFailure),
            ("C63 Native policy writes nothing before it owns a light", NativeUndoBeforeApply),
            ("C64 Native 2x rebases on the fresh sample without compounding", NativeNoCompounding),
            ("C65 Native scaling follows external baseline drift", NativeExternalDrift),
            ("C66 Native scaling keeps zero and native flicker", NativeFlickerAndZero),
            ("C67 Original restores the latest native baseline", NativeOriginalBaseline),
            ("C68 Invalid or overflowing native samples write nothing", NativeInvalidSamples),
            ("C69 Native teardown restores the baseline once", NativeRestoreOnce),
            ("C70 Native identity rejects missing, empty and malformed GUIDs", NativeGuidRejected),
            ("C71 Native identity accepts a verified placed GUID", NativeGuidAccepted),
            ("C72 Save slot -1 never yields a save id", SlotSaveId),
            ("C73 Load decision separates missing, unreadable and valid data", SlotLoadDecision),
            ("C74 A write requires the captured active slot", SlotWriteGuard),
            ("C75 Reconcile scans obey a bounded cadence", ScanCadenceBound),
            ("C76 Invalid time and nonpositive interval are rejected", ScanCadenceInvalid),
            ("C77 Color cycle visits all eleven modes", ColorCycleAllEleven),
            ("C78 Intensity cycle visits all seven levels", IntensityCycleAllSeven),
            ("C79 Original action resets to the model default", OriginalReset),
            ("C80 Native undo preserves an external write after late-update", NativeExternalUndoPreserved),
            ("C81 Native undo restores the baseline when nothing else wrote", NativeUndoRestoresBaseline),
            ("C82 Native float round-trip is not misread as an external write", NativeFloatRoundtrip),
            ("C83 Native teardown preserves an external write and releases ownership", NativeTeardownExternal),
            ("C84 Native failed setter never claims ownership", NativeFailedWrite),
            ("C85 Native repeated 2x stays 2x and zero stays zero", NativeNoCompoundRepeat),
            ("C86 Color undo preserves an external write and restores otherwise", NativeColorOwnership),
            ("C87 Color failed setter never claims ownership", NativeColorFailedWrite),
            ("C88 Owned-property gate requires a live owned property", PropertyGate),
            ("C89 Scan budget bounds nodes and milliseconds per frame", ScanBudgetBound),
            ("C90 Scan budget reports limits and truncation", ScanBudgetLimits),
            ("C91 Fair root order rotates across scans", FairRootOrder),
            ("C92 Light pointer dedupe keeps the first distinct unowned pointer", LightDedupe),
            ("C93 Persistence write uses the strict slot guard", SlotWriteWiring),
            ("C94 Persist retries a failed write at the next autosave", PersistRetryWiring),
            ("C95 Mod deinitializes symmetrically", DeinitWiring),
            ("C96 Ownership gate and unload/rebind wiring", OwnershipWiring),
            ("C97 Cooperative bounded scan wiring", ScanWiring),
            ("C98 App removes the exact wired UnityEvent without a button gate", AppCleanupWiring),
            ("C99 App action failure is reported, not silently refreshed", AppFailureWiring),
            ("C100 Blocked lamp-data read surfaces a status message", PersistenceStatusWiring),
            ("C101 LED surface slot classification", LedSlotClassify),
            ("C102 Foreign or empty surface slot is never overridden", LedSlotForeign),
            ("C103 A switched-off lamp is never recoloured", LedSlotLampOff),
            ("C104 LED surface colour scales the native emission magnitude", LedSurfaceColor),
            ("C105 Original brightness scales the native hue only", LedSurfaceOriginalBrightness),
            ("C106 Zero percent yields zero emission without switching the lamp", LedSurfaceZero),
            ("C107 LED surface rebases on the native baseline and never compounds", LedSurfaceNoCompound),
            ("C108 Invalid surface baseline writes nothing and releases ownership", LedSurfaceInvalid),
            ("C109 A failed surface write never claims ownership", LedSurfaceFailedWrite),
            ("C110 Teardown restores the native slot exactly once", LedSurfaceRestoreOnce),
            ("C111 Original at 100 percent leaves no owned material influence", LedSurfaceOriginalReset),
            ("C112 Switching the lamp off restores our own clone slot", LedSurfaceOffRestoresClone),
            ("C113 An owned clone slot keeps ownership while the lamp stays on", LedSurfaceOwnCloneKeeps),
            ("C114 Every surface brightness level scales the baseline", LedSurfaceAllLevels),
            ("C115 Owned output matching compares the exact installed floats", LedSurfaceOwnedMatch),
            ("C116 Surface sample validation rejects unsafe values", LedSurfaceValidity),
            ("C117 Runtime drives only the lamp's own panel and emitted light", UnifiedScopeWiring),
            ("C118 Runtime touches only shared materials and owns its clones", SurfaceCloneWiring),
            ("C119 Runtime restores and releases owned surface resources", SurfaceLifecycleWiring),
            ("C120 App states the unified panel and emitted-light scope", AppUnifiedScopeWiring),
            ("C121 LED surface clone source stays the installed native material", LedOwnershipSourceStable),
            ("C122 LED surface clone source never drifts across changes", LedOwnershipSourceNeverDrifts),
            ("C123 LED surface release is deferred while unproven", LedOwnershipReleaseDeferred),
            ("C124 LED surface release is safe only when proven disconnected", LedOwnershipReleaseSafe),
            ("C125 Runtime uses the ownership helper and retains unproven releases", LedOwnershipWiring),
            ("C126 Native material identity needs a usable nonzero id", NativeIdentityUsableId),
            ("C127 Distinct wrappers of one native material are the same material", NativeIdentityDistinctWrappers),
            ("C128 An unreadable or disposed material handle is never equal", NativeIdentityUnreadable),
            ("C129 An incomplete material-array scan never proves disconnection", NativeArrayScanIncomplete),
            ("C130 Second and third colours reuse the same clone without compounding", NativeIdentityReusesOwnClone),
            ("C131 Native identity restores across wrappers and rejects a same-named foreigner", NativeIdentityRestoresAcrossWrappers),
            ("C132 Native-id clone reuse keeps the source stable", LedNeedsCloneNativeIdentity),
            ("C133 A deferred restore is never converged and must retry", LedRestoreConverged),
            ("C134 Runtime decides material identity by native id only", NativeIdentityWiring),
            ("C135 Coupled lamp readiness fails closed without both outputs", CoupledReadiness),
            ("C136 A chosen colour is the same hue on panel and emitted light", CoupledSameHue),
            ("C137 Second and third picks update both through distinct wrappers", CoupledSecondAndThirdPick),
            ("C138 Original restores the panel slot and the emitted light", CoupledOriginalRestoresBoth),
            ("C139 A switched-off lamp never emits our illumination", CoupledSwitchedOff),
            ("C140 Emitted intensity scales each level against its own baseline", CoupledIntensityLevels),
            ("C141 An external write after late-update is preserved", CoupledExternalWritePreserved),
            ("C142 One failed source cannot block another lamp's cleanup", CoupledFailureIsolation),
            ("C143 Runtime couples the panel with the lamp's own native light", CoupledWiring),
            ("C144 Verified panel pairs are LED and grow-tent halogen only", PanelFamilies),
            ("C145 The prologue RV is never tracked, even while it still exists", DestroyedParentProperty),
            ("C146 Only a grow tent contributes a built-in light", GrowTentItemGate),
            ("C147 Runtime drops exploded-RV lamps and registers grow-tent lights", TentAndRvWiring),
            ("C148 The lamp list keeps the selected row when the filter still shows it", LampListFilter),
            ("C149 A palette pick sets that color instead of cycling", ColorPalettePick),
            ("C150 Lamps stay under the property they were first seen in", PropertyGroups),
            ("A01-A20 Selection, available targets, preview and save notices", PlanRules)
        };
        tests = tests.Concat(AuditRuntimeChecks.Cases)
            .Concat(PersistenceRepairChecks.Cases())
            .Concat(NativeOwnershipChecks.Cases)
            .Concat(DiskAdapterChecks.Cases)
            .Concat(UiRepairChecks.Cases)
            .Concat(ColorPickerChecks.Cases)
            .Concat(GraphiteUiChecks.Cases).ToArray();
        int failures = 0;
        Console.WriteLine($"LightControl.Core behavioral checks | runtime {Environment.Version} | {tests.Length} cases");
        foreach (var test in tests)
        {
            try { test.Run(); Console.WriteLine($"PASS {test.Name}"); }
            catch (Exception exception)
            {
                failures++;
                Console.WriteLine($"FAIL {test.Name}: {exception.GetType().Name}: {exception.Message}");
            }
        }
        Console.WriteLine($"RESULT: {tests.Length - failures}/{tests.Length} passed; {failures} failed. Core/adapter/source checks; no native-game verification.");
        return failures == 0 ? 0 : 1;
    }

    private static LightStateStore Store(SessionMode mode = SessionMode.Host, Guid? session = null, string save = "save-A")
    {
        var store = new LightStateStore();
        store.BeginSession(session ?? Session, save, mode);
        return store;
    }

    private static LampIdentity Id(string value) => LampIdentity.Persistent(new(PersistentIdentityKind.VerifiedNativeInstance, "test-native", 1, value));
    private static LampRegistration Lamp(string value, string? property = "P1", string? group = "G1", string? runtime = null) =>
        new(Id(value), runtime ?? $"runtime-{value}", property, group);
    private static CommandResult Command(LightStateStore store, ControlCommand command, double time = 1, string sender = "verified-test-peer") =>
        store.ExecuteHost(new(LightLimits.ProtocolVersion, store.SessionId, Guid.NewGuid(), store.Revision, command), sender, _ => true, time);
    private static CommandResult Set(LightStateStore store, LampSelection selection, ColorMode color, double time = 1) =>
        Command(store, ControlCommand.Set(selection, new(color)), time);
    private static JsonStatePersistence Missing(LightStateStore store)
    {
        var persistence = new JsonStatePersistence(store);
        Equal(true, persistence.ConfirmDataMissing().Success);
        return persistence;
    }
    private static StateSnapshot Snapshot(Guid session, long revision, IEnumerable<LampStateEntry> values, double time = 0, EffectState? effects = null) =>
        new(LightLimits.ProtocolVersion, session, revision, time, values, effects ?? EffectState.Empty);
    private static StateDelta Delta(Guid session, long previous, IEnumerable<LampStateEntry> values, double time = 0,
        IEnumerable<LampIdentity>? removed = null, EffectState? effects = null) =>
        new(LightLimits.ProtocolVersion, session, previous, previous + 1, time, values, removed ?? Array.Empty<LampIdentity>(), effects);
    private static LampStateEntry Entry(string value, ColorMode color = ColorMode.Red) => new(Id(value), new(color, IntensityLevel.Percent100));
    private static string SavedJson(ColorMode color = ColorMode.Purple)
    {
        var store = Store(); var persistence = Missing(store); store.Register(Lamp("one"));
        Set(store, LampSelection.World(), color);
        return persistence.CreateJsonForSave();
    }

    private static void ColorCycle()
    {
        var state = new LampBaseState(ColorMode.Original, IntensityLevel.Percent150);
        var seen = new HashSet<ColorMode>();
        for (int i = 0; i < 11; i++) { Equal((ColorMode)i, state.Color); seen.Add(state.Color); state = state.NextColor(); }
        Equal(11, seen.Count); Equal(ColorMode.Original, state.Color); Equal(IntensityLevel.Percent150, state.Intensity);
    }
    private static void InvalidStates()
    {
        Throws<ArgumentOutOfRangeException>(() => new LampBaseState((ColorMode)11, IntensityLevel.Percent100));
        Throws<ArgumentOutOfRangeException>(() => new LampBaseState(ColorMode.Red, (IntensityLevel)99));
        Throws<ArgumentOutOfRangeException>(() => new LampChange((ColorMode)(-1)));
        Throws<ArgumentException>(() => new LampChange());
    }
    private static void InvalidEffects()
    {
        foreach (double bad in new[] { double.NaN, double.PositiveInfinity, double.NegativeInfinity, -1, 0, 0.09, 61 })
            Throws<ArgumentOutOfRangeException>(() => new DiscoEffect(0, bad, 0));
        foreach (double bad in new[] { double.NaN, double.PositiveInfinity, -1, LightLimits.MaxSessionTimeSeconds + 1 })
            Throws<ArgumentOutOfRangeException>(() => new DiscoEffect(bad, 1, 0));
        Throws<ArgumentException>(() => ControlCommand.StartDisco(LampSelection.Lamp(Id("one")), 1, 0));
    }
    private static void IdentityEncoding()
    {
        var a = new PersistentLampId(PersistentIdentityKind.VerifiedWorldLocator, "a:b", 1, "c");
        var b = new PersistentLampId(PersistentIdentityKind.VerifiedWorldLocator, "a", 1, "b:c");
        NotEqual(a.CanonicalKey, b.CanonicalKey);
        NotEqual(Id("one").Key, Id("two").Key);
        Equal(Id("one"), Id("one"));
        Equal(0xa430d84680aabd0bUL, VisualResolver.StableHash("hello"));
    }
    private static void InvalidIdentities()
    {
        Throws<ArgumentException>(() => Id("")); Throws<ArgumentException>(() => Id(" item "));
        Throws<ArgumentException>(() => Id("bad\nvalue")); Throws<ArgumentException>(() => Id("\ud800"));
        Throws<ArgumentException>(() => LampIdentity.Ephemeral(Guid.Empty, "one"));
        Throws<ArgumentException>(() => LampIdentity.Ephemeral(Session, ""));
        Throws<ArgumentOutOfRangeException>(() => new PersistentLampId((PersistentIdentityKind)0, "item-definition", 1, "same-item"));
        Throws<ArgumentException>(() => new LampRegistration(Id("one"), "", "P1"));
        Throws<ArgumentException>(() => new LampRegistration(Id("one"), "one", null, "group"));
    }
    private static void RegistryCollisions()
    {
        var store = Store(); store.Register(Lamp("one"));
        Equal<StateDelta?>(null, store.Register(Lamp("one")));
        Throws<InvalidOperationException>(() => store.Register(Lamp("one", runtime: "another-object")));
        Throws<InvalidOperationException>(() => store.Register(Lamp("two", runtime: "runtime-one")));
        Equal(1, store.GetRegistrations().Count);
    }
    private static void TemporaryUnload()
    {
        var store = Store(); store.Register(Lamp("one")); Set(store, LampSelection.World(), ColorMode.Pink);
        store.MarkTemporarilyUnloaded(Id("one")); store.RebindTemporarilyUnloaded(Id("one"), "recreated-renderer");
        Equal(ColorMode.Pink, store.GetBaseState(Id("one")).Color); Equal(LampPresence.Available, store.GetRegistrations()[0].Presence);
        Throws<InvalidOperationException>(() => store.RebindTemporarilyUnloaded(Id("one"), "third-object"));
    }
    private static void Replacement()
    {
        var store = Store(); store.Register(Lamp("one")); Set(store, LampSelection.World(), ColorMode.Blue);
        store.RemovePermanently(Id("one"), 2); store.Register(Lamp("one", runtime: "replacement"), 3);
        Equal(LampBaseState.Default, store.GetBaseState(Id("one")));
    }
    private static void MoveScope()
    {
        var store = Store(); store.Register(Lamp("one")); Set(store, LampSelection.World(), ColorMode.Red);
        Command(store, ControlCommand.StartDisco(LampSelection.Property("P1"), 1, 5), 2);
        Equal(true, store.ResolveVisual(Id("one"), 3).HasTemporaryEffect);
        store.Move(Id("one"), "P2", "G2");
        Equal(false, store.ResolveVisual(Id("one"), 3).HasTemporaryEffect);
        Equal(ColorMode.Red, store.GetBaseState(Id("one")).Color);
    }
    private static void GroupNewcomer()
    {
        var store = Store(); store.Register(Lamp("one")); store.Register(Lamp("outside", "P2", "G1"));
        Set(store, LampSelection.Group("P1", "G1"), ColorMode.Green);
        store.Register(Lamp("new"), 2);
        Equal(ColorMode.Green, store.GetBaseState(Id("one")).Color);
        Equal(LampBaseState.Default, store.GetBaseState(Id("new")));
        Equal(LampBaseState.Default, store.GetBaseState(Id("outside")));
        store.Move(Id("outside"), "P1", "G1");
        Equal(LampBaseState.Default, store.GetBaseState(Id("outside")));
    }
    private static void UnloadedGroup()
    {
        // Deliberate change: a state command no longer recolours a lamp that is only registered, not available.
        var store = Store(); store.Register(Lamp("one")); store.MarkTemporarilyUnloaded(Id("one"));
        Equal(CommandStatus.UnknownTarget, Set(store, LampSelection.Group("P1", "G1"), ColorMode.Cyan).Status);
        Equal(LampBaseState.Default, store.GetBaseState(Id("one")));
        Equal(0, store.AvailableTargetKeys(LampSelection.Group("P1", "G1")).Length);
    }
    private static void DeniedGroup()
    {
        var store = Store(); store.Register(Lamp("one")); store.Register(Lamp("two")); long before = store.Revision;
        int authorizedTargets = 0;
        var command = new ClientCommand(1, Session, Guid.NewGuid(), before, ControlCommand.Set(LampSelection.World(), new(ColorMode.Red)));
        var result = store.ExecuteHost(command, "peer", context => { authorizedTargets = context.Targets.Count; return false; }, 1);
        Equal(CommandStatus.Denied, result.Status); Equal(2, authorizedTargets); Equal(before, store.Revision);
        Equal(LampBaseState.Default, store.GetBaseState(Id("one"))); Equal(LampBaseState.Default, store.GetBaseState(Id("two")));
    }
    private static void ReentrantAuthorization()
    {
        var store = Store(); store.Register(Lamp("one")); long before = store.Revision;
        var request = new ClientCommand(1, Session, Guid.NewGuid(), before, ControlCommand.Cycle(LampSelection.World()));
        var result = store.ExecuteHost(request, "peer", _ => { store.Register(Lamp("two"), 1); return true; }, 1);
        Equal(CommandStatus.AuthorizationFailed, result.Status); Equal(before, store.Revision);
        Equal(1, store.GetRegistrations().Count); Equal(LampBaseState.Default, store.GetBaseState(Id("one")));
    }
    private static void FailingAuthorization()
    {
        var store = Store(); store.Register(Lamp("one")); long before = store.Revision;
        var request = new ClientCommand(1, Session, Guid.NewGuid(), before, ControlCommand.Cycle(LampSelection.World()));
        Equal(CommandStatus.AuthorizationFailed, store.ExecuteHost(request, "peer", _ => throw new InvalidOperationException("test"), 1).Status);
        Equal(before, store.Revision);
        Equal(CommandStatus.Applied, Command(store, request.Command).Status);
    }
    private static void StaleCommand()
    {
        var store = Store(); store.Register(Lamp("one")); long old = store.Revision;
        Set(store, LampSelection.World(), ColorMode.Green);
        var result = store.ExecuteHost(new(1, Session, Guid.NewGuid(), old, ControlCommand.Cycle(LampSelection.World())), "peer", _ => true, 1);
        Equal(CommandStatus.StaleRevision, result.Status); Equal(ColorMode.Green, store.GetBaseState(Id("one")).Color);
    }
    private static void WrongCommandEnvelope()
    {
        var store = Store(); store.Register(Lamp("one")); var action = ControlCommand.Cycle(LampSelection.World());
        Equal(CommandStatus.WrongProtocol, store.ExecuteHost(new(2, Session, Guid.NewGuid(), store.Revision, action), "peer", _ => true, 1).Status);
        Equal(CommandStatus.WrongSession, store.ExecuteHost(new(1, Guid.NewGuid(), Guid.NewGuid(), store.Revision, action), "peer", _ => true, 1).Status);
    }
    private static void DuplicateCommand()
    {
        var store = Store(); store.Register(Lamp("one"));
        var request = new ClientCommand(1, Session, Guid.NewGuid(), store.Revision, ControlCommand.Cycle(LampSelection.World()));
        Equal(CommandStatus.Applied, store.ExecuteHost(request, "peer", _ => true, 1).Status); long after = store.Revision;
        Equal(CommandStatus.Duplicate, store.ExecuteHost(request, "peer", _ => true, 1).Status); Equal(after, store.Revision);
        Equal(ColorMode.White, store.GetBaseState(Id("one")).Color);
    }
    private static void SenderScopedCommands()
    {
        var store = Store(); store.Register(Lamp("one")); Guid id = Guid.NewGuid(); var action = ControlCommand.Cycle(LampSelection.World());
        Equal(CommandStatus.Applied, store.ExecuteHost(new(1, Session, id, store.Revision, action), "a", _ => true, 1).Status);
        Equal(CommandStatus.Applied, store.ExecuteHost(new(1, Session, id, store.Revision, action), "b", _ => true, 1).Status);
        Equal(ColorMode.Red, store.GetBaseState(Id("one")).Color);
    }
    private static void UnknownTargets()
    {
        var store = Store();
        Equal(CommandStatus.UnknownTarget, Set(store, LampSelection.Lamp(Id("missing")), ColorMode.Red).Status);
        var request = new ClientCommand(1, Session, Guid.NewGuid(), store.Revision, ControlCommand.StartDisco(LampSelection.Property("unknown"), 1, 0));
        Equal(CommandStatus.Denied, store.ExecuteHost(request, "peer", _ => false, 1).Status);
        Equal(0, store.Effects.Properties.Count);
    }
    private static void ClientRestrictions()
    {
        var client = Store(SessionMode.Client); client.Register(Lamp("one"));
        Equal(CommandStatus.WrongMode, client.ExecuteLocal(ControlCommand.Cycle(LampSelection.World()), 0).Status);
        Equal(CommandStatus.WrongMode, Command(client, ControlCommand.Cycle(LampSelection.World())).Status);
        Throws<InvalidOperationException>(() => new JsonStatePersistence(client));
        Throws<InvalidOperationException>(() => client.RemovePermanently(Id("one"), 0));
        Throws<InvalidOperationException>(() => client.CreateHostSnapshot(0));
    }
    private static void DeferredSnapshot()
    {
        var client = Store(SessionMode.Client);
        Equal(SynchronizationStatus.Applied, client.ApplyHostSnapshot(Snapshot(Session, 5, new[] { Entry("one") })).Status);
        client.Register(Lamp("one")); Equal(ColorMode.Red, client.GetBaseState(Id("one")).Color); Equal(5L, client.Revision);
    }
    private static void InitialSnapshotRace()
    {
        var client = Store(SessionMode.Client);
        Equal(SynchronizationStatus.NeedSnapshot, client.ApplyHostDelta(Delta(Session, 5, new[] { Entry("one", ColorMode.Blue) })).Status);
        Equal(SynchronizationStatus.NeedSnapshot, client.ApplyHostSnapshot(Snapshot(Session, 5, new[] { Entry("one", ColorMode.Red) })).Status);
        Equal(true, client.NeedsSnapshot);
        Equal(SynchronizationStatus.Applied, client.ApplyHostSnapshot(Snapshot(Session, 6, new[] { Entry("one", ColorMode.Blue) })).Status);
        Equal(ColorMode.Blue, client.GetBaseState(Id("one")).Color); Equal(false, client.NeedsSnapshot);
    }
    private static void DeltaGap()
    {
        var client = Store(SessionMode.Client); client.ApplyHostSnapshot(Snapshot(Session, 2, new[] { Entry("one") }));
        Equal(SynchronizationStatus.NeedSnapshot, client.ApplyHostDelta(Delta(Session, 3, new[] { Entry("one", ColorMode.Pink) })).Status);
        Equal(SynchronizationStatus.NeedSnapshot, client.ApplyHostSnapshot(Snapshot(Session, 3, new[] { Entry("one", ColorMode.Green) })).Status);
        Equal(ColorMode.Red, client.GetBaseState(Id("one")).Color);
        Equal(SynchronizationStatus.Applied, client.ApplyHostDelta(Delta(Session, 2, new[] { Entry("one", ColorMode.Green) })).Status);
        Equal(true, client.NeedsSnapshot);
        Equal(SynchronizationStatus.Applied, client.ApplyHostDelta(Delta(Session, 3, new[] { Entry("one", ColorMode.Pink) })).Status);
        Equal(false, client.NeedsSnapshot); Equal(ColorMode.Pink, client.GetBaseState(Id("one")).Color);
    }
    private static void OldSnapshot()
    {
        var client = Store(SessionMode.Client); var old = Snapshot(Session, 2, new[] { Entry("one") }); client.ApplyHostSnapshot(old);
        client.ApplyHostDelta(Delta(Session, 2, new[] { Entry("one", ColorMode.Cyan) }));
        Equal(SynchronizationStatus.IgnoredStale, client.ApplyHostSnapshot(old).Status); Equal(ColorMode.Cyan, client.GetBaseState(Id("one")).Color);
    }
    private static void InvalidSnapshot()
    {
        var client = Store(SessionMode.Client); client.ApplyHostSnapshot(Snapshot(Session, 1, new[] { Entry("one") }));
        var invalid = Snapshot(Session, 2, new[] { Entry("one", ColorMode.Blue), Entry("one", ColorMode.Green) });
        Equal(SynchronizationStatus.InvalidPayload, client.ApplyHostSnapshot(invalid).Status);
        Equal(1L, client.Revision); Equal(ColorMode.Red, client.GetBaseState(Id("one")).Color);
    }
    private static void InvalidDelta()
    {
        var client = Store(SessionMode.Client); client.ApplyHostSnapshot(Snapshot(Session, 1, new[] { Entry("one") }));
        var invalid = Delta(Session, 1, new[] { Entry("one", ColorMode.Blue) }, removed: new[] { Id("one") });
        Equal(SynchronizationStatus.InvalidPayload, client.ApplyHostDelta(invalid).Status);
        Equal(1L, client.Revision); Equal(ColorMode.Red, client.GetBaseState(Id("one")).Color);
    }
    private static void WrongIdentitySession()
    {
        var client = Store(SessionMode.Client); var alien = LampIdentity.Ephemeral(Guid.NewGuid(), "one");
        Equal(SynchronizationStatus.InvalidPayload, client.ApplyHostSnapshot(Snapshot(Session, 1,
            new[] { new LampStateEntry(alien, LampBaseState.Default) })).Status);
        Throws<ArgumentException>(() => client.Register(new(alien, "runtime")));
    }
    private static void DeferredDelta()
    {
        var client = Store(SessionMode.Client); client.ApplyHostSnapshot(Snapshot(Session, 0, Array.Empty<LampStateEntry>()));
        Equal(SynchronizationStatus.Applied, client.ApplyHostDelta(Delta(Session, 0, new[] { Entry("future", ColorMode.Purple) })).Status);
        client.Register(Lamp("future")); Equal(ColorMode.Purple, client.GetBaseState(Id("future")).Color);
        client.UnbindClientView(Id("future")); Equal(ColorMode.Purple, client.GetBaseState(Id("future")).Color);
    }
    private static void ProtocolBounds()
    {
        Throws<ArgumentException>(() => Snapshot(Session, 0, Enumerable.Repeat(Entry("one"), LightLimits.MaxLamps + 1)));
        Throws<ArgumentException>(() => new StateDelta(1, Session, 2, 4, 0, Array.Empty<LampStateEntry>(), Array.Empty<LampIdentity>(), null));
        Throws<ArgumentOutOfRangeException>(() => Snapshot(Session, -1, Array.Empty<LampStateEntry>()));
        Throws<ArgumentOutOfRangeException>(() => Snapshot(Session, 0, Array.Empty<LampStateEntry>(), double.NaN));
    }
    private static void EffectPriority()
    {
        var store = Store(); store.Register(Lamp("one"));
        Command(store, ControlCommand.StartDisco(LampSelection.World(), 1, 100), 1);
        var world = store.Effects.World;
        Command(store, ControlCommand.StartDisco(LampSelection.Property("P1"), 2, 200), 2);
        Equal(200UL, store.Effects.Resolve("P1")!.Seed); Equal(world, store.Effects.Resolve("P2"));
        Command(store, ControlCommand.StopDisco(LampSelection.Property("P1")), 3); Equal(world, store.Effects.Resolve("P1"));
        Command(store, ControlCommand.StopDisco(LampSelection.World()), 4); Equal<DiscoEffect?>(null, store.Effects.Resolve("P1"));
    }
    private static void BaseColorDuringDisco()
    {
        var store = Store(); store.Register(Lamp("one")); Command(store, ControlCommand.StartDisco(LampSelection.World(), 1, 0));
        var before = store.ResolveVisual(Id("one"), 2).ColorOverride;
        Set(store, LampSelection.World(), ColorMode.Pink, 2);
        Equal(before, store.ResolveVisual(Id("one"), 2).ColorOverride); Equal(ColorMode.Pink, store.GetBaseState(Id("one")).Color);
        Command(store, ControlCommand.StopDisco(LampSelection.World()), 3);
        Equal(new RgbColor(1, 0.15, 0.55), store.ResolveVisual(Id("one"), 3).ColorOverride);
    }
    private static void BrightnessDuringDisco()
    {
        var store = Store(); store.Register(Lamp("one")); Command(store, ControlCommand.StartDisco(LampSelection.World(), 1, 0));
        Command(store, ControlCommand.Set(LampSelection.World(), new(intensity: IntensityLevel.Percent25)), 2);
        Equal(0.25, store.ResolveVisual(Id("one"), 2).IntensityFactor); Equal(true, store.ResolveVisual(Id("one"), 2).HasTemporaryEffect);
    }
    private static void NewLampDuringDisco()
    {
        var store = Store(); Command(store, ControlCommand.StartDisco(LampSelection.Property("P1"), 1, 20));
        store.Register(Lamp("new"), 2); Equal(LampBaseState.Default, store.GetBaseState(Id("new")));
        Equal(true, store.ResolveVisual(Id("new"), 2).HasTemporaryEffect);
    }
    private static void DeterministicDisco()
    {
        var host = Store(); host.Register(Lamp("one")); Command(host, ControlCommand.StartDisco(LampSelection.World(), 0.5, 123), 1);
        var client = Store(SessionMode.Client); client.ApplyHostSnapshot(host.CreateHostSnapshot(2)); client.Register(Lamp("one"));
        foreach (double time in new[] { 2.0, 2.25, 5.0, 100.0 }) Equal(host.ResolveVisual(Id("one"), time), client.ResolveVisual(Id("one"), time));
        Equal(host.ResolveVisual(Id("one"), 2), host.ResolveVisual(Id("one"), 2.25));
    }
    private static void RainbowPeriod()
    {
        var state = new LampBaseState(ColorMode.Rainbow, IntensityLevel.Percent100);
        var a = VisualResolver.Resolve("lamp", null, state, EffectState.Empty, 0, rainbowPeriodSeconds: 20);
        var b = VisualResolver.Resolve("lamp", null, state, EffectState.Empty, 20, rainbowPeriodSeconds: 20);
        Equal(a, b); NotEqual(a, VisualResolver.Resolve("lamp", null, state, EffectState.Empty, 10, rainbowPeriodSeconds: 20));
        Throws<ArgumentOutOfRangeException>(() => VisualResolver.Resolve("lamp", null, state, EffectState.Empty, 0, rainbowPeriodSeconds: double.NaN));
    }
    private static void DisabledVisuals()
    {
        var effect = new EffectState(new(0, 1, 0), Array.Empty<KeyValuePair<string, DiscoEffect>>());
        var result = VisualResolver.Resolve("lamp", "P1", new(ColorMode.Blue, IntensityLevel.Percent200), effect, 10, false);
        Equal(true, result.RemovesModInfluence); Equal(false, result.HasTemporaryEffect);
    }
    private static void OriginalVisuals()
    {
        var result = VisualResolver.Resolve("lamp", null, new(ColorMode.Original, IntensityLevel.Percent150), EffectState.Empty, 1);
        Equal<RgbColor?>(null, result.ColorOverride); Equal(1.5, result.IntensityFactor);
    }
    private static void FreshNativeBaseline()
    {
        var instruction = new VisualInstruction(null, 2, false);
        Equal(20.0, instruction.ScaleCurrentNativeIntensity(10)); Equal(14.0, instruction.ScaleCurrentNativeIntensity(7));
        Equal(20.0, instruction.ScaleCurrentNativeIntensity(10)); Equal(0.0, instruction.ScaleCurrentNativeIntensity(0));
    }
    private static void InvalidNativeIntensity()
    {
        var instruction = new VisualInstruction(null, 2, false);
        foreach (double value in new[] { double.NaN, double.PositiveInfinity, -1, double.MaxValue })
            Throws<ArgumentOutOfRangeException>(() => instruction.ScaleCurrentNativeIntensity(value));
        Throws<ArgumentOutOfRangeException>(() => new VisualInstruction(null, double.NaN, false));
        Throws<ArgumentOutOfRangeException>(() => new VisualInstruction(null, 3, false));
        Throws<ArgumentOutOfRangeException>(() => new RgbColor(double.NaN, 0, 0));
    }
    private static void WriteBeforeLoad()
    {
        var persistence = new JsonStatePersistence(Store()); Throws<InvalidOperationException>(() => persistence.CreateJsonForSave());
    }
    private static void CorruptPersistence()
    {
        var store = Store(); var persistence = new JsonStatePersistence(store);
        Equal(false, persistence.LoadJson("{broken").Success); Equal(PersistenceGate.Blocked, persistence.Gate);
        Throws<InvalidOperationException>(() => persistence.CreateJsonForSave()); Equal(0L, store.Revision);
    }
    private static void MissingAfterFailure()
    {
        var persistence = new JsonStatePersistence(Store()); persistence.ReportReadFailure();
        Equal(false, persistence.ConfirmDataMissing().Success); Throws<InvalidOperationException>(() => persistence.CreateJsonForSave());
    }
    private static void FutureSchema()
    {
        var persistence = new JsonStatePersistence(Store());
        Equal(PersistenceError.UnsupportedSchema, persistence.LoadJson("{\"schemaVersion\":2,\"futureStructure\":true}").Error);
        Throws<InvalidOperationException>(() => persistence.CreateJsonForSave());
    }
    private static void DuplicateJson()
    {
        var persistence = new JsonStatePersistence(Store());
        Equal(false, persistence.LoadJson("{\"schemaVersion\":1,\"schemaVersion\":1,\"saveId\":\"save-A\",\"lamps\":[]}").Success);
        using var valid = JsonDocument.Parse(SavedJson()); string lamp = valid.RootElement.GetProperty("lamps")[0].GetRawText();
        string duplicate = "{\"schemaVersion\":1,\"saveId\":\"save-A\",\"lamps\":[" + lamp + "," + lamp + "]}";
        Equal(PersistenceError.InvalidState, new JsonStatePersistence(Store()).LoadJson(duplicate).Error);
    }
    private static void InvalidPersistedState()
    {
        string invalid = SavedJson().Replace("\"intensityPercent\": 100", "\"intensityPercent\": 999", StringComparison.Ordinal);
        var store = Store(); var persistence = new JsonStatePersistence(store); Equal(false, persistence.LoadJson(invalid).Success);
        store.Register(Lamp("one")); Equal(LampBaseState.Default, store.GetBaseState(Id("one")));
        Throws<InvalidOperationException>(() => persistence.CreateJsonForSave());
    }
    private static void RepeatedPersistence()
    {
        var store = Store(); var persistence = Missing(store); store.Register(Lamp("one")); store.Register(Lamp("two"));
        Set(store, LampSelection.Lamp(Id("one")), ColorMode.Red); Set(store, LampSelection.Lamp(Id("two")), ColorMode.Blue);
        string json = persistence.CreateJsonForSave();
        for (int cycle = 0; cycle < 5; cycle++)
        {
            var next = Store(session: Guid.NewGuid()); var loader = new JsonStatePersistence(next); Equal(true, loader.LoadJson(json).Success);
            next.Register(Lamp("two", runtime: $"second-{cycle}")); next.Register(Lamp("one", runtime: $"first-{cycle}"));
            Equal(ColorMode.Red, next.GetBaseState(Id("one")).Color); Equal(ColorMode.Blue, next.GetBaseState(Id("two")).Color);
            next.Move(Id("one"), "P2", "G2"); next.MarkTemporarilyUnloaded(Id("two"));
            Equal(json, loader.CreateJsonForSave()); json = loader.CreateJsonForSave();
        }
    }
    private static void SaveIsolation()
    {
        var store = Store(save: "save-B"); var persistence = new JsonStatePersistence(store);
        Equal(PersistenceError.WrongSave, persistence.LoadJson(SavedJson()).Error);
        Throws<InvalidOperationException>(() => persistence.CreateJsonForSave());
        var a = Store(); var writer = Missing(a); a.BeginSession(Guid.NewGuid(), "save-B", SessionMode.Host);
        Throws<InvalidOperationException>(() => writer.CreateJsonForSave());
    }
    private static void PendingPersistence()
    {
        string json = SavedJson(); var store = Store(); var persistence = new JsonStatePersistence(store);
        Equal(true, persistence.LoadJson(json).Success); Equal(0, store.GetRegistrations().Count);
        Equal(json, persistence.CreateJsonForSave());
        Equal(CommandStatus.UnknownTarget, Set(store, LampSelection.World(), ColorMode.Red).Status);
        Equal(ColorMode.Purple, store.GetBaseState(Id("one")).Color);
    }
    private static void PersistenceExclusions()
    {
        var store = Store(); var persistence = Missing(store); var ephemeral = LampIdentity.Ephemeral(Session, "host-assigned-1");
        store.Register(new(ephemeral, "runtime-1", "P1")); Command(store, ControlCommand.StartDisco(LampSelection.World(), 1, 999));
        using var json = JsonDocument.Parse(persistence.CreateJsonForSave());
        Equal(0, json.RootElement.GetProperty("lamps").GetArrayLength()); Equal(false, json.RootElement.TryGetProperty("effects", out _));
    }
    private static void LocalModeTransition()
    {
        var store = Store(SessionMode.Local); store.Register(Lamp("one"));
        Equal(CommandStatus.Applied, store.ExecuteLocal(ControlCommand.Set(LampSelection.World(), new(ColorMode.Pink)), 1).Status);
        Throws<InvalidOperationException>(() => new JsonStatePersistence(store)); Throws<InvalidOperationException>(() => store.CreateHostSnapshot(1));
        store.BeginSession(Guid.NewGuid(), "save-A", SessionMode.Host); var persistence = Missing(store); store.Register(Lamp("one"));
        Equal(LampBaseState.Default, store.GetBaseState(Id("one"))); Equal<DiscoEffect?>(null, store.Effects.World);
        Equal(true, persistence.CreateJsonForSave().Contains("\"colorMode\": 0", StringComparison.Ordinal));
    }
    private static void SessionTransition()
    {
        var client = Store(SessionMode.Client); client.ApplyHostSnapshot(Snapshot(Session, 9, new[] { Entry("one") }));
        client.BeginSession(Guid.NewGuid(), "save-B", SessionMode.Client);
        Equal(SynchronizationStatus.WrongSession, client.ApplyHostSnapshot(Snapshot(Session, 10, new[] { Entry("one") })).Status);
        Equal(0L, client.Revision); Equal(0, client.GetRegistrations().Count); Equal(true, client.NeedsSnapshot);
    }
    private static void LatePersistenceLoad()
    {
        var store = Store(); var persistence = new JsonStatePersistence(store); store.Register(Lamp("one")); Set(store, LampSelection.World(), ColorMode.Green);
        Equal(false, persistence.LoadJson(SavedJson()).Success); Equal(ColorMode.Green, store.GetBaseState(Id("one")).Color);
        Throws<InvalidOperationException>(() => persistence.CreateJsonForSave());
    }
    private static void PersistentDeletion()
    {
        var store = Store(); var persistence = new JsonStatePersistence(store); persistence.LoadJson(SavedJson());
        store.RemovePermanently(Id("one"), 0); using var document = JsonDocument.Parse(persistence.CreateJsonForSave());
        Equal(0, document.RootElement.GetProperty("lamps").GetArrayLength());
    }
    private static void RecoverInitialLoad()
    {
        var persistence = new JsonStatePersistence(Store()); persistence.LoadJson("broken");
        Equal(true, persistence.LoadJson(SavedJson()).Success); Equal(PersistenceGate.Ready, persistence.Gate);
        Equal(SavedJson(), persistence.CreateJsonForSave());
    }
    private static void JsonBounds()
    {
        Equal(false, new JsonStatePersistence(Store()).LoadJson(new string(' ', LightLimits.MaxJsonBytes + 1)).Success);
        Equal(false, new JsonStatePersistence(Store()).LoadJson(new string('[', 20) + new string(']', 20)).Success);
        Equal(false, new JsonStatePersistence(Store()).LoadJson("null").Success);
        Equal(false, new JsonStatePersistence(Store()).LoadJson("{\"schemaVersion\":1,\"saveId\":\"save-A\",\"lamps\":null}").Success);
    }
    private static void DefensiveCopies()
    {
        var values = new List<LampStateEntry> { Entry("one") }; var snapshot = Snapshot(Session, 1, values); values.Clear(); Equal(1, snapshot.Lamps.Count);
        var effects = new Dictionary<string, DiscoEffect> { ["P1"] = new(0, 1, 1) }; var state = new EffectState(null, effects); effects.Clear();
        Equal(1, state.Properties.Count);
        Throws<NotSupportedException>(() => ((IList<LampStateEntry>)snapshot.Lamps).Clear());
    }
    private static void ConcurrentCommands()
    {
        var store = Store(); store.Register(Lamp("one")); long revision = store.Revision;
        var requests = new[] { ColorMode.Red, ColorMode.Blue }.Select(color => new ClientCommand(1, Session, Guid.NewGuid(), revision,
            ControlCommand.Set(LampSelection.World(), new(color)))).ToArray();
        var tasks = requests.Select(request => Task.Run(() => store.ExecuteHost(request, "peer", _ => true, 1))).ToArray(); Task.WaitAll(tasks);
        Equal(1, tasks.Count(task => task.Result.Status == CommandStatus.Applied));
        Equal(1, tasks.Count(task => task.Result.Status == CommandStatus.StaleRevision)); Equal(revision + 1, store.Revision);
    }
    private static void RepeatedSessions()
    {
        var store = new LightStateStore();
        for (int cycle = 0; cycle < 20; cycle++)
        {
            store.BeginSession(Guid.NewGuid(), $"save-{cycle % 2}", SessionMode.Host); store.Register(Lamp("one"));
            Command(store, ControlCommand.StartDisco(LampSelection.World(), 1, 0)); store.EndSession();
        }
        store.BeginSession(Guid.NewGuid(), "save-A", SessionMode.Host); Equal(0L, store.Revision); Equal(0, store.GetRegistrations().Count);
        Equal<DiscoEffect?>(null, store.Effects.World); Throws<ArgumentException>(() => store.BeginSession(store.SessionId, "save-A", SessionMode.Local));
    }
    private static void MonotonicTime()
    {
        var store = Store(); store.Register(Lamp("one")); Set(store, LampSelection.World(), ColorMode.Red, 5);
        Throws<ArgumentOutOfRangeException>(() => Set(store, LampSelection.World(), ColorMode.Blue, 4));
        Equal(ColorMode.Red, store.GetBaseState(Id("one")).Color);
    }
    private static void FutureEffectTimestamp()
    {
        var client = Store(SessionMode.Client);
        var effect = new EffectState(new(10, 1, 0), Array.Empty<KeyValuePair<string, DiscoEffect>>());
        Equal(SynchronizationStatus.InvalidPayload, client.ApplyHostSnapshot(Snapshot(Session, 1, Array.Empty<LampStateEntry>(), 5, effect)).Status);
        Equal(true, client.NeedsSnapshot);
    }
    private static void RepeatedLoadNotification()
    {
        var persistence = new JsonStatePersistence(Store()); string json = SavedJson(); Equal(true, persistence.LoadJson(json).Success);
        Equal(PersistenceError.AlreadyLoaded, persistence.LoadJson(json).Error);
        Equal(PersistenceError.AlreadyLoaded, persistence.ConfirmDataMissing().Error);
        Equal(PersistenceGate.Ready, persistence.Gate); Equal(json, persistence.CreateJsonForSave());
    }
    private static void LaterReadFailure()
    {
        var persistence = Missing(Store()); persistence.ReportReadFailure(); Equal(PersistenceGate.Blocked, persistence.Gate);
        Throws<InvalidOperationException>(() => persistence.CreateJsonForSave());
    }

    // ---- Runtime-used pure policies (Unity-free) ------------------------------------------

    private static void NativeUndoBeforeApply()
    {
        var policy = new NativeLightPolicy();
        Equal(false, policy.Owned);
        Equal(new NativeLightStep(false, 0, false), policy.UndoOwned(0));
        Equal(new NativeLightStep(false, 0, false), policy.Restore(0));
        // Ownership (and the baseline) only exist after a real apply.
        Equal(true, policy.Apply(10, new VisualInstruction(null, 2, false)).Owned);
        Equal(true, policy.Owned);
        Equal(10.0, policy.Baseline);
    }
    private static void NativeNoCompounding()
    {
        var policy = new NativeLightPolicy();
        var twice = new VisualInstruction(null, 2, false);
        Equal(new NativeLightStep(true, 20f, true), policy.Apply(10, twice));
        Equal(new NativeLightStep(true, 10f, false), policy.UndoOwned(20));
        // The second frame samples the fresh vanilla value again: 2x, never 4x.
        Equal(new NativeLightStep(true, 20f, true), policy.Apply(10, twice));
    }
    private static void NativeExternalDrift()
    {
        var policy = new NativeLightPolicy();
        var twice = new VisualInstruction(null, 2, false);
        Equal(new NativeLightStep(true, 20f, true), policy.Apply(10, twice));
        policy.UndoOwned(20);
        // The game changed its own value to 7: the factor applies to the new baseline.
        Equal(new NativeLightStep(true, 14f, true), policy.Apply(7, twice));
    }
    private static void NativeFlickerAndZero()
    {
        var policy = new NativeLightPolicy();
        var twice = new VisualInstruction(null, 2, false);
        Equal(new NativeLightStep(true, 0f, true), policy.Apply(0, twice));
        policy.UndoOwned(0);
        Equal(new NativeLightStep(true, 1.6f, true), policy.Apply(0.8, twice));
        policy.UndoOwned(1.6);
        Equal(new NativeLightStep(true, 2f, true), policy.Apply(1.0, twice));
    }
    private static void NativeOriginalBaseline()
    {
        var policy = new NativeLightPolicy();
        Equal(new NativeLightStep(true, 20f, true), policy.Apply(10, new VisualInstruction(null, 2, false)));
        // Original after the game moved the lamp's own value to 7 restores the latest native
        // baseline (7), not the stale activation snapshot (10).
        Equal(new NativeLightStep(true, 7f, false), policy.Apply(7, new VisualInstruction(null, 1, false)));
        Equal(false, policy.Owned);
        Equal(new NativeLightStep(false, 0, false), policy.UndoOwned(0));
    }
    private static void NativeInvalidSamples()
    {
        var policy = new NativeLightPolicy();
        var twice = new VisualInstruction(null, 2, false);
        foreach (double bad in new[] { double.NaN, double.PositiveInfinity, double.NegativeInfinity, -1 })
            Equal(new NativeLightStep(false, 0, false), policy.Apply(bad, twice));
        Equal(new NativeLightStep(false, 0, false), policy.Apply(double.MaxValue, twice));
        Equal(false, policy.Owned);
        Throws<ArgumentNullException>(() => policy.Apply(1, null!));
        Equal(false, NativeLightPolicy.IsValidSample(double.NaN));
        Equal(true, NativeLightPolicy.IsValidSample(0));
    }
    private static void NativeRestoreOnce()
    {
        var policy = new NativeLightPolicy();
        policy.Apply(10, new VisualInstruction(null, 2, false));
        Equal(new NativeLightStep(true, 10f, false), policy.Restore(20));
        Equal(new NativeLightStep(false, 0, false), policy.Restore(0));
    }
    private static void NativeGuidRejected()
    {
        Equal(NativeIdentityRejection.Missing, LampIdentityPolicy.FromNativeGuid(null).Rejection);
        Equal(NativeIdentityRejection.Missing, LampIdentityPolicy.FromNativeGuid("   ").Rejection);
        Equal(NativeIdentityRejection.Empty, LampIdentityPolicy.FromNativeGuid(LampIdentityPolicy.EmptyGuidText).Rejection);
        Equal(NativeIdentityRejection.Malformed, LampIdentityPolicy.FromNativeGuid("not-a-guid").Rejection);
        Equal(NativeIdentityRejection.Malformed, LampIdentityPolicy.FromNativeGuid("{0}").Rejection);
        foreach (var result in new[] { LampIdentityPolicy.FromNativeGuid(null), LampIdentityPolicy.FromNativeGuid("x") })
        {
            Equal(false, result.Accepted);
            Equal<LampIdentity?>(null, result.Identity);
            Equal<PersistentLampId?>(null, result.PersistentId);
        }
    }
    private static void NativeGuidAccepted()
    {
        const string a = "9f1b6c2e-1f4a-4c3b-8a2d-2f6e5d4c3b1a";
        const string b = "11111111-2222-3333-4444-555555555555";
        var first = LampIdentityPolicy.FromNativeGuid(a);
        Equal(true, first.Accepted);
        Equal(NativeIdentityRejection.None, first.Rejection);
        Equal(PersistentIdentityKind.VerifiedNativeInstance, first.PersistentId!.Kind);
        Equal(LampIdentityPolicy.Scheme, first.PersistentId.Scheme);
        Equal(LampIdentityPolicy.SchemeVersion, first.PersistentId.SchemeVersion);
        Equal(a, first.PersistentId.Value);
        Equal(LampIdentity.Persistent(first.PersistentId), first.Identity);
        // Textual forms of one GUID (upper case, braces) resolve to the same logical identity.
        Equal(first.Identity!.Key, LampIdentityPolicy.FromNativeGuid(a.ToUpperInvariant()).Identity!.Key);
        Equal(first.Identity.Key, LampIdentityPolicy.FromNativeGuid("{" + a + "}").Identity!.Key);
        NotEqual(first.Identity.Key, LampIdentityPolicy.FromNativeGuid(b).Identity!.Key);
    }
    private static void SlotSaveId()
    {
        Equal(false, PersistenceSlotPolicy.HasValidSlot(-1));
        Equal(true, PersistenceSlotPolicy.HasValidSlot(0));
        Equal<string?>(null, PersistenceSlotPolicy.SaveIdForSlot(-1));
        Equal("slot-0", PersistenceSlotPolicy.SaveIdForSlot(0));
        Equal("slot-3", PersistenceSlotPolicy.SaveIdForSlot(3));
        NotEqual(PersistenceSlotPolicy.SaveIdForSlot(3)!, PersistenceSlotPolicy.SaveIdForSlot(4)!);
    }
    private static void SlotLoadDecision()
    {
        Equal(PersistenceLoadAction.ConfirmMissing, PersistenceSlotPolicy.DecideLoad(false, false, null));
        Equal(PersistenceLoadAction.ReadFailure, PersistenceSlotPolicy.DecideLoad(false, true, ""));
        Equal(PersistenceLoadAction.ReadFailure, PersistenceSlotPolicy.DecideLoad(true, false, "   "));
        Equal(PersistenceLoadAction.LoadJson, PersistenceSlotPolicy.DecideLoad(true, false, "{}"));
        Equal(PersistenceLoadAction.LoadJson, PersistenceSlotPolicy.DecideLoad(false, true, "{}"));
    }
    private static void SlotWriteGuard()
    {
        Equal(true, PersistenceSlotPolicy.CanWrite(2, 2));
        Equal(false, PersistenceSlotPolicy.CanWrite(2, 3));
        Equal(false, PersistenceSlotPolicy.CanWrite(-1, 0));
        Equal(false, PersistenceSlotPolicy.CanWrite(0, -1));
    }
    private static void ScanCadenceBound()
    {
        var cadence = new ScanCadence(5);
        Equal(true, cadence.ShouldScan(0));
        Equal(false, cadence.ShouldScan(1));
        Equal(false, cadence.ShouldScan(4.999));
        Equal(true, cadence.ShouldScan(5));
        Equal(false, cadence.ShouldScan(6));
        cadence.Reset(6);
        Equal(false, cadence.ShouldScan(10.9));
        Equal(true, cadence.ShouldScan(11));
    }
    private static void ScanCadenceInvalid()
    {
        Throws<ArgumentOutOfRangeException>(() => new ScanCadence(0));
        Throws<ArgumentOutOfRangeException>(() => new ScanCadence(double.NaN));
        Throws<ArgumentOutOfRangeException>(() => new ScanCadence(-1));
        var cadence = new ScanCadence(5);
        Equal(false, cadence.ShouldScan(double.NaN));
        Equal(false, cadence.ShouldScan(-1));
        Equal(true, cadence.ShouldScan(0));
    }
    private static void ColorCycleAllEleven()
    {
        var seen = new HashSet<ColorMode>();
        var color = ColorMode.Original;
        for (int i = 0; i < UiSelectionPolicy.ColorCount; i++) { seen.Add(color); color = UiSelectionPolicy.NextColor(color); }
        Equal(UiSelectionPolicy.ColorCount, seen.Count);
        Equal(ColorMode.Original, color);
        Equal(ColorMode.Rainbow, UiSelectionPolicy.NextColor(ColorMode.Original, -1));
        Equal(0, UiSelectionPolicy.WrapIndex(UiSelectionPolicy.ColorCount, UiSelectionPolicy.ColorCount));
    }
    private static void IntensityCycleAllSeven()
    {
        var seen = new HashSet<IntensityLevel>();
        var level = IntensityLevel.Percent10;
        for (int i = 0; i < UiSelectionPolicy.IntensityCount; i++) { seen.Add(level); level = UiSelectionPolicy.NextIntensity(level); }
        Equal(UiSelectionPolicy.IntensityCount, seen.Count);
        Equal(IntensityLevel.Percent10, level);
        Equal(IntensityLevel.Percent200, UiSelectionPolicy.NextIntensity(IntensityLevel.Percent10, -1));
        Equal(IntensityLevel.Percent200, UiSelectionPolicy.IntensityAt(-1));
        Equal(IntensityLevel.Percent10, UiSelectionPolicy.IntensityAt(UiSelectionPolicy.IntensityCount));
        Throws<ArgumentOutOfRangeException>(() => UiSelectionPolicy.NextIntensity((IntensityLevel)99));
    }
    private static void OriginalReset()
    {
        var reset = UiSelectionPolicy.OriginalReset();
        Equal(LampBaseState.Default, reset);
        Equal(ColorMode.Original, reset.Color);
        Equal(IntensityLevel.Percent100, reset.Intensity);
        // Original is the head of the color cycle and 100% sits inside the intensity cycle.
        Equal(ColorMode.White, UiSelectionPolicy.NextColor(ColorMode.Original));
        Equal(IntensityLevel.Percent150, UiSelectionPolicy.NextIntensity(IntensityLevel.Percent100));
    }

    // ---- Repaired runtime-used policies (RED/GREEN regressions) ----------------------------

    private static void NativeExternalUndoPreserved()
    {
        var policy = new NativeLightPolicy();
        var twice = new VisualInstruction(null, 2, false);
        Equal(new NativeLightStep(true, 20f, true), policy.Apply(10, twice));
        // An external writer moved the native value after our late-update write.
        Equal(new NativeLightStep(false, 0, false), policy.UndoOwned(7));
        Equal(false, policy.Owned);
        // The external value is preserved and adopted by the next apply.
        Equal(new NativeLightStep(true, 14f, true), policy.Apply(7, twice));
    }
    private static void NativeUndoRestoresBaseline()
    {
        var policy = new NativeLightPolicy();
        Equal(new NativeLightStep(true, 20f, true), policy.Apply(10, new VisualInstruction(null, 2, false)));
        // Our own value is still installed: the game's writer gets its baseline back.
        Equal(new NativeLightStep(true, 10f, false), policy.UndoOwned(20));
        Equal(false, policy.Owned);
        Equal(new NativeLightStep(false, 0, false), policy.UndoOwned(20));
    }
    private static void NativeFloatRoundtrip()
    {
        var policy = new NativeLightPolicy();
        var step = policy.Apply(0.1, new VisualInstruction(null, 1.5, false));
        Equal(true, step.Owned);
        // The native setter stores a float; reading it back yields the widened double, which must
        // still compare equal to the exact float we wrote (a double round-trip would not).
        double readBack = step.Intensity;
        Equal(new NativeLightStep(true, 0.1f, false), policy.UndoOwned(readBack));
        Equal(false, policy.Owned);
    }
    private static void NativeTeardownExternal()
    {
        var policy = new NativeLightPolicy();
        Equal(new NativeLightStep(true, 20f, true), policy.Apply(10, new VisualInstruction(null, 2, false)));
        // External write before teardown: preserve it, release ownership, write nothing.
        Equal(new NativeLightStep(false, 0, false), policy.Restore(7));
        Equal(false, policy.Owned);
        Equal(new NativeLightStep(false, 0, false), policy.Restore(7));
        // A fresh apply after the preserved external value scales the new baseline.
        Equal(new NativeLightStep(true, 10.5f, true), policy.Apply(7, new VisualInstruction(null, 1.5, false)));
    }
    private static void NativeFailedWrite()
    {
        var policy = new NativeLightPolicy();
        Equal(true, policy.Apply(10, new VisualInstruction(null, 2, false)).Owned);
        // The native setter threw: the adapter must not claim ownership of an unwritten value.
        policy.RejectWrite();
        Equal(false, policy.Owned);
        Equal(new NativeLightStep(false, 0, false), policy.UndoOwned(20));
        Equal(new NativeLightStep(false, 0, false), policy.Restore(20));
    }
    private static void NativeNoCompoundRepeat()
    {
        var policy = new NativeLightPolicy();
        var twice = new VisualInstruction(null, 2, false);
        for (int frame = 0; frame < 4; frame++)
        {
            Equal(new NativeLightStep(true, 20f, true), policy.Apply(10, twice));
            Equal(new NativeLightStep(true, 10f, false), policy.UndoOwned(20));
        }
        Equal(new NativeLightStep(true, 0f, true), policy.Apply(0, twice));
        Equal(new NativeLightStep(true, 0f, false), policy.UndoOwned(0));
    }
    private static void NativeColorOwnership()
    {
        var policy = new NativeColorPolicy();
        var red = new RgbColor(1, 0, 0);
        Equal(new NativeColorStep(true, 1f, 0f, 0f, 1f, true), policy.Apply(0.2f, 0.3f, 0.4f, 1f, red));
        // Our own color is installed: restoring returns the sampled baseline.
        Equal(new NativeColorStep(true, 0.2f, 0.3f, 0.4f, 1f, false), policy.UndoOwned(1f, 0f, 0f, 1f));
        Equal(false, policy.Owned);
        // An external color change is preserved and released, never clobbered.
        policy.Apply(0.2f, 0.3f, 0.4f, 1f, red);
        Equal(new NativeColorStep(false, 0, 0, 0, 0, false), policy.UndoOwned(0f, 1f, 0f, 1f));
        Equal(false, policy.Owned);
        // No override leaves the native color untouched.
        Equal(new NativeColorStep(false, 0, 0, 0, 0, false), policy.Apply(0.1f, 0.1f, 0.1f, 1f, null));
        Equal(false, NativeColorPolicy.IsValid(float.NaN, 0, 0, 1));
    }
    private static void NativeColorFailedWrite()
    {
        var policy = new NativeColorPolicy();
        Equal(true, policy.Apply(0f, 0f, 0f, 1f, new RgbColor(0, 0, 1)).Owned);
        policy.RejectWrite();
        Equal(false, policy.Owned);
        Equal(new NativeColorStep(false, 0, 0, 0, 0, false), policy.Restore(0f, 0f, 1f, 1f));
    }
    private static void PropertyGate()
    {
        Equal(false, PropertyOwnershipPolicy.IsOwnedProperty(null, true, null));
        Equal(false, PropertyOwnershipPolicy.IsOwnedProperty("", true, new[] { "" }));
        Equal(true, PropertyOwnershipPolicy.IsOwnedProperty("motel", true, null));
        Equal(false, PropertyOwnershipPolicy.IsOwnedProperty("npc-house", false, new[] { "motel" }));
        Equal(true, PropertyOwnershipPolicy.IsOwnedProperty("motel", false, new[] { "motel" }));
    }
    private static void ScanBudgetBound()
    {
        var budget = new SceneScanBudget(64, 2.0, 1000);
        Equal(true, budget.CanVisit(0, 0));
        Equal(true, budget.CanVisit(63, 1.999));
        Equal(false, budget.CanVisit(64, 0));
        Equal(false, budget.CanVisit(0, 2.0));
        Equal(false, budget.IsTruncated(999));
        Equal(true, budget.IsTruncated(1000));
    }
    private static void ScanBudgetLimits()
    {
        var budget = new SceneScanBudget(64, 2.0, 1000);
        Equal(64, budget.MaxNodesPerFrame);
        Equal(2.0, budget.MaxMillisecondsPerFrame);
        Equal(1000, budget.TotalNodeCap);
    }
    private static void FairRootOrder()
    {
        EqualSeq(Array.Empty<int>(), SceneScanBudget.FairRootOrder(0, 0));
        EqualSeq(new[] { 0, 1, 2 }, SceneScanBudget.FairRootOrder(3, 0));
        EqualSeq(new[] { 1, 2, 0 }, SceneScanBudget.FairRootOrder(3, 1));
        EqualSeq(new[] { 2, 0, 1 }, SceneScanBudget.FairRootOrder(3, 2));
        EqualSeq(new[] { 0, 1, 2 }, SceneScanBudget.FairRootOrder(3, 3));
        EqualSeq(new[] { 2, 3, 0, 1 }, SceneScanBudget.FairRootOrder(4, 2));
    }
    private static void LightDedupe()
    {
        EqualSeq(new[] { 0, 1 }, LightPointerDedupe.SelectUnowned(new long[] { 5, 6, 5 }, null));
        EqualSeq(new[] { 1 }, LightPointerDedupe.SelectUnowned(new long[] { 5, 6, 6 }, new long[] { 5 }));
        EqualSeq(Array.Empty<int>(), LightPointerDedupe.SelectUnowned(new long[] { 0, 0 }, null));
        EqualSeq(Array.Empty<int>(), LightPointerDedupe.SelectUnowned(Array.Empty<long>(), null));
    }

    // ---- LED-surface-only policy (RED/GREEN regressions, Unity-free) -----------------------

    private const float Red = 2.5f, Green = 0.175f, Blue = 12.358f;

    private static VisualInstruction Color(RgbColor color, double factor = 1) => new(color, factor, false);
    private static VisualInstruction Original(double factor) => new(null, factor, false);
    private static VisualInstruction RedAt(double factor) => new(new RgbColor(1, 0, 0), factor, false);

    private static void LedSlotClassify()
    {
        Equal(LedSlotState.Empty, LedSurfacePolicy.Classify(false, false, false));
        Equal(LedSlotState.Empty, LedSurfacePolicy.Classify(false, false, true));
        Equal(LedSlotState.OwnClone, LedSurfacePolicy.Classify(true, true, true));
        // Our own clone wins even if its material name is no longer recognised.
        Equal(LedSlotState.OwnClone, LedSurfacePolicy.Classify(true, true, false));
        Equal(LedSlotState.NativePanel, LedSurfacePolicy.Classify(true, false, true));
        Equal(LedSlotState.Foreign, LedSurfacePolicy.Classify(true, false, false));
    }
    private static void LedSlotForeign()
    {
        var policy = new LedSurfacePolicy();
        Equal(new LedSurfaceStep(LedSurfaceAction.None, 0, 0, 0, false), policy.Apply(true, LedSlotState.Foreign, Red, Green, Blue, RedAt(1)));
        Equal(false, policy.Owned);
        Equal(new LedSurfaceStep(LedSurfaceAction.None, 0, 0, 0, false), policy.Apply(true, LedSlotState.Empty, Red, Green, Blue, RedAt(1)));
        Equal(false, policy.Owned);
    }
    private static void LedSlotLampOff()
    {
        var policy = new LedSurfacePolicy();
        Equal(new LedSurfaceStep(LedSurfaceAction.None, 0, 0, 0, false), policy.Apply(false, LedSlotState.NativePanel, Red, Green, Blue, RedAt(1)));
        Equal(false, policy.Owned);
        // A foreign slot stays untouched even when the lamp is off.
        Equal(new LedSurfaceStep(LedSurfaceAction.None, 0, 0, 0, false), policy.Apply(false, LedSlotState.Foreign, Red, Green, Blue, RedAt(1)));
    }
    private static void LedSurfaceColor()
    {
        var policy = new LedSurfacePolicy();
        var step = policy.Apply(true, LedSlotState.NativePanel, Red, Green, Blue, RedAt(1));
        Equal(LedSurfaceAction.Install, step.Action);
        Equal(true, step.Owned);
        // The chosen colour is scaled by the native emission magnitude (the brightest channel).
        Close(Blue, step.R); Close(0f, step.G); Close(0f, step.B);
        Equal(true, policy.Owned);
    }
    private static void LedSurfaceOriginalBrightness()
    {
        var policy = new LedSurfacePolicy();
        var step = policy.Apply(true, LedSlotState.NativePanel, Red, Green, Blue, Original(1.5));
        Equal(LedSurfaceAction.Install, step.Action);
        // Original keeps the native hue and scales it: 2.5, 0.175, 12.358 -> x1.5.
        Close(3.75f, step.R); Close(0.2625f, step.G); Close(18.537f, step.B);
    }
    private static void LedSurfaceZero()
    {
        var policy = new LedSurfacePolicy();
        var step = policy.Apply(true, LedSlotState.NativePanel, Red, Green, Blue, RedAt(0.1));
        Equal(LedSurfaceAction.Install, step.Action);
        Equal(true, step.Owned);
        Close(Blue * 0.1f, step.R); Close(0f, step.G); Close(0f, step.B);
        var zero = new LedSurfacePolicy().Apply(true, LedSlotState.NativePanel, Red, Green, Blue, new VisualInstruction(new RgbColor(1, 0, 0), 0.1, false));
        Equal(LedSurfaceAction.Install, zero.Action);
        Equal(true, zero.Owned);
    }
    private static void LedSurfaceNoCompound()
    {
        var policy = new LedSurfacePolicy();
        var first = policy.Apply(true, LedSlotState.NativePanel, Red, Green, Blue, RedAt(1));
        // The second frame samples the same native baseline again: identical output, never doubled.
        var second = policy.Apply(true, LedSlotState.NativePanel, Red, Green, Blue, RedAt(1));
        Equal(first, second);
        // A drifted native baseline is adopted, not compounded.
        var drifted = policy.Apply(true, LedSlotState.NativePanel, 5f, 0.35f, 24.716f, RedAt(1));
        Close(24.716f, drifted.R);
    }
    private static void LedSurfaceInvalid()
    {
        var policy = new LedSurfacePolicy();
        Equal(new LedSurfaceStep(LedSurfaceAction.None, 0, 0, 0, false), policy.Apply(true, LedSlotState.NativePanel, float.NaN, 0, 0, RedAt(1)));
        Equal(false, policy.Owned);
        Equal(new LedSurfaceStep(LedSurfaceAction.None, 0, 0, 0, false), policy.Apply(true, LedSlotState.NativePanel, -1, 0, 0, RedAt(1)));
        Equal(new LedSurfaceStep(LedSurfaceAction.None, 0, 0, 0, false), policy.Apply(true, LedSlotState.NativePanel, 0, float.PositiveInfinity, 0, RedAt(1)));
        Equal(false, policy.Owned);
    }
    private static void LedSurfaceFailedWrite()
    {
        var policy = new LedSurfacePolicy();
        Equal(true, policy.Apply(true, LedSlotState.NativePanel, Red, Green, Blue, RedAt(1)).Owned);
        policy.RejectWrite();
        Equal(false, policy.Owned);
        Equal(new LedSurfaceStep(LedSurfaceAction.None, 0, 0, 0, false), policy.Restore());
    }
    private static void LedSurfaceRestoreOnce()
    {
        var policy = new LedSurfacePolicy();
        policy.Apply(true, LedSlotState.NativePanel, Red, Green, Blue, RedAt(1));
        Equal(new LedSurfaceStep(LedSurfaceAction.Restore, 0, 0, 0, false), policy.Restore());
        Equal(false, policy.Owned);
        Equal(new LedSurfaceStep(LedSurfaceAction.None, 0, 0, 0, false), policy.Restore());
    }
    private static void LedSurfaceOriginalReset()
    {
        var policy = new LedSurfacePolicy();
        policy.Apply(true, LedSlotState.NativePanel, Red, Green, Blue, RedAt(1));
        // Original at 100 percent removes every owned influence: the native slot is restored.
        Equal(new LedSurfaceStep(LedSurfaceAction.Restore, 0, 0, 0, false), policy.Apply(true, LedSlotState.NativePanel, Red, Green, Blue, Original(1)));
        Equal(false, policy.Owned);
        // Nothing owned and Original at 100 percent: nothing to do.
        Equal(new LedSurfaceStep(LedSurfaceAction.None, 0, 0, 0, false), new LedSurfacePolicy().Apply(true, LedSlotState.NativePanel, Red, Green, Blue, Original(1)));
    }
    private static void LedSurfaceOffRestoresClone()
    {
        var policy = new LedSurfacePolicy();
        policy.Apply(true, LedSlotState.NativePanel, Red, Green, Blue, RedAt(1));
        // The lamp switched off while our clone still occupied the slot: the native material wins.
        Equal(new LedSurfaceStep(LedSurfaceAction.Restore, 0, 0, 0, false), policy.Apply(false, LedSlotState.OwnClone, Red, Green, Blue, RedAt(1)));
        Equal(false, policy.Owned);
    }
    private static void LedSurfaceOwnCloneKeeps()
    {
        var policy = new LedSurfacePolicy();
        policy.Apply(true, LedSlotState.NativePanel, Red, Green, Blue, RedAt(1));
        var step = policy.Apply(true, LedSlotState.OwnClone, Red, Green, Blue, Color(new RgbColor(0, 0, 1), 1));
        Equal(LedSurfaceAction.Install, step.Action);
        Equal(true, step.Owned);
        Close(Blue, step.B);
    }
    private static void LedSurfaceAllLevels()
    {
        foreach (IntensityLevel level in Enum.GetValues<IntensityLevel>())
        {
            double factor = (int)level / 100.0;
            var step = new LedSurfacePolicy().Apply(true, LedSlotState.NativePanel, 2f, 0f, 0f, RedAt(factor));
            Equal(LedSurfaceAction.Install, step.Action);
            Close((float)(2f * factor), step.R);
        }
    }
    private static void LedSurfaceOwnedMatch()
    {
        var policy = new LedSurfacePolicy();
        Equal(false, policy.MatchesOwnedOutput(0f, 0f, 0f));
        policy.Apply(true, LedSlotState.NativePanel, Red, Green, Blue, RedAt(1));
        Equal(true, policy.MatchesOwnedOutput(Blue, 0f, 0f));
        Equal(false, policy.MatchesOwnedOutput(1f, 0f, 0f));
        Equal(false, policy.MatchesOwnedOutput(float.NaN, 0f, 0f));
    }
    private static void LedSurfaceValidity()
    {
        Equal(false, LedSurfacePolicy.IsValid(float.NaN, 0, 0));
        Equal(false, LedSurfacePolicy.IsValid(0, float.PositiveInfinity, 0));
        Equal(false, LedSurfacePolicy.IsValid(0, 0, float.NegativeInfinity));
        Equal(false, LedSurfacePolicy.IsValid(-0.001f, 0, 0));
        Equal(true, LedSurfacePolicy.IsValid(0, 0, 0));
        Equal(true, LedSurfacePolicy.IsValid(12.358f, 0.175f, 2.5f));
    }

    /// <summary>
    /// The unified scope contract: the runtime now drives the lamp's own emitting LED panel AND the
    /// lamp's own native light sources, and nothing else. This replaces the obsolete surface-only
    /// assertion set (the owner explicitly requested that the emitted light follow the panel colour).
    /// </summary>
    private static void UnifiedScopeWiring()
    {
        string source = ReadSource("LightRuntime.cs");
        // The coupled native-light path exists beside the LED panel, fed by the exact own sources.
        Equal(true, source.Contains("OptimizedLight", StringComparison.Ordinal));
        Equal(true, source.Contains("lightSources", StringComparison.Ordinal));
        Equal(true, source.Contains("_Light", StringComparison.Ordinal));
        Equal(true, source.Contains(".intensity =", StringComparison.Ordinal));
        Equal(true, source.Contains(".color =", StringComparison.Ordinal));
        Equal(true, source.Contains("NativeLightBinding", StringComparison.Ordinal));
        Equal(true, source.Contains("NativeLightPolicy", StringComparison.Ordinal));
        Equal(true, source.Contains("NativeColorPolicy", StringComparison.Ordinal));
        // Nothing else about a native light is ever changed: no geometry, cone, cookie, culling,
        // shadow, enabled flag or switch state.
        Equal(false, source.Contains(".range", StringComparison.Ordinal));
        Equal(false, source.Contains(".spotAngle", StringComparison.Ordinal));
        Equal(false, source.Contains(".innerSpotAngle", StringComparison.Ordinal));
        Equal(false, source.Contains(".cookie", StringComparison.Ordinal));
        Equal(false, source.Contains(".shadows", StringComparison.Ordinal));
        Equal(false, source.Contains(".cullingMask", StringComparison.Ordinal));
        Equal(false, source.Contains(".renderMode", StringComparison.Ordinal));
        Equal(false, source.Contains(".enabled", StringComparison.Ordinal));
        Equal(false, source.Contains("SetParent", StringComparison.Ordinal));
        Equal(false, source.Contains(".localPosition", StringComparison.Ordinal));
        Equal(false, source.Contains(".localRotation", StringComparison.Ordinal));
        Equal(false, source.Contains("TurnOn", StringComparison.Ordinal));
        Equal(false, source.Contains("TurnOff", StringComparison.Ordinal));
        Equal(false, source.Contains("SetIsOn", StringComparison.Ordinal));
        Equal(false, source.Contains("isOn =", StringComparison.Ordinal));
        // Plant and world materials are never accessed: the only material write is our own panel clone.
        Equal(false, source.Contains("SetFloat(", StringComparison.Ordinal));
        Equal(false, source.Contains("_BaseColor", StringComparison.Ordinal));
        // No shader object or shader keyword is ever touched (the word may still appear in a comment).
        Equal(false, source.Contains(".shader", StringComparison.Ordinal));
        Equal(false, source.Contains("Shader.", StringComparison.Ordinal));
        Equal(false, source.Contains("shaderKeywords", StringComparison.OrdinalIgnoreCase));
        Equal(false, source.Contains("GetComponents", StringComparison.Ordinal));
    }
    private static void SurfaceCloneWiring()
    {
        string source = ReadSource("LightRuntime.cs");
        // Only the reference-returning sharedMaterials accessor is used: neither .material nor
        // .materials may appear, because those instantiate a material per call.
        Equal(false, source.Contains(".material", StringComparison.Ordinal));
        Equal(true, source.Contains("sharedMaterials", StringComparison.Ordinal));
        // The LED surface is recoloured by an owned clone of the native material.
        Equal(true, source.Contains("new Material(", StringComparison.Ordinal));
        Equal(true, source.Contains("\"_EmissionColor\"", StringComparison.Ordinal));
        Equal(true, source.Contains("HasProperty", StringComparison.Ordinal));
        Equal(true, source.Contains("SetColor", StringComparison.Ordinal));
        // Only the verified LED panel material family is ever touched.
        Equal(true, source.Contains("ledlight_light_off", StringComparison.Ordinal));
        Equal(true, source.Contains("ledlight_light_on", StringComparison.Ordinal));
        // Registration is gated on the verified panel pairs (LED, and the grow-tent halogen pair).
        // Full-spectrum is still rejected.
        Equal(true, source.Contains("IsVerifiedLedFamily", StringComparison.Ordinal));
        Equal(true, source.Contains("LedFamilyOnMaterial", StringComparison.Ordinal));
        Equal(true, source.Contains("LedFamilyOffMaterial", StringComparison.Ordinal));
        // The per-frame path settles: an unchanged switch state and instruction re-read nothing.
        Equal(true, source.Contains("Converged", StringComparison.Ordinal));
        Equal(true, source.Contains("LastInstruction", StringComparison.Ordinal));
        Equal(true, source.Contains("LastOn", StringComparison.Ordinal));
    }
    private static void SurfaceLifecycleWiring()
    {
        string source = ReadSource("LightRuntime.cs");
        Equal(true, source.Contains("lightSurfacesMeshes", StringComparison.Ordinal));
        Equal(true, source.Contains("MaterialIndex", StringComparison.Ordinal));
        Equal(true, source.Contains("isOn", StringComparison.Ordinal));
        Equal(true, source.Contains("RestoreSurface", StringComparison.Ordinal));
        Equal(true, source.Contains("ReleaseSurface", StringComparison.Ordinal));
        Equal(true, source.Contains("Destroy(", StringComparison.Ordinal));
        // The surface path is driven by the pure policy, not by ad-hoc arithmetic.
        Equal(true, source.Contains("LedSurfacePolicy", StringComparison.Ordinal));
    }
    private static void AppUnifiedScopeWiring()
    {
        string source = ReadSource("LightControlApp.cs");
        Equal(true, source.Contains("LED panel and emitted light use the same color", StringComparison.Ordinal));
        Equal(true, source.Contains("SetIntensity", StringComparison.Ordinal));
        // The obsolete surface-only claims are gone.
        Equal(false, source.Contains("room lighting unchanged", StringComparison.Ordinal));
        Equal(false, source.Contains("LED surface only", StringComparison.Ordinal));
        Equal(false, source.Contains("SURFACE BRIGHTNESS", StringComparison.Ordinal));
    }

    // ---- LED-surface clone ownership / release (RED/GREEN regressions, Unity-free) ---------

    private static void LedOwnershipSourceStable()
    {
        var native = new object();
        var clone = new object();
        // Native slot: the clone is created from the slot's current native material.
        Equal<object?>(native, LedSurfaceOwnership.CloneSource(false, native, null));
        // Our own clone already occupies the slot: the source stays the installed native
        // material, never the clone itself (that would chain and compound the colour).
        Equal<object?>(native, LedSurfaceOwnership.CloneSource(true, clone, native));
        // The first install always needs a clone.
        Equal(true, LedSurfaceOwnership.NeedsClone<object>(null, native, null));
        // The same native source is reused, so the clone is never recreated.
        Equal(false, LedSurfaceOwnership.NeedsClone(clone, native, native));
        // A genuinely different native source needs a replacement clone.
        Equal(true, LedSurfaceOwnership.NeedsClone(clone, new object(), native));
    }
    private static void LedOwnershipSourceNeverDrifts()
    {
        var native = new object();
        object? clone = null;
        object? installedSource = null;
        // Install 1: from the native slot material.
        Equal(true, LedSurfaceOwnership.NeedsClone(clone, LedSurfaceOwnership.CloneSource(false, native, installedSource), installedSource));
        clone = new object();
        installedSource = native;
        // Three further instruction changes while our own clone holds the slot: the chosen source is
        // the native material every time and the clone is reused, so nothing chains or leaks.
        for (int change = 0; change < 3; change++)
        {
            object? source = LedSurfaceOwnership.CloneSource(true, clone, installedSource);
            Equal<object?>(native, source);
            Equal(false, LedSurfaceOwnership.NeedsClone(clone, source, installedSource));
            installedSource = source;
        }
        Equal<object?>(native, installedSource);
    }
    private static void LedOwnershipReleaseDeferred()
    {
        // No clone at all: nothing to release.
        Equal(LedReleaseOutcome.Nothing, LedSurfaceOwnership.DecideRelease(false, true, true, false, false, false, false));
        // The renderer is not alive: the wrapper cannot prove the native renderer is gone.
        Equal(LedReleaseOutcome.Deferred, LedSurfaceOwnership.DecideRelease(true, false, true, true, true, true, true));
        // The material array could not be read.
        Equal(LedReleaseOutcome.Deferred, LedSurfaceOwnership.DecideRelease(true, true, false, true, true, true, true));
        // The native source we would restore is gone.
        Equal(LedReleaseOutcome.Deferred, LedSurfaceOwnership.DecideRelease(true, true, true, true, true, false, false));
        // The native setter threw: the clone is still in the slot.
        Equal(LedReleaseOutcome.Deferred, LedSurfaceOwnership.DecideRelease(true, true, true, true, true, true, false));
        // The clone is referenced somewhere but not in our exact owned slot: never write a foreign slot.
        Equal(LedReleaseOutcome.Deferred, LedSurfaceOwnership.DecideRelease(true, true, true, true, false, true, false));
    }
    private static void LedOwnershipReleaseSafe()
    {
        // The renderer is readable and references our clone nowhere: disconnected, destroy is safe.
        Equal(LedReleaseOutcome.Disconnected, LedSurfaceOwnership.DecideRelease(true, true, true, false, false, true, false));
        // The exact owned slot held our clone and was written back to the live native material.
        Equal(LedReleaseOutcome.Restored, LedSurfaceOwnership.DecideRelease(true, true, true, true, true, true, true));
    }
    private static void LedOwnershipWiring()
    {
        string source = ReadSource("LightRuntime.cs");
        // The clone source is chosen through the ownership helper, so an own-clone slot reuses the
        // installed native source instead of the clone itself.
        Equal(true, source.Contains("LedSurfaceOwnership.CloneSource", StringComparison.Ordinal));
        Equal(false, source.Contains("Material? source = current;", StringComparison.Ordinal));
        // The release decision is proof-gated and unproven clones are retained, never destroyed.
        Equal(true, source.Contains("LedSurfaceOwnership.DecideRelease", StringComparison.Ordinal));
        Equal(true, source.Contains("LedSurfaceOwnership.NeedsClone", StringComparison.Ordinal));
        Equal(true, source.Contains("PendingReleases", StringComparison.Ordinal));
        Equal(true, source.Contains("RetryPendingReleases", StringComparison.Ordinal));
        // The tracked slot is only dropped once the restore is proven; a failed restore keeps it.
        string restore = Tail(source, "private static LedReleaseOutcome RestoreSurface");
        Equal(true, restore.Contains("LedReleaseOutcome outcome = LedSurfaceOwnership.DecideRelease", StringComparison.Ordinal));
        Equal(false, restore.Contains("int slot = surface.InstalledSlot;\n        surface.InstalledSlot = -1;", StringComparison.Ordinal));
    }

    // ---- Native material identity (RED/GREEN regressions, Unity-free) ---------------------

    private static void NativeIdentityUsableId()
    {
        Equal(false, NativeMaterialIdentity.IsUsable(0));
        Equal(true, NativeMaterialIdentity.IsUsable(0x1000));
        Equal(false, NativeMaterialIdentity.SameNative(0, 0));
        Equal(false, NativeMaterialIdentity.SameNative(0, 0x1000));
        Equal(false, NativeMaterialIdentity.SameNative(0x1000, 0));
        Equal(true, NativeMaterialIdentity.SameNative(0x1000, 0x1000));
        Equal(false, NativeMaterialIdentity.SameNative(0x1000, 0x2000));
    }
    private static void NativeIdentityDistinctWrappers()
    {
        var native = new NativeMaterial(0x1000, "ledlight_light_on", Red, Green, Blue);
        var first = new MaterialWrapper(native);
        var second = new MaterialWrapper(native);
        // Two reads of one native material are two distinct managed objects.
        Equal(false, ReferenceEquals(first, second));
        Equal(true, NativeMaterialIdentity.SameNative(first, second, WrapperId));
        // A distinct native material with an identical name is never the same material.
        var impostor = new NativeMaterial(0x2000, "ledlight_light_on", Red, Green, Blue);
        Equal(false, NativeMaterialIdentity.SameNative(first, new MaterialWrapper(impostor), WrapperId));
        // The managed fast path still accepts one and the same wrapper.
        Equal(true, NativeMaterialIdentity.SameNative(first, first, WrapperId));
        // A missing side is never a match.
        Equal(false, NativeMaterialIdentity.SameNative<MaterialWrapper>(first, null, WrapperId));
        Equal(false, NativeMaterialIdentity.SameNative<MaterialWrapper>(null, first, WrapperId));
        Equal(false, NativeMaterialIdentity.SameNative<MaterialWrapper>(null, null, WrapperId));
    }
    private static void NativeIdentityUnreadable()
    {
        var live = new MaterialWrapper(new NativeMaterial(0x1000, "ledlight_light_on", Red, Green, Blue));
        var disposed = new MaterialWrapper(null);
        // A disposed wrapper throws on read: never equal to a live material.
        Equal(false, NativeMaterialIdentity.SameNative(disposed, live, WrapperId));
        Equal(false, NativeMaterialIdentity.SameNative(live, disposed, WrapperId));
        // The managed fast path accepts one and the same wrapper, which is trivially that material.
        Equal(true, NativeMaterialIdentity.SameNative(disposed, disposed, WrapperId));
        // Two DIFFERENT unreadable wrappers are never the same material.
        Equal(false, NativeMaterialIdentity.SameNative(new MaterialWrapper(null), new MaterialWrapper(null), WrapperId));
        // A reader that throws is contained and fails closed, even with the fast path disabled.
        Equal(false, NativeMaterialIdentity.SameNative(live, live, _ => throw new ObjectDisposedException("wrapper"), (_, _) => false));
        // A zero id is "unknown", never a match.
        var zero = new MaterialWrapper(new NativeMaterial(0, "ledlight_light_on", Red, Green, Blue));
        Equal(false, NativeMaterialIdentity.SameNative(live, zero, WrapperId));
    }
    private static void NativeArrayScanIncomplete()
    {
        // A fully readable array that does not hold our clone proves disconnection.
        var clean = new NativeMaterialArrayScan();
        clean.Record(0x1000, 0x2000);
        clean.Record(0x3000, 0x2000);
        Equal(true, clean.FullyReadable);
        Equal(false, clean.ReferencesClone);
        Equal(true, clean.ProvesDisconnected);
        Equal(LedReleaseOutcome.Disconnected, LedSurfaceOwnership.DecideRelease(true, true, clean.FullyReadable, clean.ReferencesClone, false, true, false));

        // One unreadable element makes the scan incomplete: never a disconnect proof, never a destroy.
        var partial = new NativeMaterialArrayScan();
        partial.Record(0x1000, 0x2000);
        partial.Record(0, 0x2000);
        Equal(false, partial.FullyReadable);
        Equal(false, partial.ProvesDisconnected);
        Equal(LedReleaseOutcome.Deferred, LedSurfaceOwnership.DecideRelease(true, true, partial.FullyReadable, partial.ReferencesClone, false, true, false));

        // A whole unreadable array behaves the same.
        var missing = new NativeMaterialArrayScan();
        missing.MarkUnreadable();
        Equal(LedReleaseOutcome.Deferred, LedSurfaceOwnership.DecideRelease(true, true, missing.FullyReadable, missing.ReferencesClone, false, true, false));

        // The clone found in the exact owned slot is restored, not disconnected.
        var ours = new NativeMaterialArrayScan();
        ours.Record(0x2000, 0x2000);
        Equal(true, ours.ReferencesClone);
        Equal(false, ours.ProvesDisconnected);
        Equal(LedReleaseOutcome.Restored, LedSurfaceOwnership.DecideRelease(true, true, ours.FullyReadable, ours.ReferencesClone, true, true, true));
    }
    private static void NativeIdentityReusesOwnClone()
    {
        var native = new NativeMaterial(0x1000, "ledlight_light_on", Red, Green, Blue);
        var slot = new FakeMaterialSlot { Current = new MaterialWrapper(native) };
        var policy = new LedSurfacePolicy();

        // Frame 1 — native panel: install our own clone of the native material.
        var first = LedFrame(slot, policy, RedAt(1));
        Equal(LedSurfaceAction.Install, first.Action);
        Equal(true, policy.Owned);
        Equal(0x2000L, slot.Clone!.NativeId);
        Equal(true, ReferenceEquals(slot.Current, slot.Clone));
        Close(Blue, slot.Clone.Native!.R);
        Close(Red, native.R);
        Close(Green, native.G);
        Close(Blue, native.B);

        // Frame 2 — a colour change while the slot holds OUR clone, read back through a new wrapper.
        // Native identity reuses the same clone and keeps the native source; managed reference
        // equality misclassified it as foreign and stopped applying colours after the first one.
        MaterialWrapper cloneHandle = slot.Clone!;
        MaterialWrapper sourceHandle = slot.InstalledSource!;
        var second = LedFrame(slot, policy, Color(new RgbColor(0, 0, 1), 1));
        Equal(LedSurfaceAction.Install, second.Action);
        Equal(true, ReferenceEquals(cloneHandle, slot.Clone));
        Equal(true, ReferenceEquals(sourceHandle, slot.InstalledSource));
        Close(Blue, slot.Clone!.Native!.B);
        Close(0f, slot.Clone.Native.R);

        // Frame 3 — a third colour change: still the same clone, no residue, no compounding.
        var third = LedFrame(slot, policy, Color(new RgbColor(0, 1, 0), 1));
        Equal(LedSurfaceAction.Install, third.Action);
        Equal(true, ReferenceEquals(cloneHandle, slot.Clone));
        Close(Blue, slot.Clone!.Native!.G);
        Close(0f, slot.Clone.Native.B);
        Close(Red, native.R);
        Close(Green, native.G);
        Close(Blue, native.B);
    }
    private static void NativeIdentityRestoresAcrossWrappers()
    {
        var native = new NativeMaterial(0x1000, "ledlight_light_on", Red, Green, Blue);
        var slot = new FakeMaterialSlot { Current = new MaterialWrapper(native) };
        var policy = new LedSurfacePolicy();
        Equal(LedSurfaceAction.Install, LedFrame(slot, policy, RedAt(1)).Action);

        // The native on/off swap put the SAME native material back through a new wrapper: no
        // replacement clone is created for it. Managed reference equality re-cloned here.
        MaterialWrapper cloneHandle = slot.Clone!;
        MaterialWrapper sourceHandle = slot.InstalledSource!;
        slot.Current = new MaterialWrapper(native);
        Equal(LedSurfaceAction.Install, LedFrame(slot, policy, RedAt(1)).Action);
        Equal(true, ReferenceEquals(cloneHandle, slot.Clone));
        Equal(true, ReferenceEquals(sourceHandle, slot.InstalledSource));

        // A FOREIGN material with an identical name is never adopted as our own clone.
        var impostor = new NativeMaterial(0x3000, "ledlight_light_on (LightControl)", 1f, 1f, 1f);
        slot.Current = new MaterialWrapper(impostor);
        Equal(false, slot.Owns(slot.Read()));
        Equal(LedSurfaceAction.None, LedFrame(slot, policy, RedAt(1)).Action);

        // Original at 100% restores the exact native material into the slot, decided across wrappers.
        slot.Current = slot.Clone;
        var restore = policy.Apply(true, LedSurfacePolicy.Classify(true, slot.Owns(slot.Read()), false), native.R, native.G, native.B, Original(1));
        Equal(LedSurfaceAction.Restore, restore.Action);
        Equal(false, policy.Owned);
        slot.Current = slot.InstalledSource;
        Equal(true, NativeMaterialIdentity.SameNative(slot.Read(), new MaterialWrapper(native), WrapperId));
    }
    private static void LedNeedsCloneNativeIdentity()
    {
        var native = new NativeMaterial(0x1000, "ledlight_light_on", Red, Green, Blue);
        var clone = new MaterialWrapper(new NativeMaterial(0x2000, "ledlight_light_on (LightControl)", 0f, 0f, 0f));
        // No clone yet: always create one.
        Equal(true, LedSurfaceOwnership.NeedsClone<MaterialWrapper>(null, new MaterialWrapper(native), null, WrapperId));
        // The same native material read through two different wrappers reuses the existing clone.
        Equal(false, LedSurfaceOwnership.NeedsClone(clone, new MaterialWrapper(native), new MaterialWrapper(native), WrapperId));
        // A different native material with an identical name needs a replacement clone.
        var impostor = new NativeMaterial(0x3000, "ledlight_light_on", Red, Green, Blue);
        Equal(true, LedSurfaceOwnership.NeedsClone(clone, new MaterialWrapper(impostor), new MaterialWrapper(native), WrapperId));
        // An unreadable source can never reuse the clone.
        Equal(true, LedSurfaceOwnership.NeedsClone(clone, new MaterialWrapper(null), new MaterialWrapper(native), WrapperId));
        // The managed-reference overload is unchanged for handles without a native identity.
        object referenceOnly = new();
        Equal(false, LedSurfaceOwnership.NeedsClone(referenceOnly, referenceOnly, referenceOnly));
    }
    private static void LedRestoreConverged()
    {
        // A deferred restore must be retried: reporting it as converged drops the pending Original.
        Equal(false, LedSurfaceOwnership.RestoreConverged(LedReleaseOutcome.Deferred));
        Equal(true, LedSurfaceOwnership.RestoreConverged(LedReleaseOutcome.Nothing));
        Equal(true, LedSurfaceOwnership.RestoreConverged(LedReleaseOutcome.Disconnected));
        Equal(true, LedSurfaceOwnership.RestoreConverged(LedReleaseOutcome.Restored));
    }
    private static void NativeIdentityWiring()
    {
        string source = ReadSource("LightRuntime.cs");
        // Every native material comparison goes through the one native-id helper. On active code lines
        // managed reference equality may only ever be used for the lamp-record identity check.
        foreach (string line in source.Split('\n'))
        {
            string trimmed = line.Trim();
            if (trimmed.StartsWith("//", StringComparison.Ordinal)) continue;
            if (!trimmed.Contains("ReferenceEquals", StringComparison.Ordinal)) continue;
            Equal(true, trimmed.Contains("ReferenceEquals(lamp, exclude)", StringComparison.Ordinal) ||
                trimmed.Contains("ReferenceEquals(current, lamp)", StringComparison.Ordinal));
        }
        Equal(true, source.Contains("SameNativeMaterial", StringComparison.Ordinal));
        Equal(true, source.Contains("NativeMaterialIdentity", StringComparison.Ordinal));
        Equal(true, source.Contains("NativeMaterialId", StringComparison.Ordinal));
        // An unreadable element can never prove disconnection.
        Equal(true, source.Contains("NativeMaterialArrayScan", StringComparison.Ordinal));
        // The clone source is decided by native identity, not by managed reference.
        Equal(true, source.Contains("LedSurfaceOwnership.NeedsClone(surface.Clone, source, surface.InstalledSource, NativeMaterialId)", StringComparison.Ordinal));
        // A deferred restore must not be reported as converged.
        Equal(true, source.Contains("LedSurfaceOwnership.RestoreConverged(RestoreSurface(", StringComparison.Ordinal));
        // One bounded apply breadcrumb per real instruction change, with the actual emission readback.
        Equal(true, source.Contains("[apply] lamp", StringComparison.Ordinal));
        Equal(true, source.Contains("LastApplyLog", StringComparison.Ordinal));
        Equal(true, source.Contains("GetColor(EmissionProperty)", StringComparison.Ordinal));
    }

    // ---- Coupled LED panel + emitted native light (RED/GREEN regressions, Unity-free) -------

    /// <summary>The lamp's own native light baseline used by every coupled check below.</summary>
    private const float NativeIntensity = 1.2f;
    private const float NativeR = 0.9f, NativeG = 0.2f, NativeB = 0.9f;

    private static void CoupledReadiness()
    {
        // Both outputs resolvable: the lamp may be registered for coupled control.
        Equal(LampCouplingState.Ready, NativeCouplingPolicy.Decide(true, true, true));
        Equal(LampCouplingState.Ready, NativeCouplingPolicy.Decide(true, true, false));
        // An OFF lamp whose OptimizedLight._Light has not resolved yet is pending, never enabled by force.
        Equal(LampCouplingState.Pending, NativeCouplingPolicy.Decide(true, false, false));
        Equal(LampCouplingState.Pending, NativeCouplingPolicy.Decide(false, false, false));
        // A lamp that is ON but exposes no readable native light source is reported, never a
        // panel-only fallback: coupled mode fails closed.
        Equal(LampCouplingState.Unsupported, NativeCouplingPolicy.Decide(true, false, true));
        Equal(LampCouplingState.Unsupported, NativeCouplingPolicy.Decide(false, true, true));
        Equal(LampCouplingState.Unsupported, NativeCouplingPolicy.Decide(false, false, true));
    }
    private static void CoupledSameHue()
    {
        var lamp = new FakeLamp();
        UpdateFrame(lamp);
        CoupledFrame frame = LateFrame(lamp, RedAt(1));

        // The emitted light carries the chosen hue absolutely, with the native alpha preserved.
        Equal(true, frame.Light.Color.ShouldWrite);
        Close(1f, lamp.Light.R); Close(0f, lamp.Light.G); Close(0f, lamp.Light.B); Close(1f, lamp.Light.A);
        // The panel carries the same hue, scaled by the native emission magnitude.
        Equal(LedSurfaceAction.Install, frame.Panel.Action);
        Close(Blue, lamp.Slot.Clone!.Native!.R);
        Close(0f, lamp.Slot.Clone.Native.G);
        Close(0f, lamp.Slot.Clone.Native.B);
        // Same normalized hue on the LED panel and on the emitted light.
        Equal(NormalizedHue(1f, 0f, 0f), PanelHue(lamp));
        Equal(PanelHue(lamp), NormalizedHue(lamp.Light.R, lamp.Light.G, lamp.Light.B));
        // The emitted intensity is the lamp's own native magnitude scaled by the chosen factor.
        Equal(true, frame.Light.Intensity.ShouldWrite);
        Close(NativeIntensity, lamp.Light.Intensity);
    }
    private static void CoupledSecondAndThirdPick()
    {
        var lamp = new FakeLamp();
        UpdateFrame(lamp);
        LateFrame(lamp, RedAt(1));
        MaterialWrapper cloneHandle = lamp.Slot.Clone!;
        MaterialWrapper sourceHandle = lamp.Slot.InstalledSource!;

        // Second pick: both outputs follow through freshly read wrappers, and the native material
        // identity of the clone and its source is retained (no re-clone, no compounding).
        UpdateFrame(lamp);
        CoupledFrame second = LateFrame(lamp, Color(new RgbColor(0, 0, 1), 1));
        Equal(true, ReferenceEquals(cloneHandle, lamp.Slot.Clone));
        Equal(true, ReferenceEquals(sourceHandle, lamp.Slot.InstalledSource));
        Close(0f, lamp.Light.R); Close(0f, lamp.Light.G); Close(1f, lamp.Light.B);
        Close(Blue, lamp.Slot.Clone!.Native!.B);
        Equal(NormalizedHue(0f, 0f, 1f), PanelHue(lamp));
        Equal(PanelHue(lamp), NormalizedHue(lamp.Light.R, lamp.Light.G, lamp.Light.B));
        Equal(true, second.Light.Color.ShouldWrite);

        // Third pick with a different brightness: still the same clone and the same native source.
        UpdateFrame(lamp);
        CoupledFrame third = LateFrame(lamp, Color(new RgbColor(0, 1, 0), 1.5));
        Equal(true, ReferenceEquals(cloneHandle, lamp.Slot.Clone));
        Equal(true, ReferenceEquals(sourceHandle, lamp.Slot.InstalledSource));
        Close(0f, lamp.Light.R); Close(1f, lamp.Light.G); Close(0f, lamp.Light.B);
        Close(NativeIntensity * 1.5f, lamp.Light.Intensity);
        Equal(NormalizedHue(0f, 1f, 0f), PanelHue(lamp));
        Equal(PanelHue(lamp), NormalizedHue(lamp.Light.R, lamp.Light.G, lamp.Light.B));
        // The native material the clone came from is never mutated by our writes.
        Close(Red, lamp.Slot.InstalledSource!.Native!.R);
        Close(Green, lamp.Slot.InstalledSource.Native.G);
        Close(Blue, lamp.Slot.InstalledSource.Native.B);
    }
    private static void CoupledOriginalRestoresBoth()
    {
        var lamp = new FakeLamp();
        UpdateFrame(lamp);
        LateFrame(lamp, RedAt(2));
        Close(NativeIntensity * 2f, lamp.Light.Intensity);
        Equal(true, lamp.Binding.OwnsAny);
        Equal(true, lamp.Panel.Owned);

        // Original at 100%: the update-phase undo hands both properties back, and the late-update
        // instruction removes every owned influence.
        UpdateFrame(lamp);
        CoupledFrame frame = LateFrame(lamp, Original(1));
        Equal(LedSurfaceAction.Restore, frame.Panel.Action);
        Equal(false, lamp.Panel.Owned);
        Equal(true, NativeMaterialIdentity.SameNative(lamp.Slot.Read(), lamp.Slot.InstalledSource, WrapperId));
        Close(NativeIntensity, lamp.Light.Intensity);
        Close(NativeR, lamp.Light.R); Close(NativeG, lamp.Light.G); Close(NativeB, lamp.Light.B);
        Equal(false, lamp.Binding.OwnsAny);
        // Nothing owned: the only remaining write is the proven policy's idempotent hand-back of the
        // value it just sampled, and no colour is touched at all.
        UpdateFrame(lamp);
        CoupledFrame again = LateFrame(lamp, Original(1));
        Equal(true, again.Light.Intensity.ShouldWrite);
        Close(NativeIntensity, again.Light.Intensity.Intensity);
        Equal(false, again.Light.Color.ShouldWrite);
        Equal(LedSurfaceAction.None, again.Panel.Action);
    }
    private static void CoupledSwitchedOff()
    {
        var lamp = new FakeLamp();
        UpdateFrame(lamp);
        LateFrame(lamp, RedAt(2));
        Equal(true, lamp.Binding.OwnsAny);

        // The game's own switch-off wrote its own value before our next frame.
        lamp.On = false;
        lamp.Light.Intensity = 0f;
        UpdateFrame(lamp);
        // Our output is gone, so the undo preserves the native value and releases ownership.
        Close(0f, lamp.Light.Intensity);
        Equal(false, lamp.Binding.IntensityOwned);

        CoupledFrame frame = LateFrame(lamp, RedAt(2));
        // A switched-off lamp never emits our illumination, and is never forced on.
        Close(0f, lamp.Light.Intensity);
        Equal(false, frame.Light.Intensity.ShouldWrite);
        Equal(false, lamp.Binding.OwnsAny);
        // The colour we had installed is handed back, and the panel returns to the native material.
        Close(NativeR, lamp.Light.R); Close(NativeG, lamp.Light.G); Close(NativeB, lamp.Light.B);
        Equal(LedSurfaceAction.Restore, frame.Panel.Action);
        Equal(false, lamp.Panel.Owned);
    }
    private static void CoupledIntensityLevels()
    {
        foreach (IntensityLevel level in Enum.GetValues<IntensityLevel>())
        {
            double factor = (int)level / 100.0;
            var lamp = new FakeLamp();
            UpdateFrame(lamp);
            CoupledFrame first = LateFrame(lamp, RedAt(factor));
            Equal(true, first.Light.Intensity.ShouldWrite);
            Close((float)(NativeIntensity * factor), lamp.Light.Intensity);
            // Two further identical frames rebase on the same native value: never compounding.
            UpdateFrame(lamp);
            CoupledFrame second = LateFrame(lamp, RedAt(factor));
            Close((float)(NativeIntensity * factor), second.Light.Intensity.Intensity);
            UpdateFrame(lamp);
            CoupledFrame third = LateFrame(lamp, RedAt(factor));
            Close((float)(NativeIntensity * factor), third.Light.Intensity.Intensity);
        }

        // A native intensity of zero stays zero at every factor.
        var dark = new FakeLamp();
        dark.Light.Intensity = 0f;
        UpdateFrame(dark);
        CoupledFrame zero = LateFrame(dark, RedAt(2));
        Equal(true, zero.Light.Intensity.ShouldWrite);
        Close(0f, dark.Light.Intensity);
    }
    private static void CoupledExternalWritePreserved()
    {
        var lamp = new FakeLamp();
        UpdateFrame(lamp);
        LateFrame(lamp, RedAt(2));
        Equal(true, lamp.Binding.OwnsAny);

        // An external writer changed both native properties after our late-update output.
        lamp.Light.Intensity = 3.5f;
        lamp.Light.R = 0.4f; lamp.Light.G = 0.5f; lamp.Light.B = 0.6f;
        NativeLightApply undo = lamp.Binding.Undo(lamp.Light.Intensity, lamp.Light.R, lamp.Light.G, lamp.Light.B, lamp.Light.A);
        Equal(false, undo.Intensity.ShouldWrite);
        Equal(false, undo.Color.ShouldWrite);
        Close(3.5f, lamp.Light.Intensity);
        Close(0.4f, lamp.Light.R); Close(0.5f, lamp.Light.G); Close(0.6f, lamp.Light.B);
        Equal(false, lamp.Binding.OwnsAny);

        // On the other side of the frame our own output is still installed, so teardown hands both
        // properties back to the latest sampled native baseline.
        var owned = new FakeLamp();
        UpdateFrame(owned);
        LateFrame(owned, RedAt(2));
        NativeLightApply restore = owned.Binding.Restore(owned.Light.Intensity, owned.Light.R, owned.Light.G, owned.Light.B, owned.Light.A);
        Equal(true, restore.Intensity.ShouldWrite);
        Equal(true, restore.Color.ShouldWrite);
        Close(NativeIntensity, restore.Intensity.Intensity);
        Close(NativeR, restore.Color.R); Close(NativeG, restore.Color.G); Close(NativeB, restore.Color.B);
        Equal(false, owned.Binding.OwnsAny);
    }
    private static void CoupledFailureIsolation()
    {
        var broken = new FakeLamp();
        UpdateFrame(broken);
        LateFrame(broken, RedAt(2));
        Equal(true, broken.Binding.OwnsAny);

        // The native intensity setter threw: only that property loses ownership, so the colour we
        // did write can still be handed back on teardown.
        broken.Binding.RejectIntensityWrite();
        Equal(false, broken.Binding.IntensityOwned);
        Equal(true, broken.Binding.ColorOwned);
        NativeLightApply cleanup = broken.Binding.Restore(broken.Light.Intensity, 1f, 0f, 0f, 1f);
        Equal(false, cleanup.Intensity.ShouldWrite);
        Equal(true, cleanup.Color.ShouldWrite);
        Close(NativeR, cleanup.Color.R); Close(NativeG, cleanup.Color.G); Close(NativeB, cleanup.Color.B);
        Equal(false, broken.Binding.OwnsAny);

        // A second lamp's cleanup is completely unaffected by the first source's failure.
        var healthy = new FakeLamp();
        UpdateFrame(healthy);
        LateFrame(healthy, Color(new RgbColor(0, 1, 0), 1));
        NativeLightApply restored = healthy.Binding.Restore(healthy.Light.Intensity, healthy.Light.R, healthy.Light.G, healthy.Light.B, healthy.Light.A);
        Equal(true, restored.Intensity.ShouldWrite);
        Equal(true, restored.Color.ShouldWrite);
        Equal(false, healthy.Binding.OwnsAny);
    }
    private static void CoupledWiring()
    {
        string source = ReadSource("LightRuntime.cs");
        // The native light references come only from the lamp's own verified source list.
        Equal(true, source.Contains("toggle.lightSources", StringComparison.Ordinal));
        Equal(true, source.Contains("_Light", StringComparison.Ordinal));
        // The proven two-phase policies drive the coupled output, coordinated by the binding seam.
        Equal(true, source.Contains("NativeLightBinding", StringComparison.Ordinal));
        Equal(true, source.Contains("NativeCouplingPolicy", StringComparison.Ordinal));
        // Native light pointers are de-duplicated across lamps through the existing pure policy.
        Equal(true, source.Contains("LightPointerDedupe.SelectUnowned", StringComparison.Ordinal));
        // The update phase hands the native output back before the game's own writer runs.
        string update = Tail(source, "public static void UpdatePhase()");
        Equal(true, update.Contains("UndoLights()", StringComparison.Ordinal));
        // Coupled mode fails closed: registration requires both outputs.
        Equal(true, source.Contains("LampCouplingState.Pending", StringComparison.Ordinal));
        Equal(true, source.Contains("LampCouplingState.Unsupported", StringComparison.Ordinal));
        // Teardown, demote and removal all restore the native output.
        Equal(true, source.Contains("RestoreLightSource", StringComparison.Ordinal));
        Equal(true, source.Contains("UnsupportedLampCount", StringComparison.Ordinal));
        // A lamp that becomes controllable again drops out of the unsupported report.
        Equal(true, source.Contains("ClearUnsupportedCoupling", StringComparison.Ordinal));
        // The switch state is read once per lamp and passed on, never re-read per source.
        Equal(true, source.Contains("ReadLampSwitch", StringComparison.Ordinal));
    }

    private static void PanelFamilies()
    {
        Equal(true, PanelFamilyPolicy.IsVerifiedPair("ledlight_light_on", "ledlight_light_off"));
        Equal(true, PanelFamilyPolicy.IsVerifiedPair("ledlight_light_on (Instance)", "ledlight_light_off (Instance)"));
        Equal(true, PanelFamilyPolicy.IsVerifiedPair("halogen light on mat", "halogen light off mat"));
        Equal(true, PanelFamilyPolicy.IsVerifiedPanelName("halogen light off mat"));
        // Full-spectrum and a swapped halogen pair are not a verified family.
        Equal(false, PanelFamilyPolicy.IsVerifiedPair("fullspectrum_light_on", "fullspectrum_light_off"));
        Equal(false, PanelFamilyPolicy.IsVerifiedPair("halogen light off mat", "halogen light on mat"));
        Equal(false, PanelFamilyPolicy.IsVerifiedPair("ledlight_light_on", "halogen light off mat"));
        Equal(false, PanelFamilyPolicy.IsVerifiedPanelName("fullspectrum_light_on"));
        Equal(false, PanelFamilyPolicy.IsVerifiedPanelName(null));
    }

    private static void DestroyedParentProperty()
    {
        // The RV is prologue-only. Its lights are not tracked, destroyed or not.
        Equal(true, DestroyedPropertyPolicy.IsPrologueOnly("rv", false));
        Equal(true, DestroyedPropertyPolicy.IsPrologueOnly("RV", false));
        Equal(true, DestroyedPropertyPolicy.IsPrologueOnly(" rv ", false));
        Equal(true, DestroyedPropertyPolicy.IsPrologueOnly("motelroom", true));
        Equal(false, DestroyedPropertyPolicy.IsPrologueOnly("motelroom", false));
        Equal(false, DestroyedPropertyPolicy.IsPrologueOnly(null, false));
        Equal(false, DestroyedPropertyPolicy.IsPrologueOnly("  ", false));
        // A destroyed lamp is still removed. An ordinary live lamp is not.
        Equal(true, DestroyedPropertyPolicy.IsPermanentlyGone(false, true));
        Equal(true, DestroyedPropertyPolicy.IsPermanentlyGone(true, false));
        Equal(false, DestroyedPropertyPolicy.IsPermanentlyGone(false, false));
    }

    private static void GrowTentItemGate()
    {
        Equal(true, PanelFamilyPolicy.IsGrowTent("growtent"));
        Equal(true, PanelFamilyPolicy.IsGrowTent("GrowTent"));
        Equal(false, PanelFamilyPolicy.IsGrowTent("plasticpot"));
        Equal(false, PanelFamilyPolicy.IsGrowTent("moisturepreservingpot"));
        Equal(false, PanelFamilyPolicy.IsGrowTent("airpot"));
        Equal(false, PanelFamilyPolicy.IsGrowTent(null));
        Equal(false, PanelFamilyPolicy.IsGrowTent("  "));
    }

    private static void LampListFilter()
    {
        Equal(true, UiSelectionPolicy.Matches("Grow tent", "motel", UiSelectionPolicy.LampListFilter.All, null));
        Equal(true, UiSelectionPolicy.Matches("Grow tent", "motel", UiSelectionPolicy.LampListFilter.Tents, "motel"));
        Equal(false, UiSelectionPolicy.Matches("Grow lamp", "motel", UiSelectionPolicy.LampListFilter.Tents, null));
        Equal(false, UiSelectionPolicy.Matches("Grow tent", "barn", UiSelectionPolicy.LampListFilter.All, "motel"));
        Equal(true, UiSelectionPolicy.Matches("Grow lamp", "barn", UiSelectionPolicy.LampListFilter.Lamps, "barn"));
        Equal(false, UiSelectionPolicy.Matches("Grow tent", "barn", UiSelectionPolicy.LampListFilter.Lamps, null));
        var visible = new[] { 2, 5, 8 };
        Equal(5, UiSelectionPolicy.KeepOrFirst(5, visible));
        Equal(2, UiSelectionPolicy.KeepOrFirst(1, visible));
        Equal(-1, UiSelectionPolicy.KeepOrFirst(0, Array.Empty<int>()));
        string app = ReadSource("LightControlApp.cs");
        Equal(true, app.Contains("EOrientation.Horizontal", StringComparison.Ordinal));
        Equal(true, app.Contains("ScrollableVerticalList", StringComparison.Ordinal));
    }

    private static void ColorPalettePick()
    {
        string app = ReadSource("LightControlApp.cs");
        Equal(true, app.Contains("LightRuntime.SetColor", StringComparison.Ordinal));
        Equal(true, app.Contains("UiSelectionPolicy.PickerColorCount", StringComparison.Ordinal));
        Equal(true, app.Contains("UiSelectionPolicy.PickerColorAt", StringComparison.Ordinal));
        Equal(true, app.Contains("new Color(1f, 0f, 0f)", StringComparison.Ordinal));
        Equal(true, app.Contains("new Color(1f, 1f, 0f)", StringComparison.Ordinal));
        Equal(false, app.Contains("GamePalette.Red", StringComparison.Ordinal));
        Equal(false, app.Contains("CycleColor(_selection)", StringComparison.Ordinal));
        string runtime = ReadSource("LightRuntime.cs");
        Equal(true, runtime.Contains("public static bool SetColor", StringComparison.Ordinal));
    }

    private static void PropertyGroups()
    {
        var ordered = new List<int>();
        var groups = new List<UiSelectionPolicy.PropertyGroup>();
        UiSelectionPolicy.GroupByProperty(
            new[] { 0, 1, 2 },
            new[] { "motel", "barn", "Motel" },
            new[] { "Motel Room", "Barn", "Motel Room" },
            ordered,
            groups);
        Equal(3, ordered.Count);
        Equal(0, ordered[0]);
        Equal(1, ordered[1]);
        Equal(2, ordered[2]);
        Equal(3, groups.Count);
        Equal("Motel Room", groups[0].Name);
        Equal(1, groups[0].Count);
        Equal("Barn", groups[1].Name);
        Equal("Motel Room", groups[2].Name);

        UiSelectionPolicy.GroupByProperty(
            new[] { 4 },
            new string?[] { null },
            new string?[] { "  " },
            ordered,
            groups);
        Equal(1, groups.Count);
        Equal("Unknown", groups[0].Name);
        Equal(4, ordered[0]);

        string app = ReadSource("LightControlApp.cs");
        Equal(true, app.Contains("GroupByProperty", StringComparison.Ordinal));
        Equal(false, app.Contains("\"Tents\"", StringComparison.Ordinal));
        Equal(false, app.Contains("\"Lamps\"", StringComparison.Ordinal));
    }

    private static void PlanRules()
    {
        var visible = new[] { "a", "b", "c" };
        Equal("b", UiSelectionPolicy.ResolveSelection("b", visible, false));
        Equal("b", UiSelectionPolicy.ResolveSelection("b", new[] { "a", "c" }, false));
        Equal(null, UiSelectionPolicy.ResolveSelection("b", new[] { "a", "c" }, true));
        Equal("a", UiSelectionPolicy.ResolveSelection(null, visible, false));
        Equal(false, UiSelectionPolicy.SelectionVisible("b", new[] { "a", "c" }));

        var store = Store();
        store.Register(Lamp("one", "A"));
        store.Register(Lamp("two", "A"));
        store.Register(Lamp("three", "B"));
        long revision = store.Revision;
        long catalog = store.CatalogRevision;
        store.Move(Id("one"), "B");
        Equal(revision, store.Revision);
        NotEqual(catalog, store.CatalogRevision);
        var propertyB = store.AvailableTargetKeys(LampSelection.Property("B"));
        Equal(true, propertyB.Contains(Id("one").Key));
        Equal(false, store.AvailableTargetKeys(LampSelection.Property("A")).Contains(Id("one").Key));

        store.MarkTemporarilyUnloaded(Id("three"));
        Equal(false, store.AvailableTargetKeys(LampSelection.World()).Contains(Id("three").Key));
        Set(store, LampSelection.World(), ColorMode.Red);
        Equal(LampBaseState.Default.Color, store.GetBaseState(Id("three")).Color);
        Equal(ColorMode.Red, store.GetBaseState(Id("one")).Color);

        var shown = store.AvailableTargetKeys(LampSelection.Property("B"));
        store.MarkTemporarilyUnloaded(Id("one"));
        var current = store.AvailableTargetKeys(LampSelection.Property("B"));
        Equal(false, UiSelectionPolicy.SameKeySet(shown, current));

        long before = store.Revision;
        var same = Command(store, ControlCommand.Set(LampSelection.Lamp(Id("two")), new(ColorMode.Red)));
        Equal(CommandStatus.Unchanged, same.Status);
        Equal(before, store.Revision);

        var kept = new LampBaseState(ColorMode.Green, IntensityLevel.Percent150);
        Equal(IntensityLevel.Percent150, new LampChange(ColorMode.Blue).Apply(kept).Intensity);
        Equal(ColorMode.Green, new LampChange(intensity: IntensityLevel.Percent25).Apply(kept).Color);
        var reset = UiSelectionPolicy.OriginalReset();
        Equal(ColorMode.Original, reset.Color);
        Equal(IntensityLevel.Percent100, reset.Intensity);

        Equal(UiSelectionPolicy.SaveNotice.Blocked, UiSelectionPolicy.ClassifySave(true, true, false, 2, 2, true));
        Equal(UiSelectionPolicy.SaveNotice.WriteFailed, UiSelectionPolicy.ClassifySave(true, false, true, 3, 2, true));
        Equal(UiSelectionPolicy.SaveNotice.Pending, UiSelectionPolicy.ClassifySave(true, false, false, 3, 2, true));
        Equal(UiSelectionPolicy.SaveNotice.Saved, UiSelectionPolicy.ClassifySave(true, false, false, 3, 3, false));
        Equal(true, UiSelectionPolicy.ActionMessageVisible(10, 13.9));
        Equal(false, UiSelectionPolicy.ActionMessageVisible(10, 14));

        var ordered = new List<int>();
        var groups = new List<UiSelectionPolicy.PropertyGroup>();
        UiSelectionPolicy.GroupByProperty(new[] { 0, 1 }, new[] { "p1", "p2" }, new[] { "Motel Room", "Motel Room" }, ordered, groups);
        Equal(2, groups.Count);
    }

    private static void TentAndRvWiring()
    {
        string source = ReadSource("LightRuntime.cs");
        Equal(true, source.Contains("PanelFamilyPolicy", StringComparison.Ordinal));
        Equal(true, source.Contains("DestroyedPropertyPolicy", StringComparison.Ordinal));
        Equal(true, source.Contains("TryRegisterGrowTent", StringComparison.Ordinal));
        Equal(true, source.Contains("IsParentPropertyDestroyed", StringComparison.Ordinal));
        Equal(true, source.Contains("halogen light on mat", StringComparison.Ordinal) || source.Contains("PanelFamilyPolicy.HalogenOn", StringComparison.Ordinal));
        // Full-spectrum is still not a supported panel name in the runtime gate.
        Equal(false, source.Contains("fullspectrum_light_on", StringComparison.Ordinal));
    }

    /// <summary>Update phase of one coupled frame: hand both owned native properties back.</summary>
    private static void UpdateFrame(FakeLamp lamp)
    {
        NativeLightApply undo = lamp.Binding.Undo(lamp.Light.Intensity, lamp.Light.R, lamp.Light.G, lamp.Light.B, lamp.Light.A);
        ApplyNative(lamp, undo);
    }

    /// <summary>Late-update phase of one coupled frame: resolve the instruction onto both outputs.</summary>
    private static CoupledFrame LateFrame(FakeLamp lamp, VisualInstruction instruction)
    {
        LedSurfaceStep panel = LedFrame(lamp.Slot, lamp.Panel, instruction, lamp.On);
        NativeLightApply light = lamp.On
            ? lamp.Binding.Apply(lamp.Light.Intensity, lamp.Light.R, lamp.Light.G, lamp.Light.B, lamp.Light.A, instruction)
            : lamp.Binding.Restore(lamp.Light.Intensity, lamp.Light.R, lamp.Light.G, lamp.Light.B, lamp.Light.A);
        ApplyNative(lamp, light);
        return new CoupledFrame(panel, light);
    }

    private static void ApplyNative(FakeLamp lamp, NativeLightApply step)
    {
        if (step.Intensity.ShouldWrite) lamp.Light.Intensity = step.Intensity.Intensity;
        if (step.Color.ShouldWrite)
        {
            lamp.Light.R = step.Color.R; lamp.Light.G = step.Color.G;
            lamp.Light.B = step.Color.B; lamp.Light.A = step.Color.A;
        }
    }

    private static (float R, float G, float B) PanelHue(FakeLamp lamp) =>
        lamp.Slot.Clone?.Native is NativeMaterial material
            ? NormalizedHue(material.R, material.G, material.B)
            : (0f, 0f, 0f);

    /// <summary>
    /// The normalized hue of one output triple, so the HDR panel emission (native magnitude times the
    /// chosen colour) and the absolute emitted-light colour compare as the same colour.
    /// </summary>
    private static (float R, float G, float B) NormalizedHue(float r, float g, float b)
    {
        float max = System.Math.Max(r, System.Math.Max(g, b));
        if (max <= 0f) return (0f, 0f, 0f);
        return (Round3(r / max), Round3(g / max), Round3(b / max));
    }
    private static float Round3(float value) => System.MathF.Round(value, 3);

    private readonly record struct CoupledFrame(LedSurfaceStep Panel, NativeLightApply Light);

    /// <summary>Game-free stand-in for one Unity Light: the two properties the adapter owns.</summary>
    private sealed class FakeLight
    {
        public long NativeId = 0x4000;
        public float Intensity = NativeIntensity;
        public float R = NativeR, G = NativeG, B = NativeB, A = 1f;
    }

    /// <summary>
    /// Game-free stand-in for one coupled grow lamp: the lamp's own LED surface slot plus the lamp's
    /// own native light source, driven through the same runtime-used helpers as <c>LightRuntime</c>.
    /// </summary>
    private sealed class FakeLamp
    {
        public FakeLamp()
        {
            Slot.Current = new MaterialWrapper(new NativeMaterial(0x1000, "ledlight_light_on", Red, Green, Blue));
        }

        public FakeMaterialSlot Slot { get; } = new();
        public LedSurfacePolicy Panel { get; } = new();
        public NativeLightBinding Binding { get; } = new();
        public FakeLight Light { get; } = new();
        public bool On { get; set; } = true;
    }

    /// <summary>
    /// One late-update frame of the runtime's LED-surface decision chain for a single slot: read the
    /// slot through a fresh wrapper, decide own-clone by native id, choose the clone source and reuse
    /// or create the clone. Uses the same runtime-used helpers as <c>LightRuntime.ApplySurface</c>.
    /// </summary>
    private static LedSurfaceStep LedFrame(FakeMaterialSlot slot, LedSurfacePolicy policy, VisualInstruction instruction, bool lightOn = true)
    {
        MaterialWrapper? read = slot.Read();
        bool ownClone = slot.Owns(read);
        LedSlotState state = LedSurfacePolicy.Classify(read is not null, ownClone, read is not null && IsPanelMaterial(read));
        float baseR = 0f, baseG = 0f, baseB = 0f;
        if (state is LedSlotState.NativePanel or LedSlotState.OwnClone)
        {
            MaterialWrapper? baseline = ownClone ? slot.InstalledSource : read;
            if (baseline?.Native is null) { policy.RejectWrite(); return new(LedSurfaceAction.None, 0, 0, 0, false); }
            baseR = baseline.Native.R; baseG = baseline.Native.G; baseB = baseline.Native.B;
        }
        LedSurfaceStep step = policy.Apply(lightOn, state, baseR, baseG, baseB, instruction);
        // The native material goes back into the slot, exactly as the runtime's guarded restore does.
        if (step.Action == LedSurfaceAction.Restore) { slot.Current = slot.InstalledSource; return step; }
        if (step.Action != LedSurfaceAction.Install) return step;
        MaterialWrapper? source = LedSurfaceOwnership.CloneSource(ownClone, read, slot.InstalledSource);
        if (LedSurfaceOwnership.NeedsClone(slot.Clone, source, slot.InstalledSource, WrapperId))
        {
            slot.Clone = new MaterialWrapper(new NativeMaterial(0x2000, "ledlight_light_on (LightControl)", 0f, 0f, 0f));
            slot.InstalledSource = source;
        }
        slot.Clone!.Native!.R = step.R;
        slot.Clone.Native.G = step.G;
        slot.Clone.Native.B = step.B;
        slot.Current = slot.Clone;
        return step;
    }

    /// <summary>
    /// Game-free stand-in for one native material. The values live here, so reading the material
    /// through any wrapper sees the same data — like the native object behind an IL2CPP proxy.
    /// </summary>
    private sealed class NativeMaterial
    {
        public long NativeId;
        public string Name;
        public float R, G, B;

        public NativeMaterial(long nativeId, string name, float r, float g, float b)
        {
            NativeId = nativeId; Name = name; R = r; G = g; B = b;
        }
    }

    /// <summary>
    /// One managed wrapper around a native material. Reading the renderer slot yields a FRESH wrapper
    /// each time (the IL2CPP behaviour that broke <c>ReferenceEquals</c>), and a disposed wrapper
    /// throws on any read, like <c>Il2CppObjectBase.Pointer</c> does on a collected proxy.
    /// </summary>
    private sealed class MaterialWrapper
    {
        public MaterialWrapper(NativeMaterial? native) => Native = native;

        public NativeMaterial? Native { get; }

        public long NativeId => Native is null ? throw new ObjectDisposedException(nameof(MaterialWrapper)) : Native.NativeId;
    }

    private static readonly Func<MaterialWrapper, long> WrapperId = wrapper => wrapper.NativeId;

    /// <summary>
    /// Game-free stand-in for the renderer's material slot and our own clone handle, mirroring the
    /// identity decisions <c>LightRuntime</c> makes.
    /// </summary>
    private sealed class FakeMaterialSlot
    {
        public MaterialWrapper? Current;
        public MaterialWrapper? Clone;
        public MaterialWrapper? InstalledSource;

        /// <summary>Every read is a new wrapper around the same native material.</summary>
        public MaterialWrapper? Read() => Current is null ? null : new MaterialWrapper(Current.Native);

        /// <summary>True only when the read is our own clone, decided by native id, never by wrapper identity.</summary>
        public bool Owns(MaterialWrapper? read) =>
            Clone is not null && NativeMaterialIdentity.SameNative(read, new MaterialWrapper(Clone.Native), WrapperId);
    }

    /// <summary>The verified LED panel family, by name (the same names <c>LightRuntime</c> accepts).</summary>
    private static bool IsPanelMaterial(MaterialWrapper read) =>
        read.Native is not null &&
        (string.Equals(read.Native.Name, "ledlight_light_on", StringComparison.Ordinal) ||
         string.Equals(read.Native.Name, "ledlight_light_off", StringComparison.Ordinal));

    // ---- Supplementary wiring contracts (source text only; no runtime execution) -------------

    private static void SlotWriteWiring()
    {
        string save = Tail(ReadSource("LightPersistence.cs"), "public bool Save(JsonStatePersistence persistence)");
        // The captured slot must always be compared against the active slot, including slot -1.
        Equal(false, save.Contains("active >= 0", StringComparison.Ordinal));
        Equal(true, save.Contains("if (!PersistenceSlotPolicy.CanWrite(_slot, active))", StringComparison.Ordinal));
    }
    private static void PersistRetryWiring()
    {
        string persist = Tail(ReadSource("LightRuntime.cs"), "private static void Persist()");
        // The saved revision advances only after a successful write, so a failed I/O retries.
        Equal(true, persist.Contains("_savedRevision = revision;", StringComparison.Ordinal));
        Equal(true, persist.Contains("LastSaveFailed = true;", StringComparison.Ordinal));
        Equal(false, persist.Contains("_savedRevision = _store.Revision;", StringComparison.Ordinal));
    }
    private static void DeinitWiring()
    {
        string source = ReadSource("Mod.cs");
        Equal(true, source.Contains("public override void OnDeinitializeMelon()", StringComparison.Ordinal));
        string deinit = Tail(source, "public override void OnDeinitializeMelon()");
        int unsubscribe = deinit.IndexOf("Unsubscribe();", StringComparison.Ordinal);
        int teardown = deinit.IndexOf("LightRuntime.Teardown", StringComparison.Ordinal);
        Equal(true, unsubscribe >= 0 && teardown > unsubscribe);
        Equal(true, source.Contains("if (_deinitialized) return;", StringComparison.Ordinal));
    }
    private static void OwnershipWiring()
    {
        string source = ReadSource("LightRuntime.cs");
        Equal(true, source.Contains("PropertyOwnershipPolicy.IsOwnedProperty", StringComparison.Ordinal));
        Equal(true, source.Contains("ParentProperty", StringComparison.Ordinal));
        Equal(true, source.Contains(".Initialized", StringComparison.Ordinal));
        Equal(true, source.Contains("MarkTemporarilyUnloaded", StringComparison.Ordinal));
        Equal(true, source.Contains("RebindTemporarilyUnloaded", StringComparison.Ordinal));
    }
    private static void ScanWiring()
    {
        string source = ReadSource("LightRuntime.cs");
        Equal(false, source.Contains("GetComponentsInChildren", StringComparison.Ordinal));
        Equal(true, source.Contains("GetChild(", StringComparison.Ordinal));
        Equal(true, source.Contains("SceneScanBudget", StringComparison.Ordinal));
        Equal(true, source.Contains("_scanTruncated", StringComparison.Ordinal));
        Equal(true, source.Contains("LightPointerDedupe.SelectUnowned", StringComparison.Ordinal));
    }
    private static void AppCleanupWiring()
    {
        string source = ReadSource("LightControlApp.cs");
        Equal(true, source.Contains("List<(UnityEvent Event, Action Handler)> _wired", StringComparison.Ordinal));
        Equal(false, source.Contains("if (button != null)", StringComparison.Ordinal));
        Equal(true, source.Contains("EventHelper.RemoveListener(handler, unityEvent)", StringComparison.Ordinal));
    }
    private static void AppFailureWiring()
    {
        string source = ReadSource("LightControlApp.cs");
        Equal(true, source.Contains("private void Act(Func<string, LightRuntime.LampActionResult> action)", StringComparison.Ordinal));
        Equal(true, source.Contains("That change was not applied", StringComparison.Ordinal));
    }
    private static void PersistenceStatusWiring()
    {
        string source = ReadSource("LightRuntime.cs");
        Equal(true, source.Contains("PersistenceGate.Blocked", StringComparison.Ordinal));
        Equal(true, source.Contains("changes are not saved", StringComparison.Ordinal));
    }

    private static string ReadSource(string name)
    {
        for (DirectoryInfo? dir = new DirectoryInfo(AppContext.BaseDirectory); dir != null; dir = dir.Parent)
        {
            string path = Path.Combine(dir.FullName, "Source", "Mods", "LightControl", "src", name);
            if (File.Exists(path)) return File.ReadAllText(path);
        }
        throw new FileNotFoundException("LightControl source root not found", name);
    }
    private static string Tail(string source, string start)
    {
        int at = source.IndexOf(start, StringComparison.Ordinal);
        if (at < 0) throw new InvalidOperationException($"Missing section: {start}");
        return source.Substring(at);
    }
    private static void EqualSeq<T>(IEnumerable<T> expected, IEnumerable<T> actual)
    {
        if (!expected.SequenceEqual(actual))
            throw new InvalidOperationException($"Expected [{string.Join(",", expected)}]; actual [{string.Join(",", actual)}].");
    }

    private static void Equal<T>(T expected, T actual)
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual)) throw new InvalidOperationException($"Expected {expected}; actual {actual}.");
    }
    private static void NotEqual<T>(T a, T b)
    {
        if (EqualityComparer<T>.Default.Equals(a, b)) throw new InvalidOperationException($"Expected distinct values, got {a}.");
    }
    private static void Close(float expected, float actual, float tolerance = 1e-4f)
    {
        if (float.IsNaN(actual) || System.Math.Abs(expected - actual) > tolerance)
            throw new InvalidOperationException($"Expected {expected}; actual {actual}.");
    }
    private static void Throws<T>(Action action) where T : Exception
    {
        try { action(); }
        catch (T) { return; }
        throw new InvalidOperationException($"Expected {typeof(T).Name}.");
    }
}
