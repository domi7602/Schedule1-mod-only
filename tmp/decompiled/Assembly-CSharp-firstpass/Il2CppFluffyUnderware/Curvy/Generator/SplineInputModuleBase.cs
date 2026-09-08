using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem.Collections.Generic;
using Il2CppToolBuddy.Pooling.Collections;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.Generator;

public class SplineInputModuleBase : CGModule
{
	private static readonly IntPtr NativeFieldInfoPtr_m_UseCache;

	private static readonly IntPtr NativeFieldInfoPtr_m_UseGlobalSpace;

	private static readonly IntPtr NativeFieldInfoPtr_m_StartCP;

	private static readonly IntPtr NativeFieldInfoPtr_m_EndCP;

	private static readonly IntPtr NativeMethodInfoPtr_get_UseCache_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_UseCache_Public_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_StartCP_Public_get_CurvySplineSegment_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_StartCP_Public_set_Void_CurvySplineSegment_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_EndCP_Public_get_CurvySplineSegment_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_EndCP_Public_set_Void_CurvySplineSegment_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_UseGlobalSpace_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_UseGlobalSpace_Public_set_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_IsConfigured_Public_Virtual_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_IsInitialized_Public_Virtual_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_PathIsClosed_Public_Virtual_Final_New_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetRange_Public_Void_CurvySplineSegment_CurvySplineSegment_0;

	private static readonly IntPtr NativeMethodInfoPtr_ClearRange_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnEnable_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnDisable_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnValidate_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Reset_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnSplineRefreshed_Private_Void_CurvySplineEventArgs_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnSplineInitialized_Private_Void_CurvySplineEventArgs_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnInputSplineCoordinatesChanged_Private_Void_CurvySpline_0;

	private static readonly IntPtr NativeMethodInfoPtr_ForceRefresh_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_getPathClosed_Private_Boolean_CurvySpline_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetSplineData_Protected_CGData_CurvySpline_Boolean_CGDataRequestRasterization_CGDataRequestMetaCGOptions_0;

	private static readonly IntPtr NativeMethodInfoPtr_ProcessControlPointOptions_Private_Static_Void_ControlPointOption_Int32_List_1_SamplePointsMaterialGroup_List_1_SamplePointUData_List_1_DuplicateSamplePoint_byref_SamplePointsMaterialGroup_byref_SamplePointsPatch_byref_Single_byref_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_FillData_Private_Static_Void_CGShape_SamplePointsMaterialGroup_SubArrayList_1_Single_SubArrayList_1_Single_Boolean_SubArrayList_1_Vector3_SubArrayList_1_Vector3_SubArrayList_1_Vector3_Boolean_Transform_Transform_0;

	private static readonly IntPtr NativeMethodInfoPtr_AddPoint_Private_Static_Void_Single_Single_Boolean_Vector3_Vector3_Vector3_byref_SubArrayList_1_Single_byref_SubArrayList_1_Single_byref_SubArrayList_1_Vector3_byref_SubArrayList_1_Vector3_byref_SubArrayList_1_Vector3_0;

	private static readonly IntPtr NativeMethodInfoPtr_MoveByAngleExt_Private_Static_Boolean_CurvySpline_Boolean_byref_Single_Single_Single_byref_Vector3_byref_Vector3_Single_Boolean_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_CalculateExtendedUV_Private_Static_Void_CurvySpline_Single_Single_List_1_SamplePointUData_CGShape_0;

	private static readonly IntPtr NativeMethodInfoPtr_findPreviousReferenceCPOptions_Private_Static_MetaCGOptions_CurvySpline_Single_byref_CurvySplineSegment_0;

	private static readonly IntPtr NativeMethodInfoPtr_findNextReferenceCPOptions_Private_Static_MetaCGOptions_CurvySpline_Single_byref_CurvySplineSegment_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_InputSpline_Protected_Abstract_Virtual_New_get_CurvySpline_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_InputSpline_Protected_Abstract_Virtual_New_set_Void_CurvySpline_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnSplineAssigned_Protected_Virtual_New_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_ValidateStartAndEndCps_Protected_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Protected_Void_0;

	public unsafe bool m_UseCache
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_UseCache);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_UseCache)) = flag;
		}
	}

	public unsafe bool m_UseGlobalSpace
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_UseGlobalSpace);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_UseGlobalSpace)) = flag;
		}
	}

	public unsafe CurvySplineSegment m_StartCP
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_StartCP);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_StartCP)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvySplineSegment));
		}
	}

	public unsafe CurvySplineSegment m_EndCP
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_EndCP);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_EndCP)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvySplineSegment));
		}
	}

	public unsafe bool UseCache
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_UseCache_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45723, XrefRangeEnd = 45724, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_UseCache_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe CurvySplineSegment StartCP
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_StartCP_Public_get_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45724, XrefRangeEnd = 45731, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_StartCP_Public_set_Void_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe CurvySplineSegment EndCP
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_EndCP_Public_get_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45731, XrefRangeEnd = 45738, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_EndCP_Public_set_Void_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool UseGlobalSpace
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_UseGlobalSpace_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45738, XrefRangeEnd = 45739, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_UseGlobalSpace_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe override bool IsConfigured
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45739, XrefRangeEnd = 45746, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_IsConfigured_Public_Virtual_get_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe override bool IsInitialized
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45746, XrefRangeEnd = 45747, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_IsInitialized_Public_Virtual_get_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe virtual bool PathIsClosed
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_PathIsClosed_Public_Virtual_Final_New_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe virtual CurvySpline InputSpline
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_InputSpline_Protected_Abstract_Virtual_New_get_CurvySpline_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<CurvySpline>(intPtr) : null;
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_set_InputSpline_Protected_Abstract_Virtual_New_set_Void_CurvySpline_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static SplineInputModuleBase()
	{
		Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator", "SplineInputModuleBase");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr);
		NativeFieldInfoPtr_m_UseCache = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, "m_UseCache");
		NativeFieldInfoPtr_m_UseGlobalSpace = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, "m_UseGlobalSpace");
		NativeFieldInfoPtr_m_StartCP = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, "m_StartCP");
		NativeFieldInfoPtr_m_EndCP = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, "m_EndCP");
		NativeMethodInfoPtr_get_UseCache_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665665);
		NativeMethodInfoPtr_set_UseCache_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665666);
		NativeMethodInfoPtr_get_StartCP_Public_get_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665667);
		NativeMethodInfoPtr_set_StartCP_Public_set_Void_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665668);
		NativeMethodInfoPtr_get_EndCP_Public_get_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665669);
		NativeMethodInfoPtr_set_EndCP_Public_set_Void_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665670);
		NativeMethodInfoPtr_get_UseGlobalSpace_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665671);
		NativeMethodInfoPtr_set_UseGlobalSpace_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665672);
		NativeMethodInfoPtr_get_IsConfigured_Public_Virtual_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665673);
		NativeMethodInfoPtr_get_IsInitialized_Public_Virtual_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665674);
		NativeMethodInfoPtr_get_PathIsClosed_Public_Virtual_Final_New_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665675);
		NativeMethodInfoPtr_SetRange_Public_Void_CurvySplineSegment_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665676);
		NativeMethodInfoPtr_ClearRange_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665677);
		NativeMethodInfoPtr_OnEnable_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665678);
		NativeMethodInfoPtr_OnDisable_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665679);
		NativeMethodInfoPtr_OnValidate_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665680);
		NativeMethodInfoPtr_Reset_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665681);
		NativeMethodInfoPtr_OnSplineRefreshed_Private_Void_CurvySplineEventArgs_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665682);
		NativeMethodInfoPtr_OnSplineInitialized_Private_Void_CurvySplineEventArgs_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665683);
		NativeMethodInfoPtr_OnInputSplineCoordinatesChanged_Private_Void_CurvySpline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665684);
		NativeMethodInfoPtr_ForceRefresh_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665685);
		NativeMethodInfoPtr_getPathClosed_Private_Boolean_CurvySpline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665686);
		NativeMethodInfoPtr_GetSplineData_Protected_CGData_CurvySpline_Boolean_CGDataRequestRasterization_CGDataRequestMetaCGOptions_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665687);
		NativeMethodInfoPtr_ProcessControlPointOptions_Private_Static_Void_ControlPointOption_Int32_List_1_SamplePointsMaterialGroup_List_1_SamplePointUData_List_1_DuplicateSamplePoint_byref_SamplePointsMaterialGroup_byref_SamplePointsPatch_byref_Single_byref_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665688);
		NativeMethodInfoPtr_FillData_Private_Static_Void_CGShape_SamplePointsMaterialGroup_SubArrayList_1_Single_SubArrayList_1_Single_Boolean_SubArrayList_1_Vector3_SubArrayList_1_Vector3_SubArrayList_1_Vector3_Boolean_Transform_Transform_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665689);
		NativeMethodInfoPtr_AddPoint_Private_Static_Void_Single_Single_Boolean_Vector3_Vector3_Vector3_byref_SubArrayList_1_Single_byref_SubArrayList_1_Single_byref_SubArrayList_1_Vector3_byref_SubArrayList_1_Vector3_byref_SubArrayList_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665690);
		NativeMethodInfoPtr_MoveByAngleExt_Private_Static_Boolean_CurvySpline_Boolean_byref_Single_Single_Single_byref_Vector3_byref_Vector3_Single_Boolean_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665691);
		NativeMethodInfoPtr_CalculateExtendedUV_Private_Static_Void_CurvySpline_Single_Single_List_1_SamplePointUData_CGShape_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665692);
		NativeMethodInfoPtr_findPreviousReferenceCPOptions_Private_Static_MetaCGOptions_CurvySpline_Single_byref_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665693);
		NativeMethodInfoPtr_findNextReferenceCPOptions_Private_Static_MetaCGOptions_CurvySpline_Single_byref_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665694);
		NativeMethodInfoPtr_get_InputSpline_Protected_Abstract_Virtual_New_get_CurvySpline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665695);
		NativeMethodInfoPtr_set_InputSpline_Protected_Abstract_Virtual_New_set_Void_CurvySpline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665696);
		NativeMethodInfoPtr_OnSplineAssigned_Protected_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665697);
		NativeMethodInfoPtr_ValidateStartAndEndCps_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665698);
		NativeMethodInfoPtr__ctor_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr, 100665699);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45747, XrefRangeEnd = 45758, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetRange(CurvySplineSegment rangeStart, CurvySplineSegment rangeEnd)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rangeStart);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rangeEnd);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetRange_Public_Void_CurvySplineSegment_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45758, XrefRangeEnd = 45769, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ClearRange()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClearRange_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45769, XrefRangeEnd = 45771, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnEnable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnEnable_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45771, XrefRangeEnd = 45802, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnDisable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnDisable_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45802, XrefRangeEnd = 45807, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnValidate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnValidate_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45807, XrefRangeEnd = 45824, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Reset()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Reset_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45824, XrefRangeEnd = 45837, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnSplineRefreshed(CurvySplineEventArgs e)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)e);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnSplineRefreshed_Private_Void_CurvySplineEventArgs_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45837, XrefRangeEnd = 45850, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnSplineInitialized(CurvySplineEventArgs e)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)e);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnSplineInitialized_Private_Void_CurvySplineEventArgs_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45850, XrefRangeEnd = 45864, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnInputSplineCoordinatesChanged(CurvySpline sender)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sender);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnInputSplineCoordinatesChanged_Private_Void_CurvySpline_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45864, XrefRangeEnd = 45866, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ForceRefresh()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ForceRefresh_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 45866, XrefRangeEnd = 45873, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool getPathClosed(CurvySpline spline)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spline);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_getPathClosed_Private_Boolean_CurvySpline_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 46200, RefRangeEnd = 46202, XrefRangeStart = 45873, XrefRangeEnd = 46200, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGData GetSplineData(CurvySpline spline, bool fullPath, CGDataRequestRasterization raster, CGDataRequestMetaCGOptions options)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spline);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &fullPath;
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)raster);
		*(IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)options);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetSplineData_Protected_CGData_CurvySpline_Boolean_CGDataRequestRasterization_CGDataRequestMetaCGOptions_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<CGData>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 46230, RefRangeEnd = 46232, XrefRangeStart = 46202, XrefRangeEnd = 46230, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void ProcessControlPointOptions(ControlPointOption options, int positionsCount, List<SamplePointsMaterialGroup> shapeMaterialGroups, List<SamplePointUData> extendedUVData, List<DuplicateSamplePoint> duplicatePoints, ref SamplePointsMaterialGroup currentMaterialGroup, ref SamplePointsPatch currentPatch, out float currentDistance, out bool duplicatePoint)
	{
		IntPtr* ptr = stackalloc IntPtr[9];
		*ptr = (nint)(&options);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &positionsCount;
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)shapeMaterialGroups);
		*(IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)extendedUVData);
		*(IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)duplicatePoints);
		byte* num = (byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(IntPtr)));
		IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)currentMaterialGroup);
		*(IntPtr**)num = &intPtr;
		*(void**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(IntPtr)))) = Unsafe.AsPointer(ref currentPatch);
		*(void**)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(IntPtr)))) = Unsafe.AsPointer(ref currentDistance);
		*(void**)((byte*)ptr + checked((nuint)8u * unchecked((nuint)sizeof(IntPtr)))) = Unsafe.AsPointer(ref duplicatePoint);
		Unsafe.SkipInit(out IntPtr intPtr3);
		IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ProcessControlPointOptions_Private_Static_Void_ControlPointOption_Int32_List_1_SamplePointsMaterialGroup_List_1_SamplePointUData_List_1_DuplicateSamplePoint_byref_SamplePointsMaterialGroup_byref_SamplePointsPatch_byref_Single_byref_Boolean_0, (IntPtr)0, (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		IntPtr intPtr4 = intPtr;
		currentMaterialGroup = ((intPtr4 == (IntPtr)0) ? null : new SamplePointsMaterialGroup(intPtr4));
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 46270, RefRangeEnd = 46271, XrefRangeStart = 46232, XrefRangeEnd = 46270, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void FillData(CGShape dataToFill, SamplePointsMaterialGroup materialGroup, SubArrayList<float> sourceFs, SubArrayList<float> relativeFs, bool isFullPath, SubArrayList<Vector3> positions, SubArrayList<Vector3> tangents, SubArrayList<Vector3> normals, bool considerSplineTransform, Transform splineTransform, Transform generatorTransform)
	{
		IntPtr* ptr = stackalloc IntPtr[11];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dataToFill);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)materialGroup);
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)sourceFs));
		*(IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)relativeFs));
		*(bool**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = &isFullPath;
		*(IntPtr*)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)positions));
		*(IntPtr*)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)tangents));
		*(IntPtr*)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)normals));
		*(bool**)((byte*)ptr + checked((nuint)8u * unchecked((nuint)sizeof(IntPtr)))) = &considerSplineTransform;
		*(IntPtr*)((byte*)ptr + checked((nuint)9u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)splineTransform);
		*(IntPtr*)((byte*)ptr + checked((nuint)10u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)generatorTransform);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FillData_Private_Static_Void_CGShape_SamplePointsMaterialGroup_SubArrayList_1_Single_SubArrayList_1_Single_Boolean_SubArrayList_1_Vector3_SubArrayList_1_Vector3_SubArrayList_1_Vector3_Boolean_Transform_Transform_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 46271, XrefRangeEnd = 46285, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void AddPoint(float sourceF, float relativeF, bool isFullPath, Vector3 position, Vector3 tangent, Vector3 up, ref SubArrayList<float> sourceFList, ref SubArrayList<float> relativeFList, ref SubArrayList<Vector3> positionList, ref SubArrayList<Vector3> tangentList, ref SubArrayList<Vector3> upList)
	{
		IntPtr* ptr = stackalloc IntPtr[11];
		*ptr = (nint)(&sourceF);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &relativeF;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &isFullPath;
		*(Vector3**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &position;
		*(Vector3**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = &tangent;
		*(Vector3**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(IntPtr)))) = &up;
		*(IntPtr*)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)sourceFList));
		*(IntPtr*)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)relativeFList));
		*(IntPtr*)((byte*)ptr + checked((nuint)8u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)positionList));
		*(IntPtr*)((byte*)ptr + checked((nuint)9u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)tangentList));
		*(IntPtr*)((byte*)ptr + checked((nuint)10u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)upList));
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddPoint_Private_Static_Void_Single_Single_Boolean_Vector3_Vector3_Vector3_byref_SubArrayList_1_Single_byref_SubArrayList_1_Single_byref_SubArrayList_1_Vector3_byref_SubArrayList_1_Vector3_byref_SubArrayList_1_Vector3_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 46301, RefRangeEnd = 46302, XrefRangeStart = 46285, XrefRangeEnd = 46301, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool MoveByAngleExt(CurvySpline spline, bool useCache, ref float tf, float maxDistance, float maxAngle, out Vector3 pos, out Vector3 tan, float stopTF, bool loop, float stepDist)
	{
		IntPtr* ptr = stackalloc IntPtr[10];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spline);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &useCache;
		*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = Unsafe.AsPointer(ref tf);
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &maxDistance;
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = &maxAngle;
		*(void**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(IntPtr)))) = Unsafe.AsPointer(ref pos);
		*(void**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(IntPtr)))) = Unsafe.AsPointer(ref tan);
		*(float**)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(IntPtr)))) = &stopTF;
		*(bool**)((byte*)ptr + checked((nuint)8u * unchecked((nuint)sizeof(IntPtr)))) = &loop;
		*(float**)((byte*)ptr + checked((nuint)9u * unchecked((nuint)sizeof(IntPtr)))) = &stepDist;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MoveByAngleExt_Private_Static_Boolean_CurvySpline_Boolean_byref_Single_Single_Single_byref_Vector3_byref_Vector3_Single_Boolean_Single_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 46453, RefRangeEnd = 46454, XrefRangeStart = 46302, XrefRangeEnd = 46453, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void CalculateExtendedUV(CurvySpline spline, float startTF, float endTF, List<SamplePointUData> ext, CGShape data)
	{
		IntPtr* ptr = stackalloc IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spline);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &startTF;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &endTF;
		*(IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)ext);
		*(IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)data);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CalculateExtendedUV_Private_Static_Void_CurvySpline_Single_Single_List_1_SamplePointUData_CGShape_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 46454, XrefRangeEnd = 46470, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static MetaCGOptions findPreviousReferenceCPOptions(CurvySpline spline, float tf, out CurvySplineSegment cp)
	{
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spline);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &tf;
		byte* num = (byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)));
		nint num2 = 0;
		*(nint**)num = &num2;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_findPreviousReferenceCPOptions_Private_Static_MetaCGOptions_CurvySpline_Single_byref_CurvySplineSegment_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		nint num3 = num2;
		cp = ((num3 == 0) ? null : new CurvySplineSegment(num3));
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<MetaCGOptions>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 46470, XrefRangeEnd = 46487, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static MetaCGOptions findNextReferenceCPOptions(CurvySpline spline, float tf, out CurvySplineSegment cp)
	{
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spline);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &tf;
		byte* num = (byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)));
		nint num2 = 0;
		*(nint**)num = &num2;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_findNextReferenceCPOptions_Private_Static_MetaCGOptions_CurvySpline_Single_byref_CurvySplineSegment_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		nint num3 = num2;
		cp = ((num3 == 0) ? null : new CurvySplineSegment(num3));
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<MetaCGOptions>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 46525, RefRangeEnd = 46526, XrefRangeStart = 46487, XrefRangeEnd = 46525, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void OnSplineAssigned()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnSplineAssigned_Protected_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(10)]
	[CachedScanResults(RefRangeStart = 46587, RefRangeEnd = 46597, XrefRangeStart = 46526, XrefRangeEnd = 46587, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ValidateStartAndEndCps()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ValidateStartAndEndCps_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 46598, RefRangeEnd = 46602, XrefRangeStart = 46597, XrefRangeEnd = 46598, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SplineInputModuleBase()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SplineInputModuleBase>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SplineInputModuleBase(IntPtr pointer)
		: base(pointer)
	{
	}
}
