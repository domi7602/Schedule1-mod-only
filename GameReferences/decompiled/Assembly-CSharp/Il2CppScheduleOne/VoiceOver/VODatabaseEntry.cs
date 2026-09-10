using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.VoiceOver;

[System.Serializable]
public class VODatabaseEntry : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_LineType;

	private static readonly System.IntPtr NativeFieldInfoPtr_Clips;

	private static readonly System.IntPtr NativeFieldInfoPtr_lastClip;

	private static readonly System.IntPtr NativeFieldInfoPtr_VolumeMultiplier;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetRandomClip_Public_AudioClip_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe EVOLineType LineType
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LineType);
			return *(EVOLineType*)num;
		}
		set
		{
			*(EVOLineType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LineType)) = eVOLineType;
		}
	}

	public unsafe Il2CppReferenceArray<AudioClip> Clips
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Clips);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<AudioClip>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Clips)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe AudioClip lastClip
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastClip);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AudioClip>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastClip)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)audioClip));
		}
	}

	public unsafe float VolumeMultiplier
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VolumeMultiplier);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VolumeMultiplier)) = num;
		}
	}

	static VODatabaseEntry()
	{
		Il2CppClassPointerStore<VODatabaseEntry>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.VoiceOver", "VODatabaseEntry");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<VODatabaseEntry>.NativeClassPtr);
		NativeFieldInfoPtr_LineType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VODatabaseEntry>.NativeClassPtr, "LineType");
		NativeFieldInfoPtr_Clips = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VODatabaseEntry>.NativeClassPtr, "Clips");
		NativeFieldInfoPtr_lastClip = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VODatabaseEntry>.NativeClassPtr, "lastClip");
		NativeFieldInfoPtr_VolumeMultiplier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VODatabaseEntry>.NativeClassPtr, "VolumeMultiplier");
		NativeMethodInfoPtr_GetRandomClip_Public_AudioClip_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VODatabaseEntry>.NativeClassPtr, 100665964);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VODatabaseEntry>.NativeClassPtr, 100665965);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 91021, XrefRangeEnd = 91029, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe AudioClip GetRandomClip()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetRandomClip_Public_AudioClip_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AudioClip>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 91029, XrefRangeEnd = 91030, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe VODatabaseEntry()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<VODatabaseEntry>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public VODatabaseEntry(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
