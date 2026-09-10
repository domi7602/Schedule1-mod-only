using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.Noise;

public static class NoiseUtility : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_EmitNoise_Public_Static_Void_Vector3_ENoiseType_Single_GameObject_0;

	static NoiseUtility()
	{
		Il2CppClassPointerStore<NoiseUtility>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Noise", "NoiseUtility");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<NoiseUtility>.NativeClassPtr);
		NativeMethodInfoPtr_EmitNoise_Public_Static_Void_Vector3_ENoiseType_Single_GameObject_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NoiseUtility>.NativeClassPtr, 100669540);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 136098, RefRangeEnd = 136100, XrefRangeStart = 136044, XrefRangeEnd = 136098, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void EmitNoise(Vector3 origin, ENoiseType type, float range, GameObject source = null)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&origin);
		*(ENoiseType**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &type;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &range;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)source);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EmitNoise_Public_Static_Void_Vector3_ENoiseType_Single_GameObject_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public NoiseUtility(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
