using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Core;
using UnityEngine;

namespace Il2CppScheduleOne.Tools;

public class GenericFootstepDetector : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_GroundDetectionRange;

	private static readonly IntPtr NativeFieldInfoPtr_GroundDetectionRayOriginShift;

	private static readonly IntPtr NativeFieldInfoPtr__VolumeMultiplier_k__BackingField;

	private static readonly IntPtr NativeFieldInfoPtr__baseVolume;

	private static readonly IntPtr NativeFieldInfoPtr__stepDetectionCooldown;

	private static readonly IntPtr NativeFieldInfoPtr__referencePoint;

	private static readonly IntPtr NativeFieldInfoPtr__timeOnLastStep;

	private static readonly IntPtr NativeFieldInfoPtr__groundDetectionLayerMask;

	private static readonly IntPtr NativeMethodInfoPtr_get_VolumeMultiplier_Public_get_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_VolumeMultiplier_Public_set_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Start_Protected_Virtual_New_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_TriggerStep_Protected_Void_EMaterialType_Vector3_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_IsCooldown_Protected_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_IsGrounded_Protected_Boolean_byref_EMaterialType_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Protected_Void_0;

	public unsafe static float GroundDetectionRange
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_GroundDetectionRange, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_GroundDetectionRange, (void*)(&num));
		}
	}

	public unsafe static float GroundDetectionRayOriginShift
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_GroundDetectionRayOriginShift, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_GroundDetectionRayOriginShift, (void*)(&num));
		}
	}

	public unsafe float _VolumeMultiplier_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__VolumeMultiplier_k__BackingField);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__VolumeMultiplier_k__BackingField)) = num;
		}
	}

	public unsafe float _baseVolume
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__baseVolume);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__baseVolume)) = num;
		}
	}

	public unsafe float _stepDetectionCooldown
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__stepDetectionCooldown);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__stepDetectionCooldown)) = num;
		}
	}

	public unsafe Transform _referencePoint
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__referencePoint);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__referencePoint)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe float _timeOnLastStep
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__timeOnLastStep);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__timeOnLastStep)) = num;
		}
	}

	public unsafe static LayerMask _groundDetectionLayerMask
	{
		get
		{
			Unsafe.SkipInit(out LayerMask result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__groundDetectionLayerMask, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__groundDetectionLayerMask, (void*)(&layerMask));
		}
	}

	public unsafe float VolumeMultiplier
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_VolumeMultiplier_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 29040, RefRangeEnd = 29041, XrefRangeStart = 29040, XrefRangeEnd = 29041, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_VolumeMultiplier_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static GenericFootstepDetector()
	{
		Il2CppClassPointerStore<GenericFootstepDetector>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Tools", "GenericFootstepDetector");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GenericFootstepDetector>.NativeClassPtr);
		NativeFieldInfoPtr_GroundDetectionRange = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GenericFootstepDetector>.NativeClassPtr, "GroundDetectionRange");
		NativeFieldInfoPtr_GroundDetectionRayOriginShift = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GenericFootstepDetector>.NativeClassPtr, "GroundDetectionRayOriginShift");
		NativeFieldInfoPtr__VolumeMultiplier_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GenericFootstepDetector>.NativeClassPtr, "<VolumeMultiplier>k__BackingField");
		NativeFieldInfoPtr__baseVolume = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GenericFootstepDetector>.NativeClassPtr, "_baseVolume");
		NativeFieldInfoPtr__stepDetectionCooldown = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GenericFootstepDetector>.NativeClassPtr, "_stepDetectionCooldown");
		NativeFieldInfoPtr__referencePoint = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GenericFootstepDetector>.NativeClassPtr, "_referencePoint");
		NativeFieldInfoPtr__timeOnLastStep = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GenericFootstepDetector>.NativeClassPtr, "_timeOnLastStep");
		NativeFieldInfoPtr__groundDetectionLayerMask = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GenericFootstepDetector>.NativeClassPtr, "_groundDetectionLayerMask");
		NativeMethodInfoPtr_get_VolumeMultiplier_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GenericFootstepDetector>.NativeClassPtr, 100678036);
		NativeMethodInfoPtr_set_VolumeMultiplier_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GenericFootstepDetector>.NativeClassPtr, 100678037);
		NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GenericFootstepDetector>.NativeClassPtr, 100678038);
		NativeMethodInfoPtr_Start_Protected_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GenericFootstepDetector>.NativeClassPtr, 100678039);
		NativeMethodInfoPtr_TriggerStep_Protected_Void_EMaterialType_Vector3_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GenericFootstepDetector>.NativeClassPtr, 100678040);
		NativeMethodInfoPtr_IsCooldown_Protected_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GenericFootstepDetector>.NativeClassPtr, 100678041);
		NativeMethodInfoPtr_IsGrounded_Protected_Boolean_byref_EMaterialType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GenericFootstepDetector>.NativeClassPtr, 100678042);
		NativeMethodInfoPtr__ctor_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GenericFootstepDetector>.NativeClassPtr, 100678043);
	}

	[CallerCount(14950)]
	[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 19142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 225979, XrefRangeEnd = 226001, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Start_Protected_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 226012, RefRangeEnd = 226014, XrefRangeStart = 226001, XrefRangeEnd = 226012, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void TriggerStep(EMaterialType materialType, Vector3 stepPosition, bool spatialAudio = true)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = (nint)(&materialType);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &stepPosition;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &spatialAudio;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TriggerStep_Protected_Void_EMaterialType_Vector3_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 226015, RefRangeEnd = 226016, XrefRangeStart = 226014, XrefRangeEnd = 226015, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsCooldown()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsCooldown_Protected_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 226032, RefRangeEnd = 226034, XrefRangeStart = 226016, XrefRangeEnd = 226032, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsGrounded(out EMaterialType surfaceType)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)Unsafe.AsPointer(ref surfaceType);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsGrounded_Protected_Boolean_byref_EMaterialType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 226035, RefRangeEnd = 226037, XrefRangeStart = 226034, XrefRangeEnd = 226035, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GenericFootstepDetector()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<GenericFootstepDetector>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public GenericFootstepDetector(IntPtr pointer)
		: base(pointer)
	{
	}
}
