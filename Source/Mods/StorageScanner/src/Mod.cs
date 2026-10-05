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
            GameLifecycle.OnSaveInfoLoaded += OnSaveInfoLoaded;
            MelonLogger.Msg($"Initialized ({Constants.ModVersion}).");
        }

        public override void OnApplicationQuit()
        {
            GameLifecycle.OnSaveInfoLoaded -= OnSaveInfoLoaded;
        }

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            base.OnSceneWasUnloaded(buildIndex, sceneName);
            if (sceneName.Equals(Constants.GameplaySceneName, StringComparison.OrdinalIgnoreCase))
            {
                StorageScannerApp.TearDownForSceneUnload();
            }
        }

        private void OnSaveInfoLoaded()
        {
            try
            {
                StorageScannerApp.ResetForNewSave();
                StorageScannerApp.ActiveSource?.RefreshPropertyCache();
                StorageScannerApp.ActiveSource?.ClearItemCache();
            }
            catch (Exception ex)
            {
                MelonLogger.Warning("OnSaveInfoLoaded handler failed: " + ex.Message);
            }
        }
    }
}
