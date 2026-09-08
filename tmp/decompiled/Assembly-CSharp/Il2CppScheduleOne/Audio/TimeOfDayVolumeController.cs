using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppScheduleOne.Audio;

public class TimeOfDayVolumeController : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_MinVolume;

	private static readonly IntPtr NativeFieldInfoPtr_FadeSpeed;

	private static readonly IntPtr NativeFieldInfoPtr__timeOfDayVolumeCurve;

	private static readonly IntPtr NativeFieldInfoPtr__reduceVolumeWhenSoundtrackPlaying;

	private static readonly IntPtr NativeFieldInfoPtr__audioSourceController;

	private static readonly IntPtr NativeFieldInfoPtr__volumeMultiplier;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Update_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static float MinVolume
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MinVolume, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MinVolume, (void*)(&num));
		}
	}

	public unsafe static float FadeSpeed
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FadeSpeed, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FadeSpeed, (void*)(&num));
		}
	}

	public unsafe AnimationCurve _timeOfDayVolumeCurve
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__timeOfDayVolumeCurve);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AnimationCurve>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__timeOfDayVolumeCurve)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationCurve));
		}
	}

	public unsafe bool _reduceVolumeWhenSoundtrackPlaying
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__reduceVolumeWhenSoundtrackPlaying);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__reduceVolumeWhenSoundtrackPlaying)) = flag;
		}
	}

	public unsafe AudioSourceController _audioSourceController
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__audioSourceController);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AudioSourceController>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__audioSourceController)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)audioSourceController));
		}
	}

	public unsafe float _volumeMultiplier
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__volumeMultiplier);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__volumeMultiplier)) = num;
		}
	}

	static TimeOfDayVolumeController()
	{
		Il2CppClassPointerStore<TimeOfDayVolumeController>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Audio", "TimeOfDayVolumeController");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TimeOfDayVolumeController>.NativeClassPtr);
		NativeFieldInfoPtr_MinVolume = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TimeOfDayVolumeController>.NativeClassPtr, "MinVolume");
		NativeFieldInfoPtr_FadeSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TimeOfDayVolumeController>.NativeClassPtr, "FadeSpeed");
		NativeFieldInfoPtr__timeOfDayVolumeCurve = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TimeOfDayVolumeController>.NativeClassPtr, "_timeOfDayVolumeCurve");
		NativeFieldInfoPtr__reduceVolumeWhenSoundtrackPlaying = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TimeOfDayVolumeController>.NativeClassPtr, "_reduceVolumeWhenSoundtrackPlaying");
		NativeFieldInfoPtr__audioSourceController = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TimeOfDayVolumeController>.NativeClassPtr, "_audioSourceController");
		NativeFieldInfoPtr__volumeMultiplier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TimeOfDayVolumeController>.NativeClassPtr, "_volumeMultiplier");
		NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TimeOfDayVolumeController>.NativeClassPtr, 100676924);
		NativeMethodInfoPtr_Update_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TimeOfDayVolumeController>.NativeClassPtr, 100676925);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TimeOfDayVolumeController>.NativeClassPtr, 100676926);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 215901, XrefRangeEnd = 215905, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 215905, XrefRangeEnd = 215925, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Update_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 215925, XrefRangeEnd = 215926, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe TimeOfDayVolumeController()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<TimeOfDayVolumeController>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public TimeOfDayVolumeController(IntPtr pointer)
		: base(pointer)
	{
	}
}
