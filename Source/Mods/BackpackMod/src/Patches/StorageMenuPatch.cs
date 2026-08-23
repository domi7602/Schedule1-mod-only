using Il2CppScheduleOne.UI;
using System;

namespace BackpackMod.Patches
{
    public static class StorageMenuPatch
    {
        public static void Close_Prefix()
        {
            try
            {
                BackpackStorageManager.OnStorageMenuClosed();
            }
            catch (Exception ex)
            {
                Mod.Log?.Error($"Error in StorageMenuPatch.Close_Prefix: {ex}");
            }
        }
    }
}

