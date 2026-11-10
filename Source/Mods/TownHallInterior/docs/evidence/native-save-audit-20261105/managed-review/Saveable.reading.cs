using System;
using System.IO;
using System.Reflection;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Persistence;
using Il2CppScheduleOne.Persistence.Datas;
using Il2CppSystem.Collections.Generic;
using Newtonsoft.Json;
using S1API.Internal.Utils;
using S1API.Saveables;
using UnityEngine;

namespace S1API.Internal.Abstraction;

public abstract class Saveable : Registerable, ISaveable, IRegisterable
{
	public virtual SaveableLoadOrder LoadOrder => SaveableLoadOrder.AfterBaseGame;

	public static bool RequestGameSave(bool immediate)
	{
		return RequestGameSave();
	}

	public static bool RequestGameSave()
	{
		try
		{
			LoadManager instance = Singleton<LoadManager>.Instance;
			if ((Object)(object)instance == (Object)null || !instance.IsGameLoaded)
			{
				return false;
			}
			SaveManager instance2 = Singleton<SaveManager>.Instance;
			if ((Object)(object)instance2 == (Object)null)
			{
				return false;
			}
			instance2.Save();
			return true;
		}
		catch
		{
			return false;
		}
	}

	void ISaveable.LoadInternal(string folderPath)
	{
		LoadInternal(folderPath);
	}

	internal virtual void LoadInternal(string folderPath)
	{
		FieldInfo[] fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		FieldInfo[] array = fields;
		foreach (FieldInfo fieldInfo in array)
		{
			SaveableField customAttribute = fieldInfo.GetCustomAttribute<SaveableField>();
			if (customAttribute != null)
			{
				string path = (customAttribute.SaveName.EndsWith(".json") ? customAttribute.SaveName : (customAttribute.SaveName + ".json"));
				string path2 = Path.Combine(folderPath, path);
				if (File.Exists(path2))
				{
					string text = File.ReadAllText(path2);
					Type fieldType = fieldInfo.FieldType;
					object value = JsonConvert.DeserializeObject(text, fieldType, ISaveable.SerializerSettings);
					fieldInfo.SetValue(this, value);
				}
			}
		}
		OnLoaded();
	}

	void ISaveable.SaveInternal(string folderPath, ref List<string> extraSaveables)
	{
		SaveInternal(folderPath, ref extraSaveables);
	}

	internal virtual void SaveInternal(string folderPath, ref List<string> extraSaveables)
	{
		FieldInfo[] allFields = ReflectionUtils.GetAllFields(GetType(), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		FieldInfo[] array = allFields;
		foreach (FieldInfo fieldInfo in array)
		{
			SaveableField customAttribute = fieldInfo.GetCustomAttribute<SaveableField>();
			if (customAttribute != null)
			{
				string text = (customAttribute.SaveName.EndsWith(".json") ? customAttribute.SaveName : (customAttribute.SaveName + ".json"));
				string path = Path.Combine(folderPath, text);
				object value = fieldInfo.GetValue(this);
				if (value == null)
				{
					File.Delete(path);
					continue;
				}
				extraSaveables.Add(text);
				string contents = JsonConvert.SerializeObject(value, (Formatting)1, ISaveable.SerializerSettings);
				File.WriteAllText(path, contents);
			}
		}
		OnSaved();
	}

	internal void SaveToDynamic(DynamicSaveData dynamicSaveData)
	{
		if (dynamicSaveData == null)
		{
			return;
		}
		FieldInfo[] allFields = ReflectionUtils.GetAllFields(GetType(), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		FieldInfo[] array = allFields;
		foreach (FieldInfo fieldInfo in array)
		{
			SaveableField customAttribute = fieldInfo.GetCustomAttribute<SaveableField>();
			if (customAttribute != null)
			{
				object value = fieldInfo.GetValue(this);
				if (value != null)
				{
					string text = JsonConvert.SerializeObject(value, (Formatting)0, ISaveable.SerializerSettings);
					dynamicSaveData.AddData(customAttribute.SaveName, text);
				}
			}
		}
		OnSaved();
	}

	internal void LoadFromDynamic(DynamicSaveData dynamicSaveData)
	{
		if (dynamicSaveData == null)
		{
			return;
		}
		FieldInfo[] allFields = ReflectionUtils.GetAllFields(GetType(), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		FieldInfo[] array = allFields;
		string text = default(string);
		foreach (FieldInfo fieldInfo in array)
		{
			SaveableField customAttribute = fieldInfo.GetCustomAttribute<SaveableField>();
			if (customAttribute != null && dynamicSaveData.TryGetData(customAttribute.SaveName, ref text) && !string.IsNullOrEmpty(text))
			{
				Type fieldType = fieldInfo.FieldType;
				object value = JsonConvert.DeserializeObject(text, fieldType, ISaveable.SerializerSettings);
				fieldInfo.SetValue(this, value);
			}
		}
		OnLoaded();
	}

	void ISaveable.OnLoaded()
	{
		OnLoaded();
	}

	protected virtual void OnLoaded()
	{
	}

	void ISaveable.OnSaved()
	{
		OnSaved();
	}

	protected virtual void OnSaved()
	{
	}
}
