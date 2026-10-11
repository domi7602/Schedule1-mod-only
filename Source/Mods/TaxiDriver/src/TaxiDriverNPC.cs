using S1API.Entities;
using S1API.Entities.Appearances.AccessoryFields;
using S1API.Entities.Appearances.BodyLayerFields;
using UnityEngine;

namespace TaxiDriver;

/// <summary>
/// The taxi's own driver: a dedicated S1API NPC (physical, fixed id
/// <c>taxi_driver</c>) that waits at the taxi stand and drives every ride.
///
/// Design (Dominik): no random bystander is boarded any more. The driver is
/// visibly dressed as a taxi driver (2026-09-29: he used to show up in underwear
/// because the avatar renderers were only hidden once and the game rebuilt them
/// later) — white button-up, dark jeans, sneakers. The game's own patrol
/// behaviour (<see cref="TaxiAI"/>) drives the car. After a dismiss the NPC is
/// warped back to the stand (see <see cref="ReturnToStand"/>); it is never
/// despawned because S1API owns custom-NPC lifetime (spawn at load,
/// save-bound id).
/// </summary>
public sealed class TaxiDriverNPC : NPC
{
    public const string NPC_ID = "taxi_driver";

    public override bool IsPhysical => true;

    protected override void ConfigurePrefab(NPCPrefabBuilder builder)
    {
        builder.WithIdentity(NPC_ID, "Taxi", "Driver")
            .WithSpawnPosition(TaxiStand.StandCoordinate)
            .WithAppearanceDefaults(av =>
            {
                av.Gender = 0.5f;
                av.Height = 1.0f;
                av.Weight = 0.5f;

                // Outfit (Dominik 2026-09-29: "der Fahrer ist komplett nackt ...")
                // — a proper driver's look, applied at prefab time so it survives
                // every avatar rebuild (the old render-hide did not).
                av.WithBodyLayer<Shirts>(Shirts.Buttonup, new Color(0.85f, 0.87f, 0.90f));
                av.WithBodyLayer<Pants>(Pants.Jeans, new Color(0.15f, 0.20f, 0.35f));
                av.WithAccessoryLayer<Feet>(Feet.Sneakers, new Color(0.12f, 0.12f, 0.14f));
            })
            .WithSchedule(plan =>
            {
                // Wait at the stand. warpIfSkipped covers spawning after midnight.
                plan.WalkTo(TaxiStand.StandCoordinate, 0, warpIfSkipped: true);
            });
    }

    private static Il2CppScheduleOne.NPCs.NPC? _nativeDriver;
    private static TaxiDriverNPC? _registeredOwner;

    protected override void OnCreated()
    {
        base.OnCreated();
        ResolveNativeDriver();
        ApplyProtection(_nativeDriver);
        Appearance.Build();
        Schedule.Enable();
        ClearConversationCategories();
    }

    protected override void OnLoaded()
    {
        base.OnLoaded();
        ResolveNativeDriver();
        ApplyProtection(_nativeDriver);
    }

    protected override void OnDestroyed()
    {
        if (System.Object.ReferenceEquals(_registeredOwner, this))
        {
            _nativeDriver = null;
            _registeredOwner = null;
        }
        base.OnDestroyed();
    }

    private void ResolveNativeDriver()
    {
        try
        {
            _registeredOwner = this;
            _nativeDriver = null;
            _nativeDriver = gameObject.GetComponent<Il2CppScheduleOne.NPCs.NPC>();
        }
        catch (System.Exception ex)
        {
            Mod.Log.Warn($"[driver] Native driver resolution failed: {ex.Message}");
        }
    }

    internal static bool IsOurVanilla(Il2CppScheduleOne.NPCs.NPC? npc)
    {
        try
        {
            return S1Mods.Shared.NetworkGuard.IsAlive(npc) &&
                S1Mods.Shared.NetworkGuard.IsAlive(_nativeDriver) &&
                DriverBoardingGate.IsDedicatedDriver(npc!.Pointer, _nativeDriver!.Pointer);
        }
        catch { return false; }
    }

    private static bool ApplyProtection(Il2CppScheduleOne.NPCs.NPC? npc)
    {
        try
        {
            if (!IsOurVanilla(npc) || !S1Mods.Shared.NetworkGuard.IsAlive(npc!.Health))
                return false;
            var health = npc.Health;
            // f12 exposes _invincible. S1API.IsInvincible still looks up the old
            // Invincible member and can silently no-op; use the verified native field.
            bool changed = !health._invincible;
            health._invincible = true;
            if (changed) Mod.Log.Info("[driver] taxi_driver protection enabled (native health field read back).");
            return health._invincible;
        }
        catch (System.Exception ex)
        {
            Mod.Log.Warn($"[driver] Could not confirm Taxi Driver protection: {ex.Message}");
            return false;
        }
    }

    /// <summary>Recover only the existing owned driver, on the host, never spawn/replace one.</summary>
    internal static bool TryPrepareForService(Il2CppScheduleOne.NPCs.NPC npc)
    {
        if (!SpikeCommands.HasAuthority() || !ApplyProtection(npc))
            return false;
        try
        {
            var health = npc.Health;
            if (health.IsDead || health.IsKnockedOut)
            {
                Mod.Log.Info("[driver] Recovering the existing taxi_driver from saved dead/knocked-out state; no replacement spawn.");
                health.Revive();
            }
            return !health.IsDead && !health.IsKnockedOut && health._invincible;
        }
        catch (System.Exception ex)
        {
            Mod.Log.Warn($"[driver] Existing Taxi Driver could not be restored: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// The S1API wrapper instance, or null while custom NPCs are not ready
    /// (main menu, no save loaded).
    /// </summary>
    internal static TaxiDriverNPC? GetOurs()
    {
        try
        {
            return NPC.Get<TaxiDriverNPC>() as TaxiDriverNPC;
        }
        catch (System.Exception ex)
        {
            Mod.Log.Warn($"Taxi driver: lookup failed ({ex.GetType().Name}: {ex.Message}).");
            return null;
        }
    }

    /// <summary>
    /// The underlying vanilla NPC (what <c>EnterVehicle</c>, the occupant slots
    /// and <see cref="TaxiAI"/> operate on), or null when unavailable.
    /// </summary>
    internal static Il2CppScheduleOne.NPCs.NPC? TryGetVanilla()
    {
        try
        {
            if (S1Mods.Shared.NetworkGuard.IsAlive(_nativeDriver))
                return _nativeDriver;
            var ours = GetOurs();
            if (ours == null)
                return null;
            ours.ResolveNativeDriver();
            return S1Mods.Shared.NetworkGuard.IsAlive(_nativeDriver) ? _nativeDriver : null;
        }
        catch (System.Exception ex)
        {
            Mod.Log.Warn($"Taxi driver: vanilla NPC resolve failed ({ex.GetType().Name}: {ex.Message}).");
            return null;
        }
    }

    /// <summary>
    /// Warps our driver back to the taxi stand (called after a successful
    /// dismiss-destroy so the next ride starts from a known spot).
    /// </summary>
    internal static void ReturnToStand()
    {
        var ours = GetOurs();
        if (ours == null)
        {
            Mod.Log.Warn("Taxi driver: ReturnToStand skipped — custom NPC not available.");
            return;
        }

        try
        {
            ours.Position = TaxiStand.StandCoordinate;
            Mod.Log.Info("Taxi driver: warped back to the stand.");
        }
        catch (System.Exception ex)
        {
            Mod.Log.Warn($"Taxi driver: warp back to the stand failed ({ex.GetType().Name}: {ex.Message}).");
        }
    }
}
