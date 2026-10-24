using LightControl.Core;

namespace LightControl.Core.Tests;

internal static class AuditRuntimeChecks
{
    public static IEnumerable<(string Name, Action Run)> Cases => new (string, Action)[]
    {
        ("LC02 Rebind commits new property and group with runtime identity", RebindProperty),
        ("LC02 Rejected rebind preserves old registration and state", RejectedRebind),
        ("LC03 Throwing rebind preparation leaves model unavailable and retryable", FailedRebindPreparation),
        ("LC06 Lost preview target rejects without changing another target", LostTarget),
        ("LC06 Changed catalog rejects even with identical state revision", ChangedCatalog),
        ("LC06 Same-sized different target set rejects atomically", ChangedKeys),
        ("LC06 Exact confirmed targets apply and stale rejection is retryable", ConfirmedTargetsApply),
        ("LC06 Duplicate confirmed keys reject without mutation", DuplicateConfirmedKeys),
        ("LC11 Static visual instructions are reused", StaticVisualCache),
        ("LC11 Rainbow ticks are flagged and skip the apply breadcrumb", RainbowTickWiring),
        ("LC01 Pending restore pressure blocks new claims, not existing owners", RestoreBackpressure),
        ("LC01 Source wiring never discards an owned source at the soft limit", PendingRetentionWiring),
        ("Native destruction proof is not proxy collection or a missing pointer", NativeDestructionWiring),
        ("LC05 Material conflict and retained cleanup remain visible with empty catalog", NativeWarningsVisible),
        ("LC04 Own clone suffix after Instance can never be a native panel", OwnCloneNameRejected),
        ("LC04 Discovery checks retained clone identity before promotion", OwnCloneDiscoveryWiring),
        ("LC11 Unowned Original100 needs no native apply, pending output still does", OriginalNeedsNoApply),
        ("UI Empty catalog text distinguishes a finished scan", EmptyScanText),
        ("UI A single changed lamp has singular wording", SingularAction),
        ("UI Empty-state wiring updates outside catalog-only layout changes", EmptyStateWiring),
        ("UI Selection changes dismiss frozen world-copy confirmation", CopyDismissalWiring),
        ("LC12 An unconsulted backup is not proof of missing data", UnconsultedBackup),
        ("LC03 Source wiring commits tent model before releasing and publishing", TentRebindWiring),
        ("LC07 Source wiring reconciles inactive known lamps too", InactiveReconcileWiring),
        ("LC06 Source wiring reconciles before final preview and pins mutation targets", PreviewWiring),
        ("Teardown source wiring contains final persistence exceptions", TeardownWiring)
    };

    private static LampIdentity Id(string key) => LampIdentity.Persistent(new(PersistentIdentityKind.VerifiedNativeInstance, "audit", 1, key));
    private static LightStateStore Store()
    {
        var store = new LightStateStore();
        store.BeginSession(Guid.NewGuid(), "slot-1", SessionMode.Host);
        store.Register(new(Id("a"), "runtime-a", "A", "old"), 0);
        store.Register(new(Id("b"), "runtime-b", "A", "old"), 0);
        return store;
    }
    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
    private static ClientCommand Request(LightStateStore store) => new(LightLimits.ProtocolVersion, store.SessionId,
        Guid.NewGuid(), store.Revision, ControlCommand.Set(LampSelection.Property("A"), new(ColorMode.Red, IntensityLevel.Percent150)));

    private static void RebindProperty()
    {
        var store = Store();
        store.ExecuteHost(Request(store), "host", _ => true, 1);
        store.MarkTemporarilyUnloaded(Id("a"));
        store.RebindTemporarilyUnloaded(Id("a"), "replacement-a", "B", "new");
        var registration = store.GetRegistrations().Single(x => x.Identity.Key == Id("a").Key);
        Require(registration.PropertyId == "B" && registration.GroupId == "new", "Rebind retained old property/group.");
        Require(store.GetBaseState(Id("a")) == new LampBaseState(ColorMode.Red, IntensityLevel.Percent150), "Rebind lost durable settings.");
        Require(!store.AvailableTargetKeys(LampSelection.Property("A")).Contains(Id("a").Key), "Old property still targets moved lamp.");
        Require(store.AvailableTargetKeys(LampSelection.Property("B")).SequenceEqual(new[] { Id("a").Key }), "New property does not target exactly moved lamp.");
    }
    private static void RejectedRebind()
    {
        var store = Store();
        store.MarkTemporarilyUnloaded(Id("a"));
        long catalog = store.CatalogRevision;
        try { store.RebindTemporarilyUnloaded(Id("a"), "runtime-b", "B", "new"); }
        catch (InvalidOperationException) { }
        var old = store.GetRegistrations().Single(x => x.Identity.Key == Id("a").Key);
        Require(old.PropertyId == "A" && old.GroupId == "old" && old.Presence == LampPresence.TemporarilyUnloaded && old.RuntimeInstanceId == "runtime-a", "Rejected rebind partially changed registration.");
        Require(store.CatalogRevision == catalog, "Rejected rebind published a catalog change.");
    }
    private static void FailedRebindPreparation()
    {
        var store = Store();
        store.MarkTemporarilyUnloaded(Id("a"));
        long catalog = store.CatalogRevision;
        bool failed = false;
        try { store.RebindTemporarilyUnloaded(Id("a"), "replacement-a", "B", null, () => throw new IOException("simulated cleanup failure")); }
        catch (IOException) { failed = true; }
        Require(failed, "Rebind preparation was skipped.");
        var old = store.GetRegistrations().Single(x => x.Identity.Key == Id("a").Key);
        Require(old.Presence == LampPresence.TemporarilyUnloaded && old.PropertyId == "A" && old.RuntimeInstanceId == "runtime-a", "Failed preparation published a half transition.");
        Require(store.CatalogRevision == catalog, "Failed preparation changed the catalog.");
        store.RebindTemporarilyUnloaded(Id("a"), "replacement-a", "B", null);
        Require(store.AvailableTargetKeys(LampSelection.Property("B")).Contains(Id("a").Key), "Rebind is no longer retryable.");
    }
    private static void LostTarget()
    {
        var store = Store();
        var keys = store.AvailableTargetKeys(LampSelection.Property("A"));
        store.MarkTemporarilyUnloaded(Id("b"));
        var result = store.ExecuteHost(Request(store), "host", _ => true, 1, keys);
        Require(result.Status == CommandStatus.StaleRevision, "Stale preview partially applied after target loss.");
        Require(store.GetBaseState(Id("a")) == LampBaseState.Default, "Remaining lamp was changed.");
    }
    private static void ChangedCatalog()
    {
        var store = Store();
        var keys = store.AvailableTargetKeys(LampSelection.Property("A"));
        long catalog = store.CatalogRevision;
        store.Move(Id("b"), "A", "changed-group");
        var result = store.ExecuteHost(Request(store), "host", _ => true, 1, keys, catalog);
        Require(result.Status == CommandStatus.StaleRevision, "Catalog changes were not checked at mutation boundary.");
        Require(store.GetBaseState(Id("a")) == LampBaseState.Default, "Catalog rejection changed state.");
    }
    private static void ChangedKeys()
    {
        var store = Store();
        var keys = store.AvailableTargetKeys(LampSelection.Property("A"));
        store.Move(Id("b"), "B");
        store.Register(new(Id("c"), "runtime-c", "A"), 0);
        var result = store.ExecuteHost(Request(store), "host", _ => true, 1, keys);
        Require(result.Status == CommandStatus.StaleRevision, "Equal target count incorrectly accepted different keys.");
        Require(store.GetBaseState(Id("a")) == LampBaseState.Default && store.GetBaseState(Id("c")) == LampBaseState.Default, "Changed-key rejection was not atomic.");
    }
    private static void StaticVisualCache()
    {
        var store = Store();
        store.ExecuteHost(Request(store), "host", _ => true, 1);
        var first = store.ResolveVisual(Id("a"), 1);
        Require(ReferenceEquals(first, store.ResolveVisual(Id("a"), 2)), "Unchanged fixed-color state allocates a new instruction.");
        var original = store.ResolveVisual(Id("b"), 2, false);
        Require(ReferenceEquals(original, store.ResolveVisual(Id("b"), 3, false)), "Disabled instruction allocates each frame.");
    }

    private static void RainbowTickWiring()
    {
        var rainbowState = new LampBaseState(ColorMode.Rainbow, IntensityLevel.Percent100);
        var rainbow = VisualResolver.Resolve("lamp", null, rainbowState, EffectState.Empty, 1);
        Require(rainbow.IsRainbow, "A Rainbow instruction was not flagged as a periodic tick.");
        var fixedColor = VisualResolver.Resolve("lamp", null,
            new LampBaseState(ColorMode.Blue, IntensityLevel.Percent100), EffectState.Empty, 1);
        Require(!fixedColor.IsRainbow, "A fixed colour was flagged as a Rainbow tick.");
        var effects = new EffectState(null, new[]
        {
            new KeyValuePair<string, DiscoEffect>("property", new DiscoEffect(0, 1, 7))
        });
        var temporary = VisualResolver.Resolve("lamp", "property", rainbowState, effects, 1);
        Require(temporary.HasTemporaryEffect && !temporary.IsRainbow,
            "A temporary effect over Rainbow must retain its ordinary change-logging path.");

        string body = Body("private static void MaybeLogApplied(");
        int rainbowGuard = body.IndexOf("if (instruction.IsRainbow) return;", StringComparison.Ordinal);
        int unchangedGuard = body.IndexOf("if (lamp.LastLoggedSlot == slot && Equals(lamp.LastLoggedInstruction, instruction)) return;", StringComparison.Ordinal);
        int rateLimit = body.IndexOf("double now = Now;", StringComparison.Ordinal);
        int logCall = body.IndexOf("LogApplied(lamp, slot, instruction);", StringComparison.Ordinal);
        Require(rainbowGuard >= 0 && unchangedGuard > rainbowGuard && rateLimit > unchangedGuard && logCall > rateLimit,
            "Rainbow ticks must return before equality, rate-limit and native-readback logging work.");
        string appliedBody = Body("private static void LogApplied(");
        int nativeReadback = appliedBody.IndexOf("clone!.GetColor(EmissionProperty)", StringComparison.Ordinal);
        int formatting = appliedBody.IndexOf("string message =", StringComparison.Ordinal);
        int writeLog = appliedBody.IndexOf("Log.Info(message);", StringComparison.Ordinal);
        Require(nativeReadback >= 0 && formatting > nativeReadback && writeLog > formatting,
            "The apply breadcrumb no longer has the expected readback, formatting and write sequence.");
        string applyBody = Body("private static void ApplyLamp(");
        int surfaceWrite = applyBody.IndexOf("ApplySurface(lamp, surface", StringComparison.Ordinal);
        int lightWrite = applyBody.IndexOf("ApplyLightSource(source", StringComparison.Ordinal);
        int logging = applyBody.IndexOf("MaybeLogApplied(lamp, slot, instruction);", StringComparison.Ordinal);
        Require(surfaceWrite >= 0 && lightWrite > surfaceWrite && logging > lightWrite,
            "Rainbow log suppression must remain downstream of both visual writes.");
    }

    private static void ConfirmedTargetsApply()
    {
        var store = Store();
        var request = Request(store);
        var keys = store.AvailableTargetKeys(LampSelection.Property("A"));
        var rejected = store.ExecuteHost(request, "host", _ => true, 1, new[] { Id("a").Key });
        Require(rejected.Status == CommandStatus.StaleRevision, "Invalid target pin was accepted.");
        var applied = store.ExecuteHost(request, "host", _ => true, 1, keys.Reverse().ToArray(), store.CatalogRevision);
        Require(applied.Status == CommandStatus.Applied, "Exact unordered keys did not apply, or rejection consumed command ID.");
        Require(store.GetBaseState(Id("b")).Color == ColorMode.Red, "Confirmed target was omitted.");
    }
    private static void DuplicateConfirmedKeys()
    {
        var store = Store();
        var keys = store.AvailableTargetKeys(LampSelection.Property("A"));
        var result = store.ExecuteHost(Request(store), "host", _ => true, 1, keys.Concat(new[] { keys[0] }).ToArray());
        Require(result.Status == CommandStatus.StaleRevision && store.GetBaseState(Id("a")) == LampBaseState.Default, "Duplicate key bypassed exact-set proof.");
    }

    private static void RestoreBackpressure()
    {
        Require(!NativeLightAdmissionPolicy.MayApply(false, 256, 256), "Backlog admitted new native ownership.");
        Require(NativeLightAdmissionPolicy.MayApply(true, 1000, 256), "Backlog blocked handling of an existing owned output.");
        Require(NativeLightAdmissionPolicy.MayApply(false, 0, 256), "Empty backlog blocked normal apply.");
    }
    private static void PendingRetentionWiring()
    {
        string retain = Body("private static void RetainLightRestore(");
        Require(!retain.Contains("PendingLightRestores.Count >=", StringComparison.Ordinal), "An owned source can be dropped at backlog limit.");
        Require(Body("private static HashSet<long> ActiveLightPointers(").Contains("PendingLightRestores", StringComparison.Ordinal), "New binding can capture a source whose old output is still pending restore.");
    }

    private static void NativeDestructionWiring()
    {
        string proof = Body("private static bool IsProvablyDestroyed(");
        Require(!proof.Contains("native.WasCollected ||", StringComparison.Ordinal) &&
            !proof.Contains("native.Pointer == IntPtr.Zero ||", StringComparison.Ordinal), "Proxy unavailability is mistaken for native destruction.");
    }

    private static void NativeWarningsVisible()
    {
        string status = UiSelectionPolicy.StatusLine(true, "Host session ready.", "Changes are not saved.", false, false, 0, 0, 2, 3);
        Require(status.Contains("2 material", StringComparison.OrdinalIgnoreCase), "Material conflict is not visible.");
        Require(status.Contains("3 restore", StringComparison.OrdinalIgnoreCase), "Retained native cleanup is not visible.");
        Require(status.Contains("not saved", StringComparison.OrdinalIgnoreCase), "Native warnings hid persistence state.");
    }

    private static void OwnCloneNameRejected()
    {
        Require(!PanelFamilyPolicy.IsVerifiedPanelName("ledlight_light_on (Instance) (LightControl)"), "A tinted own clone is accepted as the native baseline.");
        Require(PanelFamilyPolicy.IsVerifiedPanelName("ledlight_light_on (Instance)"), "A legitimate native instance was rejected.");
    }
    private static void OwnCloneDiscoveryWiring()
    {
        string source = RuntimeSource();
        int start = source.IndexOf("private static List<OwnedSurface> CollectSurfaces(ToggleableLight", StringComparison.Ordinal);
        int end = source.IndexOf("private static List<OwnedLightSource>", start, StringComparison.Ordinal);
        Require(start >= 0 && end > start, "Cannot locate surface discovery.");
        string collect = source.Substring(start, end - start);
        Require(collect.Contains("IsOwnClone(material)", StringComparison.Ordinal), "Retained clone identity guard is not wired into promotion.");
    }

    private static void OriginalNeedsNoApply()
    {
        var binding = new NativeLightBinding();
        var original = new VisualInstruction(null, 1, false);
        Require(!binding.NeedsApply(original), "Untouched default lamp still needs a native write.");
        binding.Apply(10, 1, 1, 1, 1, new VisualInstruction(null, 2, false));
        Require(binding.NeedsApply(original), "Pending owned output was incorrectly skipped.");
    }
    private static void EmptyScanText()
    {
        Require(UiSelectionPolicy.EmptyCatalogText(false) == "No supported grow lamps found.", "Completed empty scan still says it is searching.");
        Require(UiSelectionPolicy.EmptyCatalogText(true) == "Looking for grow lamps...", "Active scan was reported as finished.");
    }
    private static void SingularAction() =>
        Require(UiSelectionPolicy.ActionMessage(CommandStatus.Applied, 1) == "Applied to 1 lamp.", "Single lamp uses plural wording.");

    private static void EmptyStateWiring()
    {
        string refresh = Body("private void Refresh()", "LightControlApp.cs");
        int text = refresh.IndexOf("UiSelectionPolicy.EmptyCatalogText", StringComparison.Ordinal);
        Require(text >= 0 && text < refresh.IndexOf("if (structure)", StringComparison.Ordinal), "Empty-state text is tied to a catalog-only rebuild.");
    }
    private static void CopyDismissalWiring() =>
        Require(Body("private void OnRow(", "LightControlApp.cs").Contains("CancelWorldCopy()", StringComparison.Ordinal), "Changing selected subject leaves old confirmation attached.");
    private static void UnconsultedBackup()
    {
        var plan = PersistenceSlotPolicy.PlanLoad(PersistenceReadOutcome.Absent("proven"), PersistenceReadOutcome.NotConsulted,
            json => JsonStatePersistence.Inspect(json, "slot-0"));
        Require(plan.Action == PersistenceLoadAction.ReadFailure, "Unconsulted backup opened the missing-data gate.");
    }

    private static string RuntimeSource(string name = "LightRuntime.cs")
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            string path = Path.Combine(directory.FullName, "Source", "Mods", "LightControl", "src", name);
            if (File.Exists(path)) return File.ReadAllText(path);
            directory = directory.Parent;
        }
        throw new InvalidOperationException("Cannot locate runtime source for supplementary wiring checks.");
    }
    private static string Body(string signature, string name = "LightRuntime.cs")
    {
        string source = RuntimeSource(name);
        int start = source.IndexOf(signature, StringComparison.Ordinal);
        Require(start >= 0, "Missing runtime method: " + signature);
        int open = source.IndexOf('{', start), depth = 1, end = open + 1;
        for (; end < source.Length && depth > 0; end++)
        {
            if (source[end] == '{') depth++;
            if (source[end] == '}') depth--;
        }
        return source.Substring(open, end - open);
    }
    private static void TentRebindWiring()
    {
        string body = Body("private static bool RebindTent(");
        int commit = body.IndexOf("CommitRebind(", StringComparison.Ordinal);
        Require(commit >= 0, "Tent does not use the controlled common rebind transition.");
        string common = Body("private static bool CommitRebind(");
        int model = common.IndexOf("_store.RebindTemporarilyUnloaded", StringComparison.Ordinal);
        int clear = common.IndexOf("lamp.Surfaces.Clear()", StringComparison.Ordinal);
        int publish = common.IndexOf("lamp.Available = true", StringComparison.Ordinal);
        Require(model >= 0 && clear > model && publish > clear, "Rebind publishes before model commit.");
        string retire = Body("private static void RetireRebindOutputs(");
        Require(retire.IndexOf("ReleaseOwned(surface)", StringComparison.Ordinal) < retire.IndexOf("lamp.Surfaces.Clear()", StringComparison.Ordinal), "Retirement drops resources before handing off responsibility.");
        Require(body.IndexOf("RetireRebindOutputs(lamp)", StringComparison.Ordinal) < body.IndexOf("CollectSurfaces(", StringComparison.Ordinal), "Tent discovers native surfaces before retiring old owned output.");
    }
    private static void InactiveReconcileWiring()
    {
        string body = Body("private static void ReconcileLamps()");
        Require(body.Contains("Lamps.Values", StringComparison.Ordinal), "Inactive records are excluded from positive deletion checks.");
        Require(body.IndexOf("IsPermanentlyGone", StringComparison.Ordinal) < body.IndexOf("lamp.Available", StringComparison.Ordinal), "Inactive entries skip deletion evidence.");
    }
    private static void PreviewWiring()
    {
        string body = Body("public static LampActionResult ApplyPreview(");
        Require(body.IndexOf("RecheckOwnership();", StringComparison.Ordinal) >= 0 &&
            body.IndexOf("RecheckOwnership();", StringComparison.Ordinal) < body.IndexOf("TryBuildPreview(", StringComparison.Ordinal), "Preview is checked before current ownership.");
        Require(body.Contains("current.TargetKeys", StringComparison.Ordinal) && body.Contains("CatalogRevision", StringComparison.Ordinal), "Final mutation does not carry catalog and exact target proof.");
    }
    private static void TeardownWiring()
    {
        string body = Body("public static void Teardown()");
        Require(body.Contains("try { Persist(); }", StringComparison.Ordinal) && body.Contains("catch (Exception", StringComparison.Ordinal), "Final save exception can skip native cleanup.");
    }
}
