using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.DevUtilities;

public static class TransformUtilities : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_GetScenePath_Public_Static_String_Transform_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_XZ_Public_Static_Vector2_Vector3_0;

	static TransformUtilities()
	{
		Il2CppClassPointerStore<TransformUtilities>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.DevUtilities", "TransformUtilities");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TransformUtilities>.NativeClassPtr);
		NativeMethodInfoPtr_GetScenePath_Public_Static_String_Transform_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TransformUtilities>.NativeClassPtr, 100675203);
		NativeMethodInfoPtr_XZ_Public_Static_Vector2_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TransformUtilities>.NativeClassPtr, 100675204);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 196746, XrefRangeEnd = 196758, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string GetScenePath(this Transform transform)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetScenePath_Public_Static_String_Transform_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(9)]
	[CachedScanResults(RefRangeStart = 74100, RefRangeEnd = 74109, XrefRangeStart = 74100, XrefRangeEnd = 74109, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector2 XZ(this Vector3 vector)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&vector);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_XZ_Public_Static_Vector2_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public TransformUtilities(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
