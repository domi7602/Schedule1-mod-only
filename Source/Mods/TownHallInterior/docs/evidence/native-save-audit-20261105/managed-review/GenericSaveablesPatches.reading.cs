using System;
using System.IO;
using HarmonyLib;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Persistence;
using Il2CppScheduleOne.Persistence.Loaders;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using S1API.Internal.Abstraction;
using S1API.Saveables;
using S1API.Utils;
using UnityEngine;

namespace S1API.Internal.Patches;

[HarmonyPatch]
internal static class GenericSaveablesPatches
{
	private static bool sameSession;

	[HarmonyPatch(typeof(SaveManager), "Save", new Type[] { typeof(string) })]
	[HarmonyPostfix]
	private static void SaveManager_Save_Postfix(string saveFolderPath)
	{
		try
		{
			SaveManager instance = Singleton<SaveManager>.Instance;
			string text = "Modded";
			string text2 = Path.Combine("Modded", "Saveables");
			if (!instance.ApprovedBaseLevelPaths.Contains(text))
			{
				instance.ApprovedBaseLevelPaths.Add(text);
			}
			if (!instance.ApprovedBaseLevelPaths.Contains(text2))
			{
				instance.ApprovedBaseLevelPaths.Add(text2);
			}
			string text3 = Path.Combine(saveFolderPath, "Modded", "Saveables");
			Directory.CreateDirectory(text3);
			foreach (Saveable registeredSaveable in SaveableAutoRegistry.GetRegisteredSaveables())
			{
				List<string> extraSaveables = new List<string>();
				string name = registeredSaveable.GetType().Name;
				string text4 = Path.Combine(text3, name);
				Directory.CreateDirectory(text4);
				registeredSaveable.SaveInternal(text4, ref extraSaveables);
			}
		}
		catch (Exception ex)
		{
			try
			{
				MelonLogger.Warning("[Saveables] SaveManager_Save_Postfix failed: " + ex.Message + "\n" + ex.StackTrace);
			}
			catch
			{
			}
		}
	}

	[HarmonyPatch(typeof(LoadManager), "QueueLoadRequest")]
	[HarmonyPrefix]
	private static void BeforeBaseLoaders(LoadRequest request)
	{
		try
		{
			LoadManager lm = Singleton<LoadManager>.Instance;
			if ((Object)(object)lm == (Object)null || string.IsNullOrEmpty(lm.LoadedGameFolderPath) || sameSession)
			{
				return;
			}
			string path = Path.Combine(lm.LoadedGameFolderPath, "Modded", "Saveables");
			foreach (Saveable registeredSaveable in SaveableAutoRegistry.GetRegisteredSaveables())
			{
				Saveable saveable = registeredSaveable;
				if (saveable.LoadOrder == SaveableLoadOrder.BeforeBaseGame)
				{
					string name = saveable.GetType().Name;
					string text = Path.Combine(path, name);
					if (Directory.Exists(text))
					{
						saveable.LoadInternal(text);
					}
					else
					{
						global::S1API.Utils.EventHelper.AddListener(InitializeOnLoadComplete, lm.onLoadComplete);
					}
				}
				void InitializeOnLoadComplete()
				{
					try
					{
						global::S1API.Utils.EventHelper.RemoveListener(InitializeOnLoadComplete, lm.onLoadComplete);
						((IRegisterable)saveable).CreateInternal();
					}
					catch (Exception ex3)
					{
						try
						{
							MelonLogger.Warning("[Saveables] InitializeOnLoadComplete (Before) failed: " + ex3.Message + "\n" + ex3.StackTrace);
						}
						catch
						{
						}
					}
				}
			}
			sameSession = true;
			global::S1API.Utils.EventHelper.AddListener(ClearLockOnLoadComplete, lm.onLoadComplete);
			void ClearLockOnLoadComplete()
			{
				try
				{
					global::S1API.Utils.EventHelper.RemoveListener(ClearLockOnLoadComplete, lm.onLoadComplete);
					sameSession = false;
				}
				catch (Exception ex2)
				{
					try
					{
						MelonLogger.Warning("[Saveables] ClearLockOnLoadComplete failed: " + ex2.Message + "\n" + ex2.StackTrace);
					}
					catch
					{
					}
				}
			}
		}
		catch (Exception ex)
		{
			try
			{
				MelonLogger.Warning("[Saveables] BeforeBaseLoaders failed: " + ex.Message + "\n" + ex.StackTrace);
			}
			catch
			{
			}
		}
	}

	[HarmonyPatch(typeof(NPCsLoader), "Load")]
	[HarmonyPostfix]
	private static void AfterBaseLoaders(string mainPath)
	{
		try
		{
			string path = Path.Combine(Singleton<LoadManager>.Instance.LoadedGameFolderPath, "Modded", "Saveables");
			foreach (Saveable registeredSaveable in SaveableAutoRegistry.GetRegisteredSaveables())
			{
				Saveable saveable = registeredSaveable;
				LoadManager lm;
				if (saveable.LoadOrder == SaveableLoadOrder.AfterBaseGame)
				{
					string name = saveable.GetType().Name;
					string text = Path.Combine(path, name);
					if (Directory.Exists(text))
					{
						saveable.LoadInternal(text);
						continue;
					}
					lm = Singleton<LoadManager>.Instance;
					global::S1API.Utils.EventHelper.AddListener(InitializeOnLoadComplete, lm.onLoadComplete);
				}
				void InitializeOnLoadComplete()
				{
					try
					{
						global::S1API.Utils.EventHelper.RemoveListener(InitializeOnLoadComplete, lm.onLoadComplete);
						((IRegisterable)saveable).CreateInternal();
					}
					catch (Exception ex2)
					{
						try
						{
							MelonLogger.Warning("[Saveables] InitializeOnLoadComplete (After) failed: " + ex2.Message + "\n" + ex2.StackTrace);
						}
						catch
						{
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			try
			{
				MelonLogger.Warning("[Saveables] AfterBaseLoaders failed: " + ex.Message + "\n" + ex.StackTrace);
			}
			catch
			{
			}
		}
	}
}
