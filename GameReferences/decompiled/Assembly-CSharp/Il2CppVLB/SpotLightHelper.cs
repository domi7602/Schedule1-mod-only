using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppVLB;

public static class SpotLightHelper : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_GetIntensity_Public_Static_Single_Light_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetSpotAngle_Public_Static_Single_Light_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetFallOffEnd_Public_Static_Single_Light_0;

	static SpotLightHelper()
	{
		Il2CppClassPointerStore<SpotLightHelper>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "VLB", "SpotLightHelper");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SpotLightHelper>.NativeClassPtr);
		NativeMethodInfoPtr_GetIntensity_Public_Static_Single_Light_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpotLightHelper>.NativeClassPtr, 100664363);
		NativeMethodInfoPtr_GetSpotAngle_Public_Static_Single_Light_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpotLightHelper>.NativeClassPtr, 100664364);
		NativeMethodInfoPtr_GetFallOffEnd_Public_Static_Single_Light_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpotLightHelper>.NativeClassPtr, 100664365);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 73752, RefRangeEnd = 73754, XrefRangeStart = 73747, XrefRangeEnd = 73752, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float GetIntensity(Light light)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)light);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetIntensity_Public_Static_Single_Light_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 73759, RefRangeEnd = 73761, XrefRangeStart = 73754, XrefRangeEnd = 73759, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float GetSpotAngle(Light light)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)light);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetSpotAngle_Public_Static_Single_Light_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 73766, RefRangeEnd = 73768, XrefRangeStart = 73761, XrefRangeEnd = 73766, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float GetFallOffEnd(Light light)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)light);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetFallOffEnd_Public_Static_Single_Light_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public SpotLightHelper(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
