using System;
using Il2CppScheduleOne.NPCs;
using Il2CppScheduleOne.Vehicles;
using Il2CppScheduleOne.Vehicles.AI;
using UnityEngine;

namespace TaxiDriver;

/// <summary>
/// Mutable state of the feasibility spike. Everything lives here so the console
/// command, the per-frame runner and the F6 automation share one source of truth.
/// </summary>
internal static class SpikeState
{
    /// <summary>The spike vehicle created by `taxi spawn`.</summary>
    internal static LandVehicle? Vehicle;

    /// <summary>The NPC that `taxi npc` put into the vehicle.</summary>
    internal static NPC? DriverNpc;

    /// <summary>Vehicle code used for the last successful spawn.</summary>
    internal static string? LastVehicleCode;

    /// <summary>World position the last spawn used (after ground snapping).</summary>
    internal static Vector3 LastSpawnPosition;

    // ---- Navigation polling (taxi go) ----
    internal static bool PollingActive;
    internal static Vector3 NavTarget;
    internal static Vector3 NavStartPosition;
    internal static float NavStartDistance;
    internal static float NavStartTime;
    internal static float LastHeartbeat;
    internal static bool NavEverAutoDriving;

    /// <summary>True after the settings=null Navigate retry fired (A/B test).</summary>
    internal static bool NavRetried;

    /// <summary><see cref="Time.unscaledTime"/> of the settings=null retry, 0 when it never fired.</summary>
    internal static float NavRetryAt;

    /// <summary>Keeps the managed navigation callback alive for the whole run (IL2CPP delegate).</summary>
    internal static Action<VehicleAgent.ENavigationResult>? NavCallback;

    /// <summary>Last <c>ENavigationResult</c> reported by the callback, null when none arrived.</summary>
    internal static string? NavCallbackResult;

    // ---- Deferred respawn (freeze guard, review B2/M1) ----
    /// <summary>Code of a spawn that is waiting for its own tick (never destroy + spawn in one frame).</summary>
    internal static string? PendingSpawnCode;

    /// <summary><see cref="Time.unscaledTime"/> at which the deferred spawn may run, 0 when none.</summary>
    internal static float PendingSpawnAt;

    /// <summary>Earliest frame the deferred spawn may run (a frame gap is the minimum separation).</summary>
    internal static int PendingSpawnFrame;

    // ---- Re-enter cooldown (freeze guard, review B2) ----
    /// <summary>Last NPC that was boarded — re-entering the same NPC too fast is the freeze pattern.</summary>
    internal static NPC? LastBoardedNpc;

    /// <summary><see cref="Time.unscaledTime"/> of the last successful boarding.</summary>
    internal static float LastBoardedAt;

    // ---- F6 automation ----
    internal static bool AutoRunning;
    internal static int AutoStep;
    internal static float AutoNextAt;
    internal static string? AutoCode;

    /// <summary>F11 run: skip the npc step to test Navigate without an occupant.</summary>
    internal static bool SkipNpcStep;

    /// <summary>F15 run: absolute road target for step 3 (null = blind forward*40).</summary>
    internal static Vector3? AutoTarget;

    // ---- Stage 3b: fixed taxi stand + call-taxi flow (F17) ----
    /// <summary>
    /// One-shot spawn override: when set, the next <c>SpawnVehicle</c> places the
    /// vehicle HERE (the taxi stand) instead of 6 m in front of the player.
    /// Deliberately NOT cleared by <see cref="Reset"/>/<see cref="ResetAutoRun"/>:
    /// an F17 run whose old vehicle is being torn down must keep the stand across
    /// the deferred respawn. Consumed by <c>SpawnVehicle</c>; the F6/F11/F15
    /// handlers clear it explicitly so no other flow can inherit it.
    /// </summary>
    internal static Vector3? StandSpawnPosition;

    /// <summary>Forward direction for the stand spawn (parking-spot alignment).</summary>
    internal static Vector3 StandSpawnForward = Vector3.forward;

    /// <summary>F17 run: step 3 navigates to a road point near the PLAYER (not to <see cref="AutoTarget"/>).</summary>
    internal static bool AutoToPlayer;

    /// <summary>The navigation currently in flight targets the player — gates the "taxi arrived at player" log.</summary>
    internal static bool NavToPlayer;

    // ---- Stage 2: visible model swap (taxi visual) ----
    /// <summary>Swap the vanilla visuals for <c>taxi.glb</c> on the next spawn (default on).</summary>
    internal static bool VisualSwapEnabled = true;

    /// <summary>Translate the GLB so its bounds sit where the vanilla visual sat (default on).</summary>
    internal static bool VisualAutoAlign = true;

    /// <summary>GLB root currently attached to the spike vehicle (single swap per spawn).</summary>
    internal static GameObject? VisualRoot;

    /// <summary>How many successful swaps this session produced (telemetry for <c>taxi visual</c>).</summary>
    internal static int VisualSwaps;

    internal static void ResetNavigation()
    {
        PollingActive = false;
        NavTarget = Vector3.zero;
        NavStartPosition = Vector3.zero;
        NavStartDistance = 0f;
        NavStartTime = 0f;
        LastHeartbeat = 0f;
        NavEverAutoDriving = false;
        NavRetried = false;
        NavRetryAt = 0f;
        NavCallback = null;
        NavCallbackResult = null;
        NavToPlayer = false;
    }

    internal static void ResetAutoRun()
    {
        AutoRunning = false;
        AutoStep = 0;
        AutoNextAt = 0f;
        AutoCode = null;
        // Review M1-2: a leftover absolute target would silently redirect the NEXT
        // run (F11 used to inherit F15's road target), so every teardown clears it.
        AutoTarget = null;
        // Stage 3b: same rule for the call-taxi flag — a finished F17 run must not
        // turn the next F6/F15 step 3 into a "drive to the player" dispatch.
        // StandSpawnPosition intentionally NOT reset (see its doc: it has to
        // survive the cleanup + deferred respawn of its own run).
        AutoToPlayer = false;
        // SkipNpcStep intentionally NOT reset: a deferred cleanup inside an F11
        // run (existing vehicle) must not cancel the skip for the respawn.
        // F6/F11/F15 set it explicitly where needed.
    }

    /// <summary>Cancels a deferred respawn (used by `taxi cleanup`).</summary>
    internal static void CancelPendingSpawn()
    {
        PendingSpawnCode = null;
        PendingSpawnAt = 0f;
        PendingSpawnFrame = 0;
    }

    internal static void Reset()
    {
        Vehicle = null;
        DriverNpc = null;
        LastVehicleCode = null;
        LastSpawnPosition = Vector3.zero;
        VisualRoot = null;
        CancelPendingSpawn();
        ResetNavigation();
        ResetAutoRun();
        // LastBoardedNpc/LastBoardedAt deliberately survive: the re-enter cooldown
        // protects against the repeat-EnterVehicle freeze even across a respawn.
    }
}
