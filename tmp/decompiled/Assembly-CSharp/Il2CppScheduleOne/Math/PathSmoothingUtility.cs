using System;
using System.Runtime.CompilerServices;
using Il2CppFluffyUnderware.Curvy;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppScheduleOne.Math;

public static class PathSmoothingUtility : Il2CppSystem.Object
{
	public class SmoothedPath : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_MARGIN;

		private static readonly System.IntPtr NativeFieldInfoPtr_vectorPath;

		private static readonly System.IntPtr NativeFieldInfoPtr_segmentBounds;

		private static readonly System.IntPtr NativeMethodInfoPtr_InitializePath_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe static float MARGIN
		{
			get
			{
				Unsafe.SkipInit(out float result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MARGIN, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MARGIN, (void*)(&num));
			}
		}

		public unsafe List<Vector3> vectorPath
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_vectorPath);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Vector3>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_vectorPath)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
			}
		}

		public unsafe List<Bounds> segmentBounds
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_segmentBounds);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Bounds>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_segmentBounds)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
			}
		}

		static SmoothedPath()
		{
			Il2CppClassPointerStore<SmoothedPath>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<PathSmoothingUtility>.NativeClassPtr, "SmoothedPath");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SmoothedPath>.NativeClassPtr);
			NativeFieldInfoPtr_MARGIN = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SmoothedPath>.NativeClassPtr, "MARGIN");
			NativeFieldInfoPtr_vectorPath = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SmoothedPath>.NativeClassPtr, "vectorPath");
			NativeFieldInfoPtr_segmentBounds = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SmoothedPath>.NativeClassPtr, "segmentBounds");
			NativeMethodInfoPtr_InitializePath_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SmoothedPath>.NativeClassPtr, 100674955);
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SmoothedPath>.NativeClassPtr, 100674956);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 193515, RefRangeEnd = 193516, XrefRangeStart = 193496, XrefRangeEnd = 193515, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void InitializePath()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InitializePath_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 193531, RefRangeEnd = 193533, XrefRangeStart = 193516, XrefRangeEnd = 193531, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe SmoothedPath()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SmoothedPath>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public SmoothedPath(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_MinControlPointDistance;

	private static readonly System.IntPtr NativeFieldInfoPtr__spline;

	private static readonly System.IntPtr NativeMethodInfoPtr_EnsureSplineInitialized_Public_Static_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CalculateSmoothedPath_Public_Static_SmoothedPath_List_1_Vector3_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DrawPath_Private_Static_Void_SmoothedPath_Color_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InsertIntermediatePoints_Private_Static_List_1_Vector3_List_1_Vector3_Single_0;

	public unsafe static float MinControlPointDistance
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MinControlPointDistance, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MinControlPointDistance, (void*)(&num));
		}
	}

	public unsafe static CurvySpline _spline
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__spline, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySpline>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__spline, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvySpline));
		}
	}

	static PathSmoothingUtility()
	{
		Il2CppClassPointerStore<PathSmoothingUtility>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Math", "PathSmoothingUtility");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PathSmoothingUtility>.NativeClassPtr);
		NativeFieldInfoPtr_MinControlPointDistance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathSmoothingUtility>.NativeClassPtr, "MinControlPointDistance");
		NativeFieldInfoPtr__spline = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathSmoothingUtility>.NativeClassPtr, "_spline");
		NativeMethodInfoPtr_EnsureSplineInitialized_Public_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathSmoothingUtility>.NativeClassPtr, 100674951);
		NativeMethodInfoPtr_CalculateSmoothedPath_Public_Static_SmoothedPath_List_1_Vector3_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathSmoothingUtility>.NativeClassPtr, 100674952);
		NativeMethodInfoPtr_DrawPath_Private_Static_Void_SmoothedPath_Color_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathSmoothingUtility>.NativeClassPtr, 100674953);
		NativeMethodInfoPtr_InsertIntermediatePoints_Private_Static_List_1_Vector3_List_1_Vector3_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathSmoothingUtility>.NativeClassPtr, 100674954);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 193551, RefRangeEnd = 193552, XrefRangeStart = 193533, XrefRangeEnd = 193551, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void EnsureSplineInitialized()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EnsureSplineInitialized_Public_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 193629, RefRangeEnd = 193630, XrefRangeStart = 193552, XrefRangeEnd = 193629, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static SmoothedPath CalculateSmoothedPath(List<Vector3> controlPoints, float maxCPDistance = 20f)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoints);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &maxCPDistance;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CalculateSmoothedPath_Public_Static_SmoothedPath_List_1_Vector3_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SmoothedPath>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 193630, XrefRangeEnd = 193641, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void DrawPath(SmoothedPath path, Color col, float duration)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)path);
		*(Color**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &col;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &duration;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DrawPath_Private_Static_Void_SmoothedPath_Color_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 193658, RefRangeEnd = 193659, XrefRangeStart = 193641, XrefRangeEnd = 193658, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static List<Vector3> InsertIntermediatePoints(List<Vector3> points, float maxDistance)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)points);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &maxDistance;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InsertIntermediatePoints_Private_Static_List_1_Vector3_List_1_Vector3_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Vector3>>(intPtr) : null;
	}

	public PathSmoothingUtility(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
