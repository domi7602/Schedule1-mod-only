using MelonLoader;
using S1API.Cutscenes;
using S1API.Internal;
using S1API.Internal.Compatibility;
using S1API.Internal.Diagnostics;
using S1API.Internal.Entities;
using S1API.Internal.Lifecycle;
using S1API.Internal.NPCWorkbench;
using S1API.Internal.Products;
using S1API.Internal.Rendering;
using S1API.Internal.Vehicles;
using S1API.Internal.Weather;
using S1API.Lifecycle;
using S1API.Map;
using S1API.Rendering;
using UnityEngine;

namespace S1API;

public class S1API : MelonMod
{
	public override void OnInitializeMelon()
	{
		S1APIPreferences.Initialize();
		MelonPreferences_Entry<bool>? enableUnityNullReferenceTraceLogging = S1APIPreferences.EnableUnityNullReferenceTraceLogging;
		if (enableUnityNullReferenceTraceLogging != null && enableUnityNullReferenceTraceLogging.Value)
		{
			MelonLogger.Warning("Exception trace logging is enabled. If you are not a mod developer, you probably do not need this enabled.");
			UnityExceptionTraceHook.Install();
		}
	}

	public override void OnDeinitializeMelon()
	{
		VehicleSeats.Clear();
		AvatarCompatibility.ResetRendering();
		IconFactory.ResetAccessoryIconState();
		WeatherRuntime.ResetBindings();
		NPCWorkbenchRuntime.Close();
		PresentationWorkbenchRuntime.Close();
		ProductPackagingContentRuntime.ResetForSceneChange();
		CutsceneManager.Deinitialize();
		MapPOIManager.RemoveAll();
		UnityExceptionTraceHook.Remove();
	}

	public override void OnUpdate()
	{
		VehicleSeats.Tick();
		WeatherRuntime.Tick();
		PresentationWorkbenchRuntime.Tick();
		CutsceneManager.Tick(Time.unscaledDeltaTime);
		NPCWorkbenchRuntime.Tick();
	}

	public override void OnGUI()
	{
		CutsceneManager.DrawPresentation();
	}

	public override void OnSceneWasLoaded(int buildIndex, string sceneName)
	{
		if (sceneName == "Main")
		{
			GameLifecycle.Initialize();
		}
	}

	public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
	{
		VehicleSeats.Clear();
		NPCWorkbenchRuntime.Close();
		PresentationWorkbenchRuntime.Close();
		CutsceneManager.CleanupForSceneChange();
		SceneStateCleaner.ResetForSceneChange(sceneName, afterUnload: true);
		if (sceneName == "Main" || sceneName == "Tutorial")
		{
			ProductPackagingContentRuntime.ResetForSceneChange();
			MapPOIManager.ResetForSceneChange();
		}
		if (sceneName == "Main")
		{
			GameLifecycle.Reset();
		}
	}

	public override void OnSceneWasInitialized(int buildIndex, string sceneName)
	{
		NPCNetworkBootstrap.EnsurePrefabsWarmup();
		SceneStateCleaner.ResetForSceneChange(sceneName, afterUnload: false);
	}
}
