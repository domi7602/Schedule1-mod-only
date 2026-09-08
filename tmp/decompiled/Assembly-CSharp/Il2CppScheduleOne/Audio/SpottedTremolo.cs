using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Vision;
using UnityEngine;

namespace Il2CppScheduleOne.Audio;

public class SpottedTremolo : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_MinVolume;

	private static readonly IntPtr NativeFieldInfoPtr_MaxVolume;

	private static readonly IntPtr NativeFieldInfoPtr_MinPitch;

	private static readonly IntPtr NativeFieldInfoPtr_MaxPitch;

	private static readonly IntPtr NativeFieldInfoPtr_SmoothTime;

	private static readonly IntPtr NativeFieldInfoPtr__visibilityComponent;

	private static readonly IntPtr NativeFieldInfoPtr__audio;

	private static readonly IntPtr NativeFieldInfoPtr__targetIntensity;

	private static readonly IntPtr NativeFieldInfoPtr__smoothedIntensity;

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

	public unsafe static float MaxVolume
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MaxVolume, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MaxVolume, (void*)(&num));
		}
	}

	public unsafe static float MinPitch
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MinPitch, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MinPitch, (void*)(&num));
		}
	}

	public unsafe static float MaxPitch
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MaxPitch, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MaxPitch, (void*)(&num));
		}
	}

	public unsafe static float SmoothTime
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SmoothTime, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SmoothTime, (void*)(&num));
		}
	}

	public unsafe EntityVisibility _visibilityComponent
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__visibilityComponent);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<EntityVisibility>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__visibilityComponent)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)entityVisibility));
		}
	}

	public unsafe AudioSourceController _audio
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__audio);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AudioSourceController>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__audio)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)audioSourceController));
		}
	}

	public unsafe float _targetIntensity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__targetIntensity);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__targetIntensity)) = num;
		}
	}

	public unsafe float _smoothedIntensity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__smoothedIntensity);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__smoothedIntensity)) = num;
		}
	}

	static SpottedTremolo()
	{
		Il2CppClassPointerStore<SpottedTremolo>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Audio", "SpottedTremolo");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SpottedTremolo>.NativeClassPtr);
		NativeFieldInfoPtr_MinVolume = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SpottedTremolo>.NativeClassPtr, "MinVolume");
		NativeFieldInfoPtr_MaxVolume = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SpottedTremolo>.NativeClassPtr, "MaxVolume");
		NativeFieldInfoPtr_MinPitch = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SpottedTremolo>.NativeClassPtr, "MinPitch");
		NativeFieldInfoPtr_MaxPitch = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SpottedTremolo>.NativeClassPtr, "MaxPitch");
		NativeFieldInfoPtr_SmoothTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SpottedTremolo>.NativeClassPtr, "SmoothTime");
		NativeFieldInfoPtr__visibilityComponent = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SpottedTremolo>.NativeClassPtr, "_visibilityComponent");
		NativeFieldInfoPtr__audio = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SpottedTremolo>.NativeClassPtr, "_audio");
		NativeFieldInfoPtr__targetIntensity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SpottedTremolo>.NativeClassPtr, "_targetIntensity");
		NativeFieldInfoPtr__smoothedIntensity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SpottedTremolo>.NativeClassPtr, "_smoothedIntensity");
		NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpottedTremolo>.NativeClassPtr, 100676901);
		NativeMethodInfoPtr_Update_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpottedTremolo>.NativeClassPtr, 100676902);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpottedTremolo>.NativeClassPtr, 100676903);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 215809, XrefRangeEnd = 215813, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 215813, XrefRangeEnd = 215831, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Update_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(204)]
	[CachedScanResults(RefRangeStart = 152, RefRangeEnd = 356, XrefRangeStart = 152, XrefRangeEnd = 356, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SpottedTremolo()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SpottedTremolo>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SpottedTremolo(IntPtr pointer)
		: base(pointer)
	{
	}
}
