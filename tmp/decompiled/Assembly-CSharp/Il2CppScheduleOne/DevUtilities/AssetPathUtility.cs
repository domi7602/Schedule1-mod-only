using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.DevUtilities;

public static class AssetPathUtility : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_GetResourcesPath_Public_Static_String_Object_0;

	static AssetPathUtility()
	{
		Il2CppClassPointerStore<AssetPathUtility>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.DevUtilities", "AssetPathUtility");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AssetPathUtility>.NativeClassPtr);
		NativeMethodInfoPtr_GetResourcesPath_Public_Static_String_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AssetPathUtility>.NativeClassPtr, 100674961);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 193667, XrefRangeEnd = 193669, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string GetResourcesPath(UnityEngine.Object selectedObject)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)selectedObject);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetResourcesPath_Public_Static_String_Object_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	public AssetPathUtility(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
