using System.Reflection;
using System.Text.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace TaxiDriver.Tests;

/// <summary>Executes the production hooks, receipt, scope and bootstrap against substituted
/// native/Harmony boundaries. Native comparison replay follows the pinned f12 instructions.</summary>
public class NativeShortcutExecutionTests
{
    private static readonly Lazy<MethodInfo> Guard = new(() => Compile(true));
    private static readonly Lazy<MethodInfo> Observe = new(() => Compile(false));

    [Fact]
    public void NativeErrorEmptyShortcutDiscardsRoadGroupInObserveAndGuardPreservesIt()
    {
        var red = Run("empty", false);
        Assert.False(red.Success);
        Assert.False(red.Preserved);
        Assert.Equal(0, red.ResultPoints);
        Assert.Contains(red.Logs, l => l.Contains("original=0 effective=0 override=False"));
        var green = Run("empty");
        Assert.True(green.Success);
        Assert.True(green.Preserved);
        Assert.True(green.ResultPoints >= 2);
        Assert.Equal(1, green.Overrides);
        Assert.True(green.DirectError);
        Assert.Equal(0, green.DirectCount);
        Assert.Equal(0f, green.OutsideLength);
        Assert.Contains(green.Logs, l => l.Contains("nativeResult=Success"));
        Assert.True(green.Clean);
    }

    [Theory]
    [InlineData("valid-short", true, false)]
    [InlineData("valid-long", true, true)]
    [InlineData("valid-equal", true, true)]
    [InlineData("null-vector", true, true)]
    [InlineData("zero-entry", true, true)]
    [InlineData("one-point-error", false, false)]
    public void OtherNativeComparisonOutcomesRemainNative(string scenario, bool success, bool preserved)
    {
        var result = Run(scenario);
        Assert.Equal(success, result.Success);
        Assert.Equal(preserved, result.Preserved);
        Assert.Equal(0, result.Overrides);
        Assert.True(result.Clean);
    }

    [Theory]
    [InlineData("foreign")]
    [InlineData("authority")]
    [InlineData("player-driver")]
    [InlineData("not-polling")]
    [InlineData("inactive")]
    [InlineData("lifecycle")]
    [InlineData("scene")]
    [InlineData("wrong-order")]
    [InlineData("wrong-operation")]
    [InlineData("wrong-generation")]
    [InlineData("wrong-target")]
    [InlineData("wrong-seeker")]
    [InlineData("wrong-agent")]
    [InlineData("wrong-coroutine")]
    [InlineData("wrong-closure")]
    [InlineData("no-initial-hit")]
    [InlineData("duplicate-initial-hit")]
    [InlineData("late-state0")]
    [InlineData("stale-same-seekers-target")]
    [InlineData("defaults")]
    [InlineData("epoch")]
    [InlineData("state3")]
    [InlineData("missing-patch")]
    [InlineData("interacting-patch")]
    [InlineData("interacting-il-manipulator")]
    [InlineData("duplicate-own-hook")]
    public void UnboundOrNoLongerCurrentQueriesCannotAcquirePermissionAtStateTwo(string scenario)
    {
        var result = Run(scenario);
        Assert.Equal(0, result.Overrides);
        Assert.False(result.Success);
        Assert.True(result.Clean);
        Assert.Equal(0f, result.OutsideLength);
    }

    [Theory]
    [InlineData("entry-error")]
    [InlineData("road-error")]
    [InlineData("road-empty")]
    [InlineData("nan-entry")]
    [InlineData("direct-alias")]
    [InlineData("current-path-mismatch")]
    [InlineData("pathid-change")]
    [InlineData("pointer-change-same-id")]
    [InlineData("after-first-order-change")]
    [InlineData("after-first-nonce-change")]
    [InlineData("after-first-authority-loss")]
    [InlineData("after-first-epoch")]
    [InlineData("after-first-group-change")]
    [InlineData("after-first-direct-valid")]
    [InlineData("after-first-mask-health")]
    public void InvalidInputsOrChangesAtTheMutationBoundaryNeverOverride(string scenario)
    {
        var result = Run(scenario);
        Assert.Equal(0, result.Overrides);
        Assert.True(result.Clean);
        Assert.True(float.IsFinite(result.OutsideLength));
    }

    [Theory]
    [InlineData("wrong-first")]
    [InlineData("wrong-second")]
    [InlineData("recursive-getter")]
    [InlineData("nested-foreign")]
    [InlineData("nested-other-state")]
    [InlineData("nested-owned")]
    [InlineData("move-exception")]
    [InlineData("length-exception")]
    [InlineData("capture-exception")]
    [InlineData("post-exception")]
    [InlineData("log-exception")]
    [InlineData("cross-thread")]
    [InlineData("missing-native-getters")]
    [InlineData("native-entry-nan")]
    public void ScopeMaskingAndExceptionalCleanupLeaveSubsequentQueriesNative(string scenario)
    {
        var result = Run(scenario);
        Assert.True(result.Clean);
        Assert.Equal(0f, result.OutsideLength);
        if (scenario != "post-exception" && scenario != "log-exception") Assert.Equal(0, result.Overrides);
        if (scenario is "move-exception" or "length-exception") Assert.Equal("native-original", result.Exception);
    }

    [Fact]
    public void OnlyTheSecondNativeComparisonReturnCanChange()
    {
        var result = Run("repeat-direct");
        Assert.Equal(1, result.Overrides);
        Assert.Equal(0f, result.RepeatedLength);
        Assert.True(result.Success);
        Assert.True(result.Clean);
    }

    [Fact]
    public void NestedEligibleFramesCancelOuterPermissionAndInvalidateWithoutResurrection()
    {
        ShortcutComparison.Invalidate();
        var outer = ShortcutComparison.Create();
        ShortcutComparison.Push(outer);
        ShortcutComparison.Arm(outer, 1, 2, false, () => true);
        float first = 10;
        ShortcutComparison.After(ShortcutComparison.Before(1), ref first);
        var inner = ShortcutComparison.Create();
        ShortcutComparison.Push(inner);
        ShortcutComparison.Arm(inner, 3, 4, false, () => true);
        first = 5;
        ShortcutComparison.After(ShortcutComparison.Before(3), ref first);
        float direct = 0;
        Assert.True(ShortcutComparison.After(ShortcutComparison.Before(4), ref direct));
        Assert.Equal(float.PositiveInfinity, direct);
        ShortcutComparison.Close(inner);
        direct = 0;
        Assert.False(ShortcutComparison.After(ShortcutComparison.Before(2), ref direct));
        Assert.Equal(0, direct);
        ShortcutComparison.Invalidate();
        ShortcutComparison.Close(outer);
        ShortcutComparison.Close(inner);
        Assert.Null(ShortcutComparison.Current);
    }

    [Fact]
    public void OutOfOrderCleanupInvalidatesTheStackRatherThanRestoringOldPermission()
    {
        ShortcutComparison.Invalidate();
        var outer = ShortcutComparison.Create();
        ShortcutComparison.Push(outer);
        var inner = ShortcutComparison.Create();
        ShortcutComparison.Push(inner);
        ShortcutComparison.Arm(inner, 1, 2, false, () => true);
        ShortcutComparison.Close(outer);
        Assert.True(outer.Faulted);
        ShortcutComparison.Close(inner);
        Assert.Null(ShortcutComparison.Current);
        Assert.Null(ShortcutComparison.Before(1));
    }

    [Theory]
    [InlineData("incompatible")]
    [InlineData("install-0")]
    [InlineData("install-1")]
    [InlineData("install-2")]
    [InlineData("install-3")]
    [InlineData("partial-0")]
    [InlineData("partial-1")]
    [InlineData("partial-2")]
    [InlineData("partial-3")]
    public void RequiredPatchFailuresRollbackAndRemainInert(string scenario)
    {
        var result = Run(scenario);
        Assert.Equal(0, result.Overrides);
        Assert.Equal(0, result.OwnedPatches);
        Assert.False(result.Success);
    }

    [Fact]
    public void RepeatedBootstrapAndFailedRemovalUseActualOwnerReadbacks()
    {
        var repeated = Run("repeat-apply");
        Assert.Equal(4, repeated.InstallCalls);
        Assert.True(repeated.Success);
        var pending = Run("failed-remove");
        Assert.False(pending.Success);
        Assert.True(pending.OwnedPatches > 0);
        Assert.Equal(2, pending.InstallCalls); // failed partial second hook; retry never duplicates survivors
        Assert.Contains(pending.Logs, l => l.Contains("removalPending=True"));
        var recovered = Run("remove-retry");
        Assert.True(recovered.Success);
        Assert.Equal(6, recovered.InstallCalls);
    }

    // R1 ports: the native-backed getter returns its original value AFTER reentry
    // inside production Revalidate. These are not inter-getter identity mutations.
    [Theory]
    [InlineData("probe-invalidate", false)]
    [InlineData("probe-nested-state1", false)]
    [InlineData("probe-order-change", false)]
    [InlineData("probe-nonce-change", false)]
    [InlineData("probe-fault", true)]
    public void FinalValidationRevocationCannotOverride(string scenario, bool faulted)
    {
        var result = Run(scenario);
        Assert.True(result.Injected);
        Assert.Equal(0, result.Overrides); // effect oracle; clean unwind alone missed R1
        Assert.False(result.Success);
        Assert.Equal(faulted, result.FeatureFaulted);
        Assert.True(result.ValidationExclusiveObserved);
        Assert.True(result.Clean);
        Assert.Equal(0f, result.OutsideLength);
    }

    [Theory]
    [InlineData("probe-validation-reentry")]
    [InlineData("probe-validation-return-reentry")]
    [InlineData("probe-validation-direct-entry-reentry")]
    [InlineData("probe-validation-direct-return-reentry")]
    public void ValidationEntryAndReturnRejectRecursiveGettersPersistently(string scenario)
    {
        var result = Run(scenario);
        Assert.True(result.Injected);
        Assert.Equal(0, result.Overrides);
        Assert.True(result.FeatureFaulted);
        Assert.True(result.ValidationExclusiveObserved);
        Assert.True(result.Clean);
        Assert.Equal(0f, result.OutsideLength);
    }

    [Theory]
    [InlineData("late-order")]
    [InlineData("late-nonce")]
    [InlineData("late-operation")]
    [InlineData("late-generation")]
    [InlineData("late-receipt")]
    [InlineData("late-target")]
    [InlineData("late-polling")]
    [InlineData("late-active")]
    [InlineData("late-lifecycle")]
    [InlineData("late-epoch")]
    [InlineData("late-fault")]
    public void LastNativeValidationReadCannotAuthorizeARevokedManagedReceipt(string scenario)
    {
        var result = Run(scenario);
        Assert.True(result.Injected);
        Assert.Equal(0, result.Overrides);
        Assert.False(result.Success);
        Assert.Equal(scenario == "late-fault", result.FeatureFaulted);
        Assert.True(result.ValidationExclusiveObserved);
        Assert.True(result.Clean);
    }

    [Theory]
    [InlineData("native-entry-nan")]
    [InlineData("native-direct-nan")]
    public void NonFiniteNativeComparisonReturnIsAPersistentProtocolFault(string scenario)
    {
        var result = Run(scenario);
        Assert.Equal(0, result.Overrides);
        Assert.True(result.FeatureFaulted);
        Assert.True(result.Clean);
    }

    private static Outcome Run(string scenario, bool guard = true) => JsonSerializer.Deserialize<Outcome>(
        (string)(guard ? Guard : Observe).Value.Invoke(null, new object[] { scenario })!)!;
    private sealed record Outcome(bool Success, bool Preserved, int ResultPoints, int Overrides, bool DirectError,
        int DirectCount, float OutsideLength, float RepeatedLength, bool Clean, string Exception, int InstallCalls,
        int OwnedPatches, string[] Logs, bool Injected, bool FeatureFaulted, bool ValidationExclusiveObserved);

    private static MethodInfo Compile(bool guard)
    {
        var options = new CSharpParseOptions(LanguageVersion.CSharp12,
            preprocessorSymbols: guard && Environment.GetEnvironmentVariable("TAXI_SHORTCUT_RED_CONTROL") != "1"
                ? new[] { "TAXI_SHORTCUT_GUARD" } : Array.Empty<string>());
        var root = CSharpSyntaxTree.ParseText(DriverSafetySourceContractTests.ReadSource("DrivingPatches.cs"), options).GetRoot();
        var ns = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>().Single(n =>
            n.Members.OfType<ClassDeclarationSyntax>().Any(c => c.Identifier.Text == "NativeShortcutPatch"));
        var compatibility = ns.DescendantNodes().OfType<MethodDeclarationSyntax>().Single(m => m.Identifier.Text == "Compatible");
        ns = ns.ReplaceNode(compatibility, compatibility.WithBody(SyntaxFactory.ParseStatement("{ return Fixture.Compatible; }") as BlockSyntax));
        var pure = CSharpSyntaxTree.ParseText(DriverSafetySourceContractTests.ReadSource("DrivingPolicies.cs"), options).GetRoot()
            .DescendantNodes().OfType<NamespaceDeclarationSyntax>().Single(n =>
                n.Members.OfType<ClassDeclarationSyntax>().Any(c => c.Identifier.Text == "ShortcutComparison"));
        var trees = new[] { CSharpSyntaxTree.ParseText(ns.ToFullString(), options), CSharpSyntaxTree.ParseText(pure.ToFullString(), options),
            CSharpSyntaxTree.ParseText(Boundary, options) };
        var references = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator)
            .Select(p => MetadataReference.CreateFromFile(p));
        var compilation = CSharpCompilation.Create("ShortcutReplay_" + Guid.NewGuid().ToString("N"), trees, references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, nullableContextOptions: NullableContextOptions.Enable));
        using var stream = new MemoryStream();
        var emit = compilation.Emit(stream);
        Assert.True(emit.Success, string.Join(Environment.NewLine, emit.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error)));
        return Assembly.Load(stream.ToArray()).GetType("TaxiDriver.Fixture")!.GetMethod("Run")!;
    }

    private const string Boundary = """
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using UnityEngine;
using Il2CppPathfinding;
using Il2CppScheduleOne.Vehicles.AI;
using R = Il2CppScheduleOne.Vehicles.AI.NavigationUtility.__c__DisplayClass5_0.ObjectCompilerGeneratedNPrivateSealedIEnumerator1ObjectIEnumeratorIDisposableInObPaLiIn1NoInVeLiUnique;
namespace Il2CppInterop.Runtime.InteropTypes {
 public class Il2CppObjectBase { static long id=100; public IntPtr Pointer=(IntPtr)(++id); public bool WasCollected; public T? TryCast<T>() where T:class=>this as T; }
}
namespace UnityEngine {
 public record struct Vector3(float x,float y,float z);
 public class Coroutine:Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase {}
}
namespace Il2CppPathfinding {
 public enum PathCompleteState { NotCalculated, Error, Complete, Partial }
 public class Path:Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase {
  private bool _error; public bool error {get {TaxiDriver.Fixture.OnPathErrorRead();return _error;}set=>_error=value;}
  public PathCompleteState CompleteState=PathCompleteState.Complete; public ushort pathID=1;
  public List<Vector3>? vectors=new(); public bool ThrowRead;
  public List<Vector3>? vectorPath {get {if(ThrowRead)throw new Exception("read");return vectors;}set=>vectors=value;}
  public float GetTotalLength()=>TaxiDriver.Fixture.Length(this);
  public float Raw()=>vectors==null?float.PositiveInfinity:vectors.Zip(vectors.Skip(1),(a,b)=>MathF.Sqrt((a.x-b.x)*(a.x-b.x)+(a.y-b.y)*(a.y-b.y)+(a.z-b.z)*(a.z-b.z))).Sum();
 }
 public class ABPath:Path { public Vector3 originalStartPoint,originalEndPoint,startPoint,endPoint; }
 public class Seeker:Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase { public Path? Current;public Path? GetCurrentPath(){TaxiDriver.Fixture.OnCurrentPathRead();return Current;} }
}
namespace Il2CppScheduleOne.Math { public class PathSmoothingUtility { public class SmoothedPath { public List<Vector3> vectorPath=new(); } } }
namespace Il2CppScheduleOne.Vehicles {
 public class LandVehicle:Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase { public AI.VehicleAgent Agent=null!; public object? DriverPlayer; }
}
namespace Il2CppScheduleOne.Vehicles.AI {
 public class NavigationSettings:Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase { public bool endAtRoad=true,ensureProximityToGraph=true,teleportToGraphIfCalculationFails; }
 public class DriveFlags:Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase {public bool UseRoads=true;}
 public class PathGroup:Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase { public Path? startToEntryPath,entryToExitPath,exitToDestinationPath; }
 public class VehicleAgent:Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase {
  public class NavigationCallback {}
  public Il2CppScheduleOne.Vehicles.LandVehicle vehicle=null!; public DriveFlags Flags=new();public Seeker generalSeeker=new(),roadSeeker=new();
  public Coroutine? navigationCalculationRoutine;public NavigationSettings? currentNavigationSettings;public Vector3 TargetLocation;
  public bool AutoDriving,IsReversing;public bool NavigationCalculationInProgress=>navigationCalculationRoutine!=null;
  public void Navigate(Vector3 location,NavigationSettings settings,NavigationCallback callback){}
  public void NavigationCalculationCallback(NavigationUtility.ENavigationCalculationResult result,Il2CppScheduleOne.Math.PathSmoothingUtility.SmoothedPath path){}
 }
 public class NavigationUtility {
  public enum ENavigationCalculationResult { Success, Failed }
  public class __c__DisplayClass5_0:Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase {
   public Path? lastCalculatedPath; public Vector3 startPosition,destination;public Seeker generalSeeker=null!,roadSeeker=null!;
   public NavigationSettings navSettings=null!;public DriveFlags flags=null!;
   public class ObjectCompilerGeneratedNPrivateSealedIEnumerator1ObjectIEnumeratorIDisposableInObPaLiIn1NoInVeLiUnique:Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase {
    public int __1__state; public __c__DisplayClass5_0 __4__this=null!;public PathGroup? _finalGroup_5__2;
    public bool MoveNext()=>true;
   }
  }
 }
}
namespace HarmonyLib {
 public static class Priority {public const int First=800,Last=0;}
 public enum HarmonyPatchType {All}
 public class HarmonyMethod {public MethodInfo method;public int priority;public HarmonyMethod(Type t,string n){method=t.GetMethod(n,BindingFlags.Static|BindingFlags.NonPublic)!;}}
 public class Patch {public string owner="";public MethodInfo PatchMethod=null!;}
 public class Patches {public List<Patch> Prefixes=new(),Postfixes=new(),Finalizers=new(),Transpilers=new(),ILManipulators=new();public IEnumerable<string> Owners=>Prefixes.Concat(Postfixes).Concat(Finalizers).Concat(Transpilers).Concat(ILManipulators).Select(p=>p.owner);}
 public static class AccessTools {public static MethodInfo? DeclaredMethod(Type t,string n,Type[] args)=>t.GetMethod(n,args);}
 public class Harmony {
  public string Id;public Harmony(string id){Id=id;}public static Dictionary<MethodInfo,Patches> Map=new();
  public static Patches? GetPatchInfo(MethodInfo m)=>Map.TryGetValue(m,out var p)?p:null;
  public void Unpatch(MethodInfo m,HarmonyPatchType type,string owner){if(TaxiDriver.Fixture.FailRemove)throw new Exception("remove");if(Map.TryGetValue(m,out var p)){p.Prefixes.RemoveAll(x=>x.owner==owner);p.Postfixes.RemoveAll(x=>x.owner==owner);p.Finalizers.RemoveAll(x=>x.owner==owner);}}
 }
}
namespace S1Mods.Shared {
 public static class NetworkGuard {public static bool IsInMainScene=>TaxiDriver.Fixture.Scene;public static bool IsAlive(object? p)=>p!=null;}
 public static class PatchGuard {
  public static bool TryPatch(HarmonyLib.Harmony h,MethodBase original,HarmonyLib.HarmonyMethod? prefix=null,HarmonyLib.HarmonyMethod? postfix=null,HarmonyLib.HarmonyMethod? finalizer=null,object? log=null){
   int step=TaxiDriver.Fixture.InstallCalls++;bool fail=step==TaxiDriver.Fixture.FailInstall;
   if(!fail||TaxiDriver.Fixture.Partial){var p=new HarmonyLib.Patches();void Add(List<HarmonyLib.Patch> list,HarmonyLib.HarmonyMethod? m){if(m!=null)list.Add(new(){owner=h.Id,PatchMethod=m.method});}Add(p.Prefixes,prefix);Add(p.Postfixes,postfix);Add(p.Finalizers,finalizer);HarmonyLib.Harmony.Map[(MethodInfo)original]=p;}
   return !fail;
  }
 }
}
namespace TaxiDriver {
 public class Life {public bool AcceptingWork=true;}
 public class Logger {public void Info(string m){if(Fixture.ThrowLog)throw new Exception("log");Fixture.Logs.Add(m);} public void Warn(string m)=>Info(m);}
 public static class Mod {public static Life Lifecycle=new();public static Logger Log=new();}
 public class Op {public bool Active=true;public long Generation=1;}
 public class Reverse {public bool Active;}
 public static class SpikeState {public static Il2CppScheduleOne.Vehicles.LandVehicle? Vehicle;public static bool PollingActive=true;public static Op Operation=new();public static Reverse Reverse=new();public static int NavOrder=4;public static long OperationId=2;public static Vector3 NavTarget=new(100,0,0);}
 public static class SpikeCommands {public static bool HasAuthority()=>Fixture.Authority;public static bool IsOwnedTaxi(Il2CppScheduleOne.Vehicles.LandVehicle? v)=>Fixture.Owned&&v==SpikeState.Vehicle;}
 public static class TaxiAI {public static bool Active,NavigationBlocked;}
 public static class SpikeTrace {}
 public static class Fixture {
  public static bool Compatible=true,Authority=true,Owned=true,Scene=true,FailRemove,Partial,ThrowLog;
  public static int InstallCalls,FailInstall=-1;public static List<string> Logs=new();
  static VehicleAgent agent=null!;static R routine=null!;static Path direct=null!;static string scenario="";
  static bool comparison,recursing,changed;static float repeated;static Exception? thrown;static int overrides;
  static readonly Type Guard=typeof(NativeShortcutPatch);
  static bool probeInjected,validationExclusiveObserved;
  static void ObserveValidation(){
   var f=ShortcutComparison.Current;
   validationExclusiveObserved=f?.GetType().GetField("Validating",BindingFlags.Instance|BindingFlags.NonPublic)?.GetValue(f) is true;
  }
  public static void OnPathErrorRead(){
   int expected=scenario switch {"probe-validation-reentry"=>1,"probe-validation-return-reentry"=>2,"probe-validation-direct-entry-reentry"=>3,_=>4};
   if(probeInjected||ShortcutComparison.Current?.Stage!=expected||!scenario.StartsWith("probe-"))return;
   probeInjected=true;ObserveValidation();
   switch(scenario){
    case "probe-invalidate":NativeShortcutPatch.Invalidate();break;
    case "probe-order-change":SpikeState.NavOrder++;break;
    case "probe-nonce-change":IncrementNonce();break;
    case "probe-fault":Invoke("Fault","injected-context-fault",null);break;
    case "probe-validation-reentry":case "probe-validation-return-reentry":Length(routine._finalGroup_5__2!.startToEntryPath!);break;
    case "probe-validation-direct-entry-reentry":case "probe-validation-direct-return-reentry":Length(direct);break;
    case "probe-nested-state1":
     var inner=new R {__1__state=1,__4__this=routine.__4__this};var a=new object?[]{inner,null};
     Invoke("MovePrefix",a);Invoke("MovePostfix",inner,a[1]);Invoke("MoveFinalizer",inner,null,a[1]);break;
   }
  }
  static void IncrementNonce(){var n=Guard.GetField("_nonce",BindingFlags.NonPublic|BindingFlags.Static)!;n.SetValue(null,(long)n.GetValue(null)!+1);}
  public static void OnCurrentPathRead(){
   if(probeInjected||ShortcutComparison.Current?.Stage!=4||!scenario.StartsWith("late-")||scenario=="late-state0")return;
   probeInjected=true;ObserveValidation();
   switch(scenario){
    case "late-order":SpikeState.NavOrder++;break;
    case "late-nonce":IncrementNonce();break;
    case "late-operation":SpikeState.OperationId++;break;
    case "late-generation":SpikeState.Operation.Generation++;break;
    case "late-receipt":Guard.GetField("_receipt",BindingFlags.NonPublic|BindingFlags.Static)!.SetValue(null,null);break;
    case "late-target":SpikeState.NavTarget=new(999,0,0);break;
    case "late-polling":SpikeState.PollingActive=false;break;
    case "late-active":SpikeState.Operation.Active=false;break;
    case "late-lifecycle":Mod.Lifecycle.AcceptingWork=false;break;
    case "late-epoch":NativeShortcutPatch.Invalidate();break;
    case "late-fault":Invoke("Fault","injected-late-fault",null);break;
   }
  }
  static object? Invoke(string name,params object?[] args){try{return Guard.GetMethod(name,BindingFlags.NonPublic|BindingFlags.Static)!.Invoke(null,args);}catch(TargetInvocationException e){throw e.InnerException!;}}
  static void Reset(){
   probeInjected=validationExclusiveObserved=false;
   NativeShortcutPatch.Remove();foreach(var n in new[]{"_group","_hooks","_harmony","_receipt","_dispatch"})Guard.GetField(n,BindingFlags.NonPublic|BindingFlags.Static)!.SetValue(null,null);
   Guard.GetField("_fault",BindingFlags.NonPublic|BindingFlags.Static)!.SetValue(null,false);
   Guard.GetField("_logOperation",BindingFlags.NonPublic|BindingFlags.Static)!.SetValue(null,-1L);
   Guard.GetField("_lines",BindingFlags.NonPublic|BindingFlags.Static)!.SetValue(null,0);
   Compatible=Authority=Owned=Scene=true;FailRemove=Partial=ThrowLog=false;InstallCalls=0;FailInstall=-1;Logs.Clear();HarmonyLib.Harmony.Map.Clear();
   Mod.Lifecycle=new();SpikeState.Operation=new();SpikeState.Reverse=new();SpikeState.NavOrder=4;SpikeState.OperationId=2;SpikeState.NavTarget=new(100,0,0);SpikeState.PollingActive=true;
   agent=new();SpikeState.Vehicle=new(){Agent=agent};agent.vehicle=SpikeState.Vehicle;comparison=recursing=changed=false;repeated=0;thrown=null;overrides=0;
  }
  static Path P(float length,bool error=false)=>new ABPath {error=error,CompleteState=error?PathCompleteState.Error:PathCompleteState.Complete,vectors=new(){new(0,0,0),new(length,0,0)}};
  static void Initial(R r){var a=new object?[]{r,null};Invoke("MovePrefix",a);r.__1__state=1;Invoke("MovePostfix",r,a[1]);Invoke("MoveFinalizer",r,null,a[1]);}
  static R Dispatch(){
   var settings=new NavigationSettings();if(scenario=="defaults")settings.endAtRoad=false;
   var a=new object?[]{agent,SpikeState.NavTarget,settings,null};Invoke("NavigatePrefix",a);
   var r=new R {__4__this=new(){startPosition=new(1,0,0),destination=SpikeState.NavTarget,generalSeeker=agent.generalSeeker,roadSeeker=agent.roadSeeker,flags=agent.Flags,navSettings=settings}};
   agent.currentNavigationSettings=settings;agent.TargetLocation=SpikeState.NavTarget;agent.AutoDriving=true;
   if(scenario!="no-initial-hit"&&scenario!="late-state0")Initial(r);
   if(scenario=="duplicate-initial-hit"){r.__1__state=0;Initial(r);}
   agent.navigationCalculationRoutine=new();Invoke("NavigatePostfix",agent,a[3]);Invoke("NavigateFinalizer",null,a[3]);
   if(scenario=="late-state0")Initial(r);
   return r;
  }
  static void Change(){if(changed)return;changed=true;switch(scenario){
   case "after-first-order-change":SpikeState.NavOrder++;break;
   case "after-first-nonce-change":var nonce=Guard.GetField("_nonce",BindingFlags.NonPublic|BindingFlags.Static)!;nonce.SetValue(null,(long)nonce.GetValue(null)!+1);break;
   case "after-first-authority-loss":Authority=false;break;
   case "after-first-epoch":NativeShortcutPatch.Invalidate();break;
   case "after-first-group-change":routine._finalGroup_5__2=new(){startToEntryPath=routine._finalGroup_5__2!.startToEntryPath,entryToExitPath=routine._finalGroup_5__2.entryToExitPath};break;
   case "after-first-direct-valid":direct.error=false;direct.CompleteState=PathCompleteState.Complete;break;
   case "pathid-change":direct.pathID++;break;
   case "pointer-change-same-id":direct.Pointer=(IntPtr)98765;break;
   case "after-first-mask-health":HarmonyLib.Harmony.Map.Remove(typeof(Path).GetMethod("GetTotalLength")!);break;
  }}
  public static float Length(Path p){
   if(comparison&&scenario=="missing-native-getters")return p.Raw();
   var a=new object?[]{p,null};Invoke("LengthPrefix",a);float value=0;Exception? error=null;
   try{
    if(comparison&&scenario=="recursive-getter"&&!recursing){recursing=true;Length(p);}
    if(comparison&&scenario=="length-exception")throw new Exception("native-original");
    value=comparison&&((scenario=="native-entry-nan"&&p!=direct)||(scenario=="native-direct-nan"&&p==direct))?float.NaN:p.Raw();var b=new object?[]{value,a[1]};Invoke("LengthPostfix",b);value=(float)b[0]!;
    if(float.IsPositiveInfinity(value)&&p==direct&&direct.Raw()==0)overrides++;
   }catch(Exception ex){error=ex;throw;}finally{Invoke("LengthFinalizer",error,a[1]);}
   return value;
  }
  static void Nested(){
   var inner=new R {__1__state=scenario=="nested-other-state"?1:2,__4__this=routine.__4__this,_finalGroup_5__2=routine._finalGroup_5__2};
   if(scenario=="nested-owned")inner=routine;
   var a=new object?[]{inner,null};Invoke("MovePrefix",a);Length(direct);Invoke("MovePostfix",inner,a[1]);Invoke("MoveFinalizer",inner,null,a[1]);
  }
  public static string Run(string input){
   Reset();scenario=input;
   if(input=="incompatible")Compatible=false;
   if(input.StartsWith("install-")||input.StartsWith("partial-")){FailInstall=int.Parse(input.Split('-')[1]);Partial=input.StartsWith("partial-");}
   if(input is "failed-remove" or "remove-retry"){FailInstall=1;Partial=FailRemove=true;}
   NativeShortcutPatch.Apply();
   if(input=="repeat-apply")NativeShortcutPatch.Apply();
   if(input is "failed-remove" or "remove-retry"){
    NativeShortcutPatch.Apply();if(input=="remove-retry"){FailRemove=false;FailInstall=-1;NativeShortcutPatch.Apply();}
   }
   routine=Dispatch();if(input=="stale-same-seekers-target"){var stale=routine;Dispatch();routine=stale;}
   routine.__1__state=2;var original=new PathGroup{startToEntryPath=P(10),entryToExitPath=P(100)};routine._finalGroup_5__2=original;
   direct=P(0,true);direct.vectors!.Clear();direct.pathID=130;routine.__4__this.lastCalculatedPath=direct;agent.generalSeeker.Current=direct;
   switch(input){
    case "valid-short":direct.vectors=new(){new(0,0,0),new(2,0,0)};direct.error=false;direct.CompleteState=PathCompleteState.Complete;break;
    case "valid-long":direct.vectors=new(){new(0,0,0),new(20,0,0)};direct.error=false;direct.CompleteState=PathCompleteState.Complete;break;
    case "valid-equal":direct.vectors=new(){new(0,0,0),new(10,0,0)};direct.error=false;direct.CompleteState=PathCompleteState.Complete;break;
    case "null-vector":direct.vectors=null;break;
    case "one-point-error":direct.vectors=new(){new(0,0,0)};break;
    case "zero-entry":original.startToEntryPath=P(0);break;
    case "entry-error":original.startToEntryPath!.error=true;break;
    case "road-error":original.entryToExitPath!.error=true;break;
    case "road-empty":original.entryToExitPath!.vectors!.Clear();break;
    case "nan-entry":original.startToEntryPath=P(float.NaN);break;
    case "direct-alias":original.startToEntryPath=direct;break;
    case "current-path-mismatch":agent.generalSeeker.Current=P(2);break;
    case "foreign":Owned=false;break;
    case "authority":Authority=false;break;
    case "player-driver":agent.vehicle.DriverPlayer=new();break;
    case "not-polling":SpikeState.PollingActive=false;break;
    case "inactive":SpikeState.Operation.Active=false;break;
    case "lifecycle":Mod.Lifecycle.AcceptingWork=false;break;
    case "scene":Scene=false;break;
    case "wrong-order":SpikeState.NavOrder++;break;
    case "wrong-operation":SpikeState.OperationId++;break;
    case "wrong-generation":SpikeState.Operation.Generation++;break;
    case "wrong-target":SpikeState.NavTarget=new(999,0,0);break;
    case "wrong-seeker":agent.generalSeeker=new(){Current=direct};break;
    case "wrong-agent":agent.vehicle.Agent=new();break;
    case "wrong-coroutine":agent.navigationCalculationRoutine=new();break;
    case "wrong-closure":routine.__4__this=new(){generalSeeker=agent.generalSeeker,roadSeeker=agent.roadSeeker,lastCalculatedPath=direct};break;
    case "epoch":NativeShortcutPatch.Invalidate();break;
    case "state3":routine.__1__state=3;break;
    case "missing-patch":HarmonyLib.Harmony.Map.Remove(typeof(Path).GetMethod("GetTotalLength")!);break;
    case "interacting-patch":HarmonyLib.Harmony.Map[typeof(Path).GetMethod("GetTotalLength")!].Prefixes.Add(new(){owner="foreign",PatchMethod=typeof(Fixture).GetMethod("Run")!});break;
    case "interacting-il-manipulator":HarmonyLib.Harmony.Map[typeof(Path).GetMethod("GetTotalLength")!].ILManipulators.Add(new(){owner="foreign",PatchMethod=typeof(Fixture).GetMethod("Run")!});break;
    case "duplicate-own-hook":var patches=HarmonyLib.Harmony.Map[typeof(Path).GetMethod("GetTotalLength")!];patches.Prefixes.Add(patches.Prefixes[0]);break;
    case "capture-exception":original.startToEntryPath!.ThrowRead=true;break;
    case "log-exception":ThrowLog=true;break;
   }
   var args=new object?[]{routine,null};bool success=false;int count=0;Exception? exception=null;
   void RunMove(){
    Invoke("MovePrefix",args);comparison=true;routine.__1__state=-1;
    try{
     if(input=="move-exception")throw new Exception("native-original");
     if(input.StartsWith("nested-"))Nested();
     if(input=="wrong-first")Length(original.entryToExitPath!);
     float first=Length(routine._finalGroup_5__2!.startToEntryPath!);Change();
     if(input=="wrong-second")Length(original.entryToExitPath!);
     float second=Length(direct);
     if(first>second)routine._finalGroup_5__2=new(){startToEntryPath=direct};
     if(input=="repeat-direct")repeated=Length(direct);
     var g=routine._finalGroup_5__2!;count=(g.startToEntryPath?.vectors?.Count??0)+(g.entryToExitPath?.vectors?.Count??0)+(g.exitToDestinationPath?.vectors?.Count??0);
     success=count>=2;var path=success?new Il2CppScheduleOne.Math.PathSmoothingUtility.SmoothedPath {vectorPath=new(){new(0,0,0),new(100,0,0)}}:null;
     Invoke("ResultPrefix",agent,success?NavigationUtility.ENavigationCalculationResult.Success:NavigationUtility.ENavigationCalculationResult.Failed,path);
     agent.navigationCalculationRoutine=null;
     if(input=="post-exception")routine._finalGroup_5__2!.startToEntryPath!.ThrowRead=true;
     comparison=false;Invoke("MovePostfix",routine,args[1]);
    }catch(Exception ex){exception=ex;}finally{comparison=false;Invoke("MoveFinalizer",routine,exception,args[1]);Invoke("MoveFinalizer",routine,exception,args[1]);}
   }
   if(input=="cross-thread"){var t=new System.Threading.Thread(RunMove);t.Start();t.Join();}else RunMove();
   bool clean=ShortcutComparison.Current==null;float outside=direct.GetTotalLength();
   return JsonSerializer.Serialize(new{Success=success,Preserved=ReferenceEquals(original,routine._finalGroup_5__2),ResultPoints=count,Overrides=overrides,
    DirectError=direct.error,DirectCount=direct.vectors?.Count??-1,OutsideLength=float.IsFinite(outside)?outside:-1,RepeatedLength=repeated,Clean=clean,
     Exception=exception?.Message??"",InstallCalls,Injected=probeInjected,ValidationExclusiveObserved=validationExclusiveObserved,
     FeatureFaulted=(bool)Guard.GetField("_fault",BindingFlags.NonPublic|BindingFlags.Static)!.GetValue(null)!,
     OwnedPatches=HarmonyLib.Harmony.Map.Values.Sum(p=>p.Owners.Count(o=>o=="com.taxidriver.native-shortcut")),Logs=Logs.ToArray()});
  }
 }
}
""";
}
