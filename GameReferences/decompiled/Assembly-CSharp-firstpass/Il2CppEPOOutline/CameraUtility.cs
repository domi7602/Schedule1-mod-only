using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppEPOOutline;

public static class CameraUtility : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_GetMSAA_Public_Static_Int32_Camera_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetRenderPipelineMSAA_Private_Static_Int32_0;

	static CameraUtility()
	{
		Il2CppClassPointerStore<CameraUtility>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "EPOOutline", "CameraUtility");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CameraUtility>.NativeClassPtr);
		NativeMethodInfoPtr_GetMSAA_Public_Static_Int32_Camera_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CameraUtility>.NativeClassPtr, 100667262);
		NativeMethodInfoPtr_GetRenderPipelineMSAA_Private_Static_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CameraUtility>.NativeClassPtr, 100667263);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 61670, RefRangeEnd = 61671, XrefRangeStart = 61650, XrefRangeEnd = 61670, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static int GetMSAA(Camera camera)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)camera);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetMSAA_Public_Static_Int32_Camera_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 61671, XrefRangeEnd = 61681, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static int GetRenderPipelineMSAA()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetRenderPipelineMSAA_Private_Static_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public CameraUtility(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
