using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppScheduleOne.Audio;

public class AmbientTrackGroup : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_AmbientTrackCooldown;

	private static readonly IntPtr NativeFieldInfoPtr_TimeOnLastAmbientTrackStart;

	private static readonly IntPtr NativeFieldInfoPtr_LastPlayedTrackGroup;

	private static readonly IntPtr NativeFieldInfoPtr_IsAnyTrackGroupQueued;

	private static readonly IntPtr NativeFieldInfoPtr__trackList;

	private static readonly IntPtr NativeFieldInfoPtr__windowStartTime;

	private static readonly IntPtr NativeFieldInfoPtr__windowEndTime;

	private static readonly IntPtr NativeFieldInfoPtr__chanceToPlay;

	private static readonly IntPtr NativeFieldInfoPtr__startTime;

	private static readonly IntPtr NativeFieldInfoPtr__playTrack;

	private static readonly IntPtr NativeFieldInfoPtr__trackRandomized;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_ForcePlay_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Stop_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Update_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_CanPlayNow_Protected_Virtual_New_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static float AmbientTrackCooldown
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_AmbientTrackCooldown, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_AmbientTrackCooldown, (void*)(&num));
		}
	}

	public unsafe static float TimeOnLastAmbientTrackStart
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_TimeOnLastAmbientTrackStart, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_TimeOnLastAmbientTrackStart, (void*)(&num));
		}
	}

	public unsafe static AmbientTrackGroup LastPlayedTrackGroup
	{
		get
		{
			Unsafe.SkipInit(out IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_LastPlayedTrackGroup, (void*)(&intPtr));
			IntPtr intPtr2 = intPtr;
			return (intPtr2 != (IntPtr)0) ? Il2CppObjectPool.Get<AmbientTrackGroup>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_LastPlayedTrackGroup, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)ambientTrackGroup));
		}
	}

	public unsafe static bool IsAnyTrackGroupQueued
	{
		get
		{
			Unsafe.SkipInit(out bool result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_IsAnyTrackGroupQueued, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_IsAnyTrackGroupQueued, (void*)(&flag));
		}
	}

	public unsafe List<MusicTrack> _trackList
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__trackList);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<MusicTrack>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__trackList)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe int _windowStartTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__windowStartTime);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__windowStartTime)) = num;
		}
	}

	public unsafe int _windowEndTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__windowEndTime);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__windowEndTime)) = num;
		}
	}

	public unsafe float _chanceToPlay
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__chanceToPlay);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__chanceToPlay)) = num;
		}
	}

	public unsafe int _startTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__startTime);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__startTime)) = num;
		}
	}

	public unsafe bool _playTrack
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__playTrack);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__playTrack)) = flag;
		}
	}

	public unsafe bool _trackRandomized
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__trackRandomized);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__trackRandomized)) = flag;
		}
	}

	static AmbientTrackGroup()
	{
		Il2CppClassPointerStore<AmbientTrackGroup>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Audio", "AmbientTrackGroup");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AmbientTrackGroup>.NativeClassPtr);
		NativeFieldInfoPtr_AmbientTrackCooldown = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AmbientTrackGroup>.NativeClassPtr, "AmbientTrackCooldown");
		NativeFieldInfoPtr_TimeOnLastAmbientTrackStart = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AmbientTrackGroup>.NativeClassPtr, "TimeOnLastAmbientTrackStart");
		NativeFieldInfoPtr_LastPlayedTrackGroup = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AmbientTrackGroup>.NativeClassPtr, "LastPlayedTrackGroup");
		NativeFieldInfoPtr_IsAnyTrackGroupQueued = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AmbientTrackGroup>.NativeClassPtr, "IsAnyTrackGroupQueued");
		NativeFieldInfoPtr__trackList = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AmbientTrackGroup>.NativeClassPtr, "_trackList");
		NativeFieldInfoPtr__windowStartTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AmbientTrackGroup>.NativeClassPtr, "_windowStartTime");
		NativeFieldInfoPtr__windowEndTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AmbientTrackGroup>.NativeClassPtr, "_windowEndTime");
		NativeFieldInfoPtr__chanceToPlay = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AmbientTrackGroup>.NativeClassPtr, "_chanceToPlay");
		NativeFieldInfoPtr__startTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AmbientTrackGroup>.NativeClassPtr, "_startTime");
		NativeFieldInfoPtr__playTrack = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AmbientTrackGroup>.NativeClassPtr, "_playTrack");
		NativeFieldInfoPtr__trackRandomized = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AmbientTrackGroup>.NativeClassPtr, "_trackRandomized");
		NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AmbientTrackGroup>.NativeClassPtr, 100676704);
		NativeMethodInfoPtr_ForcePlay_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AmbientTrackGroup>.NativeClassPtr, 100676705);
		NativeMethodInfoPtr_Stop_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AmbientTrackGroup>.NativeClassPtr, 100676706);
		NativeMethodInfoPtr_Update_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AmbientTrackGroup>.NativeClassPtr, 100676707);
		NativeMethodInfoPtr_CanPlayNow_Protected_Virtual_New_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AmbientTrackGroup>.NativeClassPtr, 100676708);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AmbientTrackGroup>.NativeClassPtr, 100676709);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 213673, XrefRangeEnd = 213686, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 213686, XrefRangeEnd = 213708, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ForcePlay()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ForcePlay_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 213708, XrefRangeEnd = 213714, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Stop()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Stop_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 213714, XrefRangeEnd = 213795, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Update_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 213795, XrefRangeEnd = 213833, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual bool CanPlayNow()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_CanPlayNow_Protected_Virtual_New_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 213833, XrefRangeEnd = 213841, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe AmbientTrackGroup()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<AmbientTrackGroup>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public AmbientTrackGroup(IntPtr pointer)
		: base(pointer)
	{
	}
}
