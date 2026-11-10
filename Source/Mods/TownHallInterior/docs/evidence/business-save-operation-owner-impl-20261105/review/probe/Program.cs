using System.Collections;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text.Json;
using TownHallInterior;

internal static class Program
{
    private static readonly List<object> Results = new();
    private static readonly BusinessSaveModelEffect[] Effects = Enum.GetValues<BusinessSaveModelEffect>();
    private static int _passed, _failed, _raceAdmitted, _raceSuppressed;

    private sealed class Model
    {
        public readonly BusinessSaveProcessModel Process = new();
        public readonly BusinessSaveOperationOwner Owner;
        public long Sequence;
        public Model(bool bind = true)
        {
            Owner = Process.CreateOwner();
            if (bind) Check(Owner.TryBind(Candidate, 0), "bind");
        }
        public BusinessSaveModelStamp Stamp(double time = 0) =>
            new(new(Process.ProcessId, 1), Owner.Snapshot.Binding ?? Candidate, ++Sequence, time);
        public void Before(BusinessSaveModelEffect effect)
        {
            foreach (var earlier in Effects.TakeWhile(x => x != effect))
            {
                Check(Owner.TryRecordIntent(earlier, Stamp()), "earlier intent");
                Check(Owner.RecordOutcome(earlier, BusinessSaveModelOutcome.Returned, Stamp()), "earlier returned");
                Check(Owner.RecordReadback(earlier, true, Stamp()), "earlier readback");
            }
        }
    }

    private static BusinessSaveModelBinding Candidate
    {
        get
        {
            // A string only: this program does not create/open directories or save files.
            string path = Path.GetFullPath("C:/TownHall-Synthetic-Independent-Review/profile-a/SaveGame_2");
            return new(new(true, true, true, true, path, path, "TownHallTest"),
                1, "load", "save", "list", "base", "completed", "requests", "participant");
        }
    }

    private static void Check(bool value, string label)
    {
        if (!value) throw new InvalidOperationException(label);
    }
    private static void NoAuthority(BusinessSaveModelSnapshot state)
    {
        Check(state.SimulationOnly && !state.NativeSaveWriteAllowed && !state.SafeToDestroy && !state.SafeToUnregister,
            "authority remains false");
        Check(state.ReferenceProof == "unknown" && state.NativeWriteProof == "unknown" && state.ModDataProof == "unknown",
            "native proofs remain unknown");
    }
    private static void Run(string name, Action action, string kind = "public-api")
    {
        try { action(); _passed++; Results.Add(new { name, kind, status = "passed" }); }
        catch (Exception ex) { _failed++; Results.Add(new { name, kind, status = "failed", error = ex.ToString() }); }
    }
    private static void Worker(Action action)
    {
        Exception? failure = null;
        var thread = new Thread(() => { try { action(); } catch (Exception ex) { failure = ex; } });
        thread.IsBackground = true;
        thread.Start();
        Check(thread.Join(TimeSpan.FromSeconds(5)), "worker timeout");
        if (failure != null) throw new InvalidOperationException("worker failed", failure);
    }
    private static void SetCounter(BusinessSaveProcessModel model, string field, long value) =>
        typeof(BusinessSaveProcessModel).GetField(field, BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(model, value);

    private static int Main()
    {
        foreach (var effect in Effects)
        {
            Run("late failure after confirmed return: " + effect, () =>
            {
                var m = new Model(); m.Before(effect);
                Check(m.Owner.TryRecordIntent(effect, m.Stamp()), "intent");
                Check(m.Owner.RecordOutcome(effect, BusinessSaveModelOutcome.Returned, m.Stamp()), "return");
                Check(m.Owner.RecordReadback(effect, true, m.Stamp()), "confirmed");
                Check(m.Owner.RecordOutcome(effect, BusinessSaveModelOutcome.Failed, m.Stamp()), "late failed");
                Check(m.Owner.Snapshot.Effects[effect] == new BusinessSaveModelEffectState(true, true, true, true), "all latches");
                Check(!m.Owner.RecordOutcome(effect, BusinessSaveModelOutcome.Returned, m.Stamp()), "duplicate return");
                Check(m.Owner.Snapshot.Revoked, "revoked"); NoAuthority(m.Owner.Snapshot);
            });
            Run("failed readback before return retained: " + effect, () =>
            {
                var m = new Model(); m.Before(effect);
                Check(m.Owner.TryRecordIntent(effect, m.Stamp()), "intent");
                Check(m.Owner.RecordReadback(effect, false, m.Stamp()), "failed readback");
                Check(m.Owner.RecordOutcome(effect, BusinessSaveModelOutcome.Returned, m.Stamp()), "late return");
                Check(m.Owner.RecordOutcome(effect, BusinessSaveModelOutcome.Failed, m.Stamp()), "independent failed outcome");
                Check(!m.Owner.RecordReadback(effect, true, m.Stamp()), "readback once-only");
                Check(m.Owner.Snapshot.Effects[effect] == new BusinessSaveModelEffectState(true, true, false, true), "failure retained");
                NoAuthority(m.Owner.Snapshot);
            });
        }

        Run("64th diagnostic fits, 65th intent is not admitted", () =>
        {
            var m = new Model();
            for (int i = 0; i < 63; i++) Check(m.Owner.ObserveTelemetry("hint", m.Stamp()), "telemetry");
            Check(!m.Owner.Snapshot.Revoked && m.Owner.Snapshot.Diagnostics.Count == 64, "full but complete");
            Check(!m.Owner.TryRecordIntent(BusinessSaveModelEffect.TypeRegistration, m.Stamp()), "overflow suppressed intent");
            var s = m.Owner.Snapshot;
            Check(s.Revoked && s.DiagnosticsIncomplete && s.FirstRevocationReason == "diagnostic-ring-overflow", "overflow cause");
            Check(!s.Effects[BusinessSaveModelEffect.TypeRegistration].Intent && s.Diagnostics.Count == 64, "bounded no intent");
            NoAuthority(s);
        });
        Run("late returned outcome remains recorded at ring overflow", () =>
        {
            var m = new Model();
            Check(m.Owner.TryRecordIntent(BusinessSaveModelEffect.TypeRegistration, m.Stamp()), "intent");
            for (int i = 0; i < 62; i++) m.Owner.ObserveTelemetry("hint", m.Stamp());
            Check(m.Owner.RecordOutcome(BusinessSaveModelEffect.TypeRegistration, BusinessSaveModelOutcome.Returned, m.Stamp()), "bookkeeping accepted");
            Check(m.Owner.Snapshot.Revoked && m.Owner.Snapshot.Effects[BusinessSaveModelEffect.TypeRegistration].Returned, "retained");
            NoAuthority(m.Owner.Snapshot);
        });
        Run("maximum message sequence suppresses an otherwise eligible intent", () =>
        {
            var m = new Model(); var stamp = m.Stamp() with { Sequence = long.MaxValue };
            Check(!m.Owner.TryRecordIntent(BusinessSaveModelEffect.TypeRegistration, stamp), "no max intent");
            Check(m.Owner.Snapshot.LastSequence == long.MaxValue && m.Owner.Snapshot.DiagnosticsIncomplete, "max recorded");
            Check(!m.Owner.Snapshot.Effects[BusinessSaveModelEffect.TypeRegistration].Intent, "not admitted");
        });
        Run("maximum message sequence still retains a late return", () =>
        {
            var m = new Model();
            Check(m.Owner.TryRecordIntent(BusinessSaveModelEffect.TypeRegistration, m.Stamp()), "intent");
            Check(m.Owner.RecordOutcome(BusinessSaveModelEffect.TypeRegistration, BusinessSaveModelOutcome.Returned,
                m.Stamp() with { Sequence = long.MaxValue }), "max return recorded");
            Check(m.Owner.Snapshot.Revoked && m.Owner.Snapshot.LastSequence == long.MaxValue, "closed at maximum");
            NoAuthority(m.Owner.Snapshot);
        });
        Run("ticket exhaustion never wraps and final ticket can exit", () =>
        {
            var m = new Model(); SetCounter(m.Process, "_lastTicket", long.MaxValue - 1);
            long last = m.Owner.EnterCallback(m.Stamp());
            Check(last == long.MaxValue && m.Owner.Snapshot.ActiveCallbacks == 1, "last tracked ticket");
            Check(m.Owner.Snapshot.Revoked && m.Owner.Snapshot.DiagnosticsIncomplete, "closed at max ticket");
            Check(m.Owner.EnterCallback(m.Stamp()) == 0, "no wrap");
            Check(m.Owner.Snapshot.CallbacksUncertain, "rejected entry uncertain");
            Check(m.Owner.ExitCallback(last, m.Stamp()) && m.Owner.Snapshot.ActiveCallbacks == 0, "late finally");
            NoAuthority(m.Owner.Snapshot);
        }, "white-box-counter-injection; not a reachable-duration claim");
        Run("diagnostic sequence exhaustion never wraps or recursively logs", () =>
        {
            var m = new Model(); SetCounter(m.Process, "_diagnosticSequence", long.MaxValue - 1);
            Check(m.Owner.ObserveTelemetry("last", m.Stamp()), "last diagnostic");
            var old = m.Owner.Snapshot;
            Check(old.Diagnostics.Last().Sequence == long.MaxValue && old.Revoked && old.DiagnosticsIncomplete, "max diagnostic");
            Check(m.Owner.ObserveTelemetry("not retained", m.Stamp()), "observation bookkeeping");
            m.Owner.Revoke("extra");
            Check(m.Owner.Snapshot.Diagnostics.Count == old.Diagnostics.Count, "not wrapped");
            NoAuthority(m.Owner.Snapshot);
        }, "white-box-counter-injection; not a reachable-duration claim");

        Run("off-thread bind on an existing facade cannot bind later", () =>
        {
            var m = new Model(false);
            Worker(() => Check(!m.Owner.TryBind(Candidate, 0), "off-thread bind"));
            Check(m.Owner.Snapshot.Revoked && !m.Owner.Snapshot.SlotConsumed, "early closed");
            Check(!m.Process.CreateOwner().TryBind(Candidate, 0), "new facade cannot reset");
        });
        Run("off-thread callback exit cannot remove the live ticket", () =>
        {
            var m = new Model(); long ticket = m.Owner.EnterCallback(m.Stamp()); var stamp = m.Stamp();
            Worker(() => Check(!m.Owner.ExitCallback(ticket, stamp), "off-thread exit"));
            Check(m.Owner.Snapshot.ActiveCallbacks == 1 && m.Owner.Snapshot.CallbacksUncertain, "live ticket preserved");
            Check(m.Owner.ExitCallback(ticket, m.Stamp()), "main-thread late exit");
            Check(m.Owner.Snapshot.ActiveCallbacks == 0 && m.Owner.Snapshot.CallbacksUncertain, "uncertainty remains");
            NoAuthority(m.Owner.Snapshot);
        });
        Run("unbound exit quarantines before any binding slot", () =>
        {
            var m = new Model(false);
            Check(!m.Owner.ExitCallback(1, m.Stamp()), "unbound exit rejected");
            Check(m.Owner.Snapshot.CallbacksUncertain && m.Owner.Snapshot.Revoked && !m.Owner.Snapshot.SlotConsumed, "uncertain tombstone");
        });
        foreach (string invalid in new[] { "null", "sequence", "clock", "binding" })
            Run("invalid callback entry and exit leave tickets intact: " + invalid, () =>
            {
                var m = new Model(); long ticket = m.Owner.EnterCallback(m.Stamp());
                BusinessSaveModelStamp bad = invalid switch
                {
                    "null" => null!,
                    "sequence" => m.Stamp() with { Sequence = 0 },
                    "clock" => m.Stamp(double.NaN),
                    _ => m.Stamp() with { Binding = null! }
                };
                Check(m.Owner.EnterCallback(bad) == 0 && !m.Owner.ExitCallback(ticket, bad), "invalid rejected");
                Check(m.Owner.Snapshot.ActiveCallbacks == 1 && m.Owner.Snapshot.CallbacksUncertain, "uncertain but not decremented");
                Check(m.Owner.ExitCallback(ticket, m.Stamp()), "valid late finally");
                NoAuthority(m.Owner.Snapshot);
            });

        foreach (int number in new[] { -1, int.MinValue, int.MaxValue })
            Run("undefined negative/extreme effect values: " + number, () =>
            {
                var m = new Model();
                Check(!m.Owner.TryRecordIntent((BusinessSaveModelEffect)number, m.Stamp()), "intent rejected");
                Check(!m.Owner.RecordOutcome((BusinessSaveModelEffect)number, BusinessSaveModelOutcome.Returned, m.Stamp()), "outcome rejected");
                Check(!m.Owner.RecordReadback((BusinessSaveModelEffect)number, false, m.Stamp()), "readback rejected");
                Check(m.Owner.Snapshot.Effects.Count == 6 && m.Owner.Snapshot.Effects.All(x => !x.Value.Intent), "ledger fixed");
            });
        Run("canonical stamp aliases accepted without rebinding identity labels", () =>
        {
            var m = new Model(); var old = m.Owner.Snapshot.Binding!;
            string alias = "C:/TownHall-Synthetic-Independent-Review/profile-a/alias/../SaveGame_2/";
            var stamp = m.Stamp() with { Binding = old with { Context = old.Context with { ActiveSavePath = alias.ToUpperInvariant() } } };
            Check(m.Owner.ObserveTelemetry("alias", stamp), "canonical alias");
            Check(m.Owner.Snapshot.Binding == old && !m.Owner.Snapshot.Revoked, "binding unchanged");
            Check(!m.Owner.ObserveTelemetry("changed label", m.Stamp() with { Binding = old with { LoadManager = "LOAD" } }), "label remains exact");
        });
        Run("controls in paths are denied, controls in bounded diagnostics are sanitized", () =>
        {
            var m = new Model(false); var b = Candidate;
            Check(!m.Owner.TryBind(b with { Context = b.Context with { ActiveSavePath = b.Context.ActiveSavePath + "\n" } }, 0), "path controls rejected");
            Check(m.Owner.TryBind(b, 0), "valid retry before binding");
            Check(m.Owner.ObserveTelemetry("\n\t" + new string('x', 9999), m.Stamp() with { Source = "\0\r" }), "bounded telemetry");
            m.Owner.Revoke("\t\n" + new string('r', 9999));
            var s = m.Owner.Snapshot;
            Check(s.FirstRevocationReason!.Length <= 160 && !s.FirstRevocationReason.Any(char.IsControl), "bounded cause");
            Check(s.Diagnostics.All(x => x.Kind.Length <= 160 && x.Detail.Length <= 160 &&
                !x.Kind.Any(char.IsControl) && !x.Detail.Any(char.IsControl)), "bounded controls");
        });
        Run("huge finite clock is timeout-only and late readback remains data", () =>
        {
            var m = new Model();
            Check(m.Owner.TryRecordIntent(BusinessSaveModelEffect.TypeRegistration, m.Stamp()), "intent");
            Check(m.Owner.RecordOutcome(BusinessSaveModelEffect.TypeRegistration, BusinessSaveModelOutcome.Returned, m.Stamp()), "return");
            Check(m.Owner.RecordReadback(BusinessSaveModelEffect.TypeRegistration, true, m.Stamp(double.MaxValue)), "late readback");
            Check(m.Owner.Snapshot.LastTime == double.MaxValue && m.Owner.Snapshot.FirstRevocationReason == "diagnostic-timeout", "finite timeout");
            NoAuthority(m.Owner.Snapshot);
        });
        Run("snapshot copies cannot mutate live data or computed native proofs", () =>
        {
            var m = new Model(); var a = m.Owner.Snapshot; var b = m.Owner.Snapshot;
            Check(!ReferenceEquals(a.Effects, b.Effects) && !ReferenceEquals(a.Diagnostics, b.Diagnostics), "separate containers");
            Check(((IDictionary)a.Effects).IsReadOnly && ((IList)a.Diagnostics).IsReadOnly, "read-only wrappers");
            var forged = a with { Status = "released-proven", Revoked = false, Key = new(Guid.Empty, 999) };
            NoAuthority(forged); Check(m.Owner.Snapshot.Key == a.Key && m.Owner.Snapshot.Status == "bound-data-only", "no model mutation");
            foreach (var p in typeof(BusinessSaveModelSnapshot).GetProperties().Where(p => p.Name.EndsWith("Proof") || p.Name.StartsWith("SafeTo") || p.Name == "NativeSaveWriteAllowed"))
                Check(p.SetMethod == null, "no proof/permission setter");
        });
        Run("concurrent revoke versus main-thread intent is linearizable in 200 synthetic trials", () =>
        {
            for (int i = 0; i < 200; i++)
            {
                var m = new Model(); using var barrier = new Barrier(2);
                var worker = new Thread(() => { barrier.SignalAndWait(); m.Owner.Revoke("race"); }) { IsBackground = true };
                worker.Start(); barrier.SignalAndWait();
                bool admitted = m.Owner.TryRecordIntent(BusinessSaveModelEffect.TypeRegistration, m.Stamp());
                Check(worker.Join(TimeSpan.FromSeconds(5)), "revoke race timeout");
                var s = m.Owner.Snapshot;
                Check(s.Revoked && s.Effects[BusinessSaveModelEffect.TypeRegistration].Intent == admitted, "allowed serial order");
                if (admitted) _raceAdmitted++; else _raceSuppressed++;
                Check(!m.Owner.TryRecordIntent(BusinessSaveModelEffect.TypeRegistration, m.Stamp()), "never readmitted");
                NoAuthority(s);
            }
        }, "managed-model stress; not native entry synchronization");

        Console.WriteLine(JsonSerializer.Serialize(new
        {
            schema = 1, checkedUtc = DateTime.UtcNow.ToString("O"),
            scope = "Independent probes of frozen synthetic V2 source; no native calls or IO in model",
            passed = _passed, failed = _failed, total = _passed + _failed,
            raceTrials = 200, raceAdmitted = _raceAdmitted, raceSuppressed = _raceSuppressed,
            nativeSaveWriteAllowed = false, referenceProof = "unknown", nativeWriteProof = "unknown", modDataProof = "unknown",
            results = Results
        }, new JsonSerializerOptions { WriteIndented = true }));
        return _failed == 0 ? 0 : 1;
    }
}
