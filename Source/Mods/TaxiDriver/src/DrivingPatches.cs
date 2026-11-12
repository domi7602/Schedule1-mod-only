// ---------------------------------------------------------------------------
// DrivingPatches.cs
//
// Mechanically consolidated from the former per-type source files. Each original
// file keeps its own block namespace with its own using directives, so name
// binding is unchanged; the bodies differ from the pre-consolidation sources only
// by the block-namespace indent level. Machine markers delimit the section that
// came from each original file so the source-contract tests can still read that
// file's exact content.
//
//   Driving/CruiseSpeedPatch.cs    -> section CruiseSpeedPatch.cs
//   Driving/NativeActuatorPatch.cs -> section NativeActuatorPatch.cs
//   Traffic/TrafficLightPatch.cs   -> section TrafficLightPatch.cs
//   Navigation/NativeShortcutPatch -> hash-pinned native comparison guard
//
// ---------------------------------------------------------------------------

namespace TaxiDriver
{
    using System;
    using System.Linq;
    using System.Reflection;
    using HarmonyLib;
    using Il2CppInterop.Runtime.InteropTypes;
    using Il2CppScheduleOne.Vehicles.AI;
    using S1Mods.Shared;
    using UnityEngine;
    using NativePath = Il2CppPathfinding.Path;
    using Routine = Il2CppScheduleOne.Vehicles.AI.NavigationUtility.__c__DisplayClass5_0.ObjectCompilerGeneratedNPrivateSealedIEnumerator1ObjectIEnumeratorIDisposableInObPaLiIn1NoInVeLiUnique;

    /// <summary>
    /// f12 RVA AD0D1D: an Error/empty direct shortcut must not beat a valid road group
    /// merely because its length is zero. A checked taxi detour via may instead
    /// prefer a complete, clear, shorter direct leg. Observe is the default development build.
    /// Only the second, ordered native getter return in one bound MoveNext may change.
    /// </summary>
    internal static class NativeShortcutPatch
    {
        private const string Owner = "com.taxidriver.native-shortcut";
#if TAXI_SHORTCUT_GUARD
        private const bool Observe = false;
#else
        private const bool Observe = true;
#endif
        private static Harmony? _harmony;
        private static ShortcutPatchGroup? _group;
        private static Hook[]? _hooks;
        private static bool _fault;
        private static int _thread;
        private static long _nonce;
        private static Receipt? _receipt;
        [ThreadStatic] private static Dispatch? _dispatch;
        private static long _logOperation = -1;
        private static int _lines;
        private const int LineLimit = 32;
        private const float MaxDirectDetourMeters = 40f;

        private sealed record Hook(MethodInfo Target, string Prefix, string? Postfix = null, string? Finalizer = null);
        // Scalar lease only: no path, closure, coroutine, agent or Unity wrapper across yields.
        private sealed class Receipt
        {
            internal long Epoch, Nonce, Operation, Generation;
            internal int Order;
            internal IntPtr Vehicle, Agent, General, Road, Settings, Flags, Routine, Closure, Coroutine;
            internal Vector3 Target, Start;
            internal bool Used;
        }
        private sealed class Dispatch
        {
            internal Dispatch? Parent;
            internal Receipt? Receipt;
            internal int Bindings;
            internal bool Closed;
        }
        private readonly record struct PathView(IntPtr Pointer, ushort Id, bool Error,
            Il2CppPathfinding.PathCompleteState State, int Count, float Length, Vector3 Start, Vector3 End,
            Vector3[]? Points);
        private sealed class Move
        {
            internal ShortcutComparison.Frame Frame = null!;
            internal Receipt? Receipt;
            internal IntPtr Group;
            internal PathView Entry, Road, Direct;
            internal string NativeResult = "not-observed";
            internal string NativePath = "not-observed";
            internal bool Diagnose, Finished, PreferDirect, DirectSelected;
        }

        internal static void Apply()
        {
            Invalidate();
            _thread = Environment.CurrentManagedThreadId;
            try
            {
                bool compatible = Compatible();
                _harmony ??= new Harmony(Owner);
                _hooks ??= new[]
                {
                    new Hook(Method(typeof(VehicleAgent), nameof(VehicleAgent.Navigate), typeof(void),
                        typeof(Vector3), typeof(NavigationSettings), typeof(VehicleAgent.NavigationCallback)),
                        nameof(NavigatePrefix), nameof(NavigatePostfix), nameof(NavigateFinalizer)),
                    new Hook(Method(typeof(Routine), "MoveNext", typeof(bool)),
                        nameof(MovePrefix), nameof(MovePostfix), nameof(MoveFinalizer)),
                    new Hook(Method(typeof(NativePath), nameof(NativePath.GetTotalLength), typeof(float)),
                        nameof(LengthPrefix), nameof(LengthPostfix), nameof(LengthFinalizer)),
                    new Hook(Method(typeof(VehicleAgent), nameof(VehicleAgent.NavigationCalculationCallback), typeof(void),
                        typeof(NavigationUtility.ENavigationCalculationResult), typeof(Il2CppScheduleOne.Math.PathSmoothingUtility.SmoothedPath)),
                        nameof(ResultPrefix))
                };
                _group ??= new ShortcutPatchGroup(_hooks.Length, Install, Exact, Any,
                    i => _harmony.Unpatch(_hooks[i].Target, HarmonyPatchType.All, Owner));
                bool ready = _group.Apply(compatible && !_fault);
                Mod.Log.Info($"[shortcut] mode={(Observe ? "Observe" : "Guard")} compatible={compatible} registrations={ready} removalPending={_group.RemovalPending}; native binding/getter hits unverified; max {LineLimit} lines/operation.");
            }
            catch (Exception ex)
            {
                Fault("bootstrap", ex);
                bool clean = _group?.Remove() ?? true;
                try { Mod.Log.Warn($"[shortcut] bootstrap rollbackClean={clean} removalPending={_group?.RemovalPending == true}"); } catch { }
            }
        }

        private static MethodInfo Method(Type type, string name, Type returns, params Type[] args)
        {
            var method = AccessTools.DeclaredMethod(type, name, args);
            if (method == null || method.IsStatic || method.ReturnType != returns || method.DeclaringType != type)
                throw new MissingMethodException(type.FullName, name);
            return method;
        }

        private static bool Compatible()
        {
            static bool Hash(string path, string expected)
            {
                using var input = System.IO.File.OpenRead(path);
                using var sha = System.Security.Cryptography.SHA256.Create();
                return Convert.ToHexString(sha.ComputeHash(input)).Equals(expected, StringComparison.OrdinalIgnoreCase);
            }
            static void Property(Type owner, string name, Type type)
            {
                var p = owner.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                if (p?.PropertyType != type || p.GetMethod == null || p.GetIndexParameters().Length != 0)
                    throw new MissingMemberException(owner.FullName, name);
            }
            Property(typeof(Routine), "__1__state", typeof(int));
            Property(typeof(Routine), "__4__this", typeof(NavigationUtility.__c__DisplayClass5_0));
            Property(typeof(Routine), "_finalGroup_5__2", typeof(PathGroup));
            Property(typeof(NavigationUtility.__c__DisplayClass5_0), "lastCalculatedPath", typeof(NativePath));
            Property(typeof(NavigationUtility.__c__DisplayClass5_0), "startPosition", typeof(Vector3));
            Property(typeof(NavigationUtility.__c__DisplayClass5_0), "destination", typeof(Vector3));
            Property(typeof(NavigationUtility.__c__DisplayClass5_0), "generalSeeker", typeof(Il2CppPathfinding.Seeker));
            Property(typeof(NavigationUtility.__c__DisplayClass5_0), "roadSeeker", typeof(Il2CppPathfinding.Seeker));
            Property(typeof(NavigationUtility.__c__DisplayClass5_0), "navSettings", typeof(NavigationSettings));
            Property(typeof(NavigationUtility.__c__DisplayClass5_0), "flags", typeof(DriveFlags));
            foreach (string name in new[] { "startToEntryPath", "entryToExitPath", "exitToDestinationPath" })
                Property(typeof(PathGroup), name, typeof(NativePath));
            string? game = null;
            using (var process = System.Diagnostics.Process.GetCurrentProcess())
                foreach (System.Diagnostics.ProcessModule module in process.Modules)
                    if (string.Equals(module.ModuleName, "GameAssembly.dll", StringComparison.OrdinalIgnoreCase)) game = module.FileName;
            var support = AppDomain.CurrentDomain.GetAssemblies().Single(a => a.GetName().Name == "Il2CppInterop.HarmonySupport");
            return game != null && Hash(game, "61829050e56ca180d67be592e4bfe61ec97d2e9573f720d90a54e318a7e4381d") &&
                Hash(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(game)!, "Schedule I_Data/il2cpp_data/Metadata/global-metadata.dat"),
                    "e775c97da277da8eb1a90265478f3b65c9c3d6464aff4f7255d45280a1d1181f") &&
                Hash(typeof(VehicleAgent).Assembly.Location, "346205306273805a78b01a917f14b94518498af1361595c4c417baa0b32b4bd8") &&
                Hash(typeof(NativePath).Assembly.Location, "0012f0a97bfc1104c2ebbdcfc1e8debf37be92a42da6e9a00c4079c23cd6f58d") &&
                Hash(typeof(Harmony).Assembly.Location, "6c898933b52149e8bcf6722305c7e4d94add47c9e53b364096f91721624c0877") &&
                Hash(support.Location, "f64c53d389fc76a910ffab86f7ac6d27e437b0c7c0c60918565a5d9f34f8ad06") &&
                Hash(typeof(Il2CppObjectBase).Assembly.Location, "3878ce0ba0c60385f76f2ba37e07ad0ee3649d019703aec8695db3402abeedd7");
        }

        private static HarmonyMethod Patch(string name, int priority) => new(typeof(NativeShortcutPatch), name) { priority = priority };
        private static bool Install(int i)
        {
            var hook = _hooks![i];
            return PatchGuard.TryPatch(_harmony!, hook.Target, prefix: Patch(hook.Prefix, Priority.First),
                postfix: hook.Postfix == null ? null : Patch(hook.Postfix, Priority.Last),
                finalizer: hook.Finalizer == null ? null : Patch(hook.Finalizer, Priority.Last), log: Mod.Log);
        }
        private static bool Any(int i) => Harmony.GetPatchInfo(_hooks![i].Target)?.Owners.Contains(Owner) == true;
        private static bool Exact(int i)
        {
            var hook = _hooks![i];
            var info = Harmony.GetPatchInfo(hook.Target);
            if (info == null) return false;
            bool Has(System.Collections.Generic.IEnumerable<HarmonyLib.Patch> list, string name) =>
                list.Count(p => p.owner == Owner) == 1 &&
                list.Any(p => p.owner == Owner && p.PatchMethod.DeclaringType == typeof(NativeShortcutPatch) && p.PatchMethod.Name == name);
            bool Known(HarmonyLib.Patch p) => p.owner == Owner ||
                ((i == 0 || i == 3) && p.PatchMethod.DeclaringType == typeof(SpikeTrace));
            return Has(info.Prefixes, hook.Prefix) && (hook.Postfix == null ? !info.Postfixes.Any(p => p.owner == Owner) : Has(info.Postfixes, hook.Postfix)) &&
                (hook.Finalizer == null ? !info.Finalizers.Any(p => p.owner == Owner) : Has(info.Finalizers, hook.Finalizer)) &&
                info.Prefixes.Concat(info.Postfixes).Concat(info.Finalizers).All(Known) &&
                info.Transpilers.Count == 0 && info.ILManipulators.Count == 0;
        }

        internal static void Invalidate()
        {
            ShortcutComparison.Invalidate();
            _receipt = null;
            // Existing synchronous frames still unwind via their captured Harmony state.
        }
        internal static void Remove()
        {
            Invalidate();
            bool clean = _group?.Remove() ?? true;
            try { Mod.Log.Info($"[shortcut] registrationsRemoved={clean}; removalPending={_group?.RemovalPending == true}"); } catch { }
        }
        private static void Fault(string where, Exception? ex = null)
        {
            bool first = !_fault;
            _fault = true;
            Invalidate();
            if (first) try { Mod.Log.Warn($"[shortcut] disabled for session at {where}: {ex?.Message ?? "unexpected comparison/stack protocol"}; native behavior retained."); } catch { }
        }
        private static bool Ready()
        {
            if (_fault || Environment.CurrentManagedThreadId != _thread || _group?.Healthy != true) return false;
            if (_group.Verify()) return true;
            Fault("patch-health readback");
            return false;
        }
        internal static bool CanValidateDetour => Ready();

        private static bool MatchesTarget(Vector3 target) =>
            target.Equals(SpikeState.NavTarget) || SpikeRunner.IsDetourTarget(target);
        private static IntPtr Ptr(Il2CppObjectBase? value) => value == null || value.WasCollected ? IntPtr.Zero : value.Pointer;
        private static bool Finite(Vector3 p) => float.IsFinite(p.x) && float.IsFinite(p.y) && float.IsFinite(p.z);
        private static bool BaseCurrent(VehicleAgent agent) => Ready() && Mod.Lifecycle.AcceptingWork &&
            NetworkGuard.IsInMainScene && NetworkGuard.IsAlive(agent) && SpikeState.PollingActive && SpikeState.Operation.Active &&
            SpikeState.NavOrder > 0 && SpikeState.OperationId > 0 && SpikeCommands.HasAuthority() &&
            SpikeCommands.IsOwnedTaxi(agent.vehicle) && Ptr(SpikeState.Vehicle) == Ptr(agent.vehicle) &&
            Ptr(agent.vehicle.Agent) == Ptr(agent) && agent.vehicle.DriverPlayer == null &&
            !SpikeState.Reverse.Active && !agent.IsReversing && !TaxiAI.Active && !TaxiAI.NavigationBlocked;

        private static bool Current(Receipt r, VehicleAgent agent, bool calculating) =>
            r.Epoch == ShortcutComparison.Epoch && r.Nonce == _nonce && BaseCurrent(agent) &&
            r.Order == SpikeState.NavOrder && r.Operation == SpikeState.OperationId && r.Generation == SpikeState.Operation.Generation &&
            Ptr(agent) == r.Agent && Ptr(agent.vehicle) == r.Vehicle && Ptr(agent.generalSeeker) == r.General &&
            Ptr(agent.roadSeeker) == r.Road && Ptr(agent.Flags) == r.Flags &&
            MatchesTarget(r.Target) && (!calculating ||
                (agent.AutoDriving && agent.NavigationCalculationInProgress && agent.TargetLocation.Equals(r.Target) &&
                 Ptr(agent.currentNavigationSettings) == r.Settings && Ptr(agent.navigationCalculationRoutine) == r.Coroutine));

        private static void NavigatePrefix(VehicleAgent __instance, Vector3 location, NavigationSettings settings, out Dispatch? __state)
        {
            __state = new Dispatch { Parent = _dispatch };
            _dispatch = __state; // always mask an outer creation context, including foreign calls
            try
            {
                SpikeRunner.BeginDetourCalculation(__instance, location, settings);
                if (!BaseCurrent(__instance)) return;
                _receipt = null;
                long nonce = ++_nonce;
                if (settings == null || !settings.endAtRoad || !settings.ensureProximityToGraph || settings.teleportToGraphIfCalculationFails ||
                    !Finite(location) || !MatchesTarget(location) || !__instance.Flags.UseRoads) return;
                var r = new Receipt
                {
                    Epoch = ShortcutComparison.Epoch,
                    Nonce = nonce,
                    Order = SpikeState.NavOrder,
                    Operation = SpikeState.OperationId,
                    Generation = SpikeState.Operation.Generation,
                    Vehicle = Ptr(__instance.vehicle),
                    Agent = Ptr(__instance),
                    General = Ptr(__instance.generalSeeker),
                    Road = Ptr(__instance.roadSeeker),
                    Settings = Ptr(settings),
                    Flags = Ptr(__instance.Flags),
                    Target = location
                };
                if (r.General != IntPtr.Zero && r.Road != IntPtr.Zero && r.General != r.Road && r.Settings != IntPtr.Zero)
                    __state.Receipt = r;
            }
            catch (Exception ex) { Fault("dispatch capture", ex); }
        }
        private static void NavigatePostfix(VehicleAgent __instance, Dispatch? __state)
        {
            try
            {
                if (__state?.Receipt is not { } r) return;
                if (!ReferenceEquals(_dispatch, __state) || __state.Bindings != 1 || !Current(r, __instance, calculating: false))
                {
                    Log(r, $"creation-unbound state0Hits={__state.Bindings}; no late state2 adoption");
                    return;
                }
                r.Coroutine = Ptr(__instance.navigationCalculationRoutine);
                if (r.Coroutine != IntPtr.Zero && Current(r, __instance, calculating: true))
                {
                    _receipt = r;
                    Log(r, $"bound-state0 routine={r.Routine} closure={r.Closure} coroutine={r.Coroutine}");
                }
            }
            catch (Exception ex) { Fault("dispatch confirmation", ex); }
        }
        private static Exception? NavigateFinalizer(Exception? __exception, Dispatch? __state)
        {
            if (__state == null || __state.Closed) return __exception;
            __state.Closed = true;
            if (__exception != null) { Invalidate(); }
            if (ReferenceEquals(_dispatch, __state)) _dispatch = __state.Parent;
            else Fault("dispatch stack");
            return __exception;
        }

        private static void MovePrefix(Routine __instance, out Move? __state)
        {
            var call = new Move { Frame = ShortcutComparison.Create() };
            __state = call; // publish before activating any permission, including suppression
            call.Frame.Context = call;
            ShortcutComparison.Push(call.Frame);
            try
            {
                if (call.Frame.Faulted) { Fault("scope entry"); return; }
                if (!Ready() || Ptr(__instance) == IntPtr.Zero) return;
                int entryState = __instance.__1__state;
                var c = __instance.__4__this;
                if (Ptr(c) == IntPtr.Zero) return;
                if (entryState == 0 && _dispatch?.Receipt is { } created &&
                    Current(created, SpikeState.Vehicle!.Agent, calculating: false) &&
                    Ptr(c.generalSeeker) == created.General && Ptr(c.roadSeeker) == created.Road &&
                    Ptr(c.navSettings) == created.Settings && Ptr(c.flags) == created.Flags && c.destination.Equals(created.Target) && Finite(c.startPosition))
                {
                    _dispatch.Bindings++;
                    created.Routine = Ptr(__instance);
                    created.Closure = Ptr(c);
                    created.Start = c.startPosition;
                }
                if (entryState != 2 || _receipt is not { Used: false } r ||
                    Ptr(__instance) != r.Routine || Ptr(c) != r.Closure || !Bound(r, __instance)) return;
                r.Used = true;
                call.Receipt = r;
                call.Diagnose = true;
                var group = __instance._finalGroup_5__2;
                call.Group = Ptr(group);
                // All diagnostic getters execute while this frame is inert. They cannot
                // advance the two unsuppressed native calls or count as hook-hit evidence.
                call.Entry = Read(group?.startToEntryPath, measure: true, capturePoints: true);
                call.Road = Read(group?.entryToExitPath, measure: true, capturePoints: true);
                call.Direct = Read(c.lastCalculatedPath, measure: true);
                Log(r, $"state2-pre group={call.Group} entry=[{Show(call.Entry)}] road=[{Show(call.Road)}] direct=[{Show(call.Direct)}] start={c.startPosition} target={c.destination} ab=[{Coordinates(c.lastCalculatedPath)}]");
                var agent = SpikeState.Vehicle?.Agent;
                call.PreferDirect = !Observe && agent != null && Leg(call.Direct) && Leg(call.Entry) && Leg(call.Road) &&
                    call.Direct.Length <= MaxDirectDetourMeters &&
                    (double)call.Direct.Length < (double)call.Entry.Length + call.Road.Length &&
                    SpikeRunner.CanPreferDirectDetourVia(agent, r.Target, c.lastCalculatedPath);
                if (!ValidSnapshot(call, group)) return;
                ShortcutComparison.Arm(call.Frame, call.Entry.Pointer.ToInt64(), call.Direct.Pointer.ToInt64(), Observe,
                    () => Revalidate(call, __instance), preferredDirectLength: call.PreferDirect ? call.Direct.Length : null);
            }
            catch (Exception ex) { call.Frame.Cancel(); Fault("scope capture", ex); }
        }

        private static bool Bound(Receipt r, Routine routine)
        {
            var agent = SpikeState.Vehicle?.Agent;
            var c = routine.__4__this;
            return agent != null && ReferenceEquals(_receipt, r) && Current(r, agent, calculating: true) &&
                Ptr(routine) == r.Routine && Ptr(c) == r.Closure && c.startPosition.Equals(r.Start) && c.destination.Equals(r.Target) &&
                Ptr(c.generalSeeker) == r.General && Ptr(c.roadSeeker) == r.Road && Ptr(c.navSettings) == r.Settings &&
                Ptr(c.flags) == r.Flags && c.flags.UseRoads && c.navSettings.endAtRoad && c.navSettings.ensureProximityToGraph &&
                !c.navSettings.teleportToGraphIfCalculationFails;
        }
        private static PathView Read(NativePath? path, bool measure, bool capturePoints = false)
        {
            if (Ptr(path) == IntPtr.Zero) return default;
            var points = path!.vectorPath;
            int count = points?.Count ?? -1;
            if (count > 4096) throw new InvalidOperationException("path observation bound exceeded");
            Vector3 start = default, end = default;
            var snapshot = capturePoints && count >= 0 ? new Vector3[count] : null;
            for (int i = 0; i < count; i++)
            {
                Vector3 p = points![i];
                if (!Finite(p)) throw new InvalidOperationException("non-finite native path");
                if (i == 0) start = p;
                end = p;
                if (snapshot != null) snapshot[i] = p;
            }
            return new PathView(Ptr(path), path.pathID, path.error, path.CompleteState, count,
                measure ? path.GetTotalLength() : float.NaN, start, end, snapshot);
        }
        private static bool Leg(PathView v) => v.Pointer != IntPtr.Zero && !v.Error &&
            v.State == Il2CppPathfinding.PathCompleteState.Complete && v.Count >= 2 && float.IsFinite(v.Length) && v.Length > 0f;
        private static bool ValidSnapshot(Move call, PathGroup? group) => call.Group != IntPtr.Zero &&
            Leg(call.Entry) && Leg(call.Road) && Ptr(group!.exitToDestinationPath) == IntPtr.Zero &&
            call.Entry.Pointer != call.Road.Pointer && call.Direct.Pointer != call.Entry.Pointer && call.Direct.Pointer != call.Road.Pointer &&
            (call.PreferDirect || (call.Direct.Pointer != IntPtr.Zero && call.Direct.Error &&
                call.Direct.State == Il2CppPathfinding.PathCompleteState.Error && call.Direct.Count == 0 && call.Direct.Length == 0f));
        private static bool Same(PathView a, PathView b, bool fullGeometry = false) => a.Pointer == b.Pointer && a.Id == b.Id &&
            a.Error == b.Error && a.State == b.State && a.Count == b.Count && a.Start.Equals(b.Start) && a.End.Equals(b.End) &&
            (!fullGeometry || SamePoints(a.Points, b.Points));
        private static bool SamePoints(Vector3[]? first, Vector3[]? second)
        {
            if (first == null || second == null || first.Length != second.Length) return false;
            for (int i = 0; i < first.Length; i++)
                if (first[i].x != second[i].x || first[i].y != second[i].y || first[i].z != second[i].z) return false;
            return true;
        }
        private static bool Revalidate(Move call, Routine routine)
        {
            if (call.Receipt is not { } r || !Bound(r, routine)) return false;
            var c = routine.__4__this;
            var group = routine._finalGroup_5__2;
            var agent = SpikeState.Vehicle?.Agent;
            // Do not reread state==2: the audited native entry immediately changes it to -1.
            return Ptr(group) == call.Group && Ptr(group.exitToDestinationPath) == IntPtr.Zero &&
                Same(call.Entry, Read(group.startToEntryPath, false, call.PreferDirect), call.PreferDirect) &&
                Same(call.Road, Read(group.entryToExitPath, false, call.PreferDirect), call.PreferDirect) &&
                Same(call.Direct, Read(c.lastCalculatedPath, false)) &&
                Ptr(c.generalSeeker.GetCurrentPath()) == call.Direct.Pointer &&
                (!call.PreferDirect || (agent != null && SpikeRunner.CanPreferDirectDetourVia(agent, r.Target, c.lastCalculatedPath))) &&
                ManagedReceiptCurrent(r);
        }

        // Last check after ALL native-backed reads. Keep this primitive/managed-only:
        // no Ready/Bound, Unity getters, logging or Vector3.Equals native binding here.
        private static bool ManagedReceiptCurrent(Receipt r) => !_fault && _group?.Healthy == true &&
            Environment.CurrentManagedThreadId == _thread && ReferenceEquals(_receipt, r) &&
            r.Epoch == ShortcutComparison.Epoch && r.Nonce == _nonce && Mod.Lifecycle.AcceptingWork &&
            SpikeState.PollingActive && SpikeState.Operation.Active && !SpikeState.Reverse.Active &&
            r.Operation == SpikeState.OperationId && r.Order == SpikeState.NavOrder && r.Generation == SpikeState.Operation.Generation &&
            ((r.Target.x == SpikeState.NavTarget.x && r.Target.y == SpikeState.NavTarget.y && r.Target.z == SpikeState.NavTarget.z) ||
                SpikeRunner.IsDetourTarget(r.Target));

        private static void LengthPrefix(NativePath __instance, out ShortcutComparison.Call? __state)
        {
            __state = null;
            try
            {
                if (ShortcutComparison.Current is not { Stage: 1 or 2 or 3 or 4 } frame || frame.Epoch != ShortcutComparison.Epoch) return;
                __state = ShortcutComparison.Before(Ptr(__instance).ToInt64());
                if (ShortcutComparison.Current?.Faulted == true) Fault("getter order/reentrancy");
            }
            catch (Exception ex) { Fault("getter entry", ex); }
        }
        private static void LengthPostfix(ref float __result, ShortcutComparison.Call? __state)
        {
            try
            {
                float original = __result;
                bool changed = ShortcutComparison.After(__state, ref __result);
                // Record the consumed native comparison before diagnostics can reenter.
                // This includes a direct candidate that was already natively shorter.
                if (__state is { Stage: 4 } selected && selected.Frame is { Stage: 5, Closed: false, Faulted: false } frame &&
                    ReferenceEquals(ShortcutComparison.Current, frame) && frame.Epoch == ShortcutComparison.Epoch &&
                    frame.Context is Move { PreferDirect: true } directCall && ReferenceEquals(directCall.Frame, frame) &&
                    frame.DirectLength == directCall.Direct.Length && frame.DirectLength > 0f &&
                    float.IsFinite(__result) && __result < frame.EntryLength)
                    directCall.DirectSelected = true;
                if (__state?.Frame.Faulted == true) Fault("getter validation/result protocol");
                if (__state is { } token && token.Frame.Context is Move { Receipt: { } r })
                    Log(r, $"native-getter stage={token.Stage} path={token.Pointer} original={original} effective={__result} override={changed}");
            }
            catch (Exception ex) { Fault("getter return", ex); }
        }
        private static Exception? LengthFinalizer(Exception? __exception, ShortcutComparison.Call? __state)
        {
            if (__exception != null && __state.HasValue) { __state.Value.Frame.Cancel(); Fault("getter exception"); }
            return __exception;
        }
        private static bool SelectedDirectViaCurrent(VehicleAgent agent)
        {
            if (ShortcutComparison.Current is not { Stage: 5, Closed: false, Faulted: false } frame ||
                frame.Context is not Move { DirectSelected: true, Receipt: { } r } call ||
                !ReferenceEquals(call.Frame, frame) || !ReferenceEquals(_receipt, r))
                return false;
            return Current(r, agent, calculating: true) && ManagedReceiptCurrent(r) &&
                ReferenceEquals(ShortcutComparison.Current, frame) && !frame.Closed && !frame.Faulted &&
                frame.Stage == 5 && frame.Epoch == ShortcutComparison.Epoch;
        }
        private static void ResultPrefix(VehicleAgent __instance, ref NavigationUtility.ENavigationCalculationResult result,
            Il2CppScheduleOne.Math.PathSmoothingUtility.SmoothedPath _path)
        {
            try
            {
                if (result == NavigationUtility.ENavigationCalculationResult.Success &&
                    SpikeRunner.RejectDetourPath(__instance, _path, SelectedDirectViaCurrent(__instance)))
                    result = NavigationUtility.ENavigationCalculationResult.Failed;
                if (ShortcutComparison.Current?.Context is not Move { Receipt: { } r } call || Ptr(__instance) != r.Agent) return;
                call.NativeResult = result.ToString();
                var points = _path?.vectorPath;
                int count = points?.Count ?? -1;
                call.NativePath = $"count={count},end={(count > 0 ? points![count - 1].ToString() : "none")}";
            }
            catch { /* read-only result detail is optional */ }
            finally
            {
                // A result retires the scalar lease even when no state-2 observation was
                // eligible. Clearing on an ambiguous same-agent result is fail-closed.
                try { if (_receipt?.Agent == Ptr(__instance)) _receipt = null; }
                catch { _receipt = null; }
            }
        }
        private static void MovePostfix(Routine __instance, Move? __state) => Finish(__instance, __state);
        private static Exception? MoveFinalizer(Routine __instance, Exception? __exception, Move? __state)
        {
            Finish(__instance, __state);
            if (__exception != null && __state?.Receipt != null) Fault("native MoveNext exception");
            return __exception;
        }
        private static void Finish(Routine routine, Move? call)
        {
            if (call == null || call.Finished) return;
            call.Finished = true;
            bool missingGetter = call.Frame.Stage is 1 or 2 or 3 or 4;
            call.Frame.Cancel(); // no post-observation may use comparison permission
            try
            {
                if (call.Diagnose && call.Receipt is { } r)
                {
                    var after = routine._finalGroup_5__2;
                    Log(r, $"state2-post mode={(Observe ? "Observe" : "Guard")} override={call.Frame.Applied} groupBefore={call.Group} groupAfter={Ptr(after)} entryAfter=[{Show(Read(after?.startToEntryPath, true))}] roadAfter=[{Show(Read(after?.entryToExitPath, true))}] nativeResult={call.NativeResult} nativePath=[{call.NativePath}] preferDirectVia={call.PreferDirect} directSelected={call.DirectSelected}");
                    if (ReferenceEquals(_receipt, r)) _receipt = null;
                }
            }
            catch (Exception ex) { Fault("scope post-observation", ex); }
            finally
            {
                ShortcutComparison.Close(call.Frame);
                if (call.Frame.Faulted) Fault("scope cleanup");
                if (missingGetter) Fault("incomplete native getter sequence");
                if (call.Diagnose && call.Receipt is { } r)
                    Log(r, $"cleanup-complete closed={call.Frame.Closed} fault={call.Frame.Faulted} permissionCleared={call.Frame.Stage == 0}");
            }
        }
        private static string Show(PathView p) => $"ptr={p.Pointer},id={p.Id},error={p.Error},state={p.State},count={p.Count},length={p.Length},start={p.Start},end={p.End}";
        private static string Coordinates(NativePath? path)
        {
            try
            {
                var ab = path?.TryCast<Il2CppPathfinding.ABPath>();
                return ab == null ? "unavailable" : $"original={ab.originalStartPoint}->{ab.originalEndPoint},effective={ab.startPoint}->{ab.endPoint}";
            }
            catch { return "unavailable"; }
        }
        private static void Log(Receipt r, string message)
        {
            if (_logOperation != r.Operation) { _logOperation = r.Operation; _lines = 0; }
            if (_lines >= LineLimit) return;
            _lines++;
            try { Mod.Log.Info($"[shortcut] nonce={r.Nonce} op={r.Operation} order={r.Order} epoch={r.Epoch} event={_lines}/{LineLimit} {message}{(_lines == LineLimit ? " CAP reached" : "")}"); }
            catch { /* scalar diagnostics cannot authorize an effect */ }
        }
    }
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: CruiseSpeedPatch.cs ====
    using System;
    using HarmonyLib;
    using Il2CppScheduleOne.Vehicles;
    using Il2CppScheduleOne.Vehicles.AI;
    using S1Mods.Shared;
    using UnityEngine;


    /// <summary>
    /// Changes only the freshly calculated forward base target. In installed f12 native code,
    /// UpdateSpeed (RVA 0xAD86A0) calculates zone/unmarked speed; InfrequentUpdate invokes it
    /// BEFORE UpdateSteering, UpdateSweep and UpdateSpeedReduction (0xAD55A6). The latter
    /// still applies native turn/obstacle/destination reductions to this base. LateUpdate
    /// passes targetSpeed directly alongside LandVehicle.Speed_Kmh to the throttle PID
    /// (0xAD61AB..0xAD61E0): targetSpeed is km/h, NOT metres/second.
    /// No DriveFlags object is mutated or replaced, including potentially shared flags.
    /// Native patrol resets and redispatches are handled by the next native UpdateSpeed call.
    /// </summary>
    internal static class CruiseSpeedPatch
    {
        private static HarmonyLib.Harmony? _harmony;
        private static bool _enabled;
        private static bool _errorLogged;
        private static int _loggedOrder = -1;
        private static int _loggedApproachOrder = -1;

        internal static void Apply()
        {
            if (_enabled)
                return;
            try
            {
                _harmony ??= new HarmonyLib.Harmony("com.taxidriver.cruise");
                _enabled = PatchGuard.TryPatch(
                    _harmony,
                    typeof(VehicleAgent),
                    nameof(VehicleAgent.UpdateSpeed),
                    postfix: new HarmonyMethod(typeof(CruiseSpeedPatch), nameof(UpdateSpeedPostfix)),
                    log: Mod.Log);
                if (_enabled)
                    Mod.Log.Info("[cruise] owned autonomous taxi forward base target=50 km/h with a cap near the exact pickup or passenger stop; native speed reductions remain enabled. Achieved speed requires gameplay verification.");
                else
                    Mod.Log.Warn("[cruise] UpdateSpeed hook unavailable; native cruising speed retained.");
            }
            catch (Exception ex)
            {
                _enabled = false;
                Mod.Log.Warn($"[cruise] hook installation failed; native speed retained: {ex.Message}");
            }
        }

        private static void UpdateSpeedPostfix(VehicleAgent __instance)
        {
            if (!_enabled)
                return;
            try
            {
                if (!SpikeCommands.HasAuthority() || __instance == null)
                    return;
                var veh = __instance.vehicle;
                if (!SpikeCommands.IsOwnedTaxi(veh))
                    return;
                var flags = __instance.Flags;
                if (flags == null)
                    return;
                int order = SpikeState.NavOrder;
                long operation = SpikeState.OperationId;
                float nativeBase = __instance.targetSpeed;
                float approachDistance = GetApproachDistance(__instance, veh);
                float target = CruiseSpeedPolicy.BaseTargetKmh(
                    nativeBase,
                    ownedTaxi: true,
                    activeOrder: SpikeState.PollingActive || TaxiAI.Active,
                    autoDriving: __instance.AutoDriving,
                    playerDriving: veh.DriverPlayer != null,
                    reversing: __instance.IsReversing || SpikeState.Reverse.Active,
                    nativeOverride: flags.OverrideSpeed,
                    approachDistance: approachDistance);
                if (order != SpikeState.NavOrder || operation != SpikeState.OperationId ||
                    SpikeState.Vehicle?.Pointer != veh.Pointer)
                    return;
                if (target == nativeBase || !float.IsFinite(target))
                    return;
                __instance.targetSpeed = target;
                if (_loggedOrder != order)
                {
                    _loggedOrder = order;
                    Mod.Log.Info($"[cruise] order={order} nativeBase={nativeBase:0.0} targetBase={target:0.0} km/h actual={veh.Speed_Kmh:0.0} km/h; native speed reductions run next.");
                }
                if (_loggedApproachOrder != order && NavigationArrivalPolicy.IsWithinArrival(approachDistance))
                {
                    _loggedApproachOrder = order;
                    Mod.Log.Info($"[cruise] order={order} {(SpikeState.NavToPlayer ? "pickup" : "passenger")} stop {approachDistance:0.0} m away; capped base={target:0.0} km/h actual={veh.Speed_Kmh:0.0} km/h.");
                }
            }
            catch (Exception ex)
            {
                if (_errorLogged)
                    return;
                _errorLogged = true;
                try { Mod.Log.Warn($"[cruise] target update skipped on unreadable state: {ex.Message}"); }
                catch { /* Do not let logging break the native driving path. */ }
            }
        }

        private static float GetApproachDistance(VehicleAgent agent, LandVehicle vehicle)
        {
            if (!SpikeState.Operation.Active || agent.NavigationCalculationInProgress)
                return float.NaN;

            Vector3 stop;
            if (SpikeState.NavToPlayer)
            {
                // A pickup has no passenger trip point yet, but it still approaches
                // an exact supervised stop and must not retain cruise speed there.
                if (!SpikeState.PollingActive)
                    return float.NaN;
                stop = SpikeState.NavTarget;
            }
            else
            {
                if (!SpikeState.RideActive || SpikeState.RideAwaitingDestination ||
                    SpikeState.ActiveTripPoint is not { } passengerStop ||
                    !passengerStop.Equals(SpikeState.NavTarget))
                    return float.NaN;
                stop = passengerStop;
            }
            if (!stop.Equals(agent.TargetLocation))
                return float.NaN;

            return Vector3.Distance(vehicle.transform.position, stop);
        }

        internal static void Remove()
        {
            _enabled = false;
            try { _harmony?.UnpatchSelf(); }
            catch (Exception ex) { Mod.Log.Warn($"[cruise] unpatch failed; disabled hook remains inert: {ex.Message}"); }
            finally
            {
                _harmony = null;
                _errorLogged = false;
                _loggedOrder = -1;
                _loggedApproachOrder = -1;
            }
        }
    }
    // ==== end TaxiDriver section: CruiseSpeedPatch.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: NativeActuatorPatch.cs ====
    using System;
    using System.Linq;
    using System.Text;
    using HarmonyLib;
    using Il2CppScheduleOne.Vehicles;
    using S1Mods.Shared;
    using UnityEngine;


    /// <summary>
    /// Installed f12: FixedUpdate calls virtual slot 61 / ApplyThrottle (RVA 0xAB2EE0).
    /// currentThrottle and handbrakeOverride are inputs; overrideControls selects the
    /// latter. Render Update replaces currentThrottle and LateUpdate clears the request.
    /// Supply inputs ONLY for this native consumption, then restore them even on error.
    /// No steering, yaw, native reverse-output flags or wheel torques are written here.
    /// </summary>
    internal static class NativeActuatorPatch
    {
        private const string HarmonyId = "com.taxidriver.actuator";
        private static HarmonyLib.Harmony? _harmony;
        private static bool _attempted;
        internal static bool Healthy { get; private set; }
        private static IntPtr _parkPointer;
        private static IntPtr _diagnosticPointer;
        private static NativeActuatorMode _lastMode;
        private static float _nextReadback;
        private static bool _errorLogged;
        private static long _startupReadbackOperation = -1;
        private static int _startupReadbackOrder = -1;
        private static int _startupReadbackSamples;
        private static float _nextStartupReadback;

        internal static void Apply()
        {
            if (_attempted) return;
            _attempted = true;
            try
            {
                _harmony = new HarmonyLib.Harmony(HarmonyId);
                var target = AccessTools.DeclaredMethod(typeof(LandVehicle), "ApplyThrottle", Type.EmptyTypes);
                bool patched = PatchGuard.TryPatch(_harmony, target,
                    prefix: new HarmonyMethod(typeof(NativeActuatorPatch), nameof(ApplyThrottlePrefix)) { priority = Priority.Last },
                    postfix: new HarmonyMethod(typeof(NativeActuatorPatch), nameof(ApplyThrottlePostfix)) { priority = Priority.First },
                    finalizer: new HarmonyMethod(typeof(NativeActuatorPatch), nameof(ApplyThrottleFinalizer)), log: Mod.Log);
                var info = target == null ? null : HarmonyLib.Harmony.GetPatchInfo(target);
                Healthy = patched && info != null &&
                    info.Prefixes.Any(p => p.owner == HarmonyId && p.PatchMethod.Name == nameof(ApplyThrottlePrefix)) &&
                    info.Postfixes.Any(p => p.owner == HarmonyId && p.PatchMethod.Name == nameof(ApplyThrottlePostfix)) &&
                    info.Finalizers.Any(p => p.owner == HarmonyId && p.PatchMethod.Name == nameof(ApplyThrottleFinalizer));
                Mod.Log.Info($"[actuator] ApplyThrottle prefix/postfix/finalizer healthy={Healthy}; exact-owned recovery/park native inputs only. Native hook execution and physics remain unverified.");
            }
            catch (Exception ex) { Fail(ex); }
            if (!Healthy)
                Mod.Log.Error("[actuator] required consumer hook unavailable; CALL TAXI and passenger rides remain disabled.");
        }

        internal static void RequestPark(LandVehicle veh)
        {
            if (SpikeCommands.HasAuthority() && SpikeCommands.IsOwnedTaxi(veh))
                _parkPointer = veh.Pointer;
        }

        internal static void ReleasePark(LandVehicle veh)
        {
            if (veh.Pointer == _parkPointer) _parkPointer = IntPtr.Zero;
        }

        internal static void ResetIntent()
        {
            _parkPointer = IntPtr.Zero;
            _diagnosticPointer = IntPtr.Zero;
            _lastMode = NativeActuatorMode.None;
            _nextReadback = 0f;
            _startupReadbackOperation = -1;
            _startupReadbackOrder = -1;
            _startupReadbackSamples = 0;
            _nextStartupReadback = 0f;
        }

        private sealed class CallState
        {
            internal readonly LandVehicle Vehicle;
            internal NativeActuatorScope? Scope;
            internal readonly NativeActuatorMode Mode;
            internal readonly bool Diagnose;
            internal readonly long Operation;
            internal readonly int Order;
            internal readonly IntPtr Pointer;
            internal bool Completed;
            internal CallState(LandVehicle vehicle, NativeActuatorMode mode, bool diagnose)
                => (Vehicle, Mode, Diagnose, Operation, Order, Pointer) =
                    (vehicle, mode, diagnose, SpikeState.OperationId, SpikeState.NavOrder, vehicle.Pointer);
        }

        private static void ApplyThrottlePrefix(LandVehicle __instance, out CallState? __state)
        {
            __state = null;
            if (!Healthy) return;
            try
            {
                if (!SpikeCommands.HasAuthority() || Time.timeScale == 0f) return;
                var tracked = SpikeState.Vehicle;
                bool owned = __instance != null && tracked != null && __instance.Pointer != IntPtr.Zero &&
                             __instance.Pointer == tracked.Pointer && SpikeCommands.IsOwnedTaxi(__instance);
                if (!owned) return;
                bool active = SpikeRunner.HasActuatorOrder;
                bool playerDriver = __instance!.DriverPlayer != null;
                // Explicit park intent is pointer-scoped and discarded when control changes.
                if (active || playerDriver) ReleasePark(__instance);
                bool parkIntent = _parkPointer == __instance.Pointer;
                var mode = NativeActuatorPolicy.Choose(SpikeCommands.HasAuthority(), owned,
                    Time.timeScale == 0f, playerDriver,
                    SpikeState.Reverse.IsCurrentFor(SpikeState.OperationId, SpikeState.NavOrder, Time.unscaledTime),
                    active, parkIntent);
                bool diagnose = _diagnosticPointer != __instance.Pointer || _lastMode != mode ||
                                Time.unscaledTime >= _nextReadback;
                _diagnosticPointer = __instance.Pointer;
                _lastMode = mode;
                if (diagnose) _nextReadback = Time.unscaledTime + 5f;
                if (mode == NativeActuatorMode.None)
                {
                    // The old None log ran before the consumer and could show a stale park
                    // output. Sample both sides of this call, without acquiring a control
                    // scope or writing inputs. Bound cost to 8 samples in the first 3 s.
                    if (_startupReadbackOperation != SpikeState.OperationId || _startupReadbackOrder != SpikeState.NavOrder)
                    {
                        _startupReadbackOperation = SpikeState.OperationId;
                        _startupReadbackOrder = SpikeState.NavOrder;
                        _startupReadbackSamples = 0;
                        _nextStartupReadback = 0f;
                    }
                    float age = Time.unscaledTime - SpikeState.NavStartTime;
                    bool startupSample = active && !playerDriver && SpikeState.PollingActive &&
                        SpikeState.Operation.Active && Mod.Lifecycle.AcceptingWork && age >= 0f && age <= 3f &&
                        _startupReadbackSamples < 8 && Time.unscaledTime >= _nextStartupReadback;
                    if (startupSample)
                    {
                        _startupReadbackSamples++;
                        _nextStartupReadback = Time.unscaledTime + 0.5f;
                        __state = new CallState(__instance, mode, diagnose: true);
                        Readback(__instance, mode, "startup-prefix-before");
                    }
                    else if (diagnose) Readback(__instance, mode, "native-pass-through");
                    return;
                }
                if (diagnose) Readback(__instance, mode, "prefix-before");
                // Allocate/store Harmony state BEFORE any input mutation.
                __state = new CallState(__instance, mode, diagnose);
                __state.Scope = NativeActuatorScope.Begin(
                    () => (__instance.currentThrottle, __instance.handbrakeOverride, __instance.overrideControls),
                    x => __instance.currentThrottle = x, x => __instance.handbrakeOverride = x,
                    x => __instance.overrideControls = x, mode == NativeActuatorMode.Reverse);
                if (diagnose) Readback(__instance, mode, "prefix-consume");
            }
            catch (Exception ex)
            {
                Complete(__state, "prefix-error");
                Fail(ex);
            }
        }

        private static void ApplyThrottlePostfix(CallState? __state) => Complete(__state, "postfix-consumed");

        private static Exception? ApplyThrottleFinalizer(Exception? __exception, CallState? __state)
        {
            Complete(__state, __exception == null ? "finalizer" : "finalizer-exception");
            if (__exception != null && __state?.Scope != null) Fail(__exception);
            return __exception; // never suppress the native/other patch exception
        }

        private static void Complete(CallState? state, string phase)
        {
            if (state == null || state.Completed) return;
            if (state.Mode == NativeActuatorMode.None)
            {
                state.Completed = true;
                try
                {
                    if (phase == "postfix-consumed" && IsCurrentReadback(state))
                        Readback(state.Vehicle, state.Mode, phase);
                }
                catch { /* An optional diagnostic cannot disable native control. */ }
                return; // No scope, input mutation or restoration for native pass-through.
            }
            if (state.Scope == null) return;
            state.Completed = true;
            try
            {
                if (state.Diagnose) Readback(state.Vehicle, state.Mode, phase);
            }
            finally
            {
                // Do NOT re-evaluate ownership, pause, mode, host or Healthy here: the
                // exact vehicle snapshot from prefix must always get its restoration.
                bool restored = false;
                try { state.Scope?.Dispose(); restored = true; }
                catch (Exception ex) { Fail(ex); }
                if (state.Diagnose) Readback(state.Vehicle, state.Mode, restored ? "restored" : "restoration-failed");
            }
        }

        private static bool IsCurrentReadback(CallState state) =>
            Healthy && Mod.Lifecycle.AcceptingWork && Time.timeScale != 0f &&
            SpikeState.Operation.Active && SpikeState.PollingActive && SpikeRunner.HasActuatorOrder &&
            state.Operation == SpikeState.OperationId && state.Order == SpikeState.NavOrder &&
            ReferenceEquals(state.Vehicle, SpikeState.Vehicle) && state.Pointer != IntPtr.Zero &&
            state.Vehicle != null && state.Vehicle.Pointer == state.Pointer && state.Vehicle.DriverPlayer == null &&
            SpikeCommands.HasAuthority() && SpikeCommands.IsOwnedTaxi(state.Vehicle);

        private static void Readback(LandVehicle veh, NativeActuatorMode mode, string phase)
        {
            try
            {
                var agent = veh.Agent;
                var wheels = new StringBuilder();
                if (veh.wheels != null)
                {
                    for (int i = 0; i < Math.Min(veh.wheels.Count, 4); i++)
                    {
                        var collider = veh.wheels[i]?.wheelCollider;
                        if (collider != null)
                            wheels.Append($" w{i}=(ground={collider.isGrounded},motor={collider.motorTorque:0.0},brake={collider.brakeTorque:0.0})");
                    }
                }
                Mod.Log.Info($"[actuator] op={SpikeState.OperationId} order={SpikeState.NavOrder} frame={Time.frameCount} ptr={veh.Pointer} mode={mode} phase={phase} " +
                    $"auto={agent?.AutoDriving} navCalc={agent?.NavigationCalculationInProgress} pathPoints={agent?.path?.vectorPath?.Count ?? 0} targetSpeed={agent?.targetSpeed:0.0} " +
                    $"currentThrottle={veh.currentThrottle:0.00} throttleOverride={veh.throttleOverride:0.00} " +
                    $"overrideControls={veh.overrideControls} handbrakeOverride={veh.handbrakeOverride} " +
                    $"hand={veh.HandbrakeApplied} brakes={veh.BrakesApplied} vehRev={veh.IsReversing} agentRev={agent?.IsReversing} " +
                    $"speed={veh.Speed_Kmh:0.0} yaw={veh.transform.eulerAngles.y:0.0} steer={veh.CurrentSteerAngle:0.0} " +
                    $"targetSteerNormalized={agent?.targetSteerAngle_Normalized:0.00} simulated={veh.IsPhysicallySimulated} kinematic={veh.Rb?.isKinematic}{wheels}");
            }
            catch { /* Diagnostics are read-only and may not prevent restoration. */ }
        }

        private static void Fail(Exception ex)
        {
            Healthy = false;
            if (_errorLogged) return;
            _errorLogged = true;
            try { Mod.Log.Error($"[actuator] disabled on unreadable/failed control state: {ex.GetType().Name}: {ex.Message}; new service is refused."); }
            catch { }
        }

        internal static void Remove()
        {
            Healthy = false;
            ResetIntent();
            try { _harmony?.UnpatchSelf(); }
            catch (Exception ex) { Fail(ex); }
            finally { _harmony = null; _attempted = false; _errorLogged = false; }
        }
    }
    // ==== end TaxiDriver section: NativeActuatorPatch.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: TrafficLightPatch.cs ====
    using System;
    using System.Linq;
    using HarmonyLib;
    using Il2CppScheduleOne.Vehicles.AI;
    using S1Mods.Shared;


    /// <summary>
    /// Scope private flags around the two parameterless native physics consumers.
    /// Never detour GetWheelSweepCollision: its native out-parameter bridge failed live.
    /// Kinematic raycast driving is unsupported and retains vanilla obstacle handling.
    /// </summary>
    internal static class TrafficLightPatch
    {
        private const string HarmonyId = "com.taxidriver.traffic-lights";
        private static HarmonyLib.Harmony? _harmony;
        private static bool _attempted, _healthy, _errorLogged;
        private static int _loggedOrder = -1, _waitOrder = -1;
        private static IntPtr _waitVehicle;
        private static float _nextWaitProbe;
        private static bool _waiting, _waitLogged;
        private static bool _probeDisabled;

        internal static void Apply()
        {
            if (_attempted) return;
            _attempted = true;
            try
            {
                _harmony = new HarmonyLib.Harmony(HarmonyId);
                bool sweep = PatchConsumer(nameof(VehicleAgent.UpdateSweep));
                bool speed = PatchConsumer(nameof(VehicleAgent.UpdateSpeedReduction));
                _healthy = sweep && speed;
                Mod.Log.Info($"[traffic-lights] parameterless physics consumers healthy={_healthy}; exact owned autonomous taxi only; kinematic driving retains vanilla. Native crossing requires gameplay verification.");
            }
            catch (Exception ex) { Fail(ex); }
            if (!_healthy)
            {
                try { _harmony?.UnpatchSelf(); } catch { }
                Mod.Log.Warn("[traffic-lights] complete restoration hook unavailable; native behavior retained.");
            }
        }

        private static bool PatchConsumer(string name)
        {
            var target = AccessTools.DeclaredMethod(typeof(VehicleAgent), name);
            if (target == null || target.GetParameters().Length != 0 || target.ReturnType != typeof(void)) return false;
            bool patched = PatchGuard.TryPatch(_harmony!, target,
                prefix: new HarmonyMethod(typeof(TrafficLightPatch), nameof(SweepPrefix)) { priority = Priority.Last },
                postfix: new HarmonyMethod(typeof(TrafficLightPatch), nameof(SweepPostfix)) { priority = Priority.First },
                finalizer: new HarmonyMethod(typeof(TrafficLightPatch), nameof(SweepFinalizer)), log: Mod.Log);
            var info = HarmonyLib.Harmony.GetPatchInfo(target);
            return patched && info != null &&
                info.Prefixes.Any(p => p.owner == HarmonyId && p.PatchMethod.Name == nameof(SweepPrefix)) &&
                info.Postfixes.Any(p => p.owner == HarmonyId && p.PatchMethod.Name == nameof(SweepPostfix)) &&
                info.Finalizers.Any(p => p.owner == HarmonyId && p.PatchMethod.Name == nameof(SweepFinalizer));
        }

        private sealed class CallState
        {
            internal PrivateFlagsScope<DriveFlags>? Scope;
            internal VehicleAgent Agent = null!;
            internal IntPtr OriginalPointer;
            internal bool Diagnose;
        }

        private static bool Eligible(VehicleAgent __instance)
        {
            if (!_healthy || !Mod.Lifecycle.AcceptingWork || !NetworkGuard.IsInMainScene ||
                !NetworkGuard.IsAlive(__instance)) return false;
            var veh = __instance.vehicle;
            if (!SpikeCommands.IsOwnedTaxi(veh)) return false;
            bool exact = veh.Agent != null && veh.Agent.Pointer == __instance.Pointer;
            bool eligible = TrafficLightPolicy.ShouldIgnore(SpikeCommands.HasAuthority(), exact,
                SpikeRunner.HasActuatorOrder, __instance.AutoDriving, veh.DriverPlayer != null);
            var body = veh.Rb;
            bool physical = NetworkGuard.IsAlive(body) && !body.isKinematic && veh.IsPhysicallySimulated;
            return TrafficLightPolicy.CanBypass(eligible, physical,
                SpikeState.Reverse.Active || __instance.IsReversing || veh.IsReversing, UnityEngine.Time.timeScale == 0f);
        }

        private static void SweepPrefix(VehicleAgent __instance, out CallState? __state)
        {
            __state = null;
            try
            {
                if (!Eligible(__instance)) return;
                var original = __instance.Flags;
                if (original == null || original.Pointer == IntPtr.Zero || original.WasCollected) return;
                var state = new CallState
                {
                    Agent = __instance,
                    OriginalPointer = original.Pointer,
                    Diagnose = _loggedOrder != SpikeState.NavOrder
                };
                __state = state;
                BeginScope(state, original);
                if (state.Diagnose)
                {
                    _loggedOrder = SpikeState.NavOrder;
                    Breadcrumb(state, "consume-private-copy");
                }
            }
            catch (Exception ex) { Complete(__state); Fail(ex); }
        }

        private static void BeginScope(CallState state, DriveFlags original)
        {
            PrivateFlagsScope<DriveFlags>.Begin(() => original, x => state.Agent.Flags = x,
                CopyFlags, (original, copy) => original.Pointer == copy.Pointer,
                scope => state.Scope = scope);
        }

        private static DriveFlags CopyFlags(DriveFlags original) => new DriveFlags
        {
            OverrideSpeed = original.OverrideSpeed,
            OverriddenSpeed = original.OverriddenSpeed,
            OverriddenReverseSpeed = original.OverriddenReverseSpeed,
            SpeedLimitMultiplier = original.SpeedLimitMultiplier,
            IgnoreTrafficLights = true,
            UseRoads = original.UseRoads,
            StuckDetection = original.StuckDetection,
            ObstacleMode = original.ObstacleMode,
            AutoBrakeAtDestination = original.AutoBrakeAtDestination,
            TurnBasedSpeedReduction = original.TurnBasedSpeedReduction
        };

        // A slow, stationary-only diagnostic uses normal managed-to-native calls, not
        // detours on the by-ref method. Compare native hits before/after only the flag.
        internal static bool HoldRecovery(Il2CppScheduleOne.Vehicles.LandVehicle veh)
        {
            try
            {
                if (_probeDisabled) return false;
                var agent = veh.Agent;
                if (!Eligible(agent) || !float.IsFinite(veh.Speed_Kmh) || System.Math.Abs(veh.Speed_Kmh) > 1f)
                { _waiting = false; _waitLogged = false; return false; }
                float now = UnityEngine.Time.unscaledTime;
                if (_waitOrder != SpikeState.NavOrder || _waitVehicle != veh.Pointer)
                {
                    _waiting = false;
                    _waitOrder = SpikeState.NavOrder;
                    _waitVehicle = veh.Pointer;
                    _nextWaitProbe = 0f;
                }
                if (now < _nextWaitProbe) return _waiting;
                _nextWaitProbe = now + 0.25f;
                _waiting = ProbeTrafficWait(agent);
                if (_waiting && !_waitLogged)
                    Mod.Log.Info($"[traffic-lights] order={_waitOrder} native sweeps identify an exclusive traffic-light stop; bounded recovery allowance evaluated separately.");
                _waitLogged = _waiting;
                return _waiting;
            }
            catch (Exception ex)
            {
                _waiting = false;
                _probeDisabled = true;
                try { Mod.Log.Warn($"[traffic-lights] optional wait probe disabled: {ex.Message}; functional bypass unchanged unless flag restoration failed."); }
                catch { }
                return false;
            }
        }

        private static bool ProbeTrafficWait(VehicleAgent agent)
        {
            bool traffic = false, generic = false;
            bool readable = ReadSweep(agent, VehicleAgent.ESweepType.FL, ref traffic, ref generic) &&
                ReadSweep(agent, VehicleAgent.ESweepType.FR, ref traffic, ref generic);
            if (!readable || !traffic || generic) return false;
            CallState? state = null;
            bool filteredHit = true;
            try
            {
                var original = agent.Flags;
                if (original == null || original.Pointer == IntPtr.Zero || original.WasCollected) return false;
                state = new CallState { Agent = agent, OriginalPointer = original.Pointer };
                BeginScope(state, original);
                bool filteredTraffic = false, filteredGeneric = false;
                readable = ReadSweep(agent, VehicleAgent.ESweepType.FL, ref filteredTraffic, ref filteredGeneric) &&
                    ReadSweep(agent, VehicleAgent.ESweepType.FR, ref filteredTraffic, ref filteredGeneric);
                filteredHit = filteredTraffic || filteredGeneric;
            }
            finally
            {
                Complete(state);
                // Mirror postfix + finalizer cleanup for the direct probe as well:
                // a failed first disposal retains Scope and permits one safe retry.
                Complete(state);
            }
            return _healthy && TrafficLightPolicy.IsTrafficWait(readable, true, traffic, generic, filteredHit);
        }

        private static bool ReadSweep(VehicleAgent agent, VehicleAgent.ESweepType side, ref bool traffic, ref bool generic)
        {
            bool hit = agent.GetWheelSweepCollision(side, agent.vehicle.CurrentSteerAngle, false,
                out float distance, out UnityEngine.RaycastHit result);
            if (!hit) return true;
            if (!float.IsFinite(distance) || distance < 0f || !NetworkGuard.IsAlive(result.collider)) return false;
            var obstacle = result.collider.GetComponent<Il2CppScheduleOne.Vehicles.VehicleObstacle>();
            bool light = NetworkGuard.IsAlive(obstacle) &&
                obstacle.type == Il2CppScheduleOne.Vehicles.VehicleObstacle.EObstacleType.TrafficLight;
            traffic |= light;
            generic |= !light;
            return true;
        }

        private static void SweepPostfix(CallState? __state) => Complete(__state);
        private static Exception? SweepFinalizer(Exception? __exception, CallState? __state)
        {
            Complete(__state);
            return __exception;
        }

        private static void Complete(CallState? state)
        {
            if (state?.Scope == null) return;
            try
            {
                state.Scope.Dispose();
                state.Scope = null;
                if (state.Diagnose) Breadcrumb(state, "restored-original");
            }
            catch (Exception ex) { Fail(ex); }
        }

        private static void Breadcrumb(CallState state, string phase)
        {
            try
            {
                var flags = state.Agent.Flags;
                Mod.Log.Info($"[traffic-lights] order={SpikeState.NavOrder} phase={phase} originalFlags={state.OriginalPointer} currentFlags={flags?.Pointer} ignore={flags?.IgnoreTrafficLights}; only native TrafficLight hits excluded.");
            }
            catch { }
        }

        private static void Fail(Exception ex)
        {
            _healthy = false;
            _waiting = false;
            if (_errorLogged) return;
            _errorLogged = true;
            try { Mod.Log.Warn($"[traffic-lights] disabled on unreadable state: {ex.Message}; native behavior retained when restoration succeeds."); }
            catch { }
        }

        internal static void Remove()
        {
            _healthy = false;
            _waiting = false;
            _waitVehicle = IntPtr.Zero;
            _waitOrder = -1;
            _nextWaitProbe = 0f;
            _waitLogged = false;
            _probeDisabled = false;
            try { _harmony?.UnpatchSelf(); }
            catch (Exception ex) { Fail(ex); }
            finally { _harmony = null; _attempted = false; _errorLogged = false; _loggedOrder = -1; }
        }
    }
    // ==== end TaxiDriver section: TrafficLightPatch.cs ====
}
