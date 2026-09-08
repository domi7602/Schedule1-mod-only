using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppScheduleOne.Math;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.Vehicles.AI;

public static class PathUtility : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_GetAverageAheadPoint_Public_Static_Vector3_SmoothedPath_Vector3_Int32_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetAheadPoint_Public_Static_Vector3_SmoothedPath_Vector3_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetAheadPoint_Public_Static_Vector3_SmoothedPath_Vector3_Single_Int32_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetPointAheadOfPathPoint_Public_Static_Vector3_SmoothedPath_Int32_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CalculateAngleChangeOverPath_Public_Static_Single_SmoothedPath_Int32_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CalculateCTE_Public_Static_Single_Vector3_Transform_Vector3_Vector3_SmoothedPath_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetClosestPointOnPath_Public_Static_Vector3_SmoothedPath_Vector3_byref_Int32_byref_Int32_byref_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetAheadPointDirection_Public_Static_Vector3_SmoothedPath_Vector3_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetClosestPointOnLine_Private_Static_Vector3_Vector3_Vector3_Vector3_Boolean_0;

	static PathUtility()
	{
		Il2CppClassPointerStore<PathUtility>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Vehicles.AI", "PathUtility");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PathUtility>.NativeClassPtr);
		NativeMethodInfoPtr_GetAverageAheadPoint_Public_Static_Vector3_SmoothedPath_Vector3_Int32_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathUtility>.NativeClassPtr, 100666423);
		NativeMethodInfoPtr_GetAheadPoint_Public_Static_Vector3_SmoothedPath_Vector3_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathUtility>.NativeClassPtr, 100666424);
		NativeMethodInfoPtr_GetAheadPoint_Public_Static_Vector3_SmoothedPath_Vector3_Single_Int32_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathUtility>.NativeClassPtr, 100666425);
		NativeMethodInfoPtr_GetPointAheadOfPathPoint_Public_Static_Vector3_SmoothedPath_Int32_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathUtility>.NativeClassPtr, 100666426);
		NativeMethodInfoPtr_CalculateAngleChangeOverPath_Public_Static_Single_SmoothedPath_Int32_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathUtility>.NativeClassPtr, 100666427);
		NativeMethodInfoPtr_CalculateCTE_Public_Static_Single_Vector3_Transform_Vector3_Vector3_SmoothedPath_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathUtility>.NativeClassPtr, 100666428);
		NativeMethodInfoPtr_GetClosestPointOnPath_Public_Static_Vector3_SmoothedPath_Vector3_byref_Int32_byref_Int32_byref_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathUtility>.NativeClassPtr, 100666429);
		NativeMethodInfoPtr_GetAheadPointDirection_Public_Static_Vector3_SmoothedPath_Vector3_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathUtility>.NativeClassPtr, 100666430);
		NativeMethodInfoPtr_GetClosestPointOnLine_Private_Static_Vector3_Vector3_Vector3_Vector3_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathUtility>.NativeClassPtr, 100666431);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 95687, XrefRangeEnd = 95691, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector3 GetAverageAheadPoint(PathSmoothingUtility.SmoothedPath path, Vector3 referencePoint, int sampleCount, float stepSize)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)path);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &referencePoint;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &sampleCount;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &stepSize;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAverageAheadPoint_Public_Static_Vector3_SmoothedPath_Vector3_Int32_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 95691, XrefRangeEnd = 95693, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector3 GetAheadPoint(PathSmoothingUtility.SmoothedPath path, Vector3 referencePoint, float distance)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)path);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &referencePoint;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &distance;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAheadPoint_Public_Static_Vector3_SmoothedPath_Vector3_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 95693, XrefRangeEnd = 95694, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector3 GetAheadPoint(PathSmoothingUtility.SmoothedPath path, Vector3 referencePoint, float distance, int startPointIndex, float pointLerp)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)path);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &referencePoint;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &distance;
		*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &startPointIndex;
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &pointLerp;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAheadPoint_Public_Static_Vector3_SmoothedPath_Vector3_Single_Int32_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(10)]
	[CachedScanResults(RefRangeStart = 95721, RefRangeEnd = 95731, XrefRangeStart = 95694, XrefRangeEnd = 95721, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector3 GetPointAheadOfPathPoint(PathSmoothingUtility.SmoothedPath path, int startPointIndex, float pointLerp, float distanceAhead)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)path);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &startPointIndex;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &pointLerp;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &distanceAhead;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetPointAheadOfPathPoint_Public_Static_Vector3_SmoothedPath_Int32_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 95748, RefRangeEnd = 95749, XrefRangeStart = 95731, XrefRangeEnd = 95748, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float CalculateAngleChangeOverPath(PathSmoothingUtility.SmoothedPath path, int startPointIndex, float pointLerp, float distanceAhead)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)path);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &startPointIndex;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &pointLerp;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &distanceAhead;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CalculateAngleChangeOverPath_Public_Static_Single_SmoothedPath_Int32_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 95761, RefRangeEnd = 95762, XrefRangeStart = 95749, XrefRangeEnd = 95761, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float CalculateCTE(Vector3 flatCarPos, Transform vehicleTransform, Vector3 wp_from, Vector3 wp_to, PathSmoothingUtility.SmoothedPath path)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = (nint)(&flatCarPos);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vehicleTransform);
		*(Vector3**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &wp_from;
		*(Vector3**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &wp_to;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)path);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CalculateCTE_Public_Static_Single_Vector3_Transform_Vector3_Vector3_SmoothedPath_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(12)]
	[CachedScanResults(RefRangeStart = 95781, RefRangeEnd = 95793, XrefRangeStart = 95762, XrefRangeEnd = 95781, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector3 GetClosestPointOnPath(PathSmoothingUtility.SmoothedPath path, Vector3 point, out int startPointIndex, out int endPointIndex, out float pointLerp)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)path);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &point;
		*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref startPointIndex);
		*(void**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref endPointIndex);
		*(void**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref pointLerp);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetClosestPointOnPath_Public_Static_Vector3_SmoothedPath_Vector3_byref_Int32_byref_Int32_byref_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 95793, XrefRangeEnd = 95797, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector3 GetAheadPointDirection(PathSmoothingUtility.SmoothedPath path, Vector3 referencePoint, float distanceAhead)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)path);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &referencePoint;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &distanceAhead;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAheadPointDirection_Public_Static_Vector3_SmoothedPath_Vector3_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 95797, XrefRangeEnd = 95801, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector3 GetClosestPointOnLine(Vector3 point, Vector3 line_start, Vector3 line_end, bool clamp = true)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&point);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &line_start;
		*(Vector3**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &line_end;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &clamp;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetClosestPointOnLine_Private_Static_Vector3_Vector3_Vector3_Vector3_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public PathUtility(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
