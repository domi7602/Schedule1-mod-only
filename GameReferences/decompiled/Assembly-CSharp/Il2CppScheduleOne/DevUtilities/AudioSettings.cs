using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.DevUtilities;

[System.Serializable]
public class AudioSettings : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_MasterVolume;

	private static readonly System.IntPtr NativeFieldInfoPtr_AmbientVolume;

	private static readonly System.IntPtr NativeFieldInfoPtr_MusicVolume;

	private static readonly System.IntPtr NativeFieldInfoPtr_SFXVolume;

	private static readonly System.IntPtr NativeFieldInfoPtr_UIVolume;

	private static readonly System.IntPtr NativeFieldInfoPtr_DialogueVolume;

	private static readonly System.IntPtr NativeFieldInfoPtr_FootstepsVolume;

	private static readonly System.IntPtr NativeFieldInfoPtr_WeatherVolume;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe float MasterVolume
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MasterVolume);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MasterVolume)) = num;
		}
	}

	public unsafe float AmbientVolume
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AmbientVolume);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AmbientVolume)) = num;
		}
	}

	public unsafe float MusicVolume
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MusicVolume);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MusicVolume)) = num;
		}
	}

	public unsafe float SFXVolume
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SFXVolume);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SFXVolume)) = num;
		}
	}

	public unsafe float UIVolume
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UIVolume);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UIVolume)) = num;
		}
	}

	public unsafe float DialogueVolume
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DialogueVolume);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DialogueVolume)) = num;
		}
	}

	public unsafe float FootstepsVolume
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FootstepsVolume);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FootstepsVolume)) = num;
		}
	}

	public unsafe float WeatherVolume
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_WeatherVolume);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_WeatherVolume)) = num;
		}
	}

	static AudioSettings()
	{
		Il2CppClassPointerStore<AudioSettings>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.DevUtilities", "AudioSettings");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AudioSettings>.NativeClassPtr);
		NativeFieldInfoPtr_MasterVolume = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AudioSettings>.NativeClassPtr, "MasterVolume");
		NativeFieldInfoPtr_AmbientVolume = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AudioSettings>.NativeClassPtr, "AmbientVolume");
		NativeFieldInfoPtr_MusicVolume = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AudioSettings>.NativeClassPtr, "MusicVolume");
		NativeFieldInfoPtr_SFXVolume = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AudioSettings>.NativeClassPtr, "SFXVolume");
		NativeFieldInfoPtr_UIVolume = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AudioSettings>.NativeClassPtr, "UIVolume");
		NativeFieldInfoPtr_DialogueVolume = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AudioSettings>.NativeClassPtr, "DialogueVolume");
		NativeFieldInfoPtr_FootstepsVolume = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AudioSettings>.NativeClassPtr, "FootstepsVolume");
		NativeFieldInfoPtr_WeatherVolume = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AudioSettings>.NativeClassPtr, "WeatherVolume");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AudioSettings>.NativeClassPtr, 100675246);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe AudioSettings()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<AudioSettings>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public AudioSettings(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
