using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppFluffyUnderware.DevTools;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy;

public class CurvyShape : DTVersionedMonoBehaviour
{
	private static readonly System.IntPtr NativeFieldInfoPtr_m_Plane;

	private static readonly System.IntPtr NativeFieldInfoPtr_mShapeDefs;

	private static readonly System.IntPtr NativeFieldInfoPtr_mSpline;

	private static readonly System.IntPtr NativeFieldInfoPtr_Dirty;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Plane_Public_get_CurvyPlane_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Plane_Public_set_Void_CurvyPlane_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Spline_Public_get_CurvySpline_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Update_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnValidate_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Reset_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Delete_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Refresh_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Replace_Public_CurvyShape_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_PrepareSpline_Protected_Void_CurvyInterpolation_CurvyOrientation_Int32_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetPosition_Protected_Void_Int32_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetRotation_Protected_Void_Int32_Quaternion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetBezierHandles_Protected_Void_Int32_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetBezierHandles_Protected_Void_Int32_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetBezierHandles_Protected_Void_Int32_Vector3_Vector3_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetBezierHandles_Public_Static_Void_Single_Boolean_Boolean_Il2CppReferenceArray_1_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetCGHardEdges_Protected_Void_Il2CppStructArray_1_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ApplyShape_Protected_Virtual_New_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_PrepareControlPoints_Protected_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_ShapeDefinitions_Public_Static_get_Dictionary_2_CurvyShapeInfo_Type_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetShapesMenuNames_Public_Static_List_1_String_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetShapesMenuNames_Public_Static_List_1_String_Type_byref_Int32_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetShapeName_Public_Static_String_Type_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetShapeType_Public_Static_Type_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ApplyPlane_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_applyRotation_Private_Void_Quaternion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe CurvyPlane m_Plane
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Plane);
			return *(CurvyPlane*)num;
		}
		set
		{
			*(CurvyPlane*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Plane)) = curvyPlane;
		}
	}

	public unsafe static Dictionary<CurvyShapeInfo, Il2CppSystem.Type> mShapeDefs
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_mShapeDefs, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<CurvyShapeInfo, Il2CppSystem.Type>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_mShapeDefs, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe CurvySpline mSpline
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mSpline);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySpline>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mSpline)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvySpline));
		}
	}

	public unsafe bool Dirty
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Dirty);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Dirty)) = flag;
		}
	}

	public unsafe CurvyPlane Plane
	{
		[CallerCount(149)]
		[CachedScanResults(RefRangeStart = 35494, RefRangeEnd = 35643, XrefRangeStart = 35494, XrefRangeEnd = 35494, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Plane_Public_get_CurvyPlane_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(CurvyPlane*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Plane_Public_set_Void_CurvyPlane_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe CurvySpline Spline
	{
		[CallerCount(31)]
		[CachedScanResults(RefRangeStart = 35647, RefRangeEnd = 35678, XrefRangeStart = 35643, XrefRangeEnd = 35647, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Spline_Public_get_CurvySpline_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySpline>(intPtr) : null;
		}
	}

	public unsafe static Dictionary<CurvyShapeInfo, Il2CppSystem.Type> ShapeDefinitions
	{
		[CallerCount(5)]
		[CachedScanResults(RefRangeStart = 35992, RefRangeEnd = 35997, XrefRangeStart = 35968, XrefRangeEnd = 35992, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ShapeDefinitions_Public_Static_get_Dictionary_2_CurvyShapeInfo_Type_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<CurvyShapeInfo, Il2CppSystem.Type>>(intPtr) : null;
		}
	}

	static CurvyShape()
	{
		Il2CppClassPointerStore<CurvyShape>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy", "CurvyShape");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CurvyShape>.NativeClassPtr);
		NativeFieldInfoPtr_m_Plane = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyShape>.NativeClassPtr, "m_Plane");
		NativeFieldInfoPtr_mShapeDefs = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyShape>.NativeClassPtr, "mShapeDefs");
		NativeFieldInfoPtr_mSpline = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyShape>.NativeClassPtr, "mSpline");
		NativeFieldInfoPtr_Dirty = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyShape>.NativeClassPtr, "Dirty");
		NativeMethodInfoPtr_get_Plane_Public_get_CurvyPlane_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyShape>.NativeClassPtr, 100664550);
		NativeMethodInfoPtr_set_Plane_Public_set_Void_CurvyPlane_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyShape>.NativeClassPtr, 100664551);
		NativeMethodInfoPtr_get_Spline_Public_get_CurvySpline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyShape>.NativeClassPtr, 100664552);
		NativeMethodInfoPtr_Update_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyShape>.NativeClassPtr, 100664553);
		NativeMethodInfoPtr_OnValidate_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyShape>.NativeClassPtr, 100664554);
		NativeMethodInfoPtr_Reset_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyShape>.NativeClassPtr, 100664555);
		NativeMethodInfoPtr_Delete_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyShape>.NativeClassPtr, 100664556);
		NativeMethodInfoPtr_Refresh_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyShape>.NativeClassPtr, 100664557);
		NativeMethodInfoPtr_Replace_Public_CurvyShape_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyShape>.NativeClassPtr, 100664558);
		NativeMethodInfoPtr_PrepareSpline_Protected_Void_CurvyInterpolation_CurvyOrientation_Int32_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyShape>.NativeClassPtr, 100664559);
		NativeMethodInfoPtr_SetPosition_Protected_Void_Int32_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyShape>.NativeClassPtr, 100664560);
		NativeMethodInfoPtr_SetRotation_Protected_Void_Int32_Quaternion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyShape>.NativeClassPtr, 100664561);
		NativeMethodInfoPtr_SetBezierHandles_Protected_Void_Int32_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyShape>.NativeClassPtr, 100664562);
		NativeMethodInfoPtr_SetBezierHandles_Protected_Void_Int32_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyShape>.NativeClassPtr, 100664563);
		NativeMethodInfoPtr_SetBezierHandles_Protected_Void_Int32_Vector3_Vector3_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyShape>.NativeClassPtr, 100664564);
		NativeMethodInfoPtr_SetBezierHandles_Public_Static_Void_Single_Boolean_Boolean_Il2CppReferenceArray_1_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyShape>.NativeClassPtr, 100664565);
		NativeMethodInfoPtr_SetCGHardEdges_Protected_Void_Il2CppStructArray_1_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyShape>.NativeClassPtr, 100664566);
		NativeMethodInfoPtr_ApplyShape_Protected_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyShape>.NativeClassPtr, 100664567);
		NativeMethodInfoPtr_PrepareControlPoints_Protected_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyShape>.NativeClassPtr, 100664568);
		NativeMethodInfoPtr_get_ShapeDefinitions_Public_Static_get_Dictionary_2_CurvyShapeInfo_Type_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyShape>.NativeClassPtr, 100664569);
		NativeMethodInfoPtr_GetShapesMenuNames_Public_Static_List_1_String_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyShape>.NativeClassPtr, 100664570);
		NativeMethodInfoPtr_GetShapesMenuNames_Public_Static_List_1_String_Type_byref_Int32_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyShape>.NativeClassPtr, 100664571);
		NativeMethodInfoPtr_GetShapeName_Public_Static_String_Type_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyShape>.NativeClassPtr, 100664572);
		NativeMethodInfoPtr_GetShapeType_Public_Static_Type_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyShape>.NativeClassPtr, 100664573);
		NativeMethodInfoPtr_ApplyPlane_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyShape>.NativeClassPtr, 100664574);
		NativeMethodInfoPtr_applyRotation_Private_Void_Quaternion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyShape>.NativeClassPtr, 100664575);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyShape>.NativeClassPtr, 100664576);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 35689, RefRangeEnd = 35690, XrefRangeStart = 35678, XrefRangeEnd = 35689, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Update_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 35691, RefRangeEnd = 35692, XrefRangeStart = 35690, XrefRangeEnd = 35691, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnValidate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnValidate_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35692, XrefRangeEnd = 35693, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Reset()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Reset_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 35694, RefRangeEnd = 35696, XrefRangeStart = 35693, XrefRangeEnd = 35694, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Delete()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Delete_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 35689, RefRangeEnd = 35690, XrefRangeStart = 35689, XrefRangeEnd = 35690, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Refresh()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Refresh_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35696, XrefRangeEnd = 35704, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvyShape Replace(string menuName)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(menuName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Replace_Public_CurvyShape_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvyShape>(intPtr) : null;
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 35734, RefRangeEnd = 35741, XrefRangeStart = 35704, XrefRangeEnd = 35734, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void PrepareSpline(CurvyInterpolation interpolation, CurvyOrientation orientation = CurvyOrientation.Dynamic, int cachedensity = 50, bool closed = true)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&interpolation);
		*(CurvyOrientation**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &orientation;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &cachedensity;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &closed;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PrepareSpline_Protected_Void_CurvyInterpolation_CurvyOrientation_Int32_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(21)]
	[CachedScanResults(RefRangeStart = 35753, RefRangeEnd = 35774, XrefRangeStart = 35741, XrefRangeEnd = 35753, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetPosition(int no, Vector3 position)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&no);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &position;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetPosition_Protected_Void_Int32_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35774, XrefRangeEnd = 35786, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetRotation(int no, Quaternion rotation)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&no);
		*(Quaternion**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &rotation;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetRotation_Protected_Void_Int32_Quaternion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 35787, RefRangeEnd = 35788, XrefRangeStart = 35786, XrefRangeEnd = 35787, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetBezierHandles(int no, float distanceFrag)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&no);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &distanceFrag;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetBezierHandles_Protected_Void_Int32_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 35808, RefRangeEnd = 35809, XrefRangeStart = 35788, XrefRangeEnd = 35808, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetBezierHandles(int no, float inDistanceFrag, float outDistanceFrag)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&no);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &inDistanceFrag;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &outDistanceFrag;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetBezierHandles_Protected_Void_Int32_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(11)]
	[CachedScanResults(RefRangeStart = 35825, RefRangeEnd = 35836, XrefRangeStart = 35809, XrefRangeEnd = 35825, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetBezierHandles(int no, Vector3 i, Vector3 o, Space space = Space.World)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&no);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &i;
		*(Vector3**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &o;
		*(Space**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetBezierHandles_Protected_Void_Int32_Vector3_Vector3_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35836, XrefRangeEnd = 35838, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetBezierHandles(float distanceFrag, bool setIn, bool setOut, [Optional] Il2CppReferenceArray<CurvySplineSegment> controlPoints)
	{
		if (controlPoints == null)
		{
			controlPoints = new Il2CppReferenceArray<CurvySplineSegment>(0L);
		}
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&distanceFrag);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &setIn;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &setOut;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoints);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetBezierHandles_Public_Static_Void_Single_Boolean_Boolean_Il2CppReferenceArray_1_CurvySplineSegment_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 35869, RefRangeEnd = 35870, XrefRangeStart = 35838, XrefRangeEnd = 35869, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetCGHardEdges([Optional] Il2CppStructArray<int> controlPoints)
	{
		if (controlPoints == null)
		{
			controlPoints = new Il2CppStructArray<int>(0L);
		}
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoints);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetCGHardEdges_Protected_Void_Il2CppStructArray_1_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(14950)]
	[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 19142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void ApplyShape()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_ApplyShape_Protected_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 35961, RefRangeEnd = 35968, XrefRangeStart = 35870, XrefRangeEnd = 35961, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void PrepareControlPoints(int count)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&count);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PrepareControlPoints_Protected_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 35997, XrefRangeEnd = 36027, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static List<string> GetShapesMenuNames(bool only2D = false)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&only2D);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetShapesMenuNames_Public_Static_List_1_String_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<string>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 36027, XrefRangeEnd = 36064, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static List<string> GetShapesMenuNames(Il2CppSystem.Type currentShapeType, out int currentIndex, bool only2D = false)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)currentShapeType);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref currentIndex);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &only2D;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetShapesMenuNames_Public_Static_List_1_String_Type_byref_Int32_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<string>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 36064, XrefRangeEnd = 36084, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string GetShapeName(Il2CppSystem.Type shapeType)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)shapeType);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetShapeName_Public_Static_String_Type_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 36108, RefRangeEnd = 36109, XrefRangeStart = 36084, XrefRangeEnd = 36108, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppSystem.Type GetShapeType(string menuName)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(menuName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetShapeType_Public_Static_Type_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Type>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 36122, RefRangeEnd = 36124, XrefRangeStart = 36109, XrefRangeEnd = 36122, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ApplyPlane()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApplyPlane_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 36164, RefRangeEnd = 36165, XrefRangeStart = 36124, XrefRangeEnd = 36164, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void applyRotation(Quaternion q)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&q);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_applyRotation_Private_Void_Quaternion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(12)]
	[CachedScanResults(RefRangeStart = 34977, RefRangeEnd = 34989, XrefRangeStart = 34977, XrefRangeEnd = 34989, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvyShape()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CurvyShape>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public static void SetBezierHandles(float distanceFrag, bool setIn, bool setOut, params CurvySplineSegment[] controlPoints)
	{
		SetBezierHandles(distanceFrag, setIn, setOut, new Il2CppReferenceArray<CurvySplineSegment>(controlPoints));
	}

	public void SetCGHardEdges(params int[] controlPoints)
	{
		SetCGHardEdges(new Il2CppStructArray<int>(controlPoints));
	}

	public CurvyShape(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
