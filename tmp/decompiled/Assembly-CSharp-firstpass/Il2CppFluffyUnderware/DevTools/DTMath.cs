using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppFluffyUnderware.DevTools;

public static class DTMath : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_ParallelTransportFrame_Public_Static_Vector3_Vector3_Vector3_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LeftTan_Public_Static_Vector3_byref_Vector3_byref_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RightTan_Public_Static_Vector3_byref_Vector3_byref_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Repeat_Public_Static_Single_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FixNaN_Public_Static_Double_Double_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FixNaN_Public_Static_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FixNaN_Public_Static_Vector2_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FixNaN_Public_Static_Vector3_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MapValue_Public_Static_Single_Single_Single_Single_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SnapPrecision_Public_Static_Single_Single_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SnapPrecision_Public_Static_Vector2_Vector2_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SnapPrecision_Public_Static_Vector3_Vector3_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LinePointDistanceSqr_Public_Static_Single_Vector3_Vector3_Vector3_byref_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RayLineSegmentIntersection_Public_Static_Boolean_Vector2_Vector2_Vector2_Vector2_byref_Vector2_byref_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ShortestIntersectionLine_Public_Static_Boolean_Vector3_Vector3_Vector3_Vector3_byref_Vector3_byref_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LineLineIntersection_Public_Static_Boolean_Vector3_Vector3_Vector3_Vector3_byref_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LineLineIntersect_Public_Static_Boolean_Vector2_Vector2_Vector2_Vector2_byref_Vector2_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_PointInsideTriangle_Public_Static_Boolean_Vector3_Vector3_Vector3_Vector3_byref_Single_byref_Single_Boolean_0;

	static DTMath()
	{
		Il2CppClassPointerStore<DTMath>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools", "DTMath");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DTMath>.NativeClassPtr);
		NativeMethodInfoPtr_ParallelTransportFrame_Public_Static_Vector3_Vector3_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTMath>.NativeClassPtr, 100664001);
		NativeMethodInfoPtr_LeftTan_Public_Static_Vector3_byref_Vector3_byref_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTMath>.NativeClassPtr, 100664002);
		NativeMethodInfoPtr_RightTan_Public_Static_Vector3_byref_Vector3_byref_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTMath>.NativeClassPtr, 100664003);
		NativeMethodInfoPtr_Repeat_Public_Static_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTMath>.NativeClassPtr, 100664004);
		NativeMethodInfoPtr_FixNaN_Public_Static_Double_Double_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTMath>.NativeClassPtr, 100664005);
		NativeMethodInfoPtr_FixNaN_Public_Static_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTMath>.NativeClassPtr, 100664006);
		NativeMethodInfoPtr_FixNaN_Public_Static_Vector2_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTMath>.NativeClassPtr, 100664007);
		NativeMethodInfoPtr_FixNaN_Public_Static_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTMath>.NativeClassPtr, 100664008);
		NativeMethodInfoPtr_MapValue_Public_Static_Single_Single_Single_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTMath>.NativeClassPtr, 100664009);
		NativeMethodInfoPtr_SnapPrecision_Public_Static_Single_Single_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTMath>.NativeClassPtr, 100664010);
		NativeMethodInfoPtr_SnapPrecision_Public_Static_Vector2_Vector2_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTMath>.NativeClassPtr, 100664011);
		NativeMethodInfoPtr_SnapPrecision_Public_Static_Vector3_Vector3_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTMath>.NativeClassPtr, 100664012);
		NativeMethodInfoPtr_LinePointDistanceSqr_Public_Static_Single_Vector3_Vector3_Vector3_byref_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTMath>.NativeClassPtr, 100664013);
		NativeMethodInfoPtr_RayLineSegmentIntersection_Public_Static_Boolean_Vector2_Vector2_Vector2_Vector2_byref_Vector2_byref_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTMath>.NativeClassPtr, 100664014);
		NativeMethodInfoPtr_ShortestIntersectionLine_Public_Static_Boolean_Vector3_Vector3_Vector3_Vector3_byref_Vector3_byref_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTMath>.NativeClassPtr, 100664015);
		NativeMethodInfoPtr_LineLineIntersection_Public_Static_Boolean_Vector3_Vector3_Vector3_Vector3_byref_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTMath>.NativeClassPtr, 100664016);
		NativeMethodInfoPtr_LineLineIntersect_Public_Static_Boolean_Vector2_Vector2_Vector2_Vector2_byref_Vector2_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTMath>.NativeClassPtr, 100664017);
		NativeMethodInfoPtr_PointInsideTriangle_Public_Static_Boolean_Vector3_Vector3_Vector3_Vector3_byref_Single_byref_Single_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTMath>.NativeClassPtr, 100664018);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 31710, XrefRangeEnd = 31724, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector3 ParallelTransportFrame(Vector3 up, Vector3 tan0, Vector3 tan1)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&up);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &tan0;
		*(Vector3**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &tan1;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ParallelTransportFrame_Public_Static_Vector3_Vector3_Vector3_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static Vector3 LeftTan(ref Vector3 tan, ref Vector3 up)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)Unsafe.AsPointer(ref tan);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref up);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LeftTan_Public_Static_Vector3_byref_Vector3_byref_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static Vector3 RightTan(ref Vector3 tan, ref Vector3 up)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)Unsafe.AsPointer(ref tan);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref up);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RightTan_Public_Static_Vector3_byref_Vector3_byref_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(12)]
	[CachedScanResults(RefRangeStart = 31725, RefRangeEnd = 31737, XrefRangeStart = 31724, XrefRangeEnd = 31725, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float Repeat(float t, float length)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&t);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &length;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Repeat_Public_Static_Single_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static double FixNaN(double v)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&v);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FixNaN_Public_Static_Double_Double_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(double*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static float FixNaN(float v)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&v);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FixNaN_Public_Static_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static Vector2 FixNaN(Vector2 v)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&v);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FixNaN_Public_Static_Vector2_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static Vector3 FixNaN(Vector3 v)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&v);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FixNaN_Public_Static_Vector3_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 31737, RefRangeEnd = 31741, XrefRangeStart = 31737, XrefRangeEnd = 31737, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float MapValue(float min, float max, float value, float vMin = -1f, float vMax = 1f)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = (nint)(&min);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &max;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &value;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &vMin;
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &vMax;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MapValue_Public_Static_Single_Single_Single_Single_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 31748, RefRangeEnd = 31753, XrefRangeStart = 31741, XrefRangeEnd = 31748, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float SnapPrecision(float value, int decimals)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&value);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &decimals;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SnapPrecision_Public_Static_Single_Single_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 31753, XrefRangeEnd = 31755, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector2 SnapPrecision(Vector2 value, int decimals)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&value);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &decimals;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SnapPrecision_Public_Static_Vector2_Vector2_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 31755, XrefRangeEnd = 31758, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector3 SnapPrecision(Vector3 value, int decimals)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&value);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &decimals;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SnapPrecision_Public_Static_Vector3_Vector3_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 31758, RefRangeEnd = 31760, XrefRangeStart = 31758, XrefRangeEnd = 31758, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float LinePointDistanceSqr(Vector3 l1, Vector3 l2, Vector3 p, out float frag)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&l1);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &l2;
		*(Vector3**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &p;
		*(void**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref frag);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LinePointDistanceSqr_Public_Static_Single_Vector3_Vector3_Vector3_byref_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 31760, XrefRangeEnd = 31763, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool RayLineSegmentIntersection(Vector2 r0, Vector2 dir, Vector2 l1, Vector2 l2, out Vector2 hit, out float frag)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = (nint)(&r0);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &dir;
		*(Vector2**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &l1;
		*(Vector2**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &l2;
		*(void**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref hit);
		*(void**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref frag);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RayLineSegmentIntersection_Public_Static_Boolean_Vector2_Vector2_Vector2_Vector2_byref_Vector2_byref_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 31773, RefRangeEnd = 31774, XrefRangeStart = 31763, XrefRangeEnd = 31773, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool ShortestIntersectionLine(Vector3 line1A, Vector3 line1B, Vector3 line2A, Vector3 line2B, out Vector3 resultSegmentA, out Vector3 resultSegmentB)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = (nint)(&line1A);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &line1B;
		*(Vector3**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &line2A;
		*(Vector3**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &line2B;
		*(void**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref resultSegmentA);
		*(void**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref resultSegmentB);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ShortestIntersectionLine_Public_Static_Boolean_Vector3_Vector3_Vector3_Vector3_byref_Vector3_byref_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 31774, XrefRangeEnd = 31777, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool LineLineIntersection(Vector3 line1A, Vector3 line1B, Vector3 line2A, Vector3 line2B, out Vector3 hitPoint)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = (nint)(&line1A);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &line1B;
		*(Vector3**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &line2A;
		*(Vector3**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &line2B;
		*(void**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref hitPoint);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LineLineIntersection_Public_Static_Boolean_Vector3_Vector3_Vector3_Vector3_byref_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 31777, XrefRangeEnd = 31780, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool LineLineIntersect(Vector2 line1A, Vector2 line1B, Vector2 line2A, Vector2 line2B, out Vector2 hitPoint, bool segmentOnly = true)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = (nint)(&line1A);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &line1B;
		*(Vector2**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &line2A;
		*(Vector2**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &line2B;
		*(void**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref hitPoint);
		*(bool**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &segmentOnly;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LineLineIntersect_Public_Static_Boolean_Vector2_Vector2_Vector2_Vector2_byref_Vector2_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static bool PointInsideTriangle(Vector3 A, Vector3 B, Vector3 C, Vector3 p, out float ac, out float ab, bool edgesAllowed)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[7];
		*ptr = (nint)(&A);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &B;
		*(Vector3**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &C;
		*(Vector3**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &p;
		*(void**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref ac);
		*(void**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref ab);
		*(bool**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = &edgesAllowed;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PointInsideTriangle_Public_Static_Boolean_Vector3_Vector3_Vector3_Vector3_byref_Single_byref_Single_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public DTMath(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
