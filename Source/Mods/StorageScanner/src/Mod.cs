using System;
using Il2CppInterop.Runtime.Injection;
using MelonLoader;
using S1API.Lifecycle;

[assembly: MelonInfo(typeof(StorageScanner.Mod), "StorageScanner", "0.2.0", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace StorageScanner
{
    public sealed class Mod : MelonMod
    {
        public override void OnInitializeMelon()
        {
            try { ClassInjector.RegisterTypeInIl2Cpp<StorageScannerInputFocus>(); }
            catch (Exception ex) { MelonLogger.Warning("[StorageScanner] Input focus registration failed: " + ex.Message); }

            GameLifecycle.OnSaveInfoLoaded -= OnSaveInfoLoaded;
            GameLifecycle.OnSaveInfoLoaded += OnSaveInfoLoaded;
            MelonLogger.Msg($"[StorageScanner] Initialized ({Constants.ModVersion}).");
        }

        public override void OnApplicationQuit()
        {
            GameLifecycle.OnSaveInfoLoaded -= OnSaveInfoLoaded;
        }

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            base.OnSceneWasUnloaded(buildIndex, sceneName);
            if (sceneName.Equals(Constants.GameplaySceneName, StringComparison.OrdinalIgnoreCase))
                StorageScannerApp.TearDownForSceneUnload();
        }

        private static void OnSaveInfoLoaded()
        {
            try { StorageScannerApp.ResetForNewSave(); }
            catch (Exception ex) { MelonLogger.Warning("[StorageScanner] Save reset failed: " + ex.Message); }
        }
    }
}
