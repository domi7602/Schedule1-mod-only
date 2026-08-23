using System;
using MelonLoader;
using Il2CppInterop.Runtime.Injection;

[assembly: MelonInfo(typeof(CalculatorApp.Mod), "CalculatorApp", "0.2.0", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace CalculatorApp;

public class Mod : MelonMod
{
    public override void OnInitializeMelon()
    {
        try { ClassInjector.RegisterTypeInIl2Cpp<CalculatorAppInputFocus>(); } catch (Exception ex) { MelonLogger.Warning($"Failed to register CalculatorAppInputFocus: {ex.Message}"); }
        MelonLogger.Msg("Initialized (v0.2.0).");
    }
}
