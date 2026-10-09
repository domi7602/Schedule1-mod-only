using System;
using Il2CppInterop.Runtime.Injection;
using MelonLoader;
using S1API.Lifecycle;

[assembly: MelonInfo(typeof(StorageScanner.Mod), "StorageScanner", "0.2.0", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace StorageScanner
{
    /// <summary>Mod entry point: registers the injected input helper and pipes save/scene events to the app.</summary>
    public class Mod : MelonMod
    {
        public override void OnInitializeMelon()
        {
            try
            {
                ClassInjector.RegisterTypeInIl2Cpp<StorageScannerInputFocus>();
            }
            catch (Exception ex)
            {
                MelonLogger.Warning("Failed to register StorageScannerInputFocus: " + ex.Message);
            }
            GameLifecycle.OnPreLoad += OnPreLoad;
            GameLifecycle.OnLoadComplete += OnLoadComplete;
            MelonLogger.Msg($"Initialized ({Constants.ModVersion}).");
        }

        public override void OnApplicationQuit()
        {
            GameLifecycle.OnPreLoad -= OnPreLoad;
            GameLifecycle.OnLoadComplete -= OnLoadComplete;
        }

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            base.OnSceneWasUnloaded(buildIndex, sceneName);
            if (sceneName.Equals(Constants.GameplaySceneName, StringComparison.OrdinalIgnoreCase))
            {
                StorageScannerApp.TearDownForSceneUnload();
            }
        }

        // OnSaveInfoLoaded fires 0x on game 0.4.7f6+ (lifecycle-verify 2026-09-29), so the reset
        // runs on OnPreLoad and the owned-property refresh waits for OnLoadComplete.
        private void OnPreLoad()
        {
            try
            {
                StorageScannerApp.ResetForNewSave();
                StorageScannerApp.ActiveSource?.ClearItemCache();
            }
            catch (Exception ex)
            {
                MelonLogger.Warning("OnPreLoad handler failed: " + ex.Message);
            }
        }

        private void OnLoadComplete()
        {
            try
            {
                StorageScannerApp.ActiveSource?.RefreshPropertyCache();
            }
            catch (Exception ex)
            {
                MelonLogger.Warning("OnLoadComplete handler failed: " + ex.Message);
            }
        }
    }
}
