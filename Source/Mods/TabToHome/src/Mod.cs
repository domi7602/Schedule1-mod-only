using MelonLoader;

[assembly: MelonInfo(typeof(TabToHome.Mod), "TabToHome", "0.1.0", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]
[assembly: MelonGame("TVGS", "Schedule 1")]

namespace TabToHome;

public class Mod : MelonMod
{
    public override void OnInitializeMelon()
    {
        // Harmony [HarmonyPatch] classes are applied automatically by MelonLoader.
        MelonLogger.Msg("[TabToHome] initialized (v0.1.0): Tab closes the active app to the HomeScreen, second Tab puts the phone away.");
    }
}
