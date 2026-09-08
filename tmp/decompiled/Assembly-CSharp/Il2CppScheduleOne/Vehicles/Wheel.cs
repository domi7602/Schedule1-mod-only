using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Audio;
using Il2CppScheduleOne.Core.Weather;
using Il2CppScheduleOne.Experimental;
using UnityEngine;

namespace Il2CppScheduleOne.Vehicles;

public class Wheel : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_SIDEWAY_SLIP_THRESHOLD;

	private static readonly IntPtr NativeFieldInfoPtr_FORWARD_SLIP_THRESHOLD;

	private static readonly IntPtr NativeFieldInfoPtr_DRIFT_AUDIO_THRESHOLD;

	private static readonly IntPtr NativeFieldInfoPtr_MIN_SPEED_FOR_DRIFT;

	private static readonly IntPtr NativeFieldInfoPtr_WHEEL_ANIMATION_DISTANCE;

	private static readonly IntPtr NativeFieldInfoPtr_HandbrakeFowardStiffnessMultiplier_Front;

	private static readonly IntPtr NativeFieldInfoPtr_HandbrakeSidewayStiffnessMultiplier_Front;

	private static readonly IntPtr NativeFieldInfoPtr_HandbrakeFowardStiffnessMultiplier_Rear;

	private static readonly IntPtr NativeFieldInfoPtr_HandbrakeSidewayStiffnessMultiplier_Rear;

	private static readonly IntPtr NativeFieldInfoPtr_DEBUG_MODE;

	private static readonly IntPtr NativeFieldInfoPtr_wheelModel;

	private static readonly IntPtr NativeFieldInfoPtr_modelContainer;

	private static readonly IntPtr NativeFieldInfoPtr_wheelCollider;

	private static readonly IntPtr NativeFieldInfoPtr_axleConnectionPoint;

	private static readonly IntPtr NativeFieldInfoPtr_staticCollider;

	private static readonly IntPtr NativeFieldInfoPtr_DriftParticles;

	private static readonly IntPtr NativeFieldInfoPtr__defaultData;

	private static readonly IntPtr NativeFieldInfoPtr__rainOverrideData;

	private static readonly IntPtr NativeFieldInfoPtr_DriftParticlesEnabled;

	private static readonly IntPtr NativeFieldInfoPtr_DriftAudioEnabled;

	private static readonly IntPtr NativeFieldInfoPtr_DriftAudioSource;

	private static readonly IntPtr NativeFieldInfoPtr_defaultForwardStiffness;

	private static readonly IntPtr NativeFieldInfoPtr_defaultSidewaysStiffness;

	private static readonly IntPtr NativeFieldInfoPtr__IsDrifting_k__BackingField;

	private static readonly IntPtr NativeFieldInfoPtr__DriftTime_k__BackingField;

	private static readonly IntPtr NativeFieldInfoPtr__DriftIntensity_k__BackingField;

	private static readonly IntPtr NativeFieldInfoPtr__IsSteerWheel_k__BackingField;

	private static readonly IntPtr NativeFieldInfoPtr_vehicle;

	private static readonly IntPtr NativeFieldInfoPtr_lastFixedUpdatePosition;

	private static readonly IntPtr NativeFieldInfoPtr_wheelData;

	private static readonly IntPtr NativeFieldInfoPtr_forwardCurve;

	private static readonly IntPtr NativeFieldInfoPtr_sidewaysCurve;

	private static readonly IntPtr NativeFieldInfoPtr__settings;

	private static readonly IntPtr NativeMethodInfoPtr_get_IsDrifting_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_IsDrifting_Protected_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_IsDrifting_Smoothed_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_DriftTime_Public_get_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_DriftTime_Protected_set_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_DriftIntensity_Public_get_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_DriftIntensity_Protected_set_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_IsSteerWheel_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_IsSteerWheel_Public_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Start_Protected_Virtual_New_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_FixedUpdateWheel_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_FakeWheelRotation_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_CheckDrifting_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_UpdateDriftEffects_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_UpdateDriftAudio_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_ApplyFriction_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetPhysicsEnabled_Public_Virtual_New_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_IsWheelGrounded_Public_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnWeatherChange_Public_Void_WeatherConditions_0;

	private static readonly IntPtr NativeMethodInfoPtr_ApplyDefaultWheelModelPosition_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static float SIDEWAY_SLIP_THRESHOLD
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SIDEWAY_SLIP_THRESHOLD, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SIDEWAY_SLIP_THRESHOLD, (void*)(&num));
		}
	}

	public unsafe static float FORWARD_SLIP_THRESHOLD
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FORWARD_SLIP_THRESHOLD, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FORWARD_SLIP_THRESHOLD, (void*)(&num));
		}
	}

	public unsafe static float DRIFT_AUDIO_THRESHOLD
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DRIFT_AUDIO_THRESHOLD, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DRIFT_AUDIO_THRESHOLD, (void*)(&num));
		}
	}

	public unsafe static float MIN_SPEED_FOR_DRIFT
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MIN_SPEED_FOR_DRIFT, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MIN_SPEED_FOR_DRIFT, (void*)(&num));
		}
	}

	public unsafe static float WHEEL_ANIMATION_DISTANCE
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_WHEEL_ANIMATION_DISTANCE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_WHEEL_ANIMATION_DISTANCE, (void*)(&num));
		}
	}

	public unsafe static float HandbrakeFowardStiffnessMultiplier_Front
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_HandbrakeFowardStiffnessMultiplier_Front, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_HandbrakeFowardStiffnessMultiplier_Front, (void*)(&num));
		}
	}

	public unsafe static float HandbrakeSidewayStiffnessMultiplier_Front
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_HandbrakeSidewayStiffnessMultiplier_Front, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_HandbrakeSidewayStiffnessMultiplier_Front, (void*)(&num));
		}
	}

	public unsafe static float HandbrakeFowardStiffnessMultiplier_Rear
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_HandbrakeFowardStiffnessMultiplier_Rear, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_HandbrakeFowardStiffnessMultiplier_Rear, (void*)(&num));
		}
	}

	public unsafe static float HandbrakeSidewayStiffnessMultiplier_Rear
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_HandbrakeSidewayStiffnessMultiplier_Rear, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_HandbrakeSidewayStiffnessMultiplier_Rear, (void*)(&num));
		}
	}

	public unsafe bool DEBUG_MODE
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DEBUG_MODE);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DEBUG_MODE)) = flag;
		}
	}

	public unsafe Transform wheelModel
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_wheelModel);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_wheelModel)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe Transform modelContainer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_modelContainer);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_modelContainer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe WheelCollider wheelCollider
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_wheelCollider);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<WheelCollider>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_wheelCollider)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)wheelCollider));
		}
	}

	public unsafe Transform axleConnectionPoint
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_axleConnectionPoint);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_axleConnectionPoint)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe Collider staticCollider
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_staticCollider);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Collider>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_staticCollider)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)collider));
		}
	}

	public unsafe ParticleSystem DriftParticles
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DriftParticles);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ParticleSystem>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DriftParticles)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)particleSystem));
		}
	}

	public unsafe WheelData _defaultData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__defaultData);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<WheelData>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__defaultData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)wheelData));
		}
	}

	public unsafe WheelOverrideData _rainOverrideData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__rainOverrideData);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<WheelOverrideData>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__rainOverrideData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)wheelOverrideData));
		}
	}

	public unsafe bool DriftParticlesEnabled
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DriftParticlesEnabled);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DriftParticlesEnabled)) = flag;
		}
	}

	public unsafe bool DriftAudioEnabled
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DriftAudioEnabled);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DriftAudioEnabled)) = flag;
		}
	}

	public unsafe AudioSourceController DriftAudioSource
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DriftAudioSource);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AudioSourceController>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DriftAudioSource)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)audioSourceController));
		}
	}

	public unsafe float defaultForwardStiffness
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_defaultForwardStiffness);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_defaultForwardStiffness)) = num;
		}
	}

	public unsafe float defaultSidewaysStiffness
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_defaultSidewaysStiffness);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_defaultSidewaysStiffness)) = num;
		}
	}

	public unsafe bool _IsDrifting_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__IsDrifting_k__BackingField);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__IsDrifting_k__BackingField)) = flag;
		}
	}

	public unsafe float _DriftTime_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__DriftTime_k__BackingField);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__DriftTime_k__BackingField)) = num;
		}
	}

	public unsafe float _DriftIntensity_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__DriftIntensity_k__BackingField);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__DriftIntensity_k__BackingField)) = num;
		}
	}

	public unsafe bool _IsSteerWheel_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__IsSteerWheel_k__BackingField);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__IsSteerWheel_k__BackingField)) = flag;
		}
	}

	public unsafe LandVehicle vehicle
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_vehicle);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<LandVehicle>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_vehicle)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)landVehicle));
		}
	}

	public unsafe Vector3 lastFixedUpdatePosition
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastFixedUpdatePosition);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastFixedUpdatePosition)) = vector;
		}
	}

	public unsafe WheelHit wheelData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_wheelData);
			return new WheelHit(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<WheelHit>.NativeClassPtr, (IntPtr)num));
		}
		set
		{
			// IL cpblk instruction
			Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_wheelData), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)wheelHit)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<WheelHit>.NativeClassPtr, ref *(uint*)null));
		}
	}

	public unsafe WheelFrictionCurve forwardCurve
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_forwardCurve);
			return *(WheelFrictionCurve*)num;
		}
		set
		{
			*(WheelFrictionCurve*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_forwardCurve)) = wheelFrictionCurve;
		}
	}

	public unsafe WheelFrictionCurve sidewaysCurve
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sidewaysCurve);
			return *(WheelFrictionCurve*)num;
		}
		set
		{
			*(WheelFrictionCurve*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sidewaysCurve)) = wheelFrictionCurve;
		}
	}

	public unsafe VehicleSettings _settings
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__settings);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<VehicleSettings>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__settings)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vehicleSettings));
		}
	}

	public unsafe bool IsDrifting
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsDrifting_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_IsDrifting_Protected_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool IsDrifting_Smoothed
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsDrifting_Smoothed_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe float DriftTime
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_DriftTime_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_DriftTime_Protected_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float DriftIntensity
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_DriftIntensity_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_DriftIntensity_Protected_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool IsSteerWheel
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsSteerWheel_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_IsSteerWheel_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static Wheel()
	{
		Il2CppClassPointerStore<Wheel>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Vehicles", "Wheel");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Wheel>.NativeClassPtr);
		NativeFieldInfoPtr_SIDEWAY_SLIP_THRESHOLD = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wheel>.NativeClassPtr, "SIDEWAY_SLIP_THRESHOLD");
		NativeFieldInfoPtr_FORWARD_SLIP_THRESHOLD = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wheel>.NativeClassPtr, "FORWARD_SLIP_THRESHOLD");
		NativeFieldInfoPtr_DRIFT_AUDIO_THRESHOLD = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wheel>.NativeClassPtr, "DRIFT_AUDIO_THRESHOLD");
		NativeFieldInfoPtr_MIN_SPEED_FOR_DRIFT = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wheel>.NativeClassPtr, "MIN_SPEED_FOR_DRIFT");
		NativeFieldInfoPtr_WHEEL_ANIMATION_DISTANCE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wheel>.NativeClassPtr, "WHEEL_ANIMATION_DISTANCE");
		NativeFieldInfoPtr_HandbrakeFowardStiffnessMultiplier_Front = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wheel>.NativeClassPtr, "HandbrakeFowardStiffnessMultiplier_Front");
		NativeFieldInfoPtr_HandbrakeSidewayStiffnessMultiplier_Front = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wheel>.NativeClassPtr, "HandbrakeSidewayStiffnessMultiplier_Front");
		NativeFieldInfoPtr_HandbrakeFowardStiffnessMultiplier_Rear = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wheel>.NativeClassPtr, "HandbrakeFowardStiffnessMultiplier_Rear");
		NativeFieldInfoPtr_HandbrakeSidewayStiffnessMultiplier_Rear = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wheel>.NativeClassPtr, "HandbrakeSidewayStiffnessMultiplier_Rear");
		NativeFieldInfoPtr_DEBUG_MODE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wheel>.NativeClassPtr, "DEBUG_MODE");
		NativeFieldInfoPtr_wheelModel = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wheel>.NativeClassPtr, "wheelModel");
		NativeFieldInfoPtr_modelContainer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wheel>.NativeClassPtr, "modelContainer");
		NativeFieldInfoPtr_wheelCollider = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wheel>.NativeClassPtr, "wheelCollider");
		NativeFieldInfoPtr_axleConnectionPoint = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wheel>.NativeClassPtr, "axleConnectionPoint");
		NativeFieldInfoPtr_staticCollider = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wheel>.NativeClassPtr, "staticCollider");
		NativeFieldInfoPtr_DriftParticles = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wheel>.NativeClassPtr, "DriftParticles");
		NativeFieldInfoPtr__defaultData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wheel>.NativeClassPtr, "_defaultData");
		NativeFieldInfoPtr__rainOverrideData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wheel>.NativeClassPtr, "_rainOverrideData");
		NativeFieldInfoPtr_DriftParticlesEnabled = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wheel>.NativeClassPtr, "DriftParticlesEnabled");
		NativeFieldInfoPtr_DriftAudioEnabled = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wheel>.NativeClassPtr, "DriftAudioEnabled");
		NativeFieldInfoPtr_DriftAudioSource = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wheel>.NativeClassPtr, "DriftAudioSource");
		NativeFieldInfoPtr_defaultForwardStiffness = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wheel>.NativeClassPtr, "defaultForwardStiffness");
		NativeFieldInfoPtr_defaultSidewaysStiffness = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wheel>.NativeClassPtr, "defaultSidewaysStiffness");
		NativeFieldInfoPtr__IsDrifting_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wheel>.NativeClassPtr, "<IsDrifting>k__BackingField");
		NativeFieldInfoPtr__DriftTime_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wheel>.NativeClassPtr, "<DriftTime>k__BackingField");
		NativeFieldInfoPtr__DriftIntensity_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wheel>.NativeClassPtr, "<DriftIntensity>k__BackingField");
		NativeFieldInfoPtr__IsSteerWheel_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wheel>.NativeClassPtr, "<IsSteerWheel>k__BackingField");
		NativeFieldInfoPtr_vehicle = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wheel>.NativeClassPtr, "vehicle");
		NativeFieldInfoPtr_lastFixedUpdatePosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wheel>.NativeClassPtr, "lastFixedUpdatePosition");
		NativeFieldInfoPtr_wheelData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wheel>.NativeClassPtr, "wheelData");
		NativeFieldInfoPtr_forwardCurve = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wheel>.NativeClassPtr, "forwardCurve");
		NativeFieldInfoPtr_sidewaysCurve = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wheel>.NativeClassPtr, "sidewaysCurve");
		NativeFieldInfoPtr__settings = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wheel>.NativeClassPtr, "_settings");
		NativeMethodInfoPtr_get_IsDrifting_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Wheel>.NativeClassPtr, 100666320);
		NativeMethodInfoPtr_set_IsDrifting_Protected_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Wheel>.NativeClassPtr, 100666321);
		NativeMethodInfoPtr_get_IsDrifting_Smoothed_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Wheel>.NativeClassPtr, 100666322);
		NativeMethodInfoPtr_get_DriftTime_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Wheel>.NativeClassPtr, 100666323);
		NativeMethodInfoPtr_set_DriftTime_Protected_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Wheel>.NativeClassPtr, 100666324);
		NativeMethodInfoPtr_get_DriftIntensity_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Wheel>.NativeClassPtr, 100666325);
		NativeMethodInfoPtr_set_DriftIntensity_Protected_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Wheel>.NativeClassPtr, 100666326);
		NativeMethodInfoPtr_get_IsSteerWheel_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Wheel>.NativeClassPtr, 100666327);
		NativeMethodInfoPtr_set_IsSteerWheel_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Wheel>.NativeClassPtr, 100666328);
		NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Wheel>.NativeClassPtr, 100666329);
		NativeMethodInfoPtr_Start_Protected_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Wheel>.NativeClassPtr, 100666330);
		NativeMethodInfoPtr_FixedUpdateWheel_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Wheel>.NativeClassPtr, 100666331);
		NativeMethodInfoPtr_FakeWheelRotation_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Wheel>.NativeClassPtr, 100666332);
		NativeMethodInfoPtr_CheckDrifting_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Wheel>.NativeClassPtr, 100666333);
		NativeMethodInfoPtr_UpdateDriftEffects_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Wheel>.NativeClassPtr, 100666334);
		NativeMethodInfoPtr_UpdateDriftAudio_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Wheel>.NativeClassPtr, 100666335);
		NativeMethodInfoPtr_ApplyFriction_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Wheel>.NativeClassPtr, 100666336);
		NativeMethodInfoPtr_SetPhysicsEnabled_Public_Virtual_New_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Wheel>.NativeClassPtr, 100666337);
		NativeMethodInfoPtr_IsWheelGrounded_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Wheel>.NativeClassPtr, 100666338);
		NativeMethodInfoPtr_OnWeatherChange_Public_Void_WeatherConditions_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Wheel>.NativeClassPtr, 100666339);
		NativeMethodInfoPtr_ApplyDefaultWheelModelPosition_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Wheel>.NativeClassPtr, 100666340);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Wheel>.NativeClassPtr, 100666341);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 94946, XrefRangeEnd = 94961, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 94961, XrefRangeEnd = 94966, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Start_Protected_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 94988, RefRangeEnd = 94989, XrefRangeStart = 94966, XrefRangeEnd = 94988, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void FixedUpdateWheel()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FixedUpdateWheel_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 95002, RefRangeEnd = 95003, XrefRangeStart = 94989, XrefRangeEnd = 95002, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void FakeWheelRotation()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FakeWheelRotation_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 95034, RefRangeEnd = 95035, XrefRangeStart = 95003, XrefRangeEnd = 95034, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void CheckDrifting()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CheckDrifting_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 95035, XrefRangeEnd = 95037, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateDriftEffects()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateDriftEffects_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 95037, XrefRangeEnd = 95042, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateDriftAudio()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateDriftAudio_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 95061, RefRangeEnd = 95062, XrefRangeStart = 95042, XrefRangeEnd = 95061, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ApplyFriction()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApplyFriction_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 95062, XrefRangeEnd = 95082, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void SetPhysicsEnabled(bool enabled)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&enabled);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_SetPhysicsEnabled_Public_Virtual_New_Void_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 95082, XrefRangeEnd = 95083, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsWheelGrounded()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsWheelGrounded_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 95097, RefRangeEnd = 95098, XrefRangeStart = 95083, XrefRangeEnd = 95097, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnWeatherChange(WeatherConditions newConditions)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)newConditions);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnWeatherChange_Public_Void_WeatherConditions_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 95098, XrefRangeEnd = 95110, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ApplyDefaultWheelModelPosition()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApplyDefaultWheelModelPosition_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 95110, XrefRangeEnd = 95113, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Wheel()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Wheel>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Wheel(IntPtr pointer)
		: base(pointer)
	{
	}
}
