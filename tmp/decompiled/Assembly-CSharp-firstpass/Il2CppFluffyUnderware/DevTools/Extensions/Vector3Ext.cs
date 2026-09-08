using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppFluffyUnderware.DevTools.Extensions;

public static class Vector3Ext : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_AngleSigned_Public_Static_Single_Vector3_Vector3_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RotateAround_Public_Static_Vector3_Vector3_Vector3_Quaternion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToVector2_Public_Static_Vector2_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Approximately_Public_Static_Boolean_Vector3_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_NotApproximately_Public_Static_Boolean_Vector3_Vector3_0;

	static Vector3Ext()
	{
		Il2CppClassPointerStore<Vector3Ext>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools.Extensions", "Vector3Ext");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Vector3Ext>.NativeClassPtr);
		NativeMethodInfoPtr_AngleSigned_Public_Static_Single_Vector3_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Vector3Ext>.NativeClassPtr, 100664364);
		NativeMethodInfoPtr_RotateAround_Public_Static_Vector3_Vector3_Vector3_Quaternion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Vector3Ext>.NativeClassPtr, 100664365);
		NativeMethodInfoPtr_ToVector2_Public_Static_Vector2_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Vector3Ext>.NativeClassPtr, 100664366);
		NativeMethodInfoPtr_Approximately_Public_Static_Boolean_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Vector3Ext>.NativeClassPtr, 100664367);
		NativeMethodInfoPtr_NotApproximately_Public_Static_Boolean_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Vector3Ext>.NativeClassPtr, 100664368);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 33820, RefRangeEnd = 33822, XrefRangeStart = 33819, XrefRangeEnd = 33820, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float AngleSigned(this Vector3 a, Vector3 b, Vector3 normal)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&a);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		*(Vector3**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &normal;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AngleSigned_Public_Static_Single_Vector3_Vector3_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 33823, RefRangeEnd = 33825, XrefRangeStart = 33822, XrefRangeEnd = 33823, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector3 RotateAround(this Vector3 point, Vector3 origin, Quaternion rotation)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&point);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &origin;
		*(Quaternion**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &rotation;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RotateAround_Public_Static_Vector3_Vector3_Vector3_Quaternion_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static Vector2 ToVector2(this Vector3 v)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&v);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToVector2_Public_Static_Vector2_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 33825, XrefRangeEnd = 33828, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool Approximately(this Vector3 v1, Vector3 v2)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&v1);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &v2;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Approximately_Public_Static_Boolean_Vector3_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 33832, RefRangeEnd = 33836, XrefRangeStart = 33828, XrefRangeEnd = 33832, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool NotApproximately(this Vector3 v1, Vector3 v2)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&v1);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &v2;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_NotApproximately_Public_Static_Boolean_Vector3_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public Vector3Ext(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
