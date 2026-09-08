using System;
using System.Runtime.CompilerServices;
using Il2CppFluffyUnderware.Curvy.Generator;
using Il2CppFluffyUnderware.DevTools;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.Controllers;

public class VolumeController : CurvyController
{
	private static readonly IntPtr NativeFieldInfoPtr_CrossPositionRangeMin;

	private static readonly IntPtr NativeFieldInfoPtr_CrossPositionRangeMax;

	private static readonly IntPtr NativeFieldInfoPtr_m_Volume;

	private static readonly IntPtr NativeFieldInfoPtr_m_CrossRange;

	private static readonly IntPtr NativeFieldInfoPtr_crossRelativePosition;

	private static readonly IntPtr NativeFieldInfoPtr_m_CrossClamping;

	private static readonly IntPtr NativeFieldInfoPtr_m_CrossInitialPosition;

	private static readonly IntPtr NativeMethodInfoPtr_get_Volume_Public_get_CGDataReference_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_Volume_Public_set_Void_CGDataReference_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_VolumeData_Public_get_CGVolume_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_CrossFrom_Public_get_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_CrossFrom_Public_set_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_CrossTo_Public_get_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_CrossTo_Public_set_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_CrossLength_Public_get_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_CrossClamping_Public_get_CurvyClamping_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_CrossClamping_Public_set_Void_CurvyClamping_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_CrossRelativePosition_Public_get_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_CrossRelativePosition_Public_set_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_Length_Public_Virtual_get_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_CrossRelativeToAbsolute_Public_Single_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_CrossAbsoluteToRelative_Public_Single_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_IsReady_Public_Virtual_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_RelativeToAbsolute_Protected_Virtual_Single_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_AbsoluteToRelative_Protected_Virtual_Single_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetInterpolatedSourcePosition_Protected_Virtual_Vector3_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetInterpolatedSourcePosition_Protected_Virtual_Void_Single_byref_Vector3_byref_Vector3_byref_Vector3_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetTangent_Protected_Virtual_Vector3_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetOrientation_Protected_Virtual_Vector3_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_Advance_Protected_Virtual_Void_Single_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_SimulateAdvance_Protected_Virtual_Void_byref_Single_byref_MovementDirection_Single_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_CrossRangeOptions_Private_get_RegionOptions_1_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_MinCrossRelativePosition_Private_get_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_MaxCrossRelativePosition_Private_get_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetClampedCrossPosition_Private_Single_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnAfterDeserialize_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static float CrossPositionRangeMin
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_CrossPositionRangeMin, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_CrossPositionRangeMin, (void*)(&num));
		}
	}

	public unsafe static float CrossPositionRangeMax
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_CrossPositionRangeMax, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_CrossPositionRangeMax, (void*)(&num));
		}
	}

	public unsafe CGDataReference m_Volume
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Volume);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<CGDataReference>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Volume)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cGDataReference));
		}
	}

	public unsafe FloatRegion m_CrossRange
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_CrossRange);
			return *(FloatRegion*)num;
		}
		set
		{
			*(FloatRegion*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_CrossRange)) = floatRegion;
		}
	}

	public unsafe float crossRelativePosition
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_crossRelativePosition);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_crossRelativePosition)) = num;
		}
	}

	public unsafe CurvyClamping m_CrossClamping
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_CrossClamping);
			return *(CurvyClamping*)num;
		}
		set
		{
			*(CurvyClamping*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_CrossClamping)) = curvyClamping;
		}
	}

	public unsafe float m_CrossInitialPosition
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_CrossInitialPosition);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_CrossInitialPosition)) = num;
		}
	}

	public unsafe CGDataReference Volume
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Volume_Public_get_CGDataReference_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<CGDataReference>(intPtr) : null;
		}
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 38417, RefRangeEnd = 38421, XrefRangeStart = 38417, XrefRangeEnd = 38421, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Volume_Public_set_Void_CGDataReference_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe CGVolume VolumeData
	{
		[CallerCount(16)]
		[CachedScanResults(RefRangeStart = 45642, RefRangeEnd = 45658, XrefRangeStart = 45638, XrefRangeEnd = 45642, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_VolumeData_Public_get_CGVolume_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<CGVolume>(intPtr) : null;
		}
	}

	public unsafe float CrossFrom
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CrossFrom_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_CrossFrom_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float CrossTo
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CrossTo_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_CrossTo_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float CrossLength
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45658, XrefRangeEnd = 45659, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CrossLength_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe CurvyClamping CrossClamping
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CrossClamping_Public_get_CurvyClamping_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(CurvyClamping*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 45659, RefRangeEnd = 45661, XrefRangeStart = 45659, XrefRangeEnd = 45659, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_CrossClamping_Public_set_Void_CurvyClamping_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float CrossRelativePosition
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45661, XrefRangeEnd = 45662, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CrossRelativePosition_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45662, XrefRangeEnd = 45663, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_CrossRelativePosition_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe override float Length
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45663, XrefRangeEnd = 45665, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_Length_Public_Virtual_get_Single_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe override bool IsReady
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_IsReady_Public_Virtual_get_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe RegionOptions<float> CrossRangeOptions
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45711, XrefRangeEnd = 45714, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr);
			IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CrossRangeOptions_Private_get_RegionOptions_1_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
			Il2CppException.RaiseExceptionIfNecessary(intPtr);
			return new RegionOptions<float>(pointer);
		}
	}

	public unsafe float MinCrossRelativePosition
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_MinCrossRelativePosition_Private_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe float MaxCrossRelativePosition
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_MaxCrossRelativePosition_Private_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static VolumeController()
	{
		Il2CppClassPointerStore<VolumeController>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Controllers", "VolumeController");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<VolumeController>.NativeClassPtr);
		NativeFieldInfoPtr_CrossPositionRangeMin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, "CrossPositionRangeMin");
		NativeFieldInfoPtr_CrossPositionRangeMax = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, "CrossPositionRangeMax");
		NativeFieldInfoPtr_m_Volume = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, "m_Volume");
		NativeFieldInfoPtr_m_CrossRange = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, "m_CrossRange");
		NativeFieldInfoPtr_crossRelativePosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, "crossRelativePosition");
		NativeFieldInfoPtr_m_CrossClamping = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, "m_CrossClamping");
		NativeFieldInfoPtr_m_CrossInitialPosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, "m_CrossInitialPosition");
		NativeMethodInfoPtr_get_Volume_Public_get_CGDataReference_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, 100665635);
		NativeMethodInfoPtr_set_Volume_Public_set_Void_CGDataReference_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, 100665636);
		NativeMethodInfoPtr_get_VolumeData_Public_get_CGVolume_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, 100665637);
		NativeMethodInfoPtr_get_CrossFrom_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, 100665638);
		NativeMethodInfoPtr_set_CrossFrom_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, 100665639);
		NativeMethodInfoPtr_get_CrossTo_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, 100665640);
		NativeMethodInfoPtr_set_CrossTo_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, 100665641);
		NativeMethodInfoPtr_get_CrossLength_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, 100665642);
		NativeMethodInfoPtr_get_CrossClamping_Public_get_CurvyClamping_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, 100665643);
		NativeMethodInfoPtr_set_CrossClamping_Public_set_Void_CurvyClamping_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, 100665644);
		NativeMethodInfoPtr_get_CrossRelativePosition_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, 100665645);
		NativeMethodInfoPtr_set_CrossRelativePosition_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, 100665646);
		NativeMethodInfoPtr_get_Length_Public_Virtual_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, 100665647);
		NativeMethodInfoPtr_CrossRelativeToAbsolute_Public_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, 100665648);
		NativeMethodInfoPtr_CrossAbsoluteToRelative_Public_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, 100665649);
		NativeMethodInfoPtr_get_IsReady_Public_Virtual_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, 100665650);
		NativeMethodInfoPtr_RelativeToAbsolute_Protected_Virtual_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, 100665651);
		NativeMethodInfoPtr_AbsoluteToRelative_Protected_Virtual_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, 100665652);
		NativeMethodInfoPtr_GetInterpolatedSourcePosition_Protected_Virtual_Vector3_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, 100665653);
		NativeMethodInfoPtr_GetInterpolatedSourcePosition_Protected_Virtual_Void_Single_byref_Vector3_byref_Vector3_byref_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, 100665654);
		NativeMethodInfoPtr_GetTangent_Protected_Virtual_Vector3_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, 100665655);
		NativeMethodInfoPtr_GetOrientation_Protected_Virtual_Vector3_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, 100665656);
		NativeMethodInfoPtr_Advance_Protected_Virtual_Void_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, 100665657);
		NativeMethodInfoPtr_SimulateAdvance_Protected_Virtual_Void_byref_Single_byref_MovementDirection_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, 100665658);
		NativeMethodInfoPtr_get_CrossRangeOptions_Private_get_RegionOptions_1_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, 100665659);
		NativeMethodInfoPtr_get_MinCrossRelativePosition_Private_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, 100665660);
		NativeMethodInfoPtr_get_MaxCrossRelativePosition_Private_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, 100665661);
		NativeMethodInfoPtr_GetClampedCrossPosition_Private_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, 100665662);
		NativeMethodInfoPtr_OnAfterDeserialize_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, 100665663);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VolumeController>.NativeClassPtr, 100665664);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45665, XrefRangeEnd = 45669, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float CrossRelativeToAbsolute(float relativeDistance)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&relativeDistance);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CrossRelativeToAbsolute_Public_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45669, XrefRangeEnd = 45673, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float CrossAbsoluteToRelative(float worldUnitDistance)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&worldUnitDistance);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CrossAbsoluteToRelative_Public_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45673, XrefRangeEnd = 45676, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override float RelativeToAbsolute(float relativeDistance)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&relativeDistance);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_RelativeToAbsolute_Protected_Virtual_Single_Single_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45676, XrefRangeEnd = 45679, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override float AbsoluteToRelative(float worldUnitDistance)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&worldUnitDistance);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_AbsoluteToRelative_Protected_Virtual_Single_Single_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45679, XrefRangeEnd = 45685, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override Vector3 GetInterpolatedSourcePosition(float tf)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&tf);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetInterpolatedSourcePosition_Protected_Virtual_Vector3_Single_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45685, XrefRangeEnd = 45693, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void GetInterpolatedSourcePosition(float tf, out Vector3 interpolatedPosition, out Vector3 tangent, out Vector3 up)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[4];
		*ptr = (nint)(&tf);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = Unsafe.AsPointer(ref interpolatedPosition);
		*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = Unsafe.AsPointer(ref tangent);
		*(void**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = Unsafe.AsPointer(ref up);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetInterpolatedSourcePosition_Protected_Virtual_Void_Single_byref_Vector3_byref_Vector3_byref_Vector3_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45693, XrefRangeEnd = 45699, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override Vector3 GetTangent(float tf)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&tf);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetTangent_Protected_Virtual_Vector3_Single_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45699, XrefRangeEnd = 45705, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override Vector3 GetOrientation(float tf)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&tf);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetOrientation_Protected_Virtual_Vector3_Single_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Advance(float speed, float deltaTime)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = (nint)(&speed);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &deltaTime;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Advance_Protected_Virtual_Void_Single_Single_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45705, XrefRangeEnd = 45711, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void SimulateAdvance(ref float tf, ref MovementDirection direction, float speed, float deltaTime)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[4];
		*ptr = (nint)Unsafe.AsPointer(ref tf);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = Unsafe.AsPointer(ref direction);
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &speed;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &deltaTime;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_SimulateAdvance_Protected_Virtual_Void_byref_Single_byref_MovementDirection_Single_Single_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45714, XrefRangeEnd = 45715, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float GetClampedCrossPosition(float position)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&position);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetClampedCrossPosition_Private_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45715, XrefRangeEnd = 45716, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnAfterDeserialize()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnAfterDeserialize_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45716, XrefRangeEnd = 45723, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe VolumeController()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<VolumeController>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public VolumeController(IntPtr pointer)
		: base(pointer)
	{
	}
}
