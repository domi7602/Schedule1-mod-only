using System;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Persistence;
using MelonLoader;
using S1API.Internal.Map;
using UnityEngine;
using UnityEngine.Events;

namespace S1API.Lifecycle;

public static class GameLifecycle
{
	private static bool _initialized;

	private static LoadManager? _loadManager;

	private static LoadManager? _preSceneChangeManager;

	private static SaveManager? _saveManager;

	private static UnityAction? _preLoadListener;

	private static UnityAction? _loadCompleteListener;

	private static UnityAction? _preSceneChangeListener;

	private static UnityAction? _saveInfoLoadedListener;

	private static UnityAction? _saveStartListener;

	private static UnityAction? _saveCompleteListener;

	public static event Action? OnPreLoad;

	public static event Action? OnLoadComplete;

	public static event Action? OnPreSceneChange;

	public static event Action? OnSaveInfoLoaded;

	public static event Action? OnSaveStart;

	public static event Action? OnSaveComplete;

	internal static void Initialize()
	{
		if (_initialized)
		{
			return;
		}
		LoadManager instance = Singleton<LoadManager>.Instance;
		SaveManager instance2 = Singleton<SaveManager>.Instance;
		if ((Object)(object)instance == (Object)null || (Object)(object)instance2 == (Object)null)
		{
			return;
		}
		if (_preLoadListener == null)
		{
			_preLoadListener = UnityAction.op_Implicit((Action)InvokeOnPreLoad);
		}
		if (_loadCompleteListener == null)
		{
			_loadCompleteListener = UnityAction.op_Implicit((Action)InvokeOnLoadComplete);
		}
		if (_preSceneChangeListener == null)
		{
			_preSceneChangeListener = UnityAction.op_Implicit((Action)InvokeOnPreSceneChange);
		}
		if (_saveInfoLoadedListener == null)
		{
			_saveInfoLoadedListener = UnityAction.op_Implicit((Action)InvokeOnSaveInfoLoaded);
		}
		if (_saveStartListener == null)
		{
			_saveStartListener = UnityAction.op_Implicit((Action)InvokeOnSaveStart);
		}
		if (_saveCompleteListener == null)
		{
			_saveCompleteListener = UnityAction.op_Implicit((Action)InvokeOnSaveComplete);
		}
		instance.onPreLoad.AddListener(_preLoadListener);
		instance.onLoadComplete.AddListener(_loadCompleteListener);
		if ((Object)(object)_preSceneChangeManager != (Object)(object)instance)
		{
			if ((Object)(object)_preSceneChangeManager != (Object)null)
			{
				_preSceneChangeManager.onPreSceneChange.RemoveListener(_preSceneChangeListener);
			}
			instance.onPreSceneChange.AddListener(_preSceneChangeListener);
			_preSceneChangeManager = instance;
		}
		instance.onSaveInfoLoaded.AddListener(_saveInfoLoadedListener);
		instance2.onSaveStart.AddListener(_saveStartListener);
		instance2.onSaveComplete.AddListener(_saveCompleteListener);
		_loadManager = instance;
		_saveManager = instance2;
		_initialized = true;
	}

	internal static void Reset()
	{
		if ((Object)(object)_loadManager != (Object)null)
		{
			_loadManager.onPreLoad.RemoveListener(_preLoadListener);
			_loadManager.onLoadComplete.RemoveListener(_loadCompleteListener);
			_loadManager.onSaveInfoLoaded.RemoveListener(_saveInfoLoadedListener);
		}
		if ((Object)(object)_saveManager != (Object)null)
		{
			_saveManager.onSaveStart.RemoveListener(_saveStartListener);
			_saveManager.onSaveComplete.RemoveListener(_saveCompleteListener);
		}
		_loadManager = null;
		_saveManager = null;
		_initialized = false;
	}

	private static void InvokeOnPreLoad()
	{
		try
		{
			GameLifecycle.OnPreLoad?.Invoke();
		}
		catch (Exception value)
		{
			MelonLogger.Error($"Error in GameLifecycle.OnPreLoad: {value}");
		}
	}

	private static void InvokeOnLoadComplete()
	{
		try
		{
			DeferredMapResolver.ResolveAll();
			GameLifecycle.OnLoadComplete?.Invoke();
		}
		catch (Exception value)
		{
			MelonLogger.Error($"Error in GameLifecycle.OnLoadComplete: {value}");
		}
	}

	private static void InvokeOnPreSceneChange()
	{
		try
		{
			GameLifecycle.OnPreSceneChange?.Invoke();
		}
		catch (Exception value)
		{
			MelonLogger.Error($"Error in GameLifecycle.OnPreSceneChange: {value}");
		}
	}

	private static void InvokeOnSaveInfoLoaded()
	{
		try
		{
			GameLifecycle.OnSaveInfoLoaded?.Invoke();
		}
		catch (Exception value)
		{
			MelonLogger.Error($"Error in GameLifecycle.OnSaveInfoLoaded: {value}");
		}
	}

	private static void InvokeOnSaveStart()
	{
		try
		{
			GameLifecycle.OnSaveStart?.Invoke();
		}
		catch (Exception value)
		{
			MelonLogger.Error($"Error in GameLifecycle.OnSaveStart: {value}");
		}
	}

	private static void InvokeOnSaveComplete()
	{
		try
		{
			GameLifecycle.OnSaveComplete?.Invoke();
		}
		catch (Exception value)
		{
			MelonLogger.Error($"Error in GameLifecycle.OnSaveComplete: {value}");
		}
	}
}
