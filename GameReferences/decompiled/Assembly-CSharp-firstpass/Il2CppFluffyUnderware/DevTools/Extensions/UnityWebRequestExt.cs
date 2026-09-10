using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using UnityEngine.Networking;

namespace Il2CppFluffyUnderware.DevTools.Extensions;

public static class UnityWebRequestExt : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_IsError_Public_Static_Boolean_UnityWebRequest_0;

	static UnityWebRequestExt()
	{
		Il2CppClassPointerStore<UnityWebRequestExt>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools.Extensions", "UnityWebRequestExt");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<UnityWebRequestExt>.NativeClassPtr);
		NativeMethodInfoPtr_IsError_Public_Static_Boolean_UnityWebRequest_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UnityWebRequestExt>.NativeClassPtr, 100664348);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 33703, XrefRangeEnd = 33704, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool IsError(this UnityWebRequest webRequest)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)webRequest);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsError_Public_Static_Boolean_UnityWebRequest_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public UnityWebRequestExt(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
