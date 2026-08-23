using System;
using MelonLoader;
using Il2CppInterop.Runtime.Injection;

[assembly: MelonInfo(typeof(NotesApp.Mod), "NotesApp", "1.0.0", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace NotesApp;

public class Mod : MelonMod
{
    public override void OnInitializeMelon()
    {
        try { ClassInjector.RegisterTypeInIl2Cpp<NotesAppInputFocus>(); } catch (Exception ex) { MelonLogger.Warning($"Failed to register NotesAppInputFocus: {ex.Message}"); }
        MelonLogger.Msg("Initialized (v1.0.0).");
    }
}
