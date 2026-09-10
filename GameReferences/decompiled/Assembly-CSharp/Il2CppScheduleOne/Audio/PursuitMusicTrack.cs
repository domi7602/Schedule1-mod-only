using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppScheduleOne.PlayerScripts;

namespace Il2CppScheduleOne.Audio;

public class PursuitMusicTrack : MusicTrack
{
	private static readonly IntPtr NativeFieldInfoPtr_OutOfSightTimeToDipMusic;

	private static readonly IntPtr NativeFieldInfoPtr_MinMusicVolume;

	private static readonly IntPtr NativeFieldInfoPtr_MusicChangeRate_Down;

	private static readonly IntPtr NativeFieldInfoPtr_MusicChangeRate_Up;

	private static readonly IntPtr NativeFieldInfoPtr__pursuitLevelToActivate;

	private static readonly IntPtr NativeMethodInfoPtr_Start_Protected_Virtual_New_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnLoadComplete_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_RegisterEvent_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Update_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_PursuitLevelChange_Private_Void_EPursuitLevel_EPursuitLevel_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetNewVolume_Private_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static float OutOfSightTimeToDipMusic
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_OutOfSightTimeToDipMusic, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_OutOfSightTimeToDipMusic, (void*)(&num));
		}
	}

	public unsafe static float MinMusicVolume
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MinMusicVolume, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MinMusicVolume, (void*)(&num));
		}
	}

	public unsafe static float MusicChangeRate_Down
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MusicChangeRate_Down, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MusicChangeRate_Down, (void*)(&num));
		}
	}

	public unsafe static float MusicChangeRate_Up
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MusicChangeRate_Up, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MusicChangeRate_Up, (void*)(&num));
		}
	}

	public unsafe PlayerCrimeData.EPursuitLevel _pursuitLevelToActivate
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__pursuitLevelToActivate);
			return *(PlayerCrimeData.EPursuitLevel*)num;
		}
		set
		{
			*(PlayerCrimeData.EPursuitLevel*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__pursuitLevelToActivate)) = ePursuitLevel;
		}
	}

	static PursuitMusicTrack()
	{
		Il2CppClassPointerStore<PursuitMusicTrack>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Audio", "PursuitMusicTrack");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PursuitMusicTrack>.NativeClassPtr);
		NativeFieldInfoPtr_OutOfSightTimeToDipMusic = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PursuitMusicTrack>.NativeClassPtr, "OutOfSightTimeToDipMusic");
		NativeFieldInfoPtr_MinMusicVolume = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PursuitMusicTrack>.NativeClassPtr, "MinMusicVolume");
		NativeFieldInfoPtr_MusicChangeRate_Down = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PursuitMusicTrack>.NativeClassPtr, "MusicChangeRate_Down");
		NativeFieldInfoPtr_MusicChangeRate_Up = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PursuitMusicTrack>.NativeClassPtr, "MusicChangeRate_Up");
		NativeFieldInfoPtr__pursuitLevelToActivate = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PursuitMusicTrack>.NativeClassPtr, "_pursuitLevelToActivate");
		NativeMethodInfoPtr_Start_Protected_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PursuitMusicTrack>.NativeClassPtr, 100676855);
		NativeMethodInfoPtr_OnLoadComplete_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PursuitMusicTrack>.NativeClassPtr, 100676856);
		NativeMethodInfoPtr_RegisterEvent_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PursuitMusicTrack>.NativeClassPtr, 100676857);
		NativeMethodInfoPtr_Update_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PursuitMusicTrack>.NativeClassPtr, 100676858);
		NativeMethodInfoPtr_PursuitLevelChange_Private_Void_EPursuitLevel_EPursuitLevel_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PursuitMusicTrack>.NativeClassPtr, 100676859);
		NativeMethodInfoPtr_GetNewVolume_Private_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PursuitMusicTrack>.NativeClassPtr, 100676860);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PursuitMusicTrack>.NativeClassPtr, 100676861);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 215278, XrefRangeEnd = 215291, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Start_Protected_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 215344, RefRangeEnd = 215345, XrefRangeStart = 215291, XrefRangeEnd = 215344, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnLoadComplete()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnLoadComplete_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 215345, XrefRangeEnd = 215371, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RegisterEvent()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RegisterEvent_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 215371, XrefRangeEnd = 215386, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Update_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe void PursuitLevelChange(PlayerCrimeData.EPursuitLevel oldLevel, PlayerCrimeData.EPursuitLevel newLevel)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = (nint)(&oldLevel);
		*(PlayerCrimeData.EPursuitLevel**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &newLevel;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PursuitLevelChange_Private_Void_EPursuitLevel_EPursuitLevel_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 215386, XrefRangeEnd = 215400, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float GetNewVolume()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetNewVolume_Private_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PursuitMusicTrack()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PursuitMusicTrack>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public PursuitMusicTrack(IntPtr pointer)
		: base(pointer)
	{
	}
}
