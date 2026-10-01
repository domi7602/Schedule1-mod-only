using System;
using System.Globalization;
using Il2CppFishNet;
using Il2CppScheduleOne.NPCs;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppScheduleOne.Vehicles;
using Il2CppScheduleOne.Vehicles.AI;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine;

namespace TaxiDriver;

/// <summary>
/// The spike itself: spawn a vehicle, seat an NPC, drive A→B with
/// <c>VehicleAgent.Navigate</c>, and put the local player in / out of the car.
/// Every native call is wrapped so a failure names the exact game method.
/// </summary>
internal static class SpikeCommands
{
    private const float SpawnForwardMeters = 6f;
    private const float GroundRaycastDistance = 30f;
    private const float GroundRaycastUpOffset = 3f;
    private const float DefaultRideDistance = 40f;

    /// <summary>Gap between destroying and respawning the spike vehicle (freeze guard).</summary>
    private const float FreezeGuardRespawnDelaySeconds = 0.5f;

    /// <summary>Minimum age of the last boarding before the same NPC may enter again (freeze guard).</summary>
    private const float ReEnterCooldownSeconds = 15f;

    /// <summary>Delay between a spawn and the one-shot [settle] diagnostic (<c>SpikeRunner.TickSettleLog</c>).</summary>
    internal const float SettleCheckDelaySeconds = 2f;

    /// <summary>
    /// Proven vanilla Navigate road target A (Success in round 10) — the F1/F3
    /// target and the default ride destination (ROAD A).
    /// </summary>
    internal static readonly Vector3 RideTargetRoadA = new(-131.4f, -4.0f, 51.9f);

    /// <summary>Proven vanilla Navigate road target B (Success twice in round 10) — the F2 target (ROAD B).</summary>
    internal static readonly Vector3 RideTargetRoadB = new(-17.1f, 0f, 13.4f);

    /// <summary>
    /// Console answers. Fallback: S1API exposes no <c>ConsoleHelper.Print</c> in the
    /// deployed 3.x assembly, so answers go to the MelonLoader console through the
    /// shared logger (which already prefixes [TaxiDriver]).
    /// </summary>
    internal static void Print(string message) => Mod.Log.Info(message);

    // ---------------------------------------------------------------- guards

    /// <summary>
    /// Spawning is a server-authoritative operation. In singleplayer the local
    /// client *is* the host, so this only fails when no save/host is up yet.
    /// </summary>
    internal static bool EnsureAuthority()
    {
        bool isServer;
        try
        {
            isServer = InstanceFinder.IsServer;
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"InstanceFinder.IsServer threw while checking server authority: {ex.Message}");
            return false;
        }

        if (isServer)
            return true;

        Mod.Log.Error(
            "InstanceFinder.IsServer == false — VehicleManager.SpawnAndReturnVehicle needs server authority. " +
            "In singleplayer the local client is the host, so this means no save/host is loaded yet: load a save and retry.");
        return false;
    }

    // ----------------------------------------------------------------- help

    internal static void PrintHelp()
    {
        Print("TaxiDriver spike commands (Stage 1 spike + Stage 2 visual swap + Stage 3 driver ride + Stage 3b taxi stand / call-taxi):");
        Print("  NOTE: the MelonLoader console is log-only (no input field) — all `taxi` commands are");
        Print("        output-only; control the spike via the F1-F12 hotkeys listed at the end.");
        Print("  taxi help              - this list");
        Print("  taxi codes             - vehicle code catalog (VehicleManager.VehiclePrefabs)");
        Print("  taxi spawn [code]      - spawn 6 m in front of the player, snapped to the ground (playerOwned=true)");
        Print("  taxi npc               - nearest living NPC enters the spike vehicle, then prints the one-line seat proof");
        Print("                           (OccupantNPCs slot + measured NPC root-to-seat distances, only claimed when both agree)");
        Print("  taxi ride              - local player enters the spike vehicle; dumps the seat state again as [after ride]");
        Print("                           (Player occupancy flip + NPC root-to-seat + occupant slot)");
        Print("  taxi out               - local player leaves the spike vehicle");
        Print("  taxi go [distance=40]  - VehicleAgent.Navigate() with NavigationSettings (vehicle released first: ExitPark/brakes)");
        Print("  taxi go2 [distance=40] - same but with settings=null (A/B comparison)");
        Print("  taxi stop              - VehicleAgent.StopNavigating() + end polling");
        Print("  taxi status            - dump all spike state (incl. F6 automation: AutoTarget + SkipNpcStep)");
        Print("  taxi cleanup           - everyone out + LandVehicle.DestroyVehicle() (guarded: verified exit, no NPC occupants)");
        Print("  taxi probe             - Navigate preconditions for ALL vehicles: Flags/Seekers/graph sample/ownership");
        Print("  taxi trace [on|off]    - Harmony trace of Navigate, CalculatePath, NavigationCalculationCallback, StopNavigating");
        Print("                           (no third argument = `taxi trace status`: prints patched= / logging=)");
        Print("  taxi lots              - Stage 3b: dump every live ParkingLot (name, world position, spots, first spot = spawn position)");
        Print("  taxi stand             - Stage 3b: resolve the fixed taxi stand and arm it for the next spawn (no spawn itself)");
        Print("  taxi pois              - Stage 3d: dump every destination (custom checkpoints + deal locations + named places) with its goal");
        Print("  taxi wp add <name>     - custom checkpoints: add/replace a named checkpoint at YOUR position (UserData/TaxiDriver/checkpoints.json)");
        Print("  taxi wp remove <name>  - delete a custom checkpoint   |   taxi wp list - show all custom checkpoints");
        Print("  taxi clear             - clear the selected destination (the marker goes away; a waiting ride keeps waiting)");
        Print("  taxi fare              - the meter: $1 per moving in-game minute (0 km/h free), cash first then bank (may go negative)");
        Print("                           (1 real second = 1 in-game minute — TimeManager.CycleDuration = 24 real min/day)");
        Print("  taxi to <name|index>   - Stage 3d: pick the ride destination BY NAME (deal location or lot) and resolve the drop-off");
        Print("  taxi ai                - Stage 4: the game's own driving-supervision numbers (patrol + pursuit), read LIVE");
        Print("                           (while riding this re-routes the running ride instead of waiting for the next one)");
        Print("  taxi visual [on|off]   - Stage 2 GLB visual swap status / switch (also `taxi visual align [on|off]`);");
        Print("                           the swap itself runs automatically inside `taxi spawn` — the switch is also hotkey F4");
        Print("  aliases: taxi list = codes, taxi driver = npc, taxi in = ride, taxi out = exit, taxi reset = cleanup.");
        Print("Hotkeys (the ONLY in-game control surface — the console cannot be typed into):");
        Print("  F1  = taxi go to the proven road target (-131.4, -4.0, 51.9)");
        Print("  F2  = taxi go to the second proven road target (-17.1, 0.0, 13.4)");
        Print("  F3  = full run (spawn -> npc) to the proven road target (-131.4, -4.0, 51.9)");
        Print("  F4  = taxi visual on/off toggle (applies to the NEXT spawn)");
        Print("  F5  = call-taxi (Stage 3b): spawn at the FIXED taxi stand -> npc -> navigate to a road point near the player");
        Print("  F6  = full spike run: spawn -> +1s npc -> +1s go 40 (blind forward target)");
        Print("  F7  = taxi probe");
        Print("  F8  = taxi trace on/off toggle");
        Print("  F9  = player ride/out toggle");
        Print("  F10 = taxi go2 (Navigate with settings=null)");
        Print("  F11 = full run WITHOUT the npc step (occupant hypothesis)");
        Print("  F12 = Navigate on a VANILLA vehicle (vehicle-vs-caller A/B)");
        Print("A leading '/' works too (both `taxi` and `/taxi` are registered).");
        Print("All hotkeys fit on F1-F12 (the keyboard has no keys past F12) and only fire in the");
        Print("        gameplay scene — menu scenes keep their own keys (e.g. MoreSaveSlots binds F2/R on the save screens).");
        Print("Navigation polls every 0.5 s: AutoDriving, target/actual distance, graph, stuck, speed, callback result.");
    }

    // ---------------------------------------------------------------- codes

    /// <summary>Dumps the vehicle code catalog used by every later phase.</summary>
    internal static void Codes()
    {
        VehicleManager? vm = VehicleManager.Instance;
        if (vm == null)
        {
            Mod.Log.Error("VehicleManager.Instance is null — native singleton not ready, cannot enumerate VehiclePrefabs.");
            return;
        }

        var prefabs = vm.VehiclePrefabs;
        if (prefabs == null || prefabs.Count == 0)
        {
            Mod.Log.Error("VehicleManager.VehiclePrefabs is null/empty — no vehicle code catalog available.");
            return;
        }

        Print($"VehicleManager.VehiclePrefabs: {prefabs.Count} prefab(s)");
        for (int i = 0; i < prefabs.Count; i++)
        {
            LandVehicle? prefab = prefabs[i];
            if (prefab == null)
            {
                Print($"  [{i}] <null prefab entry>");
                continue;
            }

            Print($"  [{i}] code='{prefab.VehicleCode}' name='{prefab.VehicleName}' price={prefab.VehiclePrice}");
        }
    }

    // --------------------------------------------------------------- spawn

    /// <summary>
    /// Spawns a vehicle 6 m in front of the player, snapped to the ground with a
    /// downward raycast. Returns false (with a precise error) on any failure.
    /// </summary>
    internal static bool Spawn(string? codeArg)
    {
        if (!EnsureAuthority())
            return false;

        VehicleManager? vm = VehicleManager.Instance;
        if (vm == null)
        {
            Mod.Log.Error("VehicleManager.Instance is null — cannot call SpawnAndReturnVehicle.");
            return false;
        }

        // No Player.Local check here: SpawnVehicle() owns the spawn position
        // (it re-validates the player itself), so this `player` would be unused.

        var prefabs = vm.VehiclePrefabs;
        if (prefabs == null || prefabs.Count == 0)
        {
            Mod.Log.Error("VehicleManager.VehiclePrefabs is null/empty — no vehicle code to spawn.");
            return false;
        }

        // Already waiting for its own tick — never queue a second respawn.
        if (SpikeState.PendingSpawnCode != null)
        {
            Mod.Log.Error("A deferred respawn is already pending (freeze guard) — run `taxi cleanup` first to cancel it.");
            return false;
        }

        // Resolve and validate the code BEFORE touching the old vehicle, so a typo
        // never costs us the current spike vehicle (no blind prefabs[0]).
        string code = ResolveSpawnCode(vm, codeArg);
        if (code.Length == 0)
            return false;

        LandVehicle? prefab = null;
        try
        {
            prefab = vm.GetVehiclePrefab(code);
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"VehicleManager.GetVehiclePrefab('{code}') threw: {ex.Message}");
            return false;
        }

        if (prefab == null)
        {
            Mod.Log.Error($"VehicleManager.GetVehiclePrefab('{code}') returned null — unknown vehicle code. Run `taxi codes`.");
            return false;
        }

        code = prefab.VehicleCode;

        // Freeze guard (review B2/M1): destroy + respawn in the same frame is exactly
        // the sequence behind the 57 s freeze in test run 3. Cleanup is verified and
        // the respawn is deferred to a later SpikeRunner tick.
        bool hadVehicle = SpikeState.Vehicle != null;
        if (hadVehicle)
        {
            Mod.Log.Warn("A spike vehicle already exists — running `taxi cleanup` first (verified before DestroyVehicle).");
            if (!Cleanup())
            {
                Mod.Log.Error("Cleanup of the previous spike vehicle failed — refusing to spawn a second one (freeze guard). See the errors above.");
                return false;
            }

            SpikeState.PendingSpawnCode = code;
            SpikeState.PendingSpawnAt = Time.unscaledTime + FreezeGuardRespawnDelaySeconds;
            SpikeState.PendingSpawnFrame = Time.frameCount + 1;
            Print($"Spawn of '{code}' DEFERRED by {FreezeGuardRespawnDelaySeconds:F1}s (freeze guard: destroy and respawn are separated) — it runs on the next SpikeRunner tick.");
            return true;
        }

        return SpawnVehicle(code);
    }

    /// <summary>
    /// Spawn core: places an already validated vehicle code 6 m in front of the player
    /// and snaps it to the ground. Used by `taxi spawn` for a fresh world and by the
    /// deferred-respawn tick after a guarded cleanup.
    /// </summary>
    internal static bool SpawnVehicle(string code)
    {
        if (!EnsureAuthority())
            return false;

        VehicleManager? vm = VehicleManager.Instance;
        if (vm == null)
        {
            Mod.Log.Error("VehicleManager.Instance is null — cannot call SpawnAndReturnVehicle.");
            return false;
        }

        Player? player = Player.Local;
        if (player == null || player.transform == null)
        {
            Mod.Log.Error("Player.Local is null — cannot compute the spawn position.");
            return false;
        }

        if (SpikeState.Vehicle != null)
        {
            Mod.Log.Error("SpikeState.Vehicle is already set — refusing to leak a second vehicle (freeze guard). Run `taxi cleanup`.");
            return false;
        }

        Vector3 playerPos = player.transform.position;
        Vector3 forward = player.transform.forward;
        if (forward.sqrMagnitude < 1e-4f)
            forward = Vector3.forward;

        Vector3 spawnPos = playerPos + forward * SpawnForwardMeters;
        string spawnSource = "player + forward * 6";

        // Stage 3b: the call-taxi flow spawns ON THE STAND (a fixed ParkingLot
        // spot), never at the player. One-shot override: consumed here so no later
        // F3/F6 spawn can inherit it (the F5 run itself re-arms it before step 1).
        if (SpikeState.StandSpawnPosition.HasValue)
        {
            spawnPos = SpikeState.StandSpawnPosition.Value;
            forward = SpikeState.StandSpawnForward;
            if (forward.sqrMagnitude < 1e-4f)
                forward = Vector3.forward;
            spawnSource = "taxi stand (SpikeState.StandSpawnPosition)";
            SpikeState.StandSpawnPosition = null;
            Mod.Log.Info($"[stand] spawning at the TAXI STAND instead of the player: position=({Fmt(spawnPos)}) forward=({Fmt(forward)}).");
        }

        // Paket C (2026-09-29, "es bugt manchmal rum wenn es an der Mauer kommt beim
        // Parkplatz"): validate the spawn point — a point inside/near geometry makes the
        // vehicle pop and glitch while settling. The first clear candidate wins.
        if (!FindClearSpawn(spawnPos, ref forward, out spawnPos))
        {
            Mod.Log.Warn("[patrol] spawn cancelled: no collision-free vehicle-sized box. No vehicle was created.");
            return false;
        }

        // Review M6: the old ray started at the PLAYER and had no filters, so props,
        // vehicles and the player's own collider could decide the spawn height. It now
        // starts above the spawn point and only accepts static, non-trigger surfaces.
        float spawnRawY = spawnPos.y; // 0.3.0 [snap] diag: the raw requested Y, pre ground-snap
        bool snapped = SnapToGround(spawnPos, out RaycastHit hit, out string snapInfo);

        if (snapped)
            spawnPos.y = hit.point.y;

        Quaternion rotation = Quaternion.LookRotation(forward, Vector3.up);

        LandVehicle? veh;
        try
        {
            // Round 5 probe: all 30 live vehicles were isOwner=False /
            // isPlayerOwned=False and 29 were not physically simulated —
            // spawn as player-owned so ShouldBePhysicallySimulated can pass.
            veh = vm.SpawnAndReturnVehicle(code, spawnPos, rotation, true);
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"VehicleManager.SpawnAndReturnVehicle('{code}', pos, rot, playerOwned=true) threw: {ex.Message}");
            return false;
        }

        if (veh == null)
        {
            Mod.Log.Error($"VehicleManager.SpawnAndReturnVehicle('{code}', ...) returned null — the game refused to spawn the vehicle.");
            return false;
        }

        SpikeState.Vehicle = veh;
        SpikeState.LastVehicleCode = code;
        SpikeState.ResetNavigation();

        // 0.3.0 float fix (the floating-taxi root cause): SnapToGround puts the vehicle
        // ROOT on the ground, but the model bottom (LandVehicle.boundingBox world min Y)
        // sits ~(rootY - boxMinY) BELOW the root — the collider starts embedded in the
        // road and physics pops the chassis up to its rest height. Place the root at
        // newY = hitY + (rootY - boxMinY) from the deterministic boundingBox so the model
        // BOTTOM touches the road and nothing pops after settle.
        float rootY = veh.transform.position.y;
        float hitY = snapped ? hit.point.y : rootY;
        float boxMinY = float.NaN;
        if (TryGetVehicleBoxWorldBounds(veh, out Vector3 snapBoxMin, out _))
            boxMinY = snapBoxMin.y;
        float newY = rootY;
        if (!float.IsNaN(boxMinY))
        {
            newY = hitY + (rootY - boxMinY);
            Vector3 fixedPos = veh.transform.position;
            fixedPos.y = newY;
            veh.transform.position = fixedPos;
        }
        SpikeState.LastSpawnPosition = veh.transform.position;

        // One-shot settle diagnostic (~2 s later, SpikeRunner.TickSettleLog): proves the
        // fixed root Y does not drift (= no physics pop) and reports the box/GLB gaps.
        SpikeState.SettleCheckVehicle = veh;
        SpikeState.SettleCheckSpawnY = rootY;
        SpikeState.SettleCheckHitY = hitY;
        SpikeState.SettleCheckRootY = newY;
        SpikeState.SettleCheckAt = Time.unscaledTime + SettleCheckDelaySeconds;

        string guid = TryDescribeGuid(veh);
        Print($"Spawned vehicle: code='{code}' name='{veh.VehicleName}' price={veh.VehiclePrice} playerOwned={veh.IsPlayerOwned}");
        Print($"  position=({Fmt(spawnPos)}) forward=({Fmt(forward)}) groundSnapped={snapped} source={spawnSource}");
        Print($"  groundSnap: {snapInfo} (ray origin = spawn Y + {GroundRaycastUpOffset:F0}, {GroundRaycastDistance:F0} m down)");
        if (guid.Length > 0)
            Print($"  {guid}");
        else
            Print("  (no readable GUID property on LandVehicle — omitted)");

        // Stage 2: swap the vanilla Shitbox visuals for our GLB (visual only —
        // vanilla physics, colliders and wheel colliders stay untouched).
        TaxiVisual.SwapAfterSpawn(veh);

        // 0.3.0: ONE [snap] diagnostic line per spawn (root/bbox/GLB heights).
        Print($"[snap] spawnY(raw)={FmtY(spawnRawY)} hitY={FmtY(hitY)} boxMinY={FmtY(boxMinY)} " +
              $"rootY={FmtY(rootY)} -> {FmtY(newY)} (= hitY + (rootY - boxMinY)) " +
              $"glbMinY before={FmtY(TaxiVisual.LastGlbMinYBefore)} after={FmtY(TaxiVisual.LastGlbMinYAfter)} " +
              $"(settle check in {SettleCheckDelaySeconds:F0}s)");

        // Post-spawn overlap guard: FindClearSpawn validated the REQUESTED point,
        // but the created vehicle (Y-fix, physics settle, a dynamic object moving
        // in) can still end up inside geometry. Never leave a stuck taxi behind:
        // relocate to a free point or cancel the spawn (destroy, no wall-taxi).
        if (!EnsurePostSpawnFree(veh, ref forward))
        {
            Mod.Log.Warn("[patrol] spawn overlap guard: no free point after creation — spawn cancelled, vehicle removed.");
            try
            {
                veh.DestroyVehicle();
            }
            catch (Exception ex)
            {
                Mod.Log.Error($"DestroyVehicle after blocked post-spawn check failed: {ex.Message}");
            }
            SpikeState.Vehicle = null;
            SpikeState.SettleCheckVehicle = null;
            return false;
        }
        return true;
    }

    /// <summary>
    /// Picks the spawn code: the explicit argument, otherwise the first prefab that
    /// actually carries a <c>VehicleCode</c> (never a blind <c>prefabs[0]</c>).
    /// Returns an empty string (with the reason logged) when nothing is usable.
    /// </summary>
    private static string ResolveSpawnCode(VehicleManager vm, string? codeArg)
    {
        if (!string.IsNullOrWhiteSpace(codeArg))
            return codeArg!.Trim();

        var prefabs = vm.VehiclePrefabs;
        if (prefabs == null || prefabs.Count == 0)
        {
            Mod.Log.Error("VehicleManager.VehiclePrefabs is null/empty — no vehicle code to spawn.");
            return string.Empty;
        }

        for (int i = 0; i < prefabs.Count; i++)
        {
            LandVehicle? prefab = prefabs[i];
            if (prefab == null)
                continue;

            string candidate = prefab.VehicleCode;
            if (!string.IsNullOrWhiteSpace(candidate))
                return candidate.Trim();
        }

        Mod.Log.Error("VehicleManager.VehiclePrefabs has no entry with a usable VehicleCode — pass a code explicitly (`taxi codes`).");
        return string.Empty;
    }

    /// <summary>
    /// Downward raycast for the spawn height, started above the <b>spawn point</b>.
    /// Triggers and everything attached to a rigidbody (player, vehicles, NPCs,
    /// physics props) are filtered out; the topmost remaining surface wins and the
    /// hit object is reported so a tilted spawn can be explained from the log.
    /// </summary>
    private static bool SnapToGround(Vector3 spawnPos, out RaycastHit best, out string info)
    {
        best = default;
        Vector3 origin = spawnPos + Vector3.up * GroundRaycastUpOffset;

        try
        {
            RaycastHit[] hits = Physics.RaycastAll(origin, Vector3.down, GroundRaycastDistance);
            if (hits == null || hits.Length == 0)
            {
                info = "no collider under the spawn point";
                return false;
            }

            Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            int skippedTriggers = 0;
            int skippedDynamic = 0;
            int skippedCharacter = 0;
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider == null)
                    continue;

                if (hit.collider.isTrigger)
                {
                    skippedTriggers++;
                    continue;
                }

                if (hit.collider.attachedRigidbody != null)
                {
                    // Moving objects: the player, other vehicles, NPCs, physics props.
                    skippedDynamic++;
                    continue;
                }

                // Character colliders (NPC capsules, player controller) carry no
                // rigidbody — live test 2026-10-01 snapped the spawn onto an NPC
                // 'Capsule' 2.1 m up, the taxi fell 1.9 m and bounced. Never ground
                // on people (same rule as RoadKeeper.IsObstacle, staticOnly).
                try
                {
                    if (hit.collider.GetComponentInParent<NPC>() != null ||
                        hit.collider.GetComponentInParent<Player>() != null)
                    {
                        skippedCharacter++;
                        continue;
                    }
                }
                catch (Exception)
                {
                    // Unreadable ancestry: fall through to the old accept path.
                }

                best = hit;
                info = $"hit '{hit.collider.name}' layer={LayerMask.LayerToName(hit.collider.gameObject.layer)} " +
                       $"at {hit.distance:F1} m below the ray origin ({hits.Length} hits, skipped {skippedTriggers} trigger / {skippedDynamic} rigidbody / {skippedCharacter} character)";
                return true;
            }

            info = $"all {hits.Length} hits filtered out (skipped {skippedTriggers} trigger / {skippedDynamic} rigidbody / {skippedCharacter} character)";
            return false;
        }
        catch (Exception ex)
        {
            info = $"Physics.RaycastAll threw: {ex.Message}";
            return false;
        }
    }

    // ----------------------------------------------------------------- npc

    /// <summary>
    /// Puts the nearest living NPC into the spike vehicle and dumps the seat
    /// state so we can see whether it landed on the driver seat.
    /// </summary>
    internal static bool Npc()
    {
        if (!EnsureAuthority())
            return false;

        LandVehicle? veh = SpikeState.Vehicle;
        if (veh == null)
        {
            Mod.Log.Error("No spike vehicle — run `taxi spawn` first (SpikeState.Vehicle is null).");
            return false;
        }

        // Stage 5: our own dressed driver first — a random bystander is only
        // the fallback (see TaxiDriverNPC). Already seated in our vehicle counts
        // as boardable: BoardDriver then skips EnterVehicle (no repeat boarding,
        // the 57 s freeze pattern from test run 3).
        NPC? ours = TaxiDriverNPC.TryGetVanilla();
        if (ours != null && OursBoardable(ours, veh))
        {
            Print("Taxi driver: ours is taking the wheel (no bystander needed).");
            return BoardDriver(veh, ours);
        }

        Player? player = Player.Local;
        if (player == null || player.transform == null)
        {
            Mod.Log.Error("Player.Local is null — cannot measure NPC distances.");
            return false;
        }

        var registry = NPCManager.NPCRegistry;
        if (registry == null)
        {
            Mod.Log.Error("NPCManager.NPCRegistry is null — no NPC catalog to search.");
            return false;
        }

        if (registry.Count == 0)
        {
            Mod.Log.Error("NPCManager.NPCRegistry is empty — load a save first.");
            return false;
        }

        NPC? best = null;
        float bestDistance = float.MaxValue;
        int skipped = 0;
        NPC? cooldownNpc = null;
        int cooldownSkipped = 0;

        for (int i = 0; i < registry.Count; i++)
        {
            NPC? candidate = registry[i];
            if (candidate == null)
            {
                skipped++;
                continue;
            }

            try
            {
                // 0.4.7f6 has no NPC.WasCollected — dead == Health.IsDead. A missing
                // Health component cannot prove the NPC is alive, so it is skipped too
                // (review: Health == null NPCs used to be selectable and shown as alive).
                if (candidate.Health == null || candidate.Health.IsDead)
                {
                    skipped++;
                    continue;
                }

                // Freeze guard: do not re-enter the NPC that just left a vehicle — the
                // third EnterVehicle on the same NPC is the pattern behind the 57 s
                // freeze in test run 3.
                if (ReferenceEquals(candidate, SpikeState.LastBoardedNpc) &&
                    Time.unscaledTime - SpikeState.LastBoardedAt < ReEnterCooldownSeconds)
                {
                    cooldownNpc = candidate;
                    cooldownSkipped++;
                    continue;
                }

                if (candidate.IsInVehicle)
                {
                    skipped++;
                    continue;
                }

                if (candidate.transform == null)
                {
                    skipped++;
                    continue;
                }

                float distance = Vector3.Distance(player.transform.position, candidate.transform.position);
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    best = candidate;
                }
            }
            catch (Exception ex)
            {
                Mod.Log.Error($"NPCManager.NPCRegistry[{i}] could not be inspected (native getter failed): {ex.Message}");
                skipped++;
            }
        }

        if (best == null && cooldownNpc != null)
        {
            best = cooldownNpc;
            Mod.Log.Warn(
                $"No NPC outside the {ReEnterCooldownSeconds:F0}s re-enter cooldown — reusing '{SafeId(best)}' anyway " +
                $"(freeze guard fallback, cooldownSkipped={cooldownSkipped}).");
        }

        if (best == null)
        {
            Mod.Log.Error($"No usable living NPC in NPCManager.NPCRegistry ({registry.Count} entries, {skipped} skipped, {cooldownSkipped} in re-enter cooldown).");
            return false;
        }

        // Nearest-bystander fallback (Stage 5: only reached when our own driver
        // is unavailable) — shares the boarding path below.
        Print($"Nearest usable NPC: id='{SafeId(best)}' distance={bestDistance:F1}m (registry={registry.Count}, skipped={skipped})");
        return BoardDriver(veh, best);
    }

    /// <summary>
    /// True when our own invisible driver can take the wheel of
    /// <paramref name="veh"/>: alive, with a transform, and either free or
    /// already seated in our vehicle. Never steals him out of an unrelated
    /// seating silently — an unprovable state falls back to the nearest NPC.
    /// </summary>
    internal static bool OursBoardable(NPC ours, LandVehicle veh)
    {
        try
        {
            if (ours.Health != null && ours.Health.IsDead)
            {
                Mod.Log.Warn("Taxi driver: ours is dead — falling back to the nearest NPC.");
                return false;
            }

            if (ours.transform == null)
                return false;

            if (ours.IsInVehicle && OccupantIndexOf(veh, ours, out _, out _) < 0)
            {
                // Seated in some other vehicle — take him out first, prove it.
                try
                {
                    ours.ExitVehicle();
                }
                catch (Exception ex)
                {
                    Mod.Log.Warn($"Taxi driver: ExitVehicle from foreign vehicle failed ({ex.Message}) — falling back to the nearest NPC.");
                    return false;
                }

                if (SafeInVehicle(ours))
                {
                    Mod.Log.Warn("Taxi driver: still seated elsewhere after ExitVehicle — falling back to the nearest NPC.");
                    return false;
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"Taxi driver: boardability check failed ({ex.Message}) — falling back to the nearest NPC.");
            return false;
        }
    }

    /// <summary>
    /// Seats <paramref name="best"/> in <paramref name="veh"/> (EnterVehicle +
    /// AddNPCOccupant fallback, seat proof) and records it as the driver.
    /// Skips EnterVehicle when the candidate provably already sits in our
    /// vehicle — the repeat-EnterVehicle pattern froze the game for 57 s in
    /// test run 3.
    /// </summary>
    internal static bool BoardDriver(LandVehicle veh, NPC best)
    {
        string npcId = SafeId(best);

        int alreadySlot = OccupantIndexOf(veh, best, out _, out _);
        if (best.IsInVehicle && best.CurrentVehicle != null && alreadySlot >= 0)
        {
            Print($"NPC '{npcId}' is already seated in our vehicle (occupantSlot={alreadySlot}) — skipping EnterVehicle.");
            SpikeState.DriverNpc = best;
            SpikeState.LastBoardedNpc = best;
            SpikeState.LastBoardedAt = Time.unscaledTime;
            return true;
        }

        VehicleSeat? freeBefore = SafeFirstFreeSeat(veh);
        Print($"LandVehicle.GetFirstFreeSeat() before enter -> {(freeBefore == null ? "<none>" : SeatLabel(freeBefore))}");

        try
        {
            // connection = null: NPC.EnterVehicle is virtual+networked; on a host/spike
            // run we deliberately hand in no owning connection.
            best.EnterVehicle(null, veh);
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"NPC.EnterVehicle(connection=null, veh='{codeOf(veh)}') failed: {ex.Message}");
            DumpSeats("after failed NPC.EnterVehicle", veh, best);
            SpikeState.DriverNpc = best;
            return false;
        }

        DumpSeats("after NPC.EnterVehicle", veh, best);

        // Review M3: "IsInVehicle && CurrentVehicle != null" alone never proved a seat.
        // The occupant slot list is checked too — that is what makes the
        // AddNPCOccupant fallback reachable instead of dead code.
        int slotIdx = OccupantIndexOf(veh, best, out _, out _);
        bool boardingComplete = best.IsInVehicle && best.CurrentVehicle != null && slotIdx >= 0;

        if (!boardingComplete)
        {
            Print($"NPC boarding incomplete (IsInVehicle={best.IsInVehicle}, CurrentVehicle={(best.CurrentVehicle == null ? "null" : "set")}, occupantSlot={slotIdx}) — trying the LandVehicle.AddNPCOccupant(npc) fallback.");
            try
            {
                veh.AddNPCOccupant(best);
            }
            catch (Exception ex)
            {
                Mod.Log.Error($"LandVehicle.AddNPCOccupant(npc) failed: {ex.Message}");
            }

            DumpSeats("after AddNPCOccupant fallback", veh, best);
        }

        SpikeState.DriverNpc = best;

        if (!best.IsInVehicle || best.CurrentVehicle == null)
        {
            Mod.Log.Error("NPC is still not seated after NPC.EnterVehicle + LandVehicle.AddNPCOccupant — see the seat dump above.");
            return false;
        }

        Print($"NPC '{npcId}' is seated (npc.IsInVehicle=true, npc.CurrentVehicle='{codeOf(best.CurrentVehicle)}').");
        PrintSeatProof("after enter", veh, best);
        SpikeState.LastBoardedNpc = best;
        SpikeState.LastBoardedAt = Time.unscaledTime;
        return true;
    }

    /// <summary>
    /// F12 helper: first live vanilla vehicle with an agent that is not the spike
    /// vehicle (prefers code 'shitbox' so the A/B matches the spike model).
    /// </summary>
    internal static Il2CppScheduleOne.Vehicles.LandVehicle? FindVanillaVehicle()
    {
        try
        {
            var all = VehicleManager.Instance?.AllVehicles;
            if (all == null)
            {
                Mod.Log.Error("FindVanillaVehicle: VehicleManager.AllVehicles is null.");
                return null;
            }

            Il2CppScheduleOne.Vehicles.LandVehicle? fallback = null;
            for (int i = 0; i < all.Count; i++)
            {
                Il2CppScheduleOne.Vehicles.LandVehicle? v = all[i];
                if (v == null)
                    continue;
                if (ReferenceEquals(v, SpikeState.Vehicle))
                    continue; // never pick the spike vehicle itself
                if (v.Agent == null)
                    continue;

                if (string.Equals(codeOf(v), "shitbox", StringComparison.OrdinalIgnoreCase))
                {
                    Print($"FindVanillaVehicle: [{i}] code='shitbox' pos={Fmt(v.transform.position)} — selected.");
                    return v;
                }

                fallback ??= v;
            }

            if (fallback != null)
            {
                Print($"FindVanillaVehicle: no 'shitbox' found — falling back to [{codeOf(fallback)}] pos={Fmt(fallback.transform.position)}.");
            }

            return fallback;
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"FindVanillaVehicle failed: {ex.Message}");
            return null;
        }
    }

    // ------------------------------------------------------------- ride/out

    /// <summary>
    /// Puts the local player into the spike vehicle.
    /// Seating trap fix (live-proven): <c>GetFirstFreeSeat()</c> returns the DRIVER seat
    /// even with an NPC at the wheel (NPCs are invisible to the seat system) and
    /// <c>EnterVehicle()</c> then seats the player AS DRIVER. Boarding therefore
    /// pre-reserves the driver seat with the local player so <c>EnterVehicle()</c> has to
    /// pick another seat, and releases the reservation in <c>finally</c> —
    /// <see cref="EnsurePassengerSeat"/> finishes the move and
    /// <c>DriverPlayer == null</c> is the proof.
    /// </summary>
    internal static bool Ride()
    {
        LandVehicle? veh = RequireVehicle("ride");
        if (veh == null)
            return false;

        VehicleSeat? driverSeat = FindSeat(veh, driver: true);
        bool reserved = false;
        try
        {
            if (driverSeat != null)
            {
                driverSeat.Occupant = Player.Local; // pre-reserve: EnterVehicle must not take the driver seat
                reserved = true;
            }
            veh.EnterVehicle();
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"LandVehicle.EnterVehicle() failed: {ex.Message}");
            return false;
        }
        finally
        {
            if (reserved)
            {
                try
                {
                    driverSeat!.Occupant = null;
                }
                catch (Exception ex)
                {
                    Mod.Log.Warn($"driver seat reservation release failed: {ex.Message}");
                }
            }
        }

        // Seat fix + driver proof (the NPC at the wheel keeps driving).
        EnsurePassengerSeat(veh);
        bool inVehicle = veh.LocalPlayerIsInVehicle;
        Print($"LandVehicle.EnterVehicle() returned. LocalPlayerIsInVehicle={inVehicle} " +
              $"IsOccupied={veh.IsOccupied} DriverPlayer={SafeDriverPlayerName(veh)}");
        Print(inVehicle
            ? "[primitive c] PROOF: LocalPlayerIsInVehicle=true — the local player is in the spike vehicle."
            : "[primitive c] NOT PROVEN: LocalPlayerIsInVehicle is still false after LandVehicle.EnterVehicle() — re-run `taxi ride` once the world has settled a frame.");

        // Which seat did the player take? This is the live evidence for the seat
        // semantics: VehicleSeat.isOccupied only ever flips for a Player, and it
        // shows whether the player lands on the driver seat the NPC already uses.
        DumpSeats("after ride", veh, SpikeState.DriverNpc);
        return true;
    }

    /// <summary>Paket E (2026-09-29): minimum seconds between two exits — E-spam debounce.</summary>
    internal const float ExitDebounceSeconds = 0.8f;

    /// <summary>Takes the local player back out of the spike vehicle.</summary>
    internal static bool Out()
    {
        LandVehicle? veh = RequireVehicle("out");
        if (veh == null)
            return false;

        // Paket E (2026-09-29, "nach E spammen ein/aus beamt sich das Auto ein paar
        // Meter weg ... einmal beim Aussteigen unter die Map gefallen"): rapid in/out
        // churn is ignored — the seat teleports shove the car via collision resolution.
        if (Time.unscaledTime - SpikeState.LastExitAt < ExitDebounceSeconds)
        {
            Print($"[exit] debounced — rapid in/out ignored ({ExitDebounceSeconds:0.0} s cooldown).");
            return false;
        }
        SpikeState.LastExitAt = Time.unscaledTime;

        Vector3 beforeAll = veh.transform.position;

        // Paket E root fix: the navigation stops BEFORE the player leaves. A live
        // VehicleAgent can snap the car onto its route right at the exit moment (the
        // "car beams away" report) — the exit now sees a parked car. Ride-only guard:
        // a plain `taxi out` in the diagnostic flow must not kill an unrelated run.
        bool stopNavEarly = SpikeState.RideActive && !SpikeState.RideArrived;
        if (stopNavEarly)
        {
            try
            {
                VehicleAgent? agent = veh.Agent;
                if (agent != null)
                    agent.StopNavigating();
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"VehicleAgent.StopNavigating() before the exit failed: {ex.Message}");
            }
            SpikeState.PollingActive = false;
        }

        Vector3 afterNavStop = veh.transform.position;

        try
        {
            veh.ExitVehicle();
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"LandVehicle.ExitVehicle() failed: {ex.Message}");
            return false;
        }

        bool stillInVehicle = veh.LocalPlayerIsInVehicle;
        Print($"LandVehicle.ExitVehicle() returned. LocalPlayerIsInVehicle={stillInVehicle} " +
              $"IsOccupied={veh.IsOccupied}");
        Print(!stillInVehicle
            ? "[primitive c] PROOF: LocalPlayerIsInVehicle=false — the local player left the spike vehicle."
            : "[primitive c] NOT PROVEN: LocalPlayerIsInVehicle is still true after LandVehicle.ExitVehicle().");

        // Stage 3c: leaving the vehicle ends the passenger ride (navigation already
        // stopped above) and re-arms the next board when the NPC is still at the wheel.
        EndRide("player exited", stopNavigation: false, rearm: true);

        ReportExitDeltas(veh, beforeAll, afterNavStop);
        SafeExitGroundSnap(veh);
        return true;
    }

    /// <summary>
    /// Paket E forensics: which step beams the car? Logs every position jump &gt; 0.3 m
    /// (nav-stop delta vs. ExitVehicle delta) — one line, only when something moved.
    /// </summary>
    private static void ReportExitDeltas(LandVehicle veh, Vector3 before, Vector3 afterNavStop)
    {
        try
        {
            Vector3 afterExit = veh.transform.position;
            float dNav = Vector3.Distance(before, afterNavStop);
            float dExit = Vector3.Distance(afterNavStop, afterExit);
            if (dNav < 0.3f && dExit < 0.3f)
                return;

            Mod.Log.Warn(
                $"[exit] forensics: the car moved during the exit — nav-stop delta {FmtY(dNav)} m, " +
                $"ExitVehicle delta {FmtY(dExit)} m (total {FmtY(Vector3.Distance(before, afterExit))} m).");
        }
        catch (Exception)
        {
            // cosmetic only
        }
    }

    /// <summary>
    /// Paket E: never leave the player where there is no ground (the "fell under the map
    /// on exit" report). Static-surface probe below the exit spot (the mod's proven
    /// raycast idiom, see RoadKeeper — UnityEngine.AI is not referenced): anything more
    /// than 2.5 m below road level, more than 1 m above it, or with NO ground at all
    /// gets snapped to safe ground beside the taxi.
    /// </summary>
    internal static void SafeExitGroundSnap(LandVehicle? veh = null)
    {
        try
        {
            Player? player = Player.Local;
            if (player == null || player.transform == null)
                return;

            Vector3 p = player.transform.position;
            bool hadGround = TryExitGroundBelow(p, player, out float groundY);
            if (hadGround && groundY >= p.y - 2.5f && groundY <= p.y + 1f)
                return; // plausible exit spot

            // Recover: 2.5 m beside the taxi there is always road (the car just sat on it).
            Vector3 target = p;
            if (veh != null && veh.Pointer != IntPtr.Zero)
                target = veh.transform.position + veh.transform.right * 2.5f;

            if (TryExitGroundBelow(target, player, out float safeY))
            {
                Vector3 safe = new Vector3(target.x, safeY + 0.2f, target.z);
                player.transform.position = safe;
                Mod.Log.Warn(
                    $"[exit] exit position {Fmt(p)} had {(hadGround ? $"only {FmtY(groundY)} as ground" : "NO ground below")} " +
                    $"(player Y {FmtY(p.y)}) — snapped {FmtY(Vector3.Distance(p, safe))} m to safe ground beside the taxi: {Fmt(safe)}.");
            }
            else
            {
                Mod.Log.Error($"[exit] exit position {Fmt(p)} AND the taxi-side recovery spot have no ground — cannot recover automatically.");
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[exit] SafeExitGroundSnap failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Highest static, non-trigger surface below <paramref name="from"/> (within 15 m) —
    /// the player, dynamic props and vehicles are ignored (same "static surfaces only"
    /// rule as the spawn ground-snap).
    /// </summary>
    private static bool TryExitGroundBelow(Vector3 from, Player player, out float groundY)
    {
        groundY = float.MinValue;
        bool found = false;
        try
        {
            var hits = Physics.RaycastAll(from + Vector3.up * 3f, Vector3.down, 15f, ~0, QueryTriggerInteraction.Ignore);
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                if (hit.collider == null || hit.collider.isTrigger)
                    continue;

                try
                {
                    if (hit.collider.attachedRigidbody != null)
                        continue; // dynamic (vehicles, NPCs, the player's body)

                    if (hit.transform != null && player.transform != null &&
                        (hit.transform.Pointer == player.transform.Pointer || hit.transform.IsChildOf(player.transform)))
                        continue;
                }
                catch (Exception)
                {
                    continue;
                }

                if (!found || hit.point.y > groundY)
                {
                    groundY = hit.point.y;
                    found = true;
                }
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[exit] ground probe failed: {ex.Message}");
        }
        return found;
    }

    // ------------------------------------------------- spawn clearance (Paket C)

    /// <summary>
    /// Paket C: spawn-point clearance probe. The vehicle cabin zone must be free of
    /// static geometry; candidate offsets along the entry axis and sideways get tried
    /// before refusing an unsafe spawn. The entire rotated vehicle footprint is checked.
    /// Each position is tried with four headings (0 / 180 / +90 / -90 deg) so a wall
    /// to the side or a wrong EntryForward polarity still yields a street-facing spawn.
    /// </summary>
    private static bool FindClearSpawn(Vector3 pos, ref Vector3 forward, out Vector3 result)
    {
        Vector3 dir = new Vector3(forward.x, 0f, forward.z);
        dir = dir.sqrMagnitude < 1e-4f ? Vector3.forward : dir.normalized;
        Vector3 right = Vector3.Cross(Vector3.up, dir);
        result = pos;

        // (along, side) metres — the wall case is usually a spot parallel to the wall.
        // Extended to 5-7 m toward the street: the garage entry sits close to the wall
        // and 3.5 m is not always enough to reach open street space.
        (float Along, float Side)[] probes =
        {
            (0f, 0f), (0f, 2f), (0f, -2f), (2f, 0f), (-2f, 0f),
            (0f, 3.5f), (0f, -3.5f), (3.5f, 1.8f), (-3.5f, 1.8f), (3.5f, -1.8f), (-3.5f, -1.8f),
            (5f, 0f), (-5f, 0f), (7f, 0f), (-7f, 0f),
            (5f, 2.5f), (5f, -2.5f), (-5f, 2.5f), (-5f, -2.5f),
        };

        Vector3[] headings = { dir, -dir, right, -right };
        string[] headingLabels = { "0 deg", "180 deg", "90 deg right", "90 deg left" };

        foreach ((float along, float side) in probes)
        {
            Vector3 candidate = pos + dir * along + right * side;
            if (!SnapToGround(candidate, out RaycastHit ground, out _) ||
                Vector3.Angle(ground.normal, Vector3.up) > 40f)
                continue;
            candidate.y = ground.point.y;

            for (int h = 0; h < headings.Length; h++)
            {
                Vector3 heading = headings[h];
                Quaternion rotation = Quaternion.LookRotation(heading, Vector3.up);
                if (!RoadKeeper.IsFree(null, candidate, rotation))
                    continue;

                bool frontBlocked = RoadKeeper.HasObstacle(null, candidate, rotation);
                bool rearBlocked = RoadKeeper.HasObstacle(null, candidate, rotation, rear: true);
                if (frontBlocked && rearBlocked)
                    continue;

                forward = heading;
                if (h > 0)
                {
                    Mod.Log.Info($"[patrol] spawn: front blocked on preferred heading, using {headingLabels[h]} instead.");
                }
                if (along != 0f || side != 0f)
                {
                    Mod.Log.Info(
                        $"[patrol] the spawn point was blocked — moved {FmtY(along)} m along / {FmtY(side)} m sideways " +
                        $"to {Fmt(candidate)} (Paket C clearance probe).");
                }
                result = candidate;
                Mod.Log.Info($"[patrol] spawn clearance OK at {Fmt(candidate)} heading={headingLabels[h]}; box half-extents=1.0/0.6/2.2 m.");
                return true;
            }
        }

        Mod.Log.Warn("[patrol] ALL spawn clearance probes blocked or without safe ground; refusing original-point fallback.");
        return false;
    }

    /// <summary>
    /// Post-spawn overlap guard: the vehicle EXISTS here, so its own colliders
    /// are ignored (veh-aware checks). When the created taxi sits inside geometry,
    /// relocate it to the first free probe point (same extended offsets + four
    /// headings as <see cref="FindClearSpawn"/>) instead of leaving a stuck taxi.
    /// Returns false when no free point exists (caller destroys the vehicle).
    /// </summary>
    private static bool EnsurePostSpawnFree(LandVehicle veh, ref Vector3 forward)
    {
        Vector3 pos;
        Quaternion rot;
        try
        {
            pos = veh.transform.position;
            rot = veh.transform.rotation;
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[patrol] post-spawn overlap check: unreadable transform ({ex.Message}) — assuming blocked.");
            return false;
        }

        if (RoadKeeper.IsFree(veh, pos, rot))
            return true;

        Mod.Log.Warn($"[patrol] spawn overlap detected after creation at {Fmt(pos)} — relocating to a free point.");

        Vector3 dir = new Vector3(forward.x, 0f, forward.z);
        dir = dir.sqrMagnitude < 1e-4f ? Vector3.forward : dir.normalized;
        Vector3 right = Vector3.Cross(Vector3.up, dir);

        (float Along, float Side)[] probes =
        {
            (0f, 0f), (0f, 2f), (0f, -2f), (2f, 0f), (-2f, 0f),
            (0f, 3.5f), (0f, -3.5f), (3.5f, 1.8f), (-3.5f, 1.8f), (3.5f, -1.8f), (-3.5f, -1.8f),
            (5f, 0f), (-5f, 0f), (7f, 0f), (-7f, 0f),
            (5f, 2.5f), (5f, -2.5f), (-5f, 2.5f), (-5f, -2.5f),
        };

        Vector3[] headings = { dir, -dir, right, -right };
        string[] headingLabels = { "0 deg", "180 deg", "90 deg right", "90 deg left" };

        float yOffset = 0f;
        try
        {
            if (TryGetVehicleBoxWorldBounds(veh, out Vector3 boxMin, out _))
                yOffset = pos.y - boxMin.y;
            else
                yOffset = 0f;
        }
        catch (Exception)
        {
            yOffset = 0f;
        }

        foreach ((float along, float side) in probes)
        {
            Vector3 candidate = pos + dir * along + right * side;
            if (!SnapToGround(candidate, out RaycastHit ground, out _) ||
                Vector3.Angle(ground.normal, Vector3.up) > 40f)
                continue;
            float baseY = ground.point.y + yOffset + 0.05f;

            for (int h = 0; h < headings.Length; h++)
            {
                Vector3 heading = headings[h];
                Quaternion rotation = Quaternion.LookRotation(heading, Vector3.up);
                Vector3 placed = new Vector3(candidate.x, baseY, candidate.z);
                if (!RoadKeeper.IsFree(veh, placed, rotation))
                    continue;
                if (RoadKeeper.HasObstacle(veh, placed, rotation) &&
                    RoadKeeper.HasObstacle(veh, placed, rotation, rear: true))
                    continue;

                RoadKeeper.ApplyPosition(veh, placed, rotation, zeroVelocity: true);
                Physics.SyncTransforms();
                forward = heading;
                Mod.Log.Warn(
                    $"[patrol] spawn overlap rescued: moved {FmtY(along)} m along / {FmtY(side)} m sideways " +
                    $"to {Fmt(placed)} heading={headingLabels[h]} (post-spawn guard).");
                return true;
            }
        }

        return false;
    }

    // ------------------------------------------------------ driver retention (Paket G)

    private static float _driverGuardLastLog;
    private static int _driverGuardReboards;

    /// <summary>
    /// Paket G (2026-09-29, "der Fahrer ist ohne Grund ausgestiegen und die Fahrt fuhr
    /// nicht weiter"): driver retention — while anything ride-ish lives, the NPC has to
    /// stay at the wheel. A lost driver is re-boarded immediately (BoardDriver has the
    /// AddNPCOccupant fallback) instead of leaving the car driverless. Returns true when
    /// a re-board was needed (the stuck watchdog uses that instead of burning a recovery).
    /// </summary>
    internal static bool EnsureDriverSeat(LandVehicle veh)
    {
        NPC? npc = SpikeState.DriverNpc;
        if (npc == null || npc.Pointer == IntPtr.Zero)
            return false;

        try
        {
            LandVehicle? current = SafeVehicle(npc);
            bool seatedHere = SafeInVehicle(npc) &&
                current != null && current.Pointer == veh.Pointer &&
                OccupantIndexOf(veh, npc, out _, out _) >= 0;
            if (seatedHere)
            {
                _driverGuardLastLog = 0f;
                return false;
            }

            // Forensics (at most one line per few seconds): WHY did he leave? Speed > 0
            // in the snapshot points at a crash ejection, 0 km/h at the vanilla AI taking
            // over mid-ride.
            if (Time.unscaledTime - _driverGuardLastLog > 4f)
            {
                _driverGuardLastLog = Time.unscaledTime;
                float speed = 0f;
                try
                {
                    speed = Mathf.Abs(veh.Speed_Kmh);
                }
                catch (Exception)
                {
                    // cosmetic only
                }

                Mod.Log.Warn(
                    $"[driver] the driver ({SafeId(npc)}) LEFT the wheel mid-ride — " +
                    $"IsInVehicle={SafeInVehicle(npc)} slot={OccupantIndexOf(veh, npc, out _, out _)} " +
                    $"speed={FmtY(speed)} km/h patrol={(TaxiAI.Active ? $"attached (wp {TaxiAI.CurrentWaypoint})" : "none")} — " +
                    $"re-boarding him now (re-board #{++_driverGuardReboards}).");
            }

            BoardDriver(veh, npc);
            return true;
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[driver] EnsureDriverSeat failed: {ex.Message}");
            return false;
        }
    }

    // ----------------------------------------------------- passenger ride (3c)

    /// <summary>
    /// First seat matching the request: the driver seat (<paramref name="driver"/>=true),
    /// or the first free passenger seat (falls back to the first non-driver seat).
    /// Never <c>GetFirstFreeSeat()</c> — that returns the DRIVER seat while an NPC is at
    /// the wheel (NPCs are invisible to the seat system).
    /// </summary>
    internal static VehicleSeat? FindSeat(LandVehicle veh, bool driver)
    {
        try
        {
            Il2CppReferenceArray<VehicleSeat>? seats = veh.Seats;
            if (seats == null)
                return null;

            VehicleSeat? fallback = null;
            for (int i = 0; i < seats.Length; i++)
            {
                VehicleSeat? seat = seats[i];
                if (seat == null || seat.Pointer == IntPtr.Zero)
                    continue;
                if (seat.isDriverSeat != driver)
                    continue;
                fallback ??= seat;
                if (driver)
                    return seat;

                // passenger: prefer a seat no Player holds
                try
                {
                    if (seat.Occupant == null)
                        return seat;
                }
                catch (Exception ex)
                {
                    Mod.Log.Warn($"VehicleSeat.Occupant read failed in FindSeat: {ex.Message}");
                }
            }
            return fallback;
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"LandVehicle.Seats scan failed in FindSeat(driver={driver}): {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Per-frame passenger seat fix (the seating trap in <see cref="Ride"/>): while the
    /// local player is recorded as the driver (<c>DriverPlayer == Player.Local</c> or
    /// <c>localPlayerSeat.isDriverSeat</c>), move them to a passenger seat — driver seat
    /// released, passenger seat reserved, <c>localPlayerSeat</c> repointed and
    /// <c>LocalPlayerIsDriver = false</c> — then log the <c>DriverPlayer == null</c> proof
    /// once per move. Called every frame from the ride kernel; no-op when already riding
    /// as a passenger.
    /// </summary>
    internal static void EnsurePassengerSeat(LandVehicle veh)
    {
        try
        {
            Player? local = Player.Local;
            bool localIsDriver = false;
            try
            {
                Player? driverPlayer = veh.DriverPlayer;
                localIsDriver = driverPlayer != null && local != null && driverPlayer.Pointer == local.Pointer;
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"LandVehicle.DriverPlayer read failed in EnsurePassengerSeat: {ex.Message}");
            }

            if (!localIsDriver)
            {
                VehicleSeat? localSeat = null;
                try
                {
                    localSeat = veh.localPlayerSeat;
                }
                catch (Exception ex)
                {
                    Mod.Log.Warn($"LandVehicle.localPlayerSeat read failed in EnsurePassengerSeat: {ex.Message}");
                }

                if (localSeat == null || localSeat.Pointer == IntPtr.Zero)
                    return;
                try
                {
                    localIsDriver = localSeat.isDriverSeat;
                }
                catch (Exception ex)
                {
                    Mod.Log.Warn($"VehicleSeat.isDriverSeat read failed in EnsurePassengerSeat: {ex.Message}");
                }
            }

            if (!localIsDriver)
                return;

            VehicleSeat? driverSeat = FindSeat(veh, driver: true);
            VehicleSeat? passengerSeat = FindSeat(veh, driver: false);
            if (driverSeat == null || passengerSeat == null)
            {
                Mod.Log.Warn("[ride] passenger seat fix skipped — driver or passenger seat not found (see `taxi status`).");
                return;
            }

            driverSeat.Occupant = null;
            passengerSeat.Occupant = local;
            veh.localPlayerSeat = passengerSeat;
            veh.LocalPlayerIsDriver = false;
            Mod.Log.Info(
                $"[ride] passenger seat fix applied — player moved off the driver seat; " +
                $"DriverPlayer={SafeDriverPlayerName(veh)} (null proves the NPC keeps the wheel).");
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[ride] passenger seat fix failed: {ex.Message}");
        }
    }

    /// <summary>Readable <c>DriverPlayer</c> value for logs (the name, or "null" for NPC-only/no driver).</summary>
    private static string SafeDriverPlayerName(LandVehicle veh)
    {
        try
        {
            Player? driver = veh.DriverPlayer;
            return driver == null ? "null" : driver.name;
        }
        catch (Exception ex)
        {
            return $"<threw: {ex.Message}>";
        }
    }

    /// <summary>
    /// Stage 3c — starts a passenger ride (the single source of truth shared by the ride
    /// kernel and the TaxiApp): requires an NPC at the wheel of the spike vehicle, engages
    /// the input + trunk locks (<see cref="RideLocks"/>) and dispatches the ride navigation
    /// to <see cref="SpikeState.RideDestination"/> (default <see cref="RideTargetRoadA"/>)
    /// through the standard <see cref="Go"/> path. Any failure rolls the ride back via
    /// <see cref="EndRide"/>.
    /// </summary>
    internal static bool StartRide(string caller)
    {
        LandVehicle? veh = SpikeState.Vehicle;
        if (veh == null)
        {
            Mod.Log.Error($"{caller}: cannot start the ride — no spike vehicle (run `taxi spawn` / CALL TAXI first).");
            EndRide("no vehicle", stopNavigation: false);
            return false;
        }

        NPC? npc = SpikeState.DriverNpc;
        LandVehicle? npcVehicle = npc == null ? null : SafeVehicle(npc);
        bool npcAtWheel = npc != null && SafeInVehicle(npc) && npcVehicle != null && npcVehicle.Pointer == veh.Pointer;
        if (!npcAtWheel)
        {
            Mod.Log.Error(
                $"{caller}: cannot start the ride — no NPC at the wheel of '{codeOf(veh)}' " +
                "(the ride needs the NPC driver; run `taxi npc` first).");
            EndRide("no NPC at the wheel", stopNavigation: false);
            return false;
        }

        SpikeState.RideActive = true;
        SpikeState.RideBoarded = true;
        SpikeState.RidePassengerMode = true;
        SpikeState.RideArrived = false;
        SpikeState.RideAwaitingBoard = false;
        RideLocks.LockTrunk(veh);
        EnsurePassengerSeat(veh);

        // Paket A (2026-09-29): no destination pick, no drive. The old default
        // (RideDestination ?? ROAD A) is gone — it silently drove to stale targets.
        if (!SpikeState.RideDestinationPicked || !SpikeState.RideDestination.HasValue)
        {
            SpikeState.RideAwaitingDestination = true;
            Mod.Log.Info(
                $"{caller} — passenger aboard, but NO destination is selected yet: pick a place in the Taxi app " +
                "(or run `taxi to <name>`) — the drive starts the moment you do.");
            return true;
        }

        SpikeState.RideAwaitingDestination = false;
        Vector3 destination = SpikeState.RideDestination.Value;
        Mod.Log.Info(
            $"{caller} — passenger ride to {SpikeState.RideDestinationName} {Fmt(destination)}: " +
            $"input + trunk locks engaged, '{SafeId(npc)}' keeps the wheel (DriverPlayer={SafeDriverPlayerName(veh)}).");

        if (!StartRideDrive(npc, veh, destination, caller))
        {
            EndRide("ride navigation dispatch failed");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Stage 3c — ends a passenger ride from anywhere: resets the ride state
    /// (<see cref="SpikeState.ResetRide"/>, the destination picker survives), releases the
    /// input + trunk locks and optionally stops the ride navigation (inlined
    /// <see cref="Stop"/>-style — <c>Stop()</c> itself calls <c>EndRide</c> and must not
    /// recurse; only a real ride owns the navigation, a plain <c>taxi out</c> in the
    /// diagnostic flow must not kill an unrelated run). <paramref name="rearm"/> re-arms
    /// the next board when the player exited but the NPC is still at the wheel (for player
    /// exits — NEVER for STOP).
    /// </summary>
    internal static void EndRide(string reason, bool stopNavigation = true, bool rearm = false)
    {
        bool wasRiding = SpikeState.RideActive || SpikeState.RideBoarded || SpikeState.RidePassengerMode || SpikeState.RideAwaitingBoard;
        SpikeState.ResetRide();
        FareMeter.Stop(reason);
        RideLocks.UnlockTrunk();
        // Stage 4: the game's patrol behaviour must go with the ride (it is attached to
        // the NPC, not to the vehicle, so DestroyVehicle alone would leave it running).
        TaxiAI.StopPatrol(reason);

        if (stopNavigation && wasRiding)
        {
            try
            {
                VehicleAgent? agent = SpikeState.Vehicle?.Agent;
                if (agent != null)
                    agent.StopNavigating();
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"VehicleAgent.StopNavigating() during EndRide('{reason}') failed: {ex.Message}");
            }
            SpikeState.PollingActive = false;
        }

        string rearmNote = string.Empty;
        if (rearm)
        {
            LandVehicle? veh = SpikeState.Vehicle;
            NPC? npc = SpikeState.DriverNpc;
            LandVehicle? npcVehicle = npc == null ? null : SafeVehicle(npc);
            bool playerOut = veh == null || !veh.LocalPlayerIsInVehicle;
            bool npcAtWheel = veh != null && npc != null && SafeInVehicle(npc) && npcVehicle != null && npcVehicle.Pointer == veh.Pointer;
            if (playerOut && npcAtWheel)
            {
                SpikeState.RideAwaitingBoard = true;
                rearmNote = " — next board (E or F9) starts a new ride";
            }
        }

        if (wasRiding)
            Mod.Log.Info($"[ride] ended ({reason}) — input/trunk locks released{rearmNote}.");
    }

    /// <summary>
    /// Stage 3c — destination picker target (TaxiApp ROAD A / ROAD B / STAND): writes
    /// <see cref="SpikeState.RideDestination"/> + <see cref="SpikeState.RideDestinationName"/>
    /// (both survive rides; the change takes effect on the NEXT ride). STAND is the fixed
    /// taxi stand coordinate (<see cref="TaxiStand.StandCoordinate"/>). Unknown keys change
    /// nothing.
    /// </summary>
    internal static bool SetRideDestination(string key, string caller)
    {
        string normalized = (key ?? string.Empty).Trim().ToUpperInvariant();

        // The taxi stand remains a first-class choice — proven routable, because the
        // taxi spawns on that street-side entry on every call-taxi run.
        if (normalized == "STAND")
        {
            SpikeState.RideGoal = TaxiStand.StandCoordinate;
            SpikeState.RideDestination = TaxiStand.StandCoordinate;
            SpikeState.RideDestinationName = "Taxi-Stand";
            SpikeState.RideDestinationKind = "stand";
            SpikeState.RideDropOff = "stand entry";
            SpikeState.RideDropOffLot = TaxiStand.StandName;
            SpikeState.RideDestinationPicked = true;
            Print($"{caller}: ride destination = taxi stand {Fmt(TaxiStand.StandCoordinate)}.");
            return ApplyDestinationToRunningRide(caller);
        }

        // Stage 3d: the legacy hand-read coordinates still work (the phone app's
        // picker uses them until the catalog list replaces it) — but they are named
        // as what they are, because ROAD A sits next to the skatepark area.
        if (normalized == "ROAD A" || normalized == "ROAD B")
        {
            Vector3 legacy = normalized == "ROAD A" ? RideTargetRoadA : RideTargetRoadB;
            Mod.Log.Warn(
                $"{caller}: '{normalized}' is a legacy hand-read coordinate {Fmt(legacy)} — " +
                "prefer a named place from `taxi pois`.");
            SpikeState.RideGoal = legacy;
            SpikeState.RideDestination = legacy;
            SpikeState.RideDestinationName = normalized + " (legacy)";
            SpikeState.RideDestinationKind = "legacy";
            SpikeState.RideDropOff = "legacy coordinate";
            SpikeState.RideDropOffLot = string.Empty;
            SpikeState.RideDestinationPicked = true;
            return ApplyDestinationToRunningRide(caller);
        }

        // Everything else is a catalog name or index (`taxi pois`).
        return To(key, caller);
    }

    /// <summary>
    /// Paket A (2026-09-29): a fresh pick applies to a RUNNING ride too (it starts
    /// a waiting ride, or re-routes a driving one) instead of "the NEXT ride" —
    /// picking mid-ride is exactly how a waiting taxi gets going.
    /// </summary>
    private static bool ApplyDestinationToRunningRide(string caller)
    {
        bool riding = SpikeState.RideActive || SpikeState.RideBoarded || SpikeState.RidePassengerMode;
        if (riding)
            return ReRoute(caller);

        Print($"{caller}: takes effect on the NEXT ride (no ride is running).");
        return true;
    }

    /// <summary>Guarded read of <c>LandVehicle.LocalPlayerIsInVehicle</c>.</summary>
    internal static bool SafePlayerInVehicle(LandVehicle veh)
    {
        try
        {
            return veh != null && veh.Pointer != IntPtr.Zero && veh.LocalPlayerIsInVehicle;
        }
        catch (Exception ex)
        {
            // Unknown counts as "still seated": never destroy under a live player.
            Mod.Log.Warn($"LocalPlayerIsInVehicle read failed ({ex.Message}) — treating the player as seated (destroy refused).");
            return true;
        }
    }

    // ------------------------------------------------------------------ go

    /// <summary>
    /// Fires <c>VehicleAgent.Navigate(target, settings, callback)</c> and switches the
    /// 0.5 s polling on. The callback goes through the interop assembly's implicit
    /// <c>Action&lt;ENavigationResult&gt; → NavigationCallback</c> conversion, so
    /// <c>Failed</c>/<c>Complete</c>/<c>Stopped</c> ends the polling authoritatively;
    /// polling remains the fallback when the callback path is rejected.
    /// </summary>
    internal static bool Go(float distance, bool useSettings = true, Vector3? absoluteTarget = null, bool toPlayer = false)
    {
        if (!EnsureAuthority())
            return false;

        LandVehicle? veh = SpikeState.Vehicle;
        if (veh == null)
        {
            Mod.Log.Error("No spike vehicle — run `taxi spawn` first.");
            return false;
        }

        Player? player = Player.Local;
        if (player == null || player.transform == null)
        {
            Mod.Log.Error("Player.Local is null — cannot compute the navigation target.");
            return false;
        }

        VehicleAgent? agent = veh.Agent;
        if (agent == null)
        {
            try
            {
                agent = veh.gameObject.GetComponent<VehicleAgent>();
            }
            catch (Exception ex)
            {
                Mod.Log.Error($"GameObject.GetComponent<VehicleAgent>() fallback failed: {ex.Message}");
            }

            if (agent == null)
            {
                Mod.Log.Error("LandVehicle.Agent is null and the GetComponent<VehicleAgent>() fallback found nothing — VehicleAgent.Navigate is unreachable.");
                return false;
            }

            Print("LandVehicle.Agent was null — resolved the agent via GameObject.GetComponent<VehicleAgent>() fallback.");
        }

        // Round 2 diagnosis: a fresh spawn can sit parked with brakes on, which
        // keeps AutoDriving silently false. Release the vehicle before navigating.
        try
        {
            Print($"[diag] isParked={veh.isParked} BrakesApplied={veh.BrakesApplied} " +
                  $"HandbrakeApplied={veh.HandbrakeApplied} KinematicMode={agent.KinematicMode} " +
                  $"State={SafeStateName(veh)} agent.enabled={agent.enabled} navCalc={agent.NavigationCalculationInProgress}");

            // Round 5 probe: a spawned vehicle is not physically simulated
            // (29/30 vehicles False) — Navigate gives up before CalculatePath.
            bool simBefore = veh.IsPhysicallySimulated;
            bool shouldSim;
            try
            {
                shouldSim = veh.ShouldBePhysicallySimulated();
            }
            catch (Exception ex)
            {
                shouldSim = false;
                Mod.Log.Warn($"ShouldBePhysicallySimulated() threw: {ex.Message}");
            }

            if (!simBefore)
            {
                veh.UpdatePhysicallySimulated(true);
                Print($"[diag] IsPhysicallySimulated forced: False -> {veh.IsPhysicallySimulated} (ShouldBePhysicallySimulated={shouldSim}).");
            }
            else
            {
                Print($"[diag] IsPhysicallySimulated already True (ShouldBe={shouldSim}).");
            }
            if (veh.isParked)
            {
                veh.ExitPark(false);
                Print("[diag] ExitPark(moveToExitPoint=false) called.");
            }

            if (veh.BrakesApplied)
            {
                veh.BrakesApplied = false;
                Print("[diag] BrakesApplied set to false.");
            }

            if (veh.HandbrakeApplied)
            {
                veh.HandbrakeApplied = false;
                Print("[diag] HandbrakeApplied set to false.");
            }

            if (!agent.enabled)
            {
                agent.enabled = true;
                Print("[diag] agent.enabled set to true.");
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[diag] vehicle release step failed (continuing with Navigate): {ex.Message}");
        }

        Vector3 playerPos = player.transform.position;
        Vector3 forward = player.transform.forward;
        if (forward.sqrMagnitude < 1e-4f)
            forward = Vector3.forward;

        // Round 11: the path search refuses the blind forward*40 corner while
        // every vanilla dispatch succeeds on road coordinates — absoluteTarget
        // drives to proven road points instead.
        Vector3 target;
        if (absoluteTarget.HasValue)
        {
            target = absoluteTarget.Value;
        }
        else
        {
            target = playerPos + forward * distance;
            target.y = playerPos.y;
        }

        // Round 5 probe: raw target sat 9.6 m off the vehicle graph. Snap the
        // destination onto the graph before dispatching — Navigate path-fails
        // silently when the destination projection is out of reach.
        try
        {
            Vector3 sampled = NavigationUtility.SampleVehicleGraph(target);
            float snapDelta = Vector3.Distance(target, sampled);
            Print($"[diag] target snap: {Fmt(target)} -> {Fmt(sampled)} (graph delta {snapDelta:F1}m).");
            if (snapDelta < 25f)
                target = sampled;
            else
                Print("[diag] graph snap delta > 25 m — keeping the raw target.");
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"NavigationUtility.SampleVehicleGraph(target) failed (keeping raw target): {ex.Message}");
        }

        // Stage 3d: the forced orientation snap is GONE. It rotated the car to face
        // the target regardless of the road direction (Dominik: "fährt gegen
        // Lampen") — right after a spawn the car could end up nose-first in
        // geometry, and the agent then had to reverse out of a pose it never chose.
        // The agent's own steering picks the heading; the reverse manoeuvre now
        // lives in the progress supervision (SpikeRunner.RecoverStuck).

        NavigationSettings? settings = null;
        if (useSettings)
        {
            try
            {
                settings = new NavigationSettings
                {
                    endAtRoad = true,
                    ensureProximityToGraph = true,
                    // Stage 3d: was true. A failed path calculation then TELEPORTS the
                    // vehicle onto the nearest graph point — the only way the car ends
                    // up somewhere it never drove to (suspected "suddenly inside the
                    // skatepark" moment). A failed calculation is now a normal,
                    // handled outcome: the callback reports Failed and the progress
                    // supervision reacts instead of a silent teleport.
                    teleportToGraphIfCalculationFails = false,
                };
            }
            catch (Exception ex)
            {
                Mod.Log.Error($"new NavigationSettings() (endAtRoad/ensureProximityToGraph/teleportToGraphIfCalculationFails) failed: {ex.Message}");
                return false;
            }
        }

        float startDistance = Vector3.Distance(veh.transform.position, target);
        Print($"Navigate(settings={(settings == null ? "null" : "NavigationSettings(endAtRoad=true, ensureProximityToGraph=true, teleportToGraphIfCalculationFails=false)")}) requestedDistance={distance:F1}m " +
              $"startDistance={startDistance:F1}m target=({Fmt(target)}) vehicle=({Fmt(veh.transform.position)})");

        // State first: a callback that fires inside Navigate must not be overwritten
        // by bookkeeping that runs after the dispatch (review M7).
        SpikeState.PollingActive = true;
        SpikeState.NavTarget = target;
        SpikeState.NavStartPosition = veh.transform.position;
        SpikeState.NavStartDistance = startDistance;
        SpikeState.NavStartTime = Time.unscaledTime;
        SpikeState.LastHeartbeat = Time.unscaledTime;
        SpikeState.NavEverAutoDriving = false;
        SpikeState.NavRetried = !useSettings;
        SpikeState.NavRetryAt = 0f;
        SpikeState.NavCallbackResult = null;
        SpikeState.NavCallback = SpikeRunner.OnNavigationResult;
        // Stage 3d: a fresh dispatch starts a fresh progress window — a stale window
        // would make the watchdog report "stuck" on the first tick.
        SpikeState.ProgressFrom = veh.transform.position;
        SpikeState.ProgressWindowStart = Time.unscaledTime;
        SpikeState.StuckRecoveries = 0;
        SpikeState.NavReDispatchAt = 0f;
        SpikeState.NavReDispatchTarget = Vector3.zero;
        RoadKeeper.Reset();
        SpikeRunner.ArmStartupRecovery(veh);
        // Stage 3b: gates the "taxi arrived at player" verdict for the F5 run.
        SpikeState.NavToPlayer = toPlayer;

        try
        {
            // Implicit conversion Action<ENavigationResult> -> VehicleAgent.NavigationCallback
            // (public in the interop assembly) — the documented "IL2CPP delegate-ctor
            // pitfall" does not apply to this path, so the vanilla completion signal is used.
            agent.Navigate(target, settings, SpikeState.NavCallback);
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"VehicleAgent.Navigate(target, settings, callback) failed: {ex.Message} — retrying without the callback (polling stays as the fallback).");
            try
            {
                agent.Navigate(target, settings, null);
            }
            catch (Exception ex2)
            {
                SpikeState.PollingActive = false;
                Mod.Log.Error($"VehicleAgent.Navigate(target, settings, null) failed as well: {ex2.Message}");
                return false;
            }
        }

        Print("VehicleAgent.Navigate dispatched — polling every 0.5 s plus an ENavigationResult callback (`taxi stop` cancels rides AND despawns the taxi; the progress supervision re-dispatches when the car does not move).");
        return true;
    }

    /// <summary>`taxi pois` — Stage 3d: dump every selectable destination.</summary>
    internal static void Pois() => TaxiDestinations.DumpPois("taxi pois");

    /// <summary>
    /// Stage 3d — `taxi to &lt;name|index&gt;`: resolves a place from the destination
    /// catalog, decides where the taxi actually stops
    /// (<see cref="TaxiDestinations.ResolveArrival"/>: direct when the goal is on
    /// the vehicle graph, otherwise the nearest lot entry) and stores it as the
    /// ride destination. While a ride is running the new target is dispatched
    /// immediately (re-route), otherwise it applies to the next ride.
    /// </summary>
    internal static bool To(string query, string caller)
    {
        if (!TaxiDestinations.TryFind(query, out TaxiDestinations.Destination destination))
            return false;

        if (!TaxiDestinations.ResolveArrival(destination, out TaxiDestinations.Arrival arrival))
            return false;

        SpikeState.RideGoal = destination.Goal;
        SpikeState.RideDestination = arrival.Point;
        SpikeState.RideDestinationName = destination.Name;
        SpikeState.RideDestinationKind = destination.Kind;
        SpikeState.RideDropOff = arrival.Kind;
        SpikeState.RideDropOffLot = arrival.LotName;
        SpikeState.RideDestinationPicked = true;

        Print(
            $"{caller}: destination = {destination.Name} [{destination.Kind}] goal={TaxiDestinations.Fmt(destination.Goal)} " +
            $"-> drop-off {arrival.Kind} at {TaxiDestinations.Fmt(arrival.Point)} — {arrival.Reason}.");

        bool riding = SpikeState.RideActive || SpikeState.RideBoarded || SpikeState.RidePassengerMode;
        if (riding)
            return ReRoute(caller);

        Print($"{caller}: takes effect on the NEXT ride (no ride is running).");
        return true;
    }

    /// <summary>Re-dispatches the running ride to the current ride destination.</summary>
    internal static bool ReRoute(string caller)
    {
        Vector3? target = SpikeState.RideDestination;
        if (target == null)
        {
            Mod.Log.Warn($"{caller}: no ride destination set — nothing to re-route to.");
            return false;
        }

        if (SpikeState.Vehicle == null)
        {
            Mod.Log.Warn($"{caller}: no vehicle to re-route.");
            return false;
        }

        // A re-route invalidates the previous arrival verdict.
        SpikeState.RideArrived = false;
        Print($"{caller}: re-routing the running ride to {SpikeState.RideDestinationName} " +
              $"({SpikeState.RideDropOff}) at {TaxiDestinations.Fmt(target.Value)}.");
        return StartRideDrive(SpikeState.DriverNpc, SpikeState.Vehicle, target.Value, caller);
    }

    /// <summary>
    /// Stage 4 — dispatches a ride to <paramref name="destination"/>. The GAME's own
    /// driver is tried first (<see cref="TaxiAI.StartPatrol"/>: a runtime
    /// <c>VehiclePatrolRoute</c> + <c>VehiclePatrolBehaviour</c> on the taxi NPC — the
    /// clone of what drives the police car); the mod's own <c>Navigate()</c> dispatch is
    /// the fallback. Both paths leave the polling running, so the ride kernel's arrival
    /// verdict and the App status behave identically either way.
    /// </summary>
    private static bool StartRideDrive(NPC? npc, LandVehicle? veh, Vector3 destination, string caller)
    {
        // A dispatch means the wait is over (Paket A: ride started mid-ride by the picker).
        SpikeState.RideAwaitingDestination = false;

        // The meter starts the moment the destination turns into a drive (Dominik:
        // "der Trigger beginnt wenn die Destination ausgewählt ist"). Idempotent —
        // a mid-ride re-route keeps the running total.
        FareMeter.Start();

        if (veh != null && npc != null && TaxiAI.StartPatrol(npc, veh, destination, SpikeState.RideDestinationName))
        {
            SpikeState.NavTarget = destination;
            SpikeState.NavStartTime = Time.unscaledTime;
            SpikeState.NavStartPosition = veh.transform.position;
            SpikeState.NavStartDistance = Vector3.Distance(veh.transform.position, destination);
            SpikeState.NavEverAutoDriving = false;
            SpikeState.NavRetried = false;
            SpikeState.NavCallbackResult = null;
            SpikeState.NavGaveUp = false;
            SpikeState.ProgressFrom = veh.transform.position;
            SpikeState.ProgressWindowStart = Time.unscaledTime;
            SpikeState.StuckRecoveries = 0;
            SpikeState.NavReDispatchAt = 0f;
            SpikeState.PollingActive = true;
            RoadKeeper.Reset();
            SpikeRunner.ArmStartupRecovery(veh);
            Mod.Log.Info(
                $"{caller}: the GAME's patrol driver owns the ride to {SpikeState.RideDestinationName} " +
                $"({SpikeState.RideDropOff}) — the mod only supervises ([patrol]/[hb] lines).");
            return true;
        }

        Mod.Log.Warn(
            $"{caller}: the game's patrol driver is unavailable — using the mod's own Navigate() dispatch " +
            $"to {SpikeState.RideDestinationName}.");
        return Go(0f, useSettings: true, absoluteTarget: destination, toPlayer: false);
    }

    /// <summary>Cancels navigation and the polling (and aborts a passenger ride anywhere).</summary>
    internal static bool Stop()
    {
        // Dominik (Stage 3d): "Kündigen soll das Taxi despawnen lassen". STOP is
        // therefore the proven `taxi cleanup` teardown — ride state + input/trunk
        // locks first, then everyone out (verified), then StopNavigating and
        // DestroyVehicle. The old behaviour (navigation off, player left seated in
        // a car without a driver) read as "the game is stuck" because the player
        // could neither walk nor drive: the NPC held the driver seat.
        //
        // A failed exit never destroys (freeze guard) — the state stays retryable.
        bool despawned = Cleanup("stopped");
        if (despawned)
            Print("Ride cancelled — the taxi has been despawned (destroyed) and the state is clear.");
        else
            Mod.Log.Error("taxi stop: the taxi could NOT be despawned (see the errors above) — state kept, retry `taxi stop` or `taxi cleanup`.");

        return despawned;
    }

    // -------------------------------------------------------- stand / player target

    /// <summary><c>taxi lots</c> — dump every live <c>ParkingLot</c> (the Stage-3b stand candidates).</summary>
    internal static void Lots() => TaxiStand.DumpLots("manual `taxi lots` dump");

    /// <summary>
    /// <c>taxi stand</c> / F5 step 0 — resolves the fixed taxi stand and arms it for
    /// the next spawn (<see cref="SpikeState.StandSpawnPosition"/>, one-shot). Never
    /// spawns anything itself, so it is safe to run at any time.
    /// </summary>
    internal static bool PrepareStand(string caller)
    {
        if (!TaxiStand.TryResolve(out TaxiStand.StandSelection stand))
        {
            Mod.Log.Error($"{caller}: the taxi stand could not be resolved — see the [stand] log above (`taxi lots` dumps the candidates).");
            return false;
        }

        SpikeState.StandSpawnPosition = stand.Position;
        SpikeState.StandSpawnForward = stand.Forward;
        Print($"{caller}: stand '{stand.LotName}' armed for the next spawn — position=({Fmt(stand.Position)}) forward=({Fmt(stand.Forward)}) rule='{stand.Rule}'.");
        return true;
    }

    /// <summary>
    /// Stage 3b call-taxi — the single source of truth for ordering the taxi.
    /// Shared by the F5 hotkey (<see cref="SpikeRunner.HandleHotkey"/>) and the
    /// in-game phone app (<c>TaxiApp</c> "CALL TAXI"): the shared run-start guard
    /// (<see cref="SpikeRunner.TryBeginGo"/>), the one-shot stand arm
    /// (<see cref="PrepareStand"/>) and the automation state block all live here,
    /// so the hotkey and the phone button can never drift apart.
    /// The run spawns at the taxi stand → +1 s npc → +1 s navigate to a road
    /// point near the player (step 3 reads <c>SpikeState.AutoToPlayer</c>).
    /// </summary>
    /// <param name="caller">Who ordered the taxi (e.g. <c>"F5"</c>, <c>"TaxiApp"</c>) — used verbatim in the log lines.</param>
    /// <returns><c>true</c> when the run was started; <c>false</c> with a logged reason otherwise.</returns>
    internal static bool CallTaxi(string caller)
    {
        // Shared gate: while an automatic run is in progress or a deferred respawn
        // is still pending, the request is refused (with a reason) instead of
        // interrupting the run — the same rule as every run-starting hotkey.
        if (!SpikeRunner.TryBeginGo(caller))
            return false;

        // Arms SpikeState.StandSpawnPosition (one-shot) — refuses to start when
        // no stand can be resolved (see the [stand] log for the reason).
        if (!PrepareStand(caller))
            return false;

        Mod.Log.Info($"{caller} — call-taxi: spawn at the taxi stand → +1s npc → +1s navigate to the player.");
        SpikeState.AutoRunning = true;
        SpikeState.AutoStep = 1;
        SpikeState.AutoNextAt = Time.unscaledTime;
        SpikeState.AutoCode = SpikeState.LastVehicleCode;
        SpikeState.SkipNpcStep = false;
        SpikeState.AutoTarget = null;
        SpikeState.AutoToPlayer = true;
        return true;
    }

    /// <summary>
    /// Stage 3b step 3: the road point near the player the taxi has to drive to.
    /// Returns <c>null</c> when no candidate could be projected onto the vehicle
    /// graph (the full candidate table is logged first).
    /// </summary>
    internal static Vector3? PlayerRoadTarget()
    {
        Player? player = Player.Local;
        if (player == null || player.transform == null)
        {
            Mod.Log.Error("Player.Local is null — cannot resolve a navigation target near the player.");
            return null;
        }

        Vector3 pos = player.transform.position;
        Vector3 forward = player.transform.forward;
        return RoadTarget.FindNearPlayer(pos, forward);
    }

    // -------------------------------------------------------------- status

    internal static void Status()
    {
        Print("=== TaxiDriver spike status ===");

        LandVehicle? veh = SpikeState.Vehicle;
        if (veh == null)
        {
            Print("Vehicle: none (run `taxi spawn`)");
        }
        else
        {
            Print($"Vehicle: code='{codeOf(veh)}' name='{SafeName(veh)}' position=({Fmt(veh.transform.position)}) " +
                  $"speed={veh.Speed_Kmh:F1} km/h isOccupied={veh.IsOccupied} localPlayerIsInVehicle={veh.LocalPlayerIsInVehicle} " +
                  $"isPlayerOwned={veh.IsPlayerOwned}");
            VehicleAgent? agent = veh.Agent;
            Print($"  Agent: {(agent == null ? "null" : $"present (AutoDriving={SafeAuto(agent)})")}");
        }

        Print($"LastVehicleCode: {(SpikeState.LastVehicleCode ?? "<none>")}");
        Print($"LastSpawnPosition: ({Fmt(SpikeState.LastSpawnPosition)})");

        NPC? npc = SpikeState.DriverNpc;
        if (npc == null)
        {
            Print("NPC: none (run `taxi npc`)");
        }
        else
        {
            // Health == null proves nothing — report it as unknown instead of "alive".
            string alive = npc.Health == null ? "unknown (Health is null)" : (!npc.Health.IsDead).ToString();
            Print($"NPC: id='{SafeId(npc)}' isAlive={alive} " +
                  $"isInVehicle={SafeInVehicle(npc)} currentVehicle='{(SafeVehicle(npc) == null ? "null" : codeOf(SafeVehicle(npc)!))}'");
        }

        Print($"Ride: active={SpikeState.RideActive} boarded={SpikeState.RideBoarded} arrived={SpikeState.RideArrived} " +
              $"awaitingBoard={SpikeState.RideAwaitingBoard} passengerMode={SpikeState.RidePassengerMode} " +
              $"destination='{SpikeState.RideDestinationName}' {(SpikeState.RideDestination.HasValue ? Fmt(SpikeState.RideDestination.Value) : "(default ROAD A)")}");

        Print($"Navigation polling active: {SpikeState.PollingActive}");
        if (SpikeState.PollingActive)
        {
            LandVehicle? navVeh = SpikeState.Vehicle;
            float current = navVeh == null ? -1f : Vector3.Distance(navVeh.transform.position, SpikeState.NavTarget);
            Print($"  target=({Fmt(SpikeState.NavTarget)}) start=({Fmt(SpikeState.NavStartPosition)}) " +
                  $"startDistance={SpikeState.NavStartDistance:F1}m currentDistance={current:F1}m " +
                  $"elapsed={Time.unscaledTime - SpikeState.NavStartTime:F1}s everAutoDriving={SpikeState.NavEverAutoDriving} " +
                  $"nullRetryFired={SpikeState.NavRetried} " +
                  $"retryAge={(SpikeState.NavRetryAt > 0f ? Time.unscaledTime - SpikeState.NavRetryAt : -1f):F1}s " +
                  $"callback={SpikeState.NavCallbackResult ?? "-"}");
        }

        string autoTarget = SpikeState.AutoTarget.HasValue
            ? $"({Fmt(SpikeState.AutoTarget.Value)})"
            : "<none — blind forward*40>";
        Print($"F6 automation running: {SpikeState.AutoRunning} (step {SpikeState.AutoStep})");
        Print($"  AutoTarget: {autoTarget}  SkipNpcStep: {SpikeState.SkipNpcStep}");
        Print(SpikeState.PendingSpawnCode == null
            ? "Deferred respawn pending: no"
            : $"Deferred respawn pending: yes (code='{SpikeState.PendingSpawnCode}', fires at t={SpikeState.PendingSpawnAt:F1}s)");
        Print(SpikeState.LastBoardedNpc == null
            ? "Re-enter cooldown: none"
            : $"Re-enter cooldown: last boarded '{SafeId(SpikeState.LastBoardedNpc)}' {Time.unscaledTime - SpikeState.LastBoardedAt:F1}s ago (window {ReEnterCooldownSeconds:F0}s)");
        Print($"Trace: patched={SpikeTrace.Patched} logging={SpikeTrace.Logging}");

        // Stage 2 (review M1-2): the four visual fields — `taxi visual` prints the
        // verbose form, `taxi status` must not hide them either.
        GameObject? visualRoot = SpikeState.VisualRoot;
        bool visualRootAlive = visualRoot != null && visualRoot.Pointer != IntPtr.Zero;
        Print($"VisualSwapEnabled: {SpikeState.VisualSwapEnabled}  VisualAutoAlign: {SpikeState.VisualAutoAlign}");
        Print($"VisualRoot: {(visualRootAlive ? $"'{visualRoot!.name}'" : "<none>")}  VisualSwaps: {SpikeState.VisualSwaps}");
        Print("=== end ===");
    }

    // ------------------------------------------------------------- cleanup

    /// <summary>
    /// Everyone out, then destroy the spike vehicle and clear the state. Freeze guard
    /// (review B2/M1): the NPC exit is verified, the destroy is refused while
    /// <c>LandVehicle.OccupantNPCs</c> still holds entries, and the state is only
    /// cleared after a successful destroy — a failed cleanup stays retryable instead
    /// of stranding a vehicle we can no longer reach.
    /// </summary>
    internal static bool Cleanup(string reason = "cleanup")
    {
        bool ok = true;
        EndRide(reason); // ride state + input/trunk locks before the teardown
        SpikeState.PollingActive = false;
        SpikeState.CancelPendingSpawn();

        NPC? npc = SpikeState.DriverNpc;
        if (npc != null)
        {
            try
            {
                if (SafeInVehicle(npc))
                {
                    npc.ExitVehicle();
                    Print("NPC.ExitVehicle() called.");
                }
                else
                {
                    Print("NPC was not in a vehicle — skipped NPC.ExitVehicle().");
                }
            }
            catch (Exception ex)
            {
                Mod.Log.Error($"NPC.ExitVehicle() failed: {ex.Message}");
                ok = false;
            }

            // Freeze guard: prove the exit instead of assuming it.
            if (SafeInVehicle(npc))
            {
                Print("NPC.IsInVehicle is still true after NPC.ExitVehicle() — trying LandVehicle.RemoveNPCOccupant(npc).");
                LandVehicle? occupiedVeh = SpikeState.Vehicle;
                if (occupiedVeh != null)
                {
                    try
                    {
                        occupiedVeh.RemoveNPCOccupant(npc);
                        Print("LandVehicle.RemoveNPCOccupant(npc) called.");
                    }
                    catch (Exception ex)
                    {
                        Mod.Log.Warn($"LandVehicle.RemoveNPCOccupant(npc) failed: {ex.Message}");
                    }
                }

                if (SafeInVehicle(npc))
                {
                    Mod.Log.Error("NPC is still in a vehicle after ExitVehicle + RemoveNPCOccupant — destroy refused (freeze guard). State kept, retry `taxi cleanup`.");
                    ok = false;
                }
            }
        }

        LandVehicle? veh = SpikeState.Vehicle;
        if (veh != null)
        {
            try
            {
                if (veh.LocalPlayerIsInVehicle)
                {
                    veh.ExitVehicle();
                    Print("LandVehicle.ExitVehicle() called (local player).");
                }
                else
                {
                    Print("Local player was not in the vehicle — skipped LandVehicle.ExitVehicle().");
                }

                // Freeze guard for the PLAYER (Stage 3d): destroying the vehicle
                // under a seated player is exactly the "player frozen in a car that
                // nobody drives" report — prove the exit, never assume it.
                if (SafePlayerInVehicle(veh))
                {
                    Mod.Log.Error("LocalPlayerIsInVehicle is still true after LandVehicle.ExitVehicle() — destroy refused (freeze guard). Press E/F9 (`taxi out`) and retry `taxi stop`.");
                    ok = false;
                }
            }
            catch (Exception ex)
            {
                Mod.Log.Error($"LandVehicle.ExitVehicle() failed: {ex.Message}");
                ok = false;
            }

            try
            {
                VehicleAgent? agent = veh.Agent;
                if (agent != null && agent.AutoDriving)
                {
                    agent.StopNavigating();
                    Print("VehicleAgent.StopNavigating() called before destroying the vehicle.");
                }
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"VehicleAgent.StopNavigating() during cleanup failed: {ex.Message}");
            }

            if (!ok)
            {
                Mod.Log.Error("An earlier cleanup step failed — DestroyVehicle refused (freeze guard), state kept. Fix the error above and retry `taxi cleanup`.");
            }
            else if (!ClearNpcOccupants(veh))
            {
                Mod.Log.Error("LandVehicle.OccupantNPCs still holds entries after RemoveNPCOccupant — DestroyVehicle refused (freeze guard), state kept. Retry `taxi cleanup`.");
                ok = false;
            }
            else
            {
                try
                {
                    veh.DestroyVehicle();
                    Print("LandVehicle.DestroyVehicle() called (verified: no NPC occupants left).");
                    // Stage 5: our driver goes back to the stand so the next ride
                    // starts from a known spot (dismiss = taxi gone, driver waiting).
                    TaxiDriverNPC.ReturnToStand();
                }
                catch (Exception ex)
                {
                    Mod.Log.Error($"LandVehicle.DestroyVehicle() failed: {ex.Message}");
                    ok = false;
                }
            }
        }

        if (ok)
        {
            SpikeState.Reset();
            Print("SpikeState cleared.");
        }
        else
        {
            Print("SpikeState kept (cleanup incomplete) — run `taxi cleanup` again to retry.");
        }

        return ok;
    }

    /// <summary>
    /// Removes every NPC from <c>LandVehicle.OccupantNPCs</c> and reports whether the
    /// list is empty afterwards — the gate for a safe <c>DestroyVehicle</c>.
    /// </summary>
    private static bool ClearNpcOccupants(LandVehicle veh)
    {
        var occupants = veh.OccupantNPCs;
        if (occupants == null)
            return true;

        for (int i = 0; i < occupants.Length; i++)
        {
            NPC? occupant = occupants[i];
            if (occupant == null)
                continue;

            try
            {
                veh.RemoveNPCOccupant(occupant);
                Print($"LandVehicle.RemoveNPCOccupant('{SafeId(occupant)}') called (slot {i}).");
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"LandVehicle.RemoveNPCOccupant('{SafeId(occupant)}') failed: {ex.Message}");
            }
        }

        var after = veh.OccupantNPCs;
        if (after == null)
            return true;

        int remaining = 0;
        for (int i = 0; i < after.Length; i++)
        {
            if (after[i] != null)
                remaining++;
        }

        if (remaining > 0)
            Mod.Log.Warn($"LandVehicle.OccupantNPCs still lists {remaining} NPC(s) after removal attempts.");

        return remaining == 0;
    }

    // -------------------------------------------------------------- visual

    /// <summary>
    /// Stage 2 switchboard for the GLB visual swap:
    /// <c>taxi visual</c> (status) / <c>taxi visual on|off</c> (next spawns) /
    /// <c>taxi visual align on|off</c> (bounds auto-alignment on the next spawn).
    /// Unknown modes and unknown align values both answer with a warning and
    /// change nothing (review M3-8).
    /// The swap itself runs inside <see cref="SpawnVehicle"/>; the on|off switch is
    /// also hotkey F4.
    /// </summary>
    internal static void Visual(string? mode, string? value)
    {
        if (!string.IsNullOrWhiteSpace(mode))
        {
            string m = mode!.Trim().ToLowerInvariant();
            if (m == "align")
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Print($"[visual] align = {SpikeState.VisualAutoAlign} (pass `taxi visual align on|off`).");
                    return;
                }

                string a = value.Trim().ToLowerInvariant();
                if (a is not ("on" or "off"))
                {
                    // Review M3-8: an unknown value must NOT be silently coerced to
                    // false (anything != "on" used to clear the flag) — it gets the
                    // same treatment as an unknown mode: warn, change nothing.
                    Print($"[visual] unknown value '{value}' — use `taxi visual align on|off` (align unchanged = {SpikeState.VisualAutoAlign}).");
                    return;
                }

                SpikeState.VisualAutoAlign = a == "on";
                Print($"[visual] align = {SpikeState.VisualAutoAlign} (applies to the next spawn).");
                return;
            }

            if (m is "on" or "off")
            {
                SpikeState.VisualSwapEnabled = m == "on";
                Print($"[visual] swap enabled = {SpikeState.VisualSwapEnabled} (applies to the next spawn).");
            }
            else
            {
                Print($"[visual] unknown argument '{mode}' — use `taxi visual [on|off]` or `taxi visual align [on|off]`.");
            }
        }

        string[] candidates = TaxiVisual.CandidatePaths();
        string? resolved = TaxiVisual.ResolveAssetPath();
        Print($"[visual] swap enabled = {SpikeState.VisualSwapEnabled}, align = {SpikeState.VisualAutoAlign}, swaps this session = {SpikeState.VisualSwaps}");
        Print($"[visual] GLB = {(resolved ?? "<NOT FOUND>")} (candidates: {candidates.Length})");
        Print("[visual] GLB root: " + (SpikeState.VisualRoot == null || SpikeState.VisualRoot.Pointer == IntPtr.Zero
            ? "<none attached>"
            : $"'{SpikeState.VisualRoot.name}' under '{SpikeState.VisualRoot.transform.parent?.name}'"));
    }

    // ---------------------------------------------------------------- probe

    /// <summary>
    /// Dumps the preconditions of <c>taxi go</c> for every vehicle in the world
    /// (review test finding 1, "Navigate fails silently"): Flags/Seeker state,
    /// graph sampling of start AND target, physics and ownership flags — the spike
    /// vehicle is marked so it can be compared against a vanilla vehicle that drives.
    /// </summary>
    internal static void Probe()
    {
        VehicleManager? vm = VehicleManager.Instance;
        if (vm == null)
        {
            Mod.Log.Error("VehicleManager.Instance is null — cannot probe vehicles.");
            return;
        }

        Player? player = Player.Local;
        Transform? playerTransform = player == null ? null : player.transform;
        Vector3 playerPos;
        Vector3 forward;
        if (playerTransform != null)
        {
            playerPos = playerTransform.position;
            forward = playerTransform.forward;
        }
        else
        {
            playerPos = Vector3.zero;
            forward = Vector3.forward;
        }

        if (forward.sqrMagnitude < 1e-4f)
            forward = Vector3.forward;

        Vector3 target = playerPos + forward * DefaultRideDistance;
        target.y = playerPos.y;

        Print("=== taxi probe: Navigate preconditions ===");
        Print($"  start=({Fmt(playerPos)}) probeTarget=({Fmt(target)}) — the same construction `taxi go {DefaultRideDistance:F0}` uses");
        PrintGraphSample("start", playerPos);
        PrintGraphSample("target", target);

        var all = vm.AllVehicles;
        if (all == null || all.Count == 0)
        {
            Print("  VehicleManager.AllVehicles is null/empty — no vehicle to compare against.");
            return;
        }

        Print($"  VehicleManager.AllVehicles: {all.Count} vehicle(s)");
        for (int i = 0; i < all.Count; i++)
        {
            LandVehicle? veh = all[i];
            if (veh == null)
            {
                Print($"  [{i}] <null entry>");
                continue;
            }

            ProbeOne(i, veh, ReferenceEquals(veh, SpikeState.Vehicle));
        }

        Print("=== end probe ===");
    }

    private static void ProbeOne(int index, LandVehicle veh, bool isSpikeVehicle)
    {
        try
        {
            Vector3 pos = veh.transform.position;
            Print($"  [{index}] {(isSpikeVehicle ? "SPIKE" : "other")} code='{codeOf(veh)}' name='{SafeName(veh)}' pos=({Fmt(pos)}) state={SafeStateName(veh)}");

            bool physicallySimulated = false;
            bool isOwner = false;
            try
            {
                physicallySimulated = veh.IsPhysicallySimulated;
                isOwner = veh.IsOwner;
            }
            catch (Exception ex)
            {
                Print($"      physics/owner getters threw: {ex.Message}");
            }

            Print($"      isPhysicallySimulated={physicallySimulated} isOwner={isOwner} isPlayerOwned={veh.IsPlayerOwned} " +
                  $"isParked={veh.isParked} brakes={veh.BrakesApplied} {TryDescribeGuid(veh)}");

            VehicleAgent? agent = veh.Agent;
            if (agent == null)
            {
                Print("      Agent: null — VehicleAgent.Navigate is unreachable for this vehicle.");
                return;
            }

            var flags = agent.Flags;
            string flagsText;
            if (flags == null)
            {
                flagsText = "NULL — Navigate would run without speed/stuck/obstacle configuration (prime suspect)";
            }
            else
            {
                flagsText = $"DriveFlags(useRoads={flags.UseRoads} stuckDetection={flags.StuckDetection} " +
                            $"autoBrakeAtDestination={flags.AutoBrakeAtDestination} obstacleMode={flags.ObstacleMode})";
            }

            Print($"      agent: enabled={agent.enabled} autoDriving={agent.AutoDriving} navCalc={agent.NavigationCalculationInProgress} " +
                  $"onVehicleGraph={agent.IsOnVehicleGraph()} stuck={agent.GetIsStuck()} targetLocation=({Fmt(agent.TargetLocation)})");
            Print($"      flags: {flagsText}");
            Print($"      seekers: roadSeeker={(agent.roadSeeker == null ? "NULL" : "ok")} generalSeeker={(agent.generalSeeker == null ? "NULL" : "ok")}");
            PrintGraphSample("vehiclePos", pos);
        }
        catch (Exception ex)
        {
            Print($"  [{index}] probe failed for this vehicle: {ex.Message}");
        }
    }

    private static void PrintGraphSample(string label, Vector3 point)
    {
        try
        {
            Vector3 sampled = NavigationUtility.SampleVehicleGraph(point);
            float delta = Vector3.Distance(sampled, point);
            Print($"  graph sample '{label}': point=({Fmt(point)}) sampled=({Fmt(sampled)}) delta={delta:F1}m" +
                  (delta > 10f ? " — far off the vehicle graph (prime suspect for a silent Navigate failure)" : string.Empty));
        }
        catch (Exception ex)
        {
            Print($"  graph sample '{label}' threw: {ex.Message}");
        }
    }

    // ---------------------------------------------------------------- trace

    /// <summary><c>taxi trace on|off|status</c> — Harmony trace of the Navigate dispatch.</summary>
    internal static void Trace(string? mode)
    {
        switch ((mode ?? "status").ToLowerInvariant())
        {
            case "on":
            case "enable":
                SpikeTrace.Enable();
                break;

            case "off":
            case "disable":
                SpikeTrace.Disable();
                break;

            default:
                Print($"trace: patched={SpikeTrace.Patched} logging={SpikeTrace.Logging} — usage: `taxi trace on` / `taxi trace off`");
                break;
        }
    }

    // -------------------------------------------------------------- helpers

    private static LandVehicle? RequireVehicle(string command)
    {
        LandVehicle? veh = SpikeState.Vehicle;
        if (veh != null)
            return veh;

        Mod.Log.Error($"No spike vehicle — run `taxi spawn` first (taxi {command} aborted, SpikeState.Vehicle is null).");
        return null;
    }

    /// <summary>Full seat dump: every seat, driver flag, occupancy and NPC occupants.</summary>
    private static void DumpSeats(string stage, LandVehicle veh, NPC? npc)
    {
        try
        {
            bool inVehicle = npc != null && SafeInVehicle(npc);
            LandVehicle? current = npc == null ? null : SafeVehicle(npc);
            Print($"[{stage}] npc.IsInVehicle={inVehicle} npc.CurrentVehicle!=null={(current != null)} " +
                  $"veh.IsOccupied={veh.IsOccupied} veh.LocalPlayerIsInVehicle={veh.LocalPlayerIsInVehicle}");

            VehicleSeat? free = SafeFirstFreeSeat(veh);
            Print($"[{stage}] LandVehicle.GetFirstFreeSeat() -> {(free == null ? "<none>" : SeatLabel(free))}");

            var seats = veh.Seats;
            if (seats == null)
            {
                Print($"[{stage}] LandVehicle.Seats is null.");
                return;
            }

            for (int i = 0; i < seats.Length; i++)
            {
                VehicleSeat? seat = seats[i];
                if (seat == null)
                {
                    Print($"[{stage}] seat[{i}]=<null>");
                    continue;
                }

                string name;
                try
                {
                    name = seat.gameObject != null ? seat.gameObject.name : "<no gameObject>";
                }
                catch (Exception ex)
                {
                    name = $"<gameObject.name threw: {ex.Message}>";
                }

                string occupant;
                try
                {
                    occupant = seat.Occupant == null ? "none" : seat.Occupant.name;
                }
                catch (Exception ex)
                {
                    occupant = $"<threw: {ex.Message}>";
                }

                Print($"[{stage}] seat[{i}] name='{name}' isDriverSeat={seat.isDriverSeat} " +
                      $"isOccupied={seat.isOccupied} playerOccupant='{occupant}'");
            }

            // Physical seat check: which seat does the NPC root actually sit on?
            // Independent of VehicleSeat.isOccupied (Player-only) and of any
            // assumption that OccupantNPCs[i] and Seats[i] share an index.
            if (npc != null)
            {
                int closest = ClosestSeatIndex(veh, npc, out string distances, out float closestDistance);
                Print($"[{stage}] npc root-to-seat {distances} -> closest seat[{closest}] ({closestDistance:F1}m)");
            }

            var occupants = veh.OccupantNPCs;
            if (occupants == null)
            {
                Print($"[{stage}] LandVehicle.OccupantNPCs is null.");
                return;
            }

            int capacity = occupants.Length;
            int filled = 0;
            int npcSlot = -1;
            for (int i = 0; i < occupants.Length; i++)
            {
                NPC? occupantNpc = occupants[i];
                if (occupantNpc == null)
                    continue;

                filled++;
                if (npc != null && (ReferenceEquals(occupantNpc, npc) ||
                                    string.Equals(SafeId(occupantNpc), SafeId(npc), StringComparison.Ordinal)))
                    npcSlot = i;

                Print($"[{stage}]   occupantNpc[{i}] id='{SafeId(occupantNpc)}' isInVehicle={SafeInVehicle(occupantNpc)}");
            }

            // Review M4: the array length is the seat count (= capacity), never the occupancy.
            Print($"[{stage}] LandVehicle.OccupantNPCs capacity={capacity} (array length = seat count, NOT occupancy) filled={filled}" +
                  (npc != null ? $" thisNpcAtSlot={npcSlot}" : string.Empty));
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"[{stage}] seat dump failed while reading LandVehicle.Seats/OccupantNPCs: {ex.Message}");
        }
    }

    private static string SeatLabel(VehicleSeat seat)
    {
        try
        {
            string name = seat.gameObject != null ? seat.gameObject.name : "<no gameObject>";
            return $"'{name}' isDriverSeat={seat.isDriverSeat} isOccupied={seat.isOccupied}";
        }
        catch (Exception ex)
        {
            return $"<unreadable: {ex.Message}>";
        }
    }

    private static VehicleSeat? SafeFirstFreeSeat(LandVehicle veh)
    {
        try
        {
            return veh.GetFirstFreeSeat();
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"LandVehicle.GetFirstFreeSeat() failed: {ex.Message}");
            return null;
        }
    }

    private static string codeOf(LandVehicle veh)
    {
        try
        {
            return veh.VehicleCode;
        }
        catch (Exception ex)
        {
            return $"<VehicleCode threw: {ex.Message}>";
        }
    }

    private static string SafeName(LandVehicle veh)
    {
        try
        {
            return veh.VehicleName;
        }
        catch (Exception ex)
        {
            return $"<VehicleName threw: {ex.Message}>";
        }
    }

    private static string SafeId(NPC npc)
    {
        try
        {
            return npc.ID;
        }
        catch (Exception ex)
        {
            return $"<ID threw: {ex.Message}>";
        }
    }

    private static bool SafeInVehicle(NPC npc)
    {
        try
        {
            return npc.IsInVehicle;
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"NPC.IsInVehicle getter failed: {ex.Message}");
            return false;
        }
    }

    private static LandVehicle? SafeVehicle(NPC npc)
    {
        try
        {
            return npc.CurrentVehicle;
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"NPC.CurrentVehicle getter failed: {ex.Message}");
            return null;
        }
    }

    private static string SafeAuto(VehicleAgent agent)
    {
        try
        {
            return agent.AutoDriving.ToString();
        }
        catch (Exception ex)
        {
            return $"<AutoDriving threw: {ex.Message}>";
        }
    }

    /// <summary>
    /// Typed GUID lookup (review: the reflection fallback printed the zero GUID for
    /// every vehicle). <c>LandVehicle.GUID</c> is a public property; an all-zero value
    /// means the vehicle is not network-registered yet.
    /// </summary>
    private static string TryDescribeGuid(LandVehicle veh)
    {
        try
        {
            string text = veh.GUID.ToString() ?? string.Empty;
            return LooksLikeEmptyGuid(text)
                ? "GUID=<empty — vehicle not network-registered>"
                : $"GUID={text}";
        }
        catch (Exception ex)
        {
            return $"<LandVehicle.GUID threw: {ex.Message}>";
        }
    }

    private static bool LooksLikeEmptyGuid(string text)
    {
        if (string.IsNullOrEmpty(text))
            return true;

        foreach (char c in text)
        {
            if (c != '0' && c != '-' && c != '{' && c != '}' && c != ' ')
                return false;
        }

        return true;
    }

    /// <summary>
    /// Index of <paramref name="npc"/> in <c>LandVehicle.OccupantNPCs</c> (-1 when
    /// absent) plus the capacity/fill counts. Matching falls back to the NPC id
    /// because IL2CPP wrappers for one native object are not guaranteed identical.
    /// </summary>
    private static int OccupantIndexOf(LandVehicle veh, NPC npc, out int capacity, out int filled)
    {
        capacity = -1;
        filled = -1;

        try
        {
            var occupants = veh.OccupantNPCs;
            if (occupants == null)
                return -1;

            capacity = occupants.Length;
            filled = 0;

            string npcId = SafeId(npc);
            int found = -1;
            for (int i = 0; i < occupants.Length; i++)
            {
                NPC? occupant = occupants[i];
                if (occupant == null)
                    continue;

                filled++;
                if (found < 0 && (ReferenceEquals(occupant, npc) ||
                                  string.Equals(SafeId(occupant), npcId, StringComparison.Ordinal)))
                    found = i;
            }

            return found;
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"LandVehicle.OccupantNPCs could not be read: {ex.Message}");
            return -1;
        }
    }

    /// <summary>
    /// One-line occupancy proof built from facts that do NOT depend on
    /// <c>VehicleSeat.isOccupied</c>: the NPC's slot in
    /// <see cref="LandVehicle.OccupantNPCs"/> and the measured distance from the NPC
    /// <b>root transform</b> to every seat transform (root-to-seat — the physical seat
    /// it sits on). The distance is measured, never assumed: it does not imply that
    /// <c>OccupantNPCs[i]</c> and <c>Seats[i]</c> share an index (that mapping is not
    /// directly observable in 0.4.7f6, see docs/README.md "Index mapping").
    /// The <c>driver seat claimed</c> wording is only printed when three facts agree —
    /// an occupant slot (<c>slot &gt;= 0</c>), a known driver seat
    /// (<c>driverIdx &gt;= 0</c>) and a measured closest seat equal to it; every other
    /// combination prints an explicit <c>driver seat NOT proven</c> verdict with its
    /// reason. The <c>isDriverSeat</c> claim is likewise only printed for
    /// <c>driverIdx &gt;= 0</c>.
    /// </summary>
    private static void PrintSeatProof(string stage, LandVehicle veh, NPC npc)
    {
        int slot = OccupantIndexOf(veh, npc, out int capacity, out int filled);
        int driverIdx = SafeDriverSeatIndex(veh, out string driverSeatName, out bool driverOccupied);
        int closestIdx = ClosestSeatIndex(veh, npc, out string distances, out float closestDistance);

        // Review B1-a: IsInVehicle + CurrentVehicle alone never prove the occupant
        // slot, so the claim additionally requires slot >= 0.
        bool measuredOnDriverSeat = closestIdx >= 0 && closestIdx == driverIdx;

        string verdict;
        if (measuredOnDriverSeat && slot >= 0)
        {
            verdict = $"NPC root sits on the DRIVER seat (seat[{closestIdx}], {closestDistance:F1}m) and OccupantNPCs[{slot}] holds it " +
                      $"-> driver seat claimed. VehicleSeat.isOccupied={driverOccupied} is expected: VehicleSeat.Occupant is typed Player, " +
                      "so an NPC can never be stored on the seat (NPC occupancy lives in LandVehicle.OccupantNPCs).";
        }
        else if (measuredOnDriverSeat)
        {
            // Review B1-a: same measurement, but no occupant slot — not proven.
            verdict = $"NPC root sits on the DRIVER seat (seat[{closestIdx}], {closestDistance:F1}m) but the NPC fills no OccupantNPCs slot " +
                      $"(occupantSlot={slot}, filled={filled}/{capacity}) -> driver seat NOT proven (seating incomplete) — see the seat dump.";
        }
        else if (closestIdx >= 0 && driverIdx >= 0)
        {
            // Review B1-c: passenger/other-seat case — neutral wording, no
            // OccupantNPCs[i] <-> Seats[i] mapping is implied.
            verdict = $"NPC root sits on seat[{closestIdx}] ({closestDistance:F1}m), NOT on the driver seat[{driverIdx}] " +
                      $"(occupantSlot={slot}) -> driver seat NOT claimed — see the seat dump and the in-game capture.";
        }
        else
        {
            verdict = $"driver seat NOT proven (occupantSlot={slot}, closest seat={closestIdx}, driver seat={driverIdx}) — see the seat dump.";
        }

        // Review B1-b: isDriverSeat is only stated for a real driver seat index.
        string driverSeatPart = driverIdx >= 0
            ? $"seat[{driverIdx}] isDriverSeat=True name='{driverSeatName}' isOccupied={driverOccupied}"
            : $"driver seat NOT identified (driverIdx={driverIdx}, name='{driverSeatName}')";

        string slotPart = slot >= 0
            ? $"at OccupantNPCs[{slot}] (filled={filled}/{capacity})"
            : $"not in OccupantNPCs (slot=-1, filled={filled}/{capacity})";

        Print($"[{stage}] seat proof: {driverSeatPart}; " +
              $"NPC '{SafeId(npc)}' {slotPart}; root-to-seat {distances} -> {verdict}");
    }

    /// <summary>Index of the seat flagged <c>isDriverSeat</c> (-1 when none/unreadable) plus its name and Player occupancy.</summary>
    private static int SafeDriverSeatIndex(LandVehicle veh, out string seatName, out bool occupied)
    {
        seatName = "<no driver seat>";
        occupied = false;
        try
        {
            var seats = veh.Seats;
            if (seats == null)
            {
                seatName = "<Seats is null>";
                return -1;
            }

            for (int i = 0; i < seats.Length; i++)
            {
                VehicleSeat? seat = seats[i];
                if (seat == null || !seat.isDriverSeat)
                    continue;

                seatName = seat.gameObject != null ? seat.gameObject.name : "<no gameObject>";
                occupied = seat.isOccupied;
                return i;
            }

            return -1;
        }
        catch (Exception ex)
        {
            seatName = $"<threw: {ex.Message}>";
            return -1;
        }
    }

    /// <summary>
    /// Index of the seat whose transform is nearest to <paramref name="npc"/>'s root
    /// transform — i.e. the seat the NPC physically sits on — plus a compact per-seat
    /// root-to-seat distance list and the winning distance. Returns -1 when either
    /// side is unreadable.
    /// </summary>
    private static int ClosestSeatIndex(LandVehicle veh, NPC npc, out string distances, out float closestDistance)
    {
        distances = "<no distances>";
        closestDistance = -1f;
        int closest = -1;

        try
        {
            var seats = veh.Seats;
            if (seats == null)
            {
                distances = "<Seats is null>";
                return -1;
            }

            if (npc == null || npc.transform == null)
            {
                distances = "<npc transform unavailable>";
                return -1;
            }

            Vector3 root = npc.transform.position;
            var parts = new System.Collections.Generic.List<string>(seats.Length);
            for (int i = 0; i < seats.Length; i++)
            {
                VehicleSeat? seat = seats[i];
                if (seat == null || seat.transform == null)
                {
                    parts.Add($"[{i}]=<null>");
                    continue;
                }

                float distance = Vector3.Distance(root, seat.transform.position);
                parts.Add($"[{i}]={distance:F1}m");
                if (closest < 0 || distance < closestDistance)
                {
                    closest = i;
                    closestDistance = distance;
                }
            }

            distances = string.Join(" ", parts);
            return closest;
        }
        catch (Exception ex)
        {
            distances = $"<threw: {ex.Message}>";
            return -1;
        }
    }

    /// <summary>
    /// Runtime name of the vehicle's current state (review: the raw interpolation
    /// printed <c>Il2CppScheduleOne.State.MonoState</c>, a type name instead of a state).
    /// </summary>
    private static string SafeStateName(LandVehicle veh)
    {
        try
        {
            var state = veh.State;
            return state == null ? "null" : state.GetType().Name;
        }
        catch (Exception ex)
        {
            return $"<State threw: {ex.Message}>";
        }
    }

    internal static string Fmt(Vector3 v) => $"({v.x:F1}, {v.y:F1}, {v.z:F1})";

    /// <summary>
    /// Scalar counterpart of <see cref="Fmt(Vector3)"/> for the single Y component that the
    /// float-fix diagnostics chase. Invariant culture is mandatory: the log line is grepped,
    /// and the DE locale would emit <c>,</c> as the decimal separator.
    /// </summary>
    internal static string FmtY(float y) => y.ToString("F3", CultureInfo.InvariantCulture);

    /// <summary>
    /// World-space corners of <see cref="LandVehicle.boundingBox"/> — the deterministic prefab
    /// BoxCollider. This is the alignment reference for the GLB swap, deliberately NOT
    /// <c>Renderer.bounds</c>: those are refreshed only during culling and are STALE on the spawn
    /// frame, which previously left the taxi floating (measured −0.63 m y delta and a ~41 m xz
    /// offset pointing at the pre-teleport pool location).
    /// </summary>
    /// <returns><c>false</c> when the vehicle or its BoxCollider is unavailable/destroyed —
    /// callers then skip the alignment instead of computing with NaN.</returns>
    internal static bool TryGetVehicleBoxWorldBounds(LandVehicle veh, out Vector3 min, out Vector3 max)
    {
        min = Vector3.zero;
        max = Vector3.zero;

        if (veh == null || veh.Pointer == IntPtr.Zero || veh.WasCollected)
            return false;

        try
        {
            BoxCollider box = veh.boundingBox;
            if (box == null || box.Pointer == IntPtr.Zero || box.WasCollected)
                return false;

            Transform t = box.transform;
            if (t == null || t.Pointer == IntPtr.Zero)
                return false;

            Vector3 half = box.size * 0.5f;
            Vector3 a = t.TransformPoint(box.center - half);
            Vector3 b = t.TransformPoint(box.center + half);
            min = Vector3.Min(a, b);
            max = Vector3.Max(a, b);
            return true;
        }
        catch (Exception ex)
        {
            Print($"[bounds] boundingBox read failed: {ex.Message}");
            return false;
        }
    }
}
