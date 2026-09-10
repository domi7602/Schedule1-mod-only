using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Audio;
using Il2CppScheduleOne.ObjectScripts;
using Il2CppTMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Il2CppScheduleOne.StationFramework;

public class BoilingFlask : Fillable
{
	private static readonly IntPtr NativeFieldInfoPtr_TEMPERATURE_MAX;

	private static readonly IntPtr NativeFieldInfoPtr_TEMPERATURE_MAX_VELOCITY;

	private static readonly IntPtr NativeFieldInfoPtr_TEMPERATURE_ACCELERATION;

	private static readonly IntPtr NativeFieldInfoPtr_OVERHEAT_TIME;

	private static readonly IntPtr NativeFieldInfoPtr__CurrentTemperature_k__BackingField;

	private static readonly IntPtr NativeFieldInfoPtr__CurrentTemperatureVelocity_k__BackingField;

	private static readonly IntPtr NativeFieldInfoPtr__OverheatScale_k__BackingField;

	private static readonly IntPtr NativeFieldInfoPtr__Recipe_k__BackingField;

	private static readonly IntPtr NativeFieldInfoPtr_LockTemperature;

	private static readonly IntPtr NativeFieldInfoPtr_BoilSoundPitchCurve;

	private static readonly IntPtr NativeFieldInfoPtr_LabelJitterScale;

	private static readonly IntPtr NativeFieldInfoPtr_Burner;

	private static readonly IntPtr NativeFieldInfoPtr_TemperatureCanvas;

	private static readonly IntPtr NativeFieldInfoPtr_TemperatureLabel;

	private static readonly IntPtr NativeFieldInfoPtr_TemperatureSlider;

	private static readonly IntPtr NativeFieldInfoPtr_TemperatureRangeIndicator;

	private static readonly IntPtr NativeFieldInfoPtr_SmokeParticles;

	private static readonly IntPtr NativeFieldInfoPtr_BoilSound;

	private static readonly IntPtr NativeFieldInfoPtr_OverheatMesh;

	private static readonly IntPtr NativeMethodInfoPtr_get_CurrentTemperature_Public_get_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_CurrentTemperature_Private_set_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_CurrentTemperatureVelocity_Public_get_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_CurrentTemperatureVelocity_Private_set_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_IsTemperatureInRange_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_OverheatScale_Public_get_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_OverheatScale_Private_set_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_Recipe_Public_get_StationRecipe_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_Recipe_Private_set_Void_StationRecipe_0;

	private static readonly IntPtr NativeMethodInfoPtr_Start_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Update_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_FixedUpdate_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_UpdateCanvas_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_UpdateSmoke_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetCanvasVisible_Public_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetTemperature_Public_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetRecipe_Public_Void_StationRecipe_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static float TEMPERATURE_MAX
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_TEMPERATURE_MAX, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_TEMPERATURE_MAX, (void*)(&num));
		}
	}

	public unsafe float TEMPERATURE_MAX_VELOCITY
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TEMPERATURE_MAX_VELOCITY);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TEMPERATURE_MAX_VELOCITY)) = num;
		}
	}

	public unsafe float TEMPERATURE_ACCELERATION
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TEMPERATURE_ACCELERATION);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TEMPERATURE_ACCELERATION)) = num;
		}
	}

	public unsafe static float OVERHEAT_TIME
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_OVERHEAT_TIME, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_OVERHEAT_TIME, (void*)(&num));
		}
	}

	public unsafe float _CurrentTemperature_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__CurrentTemperature_k__BackingField);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__CurrentTemperature_k__BackingField)) = num;
		}
	}

	public unsafe float _CurrentTemperatureVelocity_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__CurrentTemperatureVelocity_k__BackingField);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__CurrentTemperatureVelocity_k__BackingField)) = num;
		}
	}

	public unsafe float _OverheatScale_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__OverheatScale_k__BackingField);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__OverheatScale_k__BackingField)) = num;
		}
	}

	public unsafe StationRecipe _Recipe_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Recipe_k__BackingField);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<StationRecipe>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Recipe_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stationRecipe));
		}
	}

	public unsafe bool LockTemperature
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LockTemperature);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LockTemperature)) = flag;
		}
	}

	public unsafe AnimationCurve BoilSoundPitchCurve
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BoilSoundPitchCurve);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AnimationCurve>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BoilSoundPitchCurve)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationCurve));
		}
	}

	public unsafe float LabelJitterScale
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LabelJitterScale);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LabelJitterScale)) = num;
		}
	}

	public unsafe BunsenBurner Burner
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Burner);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<BunsenBurner>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Burner)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)bunsenBurner));
		}
	}

	public unsafe Canvas TemperatureCanvas
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TemperatureCanvas);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Canvas>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TemperatureCanvas)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)canvas));
		}
	}

	public unsafe TextMeshProUGUI TemperatureLabel
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TemperatureLabel);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<TextMeshProUGUI>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TemperatureLabel)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)textMeshProUGUI));
		}
	}

	public unsafe Slider TemperatureSlider
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TemperatureSlider);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Slider>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TemperatureSlider)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slider));
		}
	}

	public unsafe RectTransform TemperatureRangeIndicator
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TemperatureRangeIndicator);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<RectTransform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TemperatureRangeIndicator)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rectTransform));
		}
	}

	public unsafe ParticleSystem SmokeParticles
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SmokeParticles);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ParticleSystem>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SmokeParticles)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)particleSystem));
		}
	}

	public unsafe AudioSourceController BoilSound
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BoilSound);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AudioSourceController>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BoilSound)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)audioSourceController));
		}
	}

	public unsafe MeshRenderer OverheatMesh
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OverheatMesh);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<MeshRenderer>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OverheatMesh)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)meshRenderer));
		}
	}

	public unsafe float CurrentTemperature
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 29133, RefRangeEnd = 29134, XrefRangeStart = 29133, XrefRangeEnd = 29134, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CurrentTemperature_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 29134, RefRangeEnd = 29137, XrefRangeStart = 29134, XrefRangeEnd = 29137, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_CurrentTemperature_Private_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float CurrentTemperatureVelocity
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 29137, RefRangeEnd = 29138, XrefRangeStart = 29137, XrefRangeEnd = 29138, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CurrentTemperatureVelocity_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_CurrentTemperatureVelocity_Private_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool IsTemperatureInRange
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 233649, RefRangeEnd = 233650, XrefRangeStart = 233645, XrefRangeEnd = 233649, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsTemperatureInRange_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe float OverheatScale
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_OverheatScale_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_OverheatScale_Private_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe StationRecipe Recipe
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 32794, RefRangeEnd = 32795, XrefRangeStart = 32794, XrefRangeEnd = 32795, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Recipe_Public_get_StationRecipe_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<StationRecipe>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Recipe_Private_set_Void_StationRecipe_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static BoilingFlask()
	{
		Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.StationFramework", "BoilingFlask");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr);
		NativeFieldInfoPtr_TEMPERATURE_MAX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, "TEMPERATURE_MAX");
		NativeFieldInfoPtr_TEMPERATURE_MAX_VELOCITY = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, "TEMPERATURE_MAX_VELOCITY");
		NativeFieldInfoPtr_TEMPERATURE_ACCELERATION = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, "TEMPERATURE_ACCELERATION");
		NativeFieldInfoPtr_OVERHEAT_TIME = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, "OVERHEAT_TIME");
		NativeFieldInfoPtr__CurrentTemperature_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, "<CurrentTemperature>k__BackingField");
		NativeFieldInfoPtr__CurrentTemperatureVelocity_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, "<CurrentTemperatureVelocity>k__BackingField");
		NativeFieldInfoPtr__OverheatScale_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, "<OverheatScale>k__BackingField");
		NativeFieldInfoPtr__Recipe_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, "<Recipe>k__BackingField");
		NativeFieldInfoPtr_LockTemperature = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, "LockTemperature");
		NativeFieldInfoPtr_BoilSoundPitchCurve = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, "BoilSoundPitchCurve");
		NativeFieldInfoPtr_LabelJitterScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, "LabelJitterScale");
		NativeFieldInfoPtr_Burner = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, "Burner");
		NativeFieldInfoPtr_TemperatureCanvas = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, "TemperatureCanvas");
		NativeFieldInfoPtr_TemperatureLabel = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, "TemperatureLabel");
		NativeFieldInfoPtr_TemperatureSlider = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, "TemperatureSlider");
		NativeFieldInfoPtr_TemperatureRangeIndicator = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, "TemperatureRangeIndicator");
		NativeFieldInfoPtr_SmokeParticles = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, "SmokeParticles");
		NativeFieldInfoPtr_BoilSound = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, "BoilSound");
		NativeFieldInfoPtr_OverheatMesh = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, "OverheatMesh");
		NativeMethodInfoPtr_get_CurrentTemperature_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, 100678889);
		NativeMethodInfoPtr_set_CurrentTemperature_Private_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, 100678890);
		NativeMethodInfoPtr_get_CurrentTemperatureVelocity_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, 100678891);
		NativeMethodInfoPtr_set_CurrentTemperatureVelocity_Private_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, 100678892);
		NativeMethodInfoPtr_get_IsTemperatureInRange_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, 100678893);
		NativeMethodInfoPtr_get_OverheatScale_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, 100678894);
		NativeMethodInfoPtr_set_OverheatScale_Private_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, 100678895);
		NativeMethodInfoPtr_get_Recipe_Public_get_StationRecipe_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, 100678896);
		NativeMethodInfoPtr_set_Recipe_Private_set_Void_StationRecipe_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, 100678897);
		NativeMethodInfoPtr_Start_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, 100678898);
		NativeMethodInfoPtr_Update_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, 100678899);
		NativeMethodInfoPtr_FixedUpdate_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, 100678900);
		NativeMethodInfoPtr_UpdateCanvas_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, 100678901);
		NativeMethodInfoPtr_UpdateSmoke_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, 100678902);
		NativeMethodInfoPtr_SetCanvasVisible_Public_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, 100678903);
		NativeMethodInfoPtr_SetTemperature_Public_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, 100678904);
		NativeMethodInfoPtr_SetRecipe_Public_Void_StationRecipe_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, 100678905);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr, 100678906);
	}

	[CallerCount(14950)]
	[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 19142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Start_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 233650, XrefRangeEnd = 233679, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Update_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 233679, XrefRangeEnd = 233688, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void FixedUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FixedUpdate_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 233703, RefRangeEnd = 233704, XrefRangeStart = 233688, XrefRangeEnd = 233703, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateCanvas()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateCanvas_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 233704, XrefRangeEnd = 233712, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateSmoke()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateSmoke_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 233715, RefRangeEnd = 233718, XrefRangeStart = 233712, XrefRangeEnd = 233715, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetCanvasVisible(bool visible)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&visible);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetCanvasVisible_Public_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 29134, RefRangeEnd = 29137, XrefRangeStart = 29134, XrefRangeEnd = 29137, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetTemperature(float temp)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&temp);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetTemperature_Public_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 233729, RefRangeEnd = 233732, XrefRangeStart = 233718, XrefRangeEnd = 233729, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetRecipe(StationRecipe recipe)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)recipe);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetRecipe_Public_Void_StationRecipe_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 233732, XrefRangeEnd = 233740, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe BoilingFlask()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<BoilingFlask>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public BoilingFlask(IntPtr pointer)
		: base(pointer)
	{
	}
}
