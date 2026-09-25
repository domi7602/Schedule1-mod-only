using System;
using System.Globalization;
using Il2CppFishNet;
using Il2CppScheduleOne.NPCs;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppScheduleOne.Vehicles;
using Il2CppScheduleOne.Vehicles.AI;
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
        Print("        output-only; control the spike via the F6-F17 hotkeys listed at the end.");
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
        Print("  taxi visual [on|off]   - Stage 2 GLB visual swap status / switch (also `taxi visual align [on|off]`);");
        Print("                           the swap itself runs automatically inside `taxi spawn` — the switch is also hotkey F16");
        Print("  aliases: taxi list = codes, taxi driver = npc, taxi in = ride, taxi out = exit, taxi reset = cleanup.");
        Print("Hotkeys (the ONLY in-game control surface — the console cannot be typed into):");
        Print("  F6  = full spike run: spawn -> +1s npc -> +1s go 40 (blind forward target)");
        Print("  F7  = taxi probe");
        Print("  F8  = taxi trace on/off toggle");
        Print("  F9  = player ride/out toggle");
        Print("  F10 = taxi go2 (Navigate with settings=null)");
        Print("  F11 = full run WITHOUT the npc step (occupant hypothesis)");
        Print("  F12 = Navigate on a VANILLA vehicle (vehicle-vs-caller A/B)");
        Print("  F13 = taxi go to the proven road target (-131.4, -4.0, 51.9)");
        Print("  F14 = taxi go to the second proven road target (-17.1, 0.0, 13.4)");
        Print("  F15 = full run (spawn -> npc) to the proven road target (-131.4, -4.0, 51.9)");
        Print("  F16 = taxi visual on/off toggle (applies to the NEXT spawn)");
        Print("  F17 = call-taxi (Stage 3b): spawn at the FIXED taxi stand -> npc -> navigate to a road point near the player");
        Print("A leading '/' works too (both `taxi` and `/taxi` are registered).");
        Print("F13-F17 only reach the game through real key input (SendInput, e.g.");
        Print("        focus_key.py — VK_F13=0x7C, F14=0x7D, F15=0x7E, F16=0x7F, F17=0x80).");
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
        // F6/F15 spawn can inherit it (the F17 run itself re-arms it before step 1).
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

        // Review M6: the old ray started at the PLAYER and had no filters, so props,
        // vehicles and the player's own collider could decide the spawn height. It now
        // starts above the spawn point and only accepts static, non-trigger surfaces.
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
        SpikeState.LastSpawnPosition = spawnPos;
        SpikeState.ResetNavigation();

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

                best = hit;
                info = $"hit '{hit.collider.name}' layer={LayerMask.LayerToName(hit.collider.gameObject.layer)} " +
                       $"at {hit.distance:F1} m below the ray origin ({hits.Length} hits, skipped {skippedTriggers} trigger / {skippedDynamic} rigidbody)";
                return true;
            }

            info = $"all {hits.Length} hits filtered out (skipped {skippedTriggers} trigger / {skippedDynamic} rigidbody)";
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

        string npcId = SafeId(best);
        Print($"Nearest usable NPC: id='{npcId}' distance={bestDistance:F1}m (registry={registry.Count}, skipped={skipped})");

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

    /// <summary>Puts the local player into the spike vehicle.</summary>
    internal static bool Ride()
    {
        LandVehicle? veh = RequireVehicle("ride");
        if (veh == null)
            return false;

        try
        {
            veh.EnterVehicle();
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"LandVehicle.EnterVehicle() failed: {ex.Message}");
            return false;
        }

        bool inVehicle = veh.LocalPlayerIsInVehicle;
        Print($"LandVehicle.EnterVehicle() returned. LocalPlayerIsInVehicle={inVehicle} " +
              $"IsOccupied={veh.IsOccupied} DriverPlayer={(veh.DriverPlayer == null ? "null" : veh.DriverPlayer.name)}");
        Print(inVehicle
            ? "[primitive c] PROOF: LocalPlayerIsInVehicle=true — the local player is in the spike vehicle."
            : "[primitive c] NOT PROVEN: LocalPlayerIsInVehicle is still false after LandVehicle.EnterVehicle() — re-run `taxi ride` once the world has settled a frame.");

        // Which seat did the player take? This is the live evidence for the seat
        // semantics: VehicleSeat.isOccupied only ever flips for a Player, and it
        // shows whether the player lands on the driver seat the NPC already uses.
        DumpSeats("after ride", veh, SpikeState.DriverNpc);
        return true;
    }

    /// <summary>Takes the local player back out of the spike vehicle.</summary>
    internal static bool Out()
    {
        LandVehicle? veh = RequireVehicle("out");
        if (veh == null)
            return false;

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
        return true;
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

        // Round 13: the spawn forward (0.7,0,0.7) pointed AWAY from the target —
        // the agent burned 54/89 polls on reverse maneuvers and never arrived.
        // Turn the vehicle toward the destination before dispatching.
        try
        {
            Vector3 flatToTarget = target - veh.transform.position;
            flatToTarget.y = 0f;
            if (flatToTarget.sqrMagnitude > 1f)
            {
                Vector3 before = veh.transform.forward;
                veh.transform.rotation = Quaternion.LookRotation(flatToTarget.normalized, Vector3.up);
                Print($"[diag] orientation forced toward target (forward {Fmt(before)} → {Fmt(veh.transform.forward)}).");
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[diag] orientation step failed (continuing): {ex.Message}");
        }

        NavigationSettings? settings = null;
        if (useSettings)
        {
            try
            {
                settings = new NavigationSettings
                {
                    endAtRoad = true,
                    ensureProximityToGraph = true,
                    teleportToGraphIfCalculationFails = true,
                };
            }
            catch (Exception ex)
            {
                Mod.Log.Error($"new NavigationSettings() (endAtRoad/ensureProximityToGraph/teleportToGraphIfCalculationFails) failed: {ex.Message}");
                return false;
            }
        }

        float startDistance = Vector3.Distance(veh.transform.position, target);
        Print($"Navigate(settings={(settings == null ? "null" : "NavigationSettings(endAtRoad=true, ensureProximityToGraph=true, teleportToGraphIfCalculationFails=true)")}) requestedDistance={distance:F1}m " +
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
        // Stage 3b: gates the "taxi arrived at player" verdict for the F17 run.
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

        Print("VehicleAgent.Navigate dispatched — polling every 0.5 s plus an ENavigationResult callback (`taxi stop` cancels; a settings=null retry auto-fires at 3 s when nothing ever drove).");
        return true;
    }

    /// <summary>Cancels navigation and the polling.</summary>
    internal static bool Stop()
    {
        LandVehicle? veh = RequireVehicle("stop");
        if (veh == null)
            return false;

        VehicleAgent? agent = veh.Agent;
        if (agent == null)
        {
            SpikeState.PollingActive = false;
            Mod.Log.Error("LandVehicle.Agent is null — cannot call VehicleAgent.StopNavigating() (polling disabled anyway).");
            return false;
        }

        try
        {
            agent.StopNavigating();
        }
        catch (Exception ex)
        {
            SpikeState.PollingActive = false;
            Mod.Log.Error($"VehicleAgent.StopNavigating() failed: {ex.Message}");
            return false;
        }

        SpikeState.PollingActive = false;
        Print("VehicleAgent.StopNavigating() called — navigation polling disabled.");
        return true;
    }

    // -------------------------------------------------------- stand / player target

    /// <summary><c>taxi lots</c> — dump every live <c>ParkingLot</c> (the Stage-3b stand candidates).</summary>
    internal static void Lots() => TaxiStand.DumpLots("manual `taxi lots` dump");

    /// <summary>
    /// <c>taxi stand</c> / F17 step 0 — resolves the fixed taxi stand and arms it for
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
    internal static bool Cleanup()
    {
        bool ok = true;
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
    /// also hotkey F16.
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
}
