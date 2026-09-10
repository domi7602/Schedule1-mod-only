using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.DevUtilities;

public static class GeometryUtility : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_TryGetIntersection_Public_Static_Boolean_Vector2_Vector2_Vector2_Vector2_byref_Vector2_byref_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TryRayLineIntersection_Public_Static_Boolean_Vector2_Vector2_Vector2_Vector2_byref_Vector2_byref_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LineIntersection_Public_Static_Boolean_Vector2_Vector2_Vector2_Vector2_byref_Single_byref_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Cross_Public_Static_Single_Vector2_Vector2_0;

	static GeometryUtility()
	{
		Il2CppClassPointerStore<GeometryUtility>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.DevUtilities", "GeometryUtility");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GeometryUtility>.NativeClassPtr);
		NativeMethodInfoPtr_TryGetIntersection_Public_Static_Boolean_Vector2_Vector2_Vector2_Vector2_byref_Vector2_byref_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GeometryUtility>.NativeClassPtr, 100675042);
		NativeMethodInfoPtr_TryRayLineIntersection_Public_Static_Boolean_Vector2_Vector2_Vector2_Vector2_byref_Vector2_byref_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GeometryUtility>.NativeClassPtr, 100675043);
		NativeMethodInfoPtr_LineIntersection_Public_Static_Boolean_Vector2_Vector2_Vector2_Vector2_byref_Single_byref_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GeometryUtility>.NativeClassPtr, 100675044);
		NativeMethodInfoPtr_Cross_Public_Static_Single_Vector2_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GeometryUtility>.NativeClassPtr, 100675045);
	}

	[CallerCount(0)]
	public unsafe static bool TryGetIntersection(Vector2 p0, Vector2 d1, Vector2 q0, Vector2 d2, out Vector2 intersection, out float t)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = (nint)(&p0);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &d1;
		*(Vector2**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &q0;
		*(Vector2**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &d2;
		*(void**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref intersection);
		*(void**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref t);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TryGetIntersection_Public_Static_Boolean_Vector2_Vector2_Vector2_Vector2_byref_Vector2_byref_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 194359, RefRangeEnd = 194360, XrefRangeStart = 194357, XrefRangeEnd = 194359, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool TryRayLineIntersection(Vector2 p0, Vector2 d1, Vector2 r0, Vector2 d2, out Vector2 intersection, out float t)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = (nint)(&p0);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &d1;
		*(Vector2**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &r0;
		*(Vector2**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &d2;
		*(void**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref intersection);
		*(void**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref t);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TryRayLineIntersection_Public_Static_Boolean_Vector2_Vector2_Vector2_Vector2_byref_Vector2_byref_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 194361, RefRangeEnd = 194362, XrefRangeStart = 194360, XrefRangeEnd = 194361, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool LineIntersection(Vector2 p, Vector2 r, Vector2 a, Vector2 b, out float t, out float u)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = (nint)(&p);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &r;
		*(Vector2**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &a;
		*(Vector2**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		*(void**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref t);
		*(void**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref u);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LineIntersection_Public_Static_Boolean_Vector2_Vector2_Vector2_Vector2_byref_Single_byref_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static float Cross(Vector2 a, Vector2 b)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&a);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Cross_Public_Static_Single_Vector2_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public GeometryUtility(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
