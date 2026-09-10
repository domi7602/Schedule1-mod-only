using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppFluffyUnderware.DevTools.Extensions;

public static class QuaternionExt : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_SameOrientation_Public_Static_Boolean_Quaternion_Quaternion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DifferentOrientation_Public_Static_Boolean_Quaternion_Quaternion_0;

	static QuaternionExt()
	{
		Il2CppClassPointerStore<QuaternionExt>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools.Extensions", "QuaternionExt");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<QuaternionExt>.NativeClassPtr);
		NativeMethodInfoPtr_SameOrientation_Public_Static_Boolean_Quaternion_Quaternion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<QuaternionExt>.NativeClassPtr, 100664369);
		NativeMethodInfoPtr_DifferentOrientation_Public_Static_Boolean_Quaternion_Quaternion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<QuaternionExt>.NativeClassPtr, 100664370);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 33836, XrefRangeEnd = 33839, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool SameOrientation(this Quaternion q1, Quaternion q2)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&q1);
		*(Quaternion**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &q2;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SameOrientation_Public_Static_Boolean_Quaternion_Quaternion_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 33842, RefRangeEnd = 33847, XrefRangeStart = 33839, XrefRangeEnd = 33842, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool DifferentOrientation(this Quaternion q1, Quaternion q2)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&q1);
		*(Quaternion**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &q2;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DifferentOrientation_Public_Static_Boolean_Quaternion_Quaternion_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public QuaternionExt(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
