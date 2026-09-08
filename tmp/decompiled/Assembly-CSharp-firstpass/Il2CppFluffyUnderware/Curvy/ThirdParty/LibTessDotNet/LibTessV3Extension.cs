using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.ThirdParty.LibTessDotNet;

public static class LibTessV3Extension : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_Vector3_Public_Static_Vector3_Vec3_0;

	static LibTessV3Extension()
	{
		Il2CppClassPointerStore<LibTessV3Extension>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.ThirdParty.LibTessDotNet", "LibTessV3Extension");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LibTessV3Extension>.NativeClassPtr);
		NativeMethodInfoPtr_Vector3_Public_Static_Vector3_Vec3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LibTessV3Extension>.NativeClassPtr, 100667246);
	}

	[CallerCount(36)]
	[CachedScanResults(RefRangeStart = 60678, RefRangeEnd = 60714, XrefRangeStart = 60678, XrefRangeEnd = 60714, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector3 Vector3(this Vec3 v)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&v);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Vector3_Public_Static_Vector3_Vec3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public LibTessV3Extension(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
