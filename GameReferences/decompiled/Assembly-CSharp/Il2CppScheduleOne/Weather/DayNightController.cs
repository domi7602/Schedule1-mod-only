using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Core;
using Il2CppScheduleOne.Core.Weather;
using UnityEngine;

namespace Il2CppScheduleOne.Weather;

public class DayNightController : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr__lightPivot;

	private static readonly IntPtr NativeFieldInfoPtr__skyRenderer;

	private static readonly IntPtr NativeFieldInfoPtr__sunLight;

	private static readonly IntPtr NativeFieldInfoPtr__moonLight;

	private static readonly IntPtr NativeFieldInfoPtr__ambientLight;

	private static readonly IntPtr NativeFieldInfoPtr__fadeInCurve;

	private static readonly IntPtr NativeFieldInfoPtr__fadeOutCurve;

	private static readonly IntPtr NativeFieldInfoPtr__dayNightPhaseTimes;

	private static readonly IntPtr NativeFieldInfoPtr__debugRotationSpeed;

	private static readonly IntPtr NativeFieldInfoPtr__debugTimeSpeed;

	private static readonly IntPtr NativeFieldInfoPtr__enableDebugTimeControl;

	private static readonly IntPtr NativeFieldInfoPtr__debugAutoUpdateTime;

	private static readonly IntPtr NativeFieldInfoPtr__timeInHours;

	private static readonly IntPtr NativeFieldInfoPtr__timePercentage;

	private static readonly IntPtr NativeFieldInfoPtr__isDay;

	private static readonly IntPtr NativeFieldInfoPtr__currentSunRotation;

	private static readonly IntPtr NativeFieldInfoPtr__currentMoonRotation;

	private static readonly IntPtr NativeFieldInfoPtr_SUN_SHADOW_STRENGTH;

	private static readonly IntPtr NativeFieldInfoPtr_MOON_SHADOW_STRENGTH;

	private static readonly IntPtr NativeFieldInfoPtr_MAX_LIGHT_INTENSITY;

	private static readonly IntPtr NativeMethodInfoPtr_get_EnableDebugTimeControl_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_Update_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_EvaluateSky_Private_SkyState_SkyState_SkySettings_SkySettings_Single_Single_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_BlendSky_Private_SkyState_SkyState_SkyState_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_UpdateSky_Private_Void_SkyState_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetLights_Private_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_UpdateRotation_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_SnapRotation_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetRotation_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_IsDay_Private_Boolean_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_EvaluateSky_Public_SkyState_SkySettings_SkySettings_Single_SkySettings_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_EvaluateFloatByTimeOfDay_Public_Single_DynamicGradient_0;

	private static readonly IntPtr NativeMethodInfoPtr_EvaluateColorByTimeOfDay_Public_Color_DynamicGradient_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnUpdateTime_Public_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnTick_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnTimeSet_Public_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe GameObject _lightPivot
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__lightPivot);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<GameObject>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__lightPivot)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gameObject));
		}
	}

	public unsafe MeshRenderer _skyRenderer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__skyRenderer);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<MeshRenderer>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__skyRenderer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)meshRenderer));
		}
	}

	public unsafe Light _sunLight
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__sunLight);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Light>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__sunLight)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)light));
		}
	}

	public unsafe Light _moonLight
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__moonLight);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Light>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__moonLight)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)light));
		}
	}

	public unsafe Light _ambientLight
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ambientLight);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Light>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ambientLight)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)light));
		}
	}

	public unsafe AnimationCurve _fadeInCurve
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__fadeInCurve);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AnimationCurve>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__fadeInCurve)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationCurve));
		}
	}

	public unsafe AnimationCurve _fadeOutCurve
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__fadeOutCurve);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AnimationCurve>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__fadeOutCurve)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationCurve));
		}
	}

	public unsafe DayNightPhaseTimes _dayNightPhaseTimes
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__dayNightPhaseTimes);
			return *(DayNightPhaseTimes*)num;
		}
		set
		{
			*(DayNightPhaseTimes*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__dayNightPhaseTimes)) = dayNightPhaseTimes;
		}
	}

	public unsafe float _debugRotationSpeed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__debugRotationSpeed);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__debugRotationSpeed)) = num;
		}
	}

	public unsafe float _debugTimeSpeed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__debugTimeSpeed);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__debugTimeSpeed)) = num;
		}
	}

	public unsafe bool _enableDebugTimeControl
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__enableDebugTimeControl);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__enableDebugTimeControl)) = flag;
		}
	}

	public unsafe bool _debugAutoUpdateTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__debugAutoUpdateTime);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__debugAutoUpdateTime)) = flag;
		}
	}

	public unsafe float _timeInHours
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__timeInHours);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__timeInHours)) = num;
		}
	}

	public unsafe float _timePercentage
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__timePercentage);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__timePercentage)) = num;
		}
	}

	public unsafe bool _isDay
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__isDay);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__isDay)) = flag;
		}
	}

	public unsafe Quaternion _currentSunRotation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__currentSunRotation);
			return *(Quaternion*)num;
		}
		set
		{
			*(Quaternion*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__currentSunRotation)) = quaternion;
		}
	}

	public unsafe Quaternion _currentMoonRotation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__currentMoonRotation);
			return *(Quaternion*)num;
		}
		set
		{
			*(Quaternion*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__currentMoonRotation)) = quaternion;
		}
	}

	public unsafe static float SUN_SHADOW_STRENGTH
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SUN_SHADOW_STRENGTH, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SUN_SHADOW_STRENGTH, (void*)(&num));
		}
	}

	public unsafe static float MOON_SHADOW_STRENGTH
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MOON_SHADOW_STRENGTH, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MOON_SHADOW_STRENGTH, (void*)(&num));
		}
	}

	public unsafe static float MAX_LIGHT_INTENSITY
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MAX_LIGHT_INTENSITY, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MAX_LIGHT_INTENSITY, (void*)(&num));
		}
	}

	public unsafe bool EnableDebugTimeControl
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_EnableDebugTimeControl_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static DayNightController()
	{
		Il2CppClassPointerStore<DayNightController>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Weather", "DayNightController");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DayNightController>.NativeClassPtr);
		NativeFieldInfoPtr__lightPivot = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, "_lightPivot");
		NativeFieldInfoPtr__skyRenderer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, "_skyRenderer");
		NativeFieldInfoPtr__sunLight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, "_sunLight");
		NativeFieldInfoPtr__moonLight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, "_moonLight");
		NativeFieldInfoPtr__ambientLight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, "_ambientLight");
		NativeFieldInfoPtr__fadeInCurve = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, "_fadeInCurve");
		NativeFieldInfoPtr__fadeOutCurve = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, "_fadeOutCurve");
		NativeFieldInfoPtr__dayNightPhaseTimes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, "_dayNightPhaseTimes");
		NativeFieldInfoPtr__debugRotationSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, "_debugRotationSpeed");
		NativeFieldInfoPtr__debugTimeSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, "_debugTimeSpeed");
		NativeFieldInfoPtr__enableDebugTimeControl = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, "_enableDebugTimeControl");
		NativeFieldInfoPtr__debugAutoUpdateTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, "_debugAutoUpdateTime");
		NativeFieldInfoPtr__timeInHours = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, "_timeInHours");
		NativeFieldInfoPtr__timePercentage = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, "_timePercentage");
		NativeFieldInfoPtr__isDay = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, "_isDay");
		NativeFieldInfoPtr__currentSunRotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, "_currentSunRotation");
		NativeFieldInfoPtr__currentMoonRotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, "_currentMoonRotation");
		NativeFieldInfoPtr_SUN_SHADOW_STRENGTH = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, "SUN_SHADOW_STRENGTH");
		NativeFieldInfoPtr_MOON_SHADOW_STRENGTH = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, "MOON_SHADOW_STRENGTH");
		NativeFieldInfoPtr_MAX_LIGHT_INTENSITY = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, "MAX_LIGHT_INTENSITY");
		NativeMethodInfoPtr_get_EnableDebugTimeControl_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, 100685557);
		NativeMethodInfoPtr_Update_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, 100685558);
		NativeMethodInfoPtr_EvaluateSky_Private_SkyState_SkyState_SkySettings_SkySettings_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, 100685559);
		NativeMethodInfoPtr_BlendSky_Private_SkyState_SkyState_SkyState_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, 100685560);
		NativeMethodInfoPtr_UpdateSky_Private_Void_SkyState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, 100685561);
		NativeMethodInfoPtr_SetLights_Private_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, 100685562);
		NativeMethodInfoPtr_UpdateRotation_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, 100685563);
		NativeMethodInfoPtr_SnapRotation_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, 100685564);
		NativeMethodInfoPtr_SetRotation_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, 100685565);
		NativeMethodInfoPtr_IsDay_Private_Boolean_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, 100685566);
		NativeMethodInfoPtr_EvaluateSky_Public_SkyState_SkySettings_SkySettings_Single_SkySettings_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, 100685567);
		NativeMethodInfoPtr_EvaluateFloatByTimeOfDay_Public_Single_DynamicGradient_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, 100685568);
		NativeMethodInfoPtr_EvaluateColorByTimeOfDay_Public_Color_DynamicGradient_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, 100685569);
		NativeMethodInfoPtr_OnUpdateTime_Public_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, 100685570);
		NativeMethodInfoPtr_OnTick_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, 100685571);
		NativeMethodInfoPtr_OnTimeSet_Public_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, 100685572);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DayNightController>.NativeClassPtr, 100685573);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 291045, XrefRangeEnd = 291052, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Update_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 291153, RefRangeEnd = 291155, XrefRangeStart = 291052, XrefRangeEnd = 291153, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SkyState EvaluateSky(SkyState state, SkySettings activeSettings, SkySettings neighbourSettings, float blend, float timeInTwentyFourHour, float timePercentage)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[6];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)state);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)activeSettings);
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)neighbourSettings);
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &blend;
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = &timeInTwentyFourHour;
		*(float**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(IntPtr)))) = &timePercentage;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EvaluateSky_Private_SkyState_SkyState_SkySettings_SkySettings_Single_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SkyState>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 291192, RefRangeEnd = 291193, XrefRangeStart = 291155, XrefRangeEnd = 291192, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SkyState BlendSky(SkyState from, SkyState to, float blend)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)from);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)to);
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &blend;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BlendSky_Private_SkyState_SkyState_SkyState_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SkyState>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 291193, XrefRangeEnd = 291200, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateSky(SkyState skyState)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skyState);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateSky_Private_Void_SkyState_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 291200, XrefRangeEnd = 291206, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetLights(bool isDay)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&isDay);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetLights_Private_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 291228, RefRangeEnd = 291229, XrefRangeStart = 291206, XrefRangeEnd = 291228, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateRotation()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateRotation_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 291229, XrefRangeEnd = 291243, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SnapRotation()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SnapRotation_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 291245, RefRangeEnd = 291250, XrefRangeStart = 291243, XrefRangeEnd = 291245, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetRotation()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetRotation_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe bool IsDay(float timeInTwentyFourHour)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&timeInTwentyFourHour);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsDay_Private_Boolean_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 291272, RefRangeEnd = 291273, XrefRangeStart = 291250, XrefRangeEnd = 291272, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SkyState EvaluateSky(SkySettings activeSettings, SkySettings neighbourSettings, float blend, SkySettings overrideSkySettings = null, float overrideBlend = 0f)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)activeSettings);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)neighbourSettings);
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &blend;
		*(IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)overrideSkySettings);
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = &overrideBlend;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EvaluateSky_Public_SkyState_SkySettings_SkySettings_Single_SkySettings_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SkyState>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 291273, XrefRangeEnd = 291274, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float EvaluateFloatByTimeOfDay(DynamicGradient gradient)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gradient);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EvaluateFloatByTimeOfDay_Public_Single_DynamicGradient_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 291274, XrefRangeEnd = 291275, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Color EvaluateColorByTimeOfDay(DynamicGradient gradient)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gradient);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EvaluateColorByTimeOfDay_Public_Color_DynamicGradient_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 291275, XrefRangeEnd = 291278, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnUpdateTime(float normalisedTime)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&normalisedTime);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnUpdateTime_Public_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 291278, XrefRangeEnd = 291279, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnTick()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnTick_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 291279, XrefRangeEnd = 291297, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnTimeSet(float normalisedTime)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&normalisedTime);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnTimeSet_Public_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 291297, XrefRangeEnd = 291298, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DayNightController()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DayNightController>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public DayNightController(IntPtr pointer)
		: base(pointer)
	{
	}
}
