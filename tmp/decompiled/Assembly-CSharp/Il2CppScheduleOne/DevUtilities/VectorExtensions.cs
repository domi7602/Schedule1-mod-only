using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.DevUtilities;

public static class VectorExtensions : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_ToNormal_Public_Static_Vector2_Vector2_Boolean_0;

	static VectorExtensions()
	{
		Il2CppClassPointerStore<VectorExtensions>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.DevUtilities", "VectorExtensions");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<VectorExtensions>.NativeClassPtr);
		NativeMethodInfoPtr_ToNormal_Public_Static_Vector2_Vector2_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorExtensions>.NativeClassPtr, 100675216);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 196867, RefRangeEnd = 196868, XrefRangeStart = 196867, XrefRangeEnd = 196867, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector2 ToNormal(this Vector2 v, bool isClockwise = true)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&v);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &isClockwise;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToNormal_Public_Static_Vector2_Vector2_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public VectorExtensions(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
