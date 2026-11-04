using TownHallInterior;
using Xunit;

public sealed class BusinessProbeEvidenceTests
{
    private static ProbeSnapshot Snapshot(long generation = 1, bool complete = true,
        string[]? errors = null, params ProbeEntity[] entities) =>
        new("load-complete", 100, generation, complete, errors ?? Array.Empty<string>(), entities);

    [Fact]
    public void ReadableFirstLoadAbsenceIsDifferentFromUnreadableRegistry()
    {
        var absent = new ProbeTrace();
        absent.Append(Snapshot());
        var unreadable = new ProbeTrace();
        unreadable.Append(Snapshot(complete: false, errors: new[] { "Business.Businesses: read failed" }));
        Assert.Equal("observed", absent.Analyze().Status);
        Assert.Equal("incomplete", unreadable.Analyze().Status);
        Assert.Contains(unreadable.Analyze().Findings, x => x.Contains("read failed"));
    }

    [Fact]
    public void RepeatedPointerRolesAreAliasesButDifferentPointersCollide()
    {
        var trace = new ProbeTrace();
        var entity = new ProbeEntity("business", "0xA", Code: "townhall", Guid: "6a145616-644d-4245-b278-e6a00f6287cc");
        trace.Append(Snapshot(entities: new[] { entity, entity, entity with { Kind = "property" } }));
        Assert.Equal("observed", trace.Analyze().Status);
        trace.Append(Snapshot(entities: new[] { entity, entity with { Identity = "0xB", Code = " TOWNHALL " } }));
        Assert.Equal("incomplete", trace.Analyze().Status);
        Assert.Contains(trace.Analyze().Findings, x => x.Contains("duplicate-business-code"));
        Assert.Contains(trace.Analyze().Findings, x => x.Contains("duplicate-guid"));
    }

    [Fact]
    public void GuidFormattingCannotConcealCollisionButEmptyGuidIsNotAnIdentity()
    {
        var trace = new ProbeTrace();
        trace.Append(Snapshot(entities: new[] {
            new ProbeEntity("grid", "0x1", Guid: "6a145616-644d-4245-b278-e6a00f6287cc"),
            new ProbeEntity("surface", "0x2", Guid: "{6A145616-644D-4245-B278-E6A00F6287CC}") }));
        Assert.Contains(trace.Analyze().Findings, x => x.Contains("duplicate-guid"));
        var empty = new ProbeTrace();
        empty.Append(Snapshot(entities: new[] { new ProbeEntity("grid", "0x1", Guid: Guid.Empty.ToString()),
            new ProbeEntity("grid", "0x2", Guid: Guid.Empty.ToString()) }));
        Assert.Equal("incomplete", empty.Analyze().Status);
        Assert.DoesNotContain(empty.Analyze().Findings, x => x.Contains("duplicate-guid"));
    }

    [Fact]
    public void LaterPointerReplacementsAndOtherLoadsDoNotImplyCoexistingDuplicates()
    {
        var trace = new ProbeTrace();
        trace.Append(Snapshot(1, false, new[] { "early registry unreadable" }));
        trace.Append(Snapshot(2, entities: new[] { new ProbeEntity("business", "0x1", Code: "townhall") }));
        trace.Append(Snapshot(2, entities: new[] { new ProbeEntity("business", "0x2", Code: "townhall") }));
        Assert.Equal("incomplete", trace.Analyze(1).Status);
        Assert.Equal("observed", trace.Analyze(2).Status);
        Assert.Equal("incomplete", trace.Analyze().Status);
        Assert.Equal("incomplete", trace.Analyze(3).Status);
    }

    [Theory]
    [InlineData(1, 10, 10)]
    [InlineData(10, 1, 10)]
    [InlineData(10, 10, 1)]
    public void EveryCaptureCapRetainsAnExplicitIncompleteResult(int snapshots, int entities, int errors)
    {
        var trace = new ProbeTrace(snapshots, entities, errors);
        var capture = Snapshot(errors: errors == 1 ? new[] { "first", "second" } : Array.Empty<string>(), entities: new[] {
            new ProbeEntity("business", "0x1", Code: "first"), new ProbeEntity("business", "0x2", Code: "second") });
        Assert.True(trace.Append(capture));
        bool secondStored = trace.Append(Snapshot());
        Assert.Equal(snapshots != 1, secondStored);
        Assert.True(trace.Truncated);
        Assert.True(trace.Snapshots.Count <= snapshots);
        Assert.True(trace.Snapshots[0].Entities.Count <= entities);
        Assert.True(trace.Snapshots[0].Errors.Count <= errors);
        Assert.Contains("trace-truncated", trace.Analyze().Findings);
        Assert.Equal("incomplete", trace.Analyze().Status);
    }

    [Fact]
    public void CaptureFreezesListsAndMissingListsCannotBecomeReadableAbsence()
    {
        var entities = new List<ProbeEntity> { new("business", "0x1") };
        var errors = new List<string> { "unreadable code" };
        var trace = new ProbeTrace();
        trace.Append(new("capture", 1, 1, true, errors, entities));
        entities.Clear(); errors.Clear();
        Assert.Single(trace.Snapshots[0].Entities);
        Assert.False(trace.Snapshots[0].Complete);
        Assert.Equal("incomplete", trace.Analyze().Status);
        var missing = new ProbeTrace();
        missing.Append(new("capture", 1, 1, true, null!, null!));
        Assert.Equal("incomplete", missing.Analyze().Status);
    }

    [Fact]
    public void HookOnlyTraceReportsObservationWithoutBindingReadiness()
    {
        var trace = new ProbeTrace();
        Assert.Equal("incomplete", trace.Analyze().Status);
        trace.Append(Snapshot(entities: new[] { new ProbeEntity("loader-enter", "call-1", Code: "Load") }));
        trace.Append(Snapshot(entities: new[] { new ProbeEntity("loader-exit", "call-1", Code: "Load") }));
        Assert.Equal("observed", trace.Analyze().Status);
        Assert.Empty(trace.Analyze().Findings);
    }

    [Theory]
    [InlineData("business", "", "")]
    [InlineData("business", " ", "")]
    [InlineData("grid", "", "")]
    [InlineData("grid", "", "00000000-0000-0000-0000-000000000000")]
    [InlineData("grid", "", "invalid")]
    [InlineData("station", "", "")]
    [InlineData("station", "", "00000000-0000-0000-0000-000000000000")]
    [InlineData("station", "", "invalid")]
    public void UninitializedRequiredIdentifiersAreIncomplete(string kind, string code, string guid)
    {
        var trace = new ProbeTrace();
        trace.Append(Snapshot(entities: new[] { new ProbeEntity(kind, "0x1", Code: code, Guid: guid) }));
        Assert.Equal("incomplete", trace.Analyze().Status);
        Assert.Contains(trace.Analyze().Findings, x => x.Contains("uninitialized"));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void MainAndPreLoadShareOneMarkerCycleInEitherOrder(bool preLoadFirst)
    {
        var cycle = new ProbeLoadGeneration();
        cycle.Leaving();
        Assert.Equal(0, cycle.Generation);
        long first = preLoadFirst ? cycle.PreLoad() : cycle.MainSeen();
        long second = preLoadFirst ? cycle.MainSeen() : cycle.PreLoad();
        Assert.Equal(1, first);
        Assert.Equal(first, second);
        Assert.Equal(first, cycle.MainSeen());
        Assert.True(cycle.MainPresent && cycle.PreLoadObserved);
        Assert.True(cycle.LoadComplete());
        Assert.True(cycle.LoadCompleteObserved);
        Assert.Null(cycle.Anomaly);
    }

    [Fact]
    public void ActualMainUnloadSeparatesReloadButPreparationAndDuplicateMainDoNot()
    {
        var cycle = new ProbeLoadGeneration();
        cycle.MainSeen(); cycle.PreLoad(); cycle.LoadComplete();
        cycle.Leaving();
        Assert.True(cycle.TransitionPending);
        Assert.Equal(1, cycle.MainSeen());
        cycle.MainUnloaded(); cycle.MainUnloaded();
        Assert.False(cycle.MainPresent);
        Assert.Equal(2, cycle.PreLoad());
        Assert.Equal(2, cycle.MainSeen());
        Assert.False(cycle.LoadCompleteObserved);
        Assert.Null(cycle.Anomaly);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void UnloadOverlappingPreLoadCannotBeMistakenForVerifiedWorldIdentity(bool previousMainSeen)
    {
        var cycle = new ProbeLoadGeneration();
        cycle.PreLoad();
        if (previousMainSeen) { cycle.MainSeen(); cycle.LoadComplete(); cycle.PreLoad(); }
        cycle.MainUnloaded();
        Assert.NotNull(cycle.Anomaly);
        cycle.MainSeen(); cycle.PreLoad();
        var trace = new ProbeTrace();
        trace.Append(Snapshot(cycle.Generation, false, new[] { cycle.Anomaly! }));
        Assert.Equal("incomplete", trace.Analyze().Status);
    }

    [Fact]
    public void IsolatedLoadCompleteNeverInventsALoadGeneration()
    {
        var cycle = new ProbeLoadGeneration();
        Assert.False(cycle.LoadComplete());
        Assert.Equal(0, cycle.Generation);
        Assert.NotNull(cycle.Anomaly);
    }

    private static ProbeLoaderEvent Loader(long sequence, string phase, long callId, string method = "BusinessLoader.Load", long generation = 1) =>
        new(sequence, phase, 100, generation, callId, method);

    private static IEnumerable<ProbeLoaderEvent> NinetyLoaderEvents()
    {
        for (int call = 1; call <= 45; call++)
        {
            string method = call <= 24 ? "LoadManager.QueueLoadRequest" : call <= 34 ? "BusinessLoader.Load" : "PropertyLoader.Load";
            yield return Loader(2 * call - 1, "enter", call, method);
            yield return Loader(2 * call, "exit", call, method);
        }
    }

    [Fact]
    public void NinetyLoaderEventsRemainCompleteOutsideTheOldSixtyFourSnapshotBudget()
    {
        var trace = new ProbeLoaderTrace();
        var oldBudget = new ProbeLoaderTrace(64);
        foreach (var entry in NinetyLoaderEvents()) { Assert.True(trace.Record(entry)); oldBudget.Record(entry); }
        Assert.Equal(90, trace.Events.Count);
        Assert.Equal(48, trace.Events.Count(x => x.Method == "LoadManager.QueueLoadRequest"));
        Assert.Equal(42, trace.Events.Count(x => x.Method != "LoadManager.QueueLoadRequest"));
        Assert.Equal(0, trace.DroppedEvents);
        Assert.Equal("observed", trace.Analyze().Status);
        Assert.Equal(26, oldBudget.DroppedEvents);
        Assert.Equal("incomplete", oldBudget.Analyze().Status);
    }

    [Fact]
    public void NestedLoaderPairsMatchByCallIdentityAndNeedNoUnusedMethodCoverage()
    {
        var trace = new ProbeLoaderTrace();
        trace.Record(Loader(1, "enter", 1, "PropertyLoader.Load"));
        trace.Record(Loader(2, "enter", 2));
        trace.Record(Loader(3, "exit", 2));
        trace.Record(Loader(4, "exit", 1, "PropertyLoader.Load"));
        Assert.Equal("observed", trace.Analyze().Status);
        Assert.Empty(trace.Analyze().Findings);
    }

    [Fact]
    public void ExceptionWithoutExitRetainsTheOpenCallAsIncompleteEvidence()
    {
        var trace = new ProbeLoaderTrace();
        trace.Record(Loader(1, "enter", 1));
        Assert.Equal("incomplete", trace.Analyze().Status);
        Assert.Contains(trace.Analyze().Findings, x => x.Contains("open-call:1"));
    }

    [Theory]
    [InlineData("PropertyLoader.Load", 1)]
    [InlineData("BusinessLoader.Load", 2)]
    public void WrongExitCannotCloseAnEnteredCall(string exitMethod, long exitGeneration)
    {
        var trace = new ProbeLoaderTrace();
        trace.Record(Loader(1, "enter", 1));
        trace.Record(Loader(2, "exit", 1, exitMethod, exitGeneration));
        Assert.Equal("incomplete", trace.Analyze().Status);
        Assert.Contains(trace.Analyze().Findings, x => x.Contains("mismatched-exit"));
        Assert.Contains(trace.Analyze().Findings, x => x.Contains("open-call:1"));
    }

    [Fact]
    public void DuplicateUnmatchedAndReusedCallIdsCannotBeReportedAsComplete()
    {
        var trace = new ProbeLoaderTrace();
        trace.Record(Loader(1, "exit", 1));
        trace.Record(Loader(2, "enter", 1));
        trace.Record(Loader(3, "enter", 1));
        trace.Record(Loader(4, "exit", 1));
        trace.Record(Loader(5, "exit", 1));
        trace.Record(Loader(6, "enter", 1));
        Assert.Equal("incomplete", trace.Analyze().Status);
        foreach (string issue in new[] { "unmatched-exit", "duplicate-enter", "duplicate-exit" })
            Assert.Contains(trace.Analyze().Findings, x => x.Contains(issue));
    }

    [Theory]
    [InlineData(0, "enter", 1, "BusinessLoader.Load")]
    [InlineData(1, "enter", 0, "BusinessLoader.Load")]
    [InlineData(1, "other", 1, "BusinessLoader.Load")]
    [InlineData(1, "enter", 1, "")]
    public void InvalidLoaderEventMetadataIsIncomplete(long sequence, string phase, long callId, string method)
    {
        var trace = new ProbeLoaderTrace();
        trace.Record(Loader(sequence, phase, callId, method));
        Assert.Equal("incomplete", trace.Analyze().Status);
    }

    [Fact]
    public void EventLimitDoesNotPoisonANewLoadAndExposedEventsStayFrozen()
    {
        var first = new ProbeLoaderTrace(1);
        Assert.True(first.Record(Loader(1, "enter", 1)));
        var frozen = first.Events;
        Assert.False(first.Record(Loader(2, "exit", 1)));
        Assert.Equal(1, first.DroppedEvents);
        Assert.Equal("incomplete", first.Analyze().Status);
        Assert.Single(frozen);
        var next = new ProbeLoaderTrace();
        next.Record(Loader(3, "enter", 1, generation: 2));
        var beforeExit = next.Events;
        next.Record(Loader(4, "exit", 1, generation: 2));
        Assert.Single(beforeExit);
        Assert.Equal(2, next.Events.Count);
        Assert.Equal("observed", next.Analyze().Status);
        Assert.Equal(0, next.DroppedEvents);
        Assert.All(typeof(ProbeLoaderEvent).GetProperties(), property =>
            Assert.Contains(property.PropertyType, new[] { typeof(string), typeof(int), typeof(long) }));
    }

    [Fact]
    public void CommonSequencePreservesSameFrameOrderAcrossSeparateCaptureStreams()
    {
        var inventory = new ProbeTrace();
        Assert.Equal(0, Snapshot().Sequence);
        inventory.Append(Snapshot() with { Sequence = 2 });
        var loaders = new ProbeLoaderTrace();
        loaders.Record(Loader(1, "enter", 1));
        loaders.Record(Loader(3, "exit", 1));
        Assert.Equal(new long[] { 1, 2, 3 }, loaders.Events.Select(x => x.Sequence)
            .Concat(inventory.Snapshots.Select(x => x.Sequence)).OrderBy(x => x));
        Assert.Equal("observed", loaders.Analyze().Status);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(0)]
    public void CompletePairsWithDuplicateOrReversedSequenceRemainIncomplete(long exitSequence)
    {
        var trace = new ProbeLoaderTrace();
        trace.Record(Loader(1, "enter", 1));
        trace.Record(Loader(exitSequence, "exit", 1));
        Assert.Contains(trace.Analyze().Findings, x => x.Contains("invalid-sequence"));
        Assert.Equal("incomplete", trace.Analyze().Status);
    }

    [Fact]
    public void RecordingPersistsOldGenerationBeforePublishingFreshStreams()
    {
        var recording = new ProbeRecording();
        var oldSnapshots = recording.Snapshots;
        var oldLoaders = recording.Loaders;
        oldSnapshots.Append(Snapshot(0));
        oldLoaders.Record(Loader(1, "enter", 1, generation: 0));
        Assert.True(recording.TryAdvance(1, () =>
        {
            Assert.Equal(0, recording.Generation);
            Assert.Same(oldSnapshots, recording.Snapshots);
            Assert.Same(oldLoaders, recording.Loaders);
            Assert.Single(recording.Snapshots.Snapshots);
            Assert.Single(recording.Loaders.Events);
            return true;
        }));
        Assert.Equal(1, recording.Generation);
        Assert.NotSame(oldSnapshots, recording.Snapshots);
        Assert.NotSame(oldLoaders, recording.Loaders);
        Assert.Empty(recording.Snapshots.Snapshots);
        Assert.Empty(recording.Loaders.Events);
        Assert.Single(oldSnapshots.Snapshots);
        Assert.Single(oldLoaders.Events);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void FailedOrThrowingPersistenceRetainsOldRecordingForRetry(bool throws)
    {
        var recording = new ProbeRecording();
        Assert.True(recording.TryAdvance(1, () => true));
        var snapshots = recording.Snapshots;
        var loaders = recording.Loaders;
        snapshots.Append(Snapshot());
        loaders.Record(Loader(1, "enter", 1));
        bool sawOld = false;
        Assert.False(recording.TryAdvance(2, () =>
        {
            sawOld = recording.Generation == 1 && ReferenceEquals(snapshots, recording.Snapshots)
                && ReferenceEquals(loaders, recording.Loaders);
            if (throws) throw new IOException("injected disk failure");
            return false;
        }));
        Assert.True(sawOld);
        Assert.Equal(1, recording.Generation);
        Assert.Same(snapshots, recording.Snapshots);
        Assert.Same(loaders, recording.Loaders);
        Assert.Single(recording.Snapshots.Snapshots);
        Assert.Single(recording.Loaders.Events);
        Assert.True(recording.TryAdvance(2, () => true));
        Assert.Single(snapshots.Snapshots);
        Assert.Single(loaders.Events);
    }

    [Fact]
    public void SameOrBackwardGenerationDoesNotWriteAndNestedAdvanceCannotReplaceOldState()
    {
        var recording = new ProbeRecording();
        int writes = 0;
        bool Persist() { writes++; return true; }
        Assert.True(recording.TryAdvance(0, Persist));
        Assert.False(recording.TryAdvance(-1, Persist));
        bool nested = true;
        Assert.True(recording.TryAdvance(1, () => { nested = recording.TryAdvance(2, Persist); return true; }));
        Assert.False(nested);
        Assert.True(recording.TryAdvance(1, Persist));
        Assert.False(recording.TryAdvance(0, Persist));
        Assert.Equal(0, writes);
        Assert.Equal(1, recording.Generation);
    }

    [Fact]
    public void ThreeNinetyEventLoadsRetainTheirInventoriesWithoutASessionWideCap()
    {
        var recording = new ProbeRecording();
        var saved = new List<(long Generation, ProbeTrace Snapshots, ProbeLoaderTrace Loaders)>();
        bool Persist()
        {
            if (recording.Generation > 0) saved.Add((recording.Generation, recording.Snapshots, recording.Loaders));
            return true;
        }
        for (long generation = 1; generation <= 3; generation++)
        {
            Assert.True(recording.TryAdvance(generation, Persist));
            recording.Snapshots.Append(Snapshot(generation));
            foreach (var entry in NinetyLoaderEvents())
                Assert.True(recording.Loaders.Record(entry with
                {
                    MainGeneration = generation,
                    Sequence = entry.Sequence + (generation - 1) * 100,
                    CallId = entry.CallId + (generation - 1) * 45
                }));
            recording.Snapshots.Append(Snapshot(generation) with { Phase = "settled" });
        }
        Assert.True(recording.TryAdvance(4, Persist));
        Assert.Equal(new long[] { 1, 2, 3 }, saved.Select(x => x.Generation));
        Assert.Equal(270, saved.Sum(x => x.Loaders.Events.Count));
        Assert.All(saved, capture =>
        {
            Assert.Equal(2, capture.Snapshots.Snapshots.Count);
            Assert.False(capture.Snapshots.Truncated);
            Assert.Equal(90, capture.Loaders.Events.Count);
            Assert.Equal(0, capture.Loaders.DroppedEvents);
            Assert.Equal("observed", capture.Loaders.Analyze().Status);
        });
    }
}
