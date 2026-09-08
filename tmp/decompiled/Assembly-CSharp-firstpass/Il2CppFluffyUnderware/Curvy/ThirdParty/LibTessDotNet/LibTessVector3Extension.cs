using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.ThirdParty.LibTessDotNet;

public static class LibTessVector3Extension : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_Vec3_Public_Static_Vec3_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ContourVertex_Public_Static_ContourVertex_Vector3_0;

	static LibTessVector3Extension()
	{
		Il2CppClassPointerStore<LibTessVector3Extension>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.ThirdParty.LibTessDotNet", "LibTessVector3Extension");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LibTessVector3Extension>.NativeClassPtr);
		NativeMethodInfoPtr_Vec3_Public_Static_Vec3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LibTessVector3Extension>.NativeClassPtr, 100667244);
		NativeMethodInfoPtr_ContourVertex_Public_Static_ContourVertex_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LibTessVector3Extension>.NativeClassPtr, 100667245);
	}

	[CallerCount(36)]
	[CachedScanResults(RefRangeStart = 60678, RefRangeEnd = 60714, XrefRangeStart = 60678, XrefRangeEnd = 60678, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vec3 Vec3(this Vector3 v)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&v);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Vec3_Public_Static_Vec3_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vec3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 60714, RefRangeEnd = 60715, XrefRangeStart = 60714, XrefRangeEnd = 60714, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static ContourVertex ContourVertex(this Vector3 v)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&v);
		Unsafe.SkipInit(out System.IntPtr intPtr);
		System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ContourVertex_Public_Static_ContourVertex_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr);
		Il2CppException.RaiseExceptionIfNecessary(intPtr);
		return new ContourVertex(pointer);
	}

	public LibTessVector3Extension(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
