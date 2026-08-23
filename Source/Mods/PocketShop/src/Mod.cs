using MelonLoader;
using PocketShop.Config;
using S1Mods.Shared;

[assembly: MelonInfo(typeof(PocketShop.Mod), "PocketShop", "0.2.1", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace PocketShop;

public class Mod : MelonMod
{
    public static ModLogger Log { get; private set; } = null!;

    public override void OnInitializeMelon()
    {
        Log = new ModLogger("PocketShop");
        ModConfig<PocketShopConfig>.Initialize("PocketShop", Log);
        MelonLogger.Msg("initialized (v0.2.1).");
    }
}
