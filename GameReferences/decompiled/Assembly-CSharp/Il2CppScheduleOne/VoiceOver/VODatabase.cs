using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppScheduleOne.VoiceOver;

[Serializable]
public class VODatabase : ScriptableObject
{
	private static readonly IntPtr NativeFieldInfoPtr_VolumeMultiplier;

	private static readonly IntPtr NativeFieldInfoPtr_Entries;

	private static readonly IntPtr NativeMethodInfoPtr_GetEntry_Public_VODatabaseEntry_EVOLineType_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetRandomClip_Public_AudioClip_EVOLineType_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

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

	public unsafe List<VODatabaseEntry> Entries
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Entries);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<VODatabaseEntry>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Entries)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	static VODatabase()
	{
		Il2CppClassPointerStore<VODatabase>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.VoiceOver", "VODatabase");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<VODatabase>.NativeClassPtr);
		NativeFieldInfoPtr_VolumeMultiplier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VODatabase>.NativeClassPtr, "VolumeMultiplier");
		NativeFieldInfoPtr_Entries = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VODatabase>.NativeClassPtr, "Entries");
		NativeMethodInfoPtr_GetEntry_Public_VODatabaseEntry_EVOLineType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VODatabase>.NativeClassPtr, 100665961);
		NativeMethodInfoPtr_GetRandomClip_Public_AudioClip_EVOLineType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VODatabase>.NativeClassPtr, 100665962);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VODatabase>.NativeClassPtr, 100665963);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 91009, RefRangeEnd = 91012, XrefRangeStart = 90999, XrefRangeEnd = 91009, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe VODatabaseEntry GetEntry(EVOLineType lineType)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&lineType);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetEntry_Public_VODatabaseEntry_EVOLineType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<VODatabaseEntry>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 91012, XrefRangeEnd = 91013, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe AudioClip GetRandomClip(EVOLineType lineType)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&lineType);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetRandomClip_Public_AudioClip_EVOLineType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AudioClip>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 91013, XrefRangeEnd = 91021, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe VODatabase()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<VODatabase>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public VODatabase(IntPtr pointer)
		: base(pointer)
	{
	}
}
