using Xunit;
namespace PoliceTestMode.Tests;
public class RuntimeContractTests
{
    internal static string Source(string file)
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null && !Directory.Exists(Path.Combine(dir.FullName, "Source", "Mods"))) dir = dir.Parent;
        Assert.NotNull(dir);
        string path = Path.Combine(dir.FullName, "Source", "Mods", "PoliceTestMode", "src", file);
        Assert.True(File.Exists(path), $"Missing integration: {file}");
        return File.ReadAllText(path);
    }

    [Fact]
    public void Hooks_cover_police_dispatch_activities_curfew_and_vehicle_sightings_without_global_shutdown()
    {
        string source = Source("PoliceHooks.cs");
        foreach (string required in new[] { "typeof(PoliceStation)", "Dispatch", "typeof(LawActivitySettings)", "typeof(CurfewInstance)", "typeof(VehiclePatrolInstance)", "typeof(CheckpointInstance)", "typeof(SentryInstance)", "typeof(PoliceOfficer)", "ProcessVisionEvent", "BeginVehiclePursuit", "typeof(NPCResponses_Police)", "NoticedViolatingCurfew", "get_IsHardCurfewActive", "Harmony.GetPatchInfo", "UnpatchSelf" })
            Assert.Contains(required, source);
        Assert.DoesNotContain("typeof(NPCBehaviour)", source);
        Assert.DoesNotContain("typeof(VehicleAgent)", source);
        Assert.DoesNotContain("SetInternalIntensity", source);
    }

    [Fact]
    public void Runtime_is_default_on_singleplayer_bounded_and_releases_lifecycle_subscriptions()
    {
        string source = Source("Mod.cs");
        foreach (string required in new[] { "Requested = true", "SessionPolicy.ShouldSuppress", "lobby.IsInLobby", "Player.PlayerList.Count == 1", "ReadinessLimit", "OnLoadComplete +=", "OnLoadComplete -=", "OnPreLoad +=", "OnPreLoad -=", "OnPreSceneChange +=", "OnPreSceneChange -=", "policetest", "Restore" }) Assert.Contains(required, source);
        Assert.DoesNotContain("override void OnGUI", source);
        Assert.DoesNotContain("GUI.Box", source);
        Assert.Contains("internal void Status()", source);
    }

    [Fact]
    public void Unknown_occupancy_is_visibly_partial_not_active()
    {
        string source = Source("Mod.cs");
        Assert.Contains("_world.UnknownVehicles > 0", source);
        Assert.Contains("PARTIAL - vehicle occupancy unreadable", source);
        Assert.Contains("unknownSkipped={_world.UnknownVehicles}", source);
        Assert.Contains("playerOccupiedSkipped={_world.OccupiedVehicles}", source);
    }

    [Fact]
    public void World_uses_only_police_registries_and_restorable_leases()
    {
        string source = Source("PoliceWorld.cs");
        foreach (string required in new[] { "PoliceOfficer.Officers", "PoliceStation.PoliceStations", "deployedVehicles", "BoolLease", "Restore", "ReadPlayerOccupancy(vehicle)", "VehicleOccupancy.Clear", "UnknownVehicles++", "MaxEntries", "_IsCurrentlyActive_k__BackingField", "_IsHardCurfewActive_k__BackingField", "_IsEnabled_k__BackingField", "activeTargets=", "curfewRaw=" }) Assert.Contains(required, source);
        foreach (string forbidden in new[] { "FindObjects", "vehicle.IsOccupied", "Destroy(", "SetInternalIntensity", "WriteAllText", "Save(" }) Assert.DoesNotContain(forbidden, source);
    }
}
