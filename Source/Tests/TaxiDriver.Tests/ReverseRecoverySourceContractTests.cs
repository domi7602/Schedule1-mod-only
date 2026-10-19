using System;
using Xunit;

namespace TaxiDriver.Tests;

/// <summary>
/// Runtime wiring guards for the supervised recovery reverse pedal and the
/// standstill brake hold (review 2026-10-13). Unity/IL2CPP execution is NOT
/// covered — measured in-game breadcrumbs remain the runtime evidence.
/// </summary>
public class ReverseRecoverySourceContractTests
{
    private static string Section(string start, string end) => DriverSafetySourceContractTests.Section(
        DriverSafetySourceContractTests.ReadSource("SpikeRunner.cs"), start, end);

    [Fact]
    public void Update_DoesNotWritePhysicsInputsOrFakeGearOutputs()
    {
        string source = DriverSafetySourceContractTests.ReadSource("SpikeRunner.cs");
        Assert.DoesNotContain("TickReversePedal();", source);
        Assert.DoesNotContain("TickBrakeHold();", source);
        Assert.DoesNotContain("veh.currentThrottle =", source);
        Assert.DoesNotContain("veh.IsReversing =", source);
        Assert.DoesNotContain("veh.HandbrakeApplied = true;", source);
    }

    [Fact]
    public void SupervisedReverse_ArmsThePedalAndLogsMeasuredDriveState()
    {
        string reverse = Section("private static bool TryStartReverse", "private static bool TickStartupRecovery");
        Assert.Contains("agent.StartReverse();", reverse);
        Assert.Contains("SpikeState.NavReDispatchAt = now + seconds;", reverse);
        Assert.Contains("_reverseStartedAt = now;", reverse);
        Assert.Contains("DescribeDriveState(veh, agent)", reverse);
    }

    [Fact]
    public void ConsumerHook_UsesScopedInputsAndExceptionRestoration()
    {
        string mod = DriverSafetySourceContractTests.ReadSource("Mod.cs");
        Assert.Contains("NativeActuatorPatch.Apply();", mod);
        Assert.Contains("NativeActuatorPatch.Remove();", mod);
        string patch = DriverSafetySourceContractTests.ReadSource("NativeActuatorPatch.cs");
        Assert.Contains("\"ApplyThrottle\", Type.EmptyTypes", patch);
        Assert.Contains("prefix:", patch);
        Assert.Contains("postfix:", patch);
        Assert.Contains("finalizer:", patch);
        Assert.Contains("info.Finalizers.Any", patch);
        Assert.Contains("__instance.Pointer == tracked.Pointer", patch);
        Assert.Contains("SpikeCommands.HasAuthority()", patch);
        Assert.Contains("__instance!.DriverPlayer != null", patch);
        Assert.Contains("Time.timeScale == 0f", patch);
        Assert.Contains("SpikeState.Reverse.IsCurrentFor(SpikeState.OperationId, SpikeState.NavOrder, Time.unscaledTime)", patch);
        Assert.Contains("NativeActuatorScope.Begin(", patch);
        Assert.Contains("state.Scope?.Dispose();", patch);
        Assert.Contains("return __exception;", patch);
        Assert.Contains("[actuator]", patch);
        Assert.Contains("\"prefix-consume\"", patch);
        Assert.Contains("\"postfix-consumed\"", patch);
        Assert.Contains("\"restored\"", patch);
        Assert.DoesNotContain(".IsReversing =", patch);
        Assert.DoesNotContain(".motorTorque =", patch);
        Assert.DoesNotContain(".brakeTorque =", patch);
    }

    [Fact]
    public void ServiceFailsClosedIfNativeConsumerHookIsUnavailable()
    {
        string commands = DriverSafetySourceContractTests.ReadSource("SpikeCommands.cs");
        string start = DriverSafetySourceContractTests.Section(commands, "internal static bool StartRide(string caller)", "internal static void EndRide");
        string call = DriverSafetySourceContractTests.Section(commands, "internal static bool CallTaxi(string caller)", "internal static");
        Assert.Contains("!NativeActuatorPatch.Healthy", start);
        Assert.Contains("!NativeActuatorPatch.Healthy", call);
    }

    [Fact]
    public void ReverseResult_LogsMeasuredDisplacementAndPedalReadback()
    {
        string redispatch = Section("private static void TickReDispatch", "internal static void ReDispatch");
        Assert.Contains("moved ", redispatch);
        Assert.Contains("DescribeDriveState(veh, agent)", redispatch);
        Assert.True(redispatch.IndexOf("DescribeDriveState(veh, agent)", StringComparison.Ordinal) <
                    redispatch.IndexOf("agent.StopReversing();", StringComparison.Ordinal));
        Assert.Contains("escape is unproven", redispatch);
        string state = Section("private static string DescribeDriveState", "private static void TickIdleGuard()");
        Assert.Contains("thr=", state);
        Assert.Contains("autoDriving=", state);
        Assert.Contains("vehRev=", state);
    }
}
