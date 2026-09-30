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

    /// <summary>
    /// Unscaled time of the last player exit (Paket E, 2026-09-29): rapid E in/out
    /// churn is debounced — the seat teleports shove the car via collision resolution.
    /// </summary>
    internal static float LastExitAt;

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

    /// <summary>F3 run: absolute road target for step 3 (null = blind forward*40).</summary>
    internal static Vector3? AutoTarget;

    // ---- Stage 3b: fixed taxi stand + call-taxi flow (F5) ----
    /// <summary>
    /// One-shot spawn override: when set, the next <c>SpawnVehicle</c> places the
    /// vehicle HERE (the taxi stand) instead of 6 m in front of the player.
    /// Deliberately NOT cleared by <see cref="Reset"/>/<see cref="ResetAutoRun"/>:
    /// an F5 run whose old vehicle is being torn down must keep the stand across
    /// the deferred respawn. Consumed by <c>SpawnVehicle</c>; the F3/F6/F11
    /// handlers clear it explicitly so no other flow can inherit it.
    /// </summary>
    internal static Vector3? StandSpawnPosition;

    /// <summary>Forward direction for the stand spawn (parking-spot alignment).</summary>
    internal static Vector3 StandSpawnForward = Vector3.forward;

    /// <summary>F5 run: step 3 navigates to a road point near the PLAYER (not to <see cref="AutoTarget"/>).</summary>
    internal static bool AutoToPlayer;

    /// <summary>The navigation currently in flight targets the player — gates the "taxi arrived at player" log.</summary>
    internal static bool NavToPlayer;

    // ---- Stage 2: taxi visual swap ----
    /// <summary>
    /// Swap the vanilla visuals for <c>taxi.glb</c> on the next spawn.
    /// Default ON (0.3.0: source default verified `= true`; the 0.2.0 session
    /// that behaved default-off ran a stale deploy). F4 stays the diagnostic toggle.
    /// </summary>
    internal static bool VisualSwapEnabled = true;

    /// <summary>Translate the GLB so its bounds sit where the vanilla visual sat (default on).</summary>
    internal static bool VisualAutoAlign = true;

    /// <summary>GLB root currently attached to the spike vehicle (single swap per spawn).</summary>
    internal static GameObject? VisualRoot;

    /// <summary>How many successful swaps this session produced (telemetry for <c>taxi visual</c>).</summary>
    internal static int VisualSwaps;

    // ---- Stage 3c: passenger mode (ride) ----

    /// <summary>Display name shown while nothing has been picked (Paket A, 2026-09-29).</summary>
    internal const string NoDestinationName = "none selected";

    /// <summary>
    /// Destination picked in the TaxiApp (ROAD A / ROAD B / STAND), written by
    /// <see cref="SpikeCommands.SetRideDestination"/>. Null while nothing is picked:
    /// since Paket A the ride REFUSES to drive without an explicit pick (Dominik:
    /// "kein checkpoint ausgewählt und es ist zum letzten gespeicherten gefahren").
    /// Survives rides within a session (picker state) — but NOT save reloads:
    /// <see cref="ResetPicker"/> runs at every save-load boundary.
    /// </summary>
    internal static Vector3? RideDestination;

    /// <summary>Display name of the picked destination (status label + logs).</summary>
    internal static string RideDestinationName = NoDestinationName;

    /// <summary>
    /// A destination has been picked in this save session. Gates <c>StartRide</c>:
    /// without a pick the taxi waits instead of driving to a stale/default target.
    /// </summary>
    internal static bool RideDestinationPicked;

    /// <summary>
    /// The player is aboard but no destination is picked yet — the ride starts the
    /// moment the picker (or <c>taxi to</c>) supplies one. Gates the arrival verdict.
    /// </summary>
    internal static bool RideAwaitingDestination;

    /// <summary>
    /// The call-taxi pickup reached the player (or the last ride ended with an
    /// exit): the next board (the game's own E-enter or F9) starts a passenger
    /// ride via <see cref="SpikeCommands.StartRide"/>.
    /// </summary>
    internal static bool RideAwaitingBoard;

    /// <summary>A passenger ride is running: the player is aboard and the NPC drives to <see cref="RideDestination"/>.</summary>
    internal static bool RideActive;

    /// <summary>The player has boarded for this ride (seat fix + status label).</summary>
    internal static bool RideBoarded;

    /// <summary>The ride navigation has finished — "Arrived — press E to exit".</summary>
    internal static bool RideArrived;

    /// <summary>
    /// Ride locks engaged: player car inputs are zeroed (PatchGuard prefixes on
    /// <c>LandVehicle.UpdateThrottle</c>/<c>UpdateSteerAngle</c> +
    /// <c>GameInput.OnVehicleHandbrake</c>) and the trunk is locked
    /// (<c>StorageDoorAnimation.Open</c>/<c>SetIsOpen(true)</c> gate +
    /// trunk/Storage <c>InteractableObject</c> components disabled). Released on
    /// STOP / exit / arrival+exit.
    /// </summary>
    internal static bool RidePassengerMode;

    // ---------------------------------------------------- Stage 3d destination

    /// <summary>Kind of the chosen destination ("deal" / "lot") — Stage 3d.</summary>
    internal static string RideDestinationKind = string.Empty;

    /// <summary>The place the player picked (the drop-off may sit elsewhere).</summary>
    internal static Vector3? RideGoal;

    /// <summary>Drop-off rule that won ("direct" / "lot entry" / "direct (loose)").</summary>
    internal static string RideDropOff = string.Empty;

    /// <summary>Lot the ride drops off at (empty unless the rule picked a lot).</summary>
    internal static string RideDropOffLot = string.Empty;

    /// <summary>True when the progress supervision gave up on the current dispatch.</summary>
    internal static bool NavGaveUp;

    // ------------------------------------------------ Stage 3d progress watchdog

    /// <summary>Position the current progress window started from.</summary>
    internal static Vector3 ProgressFrom;

    /// <summary><see cref="Time.unscaledTime"/> when the current progress window started.</summary>
    internal static float ProgressWindowStart;

    /// <summary>Recoveries already spent on the current dispatch (0 = none).</summary>
    internal static int StuckRecoveries;

    /// <summary>Re-dispatch scheduled after a reverse manoeuvre (0 = none pending).</summary>
    internal static float NavReDispatchAt;

    /// <summary>Target the pending re-dispatch must use.</summary>
    internal static Vector3 NavReDispatchTarget;

    /// <summary><see cref="Time.unscaledTime"/> of the last ride heartbeat (E, Stage 3d).</summary>
    internal static float RideHeartbeatAt;

    // ---- 0.3.0 float fix: one-shot "after settle" diagnostic ----
    /// <summary><see cref="Time.unscaledTime"/> at which the settle diagnostic runs, 0 when none is armed.</summary>
    internal static float SettleCheckAt;

    /// <summary>Vehicle the settle diagnostic reports for.</summary>
    internal static LandVehicle? SettleCheckVehicle;

    /// <summary>Root Y exactly as <c>SpawnAndReturnVehicle</c> placed it (before the boundingBox ground-snap fix).</summary>
    internal static float SettleCheckSpawnY;

    /// <summary>Raycast ground Y of the spawn.</summary>
    internal static float SettleCheckHitY;

    /// <summary>Root Y after the boundingBox ground-snap fix — the settle log proves it does not drift.</summary>
    internal static float SettleCheckRootY;

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
        NavGaveUp = false;
        ProgressFrom = Vector3.zero;
        ProgressWindowStart = 0f;
        StuckRecoveries = 0;
        NavReDispatchAt = 0f;
        NavReDispatchTarget = Vector3.zero;
    }

    internal static void ResetAutoRun()
    {
        AutoRunning = false;
        AutoStep = 0;
        AutoNextAt = 0f;
        AutoCode = null;
        // Review M1-2: a leftover absolute target would silently redirect the NEXT
        // run (F11 used to inherit F3's road target), so every teardown clears it.
        AutoTarget = null;
        // Stage 3b: same rule for the call-taxi flag — a finished F5 run must not
        // turn the next F3/F6 step 3 into a "drive to the player" dispatch.
        // StandSpawnPosition intentionally NOT reset (see its doc: it has to
        // survive the cleanup + deferred respawn of its own run).
        AutoToPlayer = false;
        // SkipNpcStep intentionally NOT reset: a deferred cleanup inside an F11
        // run (existing vehicle) must not cancel the skip for the respawn.
        // F3/F6/F11 set it explicitly where needed.
    }

    /// <summary>
    /// Clears the Stage-3c ride state (not the destination picker: that is UI
    /// state and survives). The matching lock side effects (trunk interactables)
    /// live in <c>RideLocks.UnlockTrunk</c> — callers use
    /// <see cref="SpikeCommands.EndRide"/> when those have to run as well.
    /// </summary>
    internal static void ResetRide()
    {
        RideActive = false;
        RideBoarded = false;
        RideArrived = false;
        RideAwaitingBoard = false;
        RidePassengerMode = false;
        RideAwaitingDestination = false;
    }

    /// <summary>
    /// Clears the destination picker (Paket A). Called at every save-load boundary
    /// (<c>GameLifecycle.OnSaveInfoLoaded</c>, scene leave of "Main") — the stale
    /// picker state was the "taxi drives to the last checkpoint after reload" bug.
    /// Deliberately NOT part of <see cref="Reset"/>: within one save session the
    /// pick is a convenience that survives rides.
    /// </summary>
    internal static void ResetPicker()
    {
        RideDestination = null;
        RideGoal = null;
        RideDestinationName = NoDestinationName;
        RideDestinationKind = string.Empty;
        RideDropOff = string.Empty;
        RideDropOffLot = string.Empty;
        RideDestinationPicked = false;
        RideAwaitingDestination = false;
    }

    /// <summary>
    /// Paket D (2026-09-29, "nach Checkpoint-Auswahl bleibt immer der letzte markiert"):
    /// user-facing clear — drops the picked destination and its mark. Unlike
    /// <see cref="ResetPicker"/> (the save-load reset) a ride that is still WAITING for
    /// its first pick keeps waiting, and a running drive keeps its dispatched target
    /// (only the picker/marker state is cleared).
    /// </summary>
    internal static void ClearDestination()
    {
        bool keepWaiting = RideActive && !RideDestinationPicked;
        ResetPicker();
        if (keepWaiting)
            RideAwaitingDestination = true;
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
        SettleCheckAt = 0f;
        SettleCheckVehicle = null;
        CancelPendingSpawn();
        ResetNavigation();
        ResetAutoRun();
        ResetRide();
        // LastBoardedNpc/LastBoardedAt deliberately survive: the re-enter cooldown
        // protects against the repeat-EnterVehicle freeze even across a respawn.
    }
}
