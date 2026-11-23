using System.IO;
using MelonLoader;
using MelonLoader.Utils;
using S1API.Lifecycle;
using UnityEngine;
using UnityEngine.SceneManagement;

[assembly: MelonInfo(typeof(DowntownWarehouse.Mod), "DowntownWarehouse", "0.1.0", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace DowntownWarehouse;

/// <summary>
/// Publishes the native warehouse hall behind Hank's Hardware as a purchasable
/// singleplayer property with an interior shell, working door, save point, build
/// grid, and slot-scoped placement persistence.
/// </summary>
public sealed class Mod : MelonMod
{
    public override void OnInitializeMelon()
    {
        GameLifecycle.OnLoadComplete += OnLoadComplete;
        GameLifecycle.OnPreLoad += OnTeardown;
        GameLifecycle.OnPreSceneChange += OnTeardown;
        MelonLogger.Msg("[DowntownWarehouse] initialisiert (v0.1.0).");
    }

    public override void OnDeinitializeMelon()
    {
        GameLifecycle.OnLoadComplete -= OnLoadComplete;
        GameLifecycle.OnPreLoad -= OnTeardown;
        GameLifecycle.OnPreSceneChange -= OnTeardown;
        HallPropertyHost.DisposePatches();
        HallPropertyHost.Shutdown();
    }

    private void OnLoadComplete()
    {
        try
        {
            if (SceneManager.GetActiveScene().name != "Main")
                return;
            if (File.Exists(Path.Combine(MelonEnvironment.UserDataDirectory, "DowntownWarehouse", "run-probe.flag")))
                HallProbe.RunOnce();
            HallPropertyHost.Publish();
        }
        catch (System.Exception ex)
        {
            MelonLogger.Warning("[DowntownWarehouse] Publish fehlgeschlagen: " + ex.Message);
        }
    }

    private void OnTeardown()
    {
        try
        {
            HallPropertyHost.Shutdown();
        }
        catch (System.Exception ex)
        {
            MelonLogger.Warning("[DowntownWarehouse] Teardown: " + ex.Message);
        }
    }

    public override void OnUpdate()
    {
        HallDoor.Tick();
    }
}
