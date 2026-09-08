using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.DevUtilities;

public static class LayerUtility : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_SetLayerRecursively_Public_Static_Void_GameObject_Int32_0;

	static LayerUtility()
	{
		Il2CppClassPointerStore<LayerUtility>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.DevUtilities", "LayerUtility");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LayerUtility>.NativeClassPtr);
		NativeMethodInfoPtr_SetLayerRecursively_Public_Static_Void_GameObject_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LayerUtility>.NativeClassPtr, 100675061);
	}

	[CallerCount(31)]
	[CachedScanResults(RefRangeStart = 194578, RefRangeEnd = 194609, XrefRangeStart = 194572, XrefRangeEnd = 194578, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetLayerRecursively(GameObject go, int layerNumber)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)go);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &layerNumber;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetLayerRecursively_Public_Static_Void_GameObject_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public LayerUtility(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
